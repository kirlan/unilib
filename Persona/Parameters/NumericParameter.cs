using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public float m_fDefaultValue;

        /// <summary>
        /// Набор диапазонов для отображения пользователю. 
        /// Чисто интерфейсная штука, может быть пустым.
        /// Если текущее значение параметра попадает в один из диапазонов,
        /// название этого диапазона следует писать в интерфейсе игры 
        /// вместо числового значения.
        /// </summary>
        public List<Range> m_cRanges = new List<Range>();
    }
}
