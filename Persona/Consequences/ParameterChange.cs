using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;
using nsUniLibXML;
using System.Xml;

namespace Persona.Consequences
{
    /// <summary>
    /// Возможное последствие разыгранного события - изменение числового параметра на указанную величину.
    /// </summary>
    public class ParameterChange : Consequence
    {
        public Parameter m_pParam;

        public float m_fDelta;

        public ParameterChange()
        {
        }

        public ParameterChange(Parameter pParam, float fDelta)
        {
            m_pParam = pParam;
            m_fDelta = fDelta;
        }

        public ParameterChange(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams)
        {
            string sParam = "";
            pXml.GetStringAttribute(pParamNode, "param", ref sParam);
            foreach (Parameter pParam in cParams)
                if (pParam.m_sName == sParam)
                {
                    m_pParam = pParam;
                    break;
                }

            pXml.GetFloatAttribute(pParamNode, "delta", ref m_fDelta);
        }

        internal override void WriteXML(UniLibXML pXml, XmlNode pConsequenceNode)
        {
            pXml.AddAttribute(pConsequenceNode, "param", m_pParam.m_sName);
            pXml.AddAttribute(pConsequenceNode, "delta", m_fDelta);
        }

        public override string ToString()
        {
            return string.Format("{0} {1:+#.##;-#.##}", m_pParam != null ? m_pParam.m_sName : "НЕВЕРНЫЙ ПАРАМЕТР", m_fDelta);
        }

        public override Consequence Clone()
        {
            ParameterChange pNew = new ParameterChange(m_pParam, m_fDelta);

            return pNew;
        }
    }
}
