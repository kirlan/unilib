namespace VQMapTest2
{
    partial class GenerationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerationForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button10 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.WaterPercent = new System.Windows.Forms.HScrollBar();
            this.LandMassesCount = new System.Windows.Forms.HScrollBar();
            this.LandsCount = new System.Windows.Forms.HScrollBar();
            this.ContinentsRnd = new System.Windows.Forms.Button();
            this.ContinentsCount = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.PartialMap = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.RacesCount = new System.Windows.Forms.HScrollBar();
            this.ProvinciesCount = new System.Windows.Forms.HScrollBar();
            this.StatesCount = new System.Windows.Forms.HScrollBar();
            this.button6 = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.button5 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Equator = new System.Windows.Forms.HScrollBar();
            this.Pole = new System.Windows.Forms.HScrollBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.CustomMap = new System.Windows.Forms.Panel();
            this.Presets = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContinentsCount)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.CustomMap.SuspendLayout();
            this.Presets.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.button10);
            this.groupBox1.Location = new System.Drawing.Point(12, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 46);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Grid";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(9, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(286, 21);
            this.comboBox1.Sorted = true;
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button10
            // 
            this.button10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button10.Location = new System.Drawing.Point(301, 16);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(127, 23);
            this.button10.TabIndex = 2;
            this.button10.Text = "Show grids manager...";
            this.toolTip1.SetToolTip(this.button10, "Build completely new grid to use it and - optional - save to file for future use." +
                    "");
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Lands count:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(225, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "LandMasses count:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.WaterPercent);
            this.groupBox2.Controls.Add(this.LandMassesCount);
            this.groupBox2.Controls.Add(this.LandsCount);
            this.groupBox2.Controls.Add(this.ContinentsRnd);
            this.groupBox2.Controls.Add(this.ContinentsCount);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.PartialMap);
            this.groupBox2.Location = new System.Drawing.Point(0, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(434, 96);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Landscape";
            // 
            // WaterPercent
            // 
            this.WaterPercent.LargeChange = 5;
            this.WaterPercent.Location = new System.Drawing.Point(96, 70);
            this.WaterPercent.Maximum = 94;
            this.WaterPercent.Minimum = 10;
            this.WaterPercent.Name = "WaterPercent";
            this.WaterPercent.Size = new System.Drawing.Size(89, 16);
            this.WaterPercent.TabIndex = 29;
            this.toolTip1.SetToolTip(this.WaterPercent, "66");
            this.WaterPercent.Value = 66;
            this.WaterPercent.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // LandMassesCount
            // 
            this.LandMassesCount.Location = new System.Drawing.Point(323, 20);
            this.LandMassesCount.Maximum = 300;
            this.LandMassesCount.Minimum = 30;
            this.LandMassesCount.Name = "LandMassesCount";
            this.LandMassesCount.Size = new System.Drawing.Size(102, 16);
            this.LandMassesCount.TabIndex = 28;
            this.toolTip1.SetToolTip(this.LandMassesCount, "150");
            this.LandMassesCount.Value = 150;
            this.LandMassesCount.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // LandsCount
            // 
            this.LandsCount.LargeChange = 100;
            this.LandsCount.Location = new System.Drawing.Point(83, 20);
            this.LandsCount.Maximum = 4000;
            this.LandsCount.Minimum = 1000;
            this.LandsCount.Name = "LandsCount";
            this.LandsCount.Size = new System.Drawing.Size(102, 16);
            this.LandsCount.TabIndex = 27;
            this.toolTip1.SetToolTip(this.LandsCount, "3000");
            this.LandsCount.Value = 3000;
            this.LandsCount.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // ContinentsRnd
            // 
            this.ContinentsRnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ContinentsRnd.Location = new System.Drawing.Point(151, 43);
            this.ContinentsRnd.Margin = new System.Windows.Forms.Padding(0);
            this.ContinentsRnd.Name = "ContinentsRnd";
            this.ContinentsRnd.Size = new System.Drawing.Size(34, 20);
            this.ContinentsRnd.TabIndex = 21;
            this.ContinentsRnd.Text = "RND";
            this.ContinentsRnd.UseVisualStyleBackColor = true;
            this.ContinentsRnd.Click += new System.EventHandler(this.RndContinents_Click);
            // 
            // ContinentsCount
            // 
            this.ContinentsCount.Location = new System.Drawing.Point(102, 43);
            this.ContinentsCount.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.ContinentsCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ContinentsCount.Name = "ContinentsCount";
            this.ContinentsCount.Size = new System.Drawing.Size(51, 20);
            this.ContinentsCount.TabIndex = 18;
            this.ContinentsCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.ContinentsCount, resources.GetString("ContinentsCount.ToolTip"));
            this.ContinentsCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 45);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(90, 13);
            this.label16.TabIndex = 17;
            this.label16.Text = "Continents count:";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.Location = new System.Drawing.Point(185, 68);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(34, 20);
            this.button3.TabIndex = 16;
            this.button3.Text = "RND";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.RndWater_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(225, 70);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(114, 13);
            this.label14.TabIndex = 15;
            this.label14.Text = "(in % of total map area)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 70);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(87, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Water coverage:";
            this.toolTip1.SetToolTip(this.label13, "Haw much of map will be oceans.\r\nThat\'s not a precise parameter, final result cou" +
                    "ld slightly differ from stated value.");
            // 
            // PartialMap
            // 
            this.PartialMap.AutoSize = true;
            this.PartialMap.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PartialMap.Location = new System.Drawing.Point(225, 44);
            this.PartialMap.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.PartialMap.Name = "PartialMap";
            this.PartialMap.Size = new System.Drawing.Size(191, 17);
            this.PartialMap.TabIndex = 20;
            this.PartialMap.Text = "Continents could touch map border";
            this.PartialMap.UseVisualStyleBackColor = true;
            this.PartialMap.CheckedChanged += new System.EventHandler(this.PartialMap_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.RacesCount);
            this.groupBox3.Controls.Add(this.ProvinciesCount);
            this.groupBox3.Controls.Add(this.StatesCount);
            this.groupBox3.Controls.Add(this.button6);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Location = new System.Drawing.Point(12, 287);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(434, 76);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Population:";
            // 
            // RacesCount
            // 
            this.RacesCount.LargeChange = 5;
            this.RacesCount.Location = new System.Drawing.Point(83, 22);
            this.RacesCount.Maximum = 40;
            this.RacesCount.Minimum = 6;
            this.RacesCount.Name = "RacesCount";
            this.RacesCount.Size = new System.Drawing.Size(102, 16);
            this.RacesCount.TabIndex = 26;
            this.toolTip1.SetToolTip(this.RacesCount, "24");
            this.RacesCount.Value = 24;
            this.RacesCount.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // ProvinciesCount
            // 
            this.ProvinciesCount.LargeChange = 5;
            this.ProvinciesCount.Location = new System.Drawing.Point(323, 48);
            this.ProvinciesCount.Maximum = 250;
            this.ProvinciesCount.Minimum = 50;
            this.ProvinciesCount.Name = "ProvinciesCount";
            this.ProvinciesCount.Size = new System.Drawing.Size(102, 16);
            this.ProvinciesCount.TabIndex = 25;
            this.toolTip1.SetToolTip(this.ProvinciesCount, "100");
            this.ProvinciesCount.Value = 100;
            this.ProvinciesCount.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // StatesCount
            // 
            this.StatesCount.LargeChange = 5;
            this.StatesCount.Location = new System.Drawing.Point(83, 48);
            this.StatesCount.Maximum = 25;
            this.StatesCount.Minimum = 5;
            this.StatesCount.Name = "StatesCount";
            this.StatesCount.Size = new System.Drawing.Size(102, 16);
            this.StatesCount.TabIndex = 24;
            this.toolTip1.SetToolTip(this.StatesCount, "20");
            this.StatesCount.Value = 20;
            this.StatesCount.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button6.Location = new System.Drawing.Point(185, 46);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(34, 20);
            this.button6.TabIndex = 23;
            this.button6.Text = "RND";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.RndStates_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(70, 13);
            this.label15.TabIndex = 21;
            this.label15.Text = "States count:";
            this.toolTip1.SetToolTip(this.label15, "How many different states (kingdoms, clans, republics etc.) will be there.\r\nThat\'" +
                    "s not a precise parameter, becouse real count couldn\'t be less then \r\ntotal coun" +
                    "t of continents and islands on map.");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(225, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Provincies count:";
            this.toolTip1.SetToolTip(this.label2, "Province is part of state, consists from a central settlement and  adjacent  land" +
                    "s.\r\nEach state contains at least 1 province.\r\n");
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.Location = new System.Drawing.Point(185, 19);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(34, 20);
            this.button4.TabIndex = 17;
            this.button4.Text = "RND";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.RndRaces_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Races count:";
            this.toolTip1.SetToolTip(this.label12, "How many different races from built-in list lives in this world.");
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button7.Location = new System.Drawing.Point(68, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(146, 23);
            this.button7.TabIndex = 7;
            this.button7.Text = "Randomize all!";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.RndAll_Click);
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button8.Location = new System.Drawing.Point(148, 413);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(162, 31);
            this.button8.TabIndex = 8;
            this.button8.Text = "Start generation!";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.button5, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.button7, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(12, 373);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(434, 31);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(220, 3);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(146, 23);
            this.button5.TabIndex = 8;
            this.button5.Text = "Restore defaults";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 1;
            this.toolTip1.AutoPopDelay = 100000000;
            this.toolTip1.InitialDelay = 0;
            this.toolTip1.ReshowDelay = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(125, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Pole to equator distance:";
            this.toolTip1.SetToolTip(this.label8, "Determines a climate scheme.\r\nRegions, layed quite far from equator, will be thun" +
                    "dra or taiga.\r\n");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Equator position from map top:";
            this.toolTip1.SetToolTip(this.label6, "Determines a climate pattern.\r\nRegions along equator line will be junjles, savann" +
                    "as or deserts.\r\n");
            // 
            // Equator
            // 
            this.Equator.LargeChange = 5;
            this.Equator.Location = new System.Drawing.Point(159, 20);
            this.Equator.Maximum = 154;
            this.Equator.Minimum = -50;
            this.Equator.Name = "Equator";
            this.Equator.Size = new System.Drawing.Size(102, 16);
            this.Equator.TabIndex = 28;
            this.toolTip1.SetToolTip(this.Equator, "50");
            this.Equator.Value = 50;
            this.Equator.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // Pole
            // 
            this.Pole.LargeChange = 5;
            this.Pole.Location = new System.Drawing.Point(159, 46);
            this.Pole.Maximum = 229;
            this.Pole.Minimum = 10;
            this.Pole.Name = "Pole";
            this.Pole.Size = new System.Drawing.Size(102, 16);
            this.Pole.TabIndex = 29;
            this.toolTip1.SetToolTip(this.Pole, "45");
            this.Pole.Value = 45;
            this.Pole.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.Pole);
            this.groupBox4.Controls.Add(this.Equator);
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(0, 103);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(434, 74);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Climate";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(261, 44);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(34, 20);
            this.button2.TabIndex = 22;
            this.button2.Text = "RND";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.RndPole_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(261, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(34, 20);
            this.button1.TabIndex = 21;
            this.button1.Text = "RND";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.RndEquator_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(301, 46);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(99, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "(in % of map height)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(301, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "(in % of map height)";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "No working folder settings found. Please, select a folder to store World Builder " +
                "data.";
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.tableLayoutPanel1);
            this.groupBox6.Location = new System.Drawing.Point(12, 10);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.groupBox6.Size = new System.Drawing.Size(434, 40);
            this.groupBox6.TabIndex = 12;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Presets";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.radioButton1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.radioButton2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.radioButton3, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 14);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(430, 25);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(3, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(81, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "World maps";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.PresetType_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(146, 3);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(102, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Adventure maps";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.PresetType_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(289, 3);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(83, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Custom map";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.PresetType_CheckedChanged);
            // 
            // CustomMap
            // 
            this.CustomMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomMap.Controls.Add(this.groupBox2);
            this.CustomMap.Controls.Add(this.groupBox4);
            this.CustomMap.Location = new System.Drawing.Point(12, 102);
            this.CustomMap.Name = "CustomMap";
            this.CustomMap.Size = new System.Drawing.Size(434, 179);
            this.CustomMap.TabIndex = 13;
            // 
            // Presets
            // 
            this.Presets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Presets.Controls.Add(this.listBox1);
            this.Presets.Controls.Add(this.label1);
            this.Presets.Location = new System.Drawing.Point(316, 410);
            this.Presets.Name = "Presets";
            this.Presets.Size = new System.Drawing.Size(434, 179);
            this.Presets.TabIndex = 14;
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Items.AddRange(new object[] {
            "Continents",
            "Gondwana",
            "Archipelago"});
            this.listBox1.Location = new System.Drawing.Point(3, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(151, 172);
            this.listBox1.TabIndex = 9;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.Location = new System.Drawing.Point(167, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(258, 172);
            this.label1.TabIndex = 8;
            this.label1.Text = "label1";
            // 
            // GenerationForm
            // 
            this.AcceptButton = this.button8;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(458, 451);
            this.Controls.Add(this.Presets);
            this.Controls.Add(this.CustomMap);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenerationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Building World";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GenerationForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContinentsCount)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.CustomMap.ResumeLayout(false);
            this.Presets.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.CheckBox PartialMap;
        private System.Windows.Forms.NumericUpDown ContinentsCount;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button ContinentsRnd;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.HScrollBar StatesCount;
        private System.Windows.Forms.HScrollBar ProvinciesCount;
        private System.Windows.Forms.HScrollBar RacesCount;
        private System.Windows.Forms.HScrollBar WaterPercent;
        private System.Windows.Forms.HScrollBar LandMassesCount;
        private System.Windows.Forms.HScrollBar LandsCount;
        private System.Windows.Forms.HScrollBar Pole;
        private System.Windows.Forms.HScrollBar Equator;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Panel CustomMap;
        private System.Windows.Forms.Panel Presets;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
    }
}