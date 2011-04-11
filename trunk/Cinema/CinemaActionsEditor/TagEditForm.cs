using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CinemaEngine;

namespace CinemaActionsEditor
{
    public partial class TagEditForm : Form
    {
        private GenreTag m_pTag = null;

        public GenreTag GenreTag
        {
            get { return m_pTag; }
        }

        public TagEditForm(GenreTag pTag)
        {
            InitializeComponent();

            if (pTag != null)
                m_pTag = pTag;
            else
                m_pTag = new GenreTag();

            textBox1.Text = m_pTag.Name;

            checkBoxAction.Checked = m_pTag.Rating[GenreTag.Genre.Action] > 0;
            checkBoxFun.Checked = m_pTag.Rating[GenreTag.Genre.Fun] > 0;
            checkBoxGore.Checked = m_pTag.Rating[GenreTag.Genre.Gore] > 0;
            checkBoxHorror.Checked = m_pTag.Rating[GenreTag.Genre.Horror] > 0;
            checkBoxMistery.Checked = m_pTag.Rating[GenreTag.Genre.Mistery] > 0;
            checkBoxRomance.Checked = m_pTag.Rating[GenreTag.Genre.Romance] > 0;
            checkBoxXXX.Checked = m_pTag.Rating[GenreTag.Genre.XXX] > 0;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_pTag.Name = textBox1.Text;

            m_pTag.SetRating(GenreTag.Genre.Action, checkBoxAction.Checked ? 1 : 0);
            m_pTag.SetRating(GenreTag.Genre.Fun, checkBoxFun.Checked ? 1 : 0);
            m_pTag.SetRating(GenreTag.Genre.Gore, checkBoxGore.Checked ? 1 : 0);
            m_pTag.SetRating(GenreTag.Genre.Horror, checkBoxHorror.Checked ? 1 : 0);
            m_pTag.SetRating(GenreTag.Genre.Mistery, checkBoxMistery.Checked ? 1 : 0);
            m_pTag.SetRating(GenreTag.Genre.Romance, checkBoxRomance.Checked ? 1 : 0);
            m_pTag.SetRating(GenreTag.Genre.XXX, checkBoxXXX.Checked ? 1 : 0);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = !Universe.Instance.Tags.ContainsKey(textBox1.Text) || m_pTag.Name == textBox1.Text;
        }
    }
}
