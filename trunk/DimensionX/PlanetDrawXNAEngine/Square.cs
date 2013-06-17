using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UniLibXNA;
using Random;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualBasic.Devices;
using ContentLoader;
using Socium;
using LandscapeGeneration.PlanetBuilder;
using LandscapeGeneration;
using Socium.Settlements;
using LandscapeGeneration.PathFind;

namespace PlanetDrawXNAEngine
{
    public class Square
    {
        public struct Geometry
        {
            public Model[] m_aTreeModels;
            public Model[] m_aPalmModels;
            public Model[] m_aPineModels;

            public Texture2D m_pTreeTexture;
            
            public VertexMultitextured[] m_aLandPoints;
            public int[] m_aLandIndicesLR;
            public int[] m_aLandIndicesHR;
            public int[] m_aUnderwaterIndicesLR;
            public int[] m_aUnderwaterIndicesHR;
            public VertexPosition2[] m_aWaterPoints;
            public int[] m_aWaterIndices;

            public int[] m_aLocationReferences;
            public int[][] m_aLocations;

            public Dictionary<Model, TreeModel[]> m_aTrees;
            public SettlementModel[] m_aSettlements;

            public long Size()
            {
                int iTreeModels = 0;
                foreach (var pTree in m_aTrees)
                    iTreeModels += pTree.Value.Length;

                return VertexMultitextured.Size * m_aLandPoints.Length + sizeof(int) +
                    sizeof(int) * m_aLandIndicesLR.Length + sizeof(int) +
                    sizeof(int) * m_aLandIndicesHR.Length + sizeof(int) +
                    sizeof(int) * m_aUnderwaterIndicesLR.Length + sizeof(int) +
                    sizeof(int) * m_aUnderwaterIndicesHR.Length + sizeof(int) +
                    VertexPosition2.Size * m_aWaterPoints.Length + sizeof(int) +
                    sizeof(int) * m_aWaterIndices.Length + sizeof(int) + 
                    sizeof(int) * m_aLocationReferences.Length + sizeof(int) +
                    sizeof(int) * m_aLocations.Length + sizeof(int) +
                    sizeof(int) * m_aTrees.Count + TreeModel.Size * iTreeModels;
            }

            public void Save(string sFilename)
            {
                using (FileStream fs = new FileStream(sFilename, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(m_aLandPoints.Length);
                        for (int i = 0; i < m_aLandPoints.Length; i++)
                        {
                            bw.Write(m_aLandPoints[i].Position.X);
                            bw.Write(m_aLandPoints[i].Position.Y);
                            bw.Write(m_aLandPoints[i].Position.Z);
                            bw.Write(m_aLandPoints[i].Normal.X);
                            bw.Write(m_aLandPoints[i].Normal.Y);
                            bw.Write(m_aLandPoints[i].Normal.Z);
                            bw.Write(m_aLandPoints[i].Tangent.X);
                            bw.Write(m_aLandPoints[i].Tangent.Y);
                            bw.Write(m_aLandPoints[i].Tangent.Z);
                            bw.Write(m_aLandPoints[i].TexWeights.X);
                            bw.Write(m_aLandPoints[i].TexWeights.Y);
                            bw.Write(m_aLandPoints[i].TexWeights.Z);
                            bw.Write(m_aLandPoints[i].TexWeights.W);
                            bw.Write(m_aLandPoints[i].TexWeights2.X);
                            bw.Write(m_aLandPoints[i].TexWeights2.Y);
                            bw.Write(m_aLandPoints[i].TexWeights2.Z);
                            bw.Write(m_aLandPoints[i].TexWeights2.W);
                            bw.Write(m_aLandPoints[i].Color.R);
                            bw.Write(m_aLandPoints[i].Color.G);
                            bw.Write(m_aLandPoints[i].Color.B);
                        }

                        bw.Write(m_aLandIndicesLR.Length);
                        for (int i = 0; i < m_aLandIndicesLR.Length; i++)
                            bw.Write(m_aLandIndicesLR[i]);

                        bw.Write(m_aLandIndicesHR.Length);
                        for (int i = 0; i < m_aLandIndicesHR.Length; i++)
                            bw.Write(m_aLandIndicesHR[i]);

                        bw.Write(m_aUnderwaterIndicesLR.Length);
                        for (int i = 0; i < m_aUnderwaterIndicesLR.Length; i++)
                            bw.Write(m_aUnderwaterIndicesLR[i]);

                        bw.Write(m_aUnderwaterIndicesHR.Length);
                        for (int i = 0; i < m_aUnderwaterIndicesHR.Length; i++)
                            bw.Write(m_aUnderwaterIndicesHR[i]);

                        bw.Write(m_aWaterPoints.Length);
                        for (int i = 0; i < m_aWaterPoints.Length; i++)
                        {
                            bw.Write(m_aWaterPoints[i].Position.X);
                            bw.Write(m_aWaterPoints[i].Position.Y);
                            bw.Write(m_aWaterPoints[i].Position.Z);
                        }

                        bw.Write(m_aWaterIndices.Length);
                        for (int i = 0; i < m_aWaterIndices.Length; i++)
                            bw.Write(m_aWaterIndices[i]);

                        bw.Write(m_aLocationReferences.Length);
                        for (int i = 0; i < m_aLocationReferences.Length; i++)
                            bw.Write(m_aLocationReferences[i]);

                        bw.Write(m_aLocations.GetLength(0));
                        for (int i = 0; i < m_aLocations.GetLength(0); i++)
                        {
                            bw.Write(m_aLocations[i].Length);
                            for (int j = 0; j < m_aLocations[i].Length; j++)
                                bw.Write(m_aLocations[i][j]);
                        }

                        foreach (var pTree in m_aTrees)
                        {
                            bw.Write(pTree.Value.Length);
                            for (int i = 0; i < pTree.Value.Length; i++)
                                pTree.Value[i].Save(bw); ;
                        }
                    }
                }
            }

