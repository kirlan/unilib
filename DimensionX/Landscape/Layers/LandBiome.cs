using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandscapeGeneration
{
    /// <summary>
    /// Расширение <see cref="Land"/> для формирования кластеров <see cref="Biome"/>.
    /// Используется ТОЛЬКО при сглаживании границ локаций - внутри <see cref="Landscape.SmoothBiomesBorders"/>
    /// </summary>
    public sealed class LandBiome : TerritoryExtended<LandBiome, Biome, Land>
    {
        public LandBiome(Land pLand) : base(pLand)
        { }

        public LandBiome()
        { }
    }
}
