using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using SharpGL;
using SharpGL.WinForms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using OxyPlot.Axes;
using MathNet;
using OxyPlot.Annotations;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Markup;
using System.Xaml;
using System.Runtime.InteropServices;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Numerics;


namespace APT_calibrator_tool
{


    public partial class Pole_Simulator : Form
    {
        private readonly RisCaptureLib.ScreenCaputre screenCaputre = new RisCaptureLib.ScreenCaputre();
        private System.Windows.Size? lastSize;

        System.Drawing.Image image;
        Thread playWav;
        int imageNum = 1;
        bool isFirstTimeShow = true;
        byte[] bmpdata = null;
        string imgpath;
        //Create Plotview object
        PlotView myPlot;
        PlotModel model;

        public Pole_Simulator()
        {
            InitializeComponent();

            screenCaputre.ScreenCaputred += OnScreenCaputred;
            screenCaputre.ScreenCaputreCancelled += OnScreenCaputreCancelled;
            screenCaputre.ScreenCaputred += panel1_Paint;

        }

        private void OnScreenCaputreCancelled(object sender, System.EventArgs e)
        {
            this.Show();
            Focus();
        }

        private void OnScreenCaputred(object sender, RisCaptureLib.ScreenCaputredEventArgs e)
        {
            //pictureBox2.BackgroundImageLayout = ImageLayout.Zoom;
            //if (e.Bmp.Width < pictureBox2.Width && e.Bmp.Height < pictureBox2.Height)
            // pictureBox2.BackgroundImageLayout = ImageLayout.Center;
            //pictureBox2.BackgroundImage = NewGetBitmap(e.Bmp);
            //var bmp = newpoleImage;

            Bitmap bmp = BitmapFromSource(e.Bmp);
            bmp = ResizeImage(bmp, 310, 314);
            BitmapSource bmpsource = imagework.getBitMapSourceFromBitmap(bmp);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmpsource));

