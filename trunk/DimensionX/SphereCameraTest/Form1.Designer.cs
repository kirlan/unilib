namespace SphereCameraTest
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
            this._3dview1 = new SphereCameraTest._3dview();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // _3dview1
            // 
            this._3dview1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._3dview1.Location = new System.Drawing.Point(12, 12);
            this._3dview1.Name = "_3dview1";
            this._3dview1.Size = new System.Drawing.Size(592, 460);
            this._3dview1.TabIndex = 0;
            this._3dview1.Text = "_3dview1";
            this._3dview1.MouseDown += new System.Windows.Forms.MouseEventHandler(this._3dview1_MouseDown);
            this._3dview1.MouseEnter += new System.EventHandler(this._3dview1_MouseEnter);
            this._3dview1.MouseLeave += new System.EventHandler(this._3dview1_MouseLeave);
            this._3dview1.MouseMove += new System.Windows.Forms.MouseEventHandler(this._3dview1_MouseMove);
            this._3dview1.MouseUp += new System.Windows.Forms.MouseEventHandler(this._3dview1_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 484);
            this.Controls.Add(this._3dview1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private _3dview _3dview1;
        private System.Windows.Forms.Timer timer1;
    }
}

