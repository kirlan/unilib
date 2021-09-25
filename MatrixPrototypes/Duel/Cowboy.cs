using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Random;

namespace Duel
{
    public partial class Cowboy : UserControl
    {
        public Cowboy()
        {
            InitializeComponent();

            ApplyEditMode(true);
            RND_Button_Click(this, null);

            GENR.SelectedIndex = 0;

            MODL.Items.Clear();
            foreach (Soft.Model eModel in Enum.GetValues(typeof(Soft.Model)))
                MODL.Items.Add(eModel.ToString().Replace('_', ' '));
            MODL.SelectedIndex = 0;

            listView1.Items.Clear();
        }

        private Cowboy m_pOpponent = null;

        public Cowboy Opponent
        {
            get { return m_pOpponent; }
            set { m_pOpponent = value; }
        }

        private bool m_bEditMode = true;
        private int m_iTurn = 0;

        public bool EditMode
        {
            get { return m_bEditMode; }
            set 
            {
                if (m_bEditMode != value)
                    ApplyEditMode(value);
            }
        }

        private void ApplyEditMode(bool bValue)
        {
            m_bEditMode = bValue;

            RND_Button.Enabled = m_bEditMode;
            
            MEMR.Enabled = m_bEditMode;
            PROC.Enabled = m_bEditMode;
            CONN.Enabled = m_bEditMode;
            
            SURF.Enabled = m_bEditMode;
            TUNE.Enabled = m_bEditMode;
            HACK.Enabled = m_bEditMode;
            INTU.Enabled = m_bEditMode;


            START_Button.Enabled = !m_bEditMode;
            
            CATG.Enabled = !m_bEditMode;
            GENR.Enabled = !m_bEditMode;
            MODL.Enabled = !m_bEditMode;

            SetStats();

            m_iTurn = 0;
            richTextBox1.Clear();
        
            listView1.Items.Clear();

            CATG.Items.Clear();
            foreach (Soft.Category eCat in Enum.GetValues(typeof(Soft.Category)))
                CATG.Items.Add(eCat.ToString().Replace('_', ' '));
            CATG.SelectedIndex = 0;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void RND_Button_Click(object sender, EventArgs e)
        {
            MEMR.Value = 1 + (int)Math.Pow(Rnd.Get(19f), 3) / 1000;
            PROC.Value = 1 + (int)Math.Pow(Rnd.Get(19f), 3) / 1000;
            CONN.Value = 1 + (int)Math.Pow(Rnd.Get(19f), 3) / 1000;

            SURF.Value = 2 + Rnd.Get(4);
            TUNE.Value = 2 + Rnd.Get(4);
            HACK.Value = 2 + Rnd.Get(4);
            INTU.Value = 2 + Rnd.Get(4);

            SetStats();
        }

        private void SetStats()
        {
            COSP.Value = 0;
            COSP.Maximum = CONN.Value * CONN.Value;
            SetCOSP(COSP.Maximum);

            AP.Value = 0;
            AP.Maximum = PROC.Value + SURF.Value*2;
            SetAP(AP.Maximum);

            FMEM.Value = 0;
            FMEM.Maximum = MEMR.Value;
            SetFMEM(FMEM.Maximum);
        }

        private void SetCOSP(int iValue)
        {
            if (iValue < 0)
                iValue = 0;

            if (iValue > COSP.Maximum)
                iValue = COSP.Maximum;

            COSP.Value = iValue;

            COSP_Str.Text = COSP.Value.ToString() + "/" + COSP.Maximum.ToString();
        }

        private void SetAP(int iValue)
        {
            if (iValue < 0)
                iValue = 0;

            if (iValue > AP.Maximum)
                iValue = AP.Maximum;

            AP.Value = iValue;

            AP_Str.Text = AP.Value.ToString() + "/" + AP.Maximum.ToString();
        }

        private void SetFMEM(int iValue)
        {
            if (iValue > FMEM.Maximum)
                iValue = FMEM.Maximum;

            FMEM.Value = iValue;

            FMEM_Str.Text = FMEM.Value.ToString() + "/" + FMEM.Maximum.ToString();
        }

        private void START_Button_Click(object sender, EventArgs e)
        {
            if (CATG.SelectedIndex == -1 ||
                GENR.SelectedIndex == -1 ||
                MODL.SelectedIndex == -1)
                return;

            //if (AP.Value < 2 ||
            //    FMEM.Value < 1)
            //    return;

            Soft pSoft = new Soft((Soft.Category)Enum.GetValues(typeof(Soft.Category)).GetValue(CATG.SelectedIndex), GENR.SelectedIndex + 1, (Soft.Model)Enum.GetValues(typeof(Soft.Model)).GetValue(MODL.SelectedIndex));

            ListViewItem pItem = new ListViewItem(pSoft.CATG.ToString().Replace('_',' '));
            pItem.SubItems.Add(pSoft.GENR.ToString());
            pItem.SubItems.Add(pSoft.MODL.ToString().Replace('_', ' '));
            pItem.Tag = pSoft;

            listView1.Items.Add(pItem);

            Event("Launched " + pSoft.ToString());

            pSoft.Run(this, m_pOpponent);

            //CATG.Items.RemoveAt(CATG.SelectedIndex);

            //if(CATG.Items.Count > 0)
            //    CATG.SelectedIndex = 0;

            SetAP(AP.Value - 1);
            if (!pSoft.Quick)
            {
                SetFMEM(FMEM.Value - 1);
                SetAP(AP.Value - 1);
            }

            CATG_SelectedIndexChanged(this, null);
        }

        public void NextTurn()
        {
            groupBox1.Enabled = true;
            
            richTextBox1.AppendText("");
            //Event("Begin");

            m_bParalized = false;
            m_bTotallyParalized = false;

            m_iTurn++;

            foreach (ListViewItem pItem in listView1.Items)
            {
                (pItem.Tag as Soft).Run(this, m_pOpponent);
            }
        
            CATG_SelectedIndexChanged(this, null);
        }

        public void EndTurn()
        {
            groupBox1.Enabled = false;
            //Event("End");
            SetAP(AP.Maximum - listView1.Items.Count);
        }

        public Soft GetSoft(Soft.Category eCat)
        {
            foreach (ListViewItem pItem in listView1.Items)
                if ((pItem.Tag as Soft).CATG == eCat)
                    return pItem.Tag as Soft;

            return new Soft(eCat, 0, Soft.Model.Base);
        }

        public void KillSoft(Soft.Category eCat)
        {
            ListViewItem pDead = null;

            foreach (ListViewItem pItem in listView1.Items)
                if ((pItem.Tag as Soft).CATG == eCat)
                {
                    pDead = pItem;
                    break;
                }

            if (pDead != null)
            {
                listView1.Items.Remove(pDead);
                if(!(pDead.Tag as Soft).Quick)
                    SetFMEM(FMEM.Value + 1);
                CATG_SelectedIndexChanged(this, null);
            }
        }

        public void Event(string sText)
        {
            richTextBox1.AppendText(string.Format("Turn {0}: {1}\n", m_iTurn, sText));
            richTextBox1.ScrollToCaret();
        }

        private void CATG_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CATG.SelectedIndex == -1)
            {
                START_Button.Enabled = false;
                return;
            }

