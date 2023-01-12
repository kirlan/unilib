using System;
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
using Socium.Languages;
using Socium.Nations;
using Socium.Settlements;
using GeneLab.Genetix;
using Socium.Psychology;

namespace MapDrawEngine
{
    /// <summary>
    /// движок для рисования 2D карт средствами GUI.
    /// </summary>
    public partial class MapDraw : UserControl
    {
        #region Предопределённые перья для рисования
        internal static readonly Pen s_pDarkGrey2Pen = new Pen(Color.DarkGray, 2);
        internal static readonly Pen s_pDarkGrey3Pen = new Pen(Color.DarkGray, 3);
        internal static readonly Pen s_pAqua1Pen = new Pen(Color.Aqua, 1);
        internal static readonly Pen s_pAqua2Pen = new Pen(Color.Aqua, 2);
        internal static readonly Pen s_pRed2Pen = new Pen(Color.Red, 2);
        internal static readonly Pen s_pRed3Pen = new Pen(Color.Red, 3);
        internal static readonly Pen s_pBlack1Pen = new Pen(Color.Black, 1);
        internal static readonly Pen s_pBlack2Pen = new Pen(Color.Black, 2);
        internal static readonly Pen s_pBlack3Pen = new Pen(Color.Black, 3);
        internal static readonly Pen s_pWhite2Pen = new Pen(Color.White, 2);

        internal static readonly Pen s_pAqua1DotPen = new Pen(Color.Aqua, 1);
        internal static readonly Pen s_pBlack1DotPen = new Pen(Color.Black, 1);
        internal static readonly Pen s_pBlack3DotPen = new Pen(Color.Black, 3);
        #endregion

        /// <summary>
        /// Информация о том, какое государство выбрано на карте
        /// </summary>
        public class SelectedStateChangedEventArgs : EventArgs
        {
            /// <summary>
            /// Выбранное государство
            /// </summary>
            public State State { get; }

            /// <summary>
            /// X-координата центра государства в экранных координатах
            /// </summary>
            public int X { get; }
            /// <summary>
            /// Y-координата центра государства в экранных координатах
            /// </summary>
            public int Y { get; }

            public SelectedStateChangedEventArgs(State pState, int iX, int iY)
            {
                State = pState;

                X = iX;
                Y = iY;
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
        private readonly MapQuadrant[,] m_aQuadrants;

        /// <summary>
        /// цвета разных зон влажности
        /// </summary>
        private readonly Brush[] m_aHumidity;

        /// <summary>
        /// цвета разных уровней технического развития
        /// </summary>
        private readonly Brush[,] m_aTechLevel = new Brush[9, 9];

        /// <summary>
        /// цвета разных уровней магического развития
        /// </summary>
        private readonly Dictionary<int, Dictionary<Customs.Magic, Brush>> m_cPsiLevel = new Dictionary<int, Dictionary<Customs.Magic, Brush>>();

        /// <summary>
        /// цвета разных уровней технического развития
        /// </summary>
        private readonly Brush[,] m_aCivLevel = new Brush[9, 9];

        /// <summary>
        /// размерность матрицы квадрантов
        /// </summary>
        private const int QUADRANTS_COUNT = 8;

        private const int ROUGH_SCALE = 100;

        /// <summary>
        /// Контуры конкретных географических регионов - для определения, над каким из них находится мышь.
        /// В масштабе 1:ROUGH_SCALE для ускорения.
        /// </summary>
        private readonly Dictionary<Socium.Region, GraphicsPath> m_cRegionsBorders = new Dictionary<Socium.Region, GraphicsPath>();

        /// <summary>
        /// Контуры конкретных провинций - для определения, над какой из них находится мышь.
        /// В масштабе 1:ROUGH_SCALE для ускорения.
        /// </summary>
        private readonly Dictionary<Province, GraphicsPath> m_cProvinceBorders = new Dictionary<Province, GraphicsPath>();

        /// <summary>
        /// Контуры конкретных тектонических плит - для определения, над какой из них находится мышь.
        /// В масштабе 1:ROUGH_SCALE для ускорения.
        /// </summary>
        private readonly Dictionary<LandMass, GraphicsPath> m_cLandMassBorders = new Dictionary<LandMass, GraphicsPath>();

        /// <summary>
        /// Контуры конкретных земель - для определения, над какой из них находится мышь.
        /// В масштабе 1:ROUGH_SCALE для ускорения.
        /// </summary>
        private readonly Dictionary<Land, GraphicsPath> m_cLandBorders = new Dictionary<Land, GraphicsPath>();

        /// <summary>
        /// Контуры конкретных локаций - для определения, над какой из них находится мышь.
        /// В масштабе 1:ROUGH_SCALE для ускорения.
        /// </summary>
        private readonly Dictionary<Location, GraphicsPath> m_cLocationBorders = new Dictionary<Location, GraphicsPath>();

        /// <summary>
        /// Контуры конкретных государств - для определения, над каким из них находится мышь.
        /// В масштабе 1:ROUGH_SCALE для ускорения.
        /// </summary>
        private readonly Dictionary<State, GraphicsPath> m_cStateBorders = new Dictionary<State, GraphicsPath>();

        /// <summary>
        /// мир, карту которого мы рисуем
        /// </summary>
        internal World m_pWorld = null;

        /// <summary>
        /// Режим отрисовки карты - физическая карта, карта влажности, этническая карта...
        /// </summary>
        private MapMode m_eMode = MapMode.Areas;

        /// <summary>
        /// Режим отрисовки карты - физическая карта, карта влажности, этническая карта...
        /// </summary>
        public MapMode Mode
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
        /// Отрисовывать ли на карте границы регионов?
        /// </summary>
        private bool m_bShowRegions = false;

        /// <summary>
        /// Отрисовывать ли на карте границы регионов?
        /// </summary>
        public bool ShowRegions
        {
            get { return m_bShowRegions; }
            set
            {
                if (m_bShowRegions != value)
                {
                    m_bShowRegions = value;
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
        /// ширина "рамки" для скрытия неровного края карты - уже отмасштабированная
        /// </summary>
        private float m_fFrameWidth = 0f;

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
        private float m_fOneQuadWidth;
        /// <summary>
        /// высота одного квадранта в экранных координатах
        /// </summary>
        private float m_fOneQuadHeight;

        /// <summary>
        /// ширина отображаемого участка карты в квадрантах
        /// </summary>
        private int m_iQuadsWidth;
        /// <summary>
        /// высота отображаемого участка карты в квадрантах
        /// </summary>
        private int m_iQuadsHeight;

        /// <summary>
        /// ширина всей карты мира в экранных координатах
        /// </summary>
        private int m_iScaledMapWidth;
        /// <summary>
        /// высота всей карты мира в экранных координатах
        /// </summary>
        private int m_iScaledMapHeight;

        /// <summary>
        /// связанная миникарта
        /// </summary>
        private MiniMapDraw m_pMiniMap = null;

        /// <summary>
        /// Континент, над которым находится указатель мыши.
        /// </summary>
        private Continent m_pFocusedContinent = null;

        private LandMass m_pFocusedLandMass = null;

        /// <summary>
        /// Континент, над которым находится указатель мыши.
        /// </summary>
        public Continent ContinentInFocus
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
        private Land m_pFocusedLand = null;

        /// <summary>
        /// Земля, над которой находится указатель мыши.
        /// </summary>
        public Land LandInFocus
        {
            get { return m_pFocusedLand; }
        }

        /// <summary>
        /// Локация, над которой находится указатель мыши.
        /// </summary>
        private Location m_pFocusedLocation = null;

        /// <summary>
        /// Локация, над которой находится указатель мыши.
        /// </summary>
        public Location LocationInFocus
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
        private readonly Dictionary<TransportationNode[], Pen> m_cPaths = new Dictionary<TransportationNode[], Pen>();

        #endregion

        public MapDraw()
        {
            InitializeComponent();

            s_pBlack1DotPen.DashPattern = new float[] { 1, 2 };
            s_pAqua1DotPen.DashPattern = new float[] { 1, 2 };
            s_pBlack3DotPen.DashPattern = new float[] { 2, 4 };

            m_aQuadrants = new MapQuadrant[QUADRANTS_COUNT, QUADRANTS_COUNT];
            for (int i = 0; i < QUADRANTS_COUNT; i++)
            {
                for (int j = 0; j < QUADRANTS_COUNT; j++)
                {
                    m_aQuadrants[i, j] = new MapQuadrant();
                }
            }

            List<Brush> cHumidity = new List<Brush>();
            for (int i = 0; i <= 100; i++)
                cHumidity.Add(GetHumidityColor(i));
            m_aHumidity = cHumidity.ToArray();

            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    m_aTechLevel[i,j] = GetTechLevelColor(i, j);
                }
            }

            for (int i = 0; i <= 8; i++)
            {
                m_cPsiLevel[i] = new Dictionary<Customs.Magic, Brush>();
                foreach (Customs.Magic ePrevalence in Enum.GetValues(typeof(Customs.Magic)))
                    m_cPsiLevel[i][ePrevalence] = GetPsiLevelColor(i, ePrevalence);
            }

            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 4; j++)
                    m_aCivLevel[i, j] = GetCultureColor(i, j);
            }
        }

