using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using VixenQuest.World;
using NameGen;

namespace VixenQuest.People
{
    class Profession
    { 
    }

    public class Opponent: Person
    {
        private string m_sLongEncounterName;

        public string LongEncounterName
        {
            get { return m_sLongEncounterName; }
        }

        private string m_sShortEncounterName;

        public string ShortEncounterName
        {
            get { return m_sShortEncounterName; }
        }

        private string m_sSingleName;

        public string SingleName
        {
            get { return m_sSingleName; }
        }

        //Ранги живвотных рас 1-10-20-30-40
        //для людей ранг профессии 1-17, и ранг расы - до 60 с шагом 10
        private static ValuedString[] m_aAnimalMood = 
        {
            new ValuedString("fat ", 0),
            new ValuedString("hairy ", 0),
            new ValuedString("black ", 0),
            new ValuedString("young ", 1),
            new ValuedString("wild ", 1),
            new ValuedString("amative ", 1),
            new ValuedString("horny ", 2),
            new ValuedString("lusty ", 2),
            new ValuedString("sexy ", 3),
            new ValuedString("horned ", 4),
            new ValuedString("big-eyed ", 6),
            new ValuedString("mad ", 8),
            new ValuedString("crazy ", 10),
            new ValuedString("mighty ", 12),
            new ValuedString("posessed ", 14),
            new ValuedString("devious ", 16),
            new ValuedString("glorious ", 18),
            new ValuedString("infamous ", 21),
            new ValuedString("heroic ", 24),
            new ValuedString("well-known ", 27),
            new ValuedString("legendary ", 30),
            new ValuedString("sacred ", 33),
            new ValuedString("mythic ", 36),
        };

        //для людей ранг профессии 1-17, и ранг расы - до 60 с шагом 10
        private static ValuedString[] m_aMood = 
        {
            new ValuedString("drunken ", 0),
            new ValuedString("fat ", 0),
            new ValuedString("young ", 1),
            //new ValuedString("old ", 1),
            new ValuedString("red-haired ", 1),
            //new ValuedString("green-skin ", 1),
            new ValuedString("hairy ", 1),
            new ValuedString("poor ", 1),
            new ValuedString("black ", 1),
            new ValuedString("underage ", 1),
            new ValuedString("tall ", 1),
            //new ValuedString("slim ", 1),
            //new ValuedString("lonely ", 2),
            new ValuedString("lusty ", 2),
            new ValuedString("sad ", 2),
            new ValuedString("infurious ", 2),
            new ValuedString("mischievius ", 2),
            new ValuedString("notorious ", 2),
            //"blindfold ", 1),
            new ValuedString("barefoot ", 3),
            new ValuedString("bare-naked ", 3),
            new ValuedString("sexy ", 3),
            new ValuedString("kinky ", 3),
            new ValuedString("shameful ", 3),
            new ValuedString("rebellous ", 3),
            new ValuedString("horny ", 3),
            new ValuedString("mad ", 4),
            new ValuedString("crazy ", 4),
            new ValuedString("underfucked ", 4),
            new ValuedString("wild ", 4),
            new ValuedString("runaway ", 5),
            new ValuedString("scandalous ", 5),
            new ValuedString("cool ", 5),
            new ValuedString("amateur ", 6),
            new ValuedString("bold ", 6),
            new ValuedString("brave ", 6),
            new ValuedString("mighty ", 7),
            new ValuedString("wise ", 7),
            new ValuedString("wealthy ", 7),
            //new ValuedString("sinful ", 3),
            new ValuedString("undressed ", 8),
            new ValuedString("naked ", 8),
            new ValuedString("horned ", 9),
            new ValuedString("glorious ", 10),
            new ValuedString("famous ", 10),
            new ValuedString("heroic ", 11),
            new ValuedString("infamous ", 11),
            new ValuedString("albino ", 12),
            new ValuedString("stylish ", 12),
            new ValuedString("nudist ", 13),
            new ValuedString("posessed ", 13),
            new ValuedString("experienced ", 14),
            new ValuedString("mature ", 15),
            new ValuedString("well-known ", 16),
            //"topless ", 1),
            //"bottomless ", 1),
            new ValuedString("devious ", 18),
            new ValuedString("epic ", 20),
            new ValuedString("legendary ", 21),
            new ValuedString("innocent ", 23),
            new ValuedString("virgin ", 25),
        };

