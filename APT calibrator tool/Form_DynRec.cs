using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
using System.IO;
using SharpGL.SceneGraph;

namespace APT_calibrator_tool
{
    public partial class Form_DynRec : Form
    {
        // 存储三维点数据和颜色
        private List<Point3DWithColor> points = new List<Point3DWithColor>();
        private float zoom = -150f;  // 相机缩放变量
        private float rotationX = 0.0f;
        private float rotationY = 0.0f;
        private float translateX = 0.0f;
        private float translateY = 0.0f;
        private PictureBox pictureBox;
        private Bitmap scatterPlotBitmap;
        private Bitmap densityMapBitmap;
        private Point lastMousePosition;
        private bool isShiftPressed = false;
        private float[] x;
        private float[] y;
        private float[] z;
        private float[] m;
        public Form_DynRec()
        {
            InitializeComponent();
            openGLControl.MouseDown += openGLControl_MouseDown;
            openGLControl.MouseMove += openGLControl_MouseMove;
            openGLControl.MouseUp += openGLControl_MouseUp;
            openGLControl.MouseEnter += (s, ev) => openGLControl.Focus();  // 确保焦点
        }

        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            lastMousePosition = e.Location;
            isShiftPressed = ModifierKeys.HasFlag(Keys.Shift);
        }

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int deltaX = e.X - lastMousePosition.X;
                int deltaY = e.Y - lastMousePosition.Y;

                if (isShiftPressed)
                {
                    translateX += deltaX * 0.01f;
                    translateY -= deltaY * 0.01f;
                }
                else
                {
                    rotationX += deltaY * 0.5f;
                    rotationY += deltaX * 0.5f;
                }

