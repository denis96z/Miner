using System;

namespace Miner.Data
{
    /// <summary>
    /// Изменения, произошедшие на игровом поле.
    /// </summary>
    public enum FieldModification
    {
        /// <summary>
        /// Открыта клетка поля.
        /// </summary>
        CellRevealed,

        /// <summary>
        /// Клетка поля отмечена как
        /// возможно содержащая мину.
        /// </summary>
        CellMarked,

        /// <summary>
        /// Мины взорваны.
        /// </summary>
        MinesExploded
    }
}
