using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EconomyModel2.Trading;

namespace EconomyModel2.Buildings
{
    /// <summary>
    /// Жилые помещения - обеспечивают планету рабочими
    /// </summary>
    class CLivingQuarters: CBuilding
    {
        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="pStock">Ссылка на планетарный склад</param>
        public CLivingQuarters(CStock pStock)
            : base(pStock, Goods.None)
        {
        }

        /// <summary>
        /// Потребление товаров первой необходимости.
        /// </summary>
        public override void Consume()
        {
            foreach (Goods goods in GOODS.Primary)
            {
                m_pStock.Consume(goods, m_iLevel);
            }
        }

        /// <summary>
        /// Производство - упрощённый вариант.
        /// Пока население меньше максимума, добавляем по одному рабочему каждый отчётный период
        /// </summary>
        public override void Produce()
        {
            if(m_pStock.Stored(Goods.Workers) < m_iLevel)
            {
                m_pStock.Store(Goods.Workers, 1);
                if (!m_cProduction.ContainsKey(Goods.Workers))
                    m_cProduction[Goods.Workers] = new GoodsCrate();

                m_cProduction[Goods.Workers].Add(1);
            }
        }

        /// <summary>
        /// Строковое представление объекта
        /// </summary>
        /// <returns>Название постройки и уровень</returns>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", "Dorms", m_iLevel);
        }
    }
}
