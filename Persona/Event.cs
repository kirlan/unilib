using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Consequences;

namespace Persona
{
    /// <summary>
    /// Игровое событие
    /// </summary>
    class Event
    {
        private static int s_iCounter = 0;

        public int m_iID = s_iCounter++;

        /// <summary>
        /// Область жизни, к которой относится это событие.
        /// </summary>
        public Domain m_pSphere;
        /// <summary>
        /// Множитель вероятности события. При 1 событие имеет шансы выпасть равные все другим событиям 
        /// этой же области жизни, у которых есть хоть одно описание с удовлетворяющимися условиями.
        /// При > 1 шансы увеличиваются в соответсвующее число раз. Т.е. событие как бы дублируется в списке выбора соответствующее число раз.
        /// </summary>
        public int m_iProbability;

        /// <summary>
        /// Список описаний события с условиями. Событие возможно, если у него есть хоть одно описание 
        /// с выполняющимся условием.
        /// </summary>
        public List<Situation> m_cDescriptions = new List<Situation>();

        /// <summary>
        /// Список возможных реакций игрока на событие. Не может быть пустым.
        /// Должен содержать хотя бы одну реакцию, которая доступна всегда.
        /// </summary>
        public List<Reaction> m_cReactions = new List<Reaction>();

        /// <summary>
        /// Список последствий события, которые применяются к текущему состоянию системы после выбора пользователем реакции - 
        /// независимо от того, какая именно реакция была выбрана. Может быть пустым.
        /// </summary>
        public List<Consequence> m_cConsequences = new List<Consequence>();
    }
}
