namespace MapDrawEngine
{
    partial class MapDraw
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 500000;
            this.toolTip1.InitialDelay = 0;
            this.toolTip1.ReshowDelay = 0;
            // 
            // MapDraw
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "MapDraw";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MapDraw_Paint);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MapDraw_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapDraw_MouseDown);
            this.MouseLeave += new System.EventHandler(this.MapDraw_MouseLeave);
            this.MouseHover += new System.EventHandler(this.MapDraw_MouseHover);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapDraw_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapDraw_MouseUp);
            this.Resize += new System.EventHandler(this.MapDraw_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
    }
}
