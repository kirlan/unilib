using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RB_test1
{
    public partial class Combatant : UserControl
    {
        public Combatant()
        {
            InitializeComponent();

            comboBox1.Items.Clear();
            comboBox1.Items.Add("0 - без оружия");
            comboBox1.Items.Add("1 - меч/топор");
            comboBox1.Items.Add("2 - кремниевые пистолеты");
            comboBox1.Items.Add("3 - автомат");
            comboBox1.Items.Add("4 - гаусс-винтовка");
            comboBox1.Items.Add("5 - лучемёт");
            comboBox1.Items.Add("6 - плазмаган");
            comboBox1.Items.Add("7 - дезинтегратор");
            comboBox1.Items.Add("8 - аннигилятор");
            comboBox1.SelectedIndex = 1;
        }

        private void weapon1_Load(object sender, EventArgs e)
        {

        }

        public int Body
        {
            get { return weapon1.Level; }
            set { weapon1.Level = value; }
        }

        public int TechTier
        {
            get { return weapon1.Tier; }
            set { weapon1.Tier = value; }
        }

        public int BioTier
        {
            get { return weapon2.Tier; }
            set { weapon2.Tier = value; }
        }

        public int Mind
        {
            get { return weapon2.Level; }
            set { weapon2.Level = value; }
        }

        public int Weapon
        {
            get { return comboBox1.SelectedIndex; }
            set { comboBox1.SelectedIndex = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Body = 10;
            TechTier = 1;
            Mind = 2;
            BioTier = 1;
            Weapon = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Body = 10;
            TechTier = 3;
            Mind = 4;
            BioTier = 1;
            Weapon = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Body = 2;
            TechTier = 1;
            Mind = 10;
            BioTier = 5;
            Weapon = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Body = 10;
            TechTier = 1;
            Mind = 10;
            BioTier = 7;
            Weapon = 1;
        }
    }
}
