using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Attributes
{
    public class AttributeCoordination: CharAttribute
    {
        public override string GetDescription(int iValue)
        {
            switch (iValue)
            {
                case 0: return "персонаж без сознания";
                case 1: return "чрезвычайная неуклюжесть, всё роняет, не попадёт в мишень и с двух шагов";
                case 2: return "отвратительный стрелок, но в остальном не хуже многих других";
                case 3: return "обычный человек";
                case 4: return "талантливый баскетболист или опытный хирург";
                case 5: return "замечательная координация и рефлексы, как у пилота или гонщика";
                case 6: return "самый ловкий и быстрый человек на Земле";
                default: return "за пределами человеческих возможностей";
            }
        }
    }
}
