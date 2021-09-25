using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Persona.Parameters;

namespace Persona
{
    public partial class TestPlay : Form
    {
        private Module m_pModule;

        public TestPlay(Module pModule)
        {
            InitializeComponent();

            m_pModule = pModule;

            PlayfieldPanel_Resize(this, new EventArgs());

            GrandActionsPanel.Visible = true;
            EventPanel.Visible = false;
            ReactionPanel.Visible = false;

            m_pModule.Start();

            ActionsTextBox.Text = m_pModule.m_sHeader;

            List<Action> cAvailable = m_pModule.GetPossibleActions();

            ActionsPanel.Controls.Clear();

            foreach (Action pAction in cAvailable)
            {
                Button pButt = new Button();
                pButt.Text = WordWrap(pAction.m_sName, 50);
                pButt.AutoSize = true;
                pButt.Margin = new Padding(5);
                pButt.Tag = pAction;

                pButt.Click += new EventHandler(Action_Click);

                ActionsPanel.Controls.Add(pButt);
            }

            ShowAllParams();
        }

        private Event m_pCurrentEvent = null;

        void ShowAllParams()
        {
            if (ParamsListView.Items.Count == 0)
            {
                List<Parameter> cAllParams = new List<Parameter>();
                cAllParams.AddRange(m_pModule.m_cBoolParameters);
                cAllParams.AddRange(m_pModule.m_cNumericParameters);
                cAllParams.AddRange(m_pModule.m_cStringParameters);

                Dictionary<string, List<Parameter>> cGroups = new Dictionary<string, List<Parameter>>();
                foreach (Parameter pParam in cAllParams)
                {
                    if (pParam.m_bHidden)
                        continue;

                    if (!cGroups.ContainsKey(pParam.m_sGroup))
                        cGroups[pParam.m_sGroup] = new List<Parameter>();
                    cGroups[pParam.m_sGroup].Add(pParam);
                }

                foreach (var pGroup in cGroups)
                {
                    ListViewItem pItemGroup = ParamsListView.Items.Add(pGroup.Key);
                    pItemGroup.Font = new Font(ParamsListView.Font, FontStyle.Bold);

                    foreach (Parameter pParam in pGroup.Value)
                    {
                        ListViewItem pItemParameter = ParamsListView.Items.Add(pParam.m_sName);
                        pItemParameter.SubItems.Add(pParam.DisplayValue);
                        pItemParameter.Tag = pParam;
                    }
                }
            }
            else
            {
                foreach (ListViewItem pItem in ParamsListView.Items)
                {
                    if (pItem.Tag != null)
                    {
                        Parameter pParam = (Parameter)pItem.Tag;
                        pItem.SubItems[1].Text = pParam.DisplayValue;
                        //pItem.Font = new Font(ParamsListView.Font, pParam.m_bChanged ? FontStyle.Italic : FontStyle.Regular);
                        pItem.BackColor = pParam.m_bChanged ? Color.Yellow : Color.White;
                        pParam.m_bChanged = false;
                    }
                }
            }

            textBox1.Text = m_pModule.m_sLog.ToString();
        }

        /// <summary>
        /// Выбирает событие, могущее являться следствием указанного действия и выводит пользователю его описание.
        /// Если событие имеет несколько реакций на выбор, то предлагает пользователю выбор.
        /// Если возможна единственная реакция - сразу выполняет её.
        /// </summary>
        /// <param name="pAction"></param>
        void DoAction(Action pAction)
        {
            if (pAction == null)
                return;

            m_pCurrentEvent = m_pModule.GetEvent(pAction);

            if (m_pCurrentEvent == null)
                return;

            m_pCurrentEvent.PreReaction(m_pModule);

            List<Reaction> cAvailable = m_pCurrentEvent.GetPossibleReactions();

            if (cAvailable.Count == 1 && !cAvailable[0].m_bAlwaysVisible)
            {
                ExecuteReaction(cAvailable[0]);
                ActionsTextBox.Text = m_pModule.m_sHeader;
            }
            else
            {
                GrandActionsPanel.Visible = false;
                EventPanel.Visible = true;

                EventTextBox.Text = m_pCurrentEvent.m_pDescription.GetDescription(m_pModule.m_cStringParameters, m_pModule.m_cNumericParameters, m_pModule.m_cCollections);
                ReactionsPanel.Controls.Clear();

                foreach (Reaction pReaction in cAvailable)
                {
                    Button pButt = new Button();
                    pButt.Text = WordWrap(pReaction.m_sName, 50);
                    pButt.AutoSize = true;
                    pButt.Margin = new Padding(5);
                    pButt.Tag = pReaction;
                    pButt.Enabled = pReaction.Possible();

                    pButt.Click += new EventHandler(Reaction_Click);

                    ReactionsPanel.Controls.Add(pButt);
                }

                if(cAvailable.Count == 0)
                {
                    m_pModule.UpdateWorld();

                    Button pButt = new Button();
                    pButt.Text = "Дальше";
                    pButt.AutoSize = true;
                    pButt.Margin = new Padding(5);

                    pButt.Click += new EventHandler(ReactionEndButton_Click);

                    ReactionsPanel.Controls.Add(pButt);
                }
            }
        }

