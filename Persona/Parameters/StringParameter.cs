using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace Persona.Parameters
{
    public class StringParameter : Parameter
    {
        /// <summary>
        /// Значение в начале игры.
        /// </summary>
        public string m_sDefaultValue = "";

        public StringParameter(UniLibXML pXml, XmlNode pParamNode)
            : base(pXml, pParamNode)
        {
            pXml.GetStringAttribute(pParamNode, "value", ref m_sDefaultValue);
        }
    
        internal override void WriteXML(UniLibXML pXml, XmlNode pParamNode)
        {
            base.WriteXML(pXml, pParamNode);

            pXml.AddAttribute(pParamNode, "value", m_sDefaultValue);
        }
    }
}
