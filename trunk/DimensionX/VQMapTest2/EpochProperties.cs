using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Socium;

namespace VQMapTest2
{
    public partial class EpochProperties : UserControl
    {
        static SocietyPreset[] m_aSocietyPresets = new SocietyPreset[] 
        { 
            new SocietyPreset("Historical - antiquity", "An antique world without magic - like Ancient Greece, Rome, Egypt, Assyria, etc.", 0, 1, 0, 0),
            new SocietyPreset("Historical - medieval", "A medieval world without magic - castles, knights, tournaments, etc.", 0, 2, 0, 0),
            new SocietyPreset("Historical - renaissance", "A renaissance world without magic - musketeers, geographic exploration, etc.", 1, 3, 0, 0),
            new SocietyPreset("Historical - modern", "A modern world without magic - railroads, aviation, world wars, etc.", 4, 5, 0, 0),
            new SocietyPreset("Antique mythology", "A world of antique mythology - just a usual antique world, but with a bit of magic...", 0, 1, 1, 2),
            new SocietyPreset("Fantasy - low magic", "A medieval world with a bit of magic - like Knights of the Round Table, Lord of the Rings, etc.", 1, 2, 1, 3),
            new SocietyPreset("Fantasy - high magic", "A medieval world with a lot of magic - like Dragonlance, Wheel of Time, etc.", 1, 2, 2, 4),
            new SocietyPreset("Technomagic", "A renaissance world with a lot of magic - like Arcanum, Final Fantasy, etc.", 1, 3, 2, 4),
            new SocietyPreset("Superheroes", "A modern world with a bit of magic (aka supernatural abilities) - like Superman, Fantastic Four, Spiderman, etc.", 4, 5, 1, 3),
            //new SocietyPreset("Urban fantasy", "A modern world with a lot of magic - vampires, werewolfs, voodoo, secret societies, etc.", 4, 5, 2, 4),
            new SocietyPreset("Cyberpunk", "Nearest future world without magic - advanced technologies, mega-corporations, industrial espionage, etc.", 4, 6, 0, 0),
            new SocietyPreset("Space opera", "Far future world with a bit of magic (aka psi-abilities) - like Star Wars, Star Trek, etc.", 6, 7, 1, 3),
        }; 
        
        private EpochWrapper m_pEpoch = null;

        public EpochWrapper Epoch
        {
            set 
            { 
                m_pEpoch = value;

                textBox2.Text = m_pEpoch.Name;
                numericUpDown1.Value = m_pEpoch.Length;

                AdvancedPanel_VisibleChanged(this, new EventArgs());
            }
        }

        private bool m_bAdvancedMode = false;

        public bool AdvancedMode
        {
            get { return m_bAdvancedMode; }
            set
            {
                m_bAdvancedMode = value;
                PresetsPanel.Visible = !m_bAdvancedMode;
                AdvancedPanel.Visible = m_bAdvancedMode;
            }
        }
        
