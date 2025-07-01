using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace APT_calibrator_tool
{

    public partial class FormImageViewer : Form
    {
        private enum Mode { None, MeasureDistance, MeasureAngle, MeasureScale, CollectSeeds }
        private Mode currentMode = Mode.None;

        // 测距点
        private Point? distP1, distP2;
        // 测角点
        private Point? angleP1, angleP2, angleP3;
        // 标尺点和标签
        private Point? scaleP1, scaleP2;
        private double scaleRealLength;
        private string scaleUnit = "";
        private string scaleLabel = "";
        public Form_DynRec dynForm;
        // Golden seeds
        private int seedCount;
        private List<Point> TEMseedPoints = new List<Point>();
        // 采集结果数组 [index, 0]=X, [index,1]=Y
        public double[,] TEMSeedCoords { get; private set; }


        private Pen penLine = new Pen(Color.Red, 2);
        private Font drawFont = new Font("Arial", 12);
        private Brush drawBrush = Brushes.Blue;


        // 带已有 DynRec 实例的构造
        public FormImageViewer(Form_DynRec existingDynRec) : this()
        {
            dynForm = existingDynRec;
        }

        // 加载时自动查找已打开的 FormDynRec 实例
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (dynForm == null || dynForm.IsDisposed)
            {
                foreach (Form open in Application.OpenForms)
                {
                    if (open is Form_DynRec frm)
                    {
                        dynForm = frm;
                        break;
                    }
                }
            }
        }



        public FormImageViewer()
        {
            InitializeComponent();
            // 图片自适应
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            btnMeasureDistance.Click += (_, __) => BeginDistance();
            btnMeasureAngle.Click += (_, __) => BeginAngle();
            btnScaleBar.Click += (_, __) => BeginScaleBar();
            btnClear.Click += (_, __) => ClearAll();
            // 新增 Golden Seeds 按钮
            // 假设设计器已添加 btnCollectSeeds
            btnCollectSeeds.Click += (_, __) => BeginCollectSeeds();

            pictureBox1.MouseClick += PictureBox1_MouseClick;
            pictureBox1.Paint += PictureBox1_Paint;
        }

        public void LoadImage(string filePath)
        {
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = Image.FromFile(filePath);
            Text = "Image Viewer – " + System.IO.Path.GetFileName(filePath);
            ClearAll();
        }

        private void BeginDistance()
        {
            currentMode = Mode.MeasureDistance;
            distP1 = distP2 = null;
            lblDistance.Text = "Distance: ";
            pictureBox1.Invalidate();
        }

        private void BeginAngle()
        {
            currentMode = Mode.MeasureAngle;
            angleP1 = angleP2 = angleP3 = null;
            lblAngle.Text = "Angle: ";
            pictureBox1.Invalidate();
        }

        private void BeginScaleBar()
        {
            currentMode = Mode.MeasureScale;
            scaleP1 = scaleP2 = null;
            scaleRealLength = 0;
            scaleUnit = scaleLabel = string.Empty;
            lblDistance.Text = "Scale: ";
            pictureBox1.Invalidate();
        }
        private void BeginCollectSeeds()
        {
            string input = ShowInputDialog("Collect Seeds", "Enter number of seeds:", "5");
            if (int.TryParse(input, out var n) && n > 0)
            {
                seedCount = n;
                TEMseedPoints.Clear();
                TEMSeedCoords = new double[seedCount, 2];
                textBoxSeeds.Clear();
                textBoxSeeds.AppendText("Seed Index, RelX, RelY" + Environment.NewLine);
                currentMode = Mode.CollectSeeds;
            }
        }

        private void ClearAll()
        {
            currentMode = Mode.None;
            distP1 = distP2 = null;
            angleP1 = angleP2 = angleP3 = null;
            scaleP1 = scaleP2 = null;
            scaleRealLength = 0;
            scaleUnit = scaleLabel = string.Empty;

            TEMseedPoints.Clear();
            seedCount = 0;           // 一定要把 seedCount 清零
            TEMSeedCoords = null;        // 并且取消之前的数组
            textBoxSeeds.Clear();

            lblDistance.Text = "Distance: ";
            lblAngle.Text = "Angle: ";
            pictureBox1.Invalidate();
        }
        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = e.Location;
            switch (currentMode)
            {
                case Mode.CollectSeeds:
                    if (TEMseedPoints.Count < seedCount)
                    {
                        TEMseedPoints.Add(p);
                        int idx = TEMseedPoints.Count - 1;
                        var origin = TEMseedPoints[0];

                        // 像素差值
                        double relX_px = p.X - origin.X;
                        double relY_px = p.Y - origin.Y;
                        double realX, realY;

                        // 如果已设置标尺，则按比例换算；否则保留像素值并提示
                        if (scaleRealLength > 0 && scaleP1.HasValue && scaleP2.HasValue)
                        {
                            double dx = scaleP2.Value.X - scaleP1.Value.X;
                            double dy = scaleP2.Value.Y - scaleP1.Value.Y;
                            double pixLen = Math.Sqrt(dx * dx + dy * dy);
                            double unitPerPx = scaleRealLength / pixLen;
                            realX = relX_px * unitPerPx;
                            realY = relY_px * unitPerPx;
                        }
                        else
                        {
                            realX = relX_px;
                            realY = relY_px;
                            if (TEMseedPoints.Count == 1)
                            {
                                // 首次采集时若无标尺，提醒用户先设置标尺
                                MessageBox.Show(
                                    "Please set the scale bar first to collect coordinates in real units. Current values are in pixels (px).",
                                    "Warning",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );

                            }
                        }

                        TEMSeedCoords[idx, 0] = realX;
                        TEMSeedCoords[idx, 1] = realY;
                        string unit = (scaleRealLength > 0 && scaleUnit != "") ? scaleUnit : "px";
                        textBoxSeeds.AppendText($"{idx + 1}, {realX:F2}, {realY:F2} {unit}" + Environment.NewLine);

                        if (TEMseedPoints.Count == seedCount && dynForm != null && !dynForm.IsDisposed)
                        {
                            dynForm.TEMSeedCoords = this.TEMSeedCoords;
                            MessageBox.Show("Seeds collected!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    break;

                case Mode.MeasureScale:
                    if (scaleP1 == null)
                        scaleP1 = p;
                    else if (scaleP2 == null)
                    {
                        scaleP2 = p;
                        PromptScaleBar();
                        currentMode = Mode.None;
                    }
                    break;

                case Mode.MeasureDistance:
                    if (!distP1.HasValue || distP1.HasValue && distP2.HasValue)
                    {
                        distP1 = p;
                        distP2 = null;
                    }
                    else
                    {
                        distP2 = p;
                        CalculateDistance();
                    }
                    break;

                case Mode.MeasureAngle:
                    if (!angleP1.HasValue || angleP1.HasValue && angleP2.HasValue && angleP3.HasValue)
                    {
                        angleP1 = p;
                        angleP2 = angleP3 = null;
                    }
                    else if (!angleP2.HasValue)
                        angleP2 = p;
                    else
                    {
                        angleP3 = p;
                        CalculateAngle();
                    }
                    break;
            }
            pictureBox1.Invalidate();
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            // 绘制 seeds
            for (int i = 0; i < TEMseedPoints.Count; i++)
            {
                var pt = TEMseedPoints[i];
                g.FillEllipse(Brushes.Yellow, pt.X - 4, pt.Y - 4, 8, 8);
                g.DrawString((i + 1).ToString(), drawFont, Brushes.Red, pt.X + 4, pt.Y + 4);
            }

            // 绘制标尺定义
            if (scaleP1.HasValue && scaleP2.HasValue)
                g.DrawLine(penLine, scaleP1.Value, scaleP2.Value);
            // 绘制底部持久标尺
            if (!string.IsNullOrEmpty(scaleLabel) && scaleP1.HasValue && scaleP2.HasValue)
            {
                double dx = scaleP2.Value.X - scaleP1.Value.X;
                double dy = scaleP2.Value.Y - scaleP1.Value.Y;
                float pixLen = (float)Math.Sqrt(dx * dx + dy * dy);
                float y0 = pictureBox1.Height - 30;
                float x0 = (pictureBox1.Width - pixLen) / 2f;
                PointF A = new PointF(x0, y0);
                PointF B = new PointF(x0 + pixLen, y0);
                g.DrawLine(penLine, A, B);
                g.DrawLine(penLine, A, new PointF(A.X, A.Y - 5));
                g.DrawLine(penLine, B, new PointF(B.X, B.Y - 5));
                SizeF ts = g.MeasureString(scaleLabel, drawFont);
                g.DrawString(scaleLabel, drawFont, drawBrush, (pictureBox1.Width - ts.Width) / 2, y0 - ts.Height - 2);
            }

            // 绘制测距点/线
            if (distP1.HasValue)
                g.FillEllipse(drawBrush, distP1.Value.X - 3, distP1.Value.Y - 3, 6, 6);
            if (distP1.HasValue && distP2.HasValue)
            {
                g.FillEllipse(drawBrush, distP2.Value.X - 3, distP2.Value.Y - 3, 6, 6);
                g.DrawLine(penLine, distP1.Value, distP2.Value);
            }

            // 绘制测角点/线
            if (angleP1.HasValue)
                g.FillEllipse(drawBrush, angleP1.Value.X - 3, angleP1.Value.Y - 3, 6, 6);
            if (angleP2.HasValue)
                g.FillEllipse(drawBrush, angleP2.Value.X - 3, angleP2.Value.Y - 3, 6, 6);
            if (angleP3.HasValue)
                g.FillEllipse(drawBrush, angleP3.Value.X - 3, angleP3.Value.Y - 3, 6, 6);
            if (angleP1.HasValue && angleP2.HasValue)
                g.DrawLine(penLine, angleP2.Value, angleP1.Value);
            if (angleP2.HasValue && angleP3.HasValue)
                g.DrawLine(penLine, angleP2.Value, angleP3.Value);
        }

        private void btnScaleBar_Click(object sender, EventArgs e)
        {

        }

        private void CalculateDistance()
        {
            if (distP1.HasValue && distP2.HasValue)
            {
                double dx = distP2.Value.X - distP1.Value.X;
                double dy = distP2.Value.Y - distP1.Value.Y;
                double pixDist = Math.Sqrt(dx * dx + dy * dy);
                string result;
                if (scaleRealLength > 0 && scaleP1.HasValue && scaleP2.HasValue)
                {
                    double sx = scaleP2.Value.X - scaleP1.Value.X;
                    double sy = scaleP2.Value.Y - scaleP1.Value.Y;
                    double pixScale = Math.Sqrt(sx * sx + sy * sy);
                    double realDist = pixDist * scaleRealLength / pixScale;
                    result = $"Distance: {realDist:F2} {scaleUnit}";
                }
                else
                {
                    result = $"Distance: {pixDist:F2} px";
                }
                lblDistance.Text = result;
            }
        }

        private void CalculateAngle()
        {
            if (angleP1.HasValue && angleP2.HasValue && angleP3.HasValue)
            {
                var v1 = new PointF(angleP1.Value.X - angleP2.Value.X, angleP1.Value.Y - angleP2.Value.Y);
                var v2 = new PointF(angleP3.Value.X - angleP2.Value.X, angleP3.Value.Y - angleP2.Value.Y);
                double dot = v1.X * v2.X + v1.Y * v2.Y;
                double d1 = Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y);
                double d2 = Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y);
                double ang = Math.Acos(dot / (d1 * d2)) * 180 / Math.PI;
                lblAngle.Text = $"Angle: {ang:F2}°";
            }
        }

        private void PromptScaleBar()
        {
            string input = ShowInputDialog("Scale Bar", "Enter real length:", "100 nm");
            if (!string.IsNullOrEmpty(input))
            {
                var parts = input.Split(new[] { ' ' }, 2);
                if (double.TryParse(parts[0], out var r))
                {
                    scaleRealLength = r;
                    scaleUnit = parts.Length > 1 ? parts[1] : "units";
                    scaleLabel = $"{r} {scaleUnit}";
                }
            }
        }

        private string ShowInputDialog(string title, string prompt, string defaultVal)
        {
            using (Form dlg = new Form())
            {
                dlg.Text = title;
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.ClientSize = new Size(300, 120);
                Label lbl = new Label() { Text = prompt, Left = 10, Top = 10, AutoSize = true };
                TextBox tb = new TextBox() { Left = 10, Top = 35, Width = 280, Text = defaultVal };
                Button ok = new Button() { Text = "OK", DialogResult = DialogResult.OK, Left = 80, Top = 70 };
                Button cancel = new Button() { Text = "Cancel", DialogResult = DialogResult.Cancel, Left = 160, Top = 70 };
                dlg.Controls.AddRange(new Control[] { lbl, tb, ok, cancel });
                dlg.AcceptButton = ok;
                dlg.CancelButton = cancel;
                return dlg.ShowDialog(this) == DialogResult.OK ? tb.Text : null;
            }
        }
    }
}
