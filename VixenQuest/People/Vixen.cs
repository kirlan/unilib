using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using nsUniLibXML;
using System.Xml;
using VixenQuest.World;
using VixenQuest.Story;

namespace VixenQuest.People
{
    public enum VixenRace
    { 
        Human,
        Elf,
        Orc,
        Troll,
        HellDemon,
        Vampire,
        Faery,
        Werewolf,
        Angel,
        Dwarf,
        TentacleDemon,
        CatPeople,
        Zombie
    }

    public struct VixenClass
    {
        public string m_sNameM;
        public string m_sNameF;

        private Dictionary<Stat, int> m_cStatsBonuses;

        public Dictionary<Stat, int> StatsBonuses
        {
            get { return m_cStatsBonuses; }
        }

        public VixenClass(string sNameM, string sNameF, int iForce, int iBeauty, int iLuck, int iEndurance)
        {
            m_sNameM = sNameM;
            m_sNameF = sNameF;

            m_cStatsBonuses = new Dictionary<Stat, int>();
            m_cStatsBonuses[Stat.Force] = iForce*3;
            m_cStatsBonuses[Stat.Beauty] = iBeauty*3;
            m_cStatsBonuses[Stat.Luck] = iLuck*3;
            m_cStatsBonuses[Stat.Potency] = iEndurance*3;
        }
    }

    public enum JevelryBodyPart
    {
        Neck,
        Ears,
        Finger,
        Wrist,
        Ankle,
        Nipples,
        Belly,
        ClitCock
    }

    public class Vixen: Person
    {
        public static VixenClass[] VixenClasses = 
        { 
            new VixenClass("Dominator", "Dominatrix", 4, 1, 1, 1),  //force++
            new VixenClass("Stripper", "Stripper", 1, 4, 1, 1),  //beauty++
            new VixenClass("Priest of Love", "Priestess of Love", 1, 1, 4, 1),  //luck++
            new VixenClass("Pleasure Slave", "Sex Doll", 1, 1, 1, 4),  //endurance++
            new VixenClass("Playboy", "Playgirl", 3, 3, 1, 1),  //beauty+ force+
            new VixenClass("Bitch Hunter", "Bitch Huntress", 3, 1, 3, 1),  //force+ luck+
            new VixenClass("Battle Raper", "Cat-Fighter", 3, 1, 1, 3),  //force+ endurance+
            new VixenClass("Gigolo", "Courtesan", 1, 3, 3, 1),  //beauty+ luck+
            new VixenClass("Tantra Master", "Tantra Mistress", 1, 3, 1, 3),  //beauty+ endurance+
            new VixenClass("Temple Deflorator", "Temple Wench", 1, 1, 3, 3),  //luck+ endurance+
            new VixenClass("Sex Tutor", "Sex Tutoress", 1, 2, 2, 2),  //beauty+ luck+ endurance+
            new VixenClass("Slave Trainer", "Slave Trainer", 2, 1, 2, 2),  //force+ luck+ endurance+
            new VixenClass("Porno Actor", "Porno Actress", 2, 2, 1, 2),  //force+ beauty+ endurance+
            new VixenClass("Wandering Prince", "Wandering Princess", 2, 2, 2, 1),  //force+ beauty+ luck+
        };

        private string m_sName = "Vixen";

        public string Name
        {
            get { return m_sName; }
        }

        private VixenRace m_eRace = VixenRace.Elf;

        public VixenRace Race
        {
            get { return m_eRace; }
        }

