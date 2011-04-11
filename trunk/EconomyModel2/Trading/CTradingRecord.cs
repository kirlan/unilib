using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyModel2.Trading
{
    /// <summary>
    /// Информация о торговой сделке - предложение или запрос
    /// </summary>
    public class CTradingRecord
    {
        /// <summary>
        /// Объём сделки
        /// </summary>
        private int m_iCount = 0;

        /// <summary>
        /// Объём сделки
        /// </summary>
        public int Count
        {
            get { return m_iCount; }
            set { m_iCount = value; }
        }

        /// <summary>
        /// Цена за единицу товара
        /// </summary>
        private double m_fPrice = 100;

        /// <summary>
        /// Цена за единицу товара
        /// </summary>
        public double Price
        {
            get { return m_fPrice; }
            set { m_fPrice = value; }
        }

        /// <summary>
        /// Товар
        /// </summary>
        private Goods m_eGoods = Goods.None;

        /// <summary>
        /// Товар
        /// </summary>
        public Goods Goods
        {
            get { return m_eGoods; }
        }

        /// <summary>
        /// Удовлетворённость сделки - сколько из продаваемого/покупаемого товара уже продано/куплено
        /// </summary>
        private int m_iSatisfaction = 0;

        /// <summary>
        /// Удовлетворённость сделки - сколько из продаваемого/покупаемого товара уже продано/куплено
        /// </summary>
        public int Satisfaction
        {
            get { return m_iSatisfaction; }
        }

        /// <summary>
        /// Удовлетворить сделку
        /// </summary>
        /// <param name="iSatisfaction">Количество товара, которое желаем купить/продать</param>
        /// <returns>Количество реально купленного/проданного товара (может быть меньше, чем запрошено, если больше нет/не надо)</returns>
        public int Satisfy(int iSatisfaction)
        {
            if (m_iCount - m_iSatisfaction < iSatisfaction)
            {
                int increase = m_iCount - m_iSatisfaction;
                m_iSatisfaction = m_iCount;
                return increase;
            }
            else
            {
                m_iSatisfaction += iSatisfaction;
                return iSatisfaction;
            }
        }

        /// <summary>
        /// Ссылка на планету, предлагающую эту сделку
        /// </summary>
        private CWorld m_pOwner = null;

        /// <summary>
        /// Ссылка на планету, предлагающую эту сделку
        /// </summary>
        public CWorld Owner
        {
            get { return m_pOwner; }
        }

        /// <summary>
        /// Подтверждение сделки
        /// </summary>
        /// <param name="pOwner">Планета, предлагающая сделку</param>
        public void Declare(CWorld pOwner)
        {
            m_iSatisfaction = 0;
            m_pOwner = pOwner;
        }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="eGoods">Товар - предмет сделки</param>
        public CTradingRecord(Goods eGoods)
        {
            m_eGoods = eGoods;
        }
    }
}
