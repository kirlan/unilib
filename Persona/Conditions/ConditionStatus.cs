using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;

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
        public override string ToString()
        {
            return string.Format("{0}{1}", m_bNot ? "НЕ " : "", m_pParam1 != null ? m_pParam1.m_sName : "НЕВЕРНЫЙ ПАРАМЕТР");
        }
    }
}