        public string RaceString
        {
            get 
            {
                switch (m_eRace)
                {
                    case VixenRace.Angel:
                        return "Angel";
                    case VixenRace.CatPeople:
                        if (HaveCunt)
                            return "Catwoman";
                        else
                            return "Catman";
                    case VixenRace.Dwarf:
                        return "Dwarf";
                    case VixenRace.Elf:
                        return "Elf";
                    case VixenRace.Faery:
                        return "Faery";
                    case VixenRace.HellDemon:
                        if (HaveCunt)
                            return "Horned Demoness";
                        else
                            return "Horned Demon";
                    case VixenRace.Human:
                        return "Human";
                    case VixenRace.Orc:
                        return "Orc";
                    case VixenRace.TentacleDemon:
                        if (HaveCunt)
                            return "Tentacled Demoness";
                        else
                            return "Tentacled Demon";
                    case VixenRace.Troll:
                        return "Troll";
                    case VixenRace.Vampire:
                        return "Vampire";
                    case VixenRace.Werewolf:
                        return "Werewolf";
                    case VixenRace.Zombie:
                        return "Zombie";
                    default:
                        return "Unknown";
                }
            }
        }

        private VixenClass m_pClass;

        public VixenClass Class
        {
            get { return m_pClass; }
        }

        public int Experience2LevelUp
        {
            get 
            {
                int value = m_iLevel * m_iLevel * 10;
                if (value < 0)
                    value = int.MaxValue;
                return value; 
            }
        }
        public int m_iExperience = 0;

        private Dictionary<Item, int> m_cLoot = new Dictionary<Item, int>();

        public Dictionary<Item, int> Loot
        {
            get { return m_cLoot; }
        }

        protected Dictionary<ClothesBodyPart, RewardClothes> m_cClothes = new Dictionary<ClothesBodyPart, RewardClothes>();

        public Dictionary<ClothesBodyPart, RewardClothes> Clothes
        {
            get { return m_cClothes; }
        }

        protected Dictionary<JevelryBodyPart, RewardJevelry> m_cJevelry = new Dictionary<JevelryBodyPart, RewardJevelry>();

        public Dictionary<JevelryBodyPart, RewardJevelry> Jevelry
        {
            get { return m_cJevelry; }
        }

        private static Reward m_pGold = new RewardTrash("Gold");

        public int m_iEncumbrance = 0;

        public int MaxEncumbrance
        {
            get
            {
                return 25;// m_cStats[Stat.Luck] * 2;
            }
        }

        protected Dictionary<Stat, int> m_cStatsBonuses = new Dictionary<Stat, int>();

        public Dictionary<Stat, int> StatsBonuses
        {
            get { return m_cStatsBonuses; }
        }

        protected Dictionary<Stat, int> m_cEffectiveStats = new Dictionary<Stat, int>();

        public Dictionary<Stat, int> EffectiveStats
        {
            get { return m_cEffectiveStats; }
        }

        private int GetStatBonus(Stat eStat)
        {
            int iClothesBonus = 0;
            foreach (RewardClothes pClothes in m_cClothes.Values)
            {
                if (pClothes.m_eAffectedStat == eStat)
                    iClothesBonus += pClothes.m_iBonus;
            }

            int iJevelryBonus = 0;
            foreach (RewardJevelry pJevelry in m_cJevelry.Values)
            {
                if (pJevelry.m_eAffectedStat == eStat)
                    iJevelryBonus += pJevelry.m_iBonus;
            }

            return iJevelryBonus + iClothesBonus;
        }

        private void RecalcStatsBonuses()
        {
            foreach (Stat eStat in Enum.GetValues(typeof(Stat)))
            {
                m_cStatsBonuses[eStat] = GetStatBonus(eStat);
                m_cEffectiveStats[eStat] = m_cStats[eStat] + m_cStatsBonuses[eStat];
            }
        }

        public int EffectiveLevel
        {
            get
            {
                //int iResult = int.MaxValue;
                int iResult = 0;
                foreach (Stat eStat in m_cEffectiveStats.Keys)
                {
                    //if (m_cEffectiveStats[eStat] < iResult)
                    //    iResult = m_cEffectiveStats[eStat];
                    iResult += m_cEffectiveStats[eStat];
                }
                return iResult/(3*m_cEffectiveStats.Count);
            }
        }

        public int m_iTiredness = 0;

