using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MapDrawEngine.Signs
{
    public struct SignCapital : ILandMark
    {
        float x, y, r1, r2;
        string name;

        private static Font s_pFont = new Font("Arial", 12, FontStyle.Bold | FontStyle.Underline);

        public SignCapital(float iX, float iY, string sName)
        {
            x = iX;
            y = iY;

            r1 = 200;
            r2 = 120;

            name = sName;
        }

        public void Draw(Graphics gr, float fScaleMultiplier, float iDX, float iDY, float fScale)
        {
            if (fScaleMultiplier <= 1)
                return;

            float xx = x * fScale;
            float yy = y * fScale;

            float rr1 = r1 * fScale;
            float rr2 = r2 * fScale;
            if (rr1 < 2)
                rr1 = 2;
            if (rr2 < 1)
                rr2 = 1;

            gr.FillEllipse(Brushes.White, xx + iDX - rr1, yy + iDY - rr1, rr1 * 2, rr1 * 2);
            gr.FillEllipse(Brushes.Black, xx + iDX - rr2, yy + iDY - rr2, rr2 * 2, rr2 * 2);
            gr.DrawEllipse(MapDraw.s_pBlack2Pen, xx + iDX - rr1, yy + iDY - rr1, rr1 * 2, rr1 * 2);

            if (fScaleMultiplier > 4)
                gr.DrawString(name, s_pFont, Brushes.Black, xx + iDX + rr1, yy + iDY);
        }

        public void Translate(float fDX, float fDY)
        {
            x += fDX;
            y += fDY;
        }
    }
}
