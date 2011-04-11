using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS30Conf.Actions.Conditions
{
    class CReactionConditionRnd: CReactionCondition
    {
        private int m_iChances = 1;

        public int Chances
        {
            get { return m_iChances; }
            set 
            {
                if (m_iChances != value)
                {
                    m_iChances = value;
                    Modify();
                }
            }
        }
        private int m_iTotal = 2;

        public int Total
        {
            get { return m_iTotal; }
            set 
            {
                if (m_iTotal != value)
                {
                    m_iTotal = value;
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
            return string.Format("{0}|{1}|{2}", base.GetCfgString(), m_iChances, m_iTotal);
        }

        public override void Parse(CConfigParser pParser)
        {
            base.Parse(pParser);
            Chances = pParser.ReadInt();
            Total = pParser.ReadInt();
        }

        public override void PostParse()
        {
            m_pParent = Repository.Strings[StringCategory.REACTION][m_sParentID] as CReaction;
            m_pParent.Conditions.Add(this);
        }

        public CReactionConditionRnd(CReaction pParent)
            : base(StringSubCategory.RND, pParent)
        { }

        public CReactionConditionRnd(CConfigParser pParser)
            : base(pParser)
        { }

        public override string ToString()
        {
            return string.Format("chances {0}:{1}", Chances, Total);
        }
    }
}
