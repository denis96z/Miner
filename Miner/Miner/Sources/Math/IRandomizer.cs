using System;

namespace Miner.Math
{
    /// <summary>
    /// Генератор случайных чисел.
    /// </summary>
    public interface IRandomizer
    {
        /// <summary>
        /// Возвращает случайное число в заданном диапазоне.
        /// </summary>
        /// <param name="minValue">Минимальное значение случайного числа.</param>
        /// <param name="maxValue">Максимальное значение случайного числа.</param>
        int GetValue(int minValue, int maxValue);
    }
}
