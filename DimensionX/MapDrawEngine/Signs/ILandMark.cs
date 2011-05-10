using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MapDrawEngine
{
    internal interface ILandMark
    {
        void Draw(Graphics gr, float fScaleMultiplier, float iDX, float iDY, float fScale);

        void Translate(float fDX, float fDY);
    }
}
