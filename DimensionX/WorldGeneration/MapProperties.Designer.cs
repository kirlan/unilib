namespace WorldGeneration
{
    partial class MapProperties
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PresetsPanel = new System.Windows.Forms.Panel();
            this.MapPresetDescription = new System.Windows.Forms.Label();
            this.MapPresets = new System.Windows.Forms.ListBox();
            this.AdvancedPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LandsCountBar = new System.Windows.Forms.HScrollBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ContinentsCountEdit = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.WaterPercentBar = new System.Windows.Forms.HScrollBar();
            this.label13 = new System.Windows.Forms.Label();
            this.PartialMapBox = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.LandMassesCountBar = new System.Windows.Forms.HScrollBar();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.EquatorBar = new System.Windows.Forms.HScrollBar();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.PoleBar = new System.Windows.Forms.HScrollBar();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.StatesCountBar = new System.Windows.Forms.HScrollBar();
            this.label15 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.PresetsPanel.SuspendLayout();
            this.AdvancedPanel.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContinentsCountEdit)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // PresetsPanel
            // 
            this.PresetsPanel.Controls.Add(this.MapPresetDescription);
            this.PresetsPanel.Controls.Add(this.MapPresets);
            this.PresetsPanel.Location = new System.Drawing.Point(47, 47);
            this.PresetsPanel.Name = "PresetsPanel";
            this.PresetsPanel.Padding = new System.Windows.Forms.Padding(11, 5, 5, 5);
            this.PresetsPanel.Size = new System.Drawing.Size(200, 100);
            this.PresetsPanel.TabIndex = 0;
            this.PresetsPanel.Resize += new System.EventHandler(this.PresetsPanel_Resize);
            // 
            // MapPresetDescription
            // 
            this.MapPresetDescription.AutoEllipsis = true;
            this.MapPresetDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapPresetDescription.Location = new System.Drawing.Point(121, 5);
            this.MapPresetDescription.Name = "MapPresetDescription";
            this.MapPresetDescription.Padding = new System.Windows.Forms.Padding(5);
            this.MapPresetDescription.Size = new System.Drawing.Size(74, 90);
            this.MapPresetDescription.TabIndex = 10;
            this.MapPresetDescription.Text = "label1";
            // 
            // MapPresets
            // 
            this.MapPresets.Dock = System.Windows.Forms.DockStyle.Left;
            this.MapPresets.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MapPresets.FormattingEnabled = true;
            this.MapPresets.IntegralHeight = false;
            this.MapPresets.ItemHeight = 16;
            this.MapPresets.Items.AddRange(new object[] {
            "Continents",
            "Gondwana",
            "Archipelago"});
            this.MapPresets.Location = new System.Drawing.Point(11, 5);
            this.MapPresets.Name = "MapPresets";
            this.MapPresets.ScrollAlwaysVisible = true;
            this.MapPresets.Size = new System.Drawing.Size(110, 90);
            this.MapPresets.TabIndex = 11;
            this.MapPresets.SelectedIndexChanged += new System.EventHandler(this.MapPresets_SelectedIndexChanged);
            // 
            // AdvancedPanel
            // 
            this.AdvancedPanel.Controls.Add(this.tableLayoutPanel3);
            this.AdvancedPanel.Location = new System.Drawing.Point(3, 96);
            this.AdvancedPanel.Name = "AdvancedPanel";
            this.AdvancedPanel.Size = new System.Drawing.Size(502, 219);
            this.AdvancedPanel.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox4, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.panel7, 0, 7);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 9;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(502, 219);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.tableLayoutPanel3.SetRowSpan(this.groupBox2, 4);
            this.groupBox2.Size = new System.Drawing.Size(496, 102);
            this.groupBox2.TabIndex = 44;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Landscape";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label14, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.PartialMapBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(490, 83);
            this.tableLayoutPanel1.TabIndex = 49;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LandsCountBar);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(239, 19);
            this.panel1.TabIndex = 0;
            // 
            // LandsCountBar
            // 
            this.LandsCountBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LandsCountBar.LargeChange = 20;
            this.LandsCountBar.Location = new System.Drawing.Point(103, 0);
            this.LandsCountBar.Minimum = 1;
            this.LandsCountBar.Name = "LandsCountBar";
            this.LandsCountBar.Size = new System.Drawing.Size(136, 19);
            this.LandsCountBar.TabIndex = 27;
            this.LandsCountBar.Value = 40;
            this.LandsCountBar.ValueChanged += new System.EventHandler(this.ScrollBar_ValueChanged);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 19);
            this.label3.TabIndex = 1;
            this.label3.Text = "Lands complexity:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Location = new System.Drawing.Point(248, 53);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(239, 25);
            this.label14.TabIndex = 15;
            this.label14.Text = "(in % of total map area)";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ContinentsCountEdit);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(239, 19);
            this.panel2.TabIndex = 1;
            // 
            // ContinentsCountEdit
            // 
            this.ContinentsCountEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContinentsCountEdit.Location = new System.Drawing.Point(90, 0);
            this.ContinentsCountEdit.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.ContinentsCountEdit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ContinentsCountEdit.Name = "ContinentsCountEdit";
            this.ContinentsCountEdit.Size = new System.Drawing.Size(149, 20);
            this.ContinentsCountEdit.TabIndex = 18;
            this.ContinentsCountEdit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ContinentsCountEdit.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.Dock = System.Windows.Forms.DockStyle.Left;
            this.label16.Location = new System.Drawing.Point(0, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(90, 19);
            this.label16.TabIndex = 17;
            this.label16.Text = "Continents count:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.WaterPercentBar);
            this.panel3.Controls.Add(this.label13);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 56);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(239, 19);
            this.panel3.TabIndex = 2;
            // 
            // WaterPercentBar
            // 
            this.WaterPercentBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WaterPercentBar.LargeChange = 5;
            this.WaterPercentBar.Location = new System.Drawing.Point(87, 0);
            this.WaterPercentBar.Maximum = 94;
            this.WaterPercentBar.Minimum = 10;
            this.WaterPercentBar.Name = "WaterPercentBar";
            this.WaterPercentBar.Size = new System.Drawing.Size(152, 19);
            this.WaterPercentBar.TabIndex = 29;
            this.WaterPercentBar.Value = 66;
            this.WaterPercentBar.ValueChanged += new System.EventHandler(this.ScrollBar_ValueChanged);
            // 
            // label13
            // 
            this.label13.Dock = System.Windows.Forms.DockStyle.Left;
            this.label13.Location = new System.Drawing.Point(0, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(87, 19);
            this.label13.TabIndex = 12;
            this.label13.Text = "Water coverage:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PartialMapBox
            // 
            this.PartialMapBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PartialMapBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PartialMapBox.Location = new System.Drawing.Point(245, 31);
            this.PartialMapBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.PartialMapBox.Name = "PartialMapBox";
            this.PartialMapBox.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.PartialMapBox.Size = new System.Drawing.Size(245, 19);
            this.PartialMapBox.TabIndex = 20;
            this.PartialMapBox.Text = "Continents could touch map border";
            this.PartialMapBox.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.LandMassesCountBar);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(248, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(239, 19);
            this.panel4.TabIndex = 21;
            // 
            // LandMassesCountBar
            // 
            this.LandMassesCountBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LandMassesCountBar.Location = new System.Drawing.Point(109, 0);
            this.LandMassesCountBar.Minimum = 1;
            this.LandMassesCountBar.Name = "LandMassesCountBar";
            this.LandMassesCountBar.Size = new System.Drawing.Size(130, 19);
            this.LandMassesCountBar.TabIndex = 28;
            this.LandMassesCountBar.Value = 50;
            this.LandMassesCountBar.ValueChanged += new System.EventHandler(this.ScrollBar_ValueChanged);
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 19);
            this.label5.TabIndex = 3;
            this.label5.Text = "Coastline complexity:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tableLayoutPanel2);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 111);
            this.groupBox4.Name = "groupBox4";
            this.tableLayoutPanel3.SetRowSpan(this.groupBox4, 3);
            this.groupBox4.Size = new System.Drawing.Size(496, 75);
            this.groupBox4.TabIndex = 45;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Climate";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel5, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel6, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(490, 56);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.EquatorBar);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(484, 22);
            this.panel5.TabIndex = 0;
            // 
            // EquatorBar
            // 
            this.EquatorBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EquatorBar.LargeChange = 5;
            this.EquatorBar.Location = new System.Drawing.Point(150, 0);
            this.EquatorBar.Maximum = 154;
            this.EquatorBar.Minimum = -50;
            this.EquatorBar.Name = "EquatorBar";
            this.EquatorBar.Size = new System.Drawing.Size(235, 22);
            this.EquatorBar.TabIndex = 28;
            this.EquatorBar.Value = 50;
            this.EquatorBar.ValueChanged += new System.EventHandler(this.ScrollBar_ValueChanged);
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 22);
            this.label6.TabIndex = 15;
            this.label6.Text = "Equator position from map top:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Right;
            this.label7.Location = new System.Drawing.Point(385, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 22);
            this.label7.TabIndex = 17;
            this.label7.Text = "(in % of map height)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.PoleBar);
            this.panel6.Controls.Add(this.label8);
            this.panel6.Controls.Add(this.label9);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 31);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(484, 22);
            this.panel6.TabIndex = 1;
            // 
            // PoleBar
            // 
            this.PoleBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PoleBar.LargeChange = 5;
            this.PoleBar.Location = new System.Drawing.Point(125, 0);
            this.PoleBar.Maximum = 229;
            this.PoleBar.Minimum = 10;
            this.PoleBar.Name = "PoleBar";
            this.PoleBar.Size = new System.Drawing.Size(260, 22);
            this.PoleBar.TabIndex = 29;
            this.PoleBar.Value = 45;
            this.PoleBar.ValueChanged += new System.EventHandler(this.ScrollBar_ValueChanged);
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Left;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(125, 22);
            this.label8.TabIndex = 18;
            this.label8.Text = "Pole to equator distance:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Right;
            this.label9.Location = new System.Drawing.Point(385, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(99, 22);
            this.label9.TabIndex = 20;
            this.label9.Text = "(in % of map height)";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.StatesCountBar);
            this.panel7.Controls.Add(this.label15);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 192);
            this.panel7.Name = "panel7";
            this.panel7.Padding = new System.Windows.Forms.Padding(6, 1, 6, 0);
            this.panel7.Size = new System.Drawing.Size(496, 21);
            this.panel7.TabIndex = 49;
            // 
            // StatesCountBar
            // 
            this.StatesCountBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatesCountBar.LargeChange = 5;
            this.StatesCountBar.Location = new System.Drawing.Point(76, 1);
            this.StatesCountBar.Maximum = 25;
            this.StatesCountBar.Minimum = 1;
            this.StatesCountBar.Name = "StatesCountBar";
            this.StatesCountBar.Size = new System.Drawing.Size(414, 20);
            this.StatesCountBar.TabIndex = 48;
            this.StatesCountBar.Value = 8;
            this.StatesCountBar.ValueChanged += new System.EventHandler(this.ScrollBar_ValueChanged);
            // 
            // label15
            // 
            this.label15.Dock = System.Windows.Forms.DockStyle.Left;
            this.label15.Location = new System.Drawing.Point(6, 1);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(70, 20);
            this.label15.TabIndex = 46;
            this.label15.Text = "States count:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MapProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.AdvancedPanel);
            this.Controls.Add(this.PresetsPanel);
            this.Name = "MapProperties";
            this.Size = new System.Drawing.Size(551, 325);
            this.Resize += new System.EventHandler(this.MapProperties_Resize);
            this.PresetsPanel.ResumeLayout(false);
            this.AdvancedPanel.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ContinentsCountEdit)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PresetsPanel;
        private System.Windows.Forms.Panel AdvancedPanel;
        private System.Windows.Forms.Label MapPresetDescription;
        private System.Windows.Forms.ListBox MapPresets;
        private System.Windows.Forms.HScrollBar StatesCountBar;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.HScrollBar PoleBar;
        private System.Windows.Forms.HScrollBar EquatorBar;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.HScrollBar WaterPercentBar;
        private System.Windows.Forms.HScrollBar LandMassesCountBar;
        private System.Windows.Forms.HScrollBar LandsCountBar;
        private System.Windows.Forms.NumericUpDown ContinentsCountEdit;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox PartialMapBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
