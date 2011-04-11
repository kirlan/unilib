using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyModel2
{
    public class CTradingCompany: CFactory
    {
        private CWorld m_pHomeWorld = null;

        public CWorld HomeWorld
        {
            get { return m_pHomeWorld; }
        }

        public CTradingCompany(CStock pStock, CWorld pWorld)
            : base(pStock, Goods.None)
        {
            m_pHomeWorld = pWorld;
        }

        private List<CTradingRoute> m_cRoutes = new List<CTradingRoute>();

        public List<CTradingRoute> Routes
        {
            get { return m_cRoutes; }
        }

        private Dictionary<Goods, CTradingRecord> m_cOffers = new Dictionary<Goods, CTradingRecord>();
        public Dictionary<Goods, CTradingRecord> Offers
        {
            get
            {
                return m_cOffers;
            }
        }

        private Dictionary<Goods, CTradingRecord> m_cDemands = new Dictionary<Goods, CTradingRecord>();
        public Dictionary<Goods, CTradingRecord> Demands
        {
            get
            {
                return m_cDemands;
            }
        }

        public override void Produce()
        {
            foreach (CTradingRecord offer in m_cOffers.Values)
            {
                if (offer.Satisfaction < offer.Count)
                {
                    m_pStock.Store(offer.Goods, offer.Count - offer.Satisfaction);
                }
                //offer.Count = 0;
            }
            foreach (CTradingRecord demand in m_cDemands.Values)
            {
                if (demand.Satisfaction > 0)
                {
                    if (!m_cProduction.ContainsKey(demand.Goods))
                        m_cProduction[demand.Goods] = new GoodsCrate();
                    m_cProduction[demand.Goods].Set(demand.Satisfaction);
                    
                    m_pStock.Store(demand.Goods, demand.Satisfaction);
                }
                //demand.Count = 0;
            }
        }

        public override void Consume()
        {
            foreach (Goods goods in m_pStock.Goods)
            {
                if (m_cOffers.ContainsKey(goods))
                {
                    m_cOffers[goods].Count = m_pStock.Consume(goods);
                    if (m_cOffers[goods].Count > m_cOffers[goods].Satisfaction)
                    {
                        m_cOffers[goods].Price *= 0.99;
                    }
                    if (m_cOffers[goods].Count < m_cOffers[goods].Satisfaction)
                    {
                        m_cOffers[goods].Price *= 1.01;
                    }
                }
                else
                {
                    m_cOffers[goods] = new CTradingRecord(goods);
                    m_cOffers[goods].Count = m_pStock.Consume(goods);
                    m_cOffers[goods].Price = 100;
                }
                m_cOffers[goods].Declare(m_pHomeWorld);
            }

            foreach (Goods goods in m_pStock.Shortage.Keys)
            {
                if (m_cDemands.ContainsKey(goods))
                {
                    m_cDemands[goods].Count = m_pStock.Shortage[goods].Amount;
                    if (m_cDemands[goods].Count == m_cDemands[goods].Satisfaction)
                    {
                        m_cDemands[goods].Price *= 0.99;
                    }
                    if (m_cDemands[goods].Count > m_cDemands[goods].Satisfaction)
                    {
                        m_cDemands[goods].Price *= 1.01;
                    }
                }
                else
                {
                    m_cDemands[goods] = new CTradingRecord(goods);
                    m_cDemands[goods].Count = m_pStock.Shortage[goods].Amount;
                    m_cDemands[goods].Price = 100;

                    //m_pStock.Shortage[goods].Clear();
                }
                m_cDemands[goods].Declare(m_pHomeWorld);
            }

            List<Goods> erase = new List<Goods>();
            foreach (Goods goods in m_cDemands.Keys)
            {
                if (!m_pStock.Shortage.ContainsKey(goods) || m_cDemands[goods].Count == 0)
                    erase.Add(goods);
            }
            foreach (Goods goods in erase)
            {
                m_cDemands.Remove(goods);
            }
        }

        public override string ToString()
        {
            return string.Format("Spaceport");
        }
    }
}
