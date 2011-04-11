using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapGen;

namespace COM_test
{
    public partial class Form1 : Form
    {
        CFractalWorld world = new CFractalWorld();

        public Form1()
        {
            InitializeComponent();

            InitialSize.SelectedIndex = 1;
            FinalSize.SelectedIndex = 4;

            progressBar1.Visible = false;
            label6.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled         = false;
            InitialSize.Enabled     = false;
            FinalSize.Enabled       = false;
            WaterPercent.Enabled    = false;
            ContinentsCount.Enabled = false;
            IslandsCount.Enabled    = false;

            if (!debug.Checked)
            {
                progressBar1.Visible = true;
                label6.Visible = true;
            }

            int iDepth1 = InitialSize.SelectedIndex + 1;
            int iDepth2 = FinalSize.SelectedIndex + 1;

            int iWater = (int)WaterPercent.Value;

            int iContinents = (int)ContinentsCount.Value;
            int iIslands = (int)IslandsCount.Value;

            if (debug.Checked)
            {
                world.CreateSphere(new CFractalWorld.CreationArguments(iDepth1, iDepth2, iWater, iContinents, iIslands, 3), null);
                fractalMapView1.DrawMap(world);

                button1.Enabled = true;
                InitialSize.Enabled = true;
                FinalSize.Enabled = true;
                WaterPercent.Enabled = true;
                ContinentsCount.Enabled = true;
                IslandsCount.Enabled = true;
            }
            else
                backgroundWorker1.RunWorkerAsync(new CFractalWorld.CreationArguments(iDepth1, iDepth2, iWater, iContinents, iIslands, 3));
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Do not access the form's BackgroundWorker reference directly.
            // Instead, use the reference provided by the sender parameter.
            BackgroundWorker bw = sender as BackgroundWorker;

            // Extract the argument.
            CFractalWorld.CreationArguments args = (CFractalWorld.CreationArguments)e.Argument;

            // Start the time-consuming operation.
            world.CreateSphere(args, bw);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label6.Text = (string)e.UserState;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
            label6.Visible = false;

            fractalMapView1.DrawMap(world);

            button1.Enabled = true;
            InitialSize.Enabled = true;
            FinalSize.Enabled = true;
            WaterPercent.Enabled = true;
            ContinentsCount.Enabled = true;
            IslandsCount.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                fractalMapView1.SaveMap(saveFileDialog1.FileName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            world.Map.ClearLoneRegions(world.Depth);
            fractalMapView1.DrawMap(world);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int iDepth1 = InitialSize.SelectedIndex + 1;
            int iDepth2 = FinalSize.SelectedIndex + 1;

            int iWater = (int)WaterPercent.Value;

            int iContinents = (int)ContinentsCount.Value;
            int iIslands = (int)IslandsCount.Value;

            world.CreateSphereDebug(new CFractalWorld.CreationArguments(iDepth1, iDepth2, iWater, iContinents, iIslands, 3));

            fractalMapView1.DrawMap(world);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            world.IncreaseDepth();
            fractalMapView1.DrawMap(world);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            world.FinalizeDebug((int)WaterPercent.Value, 3);
            fractalMapView1.DrawMap(world);
        }

        private void fractalMapView1_RegionSelectedEvent(object sender, MapVis.FractalMapView2D.RegionSelectedEventArgs e)
        {
            label7.Text = string.Format("x:{0}   y:{1}   h:{2} ({3})   id:{4}", e.Region.X, e.Region.Y, e.Region.Height, e.Region.Height - world.Map.OceanLevel, e.Region.ContinentID);
        }
    }
}
