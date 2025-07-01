using System.Drawing;
namespace APT_calibrator_tool
{
    partial class Form_Rec
    {

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Rec));
            this.btnChooseEpos = new System.Windows.Forms.Button();
            this.btnChooseRrng = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartVT = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartM = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvRanges = new System.Windows.Forms.DataGridView();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblLoading = new System.Windows.Forms.Label();
            this.lbl_Range = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tb_kf = new System.Windows.Forms.TextBox();
            this.tb_icf = new System.Windows.Forms.TextBox();
            this.tb_fp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_ef = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_de = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar_rec = new System.Windows.Forms.ProgressBar();
            this.label_rec = new System.Windows.Forms.Label();
            this.btnEditSplines = new System.Windows.Forms.Button();
            this.btnFitICF = new System.Windows.Forms.Button();
            this.btnFitKf = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tb_controlpoint = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.nudYMin = new System.Windows.Forms.NumericUpDown();
            this.nudYMax = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartVT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRanges)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYMax)).BeginInit();
            this.SuspendLayout();
            // 
            // btnChooseEpos
            // 
            this.btnChooseEpos.Location = new System.Drawing.Point(25, 24);
            this.btnChooseEpos.Name = "btnChooseEpos";
            this.btnChooseEpos.Size = new System.Drawing.Size(145, 23);
            this.btnChooseEpos.TabIndex = 0;
            this.btnChooseEpos.Text = "Choose EPOS";
            this.btnChooseEpos.UseVisualStyleBackColor = true;
            this.btnChooseEpos.Click += new System.EventHandler(this.btnChooseEpos_Click);
            // 
            // btnChooseRrng
            // 
            this.btnChooseRrng.Location = new System.Drawing.Point(25, 111);
            this.btnChooseRrng.Name = "btnChooseRrng";
            this.btnChooseRrng.Size = new System.Drawing.Size(145, 23);
            this.btnChooseRrng.TabIndex = 1;
            this.btnChooseRrng.Text = "Choose RRNG";
            this.btnChooseRrng.UseVisualStyleBackColor = true;
            this.btnChooseRrng.Click += new System.EventHandler(this.btnChooseRrng_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(25, 162);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 2;
            this.btnRun.Text = "Run Reconstruction";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.BtnRun_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(1013, 32);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(369, 306);
            this.chart1.TabIndex = 3;
            this.chart1.Text = "chart1";
            // 
            // chartVT
            // 
            this.chartVT.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chartVT.Location = new System.Drawing.Point(0, 0);
            this.chartVT.Name = "chartVT";
            this.chartVT.Size = new System.Drawing.Size(523, 302);
            this.chartVT.TabIndex = 4;
            this.chartVT.Text = "chart2";
            // 
            // chartM
            // 
            this.chartM.Location = new System.Drawing.Point(441, 370);
            this.chartM.Name = "chartM";
            this.chartM.Size = new System.Drawing.Size(523, 321);
            this.chartM.TabIndex = 5;
            this.chartM.Text = "chart2";
            // 
            // dgvRanges
            // 
            this.dgvRanges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRanges.Location = new System.Drawing.Point(25, 292);
            this.dgvRanges.Name = "dgvRanges";
            this.dgvRanges.Size = new System.Drawing.Size(358, 399);
            this.dgvRanges.TabIndex = 6;
            // 
            // chart2
            // 
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            this.chart2.Location = new System.Drawing.Point(1013, 370);
            this.chart2.Name = "chart2";
            this.chart2.Size = new System.Drawing.Size(369, 321);
            this.chart2.TabIndex = 7;
            this.chart2.Text = "chart2";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(25, 65);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 23);
            this.progressBar.TabIndex = 8;
            // 
            // lblLoading
            // 
            this.lblLoading.AutoSize = true;
            this.lblLoading.Location = new System.Drawing.Point(131, 70);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(51, 13);
            this.lblLoading.TabIndex = 0;
            this.lblLoading.Text = "Loading…";
            this.lblLoading.Visible = false;
            // 
            // lbl_Range
            // 
            this.lbl_Range.AutoSize = true;
            this.lbl_Range.Location = new System.Drawing.Point(25, 276);
            this.lbl_Range.Name = "lbl_Range";
            this.lbl_Range.Size = new System.Drawing.Size(42, 13);
            this.lbl_Range.TabIndex = 9;
            this.lbl_Range.Text = "Range:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(201, 61);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(47, 13);
            this.label17.TabIndex = 38;
            this.label17.Text = "Initial Kf:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(201, 27);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 13);
            this.label16.TabIndex = 39;
            this.label16.Text = "Initial ICF:";
            // 
            // tb_kf
            // 
            this.tb_kf.Location = new System.Drawing.Point(333, 57);
            this.tb_kf.Name = "tb_kf";
            this.tb_kf.Size = new System.Drawing.Size(50, 20);
            this.tb_kf.TabIndex = 36;
            this.tb_kf.Text = "3";
            // 
            // tb_icf
            // 
            this.tb_icf.Location = new System.Drawing.Point(331, 24);
            this.tb_icf.Name = "tb_icf";
            this.tb_icf.Size = new System.Drawing.Size(52, 20);
            this.tb_icf.TabIndex = 37;
            this.tb_icf.Text = "1.5";
            // 
            // tb_fp
            // 
            this.tb_fp.Location = new System.Drawing.Point(333, 91);
            this.tb_fp.Name = "tb_fp";
            this.tb_fp.Size = new System.Drawing.Size(50, 20);
            this.tb_fp.TabIndex = 36;
            this.tb_fp.Text = "40";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(201, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "Flight path (mm):";
            // 
            // tb_ef
            // 
            this.tb_ef.Location = new System.Drawing.Point(333, 125);
            this.tb_ef.Name = "tb_ef";
            this.tb_ef.Size = new System.Drawing.Size(50, 20);
            this.tb_ef.TabIndex = 36;
            this.tb_ef.Text = "33";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(201, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 38;
            this.label2.Text = "Evaporation field (V/nm):";
            // 
            // tb_de
            // 
            this.tb_de.Location = new System.Drawing.Point(333, 159);
            this.tb_de.Name = "tb_de";
            this.tb_de.Size = new System.Drawing.Size(50, 20);
            this.tb_de.TabIndex = 36;
            this.tb_de.Text = "0.5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(201, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "Detector efficiency:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(201, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 41;
            this.label6.Text = "Output file name:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(333, 193);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(50, 20);
            this.textBox6.TabIndex = 40;
            this.textBox6.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(438, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Voltage:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(438, 353);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Mass to charge:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1010, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(26, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "ICF:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1012, 350);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "kf:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chartVT);
            this.panel1.Location = new System.Drawing.Point(441, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(523, 302);
            this.panel1.TabIndex = 42;
            // 
            // progressBar_rec
            // 
            this.progressBar_rec.Location = new System.Drawing.Point(25, 192);
            this.progressBar_rec.Name = "progressBar_rec";
            this.progressBar_rec.Size = new System.Drawing.Size(145, 23);
            this.progressBar_rec.TabIndex = 43;
            // 
            // label_rec
            // 
            this.label_rec.AutoSize = true;
            this.label_rec.Location = new System.Drawing.Point(25, 225);
            this.label_rec.Name = "label_rec";
            this.label_rec.Size = new System.Drawing.Size(51, 13);
            this.label_rec.TabIndex = 0;
            this.label_rec.Text = "Loading…";
            this.label_rec.Visible = false;
            // 
            // btnEditSplines
            // 
            this.btnEditSplines.Location = new System.Drawing.Point(1251, 7);
            this.btnEditSplines.Name = "btnEditSplines";
            this.btnEditSplines.Size = new System.Drawing.Size(81, 23);
            this.btnEditSplines.TabIndex = 44;
            this.btnEditSplines.Text = "Edit";
            this.btnEditSplines.UseVisualStyleBackColor = true;
            this.btnEditSplines.Click += new System.EventHandler(this.btnEditSplines_Click);
            // 
            // btnFitICF
            // 
            this.btnFitICF.Location = new System.Drawing.Point(1333, 7);
            this.btnFitICF.Name = "btnFitICF";
            this.btnFitICF.Size = new System.Drawing.Size(49, 23);
            this.btnFitICF.TabIndex = 45;
            this.btnFitICF.Text = "Fit";
            this.btnFitICF.UseVisualStyleBackColor = true;
            this.btnFitICF.Click += new System.EventHandler(this.btnFitICF_Click);
            // 
            // btnFitKf
            // 
            this.btnFitKf.Location = new System.Drawing.Point(1333, 345);
            this.btnFitKf.Name = "btnFitKf";
            this.btnFitKf.Size = new System.Drawing.Size(49, 23);
            this.btnFitKf.TabIndex = 46;
            this.btnFitKf.Text = "Fit";
            this.btnFitKf.UseVisualStyleBackColor = true;
            this.btnFitKf.Click += new System.EventHandler(this.btnFitKf_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1255, 345);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 23);
            this.button1.TabIndex = 44;
            this.button1.Text = "Edit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnEditSplines_Click);
            // 
            // tb_controlpoint
            // 
            this.tb_controlpoint.Location = new System.Drawing.Point(333, 243);
            this.tb_controlpoint.Name = "tb_controlpoint";
            this.tb_controlpoint.Size = new System.Drawing.Size(50, 20);
            this.tb_controlpoint.TabIndex = 40;
            this.tb_controlpoint.Text = "8";
            this.tb_controlpoint.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(201, 246);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "Control point number:";
            this.label4.Click += new System.EventHandler(this.label6_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(236, 225);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(118, 13);
            this.label10.TabIndex = 41;
            this.label10.Text = "Dynamic reconstruction";
            this.label10.Click += new System.EventHandler(this.label6_Click);
            // 
            // nudYMin
            // 
            this.nudYMin.Location = new System.Drawing.Point(1072, 343);
            this.nudYMin.Name = "nudYMin";
            this.nudYMin.Size = new System.Drawing.Size(56, 20);
            this.nudYMin.TabIndex = 48;
            this.nudYMin.Click += new System.EventHandler(this.btnSetYRange_Click);
            // 
            // nudYMax
            // 
            this.nudYMax.Location = new System.Drawing.Point(1134, 344);
            this.nudYMax.Name = "nudYMax";
            this.nudYMax.Size = new System.Drawing.Size(56, 20);
            this.nudYMax.TabIndex = 48;
            this.nudYMax.Click += new System.EventHandler(this.btnSetYRange_Click);
            // 
            // Form_Rec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1415, 708);
            this.Controls.Add(this.nudYMax);
            this.Controls.Add(this.nudYMin);
            this.Controls.Add(this.btnFitKf);
            this.Controls.Add(this.btnFitICF);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnEditSplines);
            this.Controls.Add(this.progressBar_rec);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb_controlpoint);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.tb_de);
            this.Controls.Add(this.tb_ef);
            this.Controls.Add(this.tb_fp);
            this.Controls.Add(this.tb_kf);
            this.Controls.Add(this.tb_icf);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbl_Range);
            this.Controls.Add(this.label_rec);
            this.Controls.Add(this.lblLoading);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.dgvRanges);
            this.Controls.Add(this.chartM);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnChooseRrng);
            this.Controls.Add(this.btnChooseEpos);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_Rec";
            this.Text = "Reconstruction";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartVT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRanges)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudYMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYMax)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnChooseEpos;
        private System.Windows.Forms.Button btnChooseRrng;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartM;
        private System.Windows.Forms.DataGridView dgvRanges;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblLoading;
        private System.Windows.Forms.Label lbl_Range;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tb_kf;
        private System.Windows.Forms.TextBox tb_icf;
        private System.Windows.Forms.TextBox tb_fp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_ef;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_de;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartVT;
        private System.Windows.Forms.ProgressBar progressBar_rec;
        private System.Windows.Forms.Label label_rec;
        private System.Windows.Forms.Button btnEditSplines;
        private System.Windows.Forms.Button btnFitICF;
        private System.Windows.Forms.Button btnFitKf;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_controlpoint;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudYMin;
        private System.Windows.Forms.NumericUpDown nudYMax;
    }
}