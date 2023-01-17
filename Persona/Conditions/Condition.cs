using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;
using System.Xml;
using nsUniLibXML;

namespace Persona.Conditions
{
    /// <summary>
    /// Условие для выбора описания события и определения доступности реакций
    /// </summary>
    public abstract class Condition
    {
        /// <summary>
        /// Параметр, от значения которого всё зависит.
        /// </summary>
        public Parameter m_pParam1;

        /// <summary>
        /// Флаг отрицания - если true, то условие считается истинным если указанный критерий НЕ выполняется.
        /// </summary>
        public bool m_bNot;

        protected Condition(Parameter pParam1)
        {
            m_pParam1 = pParam1;
        }

        protected Condition(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams)
        {
            string sParam = "";
            pXml.GetStringAttribute(pParamNode, "param", ref sParam);
            foreach(Parameter pParam in cParams)
                if (pParam.FullName == sParam)
                {
                    m_pParam1 = pParam;
                    break;
                }

            pXml.GetBoolAttribute(pParamNode, "inverse", ref m_bNot);
        }

        internal virtual void WriteXML(UniLibXML pXml, XmlNode pConditionNode)
        {
            pXml.AddAttribute(pConditionNode, "param", m_pParam1.FullName);
            pXml.AddAttribute(pConditionNode, "inverse", m_bNot);
        }

        public abstract Condition Clone();

        public abstract bool Check();
    }
}
