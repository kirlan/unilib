using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniLibXNA;
using Microsoft.Xna.Framework;
using LandscapeGeneration;
using Random;
using ContentLoader;

namespace MapDrawXNAEngine
{
    internal class RoadData
    {
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
                pLeft.Position = m_aShape[i].m_pPosition + pCrossAverage * 2 - pUplift * 30;
                pLeft.Normal = m_aShape[i].m_pNormal;
                //pLeft.Color = Microsoft.Xna.Framework.Color.Tan;
                pLeft.TexWeights = m_aShape[i].TexWeights;
                pLeft.TexWeights2 = m_aShape[i].TexWeights2;
                pLeft.TextureCoordinate = GetTextureCoordinate(pLeft.Position, eShape, RX, RY, fTextureScale);

                VertexMultitextured pLeftInner = new VertexMultitextured();
                pLeftInner.Position = m_aShape[i].m_pPosition + pCrossAverage / 2 + pUplift * 0.5f;
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
                pRight.Position = m_aShape[i].m_pPosition - pCrossAverage * 2 - pUplift * 30;
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
                pCenter.TextureCoordinate = GetTextureCoordinate(pCenter.Position, eShape, RX, RY, fTextureScale); //m_aShape[i].TextureCoordinate;
                pCenter.TexWeights = m_aShape[i].TexWeights;
                pCenter.TexWeights2 = m_aShape[i].TexWeights2;
                if (i == 0 || i == m_aShape.Length - 1)
                    pCenter.TexWeights2.Y = fScale * 8;
                else
                {
                    //pCenter.TexWeights = new Vector4(0);
                    //pCenter.TexWeights2 = new Vector4(0);
                    pCenter.TexWeights2.Y = fScale * 6 + Rnd.Get(fScale * 4);//0.5f + Rnd.Get(1f);// fScale + Rnd.Get(fScale * 2);
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
                    pStart.Position = m_aShape[0].m_pPosition + pBackward * pCrossAverage.Length() - pUplift * 5;
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
                    pFinish.Position = m_aShape[i].m_pPosition + pForward * pCrossAverage.Length() - pUplift * 5;
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

}
