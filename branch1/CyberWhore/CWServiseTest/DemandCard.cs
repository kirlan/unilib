using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using nsUniLibControls;
using nsUniLibXML;
using System.Xml;

namespace CWServiseTest
{
    public class DemandCard : ICard
    {
        private string m_sName = "New Card";

        [Description("The name of card, should contains a short description of requested action, e.g. 'Deep Throat'"), Category("Global"), Browsable(true)] 
        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        private string m_sDescription = "No description yet";

        [Description("Full description of requested action, e.g. 'Jack wants you to get his dick really deep!'"), Category("Global"), Browsable(true)]
        public string Description
        {
            get { return m_sDescription; }
            set { m_sDescription = value; }
        }

        private CardSuits m_eSuit = new CardSuits();

        [Description("To which suit belongs this card?"), Category("Global"), Browsable(true)]
        [EditorAttribute(typeof(EnumCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public CardSuits Suit
        {
            get { return m_eSuit; }
            set { m_eSuit = value; }
        }

        private string[] m_cKeywords = new string[] { };

        [Description("List of keywords, determines which jacks should have this card in their deck."), Category("Demand"), Browsable(true)]
        public string Keywords
        {
            get 
            {
                string sKeywords = "";
                foreach (string sKeyword in m_cKeywords)
                {
                    if (sKeywords.Length > 0 && sKeyword.Length > 0)
                        sKeywords += ", ";
                    sKeywords += sKeyword;
                }
                return sKeywords; 
            }
            set 
            { 
                m_cKeywords = value.Split(new char[] {','}); 
            }
        }

        private int m_iDiffuculty = 100;

        [Description("How difficult it is to resolve the demand (in percents). Value less then 100 means it's an easy task, while value greater then 100 means it's a difficult task."), Category("Demand"), Browsable(true)]
        public int Diffuculty
        {
            get { return m_iDiffuculty; }
            set { m_iDiffuculty = value; }
        }

        private string m_sEffect = "";

        [Description("If not empty, this card will place an effect on whore, which will remain active until this card will be neutralized by special answer card."), Category("Demand"), Browsable(true)]
        public string Effect
        {
            get { return m_sEffect; }
            set { m_sEffect = value; }
        }

        private int m_iTimer = 5;

        [Description("Length of time interval, in which whore should answer to demand (in seconds). If demand will not be answered in time, it will be counted as unsatisfied."), Category("Demand"), Browsable(true)]
        public int Timer
        {
            get { return m_iTimer; }
            set { m_iTimer = value; }
        }

        private string m_sFailureDescription = "No description yet";

        [DisplayName("Description"), Description("Full description of failure, e.g. 'He tries to put his dick int your mouth, but all he gets was your vomit.'"), Category("Failure"), Browsable(true)]
        public string FailureDescription
        {
            get { return m_sFailureDescription; }
            set { m_sFailureDescription = value; }
        }

        private int m_iSatisfaction = 0;

        [DisplayName("Satisfaction increment"), Description("Jack's satisfaction increment in case of demand will remain unsatisfied."), Category("Failure"), Browsable(true)]
        public int Satisfaction
        {
            get { return m_iSatisfaction; }
            set { m_iSatisfaction = value; }
        }

        private int m_iHappiness = 0;

        [DisplayName("Happiness cost"), Description("Whore's happiness cost in case of demand will remain unsatisfied."), Category("Failure"), Browsable(true)]
        public int Happiness
        {
            get { return m_iHappiness; }
            set { m_iHappiness = value; }
        }

        private int m_iHealth = 0;

        [DisplayName("Health cost"), Description("Whore's health cost in case of demand will remain unsatisfied."), Category("Failure"), Browsable(true)]
        public int Health
        {
            get { return m_iHealth; }
            set { m_iHealth = value; }
        }

        private int m_iEnergy = 0;

        [DisplayName("Energy cost"), Description("Whore's energy cost in case of demand will remain unsatisfied."), Category("Failure"), Browsable(true)]
        public int Energy
        {
            get { return m_iEnergy; }
            set { m_iEnergy = value; }
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", m_eSuit.ToShortString(),  m_sName);
        }

        public DemandCard()
        { }

        public DemandCard(UniLibXML pXml, XmlNode pCardNode)
            : this()
        {
            m_eSuit.Clear();

            pXml.GetStringAttribute(pCardNode, "name", ref m_sName);
            pXml.GetStringAttribute(pCardNode, "desc", ref m_sDescription);

            foreach (XmlNode pSubNode in pCardNode.ChildNodes)
            {
                if (pSubNode.Name == "Suits")
                {
                    foreach (XmlNode pSubSubNode in pSubNode.ChildNodes)
                    {
                        if (pSubNode.Name == "Suit")
                        {
                            CardSuit eSuit = CardSuit.Traditional;
                            eSuit = (CardSuit)pXml.GetEnumAttribute(pSubNode, "name", eSuit.GetType());
                            m_eSuit.Select(eSuit, true);
                        }
                    }

                }
                if (pSubNode.Name == "Demand")
                {
                    pXml.GetIntAttribute(pSubNode, "diff", ref m_iDiffuculty);
                    pXml.GetIntAttribute(pSubNode, "time", ref m_iTimer);
                    pXml.GetStringAttribute(pCardNode, "efct", ref m_sEffect);
                    string sKeywords = "";
                    pXml.GetStringAttribute(pCardNode, "kwrd", ref sKeywords);
                    Keywords = sKeywords;
                }
                if (pSubNode.Name == "Failur")
                {
                    pXml.GetIntAttribute(pSubNode, "stfc", ref m_iSatisfaction);
                    pXml.GetIntAttribute(pSubNode, "hlth", ref m_iHealth);
                    pXml.GetIntAttribute(pSubNode, "hpns", ref m_iHappiness);
                    pXml.GetIntAttribute(pSubNode, "enrg", ref m_iEnergy);
                    pXml.GetStringAttribute(pCardNode, "desc", ref m_sFailureDescription);
                }
            }
        }

        internal void Write(UniLibXML pXml, XmlNode pCardNode)
        {
            pXml.AddAttribute(pCardNode, "name", m_sName);
            pXml.AddAttribute(pCardNode, "desc", m_sDescription);
            
            XmlNode pSuitsNode = pXml.CreateNode(pCardNode, "Suits");
            foreach (CardSuit eSuit in m_eSuit.Selected)
            {
                XmlNode pRatingNode = pXml.CreateNode(pSuitsNode, "Suit");
                pXml.AddAttribute(pRatingNode, "name", eSuit);
            }

            XmlNode pDemandNode = pXml.CreateNode(pCardNode, "Demand");

            pXml.AddAttribute(pDemandNode, "diff", m_iDiffuculty);
            pXml.AddAttribute(pDemandNode, "time", m_iTimer);
            pXml.AddAttribute(pDemandNode, "efct", m_sEffect);
            pXml.AddAttribute(pDemandNode, "kwrd", Keywords);

            XmlNode pFailureNode = pXml.CreateNode(pCardNode, "Failur");

            pXml.AddAttribute(pFailureNode, "desc", m_sFailureDescription);
            pXml.AddAttribute(pFailureNode, "stfc", m_iSatisfaction);
            pXml.AddAttribute(pFailureNode, "hlth", m_iHealth);
            pXml.AddAttribute(pFailureNode, "hpns", m_iHappiness);
            pXml.AddAttribute(pFailureNode, "enrg", m_iEnergy);
        }
    }
}
