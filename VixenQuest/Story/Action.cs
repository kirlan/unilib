﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using VixenQuest.World;
using VixenQuest.People;

namespace VixenQuest.Story
{
    public enum ActionType
    {
        /// <summary>
        /// traditional vs. traditional
        /// </summary>
        Fucking,
        /// <summary>
        /// traditional vs. traditional
        /// </summary>
        Fucked,
        /// <summary>
        /// foreplay vs. traditional
        /// </summary>
        Fisting,
        /// <summary>
        /// traditional vs. foreplay
        /// </summary>
        Fisted,
        /// <summary>
        /// traditional vs. anal
        /// у нас - активный анальный секс это ДАТЬ в жопу
        /// </summary>
        AssFucking,
        /// <summary>
        /// anal vs. traditional
        /// у нас - пассивный анальный секс это ТРАХНУТЬ в жопу
        /// </summary>
        AssFucked,
        /// <summary>
        /// foreplay vs. anal
        /// </summary>
        AssFisting,
        /// <summary>
        /// anal vs. foreplay
        /// </summary>
        AssFisted,
        /// <summary>
        /// oral vs. traditional (оральный секс - это работа ртом, а не членом. Т.е. активная позиция - это брать в рот, а не давать в рот)
        /// </summary>
        OralFucking,
        /// <summary>
        /// traditional vs. oral (оральный секс - это работа ртом, а не членом. Т.е. пассивная позиция - это давать в рот, а не брать в рот)
        /// </summary>
        OralFucked,
        /// <summary>
        /// sm vs. sm
        /// </summary>
        Sado,
        /// <summary>
        /// sm vs. sm
        /// </summary>
        Maso,
        Seducing,
        Seduced,
        //Pursue,
        //Evade,
        Move,
        SellLoot,
        Bargain,
        Rest
    }

    public class VQAction
    {
        public string m_sName;

        private static string[] m_aActRest = 
        {
            "Sleeping ",
            "Resting ",
            "Relaxing ",
            "Dreaming ",
            "Dancing striptease ",
            "Dancing naked ",
            "Dancing nude ",
            "Performing exotic dances ",
            "Publicly masturbating ",
            "Acting in a striptease show ",
            "Acting in a sexual show ",
            "Acting in a pornographic show ",
            "Participating in a sexual marathon ",
            "Winning a sexual marathon ",
            "Watching a striptease show ",
            "Watching a sexual show ",
            "Watching a miraculous pornographic show ",
            "Looking for a sponsor ",
            "Discussing a new job ",
            "Meeting with the employer ",
            "Degusting a green wine ",
            "Getting drunk ",
            "Taking part in a wild orgy ",
            "Participating in a tantric ritual ",
            //"Spending a couple of days ",
            //"Spending a night ",
            //"Spending a hot night ",
            //"Spending an unforgettable night ",
            //"Spending a few nights ",
            //"Spending a few hot nights ",
            "Having a bath ",
        };

        private static string[] m_aActRestPlace = 
        {
            "in the local inn",
            "in the local bar",
            "in the local pub",
            "at the local bazaar",
            "at the local market",
            "in the local saloon",
            "in the local brothel",
            "in the local garden",
            "in the local monastery",
            "in the local sanctuary",
            "in the local tavern",
            "at the local harvest festival",
            "at the local fair",
            "at the charity fair",
            "at the local coming-of-age ceremony",
        };

        private static string[] m_aActRestOwnedPlace = 
        {
            "in the apartments of ",
            "in the bedroom of ",
            //"in the company of ",
            "in the house of ",
            "in the harem of ",
            "in the dungeon of ",
            "in the palace of ",
            "at the villa of ",
            "at the wedding of ",
            "at the birthday party of ",
            //"with ",
        };

        private static string[] m_aActRestPlaceOwner = 
        {
            "rich merchant",
            "wealthy slaver",
            "famous slaver",
            "wise mage",
            "first mage of the Kingdom",
            "mad prophet",
            "powerfull mage",
            "local landlord",
            "local sex instructor",
            "local sex-symbol",
            "local bandit king",
            "local messiah",
            "old friend",
            "old friends",
            "mysterious stranger",
            "famous traveller",
            //"experienced travellers",
        };

