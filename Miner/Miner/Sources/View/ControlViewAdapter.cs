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
        public const int CELL_SIZE = 30;

        // Контроллер графической поверхности.
        private readonly Control control;
        // Графическая поверхность.
        private readonly Graphics graphics;

        // Изображение мины.
        private readonly Image mineImage = Resources.Mine;
        // Изображение отметки возможной мины.
        private readonly Image markImage = Resources.Flag;

        // Шрифт цифр.
        private readonly Font valueFont = new Font("Arial", CELL_SIZE / 2);
        // Линия указателя выбранной клетки.
        private readonly Pen selectorPen = new Pen(Color.DarkGreen, 2);

        public ControlViewAdapter(Control control)
        {
            this.control = control ?? throw new ArgumentNullException();
            this.graphics = control.CreateGraphics();
        }

        public void ResizeControl(int fieldWidth, int fieldHeight)
        {
            int controlWidth = CELL_SIZE * fieldWidth;
            int controlHeight = CELL_SIZE * fieldHeight;
            control.ClientSize = new Size(controlWidth, controlHeight);
        }

        private (int, int) GetCoordinates(int row, int col)
        {
            int x = col * CELL_SIZE;
            int y = row * CELL_SIZE;
            return (x, y);
        }

        // Отображает скрытую клетку.
        public void DrawHiddenCell(int row, int col)
        {
            (int x, int y) = GetCoordinates(row, col);
            graphics.FillRectangle(Brushes.DarkGray, x, y, CELL_SIZE, CELL_SIZE);
            graphics.DrawRectangle(Pens.Black, x, y, CELL_SIZE, CELL_SIZE);
        }

        // Отображает отмеченную клетку.
        public void DrawMarkedCell(int row, int col)
        {
            (int x, int y) = GetCoordinates(row, col);
            graphics.DrawImage(markImage, x, y, CELL_SIZE, CELL_SIZE);
            graphics.DrawRectangle(Pens.Black, x, y, CELL_SIZE, CELL_SIZE);
        }

        // Отображает открытую клетку с миной.
        public void DrawMinedCell(int row, int col)
        {
            (int x, int y) = GetCoordinates(row, col);
            graphics.DrawImage(mineImage, x, y, CELL_SIZE, CELL_SIZE);
            graphics.DrawRectangle(Pens.Black, x, y, CELL_SIZE, CELL_SIZE);
        }

        // Отображает открытую клетку с числом мин, расположенных рядом.
        public void DrawValueCell(int value, int row, int col)
        {
            (int x, int y) = GetCoordinates(row, col);
            graphics.FillRectangle(Brushes.White, x, y, CELL_SIZE, CELL_SIZE);
            graphics.DrawString(value != 0 ? value.ToString() : String.Empty,
                valueFont, Brushes.Green, x + 1, y + 1);
            graphics.DrawRectangle(Pens.Black, x, y, CELL_SIZE, CELL_SIZE);
        }

        // Отображает указатель выбранной клетки.
        public void DrawSelector(int row, int col)
        {
            (int x, int y) = GetCoordinates(row, col);
            graphics.DrawRectangle(selectorPen, x, y, CELL_SIZE, CELL_SIZE);
        }

        public void ClearGraphics()
        {
            graphics.Clear(control.BackColor);
        }
    }
}
