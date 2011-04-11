namespace CinemaActionsEditor
{
    partial class TagEditForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxAction = new System.Windows.Forms.CheckBox();
            this.checkBoxGore = new System.Windows.Forms.CheckBox();
            this.checkBoxFun = new System.Windows.Forms.CheckBox();
            this.checkBoxMistery = new System.Windows.Forms.CheckBox();
            this.checkBoxRomance = new System.Windows.Forms.CheckBox();
            this.checkBoxHorror = new System.Windows.Forms.CheckBox();
            this.checkBoxXXX = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(56, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(97, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.checkBoxAction, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxGore, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxFun, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxMistery, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxRomance, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxHorror, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxXXX, 0, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(132, 170);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(15, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(138, 189);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Genre:";
            // 
            // checkBoxAction
            // 
            this.checkBoxAction.AutoSize = true;
            this.checkBoxAction.Location = new System.Drawing.Point(3, 3);
            this.checkBoxAction.Name = "checkBoxAction";
            this.checkBoxAction.Size = new System.Drawing.Size(56, 17);
            this.checkBoxAction.TabIndex = 7;
            this.checkBoxAction.Text = "Action";
            this.checkBoxAction.UseVisualStyleBackColor = true;
            // 
            // checkBoxGore
            // 
            this.checkBoxGore.AutoSize = true;
            this.checkBoxGore.Location = new System.Drawing.Point(3, 27);
            this.checkBoxGore.Name = "checkBoxGore";
            this.checkBoxGore.Size = new System.Drawing.Size(49, 17);
            this.checkBoxGore.TabIndex = 8;
            this.checkBoxGore.Text = "Gore";
            this.checkBoxGore.UseVisualStyleBackColor = true;
            // 
            // checkBoxFun
            // 
            this.checkBoxFun.AutoSize = true;
            this.checkBoxFun.Location = new System.Drawing.Point(3, 51);
            this.checkBoxFun.Name = "checkBoxFun";
            this.checkBoxFun.Size = new System.Drawing.Size(44, 17);
            this.checkBoxFun.TabIndex = 9;
            this.checkBoxFun.Text = "Fun";
            this.checkBoxFun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxFun.UseVisualStyleBackColor = true;
            // 
            // checkBoxMistery
            // 
            this.checkBoxMistery.AutoSize = true;
            this.checkBoxMistery.Location = new System.Drawing.Point(3, 75);
            this.checkBoxMistery.Name = "checkBoxMistery";
            this.checkBoxMistery.Size = new System.Drawing.Size(59, 17);
            this.checkBoxMistery.TabIndex = 10;
            this.checkBoxMistery.Text = "Mistery";
            this.checkBoxMistery.UseVisualStyleBackColor = true;
            // 
            // checkBoxRomance
            // 
            this.checkBoxRomance.AutoSize = true;
            this.checkBoxRomance.Location = new System.Drawing.Point(3, 99);
            this.checkBoxRomance.Name = "checkBoxRomance";
            this.checkBoxRomance.Size = new System.Drawing.Size(72, 17);
            this.checkBoxRomance.TabIndex = 11;
            this.checkBoxRomance.Text = "Romance";
            this.checkBoxRomance.UseVisualStyleBackColor = true;
            // 
            // checkBoxHorror
            // 
            this.checkBoxHorror.AutoSize = true;
            this.checkBoxHorror.Location = new System.Drawing.Point(3, 123);
            this.checkBoxHorror.Name = "checkBoxHorror";
            this.checkBoxHorror.Size = new System.Drawing.Size(55, 17);
            this.checkBoxHorror.TabIndex = 12;
            this.checkBoxHorror.Text = "Horror";
            this.checkBoxHorror.UseVisualStyleBackColor = true;
            // 
            // checkBoxXXX
            // 
            this.checkBoxXXX.AutoSize = true;
            this.checkBoxXXX.Location = new System.Drawing.Point(3, 147);
            this.checkBoxXXX.Name = "checkBoxXXX";
            this.checkBoxXXX.Size = new System.Drawing.Size(47, 17);
            this.checkBoxXXX.TabIndex = 13;
            this.checkBoxXXX.Text = "XXX";
            this.checkBoxXXX.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.buttonOK, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonCancel, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 233);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(3);
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(170, 78);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOK.Location = new System.Drawing.Point(43, 6);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(84, 30);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(43, 42);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(84, 30);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // TagEditForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(170, 311);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "TagEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TagEditForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBoxAction;
        private System.Windows.Forms.CheckBox checkBoxGore;
        private System.Windows.Forms.CheckBox checkBoxFun;
        private System.Windows.Forms.CheckBox checkBoxMistery;
        private System.Windows.Forms.CheckBox checkBoxRomance;
        private System.Windows.Forms.CheckBox checkBoxHorror;
        private System.Windows.Forms.CheckBox checkBoxXXX;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}