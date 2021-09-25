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

        /// <summary>
        /// Значение в течение игры.
        /// </summary>
        public string m_sValue = "";

        public override string DisplayValue
        {
            get
            {
                return GetDisplayValue(m_sValue);
            }
        }

        public override string GetDisplayValue(object value)
        {
            if (value is string)
                return (string)value;
            else
                return value.ToString();
        }

        public override void SetValue(string sValue)
        {
            if (m_sValue != sValue)
                m_bChanged = true;

            m_sValue = sValue;
        }

        public StringParameter()
        {
        }

        public StringParameter(StringParameter pOrigin, bool bClone)
            :base(pOrigin, bClone)
        {
            m_sDefaultValue = pOrigin.m_sDefaultValue;
        }

        public StringParameter(UniLibXML pXml, XmlNode pParamNode, string sCollection)
            : base(pXml, pParamNode, sCollection)
        {
            pXml.GetStringAttribute(pParamNode, "value", ref m_sDefaultValue);
        }
    
        internal override void WriteXML(UniLibXML pXml, XmlNode pParamNode)
        {
            base.WriteXML(pXml, pParamNode);

            pXml.AddAttribute(pParamNode, "value", m_sDefaultValue);
        }

        public override void Init()
        {
            m_sValue = m_sDefaultValue;
        }
    }
}
