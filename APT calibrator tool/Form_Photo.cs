using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
namespace APT_calibrator_tool
{
    public partial class Form_Photo : Form
    {
        int indexf = 1;
        Point point_MouseDown;
        Point point_MouseMove;
        Size t;
        bool leftFlag;
        Image im;
        Point localRe;
        public float k;
        public static Icon m_Cellicon;
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);
        public Cursor myCell()//自建十字光标
        {
            Bitmap tmp_Cursor = new Bitmap(4000, 4000);//光标大小一般为16*16个像素
            Graphics g = Graphics.FromImage(tmp_Cursor);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //SolidBrush tmp_Brush = new SolidBrush(Color.Black);//黑色边框
            //SolidBrush tmp_BrushM = new SolidBrush(Color.LightBlue);//亮蓝色底色
            //Rectangle tmp_Rect = new Rectangle(4, 0, 8, 16);//边框
            //Rectangle tmp_RectM = new Rectangle(0, 4, 16, 8);
            //Rectangle tmp_Rect2 = new Rectangle(5, 1, 6, 14);
            //Rectangle tmp_RectM2 = new Rectangle(1, 5, 14, 6);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            // g.FillRectangle(tmp_Brush, tmp_Rect);
            //g.FillRectangle(tmp_Brush, tmp_RectM);
            // g.FillRectangle(tmp_BrushM, tmp_Rect2);
            //g.FillRectangle(tmp_BrushM, tmp_RectM2);
            g.DrawLine(new Pen(Color.Red, 1), new Point(0, 2000), new Point(4000, 2000));
            g.DrawLine(new Pen(Color.Red, 1), new Point(2000, 0), new Point(2000, 4000));
            IntPtr Hicon = tmp_Cursor.GetHicon();
            //Cursor m_Cell = new Cursor(@"C:\Users\xinren\Desktop\程序设计\二元相图查询\二元相图查询\bitbug_favicon.ico");
            Icon m_Cellicon = Icon.FromHandle(Hicon);
            Cursor m_Cell = new Cursor(m_Cellicon.Handle);
            //Cursor m_Cell = new Cursor(tmp_Cursor.GetHicon());
            g.Dispose();
            tmp_Cursor.Dispose();

            return m_Cell;


        }

        public Form_Photo()
        {
            InitializeComponent();
            localRe = pictureBox1.Location;

        }


        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            //MessageBox.Show(k.ToString()); 
            Graphics g = e.Graphics;

            g.DrawImage(im, 0, 0, im.Width * k, im.Height * k);

