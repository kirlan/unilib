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

        public static readonly ProfessionInfo RulerPrimitive = new ProfessionInfo("Patriarch", "Matriarch", true, false);
        public static readonly ProfessionInfo HeirPrimitive = new ProfessionInfo("Warlord", "Princess", true, false);
        public static readonly ProfessionInfo GovernorPrimitive = new ProfessionInfo("Elder", true, false);

        public static readonly ProfessionInfo EmperorEuro = new ProfessionInfo("Emperor", "Empress", true, false);
        public static readonly ProfessionInfo KingEuro = new ProfessionInfo("King", "Queen", true, false);
        public static readonly ProfessionInfo KingHeirEuro = new ProfessionInfo("Prince", "Princess", true, false);
        public static readonly ProfessionInfo GovernorEuro = new ProfessionInfo("Baron", "Baroness", true, false);
        public static readonly ProfessionInfo GovernorEuro2 = new ProfessionInfo("Count", "Countess", true, false);
        public static readonly ProfessionInfo GovernorEuro3 = new ProfessionInfo("Duke", "Duchess", true, false);

        public static readonly ProfessionInfo EmperorLatin = new ProfessionInfo("Imperator", "Imperatrix", true, false);
        public static readonly ProfessionInfo KingLatin = new ProfessionInfo("Rex", "Regina", true, false);
        public static readonly ProfessionInfo KingHeirLatin = new ProfessionInfo("Princeps", "Principissa", true, false);
        public static readonly ProfessionInfo GovernorLatin = new ProfessionInfo("Dux", "Ducissam", true, false);

        public static readonly ProfessionInfo EmperorAsian = new ProfessionInfo("Tenno", "Chugu", true, false);
        public static readonly ProfessionInfo EmperorHeirAsian = new ProfessionInfo("Shinno", "Naishinno", true, false);
        public static readonly ProfessionInfo KingAsian = new ProfessionInfo("Shogun", "Midaidokoro", true, false);
        public static readonly ProfessionInfo KingHeirAsian = new ProfessionInfo("Prince", "Hime", true, false);
        public static readonly ProfessionInfo GovernorAsian = new ProfessionInfo("Gensui", "Gensui", true, false);
        public static readonly ProfessionInfo GovernorAsian2 = new ProfessionInfo("Daimyo", "Lady", true, false);

        public static readonly ProfessionInfo EmperorArabian = new ProfessionInfo("Padishah", "Padishahbanu", true, false);
        public static readonly ProfessionInfo KingArabian = new ProfessionInfo("Caliph", "Calipha", true, false);
        public static readonly ProfessionInfo KingArabian2 = new ProfessionInfo("Sultan", "Sultana", true, false);
        public static readonly ProfessionInfo GovernorArabian = new ProfessionInfo("Emir", "Emira", true, false);
        public static readonly ProfessionInfo GovernorArabian2 = new ProfessionInfo("Shah", "Shahbanu", true, false);

        public static readonly ProfessionInfo EmperorMongol = new ProfessionInfo("Khagan", "Great Khatun", true, false);
        public static readonly ProfessionInfo KingMongol = new ProfessionInfo("Khan", "Khatun", true, false);
        public static readonly ProfessionInfo GovernorMongol = new ProfessionInfo("Bey", "Hanım", true, false);

        public static readonly ProfessionInfo EmperorSlavic = new ProfessionInfo("Tsar", "Tsaritsa", true, false);
        public static readonly ProfessionInfo EmperorHeirSlavic = new ProfessionInfo("Tsarevich", "Tsarevna", true, false);
        public static readonly ProfessionInfo KingSlavic = new ProfessionInfo("Knyaz", "Kniagina", true, false);
        public static readonly ProfessionInfo KingHeirSlavic = new ProfessionInfo("Knyazich", "Knyazna", true, false);
        public static readonly ProfessionInfo GovernorSlavic = new ProfessionInfo("Boyar", "Boyarynia", true, false);

        public static readonly ProfessionInfo EmperorGreek = new ProfessionInfo("Autokrator", "Autokrateira", true, false);
        public static readonly ProfessionInfo KingGreek = new ProfessionInfo("Basileus", "Basilissa", true, false);
        public static readonly ProfessionInfo KingHeirGreek = new ProfessionInfo("Prinkipas", "Prinkipissa", true, false);
        public static readonly ProfessionInfo GovernorGreek = new ProfessionInfo("Hypatos", "Hypatissa", true, false);
        public static readonly ProfessionInfo GovernorGreek2 = new ProfessionInfo("Anthypatos", "Anthypatissa", true, false);

        public static readonly ProfessionInfo EmperorHindu = new ProfessionInfo("Maharaja", "Maharani", true, false);
        public static readonly ProfessionInfo EmperorHeirHindu = new ProfessionInfo("Maharajkumar", "Maharajkumari", true, false);
        public static readonly ProfessionInfo KingHindu = new ProfessionInfo("Raja", "Rani", true, false);
        public static readonly ProfessionInfo KingHeirHindu = new ProfessionInfo("Rajkumar", "Rajkumari", true, false);
        public static readonly ProfessionInfo GovernorHindu = new ProfessionInfo("Thakur", "Thakurani", true, false);

        public static readonly ProfessionInfo KingAztec = new ProfessionInfo("Tlatoani", "Cihuatlatoani", true, false);
        public static readonly ProfessionInfo KingHeirAztec = new ProfessionInfo("Tlatocapilli", "Cihuapilli", true, false);
        public static readonly ProfessionInfo GovernorAztec = new ProfessionInfo("Teuctli", "Cihuatecutli", true, false);

        public static readonly ProfessionInfo RulerModern = new ProfessionInfo("President", "President", true, false);
        public static readonly ProfessionInfo RulerModern2 = new ProfessionInfo("Chairman", "Chairman", true, false);
        public static readonly ProfessionInfo RulerModern3 = new ProfessionInfo("Speaker", "Speaker", true, false);
        public static readonly ProfessionInfo RulerModern4 = new ProfessionInfo("Ruler", "Ruler", true, false);
        public static readonly ProfessionInfo AdvisorModern = new ProfessionInfo("Minister", "Ministress", true, false);
        public static readonly ProfessionInfo AdvisorModern2 = new ProfessionInfo("Deputy", "Deputy", true, false);
        public static readonly ProfessionInfo AdvisorModern3 = new ProfessionInfo("Counsellor", "Counsellor", true, false);
        public static readonly ProfessionInfo AdvisorModern4 = new ProfessionInfo("Senator", "Senator", true, false);
        public static readonly ProfessionInfo AdvisorModern5 = new ProfessionInfo("Advisor", "Advisor", true, false);
        public static readonly ProfessionInfo GovernorModern = new ProfessionInfo("Governor", "Governor", true, false);
        public static readonly ProfessionInfo GovernorModern2 = new ProfessionInfo("Mayor", "Mayor", true, false);
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
