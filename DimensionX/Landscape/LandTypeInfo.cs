using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LandscapeGeneration
{
    public enum LandType
    {
        Coastral,
        Ocean,
        Mountains,
        Tundra,
        Taiga,
        Forest,
        Plains,
        Desert,
        Savanna,
        Swamp,
        Jungle
    }

    [Flags]
    public enum Environment
    {
        None = 0,
        /// <summary>
        /// Плоский ландшафт, без мешающих передвижению элементов, будь то скалы или деревья
        /// </summary>
        Flat = 1,
        /// <summary>
        /// Не твёрдая поверхность - песок или грязь...
        /// </summary>
        Soft = 2,
        /// <summary>
        /// Жидкость
        /// </summary>
        Liquid = 4,
        /// <summary>
        /// Ландшафт с сильными перепадами высот
        /// </summary>
        Barrier = 8,
        /// <summary>
        /// Местность с хорошим обзором, т.е. без крупной растительности
        /// </summary>
        Open = 16,
        /// <summary>
        /// Высокая влажность
        /// </summary>
        Wet = 32,
        /// <summary>
        /// Температура выше среднего
        /// </summary>
        Hot = 64,
        /// <summary>
        /// Температура ниже среднего
        /// </summary>
        Cold = 128,
        /// <summary>
        /// Вообще обитаемые земли
        /// </summary>
        Habitable = 256
    }

    public class LandTypeInfo
    {
        public Color m_pColor;
        public Brush m_pBrush;
        public LandType m_eType;
        public Environment m_eEnvironment = Environment.None;

        public void SetColor(Color pColor)
        {
            m_pColor = pColor;
            m_pBrush = new SolidBrush(m_pColor);
        }
        
        public int m_iMovementCost = 100;

        public string m_sName;

        public float m_fElevation = 0;

        public void Init(int iMovementCost, float fElevation, Environment eEnvironment, string sName)
        {
            m_iMovementCost = iMovementCost;
            m_fElevation = fElevation;
            m_eEnvironment = eEnvironment;
            m_sName = sName;
        }
    }

    public class LandTypes<LTI>
        where LTI: LandTypeInfo, new()
    {
        public Dictionary<LandType, LTI> m_pLandTypes = new Dictionary<LandType, LTI>();

        public static LandTypes<LTI> m_pInstance = new LandTypes<LTI>();

        private LandTypes()
        {
            foreach (LandType eType in Enum.GetValues(typeof(LandType)))
            {
                m_pLandTypes[eType] = new LTI();
                m_pLandTypes[eType].m_eType = eType;
            }
        }

        public static LTI Desert
        {
            get { return m_pInstance.m_pLandTypes[LandType.Desert]; }
        }

        public static LTI Forest
        {
            get { return m_pInstance.m_pLandTypes[LandType.Forest]; }
        }

        public static LTI Jungle
        {
            get { return m_pInstance.m_pLandTypes[LandType.Jungle]; }
        }

        public static LTI Mountains
        {
            get { return m_pInstance.m_pLandTypes[LandType.Mountains]; }
        }

        public static LTI Plains
        {
            get { return m_pInstance.m_pLandTypes[LandType.Plains]; }
        }

        public static LTI Savanna
        {
            get { return m_pInstance.m_pLandTypes[LandType.Savanna]; }
        }

        public static LTI Ocean
        {
            get { return m_pInstance.m_pLandTypes[LandType.Ocean]; }
        }

        public static LTI Coastral
        {
            get { return m_pInstance.m_pLandTypes[LandType.Coastral]; }
        }

        public static LTI Swamp
        {
            get { return m_pInstance.m_pLandTypes[LandType.Swamp]; }
        }

        public static LTI Taiga
        {
            get { return m_pInstance.m_pLandTypes[LandType.Taiga]; }
        }

        public static LTI Tundra
        {
            get { return m_pInstance.m_pLandTypes[LandType.Tundra]; }
        }
    }
}
