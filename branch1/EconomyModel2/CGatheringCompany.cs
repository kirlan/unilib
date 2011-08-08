using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyModel2
{
    public class CGatheringCompany: CFactory
    {
        private string m_sName = "Unknown";

        public CGatheringCompany(CStock pStock, Goods goods)
            : base(pStock, goods)
        {
            switch (goods)
            {
                case Goods.Ore:
                    m_sName = "Ore Mine";
                    break;
                //case Goods.Oil:
                //    m_sName = "Oil Derrick";
                //    break;
                //case Goods.Food:
                //    m_sName = "Grain Field";
                //    break;
            }
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
