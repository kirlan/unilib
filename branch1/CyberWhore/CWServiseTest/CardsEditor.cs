using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nsUniLibXML;
using System.Xml;

namespace CWServiseTest
{
    public partial class CardsEditor : Form
    {
        List<DemandCard> m_cDemands = new List<DemandCard>();
        List<AnswerCard> m_cAnswers = new List<AnswerCard>();

        public CardsEditor()
        {
            InitializeComponent();
        }

        private void addNewCardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPageDemands)
            {
                DemandCard pNewCard = new DemandCard();
                m_cDemands.Add(pNewCard);
                int iIndex = listBox1.Items.Add(pNewCard);
                listBox1.SelectedIndex = iIndex;
            }
            if (tabControl1.SelectedTab == tabPageAnswers)
            {
                AnswerCard pNewCard = new AnswerCard();
                m_cAnswers.Add(pNewCard);
                int iIndex = listBox1.Items.Add(pNewCard);
                listBox1.SelectedIndex = iIndex;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = listBox1.SelectedItem;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (tabControl1.SelectedTab == tabPageDemands)
            {
                listBox1.Items.AddRange(m_cDemands.ToArray());
                if (listBox1.Items.Count > 0)
                    listBox1.SelectedIndex = 0;
                else
                    propertyGrid1.SelectedObject = null;
            }
            if (tabControl1.SelectedTab == tabPageAnswers)
            {
                listBox1.Items.AddRange(m_cAnswers.ToArray());
                if (listBox1.Items.Count > 0)
                    listBox1.SelectedIndex = 0;
                else
                    propertyGrid1.SelectedObject = null;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                UniLibXML pXml = new UniLibXML("Deck");

                XmlNode pDemandsNode = pXml.CreateNode(pXml.Root, "Demands");

                foreach (DemandCard pCard in m_cDemands)
                {
                    XmlNode pCardNode = pXml.CreateNode(pDemandsNode, "Card");
                    pCard.Write(pXml, pCardNode);
                }

                XmlNode pAnswersNode = pXml.CreateNode(pXml.Root, "Answers");

                foreach (AnswerCard pCard in m_cAnswers)
                {
                    XmlNode pCardNode = pXml.CreateNode(pAnswersNode, "Card");
                    pCard.Write(pXml, pCardNode);
                }

                pXml.Write(saveFileDialog1.FileName);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                UniLibXML pXml = new UniLibXML("Deck");

                if (!pXml.Load(openFileDialog1.FileName))
                    return;

                foreach (XmlNode pCatNode in pXml.Root.ChildNodes)
                {
                    if (pCatNode.Name == "Demands")
                    {
                        foreach (XmlNode pTagNode in pCatNode.ChildNodes)
                        {
                            if (pTagNode.Name == "Card")
                            {
                                DemandCard pNewCard = new DemandCard(pXml, pTagNode);
                                m_cDemands.Add(pNewCard);
                            }
                        }
                    }
                    if (pCatNode.Name == "Answers")
                    {
                        foreach (XmlNode pTagNode in pCatNode.ChildNodes)
                        {
                            if (pTagNode.Name == "Card")
                            {
                                AnswerCard pNewCard = new AnswerCard(pXml, pTagNode);
                                m_cAnswers.Add(pNewCard);
                            }
                        }
                    }
                }
                
                tabControl1_SelectedIndexChanged(sender, e);
            }
        }
    }
}
