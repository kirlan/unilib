using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socium
{
    public class SocialOrder
    {
        public string[] m_aElite;
        public string[] m_aMiddle;
        public string[] m_aLow;
        public string[] m_aOutlaw;

        public static SocialOrder Primitive = new SocialOrder(new string[] {"Elder"}, new string[] {"Tribesman"}, new string[] {"Servant"}, new string[] {"Outsider"});
        public static SocialOrder MedievalSlavic = new SocialOrder(new string[] {"Dvoryan", "Kmet"}, new string[] {"Meshchan", "Smerd", "Batrak"}, new string[] {"Kholop"}, new string[] {"Izgoi"});
        public static SocialOrder MedievalEurope = new SocialOrder(new string[] {"Knight", "Esquare"}, new string[] {"Commoner", "Freeholder", "Villein"}, new string[] {"Serf"}, new string[] {"Rogue"});
        public static SocialOrder MedievalEurope2 = new SocialOrder(new string[] {"Peer", "Gentry"}, new string[] {"Burgher", "Commoner", "Freeman"}, new string[] {"Serf"}, new string[] {"Outlaw"});
        public static SocialOrder MedievalAsian = new SocialOrder(new string[] {"Fudai", "Samurai"}, new string[] {"Heimin", "Chonin", "Burakumin"}, new string[] {"Hinin"}, new string[] {"Ronin"});
        public static SocialOrder MedievalLatin = new SocialOrder(new string[] {"Patricius", "Equitius"}, new string[] {"Nobilis", "Plebis", "Proletaris"}, new string[] {"Serv"}, new string[] {"Bagaudis"});
        public static SocialOrder MedievalNorth = new SocialOrder(new string[] {"Earl", "Drohtin"}, new string[] {"Hauldr", "Bonde", "Huskarl"}, new string[] {"Thrall"}, new string[] {"Rogue"});
        public static SocialOrder MedievalArabian = new SocialOrder(new string[] {"Sayyid", "Effendi"}, new string[] {"Nawab", "Khoja", "Yazat"}, new string[] {"Kul"}, new string[] {"Rogue"});
        public static SocialOrder MedievalMongol = new SocialOrder(new string[] {"Noyon", "Darkhan"}, new string[] {"Batur", "Nukar", "Karach"}, new string[] {"Urtakch"}, new string[] {"Rogue"});
        public static SocialOrder MedievalGreek = new SocialOrder(new string[] {"Ephor", "Diadoch"}, new string[] {"Demos", "Metic", "Penest"}, new string[] {"Helot"}, new string[] {"Rogue"});
        public static SocialOrder MedievalHindu = new SocialOrder(new string[] {"Brahmin", "Kshatriya"}, new string[] {"Karava", "Kayastha", "Patwa"}, new string[] {"Shudra"}, new string[] {"Panchama"});
        public static SocialOrder MedievalAztec = new SocialOrder(new string[] {"Pilli", "Pochtecatli"}, new string[] {"Macehualli", "Temilti", "Malli"}, new string[] {"Tlacotli"}, new string[] {"Panchama"});
        public static SocialOrder Modern = new SocialOrder(new string[] {"Elite", "Oligarch"}, new string[] {"White collar", "Grey collar", "Pink collar"}, new string[] {"Blue collar"}, new string[] {"Gangster"});
        public static SocialOrder Future = new SocialOrder(new string[] {"Citizen"}, new string[] {"Citizen"}, new string[] {"Morlock"}, new string[] {"Outsider"});

        public SocialOrder(string[] aElite, string[] aMiddle, string[] aLow, string[] aOutlaw)
        {
            m_aElite = aElite;
            m_aMiddle = aMiddle;
            m_aLow = aLow;
            m_aOutlaw = aOutlaw;

            if(m_aElite == null || m_aElite.Length == 0)
                m_aElite = new string[] {"Elite"};

            if(m_aMiddle == null || m_aMiddle.Length == 0)
                m_aMiddle = new string[] {"Commoner"};

            if(m_aLow == null || m_aLow.Length == 0)
                m_aLow = new string[] {"Servant"};

            if(m_aOutlaw == null || m_aOutlaw.Length == 0)
                m_aOutlaw = new string[] {"Outlaw"};
        }
    }
}
