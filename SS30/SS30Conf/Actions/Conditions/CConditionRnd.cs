using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Persons;
using Random;

namespace SS30Conf.Actions.Conditions
{
    /// <summary>
    /// Случайное условие. Вероятность выполнения условия считается как
    /// Chances/Total
    /// </summary>
    public class CConditionRnd: CCondition
    {
        private CConfigProperty<int> m_pChances;

        public int Chances
        {
            get { return m_pChances.Value; }
            set { m_pChances.Value = value; }
        }
        private CConfigProperty<int> m_pTotal;

        public int Total
        {
            get { return m_pTotal.Value; }
            set { m_pTotal.Value = value; }
        }

        public override bool Check(CPerson actor, CPerson target)
        {
            return Rnd.Get(Total) < Chances;
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pChances = new CConfigProperty<int>(this, "Chances", 1);

            m_pTotal = new CConfigProperty<int>(this, "Total", 2);
        }

        public CConditionRnd(CReaction pParent)
            : base(StringSubCategory.RND, pParent)
        { }

        public CConditionRnd(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode)
        { }

        public override string ToString()
        {
            return string.Format("chances {0}:{1}", Not ? Total - Chances : Chances, Total);
        }
    }
}
