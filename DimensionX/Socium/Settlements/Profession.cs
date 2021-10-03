using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socium.Settlements
{
    public class ProfessionInfo
    {
        public enum SkillLevel
        {
            None = 0,
            Bad,
            Good,
            Excellent
        };

        public enum Caste
        {
            Elite,
            MiddleOrUp,
            Low,
            Outlaw
        };

        public static readonly List<ProfessionInfo> s_cAllProfessions = new List<ProfessionInfo>();

        #region Commoners
        public static readonly ProfessionInfo Nobody = new ProfessionInfo("nobody", false, SkillLevel.None, SkillLevel.None, SkillLevel.None);

        public static readonly ProfessionInfo Loafer = new ProfessionInfo("loafer", false, SkillLevel.None, SkillLevel.None, SkillLevel.None);

        public static readonly ProfessionInfo WorkingSlave = new ProfessionInfo("slave", false, Caste.Low, SkillLevel.Bad, SkillLevel.None, SkillLevel.None);
        public static readonly ProfessionInfo PleasureSlave = new ProfessionInfo("slave", false, Caste.Low, SkillLevel.None, SkillLevel.None, SkillLevel.Bad);
        public static readonly ProfessionInfo Slaver = new ProfessionInfo("slaver", true, Caste.MiddleOrUp, SkillLevel.None, SkillLevel.Good, SkillLevel.None);

        public static readonly ProfessionInfo Warrior = new ProfessionInfo("warrior", false, SkillLevel.Good, SkillLevel.None, SkillLevel.None);
        public static readonly ProfessionInfo Raider = new ProfessionInfo("raider", false, Caste.Outlaw, SkillLevel.Good, SkillLevel.None, SkillLevel.None);

        public static readonly ProfessionInfo Guard = new ProfessionInfo("guard", false, SkillLevel.Good, SkillLevel.None, SkillLevel.None);
        public static readonly ProfessionInfo Watcher = new ProfessionInfo("watcher", false, SkillLevel.Good, SkillLevel.None, SkillLevel.None);
        public static readonly ProfessionInfo Policeman = new ProfessionInfo("policeman", false, SkillLevel.Good, SkillLevel.None, SkillLevel.None);

        public static readonly ProfessionInfo General = new ProfessionInfo("general", false, Caste.Elite, SkillLevel.Good, SkillLevel.Good, SkillLevel.None);
        public static readonly ProfessionInfo Officer = new ProfessionInfo("officer", true, Caste.MiddleOrUp, SkillLevel.Good, SkillLevel.Good, SkillLevel.None);
        public static readonly ProfessionInfo Soldier = new ProfessionInfo("soldier", false, SkillLevel.Good, SkillLevel.None, SkillLevel.None);

        public static readonly ProfessionInfo Fisher = new ProfessionInfo("fisher", false, SkillLevel.Bad, SkillLevel.None, SkillLevel.None);

        public static readonly ProfessionInfo Farmer = new ProfessionInfo("farmer", false, SkillLevel.None, SkillLevel.None, SkillLevel.None);

        public static readonly ProfessionInfo Peasant = new ProfessionInfo("peasant", false, SkillLevel.None, SkillLevel.None, SkillLevel.None);

        public static readonly ProfessionInfo Hunter = new ProfessionInfo("hunter", false, SkillLevel.Bad, SkillLevel.None, SkillLevel.None);

        public static readonly ProfessionInfo Miner = new ProfessionInfo("miner", false, SkillLevel.Bad, SkillLevel.None, SkillLevel.None);

        public static readonly ProfessionInfo Lumberjack = new ProfessionInfo("lumberjack", false, SkillLevel.Bad, SkillLevel.None, SkillLevel.None);

        public static readonly ProfessionInfo Shaman = new ProfessionInfo("shaman", false, Caste.MiddleOrUp, SkillLevel.None, SkillLevel.Good, SkillLevel.None);

        public static readonly ProfessionInfo Priest = new ProfessionInfo("priest", "priestess", false, null, SkillLevel.None, SkillLevel.Good, SkillLevel.None);

        public static readonly ProfessionInfo Barman = new ProfessionInfo("barman", "barmaid", false, null, SkillLevel.None, SkillLevel.Bad, SkillLevel.None);

        public static readonly ProfessionInfo Rogue = new ProfessionInfo("rogue", false, Caste.Outlaw, SkillLevel.Good, SkillLevel.Bad, SkillLevel.None);
        public static readonly ProfessionInfo Pirate = new ProfessionInfo("pirate", false, Caste.Outlaw, SkillLevel.Good, SkillLevel.Bad, SkillLevel.None);
        public static readonly ProfessionInfo Bandit = new ProfessionInfo("bandit", false, Caste.Outlaw, SkillLevel.Good, SkillLevel.Bad, SkillLevel.None);

        public static readonly ProfessionInfo RogueLeader = new ProfessionInfo("rogue leader", true, Caste.Outlaw, SkillLevel.Good, SkillLevel.Good, SkillLevel.None);
        public static readonly ProfessionInfo PirateLeader = new ProfessionInfo("pirate captain", true, Caste.Outlaw, SkillLevel.Good, SkillLevel.Good, SkillLevel.None);
        public static readonly ProfessionInfo BanditLeader = new ProfessionInfo("bandit leader", true, Caste.Outlaw, SkillLevel.Good, SkillLevel.Good, SkillLevel.None);

        public static readonly ProfessionInfo Gambler = new ProfessionInfo("gambler", false, SkillLevel.None, SkillLevel.Good, SkillLevel.None);

        public static readonly ProfessionInfo Prostitute = new ProfessionInfo("prostitute", false, SkillLevel.Bad, SkillLevel.None, SkillLevel.Good);
        public static readonly ProfessionInfo Courtesan = new ProfessionInfo("gigolo", "courtesan", true, Caste.MiddleOrUp, SkillLevel.None, SkillLevel.Good, SkillLevel.Excellent);

        public static readonly ProfessionInfo Stripper = new ProfessionInfo("striper", false, Caste.Low, SkillLevel.Bad, SkillLevel.None, SkillLevel.Good);

        public static readonly ProfessionInfo Musician = new ProfessionInfo("musician", false, SkillLevel.None, SkillLevel.Bad, SkillLevel.None);
        public static readonly ProfessionInfo Composer = new ProfessionInfo("composer", true, SkillLevel.None, SkillLevel.Good, SkillLevel.None);

        public static readonly ProfessionInfo Actor = new ProfessionInfo("actor", "actress", false, null, SkillLevel.None, SkillLevel.Bad, SkillLevel.Bad);
        public static readonly ProfessionInfo Playwright = new ProfessionInfo("playwright", true, Caste.MiddleOrUp, SkillLevel.None, SkillLevel.Good, SkillLevel.None);
        public static readonly ProfessionInfo Producer = new ProfessionInfo("producer", true, Caste.MiddleOrUp, SkillLevel.None, SkillLevel.Good, SkillLevel.None);

        public static readonly ProfessionInfo Gladiator = new ProfessionInfo("gladiator", false, Caste.Low, SkillLevel.Good, SkillLevel.None, SkillLevel.None);

        public static readonly ProfessionInfo Sailor = new ProfessionInfo("sailor", false, SkillLevel.Bad, SkillLevel.None, SkillLevel.None);
        public static readonly ProfessionInfo Captain = new ProfessionInfo("captain", true, Caste.MiddleOrUp, SkillLevel.Bad, SkillLevel.Bad, SkillLevel.None);
        public static readonly ProfessionInfo Admiral = new ProfessionInfo("admiral", true, Caste.Elite, SkillLevel.Bad, SkillLevel.Good, SkillLevel.None);

        public static readonly ProfessionInfo Tailor = new ProfessionInfo("tailor", false, SkillLevel.None, SkillLevel.Bad, SkillLevel.None);

        public static readonly ProfessionInfo Jeveller = new ProfessionInfo("jeveller", false, SkillLevel.Bad, SkillLevel.Bad, SkillLevel.None);

        public static readonly ProfessionInfo Blacksmith = new ProfessionInfo("blacksmith", false, SkillLevel.Bad, SkillLevel.None, SkillLevel.None);

        public static readonly ProfessionInfo Carpenter = new ProfessionInfo("carpenter", false, SkillLevel.Bad, SkillLevel.None, SkillLevel.None);

        public static readonly ProfessionInfo Worker = new ProfessionInfo("worker", false, SkillLevel.Bad, SkillLevel.None, SkillLevel.None);
        public static readonly ProfessionInfo Master = new ProfessionInfo("master", true, SkillLevel.None, SkillLevel.Good, SkillLevel.None);

        public static readonly ProfessionInfo Scientiest = new ProfessionInfo("scientiest", false, SkillLevel.None, SkillLevel.Excellent, SkillLevel.None);
        public static readonly ProfessionInfo Chemist = new ProfessionInfo("chemist", false, SkillLevel.None, SkillLevel.Excellent, SkillLevel.None);
        public static readonly ProfessionInfo Doctor = new ProfessionInfo("doctor", false, SkillLevel.None, SkillLevel.Good, SkillLevel.None);
        public static readonly ProfessionInfo Nurce = new ProfessionInfo("infirmier", "nurse", false, null, SkillLevel.None, SkillLevel.Bad, SkillLevel.None);
        public static readonly ProfessionInfo Alchemist = new ProfessionInfo("alchemist", false, SkillLevel.None, SkillLevel.Good, SkillLevel.None);
        public static readonly ProfessionInfo Mage = new ProfessionInfo("mage", false, SkillLevel.None, SkillLevel.Good, SkillLevel.None);

        public static readonly ProfessionInfo Elder = new ProfessionInfo("elder", true, SkillLevel.None, SkillLevel.Bad, SkillLevel.Bad);
        public static readonly ProfessionInfo Mayor = new ProfessionInfo("mayor", true, SkillLevel.None, SkillLevel.Good, SkillLevel.Good);
        
        public static readonly ProfessionInfo Scribe = new ProfessionInfo("scribe", false, SkillLevel.None, SkillLevel.Bad, SkillLevel.None);
        public static readonly ProfessionInfo Administrator = new ProfessionInfo("administrator", true, SkillLevel.None, SkillLevel.Good, SkillLevel.None);
        public static readonly ProfessionInfo Clerk = new ProfessionInfo("clerk", false, SkillLevel.None, SkillLevel.Bad, SkillLevel.None);
        public static readonly ProfessionInfo Manager = new ProfessionInfo("manager", true, SkillLevel.None, SkillLevel.Good, SkillLevel.None);

        public static readonly ProfessionInfo Teacher = new ProfessionInfo("teacher", false, SkillLevel.None, SkillLevel.Good, SkillLevel.None);

        public static readonly ProfessionInfo Innkeeper = new ProfessionInfo("innkeeper", false, SkillLevel.None, SkillLevel.Bad, SkillLevel.None);

        public static readonly ProfessionInfo Merchant = new ProfessionInfo("merchant", false, SkillLevel.None, SkillLevel.Good, SkillLevel.None);

        public static readonly ProfessionInfo Noble = new ProfessionInfo("noble", true, Caste.Elite, SkillLevel.Bad, SkillLevel.Bad, SkillLevel.Bad);
        public static readonly ProfessionInfo Servant = new ProfessionInfo("servant", false, SkillLevel.None, SkillLevel.None, SkillLevel.None);

        #endregion

        #region State & City Rulers
        /// <summary>
        /// "Patriarch", "Matriarch"
        /// </summary>
        public static readonly ProfessionInfo RulerPrimitive = new ProfessionInfo("Chieftain", "Chieftess", true, Caste.Elite);
        /// <summary>
        /// "Warlord", "Princess"
        /// </summary>
        public static readonly ProfessionInfo HeirPrimitive = new ProfessionInfo("Chief's Heir", "Chief's Heiress", false, Caste.Elite);
        /// <summary>
        /// "Elder"
        /// </summary>
        public static readonly ProfessionInfo GovernorPrimitive = new ProfessionInfo("Headman", true, Caste.Elite);

        /// <summary>
        /// "Kaiser", "Kaiserin"
        /// </summary>
        public static readonly ProfessionInfo EmperorNorth = new ProfessionInfo("Kaiser", "Kaiserin", true, Caste.Elite);
        /// <summary>
        /// "Kronprins", "Kronprinzessin"
        /// </summary>
        public static readonly ProfessionInfo EmperorHeirNorth = new ProfessionInfo("Kronprinz", "Kronprinzessin", false, Caste.Elite);
        /// <summary>
        /// "Konung", "Konigin"
        /// </summary>
        public static readonly ProfessionInfo KingNorth = new ProfessionInfo("Konung", "Konigin", true, Caste.Elite);
        /// <summary>
        /// "Prinz", "Prinzessin"
        /// </summary>
        public static readonly ProfessionInfo KingHeirNorth = new ProfessionInfo("Prinz", "Prinzessin", false, Caste.Elite);
        /// <summary>
        /// "Jarl", "Jarlin"
        /// </summary>
        public static readonly ProfessionInfo GovernorNorth = new ProfessionInfo("Jarl", "Jarlin", true, Caste.Elite);
        /// <summary>
        /// "Hertug", "Hertugin"
        /// </summary>
        public static readonly ProfessionInfo GovernorNorth2 = new ProfessionInfo("Hertug", "Hertugin", true, Caste.Elite);

        /// <summary>
        /// "Emperor", "Empress"
        /// </summary>
        public static readonly ProfessionInfo EmperorEuro = new ProfessionInfo("Emperor", "Empress", true, Caste.Elite);
        /// <summary>
        /// "King", "Queen"
        /// </summary>
        public static readonly ProfessionInfo KingEuro = new ProfessionInfo("King", "Queen", true, Caste.Elite);
        /// <summary>
        /// "Prince", "Princess"
        /// </summary>
        public static readonly ProfessionInfo KingHeirEuro = new ProfessionInfo("Prince", "Princess", false, Caste.Elite);
        /// <summary>
        /// "Baron", "Baroness"
        /// </summary>
        public static readonly ProfessionInfo GovernorEuro = new ProfessionInfo("Baron", "Baroness", true, Caste.Elite);
        /// <summary>
        /// "Count", "Countess"
        /// </summary>
        public static readonly ProfessionInfo GovernorEuro2 = new ProfessionInfo("Count", "Countess", true, Caste.Elite);
        /// <summary>
        /// "Duke", "Duchess"
        /// </summary>
        public static readonly ProfessionInfo GovernorEuro3 = new ProfessionInfo("Duke", "Duchess", true, Caste.Elite);
        /// <summary>
        /// "Lord", "Lady"
        /// </summary>
        public static readonly ProfessionInfo GovernorEuro4 = new ProfessionInfo("Lord", "Lady", true, Caste.Elite);

        /// <summary>
        /// "Imperator", "Imperatrix"
        /// </summary>
        public static readonly ProfessionInfo EmperorLatin = new ProfessionInfo("Imperator", "Imperatrix", true, Caste.Elite);
        /// <summary>
        /// "Rex", "Regina"
        /// </summary>
        public static readonly ProfessionInfo KingLatin = new ProfessionInfo("Rex", "Regina", true, Caste.Elite);
        /// <summary>
        /// "Princeps", "Principissa"
        /// </summary>
        public static readonly ProfessionInfo KingHeirLatin = new ProfessionInfo("Princeps", "Principissa", false, Caste.Elite);
        /// <summary>
        /// "Dux", "Ducissam"
        /// </summary>
        public static readonly ProfessionInfo GovernorLatin = new ProfessionInfo("Dux", "Ducissam", true, Caste.Elite);

        /// <summary>
        /// "Tenno", "Chugu"
        /// </summary>
        public static readonly ProfessionInfo EmperorAsian = new ProfessionInfo("Tenno", "Chugu", true, Caste.Elite);
        /// <summary>
        /// "Shinno", "Naishinno"
        /// </summary>
        public static readonly ProfessionInfo EmperorHeirAsian = new ProfessionInfo("Shinno", "Naishinno", false, Caste.Elite);
        /// <summary>
        /// "Shogun", "Midaidokoro"
        /// </summary>
        public static readonly ProfessionInfo KingAsian = new ProfessionInfo("Shogun", "Midaidokoro", true, Caste.Elite);
        /// <summary>
        /// "Prince", "Hime"
        /// </summary>
        public static readonly ProfessionInfo KingHeirAsian = new ProfessionInfo("Prince", "Hime", false, Caste.Elite);
        /// <summary>
        /// "Gensui", "Gensui"
        /// </summary>
        public static readonly ProfessionInfo GovernorAsian = new ProfessionInfo("Gensui", "Gensui", true, Caste.Elite);
        /// <summary>
        /// "Daimyo", "Lady"
        /// </summary>
        public static readonly ProfessionInfo GovernorAsian2 = new ProfessionInfo("Daimyo", "Lady", true, Caste.Elite);

        /// <summary>
        /// "Caliph", "Calipha"
        /// </summary>
        public static readonly ProfessionInfo EmperorArabian = new ProfessionInfo("Caliph", "Calipha", true, Caste.Elite);
        /// <summary>
        /// "Shah", "Shahbanu"
        /// </summary>
        public static readonly ProfessionInfo KingArabian = new ProfessionInfo("Shah", "Shahbanu", true, Caste.Elite);
        /// <summary>
        /// "Shahzada", "Shahdokht"
        /// </summary>
        public static readonly ProfessionInfo KingHeirArabian = new ProfessionInfo("Shahzade", "Hanimshah", false, Caste.Elite);
        /// <summary>
        /// "Sultan", "Sultana"
        /// </summary>
        public static readonly ProfessionInfo KingArabian2 = new ProfessionInfo("Sultan", "Sultana", true, Caste.Elite);
        /// <summary>
        /// "Sultanzada", "Hanimsultan"
        /// </summary>
        public static readonly ProfessionInfo KingHeirArabian2 = new ProfessionInfo("Sultanzade", "Hanimsultan", false, Caste.Elite);
        /// <summary>
        /// "Emir", "Emira"
        /// </summary>
        public static readonly ProfessionInfo GovernorArabian = new ProfessionInfo("Emir", "Emira", true, Caste.Elite);
        /// <summary>
        /// "Sheikh", "Sheikha"
        /// </summary>
        public static readonly ProfessionInfo GovernorArabian2 = new ProfessionInfo("Sheikh", "Sheikha", true, Caste.Elite);

        /// <summary>
        /// "Khagan", "Great Khatun"
        /// </summary>
        public static readonly ProfessionInfo EmperorMongol = new ProfessionInfo("Khagan", "Great Khatun", true, Caste.Elite);
        /// <summary>
        /// "Khan", "Khatun"
        /// </summary>
        public static readonly ProfessionInfo KingMongol = new ProfessionInfo("Khan", "Khatun", true, Caste.Elite);
        /// <summary>
        /// "Khanzada", "Khandokht"
        /// </summary>
        public static readonly ProfessionInfo KingHeirMongol = new ProfessionInfo("Khanzada", "Khandokht", false, Caste.Elite);
        /// <summary>
        /// "Bey", "Bey"
        /// </summary>
        public static readonly ProfessionInfo GovernorMongol = new ProfessionInfo("Bey", "Bey", true, Caste.Elite);

        /// <summary>
        /// "Tsar", "Tsaritsa"
        /// </summary>
        public static readonly ProfessionInfo EmperorSlavic = new ProfessionInfo("Tsar", "Tsaritsa", true, Caste.Elite);
        /// <summary>
        /// "Tsarevich", "Tsarevna"
        /// </summary>
        public static readonly ProfessionInfo EmperorHeirSlavic = new ProfessionInfo("Tsarevich", "Tsarevna", false, Caste.Elite);
        /// <summary>
        /// "Knyaz", "Kniagina"
        /// </summary>
        public static readonly ProfessionInfo KingSlavic = new ProfessionInfo("Knyaz", "Kniagina", true, Caste.Elite);
        /// <summary>
        /// "Knyazich", "Knyazna"
        /// </summary>
        public static readonly ProfessionInfo KingHeirSlavic = new ProfessionInfo("Knyazich", "Knyazna", false, Caste.Elite);
        /// <summary>
        /// "Boyar", "Boyarynia"
        /// </summary>
        public static readonly ProfessionInfo GovernorSlavic = new ProfessionInfo("Boyar", "Boyarynia", true, Caste.Elite);

        /// <summary>
        /// "Autokrator", "Autokrateira"
        /// </summary>
        public static readonly ProfessionInfo EmperorGreek = new ProfessionInfo("Autokrator", "Autokrateira", true, Caste.Elite);
        /// <summary>
        /// "Basileus", "Basilissa"
        /// </summary>
        public static readonly ProfessionInfo KingGreek = new ProfessionInfo("Basileus", "Basilissa", true, Caste.Elite);
        /// <summary>
        /// "Prinkipas", "Prinkipissa"
        /// </summary>
        public static readonly ProfessionInfo KingHeirGreek = new ProfessionInfo("Prinkipas", "Prinkipissa", false, Caste.Elite);
        /// <summary>
        /// "Hypatos", "Hypatissa"
        /// </summary>
        public static readonly ProfessionInfo GovernorGreek = new ProfessionInfo("Hypatos", "Hypatissa", true, Caste.Elite);
        /// <summary>
        /// "Anthypatos", "Anthypatissa"
        /// </summary>
        public static readonly ProfessionInfo GovernorGreek2 = new ProfessionInfo("Anthypatos", "Anthypatissa", true, Caste.Elite);

        /// <summary>
        /// "Maharaja", "Maharani"
        /// </summary>
        public static readonly ProfessionInfo EmperorHindu = new ProfessionInfo("Maharaja", "Maharani", true, Caste.Elite);
        /// <summary>
        /// "Maharajkumar", "Maharajkumari"
        /// </summary>
        public static readonly ProfessionInfo EmperorHeirHindu = new ProfessionInfo("Maharajkumar", "Maharajkumari", false, Caste.Elite);
        /// <summary>
        /// "Raja", "Rani"
        /// </summary>
        public static readonly ProfessionInfo KingHindu = new ProfessionInfo("Raja", "Rani", true, Caste.Elite);
        /// <summary>
        /// "Rajkumar", "Rajkumari"
        /// </summary>
        public static readonly ProfessionInfo KingHeirHindu = new ProfessionInfo("Rajkumar", "Rajkumari", false, Caste.Elite);
        /// <summary>
        /// "Thakur", "Thakurani"
        /// </summary>
        public static readonly ProfessionInfo GovernorHindu = new ProfessionInfo("Thakur", "Thakurani", true, Caste.Elite);

        /// <summary>
        /// "Tlatoani", "Cihuatlatoani"
        /// </summary>
        public static readonly ProfessionInfo KingAztec = new ProfessionInfo("Tlatoani", "Cihuatlatoani", true, Caste.Elite);
        /// <summary>
        /// "Tlatocapilli", "Cihuapilli"
        /// </summary>
        public static readonly ProfessionInfo KingHeirAztec = new ProfessionInfo("Tlatocapilli", "Cihuapilli", false, Caste.Elite);
        /// <summary>
        /// "Teuctli", "Cihuatecutli"
        /// </summary>
        public static readonly ProfessionInfo GovernorAztec = new ProfessionInfo("Teuctli", "Cihuatecutli", true, Caste.Elite);

        /// <summary>
        /// "President", "President"
        /// </summary>
        public static readonly ProfessionInfo RulerModern = new ProfessionInfo("President", "President", true, Caste.Elite);
        /// <summary>
        /// "Chairman", "Chairman"
        /// </summary>
        public static readonly ProfessionInfo RulerModern2 = new ProfessionInfo("Chairman", "Chairman", true, Caste.Elite);
        /// <summary>
        /// "Speaker", "Speaker"
        /// </summary>
        public static readonly ProfessionInfo RulerModern3 = new ProfessionInfo("Speaker", "Speaker", true, Caste.Elite);
        /// <summary>
        /// "Ruler", "Ruler"
        /// </summary>
        public static readonly ProfessionInfo RulerModern4 = new ProfessionInfo("Ruler", "Ruler", true, Caste.Elite);
        /// <summary>
        /// "Minister", "Ministress"
        /// </summary>
        public static readonly ProfessionInfo AdvisorModern = new ProfessionInfo("Minister", "Ministress", true, Caste.Elite);
        /// <summary>
        /// "Deputy", "Deputy"
        /// </summary>
        public static readonly ProfessionInfo AdvisorModern2 = new ProfessionInfo("Deputy", "Deputy", true, Caste.Elite);
        /// <summary>
        /// "Counsellor", "Counsellor"
        /// </summary>
        public static readonly ProfessionInfo AdvisorModern3 = new ProfessionInfo("Counsellor", "Counsellor", true, Caste.Elite);
        /// <summary>
        /// "Senator", "Senator"
        /// </summary>
        public static readonly ProfessionInfo AdvisorModern4 = new ProfessionInfo("Senator", "Senator", true, Caste.Elite);
        /// <summary>
        /// "Advisor", "Advisor"
        /// </summary>
        public static readonly ProfessionInfo AdvisorModern5 = new ProfessionInfo("Advisor", "Advisor", true, Caste.Elite);
        /// <summary>
        /// "Governor", "Governor"
        /// </summary>
        public static readonly ProfessionInfo GovernorModern = new ProfessionInfo("Governor", "Governor", true, Caste.Elite);
        /// <summary>
        /// "Mayor", "Mayor"
        /// </summary>
        public static readonly ProfessionInfo GovernorModern2 = new ProfessionInfo("Mayor", "Mayor", true, Caste.Elite);
        /// <summary>
        /// "Manager", "Manager"
        /// </summary>
        public static readonly ProfessionInfo GovernorModern3 = new ProfessionInfo("Manager", "Manager", true, Caste.Elite);

        #endregion

        public string m_sNameM;
        public string m_sNameF;

        /// <summary>
        /// Является ли эта профессия руководящей или подчинённой?
        /// В патриархальном обществе руководитель/начальник/хозяин может быть только мужчиной, а вот подчинёнными могут быть и женщины...
        /// </summary>
        public bool m_bMaster;
        /// <summary>
        /// Если значение задано, то член этой профессии может принадлежать только к указанной касте
        /// </summary>
        public Caste? m_eCasteRestriction = null;

        /// <summary>
        /// Минимальный требуемый уровеь скилла для этой профессии
        /// </summary>
        public Dictionary<Person.Skill, SkillLevel> m_cSkills = new Dictionary<Person.Skill, SkillLevel>();

        public ProfessionInfo(string sName, bool bMaster, SkillLevel eBody, SkillLevel eMind, SkillLevel eCharm)
            : this(sName, sName, bMaster, null, eBody, eMind, eCharm)
        {
        }

        public ProfessionInfo(string sName, bool bMaster)
            : this(sName, sName, bMaster, null)
        {
        }

        public ProfessionInfo(string sName, bool bMaster, Caste? eRestriction, SkillLevel eBody, SkillLevel eMind, SkillLevel eCharm)
            : this(sName, sName, bMaster, eRestriction, eBody, eMind, eCharm)
        {
        }

        public ProfessionInfo(string sName, bool bMaster, Caste? eRestriction)
            : this(sName, sName, bMaster, eRestriction)
        {
        }

        public ProfessionInfo(string sNameM, string sNameF, bool bMaster, Caste? eRestriction, SkillLevel eBody, SkillLevel eMind, SkillLevel eCharm)
            : this(sNameM, sNameF, bMaster, eRestriction)
        {
            m_cSkills[Person.Skill.Body] = eBody;
            m_cSkills[Person.Skill.Mind] = eMind;
            m_cSkills[Person.Skill.Charm] = eCharm;
        }

        public ProfessionInfo(string sNameM, string sNameF, bool bMaster, Caste? eRestriction)
        {
            foreach (Person.Skill eSkill in Enum.GetValues(typeof(Person.Skill)))
                m_cSkills[eSkill] = SkillLevel.Good;

            m_sNameM = sNameM;
            m_sNameF = sNameF;
            m_eCasteRestriction = eRestriction;
            m_bMaster = bMaster;

            s_cAllProfessions.Add(this);
        }
    }
}
