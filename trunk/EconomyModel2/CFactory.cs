using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomyModel2
{
    public abstract class CFactory
    {
        protected Dictionary<Goods, GoodsCrate> m_cProduction = new Dictionary<Goods, GoodsCrate>();
        public Dictionary<Goods, GoodsCrate> Production
        {
            get
            {
                return m_cProduction;
            }
        }

        protected CStock m_pStock;

        protected int m_iLevel = 0;
        public int Level
        {
            get
            {
                return m_iLevel;
            }
        }

        public abstract void Consume();

        public abstract void Produce();

        public void UpgradeToLevel(int iLevel)
        {
            m_iLevel = iLevel;
        }

        public CFactory(CStock pStock, Goods goods)
        {
            m_pStock = pStock;

            if (goods != Goods.None)
            {
                if (!m_cProduction.ContainsKey(goods))
                    m_cProduction[goods] = new GoodsCrate();
                m_cProduction[goods].Set(1);
            }
        }
    }
}
