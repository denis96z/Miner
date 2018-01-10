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
        private readonly IField _field;

        // Адаптер графического контроллера.
        private readonly IControlViewAdapter _controlViewAdapter;

        // Позиция указателя выбранной клетки.
        private int _selectorRow = 0, _selectorCol = 0;

        // Контроллер пользовательского ввода.
        private readonly IInputManager _inputManager;

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
            this._field = field ?? throw new ArgumentNullException();
            this._controlViewAdapter = controlAdapter ?? throw new ArgumentNullException();
            this._inputManager = inputManager ?? throw new ArgumentNullException();

            controlAdapter.ResizeControl(field.Width, field.Height);
            SelectorVisible = selectorVisible;

            this._field.Resized += OnFieldResized;
            this._field.Modified += OnFieldModified;
            this._inputManager.SelectorMoved += OnSelectorMoved;
        }

        /// <summary>
        /// Уничтожает экземпляр класса.
        /// </summary>
        ~ControlFieldView()
        {
            _field.Resized -= OnFieldResized;
            _field.Modified -= OnFieldModified;
            _inputManager.SelectorMoved -= OnSelectorMoved;
        }

        #region ControlMethods

        // Обработчик события изменения размеров поля.
        private void OnFieldResized(object sender, EventArgs e)
        {
            _controlViewAdapter.ResizeControl(_field.Width, _field.Height);
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
            for (int row = 0; row < _field.Height; row++)
            {
                for (int col = 0; col < _field.Width; col++)
                {
                    ShowCell(_field.CellAt(row, col), row, col);
                }
            }
            if (SelectorVisible)
            {
                _controlViewAdapter.DrawSelector(SelectorRow, SelectorCol);
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
            _controlViewAdapter.DrawHiddenCell(row, col);
        }

        // Отображает отмеченную клетку.
        private void ShowMarkedCell(int row, int col)
        {
            _controlViewAdapter.DrawMarkedCell(row, col);
        }

        // Отображает открытую клетку.
        private void ShowRevealedCell(Cell cell, int row, int col)
        {
            if (cell.Object is Mine)
            { 
                _controlViewAdapter.DrawMinedCell(row, col);
            }
            else if (cell.Object is NumberOfMines)
            {
                var numberOfMines = (NumberOfMines)cell.Object;
                _controlViewAdapter.DrawValueCell(numberOfMines.Value, row, col);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        // Отображает указатель выбранной клетки.
        private void ShowSelector(int row, int col)
        {
            _controlViewAdapter.DrawSelector(row, col);
        }

        /// <summary>
        /// Скрывает отображенное на графической поверхности поле.
        /// </summary>
        public void HideField()
        {
            _controlViewAdapter.ClearGraphics();
        }

        #endregion

        #region SelectorViewMethods

        /// <summary>
        /// Возвращает строку выбранной пользователем клетки.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int SelectorRow
        {
            get => _selectorRow;

            private set
            {
                SetPropertyValue(ref _selectorRow, value, v => v >= 0 && v < _field.Height);
            }
        }

        /// <summary>
        /// Возвращает столбец выбранной пользователем клетки.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int SelectorCol
        {
            get => _selectorCol;

            private set
            {
                SetPropertyValue(ref _selectorCol, value, v => v >= 0 && v < _field.Width);
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
            SetSelectorPosition(_inputManager.SelectorRow, _inputManager.SelectorCol);
        }

        #endregion
    }
}
