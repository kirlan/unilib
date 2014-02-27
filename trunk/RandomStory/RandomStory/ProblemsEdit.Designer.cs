namespace RandomStory
{
    partial class ProblemsEdit
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
            this.problemsTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.solutionsTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // problemsTextBox
            // 
            this.problemsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.problemsTextBox.Location = new System.Drawing.Point(12, 25);
            this.problemsTextBox.Multiline = true;
            this.problemsTextBox.Name = "problemsTextBox";
            this.problemsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.problemsTextBox.Size = new System.Drawing.Size(739, 187);
            this.problemsTextBox.TabIndex = 0;
            this.problemsTextBox.TextChanged += new System.EventHandler(this.problemsTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Проблемы:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 215);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Решения:";
            // 
            // solutionsTextBox
            // 
            this.solutionsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.solutionsTextBox.Location = new System.Drawing.Point(12, 231);
            this.solutionsTextBox.Multiline = true;
            this.solutionsTextBox.Name = "solutionsTextBox";
            this.solutionsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.solutionsTextBox.Size = new System.Drawing.Size(739, 192);
            this.solutionsTextBox.TabIndex = 3;
            this.solutionsTextBox.TextChanged += new System.EventHandler(this.solutionsTextBox_TextChanged);
            // 
            // ProblemsEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 435);
            this.Controls.Add(this.solutionsTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.problemsTextBox);
            this.Name = "ProblemsEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Проблемы и решения";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox problemsTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox solutionsTextBox;
    }
}