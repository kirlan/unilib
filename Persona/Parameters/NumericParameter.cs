using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace Persona.Parameters
{
    public class NumericParameter : Parameter
    {
        /// <summary>
        /// Возможный диапазон значений для отображения 
        /// значения параметра пользователю.
        /// </summary>
        public class Range
        {
            /// <summary>
            /// Нижняя граница диапазона (включительно)
            /// </summary>
            public float m_fMin;

            /// <summary>
            /// Верхняя граница диапазона (включительно)
            /// </summary>
            public float m_fMax;

            /// <summary>
            /// Название диапазона
            /// </summary>
            public string m_sDescription;
        }

        /// <summary>
        /// Значение в начале игры.
        /// </summary>
        public float m_fDefaultValue = 0;

        /// <summary>
        /// Набор диапазонов для отображения пользователю. 
        /// Чисто интерфейсная штука, может быть пустым.
        /// Если текущее значение параметра попадает в один из диапазонов,
        /// название этого диапазона следует писать в интерфейсе игры 
        /// вместо числового значения.
        /// </summary>
        public List<Range> m_cRanges = new List<Range>();

        public NumericParameter()
        { 
        }

        public NumericParameter(string sName, string sGroup)
        {
            m_sName = sName;
            m_sGroup = sGroup;
        }

        public NumericParameter(UniLibXML pXml, XmlNode pParamNode)
            : base(pXml, pParamNode)
        {
            pXml.GetFloatAttribute(pParamNode, "value", ref m_fDefaultValue);

            foreach (XmlNode pSubNode in pParamNode.ChildNodes)
            {
                if (pSubNode.Name == "Range")
                {
                    Range pRange = new Range();
                    pXml.GetStringAttribute(pSubNode, "name", ref pRange.m_sDescription);
                    pXml.GetFloatAttribute(pSubNode, "min", ref pRange.m_fMin);
                    pXml.GetFloatAttribute(pSubNode, "max", ref pRange.m_fMax);

                    m_cRanges.Add(pRange);
                }
            }
        }

        internal override void WriteXML(UniLibXML pXml, XmlNode pParamNode)
        {
            base.WriteXML(pXml, pParamNode);

            pXml.AddAttribute(pParamNode, "value", m_fDefaultValue);

            foreach (Range pRange in m_cRanges)
            {
                XmlNode pRangeNode = pXml.CreateNode(pParamNode, "Range");
                pXml.AddAttribute(pRangeNode, "name", pRange.m_sDescription);
                pXml.AddAttribute(pRangeNode, "min", pRange.m_fMin);
                pXml.AddAttribute(pRangeNode, "max", pRange.m_fMax);
            }
        }
    }
}
