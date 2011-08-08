using System;
using System.Collections.Generic;
using System.Text;
using EconomyModel2.Buildings;

namespace EconomyModel2.Trading
{
    /// <summary>
    /// Лига Торговцев - организация, которая держит в своих руках всю межзвёздную торговлю в галактике
    /// </summary>
    public class CTradersLeague
    {
        /// <summary>
        /// Список известных Лиге космопортов
        /// </summary>
        private List<CSpaceport> m_cFilials = new List<CSpaceport>();
        /// <summary>
        /// Список известных Лиге космопортов
        /// </summary>
        public List<CSpaceport> Filials
        {
            get
            {
                return m_cFilials;
            }
        }

        /// <summary>
        /// Список торговых маршрутов
        /// </summary>
        private List<CTradingRoute> m_cRoutes = new List<CTradingRoute>();
        /// <summary>
        /// Список торговых маршрутов
        /// </summary>
        public List<CTradingRoute> Routes
        {
            get
            {
                return m_cRoutes;
            }
        }

        /// <summary>
        /// Осуществляем торговлю
        /// </summary>
        public void Update()
        {
            //Перебираем все маршруты в списке
            List<CTradingRoute> erase = new List<CTradingRoute>();
            foreach (CTradingRoute route in m_cRoutes)
            {
                //Если маршрут приносит прибыль - осуществляем торговлю, иначе закрываем маршрут
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

            AddNewRoute();
        }

        /// <summary>
        /// Сводная таблица цен по галактике на определённый товар
        /// </summary>
        private class PricesTable
        {
            /// <summary>
            /// Таблица цена - товар
            /// </summary>
            SortedList<double, List<CTradingRecord>> m_cTable = new SortedList<double, List<CTradingRecord>>();

            /// <summary>
            /// Добавить информацию о товаре в таблицу
            /// </summary>
            /// <param name="record">информация о товаре</param>
            public void AddRecord(CTradingRecord record)
            {
                if (!m_cTable.ContainsKey(record.Price))
                    m_cTable[record.Price] = new List<CTradingRecord>();

                m_cTable[record.Price].Add(record);
            }

            /// <summary>
            /// Найти первую неудовлетворённую сделку в списке
            /// </summary>
            /// <param name="cRecords">список сделок</param>
            /// <returns>найденная сделка или null, если ничего не найдено</returns>
            private CTradingRecord GetUnsatisfiedRecord(List<CTradingRecord> cRecords)
            {
                foreach (CTradingRecord pRecord in cRecords)
                {
                    if (pRecord.Satisfaction < pRecord.Count)
                        return pRecord;
                }

                return null;
            }

            /// <summary>
            /// Найти неудовлетворённую сделку с самой высокой ценой
            /// </summary>
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

            /// <summary>
            /// Найти неудовлетворённую сделку с самой низкой ценой
            /// </summary>
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

        /// <summary>
        /// Добавить новый торговый маршрут
        /// </summary>
        /// <returns>удалось или нет</returns>
        public bool AddNewRoute()
        {
            Dictionary<Goods, PricesTable> cOffers = new Dictionary<Goods, PricesTable>();
            Dictionary<Goods, PricesTable> cDemands = new Dictionary<Goods, PricesTable>();

            //Заполняем таблицы цен на товары - предложения и запросы по всей галактике
            foreach (CSpaceport world in m_cFilials)
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

            //Ищем наиболее выгодный маршрут
            CTradingRoute pBestRoute = null;
            //Перебираем все товары, которые хоть кто-то хоть где-то продаёт
            foreach (Goods goods in cOffers.Keys)
            {
                //Если этот товар хоть кому-то нужен и имеется реальные (т.е. до сих пор не удовлетворённые) предложение и спрос
                if (cDemands.ContainsKey(goods) && cOffers[goods].Cheapest != null && cDemands[goods].MostExpensive != null)
                {
                    //Создаём маршрут
                    CTradingRoute pNewRoute = new CTradingRoute(cOffers[goods].Cheapest.Owner, cDemands[goods].MostExpensive.Owner, goods);
                    //Если маршрут приносит прибыль более высокую чем самый прибыльный из рассмотренных до него маршрутов
                    if (pNewRoute.Profit > 0 && (pBestRoute == null || pBestRoute.Profit < pNewRoute.Profit))
                    {
                        //Проверим, а нет ли у нас уже точно такого же маршрута?
                        bool bDouble = false;
                        foreach (CTradingRoute pRoute in cOffers[goods].Cheapest.Owner.Spaceport.Routes)
                        {
                            if (pRoute.Byer == cDemands[goods].MostExpensive.Owner && pRoute.Goods == goods)
                                bDouble = true;
                        }
                        //Если дубликатов нет, то эапомним этот маршрут как самый прибыльный
                        if(!bDouble)
                            pBestRoute = pNewRoute;
                    }
                }
            }

            //Если нашли самый прибыльный маршрут, запускаем его
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
