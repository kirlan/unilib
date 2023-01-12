using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.Immutable;

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
        private readonly Dictionary<Type, dynamic> m_cInfoLayers = new Dictionary<Type, dynamic>();

        public LandTypeInfo AddLayer<T>(T value) where T : ILandTypeInfoExt
        {
            m_cInfoLayers[typeof(T)] = value;

            return this;
        }

        public T Get<T>() where T : ILandTypeInfoExt
        {
            return m_cInfoLayers[typeof(T)];
        }

        public LandType Type { get; }

        public int MovementCost { get; private set; }  = 100;

        public Environment Environment { get; private set; } = Environment.None;

        public string[] Names { get; private set; }

        public float Elevation { get; private set; } = 0;

        public LandTypeInfo(LandType eType)
        {
            Type = eType;
        }

        public void Init(int iMovementCost, float fElevation, Environment eEnvironment, string[] cNames)
        {
            MovementCost = iMovementCost;
            Elevation = fElevation;
            Environment = eEnvironment;
            Names = cNames;
        }
    }

    public sealed class LandTypes
    {
        public ImmutableDictionary<LandType, LandTypeInfo> Lands { get; }

        public static LandTypes Instance { get; } = new LandTypes();

        private LandTypes()
        {
            Dictionary<LandType, LandTypeInfo> cLands = new Dictionary<LandType, LandTypeInfo>();
            foreach (LandType eType in Enum.GetValues(typeof(LandType)))
            {
                cLands[eType] = new LandTypeInfo(eType);
            }
            Lands = cLands.ToImmutableDictionary();
        }

        public static LandTypeInfo Desert
        {
            get { return Instance.Lands[LandType.Desert]; }
        }

        public static LandTypeInfo Forest
        {
            get { return Instance.Lands[LandType.Forest]; }
        }

        public static LandTypeInfo Jungle
        {
            get { return Instance.Lands[LandType.Jungle]; }
        }

        public static LandTypeInfo Mountains
        {
            get { return Instance.Lands[LandType.Mountains]; }
        }

        public static LandTypeInfo Plains
        {
            get { return Instance.Lands[LandType.Plains]; }
        }

        public static LandTypeInfo Savanna
        {
            get { return Instance.Lands[LandType.Savanna]; }
        }

        public static LandTypeInfo Ocean
        {
            get { return Instance.Lands[LandType.Ocean]; }
        }

        public static LandTypeInfo Coastral
        {
            get { return Instance.Lands[LandType.Coastral]; }
        }

        public static LandTypeInfo Swamp
        {
            get { return Instance.Lands[LandType.Swamp]; }
        }

        public static LandTypeInfo Taiga
        {
            get { return Instance.Lands[LandType.Taiga]; }
        }

        public static LandTypeInfo Tundra
        {
            get { return Instance.Lands[LandType.Tundra]; }
        }
    }
}
