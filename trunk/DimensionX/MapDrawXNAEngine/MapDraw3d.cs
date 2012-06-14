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
        protected class TreeModel
        {
            public readonly Vector3 m_pPosition;
            public readonly float m_fAngle;
            public readonly Model m_pModel;
            public readonly Matrix worldMatrix;

            public TreeModel(Vector3 pPosition, float fAngle, Model pModel, WorldShape eWorldShape, Texture2D pTexture)
            {
                m_pPosition = pPosition;
                m_fAngle = fAngle;
                m_pModel = pModel;

                worldMatrix = Matrix.CreateScale(0.2f, 0.2f, 0.2f) * Matrix.CreateRotationY(m_fAngle) * Matrix.CreateTranslation(m_pPosition);

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
        }

        protected class SettlementModel
        {
            public readonly Vector3 m_pPosition;
            public readonly float m_fAngle;
            public readonly Model m_pModel;
            public readonly float m_fScale;
            public readonly Matrix worldMatrix;

            public SettlementModel(Vector3 pPosition, float fAngle, float fScale, Model pModel, WorldShape eWorldShape, Texture2D pTexture)
            {
                m_pPosition = pPosition;
                m_fAngle = fAngle;
                m_fScale = fScale;
                m_pModel = pModel;

//                worldMatrix = Matrix.CreateScale(m_fScale) * Matrix.CreateRotationX(-(float)Math.PI / 2) * Matrix.CreateRotationY(m_fAngle) * Matrix.CreateTranslation(m_pPosition);
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

        protected class MapModeData
        { 
            public VertexPositionColorNormal[] m_aVertices = new VertexPositionColorNormal[0];
            public int[] m_aIndices = new int[0];
            public int m_iTrianglesCount = 0;
        }

        /// <summary>
        /// мир, карту которого мы рисуем
        /// </summary>
        internal World m_pWorld = null;

        BasicEffect m_pBasicEffect;
        Effect m_pMyEffect;
        Stopwatch timer;

        VertexMultitextured[] m_aLandVertices;
        int[] m_aLandIndices;
        int m_iLandTrianglesCount = 0;

        VertexMultitextured[] m_aUnderwaterVertices;
        int[] m_aUnderwaterIndices;
        int m_iUnderwaterTrianglesCount = 0;

        VertexPositionTexture[] m_aWaterVertices;
        int[] m_aWaterIndices;
        int m_iWaterTrianglesCount = 0;

        Dictionary<MapMode, MapModeData> m_cMapModeData = new Dictionary<MapMode, MapModeData>();

        private float m_fLandHeightMultiplier = 0.1f;

        //private void FillPrimitivesFake()
        //{
        //    m_aLandVertices = new VertexMultitextured[5];

        //    m_aLandVertices[0] = new VertexMultitextured();
        //    m_aLandVertices[0].Position = new Vector3(-m_pWorld.m_pGrid.RX, 0, -m_pWorld.m_pGrid.RY);
        //    //userPrimitives[0].Color = Microsoft.Xna.Framework.Color.White;
        //    m_aLandVertices[0].TextureCoordinate.X = 0;
        //    m_aLandVertices[0].TextureCoordinate.Y = 0;
        //    m_aLandVertices[0].TexWeights = new Vector4(1, 0, 0, 0);
        //    m_aLandVertices[0].TexWeights2 = new Vector4(0);

        //    m_aLandVertices[1] = new VertexMultitextured();
        //    m_aLandVertices[1].Position = new Vector3(m_pWorld.m_pGrid.RX, 0, -m_pWorld.m_pGrid.RY);
        //    //userPrimitives[1].Color = Microsoft.Xna.Framework.Color.Red;
        //    m_aLandVertices[1].TextureCoordinate.X = 200;
        //    m_aLandVertices[1].TextureCoordinate.Y = 0;
        //    m_aLandVertices[1].TexWeights = new Vector4(0, 1, 0, 0);
        //    m_aLandVertices[1].TexWeights2 = new Vector4(0);

        //    m_aLandVertices[2] = new VertexMultitextured();
        //    m_aLandVertices[2].Position = new Vector3(m_pWorld.m_pGrid.RX, 0, m_pWorld.m_pGrid.RY);
        //    //userPrimitives[2].Color = Microsoft.Xna.Framework.Color.Blue;
        //    m_aLandVertices[2].TextureCoordinate.X = 200;
        //    m_aLandVertices[2].TextureCoordinate.Y = 200;
        //    m_aLandVertices[2].TexWeights = new Vector4(0, 0, 1, 0);
        //    m_aLandVertices[2].TexWeights2 = new Vector4(0); 

        //    m_aLandVertices[3] = new VertexMultitextured();
        //    m_aLandVertices[3].Position = new Vector3(-m_pWorld.m_pGrid.RX, 0, m_pWorld.m_pGrid.RY);
        //    //userPrimitives[3].Color = Microsoft.Xna.Framework.Color.Green;
        //    m_aLandVertices[3].TextureCoordinate.X = 0;
        //    m_aLandVertices[3].TextureCoordinate.Y = 200;
        //    m_aLandVertices[3].TexWeights = new Vector4(0, 0, 0, 1);
        //    m_aLandVertices[3].TexWeights2 = new Vector4(0);

        //    m_aLandVertices[4] = new VertexMultitextured();
        //    m_aLandVertices[4].Position = new Vector3(0, 0, 0);
        //    //userPrimitives[4].Color = Microsoft.Xna.Framework.Color.Black;
        //    m_aLandVertices[4].TextureCoordinate.X = 100;
        //    m_aLandVertices[4].TextureCoordinate.Y = 100;
        //    m_aLandVertices[4].TexWeights = new Vector4(0.25f);
        //    m_aLandVertices[4].TexWeights2 = new Vector4(0);

        //    m_iLandTrianglesCount = 4;
        //    m_aLandIndices = new int[m_iLandTrianglesCount * 3];

        //    m_aLandIndices[0] = 4;
        //    m_aLandIndices[1] = 0;
        //    m_aLandIndices[2] = 1;

        //    m_aLandIndices[3] = 4;
        //    m_aLandIndices[4] = 1;
        //    m_aLandIndices[5] = 2;

        //    m_aLandIndices[6] = 4;
        //    m_aLandIndices[7] = 2;
        //    m_aLandIndices[8] = 3;

        //    m_aLandIndices[9] = 4;
        //    m_aLandIndices[10] = 3;
        //    m_aLandIndices[11] = 0;
        //}

        private TreeModel[] m_aTrees = new TreeModel[0];

        private SettlementModel[] m_aSettlements = new SettlementModel[0];

        private float m_fTextureScale = 50000;

        /// <summary>
        /// Вычисляет реальные 3d координаты вертекса с учётом его собственной высоты над сеткой и формы мира
        /// </summary>
        /// <param name="pVertex">вертекс</param>
        /// <param name="eShape">форма мира</param>
        /// <param name="fMultiplier">множитель высоты</param>
        /// <returns></returns>
        private static Vector3 GetPosition(Vertex pVertex, WorldShape eShape, float fMultiplier)
        {
            return GetPosition(pVertex, eShape, pVertex.m_fHeight, fMultiplier);
        }

        /// <summary>
        /// Вычисляет реальные 3d координаты центра локации с учётом её собственной высоты над сеткой и формы мира
        /// </summary>
        /// <param name="pLoc">локация</param>
        /// <param name="eShape">форма мира</param>
        /// <param name="fMultiplier">множитель высоты</param>
        /// <returns></returns>
        private static Vector3 GetPosition(LocationX pLoc, WorldShape eShape, float fMultiplier)
        {
            return GetPosition(pLoc, eShape, pLoc.m_fHeight, fMultiplier);
        }

        /// <summary>
        /// Вычисляет реальные 3d координаты вертекса с учётом указанной высоты над сеткой и формы мира
        /// </summary>
        /// <param name="pVertex">вертекс</param>
        /// <param name="eShape">форма мира</param>
        /// <param name="fHeight">высота</param>
        /// <param name="fMultiplier">множитель высоты</param>
        /// <returns></returns>
        private static Vector3 GetPosition(Vertex pVertex, WorldShape eShape, float fHeight, float fMultiplier)
        {
            Vector3 pPosition = new Vector3(pVertex.m_fX / 1000, pVertex.m_fZ / 1000, pVertex.m_fY / 1000);
            if (eShape == WorldShape.Ringworld)
                pPosition -= Vector3.Normalize(pPosition) * fHeight * fMultiplier;
            else
                pPosition += Vector3.Up * fHeight * fMultiplier;

            return pPosition;
        }

        /// <summary>
        /// Вычисляет реальные 3d координаты центра локации с учётом указанной высоты над сеткой и формы мира
        /// </summary>
        /// <param name="pLoc">локация</param>
        /// <param name="eShape">форма мира</param>
        /// <param name="fHeight">высота</param>
        /// <param name="fMultiplier">множитель высоты</param>
        /// <returns></returns>
        private static Vector3 GetPosition(LocationX pLoc, WorldShape eShape, float fHeight, float fMultiplier)
        {
            Vector3 pPosition = new Vector3(pLoc.X / 1000, pLoc.Z / 1000, pLoc.Y / 1000);
            if (eShape == WorldShape.Ringworld)
                pPosition -= Vector3.Normalize(pPosition) * fHeight * fMultiplier;
            else
                pPosition += Vector3.Up * fHeight * fMultiplier;

            return pPosition;
        }

        /// <summary>
        /// текстурированная поверхность всего ландшафта
        /// </summary>
        private void BuildLand()
        {
            int iPrimitivesCount = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    iPrimitivesCount++;
                }

            }
            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                if (pLoc.Forbidden || pLoc.Owner == null)
                    continue;

                iPrimitivesCount++;
            }

            // Create the verticies for our triangle
            m_aLandVertices = new VertexMultitextured[iPrimitivesCount];

            Dictionary<long, int> cVertexes = new Dictionary<long, int>();
            Dictionary<long, int> cLocations = new Dictionary<long, int>();

            int iCounter = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                VertexMultitextured pVM = new VertexMultitextured();

                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    pVM.TexWeights = new Vector4(0);
                    pVM.TexWeights2 = new Vector4(0);

                    LandType eLT = (pLoc.Owner as LandX).Type.m_eType;
                    pVM.TexWeights += GetTexWeights(eLT);
                    pVM.TexWeights2 += GetTexWeights2(eLT);
                }

                //у нас x и y - это горизонтальная плоскость, причём y растёт в направлении вниз экрана, т.е. как бы к зрителю. а z - это высота.
                //в DX всё не как у людей. У них горизонтальная плоскость - это xz, причём z растёт к зрителю, а y - высота
                pVM.Position = GetPosition(pVertex, m_pWorld.m_pGrid.m_eShape, m_fLandHeightMultiplier);

                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                {
                    float fPhi = (float)Math.Atan2(pVertex.m_fX, pVertex.m_fZ);

                    pVM.TextureCoordinate.X = fPhi * m_pWorld.m_pGrid.RX / ((float)Math.PI * m_fTextureScale);
                }
                else
                {
                    pVM.TextureCoordinate.X = pVertex.m_fX / m_fTextureScale;
                }
                pVM.TextureCoordinate.Y = pVertex.m_fY / m_fTextureScale;

                m_aLandVertices[iCounter] = pVM;
                cVertexes[pVertex.m_iID] = iCounter;

                iCounter++;
            }

            m_iLandTrianglesCount = 0;
            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                if (pLoc.Forbidden || pLoc.Owner == null)
                    continue;

                m_aLandVertices[iCounter] = new VertexMultitextured();
                float fHeight = pLoc.m_fHeight * m_fLandHeightMultiplier;
                //if (pLoc.m_eType == RegionType.Peak)
                //    fHeight += 0.5f * m_fLandHeightMultiplier;
                if (pLoc.m_eType == RegionType.Volcano)
                    fHeight -= 10 * m_fLandHeightMultiplier;

                m_aLandVertices[iCounter].Position = GetPosition(pLoc, m_pWorld.m_pGrid.m_eShape, m_fLandHeightMultiplier);

                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                {
                    float fPhi = (float)Math.Atan2(pLoc.X, pLoc.Z);

                    m_aLandVertices[iCounter].TextureCoordinate.X = fPhi * m_pWorld.m_pGrid.RX / ((float)Math.PI * m_fTextureScale);
                }
                else
                {
                    m_aLandVertices[iCounter].TextureCoordinate.X = pLoc.X / m_fTextureScale;
                }
                m_aLandVertices[iCounter].TextureCoordinate.Y = pLoc.Y / m_fTextureScale;

                LandType eLT = LandType.Ocean;
                if (pLoc.Owner != null)
                    eLT = (pLoc.Owner as LandX).Type.m_eType;
                m_aLandVertices[iCounter].TexWeights = GetTexWeights(eLT);
                m_aLandVertices[iCounter].TexWeights2 = GetTexWeights2(eLT);

                cLocations[pLoc.m_iID] = iCounter;

                m_iLandTrianglesCount += pLoc.m_aBorderWith.Length;

                iCounter++;
            }

            // Create the indices used for each triangle
            m_aLandIndices = new int[m_iLandTrianglesCount * 3];

            iCounter = 0;

            List<TreeModel> cTrees = new List<TreeModel>();
            List<SettlementModel> cSettlements = new List<SettlementModel>();

            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                if (pLoc.Forbidden || pLoc.m_pFirstLine == null)
                    continue;

                LandType eLT = LandType.Ocean;
                if (pLoc.Owner != null)
                    eLT = (pLoc.Owner as LandX).Type.m_eType;

                float fMinHeight = float.MaxValue;
                Line pLine = pLoc.m_pFirstLine;
                do
                {
                    m_aLandIndices[iCounter++] = cLocations[pLoc.m_iID];
                    m_aLandIndices[iCounter++] = cVertexes[pLine.m_pPoint2.m_iID];
                    m_aLandIndices[iCounter++] = cVertexes[pLine.m_pPoint1.m_iID];

                    if (fMinHeight > pLine.m_pPoint1.m_fHeight)
                        fMinHeight = pLine.m_pPoint1.m_fHeight;

                    pLine = pLine.m_pNext;
                }
                while (pLine != pLoc.m_pFirstLine);

                if (pLoc.m_pSettlement != null)
                {
                    m_aLandVertices[cLocations[pLoc.m_iID]].Position = GetPosition(pLoc, m_pWorld.m_pGrid.m_eShape, (pLoc.m_fHeight + fMinHeight)/2, m_fLandHeightMultiplier);
                    pLine = pLoc.m_pFirstLine;
                    do
                    {
                        m_aLandVertices[cVertexes[pLine.m_pPoint1.m_iID]].Position = GetPosition(pLine.m_pPoint1, m_pWorld.m_pGrid.m_eShape, (pLine.m_pPoint1.m_fHeight + fMinHeight)/2, m_fLandHeightMultiplier);

                        pLine = pLine.m_pNext;
                    }
                    while (pLine != pLoc.m_pFirstLine);
                }

                //последовательно перебирает все связанные линии, пока круг не замкнётся.
                pLine = pLoc.m_pFirstLine; 
                do
                {
                    Model pTree = null;
                    switch (eLT)
                    {
                        case LandType.Forest:
                            pTree = treeModel[Rnd.Get(treeModel.Length)];
                            break;
                        case LandType.Jungle:
                            pTree = Rnd.OneChanceFrom(3) ? treeModel[Rnd.Get(treeModel.Length)] : palmModel[Rnd.Get(palmModel.Length)];
                            break;
                        case LandType.Taiga:
                            pTree = Rnd.OneChanceFrom(3) ? treeModel[Rnd.Get(treeModel.Length)] : pineModel[Rnd.Get(pineModel.Length)];
                            break;
                    }

                    if(pTree != null)
                    {
                        Vector3 pCenter = (m_aLandVertices[cLocations[pLoc.m_iID]].Position +
                                            m_aLandVertices[cVertexes[pLine.m_pPoint2.m_iID]].Position +
                                            m_aLandVertices[cVertexes[pLine.m_pPoint1.m_iID]].Position) / 3;

                        if (pLoc.m_pSettlement == null)
                            cTrees.Add(new TreeModel(pCenter, Rnd.Get((float)Math.PI * 2), pTree, m_pWorld.m_pGrid.m_eShape, treeTexture));

                        if (pLoc.m_pSettlement == null ||
                            pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Hamlet ||
                            pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Village)
                        {
                            cTrees.Add(new TreeModel((pCenter + m_aLandVertices[cLocations[pLoc.m_iID]].Position) / 2, Rnd.Get((float)Math.PI * 2), pTree, m_pWorld.m_pGrid.m_eShape, treeTexture));
                            cTrees.Add(new TreeModel((pCenter + m_aLandVertices[cVertexes[pLine.m_pPoint2.m_iID]].Position) / 2, Rnd.Get((float)Math.PI * 2), pTree, m_pWorld.m_pGrid.m_eShape, treeTexture));
                            cTrees.Add(new TreeModel((pCenter + m_aLandVertices[cVertexes[pLine.m_pPoint1.m_iID]].Position) / 2, Rnd.Get((float)Math.PI * 2), pTree, m_pWorld.m_pGrid.m_eShape, treeTexture));
                        }
                    }

                    //m_aLandVertices[cLocations[pLoc.m_iID]].TexWeights += m_aLandVertices[cVertexes[pLine.m_pPoint2.m_iID]].TexWeights/4;
                    //m_aLandVertices[cLocations[pLoc.m_iID]].TexWeights2 += m_aLandVertices[cVertexes[pLine.m_pPoint2.m_iID]].TexWeights2/4;

                    pLine = pLine.m_pNext;
                }
                while (pLine != pLoc.m_pFirstLine);

                if (pLoc.m_pSettlement != null && pLoc.m_pSettlement.m_iRuinsAge == 0)
                {
                    Texture2D pTexture = m_cSettlementTextures[pLoc.m_pSettlement.m_pInfo.m_eSize][pLoc.m_pSettlement.m_iTechLevel];
                    Model pModel = m_cSettlementModels[pLoc.m_pSettlement.m_pInfo.m_eSize][pLoc.m_pSettlement.m_iTechLevel];
                    float fScale = 0.01f; //0.015f;
                    //switch (pLoc.m_pSettlement.m_pInfo.m_eSize)
                    //{
                    //    case Socium.Settlements.SettlementSize.Hamlet:
                    //        fScale = 0.01f;
                    //        break;
                    //    case Socium.Settlements.SettlementSize.Village:
                    //        fScale = 0.01f;
                    //        break;
                    //    case Socium.Settlements.SettlementSize.Fort:
                    //        fScale = 0.015f;
                    //        break;
                    //    case Socium.Settlements.SettlementSize.Town:
                    //        fScale = 0.015f;
                    //        break;
                    //    case Socium.Settlements.SettlementSize.City:
                    //        fScale = 0.015f;
                    //        break;
                    //    case Socium.Settlements.SettlementSize.Capital:
                    //        fScale = 0.015f;
                    //        break;
                    //}

                    cSettlements.Add(new SettlementModel(m_aLandVertices[cLocations[pLoc.m_iID]].Position,
                                            Rnd.Get((float)Math.PI * 2), fScale*2,
                                            pModel, m_pWorld.m_pGrid.m_eShape, pTexture));
                }

                if (pLoc.m_eType == RegionType.Peak)
                {
                    m_aLandVertices[cLocations[pLoc.m_iID]].TexWeights = new Vector4(0, 0, 1, 1);
                    m_aLandVertices[cLocations[pLoc.m_iID]].TexWeights2 = new Vector4(0, 0, 0, 0);
                }
                if (pLoc.m_eType == RegionType.Volcano)
                {
                    m_aLandVertices[cLocations[pLoc.m_iID]].TexWeights = new Vector4(0, 0, 0, 0);
                    m_aLandVertices[cLocations[pLoc.m_iID]].TexWeights2 = new Vector4(0, 0, 0, 1);
                }
            }

            m_aTrees = cTrees.ToArray();
            m_aSettlements = cSettlements.ToArray();
        }

        private Vector4 GetTexWeights(LandType eLT)
        {
            Vector4 texWeights = new Vector4();
            texWeights.X = (eLT == LandType.Savanna || eLT == LandType.Coastral || eLT == LandType.Desert) ? 1 : 0;
            texWeights.Y = (eLT == LandType.Tundra || eLT == LandType.Savanna || eLT == LandType.Plains || eLT == LandType.Coastral || eLT == LandType.Ocean) ? 1 : 0;
            texWeights.Z = (eLT == LandType.Mountains || eLT == LandType.Coastral || eLT == LandType.Ocean) ? 1 : 0;
            texWeights.W = (eLT == LandType.Tundra) ? 1 : 0;

            return texWeights;
        }

        private Vector4 GetTexWeights2(LandType eLT)
        {
            Vector4 texWeights = new Vector4();
            texWeights.X = (eLT == LandType.Swamp || eLT == LandType.Forest || eLT == LandType.Jungle || eLT == LandType.Taiga) ? 1 : 0;
            texWeights.Y = 0;// (eLT == LandType.Savanna) ? 1 : 0;
            texWeights.Z = (eLT == LandType.Swamp) ? 1 : 0;
            texWeights.W = 0;

            return texWeights;
        }

        /// <summary>
        /// текстурированная поверхность подводной части ландшафта. нужна для построения водной глади
        /// </summary>
        private void BuildUnderWater()
        {
            int iPrimitivesCount = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
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
            m_aUnderwaterVertices = new VertexMultitextured[iPrimitivesCount];

            Dictionary<long, int> cVertexes = new Dictionary<long, int>();
            Dictionary<long, int> cLocations = new Dictionary<long, int>();

            int iCounter = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                VertexMultitextured pVM = new VertexMultitextured();

                bool bOcean = false;
                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    pVM.TexWeights = new Vector4(0);
                    pVM.TexWeights2 = new Vector4(0);

                    LandType eLT = (pLoc.Owner as LandX).Type.m_eType;
                    pVM.TexWeights += GetTexWeights(eLT);
                    pVM.TexWeights2 += GetTexWeights2(eLT);

                    if ((pLoc.Owner as LandX).Type.m_eType == LandType.Ocean ||
                        (pLoc.Owner as LandX).Type.m_eType == LandType.Coastral)
                        bOcean = true;
                }

                if (!bOcean)
                    continue;

                //у нас x и y - это горизонтальная плоскость, причём y растёт в направлении вниз экрана, т.е. как бы к зрителю. а z - это высота.
                //в DX всё не как у людей. У них горизонтальная плоскость - это xz, причём z растёт к зрителю, а y - высота
                pVM.Position = GetPosition(pVertex, m_pWorld.m_pGrid.m_eShape, m_fLandHeightMultiplier);

                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                {
                    float fPhi = (float)Math.Atan2(pVertex.m_fX, pVertex.m_fZ);

                    pVM.TextureCoordinate.X = fPhi * m_pWorld.m_pGrid.RX / ((float)Math.PI * m_fTextureScale);
                }
                else
                {
                    pVM.TextureCoordinate.X = pVertex.m_fX / m_fTextureScale;
                }
                pVM.TextureCoordinate.Y = pVertex.m_fY / m_fTextureScale;

                m_aUnderwaterVertices[iCounter] = pVM;
                cVertexes[pVertex.m_iID] = iCounter;

                iCounter++;
            }

            m_iUnderwaterTrianglesCount = 0;
            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                if (pLoc.Forbidden || pLoc.Owner == null)
                    continue;

                if ((pLoc.Owner as LandX).Type.m_eType != LandType.Ocean &&
                    (pLoc.Owner as LandX).Type.m_eType != LandType.Coastral)
                    continue;

                m_aUnderwaterVertices[iCounter] = new VertexMultitextured();
                float fHeight = pLoc.m_fHeight * m_fLandHeightMultiplier;

                m_aUnderwaterVertices[iCounter].Position = GetPosition(pLoc, m_pWorld.m_pGrid.m_eShape, m_fLandHeightMultiplier);

                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                {
                    float fPhi = (float)Math.Atan2(pLoc.X, pLoc.Z);

                    m_aUnderwaterVertices[iCounter].TextureCoordinate.X = fPhi * m_pWorld.m_pGrid.RX / ((float)Math.PI * m_fTextureScale);
                }
                else
                {
                    m_aUnderwaterVertices[iCounter].TextureCoordinate.X = pLoc.X / m_fTextureScale;
                }
                m_aUnderwaterVertices[iCounter].TextureCoordinate.Y = pLoc.Y / m_fTextureScale;

                LandType eLT = LandType.Ocean;
                if (pLoc.Owner != null)
                    eLT = (pLoc.Owner as LandX).Type.m_eType;
                m_aUnderwaterVertices[iCounter].TexWeights = GetTexWeights(eLT);
                m_aUnderwaterVertices[iCounter].TexWeights2 = GetTexWeights2(eLT);

                cLocations[pLoc.m_iID] = iCounter;

                m_iUnderwaterTrianglesCount += pLoc.m_aBorderWith.Length;

                iCounter++;
            }

            // Create the indices used for each triangle
            m_aUnderwaterIndices = new int[m_iUnderwaterTrianglesCount * 3];

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

                    m_aUnderwaterIndices[iCounter++] = cLocations[pLoc.m_iID];
                    m_aUnderwaterIndices[iCounter++] = cVertexes[pLine.m_pPoint2.m_iID];
                    m_aUnderwaterIndices[iCounter++] = cVertexes[pLine.m_pPoint1.m_iID];

                    pLine = pLine.m_pNext;
                }
                while (pLine != pLoc.m_pFirstLine);
            }
        }

        /// <summary>
        /// текстурированная поверхность водной глади
        /// </summary>
        private void BuildWater()
        {
            int iPrimitivesCount = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
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
            m_aWaterVertices = new VertexPositionTexture[iPrimitivesCount];

            Dictionary<long, int> cVertexes = new Dictionary<long, int>();
            Dictionary<long, int> cLocations = new Dictionary<long, int>();

            int iCounter = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                VertexPositionTexture pVM = new VertexPositionTexture();

                bool bOcean = false;
                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
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

                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                {
                    float fPhi = (float)Math.Atan2(pVertex.m_fX, pVertex.m_fZ);

                    pVM.TextureCoordinate.X = fPhi * m_pWorld.m_pGrid.RX / ((float)Math.PI * 100000);
                }
                else
                {
                    pVM.TextureCoordinate.X = pVertex.m_fX / 100000;
                }
                pVM.TextureCoordinate.Y = pVertex.m_fY / 100000;

                m_aWaterVertices[iCounter] = pVM;
                cVertexes[pVertex.m_iID] = iCounter;

                iCounter++;
            }

            m_iWaterTrianglesCount = 0;
            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                if (pLoc.Forbidden || pLoc.Owner == null)
                    continue;

                if ((pLoc.Owner as LandX).Type.m_eType != LandType.Ocean &&
                    (pLoc.Owner as LandX).Type.m_eType != LandType.Coastral)
                    continue;

                m_aWaterVertices[iCounter] = new VertexPositionTexture();
                float fHeight = pLoc.m_fHeight * m_fLandHeightMultiplier;

                m_aWaterVertices[iCounter].Position = GetPosition(pLoc, m_pWorld.m_pGrid.m_eShape, 0);

                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                {
                    float fPhi = (float)Math.Atan2(pLoc.X, pLoc.Z);

                    m_aWaterVertices[iCounter].TextureCoordinate.X = fPhi * m_pWorld.m_pGrid.RX / ((float)Math.PI * 100000);
                }
                else
                {
                    m_aWaterVertices[iCounter].TextureCoordinate.X = pLoc.X / 100000;
                }
                m_aWaterVertices[iCounter].TextureCoordinate.Y = pLoc.Y / 100000;

                cLocations[pLoc.m_iID] = iCounter;

                m_iWaterTrianglesCount += pLoc.m_aBorderWith.Length;

                iCounter++;
            }

            // Create the indices used for each triangle
            m_aWaterIndices = new int[m_iWaterTrianglesCount * 3];

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

                    m_aWaterIndices[iCounter++] = cLocations[pLoc.m_iID];
                    m_aWaterIndices[iCounter++] = cVertexes[pLine.m_pPoint2.m_iID];
                    m_aWaterIndices[iCounter++] = cVertexes[pLine.m_pPoint1.m_iID];

                    pLine = pLine.m_pNext;
                }
                while (pLine != pLoc.m_pFirstLine);
            }
        }

        private Microsoft.Xna.Framework.Color GetMapModeColor(LocationX pLoc, MapMode pMode)
        {
            if (!pLoc.Forbidden && pLoc.Owner != null)
            {
                switch (pMode)
                {
                    case MapMode.Areas:
                        {
                            if (pLoc.m_eType == RegionType.Peak)
                                return Microsoft.Xna.Framework.Color.White;
                            if (pLoc.m_eType == RegionType.Volcano)
                                return Microsoft.Xna.Framework.Color.Red;

                            return ConvertColor((pLoc.Owner as LandX).Type.m_pColor);
                        }
                    case MapMode.Natives:
                        {
                            if ((pLoc.Owner as LandX).Area != null)
                            { 
                                Nation pNation = ((pLoc.Owner as LandX).Area as AreaX).m_pNation;
                                if (pNation != null)
                                    return m_cNationColorsID[pNation];
                            }
                        }
                        break;
                    case MapMode.Nations:
                        {
                            if ((pLoc.Owner as LandX).m_pProvince != null)
                            {
                                Nation pNation = (pLoc.Owner as LandX).m_pProvince.m_pNation;
                                if (pNation != null)
                                    return m_cNationColorsID[pNation];
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

                                return ConvertColor(color.RGB);
                            }
                        }
                        break;
                    case MapMode.Elevation:
                        {
                            KColor color = new KColor();
                            color.RGB = System.Drawing.Color.Cyan;

                            if (pLoc.m_fHeight < 0)
                            {
                                color.RGB = System.Drawing.Color.Green;
                                color.Hue = 200 + 40 * pLoc.m_fHeight / m_pWorld.m_fMaxDepth;
                            }
                            if (pLoc.m_fHeight > 0)
                            {
                                color.RGB = System.Drawing.Color.Goldenrod;
                                color.Hue = 100 - 100 * pLoc.m_fHeight / m_pWorld.m_fMaxHeight;
                            }

                            return ConvertColor(color.RGB);
                        }
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
                                return ConvertColor(background.RGB);
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
                                
                                return ConvertColor(background.RGB);
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

                                return ConvertColor(background.RGB);
                            }
                        }
                        break;
                }
            }
            return Microsoft.Xna.Framework.Color.DarkBlue;
        }

        /// <summary>
        /// набор примитивов с дублированием вершин на границе разноцветных регионов, так чтобы образовывалась чёткая граница цвета
        /// </summary>
        private void BuildMapModeData(MapMode pMode)
        {
            if (pMode == MapMode.Sattelite)
                return;

            int iPrimitivesCount = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                List<Microsoft.Xna.Framework.Color> cColors = new List<Microsoft.Xna.Framework.Color>();
                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    if (!cColors.Contains(GetMapModeColor(pLoc, pMode)))
                    {
                        cColors.Add(GetMapModeColor(pLoc, pMode));
                        iPrimitivesCount++;
                    }
                }

            }

            // Create the verticies for our triangle
            m_cMapModeData[pMode].m_aVertices = new VertexPositionColorNormal[iPrimitivesCount + m_pWorld.m_pGrid.m_aLocations.Length];

            Dictionary<long, Dictionary<Microsoft.Xna.Framework.Color, int>> cVertexes = new Dictionary<long, Dictionary<Microsoft.Xna.Framework.Color, int>>();
            Dictionary<long, int> cLocations = new Dictionary<long, int>();

            int iCounter = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                cVertexes[pVertex.m_iID] = new Dictionary<Microsoft.Xna.Framework.Color, int>();

                Vector3 pPosition = GetPosition(pVertex, m_pWorld.m_pGrid.m_eShape, pVertex.m_fHeight > 0 ? m_fLandHeightMultiplier : m_fLandHeightMultiplier / 10);

                List<object> cColors = new List<object>();
                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    if (!cColors.Contains(GetMapModeColor(pLoc, pMode)))
                    {
                        cColors.Add(GetMapModeColor(pLoc, pMode));
                        m_cMapModeData[pMode].m_aVertices[iCounter] = new VertexPositionColorNormal();
                        m_cMapModeData[pMode].m_aVertices[iCounter].Position = pPosition;
                        m_cMapModeData[pMode].m_aVertices[iCounter].Color = GetMapModeColor(pLoc, pMode);

                        cVertexes[pVertex.m_iID][GetMapModeColor(pLoc, pMode)] = iCounter;

                        iCounter++;
                    }
                }
            }

            m_cMapModeData[pMode].m_iTrianglesCount = 0;
            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                m_cMapModeData[pMode].m_aVertices[iCounter] = new VertexPositionColorNormal();
                float fHeight = pLoc.m_fHeight;
                //if (pLoc.m_eType == RegionType.Peak)
                //    fHeight += 0.5f * m_fLandHeightMultiplier;
                if (pLoc.m_eType == RegionType.Volcano)
                    fHeight -= 10;

                m_cMapModeData[pMode].m_aVertices[iCounter].Position = GetPosition(pLoc, m_pWorld.m_pGrid.m_eShape, pLoc.m_fHeight > 0 ? m_fLandHeightMultiplier : m_fLandHeightMultiplier / 10);

                m_cMapModeData[pMode].m_aVertices[iCounter].Color = GetMapModeColor(pLoc, pMode);

                cLocations[pLoc.m_iID] = iCounter;

                m_cMapModeData[pMode].m_iTrianglesCount += pLoc.m_aBorderWith.Length;

                iCounter++;
            }

            // Create the indices used for each triangle
            m_cMapModeData[pMode].m_aIndices = new int[m_cMapModeData[pMode].m_iTrianglesCount * 3];

            iCounter = 0;

            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                if (pLoc.Forbidden || pLoc.m_pFirstLine == null)
                    continue;

                float fMinHeight = float.MaxValue;
                Line pLine = pLoc.m_pFirstLine;
                //последовательно перебирает все связанные линии, пока круг не замкнётся.
                do
                {
                    m_cMapModeData[pMode].m_aIndices[iCounter++] = cLocations[pLoc.m_iID];
                    m_cMapModeData[pMode].m_aIndices[iCounter++] = cVertexes[pLine.m_pPoint2.m_iID][GetMapModeColor(pLoc, pMode)];
                    m_cMapModeData[pMode].m_aIndices[iCounter++] = cVertexes[pLine.m_pPoint1.m_iID][GetMapModeColor(pLoc, pMode)];

                    if (fMinHeight > pLine.m_pPoint1.m_fHeight)
                        fMinHeight = pLine.m_pPoint1.m_fHeight;

                    pLine = pLine.m_pNext;
                }
                while (pLine != pLoc.m_pFirstLine);
            
                if (pLoc.m_pSettlement != null)
                {
                    m_cMapModeData[pMode].m_aVertices[cLocations[pLoc.m_iID]].Position = GetPosition(pLoc, m_pWorld.m_pGrid.m_eShape, (pLoc.m_fHeight + fMinHeight) / 2, m_fLandHeightMultiplier);
                    pLine = pLoc.m_pFirstLine;
                    do
                    {
                        foreach (var pVert in cVertexes[pLine.m_pPoint1.m_iID])
                        {
                            m_cMapModeData[pMode].m_aVertices[pVert.Value].Position = GetPosition(pLine.m_pPoint1, m_pWorld.m_pGrid.m_eShape, (pLine.m_pPoint1.m_fHeight + fMinHeight) / 2, m_fLandHeightMultiplier);
                        }

                        pLine = pLine.m_pNext;
                    }
                    while (pLine != pLoc.m_pFirstLine);
                }
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
        private static void CalculateNormals(VertexMultitextured[] vertexBuffer, int[] indexBuffer, int iTrianglesCount)
        {
            for (int i = 0; i < vertexBuffer.Length; i++)
                vertexBuffer[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < iTrianglesCount; i++)
            {
                int index1 = indexBuffer[i * 3];
                int index2 = indexBuffer[i * 3 + 1];
                int index3 = indexBuffer[i * 3 + 2];

                if (index1 == 0 && index2 == 0 && index3 == 0)
                    continue;

                Vector3 side1 = vertexBuffer[index1].Position - vertexBuffer[index3].Position;
                Vector3 side2 = vertexBuffer[index1].Position - vertexBuffer[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertexBuffer[index1].Normal += normal;
                vertexBuffer[index2].Normal += normal;
                vertexBuffer[index3].Normal += normal;
            }

            for (int i = 0; i < vertexBuffer.Length; i++)
            {
                vertexBuffer[i].Normal.Normalize();

                float fD = (float)Math.Sqrt(vertexBuffer[i].TexWeights.LengthSquared() + vertexBuffer[i].TexWeights2.LengthSquared());
                vertexBuffer[i].TexWeights /= fD;
                vertexBuffer[i].TexWeights2 /= fD;
            }
        }

        /// <summary>
        /// для всех вершин в списке вычисляем нормаль как нормализованный вектор суммы нормалей прилегающих граней
        /// </summary>
        private static void CalculateNormals(VertexPositionColorNormal[] vertexBuffer, int[] indexBuffer, int iTrianglesCount)
        {
            for (int i = 0; i < vertexBuffer.Length; i++)
                vertexBuffer[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < iTrianglesCount; i++)
            {
                int index1 = indexBuffer[i * 3];
                int index2 = indexBuffer[i * 3 + 1];
                int index3 = indexBuffer[i * 3 + 2];

                if (index1 == 0 && index2 == 0 && index3 == 0)
                    continue;

                Vector3 side1 = vertexBuffer[index1].Position - vertexBuffer[index3].Position;
                Vector3 side2 = vertexBuffer[index1].Position - vertexBuffer[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertexBuffer[index1].Normal += normal;
                vertexBuffer[index2].Normal += normal;
                vertexBuffer[index3].Normal += normal;
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
            
            FillNationColors();

            if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                m_fLandHeightMultiplier = 7f / m_pWorld.m_fMaxHeight;
            else
                m_fLandHeightMultiplier = 3.5f / m_pWorld.m_fMaxHeight;

            BuildLand();
            BuildUnderWater();
            BuildWater();

            //FillPrimitivesFake();
            CalculateNormals(m_aLandVertices, m_aLandIndices, m_iLandTrianglesCount);
            CalculateNormals(m_aUnderwaterVertices, m_aUnderwaterIndices, m_iUnderwaterTrianglesCount);

            foreach (MapMode pMode in Enum.GetValues(typeof(MapMode)))
            {
                BuildMapModeData(pMode);
                CalculateNormals(m_cMapModeData[pMode].m_aVertices, m_cMapModeData[pMode].m_aIndices, m_cMapModeData[pMode].m_iTrianglesCount);
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
                else
                {
                    m_pCamera = new PlainCamera(GraphicsDevice);
                    m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Land"];
                    pEffectDirectionalLightDirection.SetValue(Vector3.Normalize(new Vector3(1, -1, -1)));
                }
            }
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
        Texture2D savannaTexture;
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
            savannaTexture = LibContent.Load<Texture2D>("content/dds/plain");
            swampTexture = LibContent.Load<Texture2D>("content/dds/river");
            lavaTexture = LibContent.Load<Texture2D>("content/dds/1-lava");
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

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            foreach (MapMode pMode in Enum.GetValues(typeof(MapMode)))
                m_cMapModeData[pMode] = new MapModeData();

            m_pBasicEffect = new BasicEffect(GraphicsDevice);
            m_pBasicEffect.VertexColorEnabled = true;

            LibContent = new ContentManager(Services);

            // Create our effect.
            LoadTerrainTextures();

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
            pEffectTexture5.SetValue(savannaTexture);
            pEffectTexture6.SetValue(swampTexture);
            pEffectTexture7.SetValue(lavaTexture);

            // create the effect and vertex declaration for drawing the
            // picked triangle.
            lineEffect = new BasicEffect(GraphicsDevice);
            lineEffect.VertexColorEnabled = true;

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            refractionRenderTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat);
            
            if (m_pWorld != null && m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                m_pCamera = new RingworldCamera(m_pWorld.m_pGrid.RX / ((float)Math.PI * 1000), GraphicsDevice);
            else
                m_pCamera = new PlainCamera(GraphicsDevice);


            LoadTrees();

            LoadSettlements();

            // Start the animation timer.
            timer = Stopwatch.StartNew();
            lastTime = timer.Elapsed.TotalMilliseconds;

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        /// <summary>
        /// effect2 должен уже быть создан и настроен!
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
            m_pCamera.Update();
            if (m_bPanMode && m_pCurrentPicking != null)
            {
                if (m_pLastPicking != null)
                    m_pCamera.Target += (Vector3)m_pLastPicking - (Vector3)m_pCurrentPicking;

               // m_pLastPicking = m_pCurrentPicking;
                m_pCurrentPicking = null;
            }

            pEffectView.SetValue(m_pCamera.View);
            pEffectProjection.SetValue(m_pCamera.Projection);

            pEffectCameraPosition.SetValue(m_pCamera.Position);

            pEffectWorld.SetValue(Matrix.Identity);

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rs;

            if(m_eMode == MapMode.Sattelite)
            {
                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                    m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["LandRingworld"];
                else
                    m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Land"];
                m_pMyEffect.CurrentTechnique.Passes[0].Apply();

                GraphicsDevice.SetRenderTarget(refractionRenderTarget);
                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Microsoft.Xna.Framework.Color.Black, 1.0f, 0);
                GraphicsDevice.DrawUserIndexedPrimitives<VertexMultitextured>(PrimitiveType.TriangleList,
                                                    m_aUnderwaterVertices, 0, m_aUnderwaterVertices.Length - 1, m_aUnderwaterIndices, 0, m_iUnderwaterTrianglesCount);
                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Clear(eSkyColor);

                GraphicsDevice.DrawUserIndexedPrimitives<VertexMultitextured>(PrimitiveType.TriangleList,
                                                    m_aLandVertices, 0, m_aLandVertices.Length - 1, m_aLandIndices, 0, m_iLandTrianglesCount);
                DrawWater(time);

                for (int i = 0; i < m_aTrees.Length; i++)
                    DrawTree(m_aTrees[i]);
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

                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorNormal>(PrimitiveType.TriangleList,
                                                    m_cMapModeData[m_eMode].m_aVertices, 0, m_cMapModeData[m_eMode].m_aVertices.Length - 1, m_cMapModeData[m_eMode].m_aIndices, 0, m_cMapModeData[m_eMode].m_iTrianglesCount);
            }

            for (int i = 0; i < m_aSettlements.Length; i++)
                DrawSettlement(m_aSettlements[i]);

            // Draw the outline of the triangle under the cursor.
            DrawPickedTriangle();
        }

        private void DrawWater(float time)
        {
            m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Water"];
            //effect.Parameters["xReflectionView"].SetValue(reflectionViewMatrix);
            //effect.Parameters["xReflectionMap"].SetValue(reflectionMap);
            m_pMyEffect.Parameters["xRefractionMap"].SetValue(refractionRenderTarget);

            m_pMyEffect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList,
                                                m_aWaterVertices, 0, m_aWaterVertices.Length - 1, m_aWaterIndices, 0, m_iWaterTrianglesCount);
        }

        private void DrawTree(TreeModel pTree)
        {
            Vector3 pViewVector = pTree.m_pPosition - m_pCamera.Position;

            float fCos = Vector3.Dot(Vector3.Normalize(pViewVector), m_pCamera.Direction);
            if (fCos < 0.6) //cos(45) = 0,70710678118654752440084436210485...
                return;

            if (pViewVector.LengthSquared() > 2500)
                return;

            Matrix[] xwingTransforms = new Matrix[pTree.m_pModel.Bones.Count];
            pTree.m_pModel.CopyAbsoluteBoneTransformsTo(xwingTransforms);
            foreach (ModelMesh mesh in pTree.m_pModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    currentEffect.Parameters["World"].SetValue(xwingTransforms[mesh.ParentBone.Index] * pTree.worldMatrix); 
                    currentEffect.Parameters["View"].SetValue(m_pCamera.View);
                    currentEffect.Parameters["CameraPosition"].SetValue(m_pCamera.Position);
                    currentEffect.Parameters["Projection"].SetValue(m_pCamera.Projection);
                }
                mesh.Draw();
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

        public void FocusSelectedState()
        {
            m_pCamera.Target = new Vector3(m_pSelectedState.X, m_pSelectedState.Y, m_pSelectedState.Z);
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
                rs.FillMode = FillMode.WireFrame;
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
            float? intersection = RayIntersectsLandscape(CullMode.CullCounterClockwiseFace, cursorRay, m_aLandVertices, m_aLandIndices,
                                                        out vertex1, out vertex2,
                                                        out vertex3);
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
    }
}
