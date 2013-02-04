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
            this.cubePlanetDraw3d1 = new TestCubePlanet.CubePlanetDraw3d();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 284);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 39);
            this.panel1.TabIndex = 1;
            // 
            // cubePlanetDraw3d1
            // 
            this.cubePlanetDraw3d1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cubePlanetDraw3d1.Location = new System.Drawing.Point(0, 0);
            this.cubePlanetDraw3d1.Name = "cubePlanetDraw3d1";
            this.cubePlanetDraw3d1.Size = new System.Drawing.Size(284, 284);
            this.cubePlanetDraw3d1.TabIndex = 0;
            this.cubePlanetDraw3d1.Text = "cubePlanetDraw3d1";
            // 
            // CubePlanetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 323);
            this.Controls.Add(this.cubePlanetDraw3d1);
            this.Controls.Add(this.panel1);
            this.Name = "CubePlanetForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private CubePlanetDraw3d cubePlanetDraw3d1;
        private System.Windows.Forms.Panel panel1;
    }
}

