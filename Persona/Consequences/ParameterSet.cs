using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;
using nsUniLibXML;
using System.Xml;

namespace Persona.Consequences
{
    public class ParameterSet : Consequence
    {
        public Parameter m_pParam;

        public string m_sNewValue;

        public ParameterSet()
        { }

        public ParameterSet(Parameter pParam, string sValue)
        {
            m_pParam = pParam;
            m_sNewValue = sValue;
        }

        public ParameterSet(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams)
        {
            string sParam = "";
            pXml.GetStringAttribute(pParamNode, "param", ref sParam);
            foreach (Parameter pParam in cParams)
                if (pParam.m_sName == sParam)
                {
                    m_pParam = pParam;
                    break;
                }

            pXml.GetStringAttribute(pParamNode, "value", ref m_sNewValue);
        }
        
        internal override void WriteXML(UniLibXML pXml, XmlNode pConsequenceNode)
        {
            pXml.AddAttribute(pConsequenceNode, "param", m_pParam.m_sName);
            pXml.AddAttribute(pConsequenceNode, "value", m_sNewValue);
        }

        public override string ToString()
        {
            string sValue = m_sNewValue;
            if (m_pParam != null && m_pParam is NumericParameter)
            {
                NumericParameter pParam = m_pParam as NumericParameter;
                float fValue;
                if (float.TryParse(m_sNewValue, out fValue))
                {
                    foreach (var pRange in pParam.m_cRanges)
                    {
                        if (pRange.m_fMin <= fValue && pRange.m_fMax >= fValue)
                        {
                            if (pRange.m_fMin == pRange.m_fMax)
                                sValue = "[" + pRange.m_sDescription + "]";
                            else
                                sValue = string.Format("{0} [{1}]", m_sNewValue, pRange.m_sDescription);
                            break;
                        }
                    }
                }
            }
            if (m_pParam != null && m_pParam is BoolParameter)
            {
                bool bNewValue = true;
                if (!bool.TryParse(m_sNewValue, out bNewValue))
                {
                    float fNewValue = 0;
                    float.TryParse(m_sNewValue, out fNewValue);
                    bNewValue = (fNewValue > 0);
                }

                sValue = bNewValue ? "ДА" : "НЕТ";
            }
            return string.Format("{0} := {1}", m_pParam != null ? m_pParam.m_sName : "НЕВЕРНЫЙ ПАРАМЕТР", sValue);
        }

        public override Consequence Clone()
        {
            ParameterSet pNew = new ParameterSet(m_pParam, m_sNewValue);

            return pNew;
        }
    }
}
