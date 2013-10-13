using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

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
            EventPanel.Visible = true;

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
        }

        private Event m_pCurrentEvent = null;

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

            m_pCurrentEvent = m_pModule.DoAction(pAction);

            if (m_pCurrentEvent == null)
                return;

            List<Reaction> cAvailable = m_pCurrentEvent.GetPossibleReactions();

            if (cAvailable.Count == 1)
            {
                ExecuteReaction(cAvailable[0]);
                ActionsTextBox.Text = m_pModule.m_sHeader;
            }
            else
            {
                GrandActionsPanel.Visible = false;
                EventPanel.Visible = true;

                EventTextBox.Text = m_pCurrentEvent.m_pDescription.m_sText;
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
        /// Применяет набор последствй, соответствующий указанной реакции.
        /// Затем применяет набор последствий, соответствующий случившемуся событию в целом.
        /// Затем, если стоит флаг случайного действия, то выбирает случайное действие из числа доступных и выполняет его.
        /// Иначе, обновляет заголовок и предлагает пользователю выбрать действие.
        /// </summary>
        /// <param name="pReaction"></param>
        private void ExecuteReaction(Reaction pReaction)
        {
            if (pReaction == null)
                return;

            pReaction.Execute(m_pModule);

            m_pCurrentEvent.PostReaction(m_pModule);

            List<Action> cAvailable = m_pModule.GetPossibleActions();

            if (m_pModule.m_bRandomRound && cAvailable.Count > 0)
            {
                DoAction(cAvailable[Random.Rnd.Get(cAvailable.Count)]);
            }
            else
            {
                ActionsTextBox.Text = m_pModule.m_sHeader;

                GrandActionsPanel.Visible = true;
                EventPanel.Visible = false;

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

        void Reaction_Click(object sender, EventArgs e)
        {
            Button pButton = sender as Button;
            if (pButton == null)
                return;

            ExecuteReaction(pButton.Tag as Reaction);
        }

        private void PlayfieldPanel_Resize(object sender, EventArgs e)
        {
            GrandActionsPanel.Location = new Point(0, 0);
            GrandActionsPanel.Size = PlayfieldPanel.ClientSize;

            EventPanel.Location = new Point(0, 0);
            EventPanel.Size = PlayfieldPanel.ClientSize;
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
