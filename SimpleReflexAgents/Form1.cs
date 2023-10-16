using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleReflexAgents
{
    public partial class Form1 : Form
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());
        int currentQuadrant = 0;
        int targetX, targetY;
        int step = 5;

        List<string> labelOfStates = new List<string> { "Clean", "Clean", "Dirty", "Dirty" };

        public Form1()
        {
            InitializeComponent();
            moveTimer = new Timer();
            moveTimer.Tick += MoveVacuum;
            moveTimer.Interval = 5000;
            this.Paint += new PaintEventHandler(Form1_Paint);
        }

        private void RandomQuadrant()
        {
            currentQuadrant = random.Next(1, 5);

            int Xmin, Ymin, Xmax, Ymax;

            switch (currentQuadrant)
            {
                case 1:
                    Xmin = 0;
                    Ymin = 0;
                    Xmax = this.ClientSize.Width / 2;
                    Ymax = this.ClientSize.Height / 2;
                    break;
                case 2:
                    Xmin = this.ClientSize.Width / 2;
                    Ymin = 0;
                    Xmax = this.ClientSize.Width;
                    Ymax = this.ClientSize.Height / 2;
                    break;
                case 3:
                    Xmin = 0;
                    Ymin = this.ClientSize.Height / 2;
                    Xmax = this.ClientSize.Width / 2;
                    Ymax = this.ClientSize.Height;
                    break;
                case 4:
                    Xmin = this.ClientSize.Width / 2;
                    Ymin = this.ClientSize.Height / 2;
                    Xmax = this.ClientSize.Width;
                    Ymax = this.ClientSize.Height;
                    break;
                default:
                    Xmin = 0;
                    Ymin = 0;
                    Xmax = this.ClientSize.Width;
                    Ymax = this.ClientSize.Height;
                    break;
            }

            targetX = (Xmin + Xmax) / 2 - pictureBox2.Width / 2;
            targetY = (Ymin + Ymax) / 2 - pictureBox2.Height / 2;

            pictureBox2.Location = new Point(targetX, targetY);

            labelOfStates[currentQuadrant - 1] = (random.Next(0, 2) == 0) ? "Clean" : "Dirty";

            label1.Text = labelOfStates[0];
            label2.Text = labelOfStates[1];
            label3.Text = labelOfStates[2];
            label4.Text = labelOfStates[3];
        }




        private void MoveVacuum(object sender, EventArgs e)
        {

            if (moveTimer.Enabled)
            {
                pictureBox2.Location = new Point(targetX, targetY);
                LabelIntersection();
                RandomTarget();
                int x = targetX - pictureBox2.Location.X;
                int y = targetY - pictureBox2.Location.Y;
                int boundary_margin = 20;

                if (pictureBox2.Left <= 0 || pictureBox2.Right >= this.ClientSize.Width - boundary_margin)
                {
                    x = 0;
                }

                if (pictureBox2.Top <= 0 || pictureBox2.Bottom >= this.ClientSize.Height - boundary_margin)
                {
                    y = 0;
                }


                if (Math.Abs(x) < step && Math.Abs(y) < step)
                {
                    pictureBox2.Location = new Point(targetX, targetY);
                    RandomTarget();
                }
                else
                {
                    int newX = pictureBox2.Location.X + Math.Sign(x) * step;
                    int newY = pictureBox2.Location.Y + Math.Sign(y) * step;

                    newX = Math.Max(0, Math.Min(this.ClientSize.Width - pictureBox2.Width - boundary_margin, newX));
                    newY = Math.Max(0, Math.Min(this.ClientSize.Height - pictureBox2.Height - boundary_margin, newY));

                    pictureBox2.Location = new Point(newX, newY);
                }

                Console.WriteLine(targetX); // Debugging statement rani ha..
                Console.WriteLine(targetY); // Debugging Statement rani ha..
            }
        }

        private void LabelIntersection()
        {
            if (pictureBox2.Bounds.IntersectsWith(label1.Bounds))
            {
                if (labelOfStates[0] == "Dirty")
                {
                    labelOfStates[0] = "Clean";
                    label1.Text = labelOfStates[0];
                }
                // Console.WriteLine("Intersect"); Debugging statement rani brad mana jud ko hayz....
            }


            if (pictureBox2.Bounds.IntersectsWith(label2.Bounds))
            {
                if (labelOfStates[1] == "Dirty")
                {
                    labelOfStates[1] = "Clean";
                    label2.Text = labelOfStates[1];
                }
                // Console.WriteLine("Intersect"); Debugging statement rani brad mana jud ko hayz....
            }

            if (pictureBox2.Bounds.IntersectsWith(label3.Bounds))
            {
                if (labelOfStates[2] == "Dirty")
                {
                    labelOfStates[2] = "Clean";
                    label3.Text = labelOfStates[2];
                }
                // Console.WriteLine("Intersect"); Debugging statement rani brad mana jud ko hayz....
            }
            if (pictureBox2.Bounds.IntersectsWith(label4.Bounds))
            {
                if (labelOfStates[3] == "Dirty")
                {
                    labelOfStates[3] = "Clean";
                    label4.Text = labelOfStates[3];
                }
                // Console.WriteLine("Intersect"); Debugging statement rani brad mana jud ko hayz....
            }
        }

        private void RandomTarget()
        {
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            int newQuadrant;

            do
            {
                newQuadrant = random.Next(1, 5);
            } while (newQuadrant == currentQuadrant || IsDiagonal(currentQuadrant, newQuadrant));

            currentQuadrant = newQuadrant;

            switch (currentQuadrant)
            {
                case 1:
                    targetX = centerX / 2 - pictureBox2.Width / 2;
                    targetY = centerY / 2 - pictureBox2.Height / 2;
                    break;
                case 2:
                    targetX = centerX + centerX / 2 - pictureBox2.Width / 2;
                    targetY = centerY / 2 - pictureBox2.Height / 2;
                    break;
                case 3:
                    targetX = centerX / 2 - pictureBox2.Width / 2;
                    targetY = centerY + centerY / 2 - pictureBox2.Height / 2;
                    break;
                case 4:
                    targetX = centerX + centerX / 2 - pictureBox2.Width / 2;
                    targetY = centerY + centerY / 2 - pictureBox2.Height / 2;
                    break;
            }
        }



        private bool IsDiagonal(int currentQuadrant, int newQuadrant)
        {
            if ((currentQuadrant == 1 && newQuadrant == 4) || (currentQuadrant == 4 && newQuadrant == 1) ||
                (currentQuadrant == 2 && newQuadrant == 3) || (currentQuadrant == 3 && newQuadrant == 2))
            {
                return true;
            }

            return false;
        }



        private bool QuadrantsAllowed(int currentQuadrant, int newQuadrant)
        {

            switch (currentQuadrant)
            {
                case 1:
                    return newQuadrant == 2;
                case 2:
                    return newQuadrant == 1 || newQuadrant == 3;
                case 3:
                    return newQuadrant == 2 || newQuadrant == 4;
                case 4:
                    return newQuadrant == 3;
                default:
                    return false;
            }
        }




        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Pen pen = new Pen(Color.Black, 5);
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            graphics.DrawLine(pen, 0, centerY, this.ClientSize.Width, centerY);
            graphics.DrawLine(pen, centerX, 0, centerX, this.ClientSize.Width);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            moveTimer.Stop();
            // Console.WriteLine("button click"); Debugger again
        }

        private void button1_Click(object sender, EventArgs e)
        {
            moveTimer.Start();
            // Console.WriteLine("button click"); Debugger again
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < labelOfStates.Count; i++)
            {
                labelOfStates[i] = (random.Next(0, 2) == 0) ? "Clean" : "Dirty";
            }

            label1.Text = labelOfStates[0];
            label2.Text = labelOfStates[1];
            label3.Text = labelOfStates[2];
            label4.Text = labelOfStates[3];
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            RandomQuadrant();
        }
    }
}