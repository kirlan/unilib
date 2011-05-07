using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace VQMapTest2
{
    class MapQuadrant
    {
        internal List<ILandMark> m_cLandmarks = new List<ILandMark>();

        internal GraphicsPath m_pContinentsPath = new GraphicsPath();
        internal GraphicsPath m_pContinentsPath2 = new GraphicsPath();
        internal GraphicsPath m_pLocationsPath = new GraphicsPath();
        internal GraphicsPath m_pLandsPath = new GraphicsPath();
        internal GraphicsPath m_pProvinciesPath = new GraphicsPath();
        internal GraphicsPath m_pLandMassesPath = new GraphicsPath();
        internal GraphicsPath m_pStatesPath = new GraphicsPath();

        internal Dictionary<Brush, GraphicsPath> m_cAreaMap = new Dictionary<Brush, GraphicsPath>();
        internal Dictionary<Brush, GraphicsPath> m_cNativesMap = new Dictionary<Brush, GraphicsPath>();
        internal Dictionary<Brush, GraphicsPath> m_cNationsMap = new Dictionary<Brush, GraphicsPath>();
        internal Dictionary<Brush, GraphicsPath> m_cHumidityMap = new Dictionary<Brush, GraphicsPath>();
        internal Dictionary<WorldMap.RoadType, GraphicsPath> m_cRoadsMap = new Dictionary<WorldMap.RoadType, GraphicsPath>();

        public MapQuadrant()
        {
            foreach (WorldMap.RoadType eRT in Enum.GetValues(typeof(WorldMap.RoadType)))
                m_cRoadsMap[eRT] = new GraphicsPath();
        }

        internal void Clear(bool bFast)
        {
            m_pContinentsPath.Reset();
            m_cAreaMap.Clear();

            if (!bFast)
            {
                m_cLandmarks.Clear();

                m_pLocationsPath.Reset();
                m_pLandsPath.Reset();
                m_pProvinciesPath.Reset();
                m_pLandMassesPath.Reset();
                m_pStatesPath.Reset();

                m_cHumidityMap.Clear();
                m_cNativesMap.Clear();
                m_cNationsMap.Clear();
            }
            foreach(var pRM in m_cRoadsMap)
                pRM.Value.Reset();
        }

        internal void ScalePaths(float fScale)
        {
            if (fScale == 1)
                return;

            DateTime pTime1 = DateTime.Now;

            Matrix pMatrix = new Matrix();
            pMatrix.Scale(fScale, fScale);

            foreach (var pPair in m_cLandmarks)
            {
                pPair.Scale(fScale);
            }

            m_pContinentsPath.Transform(pMatrix);
            m_pLocationsPath.Transform(pMatrix);
            m_pLandsPath.Transform(pMatrix);
            m_pProvinciesPath.Transform(pMatrix);
            m_pLandMassesPath.Transform(pMatrix);
            m_pStatesPath.Transform(pMatrix);

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

            foreach (var pPair in m_cRoadsMap)
                pPair.Value.Transform(pMatrix);

            DateTime pTime2 = DateTime.Now;
        }

        internal void Normalize(float fStartX, float fStartY)
        {
            DateTime pTime1 = DateTime.Now;

            Matrix pMatrix = new Matrix();
            pMatrix.Translate(-fStartX, -fStartY);

            foreach (var pPair in m_cLandmarks)
            {
                pPair.Translate(-fStartX, -fStartY);
            }

            m_pContinentsPath.Transform(pMatrix);
            m_pLocationsPath.Transform(pMatrix);
            m_pLandsPath.Transform(pMatrix);
            m_pProvinciesPath.Transform(pMatrix);
            m_pLandMassesPath.Transform(pMatrix);
            m_pStatesPath.Transform(pMatrix);

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

            foreach (var pPair in m_cRoadsMap)
                pPair.Value.Transform(pMatrix);

            DateTime pTime2 = DateTime.Now;
        }

        internal void DrawPath(Graphics gr, Pen pPen, GraphicsPath pPath, float fDX, float fDY)
        {
            Matrix pMatrix = new Matrix();
            pMatrix.Translate(fDX, fDY);

            GraphicsPath pPath2 = (GraphicsPath)pPath.Clone();
            pPath2.Transform(pMatrix);

            gr.DrawPath(pPen, pPath2);
        }

        internal void FillPath(Graphics gr, Brush pBrush, GraphicsPath pPath, float fDX, float fDY)
        {
            Matrix pMatrix = new Matrix();
            pMatrix.Translate(fDX, fDY);

            GraphicsPath pPath2 = (GraphicsPath)pPath.Clone();
            pPath2.Transform(pMatrix);

            gr.FillPath(pBrush, pPath2);
        }
    }
}
