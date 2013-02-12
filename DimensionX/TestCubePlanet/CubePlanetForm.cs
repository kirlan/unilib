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
        }

        private void cubePlanetDraw3d1_MouseMove(object sender, MouseEventArgs e)
        {
            cubePlanetDraw3d1.MouseMoving(e.X, e.Y);
            //cubePlanetDraw3d1.UpdatePicking(e.X, e.Y);
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
            
            radioButton2_CheckedChanged(this, new EventArgs());

            cubePlanetDraw3d1.Visible = true;
            panel1.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonFW.Checked)
            {
                cubePlanetDraw3d1.m_pCameraDir = Vector3.Backward;
                cubePlanetDraw3d1.m_pCameraUp = Vector3.Up;
            }
            if (radioButtonBK.Checked)
            {
                cubePlanetDraw3d1.m_pCameraDir = Vector3.Forward;
                cubePlanetDraw3d1.m_pCameraUp = Vector3.Up;
            }
            if (radioButtonDW.Checked)
            {
                cubePlanetDraw3d1.m_pCameraDir = Vector3.Up;
                cubePlanetDraw3d1.m_pCameraUp = Vector3.Forward;
            }
            if (radioButtonLF.Checked)
            {
                cubePlanetDraw3d1.m_pCameraDir = Vector3.Left;
                cubePlanetDraw3d1.m_pCameraUp = Vector3.Up;
            }
            if (radioButtonRT.Checked)
            {
                cubePlanetDraw3d1.m_pCameraDir = Vector3.Right;
                cubePlanetDraw3d1.m_pCameraUp = Vector3.Up;
            }
            if (radioButtonUP.Checked)
            {
                cubePlanetDraw3d1.m_pCameraDir = Vector3.Down;
                cubePlanetDraw3d1.m_pCameraUp = Vector3.Backward;
            }
            if (radioButtonN.Checked)
            {
                cubePlanetDraw3d1.m_pCameraDir = Vector3.Normalize(Vector3.Backward + Vector3.Right + Vector3.Down);
                cubePlanetDraw3d1.m_pCameraUp = -Vector3.Normalize(Vector3.Forward + Vector3.Left + Vector3.Down);
            }
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

        private void cubePlanetDraw3d1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && cubePlanetDraw3d1.m_pCurrentPicking.HasValue)
            {
                cubePlanetDraw3d1.m_bPanMode = true;
                cubePlanetDraw3d1.StartDrag();
            }
            //if (e.Button == System.Windows.Forms.MouseButtons.Right)
            //    m_b3dMapRotate = true;
        }

        private void cubePlanetDraw3d1_MouseUp(object sender, MouseEventArgs e)
        {
            cubePlanetDraw3d1.m_bPanMode = false;
        }
    }
}
