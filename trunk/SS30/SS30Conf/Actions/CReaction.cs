using System;
using System.Collections.Generic;
using System.Text;
using SS30Conf.Actions.Conditions;
using SS30Conf.Actions.Commands;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Persons;

namespace SS30Conf.Actions
{
    public enum Subject
    {
        ACTOR,
        TARGET
    }


    /// <summary>
    /// Реакция - ответ игрового мира на Действие.
    /// Для каждого Действия может быть определено несколько реакций с различными наборами условий применимости.
    /// Если одновременно выполняются условия применимости нескольких реакций - применяем реакцию с более высоким
    /// приоритетом. Реакции с более низким приоритетом выполняем только в случае, если у реакции с более высоким 
    /// приоритетом стоит соответствующий флаг.
    /// </summary>
    public class CReaction: CParentedConfigString<CAction>
    {
        private List<CCondition> m_cConditions = new List<CCondition>();

        public List<CCondition> Conditions
        {
            get { return m_cConditions; }
        }
        private List<CCommand> m_cCommands = new List<CCommand>();

        public List<CCommand> Commands
        {
            get { return m_cCommands; }
        }
        // Условия и команды - при записи в файл хранят у себя, к какой реакции они относятся

        private CConfigProperty<int> m_pPriority;
        /// <summary>
        /// Приоритет применения реакции.
        /// Самый высокий приоритет - 0.
        /// Чем число больше, тем ниже приоритет.
        /// </summary>
        public int Priority
        {
            get { return m_pPriority.Value; }
            set 
            {
                if (Parent.CanUsePriority(value))
                {
                    m_pPriority.Value = value;
                    Parent.OnPriorityChanged(this);
                }
            }
        }

        private CConfigProperty<bool> m_pContinue;

        /// <summary>
        /// Продолжать ли поиск подходящих реакций, если эта подойдёт?
        /// </summary>
        public bool Continue
        {
            get { return m_pContinue.Value; }
            set { m_pContinue.Value = value; }
        }

        /// <summary>
        /// Применима ли эта реакция?
        /// Перебирает все условия в списке, если хоть одно не выполняется - значит не применима.
        /// </summary>
        /// <returns>true - все условия выполняются</returns>
        public bool Check(CPerson actor, CPerson target)
        {
            foreach (CCondition cond in m_cConditions)
                if (!cond.Check(actor, target))
                    return false;

            return true;
        }

        /// <summary>
        /// Последовательно выполняет все команды в списке.
        /// </summary>
        public void Execute(CPerson actor, CPerson target)
        {
            actor.Room.RecordEvent(string.Format("{0}: {1}", actor.Name, Description));
            foreach (CCommand cmd in m_cCommands)
                cmd.Execute(actor, target);
        }

        public CReaction(CAction pParent)
            : base(StringCategory.REACTION, pParent)
        {
            Priority = Parent.GetMaxPriority() + 100;
            Value = Value.Insert(3, pParent.Value.Substring(3));
            Parent.AddReaction(this);
        }

        public CReaction(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode, StringCategory.ACTION)
        { 
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pPriority = new CConfigProperty<int>(this, "Priority", 0);

            m_pContinue = new CConfigProperty<bool>(this, "Continue", false);
        }

        public override void PostParse()
        {
            if(Parent == null)
                Parent.AddReaction(this);
            else
                Parent.AddReaction(this);
        }
    }
}