        //private static ValuedString[] m_aProfessionM = 
        //{
        //    //lowest ranks - everybody could command them
        //    new ValuedString("slave", 0),
        //    new ValuedString("servant", 1),
        //    new ValuedString("peasant", 1),
        //    //still low, but they have mo masters
        //    new ValuedString("stranger", 2),
        //    new ValuedString("traveller", 2),
        //    new ValuedString("hermit", 2),
        //    //small, but own buisiness
        //    new ValuedString("barman", 3),
        //    new ValuedString("shopkeeper", 3),
        //    new ValuedString("hunter", 3),
        //    new ValuedString("ranger", 3),
        //    new ValuedString("scavenger", 3),
        //    new ValuedString("butcher", 3),
        //    //crime grunts
        //    new ValuedString("marauder", 4),
        //    new ValuedString("rogue", 4),
        //    new ValuedString("bandit", 4),
        //    new ValuedString("pirate", 4),
        //    new ValuedString("burglar", 4),
        //    //city crime
        //    new ValuedString("pimp", 5),
        //    new ValuedString("thief", 5),
        //    //well-paid workers
        //    new ValuedString("teacher", 6),
        //    new ValuedString("minstrel", 6),
        //    new ValuedString("troubadour", 6),
        //    new ValuedString("merchant", 6),
        //    new ValuedString("slaver", 6),
        //    new ValuedString("executioner", 6),
        //    new ValuedString("masturbator", 6),
        //    new ValuedString("deflorator", 6),
        //    //common adventurers
        //    new ValuedString("fighter", 7),
        //    new ValuedString("monk", 7),
        //    new ValuedString("warden", 7),
        //    new ValuedString("mage apprentice", 7),
        //    new ValuedString("monk", 7),
        //    new ValuedString("bruiser", 7),
        //    new ValuedString("guardian", 7),
        //    new ValuedString("warrior", 7),
        //    //professional adventurers
        //    new ValuedString("gladiator", 8),
        //    new ValuedString("assasin", 8),
        //    new ValuedString("ninja", 8),
        //    new ValuedString("spy", 8),
        //    new ValuedString("sentinel", 8),
        //    new ValuedString("slut-hunter", 8),
        //    new ValuedString("head-hunter", 8),
        //    new ValuedString("bitch-hunter", 8),
        //    //mages, clerics, etc.
        //    new ValuedString("witch-hunter", 9),
        //    new ValuedString("demon-hunter", 9),
        //    new ValuedString("cleric", 9),
        //    new ValuedString("necromancer", 9),
        //    new ValuedString("eromancer", 9),
        //    new ValuedString("summoner", 9),
        //    new ValuedString("inquisitor", 9),
        //    new ValuedString("priest", 9),
        //    new ValuedString("sage", 9),
        //    new ValuedString("shaman", 9),
        //    new ValuedString("mage", 9),
        //    new ValuedString("prophet", 9),
        //    //nobile & rich
        //    new ValuedString("knight", 10),
        //    new ValuedString("hero", 10),
        //    new ValuedString("playboy", 10),
        //    //officials
        //    new ValuedString("Master", 11),
        //    new ValuedString("bishop", 11),
        //    new ValuedString("vizier", 11),
        //    new ValuedString("noble", 12),
        //    new ValuedString("landlord", 12),
        //    //local rulers
        //    new ValuedString("Lord", 13),
        //    new ValuedString("Chef", 13),
        //    new ValuedString("General", 13),
        //    new ValuedString("Count", 14),
        //    new ValuedString("Baron", 14),
        //    new ValuedString("Warlord", 16),
        //    new ValuedString("Messiah", 17),
        //};

