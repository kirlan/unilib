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
    }
}
