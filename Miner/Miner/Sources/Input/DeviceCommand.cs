using System;

namespace Miner.Input
{
    /// <summary>
    /// Команда, получаемая от утройства управления.
    /// </summary>
    public enum DeviceCommand
    {
        /// <summary>
        /// Сдвинуть указатель выбранной клетки вверх.
        /// </summary>
        MoveUp,

        /// <summary>
        /// Сдвинуть указатель выбранной клетки вниз.
        /// </summary>
        MoveDown,

        /// <summary>
        /// Сдвинуть указатель выбранной клетки влево.
        /// </summary>
        MoveLeft,

        /// <summary>
        /// Сдвинуть указатель выбранной клетки вправо.
        /// </summary>
        MoveRight,

        /// <summary>
        /// Открыть клетку поля.
        /// </summary>
        RevealCell,

        /// <summary>
        /// Отметить клетку поля как возможно содержащую мину.
        /// </summary>
        MarkCell,

        /// <summary>
        /// Неподдерживаемая команда.
        /// </summary>
        Other
    }
}
