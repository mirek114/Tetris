using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris
{
    class OneGridElement
    {
        public Color elementColor { get; set; }
        public bool Reserved { get; private set; }
        private Rectangle quadrat;
        private int x, y;
        public const int size = 20;


        public OneGridElement()
            : this(0, 0)
        { 
        }

        public OneGridElement(int x, int y)
            : this(x, y, Color.Black)
        {
        }

        public OneGridElement(int x, int y, Color color)
        {
            quadrat = new Rectangle(x * size + 2, y * size + 2, size - 2, size - 2);
            elementColor = color;
            this.x = x;
            this.y = y;
            Reserved = false;
        }

        public void Draw(Graphics graph)
        {
            SolidBrush sb = new SolidBrush(elementColor);
            graph.FillRectangle(sb, quadrat);
            graph.DrawRectangle(Pens.Black, quadrat);
            Reserved = true;
        }

        public void Delete(Graphics graph, Color bacgroundColor)
        {
            SolidBrush sb = new SolidBrush(bacgroundColor);
            graph.FillRectangle(sb, quadrat);
            graph.DrawRectangle(new Pen(bacgroundColor), quadrat);
            Reserved = false;
        }

    }
}
