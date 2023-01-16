using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandscapeGeneration
{
    /// <summary>
    /// Расширение <see cref="Location"/> для формирования кластеров <see cref="Basin"/>.
    /// </summary>
    public sealed class LocationBasin : TerritoryExtended<LocationBasin, Basin, Location>
    {
        public int? Distance { get; set; } = null;

        public LocationBasin(Location pLoc) : base(pLoc)
        { }

        public LocationBasin()
        { }
        public override string ToString()
        {
            if (Distance.HasValue)
                return string.Format(" Dist: {0}", Distance.Value);
            else
                return " Dist: -";
        }
    }
}
