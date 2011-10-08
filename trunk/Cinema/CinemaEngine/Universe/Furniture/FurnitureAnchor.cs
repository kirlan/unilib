using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaEngine
{
    /// <summary>
    /// Способ использования предмета мебели. Определяет, сколько человек могут принять одновременно 
    /// определённую позу с использованием этого предмета.
    /// Возможно несколько альтернативных (взаимоисключающих) вариантов - 
    /// например, на диване могут сесть трое или лечь один...
    /// </summary>
    class FurnitureAnchor
    {
        private Dictionary<CharacterState.Pose, int> m_cPossiblePoses = new Dictionary<CharacterState.Pose, int>();

        public Dictionary<CharacterState.Pose, int> PossiblePoses
        {
            get { return m_cPossiblePoses; }
        }

        public FurnitureAnchor(CharacterState.Pose eType)
        {
            m_cPossiblePoses[eType] = 1;
        }
    }
}