        private static string[] m_aActMoveTo = 
        {
            "Trawelling to the ",
            //"Running to the ",
            "Moving to the ",
            "Riding to the ",
            "Walking to the ",
            "Heading to the ",
        };

        private static string[] m_aActMove = 
        {
            "Trawelling through the ",
            //"Running through the ",
            "Moving through the ",
            "Riding through the ",
            "Walking through the ",
            "Exploring the ",
            "Looking round the ",
            "Wandering in the ",
            //"Going to ",
            //"Running to ",
            //"Moving to ",
            //"Riding to ",
            //"Flying to ",
            //"Heading to ",
            //"Walking to ",
            //"Jumping to ",
            //"Teleporting to ",
            //"Opening a portal to ",
        };

        private static string[] m_aActSado = 
        {
           "Tying up",
           "Whipping",
           "Spanking",
            //"Biting",
           "Bondaging",
           "Branding",
            //"Inflating",
           "Blindfolding",
           "Dripping a hot wax",
           "Pouring a hot wax",
           "Pouring a hot wax on nipples",
           "Biting nipples",
           "Clystering",
            //"Nailing",
            //"Impaling",
           "Pricking needles",
           "Humiliating",
            //"Piercing",
           "Tickling",
           "Raping"
        };

        private static string[] m_aActMaso = 
        {
           "Raped",
           "Whipped",
           "Spanked",
           "Pricked",
           "Bondaged",
           "Blindfolded",
           "Wax-dripped",
           "Wax-poured",
            //"Hanged to cross",
            //"Impaled",
           "Clystered",
           "Branded",
           "Humiliated",
           "Tickled",
           "Tied up",
           "Chained up"
        };

        private static string[] m_aActFlirt1 = 
        {
           "Seducing",
           "Charming",
           //"Dancing striptease",
           "Kissing",
           "Comforting",
           "Caressing",
           "Undressing",
           //"Posing",
           "Persuading",
           "Negotiating",
           //"Posing naked",
           //"Touching own body",
           "Touching",
           //"Masturbating",
           //"Doing a makeup to",
           //"Bodypainting on",
           //"Spying",
           //"Flirting",
           //"Spyed",
           //"Spying upon a masturbating",
           //"Spying upon a sunbathing",
           //"Spying upon a",        
           //"Seduced",
        };

        private static string[] m_aActFlirt2 = 
        {
           "Smothered with kisses",
           "Comforted",
           "Spyed",
           "Kissed",
           "Charmed",
           //"Falling in love to",
            //"Negotiated with",
           //"Spying upon a masturbating",
           //"Spying upon a sunbathing",
           //"Spying upon a",
            //"Bodypainted",
           "Seduced",
           "Catched while bathing nude",
           "Catched while sunbathing nude",
           "Catched while dancing nude in the rain",
           "Catched while masturbating",
        };

        private static string[] m_aActFlirtAnimal1 = 
        {
           "Luring",
           "Decoying",
           "Feeding",
           //"Spying",
           //"Catched while bathing nude",
           //"Catched while sunbathing nude",
           //"Catched while dancing nude in the rain",
           //"Catched while masturbating",
        };

        private static string[] m_aActFlirtAnimal2 = 
        {
           "Attracted",
           "Lured",
           "Catched while bathing nude",
           "Catched while sunbathing nude",
           "Catched while dancing nude in the rain",
           "Catched while masturbating",
        };

        //private static string[] m_aActPursue = 
        //{
        //   "Catching",
        //   "Capturing",
        //   "Pursuing",
        //   "Tracking",
        //   "Chasing",
        //    //"Coming to home of",
        //    //"Seeking for",
        //    //"Collecting rumors about"
        //};

        //private static string[] m_aActEvade = 
        //{
        //   "Sneaking away",
        //   "Escaping",
        //   "Running",
        //   "Hiding"
        //};

