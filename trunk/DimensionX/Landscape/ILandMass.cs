using System;
using LandscapeGeneration.PathFind;
namespace LandscapeGeneration
{
    public interface ILandMass : ITerritory, ITransportationNode
    {
        bool IsWater { get; }

        string GetLandsString();
    }
}
