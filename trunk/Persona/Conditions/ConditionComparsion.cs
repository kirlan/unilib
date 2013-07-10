using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;
using nsUniLibXML;
using System.Xml;

namespace Persona.Conditions
{
    /// <summary>
    /// Условие - сравнение числового параметра со значением другого параметра.
    /// </summary>
    public class ConditionComparsion: Condition
    {
        public enum ComparsionType
        {
            /// <summary>
            /// много меньше
            /// </summary>
            LOLO,
            /// <summary>
            /// меньше
            /// </summary>
            LO,
            /// <summary>
            /// равно
            /// </summary>
            EQ,
            /// <summary>
            /// больше
            /// </summary>
            HI,
            /// <summary>
            /// много больше
            /// </summary>
            HIHI
        }
        
        /// <summary>
        /// Второй параметр для сравнения.
        /// </summary>
        public Parameter m_pParam2;

        /// <summary>
        /// Требуемое соотношение значений первого и второго параметров.
        /// </summary>
        public ComparsionType m_eType;

        public ConditionComparsion(Parameter pParam1, Parameter pParam2) 
            : base(pParam1)
        {
            m_pParam2 = pParam2;
        }

        public ConditionComparsion(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams)
            : base(pXml, pParamNode, cParams)
        {
            string sParam = "";
            pXml.GetStringAttribute(pParamNode, "param2", ref sParam);
            foreach (Parameter pParam in cParams)
                if (pParam.m_sName == sParam)
                {
                    m_pParam2 = pParam;
                    break;
                }

            object temp = m_eType;
            pXml.GetEnumAttribute(pParamNode, "relation", typeof(ComparsionType), ref temp);
            m_eType = (ComparsionType)temp;
        }

        internal override void WriteXML(UniLibXML pXml, XmlNode pConditionNode)
        {
            base.WriteXML(pXml, pConditionNode);

            pXml.AddAttribute(pConditionNode, "param2", m_pParam2.m_sName);
            pXml.AddAttribute(pConditionNode, "relation", m_eType);
        }
    
        public override string ToString()
        {
            string sType = "?";
            switch (m_eType)
            {
                case ComparsionType.LOLO:
                    sType = "много меньше";
                    break;
                case ComparsionType.LO:
                    sType = "меньше";
                    break;
                case ComparsionType.EQ:
                    sType = "равно";
                    break;
                case ComparsionType.HI:
                    sType = "больше";
                    break;
                case ComparsionType.HIHI:
                    sType = "много больше";
                    break;
            }
            return string.Format("{0} {1}{2} {3}", m_pParam1 != null ? m_pParam1.m_sName : "НЕВЕРНЫЙ ПАРАМЕТР 1", m_bNot ? "НЕ " : "", sType, m_pParam2 != null ? m_pParam2.m_sName : "НЕВЕРНЫЙ ПАРАМЕТР 2");
        }

        public override Condition Clone()
        {
            ConditionComparsion pNew = new ConditionComparsion(m_pParam1, m_pParam2);
            pNew.m_eType = m_eType;
            pNew.m_bNot = m_bNot;

            return pNew;
        }
    }
}
