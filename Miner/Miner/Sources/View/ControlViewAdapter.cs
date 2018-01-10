using System;
using Miner.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace Miner.View
{
    public class ControlViewAdapter : IControlViewAdapter
    {
        /// <summary>
        /// Ширина и высота клетки игрового поля.
        /// </summary>
        public const int CellSize = 30;

        // Контроллер графической поверхности.
        private readonly Control _control;
        // Графическая поверхность.
        private readonly Graphics _graphics;

        // Изображение мины.
        private readonly Image _mineImage = Resources.Mine;
        // Изображение отметки возможной мины.
        private readonly Image _markImage = Resources.Flag;

        // Шрифт цифр.
        private readonly Font _valueFont = new Font("Arial", CellSize / 2);
        // Линия указателя выбранной клетки.
        private readonly Pen _selectorPen = new Pen(Color.DarkGreen, 2);

        public ControlViewAdapter(Control control)
        {
            this._control = control ?? throw new ArgumentNullException();
            this._graphics = control.CreateGraphics();
        }

        public void ResizeControl(int fieldWidth, int fieldHeight)
        {
            int controlWidth = CellSize * fieldWidth;
            int controlHeight = CellSize * fieldHeight;
            _control.ClientSize = new Size(controlWidth, controlHeight);
        }

        private (int, int) GetCoordinates(int row, int col)
        {
            int x = col * CellSize;
            int y = row * CellSize;
            return (x, y);
        }

        // Отображает скрытую клетку.
        public void DrawHiddenCell(int row, int col)
        {
            (int x, int y) = GetCoordinates(row, col);
            _graphics.FillRectangle(Brushes.DarkGray, x, y, CellSize, CellSize);
            _graphics.DrawRectangle(Pens.Black, x, y, CellSize, CellSize);
        }

        // Отображает отмеченную клетку.
        public void DrawMarkedCell(int row, int col)
        {
            (int x, int y) = GetCoordinates(row, col);
            _graphics.DrawImage(_markImage, x, y, CellSize, CellSize);
            _graphics.DrawRectangle(Pens.Black, x, y, CellSize, CellSize);
        }

        // Отображает открытую клетку с миной.
        public void DrawMinedCell(int row, int col)
        {
            (int x, int y) = GetCoordinates(row, col);
            _graphics.DrawImage(_mineImage, x, y, CellSize, CellSize);
            _graphics.DrawRectangle(Pens.Black, x, y, CellSize, CellSize);
        }

        // Отображает открытую клетку с числом мин, расположенных рядом.
        public void DrawValueCell(int value, int row, int col)
        {
            (int x, int y) = GetCoordinates(row, col);
            _graphics.FillRectangle(Brushes.White, x, y, CellSize, CellSize);
            _graphics.DrawString(value != 0 ? value.ToString() : String.Empty,
                _valueFont, Brushes.Green, x + 1, y + 1);
            _graphics.DrawRectangle(Pens.Black, x, y, CellSize, CellSize);
        }

        // Отображает указатель выбранной клетки.
        public void DrawSelector(int row, int col)
        {
            (int x, int y) = GetCoordinates(row, col);
            _graphics.DrawRectangle(_selectorPen, x, y, CellSize, CellSize);
        }

        public void ClearGraphics()
        {
            _graphics.Clear(_control.BackColor);
        }
    }
}