        //private static ValuedString[] m_aProfessionF = 
        //{
        //    //lowest ranks - everybody could command them
        //    new ValuedString("slavegirl", 0),
        //    new ValuedString("peasant maid", 1),
        //    //still low, but they have mo masters
        //    new ValuedString("maid", 2),
        //    new ValuedString("schoolgirl", 2),
        //    new ValuedString("housemaid", 2),
        //    new ValuedString("housewife", 2),
        //    new ValuedString("chick", 2),
        //    //small, but own buisiness
        //    new ValuedString("barmaid", 3),
        //    new ValuedString("huntress", 3),
        //    //crime grunts
        //    new ValuedString("prostitute", 4),
        //    new ValuedString("whore", 4),
        //    new ValuedString("bitch", 4),
        //    new ValuedString("wench", 4),
        //    new ValuedString("slut", 4),
        //    //new ValuedString("rogue", 4),
        //    new ValuedString("burglaress", 4),
        //    //city crime
        //    new ValuedString("thief", 5),
        //    //well-paid workers
        //    new ValuedString("dancer", 6),
        //    new ValuedString("geisha", 6),
        //    new ValuedString("actress", 6),
        //    new ValuedString("bride", 6),
        //    //common adventurers
        //    new ValuedString("masturbatrix", 7),
        //    new ValuedString("wardeness", 7),
        //    //professional adventurers
        //    //new ValuedString("sentinel", 8),
        //    new ValuedString("head-huntress", 8),
        //    new ValuedString("bitch-huntress", 8),
        //    new ValuedString("assasiness", 8),
        //    new ValuedString("gladiatrix", 8),
        //    //mages, clerics, etc.
        //    new ValuedString("witch-huntress", 9),
        //    new ValuedString("demon-huntress", 9),
        //    new ValuedString("priestess", 9),
        //    new ValuedString("witch", 9),
        //    new ValuedString("sorceress", 9),
        //    new ValuedString("prophetess", 9),
        //    new ValuedString("inquisitrix", 9),
        //    new ValuedString("necromantress", 9),
        //    new ValuedString("enchantress", 9),
        //    new ValuedString("shamaness", 9),
        //    //nobile & rich
        //    new ValuedString("heroine", 10),
        //    new ValuedString("courtesan", 10),
        //    //officials
        //    new ValuedString("Mistress", 11),
        //    new ValuedString("noble lady", 12),
        //    //local rulers
        //    new ValuedString("Baroness", 13),
        //    new ValuedString("Countess", 14),
        //    new ValuedString("Messiah", 17),
        //};

        private static ValuedString[] m_aGodType = 
        {
            new ValuedString("of War", 1),
            new ValuedString("of Death", 1),
            new ValuedString("of Life", 1),
            new ValuedString("of Sun", 1),
            new ValuedString("of Wind", 1),
            new ValuedString("of Fire", 1),
            new ValuedString("of Thunder", 1),
            new ValuedString("of Moon", 1),
            new ValuedString("of Nature", 2),
            new ValuedString("of Sky", 2),
            new ValuedString("of Light", 2),
            new ValuedString("of Darkness", 2),
            new ValuedString("of Night", 2),
            new ValuedString("of Starlight", 2),
            new ValuedString("of Punishment", 3),
            new ValuedString("of Birth", 3),
            new ValuedString("of Happiness", 3),
            new ValuedString("of Pleasure", 4),
            new ValuedString("of Desire", 4),
            new ValuedString("of Love", 4),
            new ValuedString("of Sex", 4),
            new ValuedString("of Lust", 4),
            new ValuedString("of Orgazm", 5),
            //new ValuedString("of Extasy", 10),
            new ValuedString("of Virginity", 5),
            new ValuedString("of Fertility", 5),
            new ValuedString("of Mothership", 5),
            new ValuedString("of Pregnacy", 5),
            new ValuedString("of Nudity", 6),
            new ValuedString("of Masturbation", 6),
            new ValuedString("of Domination", 6),
            new ValuedString("of Submission", 6),
            new ValuedString("of Free Love", 6),
            new ValuedString("of Bondage", 7),
            new ValuedString("of Sweet Pain", 7),
            new ValuedString("of Exhibitionism", 7),
            new ValuedString("of Voyeurism", 7),
            new ValuedString("of Gynophagia", 8),
            new ValuedString("of Eternal Love", 9),
            new ValuedString("of Everlust", 10),
            new ValuedString("of Everything", 11),
        };

        public int EncounterRank
        {
            get { return (int)(m_iLevel * Math.Sqrt(m_iCount)); }
        }

        public int m_iCount;

        public Race m_pRace;

