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
    /// Возможное последствие разыгранного события - изменение числового параметра на значение другого числового параметра.
    /// </summary>
    public class ParameterChangeVariable : Consequence
    {
        public Parameter m_pParam1;

        public Parameter m_pParam2;

        public NumericParameter.Operation m_eOperation = NumericParameter.Operation.ADD;

        public ParameterChangeVariable(Parameter pParam1, Parameter pParam2, NumericParameter.Operation eOperation)
        {
            m_pParam1 = pParam1;
            m_pParam2 = pParam2;
            m_eOperation = eOperation;
        }

        public ParameterChangeVariable(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams)
        {
            string sParam = "";
            pXml.GetStringAttribute(pParamNode, "param1", ref sParam);
            foreach (Parameter pParam in cParams)
                if (pParam.FullName == sParam)
                {
                    m_pParam1 = pParam;
                    break;
                }

            pXml.GetStringAttribute(pParamNode, "param2", ref sParam);
            foreach (Parameter pParam in cParams)
                if (pParam.FullName == sParam)
                {
                    m_pParam2 = pParam;
                    break;
                }

            bool bIncrease = true;
            if (pXml.GetBoolAttribute(pParamNode, "increase", ref bIncrease))
            {
                m_eOperation = bIncrease ? NumericParameter.Operation.ADD : NumericParameter.Operation.SUB;
            }
            else
            {
                object temp = m_eOperation;
                pXml.GetEnumAttribute(pParamNode, "operation", typeof(NumericParameter.Operation), ref temp);
                m_eOperation = (NumericParameter.Operation)temp;
            }
        }

        internal override void WriteXML(UniLibXML pXml, XmlNode pConsequenceNode)
        {
            pXml.AddAttribute(pConsequenceNode, "param1", m_pParam1.FullName);
            pXml.AddAttribute(pConsequenceNode, "param2", m_pParam2.FullName);
            pXml.AddAttribute(pConsequenceNode, "operation", m_eOperation);
        }

        public override string ToString()
        {
            switch (m_eOperation)
            {
                case NumericParameter.Operation.ADD:
                    return string.Format("{0} += {1}", m_pParam1 != null ? m_pParam1.FullName : "НЕВЕРНЫЙ ПАРАМЕТР", m_pParam2 != null ? m_pParam2.FullName : "НЕВЕРНЫЙ ПАРАМЕТР");
                case NumericParameter.Operation.SUB:
                    return string.Format("{0} -= {1}", m_pParam1 != null ? m_pParam1.FullName : "НЕВЕРНЫЙ ПАРАМЕТР", m_pParam2 != null ? m_pParam2.FullName : "НЕВЕРНЫЙ ПАРАМЕТР");
                case NumericParameter.Operation.SET:
                    return string.Format("{0} := {1}", m_pParam1 != null ? m_pParam1.FullName : "НЕВЕРНЫЙ ПАРАМЕТР", m_pParam2 != null ? m_pParam2.FullName : "НЕВЕРНЫЙ ПАРАМЕТР");
                case NumericParameter.Operation.AVG:
                    return string.Format("{0} -> {1}", m_pParam1 != null ? m_pParam1.FullName : "НЕВЕРНЫЙ ПАРАМЕТР", m_pParam2 != null ? m_pParam2.FullName : "НЕВЕРНЫЙ ПАРАМЕТР");
                default:
                    return string.Format("{0} ?? {1}", m_pParam1 != null ? m_pParam1.FullName : "НЕВЕРНЫЙ ПАРАМЕТР", m_pParam2 != null ? m_pParam2.FullName : "НЕВЕРНЫЙ ПАРАМЕТР");
            }
        }

        public override Consequence Clone()
        {
            ParameterChangeVariable pNew = new ParameterChangeVariable(m_pParam1, m_pParam2, m_eOperation);

            return pNew;
        }

        /// <summary>
        /// Выполнить команду
        /// </summary>
        /// <param name="pModule">не используется, может быть null</param>
        internal override void Apply(Module pModule)
        {
            NumericParameter pParam1 = m_pParam1 as NumericParameter;
            NumericParameter pParam2 = m_pParam2 as NumericParameter;

            if (pParam1 == null || pParam2 == null)
                return;

            pModule.m_sLog.AppendLine("\tDO " + this.ToString());
            pParam1.ChangeValue(pParam2, m_eOperation);
        }
    }
}
