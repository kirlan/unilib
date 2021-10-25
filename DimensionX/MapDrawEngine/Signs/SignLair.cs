using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MapDrawEngine.Signs
{
    public struct SignLair : ILandMark
    {
        float x, y, r1;
        string name;

        //internal static Font s_pFont = new Font("Arial", 10);
        //internal static Pen s_pBlack1Pen = new Pen(Color.Black, 1);

        public SignLair(float iX, float iY, float RX, string sName)
        {
            x = iX;
            y = iY;

            r1 = RX / 625;

            name = sName;

            //s_pBlack1Pen.DashStyle = DashStyle.Dot;
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

            gr.FillPie(Brushes.DarkRed, xx + iDX - rr2, yy + iDY - rr2, rr2 * 2, rr2 * 2, 180, 180);
            gr.DrawPie(Pens.Black, xx + iDX - rr2, yy + iDY - rr2, rr2 * 2, rr2 * 2, 180, 180);

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
