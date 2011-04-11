using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaEngine
{
    /// <summary>
    /// Описывает состояние элемента обстановки локации в определённый момент времени.
    /// </summary>
    public class Furniture
    {
        private FurnitureTemplate m_pTemplate;

        public FurnitureTemplate Template
        {
          get { return m_pTemplate; }
        }

        private List<CharacterState> m_cCharacters = new List<CharacterState>();

        internal List<CharacterState> Characters
        {
            get { return m_cCharacters; }
            set { m_cCharacters = value; }
        }
    }
}
