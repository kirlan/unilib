namespace Persona.Core.Controls
{
    partial class DescriptionEdit
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
            this.contextMenuStrip_Strings = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.InsertStringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InsertNumericToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.отменаToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip_Strings.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_Strings
            // 
            this.contextMenuStrip_Strings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InsertStringToolStripMenuItem,
            this.InsertNumericToolStripMenuItem,
            this.toolStripMenuItem3,
            this.отменаToolStripMenuItem2});
            this.contextMenuStrip_Strings.Name = "contextMenuStrip_Strings";
            this.contextMenuStrip_Strings.Size = new System.Drawing.Size(242, 98);
            // 
            // InsertStringToolStripMenuItem
            // 
            this.InsertStringToolStripMenuItem.Name = "InsertStringToolStripMenuItem";
            this.InsertStringToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.InsertStringToolStripMenuItem.Text = "Вставить строковый параметр";
            // 
            // InsertNumericToolStripMenuItem
            // 
            this.InsertNumericToolStripMenuItem.Name = "InsertNumericToolStripMenuItem";
            this.InsertNumericToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.InsertNumericToolStripMenuItem.Text = "Вставить числовой параметр";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(238, 6);
            // 
            // отменаToolStripMenuItem2
            // 
            this.отменаToolStripMenuItem2.Name = "отменаToolStripMenuItem2";
            this.отменаToolStripMenuItem2.Size = new System.Drawing.Size(241, 22);
            this.отменаToolStripMenuItem2.Text = "Отмена";
            // 
            // textBox2
            // 
            this.textBox2.ContextMenuStrip = this.contextMenuStrip_Strings;
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(0, 0);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(150, 150);
            this.textBox2.TabIndex = 8;
            // 
            // DescriptionEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox2);
            this.Name = "DescriptionEdit";
            this.contextMenuStrip_Strings.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Strings;
        private System.Windows.Forms.ToolStripMenuItem InsertStringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem InsertNumericToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem отменаToolStripMenuItem2;
        private System.Windows.Forms.TextBox textBox2;
    }
}