        public EpochProperties()
        {
            InitializeComponent();

            PresetsPanel.Enabled = false;
            PresetsPanel.Top = 0;
            PresetsPanel.Left = 0;
            PresetsPanel.Width = ClientRectangle.Width;
            PresetsPanel.Height = ClientRectangle.Height;
            PresetsPanel.Visible = true;

            AdvancedPanel.Enabled = false;
            AdvancedPanel.Top = 0;
            AdvancedPanel.Left = 0;
            AdvancedPanel.Width = ClientRectangle.Width;
            AdvancedPanel.Height = ClientRectangle.Height;
            AdvancedPanel.Visible = false;

            checkedListBox1.Items.Clear();
            foreach (RaceTemplate pTemplate in Race.m_cTemplates)
                checkedListBox1.Items.Add(pTemplate);

            for (int i = 0; i < 9; i++)
            {
                comboBox2.Items.Add(string.Format("{0} [T{1}]", State.GetTechString(i), i));
                comboBox3.Items.Add(string.Format("{0} [T{1}]", State.GetTechString(i), i));

                comboBox4.Items.Add(string.Format("{0} [M{1}]", State.GetMagicString(i), i));
                comboBox5.Items.Add(string.Format("{0} [M{1}]", State.GetMagicString(i), i));
            }

            comboBox2.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;

            comboBox3.SelectedIndex = 8;
            comboBox5.SelectedIndex = 8;

            NativesSocialPreset.Items.Clear();
            NativesSocialPreset.Items.AddRange(m_aSocietyPresets);
            if (NativesSocialPreset.Items.Count > 0)
                NativesSocialPreset.SelectedIndex = 0;

            InvadersSocialPreset.Items.Clear();
            InvadersSocialPreset.Items.AddRange(m_aSocietyPresets);
            if (InvadersSocialPreset.Items.Count > 0)
                InvadersSocialPreset.SelectedIndex = 0;

            NativesRacesSet.Items.Clear();
            InvadersRacesSet.Items.Clear();
            foreach (RacesSet pSet in RacesSet.s_aSets)
            {
                NativesRacesSet.Items.Add(pSet);
                InvadersRacesSet.Items.Add(pSet);

                NativesRacesSet.CheckBoxItems[pSet.ToString()].Checked = true;
                InvadersRacesSet.CheckBoxItems[pSet.ToString()].Checked = true;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void EpochProperties_Resize(object sender, EventArgs e)
        {
            PresetsPanel.Width = ClientRectangle.Width;
            PresetsPanel.Height = ClientRectangle.Height;

            AdvancedPanel.Width = ClientRectangle.Width;
            AdvancedPanel.Height = ClientRectangle.Height;
        }

        private void AdvancedPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (AdvancedPanel.Visible)
                tabControl1_SelectedIndexChanged(sender, e);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (RaceTemplate pTemplate in checkedListBox1.Items)
                if (tabControl1.SelectedIndex == 0)
                    checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(pTemplate), m_pEpoch.NativesRaces.Contains(pTemplate));
                else
                    checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(pTemplate), m_pEpoch.InvadersRaces.Contains(pTemplate));
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            m_pEpoch.Name = textBox2.Text;
            if (textBox3.Text != textBox2.Text)
                textBox3.Text = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            m_pEpoch.Name = textBox3.Text;
            if (textBox2.Text != textBox3.Text)
                textBox2.Text = textBox3.Text;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            m_pEpoch.Length = (int)numericUpDown1.Value;
            if (numericUpDown2.Value != numericUpDown1.Value)
                numericUpDown2.Value = numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            m_pEpoch.Length = (int)numericUpDown2.Value;
            if (numericUpDown1.Value != numericUpDown2.Value)
                numericUpDown1.Value = numericUpDown2.Value;
        }

        private int m_iTechLevelLimit = 8;

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            for (int i = comboBox2.SelectedIndex; i < 9; i++)
                comboBox3.Items.Add(string.Format("{0} [T{1}]", State.GetTechString(i), i));

            comboBox3.SelectedIndex = Math.Max(0, m_iTechLevelLimit - comboBox2.SelectedIndex);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex != -1)
                m_iTechLevelLimit = comboBox2.SelectedIndex + comboBox3.SelectedIndex;
        }

        private int m_iMagicLimit = 8;

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox5.Items.Clear();
            for (int i = comboBox4.SelectedIndex; i < 9; i++)
                comboBox5.Items.Add(string.Format("{0} [M{1}]", State.GetMagicString(i), i));

            comboBox5.SelectedIndex = Math.Max(0, m_iMagicLimit - comboBox4.SelectedIndex);
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex != -1)
                m_iMagicLimit = comboBox4.SelectedIndex + comboBox5.SelectedIndex;
        }

        private void NativesSocialPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NativesSocialPreset.SelectedIndex != -1)
            {
                SocietyPreset pPreset = NativesSocialPreset.SelectedItem as SocietyPreset;
                NativesPresetDescription.Text = pPreset.m_sDescription;
            }
        }

        private void InvadersSocialPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InvadersSocialPreset.SelectedIndex != -1)
            {
                SocietyPreset pPreset = InvadersSocialPreset.SelectedItem as SocietyPreset;
                InvadersPresetDescription.Text = pPreset.m_sDescription;

                //comboBox2.SelectedIndex = pPreset.m_iMinTechLevel;
                //comboBox3.SelectedIndex = pPreset.m_iMaxTechLevel - pPreset.m_iMinTechLevel;

                //comboBox4.SelectedIndex = pPreset.m_iMinMagicLevel;
                //comboBox5.SelectedIndex = pPreset.m_iMaxMagicLevel - pPreset.m_iMinMagicLevel;
            }
        }

    }
}
