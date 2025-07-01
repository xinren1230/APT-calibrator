namespace APT_calibrator_tool
{
    partial class Form_DynRec
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_DynRec));
            this.btn_selec = new System.Windows.Forms.Button();
            this.textBox_message = new System.Windows.Forms.TextBox();
            this.openGLControl = new SharpGL.OpenGLControl();
            this.pole_calc = new System.Windows.Forms.Button();
            this.listViewData = new System.Windows.Forms.ListView();
            this.btn_xy = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_ICFscaling = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_Kfscaling = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tb_slicex = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_slicey = new System.Windows.Forms.TextBox();
            this.tb_slicez = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.tb_slicewidth = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.nm = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.bt_showall = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.tb_disPercent = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_atomM = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.checkBox_displayAll = new System.Windows.Forms.CheckBox();
            this.checkBox_backgW = new System.Windows.Forms.CheckBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.bt_fitview = new System.Windows.Forms.Button();
            this.btnRotateUp = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.BtnPlot = new System.Windows.Forms.Button();
            this.btnFlipX = new System.Windows.Forms.Button();
            this.btnFlipY = new System.Windows.Forms.Button();
            this.btnPickPoint = new System.Windows.Forms.Button();
            this.textBoxCoordinate = new System.Windows.Forms.TextBox();
            this.btnCapture = new System.Windows.Forms.Button();
            this.APTtextBoxCoords = new System.Windows.Forms.TextBox();
            this.TEMtextBoxCoords = new System.Windows.Forms.TextBox();
            this.button8 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.bt_TEMimage = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.tb_icf = new System.Windows.Forms.TextBox();
            this.tb_kf = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.btn_kficfcalc = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_Rec = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.btn_Devfunc = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_selec
            // 
            this.btn_selec.Location = new System.Drawing.Point(28, 24);
            this.btn_selec.Name = "btn_selec";
            this.btn_selec.Size = new System.Drawing.Size(170, 23);
            this.btn_selec.TabIndex = 0;
            this.btn_selec.Text = "Select POS file";
            this.btn_selec.UseVisualStyleBackColor = true;
            this.btn_selec.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // textBox_message
            // 
            this.textBox_message.Location = new System.Drawing.Point(41, 98);
            this.textBox_message.Multiline = true;
            this.textBox_message.Name = "textBox_message";
            this.textBox_message.ReadOnly = true;
            this.textBox_message.Size = new System.Drawing.Size(321, 214);
            this.textBox_message.TabIndex = 1;
            // 
            // openGLControl
            // 
            this.openGLControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.openGLControl.DrawFPS = false;
            this.openGLControl.Location = new System.Drawing.Point(30, 83);
            this.openGLControl.Name = "openGLControl";
            this.openGLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLControl.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openGLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl.Size = new System.Drawing.Size(921, 699);
            this.openGLControl.TabIndex = 3;
            this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized);
            this.openGLControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl_OpenGLDraw);
            this.openGLControl.Resized += new System.EventHandler(this.openGLControl_Resized);
            this.openGLControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseWheel);
            // 
            // pole_calc
            // 
            this.pole_calc.Location = new System.Drawing.Point(977, 28);
            this.pole_calc.Name = "pole_calc";
            this.pole_calc.Size = new System.Drawing.Size(75, 23);
            this.pole_calc.TabIndex = 4;
            this.pole_calc.Text = "Save Pos";
            this.pole_calc.UseVisualStyleBackColor = true;
            this.pole_calc.Click += new System.EventHandler(this.Save_New_file);
            // 
            // listViewData
            // 
            this.listViewData.HideSelection = false;
            this.listViewData.Location = new System.Drawing.Point(355, 205);
            this.listViewData.Name = "listViewData";
            this.listViewData.Size = new System.Drawing.Size(401, 218);
            this.listViewData.TabIndex = 2;
            this.listViewData.UseCompatibleStateImageBehavior = false;
            // 
            // btn_xy
            // 
            this.btn_xy.Location = new System.Drawing.Point(977, 83);
            this.btn_xy.Name = "btn_xy";
            this.btn_xy.Size = new System.Drawing.Size(75, 23);
            this.btn_xy.TabIndex = 6;
            this.btn_xy.Text = "XZ";
            this.btn_xy.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_xy.UseVisualStyleBackColor = true;
            this.btn_xy.Click += new System.EventHandler(this.btn_xy_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Scale Bar: 50 nm";
            // 
            // tb_ICFscaling
            // 
            this.tb_ICFscaling.Location = new System.Drawing.Point(355, 25);
            this.tb_ICFscaling.Name = "tb_ICFscaling";
            this.tb_ICFscaling.Size = new System.Drawing.Size(54, 20);
            this.tb_ICFscaling.TabIndex = 9;
            this.tb_ICFscaling.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "ICF scaling:";
            // 
            // tb_Kfscaling
            // 
            this.tb_Kfscaling.Location = new System.Drawing.Point(476, 24);
            this.tb_Kfscaling.Name = "tb_Kfscaling";
            this.tb_Kfscaling.Size = new System.Drawing.Size(40, 20);
            this.tb_Kfscaling.TabIndex = 9;
            this.tb_Kfscaling.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(415, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "kf scaling:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(529, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Re-reconstruction";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Re_recon);
            // 
            // tb_slicex
            // 
            this.tb_slicex.Location = new System.Drawing.Point(719, 26);
            this.tb_slicex.Name = "tb_slicex";
            this.tb_slicex.Size = new System.Drawing.Size(50, 20);
            this.tb_slicex.TabIndex = 9;
            this.tb_slicex.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(680, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Slice:";
            // 
            // tb_slicey
            // 
            this.tb_slicey.Location = new System.Drawing.Point(775, 26);
            this.tb_slicey.Name = "tb_slicey";
            this.tb_slicey.Size = new System.Drawing.Size(50, 20);
            this.tb_slicey.TabIndex = 9;
            this.tb_slicey.Text = "0";
            // 
            // tb_slicez
            // 
            this.tb_slicez.Location = new System.Drawing.Point(831, 26);
            this.tb_slicez.Name = "tb_slicez";
            this.tb_slicez.Size = new System.Drawing.Size(50, 20);
            this.tb_slicez.TabIndex = 9;
            this.tb_slicez.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(722, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "x:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(781, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "y:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(837, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "z:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(719, 52);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Set";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.slicex);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(775, 52);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(45, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "Set";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.slicey);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(831, 52);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(45, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "Set";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.slicez);
            // 
            // tb_slicewidth
            // 
            this.tb_slicewidth.Location = new System.Drawing.Point(887, 26);
            this.tb_slicewidth.Name = "tb_slicewidth";
            this.tb_slicewidth.Size = new System.Drawing.Size(46, 20);
            this.tb_slicewidth.TabIndex = 12;
            this.tb_slicewidth.Text = "5";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(884, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Width:";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // nm
            // 
            this.nm.AutoSize = true;
            this.nm.Location = new System.Drawing.Point(933, 31);
            this.nm.Name = "nm";
            this.nm.Size = new System.Drawing.Size(21, 13);
            this.nm.TabIndex = 10;
            this.nm.Text = "nm";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(977, 112);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 6;
            this.button5.Text = "YZ";
            this.button5.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.btn_yz_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(977, 141);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 6;
            this.button6.Text = "XY";
            this.button6.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.btn_xz_Click);
            // 
            // bt_showall
            // 
            this.bt_showall.Location = new System.Drawing.Point(887, 52);
            this.bt_showall.Name = "bt_showall";
            this.bt_showall.Size = new System.Drawing.Size(70, 23);
            this.bt_showall.TabIndex = 11;
            this.bt_showall.Text = "Show All";
            this.bt_showall.UseVisualStyleBackColor = true;
            this.bt_showall.Click += new System.EventHandler(this.ShowAll);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(967, 174);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Move: Shift+Left";
            // 
            // tb_disPercent
            // 
            this.tb_disPercent.Location = new System.Drawing.Point(286, 54);
            this.tb_disPercent.Name = "tb_disPercent";
            this.tb_disPercent.Size = new System.Drawing.Size(100, 20);
            this.tb_disPercent.TabIndex = 14;
            this.tb_disPercent.Text = "0.1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(199, 57);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Display Percent:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(392, 57);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(15, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "%";
            // 
            // tb_atomM
            // 
            this.tb_atomM.Location = new System.Drawing.Point(533, 52);
            this.tb_atomM.Name = "tb_atomM";
            this.tb_atomM.Size = new System.Drawing.Size(85, 20);
            this.tb_atomM.TabIndex = 16;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(427, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 13);
            this.label12.TabIndex = 15;
            this.label12.Text = "Display atom mass=";
            // 
            // checkBox_displayAll
            // 
            this.checkBox_displayAll.AutoSize = true;
            this.checkBox_displayAll.Checked = true;
            this.checkBox_displayAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_displayAll.Location = new System.Drawing.Point(624, 55);
            this.checkBox_displayAll.Name = "checkBox_displayAll";
            this.checkBox_displayAll.Size = new System.Drawing.Size(37, 17);
            this.checkBox_displayAll.TabIndex = 17;
            this.checkBox_displayAll.Text = "All";
            this.checkBox_displayAll.UseVisualStyleBackColor = true;
            // 
            // checkBox_backgW
            // 
            this.checkBox_backgW.AutoSize = true;
            this.checkBox_backgW.Location = new System.Drawing.Point(954, 201);
            this.checkBox_backgW.Name = "checkBox_backgW";
            this.checkBox_backgW.Size = new System.Drawing.Size(118, 17);
            this.checkBox_backgW.TabIndex = 18;
            this.checkBox_backgW.Text = "Background: white ";
            this.checkBox_backgW.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(38, 57);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(45, 13);
            this.lblStatus.TabIndex = 19;
            this.lblStatus.Text = "Loading";
            this.lblStatus.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(109, 53);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(84, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 20;
            this.progressBar.Visible = false;
            // 
            // bt_fitview
            // 
            this.bt_fitview.Location = new System.Drawing.Point(977, 235);
            this.bt_fitview.Name = "bt_fitview";
            this.bt_fitview.Size = new System.Drawing.Size(75, 23);
            this.bt_fitview.TabIndex = 21;
            this.bt_fitview.Text = "Fit to view";
            this.bt_fitview.UseVisualStyleBackColor = true;
            this.bt_fitview.Click += new System.EventHandler(this.bt_fitview_Click);
            // 
            // btnRotateUp
            // 
            this.btnRotateUp.Location = new System.Drawing.Point(967, 264);
            this.btnRotateUp.Name = "btnRotateUp";
            this.btnRotateUp.Size = new System.Drawing.Size(92, 23);
            this.btnRotateUp.TabIndex = 22;
            this.btnRotateUp.Text = "Rotate  Z  +";
            this.btnRotateUp.UseVisualStyleBackColor = true;
            this.btnRotateUp.Click += new System.EventHandler(this.btnRotateUp_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(967, 293);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(92, 23);
            this.button7.TabIndex = 22;
            this.button7.Text = "Rotate  Z  -";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.btnRotateDown_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(1089, 39);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(513, 350);
            this.chart1.TabIndex = 23;
            this.chart1.Text = "chart1";
            this.chart1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Chart1_MouseDown);
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Chart1_MouseMove);
            this.chart1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Chart1_MouseUp);
            // 
            // chart2
            // 
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart2.Legends.Add(legend2);
            this.chart2.Location = new System.Drawing.Point(1089, 432);
            this.chart2.Name = "chart2";
            this.chart2.Size = new System.Drawing.Size(513, 350);
            this.chart2.TabIndex = 24;
            this.chart2.Text = "chart2";
            // 
            // BtnPlot
            // 
            this.BtnPlot.Location = new System.Drawing.Point(1089, 10);
            this.BtnPlot.Name = "BtnPlot";
            this.BtnPlot.Size = new System.Drawing.Size(173, 23);
            this.BtnPlot.TabIndex = 25;
            this.BtnPlot.Text = "Calculate Calibration Factor";
            this.BtnPlot.UseVisualStyleBackColor = true;
            this.BtnPlot.Click += new System.EventHandler(this.BtnPlot_Click);
            // 
            // btnFlipX
            // 
            this.btnFlipX.Location = new System.Drawing.Point(977, 328);
            this.btnFlipX.Name = "btnFlipX";
            this.btnFlipX.Size = new System.Drawing.Size(75, 23);
            this.btnFlipX.TabIndex = 27;
            this.btnFlipX.Text = "Flip X";
            this.btnFlipX.UseVisualStyleBackColor = true;
            // 
            // btnFlipY
            // 
            this.btnFlipY.Location = new System.Drawing.Point(977, 357);
            this.btnFlipY.Name = "btnFlipY";
            this.btnFlipY.Size = new System.Drawing.Size(75, 23);
            this.btnFlipY.TabIndex = 27;
            this.btnFlipY.Text = "Flip Y";
            this.btnFlipY.UseVisualStyleBackColor = true;
            // 
            // btnPickPoint
            // 
            this.btnPickPoint.Enabled = false;
            this.btnPickPoint.Location = new System.Drawing.Point(208, 614);
            this.btnPickPoint.Name = "btnPickPoint";
            this.btnPickPoint.Size = new System.Drawing.Size(75, 23);
            this.btnPickPoint.TabIndex = 28;
            this.btnPickPoint.Text = "Pick Point";
            this.btnPickPoint.UseVisualStyleBackColor = true;
            this.btnPickPoint.Visible = false;
            this.btnPickPoint.Click += new System.EventHandler(this.btnPickPoint_Click);
            // 
            // textBoxCoordinate
            // 
            this.textBoxCoordinate.Enabled = false;
            this.textBoxCoordinate.Location = new System.Drawing.Point(355, 634);
            this.textBoxCoordinate.Name = "textBoxCoordinate";
            this.textBoxCoordinate.Size = new System.Drawing.Size(312, 20);
            this.textBoxCoordinate.TabIndex = 29;
            this.textBoxCoordinate.Visible = false;
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(977, 386);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(75, 23);
            this.btnCapture.TabIndex = 30;
            this.btnCapture.Text = "Set seeds";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.BtnCapture_Click);
            // 
            // APTtextBoxCoords
            // 
            this.APTtextBoxCoords.Location = new System.Drawing.Point(957, 446);
            this.APTtextBoxCoords.Multiline = true;
            this.APTtextBoxCoords.Name = "APTtextBoxCoords";
            this.APTtextBoxCoords.Size = new System.Drawing.Size(118, 120);
            this.APTtextBoxCoords.TabIndex = 31;
            // 
            // TEMtextBoxCoords
            // 
            this.TEMtextBoxCoords.Location = new System.Drawing.Point(957, 602);
            this.TEMtextBoxCoords.Multiline = true;
            this.TEMtextBoxCoords.Name = "TEMtextBoxCoords";
            this.TEMtextBoxCoords.Size = new System.Drawing.Size(118, 112);
            this.TEMtextBoxCoords.TabIndex = 31;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(1268, 10);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(121, 23);
            this.button8.TabIndex = 32;
            this.button8.Text = "Apply";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(956, 430);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(113, 13);
            this.label13.TabIndex = 33;
            this.label13.Text = "Golden Seeds of APT:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(954, 586);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(115, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "Golden Seeds of TEM:";
            // 
            // bt_TEMimage
            // 
            this.bt_TEMimage.Enabled = false;
            this.bt_TEMimage.Location = new System.Drawing.Point(41, 614);
            this.bt_TEMimage.Name = "bt_TEMimage";
            this.bt_TEMimage.Size = new System.Drawing.Size(152, 23);
            this.bt_TEMimage.TabIndex = 26;
            this.bt_TEMimage.Text = "Load TEM image";
            this.bt_TEMimage.UseVisualStyleBackColor = true;
            this.bt_TEMimage.Visible = false;
            this.bt_TEMimage.Click += new System.EventHandler(this.bt_TEMimage_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(205, 28);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(63, 13);
            this.label15.TabIndex = 10;
            this.label15.Text = "Single tune:";
            // 
            // tb_icf
            // 
            this.tb_icf.Location = new System.Drawing.Point(1145, 406);
            this.tb_icf.Name = "tb_icf";
            this.tb_icf.Size = new System.Drawing.Size(52, 20);
            this.tb_icf.TabIndex = 34;
            this.tb_icf.Text = "1.5";
            this.tb_icf.TextChanged += new System.EventHandler(this.tb_icf_TextChanged);
            // 
            // tb_kf
            // 
            this.tb_kf.Location = new System.Drawing.Point(1253, 406);
            this.tb_kf.Name = "tb_kf";
            this.tb_kf.Size = new System.Drawing.Size(50, 20);
            this.tb_kf.TabIndex = 34;
            this.tb_kf.Text = "3";
            this.tb_kf.TextChanged += new System.EventHandler(this.tb_kf_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(1089, 410);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 13);
            this.label16.TabIndex = 35;
            this.label16.Text = "Initial ICF:";
            this.label16.Click += new System.EventHandler(this.label16_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(1200, 410);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(46, 13);
            this.label17.TabIndex = 35;
            this.label17.Text = "Initial kf:";
            this.label17.Click += new System.EventHandler(this.label17_Click);
            // 
            // btn_kficfcalc
            // 
            this.btn_kficfcalc.Location = new System.Drawing.Point(1309, 405);
            this.btn_kficfcalc.Name = "btn_kficfcalc";
            this.btn_kficfcalc.Size = new System.Drawing.Size(75, 23);
            this.btn_kficfcalc.TabIndex = 36;
            this.btn_kficfcalc.Text = "Calculate";
            this.btn_kficfcalc.UseVisualStyleBackColor = true;
            this.btn_kficfcalc.Click += new System.EventHandler(this.btn_kficfcalc_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(838, 112);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // btn_Rec
            // 
            this.btn_Rec.Location = new System.Drawing.Point(951, 730);
            this.btn_Rec.Name = "btn_Rec";
            this.btn_Rec.Size = new System.Drawing.Size(132, 23);
            this.btn_Rec.TabIndex = 4;
            this.btn_Rec.Text = "Dynamic Reconstruction";
            this.btn_Rec.UseVisualStyleBackColor = true;
            this.btn_Rec.Click += new System.EventHandler(this.btn_Rec_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(858, 759);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(80, 13);
            this.label18.TabIndex = 37;
            this.label18.Text = "Size bar: 50 nm";
            // 
            // btn_Devfunc
            // 
            this.btn_Devfunc.Location = new System.Drawing.Point(951, 759);
            this.btn_Devfunc.Name = "btn_Devfunc";
            this.btn_Devfunc.Size = new System.Drawing.Size(132, 23);
            this.btn_Devfunc.TabIndex = 38;
            this.btn_Devfunc.Text = "Calculate Devation";
            this.btn_Devfunc.UseVisualStyleBackColor = true;
            this.btn_Devfunc.Click += new System.EventHandler(this.btn_DevFunc_Click);
            // 
            // Form_DynRec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1614, 801);
            this.Controls.Add(this.btn_Devfunc);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.btn_kficfcalc);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.tb_kf);
            this.Controls.Add(this.tb_icf);
            this.Controls.Add(this.openGLControl);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.TEMtextBoxCoords);
            this.Controls.Add(this.APTtextBoxCoords);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.textBoxCoordinate);
            this.Controls.Add(this.btnPickPoint);
            this.Controls.Add(this.btnFlipY);
            this.Controls.Add(this.btnFlipX);
            this.Controls.Add(this.bt_TEMimage);
            this.Controls.Add(this.BtnPlot);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.btnRotateUp);
            this.Controls.Add(this.bt_fitview);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.checkBox_backgW);
            this.Controls.Add(this.checkBox_displayAll);
            this.Controls.Add(this.tb_atomM);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tb_disPercent);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tb_slicewidth);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.bt_showall);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nm);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_slicez);
            this.Controls.Add(this.tb_slicey);
            this.Controls.Add(this.tb_slicex);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_Kfscaling);
            this.Controls.Add(this.tb_ICFscaling);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.btn_xy);
            this.Controls.Add(this.btn_Rec);
            this.Controls.Add(this.pole_calc);
            this.Controls.Add(this.listViewData);
            this.Controls.Add(this.textBox_message);
            this.Controls.Add(this.btn_selec);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_DynRec";
            this.Text = "Correlative dynamic reconstruction refinement";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_selec;
        private System.Windows.Forms.TextBox textBox_message;
        private SharpGL.OpenGLControl openGLControl;
        private System.Windows.Forms.Button pole_calc;
        private System.Windows.Forms.ListView listViewData;
        private System.Windows.Forms.Button btn_xy;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_ICFscaling;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_Kfscaling;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_slicex;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_slicey;
        private System.Windows.Forms.TextBox tb_slicez;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox tb_slicewidth;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label nm;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button bt_showall;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb_disPercent;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tb_atomM;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox checkBox_displayAll;
        private System.Windows.Forms.CheckBox checkBox_backgW;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button bt_fitview;
        private System.Windows.Forms.Button btnRotateUp;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.Button BtnPlot;
        private System.Windows.Forms.Button btnFlipX;
        private System.Windows.Forms.Button btnFlipY;
        private System.Windows.Forms.Button btnPickPoint;
        private System.Windows.Forms.TextBox textBoxCoordinate;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.TextBox APTtextBoxCoords;
        private System.Windows.Forms.TextBox TEMtextBoxCoords;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button bt_TEMimage;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tb_icf;
        private System.Windows.Forms.TextBox tb_kf;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btn_kficfcalc;
        private System.Windows.Forms.Button btn_Rec;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btn_Devfunc;
    }
}