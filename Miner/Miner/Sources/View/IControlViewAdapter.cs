using System;
using System.Drawing;

namespace Miner.View
{
    public interface IControlViewAdapter
    {
        void ResizeControl(int fieldWidth, int fieldHeight);

        void DrawHiddenCell(int row, int col);
        void DrawMarkedCell(int row, int col);
        void DrawMinedCell(int row, int col); 
        void DrawValueCell(int value, int row, int col);
        void DrawSelector(int row, int col);

        void ClearGraphics();
    }
}
