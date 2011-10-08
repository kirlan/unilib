using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Attributes
{
    public class AttributeResolve: CharAttribute
    {
        public override string GetDescription(int iValue)
        {
            switch (iValue)
            {
                case 0: return "персонаж без сознания";
                case 1: return "слабовольный, легко поддающийся уговорам и быстро впадающий в страх персонаж";
                case 2: return "нарушит новогоднюю клятву ещё до середины января";
                case 3: return "персонаж обладает силой воли и способен контролировать себя";
                case 4: return "сила воли выше среднего";
                case 5: return "необычайно решительный и целеустремлённый персонаж";
                case 6: return "один из самых непоколебимых людей на Земле";
                default: return "один из самых непоколебимых сверхлюдей и пришельцев";
            }
        }
    }
}
