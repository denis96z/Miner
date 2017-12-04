using System;
using System.Collections.Generic;
using Miner.Math;

namespace Miner.Data
{
    /// <summary>
    /// Игровое поле.
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Возвращает ширину поля.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Возвращает высоту поля.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Возвращает количество мин на поле.
        /// </summary>
        public int NumMines { get; private set; }

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
        /// Если <c>null</c>, используется генератор по умолчанию. </param>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Field(int width, int height, int numMines, IRandomizer randomizer = null)
        {
            if (width < 1 || height < 1 || numMines < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (numMines > width * height)
            {
                throw new ArgumentOutOfRangeException();
            }

            cells = new Cell[width, height];

            Width = width;
            Height = height;
            NumMines = numMines;

            minesPositionsRandomizer = randomizer ?? new StdRandomizer();
        }

        /// <summary>
        /// Выполняет подготовку поля к игре.
        /// </summary>
        public void Initialize()
        {
            ClearCells();
            PlaceMines();
            PlaceValues();
        }

        // Удаляет содержимое клеток поля.
        private void ClearCells()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    cells[i, j].Object = null;
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
                int col = minesPositionsRandomizer.GetValue(0, Width - 1);

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
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void RevealCell(int row, int col)
        {
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
                MinesExploded = true;
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
            }
        }

        /// <summary>
        /// Отмечает указанную клетку как возможно содержащую мину.
        /// </summary>
        /// <param name="row">Строка.</param>
        /// <param name="col">Столбец.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void MarkCell(int row, int col)
        {
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
        }

        // Проверяет находятся ли координаты в пределах поля.
        private bool CellAvailable(int row, int col)
        {
            return row >= 0 && row < Height && col >= 0 && col < Width;
        }

        /// <summary>
        /// Открывает все клетки.
        /// </summary>
        public void RevealAllCells()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    cells[i, j].State = CellState.Revealed;
                }
            }
        }

        /// <summary>
        /// Возвращает клетку поля с указанными координатами.
        /// </summary>
        /// <param name="row">Строка.</param>
        /// <param name="col">Столбец.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Cell this[int row, int col]
        {
            get
            {
                if (!CellAvailable(row, col))
                {
                    throw new ArgumentOutOfRangeException();
                }

                return cells[row, col];
            }
        }

        /// <summary>
        /// Возвращает true если все мины отмечены и клетки с числами открыты, иначе - false.
        /// </summary>
        public bool AllMinesMarked
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
        /// Возвращает true если мины взорвались, иначе - false.
        /// </summary>
        public bool MinesExploded { get; private set; }
    }
}
