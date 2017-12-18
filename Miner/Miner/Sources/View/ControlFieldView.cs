using System;

using Miner.Data;
using Miner.Input;

namespace Miner.View
{
    /// <summary>
    /// Компонент визуального отображения игрового поля.
    /// </summary>
    public class ControlFieldView : IFieldView
    {
        // Игровое поле.
        private readonly IField field;

        // Адаптер графического контроллера.
        private readonly IControlViewAdapter controlViewAdapter;

        // Позиция указателя выбранной клетки.
        private int selectorRow = 0, selectorCol = 0;

        // Контроллер пользовательского ввода.
        private readonly IInputManager inputManager;

        /// <summary>
        /// Создает экземпляр класса.
        /// </summary>
        /// <param name="field">Игровое поле.</param>
        /// <param name="controlAdapter">Контроллер графического адаптера.</param>
        /// <param name="inputManager">Контроллер пользовательского ввода.</param>
        /// <param name="selectorVisible">Признак: указатель выбранной клетки видим.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ControlFieldView(IField field, IControlViewAdapter controlAdapter,
            IInputManager inputManager, bool selectorVisible = true)
        {
            this.field = field ?? throw new ArgumentNullException();
            this.controlViewAdapter = controlAdapter ?? throw new ArgumentNullException();
            this.inputManager = inputManager ?? throw new ArgumentNullException();

            controlAdapter.ResizeControl(field.Width, field.Height);
            SelectorVisible = selectorVisible;

            this.field.Resized += OnFieldResized;
            this.field.Modified += OnFieldModified;
            this.inputManager.SelectorMoved += OnSelectorMoved;
        }

        /// <summary>
        /// Уничтожает экземпляр класса.
        /// </summary>
        ~ControlFieldView()
        {
            field.Resized -= OnFieldResized;
            field.Modified -= OnFieldModified;
            inputManager.SelectorMoved -= OnSelectorMoved;
        }

        #region ControlMethods

        // Обработчик события изменения размеров поля.
        private void OnFieldResized(object sender, EventArgs e)
        {
            controlViewAdapter.ResizeControl(field.Width, field.Height);
        }

        // Обработчик события изменения клеток поля.
        private void OnFieldModified(object sender, FieldModType modType)
        {
            ShowField();
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
                    ShowCell(field.CellAt(row, col), row, col);
                }
            }
            if (SelectorVisible)
            {
                controlViewAdapter.DrawSelector(SelectorRow, SelectorCol);
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
        private void ShowHiddenCell(int row, int col)
        {
            controlViewAdapter.DrawHiddenCell(row, col);
        }

        // Отображает отмеченную клетку.
        private void ShowMarkedCell(int row, int col)
        {
            controlViewAdapter.DrawMarkedCell(row, col);
        }

        // Отображает открытую клетку.
        private void ShowRevealedCell(Cell cell, int row, int col)
        {
            if (cell.Object is Mine)
            { 
                controlViewAdapter.DrawMinedCell(row, col);
            }
            else if (cell.Object is NumberOfMines)
            {
                var numberOfMines = (NumberOfMines)cell.Object;
                controlViewAdapter.DrawValueCell(numberOfMines.Value, row, col);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        // Отображает указатель выбранной клетки.
        private void ShowSelector(int row, int col)
        {
            controlViewAdapter.DrawSelector(row, col);
        }

        /// <summary>
        /// Скрывает отображенное на графической поверхности поле.
        /// </summary>
        public void HideField()
        {
            controlViewAdapter.ClearGraphics();
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
            ShowField();
        }

        /// <summary>
        /// Возвращает или устанавливает true, если указатель
        /// на выбранную пользователем клетку видим, иначе - false.
        /// </summary>
        public bool SelectorVisible { get; set; }

        // Обработчик события изменения позиции указателя выбранной клетки.
        private void OnSelectorMoved(object sender, EventArgs e)
        {
            SetSelectorPosition(inputManager.SelectorRow, inputManager.SelectorCol);
        }

        #endregion
    }
}
