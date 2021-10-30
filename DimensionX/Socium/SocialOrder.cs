using Random;
using Socium.Population;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socium
{
    public class SocialOrder
    {
        public Dictionary<Estate.Position, string[]> m_cEstates = new Dictionary<Estate.Position, string[]>();

        public static SocialOrder Primitive = new SocialOrder(new string[] {"Firstborn"}, new string[] {"Seers"}, new string[] {"Tribesman"}, new string[] {"Servant"}, new string[] {"Outsider"});
        public static SocialOrder MedievalSlavic = new SocialOrder(new string[] { "Dvoryan", "Kmet" }, new string[] {"Volkhv"}, new string[] { "Meshchan", "Smerd", "Batrak" }, new string[] { "Kholop" }, new string[] { "Izgoi" });
        public static SocialOrder MedievalEurope = new SocialOrder(new string[] { "Knight", "Esquare" }, new string[] { "Cleric" }, new string[] { "Commoner", "Freeholder", "Villein" }, new string[] { "Serf" }, new string[] { "Rogue" });
        public static SocialOrder MedievalEurope2 = new SocialOrder(new string[] { "Peer", "Gentry" }, new string[] { "Cleric" }, new string[] { "Burgher", "Yeoman", "Freeman" }, new string[] { "Serf" }, new string[] { "Outlaw" });
        public static SocialOrder MedievalEurope3 = new SocialOrder(new string[] { "Aristocrat" }, new string[] { "Cleric" }, new string[] { "Commoner", "Freeman" }, new string[] { "Serf" }, new string[] { "Outlaw" });
        public static SocialOrder MedievalAsian = new SocialOrder(new string[] { "Fudai", "Samurai" }, new string[] { "Shinkan" }, new string[] { "Heimin", "Chonin", "Burakumin" }, new string[] { "Hinin" }, new string[] { "Ronin" });
        public static SocialOrder MedievalLatin = new SocialOrder(new string[] {"Patricius", "Equitius"}, new string[] { "Vestal" }, new string[] {"Nobilis", "Plebis", "Proletaris"}, new string[] {"Serv"}, new string[] {"Bagaudis"});
        public static SocialOrder MedievalNorth = new SocialOrder(new string[] {"Earl", "Drohtin"}, new string[] { "Druid" }, new string[] {"Hauldr", "Bonde", "Huskarl"}, new string[] {"Thrall"}, new string[] {"Rogue"});
        public static SocialOrder MedievalArabian = new SocialOrder(new string[] {"Sayyid", "Effendi"}, new string[] { "Mullah" }, new string[] {"Nawab", "Khoja", "Yazat"}, new string[] {"Kul"}, new string[] {"Rogue"});
        public static SocialOrder MedievalMongol = new SocialOrder(new string[] {"Noyon", "Darkhan"}, new string[] { "Bhikkhu" }, new string[] {"Batur", "Nukar", "Karach"}, new string[] {"Urtakch"}, new string[] {"Rogue"});
        public static SocialOrder MedievalGreek = new SocialOrder(new string[] {"Ephor", "Diadoch"}, new string[] { "Hierei" }, new string[] {"Demos", "Metic", "Penest"}, new string[] {"Helot"}, new string[] {"Rogue"});
        public static SocialOrder MedievalHindu = new SocialOrder(new string[] {"Kshatriy"}, new string[] { "Brahmin" }, new string[] {"Karav", "Kayasth", "Pataw"}, new string[] {"Shudra"}, new string[] {"Pancham"});
        public static SocialOrder MedievalAztec = new SocialOrder(new string[] {"Pilli", "Pochtecatli"}, new string[] { "Tlamacazqui" }, new string[] {"Macehualli", "Temilti", "Malli"}, new string[] {"Tlacotli"}, new string[] {"Panchama"});
        public static SocialOrder Modern = new SocialOrder(new string[] { "Elite", "Oligarch" }, new string[] { "Cleric" }, new string[] { "Specialist", "Bourgeois" }, new string[] { "Proletarian", "Lumpen" }, new string[] { "Gangster" });
        public static SocialOrder Future = new SocialOrder(new string[] {"Citizen"}, new string[] { "Citizen" }, new string[] {"Citizen"}, new string[] {"Morlock"}, new string[] {"Outsider"});

        public SocialOrder(string[] aElite, string[] aCleregy, string[] aMiddle, string[] aLow, string[] aOutlaw)
        {
            m_cEstates[Estate.Position.Elite] = aElite;
            m_cEstates[Estate.Position.Clergy] = aCleregy;
            m_cEstates[Estate.Position.Commoners] = aMiddle;
            m_cEstates[Estate.Position.Lowlifes] = aLow;
            m_cEstates[Estate.Position.Outlaws] = aOutlaw;

            if (m_cEstates[Estate.Position.Elite] == null || m_cEstates[Estate.Position.Elite].Length == 0)
                m_cEstates[Estate.Position.Elite] = new string[] { "Elite" };

            if (m_cEstates[Estate.Position.Clergy] == null || m_cEstates[Estate.Position.Clergy].Length == 0)
                m_cEstates[Estate.Position.Clergy] = new string[] { "Clergy" };

            if (m_cEstates[Estate.Position.Commoners] == null || m_cEstates[Estate.Position.Commoners].Length == 0)
                m_cEstates[Estate.Position.Commoners] = new string[] { "Commoner" };

            if (m_cEstates[Estate.Position.Lowlifes] == null || m_cEstates[Estate.Position.Lowlifes].Length == 0)
                m_cEstates[Estate.Position.Lowlifes] = new string[] { "Servant" };

            if (m_cEstates[Estate.Position.Outlaws] == null || m_cEstates[Estate.Position.Outlaws].Length == 0)
                m_cEstates[Estate.Position.Outlaws] = new string[] { "Outlaw" };
        }

        public string GetEstateName(Estate.Position ePosition)
        {
            return m_cEstates[ePosition][Rnd.Get(m_cEstates[ePosition].Length)];
        }
    }
}
