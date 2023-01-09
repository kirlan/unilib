using Random;
using Socium.Population;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socium
{
    public class Formation
    {
        public static readonly Formation Primitive = new Formation(new string[] {"Firstborn"}, new string[] {"Seers"}, new string[] {"Tribesman"}, new string[] {"Servant"}, new string[] {"Outsider"});
        public static readonly Formation MedievalSlavic = new Formation(new string[] { "Dvoryan", "Kmet" }, new string[] {"Volkhv"}, new string[] { "Meshchan", "Smerd", "Batrak" }, new string[] { "Kholop" }, new string[] { "Izgoi" });
        public static readonly Formation MedievalEurope = new Formation(new string[] { "Knight", "Esquare" }, new string[] { "Cleric" }, new string[] { "Commoner", "Freeholder", "Villein" }, new string[] { "Serf" }, new string[] { "Rogue" });
        public static readonly Formation MedievalEurope2 = new Formation(new string[] { "Peer", "Gentry" }, new string[] { "Cleric" }, new string[] { "Burgher", "Yeoman", "Freeman" }, new string[] { "Serf" }, new string[] { "Outlaw" });
        public static readonly Formation MedievalEurope3 = new Formation(new string[] { "Aristocrat" }, new string[] { "Cleric" }, new string[] { "Commoner", "Freeman" }, new string[] { "Serf" }, new string[] { "Outlaw" });
        public static readonly Formation MedievalAsian = new Formation(new string[] { "Fudai", "Samurai" }, new string[] { "Shinkan" }, new string[] { "Heimin", "Chonin", "Burakumin" }, new string[] { "Hinin" }, new string[] { "Ronin" });
        public static readonly Formation MedievalLatin = new Formation(new string[] {"Patricius", "Equitius"}, new string[] { "Vestal" }, new string[] {"Nobilis", "Plebis", "Proletaris"}, new string[] {"Serv"}, new string[] {"Bagaudis"});
        public static readonly Formation MedievalNorth = new Formation(new string[] {"Earl", "Drohtin"}, new string[] { "Druid" }, new string[] {"Hauldr", "Bonde", "Huskarl"}, new string[] {"Thrall"}, new string[] {"Rogue"});
        public static readonly Formation MedievalArabian = new Formation(new string[] {"Sayyid", "Effendi"}, new string[] { "Mullah" }, new string[] {"Nawab", "Khoja", "Yazat"}, new string[] {"Kul"}, new string[] {"Rogue"});
        public static readonly Formation MedievalMongol = new Formation(new string[] {"Noyon", "Darkhan"}, new string[] { "Bhikkhu" }, new string[] {"Batur", "Nukar", "Karach"}, new string[] {"Urtakch"}, new string[] {"Rogue"});
        public static readonly Formation MedievalGreek = new Formation(new string[] {"Ephor", "Diadoch"}, new string[] { "Hierei" }, new string[] {"Demos", "Metic", "Penest"}, new string[] {"Helot"}, new string[] {"Rogue"});
        public static readonly Formation MedievalHindu = new Formation(new string[] {"Kshatriy"}, new string[] { "Brahmin" }, new string[] {"Karav", "Kayasth", "Pataw"}, new string[] {"Shudra"}, new string[] {"Pancham"});
        public static readonly Formation MedievalAztec = new Formation(new string[] {"Pilli", "Pochtecatli"}, new string[] { "Tlamacazqui" }, new string[] {"Macehualli", "Temilti", "Malli"}, new string[] {"Tlacotli"}, new string[] {"Panchama"});
        public static readonly Formation Modern = new Formation(new string[] { "Elite", "Oligarch" }, new string[] { "Cleric" }, new string[] { "Specialist", "Bourgeois" }, new string[] { "Proletarian", "Lumpen" }, new string[] { "Gangster" });
        public static readonly Formation Future = new Formation(new string[] {"Citizen"}, new string[] { "Citizen" }, new string[] {"Citizen"}, new string[] {"Morlock"}, new string[] {"Outsider"});

        public Dictionary<Estate.SocialRank, string[]> Estates { get; } = new Dictionary<Estate.SocialRank, string[]>();

        public Formation(string[] aElite, string[] aCleregy, string[] aMiddle, string[] aLow, string[] aOutlaw)
        {
            Estates[Estate.SocialRank.Elite] = aElite;
            Estates[Estate.SocialRank.Clergy] = aCleregy;
            Estates[Estate.SocialRank.Commoners] = aMiddle;
            Estates[Estate.SocialRank.Lowlifes] = aLow;
            Estates[Estate.SocialRank.Outlaws] = aOutlaw;

            if (Estates[Estate.SocialRank.Elite] == null || Estates[Estate.SocialRank.Elite].Length == 0)
                Estates[Estate.SocialRank.Elite] = new string[] { "Elite" };

            if (Estates[Estate.SocialRank.Clergy] == null || Estates[Estate.SocialRank.Clergy].Length == 0)
                Estates[Estate.SocialRank.Clergy] = new string[] { "Clergy" };

            if (Estates[Estate.SocialRank.Commoners] == null || Estates[Estate.SocialRank.Commoners].Length == 0)
                Estates[Estate.SocialRank.Commoners] = new string[] { "Commoner" };

            if (Estates[Estate.SocialRank.Lowlifes] == null || Estates[Estate.SocialRank.Lowlifes].Length == 0)
                Estates[Estate.SocialRank.Lowlifes] = new string[] { "Servant" };

            if (Estates[Estate.SocialRank.Outlaws] == null || Estates[Estate.SocialRank.Outlaws].Length == 0)
                Estates[Estate.SocialRank.Outlaws] = new string[] { "Outlaw" };
        }

        public string GetEstateName(Estate.SocialRank ePosition)
        {
            return Estates[ePosition][Rnd.Get(Estates[ePosition].Length)];
        }
    }
}