        public Opponent m_pTarget;

        public ActionType m_eType;

        public Item m_pLoot;

        public string GetDescription()
        {
            if (m_pTarget == null)
                return m_sName;
            else
            {
                string sName = m_sName.Trim();
                while (sName.EndsWith("."))
                    sName = sName.Remove(sName.Length - 1);
                if (Passive)
                {
                    //if (m_eType == ActionType.Evade)
                    //    return sName + " from " + m_pTarget.LongEncounterName + "...";
                    //else
                        return sName + " by " + m_pTarget.LongEncounterName + "...";
                }
                else
                    return sName + " " + m_pTarget.LongEncounterName + "...";
            }
        }

        private string GetActionName(ActionType eType, Vixen pVixen)
        {
            int part1;
            switch (eType)
            {
                case ActionType.Fucking:
                    return "Fucking";
                case ActionType.Fisting:
                    if (Rnd.OneChanceFrom(5))
                        return "Footing";
                    else
                        if (Rnd.OneChanceFrom(4))
                            return "Double-fisting";
                        else
                            if (Rnd.OneChanceFrom(3))
                                return "Dildoing";
                            else
                                return "Fisting";
                case ActionType.Fucked:
                    return "Fucked";
                case ActionType.Fisted:
                    if (Rnd.OneChanceFrom(5))
                        return "Footed";
                    else
                        if (Rnd.OneChanceFrom(4))
                            return "Double-fisted";
                        else
                            if (Rnd.OneChanceFrom(3))
                                return "Dildoed";
                            else
                                return "Fisted";
                case ActionType.AssFucked:
                    return "Ass-fucking";
                case ActionType.AssFisting:
                    if (Rnd.OneChanceFrom(4))
                        return "Ass-footing";
                    else
                        if (Rnd.OneChanceFrom(2))
                            return "Ass-fisting";
                        else
                            return "Ass-dildoing";
                case ActionType.AssFucking:
                    return "Fucked into ass";
                case ActionType.AssFisted:
                    if (Rnd.OneChanceFrom(4))
                        return "Footed into ass";
                    else
                        if (Rnd.OneChanceFrom(2))
                            return "Fisted into ass";
                        else
                            return "Dildoed into ass";
                case ActionType.OralFucking:
                    if (m_pTarget.Gender == Gender.Male || (m_pTarget.Gender == Gender.Shemale && Rnd.OneChanceFrom(2)))
                    {
                        if (m_pTarget.m_iCount > 1)
                            return "Sucking cocks";
                        else
                            return "Sucking cock";
                    }
                    else
                    {
                        if (m_pTarget.m_iCount > 1)
                            return "Licking cunts";
                        else
                            return "Licking cunt";
                    }
                case ActionType.OralFucked:
                    if (pVixen.Gender == Gender.Male || (pVixen.Gender == Gender.Shemale && Rnd.OneChanceFrom(2) && m_pTarget.m_pRace.m_eSapience != Sapience.Animal))
                        return "Sucked";
                    else
                        return "Licked";
                case ActionType.Seducing:
                    if (m_pTarget.m_pRace.m_eSapience != Sapience.Animal)
                    {
                        part1 = Rnd.Get(m_aActFlirt1.Length);
                        return m_aActFlirt1[part1];
                    }
                    else
                    {
                        part1 = Rnd.Get(m_aActFlirtAnimal1.Length);
                        return m_aActFlirtAnimal1[part1];
                    }
                case ActionType.Seduced:
                    if (m_pTarget.m_pRace.m_eSapience != Sapience.Animal)
                    {
                        part1 = Rnd.Get(m_aActFlirt2.Length);
                        return m_aActFlirt2[part1];
                    }
                    else
                    {
                        part1 = Rnd.Get(m_aActFlirtAnimal2.Length);
                        return m_aActFlirtAnimal2[part1];
                    }
                case ActionType.Sado:
                    part1 = Rnd.Get(m_aActSado.Length);
                    return m_aActSado[part1];
                case ActionType.Maso:
                    part1 = Rnd.Get(m_aActMaso.Length);
                    return m_aActMaso[part1];
                //case ActionType.Pursue:
                //    part1 = Rnd.Get(m_aActPursue.Length);
                //    return m_aActPursue[part1];
                //default:
                //    part1 = Rnd.Get(m_aActEvade.Length);
                //    return m_aActEvade[part1];
            }

            return "Unknown action";
        }

