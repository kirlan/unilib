using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RandomStory
{
    public partial class ProblemsEdit : Form
    {
        private Repository m_pRepository;

        public ProblemsEdit(Repository pRep)
        {
            InitializeComponent();

            m_pRepository = pRep;

            problemsTextBox.Clear();
            problemsTextBox.Lines = m_pRepository.m_cProblems.ToArray();

            solutionsTextBox.Clear();
            solutionsTextBox.Lines = m_pRepository.m_cSolutions.ToArray();

            eventsTextBox.Clear();
            eventsTextBox.Lines = m_pRepository.m_cEvents.ToArray();

            problemsTextBox.Select(0,0);
        }

        private void problemsTextBox_TextChanged(object sender, EventArgs e)
        {
            m_pRepository.m_cProblems = new Strings(problemsTextBox.Lines);
        }

        private void solutionsTextBox_TextChanged(object sender, EventArgs e)
        {
            m_pRepository.m_cSolutions = new Strings(solutionsTextBox.Lines);
        }

        private void eventsTextBox_TextChanged(object sender, EventArgs e)
        {
            m_pRepository.m_cEvents = new Strings(eventsTextBox.Lines);
        }
    }
}
