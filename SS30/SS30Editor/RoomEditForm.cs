using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SS30Conf;
using SS30Conf.Actions;

namespace SS30Editor
{
    public partial class RoomEditForm : Form
    {
        private CRoom m_pRoom;

        public RoomEditForm(CRoom pRoom)
        {
            InitializeComponent();

            m_pRoom = pRoom;

            textBox1.Text = pRoom.Value;
            textBox2.Text = pRoom.Name;
            textBox3.Text = pRoom.Description;

            numericUpDown1.Value = pRoom.CostToBuild;
            numericUpDown2.Value = pRoom.DaysToBuild;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pRoom.Value = textBox1.Text;
            m_pRoom.Name = textBox2.Text;
            m_pRoom.Description = textBox3.Text;

            m_pRoom.CostToBuild = (int)numericUpDown1.Value;
            m_pRoom.DaysToBuild = (int)numericUpDown2.Value;

            DialogResult = DialogResult.OK;
        }
    }
}
