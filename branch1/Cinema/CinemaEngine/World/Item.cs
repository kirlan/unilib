using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaEngine
{
    public class Item
    {
        private string m_sName;

        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        private List<EquipmentSlot> m_cCouldBeEquipped = new List<EquipmentSlot>();

        public List<EquipmentSlot> CouldBeEquipped
        {
            get { return m_cCouldBeEquipped; }
        }
    }
}
