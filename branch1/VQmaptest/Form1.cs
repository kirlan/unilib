using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VQmaptest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            label1.Text = "Время работы: ";
            timer1.Enabled = false;
        }

        World m_pWorld;

        long count = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            count = 0;
            timer1.Enabled = true;

            m_pWorld = new World();

            worldMap1.DrawMap(m_pWorld);

            timer1.Enabled = false;
            label1.Text = "Время работы: " + count.ToString() + " сек.";
            button1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            count++;
        }
    }
}
