using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Random;
using System.Drawing.Drawing2D;
using LandscapeGeneration;
using LandscapeGeneration.PathFind;
using nsUniLibControls;
using Socium;

namespace VQMapTest2
{
    #region Signs
    internal interface ILandMark
    {
        void Draw(Graphics gr, float fScale, float iDX, float iDY);

        void Scale(float fScale);

        void Translate(float fDX, float fDY);
    }

    public struct SignCapital: ILandMark
    {
        float x, y, r1, r2;
        string name;

        private static Font s_pFont = new Font("Arial", 12, FontStyle.Bold | FontStyle.Underline);

        public SignCapital(float fkX, float iX, float iY, string sName)
        {
            x = iX;
            y = iY;

            r1 = fkX * 200;
            r2 = fkX * 120;

            name = sName;
        }

        public void Draw(Graphics gr, float fScale, float iDX, float iDY)
        {
            if (fScale <= 1)
                return;

            float rr1 = r1;
            float rr2 = r2;
            if (rr1 < 2)
                rr1 = 2;
            if (rr2 < 1)
                rr2 = 1;

            gr.FillEllipse(Brushes.White, x + iDX - rr1, y + iDY - rr1, rr1 * 2, rr1 * 2);
            gr.FillEllipse(Brushes.Black, x + iDX - rr2, y + iDY - rr2, rr2 * 2, rr2 * 2);
            gr.DrawEllipse(WorldMap.s_pBlack2Pen, x + iDX - rr1, y + iDY - rr1, rr1 * 2, rr1 * 2);

            if (fScale > 4)
                gr.DrawString(name, s_pFont, Brushes.Black, x + iDX + rr1, y + iDY);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r1 *= fScale;
            r2 *= fScale;
        }

        public void Translate(float fDX, float fDY)
        {
            x += fDX;
            y += fDY;
        }
    }

    public struct SignCity : ILandMark
    {
        float x, y, r;
        string name;

        internal static Font s_pFont = new Font("Arial", 12, FontStyle.Bold);

        public SignCity(float fkX, float iX, float iY, string sName)
        {
            x = iX;
            y = iY;

            r = fkX * 120;
            name = sName;
        }

        public void Draw(Graphics gr, float fScale, float iDX, float iDY)
        {
            if (fScale <= 1)
                return;

            float rr = r;
            if (rr < 2)
                rr = 2;

            gr.FillEllipse(Brushes.Black, x + iDX - rr, y + iDY - rr, rr * 2, rr * 2);
            gr.DrawEllipse(Pens.White, x + iDX - rr, y + iDY - rr, rr * 2, rr * 2);

            if (fScale > 4)
                gr.DrawString(name, s_pFont, Brushes.Black, x + iDX + rr, y + iDY);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r *= fScale;
        }

        public void Translate(float fDX, float fDY)
        {
            x += fDX;
            y += fDY;
        }
    }

    public struct SignTown: ILandMark
    {
        float x, y, r;
        string name;

        internal static Font s_pFont = new Font("Arial", 10);

        public SignTown(float fkX, float iX, float iY, string sName)
        {
            x = iX;
            y = iY;

            r = fkX * 80;
            name = sName;
        }

        public void Draw(Graphics gr, float fScale, float iDX, float iDY)
        {
            if (fScale <= 2)
                return;

            float rr = r;
            if (rr < 1)
                rr = 1;

            gr.FillEllipse(Brushes.Black, x + iDX - rr, y + iDY - rr, rr * 2, rr * 2);
            gr.DrawEllipse(Pens.Black, x + iDX - rr, y + iDY - rr, rr * 2, rr * 2);

            if (fScale > 4)
                gr.DrawString(name, s_pFont, Brushes.Black, x + iDX + rr, y + iDY);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r *= fScale;
        }

        public void Translate(float fDX, float fDY)
        {
            x += fDX;
            y += fDY;
        }
    }

    public struct SignVillage: ILandMark
    {
        float x, y, r;
        string name;
        Brush brush;

        internal static Font s_pFont = new Font("Arial", 8, FontStyle.Italic);

        public SignVillage(float fkX, float iX, float iY, string sName, Brush pBrush)
        {
            x = iX;
            y = iY;

            r = fkX * 80;

            name = sName;

            brush = pBrush;
        }

        public void Draw(Graphics gr, float fScale, float iDX, float iDY)
        {
            if (fScale <= 4)
                return;

            float rr = r;
            if (rr < 1)
                rr = 1;

            gr.FillEllipse(brush, x + iDX - rr, y + iDY - rr, rr * 2, rr * 2);
            gr.DrawEllipse(Pens.Black, x + iDX - rr, y + iDY - rr, rr * 2, rr * 2);

            if (fScale > 4)
                gr.DrawString(name, s_pFont, Brushes.Black, x + iDX + rr, y + iDY);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r *= fScale;
        }

        public void Translate(float fDX, float fDY)
        {
            x += fDX;
            y += fDY;
        }
    }

    public struct SignFort : ILandMark
    {
        float x, y, r1, r2;
        string name;
        PointF[] points;

        internal static Font s_pFont = new Font("Arial", 10);

        public SignFort(float fkX, float iX, float iY, string sName)
        {
            x = iX;
            y = iY;

            r1 = fkX * 133;
            r2 = fkX * 200;

            List<PointF> cPoints = new List<PointF>();
            cPoints.Add(new PointF(x, y + r1));
            cPoints.Add(new PointF(x + r2, y));
            cPoints.Add(new PointF(x + r1, y - r2));
            cPoints.Add(new PointF(x - r1, y - r2));
            cPoints.Add(new PointF(x - r2, y));

            points = cPoints.ToArray();

            name = sName;
        }

        public void Draw(Graphics gr, float fScale, float iDX, float iDY)
        {
            if (fScale <= 2)
                return;

            float rr1 = r1;
            if (rr1 < 1)
                rr1 = 1;
            float rr2 = r2;
            if (rr2 <= rr1)
                rr2 = rr1 + 1;

            PointF[] pts = (PointF[])points.Clone();
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i].X += iDX;
                pts[i].Y += iDY;
            }

            gr.FillPolygon(Brushes.Gray, pts);
            gr.DrawPolygon(WorldMap.s_pBlack2Pen, pts);

            if (fScale > 4)
                gr.DrawString(name, s_pFont, Brushes.Black, x + iDX + rr1, y + iDY);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r1 *= fScale;
            r2 *= fScale;

            for (int i = 0; i < points.Length; i++ )
            {
                points[i].X *= fScale;
                points[i].Y *= fScale;
            }
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

    public struct SignHideout: ILandMark
    {
        float x, y, r1;
        string name;

        //internal static Font s_pFont = new Font("Arial", 10);
        internal static Pen s_pBlack1Pen = new Pen(Color.Black, 1);

        public SignHideout(float fkX, float iX, float iY, string sName)
        {
            x = iX;
            y = iY;

            r1 = fkX * 80;

            name = sName;

            s_pBlack1Pen.DashStyle = DashStyle.Dot;
        }

        public void Draw(Graphics gr, float fScale, float iDX, float iDY)
        {
            if (fScale <= 2)
                return;

            float rr1 = r1;
            if (rr1 < 1)
                rr1 = 1;
            float rr2 = rr1 + 1;

            gr.FillEllipse(Brushes.Red, x + iDX - rr1, y + iDY - rr1, rr1 * 2, rr1 * 2);
            gr.DrawEllipse(Pens.Black, x + iDX - rr1, y + iDY - rr1, rr1 * 2, rr1 * 2);
            gr.DrawEllipse(s_pBlack1Pen, x + iDX - rr2, y + iDY - rr2, rr2 * 2, rr2 * 2);

            //if (name != null)
            //    gr.DrawString(name, s_pFont, Brushes.Black, x + r, y);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r1 *= fScale;
        }

        public void Translate(float fDX, float fDY)
        {
            x += fDX;
            y += fDY;
        }
    }

    public struct SignLair: ILandMark
    {
        float x, y, r1;
        string name;

        //internal static Font s_pFont = new Font("Arial", 10);
        //internal static Pen s_pBlack1Pen = new Pen(Color.Black, 1);

        public SignLair(float fkX, float iX, float iY, string sName)
        {
            x = iX;
            y = iY;

            r1 = fkX * 80;

            name = sName;

            //s_pBlack1Pen.DashStyle = DashStyle.Dot;
        }

        public void Draw(Graphics gr, float fScale, float iDX, float iDY)
        {
            if (fScale <= 2)
                return;

            float rr1 = r1;
            if (rr1 < 1)
                rr1 = 1;
            float rr2 = rr1 + 1;

            gr.FillPie(Brushes.DarkRed, x + iDX - rr2, y + iDY - rr2, rr2 * 2, rr2 * 2, 180, 180);
            gr.DrawPie(Pens.Black, x + iDX - rr2, y + iDY - rr2, rr2 * 2, rr2 * 2, 180, 180);

            //if (name != null)
            //    gr.DrawString(name, s_pFont, Brushes.Black, x + r, y);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r1 *= fScale;
        }

        public void Translate(float fDX, float fDY)
        {
            x += fDX;
            y += fDY;
        }
    }
    
    public struct SignPeak: ILandMark
    {
        float x, y, r;
        string name;
        PointF[] points;

        //internal static Font s_pFont = new Font("Arial", 10);
        //internal static Pen s_pBlack1Pen = new Pen(Color.Black, 1);

        public SignPeak(float fkX, float iX, float iY, string sName)
        {
            x = iX;
            y = iY;

            r = fkX * 200;
            //if (r < 1)
            //    r = 1;
            
            List<PointF> cPoints = new List<PointF>();
            cPoints.Add(new PointF(x - r, y));
            cPoints.Add(new PointF(x, y - 2*r));
            cPoints.Add(new PointF(x + r, y));
            points = cPoints.ToArray();

            name = sName;

            //s_pBlack1Pen.DashStyle = DashStyle.Dot;
        }

        public void Draw(Graphics gr, float fScale, float iDX, float iDY)
        {
            gr.FillEllipse(Brushes.Silver, x + iDX - r, y + iDY - r / 2, r * 2, r);

            PointF[] pts = (PointF[])points.Clone();
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i].X += iDX;
                pts[i].Y += iDY;
            }

