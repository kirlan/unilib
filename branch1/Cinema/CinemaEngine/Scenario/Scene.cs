using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace CinemaEngine
{
    /// <summary>
    /// Одно или несколько действий, происходящих (одновременно) в определённый промежуток времени
    /// </summary>
    class Scene
    {
        private DateTime m_iTimeStamp;

        public DateTime TimeStamp
        {
            get { return m_iTimeStamp; }
            set { m_iTimeStamp = value; }
        }

        private DateTime m_iLength;

        public DateTime Length
        {
            get { return m_iLength; }
            set { m_iLength = value; }
        }

        /// <summary>
        /// Состояние всех действующих лиц в начале сцены.
        /// Должно быть передано в сцену извне при создании сцены.
        /// </summary>
        private List<CharacterState> m_cRoles = new List<CharacterState>();

        public List<CharacterState> Roles
        {
            get { return m_cRoles; }
        }

        /// <summary>
        /// Состояние всех доступных элементов мебели в начале сцены.
        /// Вычисляется внутри сцены на основании состояния действующих лиц.
        /// </summary>
        private List<Furniture> m_cFurniture = new List<Furniture>();

        internal ReadOnlyCollection<Furniture> Furniture
        {
            get { return new ReadOnlyCollection<Furniture>(m_cFurniture); }
        }

        private List<ActionCast> m_cActions = new List<ActionCast>();

        public List<ActionCast> Actions
        {
            get { return m_cActions; }
        }

        public int GetRating(GenreTag.Genre eGenre)
        {
            List<GenreTag> cTags = new List<GenreTag>();

            foreach (ActionCast pAction in m_cActions)
            {
                foreach (GenreTag pTag in pAction.Action.Tags)
                {
                    if (!cTags.Contains(pTag))
                        cTags.Add(pTag);
                }
            }

            int iRating = 0;

            foreach (GenreTag pTag in cTags)
                iRating += pTag.Rating[eGenre];

            return iRating;
        }
    }
}