            public void Load(string sFilename)
            {
                using (FileStream fs = new FileStream(sFilename, FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        int iCount;

                        iCount = br.ReadInt32();
                        m_aLandPoints = new VertexMultitextured[iCount];
                        for (int i = 0; i < iCount; i++)
                        {
                            m_aLandPoints[i] = new VertexMultitextured();
                            m_aLandPoints[i].Position = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                            m_aLandPoints[i].Normal = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                            m_aLandPoints[i].Tangent = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                            m_aLandPoints[i].TextureCoordinate = Vector4.Zero;
                            m_aLandPoints[i].TexWeights = new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                            m_aLandPoints[i].TexWeights2 = new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                            m_aLandPoints[i].Color = new Color(br.ReadByte(), br.ReadByte(), br.ReadByte());
                        }

                        iCount = br.ReadInt32();
                        m_aLandIndicesLR = new int[iCount];
                        for (int i = 0; i < iCount; i++)
                            m_aLandIndicesLR[i] = br.ReadInt32();

                        iCount = br.ReadInt32();
                        m_aLandIndicesHR = new int[iCount];
                        for (int i = 0; i < iCount; i++)
                            m_aLandIndicesHR[i] = br.ReadInt32();

                        iCount = br.ReadInt32();
                        m_aUnderwaterIndicesLR = new int[iCount];
                        for (int i = 0; i < iCount; i++)
                            m_aUnderwaterIndicesLR[i] = br.ReadInt32();

                        iCount = br.ReadInt32();
                        m_aUnderwaterIndicesHR = new int[iCount];
                        for (int i = 0; i < iCount; i++)
                            m_aUnderwaterIndicesHR[i] = br.ReadInt32();

                        iCount = br.ReadInt32();
                        m_aWaterPoints = new VertexPosition2[iCount];
                        for (int i = 0; i < iCount; i++)
                        {
                            m_aWaterPoints[i] = new VertexPosition2();
                            m_aWaterPoints[i].Position = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                            m_aWaterPoints[i].Position2 = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                        }

                        iCount = br.ReadInt32();
                        m_aWaterIndices = new int[iCount];
                        for (int i = 0; i < iCount; i++)
                            m_aWaterIndices[i] = br.ReadInt32();

                        iCount = br.ReadInt32();
                        m_aLocationReferences = new int[iCount];
                        for (int i = 0; i < iCount; i++)
                            m_aLocationReferences[i] = br.ReadInt32();

                        iCount = br.ReadInt32();
                        m_aLocations = new int[iCount][];
                        for (int i = 0; i < iCount; i++)
                        {
                            int iCount2 = br.ReadInt32();
                            m_aLocations[i] = new int[iCount2];
                            for (int j = 0; j < iCount2; j++)
                                m_aLocations[i][j] = br.ReadInt32();
                        }

                        var aModels = m_aTrees.Keys.ToArray();
                        foreach (var pTree in aModels)
                        {
                            iCount = br.ReadInt32();
                            m_aTrees[pTree] = new TreeModel[iCount];
                            for (int i = 0; i < iCount; i++)
                                m_aTrees[pTree][i] = new TreeModel(br, pTree, m_pTreeTexture);
                        }
                    }
                }
            }
        }
        
        public Geometry g;

        public int m_iLandTrianglesCountLR = 0;
        public int m_iLandTrianglesCountHR = 0;
        public int m_iUnderwaterTrianglesCountLR = 0;
        public int m_iUnderwaterTrianglesCountHR = 0;
        public int m_iWaterTrianglesCount = 0;
        private int m_iLocationsIndicesCount = 0;

        public Region8 m_pBounds8;

        private void BuildBoundingBox(Chunk<LocationX> pChunk, float fMinHeight, float fMaxHeight)
        {
            Vector3 pBoundTopLeft = new Vector3(pChunk.m_pBoundTopLeft.m_fX, pChunk.m_pBoundTopLeft.m_fY, pChunk.m_pBoundTopLeft.m_fZ);
            Vector3 pBoundTopRight = new Vector3(pChunk.m_pBoundTopRight.m_fX, pChunk.m_pBoundTopRight.m_fY, pChunk.m_pBoundTopRight.m_fZ);
            Vector3 pBoundBottomLeft = new Vector3(pChunk.m_pBoundBottomLeft.m_fX, pChunk.m_pBoundBottomLeft.m_fY, pChunk.m_pBoundBottomLeft.m_fZ);
            Vector3 pBoundBottomRight = new Vector3(pChunk.m_pBoundBottomRight.m_fX, pChunk.m_pBoundBottomRight.m_fY, pChunk.m_pBoundBottomRight.m_fZ);

            Vector3 pCentral = (pBoundTopLeft + pBoundTopRight + pBoundBottomLeft + pBoundBottomRight) / 4;

            Plane pInnerPlane = new Plane(-Vector3.Normalize(pCentral), (float)pCentral.Length() + fMinHeight);
            Plane pOuterPlane = new Plane(-Vector3.Normalize(pCentral), (float)pBoundTopLeft.Length() + fMaxHeight);

            Ray pBoundTopLeftRay = new Ray(Vector3.Zero, Vector3.Normalize(pBoundTopLeft));
            Ray pBoundTopRightRay = new Ray(Vector3.Zero, Vector3.Normalize(pBoundTopRight));
            Ray pBoundBottomLeftRay = new Ray(Vector3.Zero, Vector3.Normalize(pBoundBottomLeft));
            Ray pBoundBottomRightRay = new Ray(Vector3.Zero, Vector3.Normalize(pBoundBottomRight));

            float? fDist = pBoundTopLeftRay.Intersects(pInnerPlane);
            Vector3 pBoundTopLeft1 = Vector3.Normalize(pBoundTopLeft) * (float)fDist;
            fDist = pBoundTopRightRay.Intersects(pInnerPlane);
            Vector3 pBoundTopRight1 = Vector3.Normalize(pBoundTopRight) * (float)fDist;
            fDist = pBoundBottomLeftRay.Intersects(pInnerPlane);
            Vector3 pBoundBottomLeft1 = Vector3.Normalize(pBoundBottomLeft) * (float)fDist;
            fDist = pBoundBottomRightRay.Intersects(pInnerPlane);
            Vector3 pBoundBottomRight1 = Vector3.Normalize(pBoundBottomRight) * (float)fDist;

            fDist = pBoundTopLeftRay.Intersects(pOuterPlane);
            Vector3 pBoundTopLeft2 = Vector3.Normalize(pBoundTopLeft) * (float)fDist;
            fDist = pBoundTopRightRay.Intersects(pOuterPlane);
            Vector3 pBoundTopRight2 = Vector3.Normalize(pBoundTopRight) * (float)fDist;
            fDist = pBoundBottomLeftRay.Intersects(pOuterPlane);
            Vector3 pBoundBottomLeft2 = Vector3.Normalize(pBoundBottomLeft) * (float)fDist;
            fDist = pBoundBottomRightRay.Intersects(pOuterPlane);
            Vector3 pBoundBottomRight2 = Vector3.Normalize(pBoundBottomRight) * (float)fDist;

            //m_pBounds8 = new Region8(pBoundTopLeft1, pBoundTopRight1, pBoundTopLeft2, pBoundTopRight2,
            //    pBoundBottomLeft1, pBoundBottomRight1, pBoundBottomLeft2, pBoundBottomRight2);
            m_pBounds8 = new Region8(pBoundBottomLeft2, pBoundBottomRight2, pBoundTopLeft2, pBoundTopRight2,
                pBoundBottomLeft1, pBoundBottomRight1, pBoundTopLeft1, pBoundTopRight1);
        }

        public VertexBuffer m_pVertexBuffer;
        public IndexBuffer m_pLandIndexBufferLR;
        public IndexBuffer m_pLandIndexBufferHR;
        public IndexBuffer m_pUnderwaterIndexBufferLR;
        public IndexBuffer m_pUnderwaterIndexBufferHR;
        public VertexBuffer m_pWaterVertexBuffer;
        public IndexBuffer m_pWaterIndexBuffer;

