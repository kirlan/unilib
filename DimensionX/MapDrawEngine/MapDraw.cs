﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nsUniLibControls;
using Socium;
using System.Drawing.Drawing2D;
using LandscapeGeneration;
using LandscapeGeneration.PathFind;
using MapDrawEngine.Signs;
using Random;

namespace MapDrawEngine
{
    public partial class MapDraw : UserControl
    {
        #region Предопределённые перья для рисования
        internal static Pen s_pDarkGrey3Pen = new Pen(Color.DarkGray, 3);
        internal static Pen s_pAqua1Pen = new Pen(Color.Aqua, 1);
        internal static Pen s_pAqua2Pen = new Pen(Color.Aqua, 2);
        internal static Pen s_pRed2Pen = new Pen(Color.Red, 2);
        internal static Pen s_pRed3Pen = new Pen(Color.Red, 3);
        internal static Pen s_pBlack1Pen = new Pen(Color.Black, 1);
        internal static Pen s_pBlack2Pen = new Pen(Color.Black, 2);
        internal static Pen s_pBlack3Pen = new Pen(Color.Black, 3);
        internal static Pen s_pWhite2Pen = new Pen(Color.White, 2);

        internal static Pen s_pAqua1DotPen = new Pen(Color.Aqua, 1);
        internal static Pen s_pBlack1DotPen = new Pen(Color.Black, 1);
        internal static Pen s_pBlack3DotPen = new Pen(Color.Black, 3);
        #endregion

        /// <summary>
        /// Информация о том, какое государство выбрано на карте
        /// </summary>
        public class SelectedStateChangedEventArgs : EventArgs
        {
            /// <summary>
            /// Выбранное государство
            /// </summary>
            public State m_pState;

            /// <summary>
            /// X-координата центра государства в экранных координатах
            /// </summary>
            public int m_iX;
            /// <summary>
            /// Y-координата центра государства в экранных координатах
            /// </summary>
            public int m_iY;

            public SelectedStateChangedEventArgs(State pState, int iX, int iY)
            {
                m_pState = pState;

                m_iX = iX;
                m_iY = iY;
            }
        }

        #region Свойства и переменные
        /// <summary>
        /// холст, на котором мы будем рисовать
        /// </summary>
        private Bitmap m_pCanvas = null;

        /// <summary>
        /// матрица квадрантов
        /// </summary>
        private MapQuadrant[,] m_aQuadrants;

        /// <summary>
        /// цвета разных зон влажности
        /// </summary>
        private Brush[] m_aHumidity;

        /// <summary>
        /// размерность матрицы квадрантов
        /// </summary>
        private const int QUADRANTS_COUNT = 8;

        /// <summary>
        /// Контуры конкретных континентов - для определения, над каким из них находится мышь.
        /// В масштабе 1:100 для ускорения.
        /// </summary>
        private Dictionary<ContinentX, GraphicsPath> m_cContinentBorders = new Dictionary<ContinentX, GraphicsPath>();

        /// <summary>
        /// Контуры конкретных географических регионов - для определения, над каким из них находится мышь.
        /// В масштабе 1:100 для ускорения.
        /// </summary>
        private Dictionary<AreaX, GraphicsPath> m_cAreaBorders = new Dictionary<AreaX, GraphicsPath>();

        /// <summary>
        /// Контуры конкретных провинций - для определения, над какой из них находится мышь.
        /// В масштабе 1:100 для ускорения.
        /// </summary>
        private Dictionary<Province, GraphicsPath> m_cProvinceBorders = new Dictionary<Province, GraphicsPath>();

        /// <summary>
        /// Контуры конкретных тектонических плит - для определения, над какой из них находится мышь.
        /// В масштабе 1:100 для ускорения.
        /// </summary>
        private Dictionary<LandMass<LandX>, GraphicsPath> m_cLandMassBorders = new Dictionary<LandMass<LandX>, GraphicsPath>();

        /// <summary>
        /// Контуры конкретных земель - для определения, над какой из них находится мышь.
        /// В масштабе 1:100 для ускорения.
        /// </summary>
        private Dictionary<LandX, GraphicsPath> m_cLandBorders = new Dictionary<LandX, GraphicsPath>();

        /// <summary>
        /// Контуры конкретных локаций - для определения, над какой из них находится мышь.
        /// В масштабе 1:100 для ускорения.
        /// </summary>
        private Dictionary<LocationX, GraphicsPath> m_cLocationBorders = new Dictionary<LocationX, GraphicsPath>();

        /// <summary>
        /// Контуры конкретных государств - для определения, над каким из них находится мышь.
        /// В масштабе 1:100 для ускорения.
        /// </summary>
        private Dictionary<State, GraphicsPath> m_cStateBorders = new Dictionary<State, GraphicsPath>();

        /// <summary>
        /// мир, карту которого мы рисуем
        /// </summary>
        internal World m_pWorld = null;

        public enum VisType
        {
            /// <summary>
            /// физическая карта
            /// </summary>
            LandType,
            /// <summary>
            /// карта влажности
            /// </summary>
            Humidity,
            /// <summary>
            /// этническая карта - коренное население
            /// </summary>
            RacesNative,
            /// <summary>
            /// этническая карта - доминирующая раса
            /// </summary>
            RacesStates
        }

        /// <summary>
        /// Режим отрисовки карты - физическая карта, карта влажности, этническая карта...
        /// </summary>
        private VisType m_eMode = VisType.LandType;

        /// <summary>
        /// Режим отрисовки карты - физическая карта, карта влажности, этническая карта...
        /// </summary>
        public VisType Mode
        {
            get { return m_eMode; }
            set
            {
                if (m_eMode != value)
                {
                    m_eMode = value;
                    Draw();
                }
            }
        }

        /// <summary>
        /// Отрисовывать ли на карте дороги?
        /// </summary>
        private bool m_bShowRoads = true;

        /// <summary>
        /// Отрисовывать ли на карте дороги?
        /// </summary>
        public bool ShowRoads
        {
            get { return m_bShowRoads; }
            set
            {
                if (m_bShowRoads != value)
                {
                    m_bShowRoads = value;
                    Draw();
                }
            }
        }

        /// <summary>
        /// Отрисовывать ли на карте границы государств?
        /// </summary>
        private bool m_bShowStates = true;

        /// <summary>
        /// Отрисовывать ли на карте границы государств?
        /// </summary>
        public bool ShowStates
        {
            get { return m_bShowStates; }
            set
            {
                if (m_bShowStates != value)
                {
                    m_bShowStates = value;
                    Draw();
                }
            }
        }

        /// <summary>
        /// Отрисовывать ли на карте значки условных обозначений (города и пр.)?
        /// </summary>
        private bool m_bShowLocations = true;

        /// <summary>
        /// Отрисовывать ли на карте значки условных обозначений (города и пр.)?
        /// </summary>
        public bool ShowLocations
        {
            get { return m_bShowLocations; }
            set
            {
                if (m_bShowLocations != value)
                {
                    m_bShowLocations = value;
                    Draw();
                }
            }
        }

        /// <summary>
        /// Отрисовывать ли на карте границы провинций?
        /// </summary>
        private bool m_bShowProvincies = true;

        /// <summary>
        /// Отрисовывать ли на карте границы провинций?
        /// </summary>
        public bool ShowProvincies
        {
            get { return m_bShowProvincies; }
            set
            {
                if (m_bShowProvincies != value)
                {
                    m_bShowProvincies = value;
                    Draw();
                }
            }
        }

