using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Drawing2D;

namespace howto_draw_curve
{
   

    public partial class Form1 : Form
    {
        //this.Size = new Size(600, 600);

        // The points selected by the user.
        private List<Point> dots = new List<Point>();
        private List<Point> lines = new List<Point>();
        bool isGrid;

        //List<List<Point>> myList = new List<List<Point>>();


        List<Clines> bookList = new List<Clines>();

        string linetype = "curve";

        public Form1()
        {

            InitializeComponent();
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);


        }


        // Handle the KeyDown event to determine the type of character entered into the control.
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Initialize the flag to false.
            bool nonNumberEntered = false;
            // MessageBox.Show("ddd");

            // Determine whether the keystroke is a number from the top of the keyboard.
            if (e.KeyCode == Keys.Enter)
            {
                //MessageBox.Show("ddd");
                //Graphics.DrawCurve(Pens.Red, Points.ToArray());


            }
            //If shift key was pressed, it's not a number.
            if (Control.ModifierKeys == Keys.Shift)
            {
                nonNumberEntered = true;
            }
        }



        // Select a point.
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            dots.Add(e.Location);
            lines.Add(e.Location);
            Refresh();
           // MessageBox.Show(linetype);
        }



        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (isGrid)
            {
                DrawGrid(e);

            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // To iterate over it.
            foreach (Clines subList in bookList)
            {

                // MessageBox.Show(subList.pt.ToArray().ToString());
                if (subList.ltype == "line")
                {
                    e.Graphics.DrawCurve(Pens.Red, subList.pt.ToArray(), 0);
                }
                else if (subList.ltype == "arrow")
                {
                    //PointF[] Poly = { rPoint, lPoint, toline };
                    PointF fromline = new Point(subList.pt[0].X, subList.pt[0].Y);
                    //toline = new Point(40, 70);
                    PointF toline = new Point(subList.pt[1].X, subList.pt[1].Y);

                    PointF[] Poly = Arrow(fromline, toline);
                    SolidBrush brush = new SolidBrush(Color.Red);
                    e.Graphics.FillPolygon(brush, Poly);

                    e.Graphics.DrawCurve(Pens.Red, lines.ToArray(), 0);

                }
                else e.Graphics.DrawCurve(Pens.Red, subList.pt.ToArray(), 0.65f);


            }

            // Draw the points.
            foreach (Point point in dots)
                e.Graphics.FillEllipse(Brushes.Black,
                    point.X - 3, point.Y - 3, 5, 5);

            if (lines.Count < 2) return;

            // Draw the curve.
            if (linetype == "line")
            {
                e.Graphics.DrawCurve(Pens.Red, lines.ToArray(), 0);
            }
            else if (linetype == "arrow")
            {
                                              
                PointF fromline = new Point(lines[0].X, lines[0].Y);
                //toline = new Point(40, 70);
                PointF toline = new Point(lines[1].X, lines[1].Y);

                PointF[] Poly = Arrow(fromline, toline);

                SolidBrush brush = new SolidBrush(Color.Red);
                e.Graphics.FillPolygon(brush, Poly);

                e.Graphics.DrawCurve(Pens.Red, lines.ToArray(), 0);

            }
            else e.Graphics.DrawCurve(Pens.Red, lines.ToArray(), 0.65f);




        }
        private PointF[] Arrow(PointF fromline, PointF toline)
        {
            float nWidth = 6.2f;
            //float theta=0.196f;
            float theta = 0.442f;

            PointF lineVector = new PointF(toline.X - fromline.X, toline.Y - fromline.Y);

            double dist = GetDistance(0, 0, (double)lineVector.X, (double)lineVector.Y);


            //fcPoint = new Point((float)(toline.X - nWidth / (2 * Math.Tan(theta))*lineVector.X), (float)(toline.Y - nWidth / (2 * Math.Tan(theta)) * lineVector.Y));
            PointF fcPoint = new PointF((float)(toline.X - nWidth / (2 * Math.Tan(theta)) * lineVector.X / dist),
                (float)(toline.Y - nWidth / (2 * Math.Tan(theta)) * lineVector.Y / dist));

            //MessageBox.Show((fcPoint.X).ToString());


            PointF normalVector = new PointF(-lineVector.Y, lineVector.X);
            double ndist = GetDistance(0, 0, (double)normalVector.X, (double)normalVector.Y);
            //MessageBox.Show(ndist.ToString());


            PointF rPoint = new PointF((float)(nWidth / 2 * (normalVector.X / ndist) + fcPoint.X),
                                (float)(nWidth / 2 * (normalVector.Y / ndist) + fcPoint.Y));
            PointF lPoint = new PointF((float)(nWidth / 2 * (-normalVector.X / ndist) + fcPoint.X),
                                    (float)(nWidth / 2 * (-normalVector.Y / ndist) + fcPoint.Y));

            PointF[] tmp = { rPoint, lPoint, toline };

            return tmp;

        }

        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        // Start a new point list.
        private void mnuCurveNew_Click(object sender, EventArgs e)
        {


            dots = new List<Point>();
            lines = new List<Point>();
            bookList.Clear();

            Refresh();
        }
        private void mnuCurveCom_Click(object sender, EventArgs e)
        {
            Clines old = new Clines();

            dots = new List<Point>();

            old.ltype = linetype;

            old.pt = new List<Point>(lines);

            // MessageBox.Show(old.pt.ToString());

            bookList.Add(old);

            lines = new List<Point>();

            Refresh();
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
                     toolStripStatusLabel1.Text = "Line Selected";
            linetype = "line";
        }

        private void curveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            linetype = "curve";
            toolStripStatusLabel1.Text = "Curve Selected";
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void arrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            linetype = "arrow";
            toolStripStatusLabel1.Text = "화살표 선택됨";
        }

        private void 격자ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isGrid = true;
            Refresh();
        }
        private void DrawGrid(PaintEventArgs e)
        {
            Point a, b;
            int i = 0;
            while (i < this.Width)
            {
                a = new Point(i, 0);
                b = new Point(i, this.Height);

                if (i % 150 == 0)
                {
                    Pen pen1 = new Pen(Color.Gray, 1.0f);
                    e.Graphics.DrawLine(pen1, a, b);
                }
                else
                {
                    Pen pen2 = new Pen(Color.LightGray, 1.0f);
                    e.Graphics.DrawLine(pen2, a, b);
                }
                i += 50;
            }
            int j = 0;
            while (j < this.Height)
            {
                a = new Point(0, j);
                b = new Point(this.Width, j);

                if (j % 150 == 0)
                {
                    Pen pen1 = new Pen(Color.Gray, 1.0f);
                    e.Graphics.DrawLine(pen1, a, b);
                }
                else
                {
                    Pen pen2 = new Pen(Color.LightGray, 1.0f);
                    e.Graphics.DrawLine(pen2, a, b);
                }
                j += 50;
            }

        }

        private void pictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox playerBox = new PictureBox();
            if (Clipboard.ContainsImage())
            {

                playerBox.SizeMode = PictureBoxSizeMode.StretchImage;

                playerBox.Image = Clipboard.GetImage();
                playerBox.Width = 128;
                playerBox.Height = 132;
                playerBox.Location = new Point(0,0);

                this.Controls.Add(playerBox);
            }
            else
            {
                MessageBox.Show("Clipboard is empty. Please Copy Image.");
            }
        }
    }
}
