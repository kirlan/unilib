using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyModel2
{
    public class GoodsCrate
    {
        private int m_iAmount = 0;

        public int Amount
        {
            get { return m_iAmount; }
        }

        public void Set(int iCount)
        {
            m_iAmount = iCount;
        }

        public void Add(int iCount)
        {
            m_iAmount += iCount;
        }

        public void Remove(int iCount)
        {
            m_iAmount -= iCount;
            if (m_iAmount < 0)
                m_iAmount = 0;
        }

        public void Clear()
        {
            m_iAmount = 0;
        }
    }

    public class CStock
    {
        private Dictionary<Goods, GoodsCrate> m_cStorage = new Dictionary<Goods, GoodsCrate>();
        //public Dictionary<Goods, int> Storage
        //{
        //    get
        //    {
        //        return m_cStorage;
        //    }
        //}

        public int Stored(Goods goods)
        {
            if (!m_cStorage.ContainsKey(goods))
                return 0;

            return m_cStorage[goods].Amount;
        }

        public Dictionary<Goods, GoodsCrate>.KeyCollection Goods
        {
            get 
            {
                return m_cStorage.Keys;
            }
        }

        private Dictionary<Goods, GoodsCrate> m_cShortage = new Dictionary<Goods, GoodsCrate>();
        //private Dictionary<Goods, GoodsCrate> m_cShortage2 = new Dictionary<Goods, GoodsCrate>();

        public Dictionary<Goods, GoodsCrate> Shortage
        {
            get
            {
                return m_cShortage;
            }
        }


        public int Consume(Goods goods, int amount)
        {
            if (!m_cStorage.ContainsKey(goods))
            {
                if (!m_cShortage.ContainsKey(goods))
                    m_cShortage[goods] = new GoodsCrate();

                m_cShortage[goods].Add(amount*2);
                return 0;
            }

            if (m_cStorage[goods].Amount <= amount*2)
            {
                if (!m_cShortage.ContainsKey(goods))
                    m_cShortage[goods] = new GoodsCrate();

                m_cShortage[goods].Add(amount*2 - m_cStorage[goods].Amount);
                int iStock = m_cStorage[goods].Amount;
                m_cStorage[goods].Remove(amount);
                return iStock;
            }

            m_cStorage[goods].Remove(amount);
            return amount;
        }

        public int Consume(Goods goods)
        {
            int iStock = m_cStorage[goods].Amount;
            m_cStorage[goods].Clear();
            return iStock;
        }

        public void Clear()
        {
            m_cStorage.Clear();
            m_cShortage.Clear();
        }

        public void ResetShortage()
        {
            //foreach (Goods goods in m_cShortage.Keys)
            //{
            //    if (!m_cShortage2.ContainsKey(goods))
            //    {
            //        m_cShortage2[goods] = new GoodsCrate();
            //        m_cShortage2[goods].Set(m_cShortage[goods].Amount);
            //    }
            //    else
            //    {
            //        m_cShortage2[goods].Set((m_cShortage2[goods].Amount + m_cShortage[goods].Amount)/2);
            //    }
            //}
            m_cShortage.Clear();
        }

        internal void Store(Goods goods, int amount)
        {
            if (!m_cStorage.ContainsKey(goods))
                m_cStorage[goods] = new GoodsCrate();

            m_cStorage[goods].Add(amount);
            if (m_cShortage.ContainsKey(goods))
            {
                if (m_cShortage[goods].Amount > amount)
                    m_cShortage[goods].Remove(amount);
                else
                    m_cShortage[goods].Clear();
            }
        }
    }
}
