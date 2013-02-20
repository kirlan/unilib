using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace TestCubePlanet
{
    public partial class CubePlanetForm : Form
    {
        public CubePlanetForm()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 2;

            cubePlanetDraw3d1.MouseWheel += new MouseEventHandler(cubePlanetDraw3d1_MouseWheel);
        }

        Cube m_pCube = null;

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;
            cubePlanetDraw3d1.Visible = false;

            var now = DateTime.Now;

            int iSize = 1;

            switch (comboBox1.SelectedIndex)
            {
                case 0: iSize = 1;
                    break;
                case 1: iSize = 3;
                    break;
                case 2: iSize = 5;
                    break;
                case 3: iSize = 7;
                    break;
            }

            m_pCube = new Cube((int)numericUpDown1.Value, iSize);

            var interval = DateTime.Now - now;
            label1.Text = string.Format("Building time: {0:0.000}s (~{1:#####}k points)", interval.TotalSeconds, numericUpDown1.Value * 6 * iSize * iSize / 1000);

            now = DateTime.Now;
            cubePlanetDraw3d1.Assign(m_pCube, checkBox1.Checked);
            interval = DateTime.Now - now;
            label2.Text = string.Format("Graphics init time: {0:0.000}s", interval.TotalSeconds);
            
            cubePlanetDraw3d1.Visible = true;
            panel1.Enabled = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (m_pCube == null)
                return;

            var now = DateTime.Now;
            cubePlanetDraw3d1.Assign(m_pCube, checkBox1.Checked);
            var interval = DateTime.Now - now;
            label2.Text = string.Format("Graphics init time: {0:0.000}s", interval.TotalSeconds);
        }


        bool m_b3dMapRotate = false;

        private System.Drawing.Point m_pMap3DLastMouseLocation = new System.Drawing.Point(0, 0);

        private void cubePlanetDraw3d1_MouseMove(object sender, MouseEventArgs e)
        {
            cubePlanetDraw3d1.Focus();

            System.Drawing.Point p = new System.Drawing.Point(0, 0);

            if (m_pMap3DLastMouseLocation.X > 0)
            {
                p.X = m_pMap3DLastMouseLocation.X - e.X;
                p.Y = m_pMap3DLastMouseLocation.Y - e.Y;
            }
            m_pMap3DLastMouseLocation = e.Location;

            cubePlanetDraw3d1.UpdatePicking(e.X, e.Y);

            if (m_b3dMapRotate)
                cubePlanetDraw3d1.m_pCamera.Orbit(p.X * 0.01f, p.Y * 0.01f, 0);
                //cubePlanetDraw3d1.m_pCamera.Orbit(p.X * 0.01f, p.Y * 0.01f, 0);

            label4.Text = "Center X = " + cubePlanetDraw3d1.m_pCamera.m_pArcBallCenter.X.ToString();
            label5.Text = "Center Y = " + cubePlanetDraw3d1.m_pCamera.m_pArcBallCenter.Y.ToString();
            label6.Text = "Mouse X = " + cubePlanetDraw3d1.m_pCamera.m_fMouseX.ToString();
            label7.Text = "Mouse Y = " + cubePlanetDraw3d1.m_pCamera.m_fMouseY.ToString();
            label8.Text = "ArcBall X = " + cubePlanetDraw3d1.m_pCamera.m_pArcBallMouseRelative.X.ToString();
            label9.Text = "ArcBall Y = " + cubePlanetDraw3d1.m_pCamera.m_pArcBallMouseRelative.Y.ToString();
            label10.Text = "ArcBall Z = " + cubePlanetDraw3d1.m_pCamera.m_pArcBallMouseRelative.Z.ToString();

            label11.Text = "Axis X = " + cubePlanetDraw3d1.m_pCamera.m_pCursorPointRotationAxis.X.ToString();
            label12.Text = "Axis Y = " + cubePlanetDraw3d1.m_pCamera.m_pCursorPointRotationAxis.Y.ToString();
            label13.Text = "Axis Z = " + cubePlanetDraw3d1.m_pCamera.m_pCursorPointRotationAxis.Z.ToString();

            label14.Text = "Start X = " + cubePlanetDraw3d1.m_pCamera.m_pStartCursorPoint.X.ToString();
            label15.Text = "Start Y = " + cubePlanetDraw3d1.m_pCamera.m_pStartCursorPoint.Y.ToString();
            label16.Text = "Start Z = " + cubePlanetDraw3d1.m_pCamera.m_pStartCursorPoint.Z.ToString();
        }

        private void cubePlanetDraw3d1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && cubePlanetDraw3d1.m_pCurrentPicking.HasValue)
            {
                cubePlanetDraw3d1.m_bPanMode = true;
                cubePlanetDraw3d1.StartDrag();
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                m_b3dMapRotate = true;
        }

        private void cubePlanetDraw3d1_MouseUp(object sender, MouseEventArgs e)
        {
            cubePlanetDraw3d1.m_bPanMode = false;
            m_b3dMapRotate = false;
        }

        private void cubePlanetDraw3d1_MouseLeave(object sender, EventArgs e)
        {
            cubePlanetDraw3d1.m_bPanMode = false;
            m_b3dMapRotate = false;
        }

        private void cubePlanetDraw3d1_MouseWheel(object sender, MouseEventArgs e)
        {
            cubePlanetDraw3d1.m_fScaling = e.Delta / 5;
            //            mapDraw3d1.m_fScaling = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = "FPS: " + Math.Min(999, cubePlanetDraw3d1.m_iFrame).ToString();
        //    timer2.Interval = Math.Max(100, 800 / (mapDraw3d1.m_iFrame + 1));
            cubePlanetDraw3d1.m_iFrame = 0;
        }

        private void cubePlanetDraw3d1_Click(object sender, EventArgs e)
        {

        }
    }
}