            gr.FillPolygon(Brushes.Silver, pts);
            gr.DrawLines(Pens.Black, pts);

            //if (name != null)
            //    gr.DrawString(name, s_pFont, Brushes.Black, x + r, y);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r *= fScale;

            for (int i = 0; i < points.Length; i++)
            {
                points[i].X *= fScale;
                points[i].Y *= fScale;
            }
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
    
    public struct SignRuin: ILandMark
    {
        float x, y, r1, r2;
        string name;
        PointF[] points;

        //internal static Font s_pFont = new Font("Arial", 10);
        //internal static Pen s_pBlack1Pen = new Pen(Color.Black, 1);

        public SignRuin(float fkX, float iX, float iY, string sName)
        {
            x = iX;
            y = iY;

            r1 = fkX * 120;
            //if (r1 < 1)
            //    r1 = 1;
            r2 = r1 + 1;
            
            List<PointF> cPoints = new List<PointF>();
            cPoints.Add(new PointF(x, y - r1));
            cPoints.Add(new PointF(x + r1, y));
            cPoints.Add(new PointF(x, y + r1));
            cPoints.Add(new PointF(x - r1, y));
            points = cPoints.ToArray();

            name = sName;

            //s_pBlack1Pen.DashStyle = DashStyle.Dot;
        }

        public void Draw(Graphics gr, float fScale, float iDX, float iDY)
        {
            if (fScale <= 2)
                return;

            PointF[] pts = (PointF[])points.Clone();
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i].X += iDX;
                pts[i].Y += iDY;
            }

            gr.FillPolygon(Brushes.Silver, pts);
            gr.DrawPolygon(WorldMap.s_pBlack2Pen, pts);

            //if (name != null)
            //    gr.DrawString(name, s_pFont, Brushes.Black, x + r, y);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r1 *= fScale;
            r2 *= fScale;

            for (int i = 0; i < points.Length; i++)
            {
                points[i].X *= fScale;
                points[i].Y *= fScale;
            }
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
    
    public struct SignVolkano: ILandMark
    {
        float x, y, r;
        string name;
        PointF[] points1;
        PointF[] points2;

        //internal static Font s_pFont = new Font("Arial", 10);
        //internal static Pen s_pBlack1Pen = new Pen(Color.Black, 1);

        public SignVolkano(float fkX, float iX, float iY, string sName)
        {
            x = iX;
            y = iY;

            r = fkX * 200;
            //if (r < 1)
            //    r = 1;
            
            List<PointF> cPoints = new List<PointF>();
            cPoints.Add(new PointF(x - r, y));
            cPoints.Add(new PointF(x, y - 2*r));
            cPoints.Add(new PointF(x + r, y));
            points1 = cPoints.ToArray();

            cPoints.Clear();
            cPoints.Add(new PointF(x - r / 2, y - r));
            cPoints.Add(new PointF(x, y - 2*r));
            cPoints.Add(new PointF(x + r / 2, y - r));
            cPoints.Add(new PointF(x, y - r / 2));
            points2 = cPoints.ToArray();

            name = sName;

            //s_pBlack1Pen.DashStyle = DashStyle.Dot;
        }

        public void Draw(Graphics gr, float fScale, float iDX, float iDY)
        {
            gr.FillEllipse(Brushes.Silver, x + iDX - r, y + iDY - r / 2, r * 2, r);

            PointF[] pts1 = (PointF[])points1.Clone();
            for (int i = 0; i < pts1.Length; i++)
            {
                pts1[i].X += iDX;
                pts1[i].Y += iDY;
            }
            gr.FillPolygon(Brushes.Silver, pts1);
            gr.DrawLines(Pens.Black, pts1);

            PointF[] pts2 = (PointF[])points2.Clone();
            for (int i = 0; i < pts2.Length; i++)
            {
                pts2[i].X += iDX;
                pts2[i].Y += iDY;
            }
            gr.FillPolygon(Brushes.Red, pts2);
            gr.DrawLines(WorldMap.s_pRed2Pen, pts2);

            //if (name != null)
            //    gr.DrawString(name, s_pFont, Brushes.Black, x + r, y);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r *= fScale;

            for (int i = 0; i < points1.Length; i++)
            {
                points1[i].X *= fScale;
                points1[i].Y *= fScale;
            }

            for (int i = 0; i < points2.Length; i++)
            {
                points2[i].X *= fScale;
                points2[i].Y *= fScale;
            }
        }

        public void Translate(float fDX, float fDY)
        {
            x += fDX;
            y += fDY;

            for (int i = 0; i < points1.Length; i++)
            {
                points1[i].X += fDX;
                points1[i].Y += fDY;
            }

            for (int i = 0; i < points2.Length; i++)
            {
                points2[i].X += fDX;
                points2[i].Y += fDY;
            }
        }
    }
    #endregion

    public partial class WorldMap : UserControl
    {
        Bitmap ActualMap = null;

        float m_fkX = 1;
        float m_fkY = 1;

        private Brush[] m_aHumidity;

        private MapQuadrant[,] m_aQuadrants;

        public WorldMap()
        {
            InitializeComponent();

            m_pDrawFrame = new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            //BuildActualMap(1);

            List<Brush> cHumidity = new List<Brush>();
            for (int i = 0; i <= 100; i++)
                cHumidity.Add(GetHumidityColor(i));
            m_aHumidity = cHumidity.ToArray();

            s_pBlack1DotPen.DashPattern = new float[] { 1, 2 };
            s_pAqua1DotPen.DashPattern = new float[] { 1, 2 };
            s_pBlack3DotPen.DashPattern = new float[] { 2, 4 };

            m_aQuadrants = new MapQuadrant[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    m_aQuadrants[i, j] = new MapQuadrant();
                }
        }

        private Rectangle m_pDrawFrame;

        public Rectangle DrawFrame
        {
            get { return m_pDrawFrame; }
        }

        private void WorldMap_Resize(object sender, EventArgs e)
        {
            if (m_bFreeze)
                return;

            if (ClientRectangle.Width == 0 || ClientRectangle.Height == 0 || ActualMap == null)
                return;

            if (m_pMasterMap == null)
            {
                float fScale = (float)ClientRectangle.Height / m_pDrawFrame.Height;
                if ((float)ClientRectangle.Width / m_pDrawFrame.Width < fScale)
                    fScale = (float)ClientRectangle.Width / m_pDrawFrame.Width;

                ScalePaths(fScale);

                m_pDrawFrame.Width = ClientRectangle.Width;
                m_pDrawFrame.Height = ClientRectangle.Height;
            }

            //if (m_pDrawFrame.X + m_pDrawFrame.Width > ActualMap.Width)
            //    m_pDrawFrame.X = ActualMap.Width - m_pDrawFrame.Width;

            if (m_pDrawFrame.Y + m_pDrawFrame.Height > m_iScaledMapHeight)
                m_pDrawFrame.Y = m_iScaledMapHeight - m_pDrawFrame.Height;

            //DrawMap();
            FireSelectedStateEvent();
        }

