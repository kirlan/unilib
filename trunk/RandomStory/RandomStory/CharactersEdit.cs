using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RandomStory
{
    public partial class CharactersEdit : Form
    {
        private Repository m_pRepository;

        public CharactersEdit(Repository pRep)
        {
            InitializeComponent();
            m_pRepository = pRep;

            textBox1.Clear();
            textBox1.Lines = m_pRepository.m_cBloodRelations.ToArray();

            textBox2.Clear();
            textBox2.Lines = m_pRepository.m_cOtherRelations.ToArray();

            textBox1.Select(0, 0);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            m_pRepository.m_cBloodRelations = new Strings(textBox1.Lines);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            m_pRepository.m_cOtherRelations = new Strings(textBox2.Lines);
        }
    }
}
