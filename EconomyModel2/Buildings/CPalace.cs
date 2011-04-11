using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EconomyModel2.Trading;

namespace EconomyModel2.Buildings
{
    class CPalace: CBuilding
    {
        public CPalace(CStock pStock)
            : base(pStock, Commodity.Credits)
        { 
        }

        private int m_iWorkers;

        public override void Consume()
        {
            m_iWorkers = m_pStock.Consume(Commodity.Workers);

            m_cProduction[Commodity.Credits].Set((int)(m_iWorkers * Math.Sqrt(m_iWorkers) * 150));

            foreach (Commodity commodity in CommodityCategorizer.Luxury)
            {
                m_pStock.Consume(commodity, m_iWorkers);
            }
        }

        public override void Produce()
        {
            foreach (Commodity commodity in m_cProduction.Keys)
            {
                m_pStock.Store(commodity, m_cProduction[commodity].Amount);
            }
            m_pStock.Store(Commodity.Workers, m_iWorkers);
        }

        public override string ToString()
        {
            return "Palace";
        }
    }
}
