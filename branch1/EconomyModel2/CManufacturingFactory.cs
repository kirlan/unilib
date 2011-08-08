using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyModel2
{
    public class CManufacturingFactory: CFactory
    {
        private string m_sName = "Unknown";
        public CManufacturingFactory(CStock pStock, Goods goods)
            : base(pStock, goods)
        {
            m_sName = string.Format("{0} Factory", Enum.GetName(typeof(Goods), goods));
        }
    
        public override void Produce()
        {
            foreach (Goods goods in m_cProduction.Keys)
            {
                m_pStock.Store(goods, m_cProduction[goods].Amount);
            }
        }

        public override void Consume()
        {
            foreach (Goods goods in GOODS.Resources)
            {
                m_pStock.Consume(goods, m_iLevel);
            }

            foreach (Goods goods in m_cProduction.Keys)
            {
                m_cProduction[goods].Set(m_iLevel * m_iLevel);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", m_sName, m_iLevel);
        }
    }
}
