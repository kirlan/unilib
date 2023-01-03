using System;
using System.Collections.Generic;
namespace LandscapeGeneration
{
    public interface ITerritory
    {
        /// <summary>
        /// Границы с другими ТАКИМИ ЖЕ объектами
        /// </summary>
        Dictionary<ITerritory, List<Location.Edge>> BorderWith { get; }

        bool Forbidden { get; }
        ITerritory Owner { get; set; }

        /// <summary>
        /// Суммарная длина всех линий в BorderWith
        /// </summary>
        float PerimeterLength { get; }
    }
}