        private bool IsActionPossible(ActionType eType, Vixen pVixen, Opponent pTarget)
        {
            if (eType == ActionType.Move ||
                eType == ActionType.SellLoot ||
                eType == ActionType.Bargain ||
                eType == ActionType.Rest)
                return false;

            if (eType == ActionType.Fucking &&
                (!pVixen.HaveDick || !pTarget.HaveCunt))
                return false;

            if (eType == ActionType.Fucked &&
                (!pVixen.HaveCunt || !pTarget.HaveDick))
                return false;

            if (eType == ActionType.Fisting && !pTarget.HaveCunt)
                return false;

            if (eType == ActionType.Fisted && !pVixen.HaveCunt)
                return false;

            if (pTarget.m_pRace.m_eSapience == Sapience.Animal &&
                (eType == ActionType.Maso ||
                eType == ActionType.Sado))// ||
                //eType == ActionType.OralFucked ||
                //eType == ActionType.OralFucking))
                return false;

            //if (pTarget.m_pRace.m_eSapience == Sapience.Animal &&
            //    (pVixen.HaveCunt || pTarget.HaveCunt) &&
            //    (eType == ActionType.AssFucked ||
            //    eType == ActionType.AssFucking))
            //    return false;

            if (eType == ActionType.AssFucking && !pTarget.HaveDick)
                return false;

            if (eType == ActionType.AssFucked && !pVixen.HaveDick)
                return false;

            //if (eType == ActionType.AssFisting && pTarget.m_pRace.m_eSapience == Sapience.Animal)
            //    return false;

            if (eType == ActionType.AssFisted && pTarget.m_pRace.m_eSapience == Sapience.Animal)
                return false;

            return true;
        }

        public Location m_pLocation;

        private VixenSkill VixenActionSkill(ActionType eType)
        {
            switch (eType)
            {
                case ActionType.Fucked:
                    return VixenSkill.Traditional;
                case ActionType.Fucking:
                    return VixenSkill.Traditional;
                case ActionType.Fisted:
                    return VixenSkill.Traditional;
                case ActionType.Fisting:
                    return VixenSkill.Foreplay;
                case ActionType.AssFucked:
                    return VixenSkill.Traditional;
                case ActionType.AssFucking:
                    return VixenSkill.Anal;
                case ActionType.AssFisted:
                    return VixenSkill.Anal;
                case ActionType.AssFisting:
                    return VixenSkill.Foreplay;
                case ActionType.OralFucked:
                    return VixenSkill.Traditional;
                case ActionType.OralFucking:
                    return VixenSkill.Oral;
                case ActionType.Maso:
                    return VixenSkill.SM;
                case ActionType.Sado:
                    return VixenSkill.SM;
                default:
                    return VixenSkill.Traditional;
            }
        }

        public VixenSkill VixenSkill
        {
            get
            {
                return VixenActionSkill(m_eType);
            }
        }

        private VixenSkill TargetActionSkill(ActionType eType)
        {
            switch (eType)
            {
                case ActionType.Fucked:
                    return VixenSkill.Traditional;
                case ActionType.Fucking:
                    return VixenSkill.Traditional;
                case ActionType.Fisted:
                    return VixenSkill.Foreplay;
                case ActionType.Fisting:
                    return VixenSkill.Traditional;
                case ActionType.AssFucked:
                    return VixenSkill.Anal;
                case ActionType.AssFucking:
                    return VixenSkill.Traditional;
                case ActionType.AssFisted:
                    return VixenSkill.Foreplay;
                case ActionType.AssFisting:
                    return VixenSkill.Anal;
                case ActionType.OralFucked:
                    return VixenSkill.Oral;
                case ActionType.OralFucking:
                    return VixenSkill.Traditional;
                case ActionType.Maso:
                    return VixenSkill.SM;
                case ActionType.Sado:
                    return VixenSkill.SM;
                default:
                    return VixenSkill.Traditional;
            }
        }