        void Action_Click(object sender, EventArgs e)
        {
            Button pButton = sender as Button;
            if (pButton == null)
                return;

            DoAction(pButton.Tag as Action);
        }

        /// <summary>
        /// Выводит текстовое описание последствйи выбранной реакции и применяет соответствующий набор последствй.
        /// Затем применяет набор последствий, соответствующий случившемуся событию в целом.
        /// Если текстовое описание последствий не пустое - предлагает пользователю нажать кнопку Next, когда он прочитает описание,
        /// иначе автоматически инициирует нажатие этой кнопки.
        /// </summary>
        /// <param name="pReaction"></param>
        private void ExecuteReaction(Reaction pReaction)
        {
            if (pReaction != null)
                ReactionTextBox.Text = pReaction.Execute(m_pModule);
            else
                ReactionTextBox.Text = "";

            m_pModule.UpdateWorld();

            ReactionPanel.Visible = true;
            EventPanel.Visible = false;

            if (ReactionTextBox.Text != "")
                ShowAllParams();
            else
                ReactionEndButton_Click(null, null);
        }

        void Reaction_Click(object sender, EventArgs e)
        {
            Button pButton = sender as Button;
            if (pButton == null)
                return;

            ExecuteReaction(pButton.Tag as Reaction);
        }

        /// <summary>
        /// Завершение обработки события. Вызывается после того, как пользователь прочитал текстовое описание последствий сделанного им выбора,
        /// или - если описания нет, то сразу после выбора реакции на событие.
        /// Если стоит флаг случайного действия, то выбирает случайное действие из числа доступных и выполняет его.
        /// Иначе, просто обновляет заголовок и предлагает пользователю выбрать действие.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReactionEndButton_Click(object sender, EventArgs e)
        {
            ShowAllParams();

            List<Action> cAvailable = m_pModule.GetPossibleActions();

            if (m_pModule.m_bRandomRound && cAvailable.Count > 0)
            {
                DoAction(cAvailable[Random.Rnd.Get(cAvailable.Count)]);
            }
            else
            {
                ActionsTextBox.Text = m_pModule.m_sHeader;

                GrandActionsPanel.Visible = true;
                ReactionPanel.Visible = false;

                ActionsPanel.Controls.Clear();
                foreach (Action pAction in cAvailable)
                {
                    Button pButt = new Button();
                    pButt.Text = WordWrap(pAction.m_sName, 50);
                    pButt.AutoSize = true;
                    pButt.Margin = new Padding(5);
                    pButt.Tag = pAction;

                    pButt.Click += new EventHandler(Action_Click);

                    ActionsPanel.Controls.Add(pButt);
                }
            }
        }

        private void PlayfieldPanel_Resize(object sender, EventArgs e)
        {
            GrandActionsPanel.Location = new Point(0, 0);
            GrandActionsPanel.Size = PlayfieldPanel.ClientSize;

            EventPanel.Location = new Point(0, 0);
            EventPanel.Size = PlayfieldPanel.ClientSize;

            ReactionPanel.Location = new Point(0, 0);
            ReactionPanel.Size = PlayfieldPanel.ClientSize;
        }
    
        /// <summary>
        /// Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">Text to be word wrapped</param>
        /// <param name="width">Width, in characters, to which the text
        /// should be word wrapped</param>
        /// <returns>The modified text</returns>
        public static string WordWrap(string text, int width)
        {
            int pos, next;
            StringBuilder sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
                return text;

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                int eol = text.IndexOf(Environment.NewLine, pos);
                if (eol == -1)
                    next = eol = text.Length;
                else
                    next = eol + Environment.NewLine.Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;
                        if (len > width)
                            len = BreakLine(text, pos, width);
                        sb.Append(text, pos, len);
                        sb.Append(Environment.NewLine);

                        // Trim whitespace following break
                        pos += len;
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;
                    } while (eol > pos);
                }
                else sb.Append(Environment.NewLine); // Empty line
            }
            return sb.ToString();
        }

        /// <summary>
        /// Locates position to break the given line so as to avoid
        /// breaking words.
        /// </summary>
        /// <param name="text">String that contains line of text</param>
        /// <param name="pos">Index where line of text starts</param>
        /// <param name="max">Maximum line length</param>
        /// <returns>The modified line length</returns>
        private static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;

            // If no whitespace found, break at maximum length
            if (i < 0)
                return max;

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }
    }
}
