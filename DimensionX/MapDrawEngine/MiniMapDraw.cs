using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapDrawEngine
{
    public partial class MiniMapDraw : UserControl
    {
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
        /// связанная основная карта
        /// </summary>
        private MapDraw m_pMasterMap = null;

        public MiniMapDraw()
        {
            InitializeComponent();
        }

        internal void SinchronizeDrawFrame()
        {
            if (m_pMasterMap == null)
                return;

            m_pDrawFrame.X = (int)(m_fActualScale * m_pMasterMap.DrawFrame.X / m_pMasterMap.ActualScale) - 1;
            m_pDrawFrame.Y = (int)(m_fActualScale * m_pMasterMap.DrawFrame.Y / m_pMasterMap.ActualScale) - 1;
            m_pDrawFrame.Width = (int)(m_fActualScale * m_pMasterMap.DrawFrame.Width / m_pMasterMap.ActualScale);
            m_pDrawFrame.Height = (int)(m_fActualScale * m_pMasterMap.DrawFrame.Height / m_pMasterMap.ActualScale) + 1;

            Refresh();
        }

        internal void Assign(MapDraw pMasterMap)
        {
            m_pMasterMap = pMasterMap;

            SinchronizeDrawFrame();
        }
    }
}
