using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GridBuilderTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            mapDraw3d1.MouseWheel += new MouseEventHandler(mapDraw3d1_MouseWheel);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            Refresh();
            LocationsGrid<Location> pGrid = new LocationsGrid<Location>(2000, 314, 100, "цилиндр", true);

            //foreach (Location pLoc in pGrid.m_aLocations)
            //    pLoc.m_fHeight = 0;

            //foreach (Vertex pVertex in pGrid.m_aVertexes)
            //    pVertex.m_fZ = 0;

            mapDraw3d1.Assign(pGrid);
            button1.Enabled = true;
        }

        private bool m_b3dMapRotate = false;

        private void mapDraw3d1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mapDraw3d1.m_bPanMode = true;
                listBox1.Items.Clear();
                mapDraw3d1.ResetPanning();
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                m_b3dMapRotate = true;
        }

        private void mapDraw3d1_MouseUp(object sender, MouseEventArgs e)
        {
            mapDraw3d1.m_bPanMode = false;
            m_b3dMapRotate = false;
        }

        private void mapDraw3d1_MouseLeave(object sender, EventArgs e)
        {
            mapDraw3d1.m_bPanMode = false;
            m_b3dMapRotate = false;
        }

        void mapDraw3d1_MouseWheel(object sender, MouseEventArgs e)
        {
            mapDraw3d1.m_fScaling = e.Delta;
            //            mapDraw3d1.m_fScaling = 0;
        }

        private void mapDraw3d1_MouseEnter(object sender, EventArgs e)
        {
            mapDraw3d1.Focus();
        }

        private Point m_pMap3DLastMouseLocation = new Point(0, 0);

        private void mapDraw3d1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!m_bEnabled)
                return;

            m_bEnabled = false;

            Point p = new Point(0, 0);

            if (m_pMap3DLastMouseLocation.X > 0)
            {
                p.X = m_pMap3DLastMouseLocation.X - e.X;
                p.Y = m_pMap3DLastMouseLocation.Y - e.Y;
            }
            m_pMap3DLastMouseLocation = e.Location;

            mapDraw3d1.UpdatePicking(e.X, e.Y);
            label1.Text = mapDraw3d1.m_fDelta.ToString();

            if (mapDraw3d1.m_bPanMode)
                listBox1.Items.Add(mapDraw3d1.m_iCounter.ToString() + " : " + mapDraw3d1.m_fDeltaBig.ToString() + "  (" + mapDraw3d1.m_fDelta.ToString() + ")  [" + (mapDraw3d1.m_fCurrent - mapDraw3d1.m_fSelected) + " - " + (mapDraw3d1.m_fTarget - mapDraw3d1.m_fSelected) + "]");

            if (m_b3dMapRotate)
                //            mapDraw3d1.m_pCamera.Rotate(0, p.Y * 0.01f, p.X * 0.01f);
                //            mapDraw3d1.m_pCamera.Rotate(p.X * 0.01f, p.Y * 0.01f, 0);
                mapDraw3d1.m_pCamera.Orbit(p.X, p.Y, 0);
        }

        bool m_bEnabled = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            m_bEnabled = true;
        }
    }
}