        public Vixen()
        {
            int raceId = Rnd.Get(Enum.GetValues(typeof(VixenRace)).Length);
            m_eRace = (VixenRace)Enum.GetValues(typeof(VixenRace)).GetValue(raceId);

            int classId = Rnd.Get(VixenClasses.Length);
            m_pClass = VixenClasses[classId];

            int genderId = Rnd.Get(Enum.GetValues(typeof(Gender)).Length);
            m_eGender = (Gender)Enum.GetValues(typeof(Gender)).GetValue(genderId);

            int orientationId = Rnd.Get(Enum.GetValues(typeof(Orientation)).Length);
            m_eOrientation = (Orientation)Enum.GetValues(typeof(Orientation)).GetValue(orientationId);

            foreach (Stat eStat in Enum.GetValues(typeof(Stat)))
                m_cStats[eStat] = 5 + Rnd.Get(m_pClass.StatsBonuses[eStat] + 1);

            foreach (VixenSkill eSkill in Enum.GetValues(typeof(VixenSkill)))
                m_cSkills[eSkill] = 2 + Rnd.Get(5);

            RecalcStatsBonuses();

            AddLoot(m_pGold, 1+Rnd.Get(10));
            //BuyNewEquipment(100);
        }

        public bool m_bEncumbranced = false;

        private void RecalcEncumbrance()
        {
            m_iEncumbrance = 0;
            m_bEncumbranced = false;
            foreach (Item pLoot in m_cLoot.Keys)
            {
                if (m_cLoot[pLoot] <= 0)
                    continue;

                m_iEncumbrance += m_cLoot[pLoot] * pLoot.m_iWeight;
            }

            if(m_iEncumbrance >= MaxEncumbrance)
            {
                m_iEncumbrance = MaxEncumbrance;
                m_bEncumbranced = true;
            }
        }

        private bool Try2EquipClothes(RewardClothes pReward, int count)
        {
            ClothesBodyPart eBodyPart = pReward.m_pInfo.m_eBodyPart;
            ClothesGender eGender = pReward.m_pInfo.m_eGender;

            if (!HaveCunt && eGender == ClothesGender.Female)
                return false;
            if (HaveCunt && eGender == ClothesGender.Male)
                return false;

            if (m_cClothes.ContainsKey(eBodyPart) &&
                m_cClothes[eBodyPart].m_iBonus < pReward.m_iBonus)
            {
                Reward pOld = m_cClothes[eBodyPart];
                m_cClothes[eBodyPart] = pReward;
                AddLoot(pOld);
                if (count > 1)
                    AddLoot(pReward, count - 1);
                RecalcStatsBonuses();
                return true;
            }
            if (!m_cClothes.ContainsKey(eBodyPart))
            {
                m_cClothes[eBodyPart] = pReward;
                if (count > 1)
                    AddLoot(pReward, count - 1);
                RecalcStatsBonuses();
                return true;
            }

            return false;
        }

        private bool Try2EquipJevelry(RewardJevelry pReward, JevelryBodyPart eBodyPart, int count)
        {
            if (m_cJevelry.ContainsKey(eBodyPart) &&
                m_cJevelry[eBodyPart].m_iBonus < pReward.m_iBonus)
            {
                Reward pOld = m_cJevelry[eBodyPart];
                m_cJevelry[eBodyPart] = pReward;
                AddLoot(pOld);
                if (count > 1)
                    AddLoot(pReward, count - 1);
                RecalcStatsBonuses();
                return true;
            }
            if (!m_cJevelry.ContainsKey(eBodyPart))
            {
                m_cJevelry[eBodyPart] = pReward;
                if (count > 1)
                    AddLoot(pReward, count - 1);
                RecalcStatsBonuses();
                return true;
            }

            return false;
        }

        public void AddLoot(Item pNewLoot)
        {
            AddLoot(pNewLoot, 1);
        }

