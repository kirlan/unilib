using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace MapGen
{
    public class CLandMass
    {
        /// <summary>
        /// Потенциальная точка
        /// </summary>
        private struct TPotent
        {
            /// <summary>
            /// Ссылка на регион
            /// </summary>
            public CFRegion region;
            /// <summary>
            /// Шанс реализоваться
            /// </summary>
            public int chance;
        }

        private List<CFRegion> m_pRegions = new List<CFRegion>();
        /// <summary>
        /// массив регионов, принадлежащих континенту
        /// </summary>
        public List<CFRegion> Regions
        {
            get { return m_pRegions; }
        }

        // массив точек, образующих ВНУТРЕННЮЮ границу уже построенной части континента
        private List<CFRegion> m_cInnerBorder = new List<CFRegion>();

        public List<CFRegion> Border
        {
            get { return m_cInnerBorder; }
        }

        // массив точек, образующих ВНЕШНЮЮ границу уже построенной части континента
        private List<TPotent> m_cOuterBorder = new List<TPotent>();

        private int m_iId;

        public int Id
        {
            get { return m_iId; }
        }

        private int m_iX;

        public int X
        {
            get { return m_iX; }
            set { m_iX = value; }
        }

        private int m_iY;

        public int Y
        {
            get { return m_iY; }
            set { m_iY = value; }
        }

        private int m_iR;

        public int R
        {
            get { return m_iR; }
            set { m_iR = value; }
        }

        private int m_iSizeNeeded;
        public int SizeNeeded
        {
            get { return m_iSizeNeeded; }
        }

        private int m_iShoreDistance;

        public int ShoreDistance
        {
            get { return m_iShoreDistance; }
        }

        public CLandMass(int id, int iSizeNeeded, int iShoreDistance)
        {
            m_pRegions.Clear();
            m_cOuterBorder.Clear();

            m_iId = id;
            m_iSizeNeeded = iSizeNeeded;
            m_iShoreDistance = iShoreDistance;
        }

        public void ResetFrontier()
        {
            m_cInnerBorder.Clear();
            m_cOuterBorder.Clear();

            bestChance = 0;
            worstChance = 0;
            bestChanceCount = 0;
        }

        private int bestChance = 0;
        private int worstChance = 0;
        private int bestChanceCount = 0;

        public void AddFrontierRegion(CFRegion innerRegion, CFRegion outerRegion, int chance)
        {
            TPotent newPoint = new TPotent();
            newPoint.region = outerRegion;
            newPoint.chance = chance;


            if (bestChanceCount == 0)
            {
                if (chance >= 0)
                {
                    bestChance = chance;
                    worstChance = chance;
                    bestChanceCount = 1;
                }
            }
            else
            {
                if (chance == bestChance)
                {
                    bestChanceCount++;
                }
                if (chance < bestChance && chance >= 0)
                {
                    bestChance = chance;
                    bestChanceCount = 1;
                }
                if (chance > worstChance)
                {
                    worstChance = chance;
                }
            }

            m_cOuterBorder.Add(newPoint);

            if (!m_cInnerBorder.Contains(innerRegion))
                m_cInnerBorder.Add(innerRegion);
        }

        /// <summary>
        /// Возвращает регион с наивысшим шансом. Если таких регионов несколько - то случайно выбирает один из них.
        /// </summary>
        /// <returns>регион с наивысшим шансом</returns>
        public CFRegion GetBestFrontierRegion()
        {
            // если есть хоть один претендент
            if (bestChanceCount > 0)
            {
                // выбираем один из найденных регионов
                int chance;
                chance = Rnd.Get(bestChanceCount);
                foreach (TPotent potent in m_cOuterBorder)
                {
                    if (potent.chance == bestChance)
                    {
                        chance--;
                        if (chance < 0)
                        {
                            return potent.region;
                            //region = m_pRegions[frontier[i].x][frontier[i].y];
                            //chance = iFrontierLength * 2;
                        }
                    }
                }
            }

            return null;
        }

        public CFRegion GetRandomBorderRegion()
        {
            int iTotalChances = 0;
            foreach (TPotent potent in m_cOuterBorder)
            {
                iTotalChances += worstChance - potent.chance;
            }

            int iChance = Rnd.Get(iTotalChances);

            foreach (TPotent potent in m_cOuterBorder)
            {
                iChance -= worstChance - potent.chance;
                if (iChance <= 0)
                    return potent.region;
            }

            return null;
        }
    }
}
