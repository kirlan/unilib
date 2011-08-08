namespace MapVis
{
    partial class FractalMapView2D
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
            this.SuspendLayout();
            // 
            // FractalMapView2D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "FractalMapView2D";
            this.Size = new System.Drawing.Size(425, 219);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FractalMapView_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FractalMapView2D_MouseMove);
            this.Resize += new System.EventHandler(this.FractalMapView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
