using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KGV
{
    public partial class Form1 : Form
    {
        struct Complex
        {
            public double x;
            public double y;

        };

        const int iter = 50;
        const double min = 1e-6;
        const double max = 1e+6;

        private int Level = 0;
        //Высота и ширина для отрисовки
        private int width;
        private int height;
        // Bitmap для фрактала
        private Bitmap fractal;
        // используем для отрисовки на PictureBox
        private Graphics graph;

        public Form1()
        {
            InitializeComponent();
            //инициализируем ширину и высоту
            width = pictureBox1.Width;
            height = pictureBox1.Height;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //создаем Bitmap для прямоугольника
            fractal = new Bitmap(width, height);
            // cоздаем новый объект Graphics из указанного Bitmap
            graph = Graphics.FromImage(fractal);
            //создаем прямоугольник и вызываем функцию отрисовки ковра
            RectangleF carpet = new RectangleF(0, 0, width, height);
            DrawCarpet(Level, carpet);
            //отображаем результат
            pictureBox1.BackgroundImage = fractal;
        }
        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
            Level = Convert.ToInt32(numericUpDown1.Value);

            fractal = new Bitmap(width, height);
            graph = Graphics.FromImage(fractal);
            RectangleF carpet = new RectangleF(0, 0, width, height);
            DrawCarpet(Level, carpet);
            pictureBox1.BackgroundImage = fractal;
        }

        private void DrawCarpet(int level, RectangleF carpet)
        {
            //проверяем, закончили ли мы построение
            if (level == 0)
            {
                //Рисуем прямоугольник
                graph.FillRectangle(Brushes.MediumPurple, carpet);
            }
            else
            {
                // делим прямоугольник на 9 частей
                var width = carpet.Width / 3f;
                var height = carpet.Height / 3f;
                // (x1, y1) - координаты левой верхней вершины прямоугольника
                // от нее будем отсчитывать остальные вершины маленьких прямоугольников
                var x1 = carpet.Left;
                var x2 = x1 + width;
                var x3 = x1 + 2f * width;

                var y1 = carpet.Top;
                var y2 = y1 + height;
                var y3 = y1 + 2f * height;

                DrawCarpet(level - 1, new RectangleF(x1, y1, width, height)); // левый 1(верхний)
                DrawCarpet(level - 1, new RectangleF(x2, y1, width, height)); // средний 1
                DrawCarpet(level - 1, new RectangleF(x3, y1, width, height)); // правый 1
                DrawCarpet(level - 1, new RectangleF(x1, y2, width, height)); // левый 2
                DrawCarpet(level - 1, new RectangleF(x3, y2, width, height)); // правый 2
                DrawCarpet(level - 1, new RectangleF(x1, y3, width, height)); // левый 3
                DrawCarpet(level - 1, new RectangleF(x2, y3, width, height)); // средний 3
                DrawCarpet(level - 1, new RectangleF(x3, y3, width, height)); // правый 3
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Создаем перо цвета - Черный(Black)
            //Толщина - 1 пиксель:
            Pen myPen = new Pen(Color.Black, 1);
            //Объявляем объект "g" класса Graphics и предоставляем
            //ему возможность рисования на pictureBox1:
            Graphics g = Graphics.FromHwnd(pictureBox1.Handle);
            //Вызов функции отрисовки фрактала
            //int mx =1026, my = 428;
            int mx = pictureBox1.Width, my = pictureBox1.Height;

            DrawPool(mx, my, g, myPen);
        }

        public void DrawPool(int mx1, int my1, Graphics g, Pen pen)
        {
            int n, mx, my;
            double p;
            Complex z, t, d = new Complex();

            mx = mx1 / 2;
            my = my1 / 2;

            for (int y = -my; y < my; y++)
                for (int x = -mx; x < mx; x++)
                {
                    n = 0;
                    z.x = x * 0.005;
                    z.y = y * 0.005;
                    d = z;

                    while ((Math.Pow(z.x, 2) + Math.Pow(z.y, 2) < max) && (Math.Pow(d.x, 2) + Math.Pow(d.y, 2) > min) && (n < iter))
                    {
                        t = z;
                        p = Math.Pow(Math.Pow(t.x, 2) + Math.Pow(t.y, 2), 2);
                        z.x = 2 / 3 * t.x + (Math.Pow(t.x, 2) - Math.Pow(t.y, 2)) / (3 * p);
                        z.y = 2 / 3 * t.y * (1 - t.x / p);
                        d.x = Math.Abs(t.x - z.x);
                        d.y = Math.Abs(t.y - z.y);
                        n++;
                    }
                    pen.Color = Color.FromArgb(255, (n * 9) % 255, 0, (n * 9) % 255);
                    g.DrawRectangle(pen, mx + x, my + y, 1, 1);

                }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 0;
            pictureBox1.BackgroundImage = null;
            pictureBox1.Image = null;
            pictureBox1.Invalidate();
        }
    }
}
