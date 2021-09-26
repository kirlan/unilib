using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socium
{
    public class SocialOrder
    {
        public Dictionary<Estate.Position, string[]> m_cEstates = new Dictionary<Estate.Position, string[]>();

        public static SocialOrder Primitive = new CSocialOrder(new string[] {"Firstborn"}, new string[] {"Tribesman"}, new string[] {"Servant"}, new string[] {"Outsider"});
        public static SocialOrder MedievalSlavic = new CSocialOrder(new string[] { "Dvoryan", "Kmet" }, new string[] { "Meshchan", "Smerd", "Batrak" }, new string[] { "Kholop" }, new string[] { "Izgoi" });
        public static SocialOrder MedievalEurope = new CSocialOrder(new string[] { "Knight", "Esquare" }, new string[] { "Commoner", "Freeholder", "Villein" }, new string[] { "Serf" }, new string[] { "Rogue" });
        public static SocialOrder MedievalEurope2 = new CSocialOrder(new string[] { "Peer", "Gentry" }, new string[] { "Burgher", "Yeoman", "Freeman" }, new string[] { "Serf" }, new string[] { "Outlaw" });
        public static SocialOrder MedievalEurope3 = new CSocialOrder(new string[] { "Aristocrat" }, new string[] { "Commoner", "Freeman" }, new string[] { "Serf" }, new string[] { "Outlaw" });
        public static SocialOrder MedievalAsian = new CSocialOrder(new string[] { "Fudai", "Samurai" }, new string[] { "Heimin", "Chonin", "Burakumin" }, new string[] { "Hinin" }, new string[] { "Ronin" });
        public static SocialOrder MedievalLatin = new CSocialOrder(new string[] {"Patricius", "Equitius"}, new string[] {"Nobilis", "Plebis", "Proletaris"}, new string[] {"Serv"}, new string[] {"Bagaudis"});
        public static SocialOrder MedievalNorth = new CSocialOrder(new string[] {"Earl", "Drohtin"}, new string[] {"Hauldr", "Bonde", "Huskarl"}, new string[] {"Thrall"}, new string[] {"Rogue"});
        public static SocialOrder MedievalArabian = new CSocialOrder(new string[] {"Sayyid", "Effendi"}, new string[] {"Nawab", "Khoja", "Yazat"}, new string[] {"Kul"}, new string[] {"Rogue"});
        public static SocialOrder MedievalMongol = new CSocialOrder(new string[] {"Noyon", "Darkhan"}, new string[] {"Batur", "Nukar", "Karach"}, new string[] {"Urtakch"}, new string[] {"Rogue"});
        public static SocialOrder MedievalGreek = new CSocialOrder(new string[] {"Ephor", "Diadoch"}, new string[] {"Demos", "Metic", "Penest"}, new string[] {"Helot"}, new string[] {"Rogue"});
        public static SocialOrder MedievalHindu = new CSocialOrder(new string[] {"Brahmin", "Kshatriy"}, new string[] {"Karav", "Kayasth", "Pataw"}, new string[] {"Shudra"}, new string[] {"Pancham"});
        public static SocialOrder MedievalAztec = new CSocialOrder(new string[] {"Pilli", "Pochtecatli"}, new string[] {"Macehualli", "Temilti", "Malli"}, new string[] {"Tlacotli"}, new string[] {"Panchama"});
        public static SocialOrder Modern = new CSocialOrder(new string[] { "Elite", "Oligarch" }, new string[] { "Specialist", "Bourgeois" }, new string[] { "Proletarian", "Lumpen" }, new string[] { "Gangster" });
        public static SocialOrder Future = new CSocialOrder(new string[] {"Citizen"}, new string[] {"Citizen"}, new string[] {"Morlock"}, new string[] {"Outsider"});

        public SocialOrder(string[] aElite, string[] aMiddle, string[] aLow, string[] aOutlaw)
        {
            m_cEstates[Estate.Position.Elite] = aElite;
            m_cEstates[Estate.Position.Middle] = aMiddle;
            m_cEstates[Estate.Position.Low] = aLow;
            m_cEstates[Estate.Position.Outlaw] = aOutlaw;

            if (m_cEstates[Cstate.Position.Elite] == null || m_cEstates[Estate.Position.Elite].Length == 0)
                m_cEstates[Estate.Position.Elite] = new string[] { "Elite" };

            if (m_cEstates[Estate.Position.Middle] == null || m_cEstates[Estate.Position.Middle].Length == 0)
                m_cEstates[Estate.Position.Middle] = new string[] { "Commoner" };

            if (m_cEstates[Estate.Position.Low] == null || m_cEstates[Estate.Position.Low].Length == 0)
                m_cEstates[Estate.Position.Low] = new string[] { "Servant" };

            if (m_cEstates[Estate.Position.Outlaw] == null || m_cEstates[Estate.Position.Outlaw].Length == 0)
                m_cEstates[Estate.Position.Outlaw] = new string[] { "Outlaw" };
        }
    }
}
