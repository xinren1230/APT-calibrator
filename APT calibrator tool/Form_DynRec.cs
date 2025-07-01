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
using System.Windows.Forms.DataVisualization.Charting;
using System.Runtime.InteropServices;
using MathNet.Numerics.Interpolation;
using System.Text.RegularExpressions;



namespace APT_calibrator_tool
{
    public partial class Form_DynRec : Form
    {
        // … other fields …
        private List<double> _xValues;
        private List<double> _sqrtValues;
        private CubicSpline _icfSpline;
        private CubicSpline _kfSpline;
        private int xLength;
        // 存储每次计算出的 RMS 值
        private readonly List<double> _rmsHistory = new List<double>();
        // 每次计算的所有点偏差的平均值列表
        private readonly List<double> _avgDevs = new List<double>();



        // 翻转状态
        private bool flipX = false;
        private bool flipY = false;
        // 拖拽状态
        private bool _isDragging = false;
        private Series _activeSeries = null;
        private int _dragPointIdx = -1;
        // 存储三维点数据和颜色
        private List<Point3DWithColor> points = new List<Point3DWithColor>();
        private float zoom = -150f;  // 相机缩放变量
        private float rotationX = 0.0f;
        private float rotationY = 0.0f;
        private float translateX = 0.0f;
        private float translateY = 0.0f;
        private double[,] _TEMseedCoords;
        private double[,] _APTseedCoords;
        private CubicSpline _cubicSpline;
        // in your FormDynaReRec class:
        private double[] _lastIndicesArray;
        private double[] _lastIcfValues;
        private double[] _lastKfValues;


        public double[,] TEMSeedCoords
        {
            get => _TEMseedCoords;
            set
            {
                _TEMseedCoords = value;
                Update_TEM_Display();
            }
        }

        public double[,] APTSeedCoords
        {
            get => _APTseedCoords;
            set
            {
                _APTseedCoords = value;
                Update_APT_Display();
            }
        }

        private PictureBox pictureBox;
        private Bitmap scatterPlotBitmap;
        private Bitmap densityMapBitmap;
        private bool isPickingEnabled = false;
        private Point lastMousePosition;
        private bool isShiftPressed = false;
        // index of the point we’ve selected, or –1 if none
        private int selectedPointIdx = -1;
        // maximum distance (in world‐space units) from ray to point to count as a “hit”
        private const float PICK_TOLERANCE = 200.0f;

        private float[] x;
        private float[] y;
        private float[] z;
        private float[] m;
        private float mMin = 1f;
        private float mMax = 1f;
        private float dataCenterX, dataCenterY;
        private float dCenterX, dCenterY, dCenterZ;
        // 新增：是否根据 m 过滤，以及目标 m 值
        private bool filterEnabled = false;
        private float filterM = 0f;
        private float[] ICFscaling;
        private float[] kfscaling;
        // 类级字段，用于切换投影模式
        private bool usePerspective = false;
        private float orthoSize = 100f;       // 正交下的“视野半高”
        private readonly Random random = new Random();
        public Form_DynRec()
        {
            InitializeComponent();
            InitChart();
            btnFlipX.Click += (s, e) => { flipX = !flipX; openGLControl.Invalidate(); };
            btnFlipY.Click += (s, e) => { flipY = !flipY; openGLControl.Invalidate(); };

            // 选点模式按钮
            btnPickPoint.Click += (s, e) =>
            {
                isPickingEnabled = true;
                btnPickPoint.Text = "Click in GL…";
                openGLControl.Cursor = Cursors.Cross;
            };
            openGLControl.MouseClick += OpenGLControl_MouseClick;
            openGLControl.MouseDown += openGLControl_MouseDown;
            openGLControl.MouseMove += openGLControl_MouseMove;
            openGLControl.MouseUp += openGLControl_MouseUp;
            openGLControl.MouseWheel += openGLControl_MouseWheel;
            openGLControl.MouseEnter += (s, ev) => openGLControl.Focus();  // 确保焦点
            // —— 新增：按下回车键时刷新采样密度 ——
            tb_disPercent.KeyDown += tb_disPercent_KeyDown;
            // —— 新增：按回车触发 m 值过滤 —— 
            tb_atomM.KeyDown += tb_atomM_KeyDown;
            // —— 新增：勾选“显示所有” —— 
            checkBox_displayAll.CheckedChanged += checkBox_displayAll_CheckedChanged;

            ICFscaling = Enumerable.Range(0, 100).Select(i => 1.6f - 0.3f * i / 99f).ToArray();
            kfscaling = Enumerable.Range(0, 100).Select(i => 4.2f - 1.1f * i / 99f).ToArray();

        }


        private void btn_DevFunc_Click(object sender, EventArgs e)
        {
            var aptLines = APTtextBoxCoords.Lines;
            var temLines = TEMtextBoxCoords.Lines;

            // 1) Guard: make sure we actually have data
            int n = Math.Min(aptLines.Length, temLines.Length);
            if (n == 0)
            {
                MessageBox.Show("Please load corresponding APT and TEM coordinates first.",
                                "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2) Compute sum of squared deviations
            double sumSq = 0;
            var numberPattern = new Regex(@"X=([0-9.+-eE]+),\s*Y=([0-9.+-eE]+)");
            for (int i = 0; i < n; i++)
            {
                var mA = numberPattern.Match(aptLines[i]);
                var mT = numberPattern.Match(temLines[i]);
                if (!mA.Success || !mT.Success)
                    continue;

                double ax = double.Parse(mA.Groups[1].Value),
                       ay = double.Parse(mA.Groups[2].Value),
                       tx = double.Parse(mT.Groups[1].Value),
                       ty = double.Parse(mT.Groups[2].Value);

                double dx = ax - tx, dy = ay - ty;
                sumSq += dx * dx + dy * dy;
            }

            // 3) Compute RMS and record it
            double rms = Math.Sqrt(sumSq / n);
            _avgDevs.Add(rms);

            // 4) Pop up a chart of the history
            var popup = new Form
            {
                Width = 600,
                Height = 400,
                Text = "Mean Deviation History",
                Icon = this.Icon
            };

            var chart = new Chart { Dock = DockStyle.Fill };
            popup.Controls.Add(chart);

            // Create and add the ChartArea
            var area = new ChartArea("DevArea");
            area.AxisX.Title = "Iteration";
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 16);
            area.AxisY.Title = "RMS Deviation";
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 16);
            chart.ChartAreas.Add(area);

            // Create the series, tell it to use our area
            var series = new Series("RMS")
            {
                ChartArea = "DevArea",
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 6
            };

            for (int i = 0; i < _avgDevs.Count; i++)
                series.Points.AddXY(i + 1, _avgDevs[i]);

            chart.Series.Add(series);

            popup.Show(this);
        }

