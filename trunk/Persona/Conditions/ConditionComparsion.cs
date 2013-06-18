using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persona.Conditions
{
    /// <summary>
    /// Условие - сравнение числового параметра со значением другого параметра.
    /// </summary>
    class ConditionComparsion
    {
        public enum ComparsionType
        {
            /// <summary>
            /// много меньше
            /// </summary>
            LOLO,
            /// <summary>
            /// меньше
            /// </summary>
            LO,
            /// <summary>
            /// равно
            /// </summary>
            EQ,
            /// <summary>
            /// больше
            /// </summary>
            HI,
            /// <summary>
            /// много больше
            /// </summary>
            HIHI
        }
        
        /// <summary>
        /// Второй параметр для сравнения.
        /// </summary>
        public Parameter m_pParam2;

        /// <summary>
        /// Требуемое соотношение значений первого и второго параметров.
        /// </summary>
        public ComparsionType m_eType;
    }
}
