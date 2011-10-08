using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Attributes
{
    public class AttributePresence: CharAttribute
    {
        public override string GetDescription(int iValue)
        {
            switch (iValue)
            {
                case 0: return "персонаж без сознания";
                case 1: return "чрезвычайно грубая и антисоциальная личность";
                case 2: return "персонаж часто пренебрегает правилами вежливости, эгоистичен или наоборот - излишне подбострастен";
                case 3: return "легко сходится с окружающими";
                case 4: return "персонаж обладает шармом и определёнными лидерскими качествами";
                case 5: return "персонаж способен разрешить практически любой конфликт - не шармом так авторитетом";
                case 6: return "божество во плоти";
                default: return "сверхчеловек или пришелец, способный подчинять себе волю других";
            }
        }
    }
}
