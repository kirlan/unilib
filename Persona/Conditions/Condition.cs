using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;

namespace Persona
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

        public Condition(Parameter pParam1)
        {
            m_pParam1 = pParam1;
        }
    }
}
