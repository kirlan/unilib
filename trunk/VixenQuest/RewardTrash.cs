using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace VixenQuest
{
    class RewardTrash
        : Reward
    {
        private static ValuedString[] m_aTrashName = 
        {
            new ValuedString("slave collar", "slave collars", 1),
            new ValuedString("hat", "hats", 1),
            new ValuedString("rope", "ropes", 1),
            new ValuedString("bottle of wine", "bottles of wine", 2),
            new ValuedString("cup of wine", "cups of wine", 2),
            new ValuedString("seductive perfume", "seductive perfumes", 3),
            new ValuedString("aphrodizack", "aphrodizacks", 3),
            new ValuedString("bottle of lubricant", "bottles of lubricant", 3),
            //high-quality jevelry
            new ValuedString("hairpin in form of naked lady", "hairpins in form of naked lady", 4),
            new ValuedString("hairpin in form of a cock", "hairpins in form of a cock", 4),
            new ValuedString("hairpin in form of fucking pair", "hairpins in form of fucking pair", 4),
            new ValuedString("hairpin in form of naked boy", "hairpins in form of naked boy", 4),
            new ValuedString("tiara with a pattern of fucking pairs", "tiaras with a pattern of fucking pairs", 5),
            new ValuedString("tiara with a pattern of dancing sluts", "tiaras with a pattern of dancing sluts", 5),
            new ValuedString("tiara with a pattern of cocks", "tiaras with a pattern of cocks", 5),
            //ultra high-quality jevelry
            new ValuedString("crown of cocks", "crowns of cocks", 6),
            new ValuedString("crown of naked ladies", "crowns of naked ladies", 6),
            new ValuedString("crown of fucking pairs", "crowns of fucking pairs", 6),
        };

        public RewardTrash(string sName)
        {
            m_sName = sName;
            m_sNames = sName;
            m_iPrice = 0;
            m_iWeight = 0;

            m_eType = RewardType.Trash;
        }

        public RewardTrash()
        {
            m_iWeight = 1;
            m_eType = RewardType.Trash;

            int part1 = Rnd.Get(m_aQuality.Length);
            m_sName = m_aQuality[part1].m_sName;
            m_sNames = m_aQuality[part1].m_sName;
            m_iPrice = m_aQuality[part1].m_iValue;

            int part2 = Rnd.Get(m_aTrashName.Length);
            m_sName += m_aTrashName[part2].m_sName;
            m_sNames += m_aTrashName[part2].m_sNames;
            m_iPrice += m_aTrashName[part2].m_iValue;

            //m_pInfo = m_aTrashName[part2];

            m_iPrice *= 10;
        }

        public override void Recognize()
        {
        }
    }
}
