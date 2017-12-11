using System;

namespace Miner.Data
{
    /// <summary>
    /// Игровое поле.
    /// </summary>
    public interface IField
    {
        /// <summary>
        /// Возвращает состояние игрового поля.
        /// </summary>
        FieldState State { get; }

        /// <summary>
        /// Возвращает или устанавливает ширину поля.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        int Width { get; set; }

        /// <summary>
        /// Возвращает или устанавливает высоту поля.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        int Height { get; set; }

        /// <summary>
        /// Возвращает или устанавливает число мин на поле.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        int NumMines { get; set; }

        /// <summary>
        /// Выполняет подготовку поля к игре.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void Initialize();

        /// <summary>
        /// Открывает указанную клетку.
        /// </summary>
        /// <param name="row">Строка.</param>
        /// <param name="col">Столбец.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        void RevealCell(int row, int col);

        /// <summary>
        /// Отмечает указанную клетку как возможно содержащую мину.
        /// </summary>
        /// <param name="row">Строка.</param>
        /// <param name="col">Столбец.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        void MarkCell(int row, int col);

        /// <summary>
        /// Открывает все клетки.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void RevealAllCells();

        /// <summary>
        /// Возвращает клетку поля с указанными координатами.
        /// </summary>
        /// <param name="row">Строка.</param>
        /// <param name="col">Столбец.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Cell CellAt(int row, int col);

        /// <summary>
        /// Возникает, когда размеры поля изменяются.
        /// </summary>
        event FieldResized Resized;

        /// <summary>
        /// Возникает при изменении клеток поля.
        /// </summary>
        event FieldModified Modified;
    }

    /// <summary>
    /// Обработчик события изменения размеров поля.
    /// </summary>
    public delegate void FieldResized(object sender, EventArgs e);

    /// <summary>
    /// Обработчик события изменения клеток поля.
    /// </summary>
    public delegate void FieldModified(object sender, FieldModType modType);
}
