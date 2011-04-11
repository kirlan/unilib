using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyModel2
{
    public class CTradingRoute
    {
        private Goods m_eGoods = Goods.None;
        public Goods Goods
        {
            get { return m_eGoods; }
        }

        private CWorld m_pSeller = null;
        public CWorld Seller
        {
            get
            {
                return m_pSeller;
            }
        }

        private CWorld m_pByer = null;
        public CWorld Byer
        {
            get
            {
                return m_pByer;
            }
        }

        public CTradingRoute(CWorld pSeller, CWorld pByer, Goods eGoods)
        {
            m_pSeller = pSeller;
            m_pByer = pByer;
            m_eGoods = eGoods;
        }

        public void Kill()
        {
            m_pSeller.Spaceport.Routes.Remove(this);
            m_pByer.Spaceport.Routes.Remove(this);
        }

        public int Volume
        {
            get
            {
                if(Offer == null || Demand == null)
                    return 0;

                if (Offer.Count > Demand.Count)
                    return Demand.Count;
                else
                    return Offer.Count;
            }
        }

        public double Profit
        {
            get
            {
                if (Volume == 0)
                    return 0;

                return Volume * (Demand.Price - Offer.Price);
            }
        }

        public CTradingRecord Offer
        {
            get
            {
                if (m_pSeller == null || m_eGoods == Goods.None)
                    return null;

                if (!m_pSeller.Spaceport.Offers.ContainsKey(m_eGoods))
                    return null;

                return m_pSeller.Spaceport.Offers[m_eGoods];
            }
        }

        public CTradingRecord Demand
        {
            get
            {
                if (m_pByer == null || m_eGoods == Goods.None)
                    return null;

                if (!m_pByer.Spaceport.Demands.ContainsKey(m_eGoods))
                    return null;

                return m_pByer.Spaceport.Demands[m_eGoods];
            }
        }

        public void Update()
        {
            Offer.Satisfy(Volume);
            Demand.Satisfy(Volume);
        }
    }
}
