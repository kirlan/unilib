using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Random;
using Microsoft.Win32;

namespace RandomStory
{
    public partial class Form1 : Form
    {
        Repository m_pRepository = new Repository();

        public Form1()
        {
            InitializeComponent();

            panel2.Dock = DockStyle.Fill;
        }

        private SettingSelect m_pSelectSetting;

        private void Form1_Load(object sender, EventArgs e)
        {
            m_pRepository.LoadXML();

            m_pSelectSetting = new SettingSelect(m_pRepository);

            RegistryKey key = null;
            try
            {
                key = Application.CommonAppDataRegistry;//Registry.LocalMachine.OpenSubKey("SOFTWARE\\RandomStoryKvB", true);

                эспрессрежимToolStripMenuItem.Checked = (int)key.GetValue("Express", 0) > 0;

                key.Close();
            }
            catch (Exception ex)
            {
                if (key != null)
                    key.Close();
            }
            
            ClearUI();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.ApplicationExitCall)
                m_pRepository.SaveXML();

            RegistryKey key = null;
            try
            {
                key = Application.CommonAppDataRegistry;//Registry.LocalMachine.OpenSubKey("SOFTWARE\\RandomStoryKvB", true);

                key.SetValue("Express", эспрессрежимToolStripMenuItem.Checked ? 1:0);

                key.Close();
            }
            catch (Exception ex)
            {
                if (key != null)
                    key.Close();
            }
        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void мирыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorldsEdit pForm = new WorldsEdit(m_pRepository);
            pForm.ShowDialog();
            m_pRepository.SaveXML();

            m_pSelectSetting.Update();
        }

        private Story m_pStory = null;

