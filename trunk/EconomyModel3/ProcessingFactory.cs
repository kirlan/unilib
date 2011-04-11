using System;
using System.Collections.Generic;
using System.Text;
using Random;

namespace EconomyModel3
{
    public class ProcessingFactory : Building
    {
        public ProcessingFactory()
            : base(CommodityCategorizer.ManufacturedGoodsT1[Rnd.Get(CommodityCategorizer.ManufacturedGoodsT1.Count)], 1 + Rnd.Get(10))
        { 
        }
    }
}
