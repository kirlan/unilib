using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Consequences;

namespace Persona
{
    /// <summary>
    /// Возможная реакция персонажа на событие.
    /// </summary>
    class Reaction
    {
        /// <summary>
        /// Если true, то реакция будет отображаться в общем списке, даже если условие не выполняется,
        /// просто её нельзя будет выбрать.
        /// </summary>
        public bool m_bAlwaysVisible;

        /// <summary>
        /// Краткое описание реакции, как оно будет выводиться в списке возможных реакций для пользователя.
        /// </summary>
        public string m_sName;
        /// <summary>
        /// Полное описание реакции и её последствий, как оно будет выводиться в игровой лог после того,
        /// как пользователь выбрал эту реакцию.
        /// </summary>
        public string m_sResult;

        /// <summary>
        /// Список условий, при которых эта реакция доступна.
        /// Условия в списке связываются друг с другом логическим И.
        /// При m_bAlwaysVisible==false недоступные реакции не будут отображаться в предоставляемом игроку списке, иначе - будут, но выбрать их всё-равно нельзя.
        /// </summary>
        public List<Condition> m_cConditions = new List<Condition>();

        /// <summary>
        /// Список последствий выбора этой реакции, применяется к текущему состоянию системы после выбора
        /// реакции пользователем.
        /// </summary>
        public List<Consequence> m_cConsequences = new List<Consequence>();
    }
}
