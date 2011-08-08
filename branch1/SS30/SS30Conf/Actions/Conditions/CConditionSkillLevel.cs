using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Persons;

namespace SS30Conf.Actions.Conditions
{
    /// <summary>
    /// Условие нахождения значения указанного навыка исполнителя
    /// в заданном диапазоне.
    /// </summary>
    public class CConditionSkillLevel: CCondition
    {
        private CBindingConfigProperty<CSkill> m_pSkill;

        public CSkill Skill
        {
            get { return m_pSkill.Object; }
            set { m_pSkill.Object = value; }
        }

        private CConfigProperty<int> m_pLowTreshold;

        public int LowTreshold
        {
            get { return m_pLowTreshold.Value; }
            set { m_pLowTreshold.Value = value; }
        }

        private CConfigProperty<int> m_pHiTreshold;

        public int HiTreshold
        {
            get { return m_pHiTreshold.Value; }
            set { m_pHiTreshold.Value = value; }
        }

        private CConfigProperty<Subject> m_pSubject;

        public Subject Subject
        {
            get { return m_pSubject.Value; }
            set { m_pSubject.Value = value; }
        }


        public override bool Check(CPerson actor, CPerson target)
        {
            CPerson subject = Subject == Subject.ACTOR ? actor : target;
            return (subject.Skills[Skill.Value] >= LowTreshold) && (subject.Skills[Skill.Value] <= HiTreshold);
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pSkill = new CBindingConfigProperty<CSkill>(this, "Skill", StringCategory.SKILL);

            m_pLowTreshold = new CConfigProperty<int>(this, "LowTreshold", 0);

            m_pHiTreshold = new CConfigProperty<int>(this, "HiTreshold", 100);

            m_pSubject = new CConfigProperty<Subject>(this, "Subject", Subject.ACTOR);
        }

        public override void PostParse()
        {
            Parent.Conditions.Add(this);
        }

        public CConditionSkillLevel(CReaction pParent)
            : base(StringSubCategory.SKILL, pParent)
        { }

        public CConditionSkillLevel(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode)
        { }

        public override string ToString()
        {
            return string.Format("{0}'s {1} {2}in ({3}..{4})", Subject == Actions.Subject.ACTOR ? "actor":"target", Skill != null ? Skill.Value:"WRONG SKILL", Not ? "not " : "", LowTreshold, HiTreshold);
        }
    }
}