        public VixenSkill TargetSkill
        {
            get
            {
                return TargetActionSkill(m_eType);
            }
        }

        public bool IsSkilled
        {
            get
            {
                if (m_eType == ActionType.AssFucked ||
                    m_eType == ActionType.AssFucking ||
                    m_eType == ActionType.Fucked ||
                    m_eType == ActionType.Fucking ||
                    m_eType == ActionType.AssFisted ||
                    m_eType == ActionType.AssFisting ||
                    m_eType == ActionType.Fisted ||
                    m_eType == ActionType.Fisting ||
                    m_eType == ActionType.Maso ||
                    m_eType == ActionType.Sado ||
                    m_eType == ActionType.OralFucked ||
                    m_eType == ActionType.OralFucking)
                    return true;
                else
                    return false;
            }
        }

        //public int ActionXp
        //{
        //    get
        //    {
        //        switch (m_eType)
        //        {
        //            case ActionType.Evade:
        //                return 1;
        //            case ActionType.Seduced:
        //                return 1;
        //            case ActionType.Pursue:
        //                return 2;
        //            case ActionType.Seducing:
        //                return 2;
        //            case ActionType.Fucked:
        //                return 2;
        //            case ActionType.AssFucked:
        //                return 2;
        //            case ActionType.OralFucked:
        //                return 2;
        //            case ActionType.Maso:
        //                return 2;
        //            case ActionType.Fucking:
        //                return 4;
        //            case ActionType.AssFucking:
        //                return 4;
        //            case ActionType.OralFucking:
        //                return 4;
        //            case ActionType.Sado:
        //                return 4;
        //            default:
        //                return 0;
        //        }
        //    }
        //}

        /// <summary>
        /// Encounter progress in percents
        /// 
        /// 50 - neutral state
        /// 0 - Vixen lose encounter
        /// 100 - Vixen wins encounter
        /// </summary>
        //public int m_iProgressPercent = 0;

        public int m_iVixenPotency = -1;
        public int m_iTargetPotency = -1;

        public void SetProgress(Vixen pVixen, int iVixenTiredness, int iTargetTiredness)
        {
            m_iVixenPotency = pVixen.EffectiveStats[Stat.Potency] - iVixenTiredness;
            if (m_iVixenPotency < 0)
                m_iVixenPotency = m_bSuccess ? 1 : 0;

            m_iTargetPotency = m_pTarget.Stats[Stat.Potency] - iTargetTiredness;
            if (m_iTargetPotency < 0)
                m_iTargetPotency = m_bSuccess ? 0 : 1;

            //int iOldPercent = m_iProgressPercent;

            //int iVixenTirednessPercent = iVixenTiredness * 100 / (pVixen.Stats[Stat.Potency]*3);
            //if (iVixenTirednessPercent > 100)
            //    iVixenTirednessPercent = 100;

            //int iTargetTirednessPercent = iTargetTiredness * 100 / (m_pTarget.Stats[Stat.Potency]*3);
            //if (iTargetTirednessPercent > 100)
            //    iTargetTirednessPercent = 100;

            //int iVixenSuccess = 100 - iVixenTiredness;
            //int iTargetSuccess = 100 - iTargetTiredness;

            //if (iVixenSuccess > iTargetSuccess)
            //{
            //    int iTempTotal = 200 - 2 * iTargetSuccess;
            //    int iTempProgress = iTempTotal - (100 - iVixenSuccess);
            //    m_iProgressPercent = iTempProgress * 100 / iTempTotal;
            //}
            //else
            //{
            //    if (iVixenSuccess < iTargetSuccess)
            //    {
            //        int iTempTotal = 200 - 2 * iVixenSuccess;
            //        int iTempProgress = 100 - iTargetSuccess;
            //        m_iProgressPercent = iTempProgress * 100 / iTempTotal;
            //    }
            //    else
            //        m_iProgressPercent = 50;
            //}
        }

