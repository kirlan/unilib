using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persona.Conditions
{
    /// <summary>
    /// Условие - проверка нахождения значения указанного числового параметра в заданном диапазоне
    /// </summary>
    class ConditionRange: Condition
    {
        /// <summary>
        /// Нижняя граница диапазона (включительно)
        /// </summary>
        public float m_fMinValue;

        /// <summary>
        /// Верхняя граница диапазона (включительно)
        /// </summary>
        public float m_fMaxValue;
    }
}
