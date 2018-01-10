using System;

namespace Miner.Data
{
    /// <summary>
    /// Представляет число мин, окружающих клетку поля.
    /// </summary>
    public class NumberOfMines : CellObject
    {
        /// <summary>
        /// Минимальное число мин, окружащих клетку.
        /// </summary>
        public const int MinValue = 0;

        /// <summary>
        /// Максимальное число мин, окружащих клетку.
        /// </summary>
        public const int MaxValue = 8;

        // Число мин, окружающих клетку.
        private int _numMines = 0;

        /// <summary>
        /// Создает экземпляр класса.
        /// </summary>
        /// <param name="value">Число мин, окружающих клетку.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public NumberOfMines(int value = MinValue)
        {
            Value = value;
        }

        /// <summary>
        /// Возвращает или задает число мин, окружающих клетку.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int Value
        {
            get => _numMines;

            set
            {
                if (value >= MinValue && value <= MaxValue)
                {
                    _numMines = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
