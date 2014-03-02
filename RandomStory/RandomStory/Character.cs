﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace RandomStory
{
    public class Character
    {
        //public bool m_bMale;

        public Setting m_pHomeSetting;

        public string m_sRace;

        public string m_sProfession;

        public Strings m_cPerks = new Strings();

        public string m_sRelationAdjective;

        public int m_iRelationsCounter = 1;

        public char[] m_aFlags = null;

        public Character(Repository pRep, Setting pSetting, bool bVoyager, bool bElite, Character pRelative, string sRelationAdjective)
        {
            m_pHomeSetting = pSetting;

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

                if (pRelative.m_pHomeSetting.Equals(pSetting) && Rnd.OneChanceFrom(2))
                    bRelative = false;
            }
            
            if (bElite && Rnd.OneChanceFrom(2))
                bRelative = false;

            if (bRelative)
            {
                m_pHomeSetting = pRelative.m_pHomeSetting;

                if (!m_pHomeSetting.Equals(pSetting))
                    m_cPerks.Add(pRelative.m_cPerks[0]);

                m_sRace = m_pHomeSetting.GetRandomRace(pRelative.m_sRace, ref m_aFlags);

                m_cPerks.Add(string.Format("{0} {1}", pRep.GetRandomRelation(ref m_aFlags), pRelative.m_sRelationAdjective));

                pRelative.m_iRelationsCounter *= 2;
                m_iRelationsCounter = pRelative.m_iRelationsCounter;
            }
            else
            {
                if (bVoyager)// && Rnd.OneChanceFrom(5))
                {
                    Setting pOtherWorld = pRep.GetRandomSetting(1, false);
                    if (!pOtherWorld.Equals(pSetting))
                    {
                        m_pHomeSetting = pOtherWorld;
                        m_cPerks.Add("попаданец (" + m_pHomeSetting.ToString() + ")");
                        //if (Rnd.OneChanceFrom(2))
                        //    m_cPerks.Add("пришелец из другого мира (" + m_pHomeSetting.ToString() + ")");
                        //else
                        //    m_cPerks.Add("путешественник во времени (" + m_pHomeSetting.ToString() + ")");
                    }
                }

                m_sRace = m_pHomeSetting.GetRandomRace(ref m_aFlags);
            }
            //m_bMale = Rnd.OneChanceFrom(2);

            m_sProfession = bElite ? m_pHomeSetting.GetRandomProfessionElite(ref m_aFlags) : m_pHomeSetting.GetRandomProfession(ref m_aFlags);

            int iPerksCount = 2 + Rnd.Get(2);
            for (int i = 0; i < iPerksCount; i++)
            {
                string sPerk = m_pHomeSetting.GetRandomPerk(m_cPerks, ref m_aFlags);
                if(!string.IsNullOrWhiteSpace(sPerk))
                    m_cPerks.Add(sPerk);
            }
        }

        public override string ToString()
        {
            StringBuilder sResult = new StringBuilder(string.Format("{0}, {1}, {2}", m_sRace, m_sProfession, m_cPerks.ToString()));

            return sResult.ToString();
        }
    }
}
