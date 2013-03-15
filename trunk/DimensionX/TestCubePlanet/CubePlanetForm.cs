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
            m_iLastSize = GetSquareSize();
            comboBox2.SelectedIndex = 3;

            trackBar2.Value = 31 - (int)(1.0f / (cubePlanetDraw3d1.TimeSpeed * 1000));
            checkBox6.Checked = cubePlanetDraw3d1.TimeWarp;

            cubePlanetDraw3d1.MouseWheel += new MouseEventHandler(cubePlanetDraw3d1_MouseWheel);
        }

        Cube m_pCube = null;

        private int GetSquareSize()
        {
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
                case 4: iSize = 9;
                    break;
                case 5: iSize = 17;
                    break;
                case 6: iSize = 25;
                    break;
            }

            return iSize;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Enabled = false;
            cubePlanetDraw3d1.Visible = false;

            var now = DateTime.Now;

            int iSize = GetSquareSize();

            Cube.WorkingArea eArea = Cube.WorkingArea.OneFace;
            switch (comboBox2.SelectedIndex)
            {
                case 0: eArea = Cube.WorkingArea.OneFace;
                    break;
                case 1: eArea = Cube.WorkingArea.HalfSphereEquatorial;
                    break;
                case 2: eArea = Cube.WorkingArea.HalfSpherePolar;
                    break;
                case 3: eArea = Cube.WorkingArea.WholeSphere;
                    break;
            }
            
            cubePlanetDraw3d1.Clear();
            m_pCube = null;

            m_pCube = new Cube((int)numericUpDown1.Value, iSize, eArea);

            var interval = DateTime.Now - now;
            label1.Text = string.Format("Building time: {0:0.000}s", interval.TotalSeconds);

            now = DateTime.Now;
            cubePlanetDraw3d1.Assign(m_pCube);
            interval = DateTime.Now - now;
            label2.Text = string.Format("Graphics init time: {0:0.000}s", interval.TotalSeconds);

            int iLocs = 0;
            foreach (var pFace in m_pCube.m_cFaces)
                foreach (var pChunk in pFace.Value.m_cChunk)
                    iLocs += pChunk.m_aLocations.Length;

            label6.Text = string.Format("Actual locations: ~{0:#####}k", iLocs/1000);
            
            cubePlanetDraw3d1.Visible = true;
            panel2.Enabled = true;
        }

        bool m_b3dMapRotate = false;

        private MouseEventArgs m_pMap3DLastMouseLocation = null;

        private void cubePlanetDraw3d1_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_bMouseLock)
                return;

            m_bMouseLock = true;

            cubePlanetDraw3d1.Focus();

            System.Drawing.Point p = new System.Drawing.Point(0, 0);

            if (m_pMap3DLastMouseLocation != null)
            {
                p.X = m_pMap3DLastMouseLocation.X - e.X;
                p.Y = m_pMap3DLastMouseLocation.Y - e.Y;
            }
            m_pMap3DLastMouseLocation = e;

            cubePlanetDraw3d1.UpdatePicking(e.X, e.Y);

            if (m_b3dMapRotate)
                cubePlanetDraw3d1.m_pCamera.Orbit(p.X * 0.01f, p.Y * 0.01f, 0);
                //cubePlanetDraw3d1.m_pCamera.Orbit(p.X * 0.01f, p.Y * 0.01f, 0);

            //label4.Text = "Center X = " + cubePlanetDraw3d1.m_pCamera.m_pArcBallCenter.X.ToString();
            //label5.Text = "Center Y = " + cubePlanetDraw3d1.m_pCamera.m_pArcBallCenter.Y.ToString();
            //label6.Text = "Mouse X = " + cubePlanetDraw3d1.m_pCamera.m_fMouseX.ToString();
            //label7.Text = "Mouse Y = " + cubePlanetDraw3d1.m_pCamera.m_fMouseY.ToString();
            //label8.Text = "ArcBall X = " + cubePlanetDraw3d1.m_pCamera.m_pArcBallMouseRelative.X.ToString();
            //label9.Text = "ArcBall Y = " + cubePlanetDraw3d1.m_pCamera.m_pArcBallMouseRelative.Y.ToString();
            //label10.Text = "ArcBall Z = " + cubePlanetDraw3d1.m_pCamera.m_pArcBallMouseRelative.Z.ToString();
            //if (cubePlanetDraw3d1.m_pCurrentPicking.HasValue)
            //{
            //    label8.Text = "Selection X = " + cubePlanetDraw3d1.m_pCurrentPicking.Value.X.ToString();
            //    label9.Text = "Selection Y = " + cubePlanetDraw3d1.m_pCurrentPicking.Value.Y.ToString();
            //    label10.Text = "Selection Z = " + cubePlanetDraw3d1.m_pCurrentPicking.Value.Z.ToString();
            //}

            //label11.Text = "Axis X = " + cubePlanetDraw3d1.m_pCamera.m_pAxis.X.ToString();
            //label12.Text = "Axis Y = " + cubePlanetDraw3d1.m_pCamera.m_pAxis.Y.ToString();
            //label13.Text = "Axis Z = " + cubePlanetDraw3d1.m_pCamera.m_pAxis.Z.ToString();

            //label14.Text = "Start X = " + cubePlanetDraw3d1.m_pCamera.m_pStartCursorPoint.X.ToString();
            //label15.Text = "Start Y = " + cubePlanetDraw3d1.m_pCamera.m_pStartCursorPoint.Y.ToString();
            //label16.Text = "Start Z = " + cubePlanetDraw3d1.m_pCamera.m_pStartCursorPoint.Z.ToString();
        }

        private void cubePlanetDraw3d1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && cubePlanetDraw3d1.m_pCurrentPicking.HasValue)
            {
                cubePlanetDraw3d1.m_pTarget = cubePlanetDraw3d1.m_pCurrentPicking;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                m_b3dMapRotate = true;
        }

        private void cubePlanetDraw3d1_MouseUp(object sender, MouseEventArgs e)
        {
            //cubePlanetDraw3d1.m_bPanMode = false;
            m_b3dMapRotate = false;
        }

        private void cubePlanetDraw3d1_MouseLeave(object sender, EventArgs e)
        {
            //cubePlanetDraw3d1.m_bPanMode = false;
            m_b3dMapRotate = false;
        }

        private void cubePlanetDraw3d1_MouseWheel(object sender, MouseEventArgs e)
        {
            cubePlanetDraw3d1.m_fScaling = e.Delta / 5;
            //            mapDraw3d1.m_fScaling = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = string.Format("Draw Time: {0:0.000}s", cubePlanetDraw3d1.DrawingTime/1000);
            label5.Text = string.Format("Frame Time: {0:0.000}s", cubePlanetDraw3d1.FrameTime / 1000);
            label7.Text = string.Format("Triangles: ~{0}k", cubePlanetDraw3d1.TrianglesCount/1000);

            label9.Text = "Visible Squares: " + cubePlanetDraw3d1.VisibleQueue.ToString();
            label10.Text = "Cached Squares: " + cubePlanetDraw3d1.InvisibleQueue.ToString();

            label22.Text = string.Format("World time: {0}:{1:00}", cubePlanetDraw3d1.DayTime.Hours, cubePlanetDraw3d1.DayTime.Minutes);
        }

        private void cubePlanetDraw3d1_Click(object sender, EventArgs e)
        {

        }

        private void CubePlanetForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
                Application.Exit();
        }

        private void panel2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
                Application.Exit();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int iSize = GetSquareSize();
            label17.Text = string.Format("~{0:#####}k locations", numericUpDown1.Value * 6 * iSize * iSize / 1000);
        }

        private int m_iLastSize = 5;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSize = GetSquareSize();
            if (checkBox2.Checked)
            {
                int iCount = (int)numericUpDown1.Value * 6 * m_iLastSize * m_iLastSize / 1000;
                numericUpDown1.Value = Math.Min(numericUpDown1.Maximum, Math.Max(numericUpDown1.Minimum, iCount * 1000 / (6 * iSize * iSize)));
            }

            label17.Text = string.Format("~{0:#####}k locations", numericUpDown1.Value * 6 * iSize * iSize / 1000);
            m_iLastSize = iSize;
        }

        private void numericUpDown1_KeyUp(object sender, KeyEventArgs e)
        {
            int iSize = GetSquareSize();
            label17.Text = string.Format("~{0:#####}k locations", numericUpDown1.Value * 6 * iSize * iSize / 1000);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            cubePlanetDraw3d1.WireFrame = checkBox1.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            cubePlanetDraw3d1.UseCelShading = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            cubePlanetDraw3d1.ShowBounds = checkBox4.Checked;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label18.Text = "LOD distance: " + trackBar1.Value;
            cubePlanetDraw3d1.LODDistance = trackBar1.Value;
        }

        private bool m_bMouseLock = false;

        private void timer2_Tick(object sender, EventArgs e)
        {
            m_bMouseLock = false;
            //if (m_pMap3DLastMouseLocation != null)
            //    cubePlanetDraw3d1_MouseMove(sender, m_pMap3DLastMouseLocation);
            //m_pMap3DLastMouseLocation = null;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            cubePlanetDraw3d1.NeedDrawTrees = checkBox5.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            cubePlanetDraw3d1.TimeWarp = checkBox6.Checked;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            cubePlanetDraw3d1.TimeSpeed = 1.0f / ((31 - trackBar2.Value) * 1000);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            cubePlanetDraw3d1.ShowFrustum = checkBox7.Checked;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            label3.Text = "FPS: " + Math.Min(999, cubePlanetDraw3d1.FPS).ToString();
            //    timer2.Interval = Math.Max(100, 800 / (mapDraw3d1.m_iFrame + 1));
            timer2.Interval = Math.Max(50, 800 / (cubePlanetDraw3d1.FPS + 1));
            cubePlanetDraw3d1.ResetFPS();
        }
    }
}
