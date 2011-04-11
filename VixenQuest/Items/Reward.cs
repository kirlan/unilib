using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using nsUniLibXML;
using System.Xml;

namespace VixenQuest
{
    public enum RewardType
    { 
        Clothes,
        Jevelry,
        Toys,
        Trash
    }

    public abstract class Reward: Item
    {
        protected static ValuedString[] m_aQuality = 
        {
            new ValuedString("Ugly ", 1),
            new ValuedString("Worn ", 1),
            new ValuedString("Old ", 1),
            //new ValuedString("Torn ", 1),
            new ValuedString("Dirty ", 1),
            new ValuedString("Cursed ", 1),
            new ValuedString("Cheap ", 2),
            new ValuedString("Usual ", 2),
            new ValuedString("Common ", 2),
            new ValuedString("Casual ", 2),
            new ValuedString("Shining ", 3),
            new ValuedString("Beautiful ", 3),
            new ValuedString("Expensive ", 4),
            new ValuedString("Extravagant ", 4),
            new ValuedString("Ancient ", 5),
            new ValuedString("Pagan ", 5),
            new ValuedString("Invisible ", 6),
            //new ValuedString("Illusive ", 6),
            new ValuedString("Blessed ", 7),
            new ValuedString("Magic ", 7),
            new ValuedString("Enchanted ", 7),
            new ValuedString("Enslaving ", 8),
            new ValuedString("Sacred ", 8),
            //new ValuedString("Posessed ", 8),
            new ValuedString("Demonic ", 9),
            new ValuedString("Divine ", 9),
        };

        public RewardType m_eType = RewardType.Trash;

        public static Reward MakeReward()
        {
            if (Rnd.OneChanceFrom(2))
            {
                return new RewardClothes();
            }
            else
            {
                return new RewardJevelry();
            }
        //    int typeId = Rnd.Get(Enum.GetValues(typeof(RewardType)).Length);
        //    m_eType = (RewardType)Enum.GetValues(typeof(RewardType)).GetValue(typeId);

        //    switch (m_eType)
        //    {
        //        case RewardType.Clothes:
        //            MakeClothes();
        //            break;
        //        case RewardType.Jevelry:
        //            MakeJevelry();
        //            break;
        //        case RewardType.Toys:
        //            MakeToy();
        //            break;
        //        default:
        //            MakeTrash();
        //            break;
        //    }
        }

        protected bool m_bRecognized = false;

        public abstract void Recognize();

        public static Reward MakeReward(Person pTarget)
        {
            int iCounter = 0;

            Reward pBestReward = null;
            int iBestDifference = int.MaxValue;

            do
            {
                Reward pReward = MakeReward();

                int iDifference = Math.Abs(pTarget.Level - pReward.m_iPrice / 10);

                //if (pTarget.Level > pReward.m_iPrice / 10)
                //    //iDifference = int.MaxValue;
                //    iDifference = iDifference * iDifference * iDifference;

                if (pBestReward == null || iBestDifference > iDifference)
                {
                    pBestReward = pReward;
                    iBestDifference = iDifference;
                }

                if (Rnd.Gauss(iDifference, 3))
                    return pReward;

                if (iCounter++ > 500)
                    return pBestReward;
            }
            while (true);
        }

        internal virtual void Write2XML(UniLibXML pXml, XmlNode pRewardNode)
        {
            base.Write2XML(pXml, pRewardNode);

            pXml.AddAttribute(pRewardNode, "recognized", m_bRecognized);
            pXml.AddAttribute(pRewardNode, "type", m_eType);
        }
    }
}
