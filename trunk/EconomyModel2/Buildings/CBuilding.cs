using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EconomyModel2.Trading;

namespace EconomyModel2.Buildings
{
    /// <summary>
    /// Базовый класс для всех построек на планете
    /// </summary>
    public abstract class CBuilding
    {
        /// <summary>
        /// Товары, произведённые за последний отчётный период
        /// </summary>
        protected Dictionary<Goods, GoodsCrate> m_cProduction = new Dictionary<Goods, GoodsCrate>();
        /// <summary>
        /// Товары, произведённые за последний отчётный период
        /// </summary>
        public Dictionary<Goods, GoodsCrate> Production
        {
            get
            {
                return m_cProduction;
            }
        }

        /// <summary>
        /// Ссылка на планетарный склад
        /// </summary>
        protected CStock m_pStock;

        /// <summary>
        /// Уровень постройки, определяет эффективность производства
        /// </summary>
        protected int m_iLevel = 0;
        /// <summary>
        /// Уровень постройки, определяет эффективность производства
        /// </summary>
        public int Level
        {
            get
            {
                return m_iLevel;
            }
        }

        /// <summary>
        /// Потребление
        /// </summary>
        public abstract void Consume();

        /// <summary>
        /// Производство
        /// </summary>
        public abstract void Produce();

        /// <summary>
        /// Апгрейдит постройку до заданного уровня (какой текущий уровень - неважно)
        /// </summary>
        /// <param name="iLevel">до какого уровня апгрейдить</param>
        public void UpgradeToLevel(int iLevel)
        {
            m_iLevel = iLevel;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pStock">Ссылка на планетарный склад</param>
        /// <param name="goods">Товар, производимый постройкой. Если Goods.None - значит ничего не производится.</param>
        public CBuilding(CStock pStock, Goods goods)
        {
            m_pStock = pStock;

            if (goods != Goods.None)
            {
                if (!m_cProduction.ContainsKey(goods))
                    m_cProduction[goods] = new GoodsCrate();
                m_cProduction[goods].Set(1);
            }
        }
    }
}
