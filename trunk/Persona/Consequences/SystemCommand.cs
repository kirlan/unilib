using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;
using Persona.Parameters;

namespace Persona.Consequences
{
    /// <summary>
    /// Возможное последствие разыгранного события - системная команда.
    /// </summary>
    public class SystemCommand : Consequence
    {
        public enum ActionType
        { 
            /// <summary>
            /// Вернуться к выбору категории и сделать другой выбор.
            /// </summary>
            Return,
            /// <summary>
            /// Разыграть случайное событие из числа доступных.
            /// </summary>
            RandomRound,
            /// <summary>
            /// Конец игры.
            /// </summary>
            GameOver,
            /// <summary>
            /// Установить текст, выводимый при выборе действия.
            /// </summary>
            SetDescription
        }

        public ActionType m_eAction = ActionType.GameOver;

        public object m_pValue = null;

        public SystemCommand()
        { 
        }

        public SystemCommand(ActionType eAction)
        {
            m_eAction = eAction;
        }

        public SystemCommand(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams)
        {
            object temp = m_eAction;
            pXml.GetEnumAttribute(pParamNode, "command", typeof(ActionType), ref temp);
            m_eAction = (ActionType)temp;

            if(m_eAction == ActionType.SetDescription)
            {
                string sValue = "";
                pXml.GetStringAttribute(pParamNode, "value", ref sValue);
                m_pValue = sValue;
            }
        }

        internal override void WriteXML(UniLibXML pXml, XmlNode pConsequenceNode)
        {
            pXml.AddAttribute(pConsequenceNode, "command", m_eAction);
            if (m_eAction == ActionType.SetDescription)
            {
                pXml.AddAttribute(pConsequenceNode, "value", m_pValue as string);
            }
        }

        public override string ToString()
        {
            string sResult = "НЕИЗВЕСТНАЯ КОМАНДА";
            switch (m_eAction)
            {
                case ActionType.Return:
                    sResult = "ОТКАТ";
                    break;
                case ActionType.RandomRound:
                    sResult = "СЛУЧАЙНАЯ СИТУАЦИЯ";
                    break;
                case ActionType.GameOver:
                    sResult = "КОНЕЦ ИГРЫ";
                    break;
                case ActionType.SetDescription:
                    sResult = "ЗАГОЛОВОК := '" + m_pValue as string + "'";
                    break;
            }
            return sResult;
        }

        public override Consequence Clone()
        {
            SystemCommand pNew = new SystemCommand(m_eAction);
            if (m_eAction == ActionType.SetDescription)
                pNew.m_pValue = m_pValue as string;

            return pNew;
        }

        /// <summary>
        /// Выполнить команду
        /// </summary>
        /// <param name="pModule">модуль, состояние которого будет меняться</param>
        internal override void Apply(Module pModule)
        {
            switch (m_eAction)
            {
                case ActionType.Return:
                    pModule.m_bRollback = true;
                    break;
                case ActionType.RandomRound:
                    pModule.m_bRandomRound = true;
                    break;
                case ActionType.GameOver:
                    pModule.m_bGameOver = true;
                    break;
                case ActionType.SetDescription:
                    pModule.m_sHeader = m_pValue as string;
                    break;
            }
        }
    }
}