        public void AddLoot(Item pNewLoot, int count)
        {
            if(pNewLoot is Reward)
            {
                Reward pReward = pNewLoot as Reward;

                pReward.Recognize();

                if(pReward.m_eType == RewardType.Clothes)
                {
                    if(Try2EquipClothes(pReward as RewardClothes, count))
                        return;
                }
                if (pReward.m_eType == RewardType.Jevelry)
                {
                    RewardJevelry pJevelry = pReward as RewardJevelry;
                    JevelryType eJevelryType = pJevelry.m_pInfo.m_eBodyPart;
                    switch (eJevelryType)
                    {
                        case JevelryType.Neck:
                            if (Try2EquipJevelry(pJevelry, JevelryBodyPart.Neck, count))
                                return;
                            break;
                        case JevelryType.Finger:
                            if (Try2EquipJevelry(pJevelry, JevelryBodyPart.Finger, count))
                                return;
                            break;
                        case JevelryType.Bracelet:
                            if (Try2EquipJevelry(pJevelry, JevelryBodyPart.Wrist, count))
                                return;
                            if (Try2EquipJevelry(pJevelry, JevelryBodyPart.Ankle, count))
                                return;
                            break;
                        case JevelryType.Pierceing:
                            if (Try2EquipJevelry(pJevelry, JevelryBodyPart.Ears, count))
                                return;
                            if (Try2EquipJevelry(pJevelry, JevelryBodyPart.Nipples, count))
                                return;
                            if (Try2EquipJevelry(pJevelry, JevelryBodyPart.ClitCock, count))
                                return;
                            if (Try2EquipJevelry(pJevelry, JevelryBodyPart.Belly, count))
                                return;
                            break;
                    }
                }
            }

            foreach (Item pLoot in m_cLoot.Keys)
            {
                if (pLoot.m_sName == pNewLoot.m_sName)
                {
                    m_cLoot[pLoot] += count;
                    RecalcEncumbrance();
                    return;
                }
            }

            m_cLoot[pNewLoot] = count;

            RecalcEncumbrance();
        }

        private void UseLoot()
        {
            Dictionary<Item, int> pPile = new Dictionary<Item, int>();

            foreach (Item pLoot in m_cLoot.Keys)
            {
                pPile[pLoot] = m_cLoot[pLoot];
            }
            m_cLoot.Clear();

            foreach (Item pLoot in pPile.Keys)
            {
                AddLoot(pLoot, pPile[pLoot]);
            }
        }

        private void SellLoot(Item pSellingLoot)
        {
            Item pRemove = null;
            foreach (Item pLoot in m_cLoot.Keys)
            {
                if (pLoot.m_sName == pSellingLoot.m_sName)
                {
                    AddLoot(m_pGold, pSellingLoot.m_iPrice * m_cLoot[pLoot]);

                    m_cLoot[pLoot] = 0;
                    pRemove = pLoot;
                    break;
                }
            }
            if (pRemove != null)
                m_cLoot.Remove(pRemove);

            RecalcEncumbrance();
        }

        private bool IsBetter(RewardClothes pNewClothes)
        {
            ClothesBodyPart eBodyPart = pNewClothes.m_pInfo.m_eBodyPart;
            ClothesGender eGender = pNewClothes.m_pInfo.m_eGender;

            if (!HaveCunt && eGender == ClothesGender.Female)
                return false;
            if (HaveCunt && eGender == ClothesGender.Male)
                return false;

            if (!m_cClothes.ContainsKey(eBodyPart))
                return true;

            if (m_cClothes[eBodyPart].m_iBonus < pNewClothes.m_iBonus)
                return true;

            return false;
        }

        private bool IsBetter(RewardJevelry pJevelry, JevelryBodyPart eBodyPart)
        {
            if (!m_cJevelry.ContainsKey(eBodyPart))
                return true;

            if (m_cJevelry[eBodyPart].m_iBonus < pJevelry.m_iBonus)
                return true;

            return false;
        }