        // 辅助：从形如 "1: X=12.34, Y=56.78" 的多行文本中解析出 PointF 列表
        private List<PointF> ParseCoordsFromTextBox(TextBox tb)
        {
            var list = new List<PointF>();
            var lines = tb.Lines;
            var regex = new System.Text.RegularExpressions.Regex(@"X\s*=\s*(\-?\d+(\.\d+)?)\s*,\s*Y\s*=\s*(\-?\d+(\.\d+)?)");
            foreach (var line in lines)
            {
                var m = regex.Match(line);
                if (m.Success &&
                    float.TryParse(m.Groups[1].Value, out float x) &&
                    float.TryParse(m.Groups[3].Value, out float y))
                {
                    list.Add(new PointF(x, y));
                }
            }
            return list;
        }


        private void InitChart()
        {
            // 假设 chart1 已经放到表单上了
            chart1.Series.Clear();
            var area = new ChartArea("MainArea");
            area.CursorY.IsUserEnabled = true;  // 可选：显示 Y 轴的拖动光标
            area.CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas.Add(area);

            // 示例：两条可拖的曲线
            //var s1 = new Series("ICF") { ChartType = SeriesChartType.Line, MarkerStyle = MarkerStyle.Circle };
            //var s2 = new Series("kf") { ChartType = SeriesChartType.Line, MarkerStyle = MarkerStyle.Circle };

            // 填充示例数据
            // for (int i = 0; i < 20; i++)
            // {
            // s1.Points.AddXY(i, 1.5 + 0.1 * Math.Sin(i / 3.0));
            //s2.Points.AddXY(i, 3.5 + 0.2 * Math.Cos(i / 4.0));
            // }

            // chart1.Series.Add(s1);
            //  chart1.Series.Add(s2);

            // 鼠标事件
            chart1.MouseDown += Chart1_MouseDown;
            chart1.MouseMove += Chart1_MouseMove;
            chart1.MouseUp += Chart1_MouseUp;

        }
        private void Update_TEM_Display()
        {
            // 假设有一个 multiline TextBox 名为 textBoxCoords
            TEMtextBoxCoords.Clear();
            if (_TEMseedCoords == null)
                return;
            int rows = _TEMseedCoords.GetLength(0);
            for (int i = 0; i < rows; i++)
            {
                double x = _TEMseedCoords[i, 0];
                double y = _TEMseedCoords[i, 1];
                TEMtextBoxCoords.AppendText($"{i + 1}: X={x:F2}, Y={y:F2}{Environment.NewLine}");
            }
        }
        private void Update_APT_Display()
        {
            // 假设有一个 multiline TextBox 名为 textBoxCoords
            APTtextBoxCoords.Clear();
            if (_APTseedCoords == null)
                return;
            int rows = _APTseedCoords.GetLength(0);
            for (int i = 0; i < rows; ++i)
            {
                double x = _APTseedCoords[i, 0];
                double y = _APTseedCoords[i, 1];
                APTtextBoxCoords.AppendText($"{i + 1}: X={x:F2}, Y={y:F2}{Environment.NewLine}");
            }
        }
        private void Chart1_MouseDown(object sender, MouseEventArgs e)
        {
            // HitTest 找到鼠标下的数据点
            var result = chart1.HitTest(e.X, e.Y);
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                _activeSeries = result.Series;
                _dragPointIdx = result.PointIndex;
                _isDragging = true;
                chart1.Cursor = Cursors.Hand;
            }
        }