        /// <summary>
        /// Парное взаимодействие
        /// </summary>
        /// <param name="pVixen"></param>
        /// <param name="pLocation"></param>
        /// <param name="pTarget"></param>
        /// <param name="eType"></param>
        public VQAction(Vixen pVixen, Location pLocation, Opponent pTarget, ActionType eType)
        {
            m_pLocation = pLocation;
            m_pTarget = pTarget;
            m_eType = eType;

            //m_sName = pLocation.m_sName + ": ";
            m_sName = GetActionName(m_eType, pVixen);
            //if(eType == ActionType.Evade || eType == ActionType.Pursue || eType == ActionType.Seducing)
            //    m_sName += m_pTarget.LongEncounterName;
            //else
            //    m_sName += m_pTarget.ShortEncounterName;
            m_sName += "...";

            m_pLoot = new Loot(this);
        }

        /// <summary>
        /// Продажа лута (SellLoot)
        /// </summary>
        /// <param name="pLoot"></param>
        /// <param name="iCount"></param>
        /// <param name="pLocation"></param>
        public VQAction(Item pLoot, int iCount, Location pLocation)
        {
            m_pLocation = pLocation;

            m_pTarget = null;
            m_eType = ActionType.SellLoot;

            //m_sName = pLocation.m_sName + ": ";
            if (iCount > 1)
                m_sName = "Selling " + iCount.ToString() + " " + pLoot.m_sNames.ToLower() + " for " + iCount * pLoot.m_iPrice + " gold coins...";
            else
                m_sName = "Selling " + pLoot.m_sName.ToLower() + " for " + pLoot.m_iPrice + " gold coins...";

            m_pLoot = pLoot;
        }

        /// <summary>
        /// Движение. (Move)
        /// </summary>
        /// <param name="pLocation"></param>
        public VQAction(Vixen pVixen, Location pLocation, Location pDestination)
        {
            m_pLocation = pLocation;

            m_pTarget = null;
            m_eType = ActionType.Move;

            if (pDestination == null)
            {
                int part1 = Rnd.Get(m_aActMove.Length);
                m_sName = m_aActMove[part1] + pLocation.m_sName + "...";
            }
            else
            {
                int part1 = Rnd.Get(m_aActMoveTo.Length);
                m_sName = m_aActMoveTo[part1] + pDestination.m_sName + "...";
            }

            m_pLoot = new Loot(this);
        }

        /// <summary>
        /// "Соло"-действие.
        /// Допустимые варианты: Move, Bargain, Rest
        /// </summary>
        /// <param name="pLocation"></param>
        public VQAction(Vixen pVixen, Location pLocation, ActionType eType)
        {
            m_pLocation = pLocation;

            m_pTarget = null;
            m_eType = eType;

            switch (m_eType)
            {
                case ActionType.Move:
                    int part1 = Rnd.Get(m_aActMove.Length);
                    m_sName = m_aActMove[part1] + pLocation.m_sName + "...";
                    break;
                case ActionType.Bargain:
                    m_sName = "Bargaining for a better equipment...";
                    break;
                case ActionType.Rest:
                    part1 = Rnd.Get(m_aActRest.Length);
                    m_sName = m_aActRest[part1];

                    if (Rnd.OneChanceFrom(2))
                    {
                        int part2 = Rnd.Get(m_aActRestPlace.Length);
                        m_sName += m_aActRestPlace[part2];
                    }
                    else
                    {
                        int part2 = Rnd.Get(m_aActRestOwnedPlace.Length);
                        m_sName += m_aActRestOwnedPlace[part2];

                        int part3 = Rnd.Get(m_aActRestPlaceOwner.Length);
                        m_sName += m_aActRestPlaceOwner[part3];
                    }
                    m_sName += "...";

                    m_iVixenPotency = pVixen.EffectiveStats[Stat.Potency];

                    break;
            }

            m_pLoot = new Loot(this);
        }

