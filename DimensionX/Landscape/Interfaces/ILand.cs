using System;
using LandscapeGeneration.PathFind;
namespace LandscapeGeneration
{
    /// <summary>
    /// Интерфейс для объекта "земля"
    /// </summary>
    public interface ILand : ITerritory, ITransportationNode
    {
        /// <summary>
        /// Тип территории
        /// </summary>
        LandTypeInfo LandType { get; set; }

        /// <summary>
        /// Влажность, в процентах 0-100
        /// </summary>
        int Humidity { get; set; }

        /// <summary>
        /// Географическое образование, в которое входит эта земля
        /// </summary>
        //object Region { get; set; }

        float MovementCost { get; }

        bool IsWater { get; }

        string GetLandsString();
    }
}
