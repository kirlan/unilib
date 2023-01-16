using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandscapeGeneration
{
    /// <summary>
    /// Биом - группа ВСЕХ сопредельных земель (<see cref="Land"/>/<see cref="LandBiome"/>) одного типа <see cref="LandTypeInfo"/>.
    /// Используется ТОЛЬКО при сглаживании границ локаций - внутри <see cref="Landscape.SmoothBiomesBorders"/>
    /// </summary>
    public sealed class Biome : TerritoryCluster<Biome, Landscape, LandBiome>
    {
        public Biome()
        { }

        public Biome(LandBiome pSeed, float fCycleShift)
        {
            InitBorder(pSeed);

            Contents.Add(pSeed);

            bool bAdded;
            do
            {
                bAdded = false;

                //Сохраняем текущий край биома в отдельный массив, чтобы потом бежать по нему и модифицировать актуальный край
                LandBiome[] aBorderLands = Border.Keys.ToArray();
                foreach (LandBiome pLand in aBorderLands)
                {
                    if (pLand.Forbidden)
                        continue;

                    if (pLand.Origin.LandType == pSeed.Origin.LandType && !Contents.Contains(pLand))
                    {
                        Contents.Add(pLand);

                        Border[pLand].Clear();
                        Border.Remove(pLand);

                        foreach (var pBorderLand in pLand.BorderWith)
                        {
                            if (Contents.Contains(pBorderLand.Key))
                                continue;

                            if (!Border.ContainsKey(pBorderLand.Key))
                                Border[pBorderLand.Key] = new List<VoronoiEdge>();
                            VoronoiEdge[] cLines = pBorderLand.Value.ToArray();
                            foreach (var pLine in cLines)
                                Border[pBorderLand.Key].Add(new VoronoiEdge(pLine));
                        }

                        bAdded = true;
                    }
                }
            }
            while (bAdded);

            ChainBorder(fCycleShift);
        }

        public override float GetMovementCost() { return 0; }
    }
}
