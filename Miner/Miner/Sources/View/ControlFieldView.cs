using System;
using System.Drawing;
using Miner.Properties;
using System.Windows.Forms;
using Miner.Data;

namespace Miner.View
{
    /// <summary>
    /// Компонент визуального отображения игрового поля.
    /// </summary>
    class ControlFieldView : IFieldView
    {
        // Ширина и высота клетки.
        private const int CELL_SIZE = 10;

        // Игровое поле.
        private readonly Field field;

        // Позиция указателя выбранной клетки.
        private volatile int selectorRow = 0, selectorCol = 0;

        // Графический контроллер.
        private readonly Control control;
        // Графическая поверхность.
        private readonly Graphics graphics;

        // Изображение мины.
        private readonly Image mineImage = Resources.Mine;
        // Изображение отметки возможной мины.
        private readonly Image markImage = Resources.Flag;

        // Шрифт цифр.
        private readonly Font valueFont = new Font("Arial", CELL_SIZE / 2);
        // Линия указателя выбранной клетки.
        private readonly Pen selectorPen = new Pen(Color.DarkGreen, 2);

        /// <summary>
        /// Создает экземпляр класса.
        /// </summary>
        /// <param name="field">Игровое поле.</param>
        /// <param name="control">Графический контроллер.</param>
        public ControlFieldView(Field field, Control control)
        {
            this.field = field;
            this.control = control;
            graphics = control.CreateGraphics();
        }

        #region ControlMethods

        // Масштабирует графический контроллер для отображения поля.
        private void ResizeControl()
        {
            int controlWidth = CELL_SIZE * field.Width;
            int controlHeight = CELL_SIZE * field.Height;
            control.ClientSize = new Size(controlWidth, controlHeight);
        }

        #endregion

        #region FieldViewMethods

        /// <summary>
        /// Отображает поле на графической поверхности.
        /// </summary>
        public void ShowField()
        {
            for (int row = 0; row < field.Height; row++)
            {
                for (int col = 0; col < field.Width; col++)
                {
                    int x = col * CELL_SIZE;
                    int y = row * CELL_SIZE;
                    ShowCell(field[row, col], x, y);
                }
            }
        }

        // Отображает клетку поля.
        private void ShowCell(Cell cell, int x, int y)
        {
            switch (cell.State)
            {
                case CellState.Hidden:
                    ShowHiddenCell(x, y);
                    break;

                case CellState.Marked:
                    ShowMarkedCell(x, y);
                    break;

                case CellState.Revealed:
                    ShowRevealedCell(cell, x, y);
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        // Отображает скрытую клетку.
        private void ShowHiddenCell(int x, int y)
        {
            graphics.FillRectangle(Brushes.DarkGray,
                x, y, CELL_SIZE, CELL_SIZE);
            graphics.DrawRectangle(Pens.Black,
                x, y, CELL_SIZE, CELL_SIZE);
        }

        // Отображает отмеченную клетку.
        private void ShowMarkedCell(int x, int y)
        {
            graphics.DrawImage(markImage, x, y, CELL_SIZE, CELL_SIZE);
            graphics.DrawRectangle(Pens.Black, x, y, CELL_SIZE, CELL_SIZE);
        }

        // Отображает открытую клетку.
        private void ShowRevealedCell(Cell cell, int x, int y)
        {
            if (cell.Object is Mine)
            {
                ShowMinedCell(x, y);
            }
            else if (cell.Object is NumberOfMines)
            {
                var numberOfMines = (NumberOfMines)cell.Object;
                ShowValueCell(numberOfMines.Value, x, y);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        // Отображает открытую клетку с миной.
        private void ShowMinedCell(int x, int y)
        {
            graphics.DrawImage(mineImage, x, y, CELL_SIZE, CELL_SIZE);
            graphics.DrawRectangle(Pens.Black, x, y, CELL_SIZE, CELL_SIZE);
        }

        // Отображает открытую клетку с числом мин, расположенных рядом.
        private void ShowValueCell(int value, int x, int y)
        {
            graphics.FillRectangle(Brushes.White, x, y, CELL_SIZE, CELL_SIZE);
            graphics.DrawString(value != 0 ? value.ToString() : String.Empty,
                valueFont, Brushes.Green, x + 1, y + 1);
            graphics.DrawRectangle(Pens.Black, x, y, CELL_SIZE, CELL_SIZE);
        }

        /// <summary>
        /// Скрывает отображенное на графической поверхности поле.
        /// </summary>
        public void HideField()
        {
            graphics.Clear(Color.Transparent);
        }

        #endregion

        #region SelectorViewMethods

        /// <summary>
        /// Возвращает строку выбранной пользователем клетки.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int SelectorRow
        {
            get
            {
                return selectorRow;
            }

            private set
            {
                if (value < 0 || value >= field.Height)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    selectorRow = value;
                }
            }
        }

        /// <summary>
        /// Возвращает столбец выбранной пользователем клетки.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int SelectorCol
        {
            get
            {
                return selectorCol;
            }

            private set
            {
                if (value < 0 || value >= field.Width)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    selectorCol = value;
                }
            }
        }

        /// <summary>
        /// Устанавливает указатель на выбранную пользователем клетку.
        /// </summary>
        /// <param name="row">Строка.</param>
        /// <param name="col">Столбец.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SetSelectorPosition(int row, int col)
        {
            selectorRow = row;
            selectorCol = col;
        }

        /// <summary>
        /// Отображает указатель выбранной клетки на графической поверхности.
        /// </summary>
        public void ShowSelector()
        {
            int x = selectorCol * CELL_SIZE;
            int y = selectorRow * CELL_SIZE;
            graphics.DrawRectangle(selectorPen, x, y, CELL_SIZE, CELL_SIZE);
        }

        /// <summary>
        /// Скрывает указатель выбранной клетки на графической поверхности.
        /// </summary>
        public void HideSelector()
        {
            ShowField();
        }

        #endregion
    }
}
