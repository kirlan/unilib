using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PickerTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void mapDraw3d1_MouseMove(object sender, MouseEventArgs e)
        {
            mapDraw3d1.UpdatePicking(e.X, e.Y);
            label1.Text = string.Format("глюк: {2}  -   курсор: x: {0}  y: {1}", e.X, e.Y, !mapDraw3d1.m_bCamMode);
            label2.Text = string.Format("3D-курсор: x: {0}  y: {1}", mapDraw3d1.m_iCursorX, mapDraw3d1.m_iCursorY);
        }

        private void mapDraw3d1_Click(object sender, EventArgs e)
        {
            mapDraw3d1.m_bCamMode = !mapDraw3d1.m_bCamMode;
        }
    }
}
