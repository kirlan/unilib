using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MapDrawEngine.Signs
{
    public struct SignHideout : ILandMark
    {
        float x, y, r1;
        string name;

        //internal static Font s_pFont = new Font("Arial", 10);

        public SignHideout(float iX, float iY, string sName)
        {
            x = iX;
            y = iY;

            r1 = 60;

            name = sName;
        }

        public void Draw(Graphics gr, float fScaleMultiplier, float iDX, float iDY, float fScale)
        {
            if (fScaleMultiplier <= 2)
                return;

            float xx = x * fScale;
            float yy = y * fScale;

            float rr1 = r1 * fScale;
            if (rr1 < 1)
                rr1 = 1;
            float rr2 = rr1 + 1;

            gr.FillEllipse(Brushes.Red, xx + iDX - rr1, yy + iDY - rr1, rr1 * 2, rr1 * 2);
            gr.DrawEllipse(Pens.Black, xx + iDX - rr1, yy + iDY - rr1, rr1 * 2, rr1 * 2);
            gr.DrawEllipse(MapDraw.s_pBlack1DotPen, xx + iDX - rr2, yy + iDY - rr2, rr2 * 2, rr2 * 2);

            //if (name != null)
            //    gr.DrawString(name, s_pFont, Brushes.Black, x + r, y);
        }

        public void Translate(float fDX, float fDY)
        {
            x += fDX;
            y += fDY;
        }
    }
}
