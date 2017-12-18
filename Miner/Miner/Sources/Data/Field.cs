using System;
using System.Linq;
using System.Collections.Generic;
using Miner.Math;

namespace Miner.Data
{
    /// <summary>
    /// Игровое поле.
    /// </summary>
    public class Field : IField
    {
        // Ширина и высота поля.
        private int width = 0, height = 0;

        // Количество мин на поле.
        private int numMines = 0;

        /// <summary>
        /// Возвращает состояние игрового поля.
        /// </summary>
        public FieldState State { get; protected set; }

        /// <summary>
        /// Возвращает или устанавливает ширину поля.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public int Width
        {
            get
            {
                return width;
            }

            set
            {
                SetPropertyValue(ref width, value, v => v > 0);
                Resized.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Возвращает или устанавливает высоту поля.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public int Height
        {
            get
            {
                return height;
            }

            set
            {
                SetPropertyValue(ref height, value, v => v > 0);
                Resized.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Возвращает или устанавливает число мин на поле.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public int NumMines
        {
            get
            {
                return numMines;
            }

            set
            {
                SetPropertyValue(ref numMines, value, v => v > 0 && v <= width * height);
            }
        }

        // Функция проверки ограничения значения свойства.
        private delegate bool SatisfiesConstraint(int value);

        // Устанавливает значение свойства поля.
        private void SetPropertyValue(ref int property,
            int value, SatisfiesConstraint cFunc)
        {
            if (State == FieldState.SomeCellsMarkedOrRevealed)
            {
                throw new InvalidOperationException();
            }
            else if (!cFunc.Invoke(value))
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                property = value;
                State = FieldState.NotInitialized;
            }
        }

        // Клетки поля.
        private readonly Cell[,] cells;

        // Генератор случайных чисел для расстановки мин.
        private readonly IRandomizer minesPositionsRandomizer;

        /// <summary>
        /// Создает экземпляр класса.
        /// </summary>
        /// <param name="width">Ширина поля.</param>
        /// <param name="height">Высота поля.</param>
        /// <param name="numMines">Количество мин на поле.</param>
        /// <param name="randomizer">Генератор случайных чисел для расстановки мин.
        /// Если null, используется генератор по умолчанию. </param>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Field(int width, int height, int numMines, IRandomizer randomizer = null)
        {
            State = FieldState.NotInitialized;

            Resized += (sender, e) => { };
            Modified += (sender, e) => { };

            Width = width;
            Height = height;
            NumMines = numMines;

            cells = new Cell[width, height];
            minesPositionsRandomizer = randomizer ?? new StdRandomizer();
        }

        /// <summary>
        /// Выполняет подготовку поля к игре.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Initialize()
        {
            if (State == FieldState.SomeCellsMarkedOrRevealed)
            {
                throw new InvalidOperationException();
            }
            else
            {
                ClearCells();
                PlaceMines();
                PlaceValues();
                State = FieldState.AllCellsHidden;
                Modified.Invoke(this, FieldModType.Initialized);
            }
        }

        // Удаляет содержимое клеток поля.
        private void ClearCells()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    cells[i, j].Object = null;
                    cells[i, j].State = CellState.Hidden;
                }
            }
        }

        // Расставляет мины на поле случайным образом.
        private void PlaceMines()
        {
            int mCounter = NumMines;
            while (mCounter != 0)
            {
                int row = minesPositionsRandomizer.GetValue(0, Height - 1);
                if(row < 0 || row >= Height)
                {
                    throw new ArgumentOutOfRangeException();
                }

                int col = minesPositionsRandomizer.GetValue(0, Width - 1);
                if (col < 0 || col >= Width)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (cells[row, col].Object == null)
                {
                    cells[row, col].Object = new Mine();
                    mCounter--;
                }
            }
        }

