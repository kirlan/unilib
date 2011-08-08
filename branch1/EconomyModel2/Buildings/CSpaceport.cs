using System;
using System.Collections.Generic;
using System.Text;
using EconomyModel2.Trading;

namespace EconomyModel2.Buildings
{
    /// <summary>
    /// Космопорт - окно в галактику
    /// </summary>
    public class CSpaceport: CBuilding
    {
        /// <summary>
        /// Ссылка на планету, на которой построен
        /// </summary>
        private CWorld m_pHomeWorld = null;

        /// <summary>
        /// Ссылка на планету, на которой построен
        /// </summary>
        public CWorld HomeWorld
        {
            get { return m_pHomeWorld; }
        }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="pWorld">Ссылка на планету, на которой построен</param>
        public CSpaceport(CWorld pWorld)
            : base(pWorld.Stock, Goods.None)
        {
            m_pHomeWorld = pWorld;
        }

        /// <summary>
        /// Список активных торговых маршрутов (импорт и экспорт в одной куче)
        /// </summary>
        private List<CTradingRoute> m_cRoutes = new List<CTradingRoute>();

        /// <summary>
        /// Список активных торговых маршрутов (импорт и экспорт в одной куче)
        /// </summary>
        public List<CTradingRoute> Routes
        {
            get { return m_cRoutes; }
        }

        /// <summary>
        /// Товары, предлагаемые на продажу
        /// </summary>
        private Dictionary<Goods, CTradingRecord> m_cOffers = new Dictionary<Goods, CTradingRecord>();
        /// <summary>
        /// Товары, предлагаемые на продажу
        /// </summary>
        public Dictionary<Goods, CTradingRecord> Offers
        {
            get
            {
                return m_cOffers;
            }
        }

        /// <summary>
        /// Товары, в которых нуждается планета
        /// </summary>
        private Dictionary<Goods, CTradingRecord> m_cDemands = new Dictionary<Goods, CTradingRecord>();
        /// <summary>
        /// Товары, в которых нуждается планета
        /// </summary>
        public Dictionary<Goods, CTradingRecord> Demands
        {
            get
            {
                return m_cDemands;
            }
        }

        /// <summary>
        /// Производство - привезённые с других планет и нераспроданные товары помещаются на склад
        /// </summary>
        public override void Produce()
        {
            if (!m_cProduction.ContainsKey(Goods.Credits))
                m_cProduction[Goods.Credits] = new GoodsCrate();

            m_cProduction[Goods.Credits].Clear();

            //Просмотрим все наши предложения
            foreach (CTradingRecord offer in m_cOffers.Values)
            {
                //Если куплена только часть выставленного на продажу товара, возвращаем остаток на склад
                if (offer.Satisfaction < offer.Count)
                {
                    m_pStock.Store(offer.Goods, offer.Count - offer.Satisfaction);
                }
                //Так же отправляем на склад всю выручку от торговли
                if (offer.Satisfaction > 0)
                {
                    m_cProduction[Goods.Credits].Add((int)(offer.Satisfaction*offer.Price));
                }
            }
            //Теперь займёмся запросами
            foreach (CTradingRecord demand in m_cDemands.Values)
            {
                //Если запрос удовлетворён хотя бы частично
                if (demand.Satisfaction > 0)
                {
                    if (!m_cProduction.ContainsKey(demand.Goods))
                        m_cProduction[demand.Goods] = new GoodsCrate();

                    //Помещаем купленные товары на склад
                    m_cProduction[demand.Goods].Set(demand.Satisfaction);
                    m_pStock.Store(demand.Goods, demand.Satisfaction);

                    //Снимаем со счёта стоимость покупки
                    m_cProduction[Goods.Credits].StrongRemove((int)(demand.Satisfaction * demand.Price));
                }
            }
        }

