using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Persons;

namespace SS30Conf.Actions.Conditions
{
    public class CConditionStatLevel: CCondition
    {
        private CBindingConfigProperty<CPersonStat> m_pStat;

        public CPersonStat Stat
        {
            get { return m_pStat.Object; }
            set { m_pStat.Object = value; }
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
            CPerson subject = Subject == Subject.ACTOR ? actor:target;
            return (subject.Stats[Stat.Value] >= LowTreshold) && (subject.Stats[Stat.Value] <= HiTreshold);
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pStat = new CBindingConfigProperty<CPersonStat>(this, "Stat", StringCategory.STAT);

            m_pLowTreshold = new CConfigProperty<int>(this, "LowTreshold", 0);

            m_pHiTreshold = new CConfigProperty<int>(this, "HiTreshold", 100);

            m_pSubject = new CConfigProperty<Subject>(this, "Subject", Subject.ACTOR);
        }

        public override void PostParse()
        {
            Parent.Conditions.Add(this);
        }

        public CConditionStatLevel(CReaction pParent)
            : base(StringSubCategory.STAT, pParent)
        { }

        public CConditionStatLevel(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode)
        { }

        public override string ToString()
        {
            return string.Format("{0}'s {1} {2}in ({3}..{4})", Subject == Actions.Subject.ACTOR ? "actor" : "target", Stat != null ? Stat.Value:"WRONG STAT", Not ? "not " : "", LowTreshold, HiTreshold);
        }
    }
}
