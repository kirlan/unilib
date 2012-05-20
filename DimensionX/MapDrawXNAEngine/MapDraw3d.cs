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

            public TreeModel(Vector3 pPosition, float fAngle, Model pModel)
            {
                m_pPosition = pPosition;
                m_fAngle = fAngle;
                m_pModel = pModel;
            }
        }

        protected class SettlementModel
        {
            public readonly Vector3 m_pPosition;
            public readonly float m_fAngle;
            public readonly Model m_pModel;
            public readonly float m_fScale;

            public SettlementModel(Vector3 pPosition, float fAngle, float fScale, Model pModel)
            {
                m_pPosition = pPosition;
                m_fAngle = fAngle;
                m_fScale = fScale;
                m_pModel = pModel;
            }
        }

        /// <summary>
        /// мир, карту которого мы рисуем
        /// </summary>
        internal World m_pWorld = null;

        //BasicEffect effect;
        Effect effect2;
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

        private float m_fHeightMultiplier = 0.1f;

        private void FillPrimitivesFake()
        {
            m_aLandVertices = new VertexMultitextured[5];

            m_aLandVertices[0] = new VertexMultitextured();
            m_aLandVertices[0].Position = new Vector3(-m_pWorld.m_pGrid.RX, 0, -m_pWorld.m_pGrid.RY);
            //userPrimitives[0].Color = Microsoft.Xna.Framework.Color.White;
            m_aLandVertices[0].TextureCoordinate.X = 0;
            m_aLandVertices[0].TextureCoordinate.Y = 0;
            m_aLandVertices[0].TexWeights.X = 1;
            m_aLandVertices[0].TexWeights.Y = 0;
            m_aLandVertices[0].TexWeights.Z = 0;
            m_aLandVertices[0].TexWeights.W = 0;
            m_aLandVertices[0].TexWeights2.X = 0;
            m_aLandVertices[0].TexWeights2.Y = 0;
            m_aLandVertices[0].TexWeights2.Z = 0;
            m_aLandVertices[0].TexWeights2.W = 0;

            m_aLandVertices[1] = new VertexMultitextured();
            m_aLandVertices[1].Position = new Vector3(m_pWorld.m_pGrid.RX, 0, -m_pWorld.m_pGrid.RY);
            //userPrimitives[1].Color = Microsoft.Xna.Framework.Color.Red;
            m_aLandVertices[1].TextureCoordinate.X = 200;
            m_aLandVertices[1].TextureCoordinate.Y = 0;
            m_aLandVertices[1].TexWeights.X = 0;
            m_aLandVertices[1].TexWeights.Y = 1;
            m_aLandVertices[1].TexWeights.Z = 0;
            m_aLandVertices[1].TexWeights.W = 0;
            m_aLandVertices[1].TexWeights2.X = 0;
            m_aLandVertices[1].TexWeights2.Y = 0;
            m_aLandVertices[1].TexWeights2.Z = 0;
            m_aLandVertices[1].TexWeights2.W = 0;

            m_aLandVertices[2] = new VertexMultitextured();
            m_aLandVertices[2].Position = new Vector3(m_pWorld.m_pGrid.RX, 0, m_pWorld.m_pGrid.RY);
            //userPrimitives[2].Color = Microsoft.Xna.Framework.Color.Blue;
            m_aLandVertices[2].TextureCoordinate.X = 200;
            m_aLandVertices[2].TextureCoordinate.Y = 200;
            m_aLandVertices[2].TexWeights.X = 0;
            m_aLandVertices[2].TexWeights.Y = 0;
            m_aLandVertices[2].TexWeights.Z = 1;
            m_aLandVertices[2].TexWeights.W = 0;
            m_aLandVertices[2].TexWeights2.X = 0;
            m_aLandVertices[2].TexWeights2.Y = 0;
            m_aLandVertices[2].TexWeights2.Z = 0;
            m_aLandVertices[2].TexWeights2.W = 0;

            m_aLandVertices[3] = new VertexMultitextured();
            m_aLandVertices[3].Position = new Vector3(-m_pWorld.m_pGrid.RX, 0, m_pWorld.m_pGrid.RY);
            //userPrimitives[3].Color = Microsoft.Xna.Framework.Color.Green;
            m_aLandVertices[3].TextureCoordinate.X = 0;
            m_aLandVertices[3].TextureCoordinate.Y = 200;
            m_aLandVertices[3].TexWeights.X = 0;
            m_aLandVertices[3].TexWeights.Y = 0;
            m_aLandVertices[3].TexWeights.Z = 0;
            m_aLandVertices[3].TexWeights.W = 1;
            m_aLandVertices[3].TexWeights2.X = 0;
            m_aLandVertices[3].TexWeights2.Y = 0;
            m_aLandVertices[3].TexWeights2.Z = 0;
            m_aLandVertices[3].TexWeights2.W = 0;

            m_aLandVertices[4] = new VertexMultitextured();
            m_aLandVertices[4].Position = new Vector3(0, 0, 0);
            //userPrimitives[4].Color = Microsoft.Xna.Framework.Color.Black;
            m_aLandVertices[4].TextureCoordinate.X = 100;
            m_aLandVertices[4].TextureCoordinate.Y = 100;
            m_aLandVertices[4].TexWeights.X = 0.25f;
            m_aLandVertices[4].TexWeights.Y = 0.25f;
            m_aLandVertices[4].TexWeights.Z = 0.25f;
            m_aLandVertices[4].TexWeights.W = 0.25f;
            m_aLandVertices[4].TexWeights2.X = 0;
            m_aLandVertices[4].TexWeights2.Y = 0;
            m_aLandVertices[4].TexWeights2.Z = 0;
            m_aLandVertices[4].TexWeights2.W = 0;

            m_iLandTrianglesCount = 4;
            m_aLandIndices = new int[m_iLandTrianglesCount * 3];

            m_aLandIndices[0] = 4;
            m_aLandIndices[1] = 0;
            m_aLandIndices[2] = 1;

            m_aLandIndices[3] = 4;
            m_aLandIndices[4] = 1;
            m_aLandIndices[5] = 2;

            m_aLandIndices[6] = 4;
            m_aLandIndices[7] = 2;
            m_aLandIndices[8] = 3;

            m_aLandIndices[9] = 4;
            m_aLandIndices[10] = 3;
            m_aLandIndices[11] = 0;
        }

        private TreeModel[] m_aTrees = new TreeModel[0];

        private SettlementModel[] m_aSettlements = new SettlementModel[0];

        /// <summary>
        /// набор примитивов без дублирования вертексов, с плавным переходом цвета между разноцветными примитивами
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

                    pVM.TexWeights.X = 0;
                    pVM.TexWeights.Y = 0;
                    pVM.TexWeights.Z = 0;
                    pVM.TexWeights.W = 0;
                    pVM.TexWeights2.X = 0;
                    pVM.TexWeights2.Y = 0;
                    pVM.TexWeights2.Z = 0;
                    pVM.TexWeights2.W = 0;

                    LandType eLT = (pLoc.Owner as LandX).Type.m_eType;
                    pVM.TexWeights.X += (eLT == LandType.Savanna || eLT == LandType.Coastral || eLT == LandType.Desert) ? 1 : 0;
                    pVM.TexWeights.Y += (eLT == LandType.Tundra || eLT == LandType.Savanna || eLT == LandType.Plains) ? 1 : 0;
                    pVM.TexWeights.Z += (eLT == LandType.Mountains || eLT == LandType.Ocean) ? 1 : 0;
                    pVM.TexWeights.W += (eLT == LandType.Tundra) ? 1 : 0;

                    pVM.TexWeights2.X += (eLT == LandType.Swamp || eLT == LandType.Forest || eLT == LandType.Jungle || eLT == LandType.Taiga) ? 1 : 0;
                    //pVM.TexWeights2.Y += (eLT == LandType.Savanna) ? 1 : 0;
                    pVM.TexWeights2.Z += (eLT == LandType.Swamp) ? 1 : 0;
                    //pVM.TexWeights2.W += 0;
                }

                //у нас x и y - это горизонтальная плоскость, причём y растёт в направлении вниз экрана, т.е. как бы к зрителю. а z - это высота.
                //в DX всё не как у людей. У них горизонтальная плоскость - это xz, причём z растёт к зрителю, а y - высота
                pVM.Position = new Vector3(pVertex.m_fX / 1000, pVertex.m_fZ / 1000, pVertex.m_fY / 1000);
                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                    pVM.Position -= Vector3.Normalize(pVM.Position) * pVertex.m_fHeight * m_fHeightMultiplier * (pVertex.m_fHeight > 0 ? 1 : 10);
                else
                    pVM.Position += Vector3.Up * pVertex.m_fHeight * m_fHeightMultiplier * (pVertex.m_fHeight > 0 ? 1 : 10);

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
                float fHeight = pLoc.m_fHeight * m_fHeightMultiplier;
                if (pLoc.m_eType == RegionType.Peak)
                    fHeight += 2 * m_fHeightMultiplier;
                if (pLoc.m_eType == RegionType.Volcano)
                    fHeight -= 10 * m_fHeightMultiplier;

                m_aLandVertices[iCounter].Position = new Vector3(pLoc.X / 1000, pLoc.Z / 1000, pLoc.Y / 1000);
                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                    m_aLandVertices[iCounter].Position -= Vector3.Normalize(m_aLandVertices[iCounter].Position) * fHeight * (pLoc.m_fHeight > 0 ? 1 : 10);
                else
                    m_aLandVertices[iCounter].Position += Vector3.Up * fHeight * (pLoc.m_fHeight > 0 ? 1 : 10);

                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                {
                    float fPhi = (float)Math.Atan2(pLoc.X, pLoc.Z);

                    m_aLandVertices[iCounter].TextureCoordinate.X = fPhi * m_pWorld.m_pGrid.RX / ((float)Math.PI * 100000);
                }
                else
                {
                    m_aLandVertices[iCounter].TextureCoordinate.X = pLoc.X / 100000;
                }
                m_aLandVertices[iCounter].TextureCoordinate.Y = pLoc.Y / 100000;

                LandType eLT = LandType.Ocean;
                if (pLoc.Owner != null)
                    eLT = (pLoc.Owner as LandX).Type.m_eType;
                m_aLandVertices[iCounter].TexWeights.X = (eLT == LandType.Savanna || eLT == LandType.Coastral || eLT == LandType.Desert) ? 1 : 0;
                m_aLandVertices[iCounter].TexWeights.Y = (eLT == LandType.Tundra || eLT == LandType.Savanna || eLT == LandType.Plains) ? 1 : 0;
                m_aLandVertices[iCounter].TexWeights.Z = (eLT == LandType.Mountains || eLT == LandType.Ocean) ? 1 : 0;
                m_aLandVertices[iCounter].TexWeights.W = (eLT == LandType.Tundra) ? 1 : 0;
                m_aLandVertices[iCounter].TexWeights2.X = (eLT == LandType.Swamp || eLT == LandType.Forest || eLT == LandType.Jungle || eLT == LandType.Taiga) ? 1 : 0;
                m_aLandVertices[iCounter].TexWeights2.Y = 0;// (eLT == LandType.Savanna) ? 1 : 0;
                m_aLandVertices[iCounter].TexWeights2.Z = (eLT == LandType.Swamp) ? 1 : 0;
                m_aLandVertices[iCounter].TexWeights2.W = 0;

                foreach (LocationX pLink in pLoc.m_aBorderWith)
                {
                    if (pLink.Forbidden || pLink.Owner == null)
                        continue;

                    float fWeight = 1.0f / (float)pLoc.m_aBorderWith.Length;

                    eLT = (pLink.Owner as LandX).Type.m_eType;
                    m_aLandVertices[iCounter].TexWeights.X += (eLT == LandType.Savanna || eLT == LandType.Coastral || eLT == LandType.Desert) ? fWeight : 0;
                    m_aLandVertices[iCounter].TexWeights.Y += (eLT == LandType.Tundra || eLT == LandType.Savanna || eLT == LandType.Plains) ? fWeight : 0;
                    m_aLandVertices[iCounter].TexWeights.Z += (eLT == LandType.Mountains || eLT == LandType.Ocean) ? fWeight : 0;
                    m_aLandVertices[iCounter].TexWeights.W += (eLT == LandType.Tundra) ? fWeight : 0;
                    m_aLandVertices[iCounter].TexWeights2.X += (eLT == LandType.Swamp || eLT == LandType.Forest || eLT == LandType.Jungle || eLT == LandType.Taiga) ? fWeight : 0;
                    //m_aLandVertices[iCounter].TexWeights2.Y += (eLT == LandType.Savanna) ? fWeight : 0;
                    m_aLandVertices[iCounter].TexWeights2.Z += (eLT == LandType.Swamp) ? fWeight : 0;
                }

                if (pLoc.m_eType == RegionType.Peak)
                {
                    m_aLandVertices[iCounter].TexWeights = new Vector4(0, 0, 1, 1);
                    m_aLandVertices[iCounter].TexWeights2 = new Vector4(0, 0, 0, 0);
                }
                if (pLoc.m_eType == RegionType.Volcano)
                {
                    m_aLandVertices[iCounter].TexWeights = new Vector4(0, 0, 0, 0);
                    m_aLandVertices[iCounter].TexWeights2 = new Vector4(0, 0, 0, 1);
                }

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
                
                Line pLine = pLoc.m_pFirstLine;
                //последовательно перебирает все связанные линии, пока круг не замкнётся.
                do
                {

                    m_aLandIndices[iCounter++] = cLocations[pLoc.m_iID];
                    m_aLandIndices[iCounter++] = cVertexes[pLine.m_pPoint2.m_iID];
                    m_aLandIndices[iCounter++] = cVertexes[pLine.m_pPoint1.m_iID];

                    pLine = pLine.m_pNext;

                    if (pLoc.m_pSettlement == null)
                    {
                        if (eLT == LandType.Forest)
                            cTrees.Add(new TreeModel((m_aLandVertices[cLocations[pLoc.m_iID]].Position +
                                                      m_aLandVertices[cVertexes[pLine.m_pPoint2.m_iID]].Position +
                                                      m_aLandVertices[cVertexes[pLine.m_pPoint1.m_iID]].Position) / 3,
                                                     Rnd.Get((float)Math.PI * 2),
                                                     treeModel[Rnd.Get(treeModel.Length)]));
                        if (eLT == LandType.Jungle)
                            cTrees.Add(new TreeModel((m_aLandVertices[cLocations[pLoc.m_iID]].Position +
                                                      m_aLandVertices[cVertexes[pLine.m_pPoint2.m_iID]].Position +
                                                      m_aLandVertices[cVertexes[pLine.m_pPoint1.m_iID]].Position) / 3,
                                                     Rnd.Get((float)Math.PI * 2),
                                                     Rnd.OneChanceFrom(3) ? treeModel[Rnd.Get(treeModel.Length)] : palmModel[Rnd.Get(palmModel.Length)]));
                        if (eLT == LandType.Taiga)
                            cTrees.Add(new TreeModel((m_aLandVertices[cLocations[pLoc.m_iID]].Position +
                                                      m_aLandVertices[cVertexes[pLine.m_pPoint2.m_iID]].Position +
                                                      m_aLandVertices[cVertexes[pLine.m_pPoint1.m_iID]].Position) / 3,
                                                     Rnd.Get((float)Math.PI * 2),
                                                     Rnd.OneChanceFrom(3) ? treeModel[Rnd.Get(treeModel.Length)] : pineModel[Rnd.Get(pineModel.Length)]));
                    }
                }
                while (pLine != pLoc.m_pFirstLine);

                if (pLoc.m_pSettlement != null && pLoc.m_pSettlement.m_iRuinsAge == 0)
                {
                    if (pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Capital ||
                       pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.City)
                        cSettlements.Add(new SettlementModel(m_aLandVertices[cLocations[pLoc.m_iID]].Position,
                                             Rnd.Get((float)Math.PI * 2), 0.15f,
                                             cityModel));
                    if (pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Hamlet ||
                       pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Village)
                        cSettlements.Add(new SettlementModel(m_aLandVertices[cLocations[pLoc.m_iID]].Position,
                                             Rnd.Get((float)Math.PI * 2), 0.08f,
                                             villageModel));
                    if (pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Town)
                        cSettlements.Add(new SettlementModel(m_aLandVertices[cLocations[pLoc.m_iID]].Position,
                                             Rnd.Get((float)Math.PI * 2), 0.1f,
                                             townModel));
                    if (pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Fort)
                        cSettlements.Add(new SettlementModel(m_aLandVertices[cLocations[pLoc.m_iID]].Position,
                                             Rnd.Get((float)Math.PI * 2), 0.1f,
                                             fortModel));
                }
            }

            m_aTrees = cTrees.ToArray();
            m_aSettlements = cSettlements.ToArray();
        }
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

                    pVM.TexWeights.X = 0;
                    pVM.TexWeights.Y = 0;
                    pVM.TexWeights.Z = 0;
                    pVM.TexWeights.W = 0;

                    LandType eLT = (pLoc.Owner as LandX).Type.m_eType;
                    pVM.TexWeights.X += (eLT == LandType.Coastral || eLT == LandType.Desert) ? 1 : 0;
                    pVM.TexWeights.Y += (eLT == LandType.Forest || eLT == LandType.Jungle || eLT == LandType.Plains || eLT == LandType.Savanna || eLT == LandType.Swamp || eLT == LandType.Taiga) ? 1 : 0;
                    pVM.TexWeights.Z += (eLT == LandType.Mountains || eLT == LandType.Ocean) ? 1 : 0;
                    pVM.TexWeights.W += (eLT == LandType.Tundra) ? 1 : 0;

                    if ((pLoc.Owner as LandX).Type.m_eType == LandType.Ocean ||
                        (pLoc.Owner as LandX).Type.m_eType == LandType.Coastral)
                        bOcean = true;
                }

                if (!bOcean)
                    continue;

                //у нас x и y - это горизонтальная плоскость, причём y растёт в направлении вниз экрана, т.е. как бы к зрителю. а z - это высота.
                //в DX всё не как у людей. У них горизонтальная плоскость - это xz, причём z растёт к зрителю, а y - высота
                pVM.Position = new Vector3(pVertex.m_fX / 1000, pVertex.m_fZ / 1000, pVertex.m_fY / 1000);
                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                    pVM.Position -= Vector3.Normalize(pVM.Position) * pVertex.m_fHeight * m_fHeightMultiplier * 10;
                else
                    pVM.Position += Vector3.Up * pVertex.m_fHeight * m_fHeightMultiplier* 10;

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
                float fHeight = pLoc.m_fHeight * m_fHeightMultiplier * 10;

                m_aUnderwaterVertices[iCounter].Position = new Vector3(pLoc.X / 1000, pLoc.Z / 1000, pLoc.Y / 1000);
                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                    m_aUnderwaterVertices[iCounter].Position -= Vector3.Normalize(m_aUnderwaterVertices[iCounter].Position) * fHeight;
                else
                    m_aUnderwaterVertices[iCounter].Position += Vector3.Up * fHeight;

                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                {
                    float fPhi = (float)Math.Atan2(pLoc.X, pLoc.Z);

                    m_aUnderwaterVertices[iCounter].TextureCoordinate.X = fPhi * m_pWorld.m_pGrid.RX / ((float)Math.PI * 100000);
                }
                else
                {
                    m_aUnderwaterVertices[iCounter].TextureCoordinate.X = pLoc.X / 100000;
                }
                m_aUnderwaterVertices[iCounter].TextureCoordinate.Y = pLoc.Y / 100000;

                LandType eLT = LandType.Ocean;
                if (pLoc.Owner != null)
                    eLT = (pLoc.Owner as LandX).Type.m_eType;
                m_aUnderwaterVertices[iCounter].TexWeights.X = eLT == LandType.Coastral ? 1 : 0;
                m_aUnderwaterVertices[iCounter].TexWeights.Y = 0;
                m_aUnderwaterVertices[iCounter].TexWeights.Z = eLT == LandType.Ocean ? 1 : 0;
                m_aUnderwaterVertices[iCounter].TexWeights.W = 0;

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
                pVM.Position = new Vector3(pVertex.m_fX / 1000, pVertex.m_fZ / 1000, pVertex.m_fY / 1000);
                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                    pVM.Position -= Vector3.Normalize(pVM.Position) * (pVertex.m_fHeight > 0 ? pVertex.m_fHeight * m_fHeightMultiplier : 0);
                else
                    pVM.Position += Vector3.Up * (pVertex.m_fHeight > 0 ? pVertex.m_fHeight * m_fHeightMultiplier : 0);

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
                float fHeight = pLoc.m_fHeight * m_fHeightMultiplier;

                m_aWaterVertices[iCounter].Position = new Vector3(pLoc.X / 1000, pLoc.Z / 1000, pLoc.Y / 1000);
                if (fHeight > 0)
                {
                    if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                        m_aLandVertices[iCounter].Position -= Vector3.Normalize(m_aLandVertices[iCounter].Position) * fHeight;
                    else
                        m_aLandVertices[iCounter].Position += Vector3.Up * fHeight;
                }

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
            m_aLandVertices = new VertexMultitextured[iPrimitivesCount + m_pWorld.m_pGrid.m_aLocations.Length];

            Dictionary<long, Dictionary<LandTypeInfoX, int>> cVertexes = new Dictionary<long, Dictionary<LandTypeInfoX, int>>();
            Dictionary<long, int> cLocations = new Dictionary<long, int>();

            if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                m_fHeightMultiplier = 0.2f;
            else
                m_fHeightMultiplier = 0.1f;

            int iCounter = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                cVertexes[pVertex.m_iID] = new Dictionary<LandTypeInfoX, int>();

                //у нас x и y - это горизонтальная плоскость, причём y растёт в направлении вниз экрана, т.е. как бы к зрителю. а z - это высота.
                //в DX всё не как у людей. У них горизонтальная плоскость - это xz, причём z растёт к зрителю, а y - высота
                Vector3 pPosition = new Vector3(pVertex.m_fX / 1000, pVertex.m_fZ / 1000, pVertex.m_fY / 1000);
                if(m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                    pPosition -= Vector3.Normalize(pPosition) * (pVertex.m_fHeight > 0 ? pVertex.m_fHeight * m_fHeightMultiplier : pVertex.m_fHeight * m_fHeightMultiplier / 10);
                else
                    pPosition += Vector3.Up * (pVertex.m_fHeight > 0 ? pVertex.m_fHeight * m_fHeightMultiplier : pVertex.m_fHeight * m_fHeightMultiplier/10);

                List<LandTypeInfoX> cTypes = new List<LandTypeInfoX>();
                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    if (!cTypes.Contains((pLoc.Owner as LandX).Type))
                    {
                        cTypes.Add((pLoc.Owner as LandX).Type);
                        m_aLandVertices[iCounter] = new VertexMultitextured();
                        m_aLandVertices[iCounter].Position = pPosition;
                        //userPrimitives[iCounter].Color = ConvertColor((pLoc.Owner as LandX).Type.m_pColor);

                        if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                        {
                            float fPhi = (float)Math.Atan2(pVertex.m_fX, pVertex.m_fZ);

                            m_aLandVertices[iCounter].TextureCoordinate.X = fPhi * m_pWorld.m_pGrid.RX / ((float)Math.PI * 100000);
                        }
                        else
                        {
                            m_aLandVertices[iCounter].TextureCoordinate.X = pVertex.m_fX / 100000;
                        }
                        m_aLandVertices[iCounter].TextureCoordinate.Y = pVertex.m_fY / 100000;
                        LandType eLT = (pLoc.Owner as LandX).Type.m_eType;
                        m_aLandVertices[iCounter].TexWeights.X = (eLT == LandType.Coastral ||eLT == LandType.Desert) ? 1:0;
                        m_aLandVertices[iCounter].TexWeights.Y = (eLT == LandType.Forest || eLT == LandType.Jungle || eLT == LandType.Plains || eLT == LandType.Savanna || eLT == LandType.Swamp || eLT == LandType.Taiga) ? 1 : 0;
                        m_aLandVertices[iCounter].TexWeights.Z = (eLT == LandType.Mountains || eLT == LandType.Ocean) ? 1 : 0;
                        m_aLandVertices[iCounter].TexWeights.W = (eLT == LandType.Tundra) ? 1 : 0;

                        cVertexes[pVertex.m_iID][(pLoc.Owner as LandX).Type] = iCounter;

                        iCounter++;
                    }
                }
            }

            m_iLandTrianglesCount = 0;
            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                m_aLandVertices[iCounter] = new VertexMultitextured();
                float fHeight = pLoc.m_fHeight > 0 ? pLoc.m_fHeight * m_fHeightMultiplier : pLoc.m_fHeight * m_fHeightMultiplier/10;
                if (pLoc.m_eType == RegionType.Peak)
                    fHeight += 2 * m_fHeightMultiplier;
                if (pLoc.m_eType == RegionType.Volcano)
                    fHeight -= 10 * m_fHeightMultiplier;

                m_aLandVertices[iCounter].Position = new Vector3(pLoc.X / 1000, pLoc.Z / 1000, pLoc.Y / 1000);
                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                    m_aLandVertices[iCounter].Position -= Vector3.Normalize(m_aLandVertices[iCounter].Position) * fHeight;
                else
                    m_aLandVertices[iCounter].Position += Vector3.Up * fHeight;

                //if (pLoc.m_eType == RegionType.Peak)
                //    userPrimitives[iCounter].Color = Microsoft.Xna.Framework.Color.White;
                //if (pLoc.m_eType == RegionType.Volcano)
                //    userPrimitives[iCounter].Color = Microsoft.Xna.Framework.Color.Red;
                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                {
                    float fPhi = (float)Math.Atan2(pLoc.X, pLoc.Z);

                    m_aLandVertices[iCounter].TextureCoordinate.X = fPhi * m_pWorld.m_pGrid.RX / ((float)Math.PI * 100000);
                }
                else
                {
                    m_aLandVertices[iCounter].TextureCoordinate.X = pLoc.X / 100000;
                }
                m_aLandVertices[iCounter].TextureCoordinate.Y = pLoc.Y / 100000;
                LandType eLT = LandType.Ocean;
                if (pLoc.Owner != null)
                    eLT = (pLoc.Owner as LandX).Type.m_eType;
                m_aLandVertices[iCounter].TexWeights.X = (eLT == LandType.Coastral || eLT == LandType.Desert) ? 1 : 0;
                m_aLandVertices[iCounter].TexWeights.Y = (eLT == LandType.Forest || eLT == LandType.Jungle || eLT == LandType.Plains || eLT == LandType.Savanna || eLT == LandType.Swamp || eLT == LandType.Taiga) ? 1 : 0;
                m_aLandVertices[iCounter].TexWeights.Z = (eLT == LandType.Mountains || eLT == LandType.Ocean) ? 1 : 0;
                m_aLandVertices[iCounter].TexWeights.W = (eLT == LandType.Tundra) ? 1 : 0;

                cLocations[pLoc.m_iID] = iCounter;

                m_iLandTrianglesCount += pLoc.m_aBorderWith.Length;

                iCounter++;
            }

            // Create the indices used for each triangle
            m_aLandIndices = new int[m_iLandTrianglesCount * 3];

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

                    m_aLandIndices[iCounter++] = cLocations[pLoc.m_iID];
                    m_aLandIndices[iCounter++] = cVertexes[pLine.m_pPoint2.m_iID][(pLoc.Owner as LandX).Type];
                    m_aLandIndices[iCounter++] = cVertexes[pLine.m_pPoint1.m_iID][(pLoc.Owner as LandX).Type];

                    int index2 = m_aLandIndices[iCounter - 2];
                    int index3 = m_aLandIndices[iCounter - 1];

                    if ((m_aLandVertices[index2].Position.X == m_aLandVertices[index3].Position.X) && (m_aLandVertices[index2].Position.Z == m_aLandVertices[index3].Position.Z))
                        bError = true;

                    Vector3 pProjection2 = Vector3.Transform(m_aLandVertices[index2].Position, Matrix.CreateScale(1, 0, 1));
                    Vector3 pProjection3 = Vector3.Transform(m_aLandVertices[index3].Position, Matrix.CreateScale(1, 0, 1));

                    float fDistanceProjection = Vector3.Distance(pProjection2, pProjection3);
                    float fRealDistance = Vector3.Distance(m_aLandVertices[index2].Position, m_aLandVertices[index3].Position);
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

            BuildLand();
            BuildUnderWater();
            BuildWater();
            //FillPrimitivesFake();
            CalculateNormals(m_aLandVertices, m_aLandIndices, m_iLandTrianglesCount);
            CalculateNormals(m_aUnderwaterVertices, m_aUnderwaterIndices, m_iUnderwaterTrianglesCount);

            if (GraphicsDevice != null)
            {
                //pEffectFogNear.SetValue(m_pWorld.m_pGrid.RY / 1000);
                //pEffectFogFar.SetValue(2 * m_pWorld.m_pGrid.RY / 1000);

                if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                {
                    m_pCamera = new RingworldCamera(m_pWorld.m_pGrid.RX / ((float)Math.PI * 1000), GraphicsDevice);
                    effect2.CurrentTechnique = effect2.Techniques["LandRingworld"];
                    pEffectDirectionalLightDirection.SetValue(new Vector3(0, 0, -150));
                }
                else
                {
                    m_pCamera = new PlainCamera(GraphicsDevice);
                    effect2.CurrentTechnique = effect2.Techniques["Land"];
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
        Texture2D settlementsTexture;

        Model[] treeModel = new Model[13];
        Model[] palmModel = new Model[4];
        Model[] pineModel = new Model[4];

        Model cityModel;
        Model villageModel;
        Model fortModel;
        Model townModel;

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            LibContent = new ContentManager(Services);

            // Create our effect.
            effect2 = LibContent.Load<Effect>("content/Effect1");

            grassTexture = LibContent.Load<Texture2D>("content/1-plain");
            sandTexture = LibContent.Load<Texture2D>("content/1-desert");
            rockTexture = LibContent.Load<Texture2D>("content/rock");
            snowTexture = LibContent.Load<Texture2D>("content/snow");
            forestTexture = LibContent.Load<Texture2D>("content/grass");
            savannaTexture = LibContent.Load<Texture2D>("content/plain");
            swampTexture = LibContent.Load<Texture2D>("content/river");
            lavaTexture = LibContent.Load<Texture2D>("content/1-lava");

            treeTexture = LibContent.Load<Texture2D>("content/trees");
            settlementsTexture = LibContent.Load<Texture2D>("content/fake_houses");

            pEffectWorld = effect2.Parameters["World"];
            pEffectView = effect2.Parameters["View"];
            pEffectProjection = effect2.Parameters["Projection"];

            pEffectCameraPosition = effect2.Parameters["CameraPosition"];

            pEffectAmbientLightColor = effect2.Parameters["AmbientLightColor"];
            pEffectAmbientLightIntensity = effect2.Parameters["AmbientLightIntensity"];

            pEffectDirectionalLightDirection = effect2.Parameters["DirectionalLightDirection"];
            pEffectDirectionalLightColor = effect2.Parameters["DirectionalLightColor"];
            pEffectDirectionalLightIntensity = effect2.Parameters["DirectionalLightIntensity"];

            pEffectSpecularColor = effect2.Parameters["SpecularColor"];

            pEffectFogColor = effect2.Parameters["FogColor"];
            pEffectFogDensity = effect2.Parameters["FogDensity"];

            pEffectBlendDistance = effect2.Parameters["BlendDistance"];
            pEffectBlendWidth = effect2.Parameters["BlendWidth"];

            pEffectTexture0 = effect2.Parameters["xTexture0"];
            pEffectTexture1 = effect2.Parameters["xTexture1"];
            pEffectTexture2 = effect2.Parameters["xTexture2"];
            pEffectTexture3 = effect2.Parameters["xTexture3"];
            pEffectTexture4 = effect2.Parameters["xTexture4"];
            pEffectTexture5 = effect2.Parameters["xTexture5"];
            pEffectTexture6 = effect2.Parameters["xTexture6"];
            pEffectTexture7 = effect2.Parameters["xTexture7"];
            pEffectTextureModel = effect2.Parameters["xTextureModel"];

            effect2.CurrentTechnique = effect2.Techniques["LandRingworld"];
            pEffectWorld.SetValue(Matrix.Identity);

            pEffectAmbientLightColor.SetValue(eSkyColor.ToVector4());
            pEffectAmbientLightIntensity.SetValue(0.2f);

            pEffectDirectionalLightColor.SetValue(eSkyColor.ToVector4());
            pEffectDirectionalLightDirection.SetValue(new Vector3(0, 0, -150));
            pEffectDirectionalLightIntensity.SetValue(1);

            pEffectSpecularColor.SetValue(0);

            pEffectFogColor.SetValue(eSkyColor.ToVector4());
            pEffectFogDensity.SetValue(0.018f);

            pEffectBlendDistance.SetValue(20);
            pEffectBlendWidth.SetValue(18);

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

            LoadTree(0, "content/tree1");
            LoadTree(1, "content/tree2");
            LoadTree(2, "content/tree3");
            LoadTree(3, "content/tree4");
            LoadTree(4, "content/tree6");
            LoadTree(5, "content/tree7");
            LoadTree(6, "content/tree8");
            LoadTree(7, "content/tree9");
            LoadTree(8, "content/tree10");
            LoadTree(9, "content/tree11");
            LoadTree(10, "content/tree12");
            LoadTree(11, "content/tree15");
            LoadTree(12, "content/tree16");

            LoadPalm(0, "content/palm1");
            LoadPalm(1, "content/palm2");
            LoadPalm(2, "content/palm3");
            LoadPalm(3, "content/palm4");

            LoadPine(0, "content/tree5");
            LoadPine(1, "content/tree13");
            LoadPine(2, "content/tree14");
            LoadPine(3, "content/tree16");

            cityModel = LibContent.Load<Model>("content/city");
            foreach (ModelMesh mesh in cityModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect2.Clone();

            villageModel = LibContent.Load<Model>("content/village");
            foreach (ModelMesh mesh in villageModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect2.Clone();

            fortModel = LibContent.Load<Model>("content/fort");
            foreach (ModelMesh mesh in fortModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect2.Clone();

            townModel = LibContent.Load<Model>("content/town");
            foreach (ModelMesh mesh in townModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect2.Clone();

            // Start the animation timer.
            timer = Stopwatch.StartNew();
            lastTime = timer.Elapsed.TotalMilliseconds;

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        private void LoadTree(int index, string sPath)
        {
            treeModel[index] = LibContent.Load<Model>(sPath);
            foreach (ModelMesh mesh in treeModel[index].Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect2.Clone();
        }

        private void LoadPalm(int index, string sPath)
        {
            palmModel[index] = LibContent.Load<Model>(sPath);
            foreach (ModelMesh mesh in palmModel[index].Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect2.Clone();
        }

        private void LoadPine(int index, string sPath)
        {
            pineModel[index] = LibContent.Load<Model>(sPath);
            foreach (ModelMesh mesh in pineModel[index].Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect2.Clone();
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
        
        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
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

            // Set renderstates.
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullCounterClockwiseFace;
            //rs.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;

            if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                effect2.CurrentTechnique = effect2.Techniques["LandRingworld"]; 
            else
                effect2.CurrentTechnique = effect2.Techniques["Land"];
            effect2.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.SetRenderTarget(refractionRenderTarget);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Microsoft.Xna.Framework.Color.Black, 1.0f, 0);
            GraphicsDevice.DrawUserIndexedPrimitives<VertexMultitextured>(PrimitiveType.TriangleList,
                                              m_aUnderwaterVertices, 0, m_aUnderwaterVertices.Length - 1, m_aUnderwaterIndices, 0, m_iUnderwaterTrianglesCount);
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(eSkyColor);

            GraphicsDevice.DrawUserIndexedPrimitives<VertexMultitextured>(PrimitiveType.TriangleList,
                                              m_aLandVertices, 0, m_aLandVertices.Length - 1, m_aLandIndices, 0, m_iLandTrianglesCount);
            //GraphicsDevice.DrawUserIndexedPrimitives<VertexMultitextured>(PrimitiveType.TriangleList,
            //                                  m_aUnderwaterVertices, 0, m_aUnderwaterVertices.Length - 1, m_aUnderwaterIndices, 0, m_iUnderwaterTrianglesCount);
            DrawWater(time);

            for (int i = 0; i < m_aTrees.Length; i++)
                DrawTree(m_aTrees[i]);

            for (int i = 0; i < m_aSettlements.Length; i++)
                DrawSettlement(m_aSettlements[i]);

            // Draw the outline of the triangle under the cursor.
            DrawPickedTriangle();
        }

        private void DrawWater(float time)
        {
            effect2.CurrentTechnique = effect2.Techniques["Water"];
            //effect.Parameters["xReflectionView"].SetValue(reflectionViewMatrix);
            //effect.Parameters["xReflectionMap"].SetValue(reflectionMap);
            effect2.Parameters["xRefractionMap"].SetValue(refractionRenderTarget);

            effect2.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList,
                                                m_aWaterVertices, 0, m_aWaterVertices.Length - 1, m_aWaterIndices, 0, m_iWaterTrianglesCount);
        }

        private void DrawTree(TreeModel pTree)
        {
            Matrix worldMatrix = Matrix.CreateScale(0.4f, 0.4f, 0.4f) * Matrix.CreateRotationY(pTree.m_fAngle) * Matrix.CreateTranslation(pTree.m_pPosition);

            Matrix[] xwingTransforms = new Matrix[pTree.m_pModel.Bones.Count];
            pTree.m_pModel.CopyAbsoluteBoneTransformsTo(xwingTransforms);
            foreach (ModelMesh mesh in pTree.m_pModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                        currentEffect.CurrentTechnique = currentEffect.Techniques["ModelRingworld"];
                    else
                        currentEffect.CurrentTechnique = currentEffect.Techniques["Model"];
                    currentEffect.Parameters["xTextureModel"].SetValue(treeTexture);
                    currentEffect.Parameters["World"].SetValue(xwingTransforms[mesh.ParentBone.Index] * worldMatrix);
                    currentEffect.Parameters["View"].SetValue(m_pCamera.View);
                    currentEffect.Parameters["CameraPosition"].SetValue(m_pCamera.Position);
                    currentEffect.Parameters["Projection"].SetValue(m_pCamera.Projection);
                }
                mesh.Draw();
            }
        }

        private void DrawSettlement(SettlementModel pSettlement)
        {
            Matrix worldMatrix = Matrix.CreateScale(pSettlement.m_fScale, pSettlement.m_fScale, pSettlement.m_fScale) * Matrix.CreateRotationY(pSettlement.m_fAngle) * Matrix.CreateTranslation(pSettlement.m_pPosition);

            Matrix[] xwingTransforms = new Matrix[pSettlement.m_pModel.Bones.Count];
            pSettlement.m_pModel.CopyAbsoluteBoneTransformsTo(xwingTransforms);
            foreach (ModelMesh mesh in pSettlement.m_pModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    if (m_pWorld.m_pGrid.m_eShape == WorldShape.Ringworld)
                        currentEffect.CurrentTechnique = currentEffect.Techniques["ModelRingworld"];
                    else
                        currentEffect.CurrentTechnique = currentEffect.Techniques["Model"];

                    currentEffect.Parameters["xTextureModel"].SetValue(settlementsTexture);
                    currentEffect.Parameters["World"].SetValue(xwingTransforms[mesh.ParentBone.Index] * worldMatrix);
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
