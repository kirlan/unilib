using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LandscapeGeneration;
using System.IO;
using Socium;

namespace WorldGeneration
{
    public partial class GridBuildingForm : Form
    {
        public LocationsGrid<LocationX> m_pLocationsGrid = null;

        private string m_sWorkingFolder = "";

        public GridBuildingForm(string sWorkingFolder)
        {
            InitializeComponent();

            radioButton3_CheckedChanged(this, null);

            GridWidth.SelectedIndex = 6;
            GridHeight.SelectedIndex = 6;

            PointsCount.SelectedIndex = 2;

            NoGridWidth.Value = 5;
            NoGridHeight.Value = 4;

            m_sWorkingFolder = sWorkingFolder;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            GridPanel.Visible = !radioButton3.Checked;
            NoGridPanel.Visible = radioButton3.Checked;

            UpdateDescription();
        }

        private void GridSquare_CheckedChanged(object sender, EventArgs e)
        {
            GridHeight.Enabled = !GridSquare.Checked;
            GridHeight.SelectedIndex = GridWidth.SelectedIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            tableLayoutPanel1.Enabled = false;
            GridPanel.Enabled = false;
            NoGridPanel.Enabled = false;
            panel1.Enabled = false;
            Looped.Enabled = false;

            button1.Enabled = false;
            button1.Text = "... Please wait ...";

            Refresh();
        
            if (radioButton2.Checked)
                m_pLocationsGrid = new LocationsGrid<LocationX>(int.Parse(GridWidth.Text), int.Parse(GridHeight.Text), GridType.Hex, textBox1.Text, Looped.Checked ? WorldShape.Ringworld : WorldShape.Plain);

            if (radioButton3.Checked)
            {
                int iLocationsCount = 5000;
                switch (PointsCount.SelectedIndex)
                {
                    case 0:
                        iLocationsCount = 5000;
                        break;
                    case 1:
                        iLocationsCount = 10000;
                        break;
                    case 2:
                        iLocationsCount = 25000;
                        break;
                    case 3:
                        iLocationsCount = 50000;
                        break;
                }
                m_pLocationsGrid = new LocationsGrid<LocationX>(iLocationsCount, (int)NoGridWidth.Value, (int)NoGridHeight.Value, textBox1.Text, Looped.Checked ? WorldShape.Ringworld : WorldShape.Plain);
            }

            Cursor = Cursors.Arrow;

            tableLayoutPanel1.Enabled = true;
            GridPanel.Enabled = true;
            NoGridPanel.Enabled = true;
            panel1.Enabled = true;
            Looped.Enabled = true;

            button1.Enabled = true;
            button1.Text = "Build grid";

            if (m_pLocationsGrid == null)
            {
                return;
            }

            bool bAvailable = FileNameAvailable(m_sFilename);

            int iCounter = 0;
            while (!bAvailable)
            {
                iCounter++;
                bAvailable = FileNameAvailable(string.Format("{0}_{1}", m_sFilename, iCounter));
            }

            m_pLocationsGrid.Save(iCounter > 0 ? string.Format("{2}\\{0}_{1}.dxz", m_sFilename, iCounter, m_sWorkingFolder) : string.Format("{1}\\{0}.dxz", m_sFilename, m_sWorkingFolder));

            DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = m_pLocationsGrid == null ? DialogResult.Cancel : DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (m_pLocationsGrid == null)
                return;

            bool bAvailable = FileNameAvailable(m_sFilename);

            int iCounter = 0;
            while (!bAvailable)
            {
                iCounter++;
                bAvailable = FileNameAvailable(string.Format("{0}_{1}", m_sFilename, iCounter));
            }

            m_pLocationsGrid.Save(iCounter > 0 ? string.Format("{2}\\{0}_{1}.dxg", m_sFilename, iCounter, m_sWorkingFolder) : string.Format("{1}\\{0}.dxg", m_sFilename, m_sWorkingFolder));

            DialogResult = DialogResult.OK;
        }

        private bool FileNameAvailable(string sFileName)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(m_sWorkingFolder);
            FileInfo[] fileNames = dirInfo.GetFiles("*.*");

            foreach (FileInfo fi in fileNames)
            {
                string sDescription;
                int iLocationsCount;
                WorldShape eShape;
                if (fi.Name == sFileName + ".dxz" && LocationsGrid<LocationX>.CheckFile(fi.FullName, out sDescription, out iLocationsCount, out eShape))
                {
                    return false;
                }
            }

            return true;
        }

        private void GridWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender == GridWidth && GridSquare.Checked)
                GridHeight.SelectedIndex = GridWidth.SelectedIndex;

            UpdateDescription();
        }

        private string m_sFilename = "grid";

        private void UpdateDescription()
        {
            if (radioButton3.Checked)
            {
                if(Looped.Checked)
                    textBox1.Text = string.Format("Looped random points grid {0}x{1}, {2} points.", NoGridWidth.Value, NoGridHeight.Value, PointsCount.Text);
                else
                    textBox1.Text = string.Format("Random points grid {0}x{1}, {2} points.", NoGridWidth.Value, NoGridHeight.Value, PointsCount.Text);
                m_sFilename = string.Format("Rnd_{2}_{0}x{1}{3}", NoGridWidth.Value, NoGridHeight.Value, PointsCount.Text, Looped.Checked ? "_loop":"");
            }

            if (radioButton2.Checked)
            {
                if (Looped.Checked)
                    textBox1.Text = string.Format("Looped hexagonal grid {0}x{1}, {2} points.", GridWidth.Text, GridHeight.Text, int.Parse(GridWidth.Text) * int.Parse(GridHeight.Text) / 2);
                else
                    textBox1.Text = string.Format("Hexagonal grid {0}x{1}, {2} points.", GridWidth.Text, GridHeight.Text, int.Parse(GridWidth.Text) * int.Parse(GridHeight.Text) / 2);
                m_sFilename = string.Format("Hex_{0}x{1}{2}", GridWidth.Text, GridHeight.Text, Looped.Checked ? "_loop" : "");
            }
        }

        private void GridHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDescription();
        }

        private void PointsCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDescription();
        }

        private void NoGridWidth_ValueChanged(object sender, EventArgs e)
        {
            UpdateDescription();
        }

        private void NoGridHeight_ValueChanged(object sender, EventArgs e)
        {
            UpdateDescription();
        }

        private void Looped_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDescription();
        }
    }
}