        private void Create(Race pRace)
        {
            m_pRace = pRace;

            bool bAllowed;
            do
            {
                bAllowed = true;
                int genderId = Rnd.Get(Enum.GetValues(typeof(Gender)).Length);
                m_eGender = (Gender)Enum.GetValues(typeof(Gender)).GetValue(genderId);

                if (m_eGender == Gender.Shemale && pRace.m_eSapience == Sapience.Animal)
                    bAllowed = false;
            }
            while (!bAllowed);

            if (pRace.m_eSapience != Sapience.Human || Rnd.OneChanceFrom(2))
            {
                if (pRace.m_eSapience == Sapience.Animal)
                    m_eOrientation = Orientation.Stright;
                else
                {
                    int orientationId = Rnd.Get(Enum.GetValues(typeof(Orientation)).Length);
                    m_eOrientation = (Orientation)Enum.GetValues(typeof(Orientation)).GetValue(orientationId);
                }
            }
            else
                m_eOrientation = pRace.m_ePreferredOrientation;

            //if (m_eGender == Gender.Male && m_eOrientation == Orientation.Bi)
            //{
            //    int orientationId = Rnd.Get(Enum.GetValues(typeof(Orientation)).Length);
            //    m_eOrientation = (Orientation)Enum.GetValues(typeof(Orientation)).GetValue(orientationId);
            //}

            m_iLevel = pRace.m_iRank;
            int iMoodRank = 0;

            if (Rnd.OneChanceFrom(3) || pRace.m_sName == "")
            {
                int iMood = Rnd.Get(m_aMood.Length);
                m_sSingleName = m_aMood[iMood].m_sName;
                iMoodRank = m_aMood[iMood].m_iValue;
            }
            else
                m_sSingleName = "";

            if (pRace.m_eSapience == Sapience.Animal)
            {
                int iMood = Rnd.Get(m_aAnimalMood.Length);
                m_sSingleName = m_aAnimalMood[iMood].m_sName;
                iMoodRank = m_aAnimalMood[iMood].m_iValue;
            }

            if (HaveCunt)
                m_sSingleName += pRace.m_sNameF;
            else
                m_sSingleName += pRace.m_sName;

            if (m_eGender == Gender.Shemale)
            {
                m_sSingleName = "shemale " + m_sSingleName;
                m_iLevel += 10;
            }

            if (m_eOrientation == Orientation.Bi)
            {
                m_sSingleName = "bisexual " + m_sSingleName;
                iMoodRank += 5;
            }

            if (m_eOrientation == Orientation.Homo)
            {
                m_sSingleName = OrientationString.ToLower() + " " + m_sSingleName;
                iMoodRank += 10;
            }

            m_iLevel += iMoodRank;
        }

        public ValuedString m_pProfession;

        private void ChooseProfession(ValuedString[] aProfessionM, ValuedString[] aProfessionF)
        {
            if (m_pRace.m_eSapience == Sapience.Human)
            {
                if (HaveCunt)
                {
                        int part2 = Rnd.Get(aProfessionF.Length);
                        m_pProfession = aProfessionF[part2];
                        m_sSingleName += aProfessionF[part2].m_sName;
                        m_iLevel += aProfessionF[part2].m_iValue;
                }
                else
                {
                        int part2 = Rnd.Get(aProfessionM.Length);
                        m_pProfession = aProfessionM[part2];
                        m_sSingleName += aProfessionM[part2].m_sName;
                        m_iLevel += aProfessionM[part2].m_iValue;
                }
            }
        }

        private void MakeEncounter()
        {
            m_iCount = (int)Math.Sqrt(1 + Rnd.Get(24 / m_iLevel));

            if (m_iCount > 1)
            {
                //m_sEncounterName = m_iCount.ToString() + " " + m_sSingleName.Trim();
                string sBand;
                if (Rnd.OneChanceFrom(4))
                    sBand = "band of ";
                else
                    if (Rnd.OneChanceFrom(3))
                        sBand = "company of ";
                    else
                        if (Rnd.OneChanceFrom(2))
                            sBand = "gang of ";
                        else
                            sBand = "group of ";

                if (m_pRace.m_eSapience == Sapience.Animal)
                    sBand = "pack of ";

                m_sShortEncounterName = m_sSingleName.Trim();
                if (m_sShortEncounterName.EndsWith("s") ||
                    m_sShortEncounterName.EndsWith("ch") ||
                    m_sShortEncounterName.EndsWith("x"))
                {
                    m_sShortEncounterName += "es";
                }
                else
                {
                    if (m_sShortEncounterName.EndsWith("f"))
                    {
                        m_sShortEncounterName = m_sShortEncounterName.Remove(m_sShortEncounterName.Length - 1);
                        m_sShortEncounterName += "ves";
                    }
                    else
                    {
                        if (m_sShortEncounterName.EndsWith("man"))
                        {
                            m_sShortEncounterName = m_sShortEncounterName.Remove(m_sShortEncounterName.Length - 3);
                            m_sShortEncounterName += "men";
                        }
                        else
                            m_sShortEncounterName += "s";
                    }
                }
                m_sLongEncounterName = sBand + m_sShortEncounterName;
            }
            else
            {
                m_sShortEncounterName = m_sSingleName;
                m_sLongEncounterName = m_sSingleName;
            }

            //m_sShortEncounterName += " (lv." + Level.ToString() + "/" + EncounterRank.ToString() + ")";
            //m_sLongEncounterName += " (lv." + Level.ToString() + "/" + EncounterRank.ToString() + ")";
        }