        private void CopyToBuffers(GraphicsDevice pDevice)
        {
            m_pVertexBuffer = new VertexBuffer(pDevice, VertexMultitextured.VertexDeclaration, g.m_aLandPoints.Length, BufferUsage.WriteOnly);
            m_pVertexBuffer.SetData(g.m_aLandPoints);

            m_pLandIndexBufferLR = new IndexBuffer(pDevice, typeof(int), g.m_aLandIndicesLR.Length, BufferUsage.WriteOnly);
            m_pLandIndexBufferLR.SetData(g.m_aLandIndicesLR);

            m_pLandIndexBufferHR = new IndexBuffer(pDevice, typeof(int), g.m_aLandIndicesHR.Length, BufferUsage.WriteOnly);
            m_pLandIndexBufferHR.SetData(g.m_aLandIndicesHR);

            if (g.m_aUnderwaterIndicesLR.Length > 0)
            {
                m_pUnderwaterIndexBufferLR = new IndexBuffer(pDevice, typeof(int), g.m_aUnderwaterIndicesLR.Length, BufferUsage.WriteOnly);
                m_pUnderwaterIndexBufferLR.SetData(g.m_aUnderwaterIndicesLR);

                m_pUnderwaterIndexBufferHR = new IndexBuffer(pDevice, typeof(int), g.m_aUnderwaterIndicesHR.Length, BufferUsage.WriteOnly);
                m_pUnderwaterIndexBufferHR.SetData(g.m_aUnderwaterIndicesHR);

                m_pWaterVertexBuffer = new VertexBuffer(pDevice, VertexPosition2.VertexDeclaration, g.m_aWaterPoints.Length, BufferUsage.WriteOnly);
                m_pWaterVertexBuffer.SetData(g.m_aWaterPoints);

                m_pWaterIndexBuffer = new IndexBuffer(pDevice, typeof(int), g.m_aWaterIndices.Length, BufferUsage.WriteOnly);
                m_pWaterIndexBuffer.SetData(g.m_aWaterIndices);
            }
        }

        public Chunk<LocationX> m_pChunk;
        private float m_fR = 150;

        /// <summary>
        /// Обновляет текстурные веса.
        /// Для каждого веса выбирается максимальное значение между уже вычисленным и новым значением.
        /// </summary>
        /// <param name="pOriginal">старое значение</param>
        /// <param name="pAddon">новое значение</param>
        private void UpdateTexWeight(ref Vector4 pOriginal, Vector4 pAddon)
        {
            pOriginal.X = Math.Max(pOriginal.X, pAddon.X);
            pOriginal.Y = Math.Max(pOriginal.Y, pAddon.Y);
            pOriginal.Z = Math.Max(pOriginal.Z, pAddon.Z);
            pOriginal.W = Math.Max(pOriginal.W, pAddon.W);
        }

        /// <summary>
        /// Вычисяет текстурные веса первой четвёрки (песок, трава, скалы, снег).
        /// </summary>
        /// <param name="eLT">тип территории</param>
        /// <returns></returns>
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

        /// <summary>
        /// Вычисяет текстурные веса второй четвёрки (лес, дорога, болото, лава).
        /// </summary>
        /// <param name="eLT">тип территории</param>
        /// <returns></returns>
        private Vector4 GetTexWeights2(LandType eLT)
        {
            Vector4 texWeights = new Vector4();
            texWeights.X = (eLT == LandType.Swamp || eLT == LandType.Forest || eLT == LandType.Jungle || eLT == LandType.Taiga) ? 0.7f + Rnd.Get(0.6f) : Rnd.Get(0.2f);
            texWeights.Y = 0;// (eLT == LandType.Savanna) ? 1 : 0;
            texWeights.Z = (eLT == LandType.Swamp) ? 0.7f + Rnd.Get(0.6f) : Rnd.Get(0.1f);
            texWeights.W = 0;

            return texWeights;
        }

        private string m_sCacheFileName = string.Empty;

        public void Clear()
        {
            if (m_bCleared)
                return;

            if (string.IsNullOrEmpty(m_sCacheFileName))
            {
                m_sCacheFileName = Path.GetTempFileName();
                DriveInfo pDrive = new DriveInfo(m_sCacheFileName);

                if (pDrive.AvailableFreeSpace < g.Size())
                    m_sCacheFileName = string.Empty;
                else
                    g.Save(m_sCacheFileName);
            }

            g.m_aLandPoints = null;
            g.m_aLandIndicesLR = null;
            g.m_aLandIndicesHR = null;
            g.m_aUnderwaterIndicesLR = null;
            g.m_aUnderwaterIndicesHR = null;
            g.m_aWaterPoints = null;
            g.m_aWaterIndices = null;
            g.m_aLocationReferences = null;
            g.m_aLocations = null;

            var aTrees = g.m_aTrees.Keys.ToArray();
            foreach (var pTree in aTrees)
                g.m_aTrees[pTree] = new TreeModel[0];

            if (m_pVertexBuffer != null)
                m_pVertexBuffer.Dispose();

            if (m_pLandIndexBufferLR != null)
                m_pLandIndexBufferLR.Dispose();

            if (m_pLandIndexBufferHR != null)
                m_pLandIndexBufferHR.Dispose();

            if (m_pUnderwaterIndexBufferLR != null)
                m_pUnderwaterIndexBufferLR.Dispose();

            if (m_pUnderwaterIndexBufferHR != null)
                m_pUnderwaterIndexBufferHR.Dispose();

            if (m_pWaterVertexBuffer != null)
                m_pWaterVertexBuffer.Dispose();

            if (m_pWaterIndexBuffer != null)
                m_pWaterIndexBuffer.Dispose();

            m_bCleared = true;
        }

        public float m_fVisibleDistance = -1;

        public void UpdateVisible(GraphicsDevice pDevice, BoundingFrustum pFrustum, Vector3 pCameraPos, Vector3 pCameraDir, Vector3 pCameraTarget, Matrix pWorld, Matrix pInvert)
        {
            m_fVisibleDistance = -1;

            bool bVisible = true;

            Vector3 pRealCenter = Vector3.Transform(m_pBounds8.Center, pWorld);
            Vector3 pRealNormal = Vector3.Normalize(Vector3.Transform(m_pBounds8.Center + m_pBounds8.Normal, pInvert) - Vector3.Transform(m_pBounds8.Center, pInvert));

            Vector3 pViewVector = pRealCenter - pCameraPos;
            float fCos2 = Vector3.Dot(Vector3.Normalize(pViewVector), pRealNormal);//m_pBounds8.Normal);
            if (fCos2 > 0.1)
                bVisible = false;

            if(bVisible)
            {
                float fCos = Vector3.Dot(Vector3.Normalize(pViewVector), pCameraDir);
                if (fCos < 0.8 && !m_pBounds8.Intersects(pFrustum, pWorld) &&
                    m_pBounds8.m_pSphere.Contains(pCameraPos) == ContainmentType.Disjoint) //cos(45) = 0,70710678118654752440084436210485...
                    bVisible = false;
            }

            if(bVisible)
            {
                Vector3 pTargetVector = pRealCenter - pCameraTarget;
                m_fVisibleDistance = Math.Min(pViewVector.Length(), pTargetVector.Length());
                Rebuild(pDevice);
                return;
            }

            if (!m_bCleared && !s_pInvisibleQueue.Contains(this))
                s_pInvisibleQueue.Add(this);

            if (s_pVisibleQueue.Contains(this))
                s_pVisibleQueue.Remove(this); 
        }

        private TimeSpan m_fRebuild = TimeSpan.Zero;
        private TimeSpan m_fReload = TimeSpan.Zero;
        private TimeSpan m_fReload2 = TimeSpan.Zero;

        public bool m_bCleared = true;

        public static List<Square> s_pInvisibleQueue = new List<Square>();
        public static List<Square> s_pVisibleQueue = new List<Square>();

        public static void ClearQueues()
        {
            s_pInvisibleQueue.Clear();
            s_pVisibleQueue.Clear();
        }

        public static float m_fLandHeightMultiplier = 3.5f / 5;

