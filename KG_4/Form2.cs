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
    public partial class Form2 : Form
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
        float xmax, ymax, xmin, ymin;
        Pen pen = new Pen(Color.Black);
        Pen linePen = new Pen(Color.Red, 2);
        Pen linePen2 = new Pen(Color.Green, 2);
        Pen pointLine = new Pen(Color.Blue, 10);
        Pen Rec = new Pen(Color.Black);
        Brush Br = Brushes.Gray;
        List<Point> points = new List<Point>();
        List<Point> pic = new List<Point>();
        public Form2()
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
        List<float> coder(float xmax, float xmin, float ymax, float ymin, PointF a)
        {
            List<float> code = new List<float>() { 0, 0, 0, 0 };
            if (a.X >= xmin)
            {
                code[0] = 0;
            }
            else
            {
                code[0] = 1;
            }
            if (a.X <= xmax)
            {
                code[1] = 0;
            }
            else
            {
                code[1] = 1;
            }
            if (a.Y >= ymin)
            {
                code[2] = 0;
            }
            else
            {
                code[2] = 1;
            }
            if (a.Y <= ymax)
            {
                code[3] = 0;
            }
            else
            {
                code[3] = 1;
            }
            return code;
        }

        bool Check(List<float> code, List<float> code2)
        {
            for (int i = 0; i < code.Count; i++)
            {
                if (code[i] == code2[i] && code[i] == 1)
                    return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawAxis();
            Koen();
        }

        bool proverka(List<float> code, List<float> code2)
        {
            for (int i = 0; i < code.Count; i++)
            {
                if (code[i] != code2[i])
                    return true;

            }
            return false;
        }
        float X(float x)
        {
            return x_center + x * dx;
        }
        float Y(float y)
        {
            return y_center + y * -dy;
        }

        PointF Coor(List<float> code, PointF a, PointF b)
        {
            for (int i = 0; i < 4; i++)
            {
                if (code[i] == 1)
                {
                    if (i == 0)
                    {
                        return new PointF(xmin, a.Y + ((xmin - a.X) / (b.X - a.X)) * (b.Y - a.Y));

                    }
                    if (i == 1)
                    {
                        return new PointF(xmax, a.Y + ((xmax - a.X) /( b.X - a.X)) * (b.Y - a.Y));
                    }
                    if (i == 2)
                    {
                        return new PointF(a.X + ((ymin - a.Y) / (b.Y - a.Y)) * (b.X - a.X), ymin);
                    }
                    if (i == 3)
                    {
                        return new PointF(a.X + ((ymax - a.Y) / (b.Y - a.Y)) * (b.X - a.X), ymax);
                    }
                }
            }
            return new PointF();
        }
        void Koen()
        {
            List<PointF> points = new List<PointF>();
            points.Add(new PointF(0, 0));
            points.Add(new PointF(0, 4));
            points.Add(new PointF(4, 4));
            points.Add(new PointF(4, 0));
            PointF P1 = new PointF(10, 10);
            PointF P2 = new PointF(5, 0);
            xmax = points[0].X;
            ymax = points[0].Y;
            xmin = points[0].X;
            ymin = points[0].Y;
            for (int i = 0; i < 4; i++)
            {
                if (xmax < points[i].X)
                {
                    xmax = points[i].X;
                }
                if (ymax < points[i].Y)
                {
                    ymax = points[i].Y;
                }
                if (xmin > points[i].X)
                {
                    xmin = points[i].X;
                }
                if (ymin > points[i].Y)
                {
                    ymin = points[i].Y;
                }
            }
            List<float> P1_code = coder(xmax, xmin, ymax, ymin, P1);
            List<float> P2_code = coder(xmax, xmin, ymax, ymin, P2);
            List<PointF> new_point = new List<PointF>();
            bool flag = true;
            while (proverka(P1_code, P2_code))
            {
                if (Check(P1_code, P2_code))
                {
                    if (P1_code.Sum() != 0)
                    {
                        P1 = Coor(P1_code, P1, P2);
                        P1_code = coder(xmax, xmin, ymax, ymin, P1);
                        new_point.Add(P1);
                        continue;
                    }
                    if (P2_code.Sum() != 0)
                    {
                        P2 = Coor(P2_code, P1, P2);
                        P2_code = coder(xmax, xmin, ymax, ymin, P2);
                        new_point.Add(P2);
                        continue;

                    }
                }
                else
                {
                    flag = false;
                    break;
                }
            }
            for (int i = 0; i < new_point.Count; i++)
            {
                g.DrawEllipse(pointLine, X(new_point[i].X), Y(new_point[i].Y), 1, 1);
            }
            for (int i = 0; i < points.Count; i++)
            {
                g.DrawLine(linePen, Convert(points[i]), Convert(points[(i + 1) % points.Count]));
            }
            if (flag)
            {
                g.DrawLine(linePen, Convert(P1), Convert(P2));
            }
        }
    }
}