        private void проблемыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProblemsEdit pForm = new ProblemsEdit(m_pRepository);
            pForm.ShowDialog();
            m_pRepository.SaveXML();
        }

        private void считатьБазуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                m_pRepository.LoadXML(openFileDialog1.FileName);
        }

        private void сохранитьБазуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                m_pRepository.SaveXML(saveFileDialog1.FileName);
        }

        private void отношенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CharactersEdit pForm = new CharactersEdit(m_pRepository);
            pForm.ShowDialog();
            m_pRepository.SaveXML();
        }

        private void тестToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void glassButton1_Click(object sender, EventArgs e)
        {

        }

        private void ClearUI()
        {
            glassPanelEvents.Visible = false;
            glassPanelHelper.Visible = false;
            glassPanelHero.Visible = false;
            glassPanelItems.Visible = false;
            glassPanelMinion.Visible = false;
            glassPanelPlaces.Visible = false;
            glassPanelProblem.Visible = false;
            glassPanelSolution.Visible = false;
            glassPanelTutor.Visible = false;
            glassPanelVillain.Visible = false;

            glassPanelEvents.Enabled = true;
            glassPanelHelper.Enabled = true;
            glassPanelHero.Enabled = true;
            glassPanelItems.Enabled = true;
            glassPanelMinion.Enabled = true;
            glassPanelPlaces.Enabled = true;
            glassPanelProblem.Enabled = true;
            glassPanelSolution.Enabled = true;
            glassPanelTutor.Enabled = true;
            glassPanelVillain.Enabled = true;

            glassButtonEvents.Text = "?";
            glassButtonHelper.Text = "?";
            glassButtonHero.Text = "?";
            glassButtonItems.Text = "?";
            glassButtonMinion.Text = "?";
            glassButtonPlaces.Text = "?";
            glassButtonProblem.Text = "?";
            glassButtonSetting.Text = "?";
            glassButtonSolution.Text = "?";
            glassButtonTutor.Text = "?";
            glassButtonVillain.Text = "?";

            exportButton.Enabled = false;

            UpdateCompletitionPercent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (m_pSelectSetting.ShowDialog() == DialogResult.OK)
            {
                ClearUI();

                m_pRepository.MarkPossibleSettings(m_pSelectSetting.m_cAllowedWorlds, m_pSelectSetting.m_cPrimedWorlds, m_pSelectSetting.m_cAllowedJanres, m_pSelectSetting.m_cPrimedJanres);
                m_pStory = new Story(m_pRepository, m_pSelectSetting.Voyagers);

                if (интерактивныйРежимToolStripMenuItem.Checked)
                {
                    glassButtonSetting.Text = m_pStory.m_pSetting.ToString();
                    glassPanelHero.Visible = true;
                }
                else
                {
                    m_pStory.BuildFullStory();

                    glassPanelEvents.Visible = true;
                    glassPanelHelper.Visible = true;
                    glassPanelHero.Visible = true;
                    glassPanelItems.Visible = true;
                    glassPanelMinion.Visible = true;
                    glassPanelPlaces.Visible = true;
                    glassPanelProblem.Visible = true;
                    glassPanelSolution.Visible = true;
                    glassPanelTutor.Visible = true;
                    glassPanelVillain.Visible = true; 
                    
                    glassButtonSetting.Text = m_pStory.m_pSetting.ToString();
                    glassButtonHero.Text = m_pStory.m_pHero.ToString();

                    if (m_pStory.m_pTutor != null)
                        glassButtonTutor.Text = m_pStory.m_pTutor.ToString();

                    if (m_pStory.m_pHelper != null)
                        glassButtonHelper.Text = m_pStory.m_pHelper.ToString();

                    glassButtonProblem.Text = m_pStory.m_sProblem;
                    glassButtonVillain.Text = m_pStory.m_pVillain.ToString();

                    if (m_pStory.m_pMinion != null)
                        glassButtonMinion.Text = m_pStory.m_pMinion.ToString();

                    glassButtonSolution.Text = m_pStory.m_sSolution;
                    glassButtonItems.Text = m_pStory.m_cKeyItems.ToString("\n");
                    glassButtonPlaces.Text = m_pStory.m_cLocations.ToString("\n");
                    glassButtonEvents.Text = m_pStory.m_cEvents.ToString("\n");
                }

                UpdateCompletitionPercent();
            }
        }

        private void glassButtonSetting_Click(object sender, EventArgs e)
        {
        }

        private void UpdateCompletitionPercent()
        {
            int iTotal = 7;
            int iComplete = 0;

            if (m_pStory != null)
            {
                if (m_pStory.m_pHero != null)
                    iComplete++;
                if (m_pStory.m_pVillain != null)
                    iComplete++;
                if (!string.IsNullOrWhiteSpace(m_pStory.m_sProblem))
                    iComplete++;
                if (!string.IsNullOrWhiteSpace(m_pStory.m_sSolution))
                    iComplete++;
                if (m_pStory.m_cKeyItems.Count > 0)
                    iComplete++;
                if (m_pStory.m_cLocations.Count > 0)
                    iComplete++;
                if (m_pStory.m_cEvents.Count > 0)
                    iComplete++;
                if (m_pStory.m_pTutor != null)
                {
                    iTotal++;
                    iComplete++;
                }
                if (m_pStory.m_pHelper != null)
                {
                    iTotal++;
                    iComplete++;
                }
                if (m_pStory.m_pMinion != null)
                {
                    iTotal++;
                    iComplete++;
                }
            }

            label1.Text = string.Format("{0:P0}", (double)iComplete/iTotal);

            Font pFont = label1.Font;
            label1.Font = new Font(pFont, iTotal == iComplete ? FontStyle.Bold : FontStyle.Regular);

            exportButton.Enabled = (iTotal == iComplete);
        }

        private void glassButtonHero_Click(object sender, EventArgs e)
        {
            Randomizer pForm = new Randomizer("Герой:", m_pStory.GetRandomHero, m_pStory.m_pHero);
            if (pForm.ShowDialog() == DialogResult.OK)
            {
                m_pStory.SetHero(pForm.SelectedItem as Character);
                glassButtonHero.Text = m_pStory.m_pHero.ToString();
                glassPanelVillain.Visible = true;
                glassPanelHelper.Visible = true;
                glassPanelTutor.Visible = true;
                glassButtonSetting.Enabled = false;
                UpdateCompletitionPercent();
            }
        }

        private void glassButtonVillain_Click(object sender, EventArgs e)
        {
            Randomizer pForm = new Randomizer("Антагонист:", m_pStory.GetRandomVillain, m_pStory.m_pVillain);
            if (pForm.ShowDialog() == DialogResult.OK)
            {
                m_pStory.SetVillain(pForm.SelectedItem as Character);
                glassButtonVillain.Text = m_pStory.m_pVillain.ToString();
                glassPanelProblem.Visible = true;
                glassPanelMinion.Visible = true;
                //if (m_pStory.m_pVillain.m_pRelative != null && m_pStory.m_pVillain.m_pRelative == m_pStory.m_pHero)
                //    glassButtonHero.Enabled = false;
                //if (m_pStory.m_pVillain.m_pRelative != null && m_pStory.m_pVillain.m_pRelative == m_pStory.m_pTutor)
                //    glassButtonTutor.Enabled = false;
                UpdateCompletitionPercent();
            }
        }

        private void glassButtonTutor_Click(object sender, EventArgs e)
        {
            Randomizer pForm = new Randomizer("Наставник/покровитель героя:", m_pStory.GetRandomTutor, m_pStory.m_pTutor);
            if (pForm.ShowDialog() == DialogResult.OK)
            {
                m_pStory.SetTutor(pForm.SelectedItem as Character);
                glassButtonTutor.Text = m_pStory.m_pTutor.ToString();
                //if (m_pStory.m_pTutor.m_pRelative != null && m_pStory.m_pTutor.m_pRelative == m_pStory.m_pVillain)
                //    glassButtonVillain.Enabled = false;
                //if (m_pStory.m_pTutor.m_pRelative != null && m_pStory.m_pTutor.m_pRelative == m_pStory.m_pHero)
                //    glassButtonHero.Enabled = false;
                UpdateCompletitionPercent();
            }
        }

        private void glassButtonHelper_Click(object sender, EventArgs e)
        {
           Randomizer pForm = new Randomizer("Спутник героя:", m_pStory.GetRandomHelper, m_pStory.m_pHelper);
            if (pForm.ShowDialog() == DialogResult.OK)
            {
                m_pStory.SetHelper(pForm.SelectedItem as Character);
                glassButtonHelper.Text = m_pStory.m_pHelper.ToString();
                //if (m_pStory.m_pHelper.m_pRelative != null && m_pStory.m_pHelper.m_pRelative == m_pStory.m_pVillain)
                //    glassButtonVillain.Enabled = false;
                //if (m_pStory.m_pHelper.m_pRelative != null && m_pStory.m_pHelper.m_pRelative == m_pStory.m_pHero)
                //    glassButtonHero.Enabled = false;
                //if (m_pStory.m_pHelper.m_pRelative != null && m_pStory.m_pHelper.m_pRelative == m_pStory.m_pTutor)
                //    glassButtonTutor.Enabled = false;
                UpdateCompletitionPercent();
            }
        }

        private void glassButtonMinion_Click(object sender, EventArgs e)
        {
            Randomizer pForm = new Randomizer("Помощник антагониста:", m_pStory.GetRandomMinion, m_pStory.m_pMinion);
            if (pForm.ShowDialog() == DialogResult.OK)
            {
                m_pStory.SetMinion(pForm.SelectedItem as Character);
                glassButtonMinion.Text = m_pStory.m_pMinion.ToString();
                //if (m_pStory.m_pMinion.m_pRelative != null && m_pStory.m_pMinion.m_pRelative == m_pStory.m_pVillain)
                //    glassButtonVillain.Enabled = false;
                //if (m_pStory.m_pMinion.m_pRelative != null && m_pStory.m_pMinion.m_pRelative == m_pStory.m_pHero)
                //    glassButtonHero.Enabled = false;
                //if (m_pStory.m_pMinion.m_pRelative != null && m_pStory.m_pMinion.m_pRelative == m_pStory.m_pTutor)
                //    glassButtonTutor.Enabled = false;
                UpdateCompletitionPercent();
            }
        }

        private void glassButtonProblem_Click(object sender, EventArgs e)
        {
            Randomizer pForm = new Randomizer("Проблема:", m_pStory.GetRandomProblem, m_pStory.m_sProblem);
            if (pForm.ShowDialog() == DialogResult.OK)
            {
                m_pStory.SetProblem(pForm.SelectedItem as string);
                glassButtonProblem.Text = m_pStory.m_sProblem;
                glassPanelSolution.Visible = true;
                UpdateCompletitionPercent();
            }
        }

        private void glassButtonSolution_Click(object sender, EventArgs e)
        {
            Randomizer pForm = new Randomizer("Решение:", m_pStory.GetRandomSolution, m_pStory.m_sSolution);
            if (pForm.ShowDialog() == DialogResult.OK)
            {
                m_pStory.SetSolution(pForm.SelectedItem as string);
                glassButtonSolution.Text = m_pStory.m_sSolution;
                glassPanelItems.Visible = true;
                UpdateCompletitionPercent();
            }
        }

        private void glassButtonItems_Click(object sender, EventArgs e)
        {
            Randomizer pForm = new Randomizer("Ключевые предметы:", m_pStory.GetRandomItems, m_pStory.m_cKeyItems);
            if (pForm.ShowDialog() == DialogResult.OK)
            {
                m_pStory.SetItems(pForm.SelectedItem as Strings);
                glassButtonItems.Text = m_pStory.m_cKeyItems.ToString("\n");
                glassPanelPlaces.Visible = true;
                glassPanelEvents.Visible = true;
                UpdateCompletitionPercent();
            }
        }

        private void glassButtonPlaces_Click(object sender, EventArgs e)
        {
            Randomizer pForm = new Randomizer("Основные локации:", m_pStory.GetRandomPlaces, m_pStory.m_cLocations);
            if (pForm.ShowDialog() == DialogResult.OK)
            {
                m_pStory.SetPlaces(pForm.SelectedItem as Strings);
                glassButtonPlaces.Text = m_pStory.m_cLocations.ToString("\n");
                UpdateCompletitionPercent();
            }
        }

        private void glassButtonEvents_Click(object sender, EventArgs e)
        {
            Randomizer pForm = new Randomizer("Ключевые сцены:", m_pStory.GetRandomEvents, m_pStory.m_cEvents);
            if (pForm.ShowDialog() == DialogResult.OK)
            {
                m_pStory.SetEvents(pForm.SelectedItem as Strings);
                glassButtonEvents.Text = m_pStory.m_cEvents.ToString("\n");
                UpdateCompletitionPercent();
            }
        }

        private void эспрессрежимToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            интерактивныйРежимToolStripMenuItem.Checked = !эспрессрежимToolStripMenuItem.Checked;
            label1.Visible = интерактивныйРежимToolStripMenuItem.Checked;
        }

        private void интерактивныйРежимToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            эспрессрежимToolStripMenuItem.Checked = !интерактивныйРежимToolStripMenuItem.Checked;
            label1.Visible = интерактивныйРежимToolStripMenuItem.Checked;
        }

        private void экспортироватьИсториюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportForm pForm = new ExportForm(m_pStory);

            pForm.ShowDialog();
        }
    }
}
