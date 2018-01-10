using System;
using System.Media;
using Miner.Properties;
using Miner.Data;

namespace Miner.Sound
{
    /// <summary>
    /// Плеер звуковых эффектов игры.
    /// </summary>
    public class WaveSoundPlayer : ISoundPlayer
    {
        // Встроенный плеер для звука открытия клетки.
        private readonly SoundPlayer _cellRevealedPlayer =
            new SoundPlayer(Resources.Reveal);

        // Встроенный плеер для зука установки флажка.
        private readonly SoundPlayer _cellMarkedPlayer =
            new SoundPlayer(Resources.Mark);

        // Встроенный плеер для звука взрыва мин.
        private readonly SoundPlayer _minesExplodedPlayer =
            new SoundPlayer(Resources.Explode);

        // Игровое поле.
        private readonly IField _field;

        /// <summary>
        /// Создает экземпляр класса.
        /// </summary>
        /// <param name="field">Игровое поле.</param>
        public WaveSoundPlayer(IField field)
        {
            this._field = field;
            this._field.Modified += OnFieldModified;
        }
        
        ~WaveSoundPlayer()
        {
            _field.Modified -= OnFieldModified;
        }

        // Обработчик события изменения клеток поля.
        private void OnFieldModified(object sender, FieldModType modType)
        {
            switch (modType)
            {
                case FieldModType.Initialized:
                    break;

                case FieldModType.CellRevealed:
                    PlayCellRevealedSound();
                    break;

                case FieldModType.CellMarked:
                    PlayCellMarkedSound();
                    break;

                case FieldModType.MinesExploded:
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
            _cellRevealedPlayer.Play();
        }

        /// <summary>
        /// Воспроизводит звук установки флажка на клетку игрового поля.
        /// </summary>
        public void PlayCellMarkedSound()
        {
            _cellMarkedPlayer.Play();
        }

        /// <summary>
        /// Воспроизводит звук взрыва мин.
        /// </summary>
        public void PlayMinesExplodedSound()
        {
            _minesExplodedPlayer.Play();
        }
    }
}
