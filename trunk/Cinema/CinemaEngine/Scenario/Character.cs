using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaEngine
{
    /// <summary>
    /// Персонаж
    /// </summary>
    public class Character
    {
        private string m_sName;

        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        private Actor.Gender m_eGender;

        public Actor.Gender Gender
        {
            get { return m_eGender; }
            set { m_eGender = value; }
        }

        private Dictionary<Actor.Gender, int> m_cTemper = new Dictionary<Actor.Gender, int>();

        public Dictionary<Actor.Gender, int> Temper
        {
            get { return m_cTemper; }
        }

        private Actor m_pPerformer;

        public Actor Performer
        {
            get { return m_pPerformer; }
            set { m_pPerformer = value; }
        }
    }
}
