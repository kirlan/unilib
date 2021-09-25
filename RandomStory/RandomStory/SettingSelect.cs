using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Random;

namespace RandomStory
{
    public partial class SettingSelect : Form
    {
        private Repository m_pRepository;

        public List<Setting> m_cAllowedWorlds = new List<Setting>();
        public List<Setting> m_cPrimedWorlds = new List<Setting>();

        public List<Setting> m_cAllowedJanres = new List<Setting>();
        public List<Setting> m_cPrimedJanres = new List<Setting>();

        public bool Voyagers
        {
            get { return checkBox1.Checked; }
        }

        public SettingSelect(Repository pRepository)
        {
            InitializeComponent();

            m_pRepository = pRepository;

            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(m_pRepository.m_cWorlds.ToArray());

            checkedListBox2.Items.Clear();
            checkedListBox2.Items.AddRange(m_pRepository.m_cJanres.ToArray());

            button2_Click(this, new EventArgs());
        }

        private void Update(CheckedListBox pCheckedListBox, Setting[] aSettings)
        {
            bool bAllChecked = (pCheckedListBox.CheckedItems.Count == pCheckedListBox.Items.Count);

            List<Setting> cAllowed = new List<Setting>();
            List<Setting> cPrimed = new List<Setting>();
            for (int i = 0; i < pCheckedListBox.Items.Count; i++)
            {
                CheckState eState = pCheckedListBox.GetItemCheckState(i);

                if (eState == CheckState.Unchecked)
                    continue;

                Setting pItem = pCheckedListBox.Items[i] as Setting;
                if (eState == CheckState.Checked)
                    cPrimed.Add(pItem);
                else
                    cAllowed.Add(pItem);
            }

            pCheckedListBox.Items.Clear();
            pCheckedListBox.Items.AddRange(aSettings);

            for (int i = 0; i < pCheckedListBox.Items.Count; i++)
            {
                if (bAllChecked || cPrimed.Contains(pCheckedListBox.Items[i] as Setting))
                    pCheckedListBox.SetItemCheckState(i, CheckState.Checked);
                else if (cAllowed.Contains(pCheckedListBox.Items[i] as Setting))
                    pCheckedListBox.SetItemCheckState(i, CheckState.Indeterminate);
            }
        }

        public void Update()
        {
            checkedListBox1.ItemCheck -= new ItemCheckEventHandler(checkedListBox1_ItemCheck);
            Update(checkedListBox1, m_pRepository.m_cWorlds.ToArray());
            checkedListBox1.ItemCheck += new ItemCheckEventHandler(checkedListBox1_ItemCheck);

            checkedListBox2.ItemCheck -= new ItemCheckEventHandler(checkedListBox2_ItemCheck);
            Update(checkedListBox2, m_pRepository.m_cJanres.ToArray());
            checkedListBox2.ItemCheck += new ItemCheckEventHandler(checkedListBox2_ItemCheck);
        }

        private void SettingSelect_Load(object sender, EventArgs e)
        {

        }

