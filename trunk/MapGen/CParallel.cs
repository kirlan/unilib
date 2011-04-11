using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapGen
{
    /// <summary>
    /// Параллель - массив регионов, находящихся на одной широте
    /// </summary>
    public class CParallel
    {
        private double m_fMinLatitude;
        /// <summary>
        /// Диапазон допустимых широт - нижняя граница.
        /// </summary>
        public double MinLatitude
        {
            get { return m_fMinLatitude; }
            set { m_fMinLatitude = value; }
        }

        private double m_fMaxLatitude;
        /// <summary>
        /// Диапазон допустимых широт - верхняя граница.
        /// </summary>
        public double MaxLatitude
        {
            get { return m_fMaxLatitude; }
            set { m_fMaxLatitude = value; }
        }

        private int m_iLength;
        /// <summary>
        /// Количество регионов в параллели.
        /// </summary>
        public int Length
        {
            get { return m_iLength; }
            set { m_iLength = value; }
        }

        private CFRegion[] m_pRegions;
        /// <summary>
        /// Регионы в параллели.
        /// </summary>
        public CFRegion[] Regions
        {
            get { return m_pRegions; }
            set { m_pRegions = value; }
        }
    };
}
