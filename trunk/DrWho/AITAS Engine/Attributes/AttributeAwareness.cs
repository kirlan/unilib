using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Attributes
{
    public class AttributeAwareness: CharAttribute
    {
        public override string GetDescription(int iValue)
        {
            switch (iValue)
            {
                case 0: return "персонаж без сознания";
                case 1: return "чрезвычайная рассеяность, персонаж практически никогда не обращает внимания на важные улики и знаки";
                case 2: return "персонаж часто витает в облаках и легко отвлекается";
                case 3: return "персонаж обращает внимание на окружение и может заметить некоторые скрытые улики, услышать приближающегося противника или учуять слевина в соседней комнате";
                case 4: return "персонаж быстро замечает когда что-то 'не так'";
                case 5: return "персонаж инстинктивно чует любые странности";
                case 6: return "персонаж способен проникать в замыслы и истинные чувства других людей только на основании изменений их голоса и минимальных сокращениях лицевых мышц";
                default: return "персонажа просто невозможно застать врасплох или обмануть";
            }
        }
    }
}
