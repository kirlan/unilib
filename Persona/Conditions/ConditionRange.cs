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
            NumericParameter pNum = pParam as NumericParameter;
            m_fMinValue = pNum.m_fMin;
            m_fMaxValue = pNum.m_fMax;
        }

        public ConditionRange(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams)
            : base(pXml, pParamNode, cParams)
        {
            pXml.GetFloatAttribute(pParamNode, "min", ref m_fMinValue);
            pXml.GetFloatAttribute(pParamNode, "max", ref m_fMaxValue);
        }

        internal override void WriteXML(UniLibXML pXml, XmlNode pConditionNode)
        {
            base.WriteXML(pXml, pConditionNode);

            pXml.AddAttribute(pConditionNode, "min", m_fMinValue);
            pXml.AddAttribute(pConditionNode, "max", m_fMaxValue);
        }

        public override string ToString()
        {
            string sRange = string.Format("в {0}..{1}", m_fMinValue, m_fMaxValue);
            if(m_pParam1 != null)
            {
                NumericParameter pParam = m_pParam1 as NumericParameter;
                foreach (var pRange in pParam.m_cRanges)
                {
                    if (pRange.m_fMin == m_fMinValue && pRange.m_fMax == m_fMaxValue)
                    {
                        if (pRange.m_fMin == pRange.m_fMax)
                            sRange = "[" + pRange.m_sDescription + "]";
                        else 
                            sRange += " [" + pRange.m_sDescription + "]";
                        break;
                    }
                }
            }
            return string.Format("{0} {1}{2}", m_pParam1 != null ? m_pParam1.m_sName : "НЕВЕРНЫЙ ПАРАМЕТР", m_bNot ? "НЕ " : "", sRange);
        }

        public override Condition Clone()
        {
            ConditionRange pNew = new ConditionRange(m_pParam1);
            pNew.m_fMinValue = m_fMinValue;
            pNew.m_fMaxValue = m_fMaxValue;
            pNew.m_bNot = m_bNot;

            return pNew;
        }

        public override bool Check()
        {
            float fValue = (m_pParam1 as NumericParameter).m_fValue;

            bool bRes = (fValue >= m_fMinValue && fValue <= m_fMaxValue);

            return m_bNot ? !bRes : bRes;
        }
    }
}