        private void CalcStatsAndSkills()
        {
            foreach (Stat eStat in Enum.GetValues(typeof(Stat)))
                m_cStats[eStat] = 3 * Math.Max(1, (int)Math.Sqrt(m_iCount) * (m_iLevel / 2 + Rnd.Get(m_iLevel)));

            foreach (VixenSkill eSkill in Enum.GetValues(typeof(VixenSkill)))
                m_cSkills[eSkill] = Math.Max(1, (int)Math.Sqrt(m_iCount) * (m_iLevel / 2 + Rnd.Get(m_iLevel)));

            m_cSkills[m_pRace.m_ePreferredSexType] = m_iLevel + Rnd.Get(m_iLevel);
        }

        public string m_sName;
        public string m_sFamily;

        public State m_pCitizenship;

        /// <summary>
        /// Лица без гражданства - например, боги и демоны
        /// </summary>
        /// <param name="pRace">раса</param>
        public Opponent(Race pRace)
        {
            m_pCitizenship = null;

            Create(pRace);

            if (m_pRace.m_eSapience == Sapience.God)
            {
                int part2 = Rnd.Get(m_aGodType.Length);
                m_sSingleName += m_aGodType[part2].m_sName;
                m_iLevel += m_aGodType[part2].m_iValue;
            }

            m_iCount = 1;
            m_sShortEncounterName = m_sSingleName;
            m_sLongEncounterName = m_sSingleName;

            CalcStatsAndSkills();

            m_sName = NameGenerator.GetHeroName(m_eGender == Gender.Male);
            m_sFamily = NameGenerator.GetEthnicName(m_pRace.m_eLanguage);
            m_sSingleName = m_sName + " " + m_sFamily + ", " + m_sSingleName;
        }

        public string GetDescription()
        {
            return (m_eGender == Gender.Shemale ? "shemale " : "") +
                (m_eOrientation == Orientation.Bi ? "bisexual " : "") +
                (m_eOrientation == Orientation.Homo ? OrientationString.ToLower() + " " : "") + 
                (HaveCunt ? m_pRace.m_sNameF : m_pRace.m_sName).Trim() + " " + m_pProfession.m_sName;
        }

        /// <summary>
        /// Простые граждане
        /// </summary>
        /// <param name="pHome">место жительства</param>
        /// <param name="iMaxProfessionRank">максимальная допустимая "крутость" профессии</param>
        public Opponent(Location pHome, Race pRace, ValuedString[] aProfessionM, ValuedString[] aProfessionF, bool bSingle, Opponent pFounder)
        {
            m_pHome = pHome;
            m_pCitizenship = pHome.m_pState;

            Create(pRace);

            ChooseProfession(pHome.ProfessionM, pHome.ProfessionF);

            if (bSingle)
            {
                m_iCount = 1;
                m_sShortEncounterName = m_sSingleName;
                m_sLongEncounterName = m_sSingleName;
            }
            else
                MakeEncounter();

            CalcStatsAndSkills();

            m_sName = NameGenerator.GetHeroName(m_eGender == Gender.Male);
            if (pFounder != null)
                m_sFamily = pFounder.m_sFamily;
            else
                m_sFamily = NameGenerator.GetEthnicName(m_pRace.m_eLanguage);
            m_sSingleName = m_sName + " " + m_sFamily + ", " + m_sSingleName;
        }

