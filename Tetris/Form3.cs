using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form3 : Form
    {
        private int score;
        public Form3(int score)
        {
            InitializeComponent();
            this.score = score;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClasificationDB.DeleteLastScoreFromDb();
            ClasificationDB.AddScoreToDb(textBox1.Text, score);
            this.Close();
        }
    }
}
