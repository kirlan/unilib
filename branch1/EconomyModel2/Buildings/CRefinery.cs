using System;
using System.Collections.Generic;
using System.Text;
using EconomyModel2.Trading;

namespace EconomyModel2.Buildings
{
    /// <summary>
    /// Добыча первичных ресурсов
    /// </summary>
    public class CRefinery: CBuilding
    {
        /// <summary>
        /// Имя постройки
        /// </summary>
        private string m_sName = "Unknown";

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="pStock">Ссылка на планетарный склад</param>
        /// <param name="goods">Что добываем</param>
        public CRefinery(CStock pStock, Goods goods)
            : base(pStock, goods)
        {
            m_sName = string.Format("{0} Refinery", Enum.GetName(typeof(Goods), goods));
            //switch (goods)
            //{
            //    case Goods.Ore:
            //        m_sName = "Ore Mine";
            //        break;
            //    case Goods.Oil:
            //        m_sName = "Oil Derrick";
            //        break;
            //    case Goods.Food:
            //        m_sName = "Grain Field";
            //        break;
            //}
        }
    
        /// <summary>
        /// Производство
        /// </summary>
        public override void Produce()
        {
            foreach (Goods goods in m_cProduction.Keys)
            {
                m_pStock.Store(goods, m_cProduction[goods].Amount);
            }
        }

        /// <summary>
        /// Подготовка к производству, определение эффективности добычи
        /// </summary>
        public override void Consume()
        {
            foreach (Goods goods in m_cProduction.Keys)
            {
                m_cProduction[goods].Set(m_iLevel * m_iLevel);
            }
        }

        /// <summary>
        /// Строковое представление объекта
        /// </summary>
        /// <returns>Имя постройки и уровень</returns>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", m_sName, m_iLevel);
        }
    }
}
