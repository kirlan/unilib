using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaEngine
{
    /// <summary>
    /// Место, где происходит действие. Имеет список доступной актёрам мебели.
    /// </summary>
    class Location
    {
        private string m_sName;

        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        private List<FurnitureTemplate> m_cScenery = new List<FurnitureTemplate>();

        public List<FurnitureTemplate> Scenery
        {
            get { return m_cScenery; }
        }
    }
}
