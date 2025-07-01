using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading.Tasks;
namespace APT_calibrator_tool
{
        public class RangeEntry
        {
            public double MassMin { get; set; }
            public double MassMax { get; set; }
            public double AtomicVolume { get; set; }
        }

        public class ParamSet
        {
            public double Icf { get; set; }
            public double Kf { get; set; }
            public double FlightPath { get; set; }
            public double FieldEvaporation { get; set; }
            public double DetectorEfficiency { get; set; }
            public string OutputFileName { get; set; }
    }



    static class EposReader
    {
        public static void ReadEposReconInside(string fileName,
            out List<float> m, out List<float> vt, out List<float> dx, out List<float> dy,
            IProgress<int> progress)
        {
            const int numFloatsPerRecord = 9;
            const int floatSize = 4;
            const int trailerSize = 8;
            int recordSize = numFloatsPerRecord * floatSize + trailerSize;

            var fileInfo = new FileInfo(fileName);
            long total = fileInfo.Length / recordSize;

            // 预分配大概容量，避免频繁扩容
            m = new List<float>((int)Math.Min(total, int.MaxValue));
            vt = new List<float>((int)Math.Min(total, int.MaxValue));
            dx = new List<float>((int)Math.Min(total, int.MaxValue));
            dy = new List<float>((int)Math.Min(total, int.MaxValue));

            // 打开文件，顺序扫描模式
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 1 << 20, FileOptions.SequentialScan))
            using (var br = new BinaryReader(fs))
            {
                // 用于解码每条记录
                byte[] buf = new byte[recordSize];
                var rnd = new Random();

                // 每隔多少条上报一次进度
                long reportInterval = Math.Max(1, total / 100);  // 每 1% 一次
                reportInterval = Math.Min(reportInterval, 2000L); // 最多隔 2000 条

                for (long i = 0; i < total; i++)
                {
                    // 1) 读整条
                    int read = br.Read(buf, 0, recordSize);
                    if (read < recordSize) break;

                    // 2) 解码前 9 个 big-endian float
                    float[] rec = new float[numFloatsPerRecord];
                    for (int j = 0; j < numFloatsPerRecord; j++)
                    {
                        int pos = j * floatSize;
                        // 复制 4 字节到临时数组并翻转
                        byte b0 = buf[pos + 0], b1 = buf[pos + 1],
                             b2 = buf[pos + 2], b3 = buf[pos + 3];
                        // big-endian → little-endian
                        uint bits = ((uint)b0 << 24) | ((uint)b1 << 16) | ((uint)b2 << 8) | b3;
                        rec[j] = BitConverter.ToSingle(BitConverter.GetBytes(bits), 0);
                    }

                    // 3) 存结果
                    m.Add(rec[3]);
                    vt.Add(rec[5] + rec[6]);
                    dx.Add(rec[7] + (float)((rnd.NextDouble() - 0.5) * 0.05));
                    dy.Add(rec[8] + (float)((rnd.NextDouble() - 0.5) * 0.05));

                    // 4) 上报进度
                    if (progress != null && (i % reportInterval == 0 || i == total - 1))
                    {
                        int pct = (int)(i * 100L / (total - 1));
                        progress.Report(pct);
                    }
                }
            }
        }
    }


    static class RrngReader
        {
            public static List<RangeEntry> ReadRrng(string path)
            {
                var data = new List<List<string>>();
                foreach (var raw in File.ReadAllLines(path))
                {
                    var line = Regex.Replace(raw, "[#%].*", "");
                    var tokens = Regex.Split(line, "[=,\t :]")
                                      .Select(t => t.Trim())
                                      .ToList();
                    data.Add(tokens);
                }
                int nelem = int.Parse(data[1][1]);
                int nrng = int.Parse(data[4 + nelem][1]);
                var list = new List<RangeEntry>(nrng);
                for (int i = 0; i < nrng; i++)
                {
                    var row = data[5 + nelem + i];
                    list.Add(new RangeEntry
                    {
                        MassMin = double.Parse(row[1]),
                        MassMax = double.Parse(row[2]),
                        AtomicVolume = double.Parse(row[4])
                    });
                }
                return list;
            }
        }
    //no use below
        public class ParameterDialog : Form
        {
            private TextBox[] boxes;
            private readonly string[] labels = new[]
            {
            "Image compression factor (icf)",
            "Field factor (kf)",
            "Flight path (mm)",
            "Evaporation field (V/nm)",
            "Detector efficiency",
            "Output file name"
        };
            public ParamSet Result { get; private set; }

            public ParameterDialog()
            {
                Text = "Reconstruction parameters";
                AutoSize = true; AutoSizeMode = AutoSizeMode.GrowAndShrink;
                var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = labels.Length + 1, Padding = new Padding(10) };
                boxes = new TextBox[labels.Length];
                for (int i = 0; i < labels.Length; i++)
                {
                    layout.Controls.Add(new Label { Text = labels[i], AutoSize = true }, 0, i);
                    boxes[i] = new TextBox { Width = 200 };
                    layout.Controls.Add(boxes[i], 1, i);
                }
                boxes[0].Text = "1.5";
                boxes[1].Text = "1.25";
                boxes[2].Text = "100";
                boxes[3].Text = "33";
                boxes[4].Text = "0.8";
                boxes[5].Text = string.Empty;

                var btns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
                var ok = new Button { Text = "OK", DialogResult = DialogResult.OK };
                var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel };
                btns.Controls.Add(ok);
                btns.Controls.Add(cancel);
                layout.Controls.Add(btns, 0, labels.Length);
                layout.SetColumnSpan(btns, 2);
                Controls.Add(layout);
                AcceptButton = ok;
                CancelButton = cancel;
            }
        }
        }
