namespace PickerTest
{
    partial class Form1
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
            this.mapDraw3d1 = new PickerTest.MapDraw3d();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mapDraw3d1
            // 
            this.mapDraw3d1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mapDraw3d1.Location = new System.Drawing.Point(0, 0);
            this.mapDraw3d1.Name = "mapDraw3d1";
            this.mapDraw3d1.Size = new System.Drawing.Size(284, 262);
            this.mapDraw3d1.TabIndex = 0;
            this.mapDraw3d1.Text = "mapDraw3d1";
            this.mapDraw3d1.Click += new System.EventHandler(this.mapDraw3d1_Click);
            this.mapDraw3d1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapDraw3d1_MouseMove);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(0, 269);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 290);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mapDraw3d1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private MapDraw3d mapDraw3d1;
        private System.Windows.Forms.Label label1;
    }
}

