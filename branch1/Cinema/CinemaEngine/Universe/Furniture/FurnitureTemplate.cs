using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaEngine
{
    /// <summary>
    /// Общая информация об элементе обстановки локации.
    /// </summary>
    public class FurnitureTemplate
    {
        private string m_sName = "New Furniture";

        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        private List<FurnitureAnchor> m_cAnchors = new List<FurnitureAnchor>();

        internal List<FurnitureAnchor> Anchors
        {
            get { return m_cAnchors; }
        }

        public FurnitureTemplate(CharacterState.Pose eType)
        {
            m_cAnchors.Add(new FurnitureAnchor(eType));
        }
    }
}
