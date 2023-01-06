using System;
using System.Collections.Generic;
namespace LandscapeGeneration
{
    public interface ITerritory
    {
        /// <summary>
        /// Границы с другими ТАКИМИ ЖЕ объектами
        /// </summary>
        Dictionary<ITerritory, List<VoronoiEdge>> BorderWith { get; }

        bool Forbidden { get; }
        //ITerritory Owner { get; set; }

        ITerritory AddLayer<T>(T value) where T : class, IInfoLayer;
        T GetLayer<T>() where T : class, IInfoLayer;
        bool HasLayer<T>() where T : class, IInfoLayer;

        /// <summary>
        /// Суммарная длина всех линий в BorderWith
        /// </summary>
        float PerimeterLength { get; }
    }
}
