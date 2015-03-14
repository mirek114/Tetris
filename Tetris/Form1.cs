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
    public partial class Form1 : Form
    {
        private GameBoard board;
        private GameBoard nextBlock;
        private DateTime startTime;
        private TimeSpan timeForLvl;
        private Label Pkt, Poziom;
        private Label lblPkt, lblPoziom;
        private Label info;
        private int counterInterval;
        public int level;
        public int score;

        private bool fast;
        private bool game;
        private bool pause;

        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            level = 1;
            score = 0;
            game = false;
            pause = false;
            counterInterval = 1000;

            startTime = DateTime.Now;
            timeForLvl = new TimeSpan(0, 1, 0);

            counter.Interval = counterInterval;
            fast = false;

            this.board = new GameBoard(10, 18);
            this.nextBlock = new GameBoard(4, 4);

            this.board.Location = new System.Drawing.Point(15, 30);
            this.board.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.board.Name = "GameBoard";
            this.board.TabIndex = 0;
            this.board.Paint += new PaintEventHandler(this.Plansza_Paint);

            this.nextBlock.Location = new System.Drawing.Point(280, 30);
            this.nextBlock.Name = "nextBlock";
            this.nextBlock.TabIndex = 0;
            this.nextBlock.Paint += new PaintEventHandler(this.Plansza_Paint);

            this.Controls.Add(this.nextBlock);
            this.Controls.Add(this.board);

            this.lblPkt = new Label();
            this.lblPoziom = new Label();
            this.Pkt = new Label();
            this.Poziom = new Label();
            this.info = new Label();

            this.lblPkt.AutoSize = true;
            this.lblPkt.Location = new System.Drawing.Point(280, 140);
            this.lblPkt.Name = "lblPkt";
            this.lblPkt.Size = new System.Drawing.Size(35, 13);
            this.lblPkt.TabIndex = 0;
            this.lblPkt.Text = "Punkty:";

            this.lblPoziom.AutoSize = true;
            this.lblPoziom.Location = new System.Drawing.Point(280, 120);
            this.lblPoziom.Name = "lblPoziom";
            this.lblPoziom.Size = new System.Drawing.Size(35, 13);
            this.lblPoziom.TabIndex = 0;
            this.lblPoziom.Text = "Poziom:";

            this.Pkt.AutoSize = true;
            this.Pkt.Location = new System.Drawing.Point(330, 140);
            this.Pkt.Name = "Pkt";
            this.Pkt.Size = new System.Drawing.Size(35, 13);
            this.Pkt.TabIndex = 0;
            this.Pkt.Text = "0";

            this.Poziom.AutoSize = true;
            this.Poziom.Location = new System.Drawing.Point(330, 120);
            this.Poziom.Name = "Poziom";
            this.Poziom.Size = new System.Drawing.Size(35, 13);
            this.Poziom.TabIndex = 0;
            this.Poziom.Text = "1";


            this.Controls.Add(this.lblPoziom);
            this.Controls.Add(this.lblPkt);
            this.Controls.Add(this.Pkt);
            this.Controls.Add(this.Poziom);
            this.Controls.Add(this.info);
        }

        private void counter_Tick(object sender, EventArgs e)
        {
            if (!board.GoDown())
            {
                if (fast)
                {
                    fast = false;
                    counter.Interval = counterInterval;

                }
                int pkt = board.Lines();
                score += pkt * level;
                Pkt.Text = score.ToString();
                if (DateTime.Now - startTime > timeForLvl)
                {
                    level++;
                    startTime = DateTime.Now;
                    Poziom.Text = level.ToString();
                    counterInterval = counterInterval * 3 / 5;
                    counter.Interval = counterInterval;

                }
                Block b = nextBlock.getBlock();
                if (!board.DrawBlock(5, 0, b))
                {
                    counter.Stop();
                    counter.Interval = counterInterval;

                    if (score > ClasificationDB.GetWorstScoreResul())
                    {
                        Form3 f = new Form3(score);
                        f.Show();

                    }
                    else
                    {
                        MessageBox.Show("Twój wynik to: " + score + "\n Niestety nie dostałeś się \n na listę najlepszych:(");
                    }
                    game = false;
                    b = null;
                    board.Reset();
                    nextBlock.Reset();
                    level = 1;
                    score = 0;
                    Pkt.Text = "0";
                    Poziom.Text = "1";
                    counterInterval = 1000;
                    counter.Interval = counterInterval;
                    return;
                }
                nextBlock.Reset();
                nextBlock.DrawBlock(0, 0, new Block());
            }
        }
        private void Plansza_Paint(object sender, PaintEventArgs e)
        {
            GameBoard gb = (GameBoard)sender;
            gb.RefreshBoard();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !game)
            {
                game = true;
                board.DrawBlock(5, 0, new Block());
                nextBlock.DrawBlock(0, 0, new Block());
                counter.Start();
            }
            else if (e.KeyCode == Keys.Left && game && !pause) board.GoLeft();
            else if (e.KeyCode == Keys.Right && game && !pause) board.GoRight();
            else if (e.KeyCode == Keys.Z && game && !pause) board.TurnLeft();
            else if ((e.KeyCode == Keys.X) && game && !pause) board.TurnRight();
            else if (e.KeyCode == Keys.Down && game && !pause)
            {
                if (fast)
                {
                    counter.Interval = counterInterval;
                    fast = false;
                }
                else if (!fast)
                {
                    counter.Interval = 50;
                    fast = true;
                }
            }
            else if (e.KeyCode == Keys.Escape && game)
            {
                game = false;
                board.Reset();
                nextBlock.Reset();
                level = 1;
                score = 0;
                Poziom.Text = "1";
                Pkt.Text = "0";
                counter.Stop();
                counterInterval = 1000;
                counter.Interval = counterInterval;
            }
            else if (e.KeyCode == Keys.Space && game)
            {
                if (pause)
                {
                    counter.Start();
                    pause = false;
                }
                else
                {
                    counter.Stop();
                    pause = true;
                }
            }

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enter - start \n Esc - koniec \n Space - pauza \n Z/X - obrót klocka w lewo/prawo \n left/right - przesuniecie klocka w lewo/prwo \n down - przyspieszenie/zwolnienie");
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exitToolStripMenuItem_Click(new object(), new EventArgs());
            game = true;
            board.DrawBlock(5, 0, new Block());
            nextBlock.DrawBlock(0, 0, new Block());
            counter.Start();
        }

        private void topResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game = false;
            board.Reset();
            nextBlock.Reset();
            level = 1;
            score = 0;
            Poziom.Text = "1";
            Pkt.Text = "0";
            counter.Stop();
            counterInterval = 1000;
            counter.Interval = counterInterval;
        }
    }
}
