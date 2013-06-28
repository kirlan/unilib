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
    /// Условие - проверка нахождения значения указанного числового параметра в заданном диапазоне
    /// </summary>
    public class ConditionRange: Condition
    {
        /// <summary>
        /// Нижняя граница диапазона (включительно)
        /// </summary>
        public float m_fMinValue = 0;

        /// <summary>
        /// Верхняя граница диапазона (включительно)
        /// </summary>
        public float m_fMaxValue = 100;

        public ConditionRange(Parameter pParam)
            : base(pParam)
        { 
        }

        public ConditionRange(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams)
            : base(pXml, pParamNode, cParams)
        {
            pXml.GetFloatAttribute(pParamNode, "min", ref m_fMinValue);
            pXml.GetFloatAttribute(pParamNode, "max", ref m_fMaxValue);
        }

        internal override void SaveXML(UniLibXML pXml, XmlNode pConditionNode)
        {
            base.SaveXML(pXml, pConditionNode);

            pXml.AddAttribute(pConditionNode, "min", m_fMinValue);
            pXml.AddAttribute(pConditionNode, "max", m_fMaxValue);
        }

        public override string ToString()
        {
            return string.Format("{0} в {1}({2}..{3})", m_pParam1 != null ? m_pParam1.m_sName : "НЕВЕРНЫЙ ПАРАМЕТР", m_bNot ? "НЕ " : "", m_fMinValue, m_fMaxValue);
        }
    }
}
