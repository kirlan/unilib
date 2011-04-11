using System;
using System.Collections.Generic;
using System.Text;
using Random;

namespace EconomyModel3
{
    public class Settlement : Building
    {
        public Settlement()
            : base(Commodity.Workers, 1 + Rnd.Get(10))
        { 
        }
    }
}
