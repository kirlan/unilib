using System;
using System.Collections.Generic;
using System.Text;
using SS30Conf.Items;

namespace SS30Conf.Actions.Conditions
{
    public class CReactionConditionItem: CReactionCondition
    {
        private string m_sItemID;
        private CItem m_pItem;

        public CItem Item
        {
            get { return m_pItem; }
            set 
            {
                if (m_pItem != value)
                {
                    m_pItem = value;
                    Modify();
                }
            }
        }
        private bool m_bHaveIt;

        public bool HaveIt
        {
            get { return m_bHaveIt; }
            set 
            {
                if (m_bHaveIt != value)
                {
                    m_bHaveIt = value;
                    Modify();
                }
            }
        }

        public override bool Check()
        {
            throw new NotImplementedException();
        }

        public override string GetCfgString()
        {
            if (m_pItem == null)
                return string.Format("{0}|{1}|{2}", base.GetCfgString(), "nil", 0);
            else
                return string.Format("{0}|{1}|{2}", base.GetCfgString(), m_pItem.ID, m_bHaveIt);
        }

        public override void Parse(CConfigParser pParser)
        {
            base.Parse(pParser);
            m_sItemID = pParser.ReadString();
            HaveIt = bool.Parse(pParser.ReadString());
        }

        public override void PostParse()
        {
            if (m_sItemID != "nil")
                Item = Repository.Strings[StringCategory.ITEM][m_sItemID] as CItem;
            m_pParent = Repository.Strings[StringCategory.REACTION][m_sParentID] as CReaction;
            m_pParent.Conditions.Add(this);
        }

        public CReactionConditionItem(CReaction pParent)
            : base(StringSubCategory.ITEM, pParent)
        { }

        public CReactionConditionItem(CConfigParser pParser)
            : base(pParser)
        { }

        public override string ToString()
        {
            if (m_pItem == null)
                return "error item";

            if (m_bHaveIt)
                return string.Format("have {0}", m_pItem.ID);
            else
                return string.Format("have not {0}", m_pItem.ID);
        }
    }
}
