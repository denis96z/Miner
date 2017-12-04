using System;
using Miner.Data;

namespace Miner.View
{
    /// <summary>
    /// Компонент визуального отображения игрового поля.
    /// </summary>
    public interface IFieldView
    {
        /// <summary>
        /// Отображает поле на графической поверхности.
        /// </summary>
        void ShowField();

        /// <summary>
        /// Скрывает отображенное на графической поверхности поле.
        /// </summary>
        void HideField();

        /// <summary>
        /// Возвращает строку выбранной пользователем клетки.
        /// </summary>
        int SelectorRow { get; }

        /// <summary>
        /// Возвращает столбец выбранной пользователем клетки.
        /// </summary>
        int SelectorCol { get; }

        /// <summary>
        /// Устанавливает указатель на выбранную пользователем клетку.
        /// </summary>
        /// <param name="row">Строка.</param>
        /// <param name="col">Столбец.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        void SetSelectorPosition(int row, int col);

        /// <summary>
        /// Отображает указатель выбранной клетки на графической поверхности.
        /// </summary>
        void ShowSelector();

        /// <summary>
        /// Скрывает указатель выбранной клетки на графической поверхности.
        /// </summary>
        void HideSelector();
    }
}
