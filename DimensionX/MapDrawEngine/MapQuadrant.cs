using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using LandscapeGeneration;
using Socium;

namespace MapDrawEngine
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

    internal enum RoadType
    {
        Back,
        LandRoad1,
        LandRoad2,
        LandRoad3,
        SeaRoute1,
        SeaRoute2,
        SeaRoute3,
    }
    
    class MapQuadrant
    {
        internal List<ILandMark> m_cLandmarks = new List<ILandMark>();

        internal Dictionary<MapLayer, GraphicsPath> m_cLayers = new Dictionary<MapLayer,GraphicsPath>();
        internal Dictionary<MapMode, Dictionary<Brush, GraphicsPath>> m_cModes = new Dictionary<MapMode,Dictionary<Brush,GraphicsPath>>();
        internal Dictionary<RoadType, GraphicsPath> m_cRoadsMap = new Dictionary<RoadType, GraphicsPath>();

        public MapQuadrant()
        {
            foreach (MapLayer eLayer in Enum.GetValues(typeof(MapLayer)))
                m_cLayers[eLayer] = new GraphicsPath();

            foreach (MapMode eMode in Enum.GetValues(typeof(MapMode)))
                m_cModes[eMode] = new Dictionary<Brush,GraphicsPath>();

            foreach (RoadType eRoadType in Enum.GetValues(typeof(RoadType)))
                m_cRoadsMap[eRoadType] = new GraphicsPath();
        }

        internal void Clear()
        {
            m_cLandmarks.Clear();

            foreach (var pLayer in m_cLayers)
                pLayer.Value.Reset();

            foreach (var pMode in m_cModes)
                pMode.Value.Clear();

            foreach (var pRoadType in m_cRoadsMap)
                pRoadType.Value.Reset();
        }

        /// <summary>
        /// приводит все координаты в систему координат с началом в верхнем левом углу квадранта
        /// </summary>
        /// <param name="fStartX">X-координата верхнего левого угла квадранта в абсолютных координатах</param>
        /// <param name="fStartY">Y-координата верхнего левого угла квадранта в абсолютных координатах</param>
        internal void Normalize(float fStartX, float fStartY)
        {
            DateTime pTime1 = DateTime.Now;

            Matrix pMatrix = new Matrix();
            pMatrix.Translate(-fStartX, -fStartY);

            foreach (var pLandMark in m_cLandmarks)
                pLandMark.Translate(-fStartX, -fStartY);

            foreach (var pLayer in m_cLayers)
                pLayer.Value.Transform(pMatrix);

            foreach(var pMode in m_cModes)
                foreach (var pModeLayer in pMode.Value)
                    pModeLayer.Value.Transform(pMatrix);

            foreach (var pRoadType in m_cRoadsMap)
                pRoadType.Value.Transform(pMatrix);

            DateTime pTime2 = DateTime.Now;
        }

        /// <summary>
        /// Рисует контур
        /// </summary>
        /// <param name="gr">куда рисовать</param>
        /// <param name="eLayer">какой слой рисовать</param>
        /// <param name="fDX">экранная X-координата левого верхнего угла квадранта</param>
        /// <param name="fDY">экранная Y-координата левого верхнего угла квадранта</param>
        /// <param name="fScale">масштаб (экранные кординаты : абсолютные)</param>
        internal void DrawPath(Graphics gr, MapLayer eLayer, float fDX, float fDY, float fScale)
        {
            Pen pPen = Pens.Black;

            switch (eLayer)
            { 
                case MapLayer.Continents:
                    pPen = MapDraw.s_pBlack3Pen;
                    break;
                case MapLayer.Locations:
                    pPen = Pens.DarkGray;
                    break;
                case MapLayer.Provincies:
                    pPen = MapDraw.s_pWhite2Pen;
                    break;
                case MapLayer.States:
                    pPen = MapDraw.s_pBlack3Pen;
                    break;
                case MapLayer.LandMasses:
                    pPen = MapDraw.s_pRed2Pen;
                    break;
            }

            Matrix pMatrix = new Matrix();
            pMatrix.Translate(fDX, fDY);
            pMatrix.Scale(fScale, fScale);

            GraphicsPath pPath = (GraphicsPath)m_cLayers[eLayer].Clone();
            pPath.Transform(pMatrix);

            //if (pPath2.PointCount > 0 && fDX > 250)
            //    throw new Exception();

            gr.DrawPath(pPen, pPath);

            if (eLayer == MapLayer.Provincies)
                gr.DrawPath(MapDraw.s_pBlack3DotPen, pPath);
        }

        /// <summary>
        /// Рисует сетку дорог
        /// </summary>
        /// <param name="gr">куда рисовать</param>
        /// <param name="fScaleMultiplier">уровень мастабирования (в зависимости от него некоторые дороги могут рисоваться или нет)</param>
        /// <param name="fDX">экранная X-координата левого верхнего угла квадранта</param>
        /// <param name="fDY">экранная Y-координата левого верхнего угла квадранта</param>
        /// <param name="fScale">масштаб (экранные кординаты : абсолютные)</param>
        internal void DrawRoads(Graphics gr, float fScaleMultiplier, float fDX, float fDY, float fScale)
        {
            Matrix pMatrix = new Matrix();
            pMatrix.Translate(fDX, fDY);
            pMatrix.Scale(fScale, fScale);

            GraphicsPath pPath = (GraphicsPath)m_cRoadsMap[RoadType.Back].Clone();
            pPath.Transform(pMatrix);
            gr.DrawPath(MapDraw.s_pDarkGrey3Pen, pPath);

            pPath = (GraphicsPath)m_cRoadsMap[RoadType.LandRoad3].Clone();
            pPath.Transform(pMatrix);
            gr.DrawPath(MapDraw.s_pBlack2Pen, pPath);

            //pPath = (GraphicsPath)m_cRoadsMap[RoadType.SeaRoute3].Clone();
            //pPath.Transform(pMatrix);
            //gr.DrawPath(MapDraw.s_pAqua2Pen, pPath);

            pPath = (GraphicsPath)m_cRoadsMap[RoadType.LandRoad2].Clone();
            pPath.Transform(pMatrix);
            gr.DrawPath(MapDraw.s_pBlack1Pen, pPath);

            //pPath = (GraphicsPath)m_cRoadsMap[RoadType.SeaRoute2].Clone();
            //pPath.Transform(pMatrix);
            //gr.DrawPath(MapDraw.s_pAqua1Pen, pPath);

            if (fScaleMultiplier > 2)
            {
                pPath = (GraphicsPath)m_cRoadsMap[RoadType.LandRoad1].Clone();
                pPath.Transform(pMatrix);
                gr.DrawPath(MapDraw.s_pBlack1DotPen, pPath);

                //pPath = (GraphicsPath)m_cRoadsMap[RoadType.SeaRoute1].Clone();
                //pPath.Transform(pMatrix);
                //gr.DrawPath(MapDraw.s_pAqua1DotPen, pPath);
            }
        }

        /// <summary>
        /// Рисует условные обозначения
        /// </summary>
        /// <param name="gr">куда рисовать</param>
        /// <param name="fScaleMultiplier">уровень мастабирования (в зависимости от него некоторые значки могут рисоваться или нет)</param>
        /// <param name="fDX">экранная X-координата левого верхнего угла квадранта</param>
        /// <param name="fDY">экранная Y-координата левого верхнего угла квадранта</param>
        /// <param name="fScale">масштаб (экранные кординаты : абсолютные)</param>
        internal void DrawLandMarks(Graphics gr, float fScaleMultiplier, float fDX, float fDY, float fScale)
        {
            foreach (ILandMark pLandMark in m_cLandmarks)
                pLandMark.Draw(gr, fScaleMultiplier, fDX, fDY, fScale);
        }

        /// <summary>
        /// закрашивает контур
        /// </summary>
        /// <param name="gr">куда рисовать</param>
        /// <param name="eMode">что рисовать</param>
        /// <param name="fDX">экранная X-координата левого верхнего угла квадранта</param>
        /// <param name="fDY">экранная Y-координата левого верхнего угла квадранта</param>
        /// <param name="fScale">масштаб (экранные кординаты : абсолютные)</param>
        internal void FillPath(Graphics gr, MapMode eMode, float fDX, float fDY, float fScale)
        {
            Matrix pMatrix = new Matrix();
            pMatrix.Translate(fDX, fDY);
            pMatrix.Scale(fScale, fScale);

            foreach (var pModeLayer in m_cModes[eMode])
            {
                GraphicsPath pPath = (GraphicsPath)pModeLayer.Value.Clone();
                pPath.Transform(pMatrix);

                gr.FillPath(pModeLayer.Key, pPath);
            }
        }
    }
}
