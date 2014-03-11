using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Random;
using nsUniLibControls;
using System.Threading;

namespace RandomStory
{
    public partial class Randomizer : Form
    {
        public object SelectedItem = null;

        public delegate object GetRandomObject();

        private GetRandomObject m_dGRO;

        public Randomizer(string sCaption, GetRandomObject dGRO, object pSelectedItem)
        {
            InitializeComponent();

            SelectedItem = pSelectedItem;

            m_dGRO = dGRO;

            label2.Text = sCaption;

            if (SelectedItem != null && !string.IsNullOrWhiteSpace(SelectedItem.ToString()))
                label1.Text = SelectedItem.ToString();
            else
                button1.Enabled = false;

            ApplyFilter(m_sFilter);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval += timer1.Interval / 7;
            if (timer1.Interval > 600)
            {
                button1.Enabled = timer1.Enabled;
                button2.Enabled = timer1.Enabled;
                button3.Enabled = timer1.Enabled;
                label3.Enabled = timer1.Enabled;
                timer1.Enabled = false;
                return;
            }

            int iTries = 500;
            object pNewObject;
            do
            {
                iTries--;
                pNewObject = m_dGRO();
            }
            while (!CheckFilter(pNewObject.ToString()) && iTries > 0);

            if (iTries > 0)
            {
                SelectedItem = pNewObject;
                label1.Text = SelectedItem.ToString();
            }
            else
            {
                label1.Text = "";
                label1.Refresh();
                Thread.Sleep(100);
                label1.Text = SelectedItem == null ? "<крутите рулетку>" : SelectedItem.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = timer1.Enabled;
            button2.Enabled = timer1.Enabled;
            button3.Enabled = timer1.Enabled;
            label3.Enabled = timer1.Enabled;

            timer1.Interval = 80 + Rnd.Get(40);
            timer1.Enabled = true;

            label1.Focus();
        }

        private static string m_sFilter = "";

        private bool CheckFilter(string sTest)
        {
            return CheckFilter(m_sFilter, sTest);
        }

        private bool CheckFilter(string sFilter, string sTest)
        {
            if (string.IsNullOrWhiteSpace(sFilter))
                return true;

            string[] aFilter = sFilter.Split(';');

            if (aFilter.Length == 0)
                return true;

            foreach (string sFilterToken in aFilter)
                if (!sTest.Contains(sFilterToken))
                    return false;

            return true;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            string sFilter = m_sFilter;
            if (InputBox.Show("Фильтр рулетки", "Можно задать несколько фильтров, перечислив их через ';'. Будут отображаться только те строки, в которых присутствуют все фильтры.", ref sFilter) == DialogResult.OK)
                ApplyFilter(sFilter);
        }

        private void ApplyFilter(string sFilter)
        {
            if (string.IsNullOrWhiteSpace(sFilter))
            {
                m_sFilter = sFilter;
                UpdateFilterString();
                return;
            }

            int iTries = 1500;
            object pNewObject;
            do
            {
                iTries--;
                pNewObject = m_dGRO();
            }
            while (!CheckFilter(sFilter, pNewObject.ToString()) && iTries > 0);

            button2.Enabled = iTries > 0;

            if (iTries > 0)
                m_sFilter = sFilter;
            else
            {
                DialogResult eRes = MessageBox.Show("Заданный фильтр слишком сложен (вероятность соответствия < 1/1500) или вообще не корректен!", "Фильтр рулетки", MessageBoxButtons.AbortRetryIgnore);
                if (eRes == DialogResult.Ignore)
                {
                    m_sFilter = sFilter;
                }
                else if (eRes == DialogResult.Retry)
                {
                    m_sFilter = sFilter;
                    label3_Click(this, new EventArgs());
                }
            }
            UpdateFilterString();

            label1.Focus();
        }

        private void UpdateFilterString()
        {
            if (string.IsNullOrWhiteSpace(m_sFilter))
                label3.Text = "Фильтр";
            else
                label3.Text = string.Format("Фильтр: ({0})", m_sFilter);
        }

        private void Randomizer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ' && button2.Enabled)
                button2_Click(sender, new EventArgs());
        }

        private void Randomizer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyCode == Keys.Space && button2.Enabled)
                button2_Click(sender, new EventArgs());
        }
    }
}
