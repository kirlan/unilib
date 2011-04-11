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
using SS30Conf.Actions.Conditions;
using SS30Conf.Actions.Commands;

namespace SS30Editor
{
    public partial class CommandBuildRoomEditForm : Form
    {
        private CCommandBuildRoom m_pCommand;

        public CommandBuildRoomEditForm(CCommandBuildRoom pCommand)
        {
            InitializeComponent();

            m_pCommand = pCommand;

            textBox1.Text = pCommand.Value;

            comboBox1.Items.Clear();
            foreach (CRoom room in MainForm.m_pWorld.Rooms)
            {
                comboBox1.Items.Add(room.Value);
            }

            if (pCommand.Room == null)
                comboBox1.SelectedItem = null;
            else
                comboBox1.SelectedItem = pCommand.Room.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pCommand.Value = textBox1.Text;

            if (comboBox1.SelectedItem == null)
                m_pCommand.Room = null;
            else
            {
                foreach (CRoom room in MainForm.m_pWorld.Rooms)
                {
                    if (comboBox1.SelectedItem as string == room.Value)
                    {
                        m_pCommand.Room = room;
                        break;
                    }
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}
