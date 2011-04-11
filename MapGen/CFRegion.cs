using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapGen
{
    /// <summary>
    /// Регион на карте
    /// </summary>
    public class CFRegion
    {
        private int m_iHeight = 0;
        /// <summary>
        /// высота региона
        /// </summary>
        public int Height
        {
            get { return m_iHeight; }
            set { m_iHeight = value; }
        }

        private int m_iContinentID = -1;
        /// <summary>
        /// идентификатор континента
        /// </summary>
        public int ContinentID
        {
            get { return m_iContinentID; }
            set { m_iContinentID = value; }
        }

        private bool m_bAllowed = true;
        /// <summary>
        /// на развёртке икосаэдра для вырезанных регионов этот флаг выставляется в 0
        /// </summary>
        public bool Allowed
        {
            get { return m_bAllowed; }
            set { m_bAllowed = value; }
        }
	    
        //int distance = 0;

        private int m_iSector = -1;

        public int Sector
        {
            get { return m_iSector; }
            set { m_iSector = value; }
        }

        private int m_iParallel = -1;

        public int Parallel
        {
            get { return m_iParallel; }
            set { m_iParallel = value; }
        }

        private double m_fLongitude;
        /// <summary>
        /// Угловые координаты - долгота
        /// </summary>
        public double Longitude
        {
            get { return m_fLongitude; }
            set { m_fLongitude = value; }
        }

        private double m_fLatitude;
        /// <summary>
        /// Угловые координаты - широта
        /// </summary>
        public double Latitude
        {
            get { return m_fLatitude; }
            set { m_fLatitude = value; }
        }

        private int m_iX;
        /// <summary>
        /// Декартовы координаты - ось X
        /// </summary>
        public int X
        {
            get { return m_iX; }
            set { m_iX = value; }
        }

        private int m_iY;
        /// <summary>
        /// Декартовы координаты - ось Y
        /// </summary>
        public int Y
        {
            get { return m_iY; }
            set { m_iY = value; }
        }

        CFRegion[] m_aExits = new CFRegion[3];
        /// <summary>
        /// Смежные регионы
        /// </summary>
        public CFRegion[] Exits
        {
            get { return m_aExits; }
        }

        public CFRegion(int iX, int iY, int iHeight, int iContiID, bool bAllowed)
        {
            m_iX = iX;
            m_iY = iY;
            m_iHeight = iHeight;
            m_iContinentID = iContiID;
            m_bAllowed = bAllowed;
        }
    }
}
