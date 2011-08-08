using System;
using System.Collections.Generic;
using System.Text;
using Random;
using EconomyModel2.Trading;

namespace EconomyModel2
{
    /// <summary>
    /// Игровая вселенная - плоское двумерное поле
    /// </summary>
    public class CUniverse
    {
        /// <summary>
        /// Ширина Вселенной
        /// </summary>
        private int m_iWidth;
        /// <summary>
        /// Ширина Вселенной
        /// </summary>
        public int Width
        {
            get { return m_iWidth; }
        }

        /// <summary>
        /// Высота Вселенной
        /// </summary>
        private int m_iHeight;
        /// <summary>
        /// Высота Вселенной
        /// </summary>
        public int Height
        {
            get { return m_iHeight; }
        }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="iWidth">ширина</param>
        /// <param name="iHeight">высота</param>
        /// <param name="iHabitablePercent">процент обитаемых планет (1-100)</param>
        public CUniverse(int iWidth, int iHeight, int iHabitablePercent)
        {
            for (int x = 0; x < iWidth; x++)
            {
                for (int y = 0; y < iHeight; y++)
                {
                    if (Rnd.Get(100) < iHabitablePercent)
                    {
                        CWorld pNewWorld = new CWorld(x, y, Rnd.Get(100) < iHabitablePercent ? WorldType.AdvancedWorld : WorldType.Colony);
                        m_cWorlds.Add(pNewWorld);
                        m_pTradersLeague.Filials.Add(pNewWorld.Spaceport);
                    }
                }
            }

            m_iWidth = iWidth;
            m_iHeight = iHeight;
        }

        /// <summary>
        /// Конструктор тестовой вселенной - 2 колонии, 1 метрополия
        /// </summary>
        public CUniverse()
        {
            m_iWidth = 10;
            m_iHeight = 10;

            CWorld pNewWorld = new CWorld(5, 5, WorldType.AdvancedWorld);
            m_cWorlds.Add(pNewWorld);
            m_pTradersLeague.Filials.Add(pNewWorld.Spaceport);

            CWorld pColony1 = new CWorld(2, 5, WorldType.Colony);
            m_cWorlds.Add(pColony1);
            m_pTradersLeague.Filials.Add(pColony1.Spaceport);

            CWorld pColony2 = new CWorld(5, 2, WorldType.Colony);
            m_cWorlds.Add(pColony2);
            m_pTradersLeague.Filials.Add(pColony2.Spaceport);
        }

        /// <summary>
        /// Список планет во вселенной
        /// </summary>
        private List<CWorld> m_cWorlds = new List<CWorld>();
        /// <summary>
        /// Список планет во вселенной
        /// </summary>
        public List<CWorld> Worlds
        {
            get
            {
                return m_cWorlds;
            }
        }

        /// <summary>
        /// Лига Торговцев
        /// </summary>
        private CTradersLeague m_pTradersLeague = new CTradersLeague();
        /// <summary>
        /// Лига Торговцев
        /// </summary>
        public CTradersLeague TradersLeague
        {
            get
            {
                return m_pTradersLeague;
            }
        }

        /// <summary>
        /// Прошло 10 лет...
        /// </summary>
        public void Update()
        {
            //Все планеты готовятся к производству - потребляют сырьё, формируют спрос и предложение на межзвёздном рынке
            foreach (CWorld pWorld in m_cWorlds)
            {
                pWorld.Consume();
            }

            //Лига Торговцев осуществляет межзвёздную торговлю
            m_pTradersLeague.Update();

            //Все планеты осуществляют производство
            foreach (CWorld pWorld in m_cWorlds)
            {
                pWorld.Produce();
            }
        }
    }
}
