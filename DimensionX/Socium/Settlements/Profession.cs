using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socium.Settlements
{
    public class ProfessionInfo
    {
        public static readonly ProfessionInfo Loafer = new ProfessionInfo("loafer", true);

        public static readonly ProfessionInfo Slave = new ProfessionInfo("slave", true);
        public static readonly ProfessionInfo Slaver = new ProfessionInfo("slaver", true);

        public static readonly ProfessionInfo Warrior = new ProfessionInfo("warrior", false);

        public static readonly ProfessionInfo Guard = new ProfessionInfo("guard", false);
        public static readonly ProfessionInfo Watcher = new ProfessionInfo("watcher", false);
        public static readonly ProfessionInfo Policeman = new ProfessionInfo("policeman", false);

        public static readonly ProfessionInfo General = new ProfessionInfo("general", true, false);
        public static readonly ProfessionInfo Soldier = new ProfessionInfo("soldier", false);

        public static readonly ProfessionInfo Fisher = new ProfessionInfo("fisher", false);

        public static readonly ProfessionInfo Farmer = new ProfessionInfo("farmer", false);

        public static readonly ProfessionInfo Peasant = new ProfessionInfo("peasant", false);

        public static readonly ProfessionInfo Hunter = new ProfessionInfo("hunter", false);

        public static readonly ProfessionInfo Miner = new ProfessionInfo("miner", false);

        public static readonly ProfessionInfo Lumberjack = new ProfessionInfo("lumberjack", false);

        public static readonly ProfessionInfo Shaman = new ProfessionInfo("shaman", true);

        public static readonly ProfessionInfo Priest = new ProfessionInfo("priest", true);

        public static readonly ProfessionInfo Barman = new ProfessionInfo("barman", "barmaid", false, true);

        public static readonly ProfessionInfo Rogue = new ProfessionInfo("rogue", true);
        public static readonly ProfessionInfo Pirate = new ProfessionInfo("pirate", true);
        public static readonly ProfessionInfo Bandit = new ProfessionInfo("bandit", true);

        public static readonly ProfessionInfo Gambler = new ProfessionInfo("gambler", true);

        public static readonly ProfessionInfo Prostitute = new ProfessionInfo("prostitute", true);

        public static readonly ProfessionInfo Stripper = new ProfessionInfo("striper", true);

        public static readonly ProfessionInfo Musician = new ProfessionInfo("musician", true);

        public static readonly ProfessionInfo Actor = new ProfessionInfo("actor", "actress", false, true);

        public static readonly ProfessionInfo Gladiator = new ProfessionInfo("gladiator", true);

        public static readonly ProfessionInfo Sailor = new ProfessionInfo("sailor", false);

        public static readonly ProfessionInfo Tailor = new ProfessionInfo("tailor", false);

        public static readonly ProfessionInfo Jeveller = new ProfessionInfo("jeveller", false);

        public static readonly ProfessionInfo Blacksmith = new ProfessionInfo("blacksmith", false);

        public static readonly ProfessionInfo Carpenter = new ProfessionInfo("carpenter", false);

        public static readonly ProfessionInfo Worker = new ProfessionInfo("worker", false);

        public static readonly ProfessionInfo Scientiest = new ProfessionInfo("scientiest", true);
        public static readonly ProfessionInfo Chemist = new ProfessionInfo("chemist", true);
        public static readonly ProfessionInfo Doctor = new ProfessionInfo("doctor", true);
        public static readonly ProfessionInfo Alchemist = new ProfessionInfo("alchemist", true);
        public static readonly ProfessionInfo Mage = new ProfessionInfo("Mage", true);

        public static readonly ProfessionInfo Elder = new ProfessionInfo("elder", true, false);
        public static readonly ProfessionInfo Mayor = new ProfessionInfo("mayor", true, false);
        public static readonly ProfessionInfo Scribe = new ProfessionInfo("scribe", false);
        public static readonly ProfessionInfo Clerk = new ProfessionInfo("clerk", false);

        public static readonly ProfessionInfo Teacher = new ProfessionInfo("teacher", true);

        public static readonly ProfessionInfo Innkeeper = new ProfessionInfo("innkeeper", false);

        public static readonly ProfessionInfo Merchant = new ProfessionInfo("merchant", false);

        public static readonly ProfessionInfo Noble = new ProfessionInfo("noble", false);

        /// <summary>
        /// "Patriarch", "Matriarch"
        /// </summary>
        public static readonly ProfessionInfo RulerPrimitive = new ProfessionInfo("Patriarch", "Matriarch", true, false);
        /// <summary>
        /// "Warlord", "Princess"
        /// </summary>
        public static readonly ProfessionInfo HeirPrimitive = new ProfessionInfo("Warlord", "Princess", true, false);
        /// <summary>
        /// "Elder"
        /// </summary>
        public static readonly ProfessionInfo GovernorPrimitive = new ProfessionInfo("Elder", true, false);

        /// <summary>
        /// "Kaiser", "Kaiserin"
        /// </summary>
        public static readonly ProfessionInfo EmperorNorth = new ProfessionInfo("Kaiser", "Kaiserin", true, false);
        /// <summary>
        /// "Kronprins", "Kronprinzessin"
        /// </summary>
        public static readonly ProfessionInfo EmperorHeirNorth = new ProfessionInfo("Kronprinz", "Kronprinzessin", true, false);
        /// <summary>
        /// "Konung", "Konigin"
        /// </summary>
        public static readonly ProfessionInfo KingNorth = new ProfessionInfo("Konung", "Konigin", true, false);
        /// <summary>
        /// "Prinz", "Prinzessin"
        /// </summary>
        public static readonly ProfessionInfo KingHeirNorth = new ProfessionInfo("Prinz", "Prinzessin", true, false);
        /// <summary>
        /// "Jarl", "Jarlin"
        /// </summary>
        public static readonly ProfessionInfo GovernorNorth = new ProfessionInfo("Jarl", "Jarlin", true, false);
        /// <summary>
        /// "Hertug", "Hertugin"
        /// </summary>
        public static readonly ProfessionInfo GovernorNorth2 = new ProfessionInfo("Hertug", "Hertugin", true, false);

        /// <summary>
        /// "Emperor", "Empress"
        /// </summary>
        public static readonly ProfessionInfo EmperorEuro = new ProfessionInfo("Emperor", "Empress", true, false);
        /// <summary>
        /// "King", "Queen"
        /// </summary>
        public static readonly ProfessionInfo KingEuro = new ProfessionInfo("King", "Queen", true, false);
        /// <summary>
        /// "Prince", "Princess"
        /// </summary>
        public static readonly ProfessionInfo KingHeirEuro = new ProfessionInfo("Prince", "Princess", true, false);
        /// <summary>
        /// "Baron", "Baroness"
        /// </summary>
        public static readonly ProfessionInfo GovernorEuro = new ProfessionInfo("Baron", "Baroness", true, false);
        /// <summary>
        /// "Count", "Countess"
        /// </summary>
        public static readonly ProfessionInfo GovernorEuro2 = new ProfessionInfo("Count", "Countess", true, false);
        /// <summary>
        /// "Duke", "Duchess"
        /// </summary>
        public static readonly ProfessionInfo GovernorEuro3 = new ProfessionInfo("Duke", "Duchess", true, false);

        /// <summary>
        /// "Imperator", "Imperatrix"
        /// </summary>
        public static readonly ProfessionInfo EmperorLatin = new ProfessionInfo("Imperator", "Imperatrix", true, false);
        /// <summary>
        /// "Rex", "Regina"
        /// </summary>
        public static readonly ProfessionInfo KingLatin = new ProfessionInfo("Rex", "Regina", true, false);
        /// <summary>
        /// "Princeps", "Principissa"
        /// </summary>
        public static readonly ProfessionInfo KingHeirLatin = new ProfessionInfo("Princeps", "Principissa", true, false);
        /// <summary>
        /// "Dux", "Ducissam"
        /// </summary>
        public static readonly ProfessionInfo GovernorLatin = new ProfessionInfo("Dux", "Ducissam", true, false);

        /// <summary>
        /// "Tenno", "Chugu"
        /// </summary>
        public static readonly ProfessionInfo EmperorAsian = new ProfessionInfo("Tenno", "Chugu", true, false);
        /// <summary>
        /// "Shinno", "Naishinno"
        /// </summary>
        public static readonly ProfessionInfo EmperorHeirAsian = new ProfessionInfo("Shinno", "Naishinno", true, false);
        /// <summary>
        /// "Shogun", "Midaidokoro"
        /// </summary>
        public static readonly ProfessionInfo KingAsian = new ProfessionInfo("Shogun", "Midaidokoro", true, false);
        /// <summary>
        /// "Prince", "Hime"
        /// </summary>
        public static readonly ProfessionInfo KingHeirAsian = new ProfessionInfo("Prince", "Hime", true, false);
        /// <summary>
        /// "Gensui", "Gensui"
        /// </summary>
        public static readonly ProfessionInfo GovernorAsian = new ProfessionInfo("Gensui", "Gensui", true, false);
        /// <summary>
        /// "Daimyo", "Lady"
        /// </summary>
        public static readonly ProfessionInfo GovernorAsian2 = new ProfessionInfo("Daimyo", "Lady", true, false);

        /// <summary>
        /// "Caliph", "Calipha"
        /// </summary>
        public static readonly ProfessionInfo EmperorArabian = new ProfessionInfo("Caliph", "Calipha", true, false);
        /// <summary>
        /// "Shah", "Shahbanu"
        /// </summary>
        public static readonly ProfessionInfo KingArabian = new ProfessionInfo("Shah", "Shahbanu", true, false);
        /// <summary>
        /// "Shahzada", "Shahdokht"
        /// </summary>
        public static readonly ProfessionInfo KingHeirArabian = new ProfessionInfo("Shahzade", "Hanimshah", true, false);
        /// <summary>
        /// "Sultan", "Sultana"
        /// </summary>
        public static readonly ProfessionInfo KingArabian2 = new ProfessionInfo("Sultan", "Sultana", true, false);
        /// <summary>
        /// "Sultanzada", "Hanimsultan"
        /// </summary>
        public static readonly ProfessionInfo KingHeirArabian2 = new ProfessionInfo("Sultanzade", "Hanimsultan", true, false);
        /// <summary>
        /// "Emir", "Emira"
        /// </summary>
        public static readonly ProfessionInfo GovernorArabian = new ProfessionInfo("Emir", "Emira", true, false);
        /// <summary>
        /// "Sheikh", "Sheikha"
        /// </summary>
        public static readonly ProfessionInfo GovernorArabian2 = new ProfessionInfo("Sheikh", "Sheikha", true, false);

        /// <summary>
        /// "Khagan", "Great Khatun"
        /// </summary>
        public static readonly ProfessionInfo EmperorMongol = new ProfessionInfo("Khagan", "Great Khatun", true, false);
        /// <summary>
        /// "Khan", "Khatun"
        /// </summary>
        public static readonly ProfessionInfo KingMongol = new ProfessionInfo("Khan", "Khatun", true, false);
        /// <summary>
        /// "Khanzada", "Khandokht"
        /// </summary>
        public static readonly ProfessionInfo KingHeirMongol = new ProfessionInfo("Khanzada", "Khandokht", true, false);
        /// <summary>
        /// "Bey", "Bey"
        /// </summary>
        public static readonly ProfessionInfo GovernorMongol = new ProfessionInfo("Bey", "Bey", true, false);

        /// <summary>
        /// "Tsar", "Tsaritsa"
        /// </summary>
        public static readonly ProfessionInfo EmperorSlavic = new ProfessionInfo("Tsar", "Tsaritsa", true, false);
        /// <summary>
        /// "Tsarevich", "Tsarevna"
        /// </summary>
        public static readonly ProfessionInfo EmperorHeirSlavic = new ProfessionInfo("Tsarevich", "Tsarevna", true, false);
        /// <summary>
        /// "Knyaz", "Kniagina"
        /// </summary>
        public static readonly ProfessionInfo KingSlavic = new ProfessionInfo("Knyaz", "Kniagina", true, false);
        /// <summary>
        /// "Knyazich", "Knyazna"
        /// </summary>
        public static readonly ProfessionInfo KingHeirSlavic = new ProfessionInfo("Knyazich", "Knyazna", true, false);
        /// <summary>
        /// "Boyar", "Boyarynia"
        /// </summary>
        public static readonly ProfessionInfo GovernorSlavic = new ProfessionInfo("Boyar", "Boyarynia", true, false);

        /// <summary>
        /// "Autokrator", "Autokrateira"
        /// </summary>
        public static readonly ProfessionInfo EmperorGreek = new ProfessionInfo("Autokrator", "Autokrateira", true, false);
        /// <summary>
        /// "Basileus", "Basilissa"
        /// </summary>
        public static readonly ProfessionInfo KingGreek = new ProfessionInfo("Basileus", "Basilissa", true, false);
        /// <summary>
        /// "Prinkipas", "Prinkipissa"
        /// </summary>
        public static readonly ProfessionInfo KingHeirGreek = new ProfessionInfo("Prinkipas", "Prinkipissa", true, false);
        /// <summary>
        /// "Hypatos", "Hypatissa"
        /// </summary>
        public static readonly ProfessionInfo GovernorGreek = new ProfessionInfo("Hypatos", "Hypatissa", true, false);
        /// <summary>
        /// "Anthypatos", "Anthypatissa"
        /// </summary>
        public static readonly ProfessionInfo GovernorGreek2 = new ProfessionInfo("Anthypatos", "Anthypatissa", true, false);

        /// <summary>
        /// "Maharaja", "Maharani"
        /// </summary>
        public static readonly ProfessionInfo EmperorHindu = new ProfessionInfo("Maharaja", "Maharani", true, false);
        /// <summary>
        /// "Maharajkumar", "Maharajkumari"
        /// </summary>
        public static readonly ProfessionInfo EmperorHeirHindu = new ProfessionInfo("Maharajkumar", "Maharajkumari", true, false);
        /// <summary>
        /// "Raja", "Rani"
        /// </summary>
        public static readonly ProfessionInfo KingHindu = new ProfessionInfo("Raja", "Rani", true, false);
        /// <summary>
        /// "Rajkumar", "Rajkumari"
        /// </summary>
        public static readonly ProfessionInfo KingHeirHindu = new ProfessionInfo("Rajkumar", "Rajkumari", true, false);
        /// <summary>
        /// "Thakur", "Thakurani"
        /// </summary>
        public static readonly ProfessionInfo GovernorHindu = new ProfessionInfo("Thakur", "Thakurani", true, false);

        /// <summary>
        /// "Tlatoani", "Cihuatlatoani"
        /// </summary>
        public static readonly ProfessionInfo KingAztec = new ProfessionInfo("Tlatoani", "Cihuatlatoani", true, false);
        /// <summary>
        /// "Tlatocapilli", "Cihuapilli"
        /// </summary>
        public static readonly ProfessionInfo KingHeirAztec = new ProfessionInfo("Tlatocapilli", "Cihuapilli", true, false);
        /// <summary>
        /// "Teuctli", "Cihuatecutli"
        /// </summary>
        public static readonly ProfessionInfo GovernorAztec = new ProfessionInfo("Teuctli", "Cihuatecutli", true, false);

        /// <summary>
        /// "President", "President"
        /// </summary>
        public static readonly ProfessionInfo RulerModern = new ProfessionInfo("President", "President", true, false);
        /// <summary>
        /// "Chairman", "Chairman"
        /// </summary>
        public static readonly ProfessionInfo RulerModern2 = new ProfessionInfo("Chairman", "Chairman", true, false);
        /// <summary>
        /// "Speaker", "Speaker"
        /// </summary>
        public static readonly ProfessionInfo RulerModern3 = new ProfessionInfo("Speaker", "Speaker", true, false);
        /// <summary>
        /// "Ruler", "Ruler"
        /// </summary>
        public static readonly ProfessionInfo RulerModern4 = new ProfessionInfo("Ruler", "Ruler", true, false);
        /// <summary>
        /// "Minister", "Ministress"
        /// </summary>
        public static readonly ProfessionInfo AdvisorModern = new ProfessionInfo("Minister", "Ministress", true, false);
        /// <summary>
        /// "Deputy", "Deputy"
        /// </summary>
        public static readonly ProfessionInfo AdvisorModern2 = new ProfessionInfo("Deputy", "Deputy", true, false);
        /// <summary>
        /// "Counsellor", "Counsellor"
        /// </summary>
        public static readonly ProfessionInfo AdvisorModern3 = new ProfessionInfo("Counsellor", "Counsellor", true, false);
        /// <summary>
        /// "Senator", "Senator"
        /// </summary>
        public static readonly ProfessionInfo AdvisorModern4 = new ProfessionInfo("Senator", "Senator", true, false);
        /// <summary>
        /// "Advisor", "Advisor"
        /// </summary>
        public static readonly ProfessionInfo AdvisorModern5 = new ProfessionInfo("Advisor", "Advisor", true, false);
        /// <summary>
        /// "Governor", "Governor"
        /// </summary>
        public static readonly ProfessionInfo GovernorModern = new ProfessionInfo("Governor", "Governor", true, false);
        /// <summary>
        /// "Mayor", "Mayor"
        /// </summary>
        public static readonly ProfessionInfo GovernorModern2 = new ProfessionInfo("Mayor", "Mayor", true, false);
        /// <summary>
        /// "Manager", "Manager"
        /// </summary>
        public static readonly ProfessionInfo GovernorModern3 = new ProfessionInfo("Manager", "Manager", true, false);

        public string m_sNameM;
        public string m_sNameF;

        public bool m_bOptional;
        public bool m_bLeading;

        public ProfessionInfo(string sName, bool bOptional)
            : this(sName, sName, false, bOptional)
        {
        }

        public ProfessionInfo(string sName, bool bLeading, bool bOptional)
            : this(sName, sName, bLeading, bOptional)
        {
        }

        public ProfessionInfo(string sNameM, string sNameF, bool bLeading, bool bOptional)
        {
            m_sNameM = sNameM;
            m_sNameF = sNameF;
            m_bLeading = bLeading;
        }
    }
}
