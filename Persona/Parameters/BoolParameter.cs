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

        /// <summary>
        /// Значение в течение игры.
        /// </summary>
        public bool m_bValue = false;

        public override string DisplayValue
        {
            get
            {
                return GetDisplayValue(m_bValue);
            }
        }

        public override string GetDisplayValue(object value)
        {
            bool bValue = true;
            if (value is bool)
            {
                bValue = (bool)value;
            }
            else if (value is string)
            {
                if (!bool.TryParse((string)value, out bValue))
                {
                    float fValue = 0;
                    if(float.TryParse((string)value, out fValue))
                        bValue = (fValue > 0);
                    else
                        return base.GetDisplayValue(value);
                }
            }
            else
            {
                GetDisplayValue(value.ToString());
            }

            return bValue ? "ДА" : "НЕТ";
        }

        public override void SetValue(string sValue)
        {
            bool bNewValue = true;
            if (!bool.TryParse(sValue, out bNewValue))
            {
                float fNewValue = 0;
                float.TryParse(sValue, out fNewValue);
                bNewValue = (fNewValue > 0);
            }

            if (m_bValue != bNewValue)
                m_bChanged = true;

            m_bValue = bNewValue;
        }

        public BoolParameter()
        {
        }

        public BoolParameter(BoolParameter pOrigin, bool bClone)
            :base(pOrigin, bClone)
        {
            m_bDefaultValue = pOrigin.m_bDefaultValue;
        }

        public BoolParameter(string sName, string sGroup)
        {
            m_sName = sName;
            m_sGroup = sGroup;
        }

        public BoolParameter(UniLibXML pXml, XmlNode pParamNode, string sCollection)
            : base(pXml, pParamNode, sCollection)
        {
            pXml.GetBoolAttribute(pParamNode, "value", ref m_bDefaultValue);
        }

        internal override void WriteXML(UniLibXML pXml, XmlNode pParamNode)
        {
            base.WriteXML(pXml, pParamNode);

            pXml.AddAttribute(pParamNode, "value", m_bDefaultValue);
        }

        public override void Init()
        {
            m_bValue = m_bDefaultValue;
        }
    }
}
