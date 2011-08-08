using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomyModel3
{
    /// <summary>
    /// общая потребность в каком-то товаре... при разрешении разбивается на совокупность заказов конкретным производителям
    /// </summary>
    public class Query
    {
        private Building m_Buyer;

        /// <summary>
        /// потребитель
        /// </summary>
        public Building Buyer
        {
            get { return m_Buyer; }
        }

        private Commodity m_Commodity;

        /// <summary>
        /// необходимый товар
        /// </summary>
        public Commodity Commodity
        {
            get { return m_Commodity; }
        }

        private long m_Id;

        /// <summary>
        /// базовый конструктор
        /// </summary>
        /// <param name="buyer">потребитель</param>
        /// <param name="commodity">товар</param>
        /// <param name="id">ID цепочки заказов</param>
        public Query(Building buyer, Commodity commodity, long id)
        {
            m_Buyer = buyer;
            m_Commodity = commodity;
            m_Id = id;
        }

        private int m_Count;
        /// <summary>
        /// размер партии товара (вычисляется при разрешении потребности)
        /// </summary>
        public int Count
        {
            get { return m_Count; }
        }
        private int m_Price;

        /// <summary>
        /// цена партии товара (вычисляется при разрешении потребности)
        /// </summary>
        public int Price
        {
            get { return m_Price; }
        }
        private Dictionary<long, Demand> m_Demands = new Dictionary<long, Demand>();

        /// <summary>
        /// Разрешить потребность
        /// </summary>
        /// <param name="stillNeeds">сколько нужно единиц товара</param>
        public void Resolve(int stillNeeds)
        {
            //ищем, кто нам может поставить необходимое сырьё и сколько это будет стоить
            do
            {
                Building seller = FindBestOffer(m_Commodity);
                if (seller == null)
                    break;

                Demand demand;
                if (!m_Demands.TryGetValue(m_Id, out demand))
                {
                    demand = new Demand(seller, m_Id);
                    m_Demands[m_Id] = demand;
                }

                if (demand.CanResolve(stillNeeds) > 0)
                {
                    m_Count += demand.Count;
                    m_Price += demand.Price;
                    m_Price += Transportation(m_Buyer, seller) * demand.Count;

                    seller.Reserve(demand);

                    stillNeeds -= demand.Count;
                }
            }
            while (stillNeeds > 0);
        }
        /// <summary>
        /// стоимость доставки единицы товара от производителя
        /// </summary>
        /// <param name="seller">ссылка на строение-производитель</param>
        /// <returns>стоимость доставки</returns>
        private int Transportation(Building buyer, Building seller)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ищет ближайшую постройку, которая способна произвести необходимый товар за минимальную цену
        /// в объёме > 0
        /// </summary>
        /// <param name="commodity">искомый товар</param>
        /// <returns>ссылка на найденную постройку или null если всё плохо</returns>
        private Building FindBestOffer(Commodity commodity)
        {
            throw new NotImplementedException();
        }
    }
}
