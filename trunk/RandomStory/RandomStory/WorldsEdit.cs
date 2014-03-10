using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            worldsListBox.Items.AddRange(m_pRepository.m_cWorlds.ToArray());

            janresListBox.Items.Clear();
            janresListBox.Items.Add(m_pRepository.m_pCommon);
            janresListBox.Items.AddRange(m_pRepository.m_cJanres.ToArray());
        }

        private void добавитьМирToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting pNewWorld = new Setting();
            pNewWorld.m_pCommon = m_pRepository.m_pCommon;

            m_pRepository.m_cWorlds.Add(pNewWorld);
            worldsListBox.SelectedIndex = worldsListBox.Items.Add(pNewWorld);
        }

        private void удалитьМирToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (worldsListBox.SelectedIndex == -1)
                return;

            if (worldsListBox.SelectedItem == m_pRepository.m_pCommon)
            {
                MessageBox.Show(string.Format("Невозможно удалить мир '{0}'", worldsListBox.SelectedItem), "Удалить мир");
                return;
            }

            if (MessageBox.Show(string.Format("Действительно хотите удалить мир '{0}'?", worldsListBox.SelectedItem), "Удалить мир", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            m_pRepository.m_cWorlds.Remove(worldsListBox.SelectedItem as Setting);
            worldsListBox.Items.Remove(worldsListBox.SelectedItem);
        }

        private Setting pSelectedSetting = null;

        private void SelectSetting(Setting pSetting)
        {
            pSelectedSetting = pSetting;

            if (pSelectedSetting == null)
                return;

            string sLog = m_pRepository.FixCommon();
            if (!string.IsNullOrWhiteSpace(sLog))
                MessageBox.Show(sLog);

            nameTextBox.Text = pSelectedSetting.m_sName;
            racesTextBox.Lines = pSelectedSetting.m_cRaces.ToArray();
            perksTextBox.Lines = pSelectedSetting.m_cPerks.ToArray();
            professionsTextBox.Lines = pSelectedSetting.m_cProfessions.ToArray();
            professionsEvilTextBox.Lines = pSelectedSetting.m_cProfessionsElite.ToArray();
            locationsTextBox.Lines = pSelectedSetting.m_cLocations.ToArray();
            itemsTextBox.Lines = pSelectedSetting.m_cItems.ToArray();

            nameTextBox.Enabled = (pSelectedSetting.m_pCommon != null);
        }

        private void worldsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (worldsListBox.SelectedIndex != -1)
            {
                janresListBox.SelectedIndex = -1;
                SelectSetting(worldsListBox.SelectedItem as Setting);
            }
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        { 
            if (pSelectedSetting == null)
                return;

            pSelectedSetting.m_sName = nameTextBox.Text;
            if (worldsListBox.SelectedIndex != -1)
            {
                worldsListBox.DisplayMember = "";
                worldsListBox.DisplayMember = "Name";
            }
            else
            {
                janresListBox.DisplayMember = "";
                janresListBox.DisplayMember = "Name";
            }
        }

        private void racesTextBox_TextChanged(object sender, EventArgs e)
        {
            if (pSelectedSetting == null)
                return;

            pSelectedSetting.m_cRaces = new Strings(racesTextBox.Lines);
        }

        private void perksTextBox_TextChanged(object sender, EventArgs e)
        {
            if (pSelectedSetting == null)
                return;

            pSelectedSetting.m_cPerks = new Strings(perksTextBox.Lines);
        }

        private void professionsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (pSelectedSetting == null)
                return;

            pSelectedSetting.m_cProfessions = new Strings(professionsTextBox.Lines);
        }

        private void locationsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (pSelectedSetting == null)
                return;

            pSelectedSetting.m_cLocations = new Strings(locationsTextBox.Lines);
        }

        private void professionsEvilTextBox_TextChanged(object sender, EventArgs e)
        {
            if (pSelectedSetting == null)
                return;

            pSelectedSetting.m_cProfessionsElite = new Strings(professionsEvilTextBox.Lines);
        }

        private void itemsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (pSelectedSetting == null)
                return;

            pSelectedSetting.m_cItems = new Strings(itemsTextBox.Lines);
        }

        private void WorldsEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            string sLog = m_pRepository.FixCommon();
            if (!string.IsNullOrWhiteSpace(sLog))
                MessageBox.Show(sLog);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Setting pNewWorld = new Setting();
            pNewWorld.m_pCommon = m_pRepository.m_pCommon;

            m_pRepository.m_cJanres.Add(pNewWorld);
            janresListBox.SelectedIndex = janresListBox.Items.Add(pNewWorld);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (janresListBox.SelectedIndex == -1)
                return;

            if (janresListBox.SelectedItem == m_pRepository.m_pCommon)
            {
                MessageBox.Show(string.Format("Невозможно удалить жанр '{0}'", janresListBox.SelectedItem), "Удалить жанр");
                return;
            }

            if (MessageBox.Show(string.Format("Действительно хотите удалить жанр '{0}'?", janresListBox.SelectedItem), "Удалить жанр", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            m_pRepository.m_cJanres.Remove(janresListBox.SelectedItem as Setting);
            janresListBox.Items.Remove(janresListBox.SelectedItem);
        }

        private void janresListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (janresListBox.SelectedIndex != -1)
            {
                worldsListBox.SelectedIndex = -1;
                SelectSetting(janresListBox.SelectedItem as Setting);
            }
        }

        private void перенестиВЖанрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (worldsListBox.SelectedIndex == -1)
                return;

            if (worldsListBox.SelectedItem == m_pRepository.m_pCommon)
            {
                MessageBox.Show(string.Format("Невозможно удалить мир '{0}'", worldsListBox.SelectedItem), "Перенести в Жанры");
                return;
            }

            Setting pNewJanre = worldsListBox.SelectedItem as Setting;

            m_pRepository.m_cJanres.Add(pNewJanre);
            janresListBox.SelectedIndex = janresListBox.Items.Add(pNewJanre);

            m_pRepository.m_cWorlds.Remove(pNewJanre as Setting);
            worldsListBox.Items.Remove(pNewJanre);
        }

        private void перенестиВМирыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (janresListBox.SelectedIndex == -1)
                return;

            if (janresListBox.SelectedItem == m_pRepository.m_pCommon)
            {
                MessageBox.Show(string.Format("Невозможно удалить жанр '{0}'", worldsListBox.SelectedItem), "Перенести в Миры");
                return;
            }

            Setting pNewWorld = janresListBox.SelectedItem as Setting;

            m_pRepository.m_cWorlds.Add(pNewWorld);
            worldsListBox.SelectedIndex = worldsListBox.Items.Add(pNewWorld);

            m_pRepository.m_cJanres.Remove(pNewWorld);
            janresListBox.Items.Remove(pNewWorld);
        }
    }
}
