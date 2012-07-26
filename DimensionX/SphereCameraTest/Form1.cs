using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SphereCameraTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            _3dview1.MouseWheel += new MouseEventHandler(_3dview1_MouseWheel);
        }

        private bool m_b3dMapRotate = false;

        private void _3dview1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _3dview1.m_bPanMode = true;
                _3dview1.ResetPanning();
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                m_b3dMapRotate = true;
        }

        private void _3dview1_MouseUp(object sender, MouseEventArgs e)
        {
            _3dview1.m_bPanMode = false;
            m_b3dMapRotate = false;
        }

        private void _3dview1_MouseLeave(object sender, EventArgs e)
        {
            _3dview1.m_bPanMode = false;
            m_b3dMapRotate = false;
        }

        void _3dview1_MouseWheel(object sender, MouseEventArgs e)
        {
            _3dview1.m_fScaling = e.Delta;
            //            mapDraw3d1.m_fScaling = 0;
        }

        private void _3dview1_MouseEnter(object sender, EventArgs e)
        {
            _3dview1.Focus();
        }
    
        private Point m_pMap3DLastMouseLocation = new Point(0, 0);

        private void _3dview1_MouseMove(object sender, MouseEventArgs e)
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

            _3dview1.UpdatePicking(e.X, e.Y);

            if (m_b3dMapRotate)
                _3dview1.m_pCamera.Orbit(p.X, p.Y, 0);
        }

        bool m_bEnabled = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            m_bEnabled = true;
        }
    }
}
