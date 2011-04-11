using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace StarMap
{
    public class CStarsMap
    {
        public List<CStar> m_cStars = new List<CStar>();

        public int m_iWidth;
        public int m_iHeight;

        public CStarsMap(int iWidth, int iHeight, int iHabitablePercent)
        {
            for (int x = 0; x < iWidth; x++)
            {
                for (int y = 0; y < iHeight; y++)
                {
                    CStar pNewStar = new CStar(x, y, Rnd.Get(100) < iHabitablePercent);
                    m_cStars.Add(pNewStar);
                }
            }
            m_iWidth = iWidth;
            m_iHeight = iHeight;
        }
    }
}
