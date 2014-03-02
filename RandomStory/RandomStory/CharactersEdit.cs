using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            textBox1.Lines = m_pRepository.m_cRelations.ToArray();

            textBox1.Select(0, 0);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            m_pRepository.m_cRelations = new Strings(textBox1.Lines);
        }
    }
}
