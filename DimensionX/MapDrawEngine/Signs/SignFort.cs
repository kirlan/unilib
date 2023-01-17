using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MapDrawEngine.Signs
{
    public struct SignFort : ILandMark
    {
        private float x, y;
        private readonly float r1, r2;
        private readonly string name;
        private readonly PointF[] points;

        internal static Font s_pFont = new Font("Arial", 10);

        public SignFort(float iX, float iY, float RX, string sName)
        {
            x = iX;
            y = iY;

            r1 = RX / 375;
            r2 = RX / 250;

            points = new[]
            {
                new PointF(x, y + r1),
                new PointF(x + r2, y),
                new PointF(x + r1, y - r2),
                new PointF(x - r1, y - r2),
                new PointF(x - r2, y)
            };

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
            float rr2 = r2 * fScale;
            if (rr2 <= rr1)
                rr2 = rr1 + 1;

            PointF[] pts = (PointF[])points.Clone();
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i].X *= fScale;
                pts[i].Y *= fScale;

                pts[i].X += iDX;
                pts[i].Y += iDY;
            }

            gr.FillPolygon(Brushes.Gray, pts);
            gr.DrawPolygon(MapDraw.s_pBlack2Pen, pts);

            if (fScaleMultiplier > 4)
                gr.DrawString(name, s_pFont, Brushes.Black, xx + iDX + rr1, yy + iDY);
        }

        public void Translate(float fDX, float fDY)
        {
            x += fDX;
            y += fDY;

            for (int i = 0; i < points.Length; i++)
            {
                points[i].X += fDX;
                points[i].Y += fDY;
            }
        }
    }
}
