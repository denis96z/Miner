using System;

namespace Miner.Field
{
    public class NumberOfMines : CellObject
    {
        /// <summary>
        /// Минимальное число мин, окружащих клетку.
        /// </summary>
        public const int MIN_VALUE = 0;

        /// <summary>
        /// Максимальное число мин, окружащих клетку.
        /// </summary>
        public const int MAX_VALUE = 8;

        // Число мин, окружающих клетку.
        private int numMines = 0;

        /// <summary>
        /// Создает экземпляр класса.
        /// </summary>
        /// <param name="value">Число мин, окружающих клетку.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public NumberOfMines(int value = MIN_VALUE)
        {
            Value = value;
        }

        /// <summary>
        /// Возвращает или задает число мин, окружающих клетку.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int Value
        {
            get
            {
                return numMines;
            }

            set
            {
                if (value >= MIN_VALUE && value <= MAX_VALUE)
                {
                    numMines = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