        private void CheckRandom(CheckedListBox pCheckedListBox, int iCount, int iCount2)
        {
            if (pCheckedListBox.Items.Count > 0)
            {
                for (int i = 0; i < pCheckedListBox.Items.Count; i++)
                    pCheckedListBox.SetItemCheckState(i, CheckState.Unchecked);

                if (pCheckedListBox.Items.Count < iCount + iCount2)
                {
                    iCount = pCheckedListBox.Items.Count;
                    if (iCount > 1 && iCount2 > 0)
                    {
                        iCount -= 1;
                        iCount2 = 1;
                    }
                    else
                        iCount2 = 0;
                }

                for (int i = 0; i < iCount; i++)
                {
                    bool bSetOK = false;
                    do
                    {
                        bSetOK = false;
                        int iSetting1 = Rnd.Get(pCheckedListBox.Items.Count);
                        if (!pCheckedListBox.CheckedIndices.Contains(iSetting1))
                        {
                            pCheckedListBox.SetItemCheckState(iSetting1, CheckState.Checked);
                            bSetOK = true;
                        }
                    }
                    while (!bSetOK);
                }
                for (int i = 0; i < iCount; i++)
                {
                    bool bSetOK = false;
                    do
                    {
                        bSetOK = false;
                        int iSetting1 = Rnd.Get(pCheckedListBox.Items.Count);
                        if (!pCheckedListBox.CheckedIndices.Contains(iSetting1))
                        {
                            pCheckedListBox.SetItemCheckState(iSetting1, CheckState.Indeterminate);
                            bSetOK = true;
                        }
                    }
                    while (!bSetOK);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = Rnd.OneChanceFrom(2);

            checkedListBox1.ItemCheck -= new ItemCheckEventHandler(checkedListBox1_ItemCheck);
            CheckRandom(checkedListBox1, 1, 1);
            checkedListBox1.ItemCheck += new ItemCheckEventHandler(checkedListBox1_ItemCheck);

            checkedListBox2.ItemCheck -= new ItemCheckEventHandler(checkedListBox2_ItemCheck);
            CheckRandom(checkedListBox2, 1 + Rnd.Get(2), 1 + Rnd.Get(2));
            checkedListBox2.ItemCheck += new ItemCheckEventHandler(checkedListBox2_ItemCheck);
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            switch (e.CurrentValue)
            {
                case CheckState.Checked:
                    e.NewValue = CheckState.Indeterminate;
                    break;

                case CheckState.Indeterminate:
                    e.NewValue = CheckState.Unchecked;
                    break;

                case CheckState.Unchecked:
                    e.NewValue = CheckState.Checked;
                    break;
            }

            if (e.NewValue == CheckState.Checked)
                button1.Enabled = true;

            if (e.NewValue != CheckState.Checked && checkedListBox1.CheckedItems.Count == 1)
                button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_cPrimedWorlds.Clear();
            m_cAllowedWorlds.Clear();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                CheckState eState = checkedListBox1.GetItemCheckState(i);

                if (eState == CheckState.Unchecked)
                    continue;

                Setting pItem = checkedListBox1.Items[i] as Setting;
                if (eState == CheckState.Checked)
                    m_cPrimedWorlds.Add(pItem);
                else
                    m_cAllowedWorlds.Add(pItem);
            }

            m_cPrimedJanres.Clear();
            m_cAllowedJanres.Clear();
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                CheckState eState = checkedListBox2.GetItemCheckState(i);

                if (eState == CheckState.Unchecked)
                    continue;

                Setting pItem = checkedListBox2.Items[i] as Setting;
                if (eState == CheckState.Checked)
                    m_cPrimedJanres.Add(pItem);
                else
                    m_cAllowedJanres.Add(pItem);
            }
        }

        private void checkedListBox2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            switch (e.CurrentValue)
            {
                case CheckState.Checked:
                    e.NewValue = CheckState.Indeterminate;
                    break;

                case CheckState.Indeterminate:
                    e.NewValue = CheckState.Unchecked;
                    break;

                case CheckState.Unchecked:
                    e.NewValue = CheckState.Checked;
                    break;
            }

            if (e.NewValue == CheckState.Checked)
                button1.Enabled = true;

            if (e.NewValue != CheckState.Checked && checkedListBox2.CheckedItems.Count == 1)
                button1.Enabled = false;
        }

        private void пометитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkedListBox1.ItemCheck -= new ItemCheckEventHandler(checkedListBox1_ItemCheck);

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemCheckState(i, CheckState.Checked);

            checkedListBox1.ItemCheck += new ItemCheckEventHandler(checkedListBox1_ItemCheck);
            button1.Enabled = (checkedListBox1.CheckedItems.Count > 0 && checkedListBox2.CheckedItems.Count > 0);
        }

        private void снятьПометкиСоВсехToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkedListBox1.ItemCheck -= new ItemCheckEventHandler(checkedListBox1_ItemCheck);

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);

            checkedListBox1.ItemCheck += new ItemCheckEventHandler(checkedListBox1_ItemCheck);
            button1.Enabled = false;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            checkedListBox2.ItemCheck -= new ItemCheckEventHandler(checkedListBox2_ItemCheck);

            for (int i = 0; i < checkedListBox2.Items.Count; i++)
                checkedListBox2.SetItemCheckState(i, CheckState.Checked);

            checkedListBox2.ItemCheck += new ItemCheckEventHandler(checkedListBox2_ItemCheck);
            button1.Enabled = (checkedListBox1.CheckedItems.Count > 0 && checkedListBox2.CheckedItems.Count > 0);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            checkedListBox2.ItemCheck -= new ItemCheckEventHandler(checkedListBox2_ItemCheck);

            for (int i = 0; i < checkedListBox2.Items.Count; i++)
                checkedListBox2.SetItemCheckState(i, CheckState.Unchecked);

            checkedListBox2.ItemCheck += new ItemCheckEventHandler(checkedListBox2_ItemCheck);
            button1.Enabled = false;
        }
    }
}
