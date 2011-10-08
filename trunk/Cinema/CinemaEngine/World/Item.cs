using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaEngine
{
    /// <summary>
    /// Реквизит - одежда или какой-то небольшой предмет, который можно держать в руках
    /// </summary>
    public class Item
    {
        private string m_sName;

        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        private List<EquipmentSlot> m_cCouldBeEquipped = new List<EquipmentSlot>();
        /// <summary>
        /// Если этот предмет может быть надет на персонажа (одежда или аксессуар),
        /// то каким именно образом
        /// </summary>
        public List<EquipmentSlot> CouldBeEquipped
        {
            get { return m_cCouldBeEquipped; }
        }
    }
}