        private bool IsBetter(RewardJevelry pJevelry)
        {
            JevelryType eJevelryType = pJevelry.m_pInfo.m_eBodyPart;
            switch (eJevelryType)
            {
                case JevelryType.Neck:
                    return IsBetter(pJevelry, JevelryBodyPart.Neck);
                case JevelryType.Finger:
                    return IsBetter(pJevelry, JevelryBodyPart.Finger);
                case JevelryType.Bracelet:
                    return IsBetter(pJevelry, JevelryBodyPart.Wrist) ||
                           IsBetter(pJevelry, JevelryBodyPart.Ankle);
                case JevelryType.Pierceing:
                    return IsBetter(pJevelry, JevelryBodyPart.Ears) ||
                           IsBetter(pJevelry, JevelryBodyPart.Nipples) ||
                           IsBetter(pJevelry, JevelryBodyPart.Belly) ||
                           IsBetter(pJevelry, JevelryBodyPart.ClitCock);
            }
            return false;
        }

        private void BuyNewEquipment(int iTries)
        {
            for (int i = 0; i < iTries; i++)
            {
                Reward pNewEquipment;
                bool bIsBetter;
                int counter = 0;
                do
                {
                    pNewEquipment = Reward.MakeReward(this);
                    pNewEquipment.Recognize();
                    bIsBetter = false;
                    switch (pNewEquipment.m_eType)
                    {
                        case RewardType.Clothes:
                            RewardClothes pNewClothes = pNewEquipment as RewardClothes;
                            bIsBetter = IsBetter(pNewClothes);
                            break;
                        case RewardType.Jevelry:
                            RewardJevelry pJevelry = pNewEquipment as RewardJevelry;
                            bIsBetter = IsBetter(pJevelry);
                            break;
                        case RewardType.Toys:
                            break;
                    }
                    counter++;
                }
                while(!bIsBetter && counter < 500);
                if (bIsBetter && pNewEquipment.m_iPrice * 10 <= m_cLoot[m_pGold])
                {
                    m_cLoot[m_pGold] -= pNewEquipment.m_iPrice * 10;
                    AddLoot(pNewEquipment);
                    if (m_cLoot.Count > 1)
                    {
                        List<Item> pToSell = new List<Item>();
                        foreach (Item pItem in m_cLoot.Keys)
                        {
                            if (pItem.m_iWeight > 0 && m_cLoot[pItem] > 0)
                                pToSell.Add(pItem);
                        }
                        foreach (Item pItem in pToSell)
                            SellLoot(pItem);
                    }
                }
            }
        }

        public string m_sLastLoot = "";
        public string m_sLastLoss = "";

        public void Complete(Encounter pEncounter)
        {
            m_sLastLoot = "";
            m_sLastLoss = "";

            m_iExperience += pEncounter.Experience;
            //m_iTiredness += pAction.Tiredness;

            while (m_iExperience >= Experience2LevelUp)
            {
                m_iExperience -= Experience2LevelUp;
                LevelUp();
            }

            if (pEncounter.m_pTarget.m_pRace.m_eSapience != Sapience.Animal &&
                pEncounter.m_cActions.Count > 1 &&
                Rnd.Chances(m_cEffectiveStats[Stat.Luck], m_cEffectiveStats[Stat.Luck] + pEncounter.m_pTarget.Stats[Stat.Luck]))
            {
                AddLoot(pEncounter.m_pReward);
                m_sLastLoot = pEncounter.m_pReward.m_sName;
            }
        }

