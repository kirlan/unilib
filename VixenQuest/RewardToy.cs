using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace VixenQuest
{
    class RewardToy
        : Reward
    {
        private static ValuedString[] m_aToysQuality = 
        {
            new ValuedString("Ugly ", 1),
            new ValuedString("Cheap ", 1),
            new ValuedString("Usual ", 1),
            new ValuedString("Used ", 1),
            new ValuedString("Old ", 1),
            new ValuedString("Destroed ", 1),
            new ValuedString("Casual ", 1),
            new ValuedString("Dirty ", 1),
            new ValuedString("Cursed ", 2),
            new ValuedString("Precious ", 2),
            new ValuedString("Extravagant ", 2),
            new ValuedString("Shining ", 2),
            new ValuedString("Beautiful ", 2),
            new ValuedString("Transparent ", 2),
            new ValuedString("Semi-transparent ", 2),
            new ValuedString("Invisible ", 2),
            new ValuedString("Spiked ", 2),
            new ValuedString("Pagan ", 2),
            new ValuedString("Leather ", 3),
            new ValuedString("Metal ", 3),
            new ValuedString("Incrusted ", 3),
            new ValuedString("Ancient ", 3),
            new ValuedString("Blessed ", 4),
            new ValuedString("Magic ", 4),
            //new ValuedString("Interdimensional ", 3),
            //new ValuedString("Multidimensional ", 3),
            new ValuedString("Enchanted ", 4),
            new ValuedString("Enslaving ", 4),
            new ValuedString("Sacred ", 4),
            new ValuedString("Posessed ", 4),
            new ValuedString("Demonic ", 5),
            new ValuedString("Divine ", 5),
        };

        private static ValuedString[] m_aToysName = 
        {
            //common toys
            new ValuedString("strapon", "strapons", 1),
            new ValuedString("dildo", "dildos", 1),
            new ValuedString("vaginal balls", 1),
            new ValuedString("anal beads", 1),
            //advanced toys
            new ValuedString("collar", "collars", 2),
            new ValuedString("double dildo", "double dildos", 2),
            new ValuedString("double dong", "double dongs", 2),
            new ValuedString("blindfold", "blindfolds", 2),
            new ValuedString("blindfold mask", "blindfold masks", 2),
            new ValuedString("fluffy tickler", "fluffy tickler", 2),
            new ValuedString("feather tickler", "feather tickler", 2),
            new ValuedString("ball gag", "ball gags", 2),
            new ValuedString("whip", "whips", 2),
            new ValuedString("slapper", "slappers", 2),
            new ValuedString("flogger", "floggers", 2),
            new ValuedString("leash", "leashes", 2),
            new ValuedString("bondage strap", "bondage straps", 2),
            new ValuedString("nipple clamps", 2),
            new ValuedString("pin wheel", "pin wheels", 2),
            new ValuedString("clit clamp", "clit clamps", 2),
            new ValuedString("cock ring", "cock rings", 2),
            new ValuedString("cock circlet", "cock circlets", 2),
            new ValuedString("speculum", "speculums", 2),
            new ValuedString("bondage bracelet", "bondage bracelets", 2),
            new ValuedString("bondage tape", "bondage tapes", 2),
            new ValuedString("bondage rope", "bondage ropes", 2),
            new ValuedString("anal plug", "anal plugs", 2),
            new ValuedString("breast pump", "breast pumps", 2),
            new ValuedString("handcuffs", 2),
            //exotic toys            
            new ValuedString("ring gag", "ring gags", 3),
            new ValuedString("bit gag", "bit gags", 3),
            new ValuedString("mouth gag", "mouth gags", 3),
            new ValuedString("pump gag", "pump gags", 3),
            new ValuedString("cat-o-nine tails whip", "cat-o-nine tails whips", 3),
            new ValuedString("bondage hood", "bondage hoods", 3),
            new ValuedString("hogtie", "hogties", 3),
            new ValuedString("posture bar", "posture bars", 3),
            new ValuedString("posture collar", "posture collars", 3),
            new ValuedString("nipple squeezer", "nipple squeezers", 3),
            new ValuedString("nipple squasher", "nipple squashers", 3),
            new ValuedString("ass spreader", "ass spreaders", 3),
            new ValuedString("anal retractor", "anal retractors", 3),
            new ValuedString("urethra probe", "urethra probes", 3),
            new ValuedString("rectal speculum", "rectal speculums", 3),
            new ValuedString("ball fetters", 3),
            new ValuedString("pecker leash", "pecker leashes", 3),
            new ValuedString("cock lariat", "cock lariats", 3),
            new ValuedString("arm binder", "arm binders", 3),
            new ValuedString("thumbcuffs", 3),
            new ValuedString("ankle cuffs", 3),
            new ValuedString("yoke", "yokes", 3),
            new ValuedString("legs spreader bar", "legs spreader bars", 3),
        };

        public RewardToy()
        {
            m_iWeight = 1;
            m_eType = RewardType.Toys;

            int part1 = Rnd.Get(m_aToysQuality.Length);
            m_sName = m_aToysQuality[part1].m_sName;
            m_sNames = m_aToysQuality[part1].m_sName;
            m_iPrice = m_aToysQuality[part1].m_iValue;

            int part2 = Rnd.Get(m_aToysName.Length);
            m_sName += m_aToysName[part2].m_sName;
            m_sNames += m_aToysName[part2].m_sNames;
            m_iPrice += m_aToysName[part2].m_iValue;

            //m_pInfo = m_aToysName[part2];

            m_iPrice *= 10;
            m_iPrice += Rnd.Get(10);
        }

        public override void Recognize()
        {
        }
    }
}
