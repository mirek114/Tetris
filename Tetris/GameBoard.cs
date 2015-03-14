using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class GameBoard : Board
    {
        private int block_x, block_y;
        private Block block;

        public GameBoard()
        {
            block_x = block_y = 0;
        }

        public GameBoard(int a, int b)
            : base(a, b)
        {
            block_x = block_y = 0;
        }

        public bool DrawBlock(int x, int y, Block b)
        {
            if (!CheckDimensions(x, y)) return false;

            for (int i = 0; i < Block.gridSize; i++)
            {
                for (int j = 0; j < Block.gridSize; j++)
                {
                    if (b.grid[i, j])
                    {
                        if (!CheckDimensions(x + i, y + j)) return false;
                        if (boardGrid[x + i, y + j].Reserved) return false;
                    }
                }
            }

            block_x = x;
            block_y = y;

            if (block != b) block = b;

            for (int i = 0; i < Block.gridSize; i++)
            {
                for (int j = 0; j < Block.gridSize; j++)
                {
                    if (b.grid[i, j])
                    {
                        boardGrid[x + i, y + j].elementColor=b.blockColor;
                        DrawOneGridElement(x + i, y + j);
                    }
                }
            }

            return true;
        }

        public bool GoDown()
        {
            if (block == null) return false;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (block.grid[i, j] && (j == 3 || !block.grid[i, j + 1]))
                    {
                        if (!CheckDimensions(block_x + i, block_y + j + 1)) return false;
                        if (boardGrid[block_x + i, block_y + j+1].Reserved) return false;
                    }
                }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (block.grid[i, j]) boardGrid[block_x + i, block_y + j].Delete(graph, BackColor);
                }
            }
            DrawBlock(block_x, block_y + 1, block);
            return true;
        }

        public bool GoLeft()
        {
            if (block == null) return false;
            int i, j;
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                {
                    if (block.grid[i, j] && (i == 0 || !block.grid[i - 1, j]))
                    {
                        if (!CheckDimensions(block_x + i - 1, block_y + j)) return false;
                        if (boardGrid[block_x + i - 1, block_y + j].Reserved) return false;
                    }
                }

            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    if (block.grid[i, j]) boardGrid[block_x + i, block_y + j].Delete(graph, BackColor);
                }
            }
            DrawBlock(block_x - 1, block_y, block);
            return true;
        }

        public bool GoRight()
        {
            if (block == null) return false;
            int i, j;
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                {
                    if (block.grid[i, j] && (i == 3 || !block.grid[i + 1, j]))
                    {
                        if (!CheckDimensions(block_x + i + 1, block_y + j)) return false;
                        if (boardGrid[block_x + i + 1, block_y + j].Reserved) return false;
                    }
                }

            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    if (block.grid[i, j]) boardGrid[block_x + i, block_y + j].Delete(graph, BackColor);
                }
            }
            DrawBlock(block_x + 1, block_y, block);
            return true;
        }

        
        
        public void TurnLeft()
        {
            if (block.angle == 0) block.angle = 270;
            else block.angle -= 90;
            Block newBlock = block.Turn(block.angle);
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (newBlock.grid[i, j] && !CheckDimensions(block_x + i, block_y + j)) return;
                    if (newBlock.grid[i, j] && !block.grid[i, j] && boardGrid[block_x + i, block_y + j].Reserved) return;
                }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (block.grid[i, j]) boardGrid[block_x + i, block_y + j].Delete(graph, BackColor);
                }
            }
            DrawBlock(block_x, block_y, newBlock);
        }

        public void TurnRight()
        {
            if (block.angle == 270) block.angle = 0;
            else block.angle += 90;
            Block newBlock = block.Turn(block.angle);
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (newBlock.grid[i, j] && !CheckDimensions(block_x + i, block_y + j)) return;
                    if (newBlock.grid[i, j] && !block.grid[i, j] && boardGrid[block_x + i, block_y + j].Reserved) return;
                }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (block.grid[i, j]) boardGrid[block_x + i, block_y + j].Delete(graph, BackColor);
                }
            }

            DrawBlock(block_x, block_y, newBlock);
        }

        public void ClearLine(int n)
        {
            if (!CheckDimensions(1, n)) return;
            for (int i = 0; i < screenWidth; i++)
                boardGrid[i, n].Delete(graph, BackColor);
        }

        public void MoveLine(int n)
        {
            if (n < 0 || n >= screenHeight - 1) return;

            for (int i = 0; i < screenWidth; i++)
            {
                if (!boardGrid[i, n].Reserved)
                {
                    boardGrid[i, n + 1].Delete(graph, BackColor);
                    continue;
                }
                boardGrid[i, n + 1].elementColor = boardGrid[i, n].elementColor;
                boardGrid[i, n + 1].Draw(graph);
                boardGrid[i, n].Delete(graph, BackColor);
            }
        }

        public int Lines()
        {
            int n = screenHeight - 1;

            int lineCount = 0;
            bool czylinia = true;
            while (n > 0 && lineCount < 4)
            {
                for (int i = 0; i < screenWidth; i++)
                    if (!boardGrid[i, n].Reserved) czylinia = false;
                if (czylinia)
                {
                    lineCount++;
                    for (int m = n - 1; m >= 0; m--)
                        MoveLine(m);
                }
                else n--;
                czylinia = true;
            }
            return lineCount;
        }

        public Block getBlock()
        {
            return block;
        }
    }
}