        public bool Passive
        {
            get
            {
                if (m_eType == ActionType.AssFucked ||
                   m_eType == ActionType.Fucked ||
                   m_eType == ActionType.AssFisted ||
                   m_eType == ActionType.Fisted ||
                   m_eType == ActionType.Maso ||
                   m_eType == ActionType.OralFucked ||
                   m_eType == ActionType.Seduced)// ||
                   //m_eType == ActionType.Evade)
                    return true;
                else
                    return false;
            }
        }

        public int CostToVixen(Vixen pVixen)
        {
            int iResult = 0;
            if (IsSkilled)
            {
                if(Passive)
                    iResult = Math.Max(pVixen.Skills[VixenActionSkill(m_eType)], m_pTarget.Skills[TargetActionSkill(m_eType)]);
                else
                    iResult = Math.Min(pVixen.Skills[VixenActionSkill(m_eType)], m_pTarget.Skills[TargetActionSkill(m_eType)]);
            }
            else
            {
                switch (m_eType)
                {
                    //case ActionType.Evade:
                    //    if (Passive)
                    //        iResult = Math.Max(pVixen.Stats[Stat.Presence], m_pTarget.Stats[Stat.Presence]); 
                    //    else
                    //        iResult = Math.Min(pVixen.Stats[Stat.Presence], m_pTarget.Stats[Stat.Presence]);
                    //    break;
                    //case ActionType.Pursue:
                    //    if (Passive)
                    //        iResult = Math.Max(pVixen.Stats[Stat.Presence], m_pTarget.Stats[Stat.Presence]);
                    //    else
                    //        iResult = Math.Min(pVixen.Stats[Stat.Presence], m_pTarget.Stats[Stat.Presence]);
                    //    break;
                    case ActionType.Seduced:
                        if (Passive)
                            iResult = Math.Max(pVixen.Stats[Stat.Beauty], m_pTarget.Stats[Stat.Beauty]);
                        else
                            iResult = Math.Min(pVixen.Stats[Stat.Beauty], m_pTarget.Stats[Stat.Beauty]);
                        break;
                    case ActionType.Seducing:
                        if (Passive)
                            iResult = Math.Max(pVixen.Stats[Stat.Beauty], m_pTarget.Stats[Stat.Beauty]);
                        else
                            iResult = Math.Min(pVixen.Stats[Stat.Beauty], m_pTarget.Stats[Stat.Beauty]);
                        break;
                }
            }
            //if (Passive)
            //    iResult = iResult * 2;

            return iResult;
        }

        public int CostToTarget(Vixen pVixen)
        {
            int iResult = 0;
            if (IsSkilled)
            {
                if (!Passive)
                    iResult = Math.Max(pVixen.Skills[VixenActionSkill(m_eType)], m_pTarget.Skills[TargetActionSkill(m_eType)]);
                else
                    iResult = Math.Min(pVixen.Skills[VixenActionSkill(m_eType)], m_pTarget.Skills[TargetActionSkill(m_eType)]);
            }
            else
            {
                switch (m_eType)
                {
                    //case ActionType.Evade:
                    //    if (!Passive)
                    //        iResult = Math.Max(pVixen.Stats[Stat.Presence], m_pTarget.Stats[Stat.Presence]);
                    //    else
                    //        iResult = Math.Min(pVixen.Stats[Stat.Presence], m_pTarget.Stats[Stat.Presence]);
                    //    break;
                    //case ActionType.Pursue:
                    //    if (!Passive)
                    //        iResult = Math.Max(pVixen.Stats[Stat.Presence], m_pTarget.Stats[Stat.Presence]);
                    //    else
                    //        iResult = Math.Min(pVixen.Stats[Stat.Presence], m_pTarget.Stats[Stat.Presence]);
                    //    break;
                    case ActionType.Seduced:
                        if (!Passive)
                            iResult = Math.Max(pVixen.Stats[Stat.Beauty], m_pTarget.Stats[Stat.Beauty]);
                        else
                            iResult = Math.Min(pVixen.Stats[Stat.Beauty], m_pTarget.Stats[Stat.Beauty]);
                        break;
                    case ActionType.Seducing:
                        if (!Passive)
                            iResult = Math.Max(pVixen.Stats[Stat.Beauty], m_pTarget.Stats[Stat.Beauty]);
                        else
                            iResult = Math.Min(pVixen.Stats[Stat.Beauty], m_pTarget.Stats[Stat.Beauty]);
                        break;
                }
            }
            //if (!Passive)
            //    iResult = iResult * 2;

            return iResult;
        }