        public void Lose(Encounter pEncounter)
        {
            m_sLastLoot = "";
            m_sLastLoss = "";

            if ((pEncounter.m_cActions.Count > 1 &&
                Rnd.Chances(m_cEffectiveStats[Stat.Luck], m_cEffectiveStats[Stat.Luck] + pEncounter.m_pTarget.Stats[Stat.Luck])) ||
                Rnd.OneChanceFrom(2))
            {
                m_iExperience += Math.Max(1, pEncounter.Experience/2);
                //m_iTiredness += pAction.Tiredness;

                while (m_iExperience >= Experience2LevelUp)
                {
                    m_iExperience -= Experience2LevelUp;
                    LevelUp();
                }
                return;
            }

            int iLoss = Rnd.Get(m_cClothes.Count + m_cJevelry.Count);

            foreach (ClothesBodyPart bodyPart in m_cClothes.Keys)
            {
                if (iLoss-- <= 0)
                {
                    m_sLastLoss = m_cClothes[bodyPart].m_sName;
                    m_cClothes.Remove(bodyPart);
                    UseLoot();
                    return;
                }
            }
            foreach (JevelryBodyPart bodyPart in m_cJevelry.Keys)
            {
                if (iLoss-- <= 0)
                {
                    m_sLastLoss = m_cJevelry[bodyPart].m_sName;
                    m_cJevelry.Remove(bodyPart);
                    UseLoot();
                    return;
                }
            }
        }

        public string Complete(VQAction pAction)
        {
            if (pAction.m_eType == ActionType.Bargain)
            {
                BuyNewEquipment(10);
                return m_pGold.m_sName;
            }

            if (pAction.m_pTarget != null && pAction.IsSkilled && 
                Rnd.Get(pAction.m_pTarget.Skills[pAction.TargetSkill]) > m_cSkills[pAction.VixenSkill])
            {
                if (!pAction.Passive || Rnd.Chances((int)Math.Pow(pAction.m_pTarget.Skills[pAction.TargetSkill], 2),
                                                    (int)Math.Pow(m_cSkills[pAction.VixenSkill] +
                                                                  pAction.m_pTarget.Skills[pAction.TargetSkill], 2)))
                {
                    m_cSkills[pAction.VixenSkill]++;
                }
            }

            if (pAction.m_pLoot.m_sName.Length > 0)
            {
                if (pAction.m_eType == ActionType.SellLoot)
                {
                    SellLoot(pAction.m_pLoot);
                    return m_pGold.m_sName;
                }
                else
                {
                    int iLootCount = Rnd.Get(pAction.m_pTarget.m_iCount + 1);
                    if(iLootCount == 0)
                        iLootCount = Rnd.Get(pAction.m_pTarget.m_iCount + 1);

                    if (iLootCount > 0)
                    {
                        AddLoot(pAction.m_pLoot, iLootCount);
                        return pAction.m_pLoot.m_sName;
                    }
                }
            }

            return "";
        }

        private void LevelUp()
        {
            m_iLevel++;

            List<int> cChances = new List<int>();

            foreach (Stat eStat in Enum.GetValues(typeof(Stat)))
            {
                cChances.Add(m_pClass.StatsBonuses[eStat]);
            }
            for (int i = 0; i < 3; i++)
            {
                int iChoosen = Rnd.ChooseOne(cChances, 1);

                foreach (Stat eStat in Enum.GetValues(typeof(Stat)))
                {
                    if (iChoosen == 0)
                    {
                        m_cStats[eStat]++;
                        break;
                    }
                    iChoosen--;
                }
            }
            RecalcStatsBonuses();
        }

