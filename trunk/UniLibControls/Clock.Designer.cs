namespace MiscControls
{
    partial class Clock
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TimeSecondsLabel = new MiscControls.BorderLabel();
            this.TimeHoursLabel = new System.Windows.Forms.Label();
            this.TimeMinutesLabel = new System.Windows.Forms.Label();
            this.DayLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.DayLabel, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(140, 20);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Cyan;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Day";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TimeSecondsLabel);
            this.panel1.Controls.Add(this.TimeHoursLabel);
            this.panel1.Controls.Add(this.TimeMinutesLabel);
            this.panel1.Location = new System.Drawing.Point(93, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(47, 17);
            this.panel1.TabIndex = 4;
            // 
            // TimeSecondsLabel
            // 
            this.TimeSecondsLabel.BorderColor = System.Drawing.Color.SteelBlue;
            this.TimeSecondsLabel.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TimeSecondsLabel.ForeColor = System.Drawing.Color.Cyan;
            this.TimeSecondsLabel.Location = new System.Drawing.Point(20, -2);
            this.TimeSecondsLabel.Name = "TimeSecondsLabel";
            this.TimeSecondsLabel.Size = new System.Drawing.Size(4, 17);
            this.TimeSecondsLabel.TabIndex = 5;
            this.TimeSecondsLabel.Text = ":";
            this.TimeSecondsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // TimeHoursLabel
            // 
            this.TimeHoursLabel.AutoSize = true;
            this.TimeHoursLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.TimeHoursLabel.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TimeHoursLabel.ForeColor = System.Drawing.Color.Cyan;
            this.TimeHoursLabel.Location = new System.Drawing.Point(0, 0);
            this.TimeHoursLabel.Margin = new System.Windows.Forms.Padding(0);
            this.TimeHoursLabel.Name = "TimeHoursLabel";
            this.TimeHoursLabel.Size = new System.Drawing.Size(24, 17);
            this.TimeHoursLabel.TabIndex = 2;
            this.TimeHoursLabel.Text = "23";
            this.TimeHoursLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TimeMinutesLabel
            // 
            this.TimeMinutesLabel.AutoSize = true;
            this.TimeMinutesLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.TimeMinutesLabel.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TimeMinutesLabel.ForeColor = System.Drawing.Color.Cyan;
            this.TimeMinutesLabel.Location = new System.Drawing.Point(23, 0);
            this.TimeMinutesLabel.Margin = new System.Windows.Forms.Padding(0);
            this.TimeMinutesLabel.Name = "TimeMinutesLabel";
            this.TimeMinutesLabel.Size = new System.Drawing.Size(24, 17);
            this.TimeMinutesLabel.TabIndex = 4;
            this.TimeMinutesLabel.Text = "59";
            this.TimeMinutesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DayLabel
            // 
            this.DayLabel.AutoSize = true;
            this.DayLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DayLabel.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DayLabel.ForeColor = System.Drawing.Color.Cyan;
            this.DayLabel.Location = new System.Drawing.Point(48, 0);
            this.DayLabel.Name = "DayLabel";
            this.DayLabel.Size = new System.Drawing.Size(42, 20);
            this.DayLabel.TabIndex = 1;
            this.DayLabel.Text = "001";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 10;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Clock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Clock";
            this.Size = new System.Drawing.Size(140, 20);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label DayLabel;
        private System.Windows.Forms.Label TimeHoursLabel;
        private System.Windows.Forms.Label TimeMinutesLabel;
        private System.Windows.Forms.Panel panel1;
        private BorderLabel TimeSecondsLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
    }
}