        // Вычисляет количество мин, окружающих каждую клетку.
        private void PlaceValues()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    PlaceValue(i, j);
                }
            }
        }

        // Вычисляет количество мин, окружающих указанную клетку
        // и записывает значение в эту клетку.
        private void PlaceValue(int row, int col)
        {
            if (cells[row, col].Object == null)
            {
                cells[row, col].Object = new NumberOfMines();
                IncreaseNumberOfMines(row, col, row - 1, col - 1);
                IncreaseNumberOfMines(row, col, row - 1, col);
                IncreaseNumberOfMines(row, col, row - 1, col + 1);
                IncreaseNumberOfMines(row, col, row, col - 1);
                IncreaseNumberOfMines(row, col, row, col + 1);
                IncreaseNumberOfMines(row, col, row + 1, col - 1);
                IncreaseNumberOfMines(row, col, row + 1, col);
                IncreaseNumberOfMines(row, col, row + 1, col + 1);
            }
        }

        // В случае, если в проверяемой клетке содержится мина, увеличивает
        // счетчик мин вокруг указанной клетки.
        private void IncreaseNumberOfMines(int row, int col, int checkedRow, int checkedCol)
        {
            if (CellAvailable(checkedRow, checkedCol))
            {
                var numberOfMines = (NumberOfMines)cells[row, col].Object;
                numberOfMines.Value += cells[checkedRow, checkedCol].Object is Mine ? 1 : 0;
            }
        }

        /// <summary>
        /// Открывает указанную клетку.
        /// </summary>
        /// <param name="row">Строка.</param>
        /// <param name="col">Столбец.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void RevealCell(int row, int col)
        {
            if (!CellsModificationsAllowed())
            {
                throw new InvalidOperationException();
            }

            if (!CellAvailable(row, col))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (cells[row, col].State == CellState.Marked)
            {
                return;
            }
            else if (cells[row, col].Object is Mine)
            {
                RevealAllCells();
            }
            else
            {
                var numMinesCell = (NumberOfMines)cells[row, col].Object;
                if (numMinesCell.Value == 0)
                {
                    Stack<(int, int)> stack = new Stack<(int, int)>();
                    stack.Push((row, col));

                    while (stack.Count > 0)
                    {
                        var (r, c) = stack.Pop();
                        if (!CellAvailable(r, c) || cells[r, c].State == CellState.Revealed)
                        {
                            continue;
                        }

                        cells[r, c].State = CellState.Revealed;
                        numMinesCell = (NumberOfMines)cells[r, c].Object;
                        if (numMinesCell.Value == 0)
                        {
                            stack.Push((r - 1, c));
                            stack.Push((r, c - 1));
                            stack.Push((r, c + 1));
                            stack.Push((r + 1, c));
                            stack.Push((r - 1, c - 1));
                            stack.Push((r - 1, c + 1));
                            stack.Push((r + 1, c - 1));
                            stack.Push((r + 1, c + 1));
                        }
                    }
                }
                else
                {
                    cells[row, col].State = CellState.Revealed;
                }

                State = AllMinesMarked ?
                    FieldState.AllMinesMarked :
                    FieldState.SomeCellsMarkedOrRevealed;
                Modified.Invoke(this, FieldModType.CellRevealed);
            }
        }

        /// <summary>
        /// Отмечает указанную клетку как возможно содержащую мину.
        /// </summary>
        /// <param name="row">Строка.</param>
        /// <param name="col">Столбец.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void MarkCell(int row, int col)
        {
            if (!CellsModificationsAllowed())
            {
                throw new InvalidOperationException();
            }

            if (CellAvailable(row, col))
            {
                switch (cells[row, col].State)
                {
                    case CellState.Hidden:
                        cells[row, col].State = CellState.Marked;
                        break;

                    case CellState.Marked:
                        cells[row, col].State = CellState.Hidden;
                        break;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            State = AllMinesMarked ?
                FieldState.AllMinesMarked :
                FieldState.SomeCellsMarkedOrRevealed;
            Modified.Invoke(this, FieldModType.CellMarked);
        }

        // Проверяет находятся ли координаты в пределах поля.
        private bool CellAvailable(int row, int col)
        {
            return row >= 0 && row < Height && col >= 0 && col < Width;
        }

        /// <summary>
        /// Открывает все клетки.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void RevealAllCells()
        {
            if (!CellsModificationsAllowed())
            {
                throw new InvalidOperationException();
            }

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    cells[i, j].State = CellState.Revealed;
                }
            }

            State = FieldState.AllCellsRevealed;
            Modified.Invoke(this, FieldModType.MinesExploded);
        }

        /// <summary>
        /// Возвращает клетку поля с указанными координатами.
        /// </summary>
        /// <param name="row">Строка.</param>
        /// <param name="col">Столбец.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Cell CellAt(int row, int col)
        {
            if (State == FieldState.NotInitialized)
            {
                throw new InvalidOperationException();
            }

            if (!CellAvailable(row, col))
            {
                throw new ArgumentOutOfRangeException();
            }

            return cells[row, col];
        }

        /// <summary>
        /// Возвращает клетку поля с указанными координатами.
        /// </summary>
        /// <param name="row">Строка.</param>
        /// <param name="col">Столбец.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Cell this[int row, int col]
        {
            get
            {
                return CellAt(row, col);
            }
        }

        // Состояния, недопустимые для изменения состояния клеток поля.
        private FieldState[] modificationsDisabledStates =
        {
            FieldState.NotInitialized, FieldState.AllCellsRevealed, FieldState.AllMinesMarked
        };

        // Возвращает true, если допустимы изменения состояния клеток поля, иначе - false.
        private bool CellsModificationsAllowed()
        {
            return !modificationsDisabledStates.Contains(State);
        }

        /// <summary>
        /// Возвращает true, если все мины отмечены и клетки с числами открыты, иначе - false.
        /// </summary>
        private bool AllMinesMarked
        {
            get
            {
                foreach (var cell in cells)
                {
                    if (cell.Object is Mine && cell.State != CellState.Marked)
                    {
                        return false;
                    }
                    else if (cell.Object is NumberOfMines && cell.State != CellState.Revealed)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Возникает, когда размеры поля изменяются.
        /// </summary>
        public event FieldResized Resized;

        /// <summary>
        /// Возникает при изменении клеток поля.
        /// </summary>
        public event FieldModified Modified;
    }
}
