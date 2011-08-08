namespace CinemaActionsEditor
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
            GlacialComponents.Controls.GLColumn glColumn1 = new GlacialComponents.Controls.GLColumn();
            GlacialComponents.Controls.GLColumn glColumn2 = new GlacialComponents.Controls.GLColumn();
            GlacialComponents.Controls.GLColumn glColumn3 = new GlacialComponents.Controls.GLColumn();
            GlacialComponents.Controls.GLColumn glColumn4 = new GlacialComponents.Controls.GLColumn();
            GlacialComponents.Controls.GLItem glItem1 = new GlacialComponents.Controls.GLItem();
            GlacialComponents.Controls.GLSubItem glSubItem1 = new GlacialComponents.Controls.GLSubItem();
            GlacialComponents.Controls.GLSubItem glSubItem2 = new GlacialComponents.Controls.GLSubItem();
            GlacialComponents.Controls.GLSubItem glSubItem3 = new GlacialComponents.Controls.GLSubItem();
            GlacialComponents.Controls.GLSubItem glSubItem4 = new GlacialComponents.Controls.GLSubItem();
            GlacialComponents.Controls.GLItem glItem2 = new GlacialComponents.Controls.GLItem();
            GlacialComponents.Controls.GLSubItem glSubItem5 = new GlacialComponents.Controls.GLSubItem();
            GlacialComponents.Controls.GLSubItem glSubItem6 = new GlacialComponents.Controls.GLSubItem();
            GlacialComponents.Controls.GLSubItem glSubItem7 = new GlacialComponents.Controls.GLSubItem();
            GlacialComponents.Controls.GLSubItem glSubItem8 = new GlacialComponents.Controls.GLSubItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editTagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.glacialList1 = new GlacialComponents.Controls.GlacialList();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radioAction = new System.Windows.Forms.RadioButton();
            this.radioXXX = new System.Windows.Forms.RadioButton();
            this.radioFun = new System.Windows.Forms.RadioButton();
            this.radioGore = new System.Windows.Forms.RadioButton();
            this.radioHorror = new System.Windows.Forms.RadioButton();
            this.radioRomance = new System.Windows.Forms.RadioButton();
            this.radioMistery = new System.Windows.Forms.RadioButton();
            this.radioUnrated = new System.Windows.Forms.RadioButton();
            this.ActionsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(884, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.loadToolStripMenuItem.Text = "Load...";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.saveToolStripMenuItem.Text = "Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(117, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editTagsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(49, 22);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // editTagsToolStripMenuItem
            // 
            this.editTagsToolStripMenuItem.Name = "editTagsToolStripMenuItem";
            this.editTagsToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.editTagsToolStripMenuItem.Text = "Edit Tags...";
            this.editTagsToolStripMenuItem.Click += new System.EventHandler(this.editTagsToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "xml";
            this.openFileDialog1.Filter = "XML files|*.xml|All files|*.*";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xml";
            this.saveFileDialog1.Filter = "XML files|*.xml|All files|*.*";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 26);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ActionsListView);
            this.splitContainer1.Size = new System.Drawing.Size(884, 466);
            this.splitContainer1.SplitterDistance = 199;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.glacialList1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 205);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(199, 261);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tags:";
            // 
            // glacialList1
            // 
            this.glacialList1.AllowColumnResize = true;
            this.glacialList1.AllowMultiselect = false;
            this.glacialList1.AlternateBackground = System.Drawing.Color.DarkGreen;
            this.glacialList1.AlternatingColors = false;
            this.glacialList1.AutoHeight = true;
            this.glacialList1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.glacialList1.BackgroundStretchToFit = true;
            glColumn1.ActivatedEmbeddedType = GlacialComponents.Controls.GLActivatedEmbeddedTypes.None;
            glColumn1.CheckBoxes = false;
            glColumn1.ImageIndex = -1;
            glColumn1.Name = "Column1";
            glColumn1.NumericSort = false;
            glColumn1.Text = "Tag Name";
            glColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            glColumn1.Width = 120;
            glColumn2.ActivatedEmbeddedType = GlacialComponents.Controls.GLActivatedEmbeddedTypes.None;
            glColumn2.CheckBoxes = true;
            glColumn2.ImageIndex = -1;
            glColumn2.Name = "Column2";
            glColumn2.NumericSort = false;
            glColumn2.Text = "Show";
            glColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            glColumn2.Width = 25;
            glColumn3.ActivatedEmbeddedType = GlacialComponents.Controls.GLActivatedEmbeddedTypes.None;
            glColumn3.CheckBoxes = true;
            glColumn3.ImageIndex = -1;
            glColumn3.Name = "Column3";
            glColumn3.NumericSort = false;
            glColumn3.Text = "Hide";
            glColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            glColumn3.Width = 25;
            glColumn4.ActivatedEmbeddedType = GlacialComponents.Controls.GLActivatedEmbeddedTypes.None;
            glColumn4.CheckBoxes = false;
            glColumn4.ImageIndex = -1;
            glColumn4.Name = "Column4";
            glColumn4.NumericSort = true;
            glColumn4.Text = "Column";
            glColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            glColumn4.Width = 0;
            this.glacialList1.Columns.AddRange(new GlacialComponents.Controls.GLColumn[] {
            glColumn1,
            glColumn2,
            glColumn3,
            glColumn4});
            this.glacialList1.ControlStyle = GlacialComponents.Controls.GLControlStyles.Normal;
            this.glacialList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glacialList1.FullRowSelect = true;
            this.glacialList1.GridColor = System.Drawing.Color.LightGray;
            this.glacialList1.GridLines = GlacialComponents.Controls.GLGridLines.gridBoth;
            this.glacialList1.GridLineStyle = GlacialComponents.Controls.GLGridLineStyles.gridNone;
            this.glacialList1.GridTypes = GlacialComponents.Controls.GLGridTypes.gridOnExists;
            this.glacialList1.HeaderHeight = 0;
            this.glacialList1.HeaderVisible = false;
            this.glacialList1.HeaderWordWrap = false;
            this.glacialList1.HotColumnTracking = false;
            this.glacialList1.HotItemTracking = false;
            this.glacialList1.HotTrackingColor = System.Drawing.Color.LightGray;
            this.glacialList1.HoverEvents = true;
            this.glacialList1.HoverTime = 1;
            this.glacialList1.ImageList = null;
            this.glacialList1.ItemHeight = 17;
            glItem1.BackColor = System.Drawing.Color.White;
            glItem1.ForeColor = System.Drawing.Color.Black;
            glItem1.RowBorderColor = System.Drawing.Color.Black;
            glItem1.RowBorderSize = 0;
            glSubItem1.BackColor = System.Drawing.Color.Empty;
            glSubItem1.Checked = false;
            glSubItem1.ForceText = false;
            glSubItem1.ForeColor = System.Drawing.Color.Black;
            glSubItem1.ImageAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            glSubItem1.ImageIndex = -1;
            glSubItem1.Text = "Tag1";
            glSubItem2.BackColor = System.Drawing.Color.Empty;
            glSubItem2.Checked = true;
            glSubItem2.ForceText = false;
            glSubItem2.ForeColor = System.Drawing.Color.Black;
            glSubItem2.ImageAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            glSubItem2.ImageIndex = -1;
            glSubItem2.Text = "";
            glSubItem3.BackColor = System.Drawing.Color.Empty;
            glSubItem3.Checked = false;
            glSubItem3.ForceText = false;
            glSubItem3.ForeColor = System.Drawing.Color.Black;
            glSubItem3.ImageAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            glSubItem3.ImageIndex = -1;
            glSubItem3.Text = "";
            glSubItem4.BackColor = System.Drawing.Color.Empty;
            glSubItem4.Checked = false;
            glSubItem4.ForceText = false;
            glSubItem4.ForeColor = System.Drawing.Color.Black;
            glSubItem4.ImageAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            glSubItem4.ImageIndex = -1;
            glSubItem4.Text = "";
            glItem1.SubItems.AddRange(new GlacialComponents.Controls.GLSubItem[] {
            glSubItem1,
            glSubItem2,
            glSubItem3,
            glSubItem4});
            glItem1.Text = "Tag1";
            glItem2.BackColor = System.Drawing.Color.White;
            glItem2.ForeColor = System.Drawing.Color.Black;
            glItem2.RowBorderColor = System.Drawing.Color.Black;
            glItem2.RowBorderSize = 0;
            glSubItem5.BackColor = System.Drawing.Color.Empty;
            glSubItem5.Checked = false;
            glSubItem5.ForceText = false;
            glSubItem5.ForeColor = System.Drawing.Color.Black;
            glSubItem5.ImageAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            glSubItem5.ImageIndex = -1;
            glSubItem5.Text = "Tag2";
            glSubItem6.BackColor = System.Drawing.Color.Empty;
            glSubItem6.Checked = false;
            glSubItem6.ForceText = false;
            glSubItem6.ForeColor = System.Drawing.Color.Black;
            glSubItem6.ImageAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            glSubItem6.ImageIndex = -1;
            glSubItem6.Text = "";
            glSubItem7.BackColor = System.Drawing.Color.Empty;
            glSubItem7.Checked = true;
            glSubItem7.ForceText = false;
            glSubItem7.ForeColor = System.Drawing.Color.Black;
            glSubItem7.ImageAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            glSubItem7.ImageIndex = -1;
            glSubItem7.Text = "";
            glSubItem8.BackColor = System.Drawing.Color.Empty;
            glSubItem8.Checked = false;
            glSubItem8.ForceText = false;
            glSubItem8.ForeColor = System.Drawing.Color.Black;
            glSubItem8.ImageAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            glSubItem8.ImageIndex = -1;
            glSubItem8.Text = "";
            glItem2.SubItems.AddRange(new GlacialComponents.Controls.GLSubItem[] {
            glSubItem5,
            glSubItem6,
            glSubItem7,
            glSubItem8});
            glItem2.Text = "Tag2";
            this.glacialList1.Items.AddRange(new GlacialComponents.Controls.GLItem[] {
            glItem1,
            glItem2});
            this.glacialList1.ItemWordWrap = false;
            this.glacialList1.Location = new System.Drawing.Point(3, 16);
            this.glacialList1.Name = "glacialList1";
            this.glacialList1.Selectable = false;
            this.glacialList1.SelectedTextColor = System.Drawing.Color.White;
            this.glacialList1.SelectionColor = System.Drawing.Color.DarkBlue;
            this.glacialList1.ShowBorder = true;
            this.glacialList1.ShowFocusRect = false;
            this.glacialList1.Size = new System.Drawing.Size(193, 242);
            this.glacialList1.SortType = GlacialComponents.Controls.SortTypes.InsertionSort;
            this.glacialList1.SuperFlatHeaderColor = System.Drawing.Color.White;
            this.glacialList1.TabIndex = 1;
            this.glacialList1.Text = "glacialList1";
            this.glacialList1.ItemChangedEvent += new GlacialComponents.Controls.ChangedEventHandler(this.glacialList1_ItemChangedEvent);
            this.glacialList1.HoverEvent += new GlacialComponents.Controls.GlacialList.HoverEventDelegate(this.glacialList1_HoverEvent);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(199, 205);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sort by:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.radioAction, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.radioXXX, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.radioFun, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.radioGore, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.radioHorror, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.radioRomance, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.radioMistery, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.radioUnrated, 0, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(193, 186);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // radioAction
            // 
            this.radioAction.AutoSize = true;
            this.radioAction.Checked = true;
            this.radioAction.Location = new System.Drawing.Point(3, 3);
            this.radioAction.Name = "radioAction";
            this.radioAction.Size = new System.Drawing.Size(84, 17);
            this.radioAction.TabIndex = 0;
            this.radioAction.TabStop = true;
            this.radioAction.Text = "Action rating";
            this.radioAction.UseVisualStyleBackColor = true;
            this.radioAction.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // radioXXX
            // 
            this.radioXXX.AutoSize = true;
            this.radioXXX.Location = new System.Drawing.Point(3, 141);
            this.radioXXX.Name = "radioXXX";
            this.radioXXX.Size = new System.Drawing.Size(75, 17);
            this.radioXXX.TabIndex = 4;
            this.radioXXX.Text = "XXX rating";
            this.radioXXX.UseVisualStyleBackColor = true;
            this.radioXXX.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // radioFun
            // 
            this.radioFun.AutoSize = true;
            this.radioFun.Location = new System.Drawing.Point(3, 49);
            this.radioFun.Name = "radioFun";
            this.radioFun.Size = new System.Drawing.Size(72, 17);
            this.radioFun.TabIndex = 1;
            this.radioFun.Text = "Fun rating";
            this.radioFun.UseVisualStyleBackColor = true;
            this.radioFun.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // radioGore
            // 
            this.radioGore.AutoSize = true;
            this.radioGore.Location = new System.Drawing.Point(3, 26);
            this.radioGore.Name = "radioGore";
            this.radioGore.Size = new System.Drawing.Size(77, 17);
            this.radioGore.TabIndex = 5;
            this.radioGore.Text = "Gore rating";
            this.radioGore.UseVisualStyleBackColor = true;
            this.radioGore.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // radioHorror
            // 
            this.radioHorror.AutoSize = true;
            this.radioHorror.Location = new System.Drawing.Point(3, 118);
            this.radioHorror.Name = "radioHorror";
            this.radioHorror.Size = new System.Drawing.Size(83, 17);
            this.radioHorror.TabIndex = 3;
            this.radioHorror.Text = "Horror rating";
            this.radioHorror.UseVisualStyleBackColor = true;
            this.radioHorror.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // radioRomance
            // 
            this.radioRomance.AutoSize = true;
            this.radioRomance.Location = new System.Drawing.Point(3, 95);
            this.radioRomance.Name = "radioRomance";
            this.radioRomance.Size = new System.Drawing.Size(100, 17);
            this.radioRomance.TabIndex = 2;
            this.radioRomance.Text = "Romance rating";
            this.radioRomance.UseVisualStyleBackColor = true;
            this.radioRomance.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // radioMistery
            // 
            this.radioMistery.AutoSize = true;
            this.radioMistery.Location = new System.Drawing.Point(3, 72);
            this.radioMistery.Name = "radioMistery";
            this.radioMistery.Size = new System.Drawing.Size(87, 17);
            this.radioMistery.TabIndex = 6;
            this.radioMistery.Text = "Mistery rating";
            this.radioMistery.UseVisualStyleBackColor = true;
            // 
            // radioUnrated
            // 
            this.radioUnrated.AutoSize = true;
            this.radioUnrated.Location = new System.Drawing.Point(3, 164);
            this.radioUnrated.Name = "radioUnrated";
            this.radioUnrated.Size = new System.Drawing.Size(63, 17);
            this.radioUnrated.TabIndex = 7;
            this.radioUnrated.Text = "Unrated";
            this.radioUnrated.UseVisualStyleBackColor = true;
            this.radioUnrated.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // ActionsListView
            // 
            this.ActionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.ActionsListView.ContextMenuStrip = this.contextMenuStrip1;
            this.ActionsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionsListView.Location = new System.Drawing.Point(0, 0);
            this.ActionsListView.Name = "ActionsListView";
            this.ActionsListView.Size = new System.Drawing.Size(681, 466);
            this.ActionsListView.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.ActionsListView.TabIndex = 1;
            this.ActionsListView.UseCompatibleStateImageBehavior = false;
            this.ActionsListView.View = System.Windows.Forms.View.Details;
            this.ActionsListView.ItemActivate += new System.EventHandler(this.editActionToolStripMenuItem_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Action Name";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Actors Count";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "SubGenres";
            this.columnHeader3.Width = 300;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editActionToolStripMenuItem,
            this.newActionToolStripMenuItem,
            this.deleteActionToolStripMenuItem,
            this.toolStripMenuItem2,
            this.cancelToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(171, 98);
            // 
            // editActionToolStripMenuItem
            // 
            this.editActionToolStripMenuItem.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editActionToolStripMenuItem.Name = "editActionToolStripMenuItem";
            this.editActionToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.editActionToolStripMenuItem.Text = "Edit Action...";
            this.editActionToolStripMenuItem.Click += new System.EventHandler(this.editActionToolStripMenuItem_Click);
            // 
            // newActionToolStripMenuItem
            // 
            this.newActionToolStripMenuItem.Name = "newActionToolStripMenuItem";
            this.newActionToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.newActionToolStripMenuItem.Text = "New Action...";
            this.newActionToolStripMenuItem.Click += new System.EventHandler(this.newActionToolStripMenuItem_Click);
            // 
            // deleteActionToolStripMenuItem
            // 
            this.deleteActionToolStripMenuItem.Enabled = false;
            this.deleteActionToolStripMenuItem.Name = "deleteActionToolStripMenuItem";
            this.deleteActionToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.deleteActionToolStripMenuItem.Text = "Delete Action...";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(167, 6);
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.cancelToolStripMenuItem.Text = "Cancel";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 0;
            this.toolTip1.ReshowDelay = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 492);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton radioAction;
        private System.Windows.Forms.RadioButton radioFun;
        private System.Windows.Forms.RadioButton radioRomance;
        private System.Windows.Forms.RadioButton radioHorror;
        private System.Windows.Forms.RadioButton radioXXX;
        private System.Windows.Forms.RadioButton radioGore;
        private System.Windows.Forms.ListView ActionsListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editActionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newActionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteActionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.RadioButton radioMistery;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editTagsToolStripMenuItem;
        private System.Windows.Forms.RadioButton radioUnrated;
        private GlacialComponents.Controls.GlacialList glacialList1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

