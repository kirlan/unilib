using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace Persona.Parameters
{
    public class BoolParameter : Parameter
    {
        /// <summary>
        /// Значение в начале игры.
        /// </summary>
        public bool m_bDefaultValue = false;

        public BoolParameter()
        {
        }

        public BoolParameter(BoolParameter pOrigin)
            :base(pOrigin)
        {
            m_bDefaultValue = pOrigin.m_bDefaultValue;
        }

        public BoolParameter(string sName, string sGroup)
        {
            m_sName = sName;
            m_sGroup = sGroup;
        }

        public BoolParameter(UniLibXML pXml, XmlNode pParamNode)
            : base(pXml, pParamNode)
        {
            pXml.GetBoolAttribute(pParamNode, "value", ref m_bDefaultValue);
        }

        internal override void WriteXML(UniLibXML pXml, XmlNode pParamNode)
        {
            base.WriteXML(pXml, pParamNode);

            pXml.AddAttribute(pParamNode, "value", m_bDefaultValue);
        }
    }
}