            Soft.Category eCat = (Soft.Category)Enum.GetValues(typeof(Soft.Category)).GetValue(CATG.SelectedIndex);

            START_Button.Enabled = !m_bEditMode && (Soft.IsQuick(eCat) ? (AP.Value > 0) : (AP.Value > 1 && FMEM.Value > 0));

            foreach (ListViewItem pItem in listView1.Items)
                if ((pItem.Tag as Soft).CATG == eCat)
                    START_Button.Enabled = false;
        }

        private bool m_bInvisible = false;

        public bool Invisible
        {
            get { return m_bInvisible; }
            set { m_bInvisible = value; }
        }

        public void Damage(int iPoints)
        {
            if (COSP.Value > iPoints)
            {
                Event("Lost " + iPoints.ToString() + " COSP!");
                SetCOSP(COSP.Value - iPoints);
            }
            else
            {
                Event("Lost " + COSP.Value.ToString() + " COSP!");
                SetCOSP(0);
            }
        }

        private bool m_bParalized = false;
        private bool m_bTotallyParalized = false;

        public void Paralize(bool bFull)
        {
            if (!bFull)
            {
                if (!m_bParalized)
                {
                    Event("Lost " + SURF.Value.ToString() + " AP!");
                    SetAP(AP.Value - SURF.Value);
                    m_bParalized = true;
                }
            }
            else
            {
                if (!m_bTotallyParalized)
                {
                    Event("Lost " + (SURF.Value * 2).ToString() + " AP!");
                    SetAP(AP.Value - SURF.Value * 2);
                    m_bParalized = true;
                    m_bTotallyParalized = true;
                }
            }
        }

        private void MEMR_ValueChanged(object sender, EventArgs e)
        {
            SetStats();
        }
    }
}