        /// <summary>
        /// Подготовка к торговле - составляются списки предложений и запросов
        /// </summary>
        public override void Consume()
        {
            //Проверим, в каких товарах у нас зарегистрирована нужда
            foreach (Goods goods in m_pStock.Shortage.Keys)
            {
                //Если этот товар уже есть в списке запросов
                if (m_cDemands.ContainsKey(goods))
                {
                    //m_cDemands[goods].Count = m_pStock.Shortage[goods].Amount;
                    //m_cDemands[goods].Count = (m_cDemands[goods].Count + m_pStock.Shortage[goods].Amount)/2;

                    //Медленно и неторопливо реагируем на изменение объёма запроса - во избежание резких скачков и дребезга
                    if (m_cDemands[goods].Count <= m_pStock.Shortage[goods].Amount)
                        m_cDemands[goods].Count++;
                    else
                        m_cDemands[goods].Count--;

                    //Если поставки стабильные, попробуем снизить закупочную цену
                    if (m_cDemands[goods].Count == m_cDemands[goods].Satisfaction)
                    {
                        m_cDemands[goods].Price *= 0.99;
                    }
                    //Если объём поставок недостаточен - придётся поднять закупочную цену
                    if (m_cDemands[goods].Count > m_cDemands[goods].Satisfaction)
                    {
                        m_cDemands[goods].Price *= 1.01;
                    }
                }
                else
                {
                    //Этого товара раньше ещё не было в списке запросов. Добавляем новый запрос с базовой ценой
                    m_cDemands[goods] = new CTradingRecord(goods);
                    m_cDemands[goods].Count = m_pStock.Shortage[goods].Amount;
                    m_cDemands[goods].Price = 100;
                }
                //Подтверждаем выставление запроса
                m_cDemands[goods].Declare(m_pHomeWorld);
            }

            //Ищем запросы, которые были, но сейчас потребности в этих товарах уже нет
            List<Goods> erase = new List<Goods>();
            foreach (Goods goods in m_cDemands.Keys)
            {
                //Если исчезла потребность - не будем сразу рубить с плеча,а сначала просто снизим объёмы закупок
                if (!m_pStock.Shortage.ContainsKey(goods))
                {
                    m_cDemands[goods].Count--;
                    m_cDemands[goods].Declare(m_pHomeWorld);
                }
                //Если объёмы закупок упали до 0, только тогда удаляем этот запрос из списка вообще
                if (m_cDemands[goods].Count == 0)
                    erase.Add(goods);
            }
            foreach (Goods goods in erase)
            {
                m_cDemands.Remove(goods);
            }

            //Ищем предложения, которые были, но снейчас этих товаров на складе нет и удаляем такие предложения из списка
            erase.Clear();
            foreach (Goods goods in m_cOffers.Keys)
            {
                if (m_pStock.Stored(goods) <= 0)
                    erase.Add(goods);
            }
            foreach (Goods goods in erase)
            {
                m_cOffers.Remove(goods);
            }

            //Перебираем все товры, имеющиеся на складе
            foreach (Goods goods in m_pStock.Goods)
            {
                //Если это что-то, чем можно торговать
                if (GOODS.TradedGoods.Contains(goods))
                {
                    //Если мы сами не импортируем этот товар
                    if (!m_cDemands.ContainsKey(goods))
                    {
                        //Если этот товар уже присутсвует в списке предложений
                        if (m_cOffers.ContainsKey(goods))
                        {
                            //Выгребаем со склада всё что есть и выставляем на продажу
                            m_cOffers[goods].Count = m_pStock.Consume(goods);

                            //Если у нас покупают меньше, чем мы можем предложить - снижаем цену.
                            if (m_cOffers[goods].Count > m_cOffers[goods].Satisfaction)
                            {
                                m_cOffers[goods].Price *= 0.99;
                            }

                            //Если у нас готовы купить больше, чем мы можем предложить - поднимаем цену
                            if (m_cOffers[goods].Count < m_cOffers[goods].Satisfaction)
                            {
                                m_cOffers[goods].Price *= 1.01;
                            }
                        }
                        else
                        {
                            //Этого товара раньше не было в списке предложений. Добавляем его туда с базовой ценой.
                            m_cOffers[goods] = new CTradingRecord(goods);
                            m_cOffers[goods].Count = m_pStock.Consume(goods);
                            m_cOffers[goods].Price = 100;
                        }
                        //Подтверждаем выставление товара на продажу.
                        m_cOffers[goods].Declare(m_pHomeWorld);
                    }
                    else
                    {
                        //Если в списке предложений как-то оказался товар, который мы сами импортирем - удалим его из списка
                        if (m_cOffers.ContainsKey(goods))
                        {
                            m_cOffers.Remove(goods);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Строковое представление объекта
        /// </summary>
        /// <returns>У космопорта нет уровня, поэтому просто имя постройки</returns>
        public override string ToString()
        {
            return string.Format("Spaceport");
        }
    }
}