        /// <summary>
        /// Вычисляет реальные 3d координаты центра локации с учётом её собственной высоты над сеткой и формы мира
        /// </summary>
        /// <param name="pLoc">локация</param>
        /// <param name="eShape">форма мира</param>
        /// <param name="fMultiplier">множитель высоты</param>
        /// <returns></returns>
        public static Vector3 GetPosition(IPointF pLoc)
        {
            return GetPosition(pLoc, pLoc.H);
        }

        /// <summary>
        /// Вычисляет реальные 3d координаты центра локации с учётом указанной высоты над сеткой и формы мира
        /// </summary>
        /// <param name="pLoc">локация</param>
        /// <param name="eShape">форма мира</param>
        /// <param name="fHeight">высота</param>
        /// <param name="fMultiplier">множитель высоты</param>
        /// <returns></returns>
        public static Vector3 GetPosition(IPointF pLoc, float fHeight)
        {
            Vector3 pPosition = new Vector3(pLoc.X, pLoc.Y, pLoc.Z);
            Vector3 pUp = Vector3.Normalize(pPosition);

            pPosition += pUp * fHeight * m_fLandHeightMultiplier;

            return pPosition;
        }
        
        public void Rebuild(GraphicsDevice pDevice)
        {
            if (m_bCleared)
            {
                bool bSuccess = false;
                //Будем пытаться построить, пока не получится
                do
                {
                    try
                    {
                        DateTime now;
                        //Если у нас есть уже построенные сохранённые на диске данные - просто подгружаем их
                        if (!string.IsNullOrEmpty(m_sCacheFileName) && File.Exists(m_sCacheFileName))
                        {
                            now = DateTime.Now;
                            g.Load(m_sCacheFileName);
                            m_fReload = DateTime.Now - now;
                        }
                        else //иначе - строим
                        {
                            now = DateTime.Now;
                            g.m_aLandPoints = new VertexMultitextured[m_pChunk.m_aLocations.Length +
                                m_pChunk.m_aVertexes.Length];

                            g.m_aWaterPoints = new VertexPosition2[m_pChunk.m_aLocations.Length +
                                m_pChunk.m_aVertexes.Length];

                            Dictionary<Vertex, int> vertexIndex = new Dictionary<Vertex, int>();
                            Dictionary<Location, int> locationIndex = new Dictionary<Location, int>();

                            Dictionary<Vertex, int> vertexWaterIndex = new Dictionary<Vertex, int>();
                            Dictionary<Location, int> locationWaterIndex = new Dictionary<Location, int>();

                            int index = 0;

                            int indexWater = 0;

                            m_iLandTrianglesCountLR = 0;
                            m_iLandTrianglesCountHR = 0;
                            m_iWaterTrianglesCount = 0;
                            m_iUnderwaterTrianglesCountLR = 0;
                            m_iUnderwaterTrianglesCountHR = 0;

                            //добавляем в вершинный буффер узлы диаграммы Вороного
                            for (int i = 0; i < m_pChunk.m_aVertexes.Length; i++)
                            {
                                var vertex = m_pChunk.m_aVertexes[i];

                                g.m_aLandPoints[index] = new VertexMultitextured();
                                g.m_aLandPoints[index].Position = GetPosition(vertex);
                                g.m_aLandPoints[index].Normal = new Vector3(vertex.m_fXN, vertex.m_fYN, vertex.m_fZN);
                                g.m_aLandPoints[index].Tangent = Vector3.Zero;
                                g.m_aLandPoints[index].Color = Color.Red;
                                g.m_aLandPoints[index].TextureCoordinate = new Vector4(0, 0, 0, 0); // new Vector4(GetTexture(vertex), 0, 0); 

                                for (int j = 0; j < vertex.m_cLocations.Count; j++)
                                {
                                    LocationX pLoc = (LocationX)vertex.m_cLocations[j];

                                    LandType eLT = LandType.Ocean;
                                    if (pLoc.Owner != null)
                                        eLT = (pLoc.Owner as LandX).Type.m_eType;
                                    UpdateTexWeight(ref g.m_aLandPoints[index].TexWeights, GetTexWeights(eLT));
                                    UpdateTexWeight(ref g.m_aLandPoints[index].TexWeights2, GetTexWeights2(eLT));
                                }

                                vertexIndex[vertex] = index;

                                index++;

                                g.m_aWaterPoints[indexWater] = new VertexPosition2();
                                g.m_aWaterPoints[indexWater].Position = GetPosition(vertex, 0);
                                g.m_aWaterPoints[indexWater].Position = Vector3.Normalize(g.m_aWaterPoints[indexWater].Position) * m_fR;
                                g.m_aWaterPoints[indexWater].Position2 = GetPosition(vertex);

                                vertexWaterIndex[vertex] = indexWater;

                                indexWater++;
                            }

                            //добавляем в вершинный буффер центры локаций
                            for (int i = 0; i < m_pChunk.m_aLocations.Length; i++)
                            {
                                var pLoc = m_pChunk.m_aLocations[i];
                                
                                float fHeight = pLoc.H;
                                if (pLoc.m_eType == RegionType.Peak)
                                    fHeight += 2 + Rnd.Get(1f);
                                if (pLoc.m_eType == RegionType.Volcano)
                                    fHeight -= 4;

                                g.m_aLandPoints[index] = new VertexMultitextured();
                                g.m_aLandPoints[index].Position = GetPosition(pLoc, fHeight);
                                g.m_aLandPoints[index].Normal = new Vector3(pLoc.m_fXN, pLoc.m_fYN, pLoc.m_fZN);
                                g.m_aLandPoints[index].Tangent = Vector3.Zero;
                                g.m_aLandPoints[index].Color = Color.Red;
                                g.m_aLandPoints[index].TextureCoordinate = new Vector4(0, 0, 0, 0); //new Vector4(GetTexture(loc), 0, 0); 
                                LandType eLT = LandType.Ocean;
                                if (pLoc.Owner != null)
                                    eLT = (pLoc.Owner as LandX).Type.m_eType;
                                g.m_aLandPoints[index].TexWeights = GetTexWeights(eLT);
                                g.m_aLandPoints[index].TexWeights2 = GetTexWeights2(eLT);

                                if (pLoc.m_eType == RegionType.Volcano || pLoc.m_eType == RegionType.Peak)
                                {
                                    Location.Edge pLine = pLoc.m_pFirstLine;
                                    do
                                    {
                                        if (pLoc.m_eType == RegionType.Volcano)
                                        {
                                            g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].Position = GetPosition(pLoc, pLine.m_pInnerPoint.m_fH + 6 + Rnd.Get(3f));
                                            g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].Position = (g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].Position + g.m_aLandPoints[index].Position) / 2;
                                        }
                                        else
                                        {
                                            g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].Position = GetPosition(pLoc, pLine.m_pInnerPoint.m_fH + 1.5f + Rnd.Get(1f));
                                        }

                                        pLine = pLine.m_pNext;
                                    }
                                    while (pLine != pLoc.m_pFirstLine);
                                }

                                m_iLandTrianglesCountLR += pLoc.m_cBorderWith.Count;
                                m_iLandTrianglesCountHR += pLoc.m_cBorderWith.Count * 4;

                                locationIndex[pLoc] = index;

                                index++;

                                if (pLoc.m_fH <= 0)
                                {
                                    m_iUnderwaterTrianglesCountLR += pLoc.m_cBorderWith.Count;
                                    m_iUnderwaterTrianglesCountHR += pLoc.m_cBorderWith.Count * 4;

                                    g.m_aWaterPoints[indexWater] = new VertexPosition2();
                                    g.m_aWaterPoints[indexWater].Position = GetPosition(pLoc, 0);
                                    g.m_aWaterPoints[indexWater].Position = Vector3.Normalize(g.m_aWaterPoints[indexWater].Position) * m_fR;
                                    g.m_aWaterPoints[indexWater].Position2 = GetPosition(pLoc);
                                    m_iWaterTrianglesCount += pLoc.m_cBorderWith.Count;

                                    locationWaterIndex[pLoc] = indexWater;

                                    indexWater++;
                                }
                            }

                            g.m_aLandIndicesLR = new int[m_iLandTrianglesCountLR * 3];
                            g.m_aLandIndicesHR = new int[m_iLandTrianglesCountHR * 3];
                            g.m_aUnderwaterIndicesLR = new int[m_iUnderwaterTrianglesCountLR * 3];
                            g.m_aUnderwaterIndicesHR = new int[m_iUnderwaterTrianglesCountHR * 3];
                            g.m_aWaterIndices = new int[m_iWaterTrianglesCount * 3];

                            g.m_aLocationReferences = new int[m_iLandTrianglesCountLR];
                            g.m_aLocations = new int[m_pChunk.m_aLocations.Length][];

                            Dictionary<Model, List<TreeModel>> cTrees = new Dictionary<Model, List<TreeModel>>();
                            List<SettlementModel> cSettlements = new List<SettlementModel>();

                            index = 0;
                            indexWater = 0;
                            int indexHR = 0;
                            int indexUnderwater = 0;
                            int indexUnderwaterHR = 0;
                            int iReferenceCounter = 0;

                            m_iLocationsIndicesCount = 0;

                            //заполняем индексный буффер
                            for (int i = 0; i < m_pChunk.m_aLocations.Length; i++)
                            {
                                var pLoc = m_pChunk.m_aLocations[i];

                                LandType eLT = (pLoc.Owner as LandX).Type.m_eType;

                                //добавляем на горы снежные шапки в зависимости от высоты
                                float fHeight = pLoc.H;
                                if (pLoc.m_eType == RegionType.Peak)
                                    fHeight += 2.5f;

                                if (fHeight > m_fWorldMaxHeight / 3)
                                    g.m_aLandPoints[locationIndex[pLoc]].TexWeights.W = 2 * (float)Math.Pow((fHeight * 3 - m_fWorldMaxHeight) / m_fWorldMaxHeight, 3);

                                foreach (var edge in pLoc.m_cBorderWith)
                                {
                                    Location.Edge pLine = edge.Value[0];

                                    if (eLT == LandType.Mountains)
                                    {
                                        g.m_aLandPoints[vertexIndex[pLine.m_pMidPoint]].TexWeights = g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].TexWeights;
                                        g.m_aLandPoints[vertexIndex[pLine.m_pMidPoint]].TexWeights2 = g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].TexWeights2;

                                        g.m_aLandPoints[vertexIndex[pLine.m_pPoint1]].TexWeights = g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].TexWeights;
                                        g.m_aLandPoints[vertexIndex[pLine.m_pPoint1]].TexWeights2 = g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].TexWeights2;

