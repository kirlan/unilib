using System;
using System.Collections.Generic;
using System.Text;

namespace SS30Conf.Actions.Conditions
{
    public class CReactionConditionSkill: CReactionCondition
    {
        private string m_sSkillID;
        private CSkill m_pSkill;

        public CSkill Skill
        {
            get { return m_pSkill; }
            set 
            {
                if (m_pSkill != value)
                {
                    m_pSkill = value;
                    Modify();
                }
            }
        }
        private int m_iLowTreshold;

        public int LowTreshold
        {
            get { return m_iLowTreshold; }
            set 
            {
                if (m_iLowTreshold != value)
                {
                    m_iLowTreshold = value;
                    Modify();
                }
            }
        }
        private int m_iHiTreshold;

        public int HiTreshold
        {
            get { return m_iHiTreshold; }
            set 
            {
                if (m_iHiTreshold != value)
                {
                    m_iHiTreshold = value;
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
            if (m_pSkill == null)
                return string.Format("{0}|{1}|{2}|{3}", base.GetCfgString(), "nil", 0, 100);
            else
                return string.Format("{0}|{1}|{2}|{3}", base.GetCfgString(), m_pSkill.ID, m_iLowTreshold, m_iHiTreshold);
        }

        public override void Parse(CConfigParser pParser)
        {
            base.Parse(pParser);
            m_sSkillID = pParser.ReadString();
            LowTreshold = pParser.ReadInt();
            HiTreshold = pParser.ReadInt();
        }

        public override void PostParse()
        {
            if (m_sSkillID != "nil")
                Skill = Repository.Strings[StringCategory.SKILL][m_sSkillID] as CSkill;
            m_pParent = Repository.Strings[StringCategory.REACTION][m_sParentID] as CReaction;
            m_pParent.Conditions.Add(this);
        }

        public CReactionConditionSkill(CReaction pParent)
            : base(StringSubCategory.SKILL, pParent)
        { }

        public CReactionConditionSkill(CConfigParser pParser)
            : base(pParser)
        { }

        public override string ToString()
        {
            if (m_pSkill == null)
                return "error skill";

            return string.Format("{0} in ({1}..{2})", m_pSkill.ID, m_iLowTreshold, m_iHiTreshold);
        }
    }
}