        private void Chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging || _activeSeries == null || _dragPointIdx < 0)
                return;

            // 将鼠标的 Y 像素坐标转为图表数据坐标
            var ca = chart1.ChartAreas[0];
            double newY = ca.AxisY.PixelPositionToValue(e.Y);

            // 可选：给新值加上下限
            var minY = ca.AxisY.Minimum;
            var maxY = ca.AxisY.Maximum;
            newY = Math.Max(minY, Math.Min(maxY, newY));

            // 更新该点的 Y 值
            _activeSeries.Points[_dragPointIdx].YValues[0] = newY;

            chart1.Invalidate();  // 刷新
        }

        private void Chart1_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
            _activeSeries = null;
            _dragPointIdx = -1;
            chart1.Cursor = Cursors.Default;
        }
    
    private void tb_disPercent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;  // 不让“咔嗒”声或系统响铃
                                            // 假如 x,y,z,m 已经被加载过了
                if (x != null && x.Length > 0)
                {
                    LoadPointData(x, y, z, m);
                }
            }
        }
        // tb_atomM: 用户按回车后，根据输入设置 filterM 并重载
        private void tb_atomM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                float val;
                if (float.TryParse(tb_atomM.Text, out val))
                {
                    filterM = val;
                    filterEnabled = true;
                    checkBox_displayAll.Checked = false;
                    if (x != null && x.Length > 0)
                        LoadPointData(x, y, z, m);
                }
                else
                {
                    MessageBox.Show("Please input valid value", "format error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        // 当用户勾上“显示所有”时，关闭 m 过滤并重载
        private void checkBox_displayAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_displayAll.Checked)
            {
                filterEnabled = false;
                if (x != null && x.Length > 0)
                    LoadPointData(x, y, z, m);
            }
        }

        private void LoadPointData(float[] x, float[] y, float[] z, float[] m)
        {
            // 更新 m 范围
            mMin = m.Min();
            mMax = m.Max();
            points.Clear();

            // 解析采样率
            float percent;
            if (!float.TryParse(tb_disPercent.Text, out percent))
                percent = 10f;
            float sampleRate = Math.Max(0f, Math.Min(100f, percent)) / 100f;

            // 计算中心用于平移
            float minX = x.Min(), maxX = x.Max();
            float minY = y.Min(), maxY = y.Max();
            float minZ = z.Min(), maxZ = z.Max();
            dataCenterX = -(minZ + maxZ) / 2f;
            dataCenterY = (minY + maxY) / 2f;
            dCenterX = -(minX + maxX) / 2f;
            dCenterY = (minY + maxY) / 2f;
            dCenterZ = (minZ + maxZ) / 2f;
            for (int i = 0; i < x.Length; i++)
            {
                // 1) 如果开启了 m 过滤，则先判断范围
                if (filterEnabled && Math.Abs(m[i] - filterM) > 0.1f)
                    continue;

                // 2) 再按照用户设定的百分比随机采样
                if (random.NextDouble() <= sampleRate)
                {
                    var color = GetColorFromM(m[i]);
                    points.Add(new Point3DWithColor(x[i], y[i], z[i], color));
                }
            }

            // 更新视图中心并触发重绘
            // 设置摄像机位置
            rotationY = 0;
            rotationX = 0f;

            translateX = dataCenterX;
            translateY = dataCenterY;
            FitView();
            openGLControl.Invalidate();
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
                    translateX += deltaX * 0.1f;
                    translateY -= deltaY * 0.1f;
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

        // =============================================================
        //                         拾  取  逻  辑
        // =============================================================
        private void OpenGLControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isPickingEnabled)
                return;

            // 1) 获取 OpenGL 上下文
            var gl = openGLControl.OpenGL;

            // 2) 读取当前模型视图矩阵、投影矩阵和视口
            float[] modelview = new float[16];
            float[] projection = new float[16];
            int[] viewport = new int[4];
            gl.GetFloat(OpenGL.GL_MODELVIEW_MATRIX, modelview);
            gl.GetFloat(OpenGL.GL_PROJECTION_MATRIX, projection);
            gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);

            // 3) 将鼠标位置转换到 OpenGL 窗口坐标系（左下原点）
            int winX = e.X;
            int winY = viewport[3] - e.Y;

            // 4) 反投影得到射线起点（near）和终点（far）
            Vector3 nearPt = UnProject(winX, winY, 0f, modelview, projection, viewport);
            Vector3 farPt = UnProject(winX, winY, 1f, modelview, projection, viewport);
            Vector3 rayDir = Vector3.Normalize(farPt - nearPt);

            // 5) 在点云中找最靠近射线的点
            float bestDist = float.MaxValue;
            int bestIdx = -1;
            for (int i = 0; i < points.Count; i++)
            {
                var p = points[i];
                // 从 nearPt 到点的向量
                Vector3 v = new Vector3(p.X, p.Y, p.Z) - nearPt;
                // 投影长度
                float t = Math.Max(0f, Vector3.Dot(v, rayDir));
                // 最近点
                Vector3 proj = nearPt + rayDir * t;
                float d = Vector3.Distance(proj, new Vector3(p.X, p.Y, p.Z));
                if (d < bestDist && d < PICK_TOLERANCE)
                {
                    bestDist = d;
                    bestIdx = i;
                }
            }

            // 6) 更新状态并重绘
            selectedPointIdx = bestIdx;
            btnPickPoint.Text = "Select Point";
            openGLControl.Cursor = Cursors.Default;
            openGLControl.Invalidate();

            // 7) 更新坐标文本框
            if (selectedPointIdx >= 0)
            {
                var hit = points[selectedPointIdx];
                textBoxCoordinate.Text = $"X={hit.X:F2}, Y={hit.Y:F2}, Z={hit.Z:F2}";
            }
            else
            {
                textBoxCoordinate.Clear();
            }
        }

        // ──────────────────────────────────────────────────────────────
        // winX/winY/winZ (0..1)  →  世界坐标 Vector3
        private Vector3 UnProject(float winX, float winY, float winZ,
                                  float[] model, float[] proj, int[] view)
        {
            // 1) Window → NDC
            float ndcX = (winX - view[0]) / view[2] * 2f - 1f;
            float ndcY = (winY - view[1]) / view[3] * 2f - 1f;
            float ndcZ = winZ * 2f - 1f;
            var ndc = new Vector4(ndcX, ndcY, ndcZ, 1f);

            // 2) inv(Proj*Model) ⋅ NDC
            float[] pm = new float[16];
            float[] inv = new float[16];
            MultiplyMat4(proj, model, pm);
            if (!InvertMat4(pm, inv))     // 奇异矩阵就直接返回零向量
                return new Vector3(0, 0, 0);

            Vector4 res = new Vector4(
                inv[0] * ndc.X + inv[4] * ndc.Y + inv[8] * ndc.Z + inv[12],
                inv[1] * ndc.X + inv[5] * ndc.Y + inv[9] * ndc.Z + inv[13],
                inv[2] * ndc.X + inv[6] * ndc.Y + inv[10] * ndc.Z + inv[14],
                inv[3] * ndc.X + inv[7] * ndc.Y + inv[11] * ndc.Z + inv[15]);

            if (Math.Abs(res.W) < 1e-6f) return new Vector3(res.X, res.Y, res.Z);
            return new Vector3(res.X / res.W, res.Y / res.W, res.Z / res.W);
        }

        // 4×4 列主序矩阵乘法 C = A * B
        private void MultiplyMat4(float[] A, float[] B, float[] C)
        {
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                {
                    C[c * 4 + r] = 0;
                    for (int k = 0; k < 4; k++)
                        C[c * 4 + r] += A[k * 4 + r] * B[c * 4 + k];
                }
        }

        // ===== Vector4 struct (用于 UnProject 运算) =====
        public struct Vector4
        {
            public float X, Y, Z, W;
            public Vector4(float x, float y, float z, float w) { X = x; Y = y; Z = z; W = w; }
            public static Vector4 operator +(Vector4 a, Vector4 b) => new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
            public static Vector4 operator *(Vector4 v, float s) => new Vector4(v.X * s, v.Y * s, v.Z * s, v.W * s);
        }

        public static float Dot(Vector3 a, Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        public static float Distance(Vector3 a, Vector3 b) => (float)Math.Sqrt(Dot(a - b, a - b));
        public static Vector3 Normalize(Vector3 v)
        {
            float len = (float)Math.Sqrt(Dot(v, v));
            return len > 0.0f ? v * (1f / len) : v;
        }
    
    // 4×4 列主序矩阵求逆。成功返回 true，失败(不可逆) 返回 false。
    private bool InvertMat4(float[] m, float[] invOut)
        {
            float[] inv = new float[16];

            inv[0] = m[5] * m[10] * m[15] - m[5] * m[11] * m[14] - m[9] * m[6] * m[15]
                     + m[9] * m[7] * m[14] + m[13] * m[6] * m[11] - m[13] * m[7] * m[10];

            inv[4] = -m[4] * m[10] * m[15] + m[4] * m[11] * m[14] + m[8] * m[6] * m[15]
                     - m[8] * m[7] * m[14] - m[12] * m[6] * m[11] + m[12] * m[7] * m[10];

            inv[8] = m[4] * m[9] * m[15] - m[4] * m[11] * m[13] - m[8] * m[5] * m[15]
                     + m[8] * m[7] * m[13] + m[12] * m[5] * m[11] - m[12] * m[7] * m[9];

            inv[12] = -m[4] * m[9] * m[14] + m[4] * m[10] * m[13] + m[8] * m[5] * m[14]
                     - m[8] * m[6] * m[13] - m[12] * m[5] * m[10] + m[12] * m[6] * m[9];

            inv[1] = -m[1] * m[10] * m[15] + m[1] * m[11] * m[14] + m[9] * m[2] * m[15]
                     - m[9] * m[3] * m[14] - m[13] * m[2] * m[11] + m[13] * m[3] * m[10];

            inv[5] = m[0] * m[10] * m[15] - m[0] * m[11] * m[14] - m[8] * m[2] * m[15]
                     + m[8] * m[3] * m[14] + m[12] * m[2] * m[11] - m[12] * m[3] * m[10];

            inv[9] = -m[0] * m[9] * m[15] + m[0] * m[11] * m[13] + m[8] * m[1] * m[15]
                     - m[8] * m[3] * m[13] - m[12] * m[1] * m[11] + m[12] * m[3] * m[9];

            inv[13] = m[0] * m[9] * m[14] - m[0] * m[10] * m[13] - m[8] * m[1] * m[14]
                     + m[8] * m[2] * m[13] + m[12] * m[1] * m[10] - m[12] * m[2] * m[9];

            inv[2] = m[1] * m[6] * m[15] - m[1] * m[7] * m[14] - m[5] * m[2] * m[15]
                     + m[5] * m[3] * m[14] + m[13] * m[2] * m[7] - m[13] * m[3] * m[6];

            inv[6] = -m[0] * m[6] * m[15] + m[0] * m[7] * m[14] + m[4] * m[2] * m[15]
                     - m[4] * m[3] * m[14] - m[12] * m[2] * m[7] + m[12] * m[3] * m[6];

            inv[10] = m[0] * m[5] * m[15] - m[0] * m[7] * m[13] - m[4] * m[1] * m[15]
                     + m[4] * m[3] * m[13] + m[8] * m[1] * m[7] - m[8] * m[3] * m[5];

            inv[14] = -m[0] * m[5] * m[14] + m[0] * m[6] * m[13] + m[4] * m[1] * m[14]
                     - m[4] * m[2] * m[13] - m[8] * m[1] * m[6] + m[8] * m[2] * m[5];

            inv[3] = -m[1] * m[6] * m[11] + m[1] * m[7] * m[10] + m[5] * m[2] * m[11]
                     - m[5] * m[3] * m[10] - m[9] * m[2] * m[7] + m[9] * m[3] * m[6];

            inv[7] = m[0] * m[6] * m[11] - m[0] * m[7] * m[10] - m[4] * m[2] * m[11]
                     + m[4] * m[3] * m[10] + m[8] * m[2] * m[7] - m[8] * m[3] * m[6];

            inv[11] = -m[0] * m[5] * m[11] + m[0] * m[7] * m[9] + m[4] * m[1] * m[11]
                     - m[4] * m[3] * m[9] - m[8] * m[1] * m[7] + m[8] * m[3] * m[5];

            inv[15] = m[0] * m[5] * m[10] - m[0] * m[6] * m[9] - m[4] * m[1] * m[10]
                     + m[4] * m[2] * m[9] + m[8] * m[1] * m[6] - m[8] * m[2] * m[5];

            float det = m[0] * inv[0] + m[1] * inv[4] + m[2] * inv[8] + m[3] * inv[12];
            if (Math.Abs(det) < 1e-6f)          // 判奇异
                return false;

            det = 1.0f / det;
            for (int i = 0; i < 16; i++)
                invOut[i] = inv[i] * det;

            return true;
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

        private async void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "POS files (*.pos)|*.pos|All files (*.*)|*.*";
                if (dlg.ShowDialog() != DialogResult.OK) return;

                string fileName = dlg.FileName;
                try
                {
                    // 1) Show loading status
                    lblStatus.Text = "Reading file…";
                    lblStatus.Visible = true;
                    progressBar.Visible = true;
                    this.Cursor = Cursors.WaitCursor;

                    // 2) Perform file read on background thread
                    var result = await Task.Run(() => ReadPos(fileName));

                    // 3) Back on UI thread: assign arrays
                    x = result.Item1;
                    y = result.Item2;
                    z = result.Item3;
                    m = result.Item4;
                    xLength = x.Length;

                    // 新增：如果任何一个数组长度为 0，就直接报错并返回
                    if (x.Length == 0 || y.Length == 0 || z.Length == 0 || m.Length == 0)
                    {
                        MessageBox.Show("The selected .pos file contains no data points (or failed to parse).",
                                        "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 4) Show next status and process point cloud
                    lblStatus.Text = "Processing point cloud…";
                    LoadPointData(x, y, z, m);

                    MessageBox.Show("File loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Load failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // 5) Hide status and reset cursor
                    lblStatus.Visible = false;
                    progressBar.Visible = false;
                    this.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// 直接加载 .pos 文件，不弹出对话框
        /// </summary>
        public async Task LoadPosFile(string fileName)
        {
            try
            {
                // 1) Show loading status
                lblStatus.Text = "Reading file…";
                lblStatus.Visible = true;
                progressBar.Visible = true;
                this.Cursor = Cursors.WaitCursor;

                // 2) Perform file read on background thread
                var result = await Task.Run(() => ReadPos(fileName));

                // 3) Back on UI thread: assign arrays
                x = result.Item1;
                y = result.Item2;
                z = result.Item3;
                m = result.Item4;

                // 4) Show next status and process point cloud
                lblStatus.Text = "Processing point cloud…";
                LoadPointData(x, y, z, m);

                MessageBox.Show("File loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Load failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 5) Hide status and reset cursor
                lblStatus.Visible = false;
                progressBar.Visible = false;
                this.Cursor = Cursors.Default;
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
            const int RECORD_BYTES = 4 * sizeof(float); // 16
                                                        // 打开文件
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var br = new BinaryReader(fs))
            {
                long totalBytes = fs.Length;
                long recordCount = totalBytes / RECORD_BYTES;
                if (recordCount > int.MaxValue)
                    throw new InvalidOperationException("Record count exceeds supported range.");

                int n = (int)recordCount;
                var x = new float[n];
                var y = new float[n];
                var z = new float[n];
                var m = new float[n];

                // 临时缓冲区，只要能装下一条记录就行
                byte[] tmp = new byte[RECORD_BYTES];

                for (int i = 0; i < n; i++)
                {
                    // 读 16 字节
                    int read = br.Read(tmp, 0, RECORD_BYTES);
                    if (read < RECORD_BYTES)
                        throw new EndOfStreamException("Unexpected end of file.");

                    // 对每个 float 做 big-endian → little-endian 反转
                    // 然后从 tmp[offset] 转成单精度浮点
                    Array.Reverse(tmp, 0, 4);
                    x[i] = BitConverter.ToSingle(tmp, 0);

                    Array.Reverse(tmp, 4, 4);
                    y[i] = BitConverter.ToSingle(tmp, 4);

                    Array.Reverse(tmp, 8, 4);
                    z[i] = BitConverter.ToSingle(tmp, 8);

                    Array.Reverse(tmp, 12, 4);
                    m[i] = BitConverter.ToSingle(tmp, 12);
                }

                return Tuple.Create(x, y, z, m);
            }
        }



        /// <summary>
        /// 从 buffer[offset..offset+3] 按 big-endian 读取一个 float
        /// </summary>
        private static float ReadBigEndianFloat(byte[] buffer, int offset)
        {
            // 把 big-endian 字节倒过来，得到 little-endian 的排列
            byte b0 = buffer[offset];
            byte b1 = buffer[offset + 1];
            byte b2 = buffer[offset + 2];
            byte b3 = buffer[offset + 3];
            byte[] tmp = new byte[4] { b3, b2, b1, b0 };
            return BitConverter.ToSingle(tmp, 0);
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


        /// <summary>
        /// 把 m 范围 [mMin, mMax] 线性映射到 [0,1]，再映射到 HSV 240°→0°，最后转 RGB。
        /// </summary>
        private Color GetColorFromM(float m)
        {
            // 防止除 0
            float span = mMax - mMin;
            float norm = span > 0f
                ? (m - mMin) / span
                : 0f;
            norm = Math.Max(0f, Math.Min(1f, norm));

            // 色相：norm=0 → 240°(蓝)，norm=1 → 0°(红)
            double hue = (1.0 - norm) * 240.0;
            const double saturation = 1.0;
            const double value = 1.0;

            return ColorFromHSV(hue, saturation, value);
        }

        /// <summary>
        /// 从 HSV 生成 RGB 颜色。
        /// hue: 0-360°，saturation/value: 0-1
        /// </summary>
        private Color ColorFromHSV(double hue, double saturation, double value)
        {
            double C = value * saturation;
            double X = C * (1 - Math.Abs((hue / 60.0) % 2 - 1));
            double m = value - C;

            double r1 = 0, g1 = 0, b1 = 0;
            if (hue < 60) { r1 = C; g1 = X; }
            else if (hue < 120) { r1 = X; g1 = C; }
            else if (hue < 180) { g1 = C; b1 = X; }
            else if (hue < 240) { g1 = X; b1 = C; }
            else if (hue < 300) { r1 = X; b1 = C; }
            else { r1 = C; b1 = X; }

            int R = (int)((r1 + m) * 255);
            int G = (int)((g1 + m) * 255);
            int B = (int)((b1 + m) * 255);
            return Color.FromArgb(R, G, B);
        }
        private void openGLControl_MouseWheel(object sender, MouseEventArgs e)
        {
            const float zoomStepPersp = 5f;    // 透视摄像机沿 Z 轴平移步长
            const float zoomFactorOrtho = 0.9f;// 正交每次缩放 10%

            if (usePerspective)
            {
                // 透视：沿 Z 轴前后移动
                zoom += e.Delta > 0 ? zoomStepPersp : -zoomStepPersp;
            }
            else
            {
                // 正交：改变 viewSize
                if (e.Delta > 0)
                    orthoSize *= zoomFactorOrtho;
                else
                    orthoSize /= zoomFactorOrtho;

                // 不要让 orthoSize 变得太小或太大
                orthoSize = Math.Max(1f, Math.Min(1000f, orthoSize));
            }
            openGLControl_Resized(null, null);
            openGLControl.Invalidate();
        }



        private void DrawSizeBar(OpenGL gl, float size, float x, float y, float z)
        {
            // 设置颜色为白色
            gl.Color(1.0f, 1.0f, 1.0f); // 白色

            // 绘制标尺
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(x, y, z-0.2 * size);
            gl.Vertex(x, y, z- 1.2*size); // 线段结束位置
            gl.End();

            // 绘制尺寸标注
            DrawDimensionLabel(gl, size.ToString("F2"),
                   x,
                   y + 0.2f,
                   z - size);
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
            var gl = openGLControl.OpenGL;

            // —— 背景色切换 —— 
            if (checkBox_backgW.Checked)
                gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            else
                gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            // 1. 清除缓存 & 重置矩阵
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();

            // 应用翻转
            float sx = flipX ? -1f : 1f;
            float sy = flipY ? -1f : 1f;
            gl.Scale(sx, sy, 1f);

            const float WORLD_OFFSET_X = 60f;      // 世界坐标系偏移量
            const float CAMERA_ROT_Y_OFFSET = 90f; // 初始 Y 旋转偏移

            // 2. 相机：先做缩放（摄像机前后移动）和全局平移
            gl.Translate(0.0f, 0.0f, zoom);
            gl.Translate(translateX - WORLD_OFFSET_X, translateY, 0.0f);

            // 3. 绕 点云中心(dataCenterX, dataCenterY) 旋转
            //    a) 把中心平移到原点
            gl.Translate(dCenterX, dCenterY, dCenterZ);

            //    b) 执行旋转
            gl.Rotate(rotationX, 1.0f, 0.0f, 0.0f);
            gl.Rotate(rotationY + CAMERA_ROT_Y_OFFSET, 0.0f, 1.0f, 0.0f);

            //    c) 再把中心移回原来位置
            gl.Translate(-dCenterX, -dCenterY, -dCenterZ);

            // 3. 绘制标尺
            DrawSizeBar(gl, 50.0f, 0.0f, 0.0f, 0.0f);

            // 绘制所有点
            gl.PointSize(1f);
            gl.Begin(OpenGL.GL_POINTS);
            Point3DWithColor[] snapshot;
            lock (points) { snapshot = points.ToArray(); }
            foreach (var pt in snapshot)
            {
                gl.Color(pt.Color.R / 255f, pt.Color.G / 255f, pt.Color.B / 255f);
                gl.Vertex(pt.X, pt.Y, pt.Z);
            }
            gl.End();

            // 绘制高亮点（如果有选中）
            if (selectedPointIdx >= 0 && selectedPointIdx < points.Count)
            {
                var hit = points[selectedPointIdx];
                gl.PointSize(8f);
                gl.Color(1f, 1f, 0f);  // 黄色
                gl.Begin(OpenGL.GL_POINTS);
                gl.Vertex(hit.X, hit.Y, hit.Z);
                gl.End();
                gl.PointSize(1f);
            }

            gl.Flush();
        }

        private void BtnCapture_Click(object sender, EventArgs e)
        {
            // 捕获截图
            Bitmap bmp = CaptureControl(openGLControl);


            // 将截图传给 FormAPTViewer 显示
            var aptForm = new FormAPTViewer();
            aptForm.LoadBitmap(bmp);

            // 使用非模态窗口，这样原窗口仍可操作
            aptForm.Show(this);

        }

        /// <summary>
        /// 使用 Win32 BitBlt 从任意 Control 捕获屏幕像素，返回 Bitmap
        /// </summary>
        static Bitmap CaptureControl(Control ctrl)
        {
            int w = ctrl.Width;
            int h = ctrl.Height;
            Bitmap bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics gDest = Graphics.FromImage(bmp))
            {
                IntPtr hdcDest = gDest.GetHdc();
                IntPtr hdcSrc = GetDC(ctrl.Handle);
                BitBlt(hdcDest, 0, 0, w, h, hdcSrc, 0, 0,
                       CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);
                ReleaseDC(ctrl.Handle, hdcSrc);
                gDest.ReleaseHdc(hdcDest);
            }
            return bmp;
        }

        // PInvoke 声明
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        static extern bool BitBlt(
            IntPtr hdcDest,
            int nXDest,
            int nYDest,
            int nWidth,
            int nHeight,
            IntPtr hdcSrc,
            int nXSrc,
            int nYSrc,
            CopyPixelOperation dwRop);

        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        private void FitView()
        {
            // 1) 包围盒
            float minX = x.Min(), maxX = x.Max();
            float minY = y.Min(), maxY = y.Max();
            float minZ = z.Min(), maxZ = z.Max();

            dataCenterX = (minX + maxX) / 2f;
            dataCenterY = (minY + maxY) / 2f;
            float dataCenterZ = (minZ + maxZ) / 2f;  // 如果你需要 Z 的中心

            // 2) 最大跨度 & 包围球半径
            float spanX = maxX - minX;
            float spanY = maxY - minY;
            float spanZ = maxZ - minZ;
            float maxSpan = Math.Max(Math.Max(spanX, spanY), spanZ);
            float r = maxSpan * 0.5f * (float)Math.Sqrt(3);

            // 3) 计算距离
            float fovY = 45f * (float)Math.PI / 180f;
            float halfFov = fovY / 2f;
            float distance = r / (float)Math.Sin(halfFov);

            // 4) 应用到相机参数
            zoom = -distance/2;
            translateX = dataCenterX;
            translateY = dataCenterY;
            // （如果需要，也可以针对 Z 轴做 translateZ=dataCenterZ）
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


        // 窗口大小变化时调用
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            var gl = openGLControl.OpenGL;
            int w = openGLControl.Width;
            int h = openGLControl.Height;
            float aspect = (float)w / h;

            gl.Viewport(0, 0, w, h);
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();

            if (usePerspective)
            {
                gl.Perspective(45.0f, aspect, 0.1f, 1000.0f);
            }
            else
            {
                // left, right, bottom, top
                gl.Ortho(
                    -orthoSize * aspect, orthoSize * aspect,
                    -orthoSize, orthoSize,
                    -1000.0f, 1000.0f
                );
            }

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

            // 2. 平移到数据中心
            translateX = dataCenterX;
            translateY = dataCenterY;

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
                x[i] = x[i] * ICFscale / Kfscale;
                y[i] = y[i] * ICFscale / Kfscale;
                z[i] = Kfscale * Kfscale * z[i] / (ICFscale * ICFscale);
            }
            MessageBox.Show("Done", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadPointData(x, y, z, m);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (_cubicSpline == null)
            {
                MessageBox.Show("Please plot first to create the spline.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            for (int i = 0; i < x.Length; i++)
            {
                float s = (float)_cubicSpline.Interpolate(z[i]); 
                x[i] = x[i] / s;
                y[i] = y[i] / s;
                z[i] = s * s * z[i];
            }
            // 2) rebuild the point‐cloud from the new x,y,z[]
            //    (this repopulates ‘points’ and invalidates the GL control)
            LoadPointData(x, y, z, m);

            MessageBox.Show("Arrays updated with cubic‐spline scaling.", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        /// <summary>
        /// 对 x 做线性插值，返回对应的 √Ratio 值
        /// （如果你需要更精确的三次样条插值，可替换成 MathNet.Numerics 的 CubicSpline）
        /// </summary>
        private double InterpolateSpline(double x)
        {
            if (_cubicSpline == null)
                return 1.0;   // 或者抛异常，视需求而定

            return _cubicSpline.Interpolate(x);
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



        private void bt_fitview_Click(object sender, EventArgs e)
        {
            FitView();
            openGLControl.Invalidate();
        }

        // 向上旋转 1°
        private void btnRotateUp_Click(object sender, EventArgs e)
        {
            rotationX = (rotationX + 1f) % 360f;
            openGLControl.Invalidate();
        }

        // 向下旋转 1°
        private void btnRotateDown_Click(object sender, EventArgs e)
        {
            // +360 保证不出现负值，再对360取模
            rotationX = (rotationX - 1f + 360f) % 360f;
            openGLControl.Invalidate();
        }

        private void BtnPlot_Click(object sender, EventArgs e)
        {
            int countA = _APTseedCoords.GetLength(0);
            int countT = _TEMseedCoords.GetLength(0);
            if (countA < 2 || countT < 2)
            {
                MessageBox.Show("Need at least two points in both APT and TEM to compute distances.", "Info",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 1) 计算距离、ratio 及 X 中点
            var xValues = new List<double>();
            var aptDistances = new List<double>();
            var temDistances = new List<double>();
            var ratioValues = new List<double>();

            for (int i = 1; i < countA; i++)
            {
                double dxA = _APTseedCoords[i, 0] - _APTseedCoords[i - 1, 0];
                double dyA = _APTseedCoords[i, 1] - _APTseedCoords[i - 1, 1];
                double aDist = Math.Sqrt(dxA * dxA + dyA * dyA);
                aptDistances.Add(aDist);

                double dxT = _TEMseedCoords[i, 0] - _TEMseedCoords[i - 1, 0];
                double dyT = _TEMseedCoords[i, 1] - _TEMseedCoords[i - 1, 1];
                double tDist = Math.Sqrt(dxT * dxT + dyT * dyT);
                temDistances.Add(tDist);

                // X 轴用相邻两点的中点
                double xMid = (_APTseedCoords[i, 0] + _APTseedCoords[i - 1, 0]) / 2.0;
                xValues.Add(xMid);

                ratioValues.Add(tDist != 0.0 ?  tDist / aDist : 0.0);
            }

            // 2) 绘制 chart1：TEM/APT 距离 + Ratio
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            var area1 = new ChartArea("Area1")
            {
                AxisX = { Title = "APT Z (nm)", LabelStyle = { Format = "F1" } },
                AxisY = { Title = "Distance / Ratio" }
            };
            chart1.ChartAreas.Add(area1);

            var sAPT = new Series("APT Distance") { ChartType = SeriesChartType.Line, MarkerStyle = MarkerStyle.Circle, BorderWidth = 2 };
            var sTEM = new Series("TEM Distance") { ChartType = SeriesChartType.Line, MarkerStyle = MarkerStyle.Circle, BorderWidth = 2 };
            var sRatio = new Series("TEM/APT Ratio") { ChartType = SeriesChartType.Line, MarkerStyle = MarkerStyle.Diamond, BorderWidth = 2, YAxisType = AxisType.Secondary };

            for (int i = 0; i < aptDistances.Count; i++)
            {
                double x = xValues[i];
                sAPT.Points.AddXY(x, aptDistances[i]);
                sTEM.Points.AddXY(x, temDistances[i]);
                sRatio.Points.AddXY(x, ratioValues[i]);
            }

            chart1.Series.Add(sAPT);
            chart1.Series.Add(sTEM);
            chart1.Series.Add(sRatio);
            chart1.Legends.Clear();
            chart1.Legends.Add(new Legend { Docking = Docking.Top });
            chart1.Titles.Clear();
            chart1.Titles.Add("APT & TEM Distances and TEM/APT Ratio vs APT X Midpoint");


            // 3) 绘制 chart2：√Ratio + 样条拟合
            var sqrtValues = ratioValues.Select(r => Math.Sqrt(r)).ToList();
            // 用 MathNet.Numerics 构造 Natural Cubic Spline
            // 1) 构造 MathNet 的 Natural Cubic Spline
            _cubicSpline = CubicSpline.InterpolateNatural(
                xValues.ToArray(),
                sqrtValues.ToArray()
            );

            // 2) 在 chart2 上绘制：
            //    a) 原始散点
            chart2.Series.Clear();
            chart2.ChartAreas.Clear();
            var area2 = new ChartArea("Area2")
            {
                AxisX = { Title = "APT Z (nm)", LabelStyle = { Format = "F1" } },
                AxisY = { Title = "TEM/APT Ratio" }
            };
            chart2.ChartAreas.Add(area2);

            var sData = new Series("Data Points")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 7
            };
            for (int i = 0; i < sqrtValues.Count; i++)
                sData.Points.AddXY(xValues[i], sqrtValues[i]);

            //    b) MathNet 样条插值曲线（100 个采样点）
            var sTrueSpline = new Series("Cubic Spline")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                MarkerStyle = MarkerStyle.None
            };
            double xMin = xValues.Min(), xMax = xValues.Max();
            int samples = 100;
            for (int j = 0; j <= samples; j++)
            {
                double xx = xMin + (xMax - xMin) * j / samples;
                double yy = _cubicSpline.Interpolate(xx);  // 真正的三次样条插值
                sTrueSpline.Points.AddXY(xx, yy);
            }


            _xValues = xValues;
            _sqrtValues = sqrtValues;

            chart2.Series.Add(sData);
            chart2.Series.Add(sTrueSpline);
            chart2.Legends.Clear();
            chart2.Legends.Add(new Legend { Docking = Docking.Top });
            chart2.Titles.Clear();
            chart2.Titles.Add("Square Root of APT/TEM Ratio & Cubic Spline");
        }


            private void bt_TEMimage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All Files|*.*";
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                // 弹出新的 FormImageViewer
                var viewer = new FormImageViewer();
                viewer.LoadImage(dlg.FileName);
                viewer.Show();  // 或者 ShowDialog()，看你是否要模态
            }
        }

        private void btnPickPoint_Click(object sender, EventArgs e)
        {
            // 进入选点模式
            isPickingEnabled = true;
            btnPickPoint.Text = "Click in GL…";
            // 可选：改变鼠标形状
            openGLControl.Cursor = Cursors.Cross;
        }

        private void btn_kficfcalc_Click(object sender, EventArgs e)
        {
            // 1) Verify data
            if (_xValues == null || _sqrtValues == null || _xValues.Count != _sqrtValues.Count)
            {
                MessageBox.Show("Please click Plot first to generate xValues and sqrtValues.",
                                "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int n = _sqrtValues.Count;

            // 2) Parse scaling factors
            if (!double.TryParse(tb_icf.Text, out double icfScale))
            {
                MessageBox.Show("Invalid ICF scaling factor.", "Format Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!double.TryParse(tb_kf.Text, out double kfScale))
            {
                MessageBox.Show("Invalid kf scaling factor.", "Format Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3) Compute raw ICF/Kf values
            var icfValues = new double[n];
            var kfValues = new double[n];
            for (int i = 0; i < n; i++)
            {
                double root = Math.Sqrt(_sqrtValues[i]);      // √(√Ratio)
                icfValues[i] = icfScale * root;                     // ICF
                kfValues[i] = kfScale * Math.Pow(root, 3);        // Kf
            }

            // 3.5) Find original z[] indices by nearest‐neighbor
            var pointIdxs = new List<int>(n);
            for (int j = 0; j < n; j++)
            {
                double target = _xValues[j];
                int bestK = 0;
                double minDiff = Math.Abs(z[0] - target);
                for (int idx = 1; idx < z.Length; idx++)
                {
                    double diff = Math.Abs(z[idx] - target);
                    if (diff < minDiff)
                    {
                        minDiff = diff;
                        bestK = idx;
                    }
                }
                pointIdxs.Add(bestK);
            }

            // 4) Build splines using the real indices as X
            var indicesArray = pointIdxs.Select(i => (double)i).ToArray();
            _icfSpline = CubicSpline.InterpolateNatural(indicesArray, icfValues);
            _kfSpline = CubicSpline.InterpolateNatural(indicesArray, kfValues);
            _lastIndicesArray = indicesArray;
            _lastIcfValues = icfValues;
            _lastKfValues = kfValues;
            // 5) Prepare ChartArea
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            double minIndex = 1;
            double maxIndex = z.Length;
            double interval = (maxIndex - minIndex) / 10.0;  // ten segments

            var area = new ChartArea("AreaAll")
            {
                AxisX =
                {
                    Title        = "Ion Sequence (i)",
                    Minimum      = minIndex,
                    Maximum      = maxIndex,
                    Interval     = interval,        // your ten-segment spacing
                    LabelStyle   = { Format = "F0" } // show labels as integers
                },
                AxisY = { Title = "ICF / Kf" }
            };
            chart1.ChartAreas.Add(area);

            // 6) Raw data series
            var sRawICF = new Series("ICF Data")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 6
            };
            var sRawKf = new Series("kf Data")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Diamond,
                MarkerSize = 6
            };

            // 7) Spline series
            var sSplineICF = new Series("ICF Spline")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2
            };
            var sSplineKf = new Series("kf Spline")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dash
            };

            // 8) Fill raw‐point series (only indices >= 1)
            for (int i = 0; i < n; i++)
            {
                double xPlot = indicesArray[i];
                if (xPlot < 1) continue;

                sRawICF.Points.AddXY(xPlot, icfValues[i]);
                sRawKf.Points.AddXY(xPlot, kfValues[i]);
            }

            // 9) Sample spline curves from 1 to maxIndex
            double step = (maxIndex - 1) / 100.0;
            for (double xx = 1; xx <= maxIndex; xx += step)
            {
                sSplineICF.Points.AddXY(xx, _icfSpline.Interpolate(xx));
                sSplineKf.Points.AddXY(xx, _kfSpline.Interpolate(xx));
            }

            // 10) Add series & legend/title
            chart1.Series.Add(sRawICF);
            chart1.Series.Add(sRawKf);
            chart1.Series.Add(sSplineICF);
            chart1.Series.Add(sSplineKf);

            chart1.Legends.Clear();
            chart1.Legends.Add(new Legend { Docking = Docking.Top });
            chart1.Titles.Clear();
            chart1.Titles.Add($"ICF & kf vs Index  (ICF={icfScale:F2}, kf={kfScale:F2})");
        }



        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void btn_Rec_Click(object sender, EventArgs e)
        {
            var rec = new Form_Rec(
                _lastIndicesArray,
                _lastIcfValues,
                _lastKfValues,
                _icfSpline,
                _kfSpline,
                xLength
            );
            rec.Show();  
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void tb_icf_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void tb_kf_TextChanged(object sender, EventArgs e)
        {

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
    public struct Vector3
    {
        public float X, Y, Z;
        public Vector3(float x, float y, float z) { X = x; Y = y; Z = z; }

        // ────── 算术运算符 ──────
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3 operator *(Vector3 v, float s) => new Vector3(v.X * s, v.Y * s, v.Z * s);
        public static Vector3 operator *(float s, Vector3 v) => v * s; // 左右都支持

        // ────── 静态函数 ──────
        public static float Dot(Vector3 a, Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        public static float Distance(Vector3 a, Vector3 b) => (float)Math.Sqrt(Dot(a - b, a - b));
        public static Vector3 Normalize(Vector3 v)
        {
            float len = (float)Math.Sqrt(Dot(v, v));
            return len > 1e-6f ? v * (1f / len) : new Vector3(0, 0, 0);
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
