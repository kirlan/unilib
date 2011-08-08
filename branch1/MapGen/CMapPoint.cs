using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapGen
{
    /// <summary>
    /// Просто точка на двух-мерной карте
    /// </summary>
    public class CMapPoint
    {
        private int m_iX = 0;
        public int X
        {
            get { return m_iX; }
            set { m_iX = value; }
        }

        private int m_iY = 0;
        public int Y
        {
            get { return m_iY; }
            set { m_iY = value; }
        }

        public CMapPoint(int xx, int yy)
        {
            m_iX = xx;
            m_iY = yy;
        }
    }
}
