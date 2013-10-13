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
            this.ParametersPanel = new System.Windows.Forms.Panel();
            this.PlayfieldPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.EventTextBox = new System.Windows.Forms.TextBox();
            this.GrandActionsPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ActionsTextBox = new System.Windows.Forms.TextBox();
            this.ActionsPanel = new Persona.AlignedFlowContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ReactionsPanel = new Persona.AlignedFlowContainer();
            this.EventPanel.SuspendLayout();
            this.PlayfieldPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.GrandActionsPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
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
            // ParametersPanel
            // 
            this.ParametersPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ParametersPanel.Location = new System.Drawing.Point(0, 0);
            this.ParametersPanel.Name = "ParametersPanel";
            this.ParametersPanel.Size = new System.Drawing.Size(314, 66);
            this.ParametersPanel.TabIndex = 2;
            // 
            // PlayfieldPanel
            // 
            this.PlayfieldPanel.Controls.Add(this.GrandActionsPanel);
            this.PlayfieldPanel.Controls.Add(this.EventPanel);
            this.PlayfieldPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayfieldPanel.Location = new System.Drawing.Point(0, 66);
            this.PlayfieldPanel.Name = "PlayfieldPanel";
            this.PlayfieldPanel.Size = new System.Drawing.Size(314, 390);
            this.PlayfieldPanel.TabIndex = 3;
            this.PlayfieldPanel.Resize += new System.EventHandler(this.PlayfieldPanel_Resize);
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 196);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // EventTextBox
            // 
            this.EventTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EventTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventTextBox.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EventTextBox.Location = new System.Drawing.Point(3, 3);
            this.EventTextBox.Multiline = true;
            this.EventTextBox.Name = "EventTextBox";
            this.EventTextBox.ReadOnly = true;
            this.EventTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.EventTextBox.Size = new System.Drawing.Size(194, 52);
            this.EventTextBox.TabIndex = 2;
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
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(200, 176);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // ActionsTextBox
            // 
            this.ActionsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ActionsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionsTextBox.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ActionsTextBox.Location = new System.Drawing.Point(3, 3);
            this.ActionsTextBox.Multiline = true;
            this.ActionsTextBox.Name = "ActionsTextBox";
            this.ActionsTextBox.ReadOnly = true;
            this.ActionsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ActionsTextBox.Size = new System.Drawing.Size(194, 46);
            this.ActionsTextBox.TabIndex = 3;
            // 
            // ActionsPanel
            // 
            this.ActionsPanel.Controls.Add(this.button1);
            this.ActionsPanel.Controls.Add(this.button2);
            this.ActionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionsPanel.Location = new System.Drawing.Point(3, 55);
            this.ActionsPanel.Name = "ActionsPanel";
            this.ActionsPanel.Size = new System.Drawing.Size(194, 118);
            this.ActionsPanel.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(59, 6);
            this.button1.Margin = new System.Windows.Forms.Padding(30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(59, 89);
            this.button2.Margin = new System.Windows.Forms.Padding(30);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // ReactionsPanel
            // 
            this.ReactionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReactionsPanel.Location = new System.Drawing.Point(3, 61);
            this.ReactionsPanel.Name = "ReactionsPanel";
            this.ReactionsPanel.Size = new System.Drawing.Size(194, 132);
            this.ReactionsPanel.TabIndex = 1;
            // 
            // TestPlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 456);
            this.Controls.Add(this.PlayfieldPanel);
            this.Controls.Add(this.ParametersPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TestPlay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TestPlay";
            this.EventPanel.ResumeLayout(false);
            this.PlayfieldPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.GrandActionsPanel.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ActionsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}