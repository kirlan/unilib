using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Attributes
{
    public class AttributeIngenuity: CharAttribute
    {
        public override string GetDescription(int iValue)
        {
            switch (iValue)
            {
                case 0: return "персонаж без сознания";
                case 1: return "тупица. не обязательно идиот, но соображает с большой задержкой";
                case 2: return "с трудом подбирает аргументы в споре";
                case 3: return "персонаж способен разобраться в инструкции к бытовой технике или несложном юридическом документе";
                case 4: return "человек с высшим образованием, способный к абстрактному мышлению";
                case 5: return "быстро соображает и хорошо разбирается в логике";
                case 6: return "один из лучших умов на Земле";
                default: return "сверхчеловек или пришелец, обладатель выдающего разума (как Доктор)";
            }
        }
    }
}
