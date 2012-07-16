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
using UniLibXNA;
using System.IO;
using Random;
using Socium.Settlements;
using Socium.Nations;
using Socium.Languages;
using nsUniLibControls;
using LandscapeGeneration.PathFind;

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
        /// фотореалистичный пейзаж
        /// </summary>
        Sattelite,
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

    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, which allows it to
    /// render using a GraphicsDevice. This control shows how to draw animating
    /// 3D graphics inside a WinForms application. It hooks the Application.Idle
    /// event, using this to invalidate the control, which will cause the animation
    /// to constantly redraw.
    /// </summary>
    public class MapDraw3d : GraphicsDeviceControl
    {
        public MapDraw3d()
//            : base()
        { }

        protected class TreeModel
        {
            public readonly Vector3 m_pPosition;
            public readonly Model m_pModel;
            public readonly Matrix worldMatrix;
            public Matrix[] boneTransforms;

            public TreeModel(Vector3 pPosition, float fAngle, float fScale, Model pModel, WorldShape eWorldShape, Texture2D pTexture)
            {
                m_pPosition = pPosition;
                m_pModel = pModel;

                if (eWorldShape == WorldShape.Ringworld)
                {
                    Vector3 pUp = Vector3.Transform(-Vector3.Normalize(m_pPosition), Matrix.CreateScale(1, 1, 0));
                    //                    Vector3 pUp = -Vector3.Normalize(m_pPosition);
                    Vector3 pAxis = Vector3.Cross(Vector3.Up, pUp);
                    pAxis.Normalize();
                    float fRotAngle = (float)Math.Acos(Vector3.Dot(Vector3.Up, pUp));
                    worldMatrix = Matrix.CreateScale(fScale * 10) * Matrix.CreateRotationY(fAngle) * Matrix.CreateFromAxisAngle(pAxis, fRotAngle) * Matrix.CreateTranslation(pPosition);
                }
                else if (eWorldShape == WorldShape.Planet)
                {
                    Vector3 pUp = Vector3.Normalize(m_pPosition);
                    Vector3 pAxis = Vector3.Cross(Vector3.Up, pUp);
                    pAxis.Normalize();
                    float fRotAngle = (float)Math.Acos(Vector3.Dot(Vector3.Up, pUp));
                    worldMatrix = Matrix.CreateScale(fScale * 10) * Matrix.CreateRotationY(fAngle) * Matrix.CreateFromAxisAngle(pAxis, fRotAngle) * Matrix.CreateTranslation(pPosition);
                }
                else
                    worldMatrix = Matrix.CreateScale(fScale * 10) * Matrix.CreateRotationY(fAngle) * Matrix.CreateTranslation(pPosition);

                boneTransforms = new Matrix[m_pModel.Bones.Count];
                m_pModel.CopyAbsoluteBoneTransformsTo(boneTransforms); 
                foreach (ModelMesh mesh in m_pModel.Meshes)
                {
                    foreach (Effect currentEffect in mesh.Effects)
                    {
                        if (eWorldShape == WorldShape.Ringworld)
                            currentEffect.CurrentTechnique = currentEffect.Techniques["TreeRingworld"];
                        else
                            currentEffect.CurrentTechnique = currentEffect.Techniques["Tree"];
                        currentEffect.Parameters["xTextureModel"].SetValue(pTexture);
                    }
                }
            }

            public override string ToString()
            {
                return string.Format("{0}:{1}:{2}", m_pPosition.X, m_pPosition.Y, m_pPosition.Z);
            }
        }

        protected class SettlementModel
        {
            public readonly Vector3 m_pPosition;
            public readonly float m_fAngle;
            public readonly Model m_pModel;
            public readonly float m_fScale;
            public readonly Matrix worldMatrix;
            public readonly string m_sName;
            public readonly int m_iSize;

            public SettlementModel(Vector3 pPosition, float fAngle, float fScale, Model pModel, WorldShape eWorldShape, Texture2D pTexture, string sName, int iSize)
            {
                m_pPosition = pPosition;
                m_fAngle = fAngle;
                m_fScale = fScale;
                m_pModel = pModel;
                m_sName = sName;
                m_iSize = iSize;

                if (eWorldShape == WorldShape.Ringworld)
                {
                    Vector3 pUp = Vector3.Transform(-Vector3.Normalize(m_pPosition), Matrix.CreateScale(1, 1, 0));
                    Vector3 pAxis = Vector3.Cross(Vector3.Up, pUp);
                    pAxis.Normalize();
                    float fRotAngle = (float)Math.Acos(Vector3.Dot(Vector3.Up, pUp));
                    worldMatrix = Matrix.CreateScale(m_fScale) * Matrix.CreateRotationY(m_fAngle) * Matrix.CreateFromAxisAngle(pAxis, fRotAngle) * Matrix.CreateTranslation(m_pPosition);
                }
                else if (eWorldShape == WorldShape.Planet)
                {
                    Vector3 pUp = Vector3.Normalize(m_pPosition);
                    Vector3 pAxis = Vector3.Cross(Vector3.Up, pUp);
                    pAxis.Normalize();
                    float fRotAngle = (float)Math.Acos(Vector3.Dot(Vector3.Up, pUp));
                    worldMatrix = Matrix.CreateScale(m_fScale) * Matrix.CreateRotationY(m_fAngle) * Matrix.CreateFromAxisAngle(pAxis, fRotAngle) * Matrix.CreateTranslation(m_pPosition);
                }
                else
                    worldMatrix = Matrix.CreateScale(m_fScale) * Matrix.CreateRotationY(m_fAngle) * Matrix.CreateTranslation(m_pPosition);

                foreach (ModelMesh mesh in m_pModel.Meshes)
                {
                    foreach (Effect currentEffect in mesh.Effects)
                    {
                        if (eWorldShape == WorldShape.Ringworld)
                            currentEffect.CurrentTechnique = currentEffect.Techniques["ModelRingworld"];
                        else
                            currentEffect.CurrentTechnique = currentEffect.Techniques["Model"];

                        currentEffect.Parameters["xTextureModel"].SetValue(pTexture);
                    }
                }
            }
        }

        protected class MapModeData<VertexType> where VertexType : IVertexType
        {
            public VertexType[] m_aVertices = new VertexType[0];
            public int[] m_aIndices = new int[0];
            public int m_iTrianglesCount = 0;
        }

        protected class MapLayersData
        {
            public VertexPositionColorNormal[] m_aVertices = new VertexPositionColorNormal[0];
            public Dictionary<MapLayer, int[]> m_aIndices = new Dictionary<MapLayer,int[]>();
            public Dictionary<LocationX, int[]> m_aLocations = new Dictionary<LocationX, int[]>();
        }

        /// <summary>
        /// мир, карту которого мы рисуем
        /// </summary>
        internal World m_pWorld = null;

        BasicEffect m_pBasicEffect;
        Effect m_pMyEffect;
        Stopwatch timer;

        MapLayersData m_pLayers = new MapLayersData();
        
        MapModeData<VertexMultitextured> m_pLand = new MapModeData<VertexMultitextured>();
        MapModeData<VertexMultitextured> m_pUnderwater = new MapModeData<VertexMultitextured>();
        MapModeData<VertexPositionTexture> m_pWater = new MapModeData<VertexPositionTexture>();

        Dictionary<MapMode, MapModeData<VertexPositionColorNormal>> m_cMapModeData = new Dictionary<MapMode, MapModeData<VertexPositionColorNormal>>();

        private float m_fLandHeightMultiplier = 0.1f;

        private Dictionary<Model, TreeModel[]> m_aTrees = new Dictionary<Model,TreeModel[]>();

        private SettlementModel[] m_aSettlements = new SettlementModel[0];

        private float m_fTextureScale = 5000;

        /// <summary>
        /// Вычисляет реальные 3d координаты центра локации с учётом её собственной высоты над сеткой и формы мира
        /// </summary>
        /// <param name="pLoc">локация</param>
        /// <param name="eShape">форма мира</param>
        /// <param name="fMultiplier">множитель высоты</param>
        /// <returns></returns>
        private static Vector3 GetPosition(IPointF pLoc, WorldShape eShape, float fMultiplier)
        {
            return GetPosition(pLoc, eShape, pLoc.H, fMultiplier);
        }

        /// <summary>
        /// Вычисляет реальные 3d координаты центра локации с учётом указанной высоты над сеткой и формы мира
        /// </summary>
        /// <param name="pLoc">локация</param>
        /// <param name="eShape">форма мира</param>
        /// <param name="fHeight">высота</param>
        /// <param name="fMultiplier">множитель высоты</param>
        /// <returns></returns>
        private static Vector3 GetPosition(IPointF pLoc, WorldShape eShape, float fHeight, float fMultiplier)
        {
            Vector3 pPosition = new Vector3(pLoc.X / 1000, pLoc.Z / 1000, pLoc.Y / 1000);
            Vector3 pUp = Vector3.Up;
            if (eShape == WorldShape.Ringworld)
                pUp = Vector3.Transform(-Vector3.Normalize(pPosition), Matrix.CreateScale(1, 1, 0));
            if (eShape == WorldShape.Planet)
                pUp = Vector3.Normalize(pPosition);

            pPosition += pUp * fHeight * fMultiplier;

            return pPosition;
        }

        private void BuildGeometry(LocationsGrid<LocationX>.BeginStepDelegate BeginStep,
                     LocationsGrid<LocationX>.ProgressStepDelegate ProgressStep)
        {
            Dictionary<LocationX, GeoData> cGeoLData = new Dictionary<LocationX, GeoData>();
            Dictionary<Vertex, GeoData2> cGeoVData = new Dictionary<Vertex, GeoData2>();

            if (BeginStep != null)
                BeginStep("Building geometry...", (m_pWorld.m_pGrid.m_aVertexes.Length * 2 + m_pWorld.m_pGrid.m_aLocations.Length * 2 + m_pWorld.m_cTransportGrid.Count) / 1000);

            int iUpdateCounter = 0;
            //добавляем в вершинный буффер узлы диаграммы Вороного
            for (int k=0; k<m_pWorld.m_pGrid.m_aVertexes.Length; k++)
            {
                if (iUpdateCounter++ > 1000)
                {
                    iUpdateCounter = 0;
                    if (ProgressStep != null)
                        ProgressStep();
                }

                Vertex pVertex = m_pWorld.m_pGrid.m_aVertexes[k];

                GeoData2 pData = new GeoData2(pVertex, m_pWorld.m_pGrid.m_eShape, m_fLandHeightMultiplier);

                bool bForbidden = true;
                for (int i = 0; i < pVertex.m_aLocations.Length; i++)
                {
                    LocationX pLoc = (LocationX)pVertex.m_aLocations[i];

                    LandType eLT = LandType.Ocean;
                    if (pLoc.Owner != null)
                        eLT = (pLoc.Owner as LandX).Type.m_eType; 
                    UpdateTexWeight(ref pData.TexWeights, GetTexWeights(eLT));
                    UpdateTexWeight(ref pData.TexWeights2, GetTexWeights2(eLT));

                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    bForbidden = false;
                }

                //если в окрестностях вершины нет ни одной разрешённой локации - пролетаем мимо.
                if (bForbidden)
                    continue;

                pData.TextureCoordinate = new Vector4(GetTexture(pVertex), 0, 0); 
                
                cGeoVData[pVertex] = pData;
            }

            //добавляем в вершинный буффер центры локаций
            for (int i=0; i<m_pWorld.m_pGrid.m_aLocations.Length; i++)
            {
                LocationX pLoc = m_pWorld.m_pGrid.m_aLocations[i];
                if (iUpdateCounter++ > 1000)
                {
                    iUpdateCounter = 0;
                    if (ProgressStep != null)
                        ProgressStep();
                } 
                
                if (pLoc.Forbidden || pLoc.Owner == null)
                    continue;

                float fHeight = pLoc.H;
                if (pLoc.m_eType == RegionType.Peak)
                    fHeight += 2 + Rnd.Get(1f);
                if (pLoc.m_eType == RegionType.Volcano)
                    fHeight -= 4;

                GeoData2 pData = new GeoData2(pLoc, m_pWorld.m_pGrid.m_eShape, fHeight, m_fLandHeightMultiplier);
                cGeoLData[pLoc] = pData;

                pData.TextureCoordinate = new Vector4(GetTexture(pLoc), 0, 0); 

                LandType eLT = LandType.Ocean;
                if (pLoc.Owner != null)
                    eLT = (pLoc.Owner as LandX).Type.m_eType;
                pData.TexWeights = GetTexWeights(eLT);
                pData.TexWeights2 = GetTexWeights2(eLT); 
                
                if (pLoc.m_eType == RegionType.Volcano || pLoc.m_eType == RegionType.Peak)
                {
                    Line pLine = pLoc.m_pFirstLine;
                    do
                    {
                        if (pLoc.m_eType == RegionType.Volcano)
                        {
                            cGeoVData[pLine.m_pInnerPoint].m_pPosition = GetPosition(pLine.m_pInnerPoint, m_pWorld.m_pGrid.m_eShape, pLine.m_pInnerPoint.m_fHeight + 6 + Rnd.Get(3f), m_fLandHeightMultiplier);
                            cGeoVData[pLine.m_pInnerPoint].m_pPosition = (cGeoVData[pLine.m_pInnerPoint].m_pPosition + cGeoLData[pLoc].m_pPosition) / 2;
                        }
                        else
                            cGeoVData[pLine.m_pInnerPoint].m_pPosition = GetPosition(pLine.m_pInnerPoint, m_pWorld.m_pGrid.m_eShape, pLine.m_pInnerPoint.m_fHeight + 1.5f + Rnd.Get(1f), m_fLandHeightMultiplier);

                        pLine = pLine.m_pNext;
                    }
                    while (pLine != pLoc.m_pFirstLine);
                }
            }

            //сделаем нашим квази-вертексам ссылки на квази-локации
            for (int k = 0; k < m_pWorld.m_pGrid.m_aVertexes.Length; k++)
            {
                if (iUpdateCounter++ > 1000)
                {
                    iUpdateCounter = 0;
                    if (ProgressStep != null)
                        ProgressStep();
                }

                Vertex pVertex = m_pWorld.m_pGrid.m_aVertexes[k];
                
                List<GeoData> cAllowed = new List<GeoData>();
                for (int i = 0; i < pVertex.m_aLocations.Length; i++)
                {
                    LocationX pLoc = (LocationX)pVertex.m_aLocations[i];

                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    cAllowed.Add(cGeoLData[pLoc]);
                }

                //если в окрестностях вершины нет ни одной разрешённой локации - пролетаем мимо.
                if (cAllowed.Count == 0)
                    continue;

                cGeoVData[pVertex].m_aLinked = cAllowed.ToArray();
            }
            
            //заполняем индексный буффер
            for (int i = 0; i < m_pWorld.m_pGrid.m_aLocations.Length; i++)
            {
                LocationX pLoc = m_pWorld.m_pGrid.m_aLocations[i];
                if (iUpdateCounter++ > 1000)
                {
                    iUpdateCounter = 0;
                    if (ProgressStep != null)
                        ProgressStep();
                } 
                
                if (pLoc.Forbidden || pLoc.m_pFirstLine == null)
                    continue;

                LandType eLT = (pLoc.Owner as LandX).Type.m_eType;

                //добавляем на горы снежные шапки в зависимости от высоты
                float fHeight = pLoc.H;
                if (pLoc.m_eType == RegionType.Peak)
                    fHeight += 2.5f;

                if (fHeight > m_pWorld.m_fMaxHeight / 3)
                    cGeoLData[pLoc].TexWeights.W = 2 * (float)Math.Pow((fHeight * 3 - m_pWorld.m_fMaxHeight) / m_pWorld.m_fMaxHeight, 3);

                Line pLine = pLoc.m_pFirstLine;
                do
                {
                    if (eLT == LandType.Mountains)
                    {
                        cGeoVData[pLine.m_pMidPoint].TexWeights = cGeoVData[pLine.m_pInnerPoint].TexWeights;
                        cGeoVData[pLine.m_pMidPoint].TexWeights2 = cGeoVData[pLine.m_pInnerPoint].TexWeights2;

                        cGeoVData[pLine.m_pPoint1].TexWeights = cGeoVData[pLine.m_pInnerPoint].TexWeights;
                        cGeoVData[pLine.m_pPoint1].TexWeights2 = cGeoVData[pLine.m_pInnerPoint].TexWeights2;

                        cGeoVData[pLine.m_pPoint2].TexWeights = cGeoVData[pLine.m_pInnerPoint].TexWeights;
                        cGeoVData[pLine.m_pPoint2].TexWeights2 = cGeoVData[pLine.m_pInnerPoint].TexWeights2;

                    }
                    else
                    {
                        cGeoVData[pLine.m_pInnerPoint].TexWeights += cGeoVData[pLine.m_pMidPoint].TexWeights;///2;
                        cGeoVData[pLine.m_pInnerPoint].TexWeights2 += cGeoVData[pLine.m_pMidPoint].TexWeights2;///2;

                        //aVertices[cVertexes[pLine.m_pInnerPoint.m_iID]].TexWeights += aVertices[cVertexes[pLine.m_pPoint1.m_iID]].TexWeights / 2;
                        //aVertices[cVertexes[pLine.m_pInnerPoint.m_iID]].TexWeights2 += aVertices[cVertexes[pLine.m_pPoint1.m_iID]].TexWeights2 / 2;

                        //aVertices[cVertexes[pLine.m_pInnerPoint.m_iID]].TexWeights += aVertices[cVertexes[pLine.m_pPoint2.m_iID]].TexWeights / 2;
                        //aVertices[cVertexes[pLine.m_pInnerPoint.m_iID]].TexWeights2 += aVertices[cVertexes[pLine.m_pPoint2.m_iID]].TexWeights2 / 2;
                    }

                    //добавляем на горы снежные шапки в зависимости от высоты
                    if (pLine.m_pPoint1.m_fHeight > m_pWorld.m_fMaxHeight / 3)
                        cGeoVData[pLine.m_pPoint1].TexWeights.W = 2 * (float)Math.Pow((pLine.m_pPoint1.m_fHeight * 3 - m_pWorld.m_fMaxHeight) / m_pWorld.m_fMaxHeight, 3);
                    if (pLine.m_pPoint2.m_fHeight > m_pWorld.m_fMaxHeight / 3)
                        cGeoVData[pLine.m_pPoint2].TexWeights.W = 2 * (float)Math.Pow((pLine.m_pPoint2.m_fHeight * 3 - m_pWorld.m_fMaxHeight) / m_pWorld.m_fMaxHeight, 3);
                    if (pLine.m_pMidPoint.m_fHeight > m_pWorld.m_fMaxHeight / 3)
                        cGeoVData[pLine.m_pMidPoint].TexWeights.W = 2 * (float)Math.Pow((pLine.m_pMidPoint.m_fHeight * 3 - m_pWorld.m_fMaxHeight) / m_pWorld.m_fMaxHeight, 3);
                    if (pLine.m_pInnerPoint.m_fHeight > m_pWorld.m_fMaxHeight / 3)
                        cGeoVData[pLine.m_pInnerPoint].TexWeights.W = 2 * (float)Math.Pow((pLine.m_pInnerPoint.m_fHeight * 3 - m_pWorld.m_fMaxHeight) / m_pWorld.m_fMaxHeight, 3);

                    Vector3 n1 = GetNormal(cGeoVData[pLine.m_pInnerPoint].m_pPosition, cGeoVData[pLine.m_pMidPoint].m_pPosition, cGeoVData[pLine.m_pPoint1].m_pPosition);
                    cGeoVData[pLine.m_pInnerPoint].m_pNormal += n1;
                    cGeoVData[pLine.m_pMidPoint].m_pNormal += n1;
                    cGeoVData[pLine.m_pPoint1].m_pNormal += n1;

                    Vector3 n2 = GetNormal(cGeoVData[pLine.m_pInnerPoint].m_pPosition, cGeoVData[pLine.m_pPoint2].m_pPosition, cGeoVData[pLine.m_pMidPoint].m_pPosition);
                    cGeoVData[pLine.m_pInnerPoint].m_pNormal += n2;
                    cGeoVData[pLine.m_pPoint2].m_pNormal += n2;
                    cGeoVData[pLine.m_pMidPoint].m_pNormal += n2;

                    Vector3 n3 = GetNormal(cGeoVData[pLine.m_pInnerPoint].m_pPosition, cGeoVData[pLine.m_pNext.m_pInnerPoint].m_pPosition, cGeoVData[pLine.m_pPoint2].m_pPosition);
                    cGeoVData[pLine.m_pInnerPoint].m_pNormal += n3;
                    cGeoVData[pLine.m_pNext.m_pInnerPoint].m_pNormal += n3;
                    cGeoVData[pLine.m_pPoint2].m_pNormal += n3;

                    Vector3 n4 = GetNormal(cGeoVData[pLine.m_pInnerPoint].m_pPosition, cGeoLData[pLoc].m_pPosition, cGeoVData[pLine.m_pNext.m_pInnerPoint].m_pPosition);
                    cGeoVData[pLine.m_pInnerPoint].m_pNormal += n4;
                    cGeoLData[pLoc].m_pNormal += n4;
                    cGeoVData[pLine.m_pNext.m_pInnerPoint].m_pNormal += n4;

                    pLine = pLine.m_pNext;
                }
                while (pLine != pLoc.m_pFirstLine);

                if (pLoc.m_pSettlement != null && pLoc.m_pSettlement.m_iRuinsAge == 0)
                {
                    //cGeoLData[pLoc].TexWeights += new Vector4(0.25f, 0, 0.5f, 0);
                    float fScale = 0.1f;
                    switch (pLoc.m_pSettlement.m_pInfo.m_eSize)
                    {
                        case SettlementSize.Hamlet:
                            fScale = 0.1f;
                            break;
                        case SettlementSize.Village:
                            fScale = 0.1f;
                            break;
                        case SettlementSize.Fort:
                            fScale = 0.3f;
                            break;
                        case SettlementSize.Town:
                            fScale = 0.3f;
                            break;
                        case SettlementSize.City:
                            fScale = 0.4f;
                            break;
                        case SettlementSize.Capital:
                            fScale = 0.4f;
                            break;
                    }
                    cGeoLData[pLoc].TexWeights2.Y = fScale*2;
                } 
                
                if (pLoc.m_eType == RegionType.Volcano)
                {
                    cGeoLData[pLoc].TexWeights = new Vector4(0, 0, 0, 0);
                    cGeoLData[pLoc].TexWeights2 = new Vector4(0, 0, 0, 1);

                    pLine = pLoc.m_pFirstLine;
                    do
                    {
                        cGeoVData[pLine.m_pInnerPoint].TexWeights += new Vector4(0, 0, 0, 0);
                        cGeoVData[pLine.m_pInnerPoint].TexWeights2 += new Vector4(0, 0, 0, 0.5f);

                        pLine = pLine.m_pNext;
                    }
                    while (pLine != pLoc.m_pFirstLine);
                }
            }

            List<RoadData> cRoads = new List<RoadData>();
            //вычислим дорожную сетку
            foreach (TransportationLinkBase pRoad in m_pWorld.m_cTransportGrid)
            {
                if (iUpdateCounter++ > 1000)
                {
                    iUpdateCounter = 0;
                    if (ProgressStep != null)
                        ProgressStep();
                } 
                
                if (pRoad.RoadLevel == RoadQuality.None)
                    continue;

                RoadData pRoadData = new RoadData();

                switch (pRoad.RoadLevel)
                {
                    case RoadQuality.Country:
                        if (!pRoad.Sea && !pRoad.Embark)
                            pRoadData.m_eType = RoadType.LandRoad1;
                        //else
                        //    eRoadType = RoadType.SeaRoute1;
                        break;
                    case RoadQuality.Normal:
                        if (!pRoad.Sea && !pRoad.Embark)
                            pRoadData.m_eType = RoadType.LandRoad2;
                        //else
                        //    eRoadType = RoadType.SeaRoute2;
                        break;
                    case RoadQuality.Good:
                        if (!pRoad.Sea && !pRoad.Embark)
                            pRoadData.m_eType = RoadType.LandRoad3;
                        //else
                        //    eRoadType = RoadType.SeaRoute3;
                        break;
                }

                if (pRoadData.m_eType == RoadType.None)
                    continue;

                List<GeoData> cRoadLine = new List<GeoData>();
                foreach (Vertex pVertex in pRoad.m_aPoints)
                {
                    if(pVertex is LocationX)
                        cRoadLine.Add(cGeoLData[(LocationX)pVertex]);
                    else
                        cRoadLine.Add(cGeoVData[pVertex]);
                }
                pRoadData.m_aShape = cRoadLine.ToArray();

                cRoads.Add(pRoadData);
            }
            m_aRoads = cRoads.ToArray();
            
            m_cGeoVData = new GeoData2[cGeoVData.Count];
            m_cGeoLData = new GeoData[cGeoLData.Count];

            int iIndex = 0;
            foreach (var vVNorm in cGeoVData)
            {
                vVNorm.Value.m_pNormal.Normalize();
                m_cGeoVData[iIndex++] = vVNorm.Value;
            }

            iIndex = 0;
            foreach (var vLNorm in cGeoLData)
            {
                vLNorm.Value.m_pNormal.Normalize();
                m_cGeoLData[iIndex++] = vLNorm.Value;
            }
        }

        private Vector3 GetNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vector3 side1 = v1 - v3;
            Vector3 side2 = v1 - v2;
            return Vector3.Cross(side1, side2);
        }

        /// <summary>
        /// дополнительные слои - границы локаций, земель, провинций, государств...
        /// </summary>
        /// <param name="aVertices">вершинный буфер</param>
        /// <param name="iTrianglesCount">количество треугольников</param>
        /// <param name="aIndices">индексный буфер</param>
        /// <param name="aTrees">массив моделей деревьев для отрисовки, при bOceanOnly = true возвращается пустой!</param>
        /// <param name="aSettlements">массив моделей поселений для отрисовки, при bOceanOnly = true возвращается пустой!</param>
        private void BuildLayers(out MapLayersData pData,
                     LocationsGrid<LocationX>.BeginStepDelegate BeginStep,
                     LocationsGrid<LocationX>.ProgressStepDelegate ProgressStep)
        {
            // Create the verticies for our triangle
            pData = new MapLayersData();
            pData.m_aVertices = new VertexPositionColorNormal[m_cGeoVData.Length];

            Dictionary<Vertex, int> cVertexes = new Dictionary<Vertex, int>();
            Dictionary<LocationX, int> cLocations = new Dictionary<LocationX, int>();

            if (BeginStep != null)
            {
                BeginStep("Building layers grid data...", m_cGeoVData.Length + m_cGeoLData.Length);
            }

            //добавляем в вершинный буффер узлы диаграммы Вороного
            int iCounter = 0;
            for (int k = 0; k < m_cGeoVData.Length; k++)
            {
                Vertex pVertex = m_cGeoVData[k].m_pOwner;
                VertexPositionColorNormal pVM = new VertexPositionColorNormal();

                pVM.Color = Microsoft.Xna.Framework.Color.Red;

                pVM.Position = m_cGeoVData[k].m_pPosition;
                pVM.Normal = m_cGeoVData[k].m_pNormal;

                pData.m_aVertices[iCounter] = pVM;
                cVertexes[pVertex] = iCounter;

                iCounter++;

                if (ProgressStep != null)
                    ProgressStep();
            }

            // Create the indices used for each triangle
            for (int i = 0; i < m_cGeoLData.Length; i++)
            {
                LocationX pLoc = (LocationX)m_cGeoLData[i].m_pOwner;
                pData.m_aLocations[pLoc] = BuildLocationReferencesIndices(pLoc, ref cVertexes);
            }
            pData.m_aIndices[MapLayer.Locations] = BuildLocationsIndices(ref cVertexes);
            pData.m_aIndices[MapLayer.Lands] = BuildLandsIndices(ref cVertexes);
            pData.m_aIndices[MapLayer.LandMasses] = BuildLandMassesIndices(ref cVertexes);
            pData.m_aIndices[MapLayer.Continents] = BuildContinentsIndices(ref cVertexes);
            pData.m_aIndices[MapLayer.Provincies] = BuildProvincesIndices(ref cVertexes);
            pData.m_aIndices[MapLayer.States] = BuildStatesIndices(ref cVertexes);
        }

        internal enum RoadType
        {
            None,
            LandRoad1,
            LandRoad2,
            LandRoad3,
            SeaRoute1,
            SeaRoute2,
            SeaRoute3,
        }
        
        private int[] BuildLocationReferencesIndices(LocationX pLoc, ref Dictionary<Vertex, int> cVertexes)
        {
            //добавляем в вершинный буффер центры локаций
            int m_iLinesCount = pLoc.m_aBorderWith.Length * 2;

            // Create the indices used for each triangle
            int[] aIndices = new int[m_iLinesCount * 2];

            int iCounter = 0;

            //заполняем индексный буффер
            Line pLine = pLoc.m_pFirstLine;
            do
            {
                aIndices[iCounter++] = cVertexes[pLine.m_pPoint1];
                aIndices[iCounter++] = cVertexes[pLine.m_pMidPoint];

                aIndices[iCounter++] = cVertexes[pLine.m_pMidPoint];
                aIndices[iCounter++] = cVertexes[pLine.m_pPoint2];

                pLine = pLine.m_pNext;
            }
            while (pLine != pLoc.m_pFirstLine);

            return aIndices;
        }
        private int[] BuildLocationsIndices(ref Dictionary<Vertex, int> cVertexes)
        {
            //добавляем в вершинный буффер центры локаций
            int m_iLinesCount = 0;
            for (int i = 0; i < m_cGeoLData.Length; i++)
            {
                LocationX pLoc = (LocationX)m_cGeoLData[i].m_pOwner;

                m_iLinesCount += pLoc.m_aBorderWith.Length * 2;

                //if (ProgressStep != null)
                //    ProgressStep();
            }

            // Create the indices used for each triangle
            int[] aIndices = new int[m_iLinesCount * 2];

            int iCounter = 0;

            //заполняем индексный буффер
            for (int i = 0; i < m_cGeoLData.Length; i++)
            {
                LocationX pLoc = (LocationX)m_cGeoLData[i].m_pOwner;
                //if (ProgressStep != null)
                //    ProgressStep();

                Line pLine = pLoc.m_pFirstLine;
                do
                {
                    aIndices[iCounter++] = cVertexes[pLine.m_pPoint1];
                    aIndices[iCounter++] = cVertexes[pLine.m_pMidPoint];

                    aIndices[iCounter++] = cVertexes[pLine.m_pMidPoint];
                    aIndices[iCounter++] = cVertexes[pLine.m_pPoint2];

                    pLine = pLine.m_pNext;
                }
                while (pLine != pLoc.m_pFirstLine);

            }

            return aIndices;
        }
        private int[] BuildLandsIndices(ref Dictionary<Vertex, int> cVertexes)
        {
            List<int> cIndices = new List<int>();

            //заполняем индексный буффер
            for (int i = 0; i < m_pWorld.m_aLands.Length; i++)
            {
                LandX pLand = m_pWorld.m_aLands[i];
                if (pLand.Forbidden)
                    continue;
                //if (ProgressStep != null)
                //    ProgressStep();

                foreach (Line pFirstLine in pLand.m_cFirstLines)
                {
                    Line pLine = pFirstLine;
                    do
                    {
                        cIndices.Add(cVertexes[pLine.m_pPoint1]);
                        cIndices.Add(cVertexes[pLine.m_pMidPoint]);

                        cIndices.Add(cVertexes[pLine.m_pMidPoint]);
                        cIndices.Add(cVertexes[pLine.m_pPoint2]);

                        pLine = pLine.m_pNext;
                    }
                    while (pLine != pFirstLine);
                }

            }

            return cIndices.ToArray();
        }
        private int[] BuildLandMassesIndices(ref Dictionary<Vertex, int> cVertexes)
        {
            List<int> cIndices = new List<int>();

            //заполняем индексный буффер
            for (int i = 0; i < m_pWorld.m_aLandMasses.Length; i++)
            {
                LandMass<LandX> pLandMass = m_pWorld.m_aLandMasses[i];
                if (pLandMass.Forbidden)
                    continue;
                //if (ProgressStep != null)
                //    ProgressStep();

                foreach (Line pFirstLine in pLandMass.m_cFirstLines)
                {
                    Line pLine = pFirstLine;
                    do
                    {
                        cIndices.Add(cVertexes[pLine.m_pPoint1]);
                        cIndices.Add(cVertexes[pLine.m_pMidPoint]);

                        cIndices.Add(cVertexes[pLine.m_pMidPoint]);
                        cIndices.Add(cVertexes[pLine.m_pPoint2]);

                        pLine = pLine.m_pNext;
                    }
                    while (pLine != pFirstLine);
                }

            }

            return cIndices.ToArray();
        }
        private int[] BuildContinentsIndices(ref Dictionary<Vertex, int> cVertexes)
        {
            List<int> cIndices = new List<int>();

            //заполняем индексный буффер
            for (int i = 0; i < m_pWorld.m_aContinents.Length; i++)
            {
                ContinentX pContinent = m_pWorld.m_aContinents[i];
                if (pContinent.Forbidden)
                    continue;
                //if (ProgressStep != null)
                //    ProgressStep();

                foreach (Line pFirstLine in pContinent.m_cFirstLines)
                {
                    Line pLine = pFirstLine;
                    do
                    {
                        cIndices.Add(cVertexes[pLine.m_pPoint1]);
                        cIndices.Add(cVertexes[pLine.m_pMidPoint]);

                        cIndices.Add(cVertexes[pLine.m_pMidPoint]);
                        cIndices.Add(cVertexes[pLine.m_pPoint2]);

                        pLine = pLine.m_pNext;
                    }
                    while (pLine != pFirstLine);
                }

            }

            return cIndices.ToArray();
        }
        private int[] BuildProvincesIndices(ref Dictionary<Vertex, int> cVertexes)
        {
            List<int> cIndices = new List<int>();

            //заполняем индексный буффер
            for (int i = 0; i < m_pWorld.m_aProvinces.Length; i++)
            {
                Province pProvince = m_pWorld.m_aProvinces[i];
                if (pProvince.Forbidden)
                    continue;
                //if (ProgressStep != null)
                //    ProgressStep();

                foreach (Line pFirstLine in pProvince.m_cFirstLines)
                {
                    Line pLine = pFirstLine;
                    do
                    {
                        cIndices.Add(cVertexes[pLine.m_pPoint1]);
                        cIndices.Add(cVertexes[pLine.m_pMidPoint]);

                        cIndices.Add(cVertexes[pLine.m_pMidPoint]);
                        cIndices.Add(cVertexes[pLine.m_pPoint2]);

                        pLine = pLine.m_pNext;
                    }
                    while (pLine != pFirstLine);
                }

            }

            return cIndices.ToArray();
        }
        private int[] BuildStatesIndices(ref Dictionary<Vertex, int> cVertexes)
        {
            List<int> cIndices = new List<int>();

            //заполняем индексный буффер
            for (int i = 0; i < m_pWorld.m_aStates.Length; i++)
            {
                State pState = m_pWorld.m_aStates[i];
                if (pState.Forbidden)
                    continue;
                //if (ProgressStep != null)
                //    ProgressStep();

                foreach (Line pFirstLine in pState.m_cFirstLines)
                {
                    Line pLine = pFirstLine;
                    do
                    {
                        cIndices.Add(cVertexes[pLine.m_pPoint1]);
                        cIndices.Add(cVertexes[pLine.m_pMidPoint]);

                        cIndices.Add(cVertexes[pLine.m_pMidPoint]);
                        cIndices.Add(cVertexes[pLine.m_pPoint2]);

                        pLine = pLine.m_pNext;
                    }
                    while (pLine != pFirstLine);
                }

            }

            return cIndices.ToArray();
        }

        /// <summary>
        /// текстурированная поверхность всего ландшафта
        /// </summary>
        /// <param name="bOceanOnly">если true, то строится только подводная часть</param>
        /// <param name="aVertices">вершинный буфер</param>
        /// <param name="iTrianglesCount">количество треугольников</param>
        /// <param name="aIndices">индексный буфер</param>
        /// <param name="aTrees">массив моделей деревьев для отрисовки, при bOceanOnly = true возвращается пустой!</param>
        /// <param name="aSettlements">массив моделей поселений для отрисовки, при bOceanOnly = true возвращается пустой!</param>
        private void BuildLand(bool bOceanOnly, out MapModeData<VertexMultitextured> pData, out LocationX[] aLocationReference, out Dictionary<Model, TreeModel[]> aTrees, out SettlementModel[] aSettlements,
                     LocationsGrid<LocationX>.BeginStepDelegate BeginStep,
                     LocationsGrid<LocationX>.ProgressStepDelegate ProgressStep)
        {
            //это - количество вершин треугольников для отрисовки. 
            //вычисляется как сумма количества узлов диаграммы Вороного (включая дополнительно построенные промежуточные точки) 
            //и количества локаций, за вычетом запретных.
            int iVertexesCount = 0;

            for (int k=0; k<m_cGeoVData.Length; k++)
            {
                Vertex pVertex = m_cGeoVData[k].m_pOwner;
                //считаем только те узлы диаграммы Вороного, которые соприкасаются хотя бы с одной разрешённой локацией
                for (int i = 0; i < m_cGeoVData[k].m_aLinked.Length; i++)
                {
                    LocationX pLoc = (LocationX)m_cGeoVData[k].m_aLinked[i].m_pOwner;

                    if (!bOceanOnly || 
                        (pLoc.Owner as LandX).Type.m_eType == LandType.Ocean ||
                        (pLoc.Owner as LandX).Type.m_eType == LandType.Coastral)
                    {
                        iVertexesCount++;
                        break;
                    }
                }
            }
            for (int i=0; i<m_cGeoLData.Length; i++)
            {
                LocationX pLoc = (LocationX)m_cGeoLData[i].m_pOwner;

                if (!bOceanOnly ||
                    (pLoc.Owner as LandX).Type.m_eType == LandType.Ocean ||
                    (pLoc.Owner as LandX).Type.m_eType == LandType.Coastral)
                {
                    iVertexesCount++;
                }
            }

            // Create the verticies for our triangle
            pData = new MapModeData<VertexMultitextured>();
            pData.m_aVertices = new VertexMultitextured[iVertexesCount];

            Dictionary<Vertex, int> cVertexes = new Dictionary<Vertex, int>();
            Dictionary<LocationX, int> cLocations = new Dictionary<LocationX, int>();

            if (BeginStep != null)
            {
                if (bOceanOnly)
                    BeginStep("Building textured underwater map data...", iVertexesCount + m_cGeoLData.Length);
                else
                    BeginStep("Building textured land map data...", iVertexesCount + m_cGeoLData.Length);
            } 
            
            //добавляем в вершинный буффер узлы диаграммы Вороного
            int iCounter = 0;
            for (int k=0; k<m_cGeoVData.Length; k++)
            {
                Vertex pVertex = m_cGeoVData[k].m_pOwner;

                bool bForbidden = true;
                bool bOcean = false;
                for (int i = 0; i < m_cGeoVData[k].m_aLinked.Length; i++)
                {
                    LocationX pLoc = (LocationX)m_cGeoVData[k].m_aLinked[i].m_pOwner;

                    LandType eLT = (pLoc.Owner as LandX).Type.m_eType;

                    if (eLT == LandType.Ocean || eLT == LandType.Coastral)
                        bOcean = true;

                    bForbidden = false;
                }

                //если в окрестностях вершины нет ни одной разрешённой локации - пролетаем мимо.
                if (bForbidden || (bOceanOnly && !bOcean))
                    continue;

                VertexMultitextured pVM = new VertexMultitextured();

                pVM.TexWeights = m_cGeoVData[k].TexWeights;
                pVM.TexWeights2 = m_cGeoVData[k].TexWeights2;

                pVM.Position = m_cGeoVData[k].m_pPosition;
                pVM.Normal = m_cGeoVData[k].m_pNormal;
                pVM.TextureCoordinate = m_cGeoVData[k].TextureCoordinate;

                pData.m_aVertices[iCounter] = pVM;
                cVertexes[pVertex] = iCounter;

                iCounter++;
            
                if (ProgressStep != null)
                    ProgressStep();
            }

            //добавляем в вершинный буффер центры локаций
            pData.m_iTrianglesCount = 0;
            for (int i = 0; i<m_cGeoLData.Length; i++)
            {
                LocationX pLoc = (LocationX)m_cGeoLData[i].m_pOwner;
                if (pLoc.Forbidden || pLoc.Owner == null)
                    continue;

                if (bOceanOnly &&
                    (pLoc.Owner as LandX).Type.m_eType != LandType.Ocean &&
                    (pLoc.Owner as LandX).Type.m_eType != LandType.Coastral)
                    continue;

                pData.m_aVertices[iCounter] = new VertexMultitextured();
                pData.m_aVertices[iCounter].Position = m_cGeoLData[i].m_pPosition;
                pData.m_aVertices[iCounter].Normal = m_cGeoLData[i].m_pNormal;
                pData.m_aVertices[iCounter].TextureCoordinate = m_cGeoLData[i].TextureCoordinate;
                pData.m_aVertices[iCounter].TexWeights = m_cGeoLData[i].TexWeights;
                pData.m_aVertices[iCounter].TexWeights2 = m_cGeoLData[i].TexWeights2;

                cLocations[pLoc] = iCounter;

                pData.m_iTrianglesCount += pLoc.m_aBorderWith.Length * 4;

                iCounter++;

                if (ProgressStep != null)
                    ProgressStep();
            }

            // Create the indices used for each triangle
            pData.m_aIndices = new int[pData.m_iTrianglesCount * 3];
            aLocationReference = new LocationX[pData.m_iTrianglesCount];

            iCounter = 0;

            Dictionary<Model, List<TreeModel>> cTrees = new Dictionary<Model, List<TreeModel>>();
            List<SettlementModel> cSettlements = new List<SettlementModel>();

            int iReferenceCounter = 0;

            //заполняем индексный буффер
            for (int i=0; i< m_cGeoLData.Length; i++)
            {
                LocationX pLoc = (LocationX)m_cGeoLData[i].m_pOwner;
                if (ProgressStep != null)
                    ProgressStep(); 
                
                if (bOceanOnly &&
                    (pLoc.Owner as LandX).Type.m_eType != LandType.Ocean &&
                    (pLoc.Owner as LandX).Type.m_eType != LandType.Coastral)
                    continue; 
                
                float fMinHeight = float.MaxValue;
                Line pLine = pLoc.m_pFirstLine;
                do
                {
                    aLocationReference[iReferenceCounter++] = pLoc;
                    pData.m_aIndices[iCounter++] = cVertexes[pLine.m_pInnerPoint];
                    pData.m_aIndices[iCounter++] = cVertexes[pLine.m_pMidPoint];
                    pData.m_aIndices[iCounter++] = cVertexes[pLine.m_pPoint1];

                    aLocationReference[iReferenceCounter++] = pLoc;
                    pData.m_aIndices[iCounter++] = cVertexes[pLine.m_pInnerPoint];
                    pData.m_aIndices[iCounter++] = cVertexes[pLine.m_pPoint2];
                    pData.m_aIndices[iCounter++] = cVertexes[pLine.m_pMidPoint];

                    aLocationReference[iReferenceCounter++] = pLoc;
                    pData.m_aIndices[iCounter++] = cVertexes[pLine.m_pInnerPoint];
                    pData.m_aIndices[iCounter++] = cVertexes[pLine.m_pNext.m_pInnerPoint];
                    pData.m_aIndices[iCounter++] = cVertexes[pLine.m_pPoint2];

                    aLocationReference[iReferenceCounter++] = pLoc;
                    pData.m_aIndices[iCounter++] = cVertexes[pLine.m_pInnerPoint];
                    pData.m_aIndices[iCounter++] = cLocations[pLoc];
                    pData.m_aIndices[iCounter++] = cVertexes[pLine.m_pNext.m_pInnerPoint];

                    if (fMinHeight > pLine.m_pPoint1.m_fHeight)
                        fMinHeight = pLine.m_pPoint1.m_fHeight;

                    pLine = pLine.m_pNext;
                }
                while (pLine != pLoc.m_pFirstLine);

                //считаем, что ни деревьев, ни поселений под водой не бывает
                LandType eLT = (pLoc.Owner as LandX).Type.m_eType;
                if (!bOceanOnly)
                {
                    float fScale = 0.02f; //0.015f;

                    fScale *= (float)(70 / Math.Sqrt(m_pWorld.m_pGrid.m_iLocationsCount));

                    if (eLT == LandType.Forest || eLT == LandType.Taiga || eLT == LandType.Jungle)
                    {
                        bool bEdge = false;
                        foreach (LocationX pLink in pLoc.m_aBorderWith)
                        {
                            if (pLink.Forbidden)
                                continue;
                        
                            LandType eLinkLT = LandType.Ocean;
                            if (pLink.Owner != null)
                                eLinkLT = (pLink.Owner as LandX).Type.m_eType;

                            if (eLinkLT != LandType.Forest && eLinkLT != LandType.Taiga && eLinkLT != LandType.Jungle)
                            {
                                if (eLinkLT != LandType.Ocean && eLinkLT != LandType.Coastral)
                                    AddTreeModels(pLink, ref eLT, ref pData.m_aVertices, ref cLocations, ref cVertexes, ref cTrees, fScale, 0.1f);
                                bEdge = true;
                            }
                        }
                        AddTreeModels(pLoc, ref eLT, ref pData.m_aVertices, ref cLocations, ref cVertexes, ref cTrees, fScale, bEdge ? 0.75f : 1f);
                    }

                    if (pLoc.m_pSettlement != null && pLoc.m_pSettlement.m_iRuinsAge == 0)
                    {
                        Texture2D pTexture = m_cSettlementTextures[pLoc.m_pSettlement.m_pInfo.m_eSize][pLoc.m_pSettlement.m_iTechLevel];
                        Model pModel = m_cSettlementModels[pLoc.m_pSettlement.m_pInfo.m_eSize][pLoc.m_pSettlement.m_iTechLevel];

                        int iSize = 0;
                        switch (pLoc.m_pSettlement.m_pInfo.m_eSize)
                        {
                            case SettlementSize.Hamlet:
                                iSize = 0;
                                break;
                            case SettlementSize.Village:
                                iSize = 0;
                                break;
                            case SettlementSize.Fort:
                                iSize = 1;
                                break;
                            case SettlementSize.Town:
                                iSize = 1;
                                break;
                            case SettlementSize.City:
                                iSize = 2;
                                break;
                            case SettlementSize.Capital:
                                iSize = 2;
                                break;
                        }

                        cSettlements.Add(new SettlementModel(pData.m_aVertices[cLocations[pLoc]].Position,
                                                Rnd.Get((float)Math.PI * 2), fScale,
                                                pModel, m_pWorld.m_pGrid.m_eShape, pTexture, pLoc.m_pSettlement.m_sName, iSize));
                    }
                }
            }

            aTrees = new Dictionary<Model, TreeModel[]>();
            foreach(var vTree in cTrees)
                aTrees[vTree.Key] = vTree.Value.ToArray();

            aSettlements = cSettlements.ToArray();
        }

        private void AddTreeModels(LocationX pLoc, ref LandType eLT, ref VertexMultitextured[] aVertices, ref Dictionary<LocationX, int> cLocations, ref Dictionary<Vertex, int> cVertexes, ref Dictionary<Model, List<TreeModel>> cTrees, float fScale, float fProbability)
        {
            //последовательно перебирает все связанные линии, пока круг не замкнётся.
            Line pLine = pLoc.m_pFirstLine;
            do
            {
                Vector3 pCenter = aVertices[cVertexes[pLine.m_pInnerPoint]].Position;

                if (pLoc.m_pSettlement == null && Rnd.Get(1f) < fProbability)
                    AddTreeModel((pCenter + aVertices[cLocations[pLoc]].Position) / 2, ref eLT, ref cTrees, fScale);

                if (pLoc.m_pSettlement == null ||
                    pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Hamlet ||
                    pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Village ||
                    pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Town ||
                    pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Fort)
                {
                    if (Rnd.Get(1f) < fProbability)
                        AddTreeModel(pCenter, ref eLT, ref cTrees, fScale);
                    if (Rnd.Get(1f) < fProbability)
                        AddTreeModel((pCenter + aVertices[cVertexes[pLine.m_pPoint2]].Position) / 2, ref eLT, ref cTrees, fScale);
                    if (Rnd.Get(1f) < fProbability)
                        AddTreeModel((pCenter + aVertices[cVertexes[pLine.m_pPoint1]].Position) / 2, ref eLT, ref cTrees, fScale);
                }

                pLine = pLine.m_pNext;
            }
            while (pLine != pLoc.m_pFirstLine);
        }

        private void AddTreeModel(Vector3 pPos, ref LandType eLT, ref Dictionary<Model, List<TreeModel>> cTrees, float fScale)
        {
            Model pTree = null;
            switch (eLT)
            {
                case LandType.Forest:
                    pTree = treeModel[Rnd.Get(treeModel.Length)];
                    break;
                case LandType.Jungle:
                    if (Rnd.OneChanceFrom(3))
                        pTree = treeModel[Rnd.Get(treeModel.Length)];
                    else
                    {
                        pTree = palmModel[Rnd.Get(palmModel.Length)];
                        fScale *= 1.75f;
                    }
                    break;
                case LandType.Taiga:
                    pTree = Rnd.OneChanceFrom(3) ? treeModel[Rnd.Get(treeModel.Length)] : pineModel[Rnd.Get(pineModel.Length)];
                    break;
            }

            if (pTree != null)
            {
                List<TreeModel> cInstances;
                if(!cTrees.TryGetValue(pTree, out cInstances))
                {
                    cInstances = new List<TreeModel>();
                    cTrees[pTree] = cInstances;
                }

                cInstances.Add(new TreeModel(pPos, Rnd.Get((float)Math.PI * 2), fScale, pTree, m_pWorld.m_pGrid.m_eShape, treeTexture));
            }
        }

        private void UpdateTexWeight(ref Vector4 pOriginal, Vector4 pAddon)
        {
            pOriginal.X = Math.Max(pOriginal.X, pAddon.X);
            pOriginal.Y = Math.Max(pOriginal.Y, pAddon.Y);
            pOriginal.Z = Math.Max(pOriginal.Z, pAddon.Z);
            pOriginal.W = Math.Max(pOriginal.W, pAddon.W);
        }

        private Vector4 GetTexWeights(LandType eLT)
        {
            Vector4 texWeights = new Vector4();
            texWeights.X = (eLT == LandType.Savanna || eLT == LandType.Coastral || eLT == LandType.Desert) ? 0.7f + Rnd.Get(0.6f) : Rnd.Get(0.2f);
            texWeights.Y = (eLT == LandType.Tundra || eLT == LandType.Savanna || eLT == LandType.Plains || eLT == LandType.Coastral || eLT == LandType.Ocean) ? 0.7f + Rnd.Get(0.6f) : Rnd.Get(0.2f);
            texWeights.Y = (eLT == LandType.Forest || eLT == LandType.Jungle || eLT == LandType.Taiga) ? 0.1f + Rnd.Get(0.2f) : texWeights.Y;
            texWeights.Z = (eLT == LandType.Mountains || eLT == LandType.Coastral || eLT == LandType.Ocean) ? 0.7f + Rnd.Get(0.6f) : Rnd.Get(0.2f);
            texWeights.W = (eLT == LandType.Tundra) ? 0.7f + Rnd.Get(0.6f) : 0;

            return texWeights;
        }

        private Vector4 GetTexWeights2(LandType eLT)
        {
            Vector4 texWeights = new Vector4();
            texWeights.X = (eLT == LandType.Swamp || eLT == LandType.Forest || eLT == LandType.Jungle || eLT == LandType.Taiga) ? 0.7f + Rnd.Get(0.6f) : Rnd.Get(0.2f);
            texWeights.Y = 0;// (eLT == LandType.Savanna) ? 1 : 0;
            texWeights.Z = (eLT == LandType.Swamp) ? 0.7f + Rnd.Get(0.6f) : Rnd.Get(0.1f);
            texWeights.W = 0;

            return texWeights;
        }

        /// <summary>
        /// текстурированная поверхность водной глади.
        /// поскольку у нас миры могут быть разными и водная поверхность - не обязательно плоскость, приходится строить воду по локациям.
        /// с другой стороны, коль скоро оно гладкая, то мы можем обойтись без средних и внутренних точек.
        /// </summary>
        private void BuildWater()
        {
            int iPrimitivesCount = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                for (int i = 0; i < pVertex.m_aLocations.Length; i++)
                {
                    LocationX pLoc = (LocationX)pVertex.m_aLocations[i];

                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    if ((pLoc.Owner as LandX).Type.m_eType == LandType.Ocean ||
                        (pLoc.Owner as LandX).Type.m_eType == LandType.Coastral)
                    {
                        iPrimitivesCount++;
                        break;
                    }
                }

            }
            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                if (pLoc.Forbidden || pLoc.Owner == null)
                    continue;

                if ((pLoc.Owner as LandX).Type.m_eType == LandType.Ocean ||
                    (pLoc.Owner as LandX).Type.m_eType == LandType.Coastral)
                {
                    iPrimitivesCount++;
                }
            }

            // Create the verticies for our triangle
            m_pWater.m_aVertices = new VertexPositionTexture[iPrimitivesCount];

            Dictionary<long, int> cVertexes = new Dictionary<long, int>();
            Dictionary<long, int> cLocations = new Dictionary<long, int>();

            int iCounter = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                VertexPositionTexture pVM = new VertexPositionTexture();

                bool bOcean = false;
                for (int i = 0; i < pVertex.m_aLocations.Length; i++)
                {
                    LocationX pLoc = (LocationX)pVertex.m_aLocations[i];

                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    if ((pLoc.Owner as LandX).Type.m_eType == LandType.Ocean ||
                        (pLoc.Owner as LandX).Type.m_eType == LandType.Coastral)
                        bOcean = true;
                }

                if (!bOcean)
                    continue;

                //у нас x и y - это горизонтальная плоскость, причём y растёт в направлении вниз экрана, т.е. как бы к зрителю. а z - это высота.
                //в DX всё не как у людей. У них горизонтальная плоскость - это xz, причём z растёт к зрителю, а y - высота
                pVM.Position = GetPosition(pVertex, m_pWorld.m_pGrid.m_eShape, 0);

                pVM.TextureCoordinate = GetTexture(pVertex);

                m_pWater.m_aVertices[iCounter] = pVM;
                cVertexes[pVertex.m_iID] = iCounter;

                iCounter++;
            }

            m_pWater.m_iTrianglesCount = 0;
            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                if (pLoc.Forbidden || pLoc.Owner == null)
                    continue;

                if ((pLoc.Owner as LandX).Type.m_eType != LandType.Ocean &&
                    (pLoc.Owner as LandX).Type.m_eType != LandType.Coastral)
                    continue;

                m_pWater.m_aVertices[iCounter] = new VertexPositionTexture();
                float fHeight = pLoc.H * m_fLandHeightMultiplier;

                m_pWater.m_aVertices[iCounter].Position = GetPosition(pLoc, m_pWorld.m_pGrid.m_eShape, 0);

                m_pWater.m_aVertices[iCounter].TextureCoordinate = GetTexture(pLoc);

                cLocations[pLoc.m_iID] = iCounter;

                m_pWater.m_iTrianglesCount += pLoc.m_aBorderWith.Length;

                iCounter++;
            }

            // Create the indices used for each triangle
            m_pWater.m_aIndices = new int[m_pWater.m_iTrianglesCount * 3];

            iCounter = 0;

            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                if (pLoc.Forbidden || pLoc.m_pFirstLine == null)
                    continue;

                if ((pLoc.Owner as LandX).Type.m_eType != LandType.Ocean &&
                    (pLoc.Owner as LandX).Type.m_eType != LandType.Coastral)
                    continue;

                Line pLine = pLoc.m_pFirstLine;
                //последовательно перебирает все связанные линии, пока круг не замкнётся.
                do
                {

                    m_pWater.m_aIndices[iCounter++] = cLocations[pLoc.m_iID];
                    m_pWater.m_aIndices[iCounter++] = cVertexes[pLine.m_pPoint2.m_iID];
                    m_pWater.m_aIndices[iCounter++] = cVertexes[pLine.m_pPoint1.m_iID];

                    pLine = pLine.m_pNext;
                }
                while (pLine != pLoc.m_pFirstLine);
            }
        }

        private Vector2 GetTexture(Vertex pVertex)
        {
            Vector2 TextureCoordinate = new Vector2(0);
            
            if (m_pWorld.m_pGrid.m_eShape == WorldShape.Planet)
            {
                float fRo = (float)Math.Atan2(pVertex.m_fY, Math.Sqrt(pVertex.m_fX * pVertex.m_fX + pVertex.m_fZ * pVertex.m_fZ));
                float fPhi = (float)Math.Atan2(pVertex.m_fX, pVertex.m_fZ);

                float fPhi4 = (float)(fPhi % (Math.PI / 2));

                //чтобы был гладкий переход, нужно чтобы в RX/2 укладывалось целое количество тайлов текстуры!
                //или нужно вводить нормирующий коэффициент, чтобы на четверть экватора получалась координата текстуры кратная размеру тайла.
                if (fRo < -Math.PI / 4 && fRo > -Math.PI * 3 / 4)
                {
                    float fH = (float)(-Math.PI / 4 - fRo);
                    float fPhiRel = (float)(fH - fPhi4 * (4 * fH - Math.PI) / Math.PI);

                    TextureCoordinate.X = fPhiRel * m_pWorld.m_pGrid.RX / ((float)Math.PI * 100000);
                    TextureCoordinate.Y = fRo * 2 * m_pWorld.m_pGrid.RY / ((float)Math.PI * 100000);
                }
                else if (fRo > Math.PI / 4 && fRo < Math.PI * 3 / 4)
                {
                    float fH = (float)(fRo - Math.PI / 4);
                    float fPhiRel = (float)(fH - fPhi4 * (4 * fH - Math.PI) / Math.PI);

                    TextureCoordinate.X = fPhiRel * m_pWorld.m_pGrid.RX / ((float)Math.PI * 100000);
                    TextureCoordinate.Y = fRo * 2 * m_pWorld.m_pGrid.RY / ((float)Math.PI * 100000);
                }
                else
                {
                    TextureCoordinate.X = fPhi4 * m_pWorld.m_pGrid.RX / ((float)Math.PI * 100000);
                    TextureCoordinate.Y = fRo * 2 * m_pWorld.m_pGrid.RY / ((float)Math.PI * 100000);
                }
            }
            else
            {
                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                {
                    float fPhi = (float)Math.Atan2(pVertex.m_fX, pVertex.m_fZ);

                    TextureCoordinate.X = fPhi * m_pWorld.m_pGrid.RX / ((float)Math.PI * 100000);
                }
                else
                {
                    TextureCoordinate.X = pVertex.m_fX / 100000;
                }
                TextureCoordinate.Y = pVertex.m_fY / 100000;
            }

            return TextureCoordinate;
        }

        private Microsoft.Xna.Framework.Color GetMapModeColor(LocationX pLoc, MapMode pMode)
        {
            Microsoft.Xna.Framework.Color pResult = Microsoft.Xna.Framework.Color.DarkBlue; 
            
            if (!pLoc.Forbidden && pLoc.Owner != null)
            {
                switch (pMode)
                {
                    case MapMode.Areas:
                        {
                            if (pLoc.m_eType == RegionType.Peak)
                                pResult = Microsoft.Xna.Framework.Color.White;
                            if (pLoc.m_eType == RegionType.Volcano)
                                pResult = Microsoft.Xna.Framework.Color.Red;

                            pResult = ConvertColor((pLoc.Owner as LandX).Type.m_pColor);
                        }
                        break;
                    case MapMode.Natives:
                        {
                            if ((pLoc.Owner as LandX).Area != null)
                            { 
                                Nation pNation = ((pLoc.Owner as LandX).Area as AreaX).m_pNation;
                                if (pNation != null)
                                    pResult = m_cNationColorsID[pNation];
                            }
                        }
                        break;
                    case MapMode.Nations:
                        {
                            if ((pLoc.Owner as LandX).m_pProvince != null)
                            {
                                Nation pNation = (pLoc.Owner as LandX).m_pProvince.m_pNation;
                                if (pNation != null)
                                    pResult = m_cNationColorsID[pNation];
                            }
                        }
                        break;
                    case MapMode.Humidity:
                        {
                            if (!(pLoc.Owner as LandX).IsWater)
                            {
                                KColor color = new KColor();
                                color.RGB = System.Drawing.Color.LightBlue;
                                color.Lightness = 1.0 - (double)(pLoc.Owner as LandX).Humidity / 200;

                                pResult = ConvertColor(color.RGB);
                            }
                        }
                        break;
                    case MapMode.Elevation:
                        {
                            KColor color = new KColor();
                            color.RGB = System.Drawing.Color.Cyan;

                            if (pLoc.H < 0)
                            {
                                color.RGB = System.Drawing.Color.Green;
                                color.Hue = 200 + 40 * pLoc.H / m_pWorld.m_fMaxDepth;
                            }
                            if (pLoc.H > 0)
                            {
                                color.RGB = System.Drawing.Color.Goldenrod;
                                color.Hue = 100 - 100 * pLoc.H / m_pWorld.m_fMaxHeight;
                            }

                            pResult = ConvertColor(color.RGB);
                        }
                        break;
                    case MapMode.TechLevel:
                        {
                            if ((pLoc.Owner as LandX).m_pProvince != null)
                            {
                                KColor background = new KColor();
                                background.RGB = System.Drawing.Color.ForestGreen;
                                background.Lightness = 1.0 - (double)((pLoc.Owner as LandX).m_pProvince.Owner as State).GetEffectiveTech() / 10;

                                //KColor foreground = new KColor();
                                //foreground.RGB = Color.ForestGreen;
                                //foreground.Lightness = 1.0 - (double)iUsedTechLevel / 10;
                                pResult = ConvertColor(background.RGB);
                            }
                        }
                        break;
                    case MapMode.PsiLevel:
                        {
                            if ((pLoc.Owner as LandX).m_pProvince != null)
                            {
                                KColor background = new KColor();
                                background.RGB = System.Drawing.Color.Orchid;
                                //color2.Saturation = (double)iUsedTechLevel / 8;
                                background.Lightness = 0.9 - (double)(pLoc.Owner as LandX).m_pProvince.m_pNation.m_iMagicLimit / 10;
                                //switch (ePrevalence)
                                //{
                                //    case Customs.Magic.Magic_Praised:
                                //        return new SolidBrush(background.RGB);
                                //    case Customs.Magic.Magic_Allowed:
                                //        return new HatchBrush(HatchStyle.DottedDiamond, Color.Gray, background.RGB);
                                //    case Customs.Magic.Magic_Feared:
                                //        return new HatchBrush(HatchStyle.DiagonalCross, Color.Red, background.RGB);
                                //    default:
                                //        throw new ArgumentException();
                                //} 

                                pResult = ConvertColor(background.RGB);
                            }
                        }
                        break;
                    case MapMode.Infrastructure:
                        {
                            if ((pLoc.Owner as LandX).m_pProvince != null)
                            {
                                KColor background = new KColor();
                                background.RGB = System.Drawing.Color.Red;
                                //color1.Saturation = (double)iBaseTechLevel / 8;
                                //background.Lightness = 0.9-(double)iInfrastructureLevel / 12;
                                background.Hue += ((pLoc.Owner as LandX).m_pProvince.Owner as State).m_iCultureLevel * 16;

                                //KColor foreground = new KColor();
                                //foreground.RGB = Color.Gray;

                                //// 0 - На преступников и диссидентов власти никакого внимания не обращают, спасение утопающих - дело рук самих утопающих.
                                //// 1 - Власти занимаются только самыми вопиющими преступлениями.
                                //// 2 - Есть законы, их надо соблюдать, кто не соблюдает - тот преступник, а вор должен сидеть в тюрьме.
                                //// 3 - Законы крайне строги, широко используется смертная казнь.
                                //// 4 - Все граждане, кроме правящей верхушки, попадают под презумпцию виновности.
                                //switch (iControl)
                                //{
                                //    case 0:
                                //        return new SolidBrush(background.RGB);
                                //    case 1:
                                //        return new HatchBrush(HatchStyle.Percent05, foreground.RGB, background.RGB);
                                //    case 2:
                                //        return new HatchBrush(HatchStyle.DottedDiamond, foreground.RGB, background.RGB);
                                //    case 3:
                                //        return new HatchBrush(HatchStyle.OutlinedDiamond, foreground.RGB, background.RGB);
                                //    case 4:
                                //        return new HatchBrush(HatchStyle.DiagonalCross, foreground.RGB, background.RGB);
                                //    default:
                                //        throw new ArgumentException();
                                //}

                                pResult = ConvertColor(background.RGB);
                            }
                        }
                        break;
                }
            }
            return pResult;
        }

        /// <summary>
        /// набор примитивов с дублированием вершин на границе разноцветных регионов, так чтобы образовывалась чёткая граница цвета
        /// </summary>
        private void BuildMapModeData(MapMode pMode,
                     LocationsGrid<LocationX>.BeginStepDelegate BeginStep,
                     LocationsGrid<LocationX>.ProgressStepDelegate ProgressStep)
        {
            if (pMode == MapMode.Sattelite)
                return;

            for (int i=0; i<m_cGeoLData.Length; i++)
                m_cGeoLData[i].m_pColor = GetMapModeColor((LocationX)m_cGeoLData[i].m_pOwner, pMode);

            int iPrimitivesCount = 0;
            for (int k=0; k<m_cGeoVData.Length; k++)
            {
                Vertex pVertex = m_cGeoVData[k].m_pOwner;

                List<Microsoft.Xna.Framework.Color> cColors = new List<Microsoft.Xna.Framework.Color>();
                for (int i = 0; i < m_cGeoVData[k].m_aLinked.Length; i++)
                {
                    Microsoft.Xna.Framework.Color pColor = m_cGeoVData[k].m_aLinked[i].m_pColor;

                    if (!cColors.Contains(pColor))
                    {
                        cColors.Add(pColor);
                        iPrimitivesCount++;
                    }
                }

            }

            MapModeData<VertexPositionColorNormal> pData = m_cMapModeData[pMode];

            // Create the verticies for our triangle
            pData.m_aVertices = new VertexPositionColorNormal[iPrimitivesCount + m_cGeoLData.Length];

            //Dictionary<long, Dictionary<Microsoft.Xna.Framework.Color, int>> cVertexes = new Dictionary<long, Dictionary<Microsoft.Xna.Framework.Color, int>>();
            Dictionary<Microsoft.Xna.Framework.Color, Dictionary<Vertex, int>> cVertexes = new Dictionary<Microsoft.Xna.Framework.Color,Dictionary<Vertex,int>>();
            Dictionary<LocationX, int> cLocations = new Dictionary<LocationX, int>();

            if (BeginStep != null)
                BeginStep(string.Format("Building {0} map data...", pMode.ToString()), (iPrimitivesCount + m_cGeoLData.Length + m_cGeoLData.Length) / 1000); 
            
            int iCounter = 0;
            int iUpdateCounter = 0;
            for (int k=0; k<m_cGeoVData.Length; k++)
            {
                Vertex pVertex = m_cGeoVData[k].m_pOwner;
                //Vector3 pPosition = GetPosition(pVertex, m_pWorld.m_pGrid.m_eShape, pVertex.m_fHeight > 0 ? m_fLandHeightMultiplier : m_fLandHeightMultiplier / 10);

                List<object> cColors = new List<object>();
                for (int i = 0; i < m_cGeoVData[k].m_aLinked.Length; i++)
                {
                    LocationX pLoc = (LocationX)m_cGeoVData[k].m_aLinked[i].m_pOwner;

                    Microsoft.Xna.Framework.Color pColor = m_cGeoVData[k].m_aLinked[i].m_pColor;

                    //if (((pLoc.Owner as LandX).Type.m_eType == LandType.Ocean || (pLoc.Owner as LandX).Type.m_eType == LandType.Coastral) &&
                    //    pVertex.m_fHeight > 0)
                    //    pColor = Microsoft.Xna.Framework.Color.Red;

                    if (!cColors.Contains(pColor))
                    {
                        Dictionary<Vertex, int> pColorDic;
                        if (!cVertexes.TryGetValue(pColor, out pColorDic))
                        {
                            pColorDic = new Dictionary<Vertex, int>();
                            cVertexes[pColor] = pColorDic;
                        }

                        cColors.Add(pColor);
                        VertexPositionColorNormal pVPCN = new VertexPositionColorNormal();
                        pVPCN.Position = m_cGeoVData[k].m_pPosition;
                        pVPCN.Normal = m_cGeoVData[k].m_pNormal;
                        pVPCN.Color = pColor;

                        pData.m_aVertices[iCounter] = pVPCN;
                        pColorDic[pVertex] = iCounter;

                        iCounter++;

                        if (iUpdateCounter++ > 1000)
                        {
                            iUpdateCounter = 0;
                            if (ProgressStep != null)
                                ProgressStep();
                        }
                    }
                }
            }

            m_cMapModeData[pMode].m_iTrianglesCount = 0;
            for (int i=0; i<m_cGeoLData.Length; i++)
            {
                LocationX pLoc = (LocationX)m_cGeoLData[i].m_pOwner;

                VertexPositionColorNormal pVPCN = new VertexPositionColorNormal();
                pVPCN.Position = m_cGeoLData[i].m_pPosition;//GetPosition(pLoc, m_pWorld.m_pGrid.m_eShape, pLoc.m_fHeight > 0 ? m_fLandHeightMultiplier : m_fLandHeightMultiplier / 10);
                pVPCN.Normal = m_cGeoLData[i].m_pNormal;
                pVPCN.Color = m_cGeoLData[i].m_pColor;

                pData.m_aVertices[iCounter] = pVPCN;
                cLocations[pLoc] = iCounter;

                pData.m_iTrianglesCount += pLoc.m_aBorderWith.Length * 4;

                iCounter++;

                if (iUpdateCounter++ > 1000)
                {
                    iUpdateCounter = 0;
                    if (ProgressStep != null)
                        ProgressStep();
                }
            }

            // Create the indices used for each triangle
            pData.m_aIndices = new int[m_cMapModeData[pMode].m_iTrianglesCount * 3];

            iCounter = 0;

            for (int i = 0; i < m_cGeoLData.Length; i++)
            {
                LocationX pLoc = (LocationX)m_cGeoLData[i].m_pOwner;

                if (iUpdateCounter++ > 1000)
                {
                    iUpdateCounter = 0;
                    if (ProgressStep != null)
                        ProgressStep();
                }
                
                Microsoft.Xna.Framework.Color pColor = m_cGeoLData[i].m_pColor;
                Dictionary<Vertex, int> pColorDic = cVertexes[pColor];

                Line pLine = pLoc.m_pFirstLine;
                //последовательно перебирает все связанные линии, пока круг не замкнётся.
                do
                {
                    int iPoint1 = pColorDic[pLine.m_pPoint1];
                    int iPoint2 = pColorDic[pLine.m_pPoint2];
                    int iMidPoint = pColorDic[pLine.m_pMidPoint];
                    int iInnerPoint = pColorDic[pLine.m_pInnerPoint];
                    int iNextInnerPoint = pColorDic[pLine.m_pNext.m_pInnerPoint];

                    int iLoc = cLocations[pLoc];

                    pData.m_aIndices[iCounter++] = iInnerPoint;
                    pData.m_aIndices[iCounter++] = iMidPoint;
                    pData.m_aIndices[iCounter++] = iPoint1;

                    pData.m_aIndices[iCounter++] = iInnerPoint;
                    pData.m_aIndices[iCounter++] = iPoint2;
                    pData.m_aIndices[iCounter++] = iMidPoint;

                    pData.m_aIndices[iCounter++] = iInnerPoint;
                    pData.m_aIndices[iCounter++] = iNextInnerPoint;
                    pData.m_aIndices[iCounter++] = iPoint2;

                    pData.m_aIndices[iCounter++] = iInnerPoint;
                    pData.m_aIndices[iCounter++] = iLoc;
                    pData.m_aIndices[iCounter++] = iNextInnerPoint;

                    pLine = pLine.m_pNext;
                }
                while (pLine != pLoc.m_pFirstLine);
            }
        }

        /// <summary>
        /// Привязка цветов для конкретных рас
        /// </summary>
        private Dictionary<Nation, Microsoft.Xna.Framework.Color> m_cNationColorsID = new Dictionary<Nation, Microsoft.Xna.Framework.Color>();
        //private Dictionary<Nation, Microsoft.Xna.Framework.Color> m_cAncientNationColorsID = new Dictionary<Nation, Microsoft.Xna.Framework.Color>();
        //private Dictionary<Nation, Microsoft.Xna.Framework.Color> m_cHegemonNationColorsID = new Dictionary<Nation, Microsoft.Xna.Framework.Color>();

        private void FillNationColors()
        {
            m_cNationColorsID.Clear();
            List<int> cUsedColors = new List<int>();

            //if (m_pWorld.m_aLocalRaces.Length > m_aRaceColorsTemplate.Length)
            //    throw new Exception("Can't draw more then " + m_aRaceColorsTemplate.Length.ToString() + " races!");

            Dictionary<Language, int> cLanguages = new Dictionary<Language, int>();
            Dictionary<Language, int> cLanguageCounter = new Dictionary<Language, int>();
            Dictionary<Language, int> cLanguageIndex = new Dictionary<Language, int>();
            int iCount = 0;
            foreach (Nation pNation in m_pWorld.m_aLocalNations)
            {
                int ik = 0;
                if (!cLanguages.TryGetValue(pNation.m_pRace.m_pLanguage, out ik))
                {
                    cLanguageCounter[pNation.m_pRace.m_pLanguage] = 1;
                    cLanguageIndex[pNation.m_pRace.m_pLanguage] = iCount++;
                }
                cLanguages[pNation.m_pRace.m_pLanguage] = ik + 1;
            }

            //Dictionary<Language, Dictionary<Race, KColor>> cColors = new Dictionary<Language, Dictionary<Race, KColor>>();
            foreach (Nation pNation in m_pWorld.m_aLocalNations)
            {
                KColor color = new KColor();
                color.Hue = 360 * cLanguageIndex[pNation.m_pRace.m_pLanguage] / cLanguages.Count;
                float fK = ((float)cLanguageCounter[pNation.m_pRace.m_pLanguage]) / cLanguages[pNation.m_pRace.m_pLanguage];
                float fF = fK * Rnd.Get(1f);
                color.Lightness = 0.2 + 0.7 * fK;
                color.Saturation = 0.2 + 0.7 * fK;

                m_cNationColorsID[pNation] = ConvertColor(color.RGB);
                //m_cAncientNationColorsID[pNation] = new HatchBrush(HatchStyle.DottedDiamond, System.Drawing.Color.Black, color.RGB);
                //m_cHegemonNationColorsID[pNation] = new HatchBrush(HatchStyle.LargeConfetti, System.Drawing.Color.Black, color.RGB);
                cLanguageCounter[pNation.m_pRace.m_pLanguage]++;
            }
        }

        /// <summary>
        /// для всех вершин в списке вычисляем нормаль как нормализованный вектор суммы нормалей прилегающих граней
        /// </summary>
        private static void NormalizeTextureWeights(MapModeData<VertexMultitextured> pData)
        {
            for (int i = 0; i < pData.m_aVertices.Length; i++)
            {
                float fD = (float)Math.Sqrt(pData.m_aVertices[i].TexWeights.LengthSquared() + pData.m_aVertices[i].TexWeights2.LengthSquared());
                pData.m_aVertices[i].TexWeights /= fD;
                pData.m_aVertices[i].TexWeights2 /= fD;
            }
        }

        /// <summary>
        /// Режим отрисовки карты - физическая карта, карта влажности, этническая карта...
        /// </summary>
        private MapMode m_eMode = MapMode.Sattelite;

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
                    CopyToBuffers();
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
        /// Континент, над которым находится указатель мыши.
        /// </summary>
        private ContinentX m_pFocusedContinent = null;

        private LandMass<LandX> m_pFocusedLandMass = null;

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

        public void SelectState()
        {
            if (m_pFocusedState != null)
                SelectedState = m_pFocusedState;
        }

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

        private class GeoData
        {
            public Vertex m_pOwner;

            public Vector3 m_pPosition = new Vector3(0);
            public Vector3 m_pNormal = new Vector3(0);
            
            public Vector4 TexWeights = new Vector4(0);
            public Vector4 TexWeights2 = new Vector4(0);
            public Vector4 TextureCoordinate = new Vector4(0);
            
            public Microsoft.Xna.Framework.Color m_pColor = Microsoft.Xna.Framework.Color.Black;

            public GeoData(Vertex pOwner, WorldShape eShape, float fMultiplier)
            {
                m_pOwner = pOwner;
                m_pPosition = GetPosition(pOwner, eShape, fMultiplier);
            }

            public GeoData(Vertex pOwner, WorldShape eShape, float fHeight, float fMultiplier)
            {
                m_pOwner = pOwner;
                m_pPosition = GetPosition(pOwner, eShape, fHeight, fMultiplier);
            }
        }

        private class GeoData2 : GeoData
        {
            public GeoData[] m_aLinked;

            public GeoData2(Vertex pOwner, WorldShape eShape, float fMultiplier)
                : base(pOwner, eShape, fMultiplier)
            {
            }

            public GeoData2(Vertex pOwner, WorldShape eShape, float fHeight, float fMultiplier)
                : base(pOwner, eShape, fHeight, fMultiplier)
            {
            }
        }

        private GeoData2[] m_cGeoVData;
        private GeoData[] m_cGeoLData;

        private class RoadData
        {
            public RoadType m_eType = RoadType.None;
            public GeoData[] m_aShape;

            public VertexMultitextured[] m_aVertices = new VertexMultitextured[0];
            public int[] m_aIndices = new int[0];

            private Vector4 GetTextureCoordinate(Vector3 pPos, WorldShape eShape, int RX, int RY, float fTextureScale)
            {
                Vector4 pResult = new Vector4();
                if (eShape == WorldShape.Planet)
                {
                    float fRo = (float)Math.Atan2(pPos.Z, Math.Sqrt(pPos.X * pPos.X + pPos.Y * pPos.Y));
                    float fPhi = (float)Math.Atan2(pPos.X, pPos.Y);

                    float fPhi4 = (float)(fPhi % (Math.PI / 2));

                    //чтобы был гладкий переход, нужно чтобы в RX/2 укладывалось целое количество тайлов текстуры!
                    //или нужно вводить нормирующий коэффициент, чтобы на четверть экватора получалась координата текстуры кратная размеру тайла.
                    if (fRo < -Math.PI / 4 && fRo > -Math.PI * 3 / 4)
                    {
                        float fH = (float)(-Math.PI / 4 - fRo);
                        float fPhiRel = (float)(fH - fPhi4 * (4 * fH - Math.PI) / Math.PI);

                        pResult.X = fPhiRel * RX / ((float)Math.PI * fTextureScale);
                        pResult.Y = fRo * 2 * RY / ((float)Math.PI * fTextureScale);
                    }
                    else if (fRo > Math.PI / 4 && fRo < Math.PI * 3 / 4)
                    {
                        float fH = (float)(fRo - Math.PI / 4);
                        float fPhiRel = (float)(fH - fPhi4 * (4 * fH - Math.PI) / Math.PI);

                        pResult.X = fPhiRel * RX / ((float)Math.PI * fTextureScale);
                        pResult.Y = fRo * 2 * RY / ((float)Math.PI * fTextureScale);
                    }
                    else
                    {
                        pResult.X = fPhi4 * RX / ((float)Math.PI * fTextureScale);
                        pResult.Y = fRo * 2 * RY / ((float)Math.PI * fTextureScale);
                    }
                }
                else
                {
                    if (eShape == WorldShape.Ringworld)
                    {
                        float fPhi = (float)Math.Atan2(pPos.X, pPos.Y);

                        pResult.X = fPhi * RX / ((float)Math.PI * fTextureScale);
                    }
                    else
                    {
                        pResult.X = pPos.X * 1000 / fTextureScale;
                    }
                    pResult.Y = pPos.Z * 1000 / fTextureScale;
                } 

                return pResult;
            }

            public void Build3D(WorldShape eShape, int RX, int RY, float fTextureScale)
            {
                float fScale = 0.1f;
                switch (m_eType)
                {
                    case RoadType.LandRoad1:
                        fScale = 0.2f;
                        break;
                    case RoadType.LandRoad2:
                        fScale = 0.3f;
                        break;
                    case RoadType.LandRoad3:
                        fScale = 0.4f;
                        break;
                }

                Vector3? pCrossOld = null;

                m_aVertices = new VertexMultitextured[m_aShape.Length * 5 + 2];
                m_aIndices = new int[m_aShape.Length * 8 * 3];

                int iCounterV = 1;
                int iCounterI = 0;

                ///TODO: надо делать дорогу не в 3 вертекса в ширину, а хотя бы в 4, а лучше в 5... Чтобы форма была более выпуклая

                for (int i = 0; i < m_aShape.Length; i++)
                {
                    Vector3 pUplift;
                    if (eShape == WorldShape.Planet)
                        pUplift = Vector3.Normalize(m_aShape[i].m_pPosition);
                    else if (eShape == WorldShape.Ringworld)
                        pUplift = Vector3.Transform(-Vector3.Normalize(m_aShape[i].m_pPosition), Matrix.CreateScale(1, 0, 1));
                    else
                        pUplift = Vector3.Up;

                    //pUplift = (pUplift + m_aShape[i].m_pNormal) / 2;
                    pUplift *= fScale / 75;

                    //Vector3 pUplift = m_aShape[i].m_pNormal * fScale / 100;

                    Vector3 pCross;
                    if (i == m_aShape.Length - 1)
                        pCross = (Vector3)pCrossOld;
                    else
                        pCross = Vector3.Normalize(Vector3.Cross(m_aShape[i + 1].m_pPosition - m_aShape[i].m_pPosition, pUplift)) / 5;
                        //pCross = Vector3.Normalize(Vector3.Cross(m_aShape[i + 1].m_pPosition - m_aShape[i].m_pPosition, m_aShape[i].m_pNormal)) / 5;

                    Vector3 pCrossAverage = pCross * fScale;
                    if (pCrossOld != null)
                        pCrossAverage = ((Vector3)pCrossOld + pCross) * fScale / 2;

                    pCrossOld = pCross;

                    VertexMultitextured pLeft = new VertexMultitextured();
                    pLeft.Position = m_aShape[i].m_pPosition + pCrossAverage*2 - pUplift*30;
                    pLeft.Normal = m_aShape[i].m_pNormal;
                    //pLeft.Color = Microsoft.Xna.Framework.Color.Tan;
                    pLeft.TexWeights = m_aShape[i].TexWeights;
                    pLeft.TexWeights2 = m_aShape[i].TexWeights2;
                    pLeft.TextureCoordinate = GetTextureCoordinate(pLeft.Position, eShape, RX, RY, fTextureScale);

                    VertexMultitextured pLeftInner = new VertexMultitextured();
                    pLeftInner.Position = m_aShape[i].m_pPosition + pCrossAverage/2 + pUplift*0.5f;
                    pLeftInner.Normal = m_aShape[i].m_pNormal;
                    //pLeft.Color = Microsoft.Xna.Framework.Color.Tan;
                    pLeftInner.TexWeights = m_aShape[i].TexWeights;
                    pLeftInner.TexWeights2 = m_aShape[i].TexWeights2;
                    pLeftInner.TextureCoordinate = GetTextureCoordinate(pLeftInner.Position, eShape, RX, RY, fTextureScale);
                    //if (i == 0 || i == m_aShape.Length - 1)
                    //    pLeftInner.TexWeights2.Y = fScale;
                    //else
                    //    pLeftInner.TexWeights2.Y = fScale /2 + Rnd.Get(fScale/2);//0.5f + Rnd.Get(1f);// fScale + Rnd.Get(fScale * 2);

                    VertexMultitextured pRight = new VertexMultitextured();
                    pRight.Position = m_aShape[i].m_pPosition - pCrossAverage*2 - pUplift * 30;
                    pRight.Normal = m_aShape[i].m_pNormal;
                    //pRight.Color = Microsoft.Xna.Framework.Color.Tan;
                    pRight.TexWeights = m_aShape[i].TexWeights;
                    pRight.TexWeights2 = m_aShape[i].TexWeights2;
                    pRight.TextureCoordinate = GetTextureCoordinate(pRight.Position, eShape, RX, RY, fTextureScale);

                    VertexMultitextured pRightInner = new VertexMultitextured();
                    pRightInner.Position = m_aShape[i].m_pPosition - pCrossAverage / 2 + pUplift * 0.5f;
                    pRightInner.Normal = m_aShape[i].m_pNormal;
                    //pLeft.Color = Microsoft.Xna.Framework.Color.Tan;
                    pRightInner.TexWeights = m_aShape[i].TexWeights;
                    pRightInner.TexWeights2 = m_aShape[i].TexWeights2;
                    pRightInner.TextureCoordinate = GetTextureCoordinate(pRightInner.Position, eShape, RX, RY, fTextureScale);
                    //if (i == 0 || i == m_aShape.Length - 1)
                    //    pRightInner.TexWeights2.Y = fScale;
                    //else
                    //    pRightInner.TexWeights2.Y = fScale /2 + Rnd.Get(fScale/2);//0.5f + Rnd.Get(1f);// fScale + Rnd.Get(fScale * 2);

                    VertexMultitextured pCenter = new VertexMultitextured();
                    pCenter.Position = m_aShape[i].m_pPosition + pUplift;
                    pCenter.Normal = m_aShape[i].m_pNormal;
                    //pCenter.Color = Microsoft.Xna.Framework.Color.Tan;
                    pCenter.TextureCoordinate = m_aShape[i].TextureCoordinate;
                    pCenter.TexWeights = m_aShape[i].TexWeights;
                    pCenter.TexWeights2 = m_aShape[i].TexWeights2;
                    if (i == 0 || i == m_aShape.Length - 1)
                        pCenter.TexWeights2.Y = fScale * 8;
                    else
                    {
                        //pCenter.TexWeights = new Vector4(0);
                        //pCenter.TexWeights2 = new Vector4(0);
                        pCenter.TexWeights2.Y = fScale * 6 + Rnd.Get(fScale*4);//0.5f + Rnd.Get(1f);// fScale + Rnd.Get(fScale * 2);
                    }
                    //pCenter.TexWeights2.Y = 1;

                    m_aVertices[iCounterV++] = pLeft;
                    m_aVertices[iCounterV++] = pLeftInner;
                    m_aVertices[iCounterV++] = pCenter;
                    m_aVertices[iCounterV++] = pRightInner;
                    m_aVertices[iCounterV++] = pRight;
                    
                    if (i > 0)
                    {
                        m_aIndices[iCounterI++] = iCounterV - 9;
                        m_aIndices[iCounterI++] = iCounterV - 5;
                        m_aIndices[iCounterI++] = iCounterV - 10;

                        m_aIndices[iCounterI++] = iCounterV - 9;
                        m_aIndices[iCounterI++] = iCounterV - 4;
                        m_aIndices[iCounterI++] = iCounterV - 5;

                        m_aIndices[iCounterI++] = iCounterV - 9;
                        m_aIndices[iCounterI++] = iCounterV - 3;
                        m_aIndices[iCounterI++] = iCounterV - 4;

                        m_aIndices[iCounterI++] = iCounterV - 9;
                        m_aIndices[iCounterI++] = iCounterV - 8;
                        m_aIndices[iCounterI++] = iCounterV - 3; 
                        
                        
                        m_aIndices[iCounterI++] = iCounterV - 7;
                        m_aIndices[iCounterI++] = iCounterV - 3;
                        m_aIndices[iCounterI++] = iCounterV - 8;

                        m_aIndices[iCounterI++] = iCounterV - 7;
                        m_aIndices[iCounterI++] = iCounterV - 2;
                        m_aIndices[iCounterI++] = iCounterV - 3;

                        m_aIndices[iCounterI++] = iCounterV - 7;
                        m_aIndices[iCounterI++] = iCounterV - 1;
                        m_aIndices[iCounterI++] = iCounterV - 2;

                        m_aIndices[iCounterI++] = iCounterV - 7;
                        m_aIndices[iCounterI++] = iCounterV - 6;
                        m_aIndices[iCounterI++] = iCounterV - 1;
                    }


                    if (i == 1)
                    {
                        VertexMultitextured pStart = new VertexMultitextured();
                        Vector3 pBackward = m_aShape[0].m_pPosition - m_aShape[1].m_pPosition;
                        pBackward.Normalize();
                        pStart.Position = m_aShape[0].m_pPosition + pBackward * pCrossAverage.Length() - pUplift*5;
                        pStart.Normal = m_aShape[i].m_pNormal;
                        //pStart.Color = Microsoft.Xna.Framework.Color.Tan;
                        pStart.TexWeights = m_aShape[i].TexWeights;
                        pStart.TexWeights2 = m_aShape[i].TexWeights2;
                        pStart.TextureCoordinate = GetTextureCoordinate(pStart.Position, eShape, RX, RY, fTextureScale); 

                        m_aVertices[0] = pStart;

                        m_aIndices[iCounterI++] = 0;
                        m_aIndices[iCounterI++] = 2;
                        m_aIndices[iCounterI++] = 1;

                        m_aIndices[iCounterI++] = 0;
                        m_aIndices[iCounterI++] = 3;
                        m_aIndices[iCounterI++] = 2;

                        m_aIndices[iCounterI++] = 0;
                        m_aIndices[iCounterI++] = 4;
                        m_aIndices[iCounterI++] = 3;

                        m_aIndices[iCounterI++] = 0;
                        m_aIndices[iCounterI++] = 5;
                        m_aIndices[iCounterI++] = 4;
                    }

                    if (i == m_aShape.Length - 1)
                    {
                        VertexMultitextured pFinish = new VertexMultitextured();
                        Vector3 pForward = m_aShape[i].m_pPosition - m_aShape[i - 1].m_pPosition;
                        pForward.Normalize();
                        pFinish.Position = m_aShape[i].m_pPosition + pForward * pCrossAverage.Length() - pUplift *5;
                        pFinish.Normal = m_aShape[i].m_pNormal;
                        //pFinish.Color = Microsoft.Xna.Framework.Color.Tan;
                        pFinish.TexWeights = m_aShape[i].TexWeights;
                        pFinish.TexWeights2 = m_aShape[i].TexWeights2;
                        pFinish.TextureCoordinate = GetTextureCoordinate(pFinish.Position, eShape, RX, RY, fTextureScale); 

                        m_aVertices[m_aVertices.Length - 1] = pFinish;

                        m_aIndices[iCounterI++] = iCounterV;
                        m_aIndices[iCounterI++] = iCounterV - 5;
                        m_aIndices[iCounterI++] = iCounterV - 4;

                        m_aIndices[iCounterI++] = iCounterV;
                        m_aIndices[iCounterI++] = iCounterV - 4;
                        m_aIndices[iCounterI++] = iCounterV - 3;

                        m_aIndices[iCounterI++] = iCounterV;
                        m_aIndices[iCounterI++] = iCounterV - 3;
                        m_aIndices[iCounterI++] = iCounterV - 2;

                        m_aIndices[iCounterI++] = iCounterV;
                        m_aIndices[iCounterI++] = iCounterV - 2;
                        m_aIndices[iCounterI++] = iCounterV - 1;
                    }
                }
            }

            /// <summary>
            /// для всех вершин в списке вычисляем нормаль как нормализованный вектор суммы нормалей прилегающих граней
            /// </summary>
            public void NormalizeTextureWeights()
            {
                for (int i = 0; i < m_aVertices.Length; i++)
                {
                    float fD = (float)Math.Sqrt(m_aVertices[i].TexWeights.LengthSquared() + m_aVertices[i].TexWeights2.LengthSquared());
                    m_aVertices[i].TexWeights /= fD;
                    m_aVertices[i].TexWeights2 /= fD;
                }
            }
        }

        private RoadData[] m_aRoads;

        /// <summary>
        /// Checks whether a ray intersects a model. This method needs to access
        /// the model vertex data, so the model must have been built using the
        /// custom TrianglePickingProcessor provided as part of this sample.
        /// Returns the distance along the ray to the point of intersection, or null
        /// if there is no intersection.
        /// </summary>
        public float? RayIntersectsLandscape(CullMode culling, Ray ray,
                                         out LocationX pLoc)
        {
            pLoc = null;

            if (m_pLand.m_aIndices == null || m_pLand.m_aIndices.Length < 3)
                return null;

            float? closestIntersection = null;

            for (int i = 0; i < m_pLand.m_aIndices.Length; i += 3)
            {
                float? intersection;

                RayIntersectsTriangle(ref culling, ref ray,
                                        ref m_pLand.m_aVertices[m_pLand.m_aIndices[i]].Position,
                                        ref m_pLand.m_aVertices[m_pLand.m_aIndices[i + 1]].Position,
                                        ref m_pLand.m_aVertices[m_pLand.m_aIndices[i + 2]].Position,
                                        out intersection);

                if (intersection != null)
                {
                    if ((closestIntersection == null) ||
                        (intersection < closestIntersection))
                    {
                        closestIntersection = intersection;

                        pLoc = m_aLocationReferences[i / 3];
                    }
                }
            }

            return closestIntersection;
        }

        private LocationX[] m_aLocationReferences;

        /// <summary>
        /// Привязать карту к миру.
        /// Строим контуры всего, что придётся рисовать в ОРИГИНАЛЬНЫХ координатах
        /// и раскидываем их по квадрантам
        /// </summary>
        /// <param name="pWorld">мир</param>
        public void Assign(World pWorld,
                     LocationsGrid<LocationX>.BeginStepDelegate BeginStep,
                     LocationsGrid<LocationX>.ProgressStepDelegate ProgressStep)
        {
            m_pSelectedState = null;
            m_pFocusedLocation = null;
            m_pFocusedLand = null;
            m_pFocusedLandMass = null;
            m_pFocusedContinent = null;
            m_pFocusedProvince = null;
            m_pFocusedState = null;
            m_pWorld = pWorld;
            
            FillNationColors();

            if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                m_fLandHeightMultiplier = 7f / 60;//m_pWorld.m_fMaxHeight;
            else if (m_pWorld.m_pGrid.m_eShape == WorldShape.Planet)
                m_fLandHeightMultiplier = 3.5f / 60;//m_pWorld.m_fMaxHeight;
            else
                m_fLandHeightMultiplier = 3.5f / 60;// m_pWorld.m_fMaxHeight;

            //m_fLandHeightMultiplier = 0;
            BuildGeometry(BeginStep, ProgressStep);

            for (int i = 0; i < m_aRoads.Length; i++)
            {
                m_aRoads[i].Build3D(m_pWorld.m_pGrid.m_eShape, m_pWorld.m_pGrid.RX, m_pWorld.m_pGrid.RY, m_fTextureScale);
                m_aRoads[i].NormalizeTextureWeights();
            }

            BuildLayers(out m_pLayers, BeginStep, ProgressStep);

            BuildLand(true, out m_pUnderwater, out m_aLocationReferences, out m_aTrees, out m_aSettlements, BeginStep, ProgressStep);
            BuildLand(false, out m_pLand, out m_aLocationReferences, out m_aTrees, out m_aSettlements, BeginStep, ProgressStep);
            BuildWater();

            //FillPrimitivesFake();
            NormalizeTextureWeights(m_pLand);
            NormalizeTextureWeights(m_pUnderwater);

            //if (BeginStep != null)
            //{
            //    BeginStep("Building reference map data...", Enum.GetValues(typeof(MapMode)).Length);
            //}
            foreach (MapMode pMode in Enum.GetValues(typeof(MapMode)))
            {
                BuildMapModeData(pMode, BeginStep, ProgressStep);

                //if (ProgressStep != null)
                //    ProgressStep();
            }

            if (GraphicsDevice != null)
            {
                //pEffectFogNear.SetValue(m_pWorld.m_pGrid.RY / 1000);
                //pEffectFogFar.SetValue(2 * m_pWorld.m_pGrid.RY / 1000);

                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                {
                    m_pCamera = new RingworldCamera(m_pWorld.m_pGrid.RX / ((float)Math.PI * 1000), GraphicsDevice);
                    m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["LandRingworld"];
                    pEffectDirectionalLightDirection.SetValue(new Vector3(0, 0, -150));
                }
                else if (m_pWorld.m_pGrid.m_eShape == WorldShape.Planet)
                {
                    m_pCamera = new PlanetCamera(m_pWorld.m_pGrid.RX / ((float)Math.PI * 1000), GraphicsDevice);
                    m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Land"];
                    pEffectDirectionalLightDirection.SetValue(Vector3.Normalize(new Vector3(1, -1, -1)));
                }
                else
                {
                    m_pCamera = new PlainCamera(GraphicsDevice);
                    m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Land"];
                    pEffectDirectionalLightDirection.SetValue(Vector3.Normalize(new Vector3(1, -1, -1)));
                }
            }

            CopyToBuffers();
        }

        private Microsoft.Xna.Framework.Color eSkyColor = Microsoft.Xna.Framework.Color.Lavender;

        public Camera m_pCamera = null;

        public static ContentManager LibContent;

        EffectParameter pEffectWorld;
        EffectParameter pEffectView;
        EffectParameter pEffectProjection;
        EffectParameter pEffectAmbientLightColor;
        EffectParameter pEffectAmbientLightIntensity;
        EffectParameter pEffectDirectionalLightDirection;
        EffectParameter pEffectDirectionalLightColor;
        EffectParameter pEffectDirectionalLightIntensity;
        EffectParameter pEffectCameraPosition;
        EffectParameter pEffectSpecularColor;
        EffectParameter pEffectFogColor;
        EffectParameter pEffectFogDensity;
        EffectParameter pEffectBlendDistance;
        EffectParameter pEffectBlendWidth; 

        EffectParameter pEffectTexture0;
        EffectParameter pEffectTexture1;
        EffectParameter pEffectTexture2;
        EffectParameter pEffectTexture3;
        EffectParameter pEffectTexture4;
        EffectParameter pEffectTexture5;
        EffectParameter pEffectTexture6;
        EffectParameter pEffectTexture7;

        EffectParameter pEffectTextureModel;

        Texture2D grassTexture;
        Texture2D sandTexture;
        Texture2D rockTexture;
        Texture2D snowTexture;
        Texture2D forestTexture;
        Texture2D roadTexture;
        Texture2D swampTexture;
        Texture2D lavaTexture;

        Texture2D treeTexture;

        Model[] treeModel = new Model[13];
        Model[] palmModel = new Model[4];
        Model[] pineModel = new Model[4];

        Dictionary<SettlementSize, Dictionary<int, Model>> m_cSettlementModels = new Dictionary<SettlementSize,Dictionary<int,Model>>();
        Dictionary<SettlementSize, Dictionary<int, Texture2D>> m_cSettlementTextures = new Dictionary<SettlementSize,Dictionary<int,Texture2D>>();

        private void BindEffectParameters()
        {
            pEffectWorld = m_pMyEffect.Parameters["World"];
            pEffectView = m_pMyEffect.Parameters["View"];
            pEffectProjection = m_pMyEffect.Parameters["Projection"];

            pEffectCameraPosition = m_pMyEffect.Parameters["CameraPosition"];

            pEffectAmbientLightColor = m_pMyEffect.Parameters["AmbientLightColor"];
            pEffectAmbientLightIntensity = m_pMyEffect.Parameters["AmbientLightIntensity"];

            pEffectDirectionalLightDirection = m_pMyEffect.Parameters["DirectionalLightDirection"];
            pEffectDirectionalLightColor = m_pMyEffect.Parameters["DirectionalLightColor"];
            pEffectDirectionalLightIntensity = m_pMyEffect.Parameters["DirectionalLightIntensity"];

            pEffectSpecularColor = m_pMyEffect.Parameters["SpecularColor"];

            pEffectFogColor = m_pMyEffect.Parameters["FogColor"];
            pEffectFogDensity = m_pMyEffect.Parameters["FogDensity"];

            pEffectBlendDistance = m_pMyEffect.Parameters["BlendDistance"];
            pEffectBlendWidth = m_pMyEffect.Parameters["BlendWidth"];

            pEffectTexture0 = m_pMyEffect.Parameters["xTexture0"];
            pEffectTexture1 = m_pMyEffect.Parameters["xTexture1"];
            pEffectTexture2 = m_pMyEffect.Parameters["xTexture2"];
            pEffectTexture3 = m_pMyEffect.Parameters["xTexture3"];
            pEffectTexture4 = m_pMyEffect.Parameters["xTexture4"];
            pEffectTexture5 = m_pMyEffect.Parameters["xTexture5"];
            pEffectTexture6 = m_pMyEffect.Parameters["xTexture6"];
            pEffectTexture7 = m_pMyEffect.Parameters["xTexture7"];
            pEffectTextureModel = m_pMyEffect.Parameters["xTextureModel"];
        }

        private void LoadTerrainTextures()
        {
            grassTexture = LibContent.Load<Texture2D>("content/dds/1-plain");
            sandTexture = LibContent.Load<Texture2D>("content/dds/1-desert");
            rockTexture = LibContent.Load<Texture2D>("content/dds/rock");
            snowTexture = LibContent.Load<Texture2D>("content/dds/snow");
            forestTexture = LibContent.Load<Texture2D>("content/dds/grass");
            roadTexture = LibContent.Load<Texture2D>("content/dds/sand");
            swampTexture = LibContent.Load<Texture2D>("content/dds/river");
            lavaTexture = LibContent.Load<Texture2D>("content/dds/2-lava");
        }

        private void LoadTrees()
        {
            treeTexture = LibContent.Load<Texture2D>("content/dds/trees");

            treeModel[0] = LoadModel("content/fbx/tree1");
            treeModel[1] = LoadModel("content/fbx/tree2");
            treeModel[2] = LoadModel("content/fbx/tree3");
            treeModel[3] = LoadModel("content/fbx/tree4");
            treeModel[4] = LoadModel("content/fbx/tree6");
            treeModel[5] = LoadModel("content/fbx/tree7");
            treeModel[6] = LoadModel("content/fbx/tree8");
            treeModel[7] = LoadModel("content/fbx/tree9");
            treeModel[8] = LoadModel("content/fbx/tree10");
            treeModel[9] = LoadModel("content/fbx/tree11");
            treeModel[10] = LoadModel("content/fbx/tree12");
            treeModel[11] = LoadModel("content/fbx/tree15");
            treeModel[12] = LoadModel("content/fbx/tree16");

            palmModel[0] = LoadModel("content/fbx/palm1");
            palmModel[1] = LoadModel("content/fbx/palm2");
            palmModel[2] = LoadModel("content/fbx/palm3");
            palmModel[3] = LoadModel("content/fbx/palm4");

            pineModel[0] = LoadModel("content/fbx/tree5");
            pineModel[1] = LoadModel("content/fbx/tree13");
            pineModel[2] = LoadModel("content/fbx/tree14");
            pineModel[3] = LoadModel("content/fbx/tree16"); 

            foreach (Model pModel in treeModel)
                m_aTrees[pModel] = new TreeModel[0];

            foreach (Model pModel in palmModel)
                m_aTrees[pModel] = new TreeModel[0];

            foreach (Model pModel in pineModel)
                m_aTrees[pModel] = new TreeModel[0];
        }

        private void LoadSettlements()
        {
            foreach (SettlementSize eSize in Enum.GetValues(typeof(SettlementSize)))
            {
                m_cSettlementModels[eSize] = new Dictionary<int, Model>();
                m_cSettlementTextures[eSize] = new Dictionary<int, Texture2D>();
            }

            Texture2D pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T0");
            m_cSettlementModels[SettlementSize.Hamlet][0] = LoadModel("content/fbx/hamlet_T0");
            m_cSettlementTextures[SettlementSize.Hamlet][0] = pTexture;
            m_cSettlementModels[SettlementSize.Village][0] = LoadModel("content/fbx/village_T0");
            m_cSettlementTextures[SettlementSize.Village][0] = pTexture;
            m_cSettlementModels[SettlementSize.Town][0] = LoadModel("content/fbx/village_T0");
            m_cSettlementTextures[SettlementSize.Town][0] = pTexture;
            m_cSettlementModels[SettlementSize.City][0] = LoadModel("content/fbx/village_T0");
            m_cSettlementTextures[SettlementSize.City][0] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][0] = LoadModel("content/fbx/village_T0");
            m_cSettlementTextures[SettlementSize.Capital][0] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][0] = LoadModel("content/fbx/fort_T1");
            m_cSettlementTextures[SettlementSize.Fort][0] = LibContent.Load<Texture2D>("content/dds/Fort_T1");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T1");
            m_cSettlementModels[SettlementSize.Hamlet][1] = LoadModel("content/fbx/hamlet_T1");
            m_cSettlementTextures[SettlementSize.Hamlet][1] = pTexture;
            m_cSettlementModels[SettlementSize.Village][1] = LoadModel("content/fbx/village_T1");
            m_cSettlementTextures[SettlementSize.Village][1] = pTexture;
            m_cSettlementModels[SettlementSize.Town][1] = LoadModel("content/fbx/town_T1");
            m_cSettlementTextures[SettlementSize.Town][1] = pTexture;
            m_cSettlementModels[SettlementSize.City][1] = LoadModel("content/fbx/city_T1");
            m_cSettlementTextures[SettlementSize.City][1] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][1] = LoadModel("content/fbx/city_T1");
            m_cSettlementTextures[SettlementSize.Capital][1] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][1] = LoadModel("content/fbx/fort_T1");
            m_cSettlementTextures[SettlementSize.Fort][1] = LibContent.Load<Texture2D>("content/dds/Fort_T1");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T2_small");
            m_cSettlementModels[SettlementSize.Hamlet][2] = LoadModel("content/fbx/hamlet_T2");
            m_cSettlementTextures[SettlementSize.Hamlet][2] = pTexture;
            m_cSettlementModels[SettlementSize.Village][2] = LoadModel("content/fbx/village_T2");
            m_cSettlementTextures[SettlementSize.Village][2] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T2_big");
            m_cSettlementModels[SettlementSize.Town][2] = LoadModel("content/fbx/town_T2");
            m_cSettlementTextures[SettlementSize.Town][2] = pTexture;
            m_cSettlementModels[SettlementSize.City][2] = LoadModel("content/fbx/city_T2");
            m_cSettlementTextures[SettlementSize.City][2] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][2] = LoadModel("content/fbx/city_T2");
            m_cSettlementTextures[SettlementSize.Capital][2] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][2] = LoadModel("content/fbx/fort_T2");
            m_cSettlementTextures[SettlementSize.Fort][2] = LibContent.Load<Texture2D>("content/dds/Fort_T2");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T2_small");
            m_cSettlementModels[SettlementSize.Hamlet][3] = LoadModel("content/fbx/hamlet_T2");
            m_cSettlementTextures[SettlementSize.Hamlet][3] = pTexture;
            m_cSettlementModels[SettlementSize.Village][3] = LoadModel("content/fbx/village_T2");
            m_cSettlementTextures[SettlementSize.Village][3] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T3_big");
            m_cSettlementModels[SettlementSize.Town][3] = LoadModel("content/fbx/town_T3");
            m_cSettlementTextures[SettlementSize.Town][3] = pTexture;
            m_cSettlementModels[SettlementSize.City][3] = LoadModel("content/fbx/city_T3");
            m_cSettlementTextures[SettlementSize.City][3] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][3] = LoadModel("content/fbx/city_T3");
            m_cSettlementTextures[SettlementSize.Capital][3] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][3] = LoadModel("content/fbx/fort_T3");
            m_cSettlementTextures[SettlementSize.Fort][3] = LibContent.Load<Texture2D>("content/dds/Fort_T3");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T4_small");
            m_cSettlementModels[SettlementSize.Hamlet][4] = LoadModel("content/fbx/hamlet_T4");
            m_cSettlementTextures[SettlementSize.Hamlet][4] = pTexture;
            m_cSettlementModels[SettlementSize.Village][4] = LoadModel("content/fbx/village_T4");
            m_cSettlementTextures[SettlementSize.Village][4] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T4_big");
            m_cSettlementModels[SettlementSize.Town][4] = LoadModel("content/fbx/town_T4");
            m_cSettlementTextures[SettlementSize.Town][4] = pTexture;
            m_cSettlementModels[SettlementSize.City][4] = LoadModel("content/fbx/city_T4");
            m_cSettlementTextures[SettlementSize.City][4] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][4] = LoadModel("content/fbx/city_T4");
            m_cSettlementTextures[SettlementSize.Capital][4] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][4] = LoadModel("content/fbx/fort_T3");
            m_cSettlementTextures[SettlementSize.Fort][4] = LibContent.Load<Texture2D>("content/dds/Fort_T3");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T4_small");
            m_cSettlementModels[SettlementSize.Hamlet][5] = LoadModel("content/fbx/hamlet_T4");
            m_cSettlementTextures[SettlementSize.Hamlet][5] = pTexture;
            m_cSettlementModels[SettlementSize.Village][5] = LoadModel("content/fbx/village_T4");
            m_cSettlementTextures[SettlementSize.Village][5] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T5_big");
            m_cSettlementModels[SettlementSize.Town][5] = LoadModel("content/fbx/town_T5");
            m_cSettlementTextures[SettlementSize.Town][5] = pTexture;
            m_cSettlementModels[SettlementSize.City][5] = LoadModel("content/fbx/city_T5");
            m_cSettlementTextures[SettlementSize.City][5] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][5] = LoadModel("content/fbx/city_T5");
            m_cSettlementTextures[SettlementSize.Capital][5] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][5] = LoadModel("content/fbx/fort_T5");
            m_cSettlementTextures[SettlementSize.Fort][5] = LibContent.Load<Texture2D>("content/dds/Fort_T5");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T6_hamlet");
            m_cSettlementModels[SettlementSize.Hamlet][6] = LoadModel("content/fbx/hamlet_T6");
            m_cSettlementTextures[SettlementSize.Hamlet][6] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T6_village");
            m_cSettlementModels[SettlementSize.Village][6] = LoadModel("content/fbx/village_T6");
            m_cSettlementTextures[SettlementSize.Village][6] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T6");
            m_cSettlementModels[SettlementSize.Town][6] = LoadModel("content/fbx/town_T6");
            m_cSettlementTextures[SettlementSize.Town][6] = pTexture;
            m_cSettlementModels[SettlementSize.City][6] = LoadModel("content/fbx/city_T6");
            m_cSettlementTextures[SettlementSize.City][6] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][6] = LoadModel("content/fbx/city_T6");
            m_cSettlementTextures[SettlementSize.Capital][6] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][6] = LoadModel("content/fbx/fort_T6");
            m_cSettlementTextures[SettlementSize.Fort][6] = LibContent.Load<Texture2D>("content/dds/Fort_T6");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T7");
            m_cSettlementModels[SettlementSize.Hamlet][7] = LoadModel("content/fbx/hamlet_T7");
            m_cSettlementTextures[SettlementSize.Hamlet][7] = pTexture;
            m_cSettlementModels[SettlementSize.Village][7] = LoadModel("content/fbx/village_T7");
            m_cSettlementTextures[SettlementSize.Village][7] = pTexture;
            m_cSettlementModels[SettlementSize.Town][7] = LoadModel("content/fbx/town_T7");
            m_cSettlementTextures[SettlementSize.Town][7] = pTexture;
            m_cSettlementModels[SettlementSize.City][7] = LoadModel("content/fbx/city_T7");
            m_cSettlementTextures[SettlementSize.City][7] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][7] = LoadModel("content/fbx/city_T7");
            m_cSettlementTextures[SettlementSize.Capital][7] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][7] = LoadModel("content/fbx/fort_T7");
            m_cSettlementTextures[SettlementSize.Fort][7] = LibContent.Load<Texture2D>("content/dds/Fort_T7");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T8");
            m_cSettlementModels[SettlementSize.Hamlet][8] = LoadModel("content/fbx/hamlet_T8");
            m_cSettlementTextures[SettlementSize.Hamlet][8] = pTexture;
            m_cSettlementModels[SettlementSize.Village][8] = LoadModel("content/fbx/village_T8");
            m_cSettlementTextures[SettlementSize.Village][8] = pTexture;
            m_cSettlementModels[SettlementSize.Town][8] = LoadModel("content/fbx/town_T8");
            m_cSettlementTextures[SettlementSize.Town][8] = pTexture;
            m_cSettlementModels[SettlementSize.City][8] = LoadModel("content/fbx/city_T8");
            m_cSettlementTextures[SettlementSize.City][8] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][8] = LoadModel("content/fbx/city_T8");
            m_cSettlementTextures[SettlementSize.Capital][8] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][8] = LoadModel("content/fbx/fort_T7");
            m_cSettlementTextures[SettlementSize.Fort][8] = LibContent.Load<Texture2D>("content/dds/Fort_T7");
        }

        SpriteFont villageNameFont;
        SpriteFont townNameFont;
        SpriteFont cityNameFont;
        SpriteBatch m_pSpriteBatch;

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            foreach (MapMode pMode in Enum.GetValues(typeof(MapMode)))
                m_cMapModeData[pMode] = new MapModeData<VertexPositionColorNormal>();

            m_pBasicEffect = new BasicEffect(GraphicsDevice);
            m_pBasicEffect.VertexColorEnabled = true;
            m_pSpriteBatch = new SpriteBatch(GraphicsDevice);

            LibContent = new ContentManager(Services);

            // Create our effect.
            LoadTerrainTextures();

            villageNameFont = LibContent.Load<SpriteFont>("content/villagename");
            townNameFont = LibContent.Load<SpriteFont>("content/townname");
            cityNameFont = LibContent.Load<SpriteFont>("content/cityname");

            m_pMyEffect = LibContent.Load<Effect>("content/Effect1");
            BindEffectParameters();

            m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["LandRingworld"];
            pEffectWorld.SetValue(Matrix.Identity);

            pEffectAmbientLightColor.SetValue(eSkyColor.ToVector4());
            pEffectAmbientLightIntensity.SetValue(0.2f);

            pEffectDirectionalLightColor.SetValue(eSkyColor.ToVector4());
            pEffectDirectionalLightDirection.SetValue(new Vector3(0, 0, -150));
            pEffectDirectionalLightIntensity.SetValue(1);

            pEffectSpecularColor.SetValue(0);

            pEffectFogColor.SetValue(eSkyColor.ToVector4());
            pEffectFogDensity.SetValue(0.018f);

            pEffectBlendDistance.SetValue(2);
            pEffectBlendWidth.SetValue(9);

            pEffectTexture0.SetValue(sandTexture);
            pEffectTexture1.SetValue(grassTexture);
            pEffectTexture2.SetValue(rockTexture);
            pEffectTexture3.SetValue(snowTexture);
            pEffectTexture4.SetValue(forestTexture);
            pEffectTexture5.SetValue(roadTexture);
            pEffectTexture6.SetValue(swampTexture);
            pEffectTexture7.SetValue(lavaTexture);

            m_pMyEffect.Parameters["GridColor1"].SetValue(Microsoft.Xna.Framework.Color.Black.ToVector4());
            m_pMyEffect.Parameters["GridColor2"].SetValue(Microsoft.Xna.Framework.Color.Pink.ToVector4());
            m_pMyEffect.Parameters["GridColor3"].SetValue(Microsoft.Xna.Framework.Color.White.ToVector4());
            m_pMyEffect.Parameters["GridColor4"].SetValue(Microsoft.Xna.Framework.Color.Goldenrod.ToVector4());
            m_pMyEffect.Parameters["GridColor5"].SetValue(Microsoft.Xna.Framework.Color.Yellow.ToVector4());
            m_pMyEffect.Parameters["GridColor6"].SetValue(Microsoft.Xna.Framework.Color.Black.ToVector4());     
            
            // create the effect and vertex declaration for drawing the
            // picked triangle.
            lineEffect = new BasicEffect(GraphicsDevice);
            lineEffect.VertexColorEnabled = true;

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            refractionRenderTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat);

            if (m_pWorld != null && m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                m_pCamera = new RingworldCamera(m_pWorld.m_pGrid.RX / ((float)Math.PI * 1000), GraphicsDevice);
            else if (m_pWorld != null && m_pWorld.m_pGrid.m_eShape == WorldShape.Planet)
                m_pCamera = new PlanetCamera(m_pWorld.m_pGrid.RX / ((float)Math.PI * 1000), GraphicsDevice);
            else
                m_pCamera = new PlainCamera(GraphicsDevice);


            LoadTrees();

            LoadSettlements();

            textEffect = new BasicEffect(GraphicsDevice);

            // Start the animation timer.
            timer = Stopwatch.StartNew();
            lastTime = timer.Elapsed.TotalMilliseconds;

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        /// <summary>
        /// m_pMyEffect должен уже быть создан и настроен!
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        private Model LoadModel(string sPath)
        {
            Model pModel = LibContent.Load<Model>(sPath);
            foreach (ModelMesh mesh in pModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = m_pMyEffect.Clone();

            return pModel;
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

        public bool m_bPanMode = false;

        public void ResetPanning()
        {
            m_pLastPicking = m_pCurrentPicking;
        }

        RenderTarget2D refractionRenderTarget;

        public int m_iFrame = 0;

        VertexBuffer myVertexBuffer;
        IndexBuffer myIndexBuffer;

        private void CopyToBuffers()
        {
            if (m_pWorld == null)
                return;

            if (m_eMode == MapMode.Sattelite)
            {
                if (myVertexBuffer != null)
                    myVertexBuffer.Dispose();
                myVertexBuffer = new VertexBuffer(GraphicsDevice, VertexMultitextured.VertexDeclaration, m_pLand.m_aVertices.Length, BufferUsage.WriteOnly);
                myVertexBuffer.SetData(m_pLand.m_aVertices);

                if (myIndexBuffer != null)
                    myIndexBuffer.Dispose();
                myIndexBuffer = new IndexBuffer(GraphicsDevice, typeof(int), m_pLand.m_aIndices.Length, BufferUsage.WriteOnly);
                myIndexBuffer.SetData(m_pLand.m_aIndices);
            }
            else
            {
                if (myVertexBuffer != null)
                    myVertexBuffer.Dispose();
                myVertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColorNormal.VertexDeclaration, m_cMapModeData[m_eMode].m_aVertices.Length, BufferUsage.WriteOnly);
                myVertexBuffer.SetData(m_cMapModeData[m_eMode].m_aVertices);

                if (myIndexBuffer != null)
                    myIndexBuffer.Dispose();
                myIndexBuffer = new IndexBuffer(GraphicsDevice, typeof(int), m_cMapModeData[m_eMode].m_aIndices.Length, BufferUsage.WriteOnly);
                myIndexBuffer.SetData(m_cMapModeData[m_eMode].m_aIndices);
            }
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            m_iFrame++;

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
            if (m_bPanMode && m_pCurrentPicking != null)
            {
                if (m_pLastPicking != null)
                    m_pCamera.Target += (Vector3)m_pLastPicking - (Vector3)m_pCurrentPicking;

               // m_pLastPicking = m_pCurrentPicking;
                m_pCurrentPicking = null;
            }
            m_pCamera.Update();

            pEffectView.SetValue(m_pCamera.View);
            pEffectProjection.SetValue(m_pCamera.Projection);

            pEffectCameraPosition.SetValue(m_pCamera.Position);

            pEffectWorld.SetValue(Matrix.Identity);

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullCounterClockwiseFace;
            //rs.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;

            if (m_eMode == MapMode.Sattelite)
            {
                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                    m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["LandRingworld"];
                else
                    m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Land"];
                m_pMyEffect.CurrentTechnique.Passes[0].Apply();

                GraphicsDevice.SetRenderTarget(refractionRenderTarget);
                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Microsoft.Xna.Framework.Color.Black, 1.0f, 0);
                GraphicsDevice.DrawUserIndexedPrimitives<VertexMultitextured>(PrimitiveType.TriangleList,
                                                    m_pUnderwater.m_aVertices, 0, m_pUnderwater.m_aVertices.Length - 1, m_pUnderwater.m_aIndices, 0, m_pUnderwater.m_iTrianglesCount);
                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Clear(eSkyColor);

                GraphicsDevice.Indices = myIndexBuffer;
                GraphicsDevice.SetVertexBuffer(myVertexBuffer);
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, m_pLand.m_aVertices.Length, 0, m_pLand.m_iTrianglesCount);

                DrawWater(time);

                DrawTrees();
            }
            else
            {
                GraphicsDevice.Clear(eSkyColor);

                m_pBasicEffect.View = m_pCamera.View;
                m_pBasicEffect.Projection = m_pCamera.Projection;

                m_pBasicEffect.LightingEnabled = true;
                m_pBasicEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1, -1, -1));
                m_pBasicEffect.AmbientLightColor = Microsoft.Xna.Framework.Color.Multiply(eSkyColor, 0.2f).ToVector3();

                m_pBasicEffect.PreferPerPixelLighting = true;

                RasterizerState rs1 = new RasterizerState();
                rs1.CullMode = CullMode.CullCounterClockwiseFace;
                //rs.FillMode = FillMode.WireFrame;
                GraphicsDevice.RasterizerState = rs1;

                m_pBasicEffect.World = Matrix.Identity;
                m_pBasicEffect.CurrentTechnique.Passes[0].Apply();

                GraphicsDevice.Indices = myIndexBuffer;
                GraphicsDevice.SetVertexBuffer(myVertexBuffer);
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, m_cMapModeData[m_eMode].m_aVertices.Length, 0, m_cMapModeData[m_eMode].m_iTrianglesCount);

                //GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorNormal>(PrimitiveType.TriangleList,
                //                                    m_cMapModeData[m_eMode].m_aVertices, 0, m_cMapModeData[m_eMode].m_aVertices.Length - 1, m_cMapModeData[m_eMode].m_aIndices, 0, m_cMapModeData[m_eMode].m_iTrianglesCount);
            }

            if (m_bShowRoads)
                DrawRoads();

            if (m_bShowLocations)
                for (int i = 0; i < m_aSettlements.Length; i++)
                    DrawSettlement(m_aSettlements[i]);

            if (m_bShowLocationsBorders)
                DrawLayer(MapLayer.Locations);
            if (m_bShowLands)
                DrawLayer(MapLayer.Lands);
            if (m_bShowLandMasses)
                DrawLayer(MapLayer.LandMasses);
            if (m_bShowProvincies)
                DrawLayer(MapLayer.Provincies);
            if (m_bShowStates)
                DrawLayer(MapLayer.States);
            
            // Draw the outline of the triangle under the cursor.
            DrawPickedTriangle();

            if (m_bShowLocations)
                DrawSettlementNames();

        }

        private void DrawWater(float time)
        {
            m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Water"];
            //effect.Parameters["xReflectionView"].SetValue(reflectionViewMatrix);
            //effect.Parameters["xReflectionMap"].SetValue(reflectionMap);
            m_pMyEffect.Parameters["xRefractionMap"].SetValue(refractionRenderTarget);

            m_pMyEffect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList,
                                                m_pWater.m_aVertices, 0, m_pWater.m_aVertices.Length - 1, m_pWater.m_aIndices, 0, m_pWater.m_iTrianglesCount);
        }

        private void DrawRoads()
        {
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullCounterClockwiseFace;
            //rs.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;

            //m_pMyEffect.Parameters["GridFog"].SetValue(m_eMode == MapMode.Sattelite);
            //m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Grid"];
            if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["LandRingworld"];
            else
                m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Land"];
            m_pMyEffect.CurrentTechnique.Passes[0].Apply();

            //m_pMyEffect.CurrentTechnique.Passes[6].Apply();

            for (int i = 0; i < m_aRoads.Length; i++)
                GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                                          m_aRoads[i].m_aVertices, 0, m_aRoads[i].m_aVertices.Length, m_aRoads[i].m_aIndices, 0, m_aRoads[i].m_aIndices.Length / 3);

            // Reset renderstates to their default values.
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        private void DrawLayer(MapLayer eLayer)
        {
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullCounterClockwiseFace;
            //rs.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;
            //GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            m_pMyEffect.Parameters["GridFog"].SetValue(m_eMode == MapMode.Sattelite);
            m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Grid"];

            // Draw the triangle.
            int[] aIndices = m_pLayers.m_aIndices[eLayer];
            switch (eLayer)
            {
                case MapLayer.Locations:
                    m_pMyEffect.CurrentTechnique.Passes[0].Apply();
                    break;
                case MapLayer.Lands:
                    m_pMyEffect.CurrentTechnique.Passes[1].Apply();
                    break;
                case MapLayer.LandMasses:
                    m_pMyEffect.CurrentTechnique.Passes[2].Apply();
                    break;
                case MapLayer.Provincies:
                    m_pMyEffect.CurrentTechnique.Passes[3].Apply();
                    break;
                case MapLayer.States:
                    m_pMyEffect.CurrentTechnique.Passes[4].Apply();
                    break;
                case MapLayer.Continents:
                    m_pMyEffect.CurrentTechnique.Passes[5].Apply();
                    break;
                default:
                    throw new Exception("Unknown map layer!");
            }


            GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList,
                                      m_pLayers.m_aVertices, 0, m_pLayers.m_aVertices.Length, aIndices, 0, aIndices.Length/2);

            // Reset renderstates to their default values.
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        private void DrawTrees()
        {
            float fMaxDistanceSquared = 2500;
            //fMaxDistanceSquared *= (float)(70 / Math.Sqrt(m_pWorld.m_pGrid.m_iLocationsCount));
            
            foreach (var vTree in m_aTrees)
            {
                Model pTreeModel = vTree.Key;

                Matrix[] instancedModelBones = new Matrix[pTreeModel.Bones.Count];
                pTreeModel.CopyAbsoluteBoneTransformsTo(instancedModelBones);

                List<Matrix> instances = new List<Matrix>();
                for (int i = 0; i < vTree.Value.Length; i++)
                {
                    TreeModel pTree = vTree.Value[i];

                    Vector3 pViewVector = pTree.m_pPosition - m_pCamera.Position;

                    float fCos = Vector3.Dot(Vector3.Normalize(pViewVector), m_pCamera.Direction);
                    if (fCos < 0.6) //cos(45) = 0,70710678118654752440084436210485...
                        continue;

                    if (pViewVector.LengthSquared() > fMaxDistanceSquared)
                        continue;

                    instances.Add(pTree.worldMatrix);
                }

                DrawModelHardwareInstancing(pTreeModel, instancedModelBones,
                                         instances.ToArray(), m_pCamera.View, m_pCamera.Projection);
            }
        }

        // To store instance transform matrices in a vertex buffer, we use this custom
        // vertex type which encodes 4x4 matrices as a set of four Vector4 values.
        static VertexDeclaration instanceVertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
            new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 1),
            new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 2),
            new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 3)
        );
        private DynamicVertexBuffer instanceVertexBuffer;

        /// <summary>
        /// Efficiently draws several copies of a piece of geometry using hardware instancing.
        /// </summary>
        private void DrawModelHardwareInstancing(Model model, Matrix[] modelBones,
                                         Matrix[] instances, Matrix view, Matrix projection)
        {
            if (instances.Length == 0)
                return;

            // If we have more instances than room in our vertex buffer, grow it to the neccessary size.
            if ((instanceVertexBuffer == null) ||
                (instances.Length > instanceVertexBuffer.VertexCount))
            {
                if (instanceVertexBuffer != null)
                    instanceVertexBuffer.Dispose();

                instanceVertexBuffer = new DynamicVertexBuffer(GraphicsDevice, instanceVertexDeclaration,
                                                               instances.Length, BufferUsage.WriteOnly);
            }

            // Transfer the latest instance transform matrices into the instanceVertexBuffer.
            instanceVertexBuffer.SetData(instances, 0, instances.Length, SetDataOptions.Discard);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Tell the GPU to read from both the model vertex buffer plus our instanceVertexBuffer.
                    GraphicsDevice.SetVertexBuffers(
                        new VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset, 0),
                        new VertexBufferBinding(instanceVertexBuffer, 0, 1)
                    );

                    GraphicsDevice.Indices = meshPart.IndexBuffer;

                    // Set up the instance rendering effect.
                    Effect effect = meshPart.Effect;

                    //effect.CurrentTechnique = effect.Techniques["HardwareInstancing"];

                    effect.Parameters["World"].SetValue(modelBones[mesh.ParentBone.Index]);
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["CameraPosition"].SetValue(m_pCamera.Position);
                    effect.Parameters["Projection"].SetValue(projection);

                    // Draw all the instance copies in a single call.
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0,
                                                               meshPart.NumVertices, meshPart.StartIndex,
                                                               meshPart.PrimitiveCount, instances.Length);
                    }
                }
            }
        }

        private void DrawSettlement(SettlementModel pSettlement)
        {
            Vector3 pViewVector = pSettlement.m_pPosition - m_pCamera.Position;

            float fCos = Vector3.Dot(Vector3.Normalize(pViewVector), m_pCamera.Direction);
            if (fCos < 0.6) //cos(45) = 0,70710678118654752440084436210485...
                return;

            if (pViewVector.LengthSquared() > 2500)
                return;

            Matrix[] xwingTransforms = new Matrix[pSettlement.m_pModel.Bones.Count];
            pSettlement.m_pModel.CopyAbsoluteBoneTransformsTo(xwingTransforms);
            foreach (ModelMesh mesh in pSettlement.m_pModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    currentEffect.Parameters["World"].SetValue(xwingTransforms[mesh.ParentBone.Index] * pSettlement.worldMatrix);
                    currentEffect.Parameters["View"].SetValue(m_pCamera.View);
                    currentEffect.Parameters["CameraPosition"].SetValue(m_pCamera.Position);
                    currentEffect.Parameters["Projection"].SetValue(m_pCamera.Projection);
                }
                mesh.Draw();
            }
        }

        private void DrawSettlementNames()
        {
            m_pSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, DepthStencilState.DepthRead, null);
            for (int i = 0; i < m_aSettlements.Length; i++)
            {
                SettlementModel pSettlement = m_aSettlements[i];
                Vector3 pViewVector = pSettlement.m_pPosition - m_pCamera.Position;

                float fCos = Vector3.Dot(Vector3.Normalize(pViewVector), m_pCamera.Direction);
                if (fCos < 0.6) //cos(45) = 0,70710678118654752440084436210485...
                    continue;

                if (pSettlement.m_iSize < 2 && pViewVector.Length() * 2f / (pSettlement.m_iSize + 1) > 40)
                    continue;

                Vector3 pUplift;
                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Planet)
                    pUplift = Vector3.Normalize(pSettlement.m_pPosition);
                else if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                    pUplift = Vector3.Transform(-Vector3.Normalize(pSettlement.m_pPosition), Matrix.CreateScale(1, 0, 1));
                else
                    pUplift = Vector3.Up;

                // calculate screenspace of text3d space position
                Vector3 screenSpace = GraphicsDevice.Viewport.Project(Vector3.Zero,
                                                                        m_pCamera.Projection,
                                                                        m_pCamera.View,
                                                                        Matrix.CreateTranslation(pSettlement.m_pPosition + pUplift / 3 + m_pCamera.Top / 10));
                //                                                                         Matrix.CreateTranslation(s.position + shipTagOffset));

                Vector2 textPosition;
                // get 2D position from screenspace vector
                textPosition.X = screenSpace.X;
                textPosition.Y = screenSpace.Y;

                SpriteFont pFont = townNameFont;
                switch (pSettlement.m_iSize)
                {
                    case 0:
                        pFont = villageNameFont;
                        break;
                    case 1:
                        pFont = townNameFont;
                        break;
                    case 2:
                        pFont = cityNameFont;
                        break;
                }

                // we want to draw the text centered around textPosition, so we'll
                // calculate the center of the string, and use that as the origin
                // argument to spriteBatch.DrawString. DrawString automatically
                // centers text around the vector specified by the origin argument.
                Vector2 stringCenter = pFont.MeasureString(pSettlement.m_sName) * 0.5f;

                float d = pViewVector.Length() * 3f / (pSettlement.m_iSize + 1);
                float l = (float)Math.Exp(-Math.Pow(d * 0.018, 2));

                // draw text
                m_pSpriteBatch.DrawString(pFont,
                                            pSettlement.m_sName,
                                            textPosition,
                                            Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.Yellow, Microsoft.Xna.Framework.Color.DarkGoldenrod, 1 - l), 0, stringCenter, l, SpriteEffects.None, screenSpace.Z);
            }
            m_pSpriteBatch.End();
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque; 
        }

        public void FocusSelectedState()
        {
            m_pCamera.Target = GetPosition(m_pSelectedState, m_pWorld.m_pGrid.m_eShape, m_fLandHeightMultiplier);
        }

        // Vertex array that stores exactly which triangle was picked.
        // Effect and vertex declaration for drawing the picked triangle.
        BasicEffect lineEffect;

        BasicEffect textEffect;
        
        /// <summary>
        /// Helper for drawing the outline of the triangle currently under the cursor.
        /// </summary>
        void DrawPickedTriangle()
        {
            if (m_bPicked && m_pFocusedLocation != null)
            {
                // Set line drawing renderstates. We disable backface culling
                // and turn off the depth buffer because we want to be able to
                // see the picked triangle outline regardless of which way it is
                // facing, and even if there is other geometry in front of it.
                RasterizerState rs = new RasterizerState();
                rs.CullMode = CullMode.CullCounterClockwiseFace;
                rs.FillMode = FillMode.WireFrame;
                GraphicsDevice.RasterizerState = rs; 
                GraphicsDevice.DepthStencilState = DepthStencilState.None;

                // Activate the line drawing BasicEffect.
                lineEffect.Projection = m_pCamera.Projection;
                lineEffect.View = m_pCamera.View;

                lineEffect.CurrentTechnique.Passes[0].Apply();

                // Draw the triangle.
                GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList,
                                          m_pLayers.m_aVertices, 0, m_pLayers.m_aVertices.Length - 1, m_pLayers.m_aLocations[m_pFocusedLocation], 0, m_pLayers.m_aLocations[m_pFocusedLocation].Length/2);

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

            LocationX pLoc;

            // Perform the ray to model intersection test.
            float? intersection = RayIntersectsLandscape(CullMode.CullCounterClockwiseFace, cursorRay, 
                                                        out pLoc);
            m_bPicked = false;

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
                    if(UpdateFocus(pLoc))
                        UpdateTooltipString();

                    m_bPicked = true;

                    m_pCurrentPicking = cursorRay.Position + Vector3.Normalize(cursorRay.Direction) * intersection;
                }
            }
            else
                m_pCurrentPicking = null;
        }

        /// <summary>
        /// Определяет, что находится под курсором.
        /// </summary>
        /// <returns>true - если какая-то суша, false - если просто океан</returns>
        private bool UpdateFocus(LocationX pLoc)
        {
            if (m_pWorld == null)
                return false;

            if (m_pFocusedLocation == pLoc)
                return false;

            m_pFocusedLocation = pLoc;
            m_pFocusedLand = null;
            m_pFocusedLandMass = null;
            m_pFocusedContinent = null;
            m_pFocusedProvince = null;
            m_pFocusedState = null;

            if (m_pFocusedLocation != null)
            {
                m_pFocusedLand = m_pFocusedLocation.Owner as LandX;
                if (m_pFocusedLand != null)
                {
                    m_pFocusedLandMass = m_pFocusedLand.Owner as LandMass<LandX>;
                    if(m_pFocusedLandMass != null)
                        m_pFocusedContinent = m_pFocusedLandMass.Owner as ContinentX;

                    m_pFocusedProvince = m_pFocusedLand.m_pProvince;
                    if (m_pFocusedProvince != null)
                        m_pFocusedState = m_pFocusedProvince.Owner as State;
                }
            }

            return true;
        }

        public string sToolTip = "";

        private void UpdateTooltipString()
        {
            sToolTip = "";

            if (m_pFocusedContinent != null)
                sToolTip += m_pFocusedContinent.ToString();

            if (m_pFocusedLand != null)
            {
                if (sToolTip.Length > 0)
                    sToolTip += "\n - ";

                sToolTip += m_pFocusedLand.ToString();
            }

            if (m_pFocusedState != null)
            {
                if (sToolTip.Length > 0)
                    sToolTip += "\n   - ";

                sToolTip += string.Format("{1} {0} ({2})", m_pFocusedState.m_pInfo.m_sName, m_pFocusedState.m_sName, m_pFocusedState.m_pNation);
            }

            if (m_pFocusedProvince != null)
            {
                if (sToolTip.Length > 0)
                    sToolTip += "\n     - ";

                sToolTip += string.Format("province {0} ({2}, {1})", m_pFocusedProvince.m_sName, m_pFocusedProvince.m_pAdministrativeCenter == null ? "-" : m_pFocusedProvince.m_pAdministrativeCenter.ToString(), m_pFocusedProvince.m_pNation);

                //sToolTip += "\n          [";

                //foreach(var pNation in m_pFocusedProvince.m_cNationsCount)
                //sToolTip += string.Format("{0}: {1}, ", pNation.Key, pNation.Value);
                //sToolTip += "]";
            }

            if (m_pFocusedLocation != null)
            {
                if (sToolTip.Length > 0)
                    sToolTip += "\n       - ";

                sToolTip += m_pFocusedLocation.ToString();

                if (m_pFocusedLocation.m_pSettlement != null && m_pFocusedLocation.m_pSettlement.m_cBuildings.Count > 0)
                {
                    Dictionary<string, int> cBuildings = new Dictionary<string, int>();

                    foreach (Building pBuilding in m_pFocusedLocation.m_pSettlement.m_cBuildings)
                    {
                        int iCount = 0;
                        cBuildings.TryGetValue(pBuilding.ToString(), out iCount);
                        cBuildings[pBuilding.ToString()] = iCount + 1;
                    }

                    foreach (var vBuilding in cBuildings)
                        sToolTip += "\n         - " + vBuilding.Key + "  x" + vBuilding.Value.ToString();
                }

                if (m_pFocusedLocation.m_cHaveRoadTo.Count > 0)
                {
                    sToolTip += "\nHave roads to:";
                    foreach (var pRoad in m_pFocusedLocation.m_cHaveRoadTo)
                        sToolTip += "\n - " + pRoad.Key.m_pSettlement.m_pInfo.m_eSize.ToString() + " " + pRoad.Key.m_pSettlement.m_sName + " [" + pRoad.Value.m_eLevel.ToString() + "]";
                }

                if (m_pFocusedLocation.m_cHaveSeaRouteTo.Count > 0)
                {
                    sToolTip += "\nHave sea routes to:";
                    foreach (LocationX pRoute in m_pFocusedLocation.m_cHaveSeaRouteTo)
                        sToolTip += "\n - " + pRoute.m_pSettlement.m_pInfo.m_eSize.ToString() + " " + pRoute.m_pSettlement.m_sName;
                }
            }

            //if (toolTip1.GetToolTip(this) != sToolTip)
            //    toolTip1.SetToolTip(this, sToolTip);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}
