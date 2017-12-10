using System;

namespace Miner.Sound
{
    /// <summary>
    /// Плеер звуковых эффектов игры.
    /// </summary>
    interface ISoundPlayer
    {
        /// <summary>
        /// Воспроизводит звук открытия клетки игрового поля.
        /// </summary>
        void PlayCellRevealedSound();

        /// <summary>
        /// Воспроизводит звук установки флажка на клетку игрового поля.
        /// </summary>
        void PlayCellMarkedSound();

        /// <summary>
        /// Воспроизводит звук взрыва мин.
        /// </summary>
        void PlayMinesExplodedSound();
    }
}
