using System;
using System.Collections.Generic;
using System.Text;
using Random;

namespace EconomyModel3
{
    public class ManufaturingFactory : Building
    {
        public ManufaturingFactory()
            : base(CommodityCategorizer.ManufacturedGoodsT2[Rnd.Get(CommodityCategorizer.ManufacturedGoodsT2.Count)], 1 + Rnd.Get(10))
        { 
        }
    }
}
