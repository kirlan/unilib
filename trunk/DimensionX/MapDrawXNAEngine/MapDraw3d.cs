using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using Socium;
using LandscapeGeneration;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MapDrawXNAEngine
{
    public enum MapLayer
    {
        Continents,
        Locations,
        Lands,
        Provincies,
        LandMasses,
        States
    }

    public enum MapMode
    {
        /// <summary>
        /// физическая карта
        /// </summary>
        Areas,
        /// <summary>
        /// этническая карта - коренное население
        /// </summary>
        Natives,
        /// <summary>
        /// этническая карта - доминирующая раса
        /// </summary>    
        Nations,
        /// <summary>
        /// карта влажности
        /// </summary>
        Humidity,
        /// <summary>
        /// карта высот
        /// </summary>
        Elevation,
        /// <summary>
        /// уровень технического развития
        /// </summary>
        TechLevel,
        /// <summary>
        /// уровень пси-способностей
        /// </summary>
        PsiLevel,
        /// <summary>
        /// общий уровень жизни, цивилизованность
        /// </summary>
        Infrastructure
    }

    public struct VertexPositionColorNormal : IVertexType
    {
        public Vector3 Position;
        public Microsoft.Xna.Framework.Color Color;
        public Vector3 Normal;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }
    }
    
    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, which allows it to
    /// render using a GraphicsDevice. This control shows how to draw animating
    /// 3D graphics inside a WinForms application. It hooks the Application.Idle
    /// event, using this to invalidate the control, which will cause the animation
    /// to constantly redraw.
    /// </summary>
    public class MapDraw3d : GraphicsDeviceControl
    {
        /// <summary>
        /// мир, карту которого мы рисуем
        /// </summary>
        internal World m_pWorld = null;

        BasicEffect effect;
        Stopwatch timer;

        VertexPositionColorNormal[] userPrimitives;
        int[] userPrimitivesIndices;
        int m_iTrianglesCount = 0;

        private readonly float m_fHeightMultiplier = 300;

        private void FillPrimitivesFake()
        {
            userPrimitives = new VertexPositionColorNormal[5];

            userPrimitives[0] = new VertexPositionColorNormal();
            userPrimitives[0].Position = new Vector3(-m_pWorld.m_pGrid.RX, 0, -m_pWorld.m_pGrid.RY);
            userPrimitives[0].Color = Microsoft.Xna.Framework.Color.White;

            userPrimitives[1] = new VertexPositionColorNormal();
            userPrimitives[1].Position = new Vector3(m_pWorld.m_pGrid.RX, 0, -m_pWorld.m_pGrid.RY);
            userPrimitives[1].Color = Microsoft.Xna.Framework.Color.Red;

            userPrimitives[2] = new VertexPositionColorNormal();
            userPrimitives[2].Position = new Vector3(m_pWorld.m_pGrid.RX, 0, m_pWorld.m_pGrid.RY);
            userPrimitives[2].Color = Microsoft.Xna.Framework.Color.Blue;    
        
            userPrimitives[3] = new VertexPositionColorNormal();
            userPrimitives[3].Position = new Vector3(-m_pWorld.m_pGrid.RX, 0, m_pWorld.m_pGrid.RY);
            userPrimitives[3].Color = Microsoft.Xna.Framework.Color.Green;

            userPrimitives[4] = new VertexPositionColorNormal();
            userPrimitives[4].Position = new Vector3(0, 0, 0);
            userPrimitives[4].Color = Microsoft.Xna.Framework.Color.Black;

            m_iTrianglesCount = 4;
            userPrimitivesIndices = new int[m_iTrianglesCount * 3];

            userPrimitivesIndices[0] = 4;
            userPrimitivesIndices[1] = 0;
            userPrimitivesIndices[2] = 1;

            userPrimitivesIndices[3] = 4;
            userPrimitivesIndices[4] = 1;
            userPrimitivesIndices[5] = 2;

            userPrimitivesIndices[6] = 4;
            userPrimitivesIndices[7] = 2;
            userPrimitivesIndices[8] = 3;

            userPrimitivesIndices[9] = 4;
            userPrimitivesIndices[10] = 3;
            userPrimitivesIndices[11] = 0;
        }

        /// <summary>
        /// набор примитивов без дублирования вертексов, с плавным переходом цвета между разноцветными примитивами
        /// </summary>
        private void FillPrimitives1()
        {
            // Create the verticies for our triangle
            userPrimitives = new VertexPositionColorNormal[m_pWorld.m_pGrid.m_aVertexes.Length + m_pWorld.m_pGrid.m_aLocations.Length];

            Dictionary<long, int> cVertexes = new Dictionary<long, int>();
            Dictionary<long, int> cLocations = new Dictionary<long, int>();

            int iCounter = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                userPrimitives[iCounter] = new VertexPositionColorNormal();
                userPrimitives[iCounter].Position = new Vector3(pVertex.m_fX, pVertex.m_fY, pVertex.m_fHeight > 0 ? pVertex.m_fHeight * m_fHeightMultiplier : pVertex.m_fHeight);

                LandTypeInfoX pLTI = null;
                Microsoft.Xna.Framework.Color pMixedColor = Microsoft.Xna.Framework.Color.White;
                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    if (pLTI == null)
                    {
                        pLTI = (pLoc.Owner as LandX).Type;
                        pMixedColor = ConvertColor(pLTI.m_pColor);
                    }
                    else
                        pMixedColor = Microsoft.Xna.Framework.Color.Lerp(pMixedColor, ConvertColor((pLoc.Owner as LandX).Type.m_pColor), 0.5f);
                    //if (pLTI != (pLoc.Owner as LandX).Type)
                    //{
                    //    pLTI = null;
                    //    break;
                    //}
                }
                //if(pLTI != null)
                //    userPrimitives[iCounter].Color = ConvertColor(pLTI.m_pColor);
                userPrimitives[iCounter].Color = pMixedColor;

                cVertexes[pVertex.m_iID] = iCounter;

                iCounter++;
            }

            m_iTrianglesCount = 0;
            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                userPrimitives[iCounter] = new VertexPositionColorNormal();
                userPrimitives[iCounter].Position = new Vector3(pLoc.X, pLoc.Y, pLoc.m_fHeight > 0 ? pLoc.m_fHeight * m_fHeightMultiplier : pLoc.m_fHeight);
                if (pLoc.Forbidden || pLoc.Owner == null)
                    userPrimitives[iCounter].Color = Microsoft.Xna.Framework.Color.White;
                else
                    userPrimitives[iCounter].Color = ConvertColor((pLoc.Owner as LandX).Type.m_pColor);

                cLocations[pLoc.m_iID] = iCounter;

                m_iTrianglesCount += pLoc.m_aBorderWith.Length;

                iCounter++;
            }

            // Create the indices used for each triangle
            userPrimitivesIndices = new int[m_iTrianglesCount * 3];

            iCounter = 0;

            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                if (pLoc.Forbidden || pLoc.m_pFirstLine == null)
                    continue;

                Line pLine = pLoc.m_pFirstLine;
                //последовательно перебирает все связанные линии, пока круг не замкнётся.
                do
                {

                    userPrimitivesIndices[iCounter++] = cLocations[pLoc.m_iID];
                    userPrimitivesIndices[iCounter++] = cVertexes[pLine.m_pPoint2.m_iID];
                    userPrimitivesIndices[iCounter++] = cVertexes[pLine.m_pPoint1.m_iID];

                    pLine = pLine.m_pNext;
                }
                while (pLine != pLoc.m_pFirstLine);
            }
        }

        /// <summary>
        /// набор примитивов с дублированием вершин на границе разноцветных регионов, так чтобы образовывалась чёткая граница цвета
        /// </summary>
        private void FillPrimitives2()
        {
            int iPrimitivesCount = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                List<LandTypeInfoX> cTypes = new List<LandTypeInfoX>();
                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    if (!cTypes.Contains((pLoc.Owner as LandX).Type))
                    {
                        cTypes.Add((pLoc.Owner as LandX).Type);
                        iPrimitivesCount++;
                    }
                }

            }

            // Create the verticies for our triangle
            userPrimitives = new VertexPositionColorNormal[iPrimitivesCount + m_pWorld.m_pGrid.m_aLocations.Length];

            Dictionary<long, Dictionary<LandTypeInfoX, int>> cVertexes = new Dictionary<long, Dictionary<LandTypeInfoX, int>>();
            Dictionary<long, int> cLocations = new Dictionary<long, int>();

            int iCounter = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                cVertexes[pVertex.m_iID] = new Dictionary<LandTypeInfoX, int>();

                //у нас x и y - это горизонтальная плоскость, причём y растёт в направлении вниз экрана, т.е. как бы к зрителю. а z - это высота.
                //в DX всё не как у людей. У них горизонтальная плоскость - это xz, причём z растёт к зрителю, а y - высота
                Vector3 pPosition = new Vector3(pVertex.m_fX, pVertex.m_fZ, pVertex.m_fY);
                if(m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                    pPosition -= Vector3.Normalize(pPosition) * (pVertex.m_fHeight > 0 ? pVertex.m_fHeight * m_fHeightMultiplier : pVertex.m_fHeight);
                else
                    pPosition += Vector3.Up * (pVertex.m_fHeight > 0 ? pVertex.m_fHeight * m_fHeightMultiplier : pVertex.m_fHeight);

                List<LandTypeInfoX> cTypes = new List<LandTypeInfoX>();
                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    if (!cTypes.Contains((pLoc.Owner as LandX).Type))
                    {
                        cTypes.Add((pLoc.Owner as LandX).Type);
                        userPrimitives[iCounter] = new VertexPositionColorNormal();
                        userPrimitives[iCounter].Position = pPosition;
                        userPrimitives[iCounter].Color = ConvertColor((pLoc.Owner as LandX).Type.m_pColor);

                        cVertexes[pVertex.m_iID][(pLoc.Owner as LandX).Type] = iCounter;

                        iCounter++;
                    }
                }
            }

            m_iTrianglesCount = 0;
            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                userPrimitives[iCounter] = new VertexPositionColorNormal();
                float fHeight = pLoc.m_fHeight > 0 ? pLoc.m_fHeight * m_fHeightMultiplier : pLoc.m_fHeight;
                if (pLoc.m_eType == RegionType.Peak)
                    fHeight += 2 * m_fHeightMultiplier;
                if (pLoc.m_eType == RegionType.Volcano)
                    fHeight -= 10 * m_fHeightMultiplier;

                userPrimitives[iCounter].Position = new Vector3(pLoc.X, pLoc.Z, pLoc.Y);
                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                    userPrimitives[iCounter].Position -= Vector3.Normalize(userPrimitives[iCounter].Position) * fHeight;
                else
                    userPrimitives[iCounter].Position += Vector3.Up * fHeight;

                if (pLoc.Forbidden || pLoc.Owner == null)
                    userPrimitives[iCounter].Color = Microsoft.Xna.Framework.Color.White;
                else
                    userPrimitives[iCounter].Color = ConvertColor((pLoc.Owner as LandX).Type.m_pColor);

                if (pLoc.m_eType == RegionType.Peak)
                    userPrimitives[iCounter].Color = Microsoft.Xna.Framework.Color.White;
                if (pLoc.m_eType == RegionType.Volcano)
                    userPrimitives[iCounter].Color = Microsoft.Xna.Framework.Color.Red;

                cLocations[pLoc.m_iID] = iCounter;

                m_iTrianglesCount += pLoc.m_aBorderWith.Length;

                iCounter++;
            }

            // Create the indices used for each triangle
            userPrimitivesIndices = new int[m_iTrianglesCount * 3];

            iCounter = 0;

            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                if (pLoc.Forbidden || pLoc.m_pFirstLine == null)
                    continue;

                bool bError = false;

                Line pLine = pLoc.m_pFirstLine;
                //последовательно перебирает все связанные линии, пока круг не замкнётся.
                do
                {

                    userPrimitivesIndices[iCounter++] = cLocations[pLoc.m_iID];
                    userPrimitivesIndices[iCounter++] = cVertexes[pLine.m_pPoint2.m_iID][(pLoc.Owner as LandX).Type];
                    userPrimitivesIndices[iCounter++] = cVertexes[pLine.m_pPoint1.m_iID][(pLoc.Owner as LandX).Type];

                    int index2 = userPrimitivesIndices[iCounter - 2];
                    int index3 = userPrimitivesIndices[iCounter - 1];

                    if ((userPrimitives[index2].Position.X == userPrimitives[index3].Position.X) && (userPrimitives[index2].Position.Z == userPrimitives[index3].Position.Z))
                        bError = true;

                    Vector3 pProjection2 = Vector3.Transform(userPrimitives[index2].Position, Matrix.CreateScale(1, 0, 1));
                    Vector3 pProjection3 = Vector3.Transform(userPrimitives[index3].Position, Matrix.CreateScale(1, 0, 1));

                    float fDistanceProjection = Vector3.Distance(pProjection2, pProjection3);
                    float fRealDistance = Vector3.Distance(userPrimitives[index2].Position, userPrimitives[index3].Position);
                    if (fDistanceProjection * 5 < fRealDistance)
                        bError = true;

                    pLine = pLine.m_pNext;
                }
                while (pLine != pLoc.m_pFirstLine);
            }
        }

        /// <summary>
        /// для всех вершин в списке вычисляем нормаль как нормализованный вектор суммы нормалей прилегающих граней
        /// </summary>
        private void CalculateNormals()
        {
            bool bError = false;

            for (int i = 0; i < userPrimitives.Length; i++)
                userPrimitives[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < m_iTrianglesCount; i++)
            {
                int index1 = userPrimitivesIndices[i * 3];
                int index2 = userPrimitivesIndices[i * 3 + 1];
                int index3 = userPrimitivesIndices[i * 3 + 2];

                if (index1 == 0 && index2 == 0 && index3 == 0)
                    continue;

                if ((userPrimitives[index1].Position.X == userPrimitives[index2].Position.X) && (userPrimitives[index1].Position.Z == userPrimitives[index2].Position.Z))
                    bError = true;
                if ((userPrimitives[index1].Position.X == userPrimitives[index3].Position.X) && (userPrimitives[index1].Position.Z == userPrimitives[index3].Position.Z))
                    bError = true;
                if ((userPrimitives[index2].Position.X == userPrimitives[index3].Position.X) && (userPrimitives[index2].Position.Z == userPrimitives[index3].Position.Z))
                    bError = true;

                Vector3 side1 = userPrimitives[index1].Position - userPrimitives[index3].Position;
                Vector3 side2 = userPrimitives[index1].Position - userPrimitives[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                userPrimitives[index1].Normal += normal;
                userPrimitives[index2].Normal += normal;
                userPrimitives[index3].Normal += normal;
            }

            for (int i = 0; i < userPrimitives.Length; i++)
                userPrimitives[i].Normal.Normalize();
        }

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
                    //Draw();
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
                    //Draw();
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
                    //Draw();
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
                    //Draw();
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
                    //Draw();
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
                    //Draw();
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
                    //Draw();
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
                    //Draw();
                }
            }
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
                    //Draw();

                    Refresh();

                    FireSelectedStateEvent();
                }
            }
        }

        public void FireSelectedStateEvent()
        {
            if (m_pSelectedState == null)
                return;

            //MapQuadrant[] aQuads;
            //PointF[][] aPoints = BuildPath(m_pSelectedState.m_cFirstLines, false, out aQuads);
            //GraphicsPath pPath = new GraphicsPath();
            //foreach (var aPts in aPoints)
            //    pPath.AddPolygon(aPts);
            //RectangleF pRect = pPath.GetBounds();

            //// Copy to a temporary variable to be thread-safe.
            //EventHandler<SelectedStateChangedEventArgs> temp = SelectedStateChanged;
            //if (temp != null)
            //    temp(this, new SelectedStateChangedEventArgs(m_pSelectedState, (int)(pRect.Left + pRect.Width / 2), (int)(pRect.Top + pRect.Height / 2)));
            
            // Copy to a temporary variable to be thread-safe.
            EventHandler<SelectedStateChangedEventArgs> temp = SelectedStateChanged;
            if (temp != null)
                temp(this, new SelectedStateChangedEventArgs(m_pSelectedState));
        }

        /// <summary>
        /// Информация о том, какое государство выбрано на карте
        /// </summary>
        public class SelectedStateChangedEventArgs : EventArgs
        {
            /// <summary>
            /// Выбранное государство
            /// </summary>
            public State m_pState;

            public SelectedStateChangedEventArgs(State pState)
            {
                m_pState = pState;
            }
        }
        
        /// <summary>
        /// Событие, извещающее о том, что на карте выбрано новое государство
        /// </summary>
        public event EventHandler<SelectedStateChangedEventArgs> SelectedStateChanged;
        
        /// <summary>
        /// Привязать карту к миру.
        /// Строим контуры всего, что придётся рисовать в ОРИГИНАЛЬНЫХ координатах
        /// и раскидываем их по квадрантам
        /// </summary>
        /// <param name="pWorld">мир</param>
        public void Assign(World pWorld)
        {
            m_pWorld = pWorld;

            //FillPrimitives2();
            FillPrimitivesFake();
            CalculateNormals();

            if (GraphicsDevice != null)
            {
                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                    m_pCamera = new RingworldCamera(m_pWorld.m_pGrid.RY, GraphicsDevice);
                else
                    m_pCamera = new PlainCamera(GraphicsDevice);
            }
        }

        private Microsoft.Xna.Framework.Color eSkyColor = Microsoft.Xna.Framework.Color.Lavender;

        public Camera m_pCamera = null;

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            // Create our effect.

            effect = new BasicEffect(GraphicsDevice);
            effect.VertexColorEnabled = true;

            // create the effect and vertex declaration for drawing the
            // picked triangle.
            lineEffect = new BasicEffect(GraphicsDevice);
            lineEffect.VertexColorEnabled = true;
            
            if (m_pWorld != null && m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                m_pCamera = new RingworldCamera(m_pWorld.m_pGrid.RY, GraphicsDevice);
            else
                m_pCamera = new PlainCamera(GraphicsDevice);

            // Start the animation timer.
            timer = Stopwatch.StartNew();
            lastTime = timer.Elapsed.TotalMilliseconds;

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        private Microsoft.Xna.Framework.Color ConvertColor(System.Drawing.Color Value)
        {
            return new Microsoft.Xna.Framework.Color(Value.R, Value.G, Value.B, Value.A);
        }

        private System.Drawing.Color ConvertColor(Microsoft.Xna.Framework.Color Value) 
        {
            return System.Drawing.Color.FromArgb(Value.A, Value.R, Value.G, Value.B);
        }
        
        double lastTime = 0;

        public float m_fScaling = 0;

        public void PanCamera(float fLeft, float fUp)
        {
            if (m_pLastPicking != null && m_pCurrentPicking != null)
            {
                m_pCamera.Target += (Vector3)m_pLastPicking - (Vector3)m_pCurrentPicking;
                m_pCurrentPicking = null;
                m_pLastPicking = null;
            }
            //m_pCamera.Pan(fLeft * m_pCamera.Position.Y * 0.00118f, fUp * m_pCamera.Position.Y * 0.00118f);
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            GraphicsDevice.Clear(eSkyColor);
            
            double fElapsedTime = timer.Elapsed.TotalMilliseconds - lastTime;
            lastTime = timer.Elapsed.TotalMilliseconds;

            if (m_pWorld == null)
                return;

            float time = (float)timer.Elapsed.TotalSeconds;

            // Set transform matrices.
            float aspect = GraphicsDevice.Viewport.AspectRatio;

            if (m_fScaling != 0)
            {
                m_pCamera.ZoomIn(m_fScaling * (float)fElapsedTime);
                m_fScaling = 0;
            }
            m_pCamera.Update();

            // Update the mouse state
            effect.View = m_pCamera.View;
            effect.Projection = m_pCamera.Projection;

            effect.LightingEnabled = true;
//            effect.DirectionalLight0.Direction = Vector3.Transform(Vector3.Normalize(new Vector3(0, 1, 0)), Matrix.CreateRotationX(time * 0.2f));
            effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1, -1, -1));
            effect.AmbientLightColor = Microsoft.Xna.Framework.Color.Multiply(eSkyColor, 0.2f).ToVector3();

            effect.PreferPerPixelLighting = true;

            //effect.FogColor = eSkyColor.ToVector3();
            //effect.FogEnabled = true;
            //effect.FogStart = m_pCamera.Position.Y;//5000.0f;
            //effect.FogEnd = Math.Max(m_pCamera.m_fDistance, m_pWorld.m_pGrid.RY / 2 + m_pCamera.Target.Y * m_pCamera.Target.Y / 5000);// 180000.0f;
            
            // Set renderstates.
            //GraphicsDevice.RasterizerState = RasterizerState.CullNone;//CullCounterClockwise;
            // Set renderstates.
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullCounterClockwiseFace;
            //rs.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;

            effect.World = Matrix.Identity;//CreateScale(0.5f);
            //effect.World = Matrix.CreateTranslation(20000, 0, -20000);

            // Draw the triangle.
            effect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorNormal>(PrimitiveType.TriangleList,
                                              userPrimitives, 0, userPrimitives.Length - 1, userPrimitivesIndices, 0, m_iTrianglesCount);
        
            // Draw the outline of the triangle under the cursor.
            DrawPickedTriangle();
        }

        public void FocusSelectedState()
        {
            m_pCamera.Target = new Vector3(m_pSelectedState.X, m_pSelectedState.Y, m_pSelectedState.Z);
        }

        // CalculateCursorRay Calculates a world space ray starting at the camera's
        // "eye" and pointing in the direction of the cursor. Viewport.Unproject is used
        // to accomplish this. see the accompanying documentation for more explanation
        // of the math behind this function.
        public Ray CalculateCursorRay(int x, int y, Matrix projectionMatrix, Matrix viewMatrix)
        {
            // create 2 positions in screenspace using the cursor position. 0 is as
            // close as possible to the camera, 1 is as far away as possible.
            Vector3 nearSource = new Vector3(x, y, 0f);
            Vector3 farSource = new Vector3(x, y, 0.5f);

            // use Viewport.Unproject to tell what those two screen space positions
            // would be in world space. we'll need the projection matrix and view
            // matrix, which we have saved as member variables. We also need a world
            // matrix, which can just be identity.
            Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(nearSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 farPoint = GraphicsDevice.Viewport.Unproject(farSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            // find the direction vector that goes from the nearPoint to the farPoint
            // and normalize it....
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            // and then create a new ray using nearPoint as the source.
            return new Ray(nearPoint, direction);
        }

        // Vertex array that stores exactly which triangle was picked.
        VertexPositionColor[] pickedTriangle =
        {
            new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.Magenta),
            new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.Magenta),
            new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.Magenta),
        };

        // Effect and vertex declaration for drawing the picked triangle.
        BasicEffect lineEffect;
        
        /// <summary>
        /// Helper for drawing the outline of the triangle currently under the cursor.
        /// </summary>
        void DrawPickedTriangle()
        {
            if (m_bPicked)
            {
                // Set line drawing renderstates. We disable backface culling
                // and turn off the depth buffer because we want to be able to
                // see the picked triangle outline regardless of which way it is
                // facing, and even if there is other geometry in front of it.
                RasterizerState rs = new RasterizerState();
                rs.CullMode = CullMode.CullCounterClockwiseFace;
                //rs.FillMode = FillMode.WireFrame;
                GraphicsDevice.RasterizerState = rs; 
                GraphicsDevice.DepthStencilState = DepthStencilState.None;

                // Activate the line drawing BasicEffect.
                lineEffect.Projection = m_pCamera.Projection;
                lineEffect.View = m_pCamera.View;

                lineEffect.CurrentTechnique.Passes[0].Apply();

                // Draw the triangle.
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                                          pickedTriangle, 0, 1);

                // Reset renderstates to their default values.
                GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
        }

        bool m_bPicked = false;

        Vector3? m_pCurrentPicking = null;
        Vector3? m_pLastPicking = null;

        /// <summary>
        /// Runs a per-triangle picking algorithm over all the models in the scene,
        /// storing which triangle is currently under the cursor.
        /// </summary>
        public void UpdatePicking(int x, int y)
        {
            // Look up a collision ray based on the current cursor position. See the
            // Picking Sample documentation for a detailed explanation of this.
            Ray cursorRay = CalculateCursorRay(x, y, m_pCamera.Projection, m_pCamera.View);

            // Keep track of the closest object we have seen so far, so we can
            // choose the closest one if there are several models under the cursor.
            float closestIntersection = float.MaxValue;

            Vector3 vertex1, vertex2, vertex3;

            // Perform the ray to model intersection test.
            float? intersection = RayIntersectsLandscape(cursorRay, Matrix.Identity,//CreateScale(0.5f),
                                                        out vertex1, out vertex2,
                                                        out vertex3);
            m_bPicked = false;

            m_pLastPicking = m_pCurrentPicking;

            // Do we have a per-triangle intersection with this model?
            if (intersection != null)
            {
                // If so, is it closer than any other model we might have
                // previously intersected?
                if (intersection < closestIntersection)
                {
                    // Store information about this model.
                    closestIntersection = intersection.Value;

                    // Store vertex positions so we can display the picked triangle.
                    pickedTriangle[0].Position = vertex1;
                    pickedTriangle[1].Position = vertex2;
                    pickedTriangle[2].Position = vertex3;

                    m_bPicked = true;

                    m_pCurrentPicking = cursorRay.Position + Vector3.Normalize(cursorRay.Direction) * intersection;
                }
            }
            else
                m_pCurrentPicking = null;
        }

        /// <summary>
        /// Checks whether a ray intersects a triangle. This uses the algorithm
        /// developed by Tomas Moller and Ben Trumbore, which was published in the
        /// Journal of Graphics Tools, volume 2, "Fast, Minimum Storage Ray-Triangle
        /// Intersection".
        /// 
        /// This method is implemented using the pass-by-reference versions of the
        /// XNA math functions. Using these overloads is generally not recommended,
        /// because they make the code less readable than the normal pass-by-value
        /// versions. This method can be called very frequently in a tight inner loop,
        /// however, so in this particular case the performance benefits from passing
        /// everything by reference outweigh the loss of readability.
        /// </summary>
        static void RayIntersectsTriangle(ref Ray ray,
                                          ref Vector3 vertex1,
                                          ref Vector3 vertex2,
                                          ref Vector3 vertex3, out float? result)
        {
            // Compute vectors along two edges of the triangle.
            Vector3 edge1, edge2;

            Vector3.Subtract(ref vertex2, ref vertex1, out edge1);
            Vector3.Subtract(ref vertex3, ref vertex1, out edge2);

            // Compute the determinant.
            Vector3 directionCrossEdge2;
            //векторное произведение - перпендикуляр к перемножаемым векторам
            Vector3.Cross(ref ray.Direction, ref edge2, out directionCrossEdge2);

            float determinant;
            //скалярное произведение - произведение длин векторов на косинус угла между ними. если угол 90, то 0
            Vector3.Dot(ref edge1, ref directionCrossEdge2, out determinant);

            // If the ray is parallel to the triangle plane, there is no collision.
            if (determinant > -float.Epsilon && determinant < float.Epsilon)
            {
                result = null;
                return;
            }

            float inverseDeterminant = 1.0f / determinant;

            // Calculate the U parameter of the intersection point.
            Vector3 distanceVector;
            Vector3.Subtract(ref ray.Position, ref vertex1, out distanceVector);

            float triangleU;
            //скалярное произведение - произведение длин векторов на косинус угла между ними. если угол 90, то 0
            Vector3.Dot(ref distanceVector, ref directionCrossEdge2, out triangleU);
            triangleU *= inverseDeterminant;

            // Make sure it is inside the triangle.
            if (triangleU < 0 || triangleU > 1)
            {
                result = null;
                return;
            }

            // Calculate the V parameter of the intersection point.
            Vector3 distanceCrossEdge1;
            Vector3.Cross(ref distanceVector, ref edge1, out distanceCrossEdge1);

            float triangleV;
            Vector3.Dot(ref ray.Direction, ref distanceCrossEdge1, out triangleV);
            triangleV *= inverseDeterminant;

            // Make sure it is inside the triangle.
            if (triangleV < 0 || triangleU + triangleV > 1)
            {
                result = null;
                return;
            }

            // Compute the distance along the ray to the triangle.
            float rayDistance;
            Vector3.Dot(ref edge2, ref distanceCrossEdge1, out rayDistance);
            rayDistance *= inverseDeterminant;

            // Is the triangle behind the ray origin?
            //if (rayDistance > 0)
            //{
            //    result = null;
            //    return;
            //}

            result = rayDistance;
        }

        /// <summary>
        /// Checks whether a ray intersects a model. This method needs to access
        /// the model vertex data, so the model must have been built using the
        /// custom TrianglePickingProcessor provided as part of this sample.
        /// Returns the distance along the ray to the point of intersection, or null
        /// if there is no intersection.
        /// </summary>
        float? RayIntersectsLandscape(Ray ray, Matrix modelTransform,
                                         //out bool insideBoundingSphere,
                                         out Vector3 vertex1, out Vector3 vertex2,
                                         out Vector3 vertex3)
        {
            vertex1 = vertex2 = vertex3 = Vector3.Zero;

            if (m_pWorld == null)
                return null;

            // The input ray is in world space, but our model data is stored in object
            // space. We would normally have to transform all the model data by the
            // modelTransform matrix, moving it into world space before we test it
            // against the ray. That transform can be slow if there are a lot of
            // triangles in the model, however, so instead we do the opposite.
            // Transforming our ray by the inverse modelTransform moves it into object
            // space, where we can test it directly against our model data. Since there
            // is only one ray but typically many triangles, doing things this way
            // around can be much faster.

            Matrix inverseTransform = Matrix.Invert(modelTransform);

            ray.Position = Vector3.Transform(ray.Position, inverseTransform);
            ray.Direction = Vector3.TransformNormal(ray.Direction, inverseTransform);

            // Start off with a fast bounding sphere test.
            //BoundingSphere boundingSphere = (BoundingSphere)tagData["BoundingSphere"];

            //if (boundingSphere.Intersects(ray) == null)
            //{
            //    // If the ray does not intersect the bounding sphere, we cannot
            //    // possibly have picked this model, so there is no need to even
            //    // bother looking at the individual triangle data.
            //    insideBoundingSphere = false;

            //    return null;
            //}
            //else
            //{
                // The bounding sphere test passed, so we need to do a full
                // triangle picking test.
                //insideBoundingSphere = true;

                // Keep track of the closest triangle we found so far,
                // so we can always return the closest one.
                float? closestIntersection = null;

                // Loop over the vertex data, 3 at a time (3 vertices = 1 triangle).
                //Vector3[] vertices = (Vector3[])tagData["Vertices"];

                for (int i = 0; i < userPrimitivesIndices.Length; i += 3)
                {
                    // Perform a ray to triangle intersection test.
                    float? intersection;

                    RayIntersectsTriangle(ref ray,
                                          ref userPrimitives[userPrimitivesIndices[i]].Position,
                                          ref userPrimitives[userPrimitivesIndices[i+1]].Position,
                                          ref userPrimitives[userPrimitivesIndices[i+2]].Position,
                                          out intersection);

                    // Does the ray intersect this triangle?
                    if (intersection != null)
                    {
                        // If so, is it closer than any other previous triangle?
                        if ((closestIntersection == null) ||
                            (intersection < closestIntersection))
                        {
                            // Store the distance to this triangle.
                            closestIntersection = intersection;

                            // Transform the three vertex positions into world space,
                            // and store them into the output vertex parameters.
                            Vector3.Transform(ref userPrimitives[userPrimitivesIndices[i]].Position,
                                              ref modelTransform, out vertex1);

                            Vector3.Transform(ref userPrimitives[userPrimitivesIndices[i+1]].Position,
                                              ref modelTransform, out vertex2);

                            Vector3.Transform(ref userPrimitives[userPrimitivesIndices[i+2]].Position,
                                              ref modelTransform, out vertex3);
                        }
                    }
                }

                return closestIntersection;
            //}
        }
    }
}
