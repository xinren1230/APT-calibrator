using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.Interpolation;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace APT_calibrator_tool
{


    public partial class Form_Rec : Form
    {
        private  double[] _indices;
        private  double[] _icfValues;
        private  double[] _kfValues;
        private  int point_count;
        private  CubicSpline _icfSpline;
        private  CubicSpline _kfSpline;
        private string eposFile, rrngFile;
        private List<float> mData;
        private List<float> vtData;
        private List<float> dxData;
        private List<float> dyData;
        private List<RangeEntry> rangesData;
        private IProgress<int> _eposProgress;  // 新：EPOS 读取进度
                                               // in Form_Rec class:
        private bool _dragging;
        private int _dragSeries;    // 1 = ICF, 2 = Kf
        private int _dragPointIdx;
        // —— 新增：无参构造函数 —— 
        public Form_Rec()
        {

            InitializeComponent();
            chartVT.ChartAreas.Clear();
            chartVT.Series.Clear();
            // allow dragging
            InitDragEvents();
            InitChartZoom();     
            // 只做必要的初始化，不绘图
        }
        private void InitDragEvents()
        {
            chart1.MouseDown += Chart_MouseDown;
            chart1.MouseMove += Chart_MouseMove;
            chart1.MouseUp += Chart_MouseUp;

            chart2.MouseDown += Chart_MouseDown;
            chart2.MouseMove += Chart_MouseMove;
            chart2.MouseUp += Chart_MouseUp;
        }
        public Form_Rec(double[] indices,
        double[] icfValues,
        double[] kfValues,
        CubicSpline icfSpline,
        CubicSpline kfSpline,
        int n)
        {
            InitializeComponent();
            InitDragEvents();
            InitChartZoom();      // ← 这里也要加

            // store them
            _indices = indices;
        _icfValues  = icfValues;
        _kfValues   = kfValues;
        _icfSpline  = icfSpline;
        _kfSpline   = kfSpline;
            point_count = n;
            var areaVT = new ChartArea("VT");
            areaVT.AxisX.Title = "Ion sequence #";
            areaVT.AxisY.Title = "VT (V)";
            areaVT.AxisX.IsMarginVisible = false;
            chartVT.ChartAreas.Add(areaVT);
            chartVT.Series.Add(new Series("VT") { ChartType = SeriesChartType.Line, ChartArea = "VT" });
            var areaM = new ChartArea("M");
            areaM.AxisX.Title = "Mass-to-Charge";
            areaM.AxisY.Title = "Count (log)";
            areaM.AxisY.IsLogarithmic = true; areaM.AxisX.IsMarginVisible = false;
            chartM.ChartAreas.Add(areaM);
            chartM.Series.Add(new Series("M") { ChartType = SeriesChartType.Column, ChartArea = "M" });
            if (_indices != null )
             tb_controlpoint.Text = _indices.Length.ToString();
            // when the form shows, redraw charts
            this.Load += Form_Rec_Load;
            
        }


       private void Chart_MouseDown(object sender, MouseEventArgs e)
{
    var chart = (Chart)sender;
    var result = chart.HitTest(e.X, e.Y, ChartElementType.DataPoint);
    if (result.Series == null) return;

    // Only allow dragging the raw‐data series
    if (result.Series.Name == "ICF Data")
    {
        _dragging   = true;
        _dragSeries = 1;
        _dragPointIdx = result.PointIndex;
        chart.Cursor = Cursors.SizeNS;
    }
    else if (result.Series.Name == "Kf Data")
    {
        _dragging   = true;
        _dragSeries = 2;
        _dragPointIdx = result.PointIndex;
        chart.Cursor = Cursors.SizeNS;
    }
}

private void Chart_MouseMove(object sender, MouseEventArgs e)
{
    if (!_dragging) return;
    var chart = (Chart)sender;
    var area = chart.ChartAreas[0];
    // invert pixel Y->data Y
    double yVal = area.AxisY.PixelPositionToValue(e.Y);

    if (_dragSeries == 1)
    {
        // update underlying data
        _icfValues[_dragPointIdx] = yVal;
        // move the point visually
        chart.Series["ICF Data"].Points[_dragPointIdx].YValues[0] = yVal;
    }
    else
    {
        _kfValues[_dragPointIdx] = yVal;
        chart.Series["Kf Data"].Points[_dragPointIdx].YValues[0] = yVal;
    }
}

private void Chart_MouseUp(object sender, MouseEventArgs e)
{
    if (!_dragging) return;
    // rebuild both splines


    // redraw with updated spline
    DrawICFandKfCharts();

    _dragging = false;
    ((Chart)sender).Cursor = Cursors.Default;
}


        private void Form_Rec_Load(object sender, EventArgs e)
        {

            // if no data was passed, bail out
            if (_indices == null || _icfValues == null || _kfValues == null ||
                _icfSpline == null || _kfSpline == null)
                return;

            // chart1: ICF
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            var areaICF = new ChartArea("ICF");
            areaICF.AxisX.Title = "Ion Index (i)";
            areaICF.AxisX.LabelStyle.Format = "F0";
            areaICF.AxisY.Title = "ICF";
            chart1.ChartAreas.Add(areaICF);

            var rawICF = new Series("ICF Data")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Circle,
                ChartArea = "ICF"
            };
            for (int i = 0; i < _indices.Length; i++)
                rawICF.Points.AddXY(_indices[i], _icfValues[i]);

            var splineICF = new Series("ICF Spline")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                ChartArea = "ICF"
            };
            double minI =1, maxI = point_count, stepI = (maxI - minI) / 100.0;
            for (double x = 1; x <= point_count; x += stepI)
                splineICF.Points.AddXY(x, _icfSpline.Interpolate(x));

            chart1.Series.Add(rawICF);
            chart1.Series.Add(splineICF);

            // chart2: Kf
            chart2.Series.Clear();
            chart2.ChartAreas.Clear();
            var areaKf = new ChartArea("Kf");
            areaKf.AxisX.Title = "Ion Index (i)";
            areaKf.AxisX.LabelStyle.Format = "F0";
            areaKf.AxisY.Title = "Kf";
            chart2.ChartAreas.Add(areaKf);

            var rawKf = new Series("Kf Data")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Diamond,
                ChartArea = "Kf"
            };
            for (int i = 0; i < _indices.Length; i++)
                rawKf.Points.AddXY(_indices[i], _kfValues[i]);

            var splineKf = new Series("Kf Spline")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dash,
                ChartArea = "Kf"
            };
            for (double x = 1; x <= point_count; x += stepI)
                splineKf.Points.AddXY(x, _kfSpline.Interpolate(x));

            chart2.Series.Add(rawKf);
            chart2.Series.Add(splineKf);
        }

        // Subscribe mouse-wheel zoom in constructors (call InitChartZoom in each constructor after InitializeComponent)

        // 在类里先保存初始的 Y 范围
        private double _origYMin, _origYMax;

        private void InitChartZoom()
        {
            foreach (var chart in new[] { chart1, chart2 })
            {
                var ay = chart.ChartAreas[0].AxisY;

                // 记录原始全范围
                _origYMin = ay.Minimum;
                _origYMax = ay.Maximum;

                // 不用 ScaleView，避免自动限制
                // （如果之前有设置 ScaleView，则可以注释掉下面两行）
                // chart.ChartAreas[0].AxisY.ScaleView.Zoomable = false;
                // chart.ChartAreas[0].AxisY.ScaleView.Enabled    = false;

                // 让 chart 接收滚轮
                chart.MouseEnter += (s, e) => chart.Focus();
                chart.MouseWheel += Chart_MouseWheel;
            }
        }

        private void Chart_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var ay = chart.ChartAreas[0].AxisY;

            // 鼠标指向的 Y 值
            double yVal = ay.PixelPositionToValue(e.Y);

            // 当前轴范围
            double yMin = ay.Minimum;
            double yMax = ay.Maximum;

            // 缩放因子：向上滚轮（e.Delta>0）细节放大；向下滚粗略
            double factor = e.Delta > 0 ? 0.9 : 1.1;
            double newHeight = (yMax - yMin) * factor;

            // 计算新的 min/max
            double newMin = yVal - (yVal - yMin) * factor;
            double newMax = newMin + newHeight;

            // 如果你想在“缩小”（factor>1）时，超过原始范围就回到原始范围：
            if (factor > 1.0 && newMin <= _origYMin && newMax >= _origYMax)
            {
                ay.Minimum = _origYMin;
                ay.Maximum = _origYMax;
            }
            else
            {
                // 否则直接应用，无限制
                ay.Minimum = newMin;
                ay.Maximum = newMax;
            }
        }




        // --- Corrected btnChooseEpos_Click method ---
        private async void btnChooseEpos_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog
            {
                Title = "Select EPOS File",
                Filter = "EPOS Files (*.epos;*.bin)|*.epos;*.bin|All Files (*.*)|*.*"
            })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;

                eposFile = dlg.FileName;
                btnChooseEpos.Text = Path.GetFileName(eposFile);

                // Initialize progress UI
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Value = 0;
                progressBar.Visible = true;
                lblLoading.Visible = true;
                _eposProgress = new Progress<int>(p => progressBar.Value = p);

                try
                {
                    await Task.Run(() => EposReader.ReadEposReconInside(
                        eposFile,
                        out mData,
                        out vtData,
                        out dxData,
                        out dyData,
                        _eposProgress));

                    // Parse initial ICF/Kf
                    if (!double.TryParse(tb_icf.Text, out double initIcf) ||
                        !double.TryParse(tb_kf.Text, out double initKf))
                    {
                        MessageBox.Show("Invalid ICF or Kf", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int nPoints = vtData.Count;
                    if (!int.TryParse(tb_controlpoint.Text, out int SAMPLE_COUNT))
                        SAMPLE_COUNT = 8;
                    _indices = new double[SAMPLE_COUNT];
                    _icfValues = Enumerable.Repeat(initIcf, SAMPLE_COUNT).ToArray();
                    _kfValues = Enumerable.Repeat(initKf, SAMPLE_COUNT).ToArray();

                    double minI = 1.0, maxI = nPoints;
                    for (int j = 0; j < SAMPLE_COUNT; j++)
                    {
                        double x = minI + j * (maxI - minI) / (SAMPLE_COUNT - 1);
                        _indices[j] = x;
                    }
                    point_count = nPoints;

                    // Plot VT/M
                    LoadEposAndPlot();
                    // Initial sample points
                    DrawICFandKfCharts();
                }
                finally
                {
                    progressBar.Visible = false;
                    lblLoading.Visible = false;
                }
            }
        }


        private void btnChooseRrng_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog { Title = "Select RRNG File", Filter = "RRNG Files (*.rrng)|*.rrng|All Files (*.*)|*.*" })
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    rrngFile = dlg.FileName;
                    btnChooseRrng.Text = Path.GetFileName(rrngFile);
                    LoadRrngAndDisplay();
                }
        }

        private void LoadEposAndPlot()
        {
            if (string.IsNullOrEmpty(eposFile) || vtData == null || mData == null)
                return;

            // --- 1) 配置 VT ChartArea ---
            var areaVT = chartVT.ChartAreas["VT"];
            // 取消自动边距
            areaVT.AxisX.IsMarginVisible = false;
            areaVT.AxisY.IsMarginVisible = false;
            areaVT.Position.Auto = true;

            // 计算 X 范围：从 1 到最后一个绘点索引
            const int sample = 100;
            int N = vtData.Count;
            int maxX = ((N - 1) / sample) * sample + 1;
            double tickX = maxX / 10.0;

            areaVT.AxisX.Minimum = 1;
            areaVT.AxisX.Maximum = maxX;
            areaVT.AxisX.Interval = tickX;
            areaVT.AxisX.Title = "Ion sequence #";
            areaVT.AxisX.LabelStyle.Format = "F0";
            areaVT.InnerPlotPosition.Auto = true;


            // 计算并锁定 Y 范围
            double minVT = vtData.Min();
            double maxVT = vtData.Max();
            double tickY = (maxVT - minVT) / 10.0;

            areaVT.AxisY.Minimum = minVT;
            areaVT.AxisY.Maximum = maxVT;
            areaVT.AxisY.Interval = tickY;
            areaVT.AxisY.Title = "VT (V)";
            areaVT.AxisY.LabelStyle.Format = "F0";

            // --- 2) 绘制 VT 曲线 ---
            var seriesVT = chartVT.Series["VT"];
            seriesVT.Points.Clear();
            for (int i = 0; i < N; i += sample)
                seriesVT.Points.AddXY(i + 1, vtData[i]);
            if (N % sample != 0)
                seriesVT.Points.AddXY(N, vtData.Last());

            // --- 3) 配置 M ChartArea ---

            var areaM = chartM.ChartAreas["M"];
            // 横向不用特殊边距，纵轴对数已经在初始化时配置
            areaM.AxisX.IsMarginVisible = false;
            // 横轴自动从最小到最大
            double minM = mData.Min();
            double maxM = mData.Max();
            double rangeM = maxM - minM;
            // 刻度分十份
            double tickM = rangeM / 10.0;
            areaM.AxisX.Minimum = minM;
            areaM.AxisX.Maximum = maxM;
            areaM.AxisX.Interval = tickM;
            areaM.AxisX.Title = "Mass-to-Charge";
            areaM.AxisX.LabelStyle.Format = "F1";  // 一位小数

            // Y 轴是对数，不设置 Minimum/Maximum，自动适应

            // --- 4) 绘制 M 直方图 ---
            var seriesM = chartM.Series["M"];
            seriesM.Points.Clear();
            int binCount = 10000;
            var counts = new int[binCount];
            double width = rangeM / binCount;

            foreach (var val in mData)
            {
                int idx = (int)((val - minM) / width);
                if (idx >= binCount) idx = binCount - 1;
                counts[idx]++;
            }

            for (int b = 0; b < binCount; b++)
            {
                if (counts[b] > 0)
                {
                    double x = minM + b * width + width / 2;
                    seriesM.Points.AddXY(x, counts[b]);
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        private async void BtnRun_Click(object sender, EventArgs e)
        {
            // 0) 数据检查
            if (mData == null || vtData == null || dxData == null || dyData == null || rangesData == null)
            {
                MessageBox.Show("Please load EPOS and RRNG files before running", "Missing data",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1) 从控件解析参数
            if (!double.TryParse(tb_icf.Text, out double icf)) { MessageBox.Show("ICF Format Error"); tb_icf.Focus(); return; }
            if (!double.TryParse(tb_kf.Text, out double kf)) { MessageBox.Show("kf Format Error"); tb_kf.Focus(); return; }
            if (!double.TryParse(tb_fp.Text, out double fpMm)) { MessageBox.Show("Flight path Format Error"); tb_fp.Focus(); return; }
            if (!double.TryParse(tb_ef.Text, out double efVnm)) { MessageBox.Show("Field Evap Format Error"); tb_ef.Focus(); return; }
            if (!double.TryParse(tb_de.Text, out double de)) { MessageBox.Show("Detector Eff Format Error"); tb_de.Focus(); return; }

            var prm = new ParamSet
            {
                Icf = icf,
                Kf = kf,
                FlightPath = fpMm * 1e-3,  // mm -> m
                FieldEvaporation = efVnm * 1e9,    // V/nm -> V/m
                DetectorEfficiency = de,
                OutputFileName = string.IsNullOrWhiteSpace(textBox6.Text)
                                        ? Path.ChangeExtension(eposFile, ".pos")
                                        : textBox6.Text.Trim()
            };

            // 2) 初始化进度条
            progressBar_rec.Style = ProgressBarStyle.Continuous;
            progressBar_rec.Minimum = 0;
            progressBar_rec.Maximum = 100;
            progressBar_rec.Value = 0;
            progressBar_rec.Visible = true;
            label_rec.Text = "Reconstructing… 0%";
            label_rec.Visible = true;
            var progress = new Progress<int>(p =>
            {
                progressBar_rec.Value = p;
                label_rec.Text = $"Reconstructing… {p}%";
            });

            // 3) 真正耗时的重构放到后台线程
            try
            {
                await Task.Run(() => DoReconstruction(prm, progress));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Reconstruction exception：{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                progressBar_rec.Visible = false;
                label_rec.Visible = false;
            }

            // 5) 完成提示
            MessageBox.Show($"Reconstruction complete!\nWrote file to:\n{prm.OutputFileName}",
                            "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 6) 是否立即加载到 3D 界面
            if (MessageBox.Show("Load new reconstruction into the 3D view?",
                                "Load New Reconstruction", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                var dyn = Application.OpenForms.OfType<Form_DynRec>().FirstOrDefault()
                          ?? new Form_DynRec();
                dyn.Show();
                _ = dyn.LoadPosFile(prm.OutputFileName);
            }
        }

        private void DoReconstruction(ParamSet prm, IProgress<int> progress)
        {
            int N = mData.Count;
            long total = N;
            long written = 0;
            double cumulativeShift = 0;

            // 每个块大约代表总量的 1%
            int blockSize = Math.Max(1, N / 100);

            // 预分配缓冲区
            var buffer = new byte[16];
            double invFE = 1.0 / prm.FieldEvaporation;
            double flight = prm.FlightPath;
            // 先算出 detectorRadius、detphi0
            double detectorRadius = Enumerable.Range(0, N)
                .Max(i => Math.Sqrt(Math.Pow(dxData[i] * 1e-3, 2) + Math.Pow(dyData[i] * 1e-3, 2)));
            double detphi0 = Math.Atan2(detectorRadius, flight);

            // 为了后面计算 zref，先预分配一次 specZMax 的占位
            double globalSpecZMax = double.MinValue;

            using (var fs = new FileStream(prm.OutputFileName, FileMode.Create, FileAccess.Write))
            using (var bw = new BinaryWriter(fs))
            {
                while (written < total)
                {
                    int thisBlock = (int)Math.Min(blockSize, total - written);
                    int offset = (int)written;

                    // 1) 索引数组
                    var idxs = Enumerable.Range(offset, thisBlock).ToArray();
                    var theta = new double[thisBlock];
                    var rho = new double[thisBlock];
                    var Vblk = new double[thisBlock];
                    var kf_dyn = new double[thisBlock];
                    var specR = new double[thisBlock];

                    // 并行预计算 θ, ρ, V, kf_dyn, specR
                    Parallel.For(0, thisBlock, j =>
                    {
                        int i = idxs[j];
                        double dx = dxData[i] * 1e-3;
                        double dy = dyData[i] * 1e-3;
                        theta[j] = Math.Atan2(dy, dx);
                        rho[j] = Math.Sqrt(dx * dx + dy * dy);

                        foreach (var r in rangesData)
                            if (mData[i] >= r.MassMin && mData[i] < r.MassMax)
                            {
                                Vblk[j] = r.AtomicVolume * 1e-27;
                                break;
                            }

                        kf_dyn[j] = (_kfSpline != null)
                            ? _kfSpline.Interpolate(i)
                            : prm.Kf;

                        specR[j] = vtData[i] * invFE / kf_dyn[j];
                    });

                    // 并行计算坐标 & 面积 & 初步 specZ
                    var specX = new double[thisBlock];
                    var specY = new double[thisBlock];
                    var specZ = new double[thisBlock];
                    var area = new double[thisBlock];

                    Parallel.For(0, thisBlock, j =>
                    {
                        int i = idxs[j];
                        double curIcf = (_icfSpline != null)
                            ? _icfSpline.Interpolate(i)
                            : prm.Icf;
                        double mproj = curIcf - 1.0;

                        double phiC = Math.Atan2(rho[j], flight);
                        double phi = phiC + Math.Asin(mproj * Math.Sin(phiC));

                        specX[j] = specR[j] * Math.Sin(phi) * Math.Cos(theta[j]);
                        specY[j] = specR[j] * Math.Sin(phi) * Math.Sin(theta[j]);
                        specZ[j] = specR[j] * Math.Cos(phi);

                        double detphi = detphi0 + Math.Asin(mproj * Math.Sin(detphi0));
                        area[j] = 2 * Math.PI * specR[j] * specR[j] * (1 - Math.Cos(detphi));
                    });

                    // 更新全局 specZMax
                    double localMax = specZ.Max();
                    if (localMax > globalSpecZMax)
                        globalSpecZMax = localMax;

                    // 在写文件前先汇报进度
                    int pctBefore = (int)(written * 100 / (double)total);
                    progress.Report(pctBefore);

                    // 串行累加 shift 并写文件
                    for (int j = 0; j < thisBlock; j++, written++)
                    {
                        double dz = -Vblk[j] / (prm.DetectorEfficiency * area[j]);
                        cumulativeShift += dz;
                        double z = specZ[j] + cumulativeShift;
                        double zref = -(z - globalSpecZMax);

                        float xf = (float)(specX[j] * 1e9);
                        float yf = (float)(specY[j] * 1e9);
                        float zf = (float)(zref * 1e9);
                        float mf = mData[idxs[j]];

                        var b0 = BitConverter.GetBytes(xf); if (BitConverter.IsLittleEndian) Array.Reverse(b0);
                        var b1 = BitConverter.GetBytes(yf); if (BitConverter.IsLittleEndian) Array.Reverse(b1);
                        var b2 = BitConverter.GetBytes(zf); if (BitConverter.IsLittleEndian) Array.Reverse(b2);
                        var b3 = BitConverter.GetBytes(mf); if (BitConverter.IsLittleEndian) Array.Reverse(b3);

                        Buffer.BlockCopy(b0, 0, buffer, 0, 4);
                        Buffer.BlockCopy(b1, 0, buffer, 4, 4);
                        Buffer.BlockCopy(b2, 0, buffer, 8, 4);
                        Buffer.BlockCopy(b3, 0, buffer, 12, 4);
                        bw.Write(buffer);
                    }

                    // 块写完后再报一次，这样就能看到进度条跳动
                    int pctAfter = (int)(written * 100 / (double)total);
                    progress.Report(pctAfter);
                }
            }
        }




        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        // 3) btnEditSplines_Click (optimized: no spline, no ChartAreas.Clear)
        // 3) btnEditSplines_Click (optimized: no spline, no ChartAreas.Clear)
        private void btnEditSplines_Click(object sender, EventArgs e)
        {
            // Ensure data is loaded
            if (_indices == null || _icfValues == null || _kfValues == null)
            {
                MessageBox.Show("Please load and initialize ICF/Kf data first.",
                                "Uninitialized data",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // Only clear existing point series to minimize redrawing overhead
            chart1.Series.Clear();
            chart2.Series.Clear();
            _icfSpline = null;
            _kfSpline = null;
            // Draw ICF raw points (draggable)
            var rawICF = new Series("ICF Data")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8,
                ChartArea = chart1.ChartAreas[0].Name
            };
            for (int i = 0; i < _indices.Length; i++)
                rawICF.Points.AddXY(_indices[i], _icfValues[i]);
            chart1.Series.Add(rawICF);

            // Draw Kf raw points (draggable)
            var rawKf = new Series("Kf Data")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Diamond,
                MarkerSize = 8,
                ChartArea = chart2.ChartAreas[0].Name
            };
            for (int i = 0; i < _indices.Length; i++)
                rawKf.Points.AddXY(_indices[i], _kfValues[i]);
            chart2.Series.Add(rawKf);

            // Edit mode message
            MessageBox.Show(
                "Edit mode is on. Drag points to update ICF/Kf values. Release the mouse to apply changes.",
                "Edit mode",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }




        private void btnFitKf_Click(object sender, EventArgs e)
        {
            // Re-build only the Kf spline
            _kfSpline = CubicSpline.InterpolateNaturalSorted(_indices, _kfValues);
            DrawICFandKfCharts();
        }

        private void btnFitICF_Click(object sender, EventArgs e)
        {
            // Re-build only the ICF spline from your current _indices/_icfValues arrays
            _icfSpline = CubicSpline.InterpolateNaturalSorted(_indices, _icfValues);
            DrawICFandKfCharts();
        }
        // --- Corrected DrawICFandKfCharts method ---
        private void DrawICFandKfCharts()
        {
            if (_indices == null || _icfValues == null || _kfValues == null)
                return;

            // ICF chart
            chart1.Series.Clear();
            if (chart1.ChartAreas.Count == 0)
                chart1.ChartAreas.Add(new ChartArea("ICF"));
            var areaICF = chart1.ChartAreas[0];
            areaICF.AxisX.Title = "Ion Sequence (i)";
            areaICF.AxisY.Title = "ICF";
            areaICF.AxisY.LabelStyle.Format = "F1";
            areaICF.AxisX.LabelStyle.Format = "F1";
            // Ensure full x-axis range
            double minICF = 1.0;
            double maxICF = _indices.Max();
            areaICF.AxisX.Minimum = minICF;
            areaICF.AxisX.Maximum = maxICF;

            // Raw ICF points
            var rawICF = new Series("ICF Data")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8,
                ChartArea = areaICF.Name
            };
            int rawStep = Math.Max(1, _indices.Length / 500);
            for (int i = 0; i < _indices.Length; i += rawStep)
                rawICF.Points.AddXY(_indices[i], _icfValues[i]);
            chart1.Series.Add(rawICF);

            // ICF spline curve
            if (_icfSpline != null)
            {
                var splineICF = new Series("ICF Spline")
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    ChartArea = areaICF.Name
                };
                // Sample from i = 1 to max index
                int pts = 100;
                double step = (maxICF - minICF) / (pts - 1);
                for (int j = 0; j < pts; j++)
                {
                    double x = minICF + j * step;
                    splineICF.Points.AddXY(x, _icfSpline.Interpolate(x));
                }
                chart1.Series.Add(splineICF);
            }

            // Kf chart
            chart2.Series.Clear();
            if (chart2.ChartAreas.Count == 0)
                chart2.ChartAreas.Add(new ChartArea("Kf"));
            var areaKf = chart2.ChartAreas[0];
            areaKf.AxisX.Title = "Ion Sequence (i)";
            areaKf.AxisY.Title = "Kf";
            areaKf.AxisY.LabelStyle.Format = "F1";
            areaKf.AxisX.LabelStyle.Format = "F1";

            // Ensure full x-axis range
            double minKf = 1.0;
            double maxKf = _indices.Max();
            areaKf.AxisX.Minimum = minKf;
            areaKf.AxisX.Maximum = maxKf;

            // Raw Kf points
            var rawKf = new Series("Kf Data")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Diamond,
                MarkerSize = 8,
                ChartArea = areaKf.Name
            };
            for (int i = 0; i < _indices.Length; i += rawStep)
                rawKf.Points.AddXY(_indices[i], _kfValues[i]);
            chart2.Series.Add(rawKf);

            // Kf spline curve
            if (_kfSpline != null)
            {
                var splineKf = new Series("Kf Spline")
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    BorderDashStyle = ChartDashStyle.Dash,
                    ChartArea = areaKf.Name
                };
                int pts2 = 100;
                double stp = (maxKf - minKf) / (pts2 - 1);
                for (int j = 0; j < pts2; j++)
                {
                    double x = minKf + j * stp;
                    splineKf.Points.AddXY(x, _kfSpline.Interpolate(x));
                }
                chart2.Series.Add(splineKf);
            }
        }
        // 比如在某个按钮点击事件里，或响应两个 NumericUpDown 的 ValueChanged：
        private void btnSetYRange_Click(object sender, EventArgs e)
        {
            // 假设你在窗体上放了两个 NumericUpDown：nudYMin、nudYMax
            double yMin = (double)nudYMin.Value;
            double yMax = (double)nudYMax.Value;
            if (yMax <= yMin)
            {
                MessageBox.Show("最大值必须大于最小值。");
                return;
            }

            var area = chart2.ChartAreas[0];
            area.AxisY.Minimum = yMin;
            area.AxisY.Maximum = yMax;
        }

        private void LoadRrngAndDisplay()
        {
            if (string.IsNullOrEmpty(rrngFile)) return;
            rangesData = RrngReader.ReadRrng(rrngFile);
            dgvRanges.DataSource = rangesData;
        }
    }

}
