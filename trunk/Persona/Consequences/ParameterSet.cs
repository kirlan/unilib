using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;
using nsUniLibXML;
using System.Xml;

namespace Persona.Consequences
{
    class ParameterSet : Consequence
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
        
        internal override void SaveXML(UniLibXML pXml, XmlNode pConsequenceNode)
        {
            pXml.AddAttribute(pConsequenceNode, "param", m_pParam.m_sName);
            pXml.AddAttribute(pConsequenceNode, "value", m_sNewValue);
        }

        public override string ToString()
        {
            return string.Format("{0} := {1}", m_pParam != null ? m_pParam.m_sName : "НЕВЕРНЫЙ ПАРАМЕТР", m_sNewValue);
        }
    }
}
