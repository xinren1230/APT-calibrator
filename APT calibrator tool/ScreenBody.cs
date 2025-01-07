using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APT_calibrator_tool
{

    public partial class ScreenBody : Form
    {
        private Graphics MainPainter;  //主画笔
        private Pen pen;               //就是笔咯
        private bool isDowned;         //判断鼠标是否按下
        private bool RectReady;         //矩形是否绘制完成
        private Image baseImage;       //基本图形(原来的画面)
        private Rectangle Rect;        //就是要保存的矩形
        private Point downPoint;        //鼠标按下的点
        int tmpx;
        int tmpy;
        private int flag = 0;
        int pointax;
        int pointbx;
        int pointay;
        int pointby;
        double x;
        public ScreenBody()
        {
            InitializeComponent();
        }

        private void ScreenBody_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void ScreenBody_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDowned = false;
                RectReady = true;
            }
        }

        private void ScreenBody_MouseMove(object sender, MouseEventArgs e)
        {

            if (RectReady == false)
            {
                if (isDowned == true)
                {
                    Image New = DrawScreen((Image)baseImage.Clone(), e.X, e.Y);
                    MainPainter.DrawImage(New, 0, 0);
                    New.Dispose();
                }
            }
            if (RectReady == true)
            {
                if (Rect.Contains(e.X, e.Y))
                {
                    //this.Cursor = Cursors.Hand;
                    if (isDowned == true)
                    {
                        //和上一次的位置比较获取偏移量
                        Rect.X = Rect.X + e.X - tmpx;
                        Rect.Y = Rect.Y + e.Y - tmpy;
                        //记录现在的位置
                        tmpx = e.X;
                        tmpy = e.Y;
                        MoveRect((Image)baseImage.Clone(), Rect);
                    }
                }
            }

        }

        private void ScreenBody_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            MainPainter = this.CreateGraphics();
            pen = new Pen(Brushes.Blue);
            isDowned = false;
            baseImage = this.BackgroundImage;
            Rect = new Rectangle();
            RectReady = false;
        }
        private void DrawRect(Graphics Painter, int Mouse_x, int Mouse_y)
        {
            int width = 0;
            int heigth = 0;
            if (Mouse_y < Rect.Y)
            {
                Rect.Y = Mouse_y;
                heigth = downPoint.Y - Mouse_y;
            }
            else
            {
                heigth = Mouse_y - downPoint.Y;
            }
            if (Mouse_x < Rect.X)
            {
                Rect.X = Mouse_x;
                width = downPoint.X - Mouse_x;
            }
            else
            {
                width = Mouse_x - downPoint.X;
            }
            Rect.Size = new Size(width, heigth);
            Painter.DrawRectangle(pen, Rect);
        }

        private Image DrawScreen(Image back, int Mouse_x, int Mouse_y)
        {
            Graphics Painter = Graphics.FromImage(back);
            DrawRect(Painter, Mouse_x, Mouse_y);
            return back;
        }
        private void MoveRect(Image image, Rectangle Rect)
        {
            Graphics Painter = Graphics.FromImage(image);
            Painter.DrawRectangle(pen, Rect.X, Rect.Y, Rect.Width, Rect.Height);
            DrawRect(Painter, 0, 0);
            MainPainter.DrawImage(image, 0, 0);
            image.Dispose();
        }

        private void ScreenBody_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void ScreenBody_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Escape))
            {
                this.Close();
            }
        }


    }
}
