using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Attributes
{
    public class AttributeStrength: CharAttribute
    {
        public override string GetDescription(int iValue)
        {
            switch (iValue)
            {
                case 0: return "персонаж без сознания";
                case 1: return "дохляк";
                case 2: return "слабее обычного человека";
                case 3: return "персонаж способен постоять за себя в драке, вытащить застрявшую пробку и пронести другого человека на значительную дистанцию";
                case 4: return "сильнее обычного человека";
                case 5: return "профессиональный атлет";
                case 6: return "персонаж способен поднять другого человека над головой и отбросить в сторону - профессиональный тяжелоатлет или бодибилдер";
                default: return "сверхчеловек или пришелец, такой как слевин или вервольф";
            }
        }
    }
}
