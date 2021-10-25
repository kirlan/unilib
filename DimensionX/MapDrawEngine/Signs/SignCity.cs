using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MapDrawEngine.Signs
{
    public struct SignCity : ILandMark
    {
        float x, y, r;
        string name;

        internal static Font s_pFont = new Font("Arial", 12, FontStyle.Bold);

        public SignCity(float iX, float iY, float RX, string sName)
        {
            x = iX;
            y = iY;

            r = RX / 400;
            name = sName;
        }

        public void Draw(Graphics gr, float fScaleMultiplier, float iDX, float iDY, float fScale)
        {
            if (fScaleMultiplier <= 1)
                return;

            float xx = x * fScale;
            float yy = y * fScale;

            float rr = r * fScale;
            if (rr < 2)
                rr = 2;

            gr.FillEllipse(Brushes.Black, xx + iDX - rr, yy + iDY - rr, rr * 2, rr * 2);
            gr.DrawEllipse(Pens.White, xx + iDX - rr, yy + iDY - rr, rr * 2, rr * 2);

            if (fScaleMultiplier > 4)
                gr.DrawString(name, s_pFont, Brushes.Black, xx + iDX + rr, yy + iDY);
        }

        public void Translate(float fDX, float fDY)
        {
            x += fDX;
            y += fDY;
        }
    }
}