        /// <summary>
        /// Отрисовывать ли на карте границы локаций?
        /// </summary>
        private bool m_bShowLocationsBorders = false;

        /// <summary>
        /// Отрисовывать ли на карте границы локаций?
        /// </summary>
        public bool ShowLocationsBorders
        {
            get { return m_bShowLocationsBorders; }
            set
            {
                if (m_bShowLocationsBorders != value)
                {
                    m_bShowLocationsBorders = value;
                    Draw();
                }
            }
        }

        /// <summary>
        /// Отрисовывать ли на карте границы тектонических плит?
        /// </summary>
        private bool m_bShowLandMasses = false;

        /// <summary>
        /// Отрисовывать ли на карте границы тектонических плит?
        /// </summary>
        public bool ShowLandMasses
        {
            get { return m_bShowLandMasses; }
            set
            {
                if (m_bShowLandMasses != value)
                {
                    m_bShowLandMasses = value;
                    Draw();
                }
            }
        }

        /// <summary>
        /// Отрисовывать ли на карте границы земель?
        /// </summary>
        private bool m_bShowLands = false;

        /// <summary>
        /// Отрисовывать ли на карте границы земель?
        /// </summary>
        public bool ShowLands
        {
            get { return m_bShowLands; }
            set
            {
                if (m_bShowLands != value)
                {
                    m_bShowLands = value;
                    Draw();
                }
            }
        }

        /// <summary>
        /// множитель масштаба карты.
        /// при 1 - вся карта по ширине укладывается в 500 пикселей.
        /// для изменения - пользоваться свойством ScaleMultiplier
        /// </summary>
        private float m_fScaleMultiplier = 1;

        /// <summary>
        /// множитель масштаба карты.
        /// при 1 - вся карта по ширине укладывается в 500 пикселей.
        /// при изменении - сразу отрисовывает карту в новом мастабе
        /// </summary>
        public float ScaleMultiplier
        {
            get { return m_fScaleMultiplier; }
            set
            {
                SetScale(value);
            }
        }

        /// <summary>
        /// Отображаемый участок карты.
        /// Координаты - экранные.
        /// </summary>
        private Rectangle m_pDrawFrame;

        /// <summary>
        /// Отображаемый участок карты.
        /// Координаты - экранные.
        /// </summary>
        public Rectangle DrawFrame
        {
            get { return m_pDrawFrame; }
        }

        /// <summary>
        /// коофициент для перевода координат из абсолютной системы координат в экранную
        /// </summary>
        private float m_fActualScale = 1;

        /// <summary>
        /// коофициент для перевода координат из абсолютной системы координат в экранную
        /// </summary>
        public float ActualScale
        {
            get { return m_fActualScale; }
        }

        /// <summary>
        /// смещение отображения карты для центрирования по горизонтали - если ширина карты меньше ширины рабочей поверхности
        /// </summary>
        private int m_iShiftX = 0;
        /// <summary>
        /// смещение отображения карты для центрирования по вертикали - если высота карты меньше высоты рабочей поверхности
        /// </summary>
        private int m_iShiftY = 0;

        /// <summary>
        /// ширина одного квадранта в экранных координатах
        /// </summary>
        float m_fOneQuadWidth;
        /// <summary>
        /// высота одного квадранта в экранных координатах
        /// </summary>
        float m_fOneQuadHeight;

        /// <summary>
        /// ширина отображаемого участка карты в квадрантах
        /// </summary>
        int m_iQuadsWidth;
        /// <summary>
        /// высота отображаемого участка карты в квадрантах
        /// </summary>
        int m_iQuadsHeight;

        /// <summary>
        /// ширина всей карты мира в экранных координатах
        /// </summary>
        public int m_iScaledMapWidth;
        /// <summary>
        /// высота всей карты мира в экранных координатах
        /// </summary>
        public int m_iScaledMapHeight;

        /// <summary>
        /// связанная миникарта
        /// </summary>
        private MiniMapDraw m_pMiniMap = null;

        /// <summary>
        /// Континент, над которым находится указатель мыши.
        /// </summary>
        private ContinentX m_pFocusedContinent = null;

        /// <summary>
        /// Континент, над которым находится указатель мыши.
        /// </summary>
        public ContinentX ContinentInFocus
        {
            get { return m_pFocusedContinent; }
        }

        /// <summary>
        /// Провинция, над которой находится указатель мыши.
        /// </summary>
        private Province m_pFocusedProvince = null;

        /// <summary>
        /// Государство, над которым находится указатель мыши.
        /// </summary>
        private State m_pFocusedState = null;

        /// <summary>
        /// Земля, над которой находится указатель мыши.
        /// </summary>
        private LandX m_pFocusedLand = null;

        /// <summary>
        /// Земля, над которой находится указатель мыши.
        /// </summary>
        public LandX LandInFocus
        {
            get { return m_pFocusedLand; }
        }

        /// <summary>
        /// Локация, над которой находится указатель мыши.
        /// </summary>
        private LocationX m_pFocusedLocation = null;

        /// <summary>
        /// Локация, над которой находится указатель мыши.
        /// </summary>
        public LocationX LocationInFocus
        {
            get { return m_pFocusedLocation; }
        }

        /// <summary>
        /// Выбранное на карте государство
        /// </summary>
        private State m_pSelectedState = null;

        /// <summary>
        /// Выбранное на карте государство
        /// </summary>
        public State SelectedState
        {
            get { return m_pSelectedState; }
            set
            {
                if (value != m_pSelectedState)
                {
                    m_pSelectedState = value;
                    Draw();

                    Refresh();

                    FireSelectedStateEvent();
                }
            }
        }

        /// <summary>
        /// Событие, извещающее о том, что на карте выбрано новое государство
        /// </summary>
        public event EventHandler<SelectedStateChangedEventArgs> SelectedStateChanged;

        /// <summary>
        /// Маршруты, которые нужно отрисовывать на карте
        /// </summary>
        private Dictionary<TransportationNode[], Pen> m_cPaths = new Dictionary<TransportationNode[], Pen>();

        #endregion

        public MapDraw()
        {
            InitializeComponent();
        
            s_pBlack1DotPen.DashPattern = new float[] { 1, 2 };
            s_pAqua1DotPen.DashPattern = new float[] { 1, 2 };
            s_pBlack3DotPen.DashPattern = new float[] { 2, 4 };

            m_aQuadrants = new MapQuadrant[QUADRANTS_COUNT, QUADRANTS_COUNT];
            for (int i = 0; i < QUADRANTS_COUNT; i++)
                for (int j = 0; j < QUADRANTS_COUNT; j++)
                {
                    m_aQuadrants[i, j] = new MapQuadrant();
                }

            List<Brush> cHumidity = new List<Brush>();
            for (int i = 0; i <= 100; i++)
                cHumidity.Add(GetHumidityColor(i));
            m_aHumidity = cHumidity.ToArray();
        }

        #region Функции для работы с цветами

        /// <summary>
        /// Вычисляет цвет для отображения заданного уровня влажности
        /// </summary>
        /// <param name="iHumidity">заданный уровень влажности</param>
        /// <returns>цвет</returns>
        private Brush GetHumidityColor(int iHumidity)
        {
            KColor color = new KColor();
            color.RGB = Color.LightBlue;
            color.Lightness = 1.0 - (double)iHumidity / 200;
            return new SolidBrush(color.RGB);
        }