                lastMousePosition = e.Location;
                openGLControl.Invalidate();
            }
        }

        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            // 鼠标释放时的逻辑（如果需要）
        }
        public class SavePos
        {
            [STAThread] // Required for using Windows Forms dialogs
            public static void SavePosfile(float[] x, float[] y, float[] z, float[] m)
            {
                if (x.Length != y.Length || y.Length != z.Length || z.Length != m.Length)
                {
                    MessageBox.Show("All input arrays must have the same length.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Show SaveFileDialog to select file path
                string fileName = SelectFilePath();
                if (string.IsNullOrEmpty(fileName))
                {
                    MessageBox.Show("File saving canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int nb = x.Length;

                // Open the file with FileStream in write mode
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        // Write the data as big-endian (by reversing bytes for each float)
                        for (int i = 0; i < nb; i++)
                        {
                            WriteBigEndian(writer, x[i]);
                            WriteBigEndian(writer, y[i]);
                            WriteBigEndian(writer, z[i]);
                            WriteBigEndian(writer, m[i]);
                        }
                    }
                }

                MessageBox.Show("File written.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Helper function to write float in big-endian format
            private static void WriteBigEndian(BinaryWriter writer, float value)
            {
                byte[] bytes = BitConverter.GetBytes(value);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }

                writer.Write(bytes);
            }

            // Function to select file path using SaveFileDialog
            private static string SelectFilePath()
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Position Files (*.pos)|*.pos|All Files (*.*)|*.*";
                    saveFileDialog.Title = "Select File Location";
                    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    // Show the dialog and return the selected path if confirmed
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        return saveFileDialog.FileName;
                    }
                }

                return null; // Return null if no file was selected
            }

        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "POS files (*.pos)|*.pos|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = openFileDialog.FileName;
                    try
                    {
                        // Call ReadPos and get the Tuple result
                        Tuple<float[], float[], float[], float[]> result = ReadPos(fileName);

                        // Unpack the Tuple
                         x = result.Item1;
                         y = result.Item2;
                         z = result.Item3;
                         m = result.Item4;

                        // Populate the ListView with the unpacked arrays
                        //PopulateListView(x, y, z, m);

                        MessageBox.Show("File successfully read!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadPointData(x, y, z, m);
                        

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }



        private void LogMessage(string message)
        {
            //textBox_message.AppendText(message + Environment.NewLine);
        }



        private void PopulateListView(float[] x, float[] y, float[] z, float[] m)
        {
            listViewData.Items.Clear();
            for (int i = 0; i < x.Length; i++)
            {
                ListViewItem item = new ListViewItem(x[i].ToString());
                item.SubItems.Add(y[i].ToString());
                item.SubItems.Add(z[i].ToString());
                item.SubItems.Add(m[i].ToString());
                listViewData.Items.Add(item);
            }
        }

        // Method to read the .pos file
    public static Tuple<float[], float[], float[], float[]> ReadPos(string fileName)
    {
        try
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                long fileSize = fs.Length;
                int recordCount = (int)(fileSize / (4 * sizeof(float)));

                float[] x = new float[recordCount];
                float[] y = new float[recordCount];
                float[] z = new float[recordCount];
                float[] m = new float[recordCount];

                for (int i = 0; i < recordCount; i++)
                {
                    x[i] = ReadBigEndianFloat(br);
                    y[i] = ReadBigEndianFloat(br);
                    z[i] = ReadBigEndianFloat(br);
                    m[i] = ReadBigEndianFloat(br);
                }

                // Return a Tuple containing all four arrays
                return Tuple.Create(x, y, z, m);
  
            }
            }
        catch (Exception ex)
        {
            // Log or handle exceptions
           // Console.WriteLine("Error reading file: {ex.Message}");
            return Tuple.Create(new float[0], new float[0], new float[0], new float[0]);
        }
        }

        // Helper method for big-endian float reading
        private static float ReadBigEndianFloat(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(4);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToSingle(bytes, 0);
        }




        private void ReadEposReconInside(string file_name, out float[] m, out float[] vt, out float[] dx, out float[] dy)
        {
            try
            {
                // Opens the file
                using (FileStream fs = new FileStream(file_name, FileMode.Open, FileAccess.Read))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    // Reads through the file made of 9 floats separated by 8 (2 integers) bytes
                    float[] lflo = new float[0];
                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {
                        float[] temp = new float[9];
                        for (int i = 0; i < 9; i++)
                        {
                            temp[i] = br.ReadSingle();
                        }
                        lflo = ConcatenateArrays(lflo, temp);
                    }
                    int nb = lflo.Length / 9;

                    // Makes an array with the list of floats
                    float[,] flo = new float[9, nb];
                    int count = 0;
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < nb; j++)
                        {
                            flo[i, j] = lflo[count++];
                        }
                    }

                    // Creates output
                    m = GetColumn(flo, 3);
                    vt = AddArrays(GetColumn(flo, 5), GetColumn(flo, 6));
                    dx = GetColumn(flo, 7);
                    Random rdx = new Random();
                    for (int i = 0; i < dx.Length; i++)
                    {
                        dx[i] = dx[i] + (float)((rdx.NextDouble() - 0.5) * 0.05);
                    }

                    dy = GetColumn(flo, 8);
                    Random rdy = new Random();
                    for (int i = 0; i < dy.Length; i++)
                    {
                        dy[i] = dy[i] + (float)((rdy.NextDouble() - 0.5) * 0.05);
                    }

                    Console.WriteLine("File read successfully.");
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"An error occurred: {ex.Message}");
                m = vt = dx = dy = null;
            }
        }

        private T[] ConcatenateArrays<T>(T[] arr1, T[] arr2)
        {
            T[] result = new T[arr1.Length + arr2.Length];
            arr1.CopyTo(result, 0);
            arr2.CopyTo(result, arr1.Length);
            return result;
        }

        private T[] GetColumn<T>(T[,] matrix, int columnIndex)
        {
            int rows = matrix.GetLength(0);
            T[] column = new T[rows];
            for (int i = 0; i < rows; i++)
            {
                column[i] = matrix[i, columnIndex];
            }
            return column;
        }

        private T[] AddArrays<T>(T[] arr1, T[] arr2)
        {
            if (arr1.Length != arr2.Length)
            {
                throw new ArgumentException("Arrays must be of the same length");
            }

            T[] result = new T[arr1.Length];
            for (int i = 0; i < arr1.Length; i++)
            {
                dynamic val1 = arr1[i];
                dynamic val2 = arr2[i];
                result[i] = val1 + val2;
            }
            return result;
        }

        // 读取文件后调用的函数，用于处理 (x, y, z, m) 数据
        private void LoadPointData(float[] x, float[] y, float[] z, float[] m)
        {
            // 清除现有数据
            points.Clear();
            Random random = new Random();
            float minX = x.Min();
            float maxX = x.Max();
            float minY = y.Min();
            float maxY = y.Max();
            float minZ = z.Min();
            float maxZ = z.Max();
            float centerX = (minX + maxX) / 2.0f;
            float centerY = (minY + maxY) / 2.0f;
            float centerZ = (minZ + maxZ) / 2.0f;

            for (int i = 0; i < x.Length; i++)
            {
                // 根据 m 值设置颜色
                Color color = GetColorFromM(m[i]);


                 // 以10%的概率显示点
                 if (random.NextDouble() <= 0.1)  // 0.1 表示 10%
                 {
                     // 将点和颜色加入列表
                     points.Add(new Point3DWithColor(x[i], y[i], z[i], color));
                 }
            }

            // 触发 OpenGL 绘制
            openGLControl.Invalidate();
            translateX = centerX;
            translateY = centerY;
        }

        // 根据 m 值生成颜色，可以根据你的需求调整颜色映射规则
        private Color GetColorFromM(float m)
        {
            if (m < 0.33f)
                return Color.Red;
            else if (m < 0.66f)
                return Color.Green;
            else
                return Color.Blue;
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            // 根据鼠标滚轮调整 zoom 变量
            if (e.Delta > 0)
                zoom += 2.0f;
            else
                zoom -= 2.0f;

            // 触发重新绘制
            openGLControl.Invalidate();
        }

        private void openGLControl_MouseWheel(object sender, MouseEventArgs e)
        {
            // 根据滚轮滚动调整 zoom 值
            if (e.Delta > 0)
            {
                zoom += 2.0f;  // 向前滚动时放大
            }
            else
            {
                zoom -= 2.0f;  // 向后滚动时缩小
            }

            // 触发重新绘制
            openGLControl.Invalidate();
        }



        private void DrawSizeBar(OpenGL gl, float size, float x, float y, float z)
        {
            // 设置颜色为白色
            gl.Color(1.0f, 1.0f, 1.0f); // 白色

            // 绘制标尺
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(x, y, z);
            gl.Vertex(x, y, z+ size); // 线段结束位置
            gl.End();

            // 绘制尺寸标注
            DrawDimensionLabel(gl, size.ToString(), x + size / 2, y + 0.2f, z); // 标注在中间偏上位置
        }

        private void DrawDimensionLabel(OpenGL gl, string text, float x, float y, float z)
        {
            gl.PushMatrix();
            gl.Translate(x, y, z);

            // 设置文本颜色
            gl.Color(1.0f, 1.0f, 1.0f); // 白色

            // 在这里实现文本绘制逻辑
           // DrawText(gl, text);

            gl.PopMatrix();
        }




        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            OpenGL gl = openGLControl.OpenGL;

            // 清除颜色和深度缓冲区
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity(); // Reset the modelview matrix
            // 应用平移
            gl.Translate(translateX-60f, translateY, zoom);

            // 应用旋转
            gl.Rotate(rotationX, 1.0f, 0.0f, 0.0f);
            gl.Rotate(rotationY+90f, 0.0f, 1.0f, 0.0f);
            DrawSizeBar(gl, 50.0f, 0.0f, 50.0f, 0.0f);


            // 设置随机数生成器
            Random random = new Random();

            // 开始绘制三维点
            gl.Begin(OpenGL.GL_POINTS);

            foreach (var point in points)
            {

                    gl.Color(point.Color.R / 255.0f, point.Color.G / 255.0f, point.Color.B / 255.0f);
                    gl.Vertex(point.X, point.Y, point.Z);
            }

            gl.End();
            gl.Flush();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openGLControl.Focus();  // 强制获取焦点
        }

        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;

            // 设置清除颜色为黑色
            gl.ClearColor(0, 0, 0, 0);

            // 启用深度测试
            gl.Enable(OpenGL.GL_DEPTH_TEST);
        }

        private void openGLControl_Resized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;

            // 设置视口
            gl.Viewport(0, 0, openGLControl.Width, openGLControl.Height);

            // 设置投影矩阵
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Perspective(45.0f, (double)openGLControl.Width / (double)openGLControl.Height, 0.1, 1000.0);

            // 设置模型视图矩阵
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();



        }

        // 用于存储带颜色的三维点
        private class Point3DWithColor
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
            public Color Color { get; set; }

            public Point3DWithColor(float x, float y, float z, Color color)
            {
                X = x;
                Y = y;
                Z = z;
                Color = color;
            }
        }

        private void pole_calc_Click(object sender, EventArgs e)
        {
            // 生成图像
            LoadDataAndGeneratePlots();

            }

        private void LoadDataAndGeneratePlots()
        {
            // 检查数组长度是否足够
            if (x.Length < 3000000 || y.Length < 4000000)
            {
                MessageBox.Show("数据长度不足，无法提取指定范围的子数组。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 提取子数组，等效于 MATLAB 代码 X=x(210000000:230000000);
            int startIndex = 3000000;
            int endIndex = 4000000;
            int length = endIndex - startIndex;

            float[] X = new float[length];
            float[] Y = new float[length];

            Array.Copy(x, startIndex, X, 0, length);
            Array.Copy(y, startIndex, Y, 0, length);

            // 计算密度最高的位置
            var maxDensityPositions = DensityAnalysis.FindMaxDensityPositions(X.ToList(), Y.ToList(), 20, 2);

            // 生成散点图和密度图的 Bitmap
            scatterPlotBitmap = PlotHelper.GenerateScatterPlotBitmap(maxDensityPositions);
            densityMapBitmap = PlotHelper.GenerateDensityMapBitmap(maxDensityPositions);

            // 显示散点图或密度图
            ShowScatterPlot(); // 或者调用 ShowDensityMap();
        }

        private void ShowScatterPlot()
        {
            pictureBox1.Image = scatterPlotBitmap;
        }

        private void ShowDensityMap()
        {
            pictureBox1.Image = densityMapBitmap;
        }

        private void btn_xy_Click(object sender, EventArgs e)
        {
            // 设置摄像机位置
            rotationY=0;
            rotationX = 0f;
            openGLControl.Invalidate();

        }

        private void btn_yz_Click(object sender, EventArgs e)
        {
            // 设置摄像机位置
            rotationX = 90f;
            rotationY = 0;
            openGLControl.Invalidate();

        }

        private void btn_xz_Click(object sender, EventArgs e)
        {
            translateX = 60f;
            // 设置摄像机位置
            rotationY = 90f;
            rotationX = 0f;
            openGLControl.Invalidate();
        }

        private void Save_New_file(object sender, EventArgs e)
        {
            SavePos.SavePosfile(x, y, z, m);
        }

        private void Re_recon(object sender, EventArgs e)
        {
            // Parsing the text values to float
            float ICFscaling = Convert.ToSingle(this.tb_ICFscaling.Text);
            float Kfscaling = Convert.ToSingle(this.tb_Kfscaling.Text);

            // Assuming x, y, z are arrays
            int length = x.Length; // Assuming x, y, and z have the same length
            float minZ = z.Min(); // Get minimum value of z
            float maxZ = z.Max(); // Get maximum value of z

            // Loop through the elements
            for (int i = 0; i < length; i++)
            {
                // Scale calculations
                float ICFscale = 1+(ICFscaling-1) * ((z[i] - minZ) / (maxZ - minZ));
                float Kfscale = 1+(Kfscaling-1) * ((z[i] - minZ) / (maxZ - minZ));

                // Update the x, y, z arrays
                x[i] = x[i] * ICFscale;
                y[i] = y[i] * ICFscale;
                z[i] = Kfscale * Kfscale * z[i] / (ICFscale * ICFscale);
            }
            MessageBox.Show("Done", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadPointData(x, y, z, m);
        }

        private void slicex(object sender, EventArgs e)
        {
            // Parsing the text values to float
            float xslice = Convert.ToSingle(tb_slicex.Text);
            float width = Convert.ToSingle(tb_slicewidth.Text);
            int length = x.Length; // Assuming x, y, z, and m have the same length

            // Initialize lists to store the sliced data
            List<float> xs = new List<float>();
            List<float> ys = new List<float>();
            List<float> zs = new List<float>();
            List<float> ms = new List<float>();

            // Loop through the elements
            for (int i = 0; i < length; i++)
            {
                if (x[i] > xslice && x[i] < xslice + width) // Add parentheses around condition
                {
                    // Add the values to the new sliced lists
                    xs.Add(x[i]);
                    ys.Add(y[i]);
                    zs.Add(z[i]);
                    ms.Add(m[i]);
                }
            }

            // Show message once the operation is complete
            MessageBox.Show("Done", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Load the sliced data
            LoadPointData(xs.ToArray(), ys.ToArray(), zs.ToArray(), ms.ToArray());
            rotationY = 0f;
            rotationX = 0f;
            openGLControl.Invalidate();
        }
        private void slicey(object sender, EventArgs e)
        {
            // Parsing the text values to float
            float yslice = Convert.ToSingle(tb_slicey.Text);
            float width = Convert.ToSingle(tb_slicewidth.Text);
            int length = x.Length; // Assuming x, y, z, and m have the same length

            // Initialize lists to store the sliced data
            List<float> xs = new List<float>();
            List<float> ys = new List<float>();
            List<float> zs = new List<float>();
            List<float> ms = new List<float>();

            // Loop through the elements
            for (int i = 0; i < length; i++)
            {
                if (y[i] > yslice && y[i] < yslice + width) // Add parentheses around condition
                {
                    // Add the values to the new sliced lists
                    xs.Add(x[i]);
                    ys.Add(y[i]);
                    zs.Add(z[i]);
                    ms.Add(m[i]);
                }
            }

            // Show message once the operation is complete
            MessageBox.Show("Done", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Load the sliced data
            LoadPointData(xs.ToArray(), ys.ToArray(), zs.ToArray(), ms.ToArray());
            rotationX = 90f;
            rotationY = 0f;
            openGLControl.Invalidate();
        }
        private void slicez(object sender, EventArgs e)
        {
            // Parsing the text values to float
            float zslice = Convert.ToSingle(tb_slicez.Text);
            float width = Convert.ToSingle(tb_slicewidth.Text);
            int length = x.Length; // Assuming x, y, z, and m have the same length

            // Initialize lists to store the sliced data
            List<float> xs = new List<float>();
            List<float> ys = new List<float>();
            List<float> zs = new List<float>();
            List<float> ms = new List<float>();

            // Loop through the elements
            for (int i = 0; i < length; i++)
            {
                if (z[i] > zslice && z[i] < zslice + width) // Add parentheses around condition
                {
                    // Add the values to the new sliced lists
                    xs.Add(x[i]);
                    ys.Add(y[i]);
                    zs.Add(z[i]);
                    ms.Add(m[i]);
                }
            }

            // Show message once the operation is complete
            MessageBox.Show("Done", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Load the sliced data
            LoadPointData(xs.ToArray(), ys.ToArray(), zs.ToArray(), ms.ToArray());
            translateX = 60f;
            rotationY = 90f;
            rotationX = 0f;
            openGLControl.Invalidate();
        }

        private void ShowAll(object sender, EventArgs e)
        {

                // Load the sliced data
                LoadPointData(x, y, z, m);
                openGLControl.Invalidate();

        }
    }

    public static class DensityAnalysis
    {
        public static List<PointF> FindMaxDensityPositions(List<float> X, List<float> Y, int N, int threshold)
        {
            List<PointF> maxDensityPositions = new List<PointF>();
            int segmentSize = 15;
            int numPoints = X.Count;

            for (int i = 0; i < numPoints; i += segmentSize)
            {
                int endIdx = Math.Min(i + segmentSize - 1, numPoints - 1);
                List<float> xSegment = X.GetRange(i, endIdx - i + 1);
                List<float> ySegment = Y.GetRange(i, endIdx - i + 1);

                PointF? maxPoint = FindMaxDensityGrid(xSegment, ySegment, N, threshold);
                if (maxPoint != null)
                {
                    maxDensityPositions.Add(maxPoint.Value);
                }
            }

            return maxDensityPositions;
        }

        private static PointF? FindMaxDensityGrid(List<float> X, List<float> Y, int N, int threshold)
        {
            float xMin = X.Min();
            float xMax = X.Max();
            float yMin = Y.Min();
            float yMax = Y.Max();

            float[,] density = new float[N, N];
            float xStep = (xMax - xMin) / N;
            float yStep = (yMax - yMin) / N;

            for (int i = 0; i < X.Count; i++)
            {
                int xIdx = Math.Min((int)((X[i] - xMin) / xStep), N - 1);
                int yIdx = Math.Min((int)((Y[i] - yMin) / yStep), N - 1);
                density[yIdx, xIdx]++;
            }

            float maxDensity = 0;
            int maxXIdx = -1, maxYIdx = -1;

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (density[i, j] > maxDensity && density[i, j] >= threshold)
                    {
                        maxDensity = density[i, j];
                        maxXIdx = j;
                        maxYIdx = i;
                    }
                }
            }

            if (maxXIdx != -1 && maxYIdx != -1)
            {
                float centerX = xMin + (maxXIdx + 0.5f) * xStep;
                float centerY = yMin + (maxYIdx + 0.5f) * yStep;
                return new PointF(centerX, centerY);
            }

            return null;
        }
    }

    public static class PlotHelper
    {
        public static Bitmap GenerateScatterPlotBitmap(List<PointF> points)
        {
            int width = 800;
            int height = 600;
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                foreach (var point in points)
                {
                    g.FillEllipse(Brushes.Blue, point.X, point.Y, 4, 4);
                }
            }
            return bitmap;
        }

        public static Bitmap GenerateDensityMapBitmap(List<PointF> points, int numBins = 100)
        {
            int width = 800;
            int height = 600;
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);

                int[,] density = new int[numBins, numBins];
                float xMin = points.Min(p => p.X);
                float xMax = points.Max(p => p.X);
                float yMin = points.Min(p => p.Y);
                float yMax = points.Max(p => p.Y);

                float xStep = (xMax - xMin) / numBins;
                float yStep = (yMax - yMin) / numBins;

                foreach (var point in points)
                {
                    int xIdx = Math.Min((int)((point.X - xMin) / xStep), numBins - 1);
                    int yIdx = Math.Min((int)((point.Y - yMin) / yStep), numBins - 1);
                    density[yIdx, xIdx]++;
                }

                for (int i = 0; i < numBins; i++)
                {
                    for (int j = 0; j < numBins; j++)
                    {
                        int intensity = Math.Min(255, density[i, j] * 10);
                        Color color = Color.FromArgb(intensity, 0, 0);
                        g.FillRectangle(new SolidBrush(color), j * (width / numBins), i * (height / numBins), width / numBins, height / numBins);
                    }
                }
            }
            return bitmap;
        }
    }

}
