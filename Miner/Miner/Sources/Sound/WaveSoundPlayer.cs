using System;
using System.Media;
using Miner.Properties;
using Miner.Data;

namespace Miner.Sound
{
    /// <summary>
    /// Плеер звуковых эффектов игры.
    /// </summary>
    class WaveSoundPlayer : ISoundPlayer
    {
        // Встроенный плеер для звука открытия клетки.
        private readonly SoundPlayer cellRevealedPlayer =
            new SoundPlayer(Resources.Reveal);

        // Встроенный плеер для зука установки флажка.
        private readonly SoundPlayer cellMarkedPlayer =
            new SoundPlayer(Resources.Mark);

        // Встроенный плеер для звука взрыва мин.
        private readonly SoundPlayer minesExplodedPlayer =
            new SoundPlayer(Resources.Explode);

        // Игровое поле.
        private readonly IField field;

        /// <summary>
        /// Создает экземпляр класса.
        /// </summary>
        /// <param name="field">Игровое поле.</param>
        public WaveSoundPlayer(IField field)
        {
            this.field.Modified += OnFieldModified;
        }
        
        ~WaveSoundPlayer()
        {
            field.Modified -= OnFieldModified;
        }

        // Обработчик события изменения клеток поля.
        private void OnFieldModified(object sender, FieldModification modType)
        {
            switch (modType)
            {
                case FieldModification.CellRevealed:
                    PlayCellRevealedSound();
                    break;

                case FieldModification.CellMarked:
                    PlayCellMarkedSound();
                    break;

                case FieldModification.MinesExploded:
                    PlayMinesExplodedSound();
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Воспроизводит звук открытия клетки игрового поля.
        /// </summary>
        public void PlayCellRevealedSound()
        {
            cellRevealedPlayer.Play();
        }

        /// <summary>
        /// Воспроизводит звук установки флажка на клетку игрового поля.
        /// </summary>
        public void PlayCellMarkedSound()
        {
            cellMarkedPlayer.Play();
        }

        /// <summary>
        /// Воспроизводит звук взрыва мин.
        /// </summary>
        public void PlayMinesExplodedSound()
        {
            minesExplodedPlayer.Play();
        }
    }
}
