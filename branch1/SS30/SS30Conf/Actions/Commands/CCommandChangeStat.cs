using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Persons;

namespace SS30Conf.Actions.Commands
{
    public class CCommandChangeStat: CCommand
    {
        private CBindingConfigProperty<CPersonStat> m_pStat;

        public CPersonStat Stat
        {
            get { return m_pStat.Object; }
            set { m_pStat.Object = value; }
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
            subject.Stats[Stat.Value] += ValueChange;
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pStat = new CBindingConfigProperty<CPersonStat>(this, "State", StringCategory.STAT);

            m_iValueChange = new CConfigProperty<int>(this, "Change", 0);

            m_pSubject = new CConfigProperty<Subject>(this, "Subject", Subject.TARGET);
        }

        public CCommandChangeStat(CReaction pParent)
            : base(StringSubCategory.STAT, pParent)
        { }

        public CCommandChangeStat(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode)
        { }

        public override string ToString()
        {
            return string.Format("{0}'s {1} ({2}{3})", Subject == Subject.ACTOR ? "actor" : "target", Stat != null ? Stat.Value : "WRONG STAT", ValueChange >= 0 ? "+" : "", ValueChange);
        }
    }
}
