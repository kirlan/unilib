﻿namespace GridBuilderTest
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
            this.mapDraw3d1 = new XNAEngine.GBTestMapDraw3d();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mapDraw3d1
            // 
            this.mapDraw3d1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mapDraw3d1.Location = new System.Drawing.Point(12, 51);
            this.mapDraw3d1.Name = "mapDraw3d1";
            this.mapDraw3d1.Size = new System.Drawing.Size(725, 526);
            this.mapDraw3d1.TabIndex = 0;
            this.mapDraw3d1.Text = "mapDraw3d1";
            this.mapDraw3d1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapDraw3d1_MouseDown);
            this.mapDraw3d1.MouseEnter += new System.EventHandler(this.mapDraw3d1_MouseEnter);
            this.mapDraw3d1.MouseLeave += new System.EventHandler(this.mapDraw3d1_MouseLeave);
            this.mapDraw3d1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapDraw3d1_MouseMove);
            this.mapDraw3d1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mapDraw3d1_MouseUp);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(662, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 589);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mapDraw3d1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private XNAEngine.GBTestMapDraw3d mapDraw3d1;
        private System.Windows.Forms.Button button1;
    }
}

