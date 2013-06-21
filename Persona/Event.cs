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
        /// <summary>
        /// Идентификатор события. Используется только при редактировании модуля, для навигации по списку событий.
        /// </summary>
        public string m_sID = "Новое событие";

        /// <summary>
        /// Область жизни, к которой относится это событие.
        /// </summary>
        public Domain m_pDomain;

        /// <summary>
        /// <para>Множитель вероятности события.</para>
        /// <para>При значении 1 (по умолчанию) событие имеет шансы выпасть равные все другим событиям 
        /// этой же области жизни, у которых есть хоть одно описание с удовлетворяющимися условиями.</para>
        /// <para>При значении меньше или равно 0 - событие не случится никогда, можно использовать 
        /// при тестировании модуля для временного отключения определённых событий.</para>
        /// <para>При значении больше 1 шансы увеличиваются в соответствующее число раз. Т.е. событие как бы дублируется 
        /// в списке выбора соответствующее число раз.</para>
        /// </summary>
        public int m_iProbability = 1;

        /// <summary>
        /// <para>Повторяемость события. </para>
        /// <para>Если true, то событие может происходить неограниченное количество раз, 
        /// иначе - только 1 раз за игровую сессию. Если false и <see cref="m_iProbability"/> > 1, то событие может 
        /// произойти <see cref="m_iProbability"/> раз, при чём с каждым разом его вероятность будет снижаться.</para>
        /// </summary>
        public bool m_bRepeatable = false;

        /// <summary>
        /// <para>Приоритет события. </para>
        /// <para>Событие с более низким приоритетом может произойти только если нет 
        /// ни одного возможного события с более высоким приоритетом, независимо от категории события.</para>
        /// </summary>
        public int m_iPriority = 1;

        /// <summary>
        /// Подробное описание события и основной список условий возможности события. 
        /// Описание используется в случае, если в <see cref="m_cAlternateDescriptions"/> нет ничего более подходящего.
        /// </summary>
        public Situation m_pDescription = new Situation();

        /// <summary>
        /// Список альтернативных описаний события с условиями. 
        /// Условия из списка дополняют, а не заменяют основной список условий из <see cref="m_pDescription"/>
        /// </summary>
        public List<Situation> m_cAlternateDescriptions = new List<Situation>();

        /// <summary>
        /// Список возможных реакций пользователя на событие. Не может быть пустым.
        /// Должен содержать хотя бы одну реакцию, которая доступна всегда.
        /// </summary>
        public List<Reaction> m_cReactions = new List<Reaction>();

        /// <summary>
        /// Список последствий события, которые применяются к текущему состоянию системы после выбора пользователем реакции - 
        /// независимо от того, какая именно реакция была выбрана. Может быть пустым.
        /// </summary>
        public List<Consequence> m_cConsequences = new List<Consequence>();

        public Event(Domain pDomain)
        {
            m_pDomain = pDomain;
        }
    }
}
