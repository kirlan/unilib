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
                if (pParam.FullName == sParam)
                {
                    m_pParam = pParam;
                    break;
                }

            //if (m_pParam == null)
            //{
            //    foreach (Parameter pParam in cParams)
            //        if (pParam.m_sName == sParam)
            //        {
            //            m_pParam = pParam;
            //            break;
            //        }
            //}

            pXml.GetStringAttribute(pParamNode, "value", ref m_sNewValue);
        }
        
        internal override void WriteXML(UniLibXML pXml, XmlNode pConsequenceNode)
        {
            pXml.AddAttribute(pConsequenceNode, "param", m_pParam.FullName);
            pXml.AddAttribute(pConsequenceNode, "value", m_sNewValue);
        }

        public string DisplayValue
        {
            get
            {
                if (m_pParam != null)
                    return m_pParam.GetDisplayValue(m_sNewValue);
                else
                    return m_sNewValue;
            }
        }

        public override string ToString()
        {
            string sValue = m_sNewValue;
            if (m_pParam != null)
                sValue = m_pParam.GetDisplayValue(m_sNewValue);

            return string.Format("{0} := {1}", m_pParam != null ? m_pParam.FullName : "НЕВЕРНЫЙ ПАРАМЕТР", sValue);
        }

        public override Consequence Clone()
        {
            ParameterSet pNew = new ParameterSet(m_pParam, m_sNewValue);

            return pNew;
        }

        /// <summary>
        /// Выполнить команду
        /// </summary>
        /// <param name="pModule">не используется, может быть null</param>
        internal override void Apply(Module pModule)
        {
            pModule.m_sLog.AppendLine("\tDO " + this.ToString());
            if (m_pParam != null)
                m_pParam.SetValue(m_sNewValue);
        }
    }
}
