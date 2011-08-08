namespace SS30Main
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.gameCalendar1 = new MiscControls.GameCalendar();
            this.analogClock1 = new MiscControls.AnalogClock();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.button9 = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.SensualityStat = new UniLibControls.StatView();
            this.ArousalStat = new UniLibControls.StatView();
            this.StressStat = new UniLibControls.StatView();
            this.HealthStat = new UniLibControls.StatView();
            this.FatiqueStat = new UniLibControls.StatView();
            this.EnduranceStat = new UniLibControls.StatView();
            this.HappinessStat = new UniLibControls.StatView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ObedienceStat = new UniLibControls.StatView();
            this.SexualityStat = new UniLibControls.StatView();
            this.roomCam1 = new SS30Main.RoomCam();
            this.roomCam2 = new SS30Main.RoomCam();
            this.roomCam3 = new SS30Main.RoomCam();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load cfg";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.Location = new System.Drawing.Point(3, 148);
            this.listBox1.Margin = new System.Windows.Forms.Padding(6);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(509, 75);
            this.listBox1.TabIndex = 1;
            this.listBox1.DoubleClick += new System.EventHandler(this.ActionsList_DoubleClick);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBox1.Size = new System.Drawing.Size(509, 145);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "csv";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "CSV files|*.csv|All files|*.*";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(533, 196);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 46);
            this.button2.TabIndex = 7;
            this.button2.Text = "Look cameras";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.LookButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.button4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.button5, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 284);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Maid 1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Location = new System.Drawing.Point(0, 25);
            this.button3.Margin = new System.Windows.Forms.Padding(0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 25);
            this.button3.TabIndex = 1;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button4.Location = new System.Drawing.Point(0, 50);
            this.button4.Margin = new System.Windows.Forms.Padding(0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 25);
            this.button4.TabIndex = 2;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button5.Location = new System.Drawing.Point(0, 75);
            this.button5.Margin = new System.Windows.Forms.Padding(0);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 25);
            this.button5.TabIndex = 3;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // gameCalendar1
            // 
            this.gameCalendar1.DaysPassed = 0;
            this.gameCalendar1.Location = new System.Drawing.Point(533, 2);
            this.gameCalendar1.Name = "gameCalendar1";
            this.gameCalendar1.Size = new System.Drawing.Size(177, 133);
            this.gameCalendar1.StartDay = 1;
            this.gameCalendar1.StartMonth = 7;
            this.gameCalendar1.StartYear = 2008;
            this.gameCalendar1.TabIndex = 4;
            // 
            // analogClock1
            // 
            this.analogClock1.Draw1MinuteTicks = false;
            this.analogClock1.Draw5MinuteTicks = true;
            this.analogClock1.DrawSecondHand = false;
            this.analogClock1.EveningHour = 19;
            this.analogClock1.EveningMinute = 0;
            this.analogClock1.HourHandColor = System.Drawing.Color.DarkMagenta;
            this.analogClock1.Hours = 19;
            this.analogClock1.Location = new System.Drawing.Point(612, 144);
            this.analogClock1.MinuteHandColor = System.Drawing.Color.Green;
            this.analogClock1.Minutes = 0;
            this.analogClock1.MorningHour = 9;
            this.analogClock1.MorningMinute = 0;
            this.analogClock1.Name = "analogClock1";
            this.analogClock1.NightHour = 0;
            this.analogClock1.NightMinute = 0;
            this.analogClock1.NoonHour = 14;
            this.analogClock1.NoonMinute = 0;
            this.analogClock1.SecondHandColor = System.Drawing.Color.Red;
            this.analogClock1.ShowMinutesFlow = true;
            this.analogClock1.Size = new System.Drawing.Size(98, 98);
            this.analogClock1.TabIndex = 3;
            this.analogClock1.TicksColor = System.Drawing.Color.Black;
            this.analogClock1.GoodMorning += new System.EventHandler(this.SheduleFinished);
            this.analogClock1.GoodAfternoon += new System.EventHandler(this.SheduleFinished);
            this.analogClock1.GoodEvening += new System.EventHandler(this.SheduleFinished);
            this.analogClock1.GoodNight += new System.EventHandler(this.SheduleFinished);
            this.analogClock1.Midnight += new System.EventHandler(this.Midnight);
            this.analogClock1.AnimationFinished += new System.EventHandler(this.analogClock1_AnimationFinished);
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.ItemSize = new System.Drawing.Size(50, 15);
            this.tabControl1.Location = new System.Drawing.Point(0, 29);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(523, 249);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Controls.Add(this.listBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 19);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(515, 226);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Actions";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 19);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(515, 226);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Cameras";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.roomCam1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.roomCam2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.roomCam3, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(509, 220);
            this.tableLayoutPanel3.TabIndex = 6;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(533, 144);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(80, 46);
            this.button9.TabIndex = 10;
            this.button9.Text = "Do own buisiness";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.GoButton_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.28814F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.71186F));
            this.tableLayoutPanel2.Controls.Add(this.SensualityStat, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.ArousalStat, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.StressStat, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.HealthStat, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.FatiqueStat, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.EnduranceStat, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.HappinessStat, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.ObedienceStat, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.SexualityStat, 1, 6);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(533, 248);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 10;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(177, 160);
            this.tableLayoutPanel2.TabIndex = 11;
            // 
            // SensualityStat
            // 
            this.SensualityStat.Color00 = System.Drawing.SystemColors.Control;
            this.SensualityStat.Color01 = System.Drawing.Color.LavenderBlush;
            this.SensualityStat.Color02 = System.Drawing.Color.MistyRose;
            this.SensualityStat.Color03 = System.Drawing.Color.LightPink;
            this.SensualityStat.Color04 = System.Drawing.Color.LightSalmon;
            this.SensualityStat.Color05 = System.Drawing.Color.Salmon;
            this.SensualityStat.Color06 = System.Drawing.Color.Tomato;
            this.SensualityStat.Color07 = System.Drawing.Color.OrangeRed;
            this.SensualityStat.Color08 = System.Drawing.Color.Red;
            this.SensualityStat.Color09 = System.Drawing.Color.Firebrick;
            this.SensualityStat.Color10 = System.Drawing.Color.DarkRed;
            this.SensualityStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SensualityStat.Location = new System.Drawing.Point(69, 139);
            this.SensualityStat.MaxValue = 100;
            this.SensualityStat.MinValue = 0;
            this.SensualityStat.Name = "SensualityStat";
            this.SensualityStat.Size = new System.Drawing.Size(105, 11);
            this.SensualityStat.TabIndex = 20;
            this.SensualityStat.ToolTip = "";
            this.SensualityStat.Value = 5;
            // 
            // ArousalStat
            // 
            this.ArousalStat.Color00 = System.Drawing.SystemColors.Control;
            this.ArousalStat.Color01 = System.Drawing.Color.LavenderBlush;
            this.ArousalStat.Color02 = System.Drawing.Color.MistyRose;
            this.ArousalStat.Color03 = System.Drawing.Color.LightPink;
            this.ArousalStat.Color04 = System.Drawing.Color.LightSalmon;
            this.ArousalStat.Color05 = System.Drawing.Color.Salmon;
            this.ArousalStat.Color06 = System.Drawing.Color.Tomato;
            this.ArousalStat.Color07 = System.Drawing.Color.OrangeRed;
            this.ArousalStat.Color08 = System.Drawing.Color.Red;
            this.ArousalStat.Color09 = System.Drawing.Color.Firebrick;
            this.ArousalStat.Color10 = System.Drawing.Color.DarkRed;
            this.ArousalStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ArousalStat.Location = new System.Drawing.Point(69, 122);
            this.ArousalStat.MaxValue = 100;
            this.ArousalStat.MinValue = 0;
            this.ArousalStat.Name = "ArousalStat";
            this.ArousalStat.Size = new System.Drawing.Size(105, 11);
            this.ArousalStat.TabIndex = 19;
            this.ArousalStat.ToolTip = "";
            this.ArousalStat.Value = 5;
            // 
            // StressStat
            // 
            this.StressStat.Color00 = System.Drawing.SystemColors.Control;
            this.StressStat.Color01 = System.Drawing.Color.Aqua;
            this.StressStat.Color02 = System.Drawing.Color.Aquamarine;
            this.StressStat.Color03 = System.Drawing.Color.GreenYellow;
            this.StressStat.Color04 = System.Drawing.Color.Yellow;
            this.StressStat.Color05 = System.Drawing.Color.Gold;
            this.StressStat.Color06 = System.Drawing.Color.Orange;
            this.StressStat.Color07 = System.Drawing.Color.OrangeRed;
            this.StressStat.Color08 = System.Drawing.Color.Red;
            this.StressStat.Color09 = System.Drawing.Color.Firebrick;
            this.StressStat.Color10 = System.Drawing.Color.DarkRed;
            this.StressStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StressStat.Location = new System.Drawing.Point(69, 71);
            this.StressStat.MaxValue = 100;
            this.StressStat.MinValue = 0;
            this.StressStat.Name = "StressStat";
            this.StressStat.Size = new System.Drawing.Size(105, 11);
            this.StressStat.TabIndex = 18;
            this.StressStat.ToolTip = "";
            this.StressStat.Value = 5;
            // 
            // HealthStat
            // 
            this.HealthStat.Color00 = System.Drawing.SystemColors.Control;
            this.HealthStat.Color01 = System.Drawing.Color.Crimson;
            this.HealthStat.Color02 = System.Drawing.Color.Red;
            this.HealthStat.Color03 = System.Drawing.Color.OrangeRed;
            this.HealthStat.Color04 = System.Drawing.Color.Orange;
            this.HealthStat.Color05 = System.Drawing.Color.Gold;
            this.HealthStat.Color06 = System.Drawing.Color.Yellow;
            this.HealthStat.Color07 = System.Drawing.Color.GreenYellow;
            this.HealthStat.Color08 = System.Drawing.Color.Chartreuse;
            this.HealthStat.Color09 = System.Drawing.Color.LawnGreen;
            this.HealthStat.Color10 = System.Drawing.Color.Lime;
            this.HealthStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HealthStat.Location = new System.Drawing.Point(69, 3);
            this.HealthStat.MaxValue = 100;
            this.HealthStat.MinValue = 0;
            this.HealthStat.Name = "HealthStat";
            this.HealthStat.Size = new System.Drawing.Size(105, 11);
            this.HealthStat.TabIndex = 0;
            this.HealthStat.ToolTip = "";
            this.HealthStat.Value = 5;
            // 
            // FatiqueStat
            // 
            this.FatiqueStat.Color00 = System.Drawing.SystemColors.Control;
            this.FatiqueStat.Color01 = System.Drawing.Color.Aqua;
            this.FatiqueStat.Color02 = System.Drawing.Color.Aquamarine;
            this.FatiqueStat.Color03 = System.Drawing.Color.GreenYellow;
            this.FatiqueStat.Color04 = System.Drawing.Color.Yellow;
            this.FatiqueStat.Color05 = System.Drawing.Color.Gold;
            this.FatiqueStat.Color06 = System.Drawing.Color.Orange;
            this.FatiqueStat.Color07 = System.Drawing.Color.OrangeRed;
            this.FatiqueStat.Color08 = System.Drawing.Color.Red;
            this.FatiqueStat.Color09 = System.Drawing.Color.Firebrick;
            this.FatiqueStat.Color10 = System.Drawing.Color.DarkRed;
            this.FatiqueStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FatiqueStat.Location = new System.Drawing.Point(69, 20);
            this.FatiqueStat.MaxValue = 100;
            this.FatiqueStat.MinValue = 0;
            this.FatiqueStat.Name = "FatiqueStat";
            this.FatiqueStat.Size = new System.Drawing.Size(105, 11);
            this.FatiqueStat.TabIndex = 1;
            this.FatiqueStat.ToolTip = "";
            this.FatiqueStat.Value = 5;
            // 
            // EnduranceStat
            // 
            this.EnduranceStat.Color00 = System.Drawing.SystemColors.Control;
            this.EnduranceStat.Color01 = System.Drawing.Color.Crimson;
            this.EnduranceStat.Color02 = System.Drawing.Color.Red;
            this.EnduranceStat.Color03 = System.Drawing.Color.OrangeRed;
            this.EnduranceStat.Color04 = System.Drawing.Color.Orange;
            this.EnduranceStat.Color05 = System.Drawing.Color.Gold;
            this.EnduranceStat.Color06 = System.Drawing.Color.Yellow;
            this.EnduranceStat.Color07 = System.Drawing.Color.GreenYellow;
            this.EnduranceStat.Color08 = System.Drawing.Color.Chartreuse;
            this.EnduranceStat.Color09 = System.Drawing.Color.LawnGreen;
            this.EnduranceStat.Color10 = System.Drawing.Color.Lime;
            this.EnduranceStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EnduranceStat.Location = new System.Drawing.Point(69, 37);
            this.EnduranceStat.MaxValue = 100;
            this.EnduranceStat.MinValue = 0;
            this.EnduranceStat.Name = "EnduranceStat";
            this.EnduranceStat.Size = new System.Drawing.Size(105, 11);
            this.EnduranceStat.TabIndex = 2;
            this.EnduranceStat.ToolTip = "";
            this.EnduranceStat.Value = 5;
            // 
            // HappinessStat
            // 
            this.HappinessStat.Color00 = System.Drawing.SystemColors.Control;
            this.HappinessStat.Color01 = System.Drawing.Color.Crimson;
            this.HappinessStat.Color02 = System.Drawing.Color.Red;
            this.HappinessStat.Color03 = System.Drawing.Color.OrangeRed;
            this.HappinessStat.Color04 = System.Drawing.Color.Orange;
            this.HappinessStat.Color05 = System.Drawing.Color.Gold;
            this.HappinessStat.Color06 = System.Drawing.Color.Yellow;
            this.HappinessStat.Color07 = System.Drawing.Color.GreenYellow;
            this.HappinessStat.Color08 = System.Drawing.Color.Chartreuse;
            this.HappinessStat.Color09 = System.Drawing.Color.LawnGreen;
            this.HappinessStat.Color10 = System.Drawing.Color.Lime;
            this.HappinessStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HappinessStat.Location = new System.Drawing.Point(69, 54);
            this.HappinessStat.MaxValue = 100;
            this.HappinessStat.MinValue = 0;
            this.HappinessStat.Name = "HappinessStat";
            this.HappinessStat.Size = new System.Drawing.Size(105, 11);
            this.HappinessStat.TabIndex = 3;
            this.HappinessStat.ToolTip = "";
            this.HappinessStat.Value = 5;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Health";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Fatique";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Endurance";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Happiness";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Stress";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 136);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "Sensuality";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 85);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 17);
            this.label8.TabIndex = 12;
            this.label8.Text = "Obedience";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 102);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 17);
            this.label9.TabIndex = 13;
            this.label9.Text = "Sexuality";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(3, 119);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 17);
            this.label10.TabIndex = 14;
            this.label10.Text = "Arousal";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ObedienceStat
            // 
            this.ObedienceStat.Color00 = System.Drawing.SystemColors.Control;
            this.ObedienceStat.Color01 = System.Drawing.Color.Crimson;
            this.ObedienceStat.Color02 = System.Drawing.Color.Red;
            this.ObedienceStat.Color03 = System.Drawing.Color.OrangeRed;
            this.ObedienceStat.Color04 = System.Drawing.Color.Orange;
            this.ObedienceStat.Color05 = System.Drawing.Color.Gold;
            this.ObedienceStat.Color06 = System.Drawing.Color.Yellow;
            this.ObedienceStat.Color07 = System.Drawing.Color.GreenYellow;
            this.ObedienceStat.Color08 = System.Drawing.Color.Chartreuse;
            this.ObedienceStat.Color09 = System.Drawing.Color.LawnGreen;
            this.ObedienceStat.Color10 = System.Drawing.Color.Lime;
            this.ObedienceStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ObedienceStat.Location = new System.Drawing.Point(69, 88);
            this.ObedienceStat.MaxValue = 100;
            this.ObedienceStat.MinValue = 0;
            this.ObedienceStat.Name = "ObedienceStat";
            this.ObedienceStat.Size = new System.Drawing.Size(105, 11);
            this.ObedienceStat.TabIndex = 15;
            this.ObedienceStat.ToolTip = "";
            this.ObedienceStat.Value = 5;
            // 
            // SexualityStat
            // 
            this.SexualityStat.Color00 = System.Drawing.SystemColors.Control;
            this.SexualityStat.Color01 = System.Drawing.Color.LavenderBlush;
            this.SexualityStat.Color02 = System.Drawing.Color.MistyRose;
            this.SexualityStat.Color03 = System.Drawing.Color.LightPink;
            this.SexualityStat.Color04 = System.Drawing.Color.LightSalmon;
            this.SexualityStat.Color05 = System.Drawing.Color.Salmon;
            this.SexualityStat.Color06 = System.Drawing.Color.Tomato;
            this.SexualityStat.Color07 = System.Drawing.Color.OrangeRed;
            this.SexualityStat.Color08 = System.Drawing.Color.Red;
            this.SexualityStat.Color09 = System.Drawing.Color.Firebrick;
            this.SexualityStat.Color10 = System.Drawing.Color.DarkRed;
            this.SexualityStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SexualityStat.Location = new System.Drawing.Point(69, 105);
            this.SexualityStat.MaxValue = 100;
            this.SexualityStat.MinValue = 0;
            this.SexualityStat.Name = "SexualityStat";
            this.SexualityStat.Size = new System.Drawing.Size(105, 11);
            this.SexualityStat.TabIndex = 16;
            this.SexualityStat.ToolTip = "";
            this.SexualityStat.Value = 5;
            // 
            // roomCam1
            // 
            this.roomCam1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.roomCam1.Location = new System.Drawing.Point(3, 3);
            this.roomCam1.Name = "roomCam1";
            this.roomCam1.Room = null;
            this.roomCam1.Size = new System.Drawing.Size(163, 214);
            this.roomCam1.TabIndex = 0;
            // 
            // roomCam2
            // 
            this.roomCam2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.roomCam2.Location = new System.Drawing.Point(172, 3);
            this.roomCam2.Name = "roomCam2";
            this.roomCam2.Room = null;
            this.roomCam2.Size = new System.Drawing.Size(163, 214);
            this.roomCam2.TabIndex = 1;
            // 
            // roomCam3
            // 
            this.roomCam3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.roomCam3.Location = new System.Drawing.Point(341, 3);
            this.roomCam3.Name = "roomCam3";
            this.roomCam3.Room = null;
            this.roomCam3.Size = new System.Drawing.Size(163, 214);
            this.roomCam3.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 410);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.gameCalendar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.analogClock1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private MiscControls.AnalogClock analogClock1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private MiscControls.GameCalendar gameCalendar1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private UniLibControls.StatView HealthStat;
        private UniLibControls.StatView FatiqueStat;
        private UniLibControls.StatView EnduranceStat;
        private UniLibControls.StatView HappinessStat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private UniLibControls.StatView ObedienceStat;
        private UniLibControls.StatView SexualityStat;
        private UniLibControls.StatView SensualityStat;
        private UniLibControls.StatView ArousalStat;
        private UniLibControls.StatView StressStat;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private RoomCam roomCam1;
        private RoomCam roomCam2;
        private RoomCam roomCam3;
    }
}