        public int ActionDifficulty(VQAction pAction)
        {
            if (pAction.m_eType == ActionType.Move)
                return 50;

            if (pAction.m_eType == ActionType.SellLoot)
                return 40;

            if (pAction.m_eType == ActionType.Rest)
                return 50 + Rnd.Get(20);

            //int iRet = Math.Max(30, (int)(200 * Math.Sqrt((float)pAction.m_pTarget.EncounterRank / m_cSkills[pAction.Skill])));

            float diff = 0;

            if (pAction.m_eType == ActionType.Fucking ||
                pAction.m_eType == ActionType.AssFucking ||
                pAction.m_eType == ActionType.Fisting ||
                pAction.m_eType == ActionType.AssFisting ||
                pAction.m_eType == ActionType.OralFucking ||
                pAction.m_eType == ActionType.Sado)
                diff = (float)pAction.m_pTarget.Skills[pAction.TargetSkill] / m_cSkills[pAction.VixenSkill];

            if (pAction.m_eType == ActionType.Seducing)
                diff = (float)pAction.m_pTarget.Stats[Stat.Beauty] / m_cEffectiveStats[Stat.Beauty];

            if (pAction.m_eType == ActionType.Pursue)
                diff = (float)pAction.m_pTarget.Stats[Stat.Force] / m_cEffectiveStats[Stat.Force];

            if (pAction.m_eType == ActionType.Fucked ||
                pAction.m_eType == ActionType.AssFucked ||
                pAction.m_eType == ActionType.Fisted ||
                pAction.m_eType == ActionType.AssFisted ||
                pAction.m_eType == ActionType.OralFucked ||
                pAction.m_eType == ActionType.Maso)
                diff = (float)m_cSkills[pAction.VixenSkill] / pAction.m_pTarget.Skills[pAction.TargetSkill];

            if (pAction.m_eType == ActionType.Seduced)
                diff = (float)m_cEffectiveStats[Stat.Beauty] / pAction.m_pTarget.Stats[Stat.Beauty];

            if (pAction.m_eType == ActionType.Evade)
                diff = (float)m_cEffectiveStats[Stat.Force] / pAction.m_pTarget.Stats[Stat.Force];

            return Math.Max(20, (int)(30 * Math.Sqrt(diff)));
        }

        public override bool WannaFuck(Person pOpponent)
        {
            if (pOpponent is Opponent)
            {
                if ((pOpponent as Opponent).m_pRace.m_eSapience == Sapience.Animal)
                    return false;
            }

            return base.WannaFuck(pOpponent);
        }
    
        public void SaveXML(string sFilename)
        {
            UniLibXML pXml = new UniLibXML("Vixen");

            base.Write2XML(pXml, pXml.Root);

            pXml.AddAttribute(pXml.Root, "name", m_sName);
            pXml.AddAttribute(pXml.Root, "race", m_eRace);
            pXml.AddAttribute(pXml.Root, "xp", m_iExperience);
            pXml.AddAttribute(pXml.Root, "class", m_eGender == Gender.Male ? m_pClass.m_sNameM : m_pClass.m_sNameF);

            XmlNode pClothesNode = pXml.CreateNode(pXml.Root, "Clothes");
            foreach (ClothesBodyPart bodyPart in m_cClothes.Keys)
            {
                XmlNode pBodyPartNode = pXml.CreateNode(pClothesNode, "BodyPart");
                pXml.AddAttribute(pBodyPartNode, "name", bodyPart);
                XmlNode pRewardNode = pXml.CreateNode(pBodyPartNode, "Reward");
                m_cClothes[bodyPart].Write2XML(pXml, pRewardNode);
            }

            XmlNode pJevelryNode = pXml.CreateNode(pXml.Root, "Jevelry");
            foreach (JevelryBodyPart bodyPart in m_cJevelry.Keys)
            {
                XmlNode pBodyPartNode = pXml.CreateNode(pJevelryNode, "BodyPart");
                pXml.AddAttribute(pBodyPartNode, "name", bodyPart);
                XmlNode pRewardNode = pXml.CreateNode(pBodyPartNode, "Reward");
                m_cJevelry[bodyPart].Write2XML(pXml, pRewardNode);
            }

            XmlNode pLootNode = pXml.CreateNode(pXml.Root, "Loot");
            foreach (Item pItem in m_cLoot.Keys)
            {
                XmlNode pRewardNode = pXml.CreateNode(pLootNode, "Reward");
                pXml.AddAttribute(pRewardNode, "count", m_cLoot[pItem]);
                pItem.Write2XML(pXml, pRewardNode);
            }

            pXml.Write(sFilename);
        }
    }
}
