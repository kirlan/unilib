using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaEngine
{
    /// <summary>
    /// Последовательность сцен, объединённая общим местом действия и составом действующих лиц
    /// </summary>
    class Episode
    {
        private int m_iIndex;

        /// <summary>
        /// Порядковый индекс эпизода в сценарии
        /// </summary>
        public int Index
        {
            get { return m_iIndex; }
            set { m_iIndex = value; }
        }

        private Location m_pLocation;

        /// <summary>
        /// Место действия эпизода
        /// </summary>
        internal Location Location
        {
            get { return m_pLocation; }
        }

        private List<CharacterState> m_cRoles = new List<CharacterState>();
        
        /// <summary>
        /// Список действующих лиц эпизода с указанием их состояния на начало эпизода
        /// </summary>
        public List<CharacterState> Roles
        {
            get { return m_cRoles; }
        }

        private List<Scene> m_cScenes = new List<Scene>();

        /// <summary>
        /// Список сцен
        /// </summary>
        internal List<Scene> Scenes
        {
            get { return m_cScenes; }
        }
    
        /// <summary>
        /// Рейтинг хардкорности эпизода в определённом жанре - вычисляется на основании списка 
        /// тэгов, использованных для описания всех входящих в эпизод сцен.
        /// </summary>
        /// <param name="eGenre"></param>
        /// <returns></returns>
        public int GetRating(GenreTag.Genre eGenre)
        {
            List<GenreTag> cTags = new List<GenreTag>();

            foreach (Scene pScene in m_cScenes)
            {
                foreach (ActionCast pAction in pScene.Actions)
                {
                    foreach (GenreTag pTag in pAction.Action.Tags)
                    {
                        if (!cTags.Contains(pTag))
                            cTags.Add(pTag);
                    }
                }
            }

            int iRating = 0;

            foreach (GenreTag pTag in cTags)
                iRating += pTag.Rating[eGenre];

            return iRating;
        }
    }
}
