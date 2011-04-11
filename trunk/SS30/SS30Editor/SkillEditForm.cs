using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SS30Conf;

namespace SS30Editor
{
    public partial class SkillEditForm : Form
    {
        private CSkill m_pSkill;

        public SkillEditForm(CSkill pSkill)
        {
            InitializeComponent();

            m_pSkill = pSkill;

            textBox1.Text = pSkill.Value;
            textBox2.Text = pSkill.Name;
            textBox3.Text = pSkill.Description;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pSkill.Value = textBox1.Text;
            m_pSkill.Name = textBox2.Text;
            m_pSkill.Description = textBox3.Text;

            DialogResult = DialogResult.OK;
        }
    }
}
