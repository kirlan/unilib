namespace TestCubePlanet
{
    partial class CubePlanetForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonUP = new System.Windows.Forms.RadioButton();
            this.radioButtonFW = new System.Windows.Forms.RadioButton();
            this.radioButtonDW = new System.Windows.Forms.RadioButton();
            this.radioButtonRT = new System.Windows.Forms.RadioButton();
            this.radioButtonLF = new System.Windows.Forms.RadioButton();
            this.radioButtonBK = new System.Windows.Forms.RadioButton();
            this.radioButtonN = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.cubePlanetDraw3d1 = new TestCubePlanet.CubePlanetDraw3d();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 357);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(392, 81);
            this.panel1.TabIndex = 1;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(331, 58);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(49, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "color";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.radioButtonUP, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonFW, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonDW, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonRT, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonLF, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonBK, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonN, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(145, 6);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(235, 50);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // radioButtonUP
            // 
            this.radioButtonUP.AutoSize = true;
            this.radioButtonUP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonUP.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonUP.Location = new System.Drawing.Point(116, 0);
            this.radioButtonUP.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonUP.Name = "radioButtonUP";
            this.radioButtonUP.Size = new System.Drawing.Size(58, 16);
            this.radioButtonUP.TabIndex = 0;
            this.radioButtonUP.Text = "top";
            this.radioButtonUP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonUP.UseVisualStyleBackColor = true;
            this.radioButtonUP.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButtonFW
            // 
            this.radioButtonFW.AutoSize = true;
            this.radioButtonFW.Checked = true;
            this.radioButtonFW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonFW.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonFW.Location = new System.Drawing.Point(116, 16);
            this.radioButtonFW.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonFW.Name = "radioButtonFW";
            this.radioButtonFW.Size = new System.Drawing.Size(58, 16);
            this.radioButtonFW.TabIndex = 1;
            this.radioButtonFW.TabStop = true;
            this.radioButtonFW.Text = "forward";
            this.radioButtonFW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonFW.UseVisualStyleBackColor = true;
            this.radioButtonFW.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButtonDW
            // 
            this.radioButtonDW.AutoSize = true;
            this.radioButtonDW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonDW.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonDW.Location = new System.Drawing.Point(116, 32);
            this.radioButtonDW.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonDW.Name = "radioButtonDW";
            this.radioButtonDW.Size = new System.Drawing.Size(58, 16);
            this.radioButtonDW.TabIndex = 2;
            this.radioButtonDW.Text = "bottom";
            this.radioButtonDW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonDW.UseVisualStyleBackColor = true;
            this.radioButtonDW.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButtonRT
            // 
            this.radioButtonRT.AutoSize = true;
            this.radioButtonRT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonRT.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonRT.Location = new System.Drawing.Point(174, 16);
            this.radioButtonRT.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonRT.Name = "radioButtonRT";
            this.radioButtonRT.Size = new System.Drawing.Size(61, 16);
            this.radioButtonRT.TabIndex = 3;
            this.radioButtonRT.Text = "right";
            this.radioButtonRT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonRT.UseVisualStyleBackColor = true;
            this.radioButtonRT.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButtonLF
            // 
            this.radioButtonLF.AutoSize = true;
            this.radioButtonLF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonLF.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonLF.Location = new System.Drawing.Point(58, 16);
            this.radioButtonLF.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonLF.Name = "radioButtonLF";
            this.radioButtonLF.Size = new System.Drawing.Size(58, 16);
            this.radioButtonLF.TabIndex = 4;
            this.radioButtonLF.Text = "left";
            this.radioButtonLF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonLF.UseVisualStyleBackColor = true;
            this.radioButtonLF.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButtonBK
            // 
            this.radioButtonBK.AutoSize = true;
            this.radioButtonBK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonBK.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonBK.Location = new System.Drawing.Point(0, 16);
            this.radioButtonBK.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonBK.Name = "radioButtonBK";
            this.radioButtonBK.Size = new System.Drawing.Size(58, 16);
            this.radioButtonBK.TabIndex = 5;
            this.radioButtonBK.Text = "back";
            this.radioButtonBK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonBK.UseVisualStyleBackColor = true;
            this.radioButtonBK.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButtonN
            // 
            this.radioButtonN.AutoSize = true;
            this.radioButtonN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonN.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonN.Location = new System.Drawing.Point(174, 0);
            this.radioButtonN.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonN.Name = "radioButtonN";
            this.radioButtonN.Size = new System.Drawing.Size(61, 16);
            this.radioButtonN.TabIndex = 6;
            this.radioButtonN.Text = "north";
            this.radioButtonN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonN.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(190, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Graphic init time:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Building time:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(76, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(52, 47);
            this.button2.TabIndex = 3;
            this.button2.Text = "BUILD!";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "small",
            "normal",
            "big",
            "giant"});
            this.comboBox1.Location = new System.Drawing.Point(12, 32);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(58, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(12, 6);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(58, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // cubePlanetDraw3d1
            // 
            this.cubePlanetDraw3d1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cubePlanetDraw3d1.Location = new System.Drawing.Point(0, 0);
            this.cubePlanetDraw3d1.Name = "cubePlanetDraw3d1";
            this.cubePlanetDraw3d1.Size = new System.Drawing.Size(392, 357);
            this.cubePlanetDraw3d1.TabIndex = 0;
            this.cubePlanetDraw3d1.Text = "cubePlanetDraw3d1";
            this.cubePlanetDraw3d1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.cubePlanetDraw3d1_MouseMove);
            // 
            // CubePlanetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 438);
            this.Controls.Add(this.cubePlanetDraw3d1);
            this.Controls.Add(this.panel1);
            this.Name = "CubePlanetForm";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CubePlanetDraw3d cubePlanetDraw3d1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton radioButtonUP;
        private System.Windows.Forms.RadioButton radioButtonFW;
        private System.Windows.Forms.RadioButton radioButtonDW;
        private System.Windows.Forms.RadioButton radioButtonRT;
        private System.Windows.Forms.RadioButton radioButtonLF;
        private System.Windows.Forms.RadioButton radioButtonBK;
        private System.Windows.Forms.RadioButton radioButtonN;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

