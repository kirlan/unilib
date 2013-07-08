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
            GameOver
        }

        public ActionType m_eAction = ActionType.GameOver;

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
        }

        internal override void SaveXML(UniLibXML pXml, XmlNode pConsequenceNode)
        {
            pXml.AddAttribute(pConsequenceNode, "command", m_eAction);
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
            }
            return sResult;
        }

        public override Consequence Clone()
        {
            SystemCommand pNew = new SystemCommand(m_eAction);

            return pNew;
        }
    }
}
