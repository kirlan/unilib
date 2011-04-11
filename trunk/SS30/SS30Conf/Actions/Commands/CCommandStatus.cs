using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;
using SS30Conf.Persons;

namespace SS30Conf.Actions.Commands
{
    /// <summary>
    /// Модифицирует список статусов реципиента
    /// </summary>
    public class CCommandStatus: CCommand
    {
        private CConfigProperty<string> m_pStatus;
        /// <summary>
        /// Строковый код статуса
        /// </summary>
        public string Status
        {
            get { return m_pStatus.Value; }
            set 
            { 
                m_pStatus.Value = value;
                if (!CConfigRepository.Instance.Statuses.Contains(m_pStatus.Value))
                    CConfigRepository.Instance.Statuses.Add(m_pStatus.Value);
            }
        }
        private CConfigProperty<bool> m_pEnable;
        /// <summary>
        /// true - добавить указанный статус в список
        /// false - сбросить указанный статус
        /// </summary>
        public bool Enable
        {
            get { return m_pEnable.Value; }
            set { m_pEnable.Value = value; }
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
            if (Enable)
            {
                if (!subject.Statuses.Contains(Status))
                    subject.Statuses.Add(Status);
            }
            else
            {
                if (subject.Statuses.Contains(Status))
                    subject.Statuses.Remove(Status);
            }
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pStatus = new CConfigProperty<string>(this, "Status", "default");

            m_pEnable = new CConfigProperty<bool>(this, "Enable", true);

            m_pSubject = new CConfigProperty<Subject>(this, "Subject", Subject.TARGET);
        }

        public CCommandStatus(CReaction pParent)
            : base(StringSubCategory.STATUS, pParent)
        { }

        public CCommandStatus(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode)
        {
            Status = Status;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}({2})", Subject== Subject.ACTOR ? "actor":"target", Enable ? "+" : "-", Status);
        }
    }
}
