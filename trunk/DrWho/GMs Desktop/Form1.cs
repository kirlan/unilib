using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GMs_Desktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void adventureToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void characterCreatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Character_Creator pForm = new Character_Creator();
            pForm.ShowDialog();
        }
    }
}
