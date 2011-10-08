namespace GMs_Desktop
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newAdventureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadAdventureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAdventureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.characterCreatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gadgetCreatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newAdventureToolStripMenuItem,
            this.loadAdventureToolStripMenuItem,
            this.saveAdventureToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileToolStripMenuItem.Text = "File";
            this.FileToolStripMenuItem.Click += new System.EventHandler(this.adventureToolStripMenuItem_Click);
            // 
            // newAdventureToolStripMenuItem
            // 
            this.newAdventureToolStripMenuItem.Name = "newAdventureToolStripMenuItem";
            this.newAdventureToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.newAdventureToolStripMenuItem.Text = "New Adventure";
            // 
            // loadAdventureToolStripMenuItem
            // 
            this.loadAdventureToolStripMenuItem.Name = "loadAdventureToolStripMenuItem";
            this.loadAdventureToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.loadAdventureToolStripMenuItem.Text = "Load Adventure";
            // 
            // saveAdventureToolStripMenuItem
            // 
            this.saveAdventureToolStripMenuItem.Name = "saveAdventureToolStripMenuItem";
            this.saveAdventureToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.saveAdventureToolStripMenuItem.Text = "Save Adventure";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.characterCreatorToolStripMenuItem,
            this.gadgetCreatorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // characterCreatorToolStripMenuItem
            // 
            this.characterCreatorToolStripMenuItem.Name = "characterCreatorToolStripMenuItem";
            this.characterCreatorToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.characterCreatorToolStripMenuItem.Text = "Character Creator";
            this.characterCreatorToolStripMenuItem.Click += new System.EventHandler(this.characterCreatorToolStripMenuItem_Click);
            // 
            // gadgetCreatorToolStripMenuItem
            // 
            this.gadgetCreatorToolStripMenuItem.Name = "gadgetCreatorToolStripMenuItem";
            this.gadgetCreatorToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.gadgetCreatorToolStripMenuItem.Text = "Gadget Creator";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newAdventureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadAdventureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAdventureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem characterCreatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gadgetCreatorToolStripMenuItem;
    }
}

