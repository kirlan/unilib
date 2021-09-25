using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using nsUniLibXML;
using System.Xml;
using VixenQuest.People;

namespace VixenQuest
{
    public class RewardJevelry
        : Reward
    {
        private static ValuedString[] m_aJevelryMaterial1 = 
        {
            new ValuedString("", 0),
            new ValuedString("Copper ", 1),
            new ValuedString("Brass ", 5),
            new ValuedString("Bronze ", 5),
            new ValuedString("Iron ", 10),
            new ValuedString("Steel ", 10),
            new ValuedString("Electrum ", 15),
            new ValuedString("Silver ", 15),
            new ValuedString("Crystal ", 20),
            new ValuedString("Golden ", 25),
            new ValuedString("Platinum ", 30),
            //new ValuedString("bone ", 50),
            new ValuedString("Mithril ", 35),
            new ValuedString("Adamantine ", 40),
        };

        private static ValuedString[] m_aJevelryMaterial2 = 
        {
            new ValuedString("", 0),
            //new ValuedString("Glass ", 1),
            new ValuedString("Amber ", 1),
            new ValuedString("Malachite ", 1),
            new ValuedString("Agate ", 5),
            new ValuedString("Jade ", 5),
            new ValuedString("Aquamarine ", 15),
            new ValuedString("Obsidian ", 15),
            new ValuedString("Onyx ", 20),
            new ValuedString("Toutmaline ", 20),
            new ValuedString("Peridot ", 20),
            new ValuedString("Topaz ", 25),
            new ValuedString("Amethist ", 25),
            new ValuedString("Pearl ", 25),
            new ValuedString("Opal ", 30),
            new ValuedString("Emerald ", 30),
            new ValuedString("Ruby ", 35),
            new ValuedString("Saphire ", 35),
            new ValuedString("Diamond ", 40),
        };

        private static JevelryInfo[] m_aJevelryName = 
        {
            new JevelryInfo(JevelryType.Finger, "ring", "rings", 1),
            new JevelryInfo(JevelryType.Finger, "signet", "signets", 2),
            
            new JevelryInfo(JevelryType.Neck, "beads", 1),
            new JevelryInfo(JevelryType.Neck, "trinket", "trinkets", 2),
            new JevelryInfo(JevelryType.Neck, "necklace", "necklaces", 3),
            new JevelryInfo(JevelryType.Neck, "talisman", "talismans", 4),
            new JevelryInfo(JevelryType.Neck, "pendant", "pendants", 5),
            new JevelryInfo(JevelryType.Neck, "amulet", "amulets", 6),
            
            new JevelryInfo(JevelryType.Bracelet, "cuff", "cuffs", 1),
            new JevelryInfo(JevelryType.Bracelet, "fandangle", "fandangles", 2),
            new JevelryInfo(JevelryType.Bracelet, "bracelet", "bracelets", 3),
            new JevelryInfo(JevelryType.Bracelet, "bangle", "bangles", 4),
            new JevelryInfo(JevelryType.Bracelet, "circlet", "circlets", 5),
            
            new JevelryInfo(JevelryType.Pierceing, "ring", "rings", 1),
            new JevelryInfo(JevelryType.Pierceing, "clip", "clips", 2),
            new JevelryInfo(JevelryType.Pierceing, "plug", "plugs", 3),
            new JevelryInfo(JevelryType.Pierceing, "barbell", "barbells", 4),
            new JevelryInfo(JevelryType.Pierceing, "dangle", "dangles", 5),
            new JevelryInfo(JevelryType.Pierceing, "screw", "screws", 6),
            new JevelryInfo(JevelryType.Pierceing, "spool", "spools", 7),
            //majic items
            //new JevelryInfo(JevelryBodyPart.Neck, "amulet of fertility", "amulets of fertility", 3),
            //new JevelryInfo(JevelryBodyPart.Neck, "amulet of virginity", "amulets of virginity", 3),
            //new JevelryInfo(JevelryBodyPart.Neck, "amulet of lust", "amulets of lust", 3),
            //new JevelryInfo(JevelryBodyPart.Neck, "amulet of deep fucking", 3),
        };

        public JevelryInfo m_pInfo;

        public RewardJevelry()
        {
            m_iWeight = 1;
            m_eType = RewardType.Jevelry;

            //int part1 = Rnd.Get(m_aQuality.Length);
            //m_sName = m_aQuality[part1].m_sName;
            //m_sNames = m_aQuality[part1].m_sName;
            //m_iPrice = m_aQuality[part1].m_iValue;

            int part2 = Rnd.Get(m_aJevelryMaterial1.Length);
            m_sName = m_aJevelryMaterial1[part2].m_sName;
            m_sNames = m_aJevelryMaterial1[part2].m_sName;
            m_iPrice = m_aJevelryMaterial1[part2].m_iValue;

            int part3;
            do
            {
                part3 = Rnd.Get(m_aJevelryMaterial2.Length);
            }
            while (m_iPrice + m_aJevelryMaterial2[part3].m_iValue == 0);

            if (m_sName.Length > 0)
            {
                m_sName += m_aJevelryMaterial2[part3].m_sName.ToLower();
                m_sNames += m_aJevelryMaterial2[part3].m_sName.ToLower();
            }
            else
            {
                m_sName += m_aJevelryMaterial2[part3].m_sName;
                m_sNames += m_aJevelryMaterial2[part3].m_sName;
            }
            m_iPrice += m_aJevelryMaterial2[part3].m_iValue;

            int part4 = Rnd.Get(m_aJevelryName.Length);
            m_sName += m_aJevelryName[part4].m_sName;
            m_sNames += m_aJevelryName[part4].m_sNames;
            m_iPrice += m_aJevelryName[part4].m_iRank;

            m_pInfo = m_aJevelryName[part4];

            m_iPrice *= 10;
            m_iPrice += Rnd.Get(10);
        }

        public Stat m_eAffectedStat;

        public int m_iBonus;

        public override void Recognize()
        {
            if (m_bRecognized)
                return;

            m_iBonus = 1 + Rnd.Get(m_iPrice / 30);

            //if (m_iBonus > 50)
            //{
            //    m_iBonus = (m_iBonus / 10) * 10;
            //}
            //if (m_iBonus > 10)
            //{
            //    m_iBonus = (m_iBonus / 5) * 5;
            //}

            bool bWrong;
            do
            {
                bWrong = false;
                int statId = Rnd.Get(Enum.GetValues(typeof(Stat)).Length);
                m_eAffectedStat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(statId);

                switch (m_eAffectedStat)
                {
                    //case Stat.Presence:
                    //    m_sName = "+" + m_iBonus.ToString() + " Hypnotic " + m_sName.ToLower();
                    //    m_sNames = "+" + m_iBonus.ToString() + " Hypnotic " + m_sNames.ToLower();
                    //    break;
                    case Stat.Potency:
                        m_sName = "+" + m_iBonus.ToString() + " Magic " + m_sName.ToLower();
                        m_sNames = "+" + m_iBonus.ToString() + " Magic " + m_sNames.ToLower();
                        break;
                    case Stat.Luck:
                        m_sName = "+" + m_iBonus.ToString() + " Lucky " + m_sName.ToLower();
                        m_sNames = "+" + m_iBonus.ToString() + " Lucky " + m_sNames.ToLower();
                        break;
                    default:
                        bWrong = true;
                        break;
                }
            }
            while (bWrong);
            //switch (m_eAffectedStat)
            //{
            //    case Stat.Force:
            //        m_sName += " of +" + m_iBonus.ToString() + " Force";
            //        m_sNames += " of +" + m_iBonus.ToString() + " Force";
            //        break;
            //    case Stat.Beauty:
            //        m_sName += " of +" + m_iBonus.ToString() + " Beauty";
            //        m_sNames += " of +" + m_iBonus.ToString() + " Beauty";
            //        break;
            //    case Stat.Luck:
            //        m_sName += " of +" + m_iBonus.ToString() + " Luck";
            //        m_sNames += " of +" + m_iBonus.ToString() + " Luck";
            //        break;
            //}

            m_bRecognized = true;
        }
    
        internal virtual void Write2XML(UniLibXML pXml, XmlNode pRewardNode)
        {
            base.Write2XML(pXml, pRewardNode);

            pXml.AddAttribute(pRewardNode, "bonusStat", m_eAffectedStat);
            pXml.AddAttribute(pRewardNode, "bonus", m_iBonus);
            pXml.AddAttribute(pRewardNode, "subtype", m_pInfo.m_sName);
        }
    }
}
