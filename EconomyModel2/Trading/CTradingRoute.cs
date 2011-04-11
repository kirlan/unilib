using System;
using System.Collections.Generic;
using System.Text;
using SimpleVectors;

namespace EconomyModel2.Trading
{
    /// <summary>
    /// Торговый маршрут
    /// </summary>
    public class CTradingRoute
    {
        /// <summary>
        /// Товар - предмет торговли
        /// </summary>
        private Goods m_eGoods = Goods.None;
        /// <summary>
        /// Товар - предмет торговли
        /// </summary>
        public Goods Goods
        {
            get { return m_eGoods; }
        }

        /// <summary>
        /// Планета - продавец
        /// </summary>
        private CWorld m_pSeller = null;
        /// <summary>
        /// Планета - продавец
        /// </summary>
        public CWorld Seller
        {
            get
            {
                return m_pSeller;
            }
        }

        /// <summary>
        /// Планета - покупатель
        /// </summary>
        private CWorld m_pByer = null;
        /// <summary>
        /// Планета - покупатель
        /// </summary>
        public CWorld Byer
        {
            get
            {
                return m_pByer;
            }
        }

        /// <summary>
        /// Длина маршрута - дистанция между продавцом и покупателем
        /// </summary>
        private double m_fDistance = 0;

        /// <summary>
        /// Длина маршрута - дистанция между продавцом и покупателем
        /// </summary>
        public double Distance
        {
            get { return m_fDistance; }
        }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="pSeller">Планета - продавец</param>
        /// <param name="pByer">Планета - покупатель</param>
        /// <param name="eGoods">Товар - предмет торговли</param>
        public CTradingRoute(CWorld pSeller, CWorld pByer, Goods eGoods)
        {
            m_pSeller = pSeller;
            m_pByer = pByer;
            m_eGoods = eGoods;

            //Посчитаем расстояние
            SimpleVector3d v1 = new SimpleVector3d(pSeller.X, pSeller.Y, 0);
            SimpleVector3d v2 = new SimpleVector3d(pByer.X, pByer.Y, 0);
            m_fDistance = !(v1-v2);

            //Сформируем строку - имя маршрута (откуда, куда, расстояние, что везём, сколько, насколько это выгодно)
            m_sName = string.Format("{4}-{5}({3} pk.) {0} :  {1} / {2}$", Enum.GetName(typeof(Goods), m_eGoods), Volume, (int)Profit, (int)m_fDistance, m_pSeller.Name, m_pByer.Name);
        }

        /// <summary>
        /// Удалить сведенья о маршруте у покупателя и продавца
        /// </summary>
        public void Kill()
        {
            m_pSeller.Spaceport.Routes.Remove(this);
            m_pByer.Spaceport.Routes.Remove(this);
        }

        /// <summary>
        /// Количество реально перевозимого товара.
        /// Определяется объёмами спроса и предложения.
        /// </summary>
        public int Volume
        {
            get
            {
                //При отсутствии спроса или предложения - объём торговли понятное дело 0
                if(Offer == null || Demand == null)
                    return 0;

                if (Offer.Count - Offer.Satisfaction > Demand.Count - Demand.Satisfaction)
                    return Demand.Count - Demand.Satisfaction;
                else
                    return Offer.Count - Offer.Satisfaction;
            }
        }

        /// <summary>
        /// Временный параметр - жалованье пилотам торговых транспортов за каждый парсек.
        /// </summary>
        private double m_fSalary = 5;

        /// <summary>
        /// Временный параметр - жалованье пилотам торговых транспортов за каждый парсек.
        /// </summary>
        public double Salary
        {
            get { return m_fSalary; }
            set { m_fSalary = value; }
        }

        /// <summary>
        /// Прибыльность маршрута. Рассчитывается как объём торговли умноженный на разницу цен минус 
        /// жалованье экипажам транспортов умноженное на дальность перелёта
        /// </summary>
        public double Profit
        {
            get
            {
                if (Volume <= 0)
                    return 0;

                double fTradeProfit = Volume * (Demand.Price - Offer.Price);

                return fTradeProfit - m_fDistance * m_fSalary;
            }
        }

        /// <summary>
        /// Предложение - информация о продаваемом товаре. Null если планета-продавец не продаёт профильный для маршрута товар.
        /// </summary>
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

        /// <summary>
        /// Спрос - информация о покупаемом товаре. Null если планета-покупатель не покупает профильный для маршрута товар.
        /// </summary>
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

        /// <summary>
        /// Имя маршрута
        /// </summary>
        private string m_sName = "";

        /// <summary>
        /// Строковое представление объекта
        /// </summary>
        /// <returns>Имя маршрута</returns>
        public override string ToString()
        {
            return m_sName;
        }

        /// <summary>
        /// Осуществить перевозку товаров по маршруту
        /// </summary>
        public void Update()
        {
            //Обновить информацию о прибыльности маршрута в имени
            m_sName = string.Format("{4}-{5}({3} pk.) {0} :  {1} / {2}$", Enum.GetName(typeof(Goods), m_eGoods), Volume, (int)Profit, (int)m_fDistance, m_pSeller.Name, m_pByer.Name);

            //Объём первозки - запоминаем его в отдельной переменной, т.к. проперти Volume обсчитывается динамически и изменится
            //сразу же как только мы загрузим товар на планете - продавце.
            int iVolume = Volume;
            Offer.Satisfy(iVolume);
            Demand.Satisfy(iVolume);
        }
    }
}
