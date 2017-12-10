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
    public class ControlFieldView : IFieldView
    {
        /// <summary>
        /// Ширина и высота клетки игрового поля.
        /// </summary>
        public const int CELL_SIZE = 10;

        // Игровое поле.
        private readonly IField field;

        // Позиция указателя выбранной клетки.
        private int selectorRow = 0, selectorCol = 0;

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
        /// <param name="selectorVisible">Признак: указатель выбранной клетки видим.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ControlFieldView(IField field, Control control, bool selectorVisible = true)
        {
            this.field = field ?? throw new ArgumentNullException();
            this.control = control ?? throw new ArgumentNullException();

            ResizeControl();
            this.field.Resized += OnFieldResized;
            this.field.Modified += OnFieldModified;

            graphics = control.CreateGraphics();
        }

        ~ControlFieldView()
        {
            field.Resized -= OnFieldResized;
        }

        #region ControlMethods

        // Обработчик события изменения размеров поля.
        private void OnFieldResized(object sender, EventArgs e)
        {
            ResizeControl();
        }

        // Обработчик события изменения клеток поля.
        private void OnFieldModified(object sender, FieldModification modType)
        {
            ShowField();
        }

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
                    ShowCell(field.CellAt(row, col), x, y);
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
                SetPropertyValue(ref selectorRow, value, v => v >= 0 && v < field.Height);
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
                SetPropertyValue(ref selectorCol, value, v => v >= 0 && v < field.Width);
            }
        }

        // Функция проверки ограничения значения свойства.
        private delegate bool SatisfiesConstraint(int value);

        // Устанавливает значение свойства поля.
        private void SetPropertyValue(ref int property,
            int value, SatisfiesConstraint cFunc)
        {
            if (cFunc.Invoke(value))
            {
                property = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
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
            SelectorRow = row;
            SelectorCol = col;
        }

        /// <summary>
        /// Возвращает или устанавливает true, если указатель
        /// на выбранную пользователем клетку видим, иначе - false.
        /// </summary>
        public bool SelectorVisible { get; set; }

        #endregion
    }
}
