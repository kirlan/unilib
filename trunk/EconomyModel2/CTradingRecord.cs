using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyModel2
{
    public class CTradingRecord
    {
        private int m_iCount = 0;

        public int Count
        {
            get { return m_iCount; }
            set { m_iCount = value; }
        }

        private double m_fPrice = 100;

        public double Price
        {
            get { return m_fPrice; }
            set { m_fPrice = value; }
        }

        private Goods m_eGoods = Goods.None;

        public Goods Goods
        {
            get { return m_eGoods; }
        }

        private int m_iSatisfaction = 0;

        public int Satisfaction
        {
            get { return m_iSatisfaction; }
        }

        public int Satisfy(int iSatisfaction)
        {
            if (m_iCount < iSatisfaction)
            {
                m_iSatisfaction = m_iCount;
            }
            else
            {
                m_iSatisfaction = iSatisfaction;
            }
            return m_iSatisfaction;
        }

        private CWorld m_pOwner = null;

        public CWorld Owner
        {
            get { return m_pOwner; }
        }

        public void Declare(CWorld pOwner)
        {
            m_iSatisfaction = 0;
            m_pOwner = pOwner;
        }

        public CTradingRecord(Goods eGoods)
        {
            m_eGoods = eGoods;
        }
    }
}
