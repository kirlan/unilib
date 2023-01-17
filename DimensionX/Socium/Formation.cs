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
        public static readonly Formation Primitive = new Formation(new[]{"Firstborn"}, new[]{"Seers"}, new[]{"Tribesman"}, new[]{"Servant"}, new[]{"Outsider"});
        public static readonly Formation MedievalSlavic = new Formation(new[]{ "Dvoryan", "Kmet" }, new[]{"Volkhv"}, new[]{ "Meshchan", "Smerd", "Batrak" }, new[]{ "Kholop" }, new[]{ "Izgoi" });
        public static readonly Formation MedievalEurope = new Formation(new[]{ "Knight", "Esquare" }, new[]{ "Cleric" }, new[]{ "Commoner", "Freeholder", "Villein" }, new[]{ "Serf" }, new[]{ "Rogue" });
        public static readonly Formation MedievalEurope2 = new Formation(new[]{ "Peer", "Gentry" }, new[]{ "Cleric" }, new[]{ "Burgher", "Yeoman", "Freeman" }, new[]{ "Serf" }, new[]{ "Outlaw" });
        public static readonly Formation MedievalEurope3 = new Formation(new[]{ "Aristocrat" }, new[]{ "Cleric" }, new[]{ "Commoner", "Freeman" }, new[]{ "Serf" }, new[]{ "Outlaw" });
        public static readonly Formation MedievalAsian = new Formation(new[]{ "Fudai", "Samurai" }, new[]{ "Shinkan" }, new[]{ "Heimin", "Chonin", "Burakumin" }, new[]{ "Hinin" }, new[]{ "Ronin" });
        public static readonly Formation MedievalLatin = new Formation(new[]{"Patricius", "Equitius"}, new[]{ "Vestal" }, new[]{"Nobilis", "Plebis", "Proletaris"}, new[]{"Serv"}, new[]{"Bagaudis"});
        public static readonly Formation MedievalNorth = new Formation(new[]{"Earl", "Drohtin"}, new[]{ "Druid" }, new[]{"Hauldr", "Bonde", "Huskarl"}, new[]{"Thrall"}, new[]{"Rogue"});
        public static readonly Formation MedievalArabian = new Formation(new[]{"Sayyid", "Effendi"}, new[]{ "Mullah" }, new[]{"Nawab", "Khoja", "Yazat"}, new[]{"Kul"}, new[]{"Rogue"});
        public static readonly Formation MedievalMongol = new Formation(new[]{"Noyon", "Darkhan"}, new[]{ "Bhikkhu" }, new[]{"Batur", "Nukar", "Karach"}, new[]{"Urtakch"}, new[]{"Rogue"});
        public static readonly Formation MedievalGreek = new Formation(new[]{"Ephor", "Diadoch"}, new[]{ "Hierei" }, new[]{"Demos", "Metic", "Penest"}, new[]{"Helot"}, new[]{"Rogue"});
        public static readonly Formation MedievalHindu = new Formation(new[]{"Kshatriy"}, new[]{ "Brahmin" }, new[]{"Karav", "Kayasth", "Pataw"}, new[]{"Shudra"}, new[]{"Pancham"});
        public static readonly Formation MedievalAztec = new Formation(new[]{"Pilli", "Pochtecatli"}, new[]{ "Tlamacazqui" }, new[]{"Macehualli", "Temilti", "Malli"}, new[]{"Tlacotli"}, new[]{"Panchama"});
        public static readonly Formation Modern = new Formation(new[]{ "Elite", "Oligarch" }, new[]{ "Cleric" }, new[]{ "Specialist", "Bourgeois" }, new[]{ "Proletarian", "Lumpen" }, new[]{ "Gangster" });
        public static readonly Formation Future = new Formation(new[]{"Citizen"}, new[]{ "Citizen" }, new[]{"Citizen"}, new[]{"Morlock"}, new[]{"Outsider"});

        public Dictionary<Estate.SocialRank, string[]> Estates { get; } = new Dictionary<Estate.SocialRank, string[]>();

        public Formation(string[] aElite, string[] aCleregy, string[] aMiddle, string[] aLow, string[] aOutlaw)
        {
            Estates[Estate.SocialRank.Elite] = aElite;
            Estates[Estate.SocialRank.Clergy] = aCleregy;
            Estates[Estate.SocialRank.Commoners] = aMiddle;
            Estates[Estate.SocialRank.Lowlifes] = aLow;
            Estates[Estate.SocialRank.Outlaws] = aOutlaw;

            if (Estates[Estate.SocialRank.Elite] == null || Estates[Estate.SocialRank.Elite].Length == 0)
                Estates[Estate.SocialRank.Elite] = new[]{ "Elite" };

            if (Estates[Estate.SocialRank.Clergy] == null || Estates[Estate.SocialRank.Clergy].Length == 0)
                Estates[Estate.SocialRank.Clergy] = new[]{ "Clergy" };

            if (Estates[Estate.SocialRank.Commoners] == null || Estates[Estate.SocialRank.Commoners].Length == 0)
                Estates[Estate.SocialRank.Commoners] = new[]{ "Commoner" };

            if (Estates[Estate.SocialRank.Lowlifes] == null || Estates[Estate.SocialRank.Lowlifes].Length == 0)
                Estates[Estate.SocialRank.Lowlifes] = new[]{ "Servant" };

            if (Estates[Estate.SocialRank.Outlaws] == null || Estates[Estate.SocialRank.Outlaws].Length == 0)
                Estates[Estate.SocialRank.Outlaws] = new[]{ "Outlaw" };
        }

        public string GetEstateName(Estate.SocialRank ePosition)
        {
            return Estates[ePosition][Rnd.Get(Estates[ePosition].Length)];
        }
    }
}
