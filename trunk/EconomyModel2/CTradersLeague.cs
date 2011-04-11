using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyModel2
{
    public class CTradersLeague
    {
        private List<CTradingCompany> m_cFilials = new List<CTradingCompany>();
        public List<CTradingCompany> Filials
        {
            get
            {
                return m_cFilials;
            }
        }

        //private Dictionary<Goods, List<CTradingRecord>> m_cOffers = new Dictionary<Goods, List<CTradingRecord>>();
        //public Dictionary<Goods, List<CTradingRecord>> Offers
        //{
        //    get
        //    {
        //        return m_cOffers;
        //    }
        //}

        //private Dictionary<Goods, List<CTradingRecord>> m_cDemands = new Dictionary<Goods, List<CTradingRecord>>();
        //public Dictionary<Goods, List<CTradingRecord>> Demands
        //{
        //    get
        //    {
        //        return m_cDemands;
        //    }
        //}

        private List<CTradingRoute> m_cRoutes = new List<CTradingRoute>();
        public List<CTradingRoute> Routes
        {
            get
            {
                return m_cRoutes;
            }
        }

        public void Update()
        {
            AddNewRoute();

            List<CTradingRoute> erase = new List<CTradingRoute>();
            foreach (CTradingRoute route in m_cRoutes)
            {
                if (route.Profit > 0)
                {
                    route.Update();
                }
                else
                {
                    erase.Add(route);
                }
            }
            foreach (CTradingRoute route in erase)
            {
                m_cRoutes.Remove(route);
                route.Kill();
            }
        }

        private class PricesTable
        {
            SortedList<double, List<CTradingRecord>> m_cTable = new SortedList<double, List<CTradingRecord>>();

            public void AddRecord(CTradingRecord record)
            {
                if (!m_cTable.ContainsKey(record.Price))
                    m_cTable[record.Price] = new List<CTradingRecord>();

                m_cTable[record.Price].Add(record);
            }

            private CTradingRecord GetUnsatisfiedRecord(List<CTradingRecord> cRecords)
            {
                foreach (CTradingRecord pRecord in cRecords)
                {
                    if (pRecord.Satisfaction < pRecord.Count)
                        return pRecord;
                }

                return null;
            }

            public CTradingRecord MostExpensive
            {
                get
                {
                    if (m_cTable.Count == 0)
                        return null;

                    int index = m_cTable.Count;
                    CTradingRecord pRecord = null;
                    do
                    {
                        index--;
                        pRecord = GetUnsatisfiedRecord(m_cTable.Values[index]);
                    }
                    while(pRecord == null && index > 0);

                    return pRecord;
                }
            }

            public CTradingRecord Cheapest
            {
                get
                {
                    if (m_cTable.Count == 0)
                        return null;

                    int index = 0;
                    CTradingRecord pRecord = null;
                    do
                    {
                        pRecord = GetUnsatisfiedRecord(m_cTable.Values[index]);
                        index++;
                    }
                    while (pRecord == null && index < m_cTable.Count);

                    return pRecord;
                }
            }
        }

        public bool AddNewRoute()
        {
            //товар, цена, ссылка на объявление
            Dictionary<Goods, PricesTable> cOffers = new Dictionary<Goods, PricesTable>();
            Dictionary<Goods, PricesTable> cDemands = new Dictionary<Goods, PricesTable>();

            foreach (CTradingCompany world in m_cFilials)
            {
                foreach (CTradingRecord record in world.Offers.Values)
                {
                    if (!cOffers.ContainsKey(record.Goods))
                        cOffers[record.Goods] = new PricesTable();

                    cOffers[record.Goods].AddRecord(record);
                }

                foreach (CTradingRecord record in world.Demands.Values)
                {
                    if (!cDemands.ContainsKey(record.Goods))
                        cDemands[record.Goods] = new PricesTable();

                    cDemands[record.Goods].AddRecord(record);
                }
            }

            CTradingRoute pBestRoute = null;
            foreach (Goods goods in cOffers.Keys)
            {
                if (cDemands.ContainsKey(goods) && cOffers[goods].Cheapest != null && cDemands[goods].MostExpensive != null)
                {
                    CTradingRoute pNewRoute = new CTradingRoute(cOffers[goods].Cheapest.Owner, cDemands[goods].MostExpensive.Owner, goods);
                    if (pNewRoute.Profit > 0 && (pBestRoute == null || pBestRoute.Profit < pNewRoute.Profit))
                    {
                        bool bDouble = false;
                        foreach (CTradingRoute pRoute in cOffers[goods].Cheapest.Owner.Spaceport.Routes)
                        {
                            if (pRoute.Byer == cDemands[goods].MostExpensive.Owner && pRoute.Goods == goods)
                                bDouble = true;
                        }
                        pBestRoute = pNewRoute;
                    }
                }
            }

            if (pBestRoute != null)
            {
                m_cRoutes.Add(pBestRoute);
                pBestRoute.Demand.Owner.Spaceport.Routes.Add(pBestRoute);
                pBestRoute.Offer.Owner.Spaceport.Routes.Add(pBestRoute);
                return true;
            }

            return false;
        }
    }
}
