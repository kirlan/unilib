using System;
using System.Collections.Generic;
using System.Text;
using Random;

namespace EconomyModel3
{
    public class Refinery: Building
    {
        public Refinery()
            : base(CommodityCategorizer.PrimeResources[Rnd.Get(CommodityCategorizer.PrimeResources.Count)], 1 + Rnd.Get(10))
        { 
        }
    }
}
