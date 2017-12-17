using System;

namespace Miner.Time
{
    /// <summary>
    /// Секундомер.
    /// </summary>
    public interface IStopwatch
    {
        /// <summary>
        /// Запускает секундомер.
        /// </summary>
        void Start();

        /// <summary>
        /// Останавливает секундомер.
        /// </summary>
        void Stop();

        /// <summary>
        /// Останавливает секундомер,
        /// обнуляет затраченное время
        /// и начинает измерение заново.
        /// </summary>
        void Restart();

        /// <summary>
        /// Останавливает секундомер и
        /// обнуляет затраченное время.
        /// </summary>
        void Reset();

        /// <summary>
        /// Возвращает количество секунд,
        /// прошедших от начала измерения.
        /// </summary>
        int ElapsedSeconds { get; }
    }
}
