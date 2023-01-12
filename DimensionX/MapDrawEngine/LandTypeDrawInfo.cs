using LandscapeGeneration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDrawEngine
{
    public class LandTypeDrawInfo : ILandTypeInfoExt
    {
        public Color m_pColor;
        public Brush m_pBrush;

        public LandTypeDrawInfo(Color pColor)
        {
            m_pColor = pColor;
            m_pBrush = new SolidBrush(m_pColor);
        }
    }
}
