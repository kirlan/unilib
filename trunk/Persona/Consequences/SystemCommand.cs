using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persona.Consequences
{
    /// <summary>
    /// Возможное последствие разыгранного события - системная команда.
    /// </summary>
    class SystemCommand : Consequence
    {
        public enum ActionType
        { 
            /// <summary>
            /// Вернуться к выбору категории и сделать другой выбор.
            /// </summary>
            Return,
            /// <summary>
            /// Разыграть случайное событие из числа доступных.
            /// </summary>
            RandomRound,
            /// <summary>
            /// Конец игры.
            /// </summary>
            GameOver
        }

        public ActionType m_eAction = ActionType.GameOver;
    }
}