                                        g.m_aLandPoints[vertexIndex[pLine.m_pPoint2]].TexWeights = g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].TexWeights;
                                        g.m_aLandPoints[vertexIndex[pLine.m_pPoint2]].TexWeights2 = g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].TexWeights2;

                                    }
                                    else
                                    {
                                        g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].TexWeights += g.m_aLandPoints[vertexIndex[pLine.m_pMidPoint]].TexWeights;///2;
                                        g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].TexWeights2 += g.m_aLandPoints[vertexIndex[pLine.m_pMidPoint]].TexWeights2;///2;

                                        //aVertices[cVertexes[pLine.m_pInnerPoint.m_iID]].TexWeights += aVertices[cVertexes[pLine.m_pPoint1.m_iID]].TexWeights / 2;
                                        //aVertices[cVertexes[pLine.m_pInnerPoint.m_iID]].TexWeights2 += aVertices[cVertexes[pLine.m_pPoint1.m_iID]].TexWeights2 / 2;

                                        //aVertices[cVertexes[pLine.m_pInnerPoint.m_iID]].TexWeights += aVertices[cVertexes[pLine.m_pPoint2.m_iID]].TexWeights / 2;
                                        //aVertices[cVertexes[pLine.m_pInnerPoint.m_iID]].TexWeights2 += aVertices[cVertexes[pLine.m_pPoint2.m_iID]].TexWeights2 / 2;
                                    }

                                    //добавляем на горы снежные шапки в зависимости от высоты
                                    if (pLine.m_pPoint1.m_fH > m_fWorldMaxHeight / 3)
                                        g.m_aLandPoints[vertexIndex[pLine.m_pPoint1]].TexWeights.W = 2 * (float)Math.Pow((pLine.m_pPoint1.m_fH * 3 - m_fWorldMaxHeight) / m_fWorldMaxHeight, 3);
                                    if (pLine.m_pPoint2.m_fH > m_fWorldMaxHeight / 3)
                                        g.m_aLandPoints[vertexIndex[pLine.m_pPoint2]].TexWeights.W = 2 * (float)Math.Pow((pLine.m_pPoint2.m_fH * 3 - m_fWorldMaxHeight) / m_fWorldMaxHeight, 3);
                                    if (pLine.m_pMidPoint.m_fH > m_fWorldMaxHeight / 3)
                                        g.m_aLandPoints[vertexIndex[pLine.m_pMidPoint]].TexWeights.W = 2 * (float)Math.Pow((pLine.m_pMidPoint.m_fH * 3 - m_fWorldMaxHeight) / m_fWorldMaxHeight, 3);
                                    if (pLine.m_pInnerPoint.m_fH > m_fWorldMaxHeight / 3)
                                        g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].TexWeights.W = 2 * (float)Math.Pow((pLine.m_pInnerPoint.m_fH * 3 - m_fWorldMaxHeight) / m_fWorldMaxHeight, 3);

                                    g.m_aLocationReferences[iReferenceCounter++] = i;
                                    g.m_aLandIndicesLR[index++] = locationIndex[pLoc];
                                    g.m_aLandIndicesLR[index++] = vertexIndex[edge.Value[0].m_pPoint1];
                                    g.m_aLandIndicesLR[index++] = vertexIndex[edge.Value[0].m_pPoint2];

                                    g.m_aLandIndicesHR[indexHR++] = vertexIndex[edge.Value[0].m_pInnerPoint];
                                    g.m_aLandIndicesHR[indexHR++] = vertexIndex[edge.Value[0].m_pPoint1];
                                    g.m_aLandIndicesHR[indexHR++] = vertexIndex[edge.Value[0].m_pMidPoint];

                                    g.m_aLandIndicesHR[indexHR++] = vertexIndex[edge.Value[0].m_pInnerPoint];
                                    g.m_aLandIndicesHR[indexHR++] = vertexIndex[edge.Value[0].m_pMidPoint];
                                    g.m_aLandIndicesHR[indexHR++] = vertexIndex[edge.Value[0].m_pPoint2];

                                    g.m_aLandIndicesHR[indexHR++] = vertexIndex[edge.Value[0].m_pInnerPoint];
                                    g.m_aLandIndicesHR[indexHR++] = vertexIndex[edge.Value[0].m_pPoint2];
                                    g.m_aLandIndicesHR[indexHR++] = vertexIndex[edge.Value[0].m_pNext.m_pInnerPoint];

                                    g.m_aLandIndicesHR[indexHR++] = vertexIndex[edge.Value[0].m_pInnerPoint];
                                    g.m_aLandIndicesHR[indexHR++] = vertexIndex[edge.Value[0].m_pNext.m_pInnerPoint];
                                    g.m_aLandIndicesHR[indexHR++] = locationIndex[pLoc];

                                    if (pLoc.m_fH <= 0)
                                    {
                                        g.m_aUnderwaterIndicesLR[indexUnderwater++] = locationIndex[pLoc];
                                        g.m_aUnderwaterIndicesLR[indexUnderwater++] = vertexIndex[edge.Value[0].m_pPoint1];
                                        g.m_aUnderwaterIndicesLR[indexUnderwater++] = vertexIndex[edge.Value[0].m_pPoint2];

                                        g.m_aUnderwaterIndicesHR[indexUnderwaterHR++] = vertexIndex[edge.Value[0].m_pInnerPoint];
                                        g.m_aUnderwaterIndicesHR[indexUnderwaterHR++] = vertexIndex[edge.Value[0].m_pPoint1];
                                        g.m_aUnderwaterIndicesHR[indexUnderwaterHR++] = vertexIndex[edge.Value[0].m_pMidPoint];

                                        g.m_aUnderwaterIndicesHR[indexUnderwaterHR++] = vertexIndex[edge.Value[0].m_pInnerPoint];
                                        g.m_aUnderwaterIndicesHR[indexUnderwaterHR++] = vertexIndex[edge.Value[0].m_pMidPoint];
                                        g.m_aUnderwaterIndicesHR[indexUnderwaterHR++] = vertexIndex[edge.Value[0].m_pPoint2];

                                        g.m_aUnderwaterIndicesHR[indexUnderwaterHR++] = vertexIndex[edge.Value[0].m_pInnerPoint];
                                        g.m_aUnderwaterIndicesHR[indexUnderwaterHR++] = vertexIndex[edge.Value[0].m_pPoint2];
                                        g.m_aUnderwaterIndicesHR[indexUnderwaterHR++] = vertexIndex[edge.Value[0].m_pNext.m_pInnerPoint];

                                        g.m_aUnderwaterIndicesHR[indexUnderwaterHR++] = vertexIndex[edge.Value[0].m_pInnerPoint];
                                        g.m_aUnderwaterIndicesHR[indexUnderwaterHR++] = vertexIndex[edge.Value[0].m_pNext.m_pInnerPoint];
                                        g.m_aUnderwaterIndicesHR[indexUnderwaterHR++] = locationIndex[pLoc]; 
                                        
                                        g.m_aWaterIndices[indexWater++] = locationWaterIndex[pLoc];
                                        g.m_aWaterIndices[indexWater++] = vertexWaterIndex[edge.Value[0].m_pPoint1];
                                        g.m_aWaterIndices[indexWater++] = vertexWaterIndex[edge.Value[0].m_pPoint2];
                                    }

                                    g.m_aLandPoints[vertexIndex[edge.Value[0].m_pInnerPoint]].TexWeights = g.m_aLandPoints[locationIndex[pLoc]].TexWeights;
                                }

                                g.m_aLocations[i] = BuildLocationReferencesIndices(pLoc, ref vertexIndex);

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
                                    g.m_aLandPoints[locationIndex[pLoc]].TexWeights2.Y = fScale * 2;
                                }

                                if (pLoc.m_eType == RegionType.Volcano)
                                {
                                    g.m_aLandPoints[locationIndex[pLoc]].TexWeights = new Vector4(0, 0, 0, 0);
                                    g.m_aLandPoints[locationIndex[pLoc]].TexWeights2 = new Vector4(0, 0, 0, 1);

                                    Location.Edge pLine = pLoc.m_pFirstLine;
                                    do
                                    {
                                        g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].TexWeights += new Vector4(0, 0, 0, 0);
                                        g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].TexWeights2 += new Vector4(0, 0, 0, 0.5f);

                                        g.m_aLandPoints[vertexIndex[pLine.m_pInnerPoint]].TexWeights.W = 0;

                                        pLine = pLine.m_pNext;
                                    }
                                    while (pLine != pLoc.m_pFirstLine);
                                }
                                if (eLT != LandType.Ocean && eLT != LandType.Coastral)
                                {
                                    float fScale = 0.07f; //0.015f;
                                    fScale *= (float)(70 / Math.Sqrt(120000));

                                    if (eLT == LandType.Forest || eLT == LandType.Taiga || eLT == LandType.Jungle)
                                    {
                                        bool bEdge = false;
                                        foreach (LocationX pEdge in pLoc.m_aBorderWith)
                                        {
                                            LandType eLinkLT = (pEdge.Owner as LandX).Type.m_eType;

                                            if (eLinkLT != LandType.Forest && eLinkLT != LandType.Taiga && eLinkLT != LandType.Jungle)
                                                bEdge = true;
                                        }

                                        AddTreeModels(pLoc, ref eLT, ref g.m_aLandPoints, ref locationIndex, ref vertexIndex, ref cTrees, fScale, bEdge ? 0.75f : 1f);
                                    }
                                    else
                                    {
                                        LandType eForestLT = LandType.Ocean;
                                        foreach (LocationX pEdge in pLoc.m_aBorderWith)
                                        {
                                            LandType eLinkLT = (pEdge.Owner as LandX).Type.m_eType;

                                            if (eLinkLT == LandType.Forest || eLinkLT == LandType.Taiga || eLinkLT == LandType.Jungle)
                                                eForestLT = eLinkLT;
                                        }

                                        if (eForestLT != LandType.Ocean)
                                            AddTreeModels(pLoc, ref eForestLT, ref g.m_aLandPoints, ref locationIndex, ref vertexIndex, ref cTrees, fScale, 0.1f);
                                    }
                                }
                            }

                            for (int i = 0; i < g.m_aLandPoints.Length; i++)
                            {
                                NormalizeTextureWeights(ref g.m_aLandPoints[i]);
                            }

                            g.m_aTrees = new Dictionary<Model, TreeModel[]>();
                            foreach (var vTree in cTrees)
                                g.m_aTrees[vTree.Key] = vTree.Value.ToArray();

                            m_fRebuild = DateTime.Now - now;
                        }
                        now = DateTime.Now;
                        CopyToBuffers(pDevice);
                        m_fReload2 = DateTime.Now - now;

                        m_bCleared = false;

                        bSuccess = true;
                    }
                    catch (Exception ex)
                    //catch (OutOfMemoryException ex)
                    {
                        if (s_pInvisibleQueue.Count > 0)
                        {
                            int iCount = s_pInvisibleQueue.Count;///2 + 1;
                            for (int i = 0; i < iCount; i++)
                            {
                                var pDead = s_pInvisibleQueue[0];
                                pDead.Clear();

                                s_pInvisibleQueue.Remove(pDead);
                            }
                        }
                        //else if (s_pVisibleQueue.Count > 0)
                        //{
                        //    int iCount = s_pVisibleQueue.Count;
                        //    for (int i = 0; i < iCount; i++)
                        //    {
                        //        var pDead = s_pVisibleQueue[0];
                        //        pDead.Clear();

                        //        s_pVisibleQueue.Remove(pDead);
                        //    }
                        //}
                        else
                        {
                            if (ex is OutOfMemoryException)
                                throw new OutOfMemoryException("Really out of memory!", ex);
                            //else
                            //    throw new Exception("Something really wrong!", ex);
                        }
                    }
                }
                while (!bSuccess);
            }

            if (s_pInvisibleQueue.Contains(this))
                s_pInvisibleQueue.Remove(this);

            if (!s_pVisibleQueue.Contains(this))
                s_pVisibleQueue.Add(this);
        }

        /// <summary>
        /// наполняет локацию деревьями
        /// </summary>
        /// <param name="pLoc">локация</param>
        /// <param name="eLT">тип территории, согласно которому нужно выбрать модели деревьев. может не совпадать с типом территории локации - например, на равнине рядом с лесом могут расти специфичные для этого леса деревья</param>
        /// <param name="aVertices">заполненый вершинный буфер - отсюда мы будем брать уже вычисленные координаты точек, куда "сажать" деревья</param>
        /// <param name="cLocations">словарь индексов центров локаций в вершинном буфере</param>
        /// <param name="cVertexes">словарь индексов вертексов в вершинном буфере</param>
        /// <param name="cTrees">пополняемый список деревьев</param>
        /// <param name="fScale">масштабный коэффициент</param>
        /// <param name="fProbability">частота деревьев (0..1). Всего будет высажено примерно [число вершин в периметре локации]*3*[частота деревьев] деревьев</param>
        private void AddTreeModels(LocationX pLoc, ref LandType eLT, ref VertexMultitextured[] aVertices, ref Dictionary<Location, int> cLocations, ref Dictionary<Vertex, int> cVertexes, ref Dictionary<Model, List<TreeModel>> cTrees, float fScale, float fProbability)
        {
            //fProbability /= 1 + (pLoc.m_cRoads[RoadQuality.Country].Count + pLoc.m_cRoads[RoadQuality.Normal].Count + pLoc.m_cRoads[RoadQuality.Good].Count)/2;

            //List<LocationX> cHaveRoadsTo = new List<LocationX>();
            //foreach (Road pRoad in pLoc.m_cRoads[RoadQuality.Country])
            //    cHaveRoadsTo.AddRange(pRoad.Locations);
            //foreach (Road pRoad in pLoc.m_cRoads[RoadQuality.Normal])
            //    cHaveRoadsTo.AddRange(pRoad.Locations);
            //foreach (Road pRoad in pLoc.m_cRoads[RoadQuality.Good])
            //    cHaveRoadsTo.AddRange(pRoad.Locations);

            //while (cHaveRoadsTo.Remove(pLoc)) { };

            //последовательно перебирает все связанные линии, пока круг не замкнётся.
            Location.Edge pLine = pLoc.m_pFirstLine;
            do
            {
                bool bHaveRoad = false;
                //foreach (LocationX pLocP2 in pLine.m_pMidPoint.m_cLocations)
                //    if (cHaveRoadsTo.Contains(pLocP2))
                //        bHaveRoad = true;

                Vector3 pCenter = aVertices[cVertexes[pLine.m_pInnerPoint]].Position;

                if (pLoc.m_pSettlement == null &&
                    pLoc.m_cRoads[RoadQuality.Country].Count == 0 &&
                    pLoc.m_cRoads[RoadQuality.Normal].Count == 0 &&
                    pLoc.m_cRoads[RoadQuality.Good].Count == 0 &&
                    Rnd.Get(1f) < fProbability)
                    AddTreeModel((pCenter + aVertices[cLocations[pLoc]].Position) / 2, ref eLT, ref cTrees, fScale);

                if (pLoc.m_pSettlement == null ||
                    pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Hamlet ||
                    pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Village ||
                    pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Town ||
                    pLoc.m_pSettlement.m_pInfo.m_eSize == Socium.Settlements.SettlementSize.Fort)
                {
                    if (Rnd.Get(1f) < fProbability && !bHaveRoad)
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

        /// <summary>
        /// посадить дерево в указанную точку
        /// </summary>
        /// <param name="pPos">координаты дерева</param>
        /// <param name="eLT">тип территории для выбора модели дерева</param>
        /// <param name="cTrees">пополняемый список деревьев</param>
        /// <param name="fScale">масштабный коэффициент</param>
        private void AddTreeModel(Vector3 pPos, ref LandType eLT, ref Dictionary<Model, List<TreeModel>> cTrees, float fScale)
        {
            Model pTree = null;
            switch (eLT)
            {
                case LandType.Forest:
                    pTree = g.m_aTreeModels[Rnd.Get(g.m_aTreeModels.Length)];
                    break;
                case LandType.Jungle:
                    if (Rnd.OneChanceFrom(3))
                        pTree = g.m_aTreeModels[Rnd.Get(g.m_aTreeModels.Length)];
                    else
                    {
                        pTree = g.m_aPalmModels[Rnd.Get(g.m_aPalmModels.Length)];
                        fScale *= 1.75f;
                    }
                    break;
                case LandType.Taiga:
                    pTree = Rnd.OneChanceFrom(3) ? g.m_aTreeModels[Rnd.Get(g.m_aTreeModels.Length)] : g.m_aPineModels[Rnd.Get(g.m_aPineModels.Length)];
                    break;
            }
            //pTree = g.m_aTreeModels[Rnd.Get(g.m_aTreeModels.Length)];

            if (pTree != null)
            {
                List<TreeModel> cInstances;
                if (!cTrees.TryGetValue(pTree, out cInstances))
                {
                    cInstances = new List<TreeModel>();
                    cTrees[pTree] = cInstances;
                }

                cInstances.Add(new TreeModel(pPos, Rnd.Get((float)Math.PI * 2), fScale, pTree, g.m_pTreeTexture));
            }
        }

        ~Square()
        {
            if (!string.IsNullOrEmpty(m_sCacheFileName))
                if (File.Exists(m_sCacheFileName))
                    File.Delete(m_sCacheFileName);
        }

        public float m_fMinHeight = float.MaxValue;
        public float m_fMaxHeight = float.MinValue;

        public float m_fWorldMaxHeight = float.MinValue;

        public Square(GraphicsDevice pDevice, Chunk<LocationX> pChunk, float fR, float fMaxHeight, Model[] treeModel, Model[] palmModel, Model[] pineModel, Texture2D pTreeTexture)
        {
            m_bCleared = true;

            m_pChunk = pChunk;
            m_fR = fR;
            m_fWorldMaxHeight = fMaxHeight;

            g.m_aTreeModels = treeModel;
            g.m_aPalmModels = palmModel;
            g.m_aPineModels = pineModel;
            g.m_pTreeTexture = pTreeTexture;

            m_fMinHeight = float.MaxValue;
            m_fMaxHeight = float.MinValue;

            for (int i = 0; i < m_pChunk.m_aVertexes.Length; i++)
            {
                var vertex = m_pChunk.m_aVertexes[i];
                if (m_fMinHeight > vertex.m_fH)
                    m_fMinHeight = vertex.m_fH;
                if (m_fMaxHeight < vertex.m_fH)
                    m_fMaxHeight = vertex.m_fH;
            }

            for (int i = 0; i < m_pChunk.m_aLocations.Length; i++)
            {
                var loc = m_pChunk.m_aLocations[i];

                if (m_fMinHeight > loc.m_fH)
                    m_fMinHeight = loc.m_fH;
                if (m_fMaxHeight < loc.m_fH)
                    m_fMaxHeight = loc.m_fH;
            }

            BuildBoundingBox(m_pChunk, m_fMinHeight*m_fLandHeightMultiplier, m_fMaxHeight*m_fLandHeightMultiplier);
            
            g.m_aTrees = new Dictionary<Model, TreeModel[]>();            
            
            foreach (Model pModel in treeModel)
                g.m_aTrees[pModel] = new TreeModel[0];

            foreach (Model pModel in palmModel)
                g.m_aTrees[pModel] = new TreeModel[0];

            foreach (Model pModel in pineModel)
                g.m_aTrees[pModel] = new TreeModel[0];

            //Rebuild(pDevice);
        }

        /// <summary>
        /// нормализуем восьмимерный (2 четвёрки) вектор текстурных весов
        /// </summary>
        private static void NormalizeTextureWeights(ref VertexMultitextured pData)
        {
            float fD = (float)Math.Sqrt(pData.TexWeights.LengthSquared() + pData.TexWeights2.LengthSquared());

            if (fD == 0)
                throw new Exception();

            pData.TexWeights /= fD;
            pData.TexWeights2 /= fD;
        }

        private float GetTemperature(Vertex pVertex)
        {
            float fNorthX = 150 / (float)Math.Sqrt(3);
            float fNorthY = 150 / (float)Math.Sqrt(3);
            float fNorthZ = 150 / (float)Math.Sqrt(3);

            float fDistNorth = (float)Math.Sqrt((pVertex.m_fX - fNorthX) * (pVertex.m_fX - fNorthX) + (pVertex.m_fY - fNorthY) * (pVertex.m_fY - fNorthY) + (pVertex.m_fZ - fNorthZ) * (pVertex.m_fZ - fNorthZ));
            float fDistSouth = (float)Math.Sqrt((pVertex.m_fX + fNorthX) * (pVertex.m_fX + fNorthX) + (pVertex.m_fY + fNorthY) * (pVertex.m_fY + fNorthY) + (pVertex.m_fZ + fNorthZ) * (pVertex.m_fZ + fNorthZ));

            if (fDistNorth < fDistSouth)
                return fDistNorth / 213;
            else
                return fDistSouth / 213;
        }

        /// <summary>
        /// заполняет индексный буфер границы указанной локации
        /// </summary>
        /// <param name="pLoc">локация, границу которой строим</param>
        /// <param name="cVertexes">словарь индексов вершин в вершинном буфере</param>
        /// <returns></returns>
        private int[] BuildLocationReferencesIndices(Location pLoc, ref Dictionary<Vertex, int> cVertexes)
        {
            //добавляем в вершинный буффер центры локаций
            int m_iLinesCount = pLoc.m_cBorderWith.Count * 2;

            // Create the indices used for each triangle
            int[] aIndices = new int[m_iLinesCount * 2];

            int iCounter = 0;

            //заполняем индексный буффер
            foreach (var pEdge in pLoc.m_cBorderWith)
            {
                var pLine = pEdge.Value[0];
                aIndices[iCounter++] = cVertexes[pLine.m_pPoint1];
                aIndices[iCounter++] = cVertexes[pLine.m_pMidPoint];

                aIndices[iCounter++] = cVertexes[pLine.m_pMidPoint];
                aIndices[iCounter++] = cVertexes[pLine.m_pPoint2];
            }

            m_iLocationsIndicesCount += aIndices.Length;
            return aIndices;
        }

        /// <summary>
        /// Checks whether a ray intersects a model. This method needs to access
        /// the model vertex data, so the model must have been built using the
        /// custom TrianglePickingProcessor provided as part of this sample.
        /// Returns the distance along the ray to the point of intersection, or null
        /// if there is no intersection.
        /// </summary>
        public float? RayIntersectsLandscape(Ray ray, Matrix modelTransform,
                                         ref int iLoc)
        {
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

            // Keep track of the closest triangle we found so far,
            // so we can always return the closest one.
            float? closestIntersection = null;

            if (iLoc >= 0 && iLoc < g.m_aLocations.GetLength(0))
            {
                Vector3 pLocationCenter = new Vector3(m_pChunk.m_aLocations[iLoc].m_fX, m_pChunk.m_aLocations[iLoc].m_fY, m_pChunk.m_aLocations[iLoc].m_fZ);
                pLocationCenter += Vector3.Normalize(pLocationCenter) * m_pChunk.m_aLocations[iLoc].m_fH;
                for (int i = 0; i < g.m_aLocations[iLoc].Length; i+=4)
                {
                    float? intersection;

                    RayIntersectsTriangle(ref ray,
                                            ref pLocationCenter,
                                            ref g.m_aLandPoints[g.m_aLocations[iLoc][i]].Position,
                                            ref g.m_aLandPoints[g.m_aLocations[iLoc][i + 3]].Position,
                                            out intersection);

                    // Does the ray intersect this triangle?
                    if (intersection != null)
                    {
                        return intersection;
                    }
                }
                //поиск по непосредственным соседям - это хорошо, но работать не будет, т.к. (1) мы не знаем индекс соседней локации и (2) даже если бы знали, она вполне может оказаться в другом квадрате
                //foreach (var pEdge in m_pChunk.m_aLocations[iLoc].m_cEdges)
                //{
                //    if (!m_pChunk.m_aLocations.Contains(pEdge.Key))
                //        continue;

                //    int iEdge = m_pChunk.m_aLocations.IndexOf(pEdge.Key)

                //    pLocationCenter = new Vector3(pEdge.Key.m_fX, pEdge.Key.m_fY, pEdge.Key.m_fZ);
                //    pLocationCenter += Vector3.Normalize(pLocationCenter) * pEdge.Key.m_fH;
                //    Vector3 v1 = new Vector3(pEdge.Value.m_pFrom.m_fX, pEdge.Value.m_pFrom.m_fY, pEdge.Value.m_pFrom.m_fZ);
                //    v1 += Vector3.Normalize(v1) * pEdge.Value.m_pFrom.m_fH;
                //    Vector3 v2 = new Vector3(pEdge.Value.m_pTo.m_fX, pEdge.Value.m_pTo.m_fY, pEdge.Value.m_pTo.m_fZ);
                //    v2 += Vector3.Normalize(v2) * pEdge.Value.m_pTo.m_fH;

                //    float? intersection;

                //    RayIntersectsTriangle(ref ray,
                //                            ref pLocationCenter,
                //                            ref v1,
                //                            ref v2,
                //                            out intersection);

                //    // Does the ray intersect this triangle?
                //    if (intersection != null)
                //    {
                //        iLoc = iEdge;
                //        return intersection;
                //    }
                //}
            }

            for (int i = 0; i < g.m_aLandIndicesLR.Length; i += 3)
            {
                // Perform a ray to triangle intersection test.
                float? intersection;

                RayIntersectsTriangle(ref ray,
                                        ref g.m_aLandPoints[g.m_aLandIndicesLR[i]].Position,
                                        ref g.m_aLandPoints[g.m_aLandIndicesLR[i + 1]].Position,
                                        ref g.m_aLandPoints[g.m_aLandIndicesLR[i + 2]].Position,
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

                        iLoc = g.m_aLocationReferences[i / 3];
                    }
                }
            }

            return closestIntersection;
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
            if (rayDistance < 0)
            {
                result = null;
                return;
            }

            result = rayDistance;
        }

        public override string ToString()
        {
            return m_pBounds8.m_pSphere.ToString();
        }
    }
}
