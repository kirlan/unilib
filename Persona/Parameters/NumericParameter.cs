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
        public enum Operation
        {
            /// <summary>
            /// сложение
            /// </summary>
            ADD,
            /// <summary>
            /// вычитание
            /// </summary>
            SUB,
            /// <summary>
            /// присвоение
            /// </summary>
            SET,
            /// <summary>
            /// среднее арифметическое
            /// </summary>
            AVG
        }

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

            public Range(float fMin)
            {
                m_fMin = fMin;
                m_fMax = fMin;
            }

            public Range(Range pOrigin)
            {
                m_fMin = pOrigin.m_fMin;
                m_fMax = pOrigin.m_fMax;
                m_sDescription = pOrigin.m_sDescription;
            }

            public override string ToString()
            {
                return string.Format("{0}..{1} [{2}]", m_fMin, m_fMax, m_sDescription);
            }
        }

        public override string DisplayValue
        {
            get
            {
                return GetDisplayValue(m_fValue);
            }
        }

        public string ShortDisplayValue
        {
            get
            {
                return GetShortDisplayValue(m_fValue);
            }
        }

        public string m_sFormat = "0";

        public string GetShortDisplayValue(object value)
        {
            float fValue = float.NaN;
            if (value is float)
            {
                fValue = (float)value;
            }
            else if (value is string)
            {
                float.TryParse((string)value, out fValue);
            }
            else
            {
                GetDisplayValue(value.ToString());
            }

            if (!float.IsNaN(fValue))
                return fValue.ToString(m_sFormat);

            return base.GetDisplayValue(value);
        }

        public override string GetDisplayValue(object value)
        {
            float fValue = float.NaN;
            if (value is float)
            {
                fValue = (float)value;
            }
            else if (value is string)
            {
                float.TryParse((string)value, out fValue);
            }
            else
            {
                GetDisplayValue(value.ToString());
            }

            if (!float.IsNaN(fValue))
            {
                foreach (var pRange in m_cRanges)
                {
                    if (pRange.m_fMin <= fValue && pRange.m_fMax >= fValue)
                    {
                        if (pRange.m_fMin == pRange.m_fMax)
                            //return "[" + pRange.m_sDescription + "]";
                            return pRange.m_sDescription;
                        else
                        {
                            string sFormatter = "{0} [{1}]";
                            if (!string.IsNullOrEmpty(m_sFormat))
                                sFormatter = "{0:" + m_sFormat + "} [{1}]";
                            return string.Format(sFormatter, fValue, pRange.m_sDescription);
                        }
                    }
                }

                return fValue.ToString(m_sFormat);
            }

            return base.GetDisplayValue(value);
        }

        public override void SetValue(string sValue)
        {
            float fValue = m_fValue;
            float.TryParse(sValue, out fValue);

            if (fValue < m_fMin)
                fValue = m_fMin;
            if (fValue > m_fMax)
                fValue = m_fMax;

            if (m_fValue != fValue)
                m_bChanged = true;

            m_fValue = fValue;
        }

        public void SetValue(float fValue)
        {
            if (fValue < m_fMin)
                fValue = m_fMin;
            if (fValue > m_fMax)
                fValue = m_fMax;

            if (m_fValue != fValue)
                m_bChanged = true;

            m_fValue = fValue;
        }

        public void ChangeValue(float fDelta)
        {
            float fValue = m_fValue + fDelta;
            if (fValue < m_fMin)
                fValue = m_fMin;
            if (fValue > m_fMax)
                fValue = m_fMax;

            if (m_fValue != fValue)
                m_bChanged = true;

            m_fValue = fValue;
        }

        public static float GetOperationResult(float fValue1, float fValue2, Operation eOperation)
        {
            switch (eOperation)
            {
                case Operation.ADD:
                    return fValue1 + fValue2;
                case Operation.SUB:
                    return fValue1 - fValue2;
                case Operation.SET:
                    return fValue2;
                case Operation.AVG:
                    return (fValue1*2 + fValue2) / 3;
            }  
            return float.NaN;
        }

        public void ChangeValue(NumericParameter pModifer, Operation eOperation)
        {
            float fValue = GetOperationResult(m_fValue, pModifer.m_fValue, eOperation);

            if (fValue < m_fMin)
                fValue = m_fMin;
            if (fValue > m_fMax)
                fValue = m_fMax;

            if (m_fValue != fValue)
                m_bChanged = true;

            m_fValue = fValue;
        }

        /// <summary>
        /// Значение в начале игры.
        /// </summary>
        public float m_fDefaultValue = 0;

        /// <summary>
        /// Значение в течение игры.
        /// </summary>
        public float m_fValue = 0;

        /// <summary>
        /// Набор диапазонов для отображения пользователю. 
        /// Чисто интерфейсная штука, может быть пустым.
        /// Если текущее значение параметра попадает в один из диапазонов,
        /// название этого диапазона следует писать в интерфейсе игры 
        /// вместо числового значения.
        /// </summary>
        public List<Range> m_cRanges = new List<Range>();
        
        /// <summary>
        /// Минимальное возможное значение
        /// </summary>
        public float m_fMin = 0;

        /// <summary>
        /// Максимальное возможное значение
        /// </summary>
        public float m_fMax = 100;

        public NumericParameter()
        {
        }

        public NumericParameter(NumericParameter pOrigin, bool bClone)
            :base(pOrigin, bClone)
        {
            m_fDefaultValue = pOrigin.m_fDefaultValue;
            m_fMin = pOrigin.m_fMin;
            m_fMax = pOrigin.m_fMax;
            m_sFormat = pOrigin.m_sFormat;

            foreach (var pRange in pOrigin.m_cRanges)
                m_cRanges.Add(new Range(pRange));
        }

        public NumericParameter(string sName, string sGroup)
        {
            m_sName = sName;
            m_sGroup = sGroup;
        }

        public NumericParameter(UniLibXML pXml, XmlNode pParamNode, string sCollection)
            : base(pXml, pParamNode, sCollection)
        {
            pXml.GetFloatAttribute(pParamNode, "value", ref m_fDefaultValue);
            pXml.GetFloatAttribute(pParamNode, "min", ref m_fMin);
            pXml.GetFloatAttribute(pParamNode, "max", ref m_fMax);
            pXml.GetStringAttribute(pParamNode, "format", ref m_sFormat);

            foreach (XmlNode pSubNode in pParamNode.ChildNodes)
            {
                if (pSubNode.Name == "Range")
                {
                    Range pRange = new Range(0);
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
            pXml.AddAttribute(pParamNode, "min", m_fMin);
            pXml.AddAttribute(pParamNode, "max", m_fMax);
            pXml.AddAttribute(pParamNode, "format", m_sFormat);

            foreach (Range pRange in m_cRanges)
            {
                XmlNode pRangeNode = pXml.CreateNode(pParamNode, "Range");
                pXml.AddAttribute(pRangeNode, "name", pRange.m_sDescription);
                pXml.AddAttribute(pRangeNode, "min", pRange.m_fMin);
                pXml.AddAttribute(pRangeNode, "max", pRange.m_fMax);
            }
        }

        public override void Init()
        {
            m_fValue = m_fDefaultValue;
        }
    }
}
