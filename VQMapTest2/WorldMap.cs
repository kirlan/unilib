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
    public partial class WorldMap : UserControl
    {
        Bitmap ActualMap = null;

        float m_fkX = 1;
        float m_fkY = 1;

        public WorldMap()
        {
            InitializeComponent();

            m_pDrawFrame = new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            //BuildActualMap(1);
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

            m_fScale = fNewScale;

            BuildActualMap(m_fScale);

            PrebuildPaths();
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

        private Dictionary<Race, Color> m_cRaceColorsID = new Dictionary<Race, Color>();

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

        private int GetScaledX(float fX)
        {
            //if (fX < -m_pWorld.m_cGrid.RX)
            //    fX += m_pWorld.m_cGrid.RX * 2;
            //if (fX >= m_pWorld.m_cGrid.RX)
            //    fX -= m_pWorld.m_cGrid.RX * 2;
            return (int)(m_fkX + (m_pWorld.m_cGrid.RX + fX) * m_fkX);
        }

        private int GetScaledY(float fY)
        {
            return (int)(m_fkY + (m_pWorld.m_cGrid.RY + fY) * m_fkY);
        }

        private Point GetScaledPoint(IPointF pPoint, float fDX)
        {
            return new Point(GetScaledX(pPoint.X + fDX), GetScaledY(pPoint.Y));
        }

        private Point GetScaledPoint(PointF pPoint, float fDX)
        {
            return new Point(GetScaledX(pPoint.X + fDX), GetScaledY(pPoint.Y));
        }

        private List<Point> BuildBorder(Line pFirstLine, float fShift, out bool bCross)
        {
            bCross = false;

            List<Point> cBorder = new List<Point>();
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

            return cBorder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cFirstLines"></param>
        /// <param name="bMirror">Строить ли отражения для зацикленного мира. Нужно при отрисовке, но нельзя при определении описывающего прямоугольника.</param>
        /// <returns></returns>
        private GraphicsPath BuildPath(List<Line> cFirstLines, bool bMirror)
        {
            GraphicsPath pPath = new GraphicsPath();
            foreach (Line pFirstLine in cFirstLines)
            {
                bool bCross;
                List<Point> cBorder = BuildBorder(pFirstLine, 0, out bCross);

                if (m_bUseCurves)
                    pPath.AddClosedCurve(cBorder.ToArray());
                else
                    pPath.AddPolygon(cBorder.ToArray());

                if (m_pWorld.m_cGrid.m_bCycled && bMirror && bCross)
                {
                    if (pFirstLine.m_pPoint1.X > 0)
                        cBorder = BuildBorder(pFirstLine, -m_pWorld.m_cGrid.RX * 2, out bCross);
                    else
                        cBorder = BuildBorder(pFirstLine, m_pWorld.m_cGrid.RX * 2, out bCross);

                    if (m_bUseCurves)
                        pPath.AddClosedCurve(cBorder.ToArray());
                    else
                        pPath.AddPolygon(cBorder.ToArray());
                }
            }

            return pPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cFirstLines"></param>
        /// <param name="bMirror">Строить ли отражения для зацикленного мира. Нужно при отрисовке, но нельзя при определении описывающего прямоугольника.</param>
        /// <returns></returns>
        private GraphicsPath BuildPath(Line pFirstLine, bool bMirror)
        {
            GraphicsPath pPath = new GraphicsPath();

            bool bCross;
            List<Point> cBorder = BuildBorder(pFirstLine, 0, out bCross);

            if (m_bUseCurves)
                pPath.AddClosedCurve(cBorder.ToArray());
            else
                pPath.AddPolygon(cBorder.ToArray());

            if (m_pWorld.m_cGrid.m_bCycled && bMirror && bCross)
            {
                if (pFirstLine.m_pPoint1.X > 0)
                    cBorder = BuildBorder(pFirstLine, -m_pWorld.m_cGrid.RX * 2, out bCross);
                else
                    cBorder = BuildBorder(pFirstLine, m_pWorld.m_cGrid.RX * 2, out bCross);

                if (m_bUseCurves)
                    pPath.AddClosedCurve(cBorder.ToArray());
                else
                    pPath.AddPolygon(cBorder.ToArray());
            }

            return pPath;
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

            gr.FillPath(new SolidBrush(m_cRaceColorsID[pArea.m_pRace]), pPath);
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

            gr.FillPath(new SolidBrush(m_cRaceColorsID[pProvince.m_pRace]), pPath);
        }

        private List<Point> BuildPathLine(TransportationLink pPath, float fShift, out bool bCross)
        {
            bCross = false;
            List<Point> cRoadLine = new List<Point>();
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

        private void DrawPathLine(Graphics gr, List<Point> cPathLine, Pen pPen)
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

        private static Pen s_DarkGrey3Pen = new Pen(Color.DarkGray, 3);
        private static Pen s_pAqua2Pen = new Pen(Color.Aqua, 2);
        private static Pen s_pAqua1Pen = new Pen(Color.Aqua, 1);
        private static Pen s_pBlack1Pen = new Pen(Color.Black, 1);

        private void DrawRoad(Graphics gr, TransportationLink pRoad, bool bBack)
        {
            Pen pPen = Pens.Black;

            if (bBack)
            {
                if (pRoad.m_bSea || pRoad.m_bEmbark)
                    return;

                if (pRoad.RoadLevel != 3)
                    return;

                pPen = s_DarkGrey3Pen;
            }
            else
            {
                switch (pRoad.RoadLevel)
                {
                    case 1:
                        if (pRoad.m_bSea || pRoad.m_bEmbark)
                            pPen = s_pAqua1Pen;
                        else
                            pPen = s_pBlack1Pen;
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

            if (pRoad.RoadLevel > 0)
                DrawTransportationLink(gr, pRoad, pPen);
        }

        private void DrawTransportationLink(Graphics gr, TransportationLink pRoad, Pen pPen)
        {
            bool bCross;
            List<Point> cPathLine = BuildPathLine(pRoad, 0, out bCross);

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

            gr.FillPath(new SolidBrush(pLand.IsWater ? pLand.Type.m_pColor : GetHumidityColor(pLand.Humidity)), pPath);
        }

        private static Pen s_pBlack2Pen = new Pen(Color.Black, 2);

        private static Font s_CapitalFont = new Font("Arial", 12, FontStyle.Bold | FontStyle.Underline);
        private static Font s_CityFont = new Font("Arial", 12, FontStyle.Bold);
        private static Font s_TownFont = new Font("Arial", 10);
        private static Font s_VillageFont = new Font("Arial", 8, FontStyle.Italic);
        private static Font s_FortFont = new Font("Arial", 10);

        private void DrawSignCapital(Graphics gr, int x, int y, string sName)
        {
            int r1 = (int)(m_fkX * 200);
            int r2 = (int)(m_fkX * 120);
            if (r1 < 2)
                r1 = 2;
            if (r2 < 1)
                r2 = 1;
            gr.FillEllipse(Brushes.White, x - r1, y - r1, r1 * 2, r1 * 2);
            gr.FillEllipse(Brushes.Black, x - r2, y - r2, r2 * 2, r2 * 2);
            gr.DrawEllipse(s_pBlack2Pen, x - r1, y - r1, r1 * 2, r1 * 2);

            if (m_fScale > 4)
                gr.DrawString(sName, s_CapitalFont, Brushes.Black, x + r1, y);
        }

        private void DrawSignCity(Graphics gr, int x, int y, string sName)
        {
            int r = (int)(m_fkX * 120);
            if (r < 2)
                r = 2;
            gr.FillEllipse(Brushes.Black, x - r, y - r, r * 2, r * 2);
            gr.DrawEllipse(Pens.White, x - r, y - r, r * 2, r * 2);

            if (m_fScale > 4)
                gr.DrawString(sName, s_CityFont, Brushes.Black, x + r, y);
        }

        private void DrawSignTown(Graphics gr, int x, int y, string sName)
        {
            int r = (int)(m_fkX * 80);
            if (r < 1)
                r = 1;
            gr.FillEllipse(Brushes.Black, x - r, y - r, r * 2, r * 2);
            gr.DrawEllipse(Pens.Black, x - r, y - r, r * 2, r * 2);

            if (m_fScale > 4)
                gr.DrawString(sName, s_TownFont, Brushes.Black, x + r, y);
        }

        private void DrawSignVillage(Graphics gr, int x, int y, Brush pBrush, string sName)
        {
            int r = (int)(m_fkX * 80);
            if (r < 1)
                r = 1;
            gr.FillEllipse(pBrush, x - r, y - r, r * 2, r * 2);
            gr.DrawEllipse(Pens.Black, x - r, y - r, r * 2, r * 2);

            if (m_fScale > 4)
            {
                gr.DrawString(sName, s_VillageFont, Brushes.Black, x + r, y);
            }
        }

        private void DrawSignLair(Graphics gr, int x, int y)
        {
            int r1 = (int)(m_fkX * 80);
            if (r1 < 1)
                r1 = 1;
            int r2 = r1+1;

            gr.FillPie(Brushes.DarkRed, x - r2, y - r2, r2 * 2, r2 * 2, 180, 180);
            gr.DrawPie(Pens.Black, x - r2, y - r2, r2 * 2, r2 * 2, 180, 180);
        }

        private void DrawSignHideout(Graphics gr, int x, int y)
        {
            int r1 = (int)(m_fkX * 80);
            if (r1 < 1)
                r1 = 1;
            int r2 = r1 + 1;

            if (r1 > 0)
            {
                gr.FillEllipse(Brushes.Red, x - r1, y - r1, r1 * 2, r1 * 2);
                gr.DrawEllipse(Pens.Black, x - r1, y - r1, r1 * 2, r1 * 2);
                Pen pDot = s_pBlack1Pen;
                pDot.DashStyle = DashStyle.Dot;
                gr.DrawEllipse(pDot, x - r2, y - r2, r2 * 2, r2 * 2);
            }
        }

        private void DrawSignFort(Graphics gr, int x, int y, string sName)
        {
            int r1 = (int)(m_fkX * 133);
            if (r1 < 1)
                r1 = 1;
            int r2 = (int)(m_fkX * 200);
            if (r2 <= r1)
                r2 = r1+1;

            List<Point> cPoints = new List<Point>();
            cPoints.Add(new Point(x, y + r1));
            cPoints.Add(new Point(x + r2, y));
            cPoints.Add(new Point(x + r1, y - r2));
            cPoints.Add(new Point(x - r1, y - r2));
            cPoints.Add(new Point(x - r2, y));

            gr.FillPolygon(Brushes.Gray, cPoints.ToArray());
            gr.DrawPolygon(s_pBlack2Pen, cPoints.ToArray());

            //int r2 = r1 + 1;

            //if (r1 > 0)
            //{
            //    gr.DrawEllipse(new Pen(Color.Black, 2), x - r1, y - r1, r1 * 2, r1 * 2);
            //    Pen pDot = new Pen(Color.Black, 2);
            //    pDot.DashStyle = DashStyle.Dot;
            //    gr.DrawEllipse(pDot, x - r2, y - r2, r2 * 2, r2 * 2);
            //}

            if (m_fScale > 4)
                gr.DrawString(sName, s_FortFont, Brushes.Black, x + r1, y);
        }

        private void DrawSignRuin(Graphics gr, int x, int y)
        {
            int r1 = (int)(m_fkX * 120);
            if (r1 < 1)
                r1 = 1;
            int r2 = r1 + 1;

            if (r1 > 0)
            {
                List<Point> cPoints = new List<Point>();
                cPoints.Add(new Point(x, y - r1));
                cPoints.Add(new Point(x + r1, y));
                cPoints.Add(new Point(x, y + r1));
                cPoints.Add(new Point(x - r1, y));

                gr.FillPolygon(Brushes.Silver, cPoints.ToArray());
                gr.DrawPolygon(s_pBlack2Pen, cPoints.ToArray());
            }
        }

        private void DrawSignPeak(Graphics gr, int x, int y)
        {
            int r = (int)(m_fkX * 200);
            if (r < 1)
                r = 1;

            gr.FillEllipse(new SolidBrush(Color.Silver), x - r, y - r / 2, r * 2, r);

            List<Point> cPoints = new List<Point>();
            cPoints.Add(new Point(x - r, y));
            cPoints.Add(new Point(x, y - 2*r));
            cPoints.Add(new Point(x + r, y));

            gr.FillPolygon(Brushes.Silver, cPoints.ToArray());
            gr.DrawLines(Pens.Black, cPoints.ToArray());
        }

        private static Pen s_Red2Pen = new Pen(Color.Red, 2);

        private void DrawSignVolkano(Graphics gr, int x, int y)
        {
            int r = (int)(m_fkX * 200);
            if (r < 1)
                r = 1;

            gr.FillEllipse(new SolidBrush(Color.Silver), x - r, y - r / 2, r * 2, r);

            List<Point> cPoints = new List<Point>();
            cPoints.Add(new Point(x - r, y));
            cPoints.Add(new Point(x, y - 2*r));
            cPoints.Add(new Point(x + r, y));

            gr.FillPolygon(Brushes.Silver, cPoints.ToArray());
            gr.DrawLines(Pens.Black, cPoints.ToArray());

            cPoints.Clear();
            cPoints.Add(new Point(x - r / 2, y - r));
            cPoints.Add(new Point(x, y - 2*r));
            cPoints.Add(new Point(x + r / 2, y - r));
            cPoints.Add(new Point(x, y - r / 2));

            gr.FillPolygon(Brushes.Red, cPoints.ToArray());
            gr.DrawLines(s_Red2Pen, cPoints.ToArray());
        }

        private void DrawLocation(Graphics gr, LocationX pLoc)
        {
            int iPointX = GetScaledX(pLoc.m_pCenter.X);
            int iPointY = GetScaledY(pLoc.m_pCenter.Y);

            //gr.FillEllipse(new SolidBrush(Color.White), (int)(m_fkX / 2 + (m_pWorld.m_iWorldScale * 1.5 + pLoc.m_pCenter.X) * m_fkX),
            //                                  (int)(m_fkY / 2 + (m_pWorld.m_iWorldScale + pLoc.m_pCenter.Y) * m_fkY), m_fkX, m_fkY);
            //gr.DrawEllipse(new Pen(Color.Black), (int)(m_fkX / 2 + (m_pWorld.m_iWorldScale * 1.5 + pLoc.m_pCenter.X) * m_fkX),
            //                                  (int)(m_fkY / 2 + (m_pWorld.m_iWorldScale + pLoc.m_pCenter.Y) * m_fkY), m_fkX, m_fkY);


            switch (pLoc.m_eType)
            {
                case RegionType.Peak:
                    DrawSignPeak(gr, iPointX, iPointY);
                    break;
                case RegionType.Volcano:
                    DrawSignVolkano(gr, iPointX, iPointY);
                    break;
            }

            if (pLoc.m_pSettlement != null)
            {
                if (pLoc.m_pSettlement.m_iRuinsAge > 0)
                {
                    DrawSignRuin(gr, iPointX, iPointY);
                }
                else
                {
                    switch (pLoc.m_pSettlement.m_pInfo.m_eSize)
                    {
                        case SettlementSize.Capital:
                            DrawSignCapital(gr, iPointX, iPointY, pLoc.m_pSettlement.m_sName);
                            break;
                        case SettlementSize.City:
                            DrawSignCity(gr, iPointX, iPointY, pLoc.m_pSettlement.m_sName);
                            break;
                        case SettlementSize.Town:
                            DrawSignTown(gr, iPointX, iPointY, pLoc.m_pSettlement.m_sName);
                            break;
                        case SettlementSize.Village:
                            DrawSignVillage(gr, iPointX, iPointY, (pLoc.Owner as LandX).Type.m_pBrush, pLoc.m_pSettlement.m_sName);
                            break;
                        case SettlementSize.Hamlet:
                            DrawSignVillage(gr, iPointX, iPointY, (pLoc.Owner as LandX).Type.m_pBrush, pLoc.m_pSettlement.m_sName);
                            break;
                        case SettlementSize.Fort:
                            DrawSignFort(gr, iPointX, iPointY, pLoc.m_pSettlement.m_sName);
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
                            DrawSignLair(gr, iPointX, iPointY);
                            break;
                        case BuildingType.Hideout:
                            DrawSignHideout(gr, iPointX, iPointY);
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

        public void DrawMap(WorldMap pMasterMap)
        {
            m_pMasterMap = pMasterMap;
            m_pWorld = m_pMasterMap.m_pWorld;

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
                    m_cRaceColorsID[pRace] = m_aRaceColorsTemplate[iIndex];
                }
            }

            m_pDrawFrame.X = 0;
            m_pDrawFrame.Y = 0;

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

        private void PrebuildPaths()
        {
            m_cContinentBorders.Clear();
            m_cAreaBorders.Clear();
            m_cProvinceBorders.Clear();
            m_cLandMassBorders.Clear();
            m_cLandBorders.Clear();
            m_cLocationBorders.Clear();
            m_cStateBorders.Clear();

            foreach (LandMass<LandX> pLandMass in m_pWorld.m_aLandMasses)
            {
                m_cLandMassBorders[pLandMass] = BuildPath(pLandMass.m_cFirstLines, true);
                foreach (LandX pLand in pLandMass.m_cContents)
                {
                    m_cLandBorders[pLand] = BuildPath(pLand.m_cFirstLines, true);
                    foreach (LocationX pLoc in pLand.m_cContents)
                    {
                        m_cLocationBorders[pLoc] = BuildPath(pLoc.m_pFirstLine, true);
                    }
                }
            }
            
            foreach (ContinentX pContinent in m_pWorld.m_aContinents)
            {
                m_cContinentBorders[pContinent] = BuildPath(pContinent.m_cFirstLines, true);

                foreach (State pState in pContinent.m_cStates)
                    m_cStateBorders[pState] = BuildPath(pState.m_cFirstLines, true);
                
                foreach (AreaX pArea in pContinent.m_cAreas)
                    m_cAreaBorders[pArea] = BuildPath(pArea.m_cFirstLines, true);
            }

            foreach (Province pProvince in m_pWorld.m_aProvinces)
                m_cProvinceBorders[pProvince] = BuildPath(pProvince.m_cFirstLines, true);
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
                    foreach (ContinentX pContinent in m_pWorld.m_aContinents)
                    {
                        DrawContinentBorder(gr, pContinent, 1, 1);
                        DrawContinentShape(gr, pContinent);

                        foreach (AreaX pArea in pContinent.m_cAreas)
                            DrawArea(gr, pArea);

                        //DrawContinentBorder(gr, pContinent, 0, 0);
                    }
                    break;
                case VisType.Humidity:
                    foreach (ContinentX pContinent in m_pWorld.m_aContinents)
                        foreach (LandMass<LandX> pLandMass in pContinent.m_cContents)
                            foreach (LandX pLand in pLandMass.m_cContents)
                                DrawLandHumidity(gr, pLand);

                    foreach (ContinentX pContinent in m_pWorld.m_aContinents)
                        DrawContinentBorder(gr, pContinent, 0, 0);

                    break;
                case VisType.RacesNative:
                    foreach (ContinentX pContinent in m_pWorld.m_aContinents)
                    {
                        DrawContinentBorder(gr, pContinent, 0, 0);
                        //DrawContinentShape(gr, pContinent);

                        foreach (AreaX pArea in pContinent.m_cAreas)
                            DrawRace(gr, pArea);

                        //DrawContinentBorder(gr, pContinent, 0, 0);
                    }
                    break;
                case VisType.RacesStates:
                    foreach (ContinentX pContinent in m_pWorld.m_aContinents)
                    {
                        DrawContinentBorder(gr, pContinent, 0, 0);
                        //DrawContinentShape(gr, pContinent);

                        //DrawContinentBorder(gr, pContinent, 0, 0);
                    }
                    foreach (Province pProvince in m_pWorld.m_aProvinces)
                        DrawDominatedRace(gr, pProvince);

                    break;
            }

            if (m_bShowLocationsBorders)
            {
                foreach (LandMass<LandX> pLandMass in m_pWorld.m_aLandMasses)
                    foreach (LandX pLand in pLandMass.m_cContents)
                        foreach (LocationX pLoc in pLand.m_cContents)
                            DrawLocationBorder(gr, pLoc);

                //foreach (Land pLand in pState.m_cLands)
                //    DrawLandBorder(gr, pLand);
            }

            if (m_bShowLands)
            {
                foreach (LandMass<LandX> pLandMass in m_pWorld.m_aLandMasses)
                    foreach (LandX pLand in pLandMass.m_cContents)
                        DrawLandBorder(gr, pLand);
            }

            if (m_bShowProvincies)
            {
                foreach (Province pProvince in m_pWorld.m_aProvinces)
                    if(!pProvince.m_pCenter.IsWater)
                        DrawProvinceBorder(gr, pProvince);
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

                if (m_bShowLocations)
                {
                    foreach (LandMass<LandX> pLandMass in pContinent.m_cContents)
                        foreach (LandX pLand in pLandMass.m_cContents)
                            foreach (LocationX pLoc in pLand.m_cContents)
                                DrawLocation(gr, pLoc);
                }
            }

            if (m_bShowLandMasses)
            {
                foreach (LandMass<LandX> pLandMass in m_pWorld.m_aLandMasses)
                    //if (pLandMass.m_pDrift > 0)
                        DrawLandMass(gr, pLandMass);

                //foreach (LandMass<LandX> pLandMass in m_pWorld.m_cLandMasses)
                //    if (pLandMass.m_pDrift <= 0 )
                //        DrawLandMass(gr, pLandMass);
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

        private Color GetHumidityColor(int iHumidity)
        {
            KColor color = new KColor();
            color.RGB = Color.LightBlue;
            color.Lightness = 1.0 - (double)iHumidity / 200;
            return color.RGB; //Color.FromArgb(0, 0, 255 - 128 * iHumidity / 100);
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

            GraphicsPath pPath = BuildPath(m_pSelectedState.m_cFirstLines, false);
            RectangleF pRect = pPath.GetBounds();

            // Copy to a temporary variable to be thread-safe.
            EventHandler<StateSelectedEventArgs> temp = StateSelectedEvent;
            if (temp != null)
                temp(this, new StateSelectedEventArgs(m_pSelectedState, (int)(pRect.Left + pRect.Width / 2), (int)(pRect.Top + pRect.Height / 2)));
        }

        public Point GetCentralPoint(State pState)
        {
            GraphicsPath pPath = BuildPath(pState.m_cFirstLines, false);
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
                GraphicsPath pContinentPath = BuildPath(pContinent.m_cFirstLines, true);

                if (pContinentPath.IsVisible(iX, iY))
                {
                    m_pFocusedContinent = pContinent;

                    foreach (State pState in pContinent.m_cStates)
                    {
                        GraphicsPath pStatePath = BuildPath(pState.m_cFirstLines, true);

                        if (pStatePath.IsVisible(iX, iY))
                        {
                            m_pFocusedState = pState;

                            foreach (Province pProvince in pState.m_cContents)
                            {
                                GraphicsPath pProvincePath = BuildPath(pProvince.m_cFirstLines, true);

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
                    GraphicsPath pLandPath = BuildPath(pLand.m_cFirstLines, true);

                    if (pLandPath.IsVisible(iX, iY))
                    {
                        m_pFocusedLand = pLand;

                        foreach (LocationX pLoc in pLand.m_cContents)
                        {
                            bool bCross;
                            List<Point> cLocationBorder = BuildBorder(pLoc.m_pFirstLine, 0, out bCross);

                            GraphicsPath pLocationPath = new GraphicsPath();
                            pLocationPath.AddPolygon(cLocationBorder.ToArray());

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
