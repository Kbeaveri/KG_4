﻿using System;
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
            PointF P0 = new Point(1, 1);
            PointF P1 = new Point(8, 8);
            PointF vec = new PointF(P1.X- P0.X, P1.Y- P0.Y);
            g.DrawLine(pen,Convert(P0),Convert(P1));
            PointF D = minus(P1, P0);
            List<PointF> points = new List<PointF>();
            points.Add(new PointF(2, 0));
            points.Add(new PointF(0, 2));
            points.Add(new PointF(0, 4));
            points.Add(new PointF(2, 6));
            points.Add(new PointF(4, 6));
            points.Add(new PointF(6, 4));
            points.Add(new PointF(6, 2));
            points.Add(new PointF(4, 0));

            List<float> t_all = new List<float>();
            List<PointF> normal = new List<PointF>();
            bool flag = true;
            for (int i = 0; i < points.Count; i++)
            {
                normal.Add(new PointF((points[(i + 1) % points.Count].Y - points[i].Y), -1*(points[(i + 1) % points.Count].X - points[i].X)));
            }
            float t0 = 0, t1 = 1;
            for (int i = 0;i < normal.Count; i++)
            {
                float tmp;
                PointF w = (minus(P0, points[i]));
                if (scalar(D, normal[i]) != 0)
                {
                    tmp = (-(scalar(w, normal[i]) / scalar(D, normal[i])));
                    if (scalar(D, normal[i]) > 0)
                    {
                        if (tmp <= 1)
                        {
                            t0 = Math.Max(t0, tmp);
                        }
                    }
                    else
                    {
                        if (tmp >= 0)
                        {
                            t1 = Math.Min(t1, tmp);
                        }
                    }
                    t_all.Add(tmp);
                }
                else
                {
                    if (scalar(w, normal[i]) < 0)
                    {
                        flag = false;
                    }
                }
            }
            PointF P_t0, P_t1;
            if (t0 <= t1 && flag == true)
            {
                P_t0 = plus(P0, new PointF(t0 * D.X, t0 * D.Y));
                P_t1 = plus(P0, new PointF(t1 * D.X, t1 * D.Y));
                g.DrawLine(linePen2, Convert(P_t0), Convert(P_t1));
            }
            for (int i = 0; i < points.Count; i++)
            {
                g.DrawLine(linePen, Convert(points[i]), Convert(points[(i + 1) % points.Count]));
            }

            for (int i = 0; i < t_all.Count; i++)
            {
                PointF tmp = Convert(plus(P0, new PointF(t_all[i] * D.X, t_all[i] * D.Y)));
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



//for (int i = 0; i < normal.Count; i++)
//{
//    //if (scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D) <= 1 && scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D) >= 0)
//    //{
//    if (scalar(D, normal[i]) < 0)
//    {
//        t_H.Add(scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D));
//        if (scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D) >= 0 && scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D) <= 1 && scalar(normal[i], vec) != 0)
//        {
//            t_H2.Add(scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D));
//        }
//    }
//    if (scalar(D, normal[i]) > 0)
//    {
//        t_L.Add(scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D));
//        if (scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D) >= 0 && scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D) <= 1 && scalar(normal[i], vec) != 0)
//        {
//            t_L2.Add(scalar(normal[i], minus(points[i], P0)) / scalar(normal[i], D));
//        }

//        //}
//    }
