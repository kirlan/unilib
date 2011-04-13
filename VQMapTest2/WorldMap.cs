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

namespace VQMapTest2
{
    internal interface ILandMark
    {
        void Draw(Graphics gr, float fScale);

        void Scale(float fScale);
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

        public void Draw(Graphics gr, float fScale)
        {
            if (fScale <= 1)
                return;

            float rr1 = r1;
            float rr2 = r2;
            if (rr1 < 2)
                rr1 = 2;
            if (rr2 < 1)
                rr2 = 1;

            gr.FillEllipse(Brushes.White, x - rr1, y - rr1, rr1 * 2, rr1 * 2);
            gr.FillEllipse(Brushes.Black, x - rr2, y - rr2, rr2 * 2, rr2 * 2);
            gr.DrawEllipse(WorldMap.s_pBlack2Pen, x - rr1, y - rr1, rr1 * 2, rr1 * 2);

            if (fScale > 4)
                gr.DrawString(name, s_pFont, Brushes.Black, x + rr1, y);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r1 *= fScale;
            r2 *= fScale;
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

        public void Draw(Graphics gr, float fScale)
        {
            if (fScale <= 1)
                return;

            float rr = r;
            if (rr < 2)
                rr = 2;
                
            gr.FillEllipse(Brushes.Black, x - rr, y - rr, rr * 2, rr * 2);
            gr.DrawEllipse(Pens.White, x - rr, y - rr, rr * 2, rr * 2);

            if (fScale > 4)
                gr.DrawString(name, s_pFont, Brushes.Black, x + rr, y);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r *= fScale;
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

        public void Draw(Graphics gr, float fScale)
        {
            if (fScale <= 2)
                return;

            float rr = r;
            if (rr < 1)
                rr = 1;

            gr.FillEllipse(Brushes.Black, x - rr, y - rr, rr * 2, rr * 2);
            gr.DrawEllipse(Pens.Black, x - rr, y - rr, rr * 2, rr * 2);

            if (fScale > 4)
                gr.DrawString(name, s_pFont, Brushes.Black, x + rr, y);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r *= fScale;
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

        public void Draw(Graphics gr, float fScale)
        {
            if (fScale <= 4)
                return;

            float rr = r;
            if (rr < 1)
                rr = 1;

            gr.FillEllipse(brush, x - rr, y - rr, rr * 2, rr * 2);
            gr.DrawEllipse(Pens.Black, x - rr, y - rr, rr * 2, rr * 2);

            if (fScale > 4)
                gr.DrawString(name, s_pFont, Brushes.Black, x + rr, y);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r *= fScale;
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

        public void Draw(Graphics gr, float fScale)
        {
            if (fScale <= 2)
                return;

            float rr1 = r1;
            if (rr1 < 1)
                rr1 = 1;
            float rr2 = r2;
            if (rr2 <= rr1)
                rr2 = rr1 + 1;

            gr.FillPolygon(Brushes.Gray, points);
            gr.DrawPolygon(WorldMap.s_pBlack2Pen, points);

            if (fScale > 4)
                gr.DrawString(name, s_pFont, Brushes.Black, x + rr1, y);
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

        public void Draw(Graphics gr, float fScale)
        {
            if (fScale <= 2)
                return;

            float rr1 = r1;
            if (rr1 < 1)
                rr1 = 1;
            float rr2 = rr1 + 1;

            gr.FillEllipse(Brushes.Red, x - rr1, y - rr1, rr1 * 2, rr1 * 2);
            gr.DrawEllipse(Pens.Black, x - rr1, y - rr1, rr1 * 2, rr1 * 2);
            gr.DrawEllipse(s_pBlack1Pen, x - rr2, y - rr2, rr2 * 2, rr2 * 2);

            //if (name != null)
            //    gr.DrawString(name, s_pFont, Brushes.Black, x + r, y);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r1 *= fScale;
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

        public void Draw(Graphics gr, float fScale)
        {
            if (fScale <= 2)
                return;

            float rr1 = r1;
            if (rr1 < 1)
                rr1 = 1;
            float rr2 = rr1 + 1;

            gr.FillPie(Brushes.DarkRed, x - rr2, y - rr2, rr2 * 2, rr2 * 2, 180, 180);
            gr.DrawPie(Pens.Black, x - rr2, y - rr2, rr2 * 2, rr2 * 2, 180, 180);

            //if (name != null)
            //    gr.DrawString(name, s_pFont, Brushes.Black, x + r, y);
        }

        public void Scale(float fScale)
        {
            x *= fScale;
            y *= fScale;

            r1 *= fScale;
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
            if (r < 1)
                r = 1;
            
            List<PointF> cPoints = new List<PointF>();
            cPoints.Add(new PointF(x - r, y));
            cPoints.Add(new PointF(x, y - 2*r));
            cPoints.Add(new PointF(x + r, y));
            points = cPoints.ToArray();

            name = sName;

            //s_pBlack1Pen.DashStyle = DashStyle.Dot;
        }

        public void Draw(Graphics gr, float fScale)
        {
            gr.FillEllipse(Brushes.Silver, x - r, y - r / 2, r * 2, r);
            gr.FillPolygon(Brushes.Silver, points);
            gr.DrawLines(Pens.Black, points);

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
            if (r1 < 1)
                r1 = 1;
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

        public void Draw(Graphics gr, float fScale)
        {
            if (fScale <= 2)
                return;

            gr.FillPolygon(Brushes.Silver, points);
            gr.DrawPolygon(WorldMap.s_pBlack2Pen, points);

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
            if (r < 1)
                r = 1;
            
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

        public void Draw(Graphics gr, float fScale)
        {
            gr.FillEllipse(Brushes.Silver, x - r, y - r / 2, r * 2, r);
            
            gr.FillPolygon(Brushes.Silver, points1);
            gr.DrawLines(Pens.Black, points1);

            gr.FillPolygon(Brushes.Red, points2);
            gr.DrawLines(WorldMap.s_pRed2Pen, points2);

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
    }

    public partial class WorldMap : UserControl
    {
        Bitmap ActualMap = null;

        float m_fkX = 1;
        float m_fkY = 1;

        Brush[] m_aHumidity;

        public WorldMap()
        {
            InitializeComponent();

            m_pDrawFrame = new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            //BuildActualMap(1);

            List<Brush> cHumidity = new List<Brush>();
            for (int i = 0; i <= 100; i++)
                cHumidity.Add(GetHumidityColor(i));
            m_aHumidity = cHumidity.ToArray();
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
                m_pDrawFrame.Width = ClientRectangle.Width;
                m_pDrawFrame.Height = ClientRectangle.Height;
            }

            //if (m_pDrawFrame.X + m_pDrawFrame.Width > ActualMap.Width)
            //    m_pDrawFrame.X = ActualMap.Width - m_pDrawFrame.Width;

            if (m_pDrawFrame.Y + m_pDrawFrame.Height > ActualMap.Height)
                m_pDrawFrame.Y = ActualMap.Height - m_pDrawFrame.Height;

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
                    e.Graphics.DrawRectangle(Pens.Red, new Rectangle(m_pDrawFrame.X + m_iShiftX + ActualMap.Width, m_pDrawFrame.Y + m_iShiftY, m_pDrawFrame.Width, m_pDrawFrame.Height));

                if (m_pDrawFrame.X + m_pDrawFrame.Width > ActualMap.Width)
                    e.Graphics.DrawRectangle(Pens.Red, new Rectangle(m_pDrawFrame.X + m_iShiftX - ActualMap.Width, m_pDrawFrame.Y + m_iShiftY, m_pDrawFrame.Width, m_pDrawFrame.Height));

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
                int iActualWidth = Math.Min(m_pDrawFrame.Width, ActualMap.Width - m_pDrawFrame.X);
                e.Graphics.DrawImage(ActualMap, m_iShiftX, m_iShiftY, new Rectangle(m_pDrawFrame.X, m_pDrawFrame.Y, iActualWidth, m_pDrawFrame.Height), GraphicsUnit.Pixel);

                if (iActualWidth < m_pDrawFrame.Width)
                    e.Graphics.DrawImage(ActualMap, iActualWidth + m_iShiftX, m_iShiftY, new Rectangle(0, m_pDrawFrame.Y, m_pDrawFrame.Width - iActualWidth, m_pDrawFrame.Height), GraphicsUnit.Pixel);
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

        private int m_iShiftX = 0;
        private int m_iShiftY = 0;

        private void BuildActualMap(float fScale)
        {
            if (ClientRectangle.Width == 0)
                return;

            float fK = 1;
            if (m_pWorld != null)
                fK = (float)m_pWorld.m_cGrid.RY / m_pWorld.m_cGrid.RX;

            int iWorkingWidth = (int)(ClientRectangle.Width * fScale);
            int iWorkingHeight = (int)(iWorkingWidth * fK);

            if (iWorkingHeight > ClientRectangle.Height * fScale)
            {
                iWorkingHeight = (int)(ClientRectangle.Height * fScale);
                iWorkingWidth = (int)(iWorkingHeight / fK);
            }

            ActualMap = new Bitmap(iWorkingWidth, iWorkingHeight);

            if (m_pWorld != null)
            {
                m_fkX = (float)(ActualMap.Width) / (m_pWorld.m_cGrid.RX * 2);
                m_fkY = (float)(ActualMap.Height) / (m_pWorld.m_cGrid.RY * 2);
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
                m_pDrawFrame.Width = ClientRectangle.Width;
                m_pDrawFrame.Height = ClientRectangle.Height;
            }
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
                m_pDrawFrame.X = Math.Max(0, Math.Min(iX, ActualMap.Width - m_pDrawFrame.Width));

            while (m_pDrawFrame.X < 0)
                m_pDrawFrame.X += ActualMap.Width;

            while (m_pDrawFrame.X > ActualMap.Width)
                m_pDrawFrame.X -= ActualMap.Width;

            m_pDrawFrame.Y = Math.Max(0, Math.Min(iY, ActualMap.Height - m_pDrawFrame.Height));
            
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

        private PointF[] BuildBorder(Line pFirstLine, float fShift, out bool bCross)
        {
            bCross = false;

            List<PointF> cBorder = new List<PointF>();
            Line pLine = pFirstLine;
            cBorder.Add(GetScaledPoint(pLine.m_pPoint1, fShift));
            float fLastPointX = pLine.m_pPoint1.X + fShift;
            do
            {
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
        private PointF[][] BuildPath(List<Line> cFirstLines, bool bMirror)
        {
            List<PointF[]> cPath = new List<PointF[]>();
            foreach (Line pFirstLine in cFirstLines)
            {
                bool bCross;
                cPath.Add(BuildBorder(pFirstLine, 0, out bCross));

                //if (m_bUseCurves)
                //    pPath.AddClosedCurve(cBorder.ToArray());
                //else
                //    pPath.AddPolygon(cBorder.ToArray());

                if (m_pWorld.m_cGrid.m_bCycled && bMirror && bCross)
                {
                    if (pFirstLine.m_pPoint1.X > 0)
                        cPath.Add(BuildBorder(pFirstLine, -m_pWorld.m_cGrid.RX * 2, out bCross));
                    else
                        cPath.Add(BuildBorder(pFirstLine, m_pWorld.m_cGrid.RX * 2, out bCross));

                    //if (m_bUseCurves)
                    //    pPath.AddClosedCurve(cBorder.ToArray());
                    //else
                    //    pPath.AddPolygon(cBorder.ToArray());
                }
            }

            return cPath.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cFirstLines"></param>
        /// <param name="bMirror">Строить ли отражения для зацикленного мира. Нужно при отрисовке, но нельзя при определении описывающего прямоугольника.</param>
        /// <returns></returns>
        private PointF[][] BuildPath(Line pFirstLine, bool bMirror)
        {
            List<PointF[]> cPath = new List<PointF[]>();

            bool bCross;
            cPath.Add(BuildBorder(pFirstLine, 0, out bCross));

            //if (m_bUseCurves)
            //    cPath.AddClosedCurve(cBorder.ToArray());
            //else
            //    cPath.AddPolygon(cBorder.ToArray());

            if (m_pWorld.m_cGrid.m_bCycled && bMirror && bCross)
            {
                if (pFirstLine.m_pPoint1.X > 0)
                    cPath.Add(BuildBorder(pFirstLine, -m_pWorld.m_cGrid.RX * 2, out bCross));
                else
                    cPath.Add(BuildBorder(pFirstLine, m_pWorld.m_cGrid.RX * 2, out bCross));

                //if (m_bUseCurves)
                //    cPath.AddClosedCurve(cBorder.ToArray());
                //else
                //    cPath.AddPolygon(cBorder.ToArray());
            }

            return cPath.ToArray();
        }

        private void DrawContinentBorder(Graphics gr, ContinentX pContinent, int iDX, int iDY)
        {
            GraphicsPath pPath;
            if (!m_cContinentBorders.TryGetValue(pContinent, out pPath))
                return;
            
            Matrix pMatrix = new Matrix();
            pMatrix.Translate(iDX, iDY);
            pPath.Transform(pMatrix);

            gr.DrawPath(new Pen(Color.Black, 2), pPath);
        }

        private void DrawContinentShape(Graphics gr, ContinentX pContinent)
        {
            GraphicsPath pPath;
            if (!m_cContinentBorders.TryGetValue(pContinent, out pPath))
                return;

            Color mark = LandTypes<LandTypeInfoX>.Plains.m_pColor;

            gr.FillPath(new SolidBrush(mark), pPath);

            //gr.DrawClosedCurve(new Pen(mark, 5), cContinentBorder.ToArray(), 0.01f, FillMode.Winding);
        }

        private void DrawArea(Graphics gr, AreaX pArea)
        {
            if (pArea.m_pType == LandTypes<LandTypeInfoX>.Plains)
                return;

            GraphicsPath pPath;
            if (!m_cAreaBorders.TryGetValue(pArea, out pPath))
                return;

            gr.FillPath(new SolidBrush(pArea.m_pType.m_pColor), pPath);
        }

        private void DrawRace(Graphics gr, AreaX pArea)
        {
            if (pArea.m_pRace == null)
                return;

            GraphicsPath pPath;
            if (!m_cAreaBorders.TryGetValue(pArea, out pPath))
                return;

            gr.FillPath(m_cRaceColorsID[pArea.m_pRace], pPath);
        }

        private void DrawStateBorder(Graphics gr, State pState, bool bFocused)
        {
            if (pState == null)
                return;

            GraphicsPath pPath;
            if (!m_cStateBorders.TryGetValue(pState, out pPath))
                return;

            if (bFocused)
                gr.DrawPath(new Pen(Color.Red, 3), pPath);

            Pen pPen = new Pen(Color.Black, 3);
            //pPen.DashStyle = DashStyle.Dash;
            if (bFocused)
                pPen.DashPattern = new float[] { 2.0F, 4.0F };
            gr.DrawPath(pPen, pPath);
        }

        private void DrawProvinceBorder(Graphics gr, Province pProvince)
        {
            if (pProvince == null)
                return;

            GraphicsPath pPath;
            if (!m_cProvinceBorders.TryGetValue(pProvince, out pPath))
                return;

            Pen pPen = new Pen(Color.White, 2);
            gr.DrawPath(pPen, pPath);
        }

        private void DrawDominatedRace(Graphics gr, Province pProvince)
        {
            GraphicsPath pPath;
            if (!m_cProvinceBorders.TryGetValue(pProvince, out pPath))
                return;

            gr.FillPath(m_cRaceColorsID[pProvince.m_pRace], pPath);
        }

        private List<PointF> BuildPathLine(TransportationLink pPath, float fShift, out bool bCross)
        {
            bCross = false;
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

                fLastPointX = pPoint.X + fDX;
            }

            return cRoadLine;
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
        internal static Pen s_pAqua1DotPen = new Pen(Color.Aqua, 1);
        internal static Pen s_pBlack1DotPen = new Pen(Color.Black, 1);
        internal static Pen s_pWhite2Pen = new Pen(Color.White, 2);


        private void DrawRoad(Graphics gr, TransportationLink pRoad, bool bBack)
        {
            if (pRoad.RoadLevel == 0)
                return;
                
            if (m_fScale <= 2 && pRoad.RoadLevel <= 1)
                return;

            //if (m_fScale <= 2 && pRoad.RoadLevel == 2)
            //    return;

            Pen pPen = Pens.Black;

            if (bBack)
            {
                if (pRoad.m_bSea || pRoad.m_bEmbark)
                    return;

                if (pRoad.RoadLevel != 3)
                    return;

                pPen = s_pDarkGrey3Pen;
            }
            else
            {
                switch (pRoad.RoadLevel)
                {
                    case 1:
                        if (pRoad.m_bSea || pRoad.m_bEmbark)
                            pPen = s_pAqua1DotPen;
                        else
                            pPen = s_pBlack1DotPen;
                        pPen.DashPattern = new float[] { 1, 2 };
                        break;
                    case 2:
                        if (pRoad.m_bSea || pRoad.m_bEmbark)
                            pPen = s_pAqua1Pen;
                        else
                            pPen = s_pBlack1Pen;
                        break;
                    case 3:
                        if (pRoad.m_bSea || pRoad.m_bEmbark)
                            pPen = s_pAqua2Pen;
                        else
                            pPen = s_pBlack2Pen;
                        break;
                }
            }

            DrawTransportationLink(gr, pRoad, pPen);
        }

        private void DrawTransportationLink(Graphics gr, TransportationLink pRoad, Pen pPen)
        {
            bool bCross;
            List<PointF> cPathLine = BuildPathLine(pRoad, 0, out bCross);

            DrawPathLine(gr, cPathLine, pPen);
            
            if (m_pWorld.m_cGrid.m_bCycled && bCross)
            {
                if (pRoad.m_aPoints[0].X > 0)
                {
                    cPathLine = BuildPathLine(pRoad, -m_pWorld.m_cGrid.RX * 2, out bCross);
                    DrawPathLine(gr, cPathLine, pPen);
                }
                else
                {
                    cPathLine = BuildPathLine(pRoad, m_pWorld.m_cGrid.RX * 2, out bCross);
                    DrawPathLine(gr, cPathLine, pPen);
                }
            }
        }

        private void DrawLandHumidity(Graphics gr, LandX pLand)
        {
            GraphicsPath pPath;
            if (!m_cLandBorders.TryGetValue(pLand, out pPath))
                return;

            gr.FillPath(pLand.IsWater ? pLand.Type.m_pBrush : GetHumidityColor(pLand.Humidity), pPath);
        }

        private void AddLocationSign(LocationX pLoc)
        {
            float fPointX = GetScaledX(pLoc.m_pCenter.X);
            float fPointY = GetScaledY(pLoc.m_pCenter.Y);

            //gr.FillEllipse(new SolidBrush(Color.White), (int)(m_fkX / 2 + (m_pWorld.m_iWorldScale * 1.5 + pLoc.m_pCenter.X) * m_fkX),
            //                                  (int)(m_fkY / 2 + (m_pWorld.m_iWorldScale + pLoc.m_pCenter.Y) * m_fkY), m_fkX, m_fkY);
            //gr.DrawEllipse(new Pen(Color.Black), (int)(m_fkX / 2 + (m_pWorld.m_iWorldScale * 1.5 + pLoc.m_pCenter.X) * m_fkX),
            //                                  (int)(m_fkY / 2 + (m_pWorld.m_iWorldScale + pLoc.m_pCenter.Y) * m_fkY), m_fkX, m_fkY);


            switch (pLoc.m_eType)
            {
                case RegionType.Peak:
                    m_cLandmarks.Add(new SignPeak(m_fkX, fPointX, fPointY, ""));
                    break;
                case RegionType.Volcano:
                    m_cLandmarks.Add(new SignVolkano(m_fkX, fPointX, fPointY, ""));
                    break;
            }

            if (pLoc.m_pSettlement != null)
            {
                if (pLoc.m_pSettlement.m_iRuinsAge > 0)
                {
                    m_cLandmarks.Add(new SignRuin(m_fkX, fPointX, fPointY, ""));
                }
                else
                {
                    switch (pLoc.m_pSettlement.m_pInfo.m_eSize)
                    {
                        case SettlementSize.Capital:
                            m_cLandmarks.Add(new SignCapital(m_fkX, fPointX, fPointY, pLoc.m_pSettlement.m_sName));
                            break;
                        case SettlementSize.City:
                            m_cLandmarks.Add(new SignCity(m_fkX, fPointX, fPointY, pLoc.m_pSettlement.m_sName));
                            break;
                        case SettlementSize.Town:
                            m_cLandmarks.Add(new SignTown(m_fkX, fPointX, fPointY, pLoc.m_pSettlement.m_sName));
                            break;
                        case SettlementSize.Village:
                            m_cLandmarks.Add(new SignVillage(m_fkX, fPointX, fPointY, pLoc.m_pSettlement.m_sName, (pLoc.Owner as LandX).Type.m_pBrush));
                            break;
                        case SettlementSize.Hamlet:
                            m_cLandmarks.Add(new SignVillage(m_fkX, fPointX, fPointY, pLoc.m_pSettlement.m_sName, (pLoc.Owner as LandX).Type.m_pBrush));
                            break;
                        case SettlementSize.Fort:
                            m_cLandmarks.Add(new SignFort(m_fkX, fPointX, fPointY, pLoc.m_pSettlement.m_sName));
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
                            m_cLandmarks.Add(new SignLair(m_fkX, fPointX, fPointY, ""));
                            break;
                        case BuildingType.Hideout:
                            m_cLandmarks.Add(new SignHideout(m_fkX, fPointX, fPointY, ""));
                            break;
                    }
                }
            }
        }

        private void DrawLocationBorder(Graphics gr, LocationX pLoc)
        {
            GraphicsPath pPath;
            if (!m_cLocationBorders.TryGetValue(pLoc, out pPath))
                return;

            gr.DrawPath(Pens.DarkGray, pPath);
        }

        private void DrawLandBorder(Graphics gr, LandX pLand)
        {
            GraphicsPath pPath;
            if (!m_cLandBorders.TryGetValue(pLand, out pPath))
                return;

            gr.DrawPath(Pens.Black, pPath);
        }

        private void DrawLandMass(Graphics gr, LandMass<LandX> pLandMass)
        {
            GraphicsPath pPath;
            if (!m_cLandMassBorders.TryGetValue(pLandMass, out pPath))
                return;

            Pen pPen = new Pen(Color.Red, 2);
            gr.DrawPath(pPen, pPath);
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
        private List<ILandMark> m_cLandmarks = new List<ILandMark>();

        private GraphicsPath m_pContinentsPath = new GraphicsPath();
        private GraphicsPath m_pContinentsPath2 = new GraphicsPath();
        private GraphicsPath m_pLocationsPath = new GraphicsPath();
        private GraphicsPath m_pLandsPath = new GraphicsPath();
        private GraphicsPath m_pProvinciesPath = new GraphicsPath();

        private Dictionary<Brush, GraphicsPath> m_cAreaMap = new Dictionary<Brush, GraphicsPath>();
        private Dictionary<Brush, GraphicsPath> m_cNativesMap = new Dictionary<Brush, GraphicsPath>();
        private Dictionary<Brush, GraphicsPath> m_cNationsMap = new Dictionary<Brush, GraphicsPath>();
        private Dictionary<Brush, GraphicsPath> m_cHumidityMap = new Dictionary<Brush, GraphicsPath>();

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

            foreach (var pPair in m_cLandmarks)
            {
                pPair.Scale(fScale);
            }

            m_pContinentsPath.Transform(pMatrix);
            m_pLocationsPath.Transform(pMatrix);
            m_pLandsPath.Transform(pMatrix);
            m_pProvinciesPath.Transform(pMatrix);

            m_pContinentsPath2 = (GraphicsPath)m_pContinentsPath.Clone();
            Matrix pMatrixT = new Matrix();
            pMatrixT.Translate(1, 1);
            m_pContinentsPath2.Transform(pMatrixT);

            foreach (var pPair in m_cAreaMap)
                pPair.Value.Transform(pMatrix);

            foreach (var pPair in m_cHumidityMap)
                pPair.Value.Transform(pMatrix);

            foreach (var pPair in m_cNativesMap)
                pPair.Value.Transform(pMatrix);

            foreach (var pPair in m_cNationsMap)
                pPair.Value.Transform(pMatrix);

            DateTime pTime2 = DateTime.Now;
        }
        
        private void PrebuildPaths()
        {
            if (!m_bRebuild)
                return;

            DateTime pTime1 = DateTime.Now;

            m_cContinentBorders.Clear();
            m_cAreaBorders.Clear();

            m_pContinentsPath.Reset();
            
            m_cAreaMap.Clear();

            if (m_pMasterMap == null)
            {
                m_cProvinceBorders.Clear();
                m_cLandMassBorders.Clear();
                m_cLandBorders.Clear();
                m_cLocationBorders.Clear();
                m_cStateBorders.Clear();
                m_cLandmarks.Clear();

                m_pLocationsPath.Reset();
                m_pLandsPath.Reset();
                m_pProvinciesPath.Reset();

                m_cHumidityMap.Clear();
                m_cNativesMap.Clear();
                m_cNationsMap.Clear();
            }

            PointF[][] aPoints;
            GraphicsPath pPath;

            if (m_pMasterMap == null)
            {
                foreach (LandMass<LandX> pLandMass in m_pWorld.m_aLandMasses)
                {
                    aPoints = BuildPath(pLandMass.m_cFirstLines, true);
                    pPath = new GraphicsPath();
                    foreach (var aPts in aPoints)
                        pPath.AddPolygon(aPts);
                    m_cLandMassBorders[pLandMass] = pPath;
                    foreach (LandX pLand in pLandMass.m_cContents)
                    {
                        aPoints = BuildPath(pLand.m_cFirstLines, true);
                        pPath = new GraphicsPath();
                        foreach (var aPts in aPoints)
                        {
                            pPath.AddPolygon(aPts);
                            m_pLandsPath.AddPolygon(aPts);

                            Brush pBrush = pLand.IsWater ? pLand.Type.m_pBrush : m_aHumidity[pLand.Humidity];
                            if (!m_cHumidityMap.ContainsKey(pBrush))
                                m_cHumidityMap[pBrush] = new GraphicsPath();
                            m_cHumidityMap[pBrush].AddPolygon(aPts);
                        }
                        m_cLandBorders[pLand] = pPath;

                        foreach (LocationX pLoc in pLand.m_cContents)
                        {
                            aPoints = BuildPath(pLoc.m_pFirstLine, true);
                            pPath = new GraphicsPath();
                            foreach (var aPts in aPoints)
                            {
                                pPath.AddPolygon(aPts);
                                m_pLocationsPath.AddPolygon(aPts);
                            }
                            m_cLocationBorders[pLoc] = pPath;
                            AddLocationSign(pLoc);
                        }

                    }
                }
            }
            
            foreach (ContinentX pContinent in m_pWorld.m_aContinents)
            {
                aPoints = BuildPath(pContinent.m_cFirstLines, true);
                pPath = new GraphicsPath();
                foreach (var aPts in aPoints)
                {
                    pPath.AddPolygon(aPts);
                    m_pContinentsPath.AddPolygon(aPts);
                }
                m_cContinentBorders[pContinent] = pPath;

                if (m_pMasterMap == null)
                {
                    foreach (State pState in pContinent.m_cStates)
                    {
                        aPoints = BuildPath(pState.m_cFirstLines, true);
                        pPath = new GraphicsPath();
                        foreach (var aPts in aPoints)
                        {
                            pPath.AddPolygon(aPts);
                        }
                        m_cStateBorders[pState] = pPath;
                    }
                }

                foreach (AreaX pArea in pContinent.m_cAreas)
                {
                    aPoints = BuildPath(pArea.m_cFirstLines, true);
                    pPath = new GraphicsPath();
                    foreach (var aPts in aPoints)
                    {
                        pPath.AddPolygon(aPts);

                        if (pArea.m_pType != LandTypes<LandTypeInfoX>.Plains)
                        {
                            if (!m_cAreaMap.ContainsKey(pArea.m_pType.m_pBrush))
                                m_cAreaMap[pArea.m_pType.m_pBrush] = new GraphicsPath();
                            m_cAreaMap[pArea.m_pType.m_pBrush].AddPolygon(aPts);
                        }

                        if (m_pMasterMap == null)
                        {
                            if (pArea.m_pRace != null)
                            {
                                Brush pBrush = m_cRaceColorsID[pArea.m_pRace];
                                if (!m_cNativesMap.ContainsKey(pBrush))
                                    m_cNativesMap[pBrush] = new GraphicsPath();
                                m_cNativesMap[pBrush].AddPolygon(aPts);
                            }
                        }
                    }
                    m_cAreaBorders[pArea] = pPath;
                }
            }
            m_pContinentsPath2 = (GraphicsPath)m_pContinentsPath.Clone();
            Matrix pMatrix = new Matrix();
            pMatrix.Translate(1, 1);
            m_pContinentsPath2.Transform(pMatrix);

            if (m_pMasterMap == null)
            {
                foreach (Province pProvince in m_pWorld.m_aProvinces)
                {
                    aPoints = BuildPath(pProvince.m_cFirstLines, true);
                    pPath = new GraphicsPath();
                    foreach (var aPts in aPoints)
                    {
                        pPath.AddPolygon(aPts);
                        if (!pProvince.m_pCenter.IsWater)
                            m_pProvinciesPath.AddPolygon(aPts);

                        Brush pBrush = m_cRaceColorsID[pProvince.m_pRace];
                        if (!m_cNationsMap.ContainsKey(pBrush))
                            m_cNationsMap[pBrush] = new GraphicsPath();
                        m_cNationsMap[pBrush].AddPolygon(aPts);
                    }
                    m_cProvinceBorders[pProvince] = pPath;
                }
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

            if (m_pWorld == null)
                return;

            if (m_pWorld.m_cGrid.m_aLocations.Length == 0)
                return;

            switch (m_eMode)
            {
                case VisType.LandType:
                    gr.DrawPath(new Pen(Color.Black, 2), m_pContinentsPath2);
                    gr.FillPath(LandTypes<LandTypeInfoX>.Plains.m_pBrush, m_pContinentsPath);

                    foreach (var pArea in m_cAreaMap)
                        gr.FillPath(pArea.Key, pArea.Value);

                    break;
                case VisType.Humidity:
                    foreach(var pHum in m_cHumidityMap)
                        gr.FillPath(pHum.Key, pHum.Value);

                    gr.DrawPath(Pens.Black, m_pContinentsPath);
                    break;
                case VisType.RacesNative:
                    gr.DrawPath(Pens.Black, m_pContinentsPath);
                    foreach(var pNative in m_cNativesMap)
                        gr.FillPath(pNative.Key, pNative.Value);
                    //foreach (ContinentX pContinent in m_pWorld.m_aContinents)
                    //{
                    //    foreach (AreaX pArea in pContinent.m_cAreas)
                    //        DrawRace(gr, pArea);
                    //}
                    break;
                case VisType.RacesStates:
                    gr.DrawPath(Pens.Black, m_pContinentsPath);
                    foreach (var pNation in m_cNationsMap)
                        gr.FillPath(pNation.Key, pNation.Value);

                    //foreach (Province pProvince in m_pWorld.m_aProvinces)
                    //    DrawDominatedRace(gr, pProvince);

                    break;
            }

            if (m_bShowLocationsBorders)
            {
                gr.DrawPath(Pens.DarkGray, m_pLocationsPath);

                //foreach(var pLoc in m_cLocationBorders)
                //    gr.DrawPath(Pens.DarkGray, pLoc.Value);

                //foreach(LocationX pLoc in m_pWorld.m_cGrid.m_aLocations)
                //    DrawLocationBorder(gr, pLoc);
            }

            if (m_bShowLands)
            {
                gr.DrawPath(Pens.Black, m_pLandsPath);

                //foreach(LandX pLand in m_pWorld.m_aLands)
                //    DrawLandBorder(gr, pLand);
            }

            if (m_bShowProvincies)
            {
                gr.DrawPath(s_pWhite2Pen, m_pProvinciesPath);
                
                //foreach (Province pProvince in m_pWorld.m_aProvinces)
                //    if(!pProvince.m_pCenter.IsWater)
                //        DrawProvinceBorder(gr, pProvince);
            }

            foreach (TransportationNode[] aPath in m_cPaths.Keys)
                DrawPath(gr, aPath, m_cPaths[aPath]);

            if (m_bShowRoads)
            {
                foreach (TransportationLink pRoad in m_pWorld.m_cTransportGrid)
                    DrawRoad(gr, pRoad, true);

                foreach (TransportationLink pRoad in m_pWorld.m_cTransportGrid)
                    DrawRoad(gr, pRoad, false);
            }

            foreach (ContinentX pContinent in m_pWorld.m_aContinents)
            {
                if (m_bShowStates)
                {
                    foreach (State pState in pContinent.m_cStates)
                        DrawStateBorder(gr, pState, false);
                }
            }

            if (m_bShowLocations)
            {
                foreach (ILandMark pLandMark in m_cLandmarks)
                    pLandMark.Draw(gr, m_fScale);
            }

            if (m_bShowLandMasses)
            {
                foreach (LandMass<LandX> pLandMass in m_pWorld.m_aLandMasses)
                    DrawLandMass(gr, pLandMass);
            }

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
                DrawStateBorder(gr, m_pSelectedState, true);

            Refresh();

            DateTime pTime2 = DateTime.Now;
        }

        private void DrawPath(Graphics gr, TransportationNode[] aPath, Pen pPen)
        {
            TransportationNode pLastNode = null;
            foreach (TransportationNode pNode in aPath)
            {
                if (pLastNode != null)
                {
                    DrawTransportationLink(gr, pLastNode.m_cLinks[pNode], pPen);
                }

                pLastNode = pNode;
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

            PointF[][] aPoints = BuildPath(m_pSelectedState.m_cFirstLines, false);
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
            PointF[][] aPoints = BuildPath(pState.m_cFirstLines, false);
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

            while (iX > ActualMap.Width)
                iX -= ActualMap.Width;

            while (iX < 0)
                iX += ActualMap.Width;

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
                    foreach (LocationX pRoad in m_pFocusedLocation.m_cHaveRoadTo)
                        sToolTip += "\n - " + pRoad.m_pSettlement.m_sName;
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
