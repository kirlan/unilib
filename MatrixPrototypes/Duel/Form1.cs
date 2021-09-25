using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Duel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            cowboy2.EndTurn();
        }

        private void cowboy1_Load(object sender, EventArgs e)
        {

        }

        private bool m_bBattle = false;

        public bool Battle
        {
            get { return m_bBattle; }
            set 
            { 
                m_bBattle = value;

                button1.Text = m_bBattle ? "Reset" : "Start";

                cowboy1.EditMode = !m_bBattle;
                cowboy2.EditMode = !m_bBattle;

                button2.Enabled = m_bBattle;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_bEnemyTurn = false;
            cowboy2.EndTurn();
            cowboy1.NextTurn();
            Battle = !Battle;
        }

        private bool m_bEnemyTurn = false;

        private void button2_Click(object sender, EventArgs e)
        {
            if (m_bEnemyTurn)
            {
                cowboy2.EndTurn();
                cowboy1.NextTurn();
            }
            else
            {
                cowboy1.EndTurn();
                cowboy2.NextTurn();
            }
            m_bEnemyTurn = !m_bEnemyTurn;
        }
    }
}
