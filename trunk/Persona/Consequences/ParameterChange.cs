using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persona.Consequences
{
    /// <summary>
    /// Возможное последствие разыгранного события - изменение числового параметра на указанную величину.
    /// </summary>
    class ParameterChange : Consequence
    {
        public Parameter m_pParam;

        public string m_sDelta;
    }
}
