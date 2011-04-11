﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Persons;

namespace SS30Conf.Actions.Commands
{
    public class CCommandChangeSkill: CCommand
    {
        private CBindingConfigProperty<CSkill> m_pSkill;

        public CSkill Skill
        {
            get { return m_pSkill.Object; }
            set { m_pSkill.Object = value; }
        }
        private CConfigProperty<int> m_iValueChange;

        public int ValueChange
        {
            get { return m_iValueChange.Value; }
            set { m_iValueChange.Value = value; }
        }

        private CConfigProperty<Subject> m_pSubject;

        public Subject Subject
        {
            get { return m_pSubject.Value; }
            set { m_pSubject.Value = value; }
        }

        public override void Execute(CPerson actor, CPerson target)
        {
            CPerson subject = Subject == Subject.ACTOR ? actor : target;
            subject.Skills[Skill.Value] += ValueChange;
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pSkill = new CBindingConfigProperty<CSkill>(this, "Skill", StringCategory.SKILL);

            m_iValueChange = new CConfigProperty<int>(this, "Change", 0);

            m_pSubject = new CConfigProperty<Subject>(this, "Subject", Subject.TARGET);
        }

        public CCommandChangeSkill(CReaction pParent)
            : base(StringSubCategory.SKILL, pParent)
        { }

        public CCommandChangeSkill(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode)
        { }

        public override string ToString()
        {
            return string.Format("{0}'s {1} ({2}{3})", Subject == Subject.ACTOR ? "actor" : "target", Skill != null ? Skill.Value : "WRONG SKILL", ValueChange >= 0 ? "+" : "", ValueChange);
        }
    }
}
