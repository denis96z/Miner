using System;

namespace Miner.Data
{
    /// <summary>
    /// Клетка игрового поля.
    /// </summary>
    public struct Cell
    {
        /// <summary>
        /// Возвращает или задает состояние клетки.
        /// </summary>
        public CellState State { get; set; }

        /// <summary>
        /// Возвращает или задает объект, содержащийся в клетке.
        /// </summary>
        public CellObject Object { get; set; }
    }
}
