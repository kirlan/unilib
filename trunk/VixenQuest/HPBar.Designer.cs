namespace VixenQuest
{
    partial class HPBar
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.glassProgressBar1 = new MiscControls.GlassProgressBar();
            this.borderLabel1 = new MiscControls.BorderLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.glassProgressBar1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.borderLabel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(208, 59);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // glassProgressBar1
            // 
            this.glassProgressBar1.Color00 = System.Drawing.SystemColors.Control;
            this.glassProgressBar1.Color01 = System.Drawing.Color.Crimson;
            this.glassProgressBar1.Color02 = System.Drawing.Color.Red;
            this.glassProgressBar1.Color03 = System.Drawing.Color.OrangeRed;
            this.glassProgressBar1.Color04 = System.Drawing.Color.Orange;
            this.glassProgressBar1.Color05 = System.Drawing.Color.Gold;
            this.glassProgressBar1.Color06 = System.Drawing.Color.Yellow;
            this.glassProgressBar1.Color07 = System.Drawing.Color.GreenYellow;
            this.glassProgressBar1.Color08 = System.Drawing.Color.Chartreuse;
            this.glassProgressBar1.Color09 = System.Drawing.Color.LawnGreen;
            this.glassProgressBar1.Color10 = System.Drawing.Color.Lime;
            this.tableLayoutPanel1.SetColumnSpan(this.glassProgressBar1, 2);
            this.glassProgressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glassProgressBar1.ForeColor = System.Drawing.Color.White;
            this.glassProgressBar1.Location = new System.Drawing.Point(3, 32);
            this.glassProgressBar1.Name = "glassProgressBar1";
            this.glassProgressBar1.OuterBorderColor = System.Drawing.Color.Black;
            this.glassProgressBar1.Size = new System.Drawing.Size(132, 24);
            this.glassProgressBar1.TabIndex = 2;
            this.glassProgressBar1.ToolTip = "";
            // 
            // borderLabel1
            // 
            this.borderLabel1.BorderColor = System.Drawing.Color.Silver;
            this.tableLayoutPanel1.SetColumnSpan(this.borderLabel1, 3);
            this.borderLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderLabel1.Location = new System.Drawing.Point(3, 0);
            this.borderLabel1.Name = "borderLabel1";
            this.borderLabel1.Size = new System.Drawing.Size(202, 29);
            this.borderLabel1.TabIndex = 1;
            this.borderLabel1.Text = "borderLabel1";
            this.borderLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 500;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // HPBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "HPBar";
            this.Size = new System.Drawing.Size(208, 59);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MiscControls.BorderLabel borderLabel1;
        private MiscControls.GlassProgressBar glassProgressBar1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
    }
}
