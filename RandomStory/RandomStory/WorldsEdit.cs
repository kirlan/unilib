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
    public partial class WorldsEdit : Form
    {
        private Repository m_pRepository;

        public WorldsEdit(Repository pRep)
        {
            InitializeComponent();

            m_pRepository = pRep;

            worldsListBox.Items.Clear();
            worldsListBox.Items.Add(m_pRepository.m_pCommon);
            worldsListBox.Items.AddRange(m_pRepository.m_cWorlds.ToArray());
        }

        private void добавитьМирToolStripMenuItem_Click(object sender, EventArgs e)
        {
            World pNewWorld = new World();
            pNewWorld.m_pCommon = m_pRepository.m_pCommon;

            m_pRepository.m_cWorlds.Add(pNewWorld);
            worldsListBox.SelectedIndex = worldsListBox.Items.Add(pNewWorld);
        }

        private void удалитьМирToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (worldsListBox.SelectedIndex == -1)
                return;

            if (worldsListBox.SelectedIndex == 0)
            {
                MessageBox.Show(string.Format("Невозможно удалить мир '{0}'", worldsListBox.SelectedItem), "Удалить мир");
                return;
            }

            if (MessageBox.Show(string.Format("Действительно хотите удалить мир '{0}'", worldsListBox.SelectedItem), "Удалить мир", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            m_pRepository.m_cWorlds.Remove(worldsListBox.SelectedItem as World);
            worldsListBox.Items.Remove(worldsListBox.SelectedItem);
        }

        private void worldsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (worldsListBox.SelectedIndex == -1)
                return;

            World pSelectedWorld = worldsListBox.SelectedItem as World;

            if (pSelectedWorld == null)
                return;

            string sLog = m_pRepository.FixCommon();
            if (!string.IsNullOrWhiteSpace(sLog))
                MessageBox.Show(sLog);

            nameTextBox.Text = pSelectedWorld.m_sName;
            racesTextBox.Lines = pSelectedWorld.m_cRaces.ToArray();
            perksTextBox.Lines = pSelectedWorld.m_cPerks.ToArray();
            professionsTextBox.Lines = pSelectedWorld.m_cProfessions.ToArray();
            professionsEvilTextBox.Lines = pSelectedWorld.m_cProfessionsEvil.ToArray();
            locationsTextBox.Lines = pSelectedWorld.m_cLocations.ToArray();
            itemsTextBox.Lines = pSelectedWorld.m_cItems.ToArray();

            nameTextBox.Enabled = (pSelectedWorld.m_pCommon != null);
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        { 
            if (worldsListBox.SelectedIndex == -1)
                return;

            World pSelectedWorld = worldsListBox.SelectedItem as World;

            if (pSelectedWorld == null)
                return;

            pSelectedWorld.m_sName = nameTextBox.Text;
            worldsListBox.DisplayMember = "";
            worldsListBox.DisplayMember = "Name";
        }

        private void racesTextBox_TextChanged(object sender, EventArgs e)
        {
            if (worldsListBox.SelectedIndex == -1)
                return;

            World pSelectedWorld = worldsListBox.SelectedItem as World;

            if (pSelectedWorld == null)
                return;

            pSelectedWorld.m_cRaces.Clear();
            pSelectedWorld.m_cRaces.AddRange(racesTextBox.Lines);
        }

        private void perksTextBox_TextChanged(object sender, EventArgs e)
        {
            if (worldsListBox.SelectedIndex == -1)
                return;

            World pSelectedWorld = worldsListBox.SelectedItem as World;

            if (pSelectedWorld == null)
                return;

            pSelectedWorld.m_cPerks.Clear();
            pSelectedWorld.m_cPerks.AddRange(perksTextBox.Lines);
        }

        private void professionsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (worldsListBox.SelectedIndex == -1)
                return;

            World pSelectedWorld = worldsListBox.SelectedItem as World;

            if (pSelectedWorld == null)
                return;

            pSelectedWorld.m_cProfessions.Clear();
            pSelectedWorld.m_cProfessions.AddRange(professionsTextBox.Lines);
        }

        private void locationsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (worldsListBox.SelectedIndex == -1)
                return;

            World pSelectedWorld = worldsListBox.SelectedItem as World;

            if (pSelectedWorld == null)
                return;

            pSelectedWorld.m_cLocations.Clear();
            pSelectedWorld.m_cLocations.AddRange(locationsTextBox.Lines);
        }

        private void professionsEvilTextBox_TextChanged(object sender, EventArgs e)
        {
            if (worldsListBox.SelectedIndex == -1)
                return;

            World pSelectedWorld = worldsListBox.SelectedItem as World;

            if (pSelectedWorld == null)
                return;

            pSelectedWorld.m_cProfessionsEvil.Clear();
            pSelectedWorld.m_cProfessionsEvil.AddRange(professionsEvilTextBox.Lines);
        }

        private void itemsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (worldsListBox.SelectedIndex == -1)
                return;

            World pSelectedWorld = worldsListBox.SelectedItem as World;

            if (pSelectedWorld == null)
                return;

            pSelectedWorld.m_cItems.Clear();
            pSelectedWorld.m_cItems.AddRange(itemsTextBox.Lines);
        }

        private void WorldsEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            string sLog = m_pRepository.FixCommon();
            if (!string.IsNullOrWhiteSpace(sLog))
                MessageBox.Show(sLog);
        }
    }
}
