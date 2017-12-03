using System;

namespace Miner.Math
{
    /// <summary>
    /// Генератор псевдослучайных чисел.
    /// </summary>
    public class StdRandomizer : IRandomizer
    {
        /// <summary>
        /// Встроенный генератор случайных чисел.
        /// </summary>
        private readonly Random random = new Random();

        /// <summary>
        /// Возвращает случайное число в заданном диапазоне.
        /// </summary>
        /// <param name="minValue">Минимальное значение случайного числа.</param>
        /// <param name="maxValue">Максимальное значение случайного числа.</param>
        /// <returns></returns>
        public int GetValue(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue + 1);
        }
    }
}
