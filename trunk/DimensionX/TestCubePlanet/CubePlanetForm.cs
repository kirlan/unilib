using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestCubePlanet
{
    public partial class CubePlanetForm : Form
    {
        public CubePlanetForm()
        {
            InitializeComponent();

            Cube pCube = new Cube();

            cubePlanetDraw3d1.Assign(pCube);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cubePlanetDraw3d1.m_bCamMode = !cubePlanetDraw3d1.m_bCamMode;
        }

        private void cubePlanetDraw3d1_MouseMove(object sender, MouseEventArgs e)
        {
            cubePlanetDraw3d1.UpdatePicking(e.X, e.Y);
        }
    }
}