        private bool m_bSuccess = true;

        public bool Success
        {
            get { return m_bSuccess; }
        }

        /// <summary>
        /// Успехом считается, если герой перехватил инициативу
        /// </summary>
        /// <param name="pVixen"></param>
        public void CalcSuccess(Vixen pVixen)
        {
            switch (m_eType)
            {
                //case ActionType.Evade:
                //    CalcStatSuccess(pVixen, Stat.Presence);
                //    break;
                //case ActionType.Pursue:
                //    CalcStatSuccess(pVixen, Stat.Presence);
                //    break;
                case ActionType.Seduced:
                    CalcStatSuccess(pVixen, Stat.Beauty);
                    break;
                case ActionType.Seducing:
                    CalcStatSuccess(pVixen, Stat.Beauty);
                    break;
                case ActionType.Fucked:
                    CalcSkillSuccess(pVixen);
                    break;
                case ActionType.AssFucked:
                    CalcSkillSuccess(pVixen);
                    break;
                case ActionType.Fisted:
                    CalcSkillSuccess(pVixen);
                    break;
                case ActionType.AssFisted:
                    CalcSkillSuccess(pVixen);
                    break;
                case ActionType.OralFucked:
                    CalcSkillSuccess(pVixen);
                    break;
                case ActionType.Maso:
                    CalcSkillSuccess(pVixen);
                    break;
                case ActionType.Fucking:
                    CalcSkillSuccess(pVixen);
                    break;
                case ActionType.AssFucking:
                    CalcSkillSuccess(pVixen);
                    break;
                case ActionType.Fisting:
                    CalcSkillSuccess(pVixen);
                    break;
                case ActionType.AssFisting:
                    CalcSkillSuccess(pVixen);
                    break;
                case ActionType.OralFucking:
                    CalcSkillSuccess(pVixen);
                    break;
                case ActionType.Sado:
                    CalcSkillSuccess(pVixen);
                    break;
            }
        }

        private void CalcStatSuccess(Vixen pVixen, Stat eStat)
        {
            int iVixenChances = 0;
            int iTargetChances = 0;

            iVixenChances = pVixen.EffectiveStats[eStat];
            iTargetChances = m_pTarget.Stats[eStat];

            m_bSuccess = Rnd.Chances(iVixenChances, iVixenChances + iTargetChances);
            //return Rnd.Chances((int)Math.Pow(iVixenChances, 2), (int)Math.Pow(iVixenChances, 2) + (int)Math.Pow(iTargetChances, 2));
        }

        private void CalcSkillSuccess(Vixen pVixen)
        {
            int iVixenChances = 0;
            int iTargetChances = 0;

            iVixenChances = pVixen.Skills[VixenActionSkill(m_eType)];
            iTargetChances = m_pTarget.Skills[TargetActionSkill(m_eType)];
            if (!pVixen.WannaFuck(m_pTarget))
                iVixenChances = iVixenChances / 2;
            if (!m_pTarget.WannaFuck(pVixen))
                iTargetChances = iTargetChances / 2;

            m_bSuccess = Rnd.Chances(iVixenChances, iVixenChances + iTargetChances);
            //return Rnd.Chances((int)Math.Pow(iVixenChances, 2), (int)Math.Pow(iVixenChances, 2) + (int)Math.Pow(iTargetChances, 2));
        }

        public override string ToString()
        {
            return m_sName;
        }
    }
}