        /// <summary>
        /// Привязка цветов для конкретных рас
        /// </summary>
        private Dictionary<Race, Brush> m_cRaceColorsID = new Dictionary<Race, Brush>();

        /// <summary>
        /// Заготовка цветов для рас
        /// </summary>
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

        #endregion

        /// <summary>
        /// Привязать карту к миру.
        /// Строим контуры всего, что придётся рисовать в ОРИГИНАЛЬНЫХ координатах
        /// и раскидываем их по квадрантам
        /// </summary>
        /// <param name="pWorld">мир</param>
        public void Assign(World pWorld)
        {
            DateTime pTime1 = DateTime.Now;

            //расчистим рабочее место
            m_cContinentBorders.Clear();
            m_cAreaBorders.Clear();
            m_cProvinceBorders.Clear();
            m_cLandMassBorders.Clear();
            m_cLandBorders.Clear();
            m_cLocationBorders.Clear();
            m_cStateBorders.Clear();

            foreach (MapQuadrant pQuad in m_aQuadrants)
                pQuad.Clear();

            m_pWorld = pWorld;

            //если расам ещё не назначены цвета - назначим их
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

            //левый верхний угол зоны отображения - в ноль!
            m_pDrawFrame.X = 0;
            m_pDrawFrame.Y = 0;
            
            PointF[][] aPoints;
            GraphicsPath pPath;
            MapQuadrant[] aQuads;

            //вычислим контуры тектонических плит
            foreach (LandMass<LandX> pLandMass in m_pWorld.m_aLandMasses)
            {
                aPoints = BuildPath(pLandMass.m_cFirstLines, true, out aQuads);
                pPath = new GraphicsPath();
                //пробежимся по всем простым контурам в вычисленном сложном контуре
                foreach (var aPts in aPoints)
                {
                    //добавим вычисленный простой контур к общему рисунку контура тектонической плиты
                    pPath.AddPolygon(aPts);
                    //добавим вычисленный простой контур к рисунку контура тектонической плиты в тех квадрантах, через которые он проходит.
                    foreach(MapQuadrant pQuad in aQuads)
                        pQuad.m_cLayers[MapLayer.LandMasses].AddPolygon(aPts);
                }
                //запомним общий рисунок контура тектонической плиты - чтобы потом определять вхождение указателя мыши в него.
                m_cLandMassBorders[pLandMass] = pPath;
                //вычислим контуры земель
                foreach (LandX pLand in pLandMass.m_cContents)
                {
                    aPoints = BuildPath(pLand.m_cFirstLines, true, out aQuads);
                    pPath = new GraphicsPath();
                    foreach (var aPts in aPoints)
                    {
                        pPath.AddPolygon(aPts);

                        //определим, каким цветом эта земля должна закрашиваться на карте влажностей
                        Brush pBrush = pLand.IsWater ? pLand.Type.m_pBrush : m_aHumidity[pLand.Humidity];

                        foreach (MapQuadrant pQuad in aQuads)
                        {
                            pQuad.m_cLayers[MapLayer.Lands].AddPolygon(aPts);

                            if (!pQuad.m_cModes[MapMode.Humidity].ContainsKey(pBrush))
                                pQuad.m_cModes[MapMode.Humidity][pBrush] = new GraphicsPath();
                            pQuad.m_cModes[MapMode.Humidity][pBrush].AddPolygon(aPts);
                        }
                    }
                    m_cLandBorders[pLand] = pPath;

                    //вычислим контуры локаций
                    foreach (LocationX pLoc in pLand.m_cContents)
                    {
                        aPoints = BuildPath(pLoc.m_pFirstLine, true, out aQuads);
                        pPath = new GraphicsPath();
                        foreach (var aPts in aPoints)
                        {
                            pPath.AddPolygon(aPts);
                            foreach (MapQuadrant pQuad in aQuads)
                                pQuad.m_cLayers[MapLayer.Locations].AddPolygon(aPts);
                        }
                        m_cLocationBorders[pLoc] = pPath;
                        //добавим информацию о метке на карте
                        AddLocationSign(pLoc);
                    }

                }
            }
            
            //вычислим контуры континентов
            foreach (ContinentX pContinent in m_pWorld.m_aContinents)
            {
                aPoints = BuildPath(pContinent.m_cFirstLines, true, out aQuads);
                pPath = new GraphicsPath();
                foreach (var aPts in aPoints)
                {
                    pPath.AddPolygon(aPts);
                    foreach (MapQuadrant pQuad in aQuads)
                    {
                        if (!pQuad.m_cModes[MapMode.Continents].ContainsKey(LandTypes<LandTypeInfoX>.Plains.m_pBrush))
                            pQuad.m_cModes[MapMode.Continents][LandTypes<LandTypeInfoX>.Plains.m_pBrush] = new GraphicsPath();
                        pQuad.m_cModes[MapMode.Continents][LandTypes<LandTypeInfoX>.Plains.m_pBrush].AddPolygon(aPts);
                    }
                }
                m_cContinentBorders[pContinent] = pPath;

                //вычислим контуры государств
                foreach (State pState in pContinent.m_cStates)
                {
                    aPoints = BuildPath(pState.m_cFirstLines, true, out aQuads);
                    pPath = new GraphicsPath();
                    foreach (var aPts in aPoints)
                    {
                        pPath.AddPolygon(aPts);
                        foreach (MapQuadrant pQuad in aQuads)
                            pQuad.m_cLayers[MapLayer.States].AddPolygon(aPts);
                    }
                    m_cStateBorders[pState] = pPath;
                }

                //вычислим контуры географических регионов
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
                                //в качестве идентификатора типа региона используем цвет, которым этот регион должен рисоваться
                                if (!pQuad.m_cModes[MapMode.Areas].ContainsKey(pArea.m_pType.m_pBrush))
                                    pQuad.m_cModes[MapMode.Areas][pArea.m_pType.m_pBrush] = new GraphicsPath();
                                pQuad.m_cModes[MapMode.Areas][pArea.m_pType.m_pBrush].AddPolygon(aPts);
                            }
                        }