            this.MouseWheel += new MouseEventHandler(this.panel1_MouseWheel);
        }

        public float k2;


        //缩小
        private void button2_Click(object sender, EventArgs e)
        {

            k -= k2 / 10;
            t.Height = (int)(k * im.Height);
            t.Width = (int)(k * im.Width);
            pictureBox1.Size = t;
            pictureBox1.Invalidate();
        }
        //放大
        private void button1_Click(object sender, EventArgs e)
        {
            k += k2 / 10;
            if (k < k2) k = k2;
            t.Height = (int)(k * im.Height);
            t.Width = (int)(k * im.Width);
            pictureBox1.Size = t;
            pictureBox1.Invalidate();
        }
        private void button3_Click(object sender, EventArgs e)
        {

            k = k2;
            t.Height = (int)(k * im.Height);
            t.Width = (int)(k * im.Width);
            pictureBox1.Size = t;
            pictureBox1.Location = localRe;
            pictureBox1.Invalidate();

        }
        private void Form_Photo_Load(object sender, EventArgs e)
        {
            im = Properties.Resources.PER_iso; 
            t = im.Size;
            //if (Form1.filedirOpen[1].Contains(".jpg"))
            //{
                k = 0.18f;//计算比例
                k2 = k;
            //}
            //else
            //{
            //    k = (1076 / (float)t.Height);//计算比例
             //   k2 = k;
            //}

            if (Properties.Settings.Default.Language.Equals("EN"))
            {
                toolStripButton1.Text = "Reduce";
                toolStripButton2.Text = "Enlarge";
                toolStripButton3.Text = "Print";
                this.Text = "Evaporation field";
                button1.Text = "Enlarge";
                button2.Text = "Reduce";
                button3.Text = "Initialize";
                button4.Text = "Cross";
                button5.Text = "Next";
                button6.Text = "Previous";
            }
            if (Properties.Settings.Default.Language.Equals("CH"))
            {
                toolStripButton1.Text = "缩小";
                toolStripButton2.Text = "放大";
                toolStripButton3.Text = "打印";
                this.Text = "相图";
                button1.Text = "放大";
                button2.Text = "缩小";
                button3.Text = "还原";
                button4.Text = "十字线";
                button5.Text = "下一张";
                button6.Text = "上一张";
            }

        }
        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {

            //获取鼠标轮已转动的制动器数的有符号计数乘以WHEEL_DELTA。制动器是鼠标轮的一个凹口
            //除以4降低图片缩放灵敏度
            k += e.Delta * (k2 / 100000);
            if (k < k2) k = k2;
            t.Height = (int)(k * im.Height);
            t.Width = (int)(k * im.Width);
            pictureBox1.Size = t;
            pictureBox1.Invalidate();
            //获取鼠标轮已转动的制动器数的有符号计数乘以WHEEL_DELTA。制动器是鼠标轮的一个凹口
            //除以4降低图片缩放灵敏度

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//左击
            {
                //左击时鼠标坐标，相对于窗口
                point_MouseDown = new Point(e.X, e.Y);
                //将左击时的鼠标坐标(相对于窗口)，变换成相对于屏幕坐标
                point_MouseDown = PointToScreen(point_MouseDown);
                leftFlag = true;
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                //鼠标移动的坐标，相对于屏幕
                point_MouseMove = Control.MousePosition;
                point_MouseMove.Offset(-point_MouseDown.X, -point_MouseDown.Y);
                pictureBox1.Location = point_MouseMove;

            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;
            }
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Graphics myGraphics = pictureBox1.CreateGraphics();
            //通过调用Graphics对象的DrawLine方法实现鼠标十字定位功能
            myGraphics.DrawLine(new Pen(Color.Red, 1), new Point(e.X, 0), new Point(e.X, e.Y));
            myGraphics.DrawLine(new Pen(Color.Red, 1), new Point(e.X, e.Y), new Point(e.X, im.Height - e.Y));
            myGraphics.DrawLine(new Pen(Color.Red, 1), new Point(0, e.Y), new Point(e.X, e.Y));
            myGraphics.DrawLine(new Pen(Color.Red, 1), new Point(e.X, e.Y), new Point(im.Width - e.X, e.Y));


            //// string filePath = @"face.bmp";
            //         Bitmap bmp = new Bitmap(4000, 4000);
            //         Graphics gs = Graphics.FromImage(bmp);
            //        Metafile mf = new Metafile(filePath, gs.GetHdc());
            //       Graphics g = Graphics.FromImage(mf);
            //         g.SmoothingMode = SmoothingMode.AntiAlias;

            //         g.DrawLine(new Pen(Color.Red, 1), new Point(0, 2000), new Point(4000, 2000));
            //        // g.DrawLine(new Pen(Color.Red, 1), new Point(2000, 0), new Point(2000, 4000));
            //         //g.Save();
            //        g.Dispose();
            //         mf.Dispose();
        }
        //bool b4Status = true;//标记按钮4当前状态
        private void button4_Click(object sender, EventArgs e)
        {
            //b4Status = !b4Status;//状态切换
            //if (b4Status == false)
            //{
            this.pictureBox1.Cursor = myCell();
            //button4.Text = "取消十字线";
            //DestroyIcon(m_Cellicon.Handle);

            //}
            //else
            //做你想做的事情，比如扫面文件
            //{

            //button4.Text = "十字线";
            //做你想做的事情，比如暂停文件按扫描
            //this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Default;
            //}
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            k -= k2 / 10;
            t.Height = (int)(k * im.Height);
            t.Width = (int)(k * im.Width);
            pictureBox1.Size = t;
            pictureBox1.Invalidate();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            k += k2 / 10;
            if (k < 0.1f) k = k2;
            t.Height = (int)(k * im.Height);
            t.Width = (int)(k * im.Width);
            pictureBox1.Size = t;
            pictureBox1.Invalidate();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            System.Drawing.Printing.PrintDocument myPrintDocument1 = new System.Drawing.Printing.PrintDocument();
            PrintDialog myPrinDialog1 = new PrintDialog();
            myPrintDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);
            myPrinDialog1.Document = myPrintDocument1;
            if (myPrinDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    myPrintDocument1.Print();
                }
                catch
                {   //停止打印
                    myPrintDocument1.PrintController.OnEndPrint(printDocument1, new System.Drawing.Printing.PrintEventArgs());
                }
            }
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap myBitmap1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(myBitmap1, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            e.Graphics.DrawImage(myBitmap1, 0, 0);
            myBitmap1.Dispose();
            /*
            //e.Graphics.DrawImage(im, e.MarginBounds.X, e.MarginBounds.Y, pictureBox1.Image.Width, pictureBox1.Image.Height);
            string text = DateTime.Now.ToString();
            System.Drawing.Font printFont = new System.Drawing.Font
                ("Arial", 14, System.Drawing.FontStyle.Regular);

            // Draw the content.
            e.Graphics.DrawString(text, printFont,
                System.Drawing.Brushes.Black, 10, 10);
             */
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }


    }
}