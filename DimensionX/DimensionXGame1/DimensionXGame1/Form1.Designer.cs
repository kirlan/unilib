namespace DimensionXGame1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.worldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.repeatCreationFromPresetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLabelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.villagesHamletsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.townsFortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.citiesCapitalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.showAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.showLandmarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showRoadsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showStateBordersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showProvinciesBordersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.debugToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.showLocationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLandMassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.testPathFinding1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useCelshadingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cubePlanetDraw3d1 = new PlanetDrawXNAEngine.CubePlanetDraw3d();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.showBirdviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showBoundsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.worldToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.debugToolStripMenuItem2});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(646, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // worldToolStripMenuItem
            // 
            this.worldToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.repeatCreationFromPresetToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.worldToolStripMenuItem.Name = "worldToolStripMenuItem";
            this.worldToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.worldToolStripMenuItem.Text = "World";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.newToolStripMenuItem.Text = "New...";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // repeatCreationFromPresetToolStripMenuItem
            // 
            this.repeatCreationFromPresetToolStripMenuItem.Name = "repeatCreationFromPresetToolStripMenuItem";
            this.repeatCreationFromPresetToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.repeatCreationFromPresetToolStripMenuItem.Text = "Repeat creation from preset";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.saveToolStripMenuItem.Text = "Save...";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(217, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showLabelsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.showLandmarksToolStripMenuItem,
            this.showRoadsToolStripMenuItem,
            this.showStateBordersToolStripMenuItem,
            this.showProvinciesBordersToolStripMenuItem,
            this.toolStripSeparator2,
            this.debugToolStripMenuItem1});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.debugToolStripMenuItem.Text = "View";
            // 
            // showLabelsToolStripMenuItem
            // 
            this.showLabelsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.villagesHamletsToolStripMenuItem,
            this.townsFortsToolStripMenuItem,
            this.citiesCapitalsToolStripMenuItem,
            this.toolStripSeparator3,
            this.showAllToolStripMenuItem,
            this.hideAllToolStripMenuItem});
            this.showLabelsToolStripMenuItem.Name = "showLabelsToolStripMenuItem";
            this.showLabelsToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.showLabelsToolStripMenuItem.Text = "Show Labels";
            // 
            // villagesHamletsToolStripMenuItem
            // 
            this.villagesHamletsToolStripMenuItem.Checked = true;
            this.villagesHamletsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.villagesHamletsToolStripMenuItem.Name = "villagesHamletsToolStripMenuItem";
            this.villagesHamletsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.villagesHamletsToolStripMenuItem.Text = "Villages && Hamlets";
            // 
            // townsFortsToolStripMenuItem
            // 
            this.townsFortsToolStripMenuItem.Checked = true;
            this.townsFortsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.townsFortsToolStripMenuItem.Name = "townsFortsToolStripMenuItem";
            this.townsFortsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.townsFortsToolStripMenuItem.Text = "Towns && Forts";
            // 
            // citiesCapitalsToolStripMenuItem
            // 
            this.citiesCapitalsToolStripMenuItem.Checked = true;
            this.citiesCapitalsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.citiesCapitalsToolStripMenuItem.Name = "citiesCapitalsToolStripMenuItem";
            this.citiesCapitalsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.citiesCapitalsToolStripMenuItem.Text = "Cities && Capitals";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(171, 6);
            // 
            // showAllToolStripMenuItem
            // 
            this.showAllToolStripMenuItem.Name = "showAllToolStripMenuItem";
            this.showAllToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.showAllToolStripMenuItem.Text = "Show All";
            // 
            // hideAllToolStripMenuItem
            // 
            this.hideAllToolStripMenuItem.Name = "hideAllToolStripMenuItem";
            this.hideAllToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.hideAllToolStripMenuItem.Text = "Hide All";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(200, 6);
            // 
            // showLandmarksToolStripMenuItem
            // 
            this.showLandmarksToolStripMenuItem.Checked = true;
            this.showLandmarksToolStripMenuItem.CheckOnClick = true;
            this.showLandmarksToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showLandmarksToolStripMenuItem.Name = "showLandmarksToolStripMenuItem";
            this.showLandmarksToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.showLandmarksToolStripMenuItem.Text = "Show Landmarks";
            // 
            // showRoadsToolStripMenuItem
            // 
            this.showRoadsToolStripMenuItem.Checked = true;
            this.showRoadsToolStripMenuItem.CheckOnClick = true;
            this.showRoadsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showRoadsToolStripMenuItem.Name = "showRoadsToolStripMenuItem";
            this.showRoadsToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.showRoadsToolStripMenuItem.Text = "Show Roads";
            // 
            // showStateBordersToolStripMenuItem
            // 
            this.showStateBordersToolStripMenuItem.CheckOnClick = true;
            this.showStateBordersToolStripMenuItem.Name = "showStateBordersToolStripMenuItem";
            this.showStateBordersToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.showStateBordersToolStripMenuItem.Text = "Show States Borders";
            // 
            // showProvinciesBordersToolStripMenuItem
            // 
            this.showProvinciesBordersToolStripMenuItem.CheckOnClick = true;
            this.showProvinciesBordersToolStripMenuItem.Name = "showProvinciesBordersToolStripMenuItem";
            this.showProvinciesBordersToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.showProvinciesBordersToolStripMenuItem.Text = "Show Provincies Borders";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(200, 6);
            // 
            // debugToolStripMenuItem1
            // 
            this.debugToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showLocationsToolStripMenuItem,
            this.showLandsToolStripMenuItem,
            this.showLandMassesToolStripMenuItem});
            this.debugToolStripMenuItem1.Name = "debugToolStripMenuItem1";
            this.debugToolStripMenuItem1.Size = new System.Drawing.Size(203, 22);
            this.debugToolStripMenuItem1.Text = "Debug";
            // 
            // showLocationsToolStripMenuItem
            // 
            this.showLocationsToolStripMenuItem.CheckOnClick = true;
            this.showLocationsToolStripMenuItem.Name = "showLocationsToolStripMenuItem";
            this.showLocationsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.showLocationsToolStripMenuItem.Text = "Show Locations";
            // 
            // showLandsToolStripMenuItem
            // 
            this.showLandsToolStripMenuItem.CheckOnClick = true;
            this.showLandsToolStripMenuItem.Name = "showLandsToolStripMenuItem";
            this.showLandsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.showLandsToolStripMenuItem.Text = "Show Lands";
            // 
            // showLandMassesToolStripMenuItem
            // 
            this.showLandMassesToolStripMenuItem.CheckOnClick = true;
            this.showLandMassesToolStripMenuItem.Name = "showLandMassesToolStripMenuItem";
            this.showLandMassesToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.showLandMassesToolStripMenuItem.Text = "Show Tectonic Plates";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Enabled = false;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // debugToolStripMenuItem2
            // 
            this.debugToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testPathFinding1ToolStripMenuItem,
            this.useCelshadingToolStripMenuItem,
            this.showBirdviewToolStripMenuItem,
            this.showBoundsToolStripMenuItem});
            this.debugToolStripMenuItem2.Name = "debugToolStripMenuItem2";
            this.debugToolStripMenuItem2.Size = new System.Drawing.Size(54, 20);
            this.debugToolStripMenuItem2.Text = "Debug";
            // 
            // testPathFinding1ToolStripMenuItem
            // 
            this.testPathFinding1ToolStripMenuItem.Name = "testPathFinding1ToolStripMenuItem";
            this.testPathFinding1ToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.testPathFinding1ToolStripMenuItem.Text = "Build random path";
            // 
            // useCelshadingToolStripMenuItem
            // 
            this.useCelshadingToolStripMenuItem.Checked = true;
            this.useCelshadingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useCelshadingToolStripMenuItem.Name = "useCelshadingToolStripMenuItem";
            this.useCelshadingToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.useCelshadingToolStripMenuItem.Text = "Use cel-shading";
            this.useCelshadingToolStripMenuItem.Click += new System.EventHandler(this.useCelshadingToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // cubePlanetDraw3d1
            // 
            this.cubePlanetDraw3d1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cubePlanetDraw3d1.Location = new System.Drawing.Point(0, 24);
            this.cubePlanetDraw3d1.LODDistance = 50F;
            this.cubePlanetDraw3d1.Name = "cubePlanetDraw3d1";
            this.cubePlanetDraw3d1.NeedDrawTrees = true;
            this.cubePlanetDraw3d1.ShowBounds = false;
            this.cubePlanetDraw3d1.ShowFrustum = false;
            this.cubePlanetDraw3d1.Size = new System.Drawing.Size(646, 361);
            this.cubePlanetDraw3d1.TabIndex = 0;
            this.cubePlanetDraw3d1.Text = "cubePlanetDraw3d1";
            this.cubePlanetDraw3d1.TimeSpeed = 6.25E-05F;
            this.cubePlanetDraw3d1.TimeWarp = true;
            this.cubePlanetDraw3d1.UseCelShading = true;
            this.cubePlanetDraw3d1.WireFrame = false;
            this.cubePlanetDraw3d1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cubePlanetDraw3d1_MouseDown);
            this.cubePlanetDraw3d1.MouseLeave += new System.EventHandler(this.cubePlanetDraw3d1_MouseLeave);
            this.cubePlanetDraw3d1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.cubePlanetDraw3d1_MouseMove);
            this.cubePlanetDraw3d1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cubePlanetDraw3d1_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 385);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(646, 77);
            this.panel1.TabIndex = 21;
            // 
            // showBirdviewToolStripMenuItem
            // 
            this.showBirdviewToolStripMenuItem.Name = "showBirdviewToolStripMenuItem";
            this.showBirdviewToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.showBirdviewToolStripMenuItem.Text = "Show bird-view";
            this.showBirdviewToolStripMenuItem.Click += new System.EventHandler(this.showBirdviewToolStripMenuItem_Click);
            // 
            // showBoundsToolStripMenuItem
            // 
            this.showBoundsToolStripMenuItem.Name = "showBoundsToolStripMenuItem";
            this.showBoundsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.showBoundsToolStripMenuItem.Text = "Show bounds";
            this.showBoundsToolStripMenuItem.Click += new System.EventHandler(this.showBoundsToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 462);
            this.Controls.Add(this.cubePlanetDraw3d1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PlanetDrawXNAEngine.CubePlanetDraw3d cubePlanetDraw3d1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem worldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem repeatCreationFromPresetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLabelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem villagesHamletsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem townsFortsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem citiesCapitalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem showAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem showLandmarksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showRoadsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showStateBordersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showProvinciesBordersToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem showLocationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLandsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLandMassesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem testPathFinding1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useCelshadingToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem showBirdviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showBoundsToolStripMenuItem;
    }
}