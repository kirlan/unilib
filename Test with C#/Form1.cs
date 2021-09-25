using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using QUICKFILTERLib;

namespace QFilterTest
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmTestQFilter : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox fraNoOfColumns;
		private System.Windows.Forms.RadioButton opt2Col;
		private System.Windows.Forms.RadioButton opt3Col;
		private System.Windows.Forms.RadioButton opt4Col;
		private System.Windows.Forms.GroupBox fraNoOfFilters;
		private System.Windows.Forms.RadioButton opt6Filters;
		private System.Windows.Forms.RadioButton opt5Filters;
		private System.Windows.Forms.RadioButton opt4Filters;
		private AxQUICKFILTERLib.AxQFilter QF;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private const int ROOT_LEVEL = 0;
		private System.Windows.Forms.Label lblSQLStatement;
		private const int INVALID_ID = -1;
		
		public frmTestQFilter()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTestQFilter));
            this.QF = new AxQUICKFILTERLib.AxQFilter();
            this.fraNoOfColumns = new System.Windows.Forms.GroupBox();
            this.opt4Col = new System.Windows.Forms.RadioButton();
            this.opt3Col = new System.Windows.Forms.RadioButton();
            this.opt2Col = new System.Windows.Forms.RadioButton();
            this.fraNoOfFilters = new System.Windows.Forms.GroupBox();
            this.opt6Filters = new System.Windows.Forms.RadioButton();
            this.opt5Filters = new System.Windows.Forms.RadioButton();
            this.opt4Filters = new System.Windows.Forms.RadioButton();
            this.lblSQLStatement = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.QF)).BeginInit();
            this.fraNoOfColumns.SuspendLayout();
            this.fraNoOfFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // QF
            // 
            this.QF.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.QF.Enabled = true;
            this.QF.Location = new System.Drawing.Point(0, 46);
            this.QF.Name = "QF";
            this.QF.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("QF.OcxState")));
            this.QF.Size = new System.Drawing.Size(592, 72);
            this.QF.TabIndex = 1;
            this.QF.Change += new System.EventHandler(this.QF_Change);
            // 
            // fraNoOfColumns
            // 
            this.fraNoOfColumns.Controls.Add(this.opt4Col);
            this.fraNoOfColumns.Controls.Add(this.opt3Col);
            this.fraNoOfColumns.Controls.Add(this.opt2Col);
            this.fraNoOfColumns.Location = new System.Drawing.Point(0, 0);
            this.fraNoOfColumns.Name = "fraNoOfColumns";
            this.fraNoOfColumns.Size = new System.Drawing.Size(128, 40);
            this.fraNoOfColumns.TabIndex = 2;
            this.fraNoOfColumns.TabStop = false;
            this.fraNoOfColumns.Text = "No Of Columns:";
            // 
            // opt4Col
            // 
            this.opt4Col.Location = new System.Drawing.Point(88, 16);
            this.opt4Col.Name = "opt4Col";
            this.opt4Col.Size = new System.Drawing.Size(32, 16);
            this.opt4Col.TabIndex = 2;
            this.opt4Col.Text = "4";
            this.opt4Col.CheckedChanged += new System.EventHandler(this.opt4Col_CheckedChanged);
            // 
            // opt3Col
            // 
            this.opt3Col.Location = new System.Drawing.Point(48, 16);
            this.opt3Col.Name = "opt3Col";
            this.opt3Col.Size = new System.Drawing.Size(32, 16);
            this.opt3Col.TabIndex = 1;
            this.opt3Col.Text = "3";
            this.opt3Col.CheckedChanged += new System.EventHandler(this.opt3Col_CheckedChanged);
            // 
            // opt2Col
            // 
            this.opt2Col.Checked = true;
            this.opt2Col.Location = new System.Drawing.Point(8, 16);
            this.opt2Col.Name = "opt2Col";
            this.opt2Col.Size = new System.Drawing.Size(32, 16);
            this.opt2Col.TabIndex = 0;
            this.opt2Col.TabStop = true;
            this.opt2Col.Text = "2";
            this.opt2Col.CheckedChanged += new System.EventHandler(this.opt2Col_CheckedChanged);
            // 
            // fraNoOfFilters
            // 
            this.fraNoOfFilters.Controls.Add(this.opt6Filters);
            this.fraNoOfFilters.Controls.Add(this.opt5Filters);
            this.fraNoOfFilters.Controls.Add(this.opt4Filters);
            this.fraNoOfFilters.Location = new System.Drawing.Point(136, 0);
            this.fraNoOfFilters.Name = "fraNoOfFilters";
            this.fraNoOfFilters.Size = new System.Drawing.Size(128, 40);
            this.fraNoOfFilters.TabIndex = 3;
            this.fraNoOfFilters.TabStop = false;
            this.fraNoOfFilters.Text = "No Of Filters:";
            // 
            // opt6Filters
            // 
            this.opt6Filters.Location = new System.Drawing.Point(88, 16);
            this.opt6Filters.Name = "opt6Filters";
            this.opt6Filters.Size = new System.Drawing.Size(32, 16);
            this.opt6Filters.TabIndex = 2;
            this.opt6Filters.Text = "6";
            this.opt6Filters.CheckedChanged += new System.EventHandler(this.opt6Filters_CheckedChanged);
            // 
            // opt5Filters
            // 
            this.opt5Filters.Location = new System.Drawing.Point(48, 16);
            this.opt5Filters.Name = "opt5Filters";
            this.opt5Filters.Size = new System.Drawing.Size(32, 16);
            this.opt5Filters.TabIndex = 1;
            this.opt5Filters.Text = "5";
            this.opt5Filters.CheckedChanged += new System.EventHandler(this.opt5Filters_CheckedChanged);
            // 
            // opt4Filters
            // 
            this.opt4Filters.Checked = true;
            this.opt4Filters.Location = new System.Drawing.Point(8, 16);
            this.opt4Filters.Name = "opt4Filters";
            this.opt4Filters.Size = new System.Drawing.Size(32, 16);
            this.opt4Filters.TabIndex = 0;
            this.opt4Filters.TabStop = true;
            this.opt4Filters.Text = "4";
            this.opt4Filters.CheckedChanged += new System.EventHandler(this.opt4Filters_CheckedChanged);
            // 
            // lblSQLStatement
            // 
            this.lblSQLStatement.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSQLStatement.BackColor = System.Drawing.Color.White;
            this.lblSQLStatement.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSQLStatement.ForeColor = System.Drawing.Color.Black;
            this.lblSQLStatement.Location = new System.Drawing.Point(0, 120);
            this.lblSQLStatement.Name = "lblSQLStatement";
            this.lblSQLStatement.Size = new System.Drawing.Size(592, 184);
            this.lblSQLStatement.TabIndex = 4;
            this.lblSQLStatement.Text = "label1";
            // 
            // frmTestQFilter
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(592, 301);
            this.Controls.Add(this.lblSQLStatement);
            this.Controls.Add(this.fraNoOfColumns);
            this.Controls.Add(this.QF);
            this.Controls.Add(this.fraNoOfFilters);
            this.Name = "frmTestQFilter";
            this.Text = "Test QFilter";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.QF)).EndInit();
            this.fraNoOfColumns.ResumeLayout(false);
            this.fraNoOfFilters.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmTestQFilter());
		}

		#region SimpleHandlers
			private void opt2Col_CheckedChanged(object sender, System.EventArgs e) {
				if (opt2Col.Checked) QF.NoOfColumns = NoOfColumnsValues.fil2Columns;
			}
			private void opt3Col_CheckedChanged(object sender, System.EventArgs e) {
				if (opt3Col.Checked) QF.NoOfColumns = NoOfColumnsValues.fil3Columns;
			}
			private void opt4Col_CheckedChanged(object sender, System.EventArgs e) {
				if (opt4Col.Checked) QF.NoOfColumns = NoOfColumnsValues.fil4Columns;
			}
			private void opt4Filters_CheckedChanged(object sender, System.EventArgs e) {
				if (opt4Filters.Checked) QF.NoOfCheckFilters = 4;
			}
			private void opt5Filters_CheckedChanged(object sender, System.EventArgs e) {
				if (opt5Filters.Checked) QF.NoOfCheckFilters = 5;
			}
			private void opt6Filters_CheckedChanged(object sender, System.EventArgs e) {
				if (opt6Filters.Checked) QF.NoOfCheckFilters = 6;
			}
		#endregion
		
		private void Form1_Load(object sender, System.EventArgs e) {
			//## INIT
			QF.NoOfCheckFilters = 4;
			QF.NoOfColumns = NoOfColumnsValues.fil2Columns;
			
			//## POPULATE
			Populate();
			RefreshSQLStatement();
		}

		private void QF_Change(object sender, System.EventArgs e) {
			RefreshSQLStatement();
		}

		private void Populate() {
			//## INIT Countries ==================================================================
			QF.set_CheckLabel( 0, "Countries:" );
			QF.set_Field( 0, "Countries.ID" );
			QF.AddFolder( 0, "North America" );
				QF.AddString( 0, "USA", 5, ROOT_LEVEL + 2 );
				QF.AddString( 0, "Canada", 6, ROOT_LEVEL + 2 );
				QF.AddFolder( 0, "Europe" );
				QF.AddString( 0, "UK", 7, ROOT_LEVEL + 2 );
				QF.AddString( 0, "Germany", 8, ROOT_LEVEL + 2 );
				QF.AddString( 0, "Russia", 9, ROOT_LEVEL + 2 );
			QF.AddFolder( 0, "Asia" );
				QF.AddString( 0, "Israel", 10, ROOT_LEVEL + 2 );
			QF.CheckAll( 0, true );

			//## INIT Cities ====================================================================
			QF.set_CheckLabel( 1, "Cities:" );
			QF.set_Field( 1, "Cities.ID" );
			QF.AddFolder( 1, "North America" );
				QF.AddString( 1, "New York", 5, ROOT_LEVEL + 2 );
				QF.AddString( 1, "Boston", 6, ROOT_LEVEL + 2 );
				QF.AddString( 1, "Chicago", 21, ROOT_LEVEL + 2 );
				QF.AddString( 1, "San Francisco", 22, ROOT_LEVEL + 2 );
				QF.AddString( 1, "Atlanta", 23, ROOT_LEVEL + 2 );
			QF.AddFolder( 1, "Europe" );
				QF.AddString( 1, "London", 7, ROOT_LEVEL + 2 );
				QF.AddString( 1, "Madrid", 8, ROOT_LEVEL + 2 );
				QF.AddString( 1, "Brussels", 9, ROOT_LEVEL + 2 );
				QF.AddString( 1, "Athens", 10, ROOT_LEVEL + 2 );
				QF.AddString( 1, "Prague", 11, ROOT_LEVEL + 2 );
				QF.AddString( 1, "Bratislava", 12, ROOT_LEVEL + 2 );
				QF.AddString( 1, "Edinburgh", 14, ROOT_LEVEL + 2 );
				QF.AddString( 1, "Lisbon", 15, ROOT_LEVEL + 2 );
				QF.AddString( 1, "Zagreb", 17, ROOT_LEVEL + 2 );
			QF.CheckAll( 1, true );
			QF.set_Check( 1, 14, false );

			//## INIT Products ==================================================================
			QF.set_CheckLabel( 2, "Products:" );
			QF.set_Field( 2, "Products.ID" );
			QF.AddFolder( 2, "Milk" );
			QF.AddString( 2, "Tnuva 1L", 1, ROOT_LEVEL + 2 );
			QF.AddFolder( 2, "Beer" );
			QF.AddString( 2, "Interbrew", INVALID_ID, ROOT_LEVEL + 2 );
			QF.AddString( 2, "Stella 0.5", 2, ROOT_LEVEL + 3 );
			QF.AddString( 2, "Aramia 0.5", 3, ROOT_LEVEL + 3 );
			QF.AddString( 2, "Karlovacko 0.5", 4, ROOT_LEVEL + 2 );
			QF.AddString( 2, "Lowenbrau 0.5", 5, ROOT_LEVEL + 2 );
			QF.AddString( 2, "Spoon", 6, ROOT_LEVEL + 1 );
			QF.AddString( 2, "Oil", 7, ROOT_LEVEL + 2 );
			QF.CheckAll( 2, true );

			//## INIT Brands ====================================================================
			QF.set_CheckLabel( 3, "Brands:" );
			QF.set_Field( 3, "Brands.ID" );
			for(int i=0; i < 10; i++)
				for(int j=0; j < 26; j++)
					QF.AddString( 3, "Brand " + ("A" + i) + ("A" + j), i * 26 + j, ROOT_LEVEL + 1 );
			QF.CheckAll( 3, true );

			//## SET DroppedWidth
			QF.set_DroppedWidth( 2, QF.get_DroppedWidth(2) + 25 );
			QF.set_DroppedWidth( 3, QF.get_DroppedWidth(3) + 75 );
		}
		private void RefreshSQLStatement(){
			string str = "";
			str += "SELECT * \n";
			str += "FROM Products\n";
			str += "    LEFT JOIN Brands ON (Product.BrandID = Brands.ID)\n";
			str += "    LEFT JOIN Distributors ON (Product.DistributorID = Distributors.ID)\n";
			str += "    LEFT JOIN Cities ON (Distributors.CityID = Cities.ID)\n";
			str += "    LEFT JOIN Countries ON (Cities.CountryID = Countries.ID)\n";
			if (QF.SQLFilter.Length > 0) str += " WHERE " + QF.SQLFilter;

			lblSQLStatement.Text = str;
		}
	}
}
