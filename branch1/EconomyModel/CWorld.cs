using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomyModel
{
    public class CWorld: CFactory
    {
        public int m_iX;
        public int m_iY;

        public CWorld(int iX, int iY)
            : base()
        {
            m_iX = iX;
            m_iY = iY;
        }
    }
}
