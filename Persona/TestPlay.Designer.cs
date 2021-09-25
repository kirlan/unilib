namespace Persona
{
    partial class TestPlay
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
            this.EventPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.EventTextBox = new System.Windows.Forms.TextBox();
            this.ParametersPanel = new System.Windows.Forms.Panel();
            this.PlayfieldPanel = new System.Windows.Forms.Panel();
            this.ReactionPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.ReactionTextBox = new System.Windows.Forms.TextBox();
            this.GrandActionsPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ActionsTextBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ParamsListView = new System.Windows.Forms.ListView();
            this.ParamHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.alignedFlowContainer1 = new Persona.AlignedFlowContainer();
            this.ReactionEndButton = new System.Windows.Forms.Button();
            this.ActionsPanel = new Persona.AlignedFlowContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ReactionsPanel = new Persona.AlignedFlowContainer();
            this.EventPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.PlayfieldPanel.SuspendLayout();
            this.ReactionPanel.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.GrandActionsPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.alignedFlowContainer1.SuspendLayout();
            this.ActionsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // EventPanel
            // 
            this.EventPanel.Controls.Add(this.tableLayoutPanel1);
            this.EventPanel.Location = new System.Drawing.Point(32, 182);
            this.EventPanel.Name = "EventPanel";
            this.EventPanel.Size = new System.Drawing.Size(200, 196);
            this.EventPanel.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ReactionsPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.EventTextBox, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.36364F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 63.63636F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 196);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // EventTextBox
            // 
            this.EventTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EventTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventTextBox.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EventTextBox.Location = new System.Drawing.Point(7, 7);
            this.EventTextBox.Margin = new System.Windows.Forms.Padding(7);
            this.EventTextBox.Multiline = true;
            this.EventTextBox.Name = "EventTextBox";
            this.EventTextBox.ReadOnly = true;
            this.EventTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.EventTextBox.Size = new System.Drawing.Size(186, 57);
            this.EventTextBox.TabIndex = 2;
            this.EventTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ParametersPanel
            // 
            this.ParametersPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ParametersPanel.Location = new System.Drawing.Point(301, 0);
            this.ParametersPanel.Name = "ParametersPanel";
            this.ParametersPanel.Size = new System.Drawing.Size(365, 66);
            this.ParametersPanel.TabIndex = 2;
            // 
            // PlayfieldPanel
            // 
            this.PlayfieldPanel.Controls.Add(this.ReactionPanel);
            this.PlayfieldPanel.Controls.Add(this.GrandActionsPanel);
            this.PlayfieldPanel.Controls.Add(this.EventPanel);
            this.PlayfieldPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayfieldPanel.Location = new System.Drawing.Point(301, 66);
            this.PlayfieldPanel.Name = "PlayfieldPanel";
            this.PlayfieldPanel.Size = new System.Drawing.Size(365, 609);
            this.PlayfieldPanel.TabIndex = 3;
            this.PlayfieldPanel.Resize += new System.EventHandler(this.PlayfieldPanel_Resize);
            // 
            // ReactionPanel
            // 
            this.ReactionPanel.Controls.Add(this.tableLayoutPanel3);
            this.ReactionPanel.Location = new System.Drawing.Point(6, 144);
            this.ReactionPanel.Name = "ReactionPanel";
            this.ReactionPanel.Size = new System.Drawing.Size(200, 196);
            this.ReactionPanel.TabIndex = 4;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.alignedFlowContainer1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.ReactionTextBox, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.36364F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 63.63636F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(200, 196);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // ReactionTextBox
            // 
            this.ReactionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ReactionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReactionTextBox.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ReactionTextBox.Location = new System.Drawing.Point(7, 7);
            this.ReactionTextBox.Margin = new System.Windows.Forms.Padding(7);
            this.ReactionTextBox.Multiline = true;
            this.ReactionTextBox.Name = "ReactionTextBox";
            this.ReactionTextBox.ReadOnly = true;
            this.ReactionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ReactionTextBox.Size = new System.Drawing.Size(186, 57);
            this.ReactionTextBox.TabIndex = 2;
            this.ReactionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // GrandActionsPanel
            // 
            this.GrandActionsPanel.Controls.Add(this.tableLayoutPanel2);
            this.GrandActionsPanel.Location = new System.Drawing.Point(32, 0);
            this.GrandActionsPanel.Name = "GrandActionsPanel";
            this.GrandActionsPanel.Size = new System.Drawing.Size(200, 176);
            this.GrandActionsPanel.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.ActionsTextBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ActionsPanel, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.36364F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 63.63636F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(200, 176);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // ActionsTextBox
            // 
            this.ActionsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ActionsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionsTextBox.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ActionsTextBox.Location = new System.Drawing.Point(7, 7);
            this.ActionsTextBox.Margin = new System.Windows.Forms.Padding(7);
            this.ActionsTextBox.Multiline = true;
            this.ActionsTextBox.Name = "ActionsTextBox";
            this.ActionsTextBox.ReadOnly = true;
            this.ActionsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ActionsTextBox.Size = new System.Drawing.Size(186, 50);
            this.ActionsTextBox.TabIndex = 3;
            this.ActionsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ParamsListView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(301, 675);
            this.panel1.TabIndex = 4;
            // 
            // ParamsListView
            // 
            this.ParamsListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ParamsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ParamHeader,
            this.ValueHeader});
            this.ParamsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParamsListView.FullRowSelect = true;
            this.ParamsListView.GridLines = true;
            this.ParamsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ParamsListView.Location = new System.Drawing.Point(0, 0);
            this.ParamsListView.MultiSelect = false;
            this.ParamsListView.Name = "ParamsListView";
            this.ParamsListView.Size = new System.Drawing.Size(301, 675);
            this.ParamsListView.TabIndex = 0;
            this.ParamsListView.UseCompatibleStateImageBehavior = false;
            this.ParamsListView.View = System.Windows.Forms.View.Details;
            // 
            // ParamHeader
            // 
            this.ParamHeader.Text = "Параметр";
            this.ParamHeader.Width = 120;
            // 
            // ValueHeader
            // 
            this.ValueHeader.Text = "Значение";
            this.ValueHeader.Width = 150;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.textBox1.Location = new System.Drawing.Point(666, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(323, 675);
            this.textBox1.TabIndex = 5;
            this.textBox1.WordWrap = false;
            // 
            // alignedFlowContainer1
            // 
            this.alignedFlowContainer1.Controls.Add(this.ReactionEndButton);
            this.alignedFlowContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alignedFlowContainer1.Location = new System.Drawing.Point(3, 74);
            this.alignedFlowContainer1.Name = "alignedFlowContainer1";
            this.alignedFlowContainer1.Size = new System.Drawing.Size(194, 119);
            this.alignedFlowContainer1.TabIndex = 1;
            // 
            // ReactionEndButton
            // 
            this.ReactionEndButton.Location = new System.Drawing.Point(59, 48);
            this.ReactionEndButton.Name = "ReactionEndButton";
            this.ReactionEndButton.Size = new System.Drawing.Size(75, 23);
            this.ReactionEndButton.TabIndex = 0;
            this.ReactionEndButton.Text = "Дальше...";
            this.ReactionEndButton.UseVisualStyleBackColor = true;
            this.ReactionEndButton.Click += new System.EventHandler(this.ReactionEndButton_Click);
            // 
            // ActionsPanel
            // 
            this.ActionsPanel.Controls.Add(this.button1);
            this.ActionsPanel.Controls.Add(this.button2);
            this.ActionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionsPanel.Location = new System.Drawing.Point(3, 67);
            this.ActionsPanel.Name = "ActionsPanel";
            this.ActionsPanel.Size = new System.Drawing.Size(194, 106);
            this.ActionsPanel.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(59, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(59, 83);
            this.button2.Margin = new System.Windows.Forms.Padding(30);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // ReactionsPanel
            // 
            this.ReactionsPanel.AutoScroll = true;
            this.ReactionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReactionsPanel.Location = new System.Drawing.Point(3, 74);
            this.ReactionsPanel.Name = "ReactionsPanel";
            this.ReactionsPanel.Size = new System.Drawing.Size(194, 119);
            this.ReactionsPanel.TabIndex = 1;
            // 
            // TestPlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 675);
            this.Controls.Add(this.PlayfieldPanel);
            this.Controls.Add(this.ParametersPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TestPlay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TestPlay";
            this.EventPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.PlayfieldPanel.ResumeLayout(false);
            this.ReactionPanel.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.GrandActionsPanel.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.alignedFlowContainer1.ResumeLayout(false);
            this.ActionsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel EventPanel;
        private System.Windows.Forms.Panel ParametersPanel;
        private System.Windows.Forms.Panel PlayfieldPanel;
        private AlignedFlowContainer ActionsPanel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AlignedFlowContainer ReactionsPanel;
        private System.Windows.Forms.TextBox EventTextBox;
        private System.Windows.Forms.Panel GrandActionsPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox ActionsTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView ParamsListView;
        private System.Windows.Forms.ColumnHeader ParamHeader;
        private System.Windows.Forms.ColumnHeader ValueHeader;
        private System.Windows.Forms.Panel ReactionPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private AlignedFlowContainer alignedFlowContainer1;
        private System.Windows.Forms.Button ReactionEndButton;
        private System.Windows.Forms.TextBox ReactionTextBox;
        private System.Windows.Forms.TextBox textBox1;
    }
}