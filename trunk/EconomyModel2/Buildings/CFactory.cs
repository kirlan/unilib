using System;
using System.Collections.Generic;
using System.Text;
using EconomyModel2.Trading;

namespace EconomyModel2.Buildings
{
    /// <summary>
    /// Фабрика - постройка, перерабатывающая материальное сырьё в материальный товар.
    /// </summary>
    public class CFactory: CBuilding
    {
        /// <summary>
        /// Имя постройки
        /// </summary>
        private string m_sName = "Unknown";

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="pStock">Ссылка на планетарный склад</param>
        /// <param name="goods">Что производит</param>
        public CFactory(CStock pStock, Goods goods)
            : base(pStock, goods)
        {
            m_sName = string.Format("{0} Factory", Enum.GetName(typeof(Goods), goods));
        }
    
        /// <summary>
        /// Производство товара
        /// </summary>
        public override void Produce()
        {
            //Кладём на склад, всё что произвели
            foreach (Goods goods in m_cProduction.Keys)
            {
                m_pStock.Store(goods, m_cProduction[goods].Amount);
            }
        }

        /// <summary>
        /// Потребление сырья
        /// </summary>
        public override void Consume()
        {
            //Какого сырья на складе меньше всего?
            //Интересует только то сырьё, количество которого меньше нормы потребления (равной уровню фабрики)
            int iMinResourses = m_iLevel;
            foreach (Goods goods in GOODS.Resources)
            {
                if (m_pStock.Stored(goods) < iMinResourses)
                    iMinResourses = m_pStock.Stored(goods);
            }
            //Потребляем сырьё со склада в объёме равном найденному минимальному значению
            //(либо в объёме нормы потребления - если всего хватает)
            foreach (Goods goods in GOODS.Resources)
            {
                m_pStock.Consume(goods, iMinResourses);
            }

            //Установим эффективность производства как квадрат от минимального объёма доступного на складе сырья.
            foreach (Goods goods in m_cProduction.Keys)
            {
                m_cProduction[goods].Set(iMinResourses * iMinResourses);
            }
        }

        /// <summary>
        /// Строковое представление объекта
        /// </summary>
        /// <returns>Возвращаем строку, содержащуюю имя постройки и её уровень</returns>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", m_sName, m_iLevel);
        }
    }
}
