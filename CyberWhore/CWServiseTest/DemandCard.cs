using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using nsUniLibControls;

namespace CWServiseTest
{
    public class DemandCard
    {
        private string m_sName = "New Card";

        [Description("The name of card, should contains a short description of requested action, e.g. 'Deep Throat'"), Category("Appearance"), Browsable(true)] 
        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        private string m_sDescription = "No description yet";

        [Description("Full description of requested action, e.g. 'Jack wants you to get his dick really deep!'"), Category("Appearance"), Browsable(true)]
        public string Description
        {
            get { return m_sDescription; }
            set { m_sDescription = value; }
        }

        private CardSuits m_eSuit = new CardSuits();

        [Description("To which suit belongs this card?"), Category("Appearance"), Browsable(true)]
        [EditorAttribute(typeof(EnumCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public CardSuits Suit
        {
            get { return m_eSuit; }
            set { m_eSuit = value; }
        }

        public override string ToString()
        {
            return m_sName;
        }
    }
}