            // image to byte[]
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                bmpdata = ms.ToArray();
            }
            //*

            this.Show();
        }


        /// <summary>
        /// </summary>
        private uint _model = OpenGL.GL_QUADS;
        private uint _model_2 = OpenGL.GL_POLYGON;
        private uint _model_3 = OpenGL.GL_POLYGON;
        /// <summary>
        /// </summary>
        private float _x = 0;

        /// <summary>
        /// </summary>
        private float _y = 0;

        /// <summary>
        ///
        /// </summary>
        private float _z = 0;
        private object chartControl;

        private void openGLControl2_OpenGLDraw(object sender, RenderEventArgs args)
        {
            if (true)
            {
                SharpGL.OpenGL gl = this.openGLControl2.OpenGL;
                double[] m = new double[16];
                double phi1, theta, phi2;
                gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);	//
                gl.ClearColor(1.0f, 1.0f, 1.0f, 0.0f);
                gl.LoadIdentity();                  // 
                gl.Translate(0.0f, 0.0f, -10.0f);	// 
                //gl.Rotate(_x, 1.0f, 0.0f, 0.0f);	// 
                //gl.Rotate(_y, 0.0f, 1.0f, 0.0f);	// 
                //gl.Rotate(_z, 0.0f, 0.0f, 1.0f);    // 
                // Attitude matix of current orientation
                gl.Scale(1.0f, 1.0f, 0.01f);//0.01f can inhibit deformation by persective, 0.0f will be weird

                //gl.Ortho2D(0.0f, 0.0f, 0.0f, 0.0f);
                // Euler  angle to transform martrix https://community.khronos.org/t/how-implement-euler-angle-in-opengl/60396

                phi1 = _x / 57.28835; //Euler angle phi1
                theta = _y / 57.28835; //Euler angle theta
                phi2 = _z / 57.28835; //Euler angle phi2


                // Define Euler angles in the z-x-z convention
                //    double phi = _x / 57.28835; // First rotation about z-axis
                //   double theta = _y / 57.28835; // Rotation about x-axis
                //   double psi = _z / 57.28835; // Second rotation about z-axis

                // Calculate the corresponding Tait-Bryan angles
                // phi1 = Math.Atan2(Math.Sin(psi) * Math.Sin(phi) - Math.Cos(psi) * Math.Sin(theta) * Math.Cos(phi), Math.Cos(psi) * Math.Cos(theta)); // First rotation about x-axis
                // theta = Math.Asin(Math.Cos(psi) * Math.Sin(theta) * Math.Sin(phi) + Math.Sin(psi) * Math.Cos(phi)); // Rotation about y-axis
                //   phi2 = Math.Atan2(Math.Sin(psi) * Math.Cos(phi) + Math.Cos(psi) * Math.Sin(theta) * Math.Sin(phi), Math.Cos(psi) * Math.Cos(theta)); // Second rotation about z-axis

                //equation from edsb book(with transfer T转置后的，pls note m[0-4]is one colume (vetical)) 
                //m[0] = Math.Cos(phi1) * Math.Cos(phi2)- Math.Sin(phi1) * Math.Sin(phi2) * Math.Cos(theta); // CA*CE;
                //m[1] = Math.Sin(phi1) * Math.Cos(phi2)+Math.Cos(phi1) * Math.Sin(phi2) * Math.Cos(theta); // CA*SE*SR - SA*CR
                //m[2] = Math.Sin(phi2) * Math.Sin(theta); // CA*SE*CR + SA*SR
                //m[3] = 0;

                // m[4] = -Math.Cos(phi1) * Math.Sin(phi2) - Math.Sin(phi1) * Math.Cos(phi2) * Math.Cos(theta); ; // SA*CE
                // m[5] =- Math.Sin(phi1) * Math.Sin(phi2) + Math.Cos(phi1) * Math.Cos(theta) * Math.Cos(phi2); // CA*CR + SA*SE*SR
                //  m[6] = Math.Cos(phi2) * Math.Sin(theta); // SA*SE*CR – CA*SR
                //  m[7] = 0;

                //  m[8] =  Math.Sin(phi1) * Math.Sin(theta); // -SE
                //  m[9] = -Math.Cos(phi1) * Math.Sin(theta); // CE*SR
                //  m[10] = Math.Cos(theta); // CE*CR
                //  m[11] = 0;

                //  m[12] = 0;
                //  m[13] = 0;
                //  m[14] = 0;
                //  m[15] = 1;


                //rotate 90 in Z of above (the result is below) , use below, it is same with OIM
                m[0] = -Math.Sin(phi1) * Math.Cos(phi2) - Math.Cos(phi1) * Math.Sin(phi2) * Math.Cos(theta);
                m[1] = Math.Cos(phi1) * Math.Cos(phi2) - Math.Sin(phi1) * Math.Sin(phi2) * Math.Cos(theta);
                m[2] = -Math.Sin(phi2) * Math.Sin(theta);
                m[3] = 0;

                m[4] = Math.Sin(phi1) * Math.Sin(phi2) - Math.Cos(phi1) * Math.Cos(theta) * Math.Cos(phi2);
                m[5] = -Math.Cos(phi1) * Math.Sin(phi2) - Math.Sin(phi1) * Math.Cos(phi2) * Math.Cos(theta);
                m[6] = -Math.Cos(phi2) * Math.Sin(theta);
                m[7] = 0;

                m[8] = -Math.Cos(phi1) * Math.Sin(theta);
                m[9] = -Math.Sin(phi1) * Math.Sin(theta);
                m[10] = Math.Cos(theta);
                m[11] = 0;

                m[12] = 0;
                m[13] = 0;
                m[14] = 0;
                m[15] = 1;



                gl.MultMatrix(m);

                // glutWireCube(1.0);比如用库函数画一个四面体线框：
                //gl.Rotate(90, 0.0f, 0.0f, 0.0f);	//  Top view
                //gl.Rotate(90, 0.0f, 1.0f, 0.0f);	// Top view
                //gl.Rotate(_z, 0.0f, 0.0f, 1.0f);    // 
                //https://www.cnblogs.com/DOMLX/p/11783026.html
                // gl.Scale(0.03, 0.03, 0.03);
                //myReadObj obj = new myReadObj();
                //obj.loadFile("j20.obj");
                //obj.createListFace(ref gl);
                //gl.CallList(obj.showFaceList);

                if (comboBox_cstr.Text == "HCP")
                {
                    if (textBox_HCPc.Text != "")
                    {
                        double hcpac = Convert.ToDouble(textBox_HCPc.Text);
                        double sideLength = 2.0; // Side length of the hexagon base
                        double halfHeight = 1 * hcpac; // Half height of the hexagonal prism
                        double sqrt3over2 = Math.Sqrt(3);
                        if (_model == OpenGL.GL_LINES)
                        {
                            gl.Begin(_model);                   // 
                            gl.Color(0.0f, 0.0f, 0.0f);         //

                            gl.Vertex(2.00, 0.00, -halfHeight);
                            gl.Vertex(1, sqrt3over2, -halfHeight);
                            gl.Vertex(1, sqrt3over2, -halfHeight);
                            gl.Vertex(-1, sqrt3over2, -halfHeight);
                            gl.Vertex(-1, sqrt3over2, -halfHeight);
                            gl.Vertex(-2.00, 0.00, -halfHeight);
                            gl.Vertex(-2.00, 0.00, -halfHeight);
                            gl.Vertex(-1, -sqrt3over2, -halfHeight);
                            gl.Vertex(-1, -sqrt3over2, -halfHeight);
                            gl.Vertex(1, -sqrt3over2, -halfHeight);
                            gl.Vertex(1, -sqrt3over2, -halfHeight);
                            gl.Vertex(2.00, 0.00, -halfHeight);

                            gl.Vertex(1, sqrt3over2, halfHeight);
                            gl.Vertex(1, sqrt3over2, -halfHeight);
                            gl.Vertex(-1, sqrt3over2, halfHeight);
                            gl.Vertex(-1, sqrt3over2, -halfHeight);
                            gl.Vertex(-2.00, 0.00, halfHeight);
                            gl.Vertex(-2.00, 0.00, -halfHeight);
                            gl.Vertex(-1, -sqrt3over2, halfHeight);
                            gl.Vertex(-1, -sqrt3over2, -halfHeight);
                            gl.Vertex(1, -sqrt3over2, halfHeight);
                            gl.Vertex(1, -sqrt3over2, -halfHeight);
                            gl.Vertex(2.00, 0.00, halfHeight);
                            gl.Vertex(2.00, 0.00, -halfHeight);


                            gl.Vertex(2.00, 0.00, halfHeight);
                            gl.Vertex(1, sqrt3over2, halfHeight);
                            gl.Vertex(1, sqrt3over2, halfHeight);
                            gl.Vertex(-1, sqrt3over2, halfHeight);
                            gl.Vertex(-1, sqrt3over2, halfHeight);
                            gl.Vertex(-2.00, 0.00, halfHeight);
                            gl.Vertex(-2.00, 0.00, halfHeight);
                            gl.Vertex(-1, -sqrt3over2, halfHeight);
                            gl.Vertex(-1, -sqrt3over2, halfHeight);
                            gl.Vertex(1, -sqrt3over2, halfHeight);
                            gl.Vertex(1, -sqrt3over2, halfHeight);
                            gl.Vertex(2.00, 0.00, halfHeight);

                            gl.End();
                        }
                        else if (_model == OpenGL.GL_QUADS)

                        {
                            gl.Begin(_model);                   // 
                            gl.Color(1.0f, 0.5f, 0.0f);
                            gl.Vertex(1, sqrt3over2, halfHeight);
                            gl.Vertex(1, sqrt3over2, -halfHeight);
                            gl.Vertex(-1, sqrt3over2, -halfHeight);
                            gl.Vertex(-1, sqrt3over2, halfHeight);

                            gl.Color(1.0f, 0.0f, 0.0f);
                            gl.Vertex(-1, sqrt3over2, -halfHeight);
                            gl.Vertex(-1, sqrt3over2, halfHeight);
                            gl.Vertex(-2.00, 0.00, halfHeight);
                            gl.Vertex(-2.00, 0.00, -halfHeight);

                            gl.Color(1.0f, 1.0f, 0.0f);
                            gl.Vertex(-2.00, 0.00, halfHeight);
                            gl.Vertex(-2.00, 0.00, -halfHeight);
                            gl.Vertex(-1, -sqrt3over2, -halfHeight);
                            gl.Vertex(-1, -sqrt3over2, halfHeight);

                            gl.Color(0.0f, 0.0f, 1.0f);
                            gl.Vertex(-1, -sqrt3over2, -halfHeight);
                            gl.Vertex(-1, -sqrt3over2, halfHeight);
                            gl.Vertex(1, -sqrt3over2, halfHeight);
                            gl.Vertex(1, -sqrt3over2, -halfHeight);

                            gl.Color(1.0f, 0.0f, 1.0f);
                            gl.Vertex(1, -sqrt3over2, halfHeight);
                            gl.Vertex(1, -sqrt3over2, -halfHeight);
                            gl.Vertex(2.00, 0.00, -halfHeight);
                            gl.Vertex(2.00, 0.00, halfHeight);

                            gl.Color(0.0f, 0.5f, 1.0f);
                            gl.Vertex(2.00, 0.00, -halfHeight);
                            gl.Vertex(2.00, 0.00, halfHeight);
                            gl.Vertex(1, sqrt3over2, halfHeight);
                            gl.Vertex(1, sqrt3over2, -halfHeight);

                            gl.End();

                            _model_2 = OpenGL.GL_POLYGON;
                            gl.Begin(_model_2);

                            gl.Color(0.0f, 1.0f, 0.0f);         //
                            gl.Vertex(2.00, 0.00, -halfHeight);
                            gl.Vertex(1, sqrt3over2, -halfHeight);
                            gl.Vertex(-1, sqrt3over2, -halfHeight);
                            gl.Vertex(-2.00, 0.00, -halfHeight);
                            gl.Vertex(-1, -sqrt3over2, -halfHeight);
                            gl.Vertex(1, -sqrt3over2, -halfHeight);
                            gl.End();


                            _model_3 = OpenGL.GL_POLYGON;
                            gl.Begin(_model_3);

                            gl.Color(0.0f, 1.0f, 0.5f);         //
                            gl.Vertex(2.00, 0.00, halfHeight);
                            gl.Vertex(1, sqrt3over2, halfHeight);
                            gl.Vertex(-1, sqrt3over2, halfHeight);
                            gl.Vertex(-2.00, 0.00, halfHeight);
                            gl.Vertex(-1, -sqrt3over2, halfHeight);
                            gl.Vertex(1, -sqrt3over2, halfHeight);

                            gl.End();
                        }
                    }
                }
                else
                {
                    if (_model == OpenGL.GL_QUADS)
                    {
                        gl.Begin(_model);                   // 
                        gl.Color(0.0f, 1.0f, 0.0f);         //
                        gl.Vertex(2.0f, 2.0f, -2.0f);
                        gl.Vertex(-2.0f, 2.0f, -2.0f);
                        gl.Vertex(-2.0f, 2.0f, 2.0f);
                        gl.Vertex(2.0f, 2.0f, 2.0f);
                        //
                        gl.Color(1.0f, 0.5f, 0.0f);
                        gl.Vertex(2.0f, -2.0f, 2.0f);
                        gl.Vertex(-2.0f, -2.0f, 2.0f);
                        gl.Vertex(-2.0f, -2.0f, -2.0f);
                        gl.Vertex(2.0f, -2.0f, -2.0f);

                        gl.Color(1.0f, 0.0f, 0.0f);
                        gl.Vertex(2.0f, 2.0f, 2.0f);
                        gl.Vertex(-2.0f, 2.0f, 2.0f);
                        gl.Vertex(-2.0f, -2.0f, 2.0f);
                        gl.Vertex(2.0f, -2.0f, 2.0f);

                        gl.Color(1.0f, 1.0f, 0.0f);
                        gl.Vertex(2.0f, -2.0f, -2.0f);
                        gl.Vertex(-2.0f, -2.0f, -2.0f);
                        gl.Vertex(-2.0f, 2.0f, -2.0f);
                        gl.Vertex(2.0f, 2.0f, -2.0f);

                        gl.Color(0.0f, 0.0f, 1.0f);
                        gl.Vertex(-2.0f, 2.0f, 2.0f);
                        gl.Vertex(-2.0f, 2.0f, -2.0f);
                        gl.Vertex(-2.0f, -2.0f, -2.0f);
                        gl.Vertex(-2.0f, -2.0f, 2.0f);

                        gl.Color(1.0f, 0.0f, 1.0f);
                        gl.Vertex(2.0f, 2.0f, -2.0f);
                        gl.Vertex(2.0f, 2.0f, 2.0f);
                        gl.Vertex(2.0f, -2.0f, 2.0f);
                        gl.Vertex(2.0f, -2.0f, -2.0f);
                        gl.End();
                    }
                    else if (_model == OpenGL.GL_LINES)
                    {
                        gl.Begin(_model);                   // 
                        gl.Color(0.0f, 0.0f, 0.0f);         //
                        gl.Vertex(2.0f, 2.0f, -2.0f);
                        gl.Vertex(-2.0f, 2.0f, -2.0f);
                        gl.Vertex(-2.0f, 2.0f, -2.0f);
                        gl.Vertex(-2.0f, 2.0f, 2.0f);
                        gl.Vertex(-2.0f, 2.0f, 2.0f);
                        gl.Vertex(2.0f, 2.0f, 2.0f);
                        gl.Vertex(2.0f, 2.0f, 2.0f);
                        gl.Vertex(2.0f, 2.0f, -2.0f);

                        gl.Vertex(2.0f, -2.0f, 2.0f);
                        gl.Vertex(-2.0f, -2.0f, 2.0f);
                        gl.Vertex(-2.0f, -2.0f, 2.0f);
                        gl.Vertex(-2.0f, -2.0f, -2.0f);
                        gl.Vertex(-2.0f, -2.0f, -2.0f);
                        gl.Vertex(2.0f, -2.0f, -2.0f);
                        gl.Vertex(2.0f, -2.0f, -2.0f);
                        gl.Vertex(2.0f, -2.0f, 2.0f);

                        gl.Vertex(2.0f, 2.0f, 2.0f);
                        gl.Vertex(2.0f, -2.0f, 2.0f);

                        gl.Vertex(-2.0f, 2.0f, 2.0f);
                        gl.Vertex(-2.0f, -2.0f, 2.0f);
                        gl.Vertex(2.0f, 2.0f, -2.0f);
                        gl.Vertex(2.0f, -2.0f, -2.0f);
                        gl.Vertex(-2.0f, 2.0f, -2.0f);
                        gl.Vertex(-2.0f, -2.0f, -2.0f);

                        gl.End();


                    }

                }

            }
        }






        private void rbfull_CheckedChanged(object sender, EventArgs e)
        {
            _model = OpenGL.GL_QUADS;
            _model_2 = OpenGL.GL_POLYGON;
            _model_3 = OpenGL.GL_POLYGON;
        }

        private void rbline_CheckedChanged(object sender, EventArgs e)
        {
            _model = OpenGL.GL_LINES;
        }

        private void tbX_Scroll(object sender, EventArgs e)
        {
            float x = trackBarx.Value;
            _x = x;
            x_euler.Text = x.ToString();
        }

        private void tbY_Scroll(object sender, EventArgs e)
        {
            float y = trackBary.Value;
            _y = y;
            y_euler.Text = y.ToString();
        }

        private void tbZ_Scroll(object sender, EventArgs e)
        {
            float z = trackBarz.Value;
            _z = z;
            z_euler.Text = z.ToString();
        }

        private void Pole_Simulator_Load(object sender, EventArgs e)
        {

        }

        private void Euler_Btn_Click(object sender, EventArgs e)
        {
            try
            {

                float x = Convert.ToSingle(x_euler.Text);
                _x = x;
                float z = Convert.ToSingle(z_euler.Text);
                _z = z;
                float y = Convert.ToSingle(y_euler.Text);
                _y = y;
                //if (x<=90 && y<=90 && z<=90)
                if (1 == 1)
                {
                    trackBarx.Value = (int)x;
                    trackBarz.Value = (int)z;
                    trackBary.Value = (int)y;
                }
                else
                {
                    MessageBox.Show("Angle<=90°");
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Pls check all parameters are inputted");
            }
        }


        private void panel1_Paint(object sender, EventArgs e)
        {
            try
            {


                // *

                //double theta1=pi/6;
                //double theta2=pi/6;
                //double theta3=pi/6;


                myPlot = new PlotView();
                model = new PlotModel { Title = "", Background = OxyColors.White };

                var point_color = OxyColors.DarkRed;
                if (radioButton_white.Checked == true) { point_color = OxyColors.White; }

                var scatterSeries = new ScatterSeries
                {
                    Title = "[Poles]",
                    MarkerFill = point_color,
                    MarkerType = MarkerType.Circle
                };



                double radius = 18.5;
                double angle_offset = 0;
                double mirror_index = 1;
                double mirrory_index = 1;
                label31.Visible = true;
                label32.Visible = true;
                label33.Visible = true;
                label36.Visible = true;
                label37.Visible = true;
                // * Reflectron axis
                if (checkBox_REF.Checked == false)
                { radius = 35.15; }
                if (checkBox_5076.Checked == true)
                {
                    checkBox_5096.Checked = false; angle_offset = -45;
                    mirrory_index = -1;
                    mirror_index = -1;
                    label31.Visible = false;
                    label32.Visible = false;
                    label33.Visible = false;
                    label36.Visible = false;
                    label37.Visible = false;

                    var lineseries_laser76 = new LineSeries { };
                    lineseries_laser76.Points.Add(new DataPoint(-35, -35));//添加线的第一个点坐标   //x,y exchange and -x to make the coordinate same with APTsuite OIM
                    lineseries_laser76.Points.Add(new DataPoint(-30, -30));//添加线的第二个点的坐标//x,y exchange and -x to make the coordinate same with APTsuite OIM
                    model.Series.Add(lineseries_laser76);//将线添加到图标的容器中
                }

                if (checkBox_5096.Checked == true)
                {
                    checkBox_5076.Checked = false;
                    label31.Visible = false;
                    label32.Visible = false;
                    label33.Visible = false;
                    label36.Visible = false;
                    label37.Visible = false;
                    mirror_index = -1;
                    mirrory_index = -1;
                    //angle_offset = 15; // offset angle of Z rotation based on APT type, mirror_index mirror of x based on APT type, due to mirror, angle should be the inverse
                    angle_offset = 15;
                    var lineseries_laser96 = new LineSeries { };
                    lineseries_laser96.Points.Add(new DataPoint(11.55, -20));//添加线的第一个点坐标   //x,y exchange and -x to make the coordinate same with APTsuite OIM
                    lineseries_laser96.Points.Add(new DataPoint(9.81, -17));//添加线的第二个点的坐标//x,y exchange and -x to make the coordinate same with APTsuite OIM
                    model.Series.Add(lineseries_laser96);//将线添加到图标的容器中

                }
                double flightpath = Convert.ToDouble(textBox_FP.Text);
                double R_tip = 30;
                double ICF = Convert.ToDouble(textBox_ICF.Text);
                double pi = 3.1415926;
                double C_lattice = Convert.ToDouble(textBox_LC.Text);
                double D_pole_cut = Convert.ToDouble(textBox_DP.Text);
                double theta1 = (_x + angle_offset) / 57.28835;
                double theta2 = _y / 57.28835;
                double theta3 = _z / 57.28835;
                var g3 = DenseMatrix.OfArray(new[,] { { Math.Cos(theta3), Math.Sin(theta3), 0 }, { -Math.Sin(theta3), Math.Cos(theta3), 0 }, { 0, 0, 1 } });
                var g2 = DenseMatrix.OfArray(new[,] { { 1, 0, 0 }, { 0, Math.Cos(theta2), Math.Sin(theta2) }, { 0, -Math.Sin(theta2), Math.Cos(theta2) } });
                var g1 = DenseMatrix.OfArray(new[,] { { Math.Cos(theta1), Math.Sin(theta1), 0 }, { -Math.Sin(theta1), Math.Cos(theta1), 0 }, { 0, 0, 1 } });

                //var lineseries_CP = new LineSeries
                //{
                //StrokeThickness = 0.5,
                // Title = "Series 2", //线的说明
                // MarkerType = MarkerType.Square //标记点 的类型、形状

                //};
                // var r = new Random(314);

                if (comboBox_cstr.Text != "HCP")
                {

                    for (double i_t = -9; i_t <= 9; ++i_t)
                    {
                        for (double j_t = -9; j_t <= 9; ++j_t)
                        {
                            for (double k_t = -9; k_t <= 9; ++k_t)
                            {
                                double i = i_t, j = j_t, k = k_t;
                                double Denominator = Math.Sqrt(i * i + j * j + k * k);

                                if (Denominator != 0)
                                {
                                    double D_pole = C_lattice / Denominator;


                                    if (D_pole > D_pole_cut)
                                    {
                                        double[] tempvec = { i, j, k };
                                        var avector = new DenseVector(tempvec);
                                        avector = avector * g3 * g2 * g1;
                                        var r = new Random(314);
                                        if (avector[2] > 0)
                                        {
                                            double x1 = avector[0] / avector[2];
                                            double y1 = avector[1] / avector[2];
                                            if (x1 * x1 + y1 * y1 < 3)
                                            {
                                                var size = (int)(D_pole * 20);
                                                var colorValue = r.Next(10, 100);

                                                if (radioButton_white.Checked == true) { colorValue = 50; }

                                                scatterSeries.Points.Add(new ScatterPoint((flightpath / ICF) * y1 * mirror_index, (flightpath / ICF) * -x1 * mirrory_index, size, colorValue)); //x,y exchange and -x to make the coordinate same with APTsuite OIM
                                                //if (checkBox_PA.Checked == true && ((i == 1 && j == 1 && k == 1) || (i == 1 && j == 1 && k == 0) || (i == 0 && j == 1 && k == 1) || (i == 1 && j == 0 && k == 1) || (i == 1 && j == 0 && k == 0) || (i == 0 && j == 1 && k == 0) || (i == 0 && j == 0 && k == 1)))
                                                if (checkBox_PA.Checked == true)
                                                {
                                                    // coprime number
                                                    double[] matrixA = new double[3] { Math.Abs(i_t), Math.Abs(j_t), Math.Abs(k_t) };
                                                    //double[] matrixA = new double[3] { i, j, k };

                                                    int[] signsA = Array.ConvertAll(tempvec, n => n > 0 ? 1 : n < 0 ? -1 : 0);//get sign as 1 or -1 or 0
                                                    var m = coprime(matrixA);
                                                    m[0] = m[0] * signsA[0];
                                                    m[1] = m[1] * signsA[1];
                                                    m[2] = m[2] * signsA[2];

                                                    model.Annotations.Add(new TextAnnotation()
                                                    {

                                                        //Text = "(" + (Math.Sign(i_t) * m[0]).ToString() + " " + (Math.Sign(j_t) * m[1]).ToString() + " " + (Math.Sign(k_t) * m[2]).ToString() + ")",

                                                        Text = "(" + m[0].ToString() + " " + m[1].ToString() + " " + m[2].ToString() + ")",
                                                        TextPosition = new DataPoint((flightpath / ICF) * y1 * mirror_index, (flightpath / ICF) * -x1 * mirrory_index + 0.5),
                                                        Font = "Times New Roman",
                                                        FontSize = 12,
                                                        TextColor = OxyColors.Black
                                                    });

                                                }

                                            }
                                        }



                                        if (checkBox_PP.Checked == true)
                                        {
                                            if (avector[0] != 0)
                                            {
                                                var lineseries_CP = new LineSeries { };
                                                lineseries_CP.StrokeThickness = (D_pole * 10);
                                                double ykikuchi = -1;
                                                double xkikuchi = -ykikuchi * avector[1] / avector[0] - avector[2] / avector[0];
                                                lineseries_CP.Points.Add(new DataPoint((flightpath / ICF) * ykikuchi * mirror_index, mirrory_index * (flightpath / ICF) * -xkikuchi));//添加线的第一个点坐标   //x,y exchange and -x to make the coordinate same with APTsuite OIM
                                                ykikuchi = 1;
                                                xkikuchi = -ykikuchi * avector[1] / avector[0] - avector[2] / avector[0];
                                                lineseries_CP.Points.Add(new DataPoint((flightpath / ICF) * ykikuchi * mirror_index, mirrory_index * (flightpath / ICF) * -xkikuchi));//添加线的第二个点的坐标//x,y exchange and -x to make the coordinate same with APTsuite OIM
                                                model.Series.Add(lineseries_CP);//将线添加到图标的容器中
                                            }
                                            else
                                            {
                                                var lineseries_CP = new LineSeries { };
                                                lineseries_CP.StrokeThickness = (D_pole * 10);
                                                double xkikuchi = -1;
                                                double ykikuchi = -xkikuchi * avector[0] / avector[1] - avector[2] / avector[1];
                                                lineseries_CP.Points.Add(new DataPoint((flightpath / ICF) * ykikuchi, mirrory_index * (flightpath / ICF) * -xkikuchi));//添加线的第一个点坐标  //x,y exchange and -x to make the coordinate same with APTsuite OIM
                                                xkikuchi = 1;
                                                ykikuchi = -xkikuchi * avector[0] / avector[1] - avector[2] / avector[1];
                                                lineseries_CP.Points.Add(new DataPoint((flightpath / ICF) * ykikuchi, mirrory_index * (flightpath / ICF) * -xkikuchi));//添加线的第二个点的坐标//x,y exchange and -x to make the coordinate same with APTsuite OIM
                                                model.Series.Add(lineseries_CP);//将线添加到图标的容器中
                                            }

                                        }


                                    }
                                }
                            }
                        }
                    }
                }
                else//when select HCP
                {
                    for (double i_t = -9; i_t <= 9; ++i_t)
                    {
                        for (double j_t = -9; j_t <= 9; ++j_t)
                        {
                            for (double k_t = -9; k_t <= 9; ++k_t)
                            {
                                double i = i_t, j = j_t, k = k_t;

                                double hcpac = Convert.ToDouble(textBox_HCPc.Text);

                                double dhkl = Math.Sqrt(Math.Pow((2 * i + j), 2) + 3 * Math.Pow((j), 2) + 3 * Math.Pow((k * hcpac), 2));
                                i = (2 * i + j) / dhkl;
                                j = (Math.Sqrt(3) * j) / dhkl;
                                k = (Math.Sqrt(3) * k * (hcpac)) / dhkl;
                                double Denominator = Math.Sqrt((4.0 / 3.0) * ((i_t * i_t) + (i_t * j_t) + (j_t * j_t)) + (k_t * k_t) / (hcpac * hcpac));


                                if (Denominator != 0)
                                {
                                    double D_pole = C_lattice / Denominator;


                                    if (D_pole > D_pole_cut)
                                    {
                                        double[] tempvec = { i, j, k };
                                        var avector = new DenseVector(tempvec);
                                        avector = avector * g3 * g2 * g1;
                                        var r = new Random(314);
                                        if (avector[2] > 0)
                                        {
                                            double x1 = avector[0] / avector[2];
                                            double y1 = avector[1] / avector[2];
                                            if (x1 * x1 + y1 * y1 < 3)
                                            {
                                                var size = (int)(D_pole * 20);
                                                var colorValue = r.Next(10, 100);

                                                if (radioButton_white.Checked == true) { colorValue = 50; }

                                                scatterSeries.Points.Add(new ScatterPoint((flightpath / ICF) * y1 * mirror_index, (flightpath / ICF) * -x1 * mirrory_index, size, colorValue)); //x,y exchange and -x to make the coordinate same with APTsuite OIM
                                                //if (checkBox_PA.Checked == true && ((i == 1 && j == 1 && k == 1) || (i == 1 && j == 1 && k == 0) || (i == 0 && j == 1 && k == 1) || (i == 1 && j == 0 && k == 1) || (i == 1 && j == 0 && k == 0) || (i == 0 && j == 1 && k == 0) || (i == 0 && j == 0 && k == 1)))
                                                if (checkBox_PA.Checked == true)
                                                {
                                                    // coprime number
                                                    double[] matrixA = new double[3] { Math.Abs(i_t), Math.Abs(j_t), Math.Abs(k_t) };
                                                    //double[] matrixA = new double[3] { i, j, k };

                                                    int[] signsA = Array.ConvertAll(tempvec, n => n > 0 ? 1 : n < 0 ? -1 : 0);//get sign as 1 or -1 or 0

                                                    var m = coprime(matrixA);
                                                    m[0] = m[0] * signsA[0];
                                                    m[1] = m[1] * signsA[1];
                                                    m[2] = m[2] * signsA[2];

                                                    if (comboBox_cstr.Text == "HCP")
                                                    {
                                                        model.Annotations.Add(new TextAnnotation()
                                                        {
                                                            //Text = "(" + (Math.Sign(i_t) * m[0]).ToString() + " " + (Math.Sign(j_t) * m[1]).ToString() + " " + (-Math.Sign(i_t) * m[0] - Math.Sign(j_t) * m[1]).ToString() + " " + (Math.Sign(k_t) * m[2]).ToString() + ")",

                                                            Text = "(" + m[0].ToString() + " " + m[1].ToString() + " " + (-m[0] - m[1]).ToString() + " " + m[2].ToString() + ")",
                                                            TextPosition = new DataPoint((flightpath / ICF) * y1 * mirror_index, (flightpath / ICF) * -x1 * mirrory_index + 0.5),
                                                            Font = "Times New Roman",
                                                            FontSize = 12,
                                                            TextColor = OxyColors.Black
                                                        });
                                                    }
                                                    else
                                                    {

                                                        model.Annotations.Add(new TextAnnotation()
                                                        {

                                                            //Text = "(" + (Math.Sign(i_t) * m[0]).ToString() + " " + (Math.Sign(j_t) * m[1]).ToString() + " " + (Math.Sign(k_t) * m[2]).ToString() + ")",

                                                            Text = "(" + m[0].ToString() + " " + m[1].ToString() + " " + m[2].ToString() + ")",
                                                            TextPosition = new DataPoint((flightpath / ICF) * y1 * mirror_index, (flightpath / ICF) * -x1 * mirrory_index + 0.5),
                                                            Font = "Times New Roman",
                                                            FontSize = 12,
                                                            TextColor = OxyColors.Black
                                                        });
                                                    }
                                                }

                                            }
                                        }



                                        if (checkBox_PP.Checked == true)
                                        {
                                            if (avector[0] != 0)
                                            {
                                                var lineseries_CP = new LineSeries { };
                                                lineseries_CP.StrokeThickness = (D_pole * 10);
                                                double ykikuchi = -1;
                                                double xkikuchi = -ykikuchi * avector[1] / avector[0] - avector[2] / avector[0];
                                                lineseries_CP.Points.Add(new DataPoint((flightpath / ICF) * ykikuchi * mirror_index, mirrory_index * (flightpath / ICF) * -xkikuchi));//添加线的第一个点坐标   //x,y exchange and -x to make the coordinate same with APTsuite OIM
                                                ykikuchi = 1;
                                                xkikuchi = -ykikuchi * avector[1] / avector[0] - avector[2] / avector[0];
                                                lineseries_CP.Points.Add(new DataPoint((flightpath / ICF) * ykikuchi * mirror_index, mirrory_index * (flightpath / ICF) * -xkikuchi));//添加线的第二个点的坐标//x,y exchange and -x to make the coordinate same with APTsuite OIM
                                                model.Series.Add(lineseries_CP);//将线添加到图标的容器中
                                            }
                                            else
                                            {
                                                var lineseries_CP = new LineSeries { };
                                                lineseries_CP.StrokeThickness = (D_pole * 10);
                                                double xkikuchi = -1;
                                                double ykikuchi = -xkikuchi * avector[0] / avector[1] - avector[2] / avector[1];
                                                lineseries_CP.Points.Add(new DataPoint((flightpath / ICF) * ykikuchi, mirrory_index * (flightpath / ICF) * -xkikuchi));//添加线的第一个点坐标  //x,y exchange and -x to make the coordinate same with APTsuite OIM
                                                xkikuchi = 1;
                                                ykikuchi = -xkikuchi * avector[0] / avector[1] - avector[2] / avector[1];
                                                lineseries_CP.Points.Add(new DataPoint((flightpath / ICF) * ykikuchi, mirrory_index * (flightpath / ICF) * -xkikuchi));//添加线的第二个点的坐标//x,y exchange and -x to make the coordinate same with APTsuite OIM
                                                model.Series.Add(lineseries_CP);//将线添加到图标的容器中
                                            }

                                        }


                                    }
                                }
                            }
                        }
                    }
                }


                //Bitmap startBitmap = CreateBitmapFromBytes(bmpdata); // write CreateBitmapFromBytes  
                //Bitmap newBitmap = new Bitmap(330, 330);
                //using (Graphics graphics = Graphics.FromImage(newBitmap))
                //{
                //    graphics.DrawImage(startBitmap, new Rectangle(0, 0, 330, 330), new Rectangle(0, 0, startBitmap.Width, startBitmap.Height), GraphicsUnit.Pixel);
                //}

                //byte[] newBytes = CreateBytesFromBitmap(newBitmap); // write CreateBytesFromBitmap 

                // ///****byte[] bbmp = CreateThumbnail(bmpdata, 1);

                // *Add background image
                if (checkBox_BG.Checked != false)
                {
                    if (bmpdata != null)
                    {
                        //var imgAnnotation = new ImageAnnotation { X = new PlotLength(4, PlotLengthUnit.Data), Y = new PlotLength(2, PlotLengthUnit.Data), HorizontalAlignment = OxyPlot.HorizontalAlignment.Right };

                        var imgAnnotation = new ImageAnnotation();
                        imgAnnotation.ImageSource = new OxyImage(bmpdata);
                        imgAnnotation.Layer = AnnotationLayer.BelowAxes;
                        model.Annotations.Add(imgAnnotation);
                    }
                }
                // *
                model.Annotations.Add(new EllipseAnnotation { X = 0, Y = 0, Width = 2 * radius, Height = 2 * radius, Fill = OxyColors.Transparent, Stroke = OxyColors.Black, StrokeThickness = 1 });
                //FunctionSeries f = new FunctionSeries((x) => Math.Sqrt(Math.Max(16 - Math.Pow(x, 2), 0)));
                // model.Series.Add(new FunctionSeries((x) => Math.Sqrt(Math.Max((16 - Math.Pow(x, 2)),0), -4, 4, 0.1, "x^2 + y^2 = 16") { Color = OxyColors.Red });
                model.Series.Add(scatterSeries);

                // * Reflectron axis
                int axiss = 20;
                if (checkBox_REF.Checked != false)
                { axiss = 20; }
                else { axiss = 38; }
                // *

                LinearAxis leftAxis = new LinearAxis()
                {

                    Position = AxisPosition.Left,
                    Minimum = 0 - axiss,
                    Maximum = axiss,
                    Title = "Y",//显示标题内容
                    TitlePosition = 1,//显示标题位置
                    TitleColor = OxyColor.Parse("#d3d3d3"),//显示标题位置
                    IsZoomEnabled = true,//坐标轴缩放关闭
                    IsPanEnabled = true,//图表缩放功能关闭

                    //MajorGridlineStyle = LineStyle.Solid,//主刻度设置格网
                    //MajorGridlineColor = OxyColor.Parse("#7379a0"),
                    //MinorGridlineStyle = LineStyle.Dot,//子刻度设置格网样式
                    //MinorGridlineColor = OxyColor.Parse("#666b8d")

                };


                LinearAxis bottomAxis = new LinearAxis()
                {
                    Position = AxisPosition.Bottom,
                    Minimum = 0 - axiss,
                    Maximum = axiss,
                    Title = "X",//显示标题内容
                    TitlePosition = 1,//显示标题位置
                    TitleColor = OxyColor.Parse("#d3d3d3"),//显示标题位置
                    IsZoomEnabled = true,//坐标轴缩放关闭
                    IsPanEnabled = true,//图表缩放功能关闭
                    //MajorGridlineStyle = LineStyle.Solid,//主刻度设置格网
                    //MajorGridlineColor = OxyColor.Parse("#7379a0"),
                    //MinorGridlineStyle = LineStyle.Dot,//子刻度设置格网样式
                    //MinorGridlineColor = OxyColor.Parse("#666b8d")
                };


                model.Axes.Add(leftAxis);
                model.Axes.Add(bottomAxis);
                //Assign PlotModel to PlotView
                myPlot.Model = model;

                //Set up plot for display
                myPlot.Dock = System.Windows.Forms.DockStyle.Bottom;
                myPlot.Location = new System.Drawing.Point(0, 0);
                myPlot.Size = new System.Drawing.Size(1000, 390);
                // myPlot.BackgroundImage = new BitmapImage(new Uri(@"path/to/some/image"));
                //定义y轴

                myPlot.TabIndex = 0;

                //Add plot control to form
                panel1.Controls.Add(myPlot);
            }
            catch (Exception)
            {
                MessageBox.Show("Pls check all parameters are inputted");
            }
            //
            //try
            //{

            //var fai = double.Parse(z_euler.Text);//注意var定义在方法内
            //var fai1 = double.Parse(x_euler.Text);
            //var fai2 = double.Parse(y_euler.Text);

            //创建A，B，C，D矩阵
            //double[] matrixA = new double[3] { Math.Cos(Math.PI * fai1 / 180) * Math.Cos(Math.PI * fai2 / 180) - Math.Sin(Math.PI * fai1 / 180) * Math.Sin(Math.PI * fai2 / 180) * Math.Cos(Math.PI * fai / 180), -Math.Sin(Math.PI * fai2 / 180) * Math.Cos(Math.PI * fai1 / 180) - Math.Sin(Math.PI * fai1 / 180) * Math.Cos(Math.PI * fai2 / 180) * Math.Cos(Math.PI * fai / 180), Math.Sin(Math.PI * fai1 / 180) * Math.Sin(Math.PI * fai / 180) };
            //double[] matrixB = new double[3] { Math.Sin(Math.PI * fai / 180) * Math.Sin(Math.PI * fai2 / 180), Math.Sin(Math.PI * fai / 180) * Math.Cos(Math.PI * fai2 / 180), Math.Cos(Math.PI * fai / 180) };

            //2个矩阵相乘，要注意矩阵乘法的维数要求
            //var m = matrixA;
            //var n = matrixB;
            //m = primecvt(m);
            //n = primecvt(n);
            //textBoxH.Text = m[0].ToString();
            //textBoxK.Text = m[1].ToString();
            //textBoxL.Text = m[2].ToString();
            //textBoxU.Text = n[0].ToString();
            //textBoxV.Text = n[1].ToString();
            //textBoxW.Text = n[2].ToString();

            // }
            //
            //catch (Exception)
            //{
            //    MessageBox.Show("Unexpected error");
            //}

        }

        private void button_miller_Click(object sender, EventArgs e)
        {
            try
            {
                double L = System.Convert.ToDouble(textBoxL.Text);
                double K = System.Convert.ToDouble(textBoxK.Text);
                double H = System.Convert.ToDouble(textBoxH.Text);
                double U = System.Convert.ToDouble(textBoxU.Text);
                double V = System.Convert.ToDouble(textBoxV.Text);
                double W = System.Convert.ToDouble(textBoxW.Text);
                double fai = 180 * Math.Acos(L / (Math.Sqrt((Math.Pow(H, 2) + Math.Pow(K, 2) + Math.Pow(L, 2))))) / Math.PI;
                double fai1 = 180 * Math.Asin(W / Math.Sqrt(Math.Pow(U, 2) + Math.Pow(V, 2) + Math.Pow(W, 2)) * Math.Sqrt(Math.Pow(H, 2) + Math.Pow(K, 2) + Math.Pow(L, 2)) / Math.Sqrt(Math.Pow(H, 2) + Math.Pow(K, 2))) / Math.PI;
                double fai2 = 180 * Math.Acos(K / (Math.Sqrt((Math.Pow(H, 2) + Math.Pow(K, 2))))) / Math.PI;
                x_euler.Text = fai1.ToString();
                y_euler.Text = fai.ToString();
                z_euler.Text = fai2.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Unexpected error");
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            //Thread.Sleep(300);
            screenCaputre.StartCaputre(30, lastSize);
        }

        //用于晶体指数互质
        public double[] coprime(double[] mart)//可以将输入的小数数组做互质处理后输出
        {
            try
            {
                double min1;//用于储存最大的分母
                int a = mart.GetLength(0);//得到输入数组行数
                //#region把Mart里面的数取非0的最小数

                min1 = 1;//初始值
                for (int i = 0; i < a; i++)//取出非零的一个绝对值做min1的初始值
                {
                    if (mart[i] != 0)
                    {
                        if (mart[i] < 0)
                        {
                            min1 = -mart[i];
                        }
                        else
                        {
                            min1 = mart[i];
                        }
                        break;
                    }
                }

                for (int i = 0; i < a; i++)//取非零最小绝对值min1
                {

                    if (mart[i] < 0)
                    {
                        if (-mart[i] < min1) { min1 = -mart[i]; }
                    }
                    else if (mart[i] == 0)
                    {

                    }
                    else
                    {
                        if (mart[i] < min1) { min1 = mart[i]; }
                    }

                }

                //#endregion

                int a2 = a;
                int n = 0;

                while (n != a2)//当整数数与元素数不同时执行??
                {
                    n = 0;
                    for (int i = 0; i < a; i++)
                    {
                        if (mart[i] != 0)
                        {
                            if ((Convert.ToDouble((int)(mart[i] / min1))) == mart[i] / min1)//判断是不是整数
                            {
                                n++;
                            }
                        }
                        else //如果遇到0就把元素数减1
                        {
                            a2--;
                        }

                    }
                    min1--;
                }
                min1++;

                for (int i = 0; i < a; i++)
                {
                    mart[i] = mart[i] / min1;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unexpected error");
            }
            return mart;
        }


        public double[] primecvt(double[] mart)//可以将输入的小数数组做互质处理后输出
        {
            try
            {
                double max1;//用于储存最大的分母
                double min1;//用于储存最大的分母
                int a = mart.GetLength(0);//得到输入数组行数
                string[] b = new string[a];//用于储存转化后的分数
                double[] sArray = new double[a];//新建二维数组用于储存转化后的分母
                string[] sArra;

                do
                {
                    int count = 0;
                    while (count < a)
                    {
                        b[count] = this.integercvt(mart[count]);//把mart2中各数转成分数存b
                        sArra = b[count].Split(new char[1] { '/' });//把b中各数按/分割后存到sArra
                        sArray[count] = Convert.ToDouble(sArra[1]);//sArra取分母转double存sArray
                        count++;
                    }
                    max1 = sArray.Max();//储存最大的分母
                    for (int i = 0; i < a; i++)
                    {
                        mart[i] = mart[i] * max1;//mart里面各个数乘上最大分母
                    }
                }
                while (max1 != 1);//当最大分母为1时退出

                //#region把Mart里面的数取非0的最小数

                min1 = 1;//初始值
                for (int i = 0; i < a; i++)//取出非零的一个绝对值做min1的初始值
                {
                    if (mart[i] != 0)
                    {
                        if (mart[i] < 0)
                        {
                            min1 = -mart[i];
                        }
                        else
                        {
                            min1 = mart[i];
                        }
                        break;
                    }
                }

                for (int i = 0; i < a; i++)//取非零最小绝对值min1
                {

                    if (mart[i] < 0)
                    {
                        if (-mart[i] < min1) { min1 = -mart[i]; }
                    }
                    else if (mart[i] == 0)
                    {

                    }
                    else
                    {
                        if (mart[i] < min1) { min1 = mart[i]; }
                    }

                }

                //#endregion

                int a2 = a;
                int n = 0;

                while (n != a2)//当整数数与元素数不同时执行??
                {
                    n = 0;
                    for (int i = 0; i < a; i++)
                    {
                        if (mart[i] != 0)
                        {
                            if ((Convert.ToDouble((int)(mart[i] / min1))) == mart[i] / min1)//判断是不是整数
                            {
                                n++;
                            }
                        }
                        else //如果遇到0就把元素数减1
                        {
                            a2--;
                        }

                    }
                    min1--;
                }
                min1++;

                for (int i = 0; i < a; i++)
                {
                    mart[i] = mart[i] / min1;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unexpected error");
            }
            return mart;
        }


        public string integercvt(double numb)//可以将输入的小数转换为分数
        {

            string result = default(string);
            try
            {
                if (numb > 6E-17)//6e-17看做0
                {
                    double inputnum = numb;
                    string[] array = inputnum.ToString().Split('.');
                    int len = array[1].Length;
                    long num = Convert.ToInt64(Math.Pow(10, len));
                    long value = Convert.ToInt64(inputnum * num);
                    long a = value;
                    long b = num;
                    while (a != b)
                    {
                        if (a > b)
                            a = a - b;
                        else
                            b = b - a;
                    }
                    value = value / a;
                    num = num / a;
                    result = string.Format("{0}/{1}", value, num);
                }
                else if (numb < -6E-17)
                {
                    double inputnum = numb;
                    string[] array = inputnum.ToString().Split('-', '.');
                    int len = array[2].Length;
                    long num = Convert.ToInt64(Math.Pow(10, len));
                    long value = Convert.ToInt64(-inputnum * num);
                    long a = value;
                    long b = num;
                    while (a != b)
                    {
                        if (a > b)
                            a = a - b;
                        else
                            b = b - a;
                    }
                    value = value / a;
                    num = num / a;
                    result = string.Format("-{0}/{1}", value, num);
                }
                else if (numb < 6E-17 && numb > -6E-17)//6e-17看做0
                {
                    result = "0/1";
                }
            }
            catch (Exception)
            {
                result = numb.ToString() + "/1";
            }
            return result;
        }



        //暂时没用下面的func
        public double[] HCPEULERTOMILLER(double phi1, double theta, double phi2, double caratio)
        {
            var fai = theta;//注意var定义在方法内
            var fai1 = phi1;
            var fai2 = phi2;
            var ca = caratio;

            //创建A，B，C，D矩阵
            var matrixA = DenseMatrix.OfArray(new[,] { { System.Math.Sqrt(3) / 2, -1 / 2, 0 }, { 0, 1, 0 }, { System.Math.Sqrt(3) / -2, -1 / 2, 0 }, { 0, 0, ca } });
            var matrixB = DenseMatrix.OfArray(new[,] { { 2 / 3, -1 / 3, 0 }, { 0, 2 / 3, 0 }, { -2 / 3, -1 / 3, 0 }, { 0, 0, ca } });
            var matrixC = DenseMatrix.OfArray(new[,] { { Math.Cos(Math.PI * fai1 / 180) * Math.Cos(Math.PI * fai2 / 180) - Math.Sin(Math.PI * fai1 / 180) * Math.Sin(Math.PI * fai2 / 180) * Math.Cos(Math.PI * fai / 180) }, { -Math.Sin(Math.PI * fai2 / 180) * Math.Cos(Math.PI * fai1 / 180) - Math.Sin(Math.PI * fai1 / 180) * Math.Cos(Math.PI * fai2 / 180) * Math.Cos(Math.PI * fai / 180) }, { Math.Sin(Math.PI * fai1 / 180) * Math.Sin(Math.PI * fai / 180) } });
            var matrixD = DenseMatrix.OfArray(new[,] { { Math.Sin(Math.PI * fai / 180) * Math.Sin(Math.PI * fai2 / 180) }, { Math.Sin(Math.PI * fai / 180) * Math.Cos(Math.PI * fai2 / 180) }, { Math.Cos(Math.PI * fai / 180) } });

            //2个矩阵相乘，要注意矩阵乘法的维数要求
            var resultM = matrixA * matrixC;//也可以使用Multiply方法
            var resultN = matrixB * matrixD;//也可以使用Multiply方法
            double[][] resultM2 = resultM.ToRowArrays();//转换为double[][]数组
            double[] m = new double[2 * resultM2.GetLength(0)];//转换为double[]数组
            double[][] resultN2 = resultN.ToRowArrays();//转换为double[][]数组

            for (int i = 0; i < resultM2.GetLength(0); i++)
            {
                m[i] = resultM2[i][0];
            }

            for (int i = 0; i < resultM2.GetLength(0); i++)
            {
                m[i + 4] = resultN2[i][0];
            }
            return m;
        }




        private System.Drawing.Image NewGetBitmap(System.Windows.Media.Imaging.BitmapSource imgSource)
        {
            //创建一个bmp文件流
            FileStream stream = new FileStream("new.bmp", FileMode.Create);
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imgSource));
            encoder.Save(stream);
            image = System.Drawing.Image.FromStream(stream);
            stream.Close();
            return image;
        }


        private void button_slimg_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = true;
                fileDialog.Title = "Select Pole Figure";
                fileDialog.Filter = "Bitmap files (*.bmp)|*.bmp|All files (*.*)|*.*";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    imgpath = fileDialog.FileName;
                    // textBox1.Text = imgpath;
                }

                //*put bmp into bmpdata for background
                var polebmp = new BitmapImage(new Uri(@imgpath));
                Bitmap bmp = BitmapImage2Bitmap(polebmp);
                bmp = ResizeImage(bmp, 310, 314);
                BitmapSource bmpsource = imagework.getBitMapSourceFromBitmap(bmp);

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmpsource));

                // image to byte[]
                using (var ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    bmpdata = ms.ToArray();
                }
                //*
                this.Show();
                this.panel1_Paint(null, null);
            }
            catch
            {
                // recover from exception
            }
        }

        private void button_refmessge_Click(object sender, EventArgs e)
        {
            MessageBox.Show("LEAP 5000R (5096,5108) with reflectron, LEAP 5000 (5076) without reflectron.");
        }

        // Create a thumbnail in byte array format from the image encoded in the passed byte array.  
        // (RESIZE an image in a byte[] variable.)  
        public static byte[] CreateThumbnail(byte[] PassedImage, int LargestSide)
        {
            byte[] ReturnedThumbnail;

            using (MemoryStream StartMemoryStream = new MemoryStream(),
                                NewMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                StartMemoryStream.Write(PassedImage, 0, PassedImage.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                Bitmap startBitmap = new Bitmap(StartMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double HW_ratio;
                if (startBitmap.Height > startBitmap.Width)
                {
                    newHeight = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Height);
                    newWidth = (int)(HW_ratio * (double)startBitmap.Width);
                }
                else
                {
                    newWidth = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Width);
                    newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                }

                // create a new Bitmap with dimensions for the thumbnail.  
                Bitmap newBitmap = new Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                ReturnedThumbnail = NewMemoryStream.ToArray();
            }

            // return the resized image as a string of bytes.  
            return ReturnedThumbnail;
        }

        // Resize a Bitmap  
        private static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return resizedImage;
        }

        private System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();

                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = "png";
                dlg.Filter = "png Files|*.png";
                dlg.FileName = "ScreenSnap" + imageNum.ToString();
                DialogResult res = dlg.ShowDialog();
                var pngExporter = new PngExporter { Width = 310, Height = 314, Resolution = 200 };
                pngExporter.ExportToFile(model, dlg.FileName);
                //MessageBox.Show("Save successful!");
                imageNum++;
            }
            catch (System.Runtime.InteropServices.ExternalException ex) { throw ex; }
        }


        private void button_copy_Click(object sender, EventArgs e)
        {
            try
            {
                var pngExporter = new PngExporter { Width = 310, Height = 314, Resolution = 200 };
                var bitmap = pngExporter.ExportToBitmap(model);
                System.Windows.Forms.Clipboard.SetImage(bitmap);
            }
            catch (System.Runtime.InteropServices.ExternalException ex) { throw ex; }
        }

        private void button_saveG1_Click(object sender, EventArgs e)
        {
            //double[] euler1 = MisorientationAngleCalculator.EulerAngleConverter(Convert.ToDouble(x_euler.Text), Convert.ToDouble(y_euler.Text), Convert.ToDouble(z_euler.Text));
            textBox_G1phi1.Text = x_euler.Text;
            textBox_G1theta.Text = y_euler.Text;
            textBox_G1phi2.Text = z_euler.Text;
        }

        private void button_saveG2_Click(object sender, EventArgs e)
        {
            textBox_G2phi1.Text = x_euler.Text;
            textBox_G2theta.Text = y_euler.Text;
            textBox_G2phi2.Text = z_euler.Text;
        }

        private void button_G1_Click(object sender, EventArgs e)
        {
            x_euler.Text = textBox_G1phi1.Text;
            y_euler.Text = textBox_G1theta.Text;
            z_euler.Text = textBox_G1phi2.Text;
        }

        private void button_G2_Click(object sender, EventArgs e)
        {
            x_euler.Text = textBox_G2phi1.Text;
            y_euler.Text = textBox_G2theta.Text;
            z_euler.Text = textBox_G2phi2.Text;
        }

        private void button_misori_Click(object sender, EventArgs e)
        {
            double[] RAngAxis = MisorientationAngleCalculator.MisorientationAngle(Convert.ToDouble(textBox_G1phi1.Text), Convert.ToDouble(textBox_G1theta.Text), Convert.ToDouble(textBox_G1phi2.Text), Convert.ToDouble(textBox_G2phi1.Text), Convert.ToDouble(textBox_G2theta.Text), Convert.ToDouble(textBox_G2phi2.Text));
            textBox_miso.Text = RAngAxis[0].ToString();
            textBox_RAxis1.Text = RAngAxis[1].ToString();
            textBox_RAxis2.Text = RAngAxis[2].ToString();
            textBox_RAxis3.Text = RAngAxis[3].ToString();
            // Example usage
            double[] axis1 = { RAngAxis[1], RAngAxis[2], RAngAxis[3] };
            double misorientationAngle1 = RAngAxis[0];
            string structureType = "FCC";

            var result1 = SigmaCSL.IsSigmaBoundary(axis1, misorientationAngle1, structureType);
            if (result1.isSigma)
            {
                richTextBox_Sigma.Text = $"This is a Σ{result1.sigmaValue} boundary with a deviation of {result1.deviation:F2} degrees";
            }
            else
            {
                richTextBox_Sigma.Text = "This is not a Σ boundary";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            x_euler.Text = (Convert.ToDouble(x_euler.Text) + 1).ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            x_euler.Text = (Convert.ToDouble(x_euler.Text) - 1).ToString();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            y_euler.Text = (Convert.ToDouble(y_euler.Text) + 1).ToString();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            y_euler.Text = (Convert.ToDouble(y_euler.Text) - 1).ToString();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            z_euler.Text = (Convert.ToDouble(z_euler.Text) + 1).ToString();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            z_euler.Text = (Convert.ToDouble(z_euler.Text) - 1).ToString();

        }

        private void comboBox_cstr_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox_cstr.Text == "HCP")
            {
                textBoxI.ReadOnly = false;
                textBoxT.ReadOnly = false;
                label_HCPc.Visible = true;
                textBox_HCPc.Visible = true;
            }
            else
            {
                textBoxI.ReadOnly = true;
                textBoxT.ReadOnly = true;
                label_HCPc.Visible = false;
                textBox_HCPc.Visible = false;
            }
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Baesed on LEAP 5000R (5096,5108), LEAP 5000 (5076)");
        }

        private void button_5076_Click(object sender, EventArgs e)
        {
            textBox_FP.Text = "100";
            textBox_ICF.Text = "2";
            checkBox_REF.Checked = false;
        }

        private void button_5096_Click(object sender, EventArgs e)
        {
            textBox_FP.Text = "40";
            textBox_ICF.Text = "1.5";
            checkBox_REF.Checked = true;
        }

    }

    public class MisorientationAngleCalculator
    {
        // Helper method to multiply two 3x3 matrices
        public static double[,] MatrixMultiply(double[,] mat1, double[,] mat2)
        {
            int row1 = mat1.GetLength(0);
            int col1 = mat1.GetLength(1);
            int col2 = mat2.GetLength(1);

            double[,] result = new double[row1, col2];

            for (int i = 0; i < row1; i++)
            {
                for (int j = 0; j < col2; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < col1; k++)
                    {
                        sum += mat1[i, k] * mat2[k, j];
                    }
                    result[i, j] = sum;
                }
            }
            return result;
        }

        // Helper method to calculate the inverse of a 3x3 matrix
        public static double[,] InverseMatrix(double[,] matrix)
        {
            double determinant = matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[2, 1] * matrix[1, 2]) -
                                 matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0]) +
                                 matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);

            double invDet = 1 / determinant;

            double[,] inverse = new double[3, 3];
            inverse[0, 0] = (matrix[1, 1] * matrix[2, 2] - matrix[2, 1] * matrix[1, 2]) * invDet;
            inverse[0, 1] = (matrix[0, 2] * matrix[2, 1] - matrix[0, 1] * matrix[2, 2]) * invDet;
            inverse[0, 2] = (matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1]) * invDet;
            inverse[1, 0] = (matrix[1, 2] * matrix[2, 0] - matrix[1, 0] * matrix[2, 2]) * invDet;
            inverse[1, 1] = (matrix[0, 0] * matrix[2, 2] - matrix[0, 2] * matrix[2, 0]) * invDet;
            inverse[1, 2] = (matrix[0, 2] * matrix[1, 0] - matrix[0, 0] * matrix[1, 2]) * invDet;
            inverse[2, 0] = (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]) * invDet;
            inverse[2, 1] = (matrix[0, 1] * matrix[2, 0] - matrix[0, 0] * matrix[2, 1]) * invDet;
            inverse[2, 2] = (matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]) * invDet;
            return inverse;
        }

        //REF:https://www.researchgate.net/publication/331488376_Finding_misorientation_of_grain_boundaries_from_Euler_angles_for_cubic_systems
        public static double[] MisorientationAngle(double p1, double p, double p2, double q1, double q, double q2)
        {
            p1 = p1 * Math.PI / 180;
            p = p * Math.PI / 180;
            p2 = p2 * Math.PI / 180;
            q1 = q1 * Math.PI / 180;
            q = q * Math.PI / 180;
            q2 = q2 * Math.PI / 180;
            // Convert Euler angles to matrices for both grains
            double[,] gp1 = new double[3, 3];
            double[,] gp2 = new double[3, 3];
            double[,] gp = new double[3, 3];
            double[,] gq1 = new double[3, 3];
            double[,] gq2 = new double[3, 3];
            double[,] gq = new double[3, 3];
            double[,] g1 = new double[3, 3];
            double[,] g2 = new double[3, 3];

            gp1[0, 0] = Math.Cos(p1);
            gp1[1, 0] = -Math.Sin(p1);
            gp1[0, 1] = Math.Sin(p1);
            gp1[1, 1] = Math.Cos(p1);
            gp1[2, 2] = 1;

            gp2[0, 0] = Math.Cos(p2);
            gp2[1, 0] = -Math.Sin(p2);
            gp2[0, 1] = Math.Sin(p2);
            gp2[1, 1] = Math.Cos(p2);
            gp2[2, 2] = 1;

            gp[0, 0] = 1;
            gp[1, 1] = Math.Cos(p);
            gp[1, 2] = Math.Sin(p);
            gp[2, 1] = -Math.Sin(p);
            gp[2, 2] = Math.Cos(p);

            gq1[0, 0] = Math.Cos(q1);
            gq1[1, 0] = -Math.Sin(q1);
            gq1[0, 1] = Math.Sin(q1);
            gq1[1, 1] = Math.Cos(q1);
            gq1[2, 2] = 1;

            gq2[0, 0] = Math.Cos(q2);
            gq2[1, 0] = -Math.Sin(q2);
            gq2[0, 1] = Math.Sin(q2);
            gq2[1, 1] = Math.Cos(q2);
            gq2[2, 2] = 1;

            gq[0, 0] = 1;
            gq[1, 1] = Math.Cos(q);
            gq[1, 2] = Math.Sin(q);
            gq[2, 1] = -Math.Sin(q);
            gq[2, 2] = Math.Cos(q);

            g1 = MatrixMultiply(gp2, gp);
            g1 = MatrixMultiply(g1, gp1);
            g2 = MatrixMultiply(gq2, gq);
            g2 = MatrixMultiply(g2, gq1);


            // Symmetry matrices considering the 24 symmetries for the cubic system
            double[,,] T = new double[3, 3, 24];
            T[0, 0, 0] = 1; T[0, 1, 0] = 0; T[0, 2, 0] = 0;
            T[1, 0, 0] = 0; T[1, 1, 0] = 1; T[1, 2, 0] = 0;
            T[2, 0, 0] = 0; T[2, 1, 0] = 0; T[2, 2, 0] = 1;

            T[0, 0, 1] = 0; T[0, 1, 1] = 0; T[0, 2, 1] = -1;
            T[1, 0, 1] = 0; T[1, 1, 1] = -1; T[1, 2, 1] = 0;
            T[2, 0, 1] = -1; T[2, 1, 1] = 0; T[2, 2, 1] = 0;

            T[0, 0, 2] = 0; T[0, 1, 2] = 0; T[0, 2, 2] = -1;
            T[1, 0, 2] = 0; T[1, 1, 2] = 1; T[1, 2, 2] = 0;
            T[2, 0, 2] = 1; T[2, 1, 2] = 0; T[2, 2, 2] = 0;


            T[0, 0, 3] = -1; T[0, 1, 3] = 0; T[0, 2, 3] = 0;
            T[1, 0, 3] = 0; T[1, 1, 3] = 1; T[1, 2, 3] = 0;
            T[2, 0, 3] = 0; T[2, 1, 3] = 0; T[2, 2, 3] = -1;

            T[0, 0, 4] = 0; T[0, 1, 4] = 0; T[0, 2, 4] = 1;
            T[1, 0, 4] = 0; T[1, 1, 4] = 1; T[1, 2, 4] = 0;
            T[2, 0, 4] = -1; T[2, 1, 4] = 0; T[2, 2, 4] = 0;

            T[0, 0, 5] = 0; T[0, 1, 5] = 0; T[0, 2, 5] = 1;
            T[1, 0, 5] = 0; T[1, 1, 5] = 1; T[1, 2, 5] = 0;
            T[2, 0, 5] = -1; T[2, 1, 5] = 0; T[2, 2, 5] = 0;


            T[0, 0, 6] = 1; T[0, 1, 6] = 0; T[0, 2, 6] = 0;
            T[1, 0, 6] = 0; T[1, 1, 6] = 0; T[1, 2, 6] = -1;
            T[2, 0, 6] = 0; T[2, 1, 6] = 1; T[2, 2, 6] = 0;

            T[0, 0, 7] = 1; T[0, 1, 7] = 0; T[0, 2, 7] = 0;
            T[1, 0, 7] = 0; T[1, 1, 7] = -1; T[1, 2, 7] = 0;
            T[2, 0, 7] = 0; T[2, 1, 7] = 0; T[2, 2, 7] = -1;

            T[0, 0, 8] = 1; T[0, 1, 8] = 0; T[0, 2, 8] = 0;
            T[1, 0, 8] = 0; T[1, 1, 8] = 0; T[1, 2, 8] = 1;
            T[2, 0, 8] = 0; T[2, 1, 8] = -1; T[2, 2, 8] = 0;

            T[0, 0, 9] = 0; T[0, 1, 9] = -1; T[0, 2, 9] = 0;
            T[1, 0, 9] = 1; T[1, 1, 9] = 0; T[1, 2, 9] = 0;
            T[2, 0, 9] = 0; T[2, 1, 9] = 0; T[2, 2, 9] = 1;

            T[0, 0, 10] = -1; T[0, 1, 10] = 0; T[0, 2, 10] = 0;
            T[1, 0, 10] = 0; T[1, 1, 10] = -1; T[1, 2, 10] = 0;
            T[2, 0, 10] = 0; T[2, 1, 10] = 0; T[2, 2, 10] = 1;

            T[0, 0, 11] = 0; T[0, 1, 11] = 1; T[0, 2, 11] = 0;
            T[1, 0, 11] = -1; T[1, 1, 11] = 0; T[1, 2, 11] = 0;
            T[2, 0, 11] = 0; T[2, 1, 11] = 0; T[2, 2, 11] = 1;

            T[0, 0, 12] = 0; T[0, 1, 12] = 0; T[0, 2, 12] = 1;
            T[1, 0, 12] = 1; T[1, 1, 12] = 0; T[1, 2, 12] = 0;
            T[2, 0, 12] = 0; T[2, 1, 12] = 1; T[2, 2, 12] = 0;

            T[0, 0, 13] = 0; T[0, 1, 13] = 1; T[0, 2, 13] = 0;
            T[1, 0, 13] = 0; T[1, 1, 13] = 0; T[1, 2, 13] = 1;
            T[2, 0, 13] = 1; T[2, 1, 13] = 0; T[2, 2, 13] = 0;

            T[0, 0, 14] = 0; T[0, 1, 14] = 0; T[0, 2, 14] = -1;
            T[1, 0, 14] = -1; T[1, 1, 14] = 0; T[1, 2, 14] = 0;
            T[2, 0, 14] = 0; T[2, 1, 14] = 1; T[2, 2, 14] = 0;

            T[0, 0, 15] = 0; T[0, 1, 15] = -1; T[0, 2, 15] = 0;
            T[1, 0, 15] = 0; T[1, 1, 15] = 0; T[1, 2, 15] = 1;
            T[2, 0, 15] = -1; T[2, 1, 15] = 0; T[2, 2, 15] = 0;

            T[0, 0, 16] = 0; T[0, 1, 16] = 1; T[0, 2, 16] = 0;
            T[1, 0, 16] = 0; T[1, 1, 16] = 0; T[1, 2, 16] = -1;
            T[2, 0, 16] = -1; T[2, 1, 16] = 0; T[2, 2, 16] = 0;

            T[0, 0, 17] = 0; T[0, 1, 17] = 0; T[0, 2, 17] = -1;
            T[1, 0, 17] = 1; T[1, 1, 17] = 0; T[1, 2, 17] = 0;
            T[2, 0, 17] = 0; T[2, 1, 17] = -1; T[2, 2, 17] = 0;

            T[0, 0, 18] = 0; T[0, 1, 18] = 0; T[0, 2, 18] = 1;
            T[1, 0, 18] = -1; T[1, 1, 18] = 0; T[1, 2, 18] = 0;
            T[2, 0, 18] = 0; T[2, 1, 18] = -1; T[2, 2, 18] = 0;


            T[0, 0, 19] = 0; T[0, 1, 19] = -1; T[0, 2, 19] = 0;
            T[1, 0, 19] = 0; T[1, 1, 19] = 0; T[1, 2, 19] = -1;
            T[2, 0, 19] = 1; T[2, 1, 19] = 0; T[2, 2, 19] = 0;


            T[0, 0, 20] = 0; T[0, 1, 20] = 1; T[0, 2, 20] = 0;
            T[1, 0, 20] = 1; T[1, 1, 20] = 0; T[1, 2, 20] = 0;
            T[2, 0, 20] = 0; T[2, 1, 20] = 0; T[2, 2, 20] = -1;

            T[0, 0, 21] = -1; T[0, 1, 21] = 0; T[0, 2, 21] = 0;
            T[1, 0, 21] = 0; T[1, 1, 21] = 0; T[1, 2, 21] = 1;
            T[2, 0, 21] = 0; T[2, 1, 21] = 1; T[2, 2, 21] = 0;

            T[0, 0, 22] = 0; T[0, 1, 22] = 0; T[0, 2, 22] = 1;
            T[1, 0, 22] = 0; T[1, 1, 22] = -1; T[1, 2, 22] = 0;
            T[2, 0, 22] = 1; T[2, 1, 22] = 0; T[2, 2, 22] = 0;

            // Define the remaining symmetry matrices similarly for indices 2 to 23

            T[0, 0, 23] = -1; T[0, 1, 23] = 0; T[0, 2, 23] = 0;
            T[1, 0, 23] = 0; T[1, 1, 23] = 0; T[1, 2, 23] = -1;
            T[2, 0, 23] = 0; T[2, 1, 23] = -1; T[2, 2, 23] = 0;

            // Finding the 24 misorientation matrices (also can be calculated for 576 matrices)
            double[,] m = new double[3, 3];
            double[] theta = new double[24];
            double[] minRotationAxis = new double[3];
            double minTheta = double.MaxValue;
            double[,] minRotationAxisMatrix = new double[3, 1];
            double[,] minRotationAxis_g1 = new double[3, 1];
            for (int i = 0; i < 24; i++)
            {
                double[,] T1I = new double[3, 3];
                T1I[0, 0] = T[0, 0, i]; T1I[0, 1] = T[0, 1, i]; T1I[0, 2] = T[0, 2, i];
                T1I[1, 0] = T[1, 0, i]; T1I[1, 1] = T[1, 1, i]; T1I[1, 2] = T[1, 2, i];
                T1I[2, 0] = T[2, 0, i]; T1I[2, 1] = T[2, 1, i]; T1I[2, 2] = T[2, 2, i];

                
                double[,] g1I = MatrixMultiply(T1I, g1); //SYSMETRIC
                double[,] inverseTg1 = InverseMatrix(g1I);
                m = MatrixMultiply(inverseTg1, g2);

                double t1 = m[0, 0];
                double t2 = m[1, 1];
                double t3 = m[2, 2];
                double currentTheta = Math.Acos(0.5 * (t1 + t2 + t3 - 1));

                if (currentTheta < minTheta)
                {
                    minTheta = currentTheta;
                    minRotationAxis = GetRotationAxis(m);
                    for (int j = 0; j < 3; j++)
                    {
                        minRotationAxisMatrix[j, 0] = minRotationAxis[j];
                    }

                    minRotationAxis_g1 = MatrixMultiply(g1I, minRotationAxisMatrix);

                    // Convert the axis (array) to a readable string format
                    string axisString = $"[{minRotationAxis_g1[0,0]}, {minRotationAxis_g1[1,0]}, {minRotationAxis_g1[2,0]}]";
                }
            }
            double ansTheta = minTheta * 180 / Math.PI;
            return new double[] { ansTheta, minRotationAxis_g1[0, 0], minRotationAxis_g1[1,0], minRotationAxis_g1[2,0] }; ;
        }

        public static string MatrixToString(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            string result = "";

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result += matrix[i, j].ToString() + "\t";
                }
                result += Environment.NewLine;
            }

            return result;
        }

        public static double[] GetRotationAxis(double[,] delta_rot)
        {

            // Extract the axis from the rotation matrix (delta_rot)
            double rx = delta_rot[1, 2] - delta_rot[2, 1];
            double ry = delta_rot[2, 0] - delta_rot[0, 2];
            double rz = delta_rot[0, 1] - delta_rot[1, 0];

            double[] axis = { rx, ry, rz };

            // Normalize the axis vector
            double norm = Math.Sqrt(rx * rx + ry * ry + rz * rz);
            if (norm > 0)
            {
                axis[0] /= norm;
                axis[1] /= norm;
                axis[2] /= norm;
            }

            return axis;
        }




        //BELOW IS OLD METHOD WORKS WHEN PHI<90

        //public static double MisorientationAngle(double a1, double a2, double a3, double a4, double a5, double a6)
        //{
        //    // Define the Euler angles of the two grains
        //    double[] euler1 = EulerAngleConverter(a1, a2, a3); // In degrees
        //    double[] euler2 = EulerAngleConverter(a4, a5, a6);

        //    // Convert the Euler angles to rotation matrices
        //    double[,] rotMat1 = EulerToRotationMatrix(euler1);
        //    double[,] rotMat2 = EulerToRotationMatrix(euler2);

        //    // Calculate the misorientation angle between the two grains

        //    double misorientationAngle = MisorientationAngle(rotMat1, rotMat2);
        //    return misorientationAngle;
        //    }

        //// Convert Euler angles to a rotation matrix
        //public static double[,] EulerToRotationMatrix(double[] euler)
        //{
        //    double phi = euler[0] * Math.PI / 180;
        //    double theta = euler[1] * Math.PI / 180;
        //    double psi = euler[2] * Math.PI / 180;

        //    double[,] rotMat = new double[3, 3];

        //    rotMat[0, 0] = Math.Cos(phi) * Math.Cos(psi) - Math.Sin(phi) * Math.Cos(theta) * Math.Sin(psi);
        //    rotMat[0, 1] = Math.Sin(phi) * Math.Cos(psi) + Math.Cos(phi) * Math.Cos(theta) * Math.Sin(psi);
        //    rotMat[0, 2] = Math.Sin(theta) * Math.Sin(psi);

        //    rotMat[1, 0] = -Math.Cos(phi) * Math.Sin(psi) - Math.Sin(phi) * Math.Cos(theta) * Math.Cos(psi);
        //    rotMat[1, 1] = -Math.Sin(phi) * Math.Sin(psi) + Math.Cos(phi) * Math.Cos(theta) * Math.Cos(psi);
        //    rotMat[1, 2] = Math.Sin(theta) * Math.Cos(psi);

        //    rotMat[2, 0] = Math.Sin(phi) * Math.Sin(theta);
        //    rotMat[2, 1] = -Math.Cos(phi) * Math.Sin(theta);
        //    rotMat[2, 2] = Math.Cos(theta);

        //    return rotMat;
        //}

        //// Calculate the misorientation angle between two rotation matrices
        //public static double MisorientationAngle(double[,] rotMat1, double[,] rotMat2)
        //{
        //    double[,] diffRotMat = MatrixMultiply(rotMat2, MatrixTranspose(rotMat1));
        //    double trace = diffRotMat[0, 0] + diffRotMat[1, 1] + diffRotMat[2, 2];
        //    double angle = Math.Acos((trace - 1) / 2) * 180 / Math.PI;
        //    return angle;
        //}

        //// Multiply two matrices


        //// Transpose a matrix
        //public static double[,] MatrixTranspose(double[,] matrix)
        //{
        //    int rows = matrix.GetLength(0);
        //    int cols = matrix.GetLength(1);

        //    double[,] transpose = new double[cols, rows];

        //    for (int i = 0; i < rows; i++)
        //    {
        //        for (int j = 0; j < cols; j++)
        //        {
        //            transpose[j, i] = matrix[i, j];
        //        }
        //    }

        //    return transpose;
        //}

        ////convert Euler angles in the range [0, 2π) to the range [0, π/2]  !!!wrong , from chatgpt
        //public static double[] EulerAngleConverter(double phi1, double theta, double phi2)
        //{
        //    // Example Euler angles (in degree)
        //    // Convert Euler angles to the range [0, π/2]
        //    double[] result_M = new double[3];
        //    phi1 = phi1 % (360);
        //    if (phi1 < 0)
        //        phi1 += 360;

        //    if (phi1 > 180)
        //        phi1 = 360 - phi1;


        //      phi2 = phi2 % (360);
        //    if (phi2 < 0)
        //        phi2 += 360;

        //    if (phi2 > 180)
        //        phi2 = 360 - phi2;

        //    if (theta > 180 / 2)
        //        theta =180 - theta;


        //    result_M[0] = phi1;
        //    result_M[1] = theta;
        //    result_M[2] = phi2;
        //    return result_M;

        //}


    }


    class SigmaCSL
    {
        static bool AreAxesEquivalent(double[] axis2, double[] axis1, double angleThreshold = 2.0)
        {
            // Normalize both axes
            Normalize(axis1);
            Normalize(axis2);

            // Generate all permutations with sign changes of axis1 to account for Cubic symmetry
            var axisPermutations = GenerateAxisPermutations(axis1);

            // Check if axis2 is equivalent to any of the permutations of axis1 within the specified angle threshold
            foreach (var perm in axisPermutations)
            {
                // Calculate the cosine of the angle between the two vectors
                double cosAngle = DotProduct(perm, axis2);

                // Ensure the value is within the valid range for arccos
                cosAngle = Clamp(cosAngle, -1.0, 1.0);

                // Calculate the angle in degrees
                double angle = Math.Acos(cosAngle) * (180 / Math.PI);

                // Check if the angle is less than the threshold
                if (angle <= angleThreshold || (180 - angle) <= angleThreshold)
                {
                    MessageBox.Show($"Angle between Axis and CSL Axis {string.Join(",", perm)}: {angle:F2}°");
                    return true;
                }
            }

            return false;
        }
        public static double Clamp(double value, double min, double max)
        {
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }

        public static (bool isSigma, string sigmaValue, double deviation) IsSigmaBoundary(double[] axis, double misorientationAngle, string structureType = "FCC", double angleThreshold = 10.0)
        {
            // Predefined Σ values with corresponding rotation axes and misorientation angles for both FCC and BCC
            var sigmaData = new Dictionary<string, Dictionary<string, List<(double[] axis, double angle)>>>()
            {
                ["3"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 60) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 60) }
                },
                ["5"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 0, 0 }, 36.87) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 0, 0 }, 36.87) }
                },
                ["7"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 38.21) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 38.21) }
                },
                ["9"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 0 }, 38.94) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 0 }, 38.94) }
                },
                ["11"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 0 }, 50.48) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 0 }, 50.48) }
                },
                ["13a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 0, 0 }, 22.62) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 0, 0 }, 22.62) }
                },
                ["13b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 27.79) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 27.79) }
                },
                ["15"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 2, 1, 0 }, 48.2) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 2, 1, 0 }, 48.2) }
                },
                ["17a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 0, 0 }, 28.07) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 0, 0 }, 28.07) }
                },
                ["17b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 2, 2, 1 }, 61.9) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 2, 2, 1 }, 61.9) }
                },
                ["19a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 0 }, 26.53) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 0 }, 26.53) }
                },
                ["19b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 46.8) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 46.8) }
                },
                ["21a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 21.79) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 21.79) }
                },
                ["21b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 2, 1, 1 }, 44.4) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 2, 1, 1 }, 44.4) }
                },
                ["23"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 3, 1, 1 }, 40.5) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 3, 1, 1 }, 40.5) }
                },
                ["25a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 0, 0 }, 16.3) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 0, 0 }, 16.3) }
                },
                ["25b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 3, 3, 1 }, 51.7) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 3, 3, 1 }, 51.7) }
                },
                ["27a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 0 }, 31.59) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 0 }, 31.59) }
                },
                ["27b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 2, 1, 0 }, 35.43) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 2, 1, 0 }, 35.43) }
                },
                ["29a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 0, 0 }, 43.6) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 0, 0 }, 43.6) }
                },
                ["29b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 2, 2, 1 }, 46.4) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 2, 2, 1 }, 46.4) }
                },
                ["31a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 17.9) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 17.9) }
                },
                ["31b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 2, 1, 1 }, 52.2) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 2, 1, 1 }, 52.2) }
                },
                ["33a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 0 }, 20.0) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 0 }, 20.0) }
                },
                ["33b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 3, 1, 1 }, 33.6) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 3, 1, 1 }, 33.6) }
                },
                ["33c"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 0 }, 59.0) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 0 }, 59.0) }
                },
                ["35a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 2, 1, 1 }, 34.0) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 2, 1, 1 }, 34.0) }
                },
                ["35b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 3, 3, 1 }, 43.2) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 3, 3, 1 }, 43.2) }
                },
                ["37a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 0, 0 }, 18.9) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 0, 0 }, 18.9) }
                },
                ["37b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 3, 1, 0 }, 43.1) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 3, 1, 0 }, 43.1) }
                },
                ["37c"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 50.6) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 50.6) }
                },
                ["39a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 32.2) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 32.2) }
                },
                ["39b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 3, 2, 1 }, 50.1) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 3, 2, 1 }, 50.1) }
                },
                ["41a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 0, 0 }, 12.7) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 0, 0 }, 12.7) }
                },
                ["41b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 2, 1, 0 }, 40.9) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 2, 1, 0 }, 40.9) }
                },
                ["41c"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 0 }, 55.9) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 0 }, 55.9) }
                },
                ["43a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 15.2) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 15.2) }
                },
                ["43b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 2, 1, 0 }, 27.9) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 2, 1, 0 }, 27.9) }
                },
                ["43c"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 3, 3, 2 }, 60.8) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 3, 3, 2 }, 60.8) }
                },
                ["45a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 3, 1, 1 }, 28.6) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 3, 1, 1 }, 28.6) }
                },
                ["45b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 2, 2, 1 }, 36.9) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 2, 2, 1 }, 36.9) }
                },
                ["45c"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 2, 2, 1 }, 53.1) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 2, 2, 1 }, 53.1) }
                },
                ["47a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 3, 3, 1 }, 37.1) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 3, 3, 1 }, 37.1) }
                },
                ["47b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 3, 2, 0 }, 43.7) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 3, 2, 0 }, 43.7) }
                },
                ["49a"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 43.6) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 1, 1, 1 }, 43.6) }
                },
                ["49b"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 5, 1, 1 }, 43.6) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 5, 1, 1 }, 43.6) }
                },
                ["49c"] = new Dictionary<string, List<(double[], double)>>
                {
                    ["FCC"] = new List<(double[], double)> { (new double[] { 3, 2, 2 }, 49.2) },
                    ["BCC"] = new List<(double[], double)> { (new double[] { 3, 2, 2 }, 49.2) }
                }


                // Add more Σ values as needed
            };

            Normalize(axis);

            foreach (var sigma in sigmaData)
            {
                if (!sigma.Value.ContainsKey(structureType)) continue;

                foreach (var config in sigma.Value[structureType])
                {
                    var predefinedAxis = config.axis;

                    // Check if the given axis is equivalent to the predefined axis
                    if (AreAxesEquivalent(axis, predefinedAxis, angleThreshold))
                    {
                        double predefinedAngle = config.angle;

                        // Calculate the deviation
                        double deviation = Math.Abs(misorientationAngle - predefinedAngle);

                        // Calculate the Brandon criterion threshold
                        double brandonThreshold = 15 / Math.Sqrt(double.Parse(sigma.Key.TrimEnd('a', 'b', 'c')));

                        // Check if the misorientation angle matches within the Brandon criterion threshold
                        if (deviation <= brandonThreshold)
                        {
                            return (true, sigma.Key, deviation);
                        }
                    }
                }
            }

            return (false, null, 0);
        }

        static void Normalize(double[] vector)
        {
            double magnitude = Math.Sqrt(vector.Sum(x => x * x));
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] /= magnitude;
            }
        }

        static double DotProduct(double[] v1, double[] v2)
        {
            return v1.Zip(v2, (x, y) => x * y).Sum();
        }

        static List<double[]> GenerateAxisPermutations(double[] axis)
        {
            var permutations = new List<double[]>();

            double[][] signCombinations = new double[][]
            {
            new double[] { 1, 1, 1 }, new double[] { -1, 1, 1 }, new double[] { 1, -1, 1 }, new double[] { 1, 1, -1 },
            new double[] { -1, -1, 1 }, new double[] { -1, 1, -1 }, new double[] { 1, -1, -1 }, new double[] { -1, -1, -1 }
            };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < signCombinations.Length; j++)
                {
                    double[] permutation = new double[3];
                    for (int k = 0; k < 3; k++)
                    {
                        permutation[k] = axis[(k + i) % 3] * signCombinations[j][k];
                    }
                    permutations.Add(permutation);
                }
            }

            return permutations;
        }
    }


    class imagework
    {
        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);
        /// <summary>
        /// Bitmap->BitmapSource
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapSource getBitMapSourceFromBitmap(Bitmap bitmap)
        {
            IntPtr intPtrl = bitmap.GetHbitmap();
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(intPtrl,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(intPtrl);
            return bitmapSource;
        }



    }
}
