using System;
using System.Collections.Generic;
namespace LandscapeGeneration
{
    public interface ITerritory
    {
        /// <summary>
        /// Границы с другими такими же объектами
        /// </summary>
        Dictionary<object, List<Line>> BorderWith { get; }
        bool Forbidden { get; }
        object Owner { get; set; }
    }
}
