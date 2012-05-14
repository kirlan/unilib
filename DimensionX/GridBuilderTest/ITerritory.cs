using System;
using System.Collections.Generic;
namespace GridBuilderTest
{
    public interface ITerritory
    {
        /// <summary>
        /// Границы с другими такими же объектами
        /// </summary>
        Dictionary<object, List<Line>> BorderWith { get; }
        bool Forbidden { get; }
        object Owner { get; set; }
        float PerimeterLength { get; }
    }
}