                        //если регион обитаем
                        if (pArea.m_pRace != null)
                        {
                            //сохраним информацию о контуре региона и для этнографической карты
                            Brush pBrush = m_cRaceColorsID[pArea.m_pRace];
                            foreach (MapQuadrant pQuad in aQuads)
                            {
                                if (!pQuad.m_cModes[MapMode.Natives].ContainsKey(pBrush))
                                    pQuad.m_cModes[MapMode.Natives][pBrush] = new GraphicsPath();
                                pQuad.m_cModes[MapMode.Natives][pBrush].AddPolygon(aPts);
                            }
                        }
                    }
                    m_cAreaBorders[pArea] = pPath;
                }
            }

            //сохраним сдвинутые на 1 вниз и влево контуры континентов - для более красивого отображения береговой линии
            Matrix pMatrix = new Matrix();
            pMatrix.Translate(1, 1);
            
            foreach (MapQuadrant pQuad in m_aQuadrants)
            {
                if (pQuad.m_cModes[MapMode.Continents].TryGetValue(LandTypes<LandTypeInfoX>.Plains.m_pBrush, out pPath))
                {
                    pQuad.m_cLayers[MapLayer.ContinentsShadow] = (GraphicsPath)pPath.Clone();
                    pQuad.m_cLayers[MapLayer.ContinentsShadow].Transform(pMatrix);
                }
            }

            //вычислим контуры провинций
            foreach (Province pProvince in m_pWorld.m_aProvinces)
            {
                aPoints = BuildPath(pProvince.m_cFirstLines, true, out aQuads);
                pPath = new GraphicsPath();
                foreach (var aPts in aPoints)
                {
                    pPath.AddPolygon(aPts);
                    if (!pProvince.m_pCenter.IsWater)
                        foreach (MapQuadrant pQuad in aQuads)
                            pQuad.m_cLayers[MapLayer.Provincies].AddPolygon(aPts);

                    //сохраним информацию о контуре провинции для этнографической карты
                    Brush pBrush = m_cRaceColorsID[pProvince.m_pRace];
                    foreach (MapQuadrant pQuad in aQuads)
                    {
                        if (!pQuad.m_cModes[MapMode.Nations].ContainsKey(pBrush))
                            pQuad.m_cModes[MapMode.Nations][pBrush] = new GraphicsPath();
                        pQuad.m_cModes[MapMode.Nations][pBrush].AddPolygon(aPts);
                    }
                }
                m_cProvinceBorders[pProvince] = pPath;
            }

            //вычислим дорожную сетку
            foreach (TransportationLink pRoad in m_pWorld.m_cTransportGrid)
            {
                AddRoad(pRoad);
            }

            //нормализуем все квадранты таким образом, чтобы внутри каждого квадранта координаты считались от
            //верхнего левого угла самого квадранта, а не от центра оригинальной системы координат
            for (int i = 0; i < QUADRANTS_COUNT; i++)
                for (int j = 0; j < QUADRANTS_COUNT; j++)
                {
                    float fQuadX = m_pWorld.m_cGrid.RX * i * 2 / QUADRANTS_COUNT;
                    float fQuadY = m_pWorld.m_cGrid.RY * j * 2 / QUADRANTS_COUNT;
                    m_aQuadrants[i, j].Normalize(fQuadX, fQuadY);
                }

            //смасштабируем используемые для определения положения курсора контуры в 100 раз - 
            //для ускорения выполнения функции GraphicsPath::IsVisible(x,y)
            Matrix pScaleMatrix = new Matrix();
            pScaleMatrix.Scale(0.01f, 0.01f);

            foreach (var pPair in m_cContinentBorders)
                pPair.Value.Transform(pScaleMatrix);

            foreach (var pPair in m_cAreaBorders)
                pPair.Value.Transform(pScaleMatrix);

            foreach (var pPair in m_cProvinceBorders)
                pPair.Value.Transform(pScaleMatrix);

            foreach (var pPair in m_cLandMassBorders)
                pPair.Value.Transform(pScaleMatrix);

            foreach (var pPair in m_cLandBorders)
                pPair.Value.Transform(pScaleMatrix);

            foreach (var pPair in m_cLocationBorders)
                pPair.Value.Transform(pScaleMatrix);

            foreach (var pPair in m_cStateBorders)
                pPair.Value.Transform(pScaleMatrix);

            DateTime pTime2 = DateTime.Now;

            ScaleMultiplier = 1;

            if (m_pMiniMap != null)
                m_pMiniMap.WorldAssigned();
        }

        #region Вспомогательные функции, используемые в Assign

        /// <summary>
        /// Строит сложный контур в ОРИГИНАЛЬНЫХ координатах (с отражениями)
        /// Сложный контур - это когда например, у континента есть две замкнутые береговые линии - 
        /// одна с внешним океаном, а другая с внутренним морем. Каждая из них - простой контур,
        /// а в совокупности - сложный.
        /// </summary>
        /// <param name="cFirstLines">Затравки контуров</param>
        /// <param name="bMirror">Строить ли отражения для зацикленного мира. Нужно при отрисовке, но нельзя при определении описывающего прямоугольника.</param>
        /// <param name="aQuadrants">список квадрантов, в которые входит этот контур</param>
        /// <returns></returns>
        private PointF[][] BuildPath(List<Line> cFirstLines, bool bMirror, out MapQuadrant[] aQuadrants)
        {
            bool[,] aQuadsAll = new bool[QUADRANTS_COUNT, QUADRANTS_COUNT];
            List<PointF[]> cPath = new List<PointF[]>();

            //пробежимся по всем затравкам
            foreach (Line pFirstLine in cFirstLines)
            {
                bool bCross;
                bool[,] aQuads;

                //получаем простой одиночный контур
                cPath.Add(BuildBorder(pFirstLine, 0, out bCross, out aQuads));

                //определяем, через какие квадранты он проходит
                for (int i = 0; i < QUADRANTS_COUNT; i++)
                    for (int j = 0; j < QUADRANTS_COUNT; j++)
                        if (aQuads[i, j])
                            aQuadsAll[i, j] = true;

                //если карта зациклена по горизонтали, нужно строить отражения и 
                //контур пересекает нулевой меридиан, то строим отражение!
                if (m_pWorld.m_cGrid.m_bCycled && bMirror && bCross)
                {
                    //определяем, на западе или на востоке будем строить отражение
                    if (pFirstLine.m_pPoint1.X > 0)
                        cPath.Add(BuildBorder(pFirstLine, -m_pWorld.m_cGrid.RX * 2, out bCross, out aQuads));
                    else
                        cPath.Add(BuildBorder(pFirstLine, m_pWorld.m_cGrid.RX * 2, out bCross, out aQuads));

                    //определяем, через какие квадранты оно проходит
                    for (int i = 0; i < QUADRANTS_COUNT; i++)
                        for (int j = 0; j < QUADRANTS_COUNT; j++)
                            if (aQuads[i, j])
                                aQuadsAll[i, j] = true;
                }
            }

            //переносим все квадранты, через которые проходит контур и его отражение, в линейный массив
            List<MapQuadrant> cQuadrants = new List<MapQuadrant>();
            for (int i = 0; i < QUADRANTS_COUNT; i++)
                for (int j = 0; j < QUADRANTS_COUNT; j++)
                    if (aQuadsAll[i, j])
                        cQuadrants.Add(m_aQuadrants[i, j]);
            aQuadrants = cQuadrants.ToArray();

            return cPath.ToArray();
        }

        /// <summary>
        /// строит простой контур в ОРИГИНАЛЬНЫХ координатах (с отражениями)
        /// </summary>
        /// <param name="pFirstLine">затравка контура</param>
        /// <param name="bMirror">Строить ли отражения для зацикленного мира. Нужно при отрисовке, но нельзя при определении описывающего прямоугольника.</param>
        /// <param name="aQuadrants">список квадрантов, в которые входит этот контур</param>
        /// <returns></returns>
        private PointF[][] BuildPath(Line pFirstLine, bool bMirror, out MapQuadrant[] aQuadrants)
        {
            bool[,] aQuadsAll = new bool[QUADRANTS_COUNT, QUADRANTS_COUNT];
            List<PointF[]> cPath = new List<PointF[]>();

            bool bCross;
            bool[,] aQuads;

            //получаем простой одиночный контур
            cPath.Add(BuildBorder(pFirstLine, 0, out bCross, out aQuads));

            //определяем, через какие квадранты он проходит
            for (int i = 0; i < QUADRANTS_COUNT; i++)
                for (int j = 0; j < QUADRANTS_COUNT; j++)
                    if (aQuads[i, j])
                        aQuadsAll[i, j] = true;

            //если карта зациклена по горизонтали, нужно строить отражения и 
            //контур пересекает нулевой меридиан, то строим отражение!
            if (m_pWorld.m_cGrid.m_bCycled && bMirror && bCross)
            {
                //определяем, на западе или на востоке будем строить отражение
                if (pFirstLine.m_pPoint1.X > 0)
                    cPath.Add(BuildBorder(pFirstLine, -m_pWorld.m_cGrid.RX * 2, out bCross, out aQuads));
                else
                    cPath.Add(BuildBorder(pFirstLine, m_pWorld.m_cGrid.RX * 2, out bCross, out aQuads));

                //определяем, через какие квадранты оно проходит
                for (int i = 0; i < QUADRANTS_COUNT; i++)
                    for (int j = 0; j < QUADRANTS_COUNT; j++)
                        if (aQuads[i, j])
                            aQuadsAll[i, j] = true;
            }

            //переносим все квадранты, через которые проходит контур и его отражение, в линейный массив
            List<MapQuadrant> cQuadrants = new List<MapQuadrant>();
            for (int i = 0; i < QUADRANTS_COUNT; i++)
                for (int j = 0; j < QUADRANTS_COUNT; j++)
                    if (aQuadsAll[i, j])
                        cQuadrants.Add(m_aQuadrants[i, j]);
            aQuadrants = cQuadrants.ToArray();

            return cPath.ToArray();
        }

        /// <summary>
        /// строит простой контур в ОРИГИНАЛЬНЫХ координатах (без отражений)
        /// </summary>
        /// <param name="pFirstLine">затравка контура</param>
        /// <param name="fShift">сдвиг по горизонтали для закольцованной карты</param>
        /// <param name="bCross">признак того, что контур пересекает нулевой меридиан</param>
        /// <param name="aQuadrants">массив, в котором указано, в каких квадрантах лежит контур</param>
        /// <returns></returns>
        private PointF[] BuildBorder(Line pFirstLine, float fShift, out bool bCross, out bool[,] aQuadrants)
        {
            bCross = false;

            aQuadrants = new bool[QUADRANTS_COUNT, QUADRANTS_COUNT];

            List<PointF> cBorder = new List<PointF>();
            Line pLine = pFirstLine;
            cBorder.Add(ShiftPoint(pLine.m_pPoint1, fShift));
            float fLastPointX = pLine.m_pPoint1.X + fShift;
            //последовательно перебирает все связанные линии, пока круг не замкнётся.
            do
            {
                //в каком квадранте лежит первая точка линии
                int iQuad1X = (int)(QUADRANTS_COUNT * (pLine.m_pPoint1.X + m_pWorld.m_cGrid.RX) / (2*m_pWorld.m_cGrid.RX));
                int iQuad1Y = (int)(QUADRANTS_COUNT * (pLine.m_pPoint1.Y + m_pWorld.m_cGrid.RY) / (2*m_pWorld.m_cGrid.RY));

                if (iQuad1X >= 0 && iQuad1X < QUADRANTS_COUNT && iQuad1Y >= 0 && iQuad1Y < QUADRANTS_COUNT)
                    aQuadrants[iQuad1X, iQuad1Y] = true;

                //в каком квадранте лежит вторая точка линии
                int iQuad2X = (int)(QUADRANTS_COUNT * (pLine.m_pPoint2.X + m_pWorld.m_cGrid.RX) / (2*m_pWorld.m_cGrid.RX));
                int iQuad2Y = (int)(QUADRANTS_COUNT * (pLine.m_pPoint2.Y + m_pWorld.m_cGrid.RY) / (2*m_pWorld.m_cGrid.RY));

                if (iQuad2X >= 0 && iQuad2X < QUADRANTS_COUNT && iQuad2Y >= 0 && iQuad2Y < QUADRANTS_COUNT)
                    aQuadrants[iQuad2X, iQuad2Y] = true;

                //пересекает-ли линия нулевой меридиан?
                float fDX = fShift;
                if (Math.Abs(fLastPointX - pLine.m_pPoint2.X - fShift) > m_pWorld.m_cGrid.RX)
                {
                    //определимся, где у нас была предыдущая часть контура - на западе или на востоке?
                    //в зависимости от этого вычислим смещение для оставшейся части контура, чтобы 
                    //не было разрыва
                    fDX += fLastPointX < fShift ? -m_pWorld.m_cGrid.RX * 2 : m_pWorld.m_cGrid.RX * 2;
                    bCross = true;
                }

                if (pLine.m_pPoint2.X > m_pWorld.m_cGrid.RX ||
                    pLine.m_pPoint2.X < -m_pWorld.m_cGrid.RX)
                    bCross = true;

                cBorder.Add(ShiftPoint(pLine.m_pPoint2, fDX));

                //X-координата последней добавленной точки с учётом вычисленного смещения
                fLastPointX = pLine.m_pPoint2.X + fDX;

                pLine = pLine.m_pNext;
            }
            while (pLine != pFirstLine);

            return cBorder.ToArray();
        }

        /// <summary>
        /// Смещает начало координат из пересечения экватора и нулевого меридиана в верхний левый угол карты, плюс заданное смещение по горизонтали.
        /// </summary>
        /// <param name="pPoint">исходная точка</param>
        /// <param name="fDX">заданное смещение</param>
        /// <returns>смещённая точка</returns>
        private PointF ShiftPoint(IPointF pPoint, float fDX)
        {
            return new PointF(m_pWorld.m_cGrid.RX + pPoint.X + fDX, m_pWorld.m_cGrid.RY + pPoint.Y);
        }

        /// <summary>
        /// Смещает начало координат из пересечения экватора и нулевого меридиана в верхний левый угол карты, плюс заданное смещение по горизонтали.
        /// </summary>
        /// <param name="pPoint">исходная точка</param>
        /// <param name="fDX">заданное смещение</param>
        /// <returns>смещённая точка</returns>
        private PointF ShiftPoint(PointF pPoint, float fDX)
        {
            return new PointF(m_pWorld.m_cGrid.RX + pPoint.X + fDX, m_pWorld.m_cGrid.RY + pPoint.Y);
        }

        /// <summary>
        /// добавляет в соответствующий квадрант информацию о метке на карте
        /// </summary>
        /// <param name="pLoc">локация, содержащая метку</param>
        private void AddLocationSign(LocationX pLoc)
        {
            float fPointX = m_pWorld.m_cGrid.RX + pLoc.m_pCenter.X;
            float fPointY = m_pWorld.m_cGrid.RY + pLoc.m_pCenter.Y;

            int iQuadX = (int)(QUADRANTS_COUNT * (pLoc.m_pCenter.X + m_pWorld.m_cGrid.RX) / (2*m_pWorld.m_cGrid.RX));
            int iQuadY = (int)(QUADRANTS_COUNT * (pLoc.m_pCenter.Y + m_pWorld.m_cGrid.RY) / (2*m_pWorld.m_cGrid.RY));

            if (iQuadX < 0 || iQuadX >= QUADRANTS_COUNT || iQuadY < 0 || iQuadY >= QUADRANTS_COUNT)
                return;

            switch (pLoc.m_eType)
            {
                case RegionType.Peak:
                    m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignPeak(fPointX, fPointY, ""));
                    break;
                case RegionType.Volcano:
                    m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignVolkano(fPointX, fPointY, ""));
                    break;
            }

            if (pLoc.m_pSettlement != null)
            {
                if (pLoc.m_pSettlement.m_iRuinsAge > 0)
                {
                    m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignRuin(fPointX, fPointY, ""));
                }
                else
                {
                    switch (pLoc.m_pSettlement.m_pInfo.m_eSize)
                    {
                        case SettlementSize.Capital:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignCapital(fPointX, fPointY, pLoc.m_pSettlement.m_sName));
                            break;
                        case SettlementSize.City:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignCity(fPointX, fPointY, pLoc.m_pSettlement.m_sName));
                            break;
                        case SettlementSize.Town:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignTown(fPointX, fPointY, pLoc.m_pSettlement.m_sName));
                            break;
                        case SettlementSize.Village:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignVillage(fPointX, fPointY, pLoc.m_pSettlement.m_sName, (pLoc.Owner as LandX).Type.m_pBrush));
                            break;
                        case SettlementSize.Hamlet:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignVillage(fPointX, fPointY, pLoc.m_pSettlement.m_sName, (pLoc.Owner as LandX).Type.m_pBrush));
                            break;
                        case SettlementSize.Fort:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignFort(fPointX, fPointY, pLoc.m_pSettlement.m_sName));
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
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignLair(fPointX, fPointY, ""));
                            break;
                        case BuildingType.Hideout:
                            m_aQuadrants[iQuadX, iQuadY].m_cLandmarks.Add(new SignHideout(fPointX, fPointY, ""));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Сохраняет информацию об участке дороги в соответствующем квадранте
        /// </summary>
        /// <param name="pRoad">участок дороги</param>
        private void AddRoad(TransportationLink pRoad)
        {
            if (pRoad.RoadLevel == 0)
                return;

            RoadType eRoadType = RoadType.LandRoad2;

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

            bool[,] aQuadrants;
            PointF[][] aLinks = GetTransportationLink(pRoad, out aQuadrants);

            for (int i = 0; i < QUADRANTS_COUNT; i++)
                for (int j = 0; j < QUADRANTS_COUNT; j++)
                    if (aQuadrants[i, j])
                    {
                        foreach (var aLink in aLinks)
                        {
                            m_aQuadrants[i, j].m_cRoadsMap[eRoadType].StartFigure();
                            //m_aQuadrants[i, j].m_cRoadsMap[eRoadType].AddLines(aLink);
                            m_aQuadrants[i, j].m_cRoadsMap[eRoadType].AddCurve(aLink);

                            if (pRoad.RoadLevel == 3)
                            {
                                m_aQuadrants[i, j].m_cRoadsMap[RoadType.Back].StartFigure();
                                //m_aQuadrants[i, j].m_cRoadsMap[RoadType.Back].AddLines(aLink);
                                m_aQuadrants[i, j].m_cRoadsMap[RoadType.Back].AddCurve(aLink);
                            }
                        }
                    }
        }

        /// <summary>
        /// строит линию, отображающую участок дороги - в ОРИГИНАЛЬНЫХ координатах (с отражениями)
        /// </summary>
        /// <param name="pRoad">участок дороги</param>
        /// <param name="aQuadrants">список квадрантов, через которые проходит дорога</param>
        /// <returns></returns>
        private PointF[][] GetTransportationLink(TransportationLink pRoad, out bool[,] aQuadrants)
        {
            aQuadrants = new bool[QUADRANTS_COUNT, QUADRANTS_COUNT];

            bool[,] aQuads;
            List<PointF[]> cPathLines = new List<PointF[]>();

            bool bCross;
            //получаем линию без отражений
            cPathLines.Add(BuildPathLine(pRoad, 0, out bCross, out aQuads));

            //если мир закольцован и построенная линия пересекает нулевой меридиан,
            //то построим для неё отражение
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

            //переносим информацию о квадрантах, через которые проходит линия, в общую матрицу
            for (int i = 0; i < QUADRANTS_COUNT; i++)
                for (int j = 0; j < QUADRANTS_COUNT; j++)
                    if (aQuads[i, j])
                        aQuadrants[i, j] = true; 
            
            return cPathLines.ToArray();
        }

        /// <summary>
        /// строит линию, отображающую участок дороги - в ОРИГИНАЛЬНЫХ координатах (без отражений)
        /// </summary>
        /// <param name="pPath">участок дороги</param>
        /// <param name="fShift">сдвиг координат - для построения отражений</param>
        /// <param name="bCross">признак того, что дорога пересекает нулевой меридиан</param>
        /// <param name="aQuadrants">список квадрантов, через которые проходит дорога</param>
        /// <returns>массив вершин ломаной линии</returns>
        private PointF[] BuildPathLine(TransportationLink pPath, float fShift, out bool bCross, out bool[,] aQuadrants)
        {
            bCross = false;
            aQuadrants = new bool[QUADRANTS_COUNT, QUADRANTS_COUNT];

            List<PointF> cRoadLine = new List<PointF>();
            float fLastPointX = pPath.m_aPoints[0].X + fShift;
            foreach (PointF pPoint in pPath.m_aPoints)
            {
                //пересекает-ли линия от предыдущей точки к текущей нулевой меридиан?
                float fDX = fShift;
                if (Math.Abs(fLastPointX - pPoint.X - fShift) > m_pWorld.m_cGrid.RX)
                {
                    //определимся, где у нас была предыдущая часть линии - на западе или на востоке?
                    //в зависимости от этого вычислим смещение для оставшейся части линии, чтобы 
                    //не было разрыва
                    fDX += fLastPointX < fShift ? -m_pWorld.m_cGrid.RX * 2 : m_pWorld.m_cGrid.RX * 2;
                    bCross = true;
                }
                
                if (pPoint.X > m_pWorld.m_cGrid.RX ||
                    pPoint.X < -m_pWorld.m_cGrid.RX)
                    bCross = true;
                
                cRoadLine.Add(ShiftPoint(pPoint, fDX));

                //в каком квадранте лежит новая точка линии
                int iQuadX = (int)(QUADRANTS_COUNT * (pPoint.X + m_pWorld.m_cGrid.RX) / (2*m_pWorld.m_cGrid.RX));
                int iQuadY = (int)(QUADRANTS_COUNT * (pPoint.Y + m_pWorld.m_cGrid.RY) / (2*m_pWorld.m_cGrid.RY));

                if (iQuadX >= 0 && iQuadX < QUADRANTS_COUNT && iQuadY >= 0 && iQuadY < QUADRANTS_COUNT)
                    aQuadrants[iQuadX, iQuadY] = true;

                fLastPointX = pPoint.X + fDX;
            }

            return cRoadLine.ToArray();
        }
        #endregion

        /// <summary>
        /// Привязывает к нашей карте миникарту
        /// </summary>
        /// <param name="pMiniMap">контрол - миникарта</param>
        public void BindMiniMap(MiniMapDraw pMiniMap)
        {
            m_pMiniMap = pMiniMap;
            m_pMiniMap.Assign(this);
        }

        /// <summary>
        /// создаёт холст для рисования видимого участка карты в текущем масштабе.
        /// размеры холста зависят от размеров окна рисования и выбранного масштаба карты (холст может быть меньше окна рисования).
        /// так же здесь выполняется пересчёт всех мастабных коэффициентов
        /// </summary>
        private void CreateCanvas()
        {
            //если компонент не готов - все в сад
            if (ClientRectangle.Width == 0)
                return;

            //соотношение высоты и ширины координатной сетки мира
            float fK = 1;
            if (m_pWorld != null)
                fK = (float)m_pWorld.m_cGrid.RY / m_pWorld.m_cGrid.RX;

            //ширина и высота карты мира в экранных координатах
            //из расчёта того, чтобы при единичном масштабе вся карта имела ширину 500 пикселей
            m_iScaledMapWidth = (int)(500 * m_fScaleMultiplier);
            m_iScaledMapHeight = (int)(m_iScaledMapWidth * fK);

            //создадим холст такого же размера, как и окно рисования, но не больше 
            //отмасштабированной карты
            m_pCanvas = new Bitmap(Math.Min(ClientRectangle.Width, m_iScaledMapWidth), Math.Min(ClientRectangle.Height, m_iScaledMapHeight));

            //коэффициент для перевода координат из абсолютной системы координат в экранную
            if (m_pWorld != null)
                m_fActualScale = (float)(m_iScaledMapWidth) / (m_pWorld.m_cGrid.RX * 2);

            //если холст уже окна рисования, вычислим смещение для центрирования холста
            m_iShiftX = (ClientRectangle.Width - m_pCanvas.Width) / 2;

            //если холст меньше окна рисования по вертикали, вычислим смещение для центрирования холста
            m_iShiftY = (ClientRectangle.Height - m_pCanvas.Height) / 2;

            //определим размеры отображаемого участка карты равными размерам холста
            m_pDrawFrame.Width = m_pCanvas.Width;
            m_pDrawFrame.Height = m_pCanvas.Height;

            //размеры одного квадранта в экранных координатах из рассчёта сетки квадрантов 8х8
            m_fOneQuadWidth = m_fActualScale * m_pWorld.m_cGrid.RX * 2 / QUADRANTS_COUNT;
            m_fOneQuadHeight = m_fActualScale * m_pWorld.m_cGrid.RY * 2 / QUADRANTS_COUNT;

            //размеры отображаемого участка карты в квадрантах
            //+2 потому что 1 квадрант мы отображаем всегда и нужно ещё иметь запас в 1 квадрант на случай, 
            //когда рамка отображаемой области не совпадает с границами квадрантов
            m_iQuadsWidth = 2 + (int)(m_pDrawFrame.Width / m_fOneQuadWidth);
            m_iQuadsHeight = 2 + (int)(m_pDrawFrame.Height / m_fOneQuadHeight);
        }

        #region Управление картой - масштабирование и смещение зоны просмотра

        /// <summary>
        /// Изменяет масштаб и перерисовывает карту
        /// </summary>
        /// <param name="fNewScale">новый масштаб</param>
        private void SetScale(float fNewScale)
        {
            //если окно рисования неактивно или нет мира - делать нечего
            if (ClientRectangle.Width == 0 || m_pWorld == null)
            {
                m_fScaleMultiplier = fNewScale;
                return;
            }

            bool bFirstTime = (m_pCanvas == null);

            //если это не первая отрисовка, то вычислим центр зоны зоны отображения в 
            //оригинальных координатах, чтобы потом можно было восстановить его
            int iDrawFrameCenterX = 0;
            int iDrawFrameCenterY = 0;
            if (!bFirstTime)
            {
                iDrawFrameCenterX = (int)((m_pDrawFrame.X + m_pDrawFrame.Width / 2) / m_fActualScale);
                iDrawFrameCenterY = (int)((m_pDrawFrame.Y + m_pDrawFrame.Height / 2) / m_fActualScale);
            }

            m_fScaleMultiplier = fNewScale;

            //строим холст и вычисляем все связанные с масштабированием параметры
            //холст перестраиваем при каждом масштабировании, т.к. его размеры - это
            //меньшие из размеров карты в текущем масштабе и размеров области рисования
            CreateCanvas();

            //если это не первая отрисовка, то сдвинем область просмотра так, чтобы её центр оказался
            //в тех же оригинальных координатах, что и до мастабирования.
            //иначе - просто отрисовываем карту.
            if (!bFirstTime)
                SetPan((int)(m_fActualScale * iDrawFrameCenterX) - m_pDrawFrame.Width / 2, (int)(m_fActualScale * iDrawFrameCenterY) - m_pDrawFrame.Height / 2);
            else
                Draw();

            //если есть связанная миникарта - сигнализируем ей о том, что 
            //координаты и размеры области отображения изменились
            if (m_pMiniMap != null)
                m_pMiniMap.SinchronizeDrawFrame();
        }

        /// <summary>
        /// сдвигает область просмотра в указанные координаты и перерисовывает карту
        /// </summary>
        /// <param name="iX">X-координата левого верхнего угла области просмотра</param>
        /// <param name="iY">Y-координата левого верхнего угла области просмотра</param>
        public void SetPan(int iX, int iY)
        {
            if (m_pWorld.m_cGrid.m_bCycled)
                m_pDrawFrame.X = iX;
            else
                m_pDrawFrame.X = Math.Max(0, Math.Min(iX, m_iScaledMapWidth - m_pDrawFrame.Width));

            while (m_pDrawFrame.X < 0)
                m_pDrawFrame.X += m_iScaledMapWidth;

            while (m_pDrawFrame.X > m_iScaledMapWidth)
                m_pDrawFrame.X -= m_iScaledMapWidth;

            m_pDrawFrame.Y = Math.Max(0, Math.Min(iY, m_iScaledMapHeight - m_pDrawFrame.Height));

            Draw();

            //если у нас есть связанная миникарта - сигнализируем ей о том, что область просмотра сдвинулась
            if (m_pMiniMap != null)
                m_pMiniMap.SinchronizeDrawFrame();
        }

        public void SinchronizeDrawFrame()
        {
            if (m_pMiniMap == null || m_pWorld == null)
                return;

            m_pDrawFrame.X = (int)(m_fActualScale * m_pMiniMap.DrawFrame.X / m_pMiniMap.ActualScale) - 1;
            m_pDrawFrame.Y = (int)(m_fActualScale * m_pMiniMap.DrawFrame.Y / m_pMiniMap.ActualScale) - 1;
            //m_pDrawFrame.Width = (int)(m_fActualScale * m_pMiniMap.DrawFrame.Width / m_pMiniMap.m_fK);
            //m_pDrawFrame.Height = (int)(m_fActualScale * m_pMiniMap.DrawFrame.Height / m_pMiniMap.m_fK) + 1;

            Draw();
        }
        
        #endregion

        /// <summary>
        /// Нарисовать видимый участок карты на холсте
        /// </summary>
        public void Draw()
        {
            //если хоста нет - делать нечего
            if (m_pCanvas == null)
                return;

            DateTime pTime1 = DateTime.Now;

            Graphics gr = Graphics.FromImage(m_pCanvas);

            //грунтуем холст цветом моря
            gr.FillRectangle(new SolidBrush(LandTypes<LandTypeInfoX>.Sea.m_pColor), 0, 0, m_pCanvas.Width, m_pCanvas.Height);

            //если нет мира или мир вырожденный - больше рисовать нечего
            if (m_pWorld == null || m_pWorld.m_cGrid.m_aLocations.Length == 0)
                return;

            //координаты квадранта, в котором находится левый верхний угол отображаемого участка карты
            int iQuadX = (int)(m_pDrawFrame.X / m_fOneQuadWidth);
            int iQuadY = (int)(m_pDrawFrame.Y / m_fOneQuadHeight);

            //координаты левого верхнего угла отображаемого участка карты внутри квадранта, в котором он находится
            int iQuadDX = (int)(iQuadX * m_fOneQuadWidth) - m_pDrawFrame.X;
            int iQuadDY = (int)(iQuadY * m_fOneQuadHeight) - m_pDrawFrame.Y;

            //массив квадрантов, попадающих в область отображения
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

                    aVisibleQuads[i, j] = m_aQuadrants[iQX, iQY];
                }
            }

            //рисуем контуры континентов и заливаем внутреннее пространство цветом равнин
            for (int i = 0; i < m_iQuadsWidth; i++)
                for (int j = 0; j < m_iQuadsHeight; j++)
                    if (aVisibleQuads[i, j] != null)
                        aVisibleQuads[i, j].DrawPath(gr, MapLayer.ContinentsShadow, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);
            for (int i = 0; i < m_iQuadsWidth; i++)
                for (int j = 0; j < m_iQuadsHeight; j++)
                    if (aVisibleQuads[i, j] != null)
                        aVisibleQuads[i, j].FillPath(gr, MapMode.Continents, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);

            //закрашиваем карту в соответствии с выбранным режимом отображения карты
            for (int i = 0; i < m_iQuadsWidth; i++)
                for (int j = 0; j < m_iQuadsHeight; j++)
                    if (aVisibleQuads[i, j] != null)
                        switch (m_eMode)
                        {
                            case VisType.LandType:
                                aVisibleQuads[i, j].FillPath(gr, MapMode.Areas, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);
                                break;
                            case VisType.Humidity:
                                aVisibleQuads[i, j].FillPath(gr, MapMode.Humidity, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);
                                break;
                            case VisType.RacesNative:
                                aVisibleQuads[i, j].FillPath(gr, MapMode.Natives, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);
                                break;
                            case VisType.RacesStates:
                                aVisibleQuads[i, j].FillPath(gr, MapMode.Nations, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);
                                break;
                        }

            if (m_bShowLocationsBorders)
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            aVisibleQuads[i, j].DrawPath(gr, MapLayer.Locations, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);

            if (m_bShowLands)
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            aVisibleQuads[i, j].DrawPath(gr, MapLayer.Lands, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);

            if (m_bShowProvincies)
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            aVisibleQuads[i, j].DrawPath(gr, MapLayer.Provincies, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);

            //foreach (TransportationNode[] aPath in m_cPaths.Keys)
            //    DrawPath(gr, aPath, m_cPaths[aPath]);

            if (m_bShowRoads)
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            aVisibleQuads[i, j].DrawRoads(gr, m_fScaleMultiplier, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);

            if (m_bShowStates)
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            aVisibleQuads[i, j].DrawPath(gr, MapLayer.States, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);

            if (m_bShowLocations)
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            aVisibleQuads[i, j].DrawLandMarks(gr, m_fScaleMultiplier, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);

            if (m_bShowLandMasses)
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            aVisibleQuads[i, j].DrawPath(gr, MapLayer.LandMasses, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);

            //if (m_pSelectedState != null)
            //    DrawStateBorder(gr, m_pSelectedState);

            Refresh();

            DateTime pTime2 = DateTime.Now;
        }

        private void MapDraw_Paint(object sender, PaintEventArgs e)
        {
            if (m_pCanvas == null)
                return;

            e.Graphics.DrawImage(m_pCanvas, m_iShiftX, m_iShiftY, new Rectangle(0, 0, m_pDrawFrame.Width, m_pDrawFrame.Height), GraphicsUnit.Pixel);
        }

        private void MapDraw_Resize(object sender, EventArgs e)
        {
            if (ClientRectangle.Width == 0 || ClientRectangle.Height == 0 || m_pCanvas == null)
                return;

            //полностью пересчитываем все коэффициенты и перерисовываем карту
            ScaleMultiplier = ScaleMultiplier;
        }

        private bool m_bScrolling = false;

        private Point m_pLastMouseLocation = new Point(0, 0);

        private void MapDraw_MouseDown(object sender, MouseEventArgs e)
        {
            m_bScrolling = true;
            //m_pLastMouseLocation = new Point(-1, -1);
        }

        private void MapDraw_MouseLeave(object sender, EventArgs e)
        {
            m_bScrolling = false;
        }

        private void MapDraw_MouseUp(object sender, MouseEventArgs e)
        {
            m_bScrolling = false;
        }

        private void MapDraw_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = m_pDrawFrame.Location;

            if (m_pLastMouseLocation.X > 0)
            {
                p.X += e.X - m_pLastMouseLocation.X;
                p.Y += e.Y - m_pLastMouseLocation.Y;
            }
            m_pLastMouseLocation = e.Location;

            if (m_bScrolling)
                SetPan(p.X, p.Y);
        }

        /// <summary>
        /// Определяет, что находится под курсором.
        /// </summary>
        /// <returns>true - если какая-то суша, false - если просто океан</returns>
        private bool CheckMousePosition()
        {
            if (m_pWorld == null)
                return false;

            int iX = m_pLastMouseLocation.X + m_pDrawFrame.X - m_iShiftX;
            int iY = m_pLastMouseLocation.Y + m_pDrawFrame.Y - m_iShiftY;

            while (iX > m_iScaledMapWidth)
                iX -= m_iScaledMapWidth;

            while (iX < 0)
                iX += m_iScaledMapWidth;

            //переведём координаты курсора из экранных в оригинальные/100 координаты
            iX = (int)(iX / m_fActualScale) / 100;
            iY = (int)(iY / m_fActualScale) / 100;

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

            return bContinent;
        }

        private void MapDraw_MouseHover(object sender, EventArgs e)
        {

            string sToolTip = "";

            bool bContinent = CheckMousePosition();

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

            if (toolTip1.GetToolTip(this) != sToolTip)
                toolTip1.SetToolTip(this, sToolTip);
        }

        private void MapDraw_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            bool bContinent = CheckMousePosition();
        
            if (m_pFocusedState != null)
                SelectedState = m_pFocusedState;
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
            EventHandler<SelectedStateChangedEventArgs> temp = SelectedStateChanged;
            if (temp != null)
                temp(this, new SelectedStateChangedEventArgs(m_pSelectedState, (int)(pRect.Left + pRect.Width / 2), (int)(pRect.Top + pRect.Height / 2)));
        }

        /// <summary>
        /// Вычисляет центральную точку указанного государства
        /// </summary>
        /// <param name="pState"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Удаляет все отображаемые на карте маршруты
        /// </summary>
        public void ClearPath()
        {
            m_cPaths.Clear();
            Draw();
        }

        /// <summary>
        /// Добавляет новый маршрут для отображения на карте
        /// </summary>
        /// <param name="aPath">маршрут</param>
        /// <param name="pColor">цвет для отображения маршрута</param>
        public void AddPath(TransportationNode[] aPath, Color pColor)
        {
            Pen pPen = new Pen(pColor, 5);
            pPen.DashPattern = new float[] { 2, 3 };
            m_cPaths[aPath] = pPen;
            Draw();
        }
    }
}