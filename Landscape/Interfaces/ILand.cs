using System;
using LandscapeGeneration.PathFind;
namespace LandscapeGeneration
{
    public interface ITypedLand<LTI> : ILand
        where LTI : LandTypeInfo
    {
        /// <summary>
        /// Тип территории
        /// </summary>
        LTI Type { get; set; }
    }

    /// <summary>
    /// Интерфейс для объекта "земля"
    /// </summary>
    public interface ILand : ITerritory, ITransportationNode
    {
        /// <summary>
        /// Влажность, в процентах 0-100
        /// </summary>
        int Humidity { get; set; }

        /// <summary>
        /// Географическое образование, в которое входит эта земля
        /// </summary>
        object Area { get; set; }

        float MovementCost { get; }

        bool IsWater { get; }

        string GetLandsString();
    }
}
