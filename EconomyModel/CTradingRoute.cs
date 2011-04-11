using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleVectors;

namespace EconomyModel
{
    public class CTradingRoute
    {
        public Goods m_eGoods = Goods.None;

        public CWorld m_pSeller = null;
        public CWorld m_pByer = null;

        public CTradingRoute(CWorld pSeller, CWorld pByer, Goods eGoods)
        {
            m_pSeller = pSeller;
            m_pByer = pByer;
            m_eGoods = eGoods;
        }

        public int Volume
        {
            get 
            { 
                int iAmount = (int)(m_pByer.Wealth / m_pByer.m_pStock[m_eGoods].m_fPrice);
                if (iAmount > m_pSeller.m_pStock[m_eGoods].m_iStore)
                    iAmount = m_pSeller.m_pStock[m_eGoods].m_iStore;

                if (iAmount > m_pByer.m_pConsumption[m_eGoods])
                    iAmount = m_pByer.m_pConsumption[m_eGoods];

                return iAmount;
            }
        }

        public double Profit
        {
            get
            {
                double fProfit = Volume * CMarket.GetProfit(m_pSeller, m_pByer, m_eGoods);
                SimpleVector3d v1 = new SimpleVector3d(m_pSeller.m_iX, m_pSeller.m_iY, 0);
                SimpleVector3d v2 = new SimpleVector3d(m_pByer.m_iX, m_pByer.m_iY, 0);
                double dist = !(v1 - v2);

                return fProfit - dist / 100;
            }
        }
    }
}
