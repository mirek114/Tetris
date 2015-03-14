using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris
{
    class Block
    {
        public enum Blocks { L, I, T, Quadrat, Lightning }
        public enum Colors { Blue, Red, Yellow, Green, Orange }
        public bool[,] grid;
        public Blocks selected;
        public Color blockColor;
        public int angle;
        public const int gridSize = 4;

        public static Random rdn = new Random();


        public Block()
        {
            angle = 0;
            grid = new bool[4, 4];
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    grid[i, j] = false;
                }
            }

            Colors drawnColor = (Colors)rdn.Next(5);
            switch (drawnColor)
            {
                case Colors.Blue: blockColor = Color.Blue; break;
                case Colors.Green: blockColor = Color.Green; break;
                case Colors.Orange: blockColor = Color.Orange; break;
                case Colors.Red: blockColor = Color.Red; break;
                case Colors.Yellow: blockColor = Color.Yellow; break;
            }

            Blocks drawnBlock = (Blocks)rdn.Next(5);
            switch (drawnBlock)
            {
                case Blocks.I: grid[0, 0] = grid[1, 0] = grid[2, 0] = grid[3, 0] = true;
                    selected = Blocks.I; break;
                case Blocks.L: grid[0, 0] = grid[0, 1] = grid[0, 2] = grid[0, 3] = grid[1, 3] = true;
                    selected = Blocks.L; break;
                case Blocks.Lightning: grid[0, 0] = grid[1, 0] = grid[1, 1] = grid[2, 1] = true;
                    selected = Blocks.Lightning; break;
                case Blocks.Quadrat: grid[0, 0] = grid[0, 1] = grid[1, 0] = grid[1, 1] = true;
                    selected = Blocks.Quadrat; break;
                case Blocks.T: grid[0, 0] = grid[0, 1] = grid[0, 2] = grid[1, 1] = true;
                    selected = Blocks.T; break;
            }
        }

        public Block Turn(int newAngle)
        {
            if (newAngle != 0 && newAngle != 90 && newAngle != 180 && newAngle != 270) return this;

            Block newBlock = new Block();
            newBlock.angle = newAngle;
            newBlock.blockColor = blockColor;
            newBlock.selected = selected;
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    newBlock.grid[i, j] = false;
                }
            }

            switch (selected)
            {
                case Blocks.I:
                    switch (newAngle)
                    {
                        case 0:
                        case 180: newBlock.grid[0, 0] = newBlock.grid[1, 0] = newBlock.grid[2, 0] = newBlock.grid[3, 0] = true; break;
                        case 90:
                        case 270: newBlock.grid[0, 0] = newBlock.grid[0, 1] = newBlock.grid[0, 2] = newBlock.grid[0, 3] = true; break;

                    }
                    break;
                case Blocks.L:
                    switch (newAngle)
                    {
                        case 0: newBlock.grid[0, 0] = newBlock.grid[0, 1] = newBlock.grid[0, 2] = newBlock.grid[0, 3] = newBlock.grid[1, 3] = true; break;
                        case 90: newBlock.grid[0, 0] = newBlock.grid[0, 1] = newBlock.grid[1, 0] = newBlock.grid[2, 0] = newBlock.grid[3, 0] = true; break;
                        case 180: newBlock.grid[0, 0] = newBlock.grid[1, 0] = newBlock.grid[1, 1] = newBlock.grid[1, 2] = newBlock.grid[1, 3] = true; break;
                        case 270: newBlock.grid[3, 0] = newBlock.grid[3, 1] = newBlock.grid[2, 1] = newBlock.grid[1, 1] = newBlock.grid[0, 1] = newBlock.grid[3, 1] = true; break;

                    }
                    break;
                case Blocks.Lightning:
                    switch (newAngle)
                    {
                        case 0: newBlock.grid[0, 0] = newBlock.grid[1, 0] = newBlock.grid[1, 1] = newBlock.grid[2, 1] = true; break;
                        case 90: newBlock.grid[1, 0] = newBlock.grid[1, 1] = newBlock.grid[0, 1] = newBlock.grid[0, 2] = true; break;
                        case 180: newBlock.grid[0, 3] = newBlock.grid[1, 3] = newBlock.grid[1, 2] = newBlock.grid[2, 2] = true; break;
                        case 270: newBlock.grid[0, 0] = newBlock.grid[0, 1] = newBlock.grid[1, 1] = newBlock.grid[1, 2] = true; break;
                    }
                    break;

                case Blocks.Quadrat:
                    newBlock.grid[0, 0] = newBlock.grid[0, 1] = newBlock.grid[1, 0] = newBlock.grid[1, 1] = true; break;

                case Blocks.T:
                    switch (newAngle)
                    {
                        case 0: newBlock.grid[0, 0] = newBlock.grid[0, 1] = newBlock.grid[0, 2] = newBlock.grid[1, 1] = true; break;
                        case 90: newBlock.grid[0, 0] = newBlock.grid[1, 0] = newBlock.grid[2, 0] = newBlock.grid[1, 1] = true; break;
                        case 180: newBlock.grid[1, 0] = newBlock.grid[1, 1] = newBlock.grid[1, 2] = newBlock.grid[0, 1] = true; break;
                        case 270: newBlock.grid[0, 1] = newBlock.grid[1, 1] = newBlock.grid[2, 1] = newBlock.grid[1, 0] = true; break;
                    }
                    break;
            }
            return newBlock;
        }

    }
}
