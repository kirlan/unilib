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

        private bool m_b3dMapDragging = false;
        private bool m_b3dMapRotate = false;

        private void mapDraw3d1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                m_b3dMapDragging = true;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                m_b3dMapRotate = true;
        }

        private void mapDraw3d1_MouseUp(object sender, MouseEventArgs e)
        {
            m_b3dMapDragging = false;
            m_b3dMapRotate = false;
        }

        private void mapDraw3d1_MouseLeave(object sender, EventArgs e)
        {
            m_b3dMapDragging = false;
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
            Point p = new Point(0, 0);

            if (m_pMap3DLastMouseLocation.X > 0)
            {
                p.X = m_pMap3DLastMouseLocation.X - e.X;
                p.Y = m_pMap3DLastMouseLocation.Y - e.Y;
            }
            m_pMap3DLastMouseLocation = e.Location;

            if (m_b3dMapDragging)
                mapDraw3d1.PanCamera(p.X, p.Y);

            if (m_b3dMapRotate)
                //            mapDraw3d1.m_pCamera.Rotate(0, p.Y * 0.01f, p.X * 0.01f);
                //            mapDraw3d1.m_pCamera.Rotate(p.X * 0.01f, p.Y * 0.01f, 0);
                mapDraw3d1.m_pCamera.Orbit(p.X * 0.01f, p.Y * 0.01f, 0);
        }
    }
}
