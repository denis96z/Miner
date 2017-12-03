using System;

namespace Miner.Field
{
    /// <summary>
    /// Состояние клетки игрового поля.
    /// </summary>
    public enum CellState
    {
        /// <summary>
        /// Клетка скрыта.
        /// </summary>
        Hidden,

        /// <summary>
        /// Клетка открыта.
        /// </summary>
        Revealed,

        /// <summary>
        /// Клетка помечена пользователем
        /// как возможно содержащая мину.
        /// </summary>
        Marked
    }
}
