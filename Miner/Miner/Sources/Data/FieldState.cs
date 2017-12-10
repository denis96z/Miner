using System;

namespace Miner.Data
{
    /// <summary>
    /// Состояние игрового поля.
    /// </summary>
    public enum FieldState
    {
        /// <summary>
        /// Поле не инициализировано.
        /// </summary>
        NotInitialized,

        /// <summary>
        /// Все клетки поля скрыты.
        /// </summary>
        AllCellsHidden,

        /// <summary>
        /// Некоторые клетки поля открыты, некоторые
        /// отмечены как возможно содержащие мину.
        /// </summary>
        SomeCellsMarkedOrRevealed,

        /// <summary>
        /// Все мины отмечены, все клетки,
        /// не содержащие мины открыты.
        /// </summary>
        AllMinesMarked,

        /// <summary>
        /// Все клетки поля открыты.
        /// </summary>
        AllCellsRevealed
    }
}