        #region Функции для работы с цветами

        /// <summary>
        /// Вычисляет цвет для отображения заданного уровня влажности
        /// </summary>
        /// <param name="iHumidity">заданный уровень влажности (0-100)</param>
        /// <returns>цвет</returns>
        private Brush GetHumidityColor(int iHumidity)
        {
            KColor color = new KColor();
            color.RGB = Color.LightBlue;
            color.Lightness = 1.0 - (double)iHumidity / 200;
            return new SolidBrush(color.RGB);
        }
        /// <summary>
        /// Вычисляет цвет для отображения заданного уровня технического развития
        /// </summary>
        /// <param name="iBaseTechLevel">уровень индустриальной базы (0-8)</param>
        /// <param name="iUsedTechLevel">уровень доступной в стране техники (0-8)</param>
        /// <returns>цвет</returns>
        private Brush GetTechLevelColor(int iBaseTechLevel, int iUsedTechLevel)
        {
            KColor background = new KColor();
            background.RGB = Color.ForestGreen;
            background.Lightness = 1.0 - (double)iBaseTechLevel / 10;

            KColor foreground = new KColor();
            foreground.RGB = Color.ForestGreen;
            foreground.Lightness = 1.0 - (double)iUsedTechLevel / 10;
            return new HatchBrush(HatchStyle.LargeConfetti, foreground.RGB, background.RGB);
        }
        /// <summary>
        /// Вычисляет цвет для отображения заданного уровня магии
        /// </summary>
        /// <param name="iMaxPsiLevel">максимальный доступный уровень магии (0-8)</param>
        /// <param name="ePrevalence">отношение местных к магии</param>
        /// <returns>цвет</returns>
        /// <exception cref="ArgumentException"></exception>
        private Brush GetPsiLevelColor(int iMaxPsiLevel, Customs.Magic ePrevalence)
        {
            KColor background = new KColor();
            background.RGB = Color.Orchid;
            background.Lightness = 0.9 - (double)iMaxPsiLevel / 10;

            KColor foreground = new KColor();
            foreground.RGB = Color.Gray;

            switch (ePrevalence)
            {
                case Customs.Magic.Magic_Praised:
                    return new SolidBrush(background.RGB);
                case Customs.Magic.Magic_Allowed:
                    return new HatchBrush(HatchStyle.DottedDiamond, Color.Gray, background.RGB);
                case Customs.Magic.Magic_Feared:
                    return new HatchBrush(HatchStyle.DiagonalCross, Color.Red, background.RGB);
                default:
                    throw new ArgumentException();
            }
        }
        /// <summary>
        /// Вычисляет цвет для отображения заданного уровня развития цивилизации в общем
        /// </summary>
        /// <param name="iCultureLevel">уровень цивилизованности</param>
        /// <param name="iControl">уровень правительственного контроля</param>
        /// <returns>цвет</returns>
        private Brush GetCultureColor(int iCultureLevel, int iControl)
        {
            KColor background = new KColor { RGB = Color.Red };
            background.Hue += iCultureLevel * 16;

            KColor foreground = new KColor { RGB = Color.Gray };

            // 0 - На преступников и диссидентов власти никакого внимания не обращают, спасение утопающих - дело рук самих утопающих.
            // 1 - Власти занимаются только самыми вопиющими преступлениями.
            // 2 - Есть законы, их надо соблюдать, кто не соблюдает - тот преступник, а вор должен сидеть в тюрьме.
            // 3 - Законы крайне строги, широко используется смертная казнь.
            // 4 - Все граждане, кроме правящей верхушки, попадают по презумпцию виновности.
            switch (iControl)
            {
                case 0:
                    return new SolidBrush(background.RGB);
                case 1:
                    return new HatchBrush(HatchStyle.Percent05, foreground.RGB, background.RGB);
                case 2:
                    return new HatchBrush(HatchStyle.DottedDiamond, foreground.RGB, background.RGB);
                case 3:
                    return new HatchBrush(HatchStyle.OutlinedDiamond, foreground.RGB, background.RGB);
                case 4:
                    return new HatchBrush(HatchStyle.DiagonalCross, foreground.RGB, background.RGB);
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Привязка цветов для конкретных рас
        /// </summary>
        private Dictionary<Nation, Brush> m_cNationColorsID = new Dictionary<Nation, Brush>();
        private Dictionary<Nation, Brush> m_cAncientNationColorsID = new Dictionary<Nation, Brush>();
        private Dictionary<Nation, Brush> m_cHegemonNationColorsID = new Dictionary<Nation, Brush>();

        #endregion Функции для работы с цветами

        /// <summary>
        /// Привязать карту к миру.
        /// Строим контуры всего, что придётся рисовать в ОРИГИНАЛЬНЫХ координатах
        /// и раскидываем их по квадрантам
        /// </summary>
        /// <param name="pWorld">мир</param>
        public void Assign(World pWorld)
        {
            DateTime pTime1 = DateTime.Now;

            m_pFocusedContinent = null;
            m_pFocusedLand = null;
            m_pFocusedLandMass = null;
            m_pFocusedLocation = null;
            m_pFocusedProvince = null;
            m_pFocusedState = null;

            //расчистим рабочее место
            m_cRegionsBorders.Clear();
            m_cProvinceBorders.Clear();
            m_cLandMassBorders.Clear();
            m_cLandBorders.Clear();
            m_cLocationBorders.Clear();
            m_cStateBorders.Clear();

            foreach (MapQuadrant pQuad in m_aQuadrants)
                pQuad.Clear();

            m_pWorld = pWorld;

            ClearPath();

            m_cNationColorsID.Clear();

            Dictionary<Language, int> cLanguages = new Dictionary<Language, int>();
            Dictionary<Language, int> cLanguageCounter = new Dictionary<Language, int>();
            Dictionary<Language, int> cLanguageIndex = new Dictionary<Language, int>();
            int iCount = 0;
            foreach (Language pLanguage in m_pWorld.LocalNations.Select(x => x.Race.Language))
            {
                if (!cLanguages.TryGetValue(pLanguage, out int ik))
                {
                    cLanguageCounter[pLanguage] = 1;
                    cLanguageIndex[pLanguage] = iCount++;
                }
                cLanguages[pLanguage] = ik + 1;
            }

            foreach (Nation pNation in m_pWorld.LocalNations)
            {
                KColor color = new KColor { Hue = 360.0 * cLanguageIndex[pNation.Race.Language] / cLanguages.Count };
                float fK = ((float)cLanguageCounter[pNation.Race.Language]) / cLanguages[pNation.Race.Language];
                color.Lightness = 0.2 + 0.7 * fK;
                color.Saturation = 0.2 + 0.7 * fK;
                m_cNationColorsID[pNation] = new SolidBrush(color.RGB);
                m_cAncientNationColorsID[pNation] = new HatchBrush(HatchStyle.DottedDiamond, Color.Black, color.RGB);
                m_cHegemonNationColorsID[pNation] = new HatchBrush(HatchStyle.LargeConfetti, Color.Black, color.RGB);
                cLanguageCounter[pNation.Race.Language]++;
            }

            //foreach (Race pRace in m_pWorld.m_aLocalRaces)
            //{
            //    int iIndex;
            //    do
            //    {
            //        iIndex = Rnd.Get(m_aRaceColorsTemplate.Length);
            //    }
            //    while (cUsedColors.Contains(iIndex));

            //    cUsedColors.Add(iIndex);
            //    m_cRaceColorsID[pRace] = new SolidBrush(m_aRaceColorsTemplate[iIndex]);
            //    m_cAncientRaceColorsID[pRace] = new HatchBrush(HatchStyle.DottedDiamond, Color.Black, m_aRaceColorsTemplate[iIndex]);
            //    m_cHegemonRaceColorsID[pRace] = new HatchBrush(HatchStyle.LargeConfetti, Color.Black, m_aRaceColorsTemplate[iIndex]);
            //}

            PointF[][] aPoints;
            GraphicsPath pPath;
            MapQuadrant[] aQuads;

            //вычислим контуры тектонических плит
            foreach (LandMass pLandMass in m_pWorld.LandMasses)
            {
                aPoints = BuildPath(pLandMass.FirstLines, true, out aQuads);
                pPath = new GraphicsPath();
                //пробежимся по всем простым контурам в вычисленном сложном контуре
                foreach (var aPts in aPoints)
                {
                    //добавим вычисленный простой контур к общему рисунку контура тектонической плиты
                    pPath.AddPolygon(aPts);
                    //добавим вычисленный простой контур к рисунку контура тектонической плиты в тех квадрантах, через которые он проходит.
                    foreach (var pQuadLayers in aQuads.Select(x => x.Layers))
                    {
                        pQuadLayers[MapLayer.LandMasses].StartFigure();
                        pQuadLayers[MapLayer.LandMasses].AddLines(aPts);
                    }
                }
                //запомним общий рисунок контура тектонической плиты - чтобы потом определять вхождение указателя мыши в него.
                m_cLandMassBorders[pLandMass] = pPath;
                //вычислим контуры земель
                foreach (Land pLand in pLandMass.Contents)
                {
                    aPoints = BuildPath(pLand.FirstLines, true, out aQuads);
                    pPath = new GraphicsPath();
                    foreach (var aPts in aPoints)
                    {
                        pPath.AddPolygon(aPts);

                        //определим, каким цветом эта земля должна закрашиваться на карте влажностей
                        Brush pBrush = pLand.IsWater ? pLand.LandType.Get<LandTypeDrawInfo>().m_pBrush : m_aHumidity[pLand.Humidity];

                        foreach (MapQuadrant pQuad in aQuads)
                        {
                            pQuad.Layers[MapLayer.Lands].StartFigure();
                            pQuad.Layers[MapLayer.Lands].AddLines(aPts);

                            if (!pQuad.Modes[MapMode.Humidity].ContainsKey(pBrush))
                                pQuad.Modes[MapMode.Humidity][pBrush] = new GraphicsPath();
                            pQuad.Modes[MapMode.Humidity][pBrush].AddPolygon(aPts);

                            if (pLand.LandType == LandTypes.Coastral)
                            {
                                if (!pQuad.Modes[MapMode.Areas].ContainsKey(pLand.LandType.Get<LandTypeDrawInfo>().m_pBrush))
                                    pQuad.Modes[MapMode.Areas][pLand.LandType.Get<LandTypeDrawInfo>().m_pBrush] = new GraphicsPath();
                                pQuad.Modes[MapMode.Areas][pLand.LandType.Get<LandTypeDrawInfo>().m_pBrush].AddPolygon(aPts);
                            }
                        }
                    }
                    m_cLandBorders[pLand] = pPath;

                    //вычислим контуры локаций
                    foreach (Location pLoc in pLand.Contents)
                    {
                        aPoints = BuildPath(pLoc.FirstLine, true, out aQuads);
                        pPath = new GraphicsPath();
                        foreach (var aPts in aPoints)
                        {
                            pPath.AddPolygon(aPts);

                            //определим, каким цветом эта земля должна закрашиваться на карте высот
                            Brush pBrush = GetElevationColor(pLoc.H, m_pWorld.MaxDepth, m_pWorld.MaxHeight, pLand.LandType);

                            foreach (MapQuadrant pQuad in aQuads)
                            {
                                pQuad.Layers[MapLayer.Locations].StartFigure();
                                pQuad.Layers[MapLayer.Locations].AddLines(aPts);

                                if (!pQuad.Modes[MapMode.Elevation].ContainsKey(pBrush))
                                    pQuad.Modes[MapMode.Elevation][pBrush] = new GraphicsPath();
                                pQuad.Modes[MapMode.Elevation][pBrush].AddPolygon(aPts);
                            }
                        }
                        m_cLocationBorders[pLoc] = pPath;
                        //добавим информацию о метке на карте
                        AddLocationSign(pLoc);
                    }
                }
            }

            //вычислим контуры континентов
            foreach (Continent pContinent in m_pWorld.Contents)
            {
                aPoints = BuildPath(pContinent.FirstLines, true, out aQuads);
                foreach (var aPts in aPoints)
                {
                    foreach (var pQuadLayers in aQuads.Select(x => x.Layers))
                    {
                        pQuadLayers[MapLayer.Continents].StartFigure();
                        pQuadLayers[MapLayer.Continents].AddLines(aPts);
                    }
                }

                //вычислим контуры государств
                foreach (State pState in pContinent.As<ContinentX>().Contents)
                {
                    aPoints = BuildPath(pState.FirstLines, true, out aQuads);
                    pPath = new GraphicsPath();
                    foreach (var aPts in aPoints)
                    {
                        pPath.AddPolygon(aPts);

                        //определим, каким цветом эта земля должна закрашиваться на карте технологий
                        int iImported = pState.Society.GetImportedTech();
                        Brush pTechBrush = m_aTechLevel[pState.Society.GetEffectiveTech(), iImported == -1 ? pState.Society.GetEffectiveTech() : iImported];
                        Brush pCivBrush = m_aCivLevel[pState.Society.DominantCulture.ProgressLevel, pState.Society.Control];

                        foreach (MapQuadrant pQuad in aQuads)
                        {
                            pQuad.Layers[MapLayer.States].StartFigure();
                            pQuad.Layers[MapLayer.States].AddLines(aPts);

                            if (!pQuad.Modes[MapMode.TechLevel].ContainsKey(pTechBrush))
                                pQuad.Modes[MapMode.TechLevel][pTechBrush] = new GraphicsPath();
                            pQuad.Modes[MapMode.TechLevel][pTechBrush].AddPolygon(aPts);

                            if (!pQuad.Modes[MapMode.Infrastructure].ContainsKey(pCivBrush))
                                pQuad.Modes[MapMode.Infrastructure][pCivBrush] = new GraphicsPath();
                            pQuad.Modes[MapMode.Infrastructure][pCivBrush].AddPolygon(aPts);
                        }
                    }
                    m_cStateBorders[pState] = pPath;
                }

                //вычислим контуры географических регионов
                foreach (Socium.Region pRegion in pContinent.As<ContinentX>().Regions)
                {
                    aPoints = BuildPath(pRegion.FirstLines, true, out aQuads);
                    pPath = new GraphicsPath();
                    foreach (var aPts in aPoints)
                    {
                        pPath.AddPolygon(aPts);

                        foreach (MapQuadrant pQuad in aQuads)
                        {
                            pQuad.Layers[MapLayer.Regions].StartFigure();
                            pQuad.Layers[MapLayer.Regions].AddLines(aPts);

                            //в качестве идентификатора типа региона используем цвет, которым этот регион должен рисоваться
                            if (!pQuad.Modes[MapMode.Areas].ContainsKey(pRegion.Type.Get<LandTypeDrawInfo>().m_pBrush))
                            {
                                pQuad.Modes[MapMode.Areas][pRegion.Type.Get<LandTypeDrawInfo>().m_pBrush] = new GraphicsPath();
                            }
                            pQuad.Modes[MapMode.Areas][pRegion.Type.Get<LandTypeDrawInfo>().m_pBrush].AddPolygon(aPts);

                            //если регион обитаем
                            if (pRegion.Natives != null)
                            {
                                //сохраним информацию о контуре региона и для этнографической карты
                                Brush pBrush = m_cNationColorsID[pRegion.Natives];
                                if (pRegion.Natives.IsAncient)
                                    pBrush = m_cAncientNationColorsID[pRegion.Natives];
                                if (pRegion.Natives.IsHegemon)
                                    pBrush = m_cHegemonNationColorsID[pRegion.Natives];

                                if (!pQuad.Modes[MapMode.Natives].ContainsKey(pBrush))
                                {
                                    pQuad.Modes[MapMode.Natives][pBrush] = new GraphicsPath();
                                }
                                pQuad.Modes[MapMode.Natives][pBrush].AddPolygon(aPts);
                            }
                        }
                    }
                    m_cRegionsBorders[pRegion] = pPath;
                }
            }

            //вычислим контуры провинций
            foreach (Province pProvince in m_pWorld.Provinces)
            {
                aPoints = BuildPath(pProvince.FirstLines, true, out aQuads);
                pPath = new GraphicsPath();
                foreach (var aPts in aPoints)
                {
                    pPath.AddPolygon(aPts);

                    //сохраним информацию о контуре провинции для этнографической карты
                    Brush pBrush = m_cNationColorsID[pProvince.LocalSociety.TitularNation];
                    if (pProvince.LocalSociety.TitularNation.IsAncient)
                        pBrush = m_cAncientNationColorsID[pProvince.LocalSociety.TitularNation];
                    if (pProvince.LocalSociety.TitularNation.IsHegemon)
                        pBrush = m_cHegemonNationColorsID[pProvince.LocalSociety.TitularNation];

                    Brush pPsiBrush = m_cPsiLevel[pProvince.LocalSociety.MagicLimit][pProvince.LocalSociety.DominantCulture.Customs.ValueOf<Customs.Magic>()];

                    foreach (MapQuadrant pQuad in aQuads)
                    {
                        if (!pProvince.Center.IsWater)
                        {
                            pQuad.Layers[MapLayer.Provincies].StartFigure();
                            pQuad.Layers[MapLayer.Provincies].AddLines(aPts);
                        }

                        if (!pQuad.Modes[MapMode.Nations].ContainsKey(pBrush))
                            pQuad.Modes[MapMode.Nations][pBrush] = new GraphicsPath();
                        pQuad.Modes[MapMode.Nations][pBrush].AddPolygon(aPts);

                        if (!pQuad.Modes[MapMode.PsiLevel].ContainsKey(pPsiBrush))
                            pQuad.Modes[MapMode.PsiLevel][pPsiBrush] = new GraphicsPath();
                        pQuad.Modes[MapMode.PsiLevel][pPsiBrush].AddPolygon(aPts);
                    }
                }
                m_cProvinceBorders[pProvince] = pPath;
            }

            //вычислим дорожную сетку
            foreach (TransportationLinkBase pRoad in m_pWorld.TransportGrid)
            {
                AddRoad(pRoad);
            }

            //нормализуем все квадранты таким образом, чтобы внутри каждого квадранта координаты считались от
            //верхнего левого угла самого квадранта, а не от центра оригинальной системы координат
            for (int i = 0; i < QUADRANTS_COUNT; i++)
            {
                for (int j = 0; j < QUADRANTS_COUNT; j++)
                {
                    float fQuadX = (float)m_pWorld.LocationsGrid.RX * i * 2 / QUADRANTS_COUNT;
                    float fQuadY = (float)m_pWorld.LocationsGrid.RY * j * 2 / QUADRANTS_COUNT;
                    m_aQuadrants[i, j].Normalize(fQuadX, fQuadY);
                }
            }

            //смасштабируем используемые для определения положения курсора контуры в ROUGH_SCALE раз - 
            //для ускорения выполнения функции GraphicsPath::IsVisible(x,y)
            Matrix pScaleMatrix = new Matrix();
            pScaleMatrix.Scale(1f / ROUGH_SCALE, 1f / ROUGH_SCALE);

            foreach (var pPair in m_cRegionsBorders)
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

            SetPan(0, 0);
        }

        private Brush GetElevationColor(float fHeight, float fMinHeight, float fMaxHeight, LandTypeInfo pType)
        {
            if (float.IsNaN(fHeight))
                return new SolidBrush(Color.Black);

            if (fMinHeight == 0)
                fMinHeight = fHeight;
            if (fMaxHeight == 0)
                fMaxHeight = fHeight;

            KColor color = new KColor();
            color.RGB = Color.Cyan;

            if (fHeight < 0 || pType.Environment.HasFlag(LandscapeGeneration.Environment.Liquid))
            {
                color.RGB = Color.Green;
                color.Hue = Math.Min(360, 200 + 40 * fHeight / fMinHeight);
            }
            else if (fHeight > 0)
            {
                color.RGB = Color.Goldenrod;
                color.Hue = 100 * (fMaxHeight - fHeight) * (fMaxHeight - fHeight) / (fMaxHeight * fMaxHeight);
            }

            return new SolidBrush(color.RGB);
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
        private PointF[][] BuildPath(List<VoronoiEdge> cFirstLines, bool bMirror, out MapQuadrant[] aQuadrants)
        {
            bool[,] aQuadsAll = new bool[QUADRANTS_COUNT, QUADRANTS_COUNT];
            List<PointF[]> cPath = new List<PointF[]>();

            //пробежимся по всем затравкам
            foreach (VoronoiEdge pFirstLine in cFirstLines)
            {
                //получаем простой одиночный контур
                cPath.Add(BuildBorder(pFirstLine, 0, out bool bCross, out bool[,] aQuads));

                //определяем, через какие квадранты он проходит
                for (int i = 0; i < QUADRANTS_COUNT; i++)
                {
                    for (int j = 0; j < QUADRANTS_COUNT; j++)
                    {
                        if (aQuads[i, j])
                            aQuadsAll[i, j] = true;
                    }
                }

                //если карта зациклена по горизонтали, нужно строить отражения и 
                //контур пересекает нулевой меридиан, то строим отражение!
                if (m_pWorld.LocationsGrid.CycleShift != 0 && bMirror && bCross)
                {
                    //определяем, на западе или на востоке будем строить отражение
                    if (pFirstLine.Point1.X > 0)
                        cPath.Add(BuildBorder(pFirstLine, -m_pWorld.LocationsGrid.RX * 2, out bCross, out aQuads));
                    else
                        cPath.Add(BuildBorder(pFirstLine, m_pWorld.LocationsGrid.RX * 2, out bCross, out aQuads));

                    //определяем, через какие квадранты оно проходит
                    for (int i = 0; i < QUADRANTS_COUNT; i++)
                    {
                        for (int j = 0; j < QUADRANTS_COUNT; j++)
                        {
                            if (aQuads[i, j])
                                aQuadsAll[i, j] = true;
                        }
                    }
                }
            }

            //переносим все квадранты, через которые проходит контур и его отражение, в линейный массив
            List<MapQuadrant> cQuadrants = new List<MapQuadrant>();
            for (int i = 0; i < QUADRANTS_COUNT; i++)
            {
                for (int j = 0; j < QUADRANTS_COUNT; j++)
                {
                    if (aQuadsAll[i, j])
                        cQuadrants.Add(m_aQuadrants[i, j]);
                }
            }
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
        private PointF[][] BuildPath(VoronoiEdge pFirstLine, bool bMirror, out MapQuadrant[] aQuadrants)
        {
            bool[,] aQuadsAll = new bool[QUADRANTS_COUNT, QUADRANTS_COUNT];
            List<PointF[]> cPath = new List<PointF[]>();

            //получаем простой одиночный контур
            cPath.Add(BuildBorder(pFirstLine, 0, out bool bCross, out bool[,] aQuads));

            //определяем, через какие квадранты он проходит
            for (int i = 0; i < QUADRANTS_COUNT; i++)
            {
                for (int j = 0; j < QUADRANTS_COUNT; j++)
                {
                    if (aQuads[i, j])
                        aQuadsAll[i, j] = true;
                }
            }

            //если карта зациклена по горизонтали, нужно строить отражения и 
            //контур пересекает нулевой меридиан, то строим отражение!
            if (m_pWorld.LocationsGrid.CycleShift != 0 && bMirror && bCross)
            {
                //определяем, на западе или на востоке будем строить отражение
                if (pFirstLine.Point1.X > 0)
                    cPath.Add(BuildBorder(pFirstLine, -m_pWorld.LocationsGrid.RX * 2, out bCross, out aQuads));
                else
                    cPath.Add(BuildBorder(pFirstLine, m_pWorld.LocationsGrid.RX * 2, out bCross, out aQuads));

                //определяем, через какие квадранты оно проходит
                for (int i = 0; i < QUADRANTS_COUNT; i++)
                {
                    for (int j = 0; j < QUADRANTS_COUNT; j++)
                    {
                        if (aQuads[i, j])
                            aQuadsAll[i, j] = true;
                    }
                }
            }

            //переносим все квадранты, через которые проходит контур и его отражение, в линейный массив
            List<MapQuadrant> cQuadrants = new List<MapQuadrant>();
            for (int i = 0; i < QUADRANTS_COUNT; i++)
            {
                for (int j = 0; j < QUADRANTS_COUNT; j++)
                {
                    if (aQuadsAll[i, j])
                        cQuadrants.Add(m_aQuadrants[i, j]);
                }
            }
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
        private PointF[] BuildBorder(VoronoiEdge pFirstLine, float fShift, out bool bCross, out bool[,] aQuadrants)
        {
            bCross = false;

            aQuadrants = new bool[QUADRANTS_COUNT, QUADRANTS_COUNT];

            List<PointF> cBorder = new List<PointF>();
            VoronoiEdge pLine = pFirstLine;
            cBorder.Add(ShiftPoint(pLine.Point1, fShift));
            float fLastPointX = pLine.Point1.X + fShift;
            //последовательно перебирает все связанные линии, пока круг не замкнётся.
            do
            {
                //в каком квадранте лежит первая точка линии
                int iQuad1X = (int)(QUADRANTS_COUNT * (pLine.Point1.X + m_pWorld.LocationsGrid.RX) / (2*m_pWorld.LocationsGrid.RX));
                int iQuad1Y = (int)(QUADRANTS_COUNT * (pLine.Point1.Y + m_pWorld.LocationsGrid.RY) / (2*m_pWorld.LocationsGrid.RY));

                if (iQuad1X >= 0 && iQuad1X < QUADRANTS_COUNT && iQuad1Y >= 0 && iQuad1Y < QUADRANTS_COUNT)
                    aQuadrants[iQuad1X, iQuad1Y] = true;

                //в каком квадранте лежит вторая точка линии
                int iQuad2X = (int)(QUADRANTS_COUNT * (pLine.Point2.X + m_pWorld.LocationsGrid.RX) / (2*m_pWorld.LocationsGrid.RX));
                int iQuad2Y = (int)(QUADRANTS_COUNT * (pLine.Point2.Y + m_pWorld.LocationsGrid.RY) / (2*m_pWorld.LocationsGrid.RY));

                if (iQuad2X >= 0 && iQuad2X < QUADRANTS_COUNT && iQuad2Y >= 0 && iQuad2Y < QUADRANTS_COUNT)
                    aQuadrants[iQuad2X, iQuad2Y] = true;

                //пересекает-ли линия нулевой меридиан?
                float fDX = fShift;
                if (Math.Abs(fLastPointX - pLine.Point2.X - fShift) > m_pWorld.LocationsGrid.RX)
                {
                    //определимся, где у нас была предыдущая часть контура - на западе или на востоке?
                    //в зависимости от этого вычислим смещение для оставшейся части контура, чтобы 
                    //не было разрыва
                    fDX += fLastPointX < fShift ? -m_pWorld.LocationsGrid.RX * 2 : m_pWorld.LocationsGrid.RX * 2;
                    bCross = true;
                }

                if (pLine.Point2.X > m_pWorld.LocationsGrid.RX ||
                    pLine.Point2.X < -m_pWorld.LocationsGrid.RX)
                {
                    bCross = true;
                }

                cBorder.Add(ShiftPoint(pLine.Point2, fDX));

                if (cBorder.Count > 1000)
                    break;

                //X-координата последней добавленной точки с учётом вычисленного смещения
                fLastPointX = pLine.Point2.X + fDX;

                pLine = pLine.Next;
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
        private PointF ShiftPoint(VoronoiVertex pPoint, float fDX)
        {
            return new PointF(m_pWorld.LocationsGrid.RX + pPoint.X + fDX, m_pWorld.LocationsGrid.RY + pPoint.Y);
        }

        /// <summary>
        /// добавляет в соответствующий квадрант информацию о метке на карте
        /// </summary>
        /// <param name="pLoc">локация, содержащая метку</param>
        private void AddLocationSign(Location pLoc)
        {
            float fPointX = m_pWorld.LocationsGrid.RX + pLoc.X;
            float fPointY = m_pWorld.LocationsGrid.RY + pLoc.Y;

            int iQuadX = (int)(QUADRANTS_COUNT * (pLoc.X + m_pWorld.LocationsGrid.RX) / (2*m_pWorld.LocationsGrid.RX));
            int iQuadY = (int)(QUADRANTS_COUNT * (pLoc.Y + m_pWorld.LocationsGrid.RY) / (2*m_pWorld.LocationsGrid.RY));

            if (iQuadX < 0 || iQuadX >= QUADRANTS_COUNT || iQuadY < 0 || iQuadY >= QUADRANTS_COUNT)
                return;

            switch (pLoc.Landmark)
            {
                case LandmarkType.Peak:
                    m_aQuadrants[iQuadX, iQuadY].Landmarks.Add(new SignPeak(fPointX, fPointY, m_pWorld.LocationsGrid.RX, ""));
                    break;
                case LandmarkType.Volcano:
                    m_aQuadrants[iQuadX, iQuadY].Landmarks.Add(new SignVolkano(fPointX, fPointY, m_pWorld.LocationsGrid.RX, ""));
                    break;
            }

            LocationX pLocX = pLoc.As<LocationX>();

            if (pLocX.Settlement != null)
            {
                if (pLocX.Settlement.RuinsAge > 0)
                {
                    m_aQuadrants[iQuadX, iQuadY].Landmarks.Add(new SignRuin(fPointX, fPointY, m_pWorld.LocationsGrid.RX, ""));
                }
                else
                {
                    switch (pLocX.Settlement.Profile.Size)
                    {
                        case SettlementSize.Capital:
                            m_aQuadrants[iQuadX, iQuadY].Landmarks.Add(new SignCapital(fPointX, fPointY, m_pWorld.LocationsGrid.RX, pLocX.Settlement.Name));
                            break;
                        case SettlementSize.City:
                            m_aQuadrants[iQuadX, iQuadY].Landmarks.Add(new SignCity(fPointX, fPointY, m_pWorld.LocationsGrid.RX, pLocX.Settlement.Name));
                            break;
                        case SettlementSize.Town:
                            m_aQuadrants[iQuadX, iQuadY].Landmarks.Add(new SignTown(fPointX, fPointY, m_pWorld.LocationsGrid.RX, pLocX.Settlement.Name));
                            break;
                        case SettlementSize.Village:
                            m_aQuadrants[iQuadX, iQuadY].Landmarks.Add(new SignVillage(fPointX, fPointY, m_pWorld.LocationsGrid.RX, pLocX.Settlement.Name, pLoc.GetOwner().LandType.Get<LandTypeDrawInfo>().m_pBrush));
                            break;
                        case SettlementSize.Hamlet:
                            m_aQuadrants[iQuadX, iQuadY].Landmarks.Add(new SignVillage(fPointX, fPointY, m_pWorld.LocationsGrid.RX, pLocX.Settlement.Name, pLoc.GetOwner().LandType.Get<LandTypeDrawInfo>().m_pBrush));
                            break;
                        case SettlementSize.Fort:
                            m_aQuadrants[iQuadX, iQuadY].Landmarks.Add(new SignFort(fPointX, fPointY, m_pWorld.LocationsGrid.RX, pLocX.Settlement.Name));
                            break;
                    }
                }
            }
            else
            {
                if (pLocX.Building != null)
                {
                    switch (pLocX.Building.Type)
                    {
                        case BuildingType.Lair:
                            m_aQuadrants[iQuadX, iQuadY].Landmarks.Add(new SignLair(fPointX, fPointY, m_pWorld.LocationsGrid.RX, ""));
                            break;
                        case BuildingType.Hideout:
                            m_aQuadrants[iQuadX, iQuadY].Landmarks.Add(new SignHideout(fPointX, fPointY, m_pWorld.LocationsGrid.RX, ""));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Сохраняет информацию об участке дороги в соответствующем квадранте
        /// </summary>
        /// <param name="pRoad">участок дороги</param>
        private void AddRoad(TransportationLinkBase pRoad)
        {
            if (pRoad.RoadLevel == RoadQuality.None)
                return;

            RoadType eRoadType = RoadType.LandRoad2;

            switch (pRoad.RoadLevel)
            {
                case RoadQuality.Country:
                    if (pRoad.Sea || pRoad.Embark)
                        return; //eRoadType = RoadType.SeaRoute1;
                    else
                        eRoadType = RoadType.LandRoad1;
                    break;
                case RoadQuality.Normal:
                    if (pRoad.Sea || pRoad.Embark)
                        return; //eRoadType = RoadType.SeaRoute2;
                    else
                        eRoadType = RoadType.LandRoad2;
                    break;
                case RoadQuality.Good:
                    if (pRoad.Sea || pRoad.Embark)
                        return; //eRoadType = RoadType.SeaRoute3;
                    else
                        eRoadType = RoadType.LandRoad3;
                    break;
            }

            bool[,] aQuadrants;
            PointF[][] aLinks = GetTransportationLink(pRoad, out aQuadrants);

            for (int i = 0; i < QUADRANTS_COUNT; i++)
            {
                for (int j = 0; j < QUADRANTS_COUNT; j++)
                {
                    if (aQuadrants[i, j])
                    {
                        foreach (var aLink in aLinks)
                        {
                            m_aQuadrants[i, j].RoadsMap[eRoadType].StartFigure();
                            m_aQuadrants[i, j].RoadsMap[eRoadType].AddCurve(aLink);

                            if (pRoad.RoadLevel == RoadQuality.Good)
                            {
                                m_aQuadrants[i, j].RoadsMap[RoadType.Back].StartFigure();
                                m_aQuadrants[i, j].RoadsMap[RoadType.Back].AddCurve(aLink);
                            }
                        }
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
        private PointF[][] GetTransportationLink(TransportationLinkBase pRoad, out bool[,] aQuadrants)
        {
            aQuadrants = new bool[QUADRANTS_COUNT, QUADRANTS_COUNT];

            List<PointF[]> cPathLines = new List<PointF[]>();

            //получаем линию без отражений
            cPathLines.Add(BuildPathLine(pRoad, 0, out bool bCross, out bool[,] aQuads));

            //если мир закольцован и построенная линия пересекает нулевой меридиан,
            //то построим для неё отражение
            if (m_pWorld.LocationsGrid.CycleShift != 0 && bCross)
            {
                if (pRoad.Points[0].X > 0)
                {
                    cPathLines.Add(BuildPathLine(pRoad, -m_pWorld.LocationsGrid.RX * 2, out bCross, out aQuads));
                }
                else
                {
                    cPathLines.Add(BuildPathLine(pRoad, m_pWorld.LocationsGrid.RX * 2, out bCross, out aQuads));
                }
            }

            //переносим информацию о квадрантах, через которые проходит линия, в общую матрицу
            for (int i = 0; i < QUADRANTS_COUNT; i++)
            {
                for (int j = 0; j < QUADRANTS_COUNT; j++)
                {
                    if (aQuads[i, j])
                        aQuadrants[i, j] = true;
                }
            }

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
        private PointF[] BuildPathLine(TransportationLinkBase pPath, float fShift, out bool bCross, out bool[,] aQuadrants)
        {
            bCross = false;
            aQuadrants = new bool[QUADRANTS_COUNT, QUADRANTS_COUNT];

            List<PointF> cRoadLine = new List<PointF>();
            float fLastPointX = pPath.Points[0].X + fShift;
            foreach (VoronoiVertex pPoint in pPath.Points)
            {
                //пересекает-ли линия от предыдущей точки к текущей нулевой меридиан?
                float fDX = fShift;
                if (Math.Abs(fLastPointX - pPoint.X - fShift) > m_pWorld.LocationsGrid.RX)
                {
                    //определимся, где у нас была предыдущая часть линии - на западе или на востоке?
                    //в зависимости от этого вычислим смещение для оставшейся части линии, чтобы 
                    //не было разрыва
                    fDX += fLastPointX < fShift ? -m_pWorld.LocationsGrid.RX * 2 : m_pWorld.LocationsGrid.RX * 2;
                    bCross = true;
                }

                if (pPoint.X > m_pWorld.LocationsGrid.RX ||
                    pPoint.X < -m_pWorld.LocationsGrid.RX)
                {
                    bCross = true;
                }

                cRoadLine.Add(ShiftPoint(pPoint, fDX));

                //в каком квадранте лежит новая точка линии
                int iQuadX = (int)(QUADRANTS_COUNT * (pPoint.X + m_pWorld.LocationsGrid.RX) / (2*m_pWorld.LocationsGrid.RX));
                int iQuadY = (int)(QUADRANTS_COUNT * (pPoint.Y + m_pWorld.LocationsGrid.RY) / (2*m_pWorld.LocationsGrid.RY));

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
                fK = (float)m_pWorld.LocationsGrid.RY / m_pWorld.LocationsGrid.RX;

            //ширина и высота карты мира в экранных координатах
            //из расчёта того, чтобы при единичном масштабе вся карта имела ширину 980 пикселей
            m_iScaledMapWidth = (int)(ClientRectangle.Width * m_fScaleMultiplier);
            m_iScaledMapHeight = (int)(m_iScaledMapWidth * fK);

            //создадим холст такого же размера, как и окно рисования, но не больше 
            //отмасштабированной карты
            m_pCanvas = new Bitmap(Math.Min(ClientRectangle.Width, m_iScaledMapWidth), Math.Min(ClientRectangle.Height, m_iScaledMapHeight));

            //если холст уже окна рисования, вычислим смещение для центрирования холста
            m_iShiftX = (ClientRectangle.Width - m_pCanvas.Width) / 2;

            //если холст меньше окна рисования по вертикали, вычислим смещение для центрирования холста
            m_iShiftY = (ClientRectangle.Height - m_pCanvas.Height) / 2;

            //определим размеры отображаемого участка карты равными размерам холста
            m_pDrawFrame.Width = m_pCanvas.Width;
            m_pDrawFrame.Height = m_pCanvas.Height;

            //коэффициент для перевода координат из абсолютной системы координат в экранную
            if (m_pWorld != null)
            {
                m_fActualScale = (float)(m_iScaledMapWidth) / (m_pWorld.LocationsGrid.RX * 2 - m_pWorld.LocationsGrid.FrameWidth * 2);

                m_fFrameWidth = m_pWorld.LocationsGrid.FrameWidth * m_fActualScale;

                //размеры одного квадранта в экранных координатах из рассчёта сетки квадрантов 8х8
                m_fOneQuadWidth = m_fActualScale * m_pWorld.LocationsGrid.RX * 2 / QUADRANTS_COUNT;
                m_fOneQuadHeight = m_fActualScale * m_pWorld.LocationsGrid.RY * 2 / QUADRANTS_COUNT;

                //размеры отображаемого участка карты в квадрантах
                //+2 потому что 1 квадрант мы отображаем всегда и нужно ещё иметь запас в 1 квадрант на случай, 
                //когда рамка отображаемой области не совпадает с границами квадрантов
                m_iQuadsWidth = 2 + (int)(m_pDrawFrame.Width / m_fOneQuadWidth);
                m_iQuadsHeight = 2 + (int)(m_pDrawFrame.Height / m_fOneQuadHeight);
            }
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
            if (m_pWorld == null)
                return;

            if (m_pWorld.LocationsGrid.CycleShift != 0)
                m_pDrawFrame.X = iX;
            else
                m_pDrawFrame.X = Math.Max(0, Math.Min(iX, m_iScaledMapWidth - m_pDrawFrame.Width + (int)m_fFrameWidth));

            while (m_pDrawFrame.X < 0)
                m_pDrawFrame.X += m_iScaledMapWidth;

            while (m_pDrawFrame.X > m_iScaledMapWidth)
                m_pDrawFrame.X -= m_iScaledMapWidth;

            m_pDrawFrame.Y = Math.Max(0, Math.Min(iY, m_iScaledMapHeight - m_pDrawFrame.Height + (int)m_fFrameWidth));

            if (m_pDrawFrame.X < (int)m_fFrameWidth)
                m_pDrawFrame.X = (int)m_fFrameWidth;

            if (m_pDrawFrame.X > m_iScaledMapWidth - m_pDrawFrame.Width + (int)m_fFrameWidth)
                m_pDrawFrame.X = m_iScaledMapWidth - m_pDrawFrame.Width + (int)m_fFrameWidth;

            if (m_pDrawFrame.Y < (int)m_fFrameWidth)
                m_pDrawFrame.Y = (int)m_fFrameWidth;

            if (m_pDrawFrame.Y > m_iScaledMapHeight - m_pDrawFrame.Height + (int)m_fFrameWidth)
                m_pDrawFrame.Y = m_iScaledMapHeight - m_pDrawFrame.Height + (int)m_fFrameWidth;

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

            if (m_pDrawFrame.X < (int)m_fFrameWidth)
                m_pDrawFrame.X = (int)m_fFrameWidth;

            if (m_pDrawFrame.X > m_iScaledMapWidth - m_pDrawFrame.Width + (int)m_fFrameWidth)
                m_pDrawFrame.X = m_iScaledMapWidth - m_pDrawFrame.Width + (int)m_fFrameWidth;

            if (m_pDrawFrame.Y < (int)m_fFrameWidth)
                m_pDrawFrame.Y = (int)m_fFrameWidth;

            if (m_pDrawFrame.Y > m_iScaledMapHeight - m_pDrawFrame.Height + (int)m_fFrameWidth)
                m_pDrawFrame.Y = m_iScaledMapHeight - m_pDrawFrame.Height + (int)m_fFrameWidth;

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
            gr.FillRectangle(new SolidBrush(LandTypes.Ocean.Get<LandTypeDrawInfo>().m_pColor), 0, 0, m_pCanvas.Width, m_pCanvas.Height);

            //если нет мира или мир вырожденный - больше рисовать нечего
            if (m_pWorld == null || m_pWorld.LocationsGrid.Locations.Length == 0)
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

            //закрашиваем карту в соответствии с выбранным режимом отображения карты
            for (int i = 0; i < m_iQuadsWidth; i++)
                for (int j = 0; j < m_iQuadsHeight; j++)
                    if (aVisibleQuads[i, j] != null)
                        aVisibleQuads[i, j].FillPath(gr, m_eMode, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);

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

            if (m_bShowRegions)
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            aVisibleQuads[i, j].DrawPath(gr, MapLayer.Regions, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);

            if (m_bShowProvincies)
                for (int i = 0; i < m_iQuadsWidth; i++)
                    for (int j = 0; j < m_iQuadsHeight; j++)
                        if (aVisibleQuads[i, j] != null)
                            aVisibleQuads[i, j].DrawPath(gr, MapLayer.Provincies, i * m_fOneQuadWidth + iQuadDX, j * m_fOneQuadHeight + iQuadDY, m_fActualScale);

            foreach (TransportationNode[] aPath in m_cPaths.Keys)
                DrawPath(gr, aPath, m_cPaths[aPath]);

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

            if (m_pSelectedState != null)
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
                    PointF[][] aLinks = GetTransportationLink(pLastNode.Links[pNode], out aQuads);
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
            pMatrix.Scale(m_fActualScale, m_fActualScale);

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

        private void DrawStateBorder(Graphics gr, State pState)
        {
            if (pState == null)
                return;

            if (!m_cStateBorders.TryGetValue(pState, out GraphicsPath pPath))
                return;

            Matrix pMatrix = new Matrix();

            int iDX = m_pDrawFrame.X;
            while (iDX < 0)
                iDX += m_iScaledMapWidth;
            while (iDX >= m_iScaledMapWidth)
                iDX -= m_iScaledMapWidth;
            pMatrix.Translate(-iDX, -m_pDrawFrame.Y);
            //контуры государств в m_cStateBorders лежат уменьшенные в ROUGH_SCALE раз, поэтому масштабный коэффициент здесь домножаем на ROUGH_SCALE
            pMatrix.Scale(m_fActualScale * ROUGH_SCALE, m_fActualScale * ROUGH_SCALE);

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
            SetScale(ScaleMultiplier);
        }

        private bool m_bScrolling = false;

        private Point m_pLastMouseLocation = new Point(0, 0);

        private void MapDraw_MouseDown(object sender, MouseEventArgs e)
        {
            m_bScrolling = true;
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
                p.X -= e.X - m_pLastMouseLocation.X;
                p.Y -= e.Y - m_pLastMouseLocation.Y;
            }
            m_pLastMouseLocation = e.Location;

            if (m_bScrolling)
                SetPan(p.X, p.Y);
            else
            {
                string sToolTip = GetTooltipString();

                if (toolTip1.GetToolTip(this) != sToolTip)
                    toolTip1.SetToolTip(this, sToolTip);
            }
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

            //переведём координаты курсора из экранных в оригинальные/ROUGH_SCALE координаты
            iX = (int)(iX / m_fActualScale) / ROUGH_SCALE;
            iY = (int)(iY / m_fActualScale) / ROUGH_SCALE;

            bool bContinent = false;
            if (m_pFocusedLandMass == null || !m_cLandMassBorders[m_pFocusedLandMass].IsVisible(iX, iY))
            {
                m_pFocusedContinent = null;
                m_pFocusedLandMass = null;
                m_pFocusedLand = null;
                m_pFocusedLocation = null;

                foreach (LandMass pLandMass in m_pWorld.LandMasses)
                {
                    GraphicsPath pLandMassPath = m_cLandMassBorders[pLandMass];

                    if (pLandMassPath.IsVisible(iX, iY))
                    {
                        m_pFocusedLandMass = pLandMass;
                        break;
                    }
                }
            }

            if (m_pFocusedLandMass != null)
            {
                bContinent = !m_pFocusedLandMass.IsWater;
                if (bContinent)
                    m_pFocusedContinent = m_pFocusedLandMass.GetOwner();
            }

            if (m_pFocusedContinent != null &&
                (m_pFocusedState == null || !m_cStateBorders[m_pFocusedState].IsVisible(iX, iY)))
            {
                m_pFocusedState = null;
                m_pFocusedProvince = null;

                foreach (State pState in m_pFocusedContinent.As<ContinentX>().Contents)
                {
                    GraphicsPath pStatePath = m_cStateBorders[pState];

                    if (pStatePath.IsVisible(iX, iY))
                    {
                        m_pFocusedState = pState;
                        break;
                    }
                }
            }

            if (m_pFocusedState != null &&
                (m_pFocusedProvince == null || !m_cProvinceBorders[m_pFocusedProvince].IsVisible(iX, iY)))
            {
                m_pFocusedProvince = null;

                foreach (Province pProvince in m_pFocusedState.Contents)
                {
                    GraphicsPath pProvincePath = m_cProvinceBorders[pProvince];

                    if (pProvincePath.IsVisible(iX, iY))
                    {
                        m_pFocusedProvince = pProvince;
                        break;
                    }
                }
            }


            if (m_pFocusedLandMass != null &&
                (m_pFocusedLand == null || !m_cLandBorders[m_pFocusedLand].IsVisible(iX, iY)))
            {
                m_pFocusedLocation = null;

                foreach (Land pLand in m_pFocusedLandMass.Contents)
                {
                    GraphicsPath pLandPath = m_cLandBorders[pLand];

                    if (pLandPath.IsVisible(iX, iY))
                    {
                        m_pFocusedLand = pLand;
                        break;
                    }
                }
            }

            if (m_pFocusedLand != null &&
                (m_pFocusedLocation == null || !m_cLocationBorders[m_pFocusedLocation].IsVisible(iX, iY)))
            {
                m_pFocusedLocation = null;

                foreach (Location pLoc in m_pFocusedLand.Contents)
                {
                    GraphicsPath pLocationPath = m_cLocationBorders[pLoc];

                    if (pLocationPath.IsVisible(iX, iY))
                    {
                        m_pFocusedLocation = pLoc;
                        break;
                    }
                }
            }

            return bContinent;
        }

        private string GetTooltipString()
        {
            if (m_pWorld == null)
                return "no world";

            StringBuilder sToolTip = new StringBuilder();

            bool bContinent = CheckMousePosition();

            /*
            {
                sToolTip += "Mouse          X = " + m_pLastMouseLocation.X.ToString() + ", Y = " + m_pLastMouseLocation.Y.ToString() + "\n";
                sToolTip += "Frame          X = " + m_pDrawFrame.X.ToString() + ", Y = " + m_pDrawFrame.Y.ToString() + "\n";
                sToolTip += "Shift          X = " + m_iShiftX.ToString() + ", Y = " + m_iShiftY.ToString() + "\n";

                float fX = m_pLastMouseLocation.X + m_pDrawFrame.X - m_iShiftX;
                float fY = m_pLastMouseLocation.Y + m_pDrawFrame.Y - m_iShiftY;

                sToolTip += "Scaled screen  X = " + ((int)fX).ToString() + ", Y = " + ((int)fY).ToString() + "\n";

                //while (iX > m_iScaledMapWidth)
                //    iX -= m_iScaledMapWidth;

                //while (iX < 0)
                //    iX += m_iScaledMapWidth;

                //переведём координаты курсора из экранных в оригинальные/ROUGH_SCALE координаты
                fX = fX / (m_fActualScale * ROUGH_SCALE);
                fY = fY / (m_fActualScale * ROUGH_SCALE);

                sToolTip += "Original 1:100 X = " + ((int)fX).ToString() + ", Y = " + ((int)fY).ToString() + "\n";

                Matrix pMatrix = new Matrix();
                int iDX = m_pDrawFrame.X;
                while (iDX < 0)
                    iDX += m_iScaledMapWidth;
                while (iDX >= m_iScaledMapWidth)
                    iDX -= m_iScaledMapWidth;
                pMatrix.Translate(-iDX, -m_pDrawFrame.Y);
                //контуры государств в m_cStateBorders лежат уменьшенные в ROUGH_SCALE раз, поэтому масштабный коэффициент здесь домножаем на ROUGH_SCALE
                pMatrix.Scale(m_fActualScale * ROUGH_SCALE, m_fActualScale * ROUGH_SCALE);

                PointF[] pPoints = { new PointF(fX, fY) };
                pMatrix.TransformPoints(pPoints);
                sToolTip += "Reverse        X = " + pPoints[0].X.ToString() + ", Y = " + pPoints[0].Y.ToString() + "\n";

                fX = fX * ROUGH_SCALE - m_pWorld.m_pGrid.RX;
                fY = fY * ROUGH_SCALE - m_pWorld.m_pGrid.RY;

                sToolTip += "Map            X = " + ((int)fX).ToString() + ", Y = " + ((int)fY).ToString() + "\n";

                if (m_pFocusedLocation != null)
                {
                    PointF[][] aPoints;
                    GraphicsPath pPath;
                    MapQuadrant[] aQuads;

                    aPoints = BuildPath(m_pFocusedLocation.m_pFirstLine, true, out aQuads);

                    foreach (var aPts in aPoints)
                    {
                        pPath = new GraphicsPath();
                        pPath.AddPolygon(aPts);
                        Matrix pReverseMatrix = new Matrix();
                        pReverseMatrix.Translate(-m_pWorld.m_pGrid.RX, -m_pWorld.m_pGrid.RY);
                        //pReverseMatrix.Scale(0.01f / m_fActualScale, 0.01f / m_fActualScale);
                        pPath.Transform(pReverseMatrix);
                        
                        var pBounds = pPath.GetBounds();

                        sToolTip += "Focused bounds X = (" + ((int)pBounds.X).ToString() + ", " + ((int)(pBounds.X + pBounds.Width)).ToString() + "), Y = (" + ((int)pBounds.Y).ToString() + ", " + ((int)(pBounds.Y + pBounds.Height)).ToString() + ")\n";
                    }
                }
            }
            */

            if (bContinent && m_pFocusedContinent != null)
                sToolTip.Append(m_pFocusedContinent.As<ContinentX>().ToString());

            if (m_pFocusedLand != null)
            {
                if (sToolTip.Length > 0)
                    sToolTip.Append("\n - ");

                sToolTip.Append(m_pFocusedLand.As<LandX>().ToString());
            }

            if (bContinent && m_pFocusedState != null)
            {
                if (sToolTip.Length > 0)
                    sToolTip.Append("\n   - ");

                sToolTip.AppendFormat("{1} {0} ({2})", m_pFocusedState.Society.Polity.Name, m_pFocusedState.Society.Name, m_pFocusedState.Society.TitularNation);
            }

            if (bContinent && m_pFocusedProvince != null)
            {
                if (sToolTip.Length > 0)
                    sToolTip.Append("\n     - ");

                sToolTip.AppendFormat("province {0} ({2}, {1})", m_pFocusedProvince.LocalSociety.Name, m_pFocusedProvince.AdministrativeCenter == null ? "-" : m_pFocusedProvince.AdministrativeCenter.ToString(), m_pFocusedProvince.LocalSociety.TitularNation);

                //sToolTip += "\n          [";

                //foreach(var pNation in m_pFocusedProvince.m_cNationsCount)
                //sToolTip += string.Format("{0}: {1}, ", pNation.Key, pNation.Value);
                //sToolTip += "]";
            }

            if (m_pFocusedLocation != null)
            {
                if (sToolTip.Length > 0)
                    sToolTip.Append("\n       - ");

                LocationX pFocusedLocX = m_pFocusedLocation.As<LocationX>();

                sToolTip.Append(pFocusedLocX.ToString());

                if (pFocusedLocX.Settlement != null && pFocusedLocX.Settlement.Buildings.Count > 0)
                {
                    Dictionary<string, int> cBuildings = new Dictionary<string, int>();

                    foreach (Building pBuilding in pFocusedLocX.Settlement.Buildings)
                    {
                        int iCount = 0;
                        cBuildings.TryGetValue(pBuilding.ToString(), out iCount);
                        cBuildings[pBuilding.ToString()] = iCount + 1;
                    }

                    foreach (var vBuilding in cBuildings)
                        sToolTip.Append("\n         - ").Append(vBuilding.Key).Append("  x").Append(vBuilding.Value.ToString());
                }

                if (pFocusedLocX.HaveRoadTo.Count > 0)
                {
                    sToolTip.Append("\nHave roads to:");
                    foreach (var pRoad in pFocusedLocX.HaveRoadTo)
                        sToolTip.Append("\n - ").Append(pRoad.Key.Settlement.Profile.Size.ToString()).Append(' ').Append(pRoad.Key.Settlement.Name).Append(" [").Append(pRoad.Value.Level.ToString()).Append(']');
                }

                if (pFocusedLocX.HaveSeaRouteTo.Count > 0)
                {
                    sToolTip.Append("\nHave sea routes to:");
                    foreach (Settlement pHarbour in pFocusedLocX.HaveSeaRouteTo.Select(x => x.Settlement))
                        sToolTip.Append("\n - ").Append(pHarbour.Profile.Size.ToString()).Append(' ').Append(pHarbour.Name);
                }
            }

            return sToolTip.ToString();
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

            PointF[][] aPoints = BuildPath(m_pSelectedState.FirstLines, false, out MapQuadrant[] aQuads);
            GraphicsPath pPath = new GraphicsPath();
            foreach (var aPts in aPoints)
                pPath.AddPolygon(aPts);
            RectangleF pRect = pPath.GetBounds();

            // Copy to a temporary variable to be thread-safe.
            SelectedStateChanged?.Invoke(this, new SelectedStateChangedEventArgs(m_pSelectedState, (int)(pRect.Left + pRect.Width / 2), (int)(pRect.Top + pRect.Height / 2)));
        }

        /// <summary>
        /// Вычисляет центральную точку указанного государства
        /// </summary>
        /// <param name="pState"></param>
        /// <returns></returns>
        public Point GetCentralPoint(State pState)
        {
            PointF[][] aPoints = BuildPath(pState.FirstLines, false, out MapQuadrant[] aQuads);
            GraphicsPath pPath = new GraphicsPath();
            foreach (var aPts in aPoints)
                pPath.AddPolygon(aPts);

            Matrix pMatrix = new Matrix();
            pMatrix.Scale(m_fActualScale, m_fActualScale);
            pPath.Transform(pMatrix);

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
