using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KG_4
{
    public partial class Form1 : Form
    {
        Graphics g;
        float W;
        float H;
        float x_center;
        float y_center;
        float dx;
        float dy;
        int x1 = -2;
        int y1 = 1;
        int x2 = 6;
        int y2 = 3;
        int R = 5;
        int x_centering;
        int y_centering;
        bool flag = false;
        bool invertingX = false;
        bool invertingY = false;
        bool invertingXY = false;
        bool startBrzd = false;
        bool startPic = false;
        Pen pen = new Pen(Color.Black);
        Pen linePen = new Pen(Color.Red, 2);
        Pen linePen2 = new Pen(Color.Green, 2);
        Pen pointLine = new Pen(Color.Blue, 10);
        Pen Rec = new Pen(Color.Black);
        Brush Br = Brushes.Gray;
        List<Point> points = new List<Point>();
        List<Point> pic = new List<Point>();
        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            W = this.pictureBox1.Width;
            H = this.pictureBox1.Height;
            x_center = W / 2;
            y_center = H / 2;
            dx = W / 20;
            dy = H / 20;
        }
        private void DrawAxis()
        {
            g.Clear(Color.White);
            Pen Axis = new Pen(Color.Black, 3);
            g.DrawLine(Axis, x_center, 0, x_center, H);
            g.DrawLine(Axis, 0, y_center, W, y_center);
            Font Fon = new Font("Arial", 9, FontStyle.Regular);
            Brush Br = Brushes.Black;
            g.DrawString("X", Fon, Br, W - 15, y_center + 10);
            g.DrawString("Y", Fon, Br, x_center - 20, 10);
            for (int i = -10; i < 10; i++)
            {
                g.DrawString(i.ToString(), Fon, Br, x_center - 15, y_center + dy * i);
                g.DrawString(i.ToString(), Fon, Br, x_center + dx * i - 10, y_center + 10);
            }
            for (int i = 0; i < 20; i++)
            {
                g.DrawLine(pen, 1, 1 * i * dy, W, 1 * i * dy);
                g.DrawLine(pen, 1 * i * dx, 1, 1 * i * dx, H);
            }
        }

        PointF Convert(PointF a)
        {
            return new PointF(x_center + a.X * dx, y_center + a.Y * -dy);
        }
        float X(int x)
        {
            return x_center + x * dx;
        }
        float Y(int y)
        {
            return y_center + y * -dy;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawAxis();
            CerusBeck();
        }

        private void CerusBeck()
        {
            PointF P0 = new Point(5, -1);
            PointF P1 = new Point(7, 2);
            PointF vec = new PointF(P1.X- P0.X, P1.Y- P0.Y);
            g.DrawLine(pen,Convert(P0),Convert(P1));
            PointF D = minus(P1, P0);
            List<PointF> points = new List<PointF>();
            points.Add(new PointF(2, 0));
            points.Add(new PointF(4, 0));
            points.Add(new PointF(6, 2));
            points.Add(new PointF(6, 4));
            points.Add(new PointF(4, 6));
            points.Add(new PointF(2, 6));
            points.Add(new PointF(0, 4));
            points.Add(new PointF(0, 2));
            List<float> t_H = new List<float>();
            List<float> t_H2 = new List<float>();
            List<float> t_L = new List<float>();
            List<float> t_L2 = new List<float>();
            List<PointF> normal = new List<PointF>();
            for (int i = 0; i < points.Count; i++)
            {
                normal.Add(new PointF(-1 * (points[(i + 1) % points.Count].Y - points[i].Y), points[(i + 1) % points.Count].X - points[i].X));
            }
            for (int i = 0; i < normal.Count; i++)
            {
                //if (scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D) <= 1 && scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D) >= 0)
                //{
                if (scalar(D, normal[i]) < 0)
                {
                    t_H.Add(scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D));
                    if (scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D) >= 0 && scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D) <= 1&&scalar(normal[i], vec) != 0)
                    {
                        t_H2.Add(scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D));
                    }
                }
                if (scalar(D, normal[i]) >= 0)
                {
                    t_L.Add(scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D));
                    if (scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D) >= 0 && scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D) <= 1 && scalar(normal[i],vec)!=0)
                    {
                        t_L2.Add(scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D));
                    }
                    //}
                }
            }
            float t_Lower, t_Higher;
            PointF P_t2, P_t;
            if (t_L2.Count == 0)
            {
                P_t = new PointF(P0.X, P0.Y);
            }
            else
            {
                t_Lower = t_L2.Max();
                P_t = plus(P0, new PointF(t_Lower * D.X, t_Lower * D.Y));
            }
            if (t_H2.Count == 0)
            {
                P_t2 = new PointF(P1.X, P1.Y);
            }
            else
            {
                t_Higher = t_H2.Min();
                P_t2 = plus(P0, new PointF(t_Higher * D.X, t_Higher * D.Y));

            }
            for (int i = 0; i < points.Count; i++)
            {
                g.DrawLine(linePen, Convert(points[i]), Convert(points[(i + 1) % points.Count]));
            }

            g.DrawLine(linePen2, Convert(P_t), Convert(P_t2));
            for (int i = 0; i < t_L.Count; i++)
            {
                PointF tmp = Convert(plus(P0, new PointF(t_L[i] * D.X, t_L[i] * D.Y)));
                g.DrawEllipse(linePen2, tmp.X, tmp.Y, 10, 10);
            }
            for (int i = 0; i < t_H.Count; i++)
            {
                PointF tmp = Convert(plus(P0, new PointF(t_H[i] * D.X, t_H[i] * D.Y)));
                g.DrawEllipse(linePen2, tmp.X, tmp.Y, 10, 10);
            }
        }

        float scalar(PointF a, PointF b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
        PointF minus(PointF a, PointF b)
        {
            return new PointF(a.X - b.X, a.Y - b.Y);
        }
        PointF plus(PointF a, PointF b)
        {
            return new PointF(a.X + b.X, a.Y + b.Y);
        }
    }
}
