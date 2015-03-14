using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    class Board : Panel
    {
        protected Graphics graph;
        protected int screenWidth;
        protected int screenHeight;
        protected OneGridElement[,] boardGrid;

        public Board()
            : this(10,10)
        {
        }

        public Board(int a, int b)
        {
            if (a <= 0 || b <= 0) a = b = 10;

            screenWidth = a;
            screenHeight = b;

            this.Size = new System.Drawing.Size(screenWidth * OneGridElement.size + 4, screenHeight * OneGridElement.size + 4);
            boardGrid = new OneGridElement[screenWidth, screenHeight];
            for (int i = 0; i < screenWidth; i++)
            {
                for (int j = 0; j < screenHeight; j++)
                {
                    boardGrid[i, j] = new OneGridElement(i, j);
                }
            }
            graph = CreateGraphics();
            this.Show();
        }

        public bool CheckDimensions(int x, int y)
        {
            if (x >= screenWidth || x < 0 || y >= screenHeight || y < 0)
                return false;

            return true;
        }

        public bool DrawOneGridElement(int x, int y)
        {
            if(!CheckDimensions(x,y)) return false;

            if (boardGrid[x, y].Reserved) return false;

            boardGrid[x, y].Draw(graph);

            return true;
        }

        public void RefreshBoard()
        {
            for (int i = 0; i < screenWidth; i++)
            {
                for (int j = 0; j < screenHeight; j++)
                {
                    if (boardGrid[i, j].Reserved)
                        boardGrid[i, j].Draw(graph);
                }
            }
        }

        public void Reset()
        {
            for (int i = 0; i < screenWidth; i++)
            {
                for (int j = 0; j < screenHeight; j++)
                {
                    boardGrid[i, j].Delete(graph, BackColor);
                }
            }
        }
    }
}
