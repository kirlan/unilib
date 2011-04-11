using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyModel2.Trading
{
    /// <summary>
    /// Ящик с товаром
    /// </summary>
    public class GoodsCrate
    {
        /// <summary>
        /// Количество товара
        /// </summary>
        private int m_iAmount = 0;

        /// <summary>
        /// Количество товара
        /// </summary>
        public int Amount
        {
            get { return m_iAmount; }
        }

        /// <summary>
        /// Установить количество товара
        /// </summary>
        /// <param name="iCount">количество товара</param>
        public void Set(int iCount)
        {
            m_iAmount = iCount;
        }

        /// <summary>
        /// Доложить товар
        /// </summary>
        /// <param name="iCount">количество докладываемого товара</param>
        public void Add(int iCount)
        {
            m_iAmount += iCount;
        }

        /// <summary>
        /// Взять товар. Больше чем есть забрать не получится.
        /// </summary>
        /// <param name="iCount">количество забираемого товара</param>
        public void Remove(int iCount)
        {
            m_iAmount -= iCount;
            if (m_iAmount < 0)
                m_iAmount = 0;
        }

        /// <summary>
        /// Взять товар. Количество товара в хранилище может стать отрицательным.
        /// </summary>
        /// <param name="iCount">количество забираемого товара</param>
        public void StrongRemove(int iCount)
        {
            m_iAmount -= iCount;
        }

        /// <summary>
        /// Взять весь товар.
        /// </summary>
        public void Clear()
        {
            m_iAmount = 0;
        }
    }

    /// <summary>
    /// Планетарный склад
    /// </summary>
    public class CStock
    {
        /// <summary>
        /// Хранилище товаров
        /// </summary>
        private Dictionary<Goods, GoodsCrate> m_cStorage = new Dictionary<Goods, GoodsCrate>();

        /// <summary>
        /// Сколько товара на складе
        /// </summary>
        /// <param name="goods">товар</param>
        /// <returns>количество товара на складе</returns>
        public int Stored(Goods goods)
        {
            if (!m_cStorage.ContainsKey(goods))
                return 0;

            return m_cStorage[goods].Amount;
        }

        /// <summary>
        /// Список имеющихся на складе товаров
        /// </summary>
        public Dictionary<Goods, GoodsCrate>.KeyCollection Goods
        {
            get 
            {
                return m_cStorage.Keys;
            }
        }

        /// <summary>
        /// Таблица товаров, которых со склада пытались взять больше, чем их было
        /// </summary>
        private Dictionary<Goods, GoodsCrate> m_cShortage = new Dictionary<Goods, GoodsCrate>();
        //private Dictionary<Goods, GoodsCrate> m_cShortage2 = new Dictionary<Goods, GoodsCrate>();

        /// <summary>
        /// Таблица товаров, которых со склада пытались взять больше, чем их было
        /// </summary>
        public Dictionary<Goods, GoodsCrate> Shortage
        {
            get
            {
                return m_cShortage;
            }
        }


        /// <summary>
        /// Взять товар со склада
        /// </summary>
        /// <param name="goods">что брать</param>
        /// <param name="amount">сколько брать</param>
        /// <returns>сколько получилось взять (может быть меньше, чем хотелось)</returns>
        public int Consume(Goods goods, int amount)
        {
            //Если на складе такого товара вообще нет, зарегистрируем нехватку в полном объёме запроса
            if (!m_cStorage.ContainsKey(goods))
            {
                if (!m_cShortage.ContainsKey(goods))
                    m_cShortage[goods] = new GoodsCrate();

                m_cShortage[goods].Add(amount);
                return 0;
            }

            //Если на складе есть, но меньше чем нужно, то выдадим сколько есть, а остаток зарегистрируем в нехватку
            if (m_cStorage[goods].Amount <= amount)
            {
                if (!m_cShortage.ContainsKey(goods))
                    m_cShortage[goods] = new GoodsCrate();

                //m_cShortage[goods].Add(amount - m_cStorage[goods].Amount);
                m_cShortage[goods].Add(amount);
                int iStock = m_cStorage[goods].Amount;
                m_cStorage[goods].Remove(amount);
                return iStock;
            }

            //Если есть больше чем нужно, то просто выдадим запрошенное
            m_cStorage[goods].Remove(amount);
            return amount;
        }

        /// <summary>
        /// Взять весь товар со склада (т.е. всё что есть)
        /// </summary>
        /// <param name="goods">что брать</param>
        /// <returns>сколько удалось взять</returns>
        public int Consume(Goods goods)
        {
            int iStock = m_cStorage[goods].Amount;
            m_cStorage[goods].Clear();
            return iStock;
        }

        /// <summary>
        /// Очистить склад
        /// </summary>
        public void Clear()
        {
            m_cStorage.Clear();
            m_cShortage.Clear();
        }

        /// <summary>
        /// Сбросить даные о нехватке товаров
        /// </summary>
        public void ResetShortage()
        {
            m_cShortage.Clear();
        }

        /// <summary>
        /// Добавить товар на склад
        /// </summary>
        /// <param name="goods"></param>
        /// <param name="amount"></param>
        internal void Store(Goods goods, int amount)
        {
            //Если этого товара на складе ещё не было, создадим под него новый контейнер
            if (!m_cStorage.ContainsKey(goods))
                m_cStorage[goods] = new GoodsCrate();

            m_cStorage[goods].Add(amount);

            //if (m_cShortage.ContainsKey(goods))
            //{
            //    if (m_cShortage[goods].Amount > amount)
            //        m_cShortage[goods].Remove(amount);
            //    else
            //        m_cShortage[goods].Clear();
            //}
        }
    }
}
