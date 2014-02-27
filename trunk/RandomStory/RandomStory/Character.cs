using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace RandomStory
{
    public class Character
    {
        //public bool m_bMale;

        public World m_pHomeWorld;

        public string m_sRace;

        public string m_sProfession;

        public List<string> m_cPerks = new List<string>();

        public string m_sRelationAdjective;

        public int m_iRelationsCounter = 1;

        public Character(Repository pRep, World pWorld, bool bVoyagersAllowed, bool bElite, Character pRelative, string sRelationAdjective)
        {
            m_pHomeWorld = pWorld;

            m_sRelationAdjective = sRelationAdjective;

            bool bRelative = true;

            if (pRelative == null)
            {
                bRelative = false;
            }
            else
            {
                if (Rnd.Chances(pRelative.m_iRelationsCounter, 5))
                    bRelative = false;

                if (pRelative.m_pHomeWorld.m_sName != pWorld.m_sName && Rnd.OneChanceFrom(2))
                    bRelative = false;
            }
            
            if (bElite && Rnd.OneChanceFrom(2))
                bRelative = false;

            if (bRelative)
            {
                m_pHomeWorld = pRelative.m_pHomeWorld;

                if (m_pHomeWorld.m_sName != pWorld.m_sName)
                    m_cPerks.Add(pRelative.m_cPerks[0]);

                m_sRace = m_pHomeWorld.GetRandomRace(pRelative.m_sRace);

                if (Rnd.OneChanceFrom(3))
                    m_cPerks.Add(string.Format("{0} родственник {1}", Rnd.OneChanceFrom(2) ? "близкий":"дальний", pRelative.m_sRelationAdjective));
                else
                    m_cPerks.Add(string.Format("{0} знакомый {1}", Rnd.OneChanceFrom(2) ? "давний" : "случайный", pRelative.m_sRelationAdjective));

                pRelative.m_iRelationsCounter *= 2;
                m_iRelationsCounter = pRelative.m_iRelationsCounter;
            }
            else
            {
                if (bVoyagersAllowed && Rnd.OneChanceFrom(5))
                {
                    World pOtherWorld = pRep.GetRandomWorld(2);
                    if (pOtherWorld.m_sName != pWorld.m_sName)
                    {
                        m_pHomeWorld = pOtherWorld;
                        if (Rnd.OneChanceFrom(2))
                            m_cPerks.Add("пришелец из другого мира (" + m_pHomeWorld.ToString() + ")");
                        else
                            m_cPerks.Add("путешественник во времени (" + m_pHomeWorld.ToString() + ")");
                    }
                }

                m_sRace = m_pHomeWorld.GetRandomRace();
            }
            //m_bMale = Rnd.OneChanceFrom(2);

            m_sProfession = bElite ? m_pHomeWorld.GetRandomProfessionEvil() : m_pHomeWorld.GetRandomProfession();

            int iPerksCount = 2 + Rnd.Get(2);
            for (int i = 0; i < iPerksCount; i++)
            {
                string sPerk = m_pHomeWorld.GetRandomPerk(m_cPerks);
                if(!string.IsNullOrWhiteSpace(sPerk))
                    m_cPerks.Add(sPerk);
            }
        }

        public override string ToString()
        {
            //StringBuilder sResult = new StringBuilder(string.Format("{0} - {1}, {2}", m_bMale ? "мужчина" : "женщина", m_sRace, m_sProfession));
            StringBuilder sResult = new StringBuilder(string.Format("{0}, {1}", m_sRace, m_sProfession));

            foreach (string sPerk in m_cPerks)
                sResult.AppendFormat(", {0}", sPerk);

            return sResult.ToString();
        }
    }
}
