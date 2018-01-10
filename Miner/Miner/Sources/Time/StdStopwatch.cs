using System;
using System.Diagnostics;

namespace Miner.Time
{
    /// <summary>
    /// Секундомер, использующий стандартную библиотеку.
    /// </summary>
    public class StdStopwatch : IStopwatch
    {
        // Секундомер.
        private readonly Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        /// Создает экземпляр класса.
        /// </summary>
        public StdStopwatch()
        {
            _stopwatch.Reset();
        }

        /// <summary>
        /// Запускает секундомер.
        /// </summary>
        public void Start()
        {
            _stopwatch.Start();
        }

        /// <summary>
        /// Останавливает секундомер.
        /// </summary>
        public void Stop()
        {
            _stopwatch.Stop();
        }

        /// <summary>
        /// Останавливает секундомер,
        /// обнуляет затраченное время
        /// и начинает измерение заново.
        /// </summary>
        public void Restart()
        {
            _stopwatch.Restart();
        }

        /// <summary>
        /// Останавливает секундомер и
        /// обнуляет затраченное время.
        /// </summary>
        public void Reset()
        {
            _stopwatch.Reset();
        }

        /// <summary>
        /// Возвращает количество секунд,
        /// прошедших от начала измерения.
        /// </summary>
        public int ElapsedSeconds
        {
            get
            {
                var elapsed = _stopwatch.Elapsed;
                return elapsed.Hours * 3600 +
                    elapsed.Minutes * 60 +
                    elapsed.Seconds;
            }
        }
    }
}
