using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using LandscapeGeneration;
using Socium;

namespace WorldGeneration
{
    public partial class GridsManager : Form
    {
        string m_sWorkingDir;

        public GridsManager(string sWorkingDir)
        {
            InitializeComponent();

            button2.Enabled = false;
            button3.Enabled = false;

            m_sWorkingDir = sWorkingDir;

            listBox1.Items.Clear();

            DirectoryInfo dirInfo = new DirectoryInfo(m_sWorkingDir);
            FileInfo[] fileNames = dirInfo.GetFiles("*.*");

            foreach (FileInfo fi in fileNames)
            {
                if (fi.Extension == ".dxg")
                {
                    try
                    {
                        LocationsGrid<LocationX> pGrid = new LocationsGrid<LocationX>(fi.FullName);
                        listBox1.Items.Add(pGrid);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = listBox1.SelectedIndex != -1;
            button3.Enabled = listBox1.SelectedIndex != -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GridBuildingForm pForm = new GridBuildingForm(m_sWorkingDir);

            if (pForm.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Add(pForm.m_pLocationsGrid);
                listBox1.SelectedItem = pForm.m_pLocationsGrid;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;

            LocationsGrid<LocationX> pGrid = listBox1.SelectedItem as LocationsGrid<LocationX>;

            if (MessageBox.Show("This will delete grid file '" + pGrid.ToString() + "'. Are you sure?", "Warning!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.Delete(pGrid.m_sFilename);

                listBox1.Items.Remove(pGrid);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;

            LocationsGrid<LocationX> pGrid = listBox1.SelectedItem as LocationsGrid<LocationX>;

            FileInfo pInfo = new FileInfo(pGrid.m_sFilename);
            string sDesc = Microsoft.VisualBasic.Interaction.InputBox("Please, enter description text for grid '" + pInfo.Name +"':", "Edit grid description", pGrid.m_sDescription);

            if (sDesc.Length > 0)
            {
                Enabled = false;

                pGrid.Load(null, null);
                pGrid.m_sDescription = sDesc;
                pGrid.Save(pGrid.m_sFilename);

                listBox1.Items.Remove(pGrid);
                listBox1.Items.Add(pGrid);

                listBox1.SelectedItem = pGrid;

                Enabled = true;
            }
        }
    }
}
