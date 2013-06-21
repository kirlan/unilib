using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persona
{
    /// <summary>
    /// Привязанный к условию текст описания события - используется для того,
    /// чтобы давать различные описания одного и того же события в зависимости от условия, 
    /// сохраняя при этом все возможные реакции и последствия.
    /// Например - для различного пола персонажа или в зависимости от каких-то предыдущих действий...
    /// </summary>
    class Situation
    {
        /// <summary>
        /// Описание события
        /// </summary>
        public string m_sText = "Описание события";

        /// <summary>
        /// Список условий при выполнении которых следует выводить это описание.
        /// Условия в списке связываются друг с другом логическим И.
        /// </summary>
        public List<Condition> m_cConditions = new List<Condition>();
    }
}
