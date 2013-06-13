using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WorldGeneration;
using Socium;

namespace DimensionXGame1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            cubePlanetDraw3d1.MouseWheel += new MouseEventHandler(cubePlanetDraw3d1_MouseWheel);
        }

        private World m_pWorld;

        private GenerationForm m_pGenerationForm = new GenerationForm();

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pGenerationForm.ShowDialog() == DialogResult.OK)
            {
                m_pWorld = m_pGenerationForm.World;
                cubePlanetDraw3d1.Assign(m_pWorld); 
            }
        }

        bool m_b3dMapRotate = false;

        private MouseEventArgs m_pMap3DLastMouseLocation = null;

        private bool m_bMouseLock = false;

        private void timer2_Tick(object sender, EventArgs e)
        {
            m_bMouseLock = false;
            //if (m_pMap3DLastMouseLocation != null)
            //    cubePlanetDraw3d1_MouseMove(sender, m_pMap3DLastMouseLocation);
            //m_pMap3DLastMouseLocation = null;
        }

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

            if (toolTip1.GetToolTip(cubePlanetDraw3d1) != cubePlanetDraw3d1.sToolTip)
            {
                toolTip1.SetToolTip(cubePlanetDraw3d1, cubePlanetDraw3d1.sToolTip);
                toolTip1.Active = true;
            }
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

        private void showBirdviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showBirdviewToolStripMenuItem.Checked = !showBirdviewToolStripMenuItem.Checked;

            cubePlanetDraw3d1.ShowFrustum = showBirdviewToolStripMenuItem.Checked;
        }

        private void useCelshadingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useCelshadingToolStripMenuItem.Checked = !useCelshadingToolStripMenuItem.Checked;

            cubePlanetDraw3d1.UseCelShading = useCelshadingToolStripMenuItem.Checked;
        }

        private void showBoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showBoundsToolStripMenuItem.Checked = !showBoundsToolStripMenuItem.Checked;

            cubePlanetDraw3d1.ShowBounds = showBoundsToolStripMenuItem.Checked;
        }
    }
}
