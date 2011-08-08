using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomyModel3
{
    public abstract class Building
    {
        public Building(Commodity production, int power)
        {
            m_Production = production;
            m_Power = power;
        }

        protected Dictionary<Commodity, int> m_Demands = new Dictionary<Commodity, int>();
        /// <summary>
        /// расход ресурсов на единицу продукции
        /// </summary>
        public Dictionary<Commodity, int> Demands
        {
            get { return m_Demands; }
        }
        protected int m_Cash;

        private Commodity m_Production;

        public Commodity Production
        {
            get { return m_Production; }
        }

        protected int m_Power;
        protected int m_Used;

        /// <summary>
        /// свободные производственные мощности
        /// </summary>
        public int FreePower
        {
            get { return m_Power - m_Used; }
        }

        protected List<Demand> m_Reserved = new List<Demand>();

        /// <summary>
        /// резервирование производства
        /// </summary>
        /// <param name="demand">заказ на производство</param>
        internal void Reserve(Demand demand)
        {
            if (m_Reserved.Contains(demand))
                return;

            m_Reserved.Add(demand);
            m_Used += demand.Count;
        }

        /// <summary>
        /// отмена резервирования производства
        /// </summary>
        /// <param name="demand">заказ на производство</param>
        internal void Unreserve(Demand demand)
        {
            if (!m_Reserved.Contains(demand))
                return;

            m_Reserved.Remove(demand);
            m_Used -= demand.Count;
        }
    }
}
