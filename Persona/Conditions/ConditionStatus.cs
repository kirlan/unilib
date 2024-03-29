﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;
using nsUniLibXML;
using System.Xml;

namespace Persona.Conditions
{
    /// <summary>
    /// Условие - проверка истинности булевского параметра.
    /// </summary>
    public class ConditionStatus: Condition
    {
        public ConditionStatus(Parameter pParam)
            : base(pParam)
        { 
        }

        public ConditionStatus(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams)
            : base(pXml, pParamNode, cParams)
        {
        }

        internal override void WriteXML(UniLibXML pXml, XmlNode pConditionNode)
        {
            base.WriteXML(pXml, pConditionNode);
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", m_bNot ? "НЕ " : "", m_pParam1 != null ? m_pParam1.FullName : "НЕВЕРНЫЙ ПАРАМЕТР");
        }

        public override Condition Clone()
        {
            ConditionStatus pNew = new ConditionStatus(m_pParam1);
            pNew.m_bNot = m_bNot;

            return pNew;
        }

        public override bool Check()
        {
            bool bValue = (m_pParam1 as BoolParameter).m_bValue;

            return m_bNot ? !bValue : bValue;
        }
    }
}
