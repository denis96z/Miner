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
        private readonly Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Создает экземпляр класса.
        /// </summary>
        public StdStopwatch()
        {
            stopwatch.Reset();
        }

        /// <summary>
        /// Запускает секундомер.
        /// </summary>
        public void Start()
        {
            stopwatch.Start();
        }

        /// <summary>
        /// Останавливает секундомер.
        /// </summary>
        public void Stop()
        {
            stopwatch.Stop();
        }

        /// <summary>
        /// Останавливает секундомер,
        /// обнуляет затраченное время
        /// и начинает измерение заново.
        /// </summary>
        public void Restart()
        {
            stopwatch.Restart();
        }

        /// <summary>
        /// Останавливает секундомер и
        /// обнуляет затраченное время.
        /// </summary>
        public void Reset()
        {
            stopwatch.Reset();
        }

        /// <summary>
        /// Возвращает количество секунд,
        /// прошедших от начала измерения.
        /// </summary>
        public int ElapsedSeconds
        {
            get
            {
                var elapsed = stopwatch.Elapsed;
                return elapsed.Hours * 3600 +
                    elapsed.Minutes * 60 +
                    elapsed.Seconds;
            }
        }
    }
}