        /// <summary>
        /// Правящая верхушка
        /// </summary>
        /// <param name="pState">государство</param>
        /// <param name="bRuler">правитель (true) или наследник (false)</param>
        public Opponent(State pState, bool bRuler)
        {
            m_pCitizenship = pState;

            int index = Rnd.Get(pState.m_cRaces[State.DwellersCathegory.MajorRaces].Count);

            Create(pState.m_cRaces[State.DwellersCathegory.MajorRaces][index]);

            if (HaveCunt)
                m_pProfession = new ValuedString(bRuler ? pState.m_pInfo.m_sRulerF : pState.m_pInfo.m_sHeirF, bRuler ? pState.m_pInfo.m_iRank : pState.m_pInfo.m_iRank-1);
            else
                m_pProfession = new ValuedString(bRuler ? pState.m_pInfo.m_sRulerM : pState.m_pInfo.m_sHeirM, bRuler ? pState.m_pInfo.m_iRank : pState.m_pInfo.m_iRank - 1);

            m_sSingleName += m_pProfession.m_sName + " of " + pState.m_sShortName;

            m_iLevel += m_pProfession.m_iValue;

            m_iCount = 1;
            m_sShortEncounterName = m_sSingleName;
            m_sLongEncounterName = m_sSingleName;

            CalcStatsAndSkills();

            m_sName = NameGenerator.GetHeroName(m_eGender == Gender.Male);
            if(pState.m_cRulers.Count > 0)
                m_sFamily = pState.m_cRulers[0].m_sFamily;
            else
                m_sFamily = NameGenerator.GetEthnicName(m_pRace.m_eLanguage);
            m_sSingleName = m_sName + " " + m_sFamily + ", " + m_sSingleName;
        }

        /// <summary>
        /// Чей-то знакомый или родственник
        /// </summary>
        /// <param name="pBase">тот, чей</param>
        /// <param name="bCouldBeSibling">родственник (true) или просто знакомый (false)</param>
        public Opponent(Opponent pBase, bool bCouldBeSibling)
        {
            m_pCitizenship = pBase.m_pCitizenship;

            Create(pBase.m_pRace);

            m_sName = NameGenerator.GetHeroName(m_eGender == Gender.Male);
            m_sFamily = NameGenerator.GetEthnicName(pBase.m_pRace.m_eLanguage);

            if (pBase.m_pRace.m_eSapience == Sapience.Human)
            {
                string sRelation = pBase.m_pProfession.m_sName + " " + pBase.m_sName;

                if (sRelation.EndsWith("s"))
                    sRelation += "'";
                else
                    sRelation += "'s";

                if (HaveCunt)
                {
                    if (Rnd.OneChanceFrom(5) && bCouldBeSibling)
                    {
                        sRelation += " stepsister";
                        if(Rnd.OneChanceFrom(2))
                            m_sFamily = pBase.m_sFamily;
                    }
                    else
                        if (Rnd.OneChanceFrom(4) && bCouldBeSibling)
                        {
                            sRelation += " sister";
                            m_sFamily = pBase.m_sFamily;
                        }
                        else
                            if (Rnd.OneChanceFrom(3) && bCouldBeSibling)
                            {
                                sRelation += " daughter";
                                m_sFamily = pBase.m_sFamily;
                            }
                            else
                                if (Rnd.OneChanceFrom(2))
                                    sRelation += " ex-girlfriend";
                                else
                                    sRelation += " girlfriend";
                }
                else
                {
                    if (Rnd.OneChanceFrom(5) && bCouldBeSibling)
                    {
                        sRelation += " stepbrother";
                        if (Rnd.OneChanceFrom(2))
                            m_sFamily = pBase.m_sFamily;
                    }
                    else
                        if (Rnd.OneChanceFrom(4) && bCouldBeSibling)
                        {
                            sRelation += " brother";
                            m_sFamily = pBase.m_sFamily;
                        }
                        else
                            if (Rnd.OneChanceFrom(3) && bCouldBeSibling)
                            {
                                sRelation += " son";
                                m_sFamily = pBase.m_sFamily;
                            }
                            else
                                if (Rnd.OneChanceFrom(2))
                                    sRelation += " ex-boyfriend";
                                else
                                    sRelation += " boyfriend";
                }

                m_pProfession = new ValuedString(sRelation, pBase.m_pProfession.m_iValue);

                m_sSingleName += m_pProfession.m_sName;
                m_iLevel += m_pProfession.m_iValue;
            }

            MakeEncounter();

            CalcStatsAndSkills();

            m_sSingleName = m_sName + " " + m_sFamily + ", " + m_sSingleName;
        }

        public override string ToString()
        {
            return m_sLongEncounterName;
        }
    }
}
