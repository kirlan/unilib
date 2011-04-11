using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomyModel3
{
    /// <summary>
    /// заказ конкретному производителю на конкретную партию товара, который он производит
    /// </summary>
    public class Demand
    {
        private static long Counter = 0;

        private long m_Id;

        /// <summary>
        /// дентификатор заказа (должен быть уникальным в пределах одного производителя)
        /// </summary>
        public long Id
        {
            get { return m_Id; }
        }

        /// <summary>
        /// базовый конструктор
        /// </summary>
        /// <param name="producer">производитель</param>
        public Demand(Building producer)
        {
            m_Producer = producer;

            m_Id = Counter++;
        }

        /// <summary>
        /// альтернативный конструктор
        /// </summary>
        /// <param name="producer">производитель</param>
        /// <param name="id">идентификатор заказа</param>
        public Demand(Building producer, long id)
        {
            m_Producer = producer;

            m_Id = id;
        }

        Building m_Producer;

        /// <summary>
        /// производитель
        /// </summary>
        public Building Producer
        {
            get { return m_Producer; }
        }
        private int m_Count;

        /// <summary>
        /// размер произведённой партии заказа (вычисляется в процессе разрешения заказа)
        /// </summary>
        public int Count
        {
            get { return m_Count; }
            set { m_Count = value; }
        }
        private int m_Price;

        /// <summary>
        /// цена произведённой партии заказа (вычисляется в процессе разрешения заказа)
        /// </summary>
        public int Price
        {
            get { return m_Price; }
            set { m_Price = value; }
        }
        private List<Query> m_Queries = new List<Query>();

        /// <summary>
        /// порождённые в процессе выполнения заказа потребности производителя (на сырьё)
        /// </summary>
        internal List<Query> Queries
        {
            get { return m_Queries; }
        }

        /// <summary>
        /// проверка, сколько можем произвести и сколько это будет стоить (без стоимости доставки заказчику)
        /// </summary>
        /// <param name="count">сколько нужно</param>
        /// <returns>сколько получилось</returns>
        public int CanResolve(int stillNeeds)
        {
            m_Producer.Unreserve(this);
            //прежде всего, проверим - остались ли ещё неиспользованные производственные мощности
            m_Count = Math.Min(stillNeeds, m_Producer.FreePower);
            m_Price = 0;

            if (m_Count <= 0)
                return 0;

            int canAfford = m_Count;
            foreach (Commodity commodity in m_Producer.Demands.Keys)
            {
                Query query = new Query(m_Producer, commodity, m_Id);
                query.Resolve(m_Producer.Demands[commodity] * m_Count);
                m_Queries.Add(query);

                m_Price += query.Price;

                //проверяем, удалось ли собрать достаточно сырья?
                //если нет - считаем, на сколько единиц продукции хватит того что набрали
                if (query.Count < m_Producer.Demands[commodity] * m_Count)
                {
                    int affordedAmount = query.Count / m_Producer.Demands[commodity];
                    if (affordedAmount < canAfford)
                        canAfford = affordedAmount;
                }
            }

            //если сырья не хватает на производство необходимого количества товара
            if (canAfford != m_Count)
            {
                //пересчитем, сколько будет стоить производство того количества, на которое сырья хватает
                m_Count = canAfford;
                m_Price = 0;
                foreach (Query query in m_Queries)
                {
                    query.Resolve(m_Producer.Demands[query.Commodity] * m_Count);
                    m_Price += query.Price;
                }
            }

            return m_Count;
        }
    }
}