        private void WorldMap_Paint(object sender, PaintEventArgs e)
        {
            if (ActualMap == null)
                return;

            if (m_pMasterMap != null)
            {
                e.Graphics.DrawImage(ActualMap, m_iShiftX, m_iShiftY);
                e.Graphics.DrawRectangle(Pens.Red, new Rectangle(m_pDrawFrame.X + m_iShiftX, m_pDrawFrame.Y + m_iShiftY, m_pDrawFrame.Width, m_pDrawFrame.Height));

                if (m_pDrawFrame.X < 0)
                    e.Graphics.DrawRectangle(Pens.Red, new Rectangle(m_pDrawFrame.X + m_iShiftX + m_iScaledMapWidth, m_pDrawFrame.Y + m_iShiftY, m_pDrawFrame.Width, m_pDrawFrame.Height));

                if (m_pDrawFrame.X + m_pDrawFrame.Width > m_iScaledMapWidth)
                    e.Graphics.DrawRectangle(Pens.Red, new Rectangle(m_pDrawFrame.X + m_iShiftX - m_iScaledMapWidth, m_pDrawFrame.Y + m_iShiftY, m_pDrawFrame.Width, m_pDrawFrame.Height));

                if (m_iShiftX > 0)
                {
                    e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, m_iShiftX, ClientRectangle.Height);
                    e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle.Width - m_iShiftX, 0, m_iShiftX, ClientRectangle.Height);
                }
                if (m_iShiftY > 0)
                {
                    e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, ClientRectangle.Width, m_iShiftY);
                    e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, ClientRectangle.Height - m_iShiftY, ClientRectangle.Width, m_iShiftY);
                }
            }
            else
            {
//                int iActualWidth = Math.Min(m_pDrawFrame.Width, ActualMap.Width - m_pDrawFrame.X);
//                e.Graphics.DrawImage(ActualMap, m_iShiftX, m_iShiftY, new Rectangle(m_pDrawFrame.X, m_pDrawFrame.Y, iActualWidth, m_pDrawFrame.Height), GraphicsUnit.Pixel);
                e.Graphics.DrawImage(ActualMap, m_iShiftX, m_iShiftY, new Rectangle(0, 0, m_pDrawFrame.Width, m_pDrawFrame.Height), GraphicsUnit.Pixel);

                //if (iActualWidth < m_pDrawFrame.Width)
                //    e.Graphics.DrawImage(ActualMap, iActualWidth + m_iShiftX, m_iShiftY, new Rectangle(0, m_pDrawFrame.Y, m_pDrawFrame.Width - iActualWidth, m_pDrawFrame.Height), GraphicsUnit.Pixel);
            }
        }

        bool m_bFreeze = false;

        private float m_fScale = 1;

        public float MapScale
        {
            get { return m_fScale; }
            set 
            { 
                SetScale(value);
            }
        }

        /// <summary>
        /// смещение отображения карты для центрирования - если карта уже рабочей поверхности
        /// </summary>
        private int m_iShiftX = 0;
        /// <summary>
        /// смещение отображения карты для центрирования - если карта уже рабочей поверхности
        /// </summary>
        private int m_iShiftY = 0;

        float m_fOneQuadWidth;
        float m_fOneQuadHeight;
        
        int m_iQuadsWidth;
        int m_iQuadsHeight;

        public int m_iScaledMapWidth;
        public int m_iScaledMapHeight;

        private void BuildActualMap(float fScale)
        {
            if (ClientRectangle.Width == 0)
                return;

            float fK = 1;
            if (m_pWorld != null)
                fK = (float)m_pWorld.m_cGrid.RY / m_pWorld.m_cGrid.RX;

            m_iScaledMapWidth = (int)(ClientRectangle.Width * fScale);
            m_iScaledMapHeight = (int)(m_iScaledMapWidth * fK);

            if (m_iScaledMapHeight > ClientRectangle.Height * fScale)
            {
                m_iScaledMapHeight = (int)(ClientRectangle.Height * fScale);
                m_iScaledMapWidth = (int)(m_iScaledMapHeight / fK);
            }

            ActualMap = new Bitmap(Math.Min(ClientRectangle.Width, m_iScaledMapWidth), Math.Min(ClientRectangle.Height, m_iScaledMapHeight));

            if (m_pWorld != null)
            {
                m_fkX = (float)(m_iScaledMapWidth) / (m_pWorld.m_cGrid.RX * 2);
                m_fkY = (float)(m_iScaledMapHeight) / (m_pWorld.m_cGrid.RY * 2);
            }

            if (ActualMap.Width < ClientRectangle.Width)
                m_iShiftX = (ClientRectangle.Width - ActualMap.Width) / 2;
            else
                m_iShiftX = 0;

            if (ActualMap.Height < ClientRectangle.Height)
                m_iShiftY = (ClientRectangle.Height - ActualMap.Height) / 2;
            else
                m_iShiftY = 0;

            if (m_pMasterMap == null)
            {
                m_pDrawFrame.Width = ClientRectangle.Width - m_iShiftX*2;
                m_pDrawFrame.Height = ClientRectangle.Height - m_iShiftY*2;
            }

            m_fOneQuadWidth = m_fkX * m_pWorld.m_cGrid.RX / 4;
            m_fOneQuadHeight = m_fkY * m_pWorld.m_cGrid.RY / 4;

            m_iQuadsWidth = 2 + (int)(m_pDrawFrame.Width / m_fOneQuadWidth);
            m_iQuadsHeight = 2 + (int)(m_pDrawFrame.Height / m_fOneQuadHeight);
        }

        private void SetScale(float fNewScale)
        {
            if (ClientRectangle.Width == 0 || m_pWorld == null)
            {
                m_fScale = fNewScale;
                return;
            }

            bool bFirstTime = (ActualMap == null);

            int iDrawFrameCenterX = 0;
            int iDrawFrameCenterY = 0;
            if (!bFirstTime)
            {
                iDrawFrameCenterX = (int)((m_pDrawFrame.X + m_pDrawFrame.Width / 2) / m_fkX);
                iDrawFrameCenterY = (int)((m_pDrawFrame.Y + m_pDrawFrame.Height / 2) / m_fkY);
            }


            float fScale = fNewScale / m_fScale;

            m_fScale = fNewScale;

            BuildActualMap(m_fScale);

            PrebuildPaths();
            ScalePaths(fScale);

            DrawMap();

            if(!bFirstTime)
                SetPan((int)(m_fkX * iDrawFrameCenterX) - m_pDrawFrame.Width / 2, (int)(m_fkY * iDrawFrameCenterY) - m_pDrawFrame.Height / 2);

            FireSelectedStateEvent();

            if (m_pMiniMap != null)
                m_pMiniMap.SinchronizeDrawFrame(this);
        }

        private WorldMap m_pMasterMap = null;

        //private bool m_bMiniMapMode = false;

        //public bool MiniMapMode
        //{
        //    get { return m_bMiniMapMode; }
        //    set 
        //    {
        //        if (m_bMiniMapMode == value)
        //            return;
                
        //        m_bMiniMapMode = value;
        //        if (m_bMiniMapMode)
        //        {
        //            BuildActualMap(1);
        //            DrawMap();
        //        }
        //        SetScale();
        //    }
        //}

        public void SetPan(int iX, int iY)
        {
            if(m_pWorld.m_cGrid.m_bCycled)
                m_pDrawFrame.X = iX;
            else
                m_pDrawFrame.X = Math.Max(0, Math.Min(iX, m_iScaledMapWidth - m_pDrawFrame.Width));

            while (m_pDrawFrame.X < 0)
                m_pDrawFrame.X += m_iScaledMapWidth;

            while (m_pDrawFrame.X > m_iScaledMapWidth)
                m_pDrawFrame.X -= m_iScaledMapWidth;

            m_pDrawFrame.Y = Math.Max(0, Math.Min(iY, m_iScaledMapHeight - m_pDrawFrame.Height));

            if (m_pMasterMap == null)
                DrawMap();

            Refresh();

            if (m_pMiniMap != null)
                m_pMiniMap.SinchronizeDrawFrame(this);

            if (m_pMasterMap != null)
                m_pMasterMap.SinchronizeDrawFrame(this);
        }

        private Dictionary<Race, Brush> m_cRaceColorsID = new Dictionary<Race, Brush>();

        private Color[] m_aRaceColorsTemplate = new Color[] 
        { 
            Color.FromArgb(60, 60, 60),
            Color.FromArgb(120, 60, 60),
            Color.FromArgb(180, 60, 60),
            Color.FromArgb(240, 60, 60),

            Color.FromArgb(60, 120, 60),
            Color.FromArgb(120, 120, 60),
            Color.FromArgb(180, 120, 60),
            Color.FromArgb(240, 120, 60),
            
            Color.FromArgb(60, 180, 60),
            Color.FromArgb(120, 180, 60),
            Color.FromArgb(180, 180, 60),
            Color.FromArgb(240, 180, 60),
            
            Color.FromArgb(60, 240, 60),
            Color.FromArgb(120, 240, 60),
            Color.FromArgb(180, 240, 60),
            Color.FromArgb(240, 240, 60),

            Color.FromArgb(60, 60, 120),
            Color.FromArgb(120, 60, 120),
            Color.FromArgb(180, 60, 120),
            Color.FromArgb(240, 60, 120),

            Color.FromArgb(60, 120, 120),
            Color.FromArgb(120, 120, 120),
            Color.FromArgb(180, 120, 120),
            Color.FromArgb(240, 120, 120),
            
            Color.FromArgb(60, 180, 120),
            Color.FromArgb(120, 180, 120),
            Color.FromArgb(180, 180, 120),
            Color.FromArgb(240, 180, 120),
            
            Color.FromArgb(60, 240, 120),
            Color.FromArgb(120, 240, 120),
            Color.FromArgb(180, 240, 120),
            Color.FromArgb(240, 240, 120),

            Color.FromArgb(60, 60, 180),
            Color.FromArgb(120, 60, 180),
            Color.FromArgb(180, 60, 180),
            Color.FromArgb(240, 60, 180),

            Color.FromArgb(60, 120, 180),
            Color.FromArgb(120, 120, 180),
            Color.FromArgb(180, 120, 180),
            Color.FromArgb(240, 120, 180),
            
            Color.FromArgb(60, 180, 180),
            Color.FromArgb(120, 180, 180),
            Color.FromArgb(180, 180, 180),
            Color.FromArgb(240, 180, 180),
            
            Color.FromArgb(60, 240, 180),
            Color.FromArgb(120, 240, 180),
            Color.FromArgb(180, 240, 180),
            Color.FromArgb(240, 240, 180),

            Color.FromArgb(60, 60, 240),
            Color.FromArgb(120, 60, 240),
            Color.FromArgb(180, 60, 240),
            Color.FromArgb(240, 60, 240),

            Color.FromArgb(60, 120, 240),
            Color.FromArgb(120, 120, 240),
            Color.FromArgb(180, 120, 240),
            Color.FromArgb(240, 120, 240),
            
            Color.FromArgb(60, 180, 240),
            Color.FromArgb(120, 180, 240),
            Color.FromArgb(180, 180, 240),
            Color.FromArgb(240, 180, 240),
            
            Color.FromArgb(60, 240, 240),
            Color.FromArgb(120, 240, 240),
            Color.FromArgb(180, 240, 240),
            Color.FromArgb(240, 240, 240),

        };

        private World m_pWorld = null;

        public enum VisType
        { 
            LandType,
            Humidity,
            RacesNative,
            RacesStates
        }

        private VisType m_eMode = VisType.LandType;

        public VisType Mode
        {
            get { return m_eMode; }
            set 
            {
                if (m_eMode != value)
                {
                    m_eMode = value;
                    DrawMap();
                }
            }
        }

        private bool m_bShowRoads = true;

        public bool ShowRoads
        {
            get { return m_bShowRoads; }
            set 
            {
                if (m_bShowRoads != value)
                {
                    m_bShowRoads = value;
                    DrawMap();
                }
            }
        }

        private bool m_bShowStates = true;

        public bool ShowStates
        {
            get { return m_bShowStates; }
            set
            {
                if (m_bShowStates != value)
                {
                    m_bShowStates = value;
                    DrawMap();
                }
            }
        }

        private bool m_bShowLocations = true;

        public bool ShowLocations
        {
            get { return m_bShowLocations; }
            set
            {
                if (m_bShowLocations != value)
                {
                    m_bShowLocations = value;
                    DrawMap();
                }
            }
        }

        private bool m_bShowProvincies = true;

        public bool ShowProvincies
        {
            get { return m_bShowProvincies; }
            set
            {
                if (m_bShowProvincies != value)
                {
                    m_bShowProvincies = value;
                    DrawMap();
                }
            }
        }

        private bool m_bUseCurves = false;

        public bool UseCurves
        {
            get { return m_bUseCurves; }
            set 
            {
                if (m_bUseCurves != value)
                {
                    m_bUseCurves = value;
                    DrawMap();
                }
            }
        }

        private bool m_bShowLocationsBorders = false;

        public bool ShowLocationsBorders
        {
            get { return m_bShowLocationsBorders; }
            set
            {
                if (m_bShowLocationsBorders != value)
                {
                    m_bShowLocationsBorders = value;
                    DrawMap();
                }
            }
        }

        private bool m_bShowLandMasses = false;

        public bool ShowLandMasses
        {
            get { return m_bShowLandMasses; }
            set
            {
                if (m_bShowLandMasses != value)
                {
                    m_bShowLandMasses = value;
                    DrawMap();
                }
            }
        }

        private bool m_bShowLands = false;

        public bool ShowLands
        {
            get { return m_bShowLands; }
            set 
            {
                if (m_bShowLands != value)
                {
                    m_bShowLands = value;
                    DrawMap();
                }
            }
        }

        private float GetScaledX(float fX)
        {
            //if (fX < -m_pWorld.m_cGrid.RX)
            //    fX += m_pWorld.m_cGrid.RX * 2;
            //if (fX >= m_pWorld.m_cGrid.RX)
            //    fX -= m_pWorld.m_cGrid.RX * 2;
            return m_fkX + (m_pWorld.m_cGrid.RX + fX) * m_fkX;
        }

        private float GetScaledY(float fY)
        {
            return m_fkY + (m_pWorld.m_cGrid.RY + fY) * m_fkY;
        }

        private PointF GetScaledPoint(IPointF pPoint, float fDX)
        {
            return new PointF(GetScaledX(pPoint.X + fDX), GetScaledY(pPoint.Y));
        }

        private PointF GetScaledPoint(PointF pPoint, float fDX)
        {
            return new PointF(GetScaledX(pPoint.X + fDX), GetScaledY(pPoint.Y));
        }

        private PointF[] BuildBorder(Line pFirstLine, float fShift, out bool bCross, out bool[,] aQuadrants)
        {
            bCross = false;

            aQuadrants = new bool[8, 8];

            List<PointF> cBorder = new List<PointF>();
            Line pLine = pFirstLine;
            cBorder.Add(GetScaledPoint(pLine.m_pPoint1, fShift));
            float fLastPointX = pLine.m_pPoint1.X + fShift;
            do
            {
                int iQuad1X = (int)(4 * (pLine.m_pPoint1.X + m_pWorld.m_cGrid.RX) / m_pWorld.m_cGrid.RX);
                int iQuad1Y = (int)(4 * (pLine.m_pPoint1.Y + m_pWorld.m_cGrid.RY) / m_pWorld.m_cGrid.RY);

                if (iQuad1X >= 0 && iQuad1X < 8 && iQuad1Y >= 0 && iQuad1Y < 8)
                    aQuadrants[iQuad1X, iQuad1Y] = true;

                int iQuad2X = (int)(4 * (pLine.m_pPoint2.X + m_pWorld.m_cGrid.RX) / m_pWorld.m_cGrid.RX);
                int iQuad2Y = (int)(4 * (pLine.m_pPoint2.Y + m_pWorld.m_cGrid.RY) / m_pWorld.m_cGrid.RY);

                if (iQuad2X >= 0 && iQuad2X < 8 && iQuad2Y >= 0 && iQuad2Y < 8)
                    aQuadrants[iQuad2X, iQuad2Y] = true;

                float fDX = fShift;
                if (Math.Abs(fLastPointX - pLine.m_pPoint2.X - fShift) > m_pWorld.m_cGrid.RX)
                {
                    fDX += fLastPointX < fShift ? -m_pWorld.m_cGrid.RX * 2 : m_pWorld.m_cGrid.RX * 2;
                    bCross = true;
                }
                if (pLine.m_pPoint2.X > m_pWorld.m_cGrid.RX ||
                    pLine.m_pPoint2.X < -m_pWorld.m_cGrid.RX)
                    bCross = true;
                cBorder.Add(GetScaledPoint(pLine.m_pPoint2, fDX));

                fLastPointX = pLine.m_pPoint2.X + fDX;

                //if (Math.Abs(pLine.m_pPoint2.X - pLine.m_pNext.m_pPoint2.X) > World.RX / 5 ||
                //    Math.Abs(pLine.m_pPoint2.Y - pLine.m_pNext.m_pPoint2.Y) > World.RY / 5)
                //    throw new Exception();

                pLine = pLine.m_pNext;
            }
            while (pLine != pFirstLine);

            return cBorder.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cFirstLines"></param>
        /// <param name="bMirror">Строить ли отражения для зацикленного мира. Нужно при отрисовке, но нельзя при определении описывающего прямоугольника.</param>
        /// <returns></returns>
        private PointF[][] BuildPath(List<Line> cFirstLines, bool bMirror, out MapQuadrant[] aQuadrants)
        {
            bool[,] aQuadsAll = new bool[8, 8];
            List<PointF[]> cPath = new List<PointF[]>();
            foreach (Line pFirstLine in cFirstLines)
            {
                bool bCross;
                bool[,] aQuads;
                cPath.Add(BuildBorder(pFirstLine, 0, out bCross, out aQuads));

                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (aQuads[i, j])
                            aQuadsAll[i, j] = true;

                //if (m_bUseCurves)
                //    pPath.AddClosedCurve(cBorder.ToArray());
                //else
                //    pPath.AddPolygon(cBorder.ToArray());

                if (m_pWorld.m_cGrid.m_bCycled && bMirror && bCross)
                {
                    if (pFirstLine.m_pPoint1.X > 0)
                        cPath.Add(BuildBorder(pFirstLine, -m_pWorld.m_cGrid.RX * 2, out bCross, out aQuads));
                    else
                        cPath.Add(BuildBorder(pFirstLine, m_pWorld.m_cGrid.RX * 2, out bCross, out aQuads));

                    for (int i = 0; i < 8; i++)
                        for (int j = 0; j < 8; j++)
                            if (aQuads[i, j])
                                aQuadsAll[i, j] = true;

                    //if (m_bUseCurves)
                    //    pPath.AddClosedCurve(cBorder.ToArray());
                    //else
                    //    pPath.AddPolygon(cBorder.ToArray());
                }
            }

            List<MapQuadrant> cQuadrants = new List<MapQuadrant>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (aQuadsAll[i, j])
                        cQuadrants.Add(m_aQuadrants[i, j]);
            aQuadrants = cQuadrants.ToArray();

            return cPath.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cFirstLines"></param>
        /// <param name="bMirror">Строить ли отражения для зацикленного мира. Нужно при отрисовке, но нельзя при определении описывающего прямоугольника.</param>
        /// <returns></returns>
        private PointF[][] BuildPath(Line pFirstLine, bool bMirror, out MapQuadrant[] aQuadrants)
        {
            bool[,] aQuadsAll = new bool[8, 8];
            List<PointF[]> cPath = new List<PointF[]>();

            bool bCross;
            bool[,] aQuads;
            cPath.Add(BuildBorder(pFirstLine, 0, out bCross, out aQuads));

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (aQuads[i, j])
                        aQuadsAll[i, j] = true;

            //if (m_bUseCurves)
            //    cPath.AddClosedCurve(cBorder.ToArray());
            //else
            //    cPath.AddPolygon(cBorder.ToArray());

            if (m_pWorld.m_cGrid.m_bCycled && bMirror && bCross)
            {
                if (pFirstLine.m_pPoint1.X > 0)
                    cPath.Add(BuildBorder(pFirstLine, -m_pWorld.m_cGrid.RX * 2, out bCross, out aQuads));
                else
                    cPath.Add(BuildBorder(pFirstLine, m_pWorld.m_cGrid.RX * 2, out bCross, out aQuads));

                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (aQuads[i, j])
                            aQuadsAll[i, j] = true;

                //if (m_bUseCurves)
                //    cPath.AddClosedCurve(cBorder.ToArray());
                //else
                //    cPath.AddPolygon(cBorder.ToArray());
            }

            List<MapQuadrant> cQuadrants = new List<MapQuadrant>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (aQuadsAll[i, j])
                        cQuadrants.Add(m_aQuadrants[i, j]);
            aQuadrants = cQuadrants.ToArray();

            return cPath.ToArray();
        }

        private void DrawStateBorder(Graphics gr, State pState)
        {
            if (pState == null)
                return;

            GraphicsPath pPath;
            if (!m_cStateBorders.TryGetValue(pState, out pPath))
                return;

            Matrix pMatrix = new Matrix();
            int iDX = m_pDrawFrame.X;
            while (iDX < 0)
                iDX += m_iScaledMapWidth;
            while (iDX >= m_iScaledMapWidth)
                iDX -= m_iScaledMapWidth;
            pMatrix.Translate(-iDX, -m_pDrawFrame.Y);

            GraphicsPath pPath2 = (GraphicsPath)pPath.Clone();
            pPath2.Transform(pMatrix);

            gr.DrawPath(s_pRed3Pen, pPath2); 

            gr.DrawPath(s_pBlack3DotPen, pPath2);

            RectangleF pBounds = pPath2.GetBounds();

            if (pBounds.X < 0)
            {
                Matrix pMatrixMirror = new Matrix();
                pMatrixMirror.Translate(m_iScaledMapWidth, 0);

                GraphicsPath pPath3 = (GraphicsPath)pPath2.Clone();
                pPath3.Transform(pMatrixMirror);

                gr.DrawPath(s_pRed3Pen, pPath3);
                gr.DrawPath(s_pBlack3DotPen, pPath3);
            }

            if (pBounds.X + pBounds.Width > m_pDrawFrame.Width)
            {
                Matrix pMatrixMirror = new Matrix();
                pMatrixMirror.Translate(-m_iScaledMapWidth, 0);

                GraphicsPath pPath3 = (GraphicsPath)pPath2.Clone();
                pPath3.Transform(pMatrixMirror);

                gr.DrawPath(s_pRed3Pen, pPath3);
                gr.DrawPath(s_pBlack3DotPen, pPath3);
            }
        }

        private PointF[] BuildPathLine(TransportationLink pPath, float fShift, out bool bCross, out bool[,] aQuadrants)
        {
            bCross = false;
            aQuadrants = new bool[8, 8];

            List<PointF> cRoadLine = new List<PointF>();
            float fLastPointX = pPath.m_aPoints[0].X + fShift;
            foreach (PointF pPoint in pPath.m_aPoints)
            {
                float fDX = fShift;
                if (Math.Abs(fLastPointX - pPoint.X - fShift) > m_pWorld.m_cGrid.RX)
                {
                    fDX += fLastPointX < fShift ? -m_pWorld.m_cGrid.RX * 2 : m_pWorld.m_cGrid.RX * 2;
                    bCross = true;
                }
                if (pPoint.X > m_pWorld.m_cGrid.RX ||
                    pPoint.X < -m_pWorld.m_cGrid.RX)
                    bCross = true;
                
                cRoadLine.Add(GetScaledPoint(pPoint, fDX));

                int iQuadX = (int)(4 * (pPoint.X + m_pWorld.m_cGrid.RX) / m_pWorld.m_cGrid.RX);
                int iQuadY = (int)(4 * (pPoint.Y + m_pWorld.m_cGrid.RY) / m_pWorld.m_cGrid.RY);

                if (iQuadX >= 0 && iQuadX < 8 && iQuadY >= 0 && iQuadY < 8)
                    aQuadrants[iQuadX, iQuadY] = true;

                fLastPointX = pPoint.X + fDX;
            }

            return cRoadLine.ToArray();
        }

        private void DrawPathLine(Graphics gr, List<PointF> cPathLine, Pen pPen)
        {
            if (m_bUseCurves)
                gr.DrawCurve(pPen, cPathLine.ToArray());
            else
                gr.DrawLines(pPen, cPathLine.ToArray());
        }

        private Dictionary<TransportationNode[], Pen> m_cPaths = new Dictionary<TransportationNode[],Pen>();

        public void ClearPath()
        {
            m_cPaths.Clear();
            DrawMap();
        }

        public void AddPath(TransportationNode[] aPath, Color pColor)
        {
            Pen pPen = new Pen(pColor, 5);
            pPen.DashPattern = new float[] {2, 3};
            m_cPaths[aPath] = pPen;
            DrawMap();
        }

        internal static Pen s_pDarkGrey3Pen = new Pen(Color.DarkGray, 3);
        internal static Pen s_pAqua2Pen = new Pen(Color.Aqua, 2);
        internal static Pen s_pAqua1Pen = new Pen(Color.Aqua, 1);
        internal static Pen s_pBlack1Pen = new Pen(Color.Black, 1);
        internal static Pen s_pBlack2Pen = new Pen(Color.Black, 2);
        internal static Pen s_pRed2Pen = new Pen(Color.Red, 2);
        internal static Pen s_pRed3Pen = new Pen(Color.Red, 3);
        internal static Pen s_pAqua1DotPen = new Pen(Color.Aqua, 1);
        internal static Pen s_pBlack1DotPen = new Pen(Color.Black, 1);
        internal static Pen s_pWhite2Pen = new Pen(Color.White, 2);
        internal static Pen s_pBlack3Pen = new Pen(Color.Black, 3);
        internal static Pen s_pBlack3DotPen = new Pen(Color.Black, 3);

        internal enum RoadType
        {
            Back,
            LandRoad1,
            LandRoad2,
            LandRoad3,
            SeaRoute1,
            SeaRoute2,
            SeaRoute3,
        }

        private void AddRoad(TransportationLink pRoad, bool bBack)
        {
            if (pRoad.RoadLevel == 0)
                return;
                
            //if (m_fScale <= 2 && pRoad.RoadLevel <= 1)
            //    return;

            //if (m_fScale <= 2 && pRoad.RoadLevel == 2)
            //    return;

            RoadType eRoadType = RoadType.LandRoad2;

            if (bBack)
            {
                if (pRoad.m_bSea || pRoad.m_bEmbark)
                    return;

                if (pRoad.RoadLevel != 3)
                    return;

                eRoadType = RoadType.Back;
            }
            else
            {
                switch (pRoad.RoadLevel)
                {
                    case 1:
                        if (pRoad.m_bSea || pRoad.m_bEmbark)
                            eRoadType = RoadType.SeaRoute1;
                        else
                            eRoadType = RoadType.LandRoad1;
                        break;
                    case 2:
                        if (pRoad.m_bSea || pRoad.m_bEmbark)
                            eRoadType = RoadType.SeaRoute2;
                        else
                            eRoadType = RoadType.LandRoad2;
                        break;
                    case 3:
                        if (pRoad.m_bSea || pRoad.m_bEmbark)
                            eRoadType = RoadType.SeaRoute3;
                        else
                            eRoadType = RoadType.LandRoad3;
                        break;
                }
            }
            bool[,] aQuadrants;
            PointF[][] aLinks = GetTransportationLink(pRoad, out aQuadrants);

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (aQuadrants[i, j])
                    {
                        //if (!m_aQuadrants[i, j].m_cRoadsMap.ContainsKey(eRoadType))
                        //    m_aQuadrants[i, j].m_cRoadsMap[eRoadType] = new GraphicsPath();

                        foreach (var aLink in aLinks)
                        {
                            m_aQuadrants[i, j].m_cRoadsMap[eRoadType].StartFigure();
                            //m_aQuadrants[i, j].m_cRoadsMap[eRoadType].AddLines(aLink);
                            m_aQuadrants[i, j].m_cRoadsMap[eRoadType].AddCurve(aLink);
                        }
                    }
        }

        private PointF[][] GetTransportationLink(TransportationLink pRoad, out bool[,] aQuadrants)
        {
            aQuadrants = new bool[8, 8];

            bool[,] aQuads;
            List<PointF[]> cPathLines = new List<PointF[]>();

            bool bCross;
            cPathLines.Add(BuildPathLine(pRoad, 0, out bCross, out aQuads));

            if (m_pWorld.m_cGrid.m_bCycled && bCross)
            {
                if (pRoad.m_aPoints[0].X > 0)
                {
                    cPathLines.Add(BuildPathLine(pRoad, -m_pWorld.m_cGrid.RX * 2, out bCross, out aQuads));
                }
                else
                {
                    cPathLines.Add(BuildPathLine(pRoad, m_pWorld.m_cGrid.RX * 2, out bCross, out aQuads));
                }
            }

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (aQuads[i, j])
                        aQuadrants[i, j] = true; 
            
            return cPathLines.ToArray();
        }

        private void AddLocationSign(LocationX pLoc)
        {
            float fPointX = GetScaledX(pLoc.m_pCenter.X);
            float fPointY = GetScaledY(pLoc.m_pCenter.Y);

            int iQuadX = (int)(4 * (pLoc.m_pCenter.X + m_pWorld.m_cGrid.RX) / m_pWorld.m_cGrid.RX);
            int iQuadY = (int)(4 * (pLoc.m_pCenter.Y + m_pWorld.m_cGrid.RY) / m_pWorld.m_cGrid.RY);

            if (iQuadX < 0 || iQuadX >= 8 || iQuadY < 0 || iQuadY >= 8)
                return;

            //gr.FillEllipse(new SolidBrush(Color.White), (int)(m_fkX / 2 + (m_pWorld.m_iWorldScale * 1.5 + pLoc.m_pCenter.X) * m_fkX),
            //                                  (int)(m_fkY / 2 + (m_pWorld.m_iWorldScale + pLoc.m_pCenter.Y) * m_fkY), m_fkX, m_fkY);
            //gr.DrawEllipse(new Pen(Color.Black), (int)(m_fkX / 2 + (m_pWorld.m_iWorldScale * 1.5 + pLoc.m_pCenter.X) * m_fkX),
            //                                  (int)(m_fkY / 2 + (m_pWorld.m_iWorldScale + pLoc.m_pCenter.Y) * m_fkY), m_fkX, m_fkY);


            switch (pLoc.m_eType)
            {
                case RegionType.Peak:
                    m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignPeak(m_fkX, fPointX, fPointY, ""));
                    break;
                case RegionType.Volcano:
                    m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignVolkano(m_fkX, fPointX, fPointY, ""));
                    break;
            }

            if (pLoc.m_pSettlement != null)
            {
                if (pLoc.m_pSettlement.m_iRuinsAge > 0)
                {
                    m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignRuin(m_fkX, fPointX, fPointY, ""));
                }
                else
                {
                    switch (pLoc.m_pSettlement.m_pInfo.m_eSize)
                    {
                        case SettlementSize.Capital:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignCapital(m_fkX, fPointX, fPointY, pLoc.m_pSettlement.m_sName));
                            break;
                        case SettlementSize.City:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignCity(m_fkX, fPointX, fPointY, pLoc.m_pSettlement.m_sName));
                            break;
                        case SettlementSize.Town:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignTown(m_fkX, fPointX, fPointY, pLoc.m_pSettlement.m_sName));
                            break;
                        case SettlementSize.Village:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignVillage(m_fkX, fPointX, fPointY, pLoc.m_pSettlement.m_sName, (pLoc.Owner as LandX).Type.m_pBrush));
                            break;
                        case SettlementSize.Hamlet:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignVillage(m_fkX, fPointX, fPointY, pLoc.m_pSettlement.m_sName, (pLoc.Owner as LandX).Type.m_pBrush));
                            break;
                        case SettlementSize.Fort:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignFort(m_fkX, fPointX, fPointY, pLoc.m_pSettlement.m_sName));
                            break;
                    }
                }
            }
            else
            {
                if (pLoc.m_pBuilding != null)
                {
                    switch (pLoc.m_pBuilding.m_eType)
                    {
                        case BuildingType.Lair:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignLair(m_fkX, fPointX, fPointY, ""));
                            break;
                        case BuildingType.Hideout:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignHideout(m_fkX, fPointX, fPointY, ""));
                            break;
                    }
                }
            }
        }

        private bool m_bRebuild = false;

        public void DrawMap(WorldMap pMasterMap)
        {
            m_pMasterMap = pMasterMap;
            m_pWorld = m_pMasterMap.m_pWorld;

            m_bRebuild = true;

            m_cRaceColorsID = m_pMasterMap.m_cRaceColorsID;

            MapScale = 1;

            SinchronizeDrawFrame(pMasterMap);
        }

        private void SinchronizeDrawFrame(WorldMap pMap)
        {
            if (pMap == null || pMap.m_pWorld == null || m_pWorld == null)
                return;

            m_pDrawFrame.X = (int)(m_fkX * pMap.m_pDrawFrame.X / pMap.m_fkX) - 1;
            m_pDrawFrame.Y = (int)(m_fkY * pMap.m_pDrawFrame.Y / pMap.m_fkY) - 1;
            m_pDrawFrame.Width = (int)(m_fkX * pMap.m_pDrawFrame.Width / pMap.m_fkX);
            m_pDrawFrame.Height = (int)(m_fkY * pMap.m_pDrawFrame.Height / pMap.m_fkY) + 1;

            if (m_pMasterMap == null)
                DrawMap();

            Refresh();
        }

        private WorldMap m_pMiniMap = null;

        public void AddMiniMapView(WorldMap pMiniMap)
        {
            m_pMiniMap = pMiniMap;

            if (m_pWorld != null)
                pMiniMap.DrawMap(this);
        }

        public void DrawMap(World pWorld, float fScale)
        {
            m_pWorld = pWorld;

            if (m_cRaceColorsID.Count == 0)
            {
                List<int> cUsedColors = new List<int>();
                foreach (Race pRace in World.m_cAllRaces)
                {
                    int iIndex;
                    do
                    {
                        iIndex = Rnd.Get(m_aRaceColorsTemplate.Length);
                    }
                    while (cUsedColors.Contains(iIndex));

                    cUsedColors.Add(iIndex);
                    m_cRaceColorsID[pRace] = new SolidBrush(m_aRaceColorsTemplate[iIndex]);
                }
            }

            m_pDrawFrame.X = 0;
            m_pDrawFrame.Y = 0;

            m_bRebuild = true;

            MapScale = fScale;

            if (m_pMiniMap != null)
                m_pMiniMap.DrawMap(this);
        }

        private Dictionary<ContinentX, GraphicsPath> m_cContinentBorders = new Dictionary<ContinentX, GraphicsPath>();
        private Dictionary<AreaX, GraphicsPath> m_cAreaBorders = new Dictionary<AreaX, GraphicsPath>();
        private Dictionary<Province, GraphicsPath> m_cProvinceBorders = new Dictionary<Province, GraphicsPath>();
        private Dictionary<LandMass<LandX>, GraphicsPath> m_cLandMassBorders = new Dictionary<LandMass<LandX>, GraphicsPath>();
        private Dictionary<LandX, GraphicsPath> m_cLandBorders = new Dictionary<LandX, GraphicsPath>();
        private Dictionary<LocationX, GraphicsPath> m_cLocationBorders = new Dictionary<LocationX, GraphicsPath>();
        private Dictionary<State, GraphicsPath> m_cStateBorders = new Dictionary<State, GraphicsPath>();

        private void ScalePaths(float fScale)
        {
            if (fScale == 1)
                return;

            DateTime pTime1 = DateTime.Now;

            Matrix pMatrix = new Matrix();
            pMatrix.Scale(fScale, fScale);

            foreach (var pPair in m_cContinentBorders)
                pPair.Value.Transform(pMatrix);

            foreach (var pPair in m_cAreaBorders)
                pPair.Value.Transform(pMatrix);

            foreach (var pPair in m_cProvinceBorders)
                pPair.Value.Transform(pMatrix);

            foreach (var pPair in m_cLandMassBorders)
                pPair.Value.Transform(pMatrix);

            foreach (var pPair in m_cLandBorders)
                pPair.Value.Transform(pMatrix);

            foreach (var pPair in m_cLocationBorders)
                pPair.Value.Transform(pMatrix);

            foreach (var pPair in m_cStateBorders)
                pPair.Value.Transform(pMatrix);

            foreach (var pQuad in m_aQuadrants)
            {
                pQuad.ScalePaths(fScale);
            }

            DateTime pTime2 = DateTime.Now;
        }
        
        private void PrebuildPaths()
        {
            if (!m_bRebuild)
                return;

            DateTime pTime1 = DateTime.Now;

            m_cContinentBorders.Clear();
            m_cAreaBorders.Clear();

            foreach (MapQuadrant pQuad in m_aQuadrants)
                pQuad.Clear(m_pMasterMap != null);

            if (m_pMasterMap == null)
            {
                m_cProvinceBorders.Clear();
                m_cLandMassBorders.Clear();
                m_cLandBorders.Clear();
                m_cLocationBorders.Clear();
                m_cStateBorders.Clear();
            }

            PointF[][] aPoints;
            GraphicsPath pPath;
            MapQuadrant[] aQuads;

            if (m_pMasterMap == null)
            {
                foreach (LandMass<LandX> pLandMass in m_pWorld.m_aLandMasses)
                {
                    aPoints = BuildPath(pLandMass.m_cFirstLines, true, out aQuads);
                    pPath = new GraphicsPath();
                    foreach (var aPts in aPoints)
                    {
                        pPath.AddPolygon(aPts);
                        foreach(MapQuadrant pQuad in aQuads)
                            pQuad.m_pLandMassesPath.AddPolygon(aPts);
                    }
                    m_cLandMassBorders[pLandMass] = pPath;
                    foreach (LandX pLand in pLandMass.m_cContents)
                    {
                        aPoints = BuildPath(pLand.m_cFirstLines, true, out aQuads);
                        pPath = new GraphicsPath();
                        foreach (var aPts in aPoints)
                        {
                            pPath.AddPolygon(aPts);

                            Brush pBrush = pLand.IsWater ? pLand.Type.m_pBrush : m_aHumidity[pLand.Humidity];

                            foreach (MapQuadrant pQuad in aQuads)
                            {
                                pQuad.m_pLandsPath.AddPolygon(aPts);

                                if (!pQuad.m_cHumidityMap.ContainsKey(pBrush))
                                    pQuad.m_cHumidityMap[pBrush] = new GraphicsPath();
                                pQuad.m_cHumidityMap[pBrush].AddPolygon(aPts);
                            }
                        }
                        m_cLandBorders[pLand] = pPath;

                        foreach (LocationX pLoc in pLand.m_cContents)
                        {
                            aPoints = BuildPath(pLoc.m_pFirstLine, true, out aQuads);
                            pPath = new GraphicsPath();
                            foreach (var aPts in aPoints)
                            {
                                pPath.AddPolygon(aPts);
                                foreach (MapQuadrant pQuad in aQuads)
                                    pQuad.m_pLocationsPath.AddPolygon(aPts);
                            }
                            m_cLocationBorders[pLoc] = pPath;
                            AddLocationSign(pLoc);
                        }

                    }
                }
            }
            
            foreach (ContinentX pContinent in m_pWorld.m_aContinents)
            {
                aPoints = BuildPath(pContinent.m_cFirstLines, true, out aQuads);
                pPath = new GraphicsPath();
                foreach (var aPts in aPoints)
                {
                    pPath.AddPolygon(aPts);
                    foreach (MapQuadrant pQuad in aQuads)
                        pQuad.m_pContinentsPath.AddPolygon(aPts);
                }
                m_cContinentBorders[pContinent] = pPath;

                if (m_pMasterMap == null)
                {
                    foreach (State pState in pContinent.m_cStates)
                    {
                        aPoints = BuildPath(pState.m_cFirstLines, true, out aQuads);
                        pPath = new GraphicsPath();
                        foreach (var aPts in aPoints)
                        {
                            pPath.AddPolygon(aPts);
                            foreach (MapQuadrant pQuad in aQuads)
                                pQuad.m_pStatesPath.AddPolygon(aPts);
                        }
                        m_cStateBorders[pState] = pPath;
                    }
                }

                foreach (AreaX pArea in pContinent.m_cAreas)
                {
                    aPoints = BuildPath(pArea.m_cFirstLines, true, out aQuads);
                    pPath = new GraphicsPath();
                    foreach (var aPts in aPoints)
                    {
                        pPath.AddPolygon(aPts);

                        if (pArea.m_pType != LandTypes<LandTypeInfoX>.Plains)
                        {
                            foreach (MapQuadrant pQuad in aQuads)
                            {
                                if (!pQuad.m_cAreaMap.ContainsKey(pArea.m_pType.m_pBrush))
                                    pQuad.m_cAreaMap[pArea.m_pType.m_pBrush] = new GraphicsPath();
                                pQuad.m_cAreaMap[pArea.m_pType.m_pBrush].AddPolygon(aPts);
                            }
                        }

                        if (m_pMasterMap == null)
                        {
                            if (pArea.m_pRace != null)
                            {
                                Brush pBrush = m_cRaceColorsID[pArea.m_pRace];
                                foreach (MapQuadrant pQuad in aQuads)
                                {
                                    if (!pQuad.m_cNativesMap.ContainsKey(pBrush))
                                        pQuad.m_cNativesMap[pBrush] = new GraphicsPath();
                                    pQuad.m_cNativesMap[pBrush].AddPolygon(aPts);
                                }
                            }
                        }
                    }
                    m_cAreaBorders[pArea] = pPath;
                }
            }

            Matrix pMatrix = new Matrix();
            pMatrix.Translate(1, 1);
            
            foreach (MapQuadrant pQuad in m_aQuadrants)
            {
                pQuad.m_pContinentsPath2 = (GraphicsPath)pQuad.m_pContinentsPath.Clone();
                pQuad.m_pContinentsPath2.Transform(pMatrix);
            }

            if (m_pMasterMap == null)
            {
                foreach (Province pProvince in m_pWorld.m_aProvinces)
                {
                    aPoints = BuildPath(pProvince.m_cFirstLines, true, out aQuads);
                    pPath = new GraphicsPath();
                    foreach (var aPts in aPoints)
                    {
                        pPath.AddPolygon(aPts);
                        if (!pProvince.m_pCenter.IsWater)
                            foreach (MapQuadrant pQuad in aQuads)
                                pQuad.m_pProvinciesPath.AddPolygon(aPts);

                        Brush pBrush = m_cRaceColorsID[pProvince.m_pRace];
                        foreach (MapQuadrant pQuad in aQuads)
                        {
                            if (!pQuad.m_cNationsMap.ContainsKey(pBrush))
                                pQuad.m_cNationsMap[pBrush] = new GraphicsPath();
                            pQuad.m_cNationsMap[pBrush].AddPolygon(aPts);
                        }
                    }
                    m_cProvinceBorders[pProvince] = pPath;
                }

                foreach (TransportationLink pRoad in m_pWorld.m_cTransportGrid)
                {
                    AddRoad(pRoad, true);
                    AddRoad(pRoad, false);
                }
            }

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    float fQuadX = m_fkX * m_pWorld.m_cGrid.RX * i / 4;
                    float fQuadY = m_fkY * m_pWorld.m_cGrid.RY * j / 4;
                    m_aQuadrants[i, j].Normalize(fQuadX, fQuadY);
                }

            m_bRebuild = false;
            DateTime pTime2 = DateTime.Now;
        }

        public void DrawMap()
        {
            if (ActualMap == null)
                return;

            DateTime pTime1 = DateTime.Now;

            Graphics gr = Graphics.FromImage(ActualMap);

            gr.FillRectangle(new SolidBrush(LandTypes<LandTypeInfoX>.Sea.m_pColor), 0, 0, ActualMap.Width, ActualMap.Height);

            if (m_pWorld == null || m_pWorld.m_cGrid.m_aLocations.Length == 0)
                return;

            //координаты квадранта, в котором находится левый верхний угол отображаемого участка карты
            int iQuadX = (int)(m_pDrawFrame.X / m_fOneQuadWidth);
            int iQuadY = (int)(m_pDrawFrame.Y / m_fOneQuadHeight);

            //координаты левого верхнего угла отображаемого участка карты внутри квадранта, в котором он находится
            int iQuadDX = (int)(iQuadX * m_fOneQuadWidth) - m_pDrawFrame.X;
            int iQuadDY = (int)(iQuadY * m_fOneQuadHeight) - m_pDrawFrame.Y;

            if (m_pMasterMap != null)
            {
                iQuadX = 0;
                iQuadDX = 0;
                
                iQuadY = 0;
                iQuadDY = 0;
            }

            MapQuadrant[,] aVisibleQuads = new MapQuadrant[m_iQuadsWidth, m_iQuadsHeight];

            for (int i = 0; i < m_iQuadsWidth; i++)
            {
                int iQX = iQuadX + i;

                while (iQX >= 8)
                    iQX -= 8;

                while (iQX < 0)
                    iQX += 8;

                for (int j = 0; j < m_iQuadsHeight; j++)
                {
                    int iQY = iQuadY + j;

                    if (iQY < 0 || iQY >= 8)
                        continue;

                    aVisibleQuads[i,j] = m_aQuadrants[iQX, iQY];
                }
            }

            switch (m_eMode)
            {
                case VisType.LandType:
                    {
                        for (int i = 0; i < m_iQuadsWidth; i++)
                            for (int j = 0; j < m_iQuadsHeight; j++)
                                if (aVisibleQuads[i, j] != null)
                                    aVisibleQuads[i, j].DrawPath(gr, s_pBlack2Pen, aVisibleQuads[i, j].m_pContinentsPath2, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);
                        for (int i = 0; i < m_iQuadsWidth; i++)
                            for (int j = 0; j < m_iQuadsHeight; j++)
                                if (aVisibleQuads[i, j] != null)
                                    aVisibleQuads[i, j].FillPath(gr, LandTypes<LandTypeInfoX>.Plains.m_pBrush, aVisibleQuads[i, j].m_pContinentsPath, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);

                        for (int i = 0; i < m_iQuadsWidth; i++)
                            for (int j = 0; j < m_iQuadsHeight; j++)
                                if (aVisibleQuads[i, j] != null)
                                    foreach (var pArea in aVisibleQuads[i, j].m_cAreaMap)
                                        aVisibleQuads[i, j].FillPath(gr, pArea.Key, pArea.Value, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);
                    }
                    break;
                case VisType.Humidity:
                    {
                        for (int i = 0; i < m_iQuadsWidth; i++)
                            for (int j = 0; j < m_iQuadsHeight; j++)
                                if (aVisibleQuads[i, j] != null)
                                    foreach (var pHum in aVisibleQuads[i, j].m_cHumidityMap)
                                        aVisibleQuads[i, j].FillPath(gr, pHum.Key, pHum.Value, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);

                        for (int i = 0; i < m_iQuadsWidth; i++)
                            for (int j = 0; j < m_iQuadsHeight; j++)
                                if (aVisibleQuads[i, j] != null)
                                    aVisibleQuads[i, j].DrawPath(gr, Pens.Black, aVisibleQuads[i, j].m_pContinentsPath, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);
                    }
                    break;
                case VisType.RacesNative:
                    {
                        for (int i = 0; i < m_iQuadsWidth; i++)
                            for (int j = 0; j < m_iQuadsHeight; j++)
                                if (aVisibleQuads[i, j] != null)
                                    aVisibleQuads[i, j].DrawPath(gr, Pens.Black, aVisibleQuads[i, j].m_pContinentsPath, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);

                        for (int i = 0; i < m_iQuadsWidth; i++)
                            for (int j = 0; j < m_iQuadsHeight; j++)
                                if (aVisibleQuads[i, j] != null)
                                    foreach (var pNative in aVisibleQuads[i, j].m_cNativesMap)
                                        aVisibleQuads[i, j].FillPath(gr, pNative.Key, pNative.Value, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);
                    }
                    break;
                case VisType.RacesStates:
                    {
                        for (int i = 0; i < m_iQuadsWidth; i++)
                            for (int j = 0; j < m_iQuadsHeight; j++)
                                if (aVisibleQuads[i, j] != null)
                                    aVisibleQuads[i, j].DrawPath(gr, Pens.Black, aVisibleQuads[i, j].m_pContinentsPath, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);

                        for (int i = 0; i < m_iQuadsWidth; i++)
                            for (int j = 0; j < m_iQuadsHeight; j++)
                                if (aVisibleQuads[i, j] != null)
                                    foreach (var pNation in aVisibleQuads[i, j].m_cNationsMap)
                                        aVisibleQuads[i, j].FillPath(gr, pNation.Key, pNation.Value, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);
                    }
                    break;
            }

            if (m_bShowLocationsBorders)
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            aVisibleQuads[i, j].DrawPath(gr, Pens.DarkGray, aVisibleQuads[i, j].m_pLocationsPath, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);

            if (m_bShowLands)
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            aVisibleQuads[i, j].DrawPath(gr, Pens.Black, aVisibleQuads[i, j].m_pLandsPath, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);

            if (m_bShowProvincies)
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            aVisibleQuads[i, j].DrawPath(gr, s_pWhite2Pen, aVisibleQuads[i, j].m_pProvinciesPath, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);

            foreach (TransportationNode[] aPath in m_cPaths.Keys)
                DrawPath(gr, aPath, m_cPaths[aPath]);

            if (m_bShowRoads)
            {
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                    {
                        if (aVisibleQuads[i, j] != null)
                        {
                            aVisibleQuads[i, j].DrawPath(gr, s_pDarkGrey3Pen, aVisibleQuads[i, j].m_cRoadsMap[RoadType.Back], i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);
                            aVisibleQuads[i, j].DrawPath(gr, s_pBlack2Pen, aVisibleQuads[i, j].m_cRoadsMap[RoadType.LandRoad3], i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);
                            aVisibleQuads[i, j].DrawPath(gr, s_pAqua2Pen, aVisibleQuads[i, j].m_cRoadsMap[RoadType.SeaRoute3], i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);

                            aVisibleQuads[i, j].DrawPath(gr, s_pBlack1Pen, aVisibleQuads[i, j].m_cRoadsMap[RoadType.LandRoad2], i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);
                            aVisibleQuads[i, j].DrawPath(gr, s_pAqua1Pen, aVisibleQuads[i, j].m_cRoadsMap[RoadType.SeaRoute2], i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);

                            if (m_fScale > 2)
                            {
                                aVisibleQuads[i, j].DrawPath(gr, s_pBlack1DotPen, aVisibleQuads[i, j].m_cRoadsMap[RoadType.LandRoad1], i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);
                                aVisibleQuads[i, j].DrawPath(gr, s_pAqua1DotPen, aVisibleQuads[i, j].m_cRoadsMap[RoadType.SeaRoute1], i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);
                            }
                        }
                    }
            }

            if (m_bShowStates)
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            aVisibleQuads[i, j].DrawPath(gr, s_pBlack3Pen, aVisibleQuads[i, j].m_pStatesPath, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);

            if (m_bShowLocations)
            {
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            foreach (ILandMark pLandMark in aVisibleQuads[i, j].m_cLandmarks)
                                pLandMark.Draw(gr, m_fScale, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);
            }

            if (m_bShowLandMasses)
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            aVisibleQuads[i, j].DrawPath(gr, s_pRed2Pen, aVisibleQuads[i, j].m_pLandMassesPath, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY);

            //foreach (TransportationNode pNode in m_pWorld.m_cSeaTransportGrid.Values)
            //{
            //    foreach (TransportationLink pLink in pNode.m_pLinks.Values)
            //    {
            //        gr.DrawLine(new Pen(Color.Gray), (int)(fkX + (m_pWorld.m_iWorldScale * 1.5 + pNode.X) * fkX),
            //            (int)(fkY + (m_pWorld.m_iWorldScale + pNode.Y) * fkY),
            //            (int)(fkX + (m_pWorld.m_iWorldScale * 1.5 + pLink.Destination.X) * fkX),
            //            (int)(fkY + (m_pWorld.m_iWorldScale + pLink.Destination.Y) * fkY));
            //    }
            //}
            //foreach (TransportationNode pNode in m_pWorld.m_cGroundTransportGrid.Values)
            //{
            //    foreach (TransportationLink pLink in pNode.m_pLinks.Values)
            //    {
            //        gr.DrawLine(new Pen(Color.Gray), (int)(fkX + (m_pWorld.m_iWorldScale * 1.5 + pNode.X) * fkX),
            //            (int)(fkY + (m_pWorld.m_iWorldScale + pNode.Y) * fkY),
            //            (int)(fkX + (m_pWorld.m_iWorldScale * 1.5 + pLink.Destination.X) * fkX),
            //            (int)(fkY + (m_pWorld.m_iWorldScale + pLink.Destination.Y) * fkY));
            //    }
            //}

            if(m_pSelectedState != null)
                DrawStateBorder(gr, m_pSelectedState);

            Refresh();

            DateTime pTime2 = DateTime.Now;
        }

        private void DrawPath(Graphics gr, TransportationNode[] aPath, Pen pPen)
        {
            GraphicsPath pPath = new GraphicsPath();
            TransportationNode pLastNode = null;
            foreach (TransportationNode pNode in aPath)
            {
                if (pLastNode != null)
                {
                    bool[,] aQuads;
                    PointF[][] aLinks = GetTransportationLink(pLastNode.m_cLinks[pNode], out aQuads);
                    foreach (var aLink in aLinks)
                    {
                        pPath.StartFigure();
                        pPath.AddLines(aLink);
                    }
                }

                pLastNode = pNode;
            }

            Matrix pMatrix = new Matrix();
            int iDX = m_pDrawFrame.X;
            while (iDX < 0)
                iDX += m_iScaledMapWidth;
            while (iDX >= m_iScaledMapWidth)
                iDX -= m_iScaledMapWidth;
            pMatrix.Translate(-iDX, -m_pDrawFrame.Y);

            pPath.Transform(pMatrix); 
            
            gr.DrawPath(pPen, pPath);

            RectangleF pBounds = pPath.GetBounds();

            if (pBounds.X < 0)
            {
                Matrix pMatrixMirror = new Matrix();
                pMatrixMirror.Translate(m_iScaledMapWidth, 0);

                GraphicsPath pPath2 = (GraphicsPath)pPath.Clone();
                pPath2.Transform(pMatrixMirror);

                gr.DrawPath(pPen, pPath2);
            }

            if (pBounds.X + pBounds.Width > m_pDrawFrame.Width)
            {
                Matrix pMatrixMirror = new Matrix();
                pMatrixMirror.Translate(-m_iScaledMapWidth, 0);

                GraphicsPath pPath2 = (GraphicsPath)pPath.Clone();
                pPath2.Transform(pMatrixMirror);

                gr.DrawPath(pPen, pPath2);
            }
        }

        private Brush GetHumidityColor(int iHumidity)
        {
            KColor color = new KColor();
            color.RGB = Color.LightBlue;
            color.Lightness = 1.0 - (double)iHumidity / 200;
            return new SolidBrush(color.RGB); //Color.FromArgb(0, 0, 255 - 128 * iHumidity / 100);
        }

        private ContinentX m_pFocusedContinent = null;

        public ContinentX ContinentInFocus
        {
            get { return m_pFocusedContinent; }
        }

        public class StateSelectedEventArgs : EventArgs
        {
            private State m_pState;
            public State State
            {
                get { return m_pState; }
            }

            public int m_iX;
            public int m_iY;

            public StateSelectedEventArgs(State pState, int iX, int iY)
            {
                m_pState = pState;

                m_iX = iX;
                m_iY = iY;
            }
        }

        public event EventHandler<StateSelectedEventArgs> StateSelectedEvent;

        private Province m_pFocusedProvince = null;

        private State m_pFocusedState = null;

        private State m_pSelectedState = null;

        public State SelectedState
        {
            get { return m_pSelectedState; }
            set 
            {
                if (value != m_pSelectedState)
                {
                    m_pSelectedState = value;
                    DrawMap();

                    Refresh();

                    FireSelectedStateEvent();

                    if (m_pMiniMap != null)
                        m_pMiniMap.SelectedState = m_pSelectedState;
                }
            }
        }

        public void FireSelectedStateEvent()
        {
            if (m_pSelectedState == null)
                return;

            MapQuadrant[] aQuads;
            PointF[][] aPoints = BuildPath(m_pSelectedState.m_cFirstLines, false, out aQuads);
            GraphicsPath pPath = new GraphicsPath();
            foreach (var aPts in aPoints)
                pPath.AddPolygon(aPts);
            RectangleF pRect = pPath.GetBounds();

            // Copy to a temporary variable to be thread-safe.
            EventHandler<StateSelectedEventArgs> temp = StateSelectedEvent;
            if (temp != null)
                temp(this, new StateSelectedEventArgs(m_pSelectedState, (int)(pRect.Left + pRect.Width / 2), (int)(pRect.Top + pRect.Height / 2)));
        }

        public Point GetCentralPoint(State pState)
        {
            MapQuadrant[] aQuads;
            PointF[][] aPoints = BuildPath(pState.m_cFirstLines, false, out aQuads);
            GraphicsPath pPath = new GraphicsPath();
            foreach (var aPts in aPoints)
                pPath.AddPolygon(aPts);
            RectangleF pRect = pPath.GetBounds();

            return new Point((int)(pRect.Left + pRect.Width / 2), (int)(pRect.Top + pRect.Height / 2));
        }

        private LandX m_pFocusedLand = null;

        public LandX LandInFocus
        {
            get { return m_pFocusedLand; }
        }

        private LocationX m_pFocusedLocation = null;

        public LocationX LocationInFocus
        {
            get { return m_pFocusedLocation; }
        }

        private void WorldMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (timer1.Enabled)
                return;

            if (m_pMasterMap != null)
                return;

            if (m_pWorld == null)
                return;

            if (e.Button != MouseButtons.None)
                return;

            int iX = e.X + m_pDrawFrame.X - m_iShiftX;
            int iY = e.Y + m_pDrawFrame.Y - m_iShiftY;

            while (iX > m_iScaledMapWidth)
                iX -= m_iScaledMapWidth;

            while (iX < 0)
                iX += m_iScaledMapWidth;

            m_pFocusedContinent = null;
            m_pFocusedLand = null;
            m_pFocusedLocation = null;

            bool bContinent = false;
            foreach (ContinentX pContinent in m_pWorld.m_aContinents)
            {
                GraphicsPath pContinentPath = m_cContinentBorders[pContinent];

                if (pContinentPath.IsVisible(iX, iY))
                {
                    m_pFocusedContinent = pContinent;

                    foreach (State pState in pContinent.m_cStates)
                    {
                        GraphicsPath pStatePath = m_cStateBorders[pState];

                        if (pStatePath.IsVisible(iX, iY))
                        {
                            m_pFocusedState = pState;

                            foreach (Province pProvince in pState.m_cContents)
                            {
                                GraphicsPath pProvincePath = m_cProvinceBorders[pProvince];

                                if (pProvincePath.IsVisible(iX, iY))
                                {
                                    m_pFocusedProvince = pProvince;
                                    break;
                                }
                            }

                            break;
                        }
                    }

                    bContinent = true;
                    break;
                }
            }

            foreach (LandMass<LandX> pLandMass in m_pWorld.m_aLandMasses)
            {
                foreach (LandX pLand in pLandMass.m_cContents)
                {
                    GraphicsPath pLandPath = m_cLandBorders[pLand];

                    if (pLandPath.IsVisible(iX, iY))
                    {
                        m_pFocusedLand = pLand;

                        foreach (LocationX pLoc in pLand.m_cContents)
                        {
                            GraphicsPath pLocationPath = m_cLocationBorders[pLoc];

                            if (pLocationPath.IsVisible(iX, iY))
                            {
                                m_pFocusedLocation = pLoc;
                                break;
                            }
                        }

                        break;
                    }
                }
            }

            string sToolTip = "";

            if (bContinent && m_pFocusedContinent != null)
                sToolTip += m_pFocusedContinent.ToString();

            if (m_pFocusedLand != null)
            {
                if (sToolTip.Length > 0)
                    sToolTip += "\n - ";

                sToolTip += m_pFocusedLand.ToString();
            }

            if (bContinent && m_pFocusedState != null)
            {
                if (sToolTip.Length > 0)
                    sToolTip += "\n   - ";

                sToolTip += string.Format("{1} {0} ({2})", m_pFocusedState.m_pInfo.m_sName, m_pFocusedState.m_sName, m_pFocusedState.m_pRace.m_sName);
            }

            if (bContinent && m_pFocusedProvince != null)
            {
                if (sToolTip.Length > 0)
                    sToolTip += "\n     - ";

                sToolTip += string.Format("province {0} ({2}, {1})", m_pFocusedProvince.m_sName, m_pFocusedProvince.m_pAdministrativeCenter == null ? "-" : m_pFocusedProvince.m_pAdministrativeCenter.ToString(), m_pFocusedProvince.m_pRace.m_sName);
            }

            if (m_pFocusedLocation != null)
            {
                if (sToolTip.Length > 0)
                    sToolTip += "\n       - ";

                sToolTip += m_pFocusedLocation.ToString();

                if (m_pFocusedLocation.m_pSettlement != null && m_pFocusedLocation.m_pSettlement.m_cBuildings.Count > 0)
                {
                    foreach (Building pBuilding in m_pFocusedLocation.m_pSettlement.m_cBuildings)
                    {
                        sToolTip += "\n         - " + pBuilding.ToString();
                    }
                }

                if (m_pFocusedLocation.m_cHaveRoadTo.Count > 0)
                {
                    sToolTip += "\nHave roads to:";
                    foreach (var pRoad in m_pFocusedLocation.m_cHaveRoadTo)
                        sToolTip += "\n - " + pRoad.Key.m_pSettlement.m_sName;
                }

                if (m_pFocusedLocation.m_cHaveSeaRouteTo.Count > 0)
                {
                    sToolTip += "\nHave sea routes to:";
                    foreach (LocationX pRoute in m_pFocusedLocation.m_cHaveSeaRouteTo)
                        sToolTip += "\n - " + pRoute.m_pSettlement.m_sName;
                }
            }

            if(toolTip1.GetToolTip(this) != sToolTip)
                toolTip1.SetToolTip(this, sToolTip);

            timer1.Enabled = true;
        }

        private void WorldMap_DoubleClick(object sender, EventArgs e)
        {
            if (m_pFocusedState != null)
                SelectedState = m_pFocusedState;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
