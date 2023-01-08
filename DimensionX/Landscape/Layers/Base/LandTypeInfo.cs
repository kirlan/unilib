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

    public interface ILandTypeInfoExt
    { 
    }

    public class LandTypeInfo
    {
        public readonly Dictionary<Type, dynamic> m_cInfoLayers = new Dictionary<Type, dynamic>();

        public LandTypeInfo AddLayer<T>(T value) where T : ILandTypeInfoExt
        {
            m_cInfoLayers[typeof(T)] = value;

            return this;
        }

        public T Get<T>() where T : ILandTypeInfoExt
        {
            return m_cInfoLayers[typeof(T)];
        }

        public LandType m_eType;

        public int m_iMovementCost = 100;

        public Environment m_eEnvironment = Environment.None;

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

    public class LandTypes
    {
        public Dictionary<LandType, LandTypeInfo> m_pLandTypes = new Dictionary<LandType, LandTypeInfo>();

        public static LandTypes m_pInstance = new LandTypes();

        private LandTypes()
        {
            foreach (LandType eType in Enum.GetValues(typeof(LandType)))
            {
                m_pLandTypes[eType] = new LandTypeInfo();
                m_pLandTypes[eType].m_eType = eType;
            }
        }

        public static LandTypeInfo Desert
        {
            get { return m_pInstance.m_pLandTypes[LandType.Desert]; }
        }

        public static LandTypeInfo Forest
        {
            get { return m_pInstance.m_pLandTypes[LandType.Forest]; }
        }

        public static LandTypeInfo Jungle
        {
            get { return m_pInstance.m_pLandTypes[LandType.Jungle]; }
        }

        public static LandTypeInfo Mountains
        {
            get { return m_pInstance.m_pLandTypes[LandType.Mountains]; }
        }

        public static LandTypeInfo Plains
        {
            get { return m_pInstance.m_pLandTypes[LandType.Plains]; }
        }

        public static LandTypeInfo Savanna
        {
            get { return m_pInstance.m_pLandTypes[LandType.Savanna]; }
        }

        public static LandTypeInfo Ocean
        {
            get { return m_pInstance.m_pLandTypes[LandType.Ocean]; }
        }

        public static LandTypeInfo Coastral
        {
            get { return m_pInstance.m_pLandTypes[LandType.Coastral]; }
        }

        public static LandTypeInfo Swamp
        {
            get { return m_pInstance.m_pLandTypes[LandType.Swamp]; }
        }

        public static LandTypeInfo Taiga
        {
            get { return m_pInstance.m_pLandTypes[LandType.Taiga]; }
        }

        public static LandTypeInfo Tundra
        {
            get { return m_pInstance.m_pLandTypes[LandType.Tundra]; }
        }
    }
}
