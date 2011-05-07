using System;
using System.Collections.Generic;
namespace LandscapeGeneration
{
    public interface ITerritory
    {
        Dictionary<object, List<Line>> BorderWith { get; }
        bool Forbidden { get; }
        object Owner { get; set; }
    }
}
