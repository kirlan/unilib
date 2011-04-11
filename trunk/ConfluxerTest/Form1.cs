using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NameGen;

namespace ConfluxerTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Beloslav Berimir Berislav Blagoslav Bogdan Boleslav Borimir Borislav Bratislav Bronislav Bryacheslav Budimir Velimir Velislav Vladimir Vladislav Vsevolod Vseslav Vyacheslav Gorislav Gostemil Gostomisl Gradimir Gradislav Granislav Dobromil Dobromir Dobromisl Dragomir Zvenislav Zlatomir Izyaslav Istislav Ladislav Lubomir Lubomisl Mechislav Milorad Miloslav Miroslav Mstislav Nevzor Ostromir Peresvet Putimir Putislav Radimir Radislav Ratibor Rodislav Rostislav Svetovid Svetozar Svyatogor Svyatopolk Svyatoslav Stanimir Stanislav Sudimir Sudislav Tverdimir Tverdislav Tihomir Yaromir Yaropolk Yaroslav
        private void button1_Click(object sender, EventArgs e)
        {
            if(richTextBox1.Text == "")
                return;

            Confluxer pConf = new Confluxer(richTextBox1.Text, (int)numericUpDown1.Value);

            listBox1.Items.Clear();
            for (int i = 0; i < 50; i++)
                listBox1.Items.Add(pConf.Generate());
        }
    }
}
