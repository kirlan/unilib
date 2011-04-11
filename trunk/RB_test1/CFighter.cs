using System;
using System.Collections.Generic;
using System.Text;
using Random;

namespace RB_test1
{
    class CCiv
    {
        private int m_iTier;
        /// <summary>
        /// Уровень развития
        /// </summary>
        public int Tier
        {
            get { return m_iTier; }
            set { m_iTier = value; }
        }
        private int m_iLevel;
        /// <summary>
        /// Мастерство бойца
        /// </summary>
        public int Mastery
        {
            get { return m_iLevel; }
            set { m_iLevel = value; }
        }
        private int m_iHits;
        /// <summary>
        /// Сколько ударов получил боец
        /// </summary>
        public int Hits
        {
            get { return m_iHits; }
            set { m_iHits = value; }
        }
        private int m_iParry;
        /// <summary>
        /// Сколько ударов боец отразил
        /// </summary>
        public int Parry
        {
            get { return m_iParry; }
            set { m_iParry = value; }
        }
    }

    enum DeathReason
    { 
        TechHit,
        BioHit,
        Wounds,
        None
    }

    class CFighter
    {
        CCiv m_pTech = new CCiv();
        /// <summary>
        /// Техногенная подготовка
        /// </summary>
        internal CCiv Tech
        {
            get { return m_pTech; }
        }
        CCiv m_pBio = new CCiv();
        /// <summary>
        /// Биогенная подготовка
        /// </summary>
        internal CCiv Bio
        {
            get { return m_pBio; }
        }

        DeathReason m_eDeathReason = DeathReason.None;

        internal DeathReason DeathReason
        {
            get { return m_eDeathReason; }
        }

        public string DeathLabel
        {
            get
            {
                switch (m_eDeathReason)
                {
                    case DeathReason.TechHit:
                        return "Техно";
                    case DeathReason.BioHit:
                        return "Био";
                    case DeathReason.Wounds:
                        return "Кровотечение";
                    default:
                        return "-";
                }
            }
        }

        public CFighter(int iTTier, int iTLevel, int iBTier, int iBLevel, int iWeaponLevel)
        {
            m_pTech.Tier = iTTier;
            m_pTech.Mastery = iTLevel;
            m_pTech.Hits = 0;
            m_pTech.Parry = 0;

            if (iWeaponLevel != iTTier)
            { 
                m_pTech.Tier = iWeaponLevel;
                if (iWeaponLevel < iTTier)
                    m_pTech.Mastery -= iTTier - iWeaponLevel;
                else
                    m_pTech.Mastery -= 2 * (iWeaponLevel - iTTier);
            }

            m_pBio.Tier = iBTier;
            m_pBio.Mastery = iBLevel;
            m_pBio.Hits = 0;
            m_pBio.Parry = 0;
        }

        public CFighter(): this(0,0,0,0,0)
        { }

        public void RestoreHealth()
        {
            m_eDeathReason = DeathReason.None;
        }

        public void Reset()
        {
            m_pTech.Hits = 0;
            m_pTech.Parry = 0;

            m_pBio.Hits = 0;
            m_pBio.Parry = 0;

            m_eDeathReason = DeathReason.None;
        }

        public void TakeHit(CFighter pOpponent)
        {
            //оппонент бьёт техникой
            if (pOpponent.Tech.Tier > 0 && pOpponent.Tech.Mastery * pOpponent.Tech.Mastery >= Rnd.Get(200))
            {
                m_pTech.Hits++;
                double attack = Math.Sqrt(pOpponent.Tech.Tier);// *pOpponent.Tech.Tier;
                double tDefence = Math.Sqrt(Tech.Tier);// *Tech.Tier;
                double bDefence = Math.Sqrt(Bio.Tier);// *Bio.Tier;
                //боец защищается техникой
                if (Tech.Mastery >= Rnd.Get(10) && 
                    Rnd.ChooseOne(tDefence, attack))//боец отразил удар
                {
                    m_pTech.Parry++;//удар успешно отражён
                }
                else
                {
                    //боец защищается бионикой
                    if (Bio.Mastery >= Rnd.Get(20) && 
                        Rnd.ChooseOne(bDefence, attack))//боец отразил удар
                    {
                        m_pBio.Parry++;//удар успешно отражён
                    }
                    else
                    {
                        //защита не выдержала
                        m_eDeathReason = DeathReason.TechHit;
                    }
                }
            }
            //оппонент бьёт бионикой
            if (pOpponent.Bio.Tier > 0 && pOpponent.Bio.Mastery * pOpponent.Bio.Mastery >= Rnd.Get(200))
            {
                m_pBio.Hits++;
                double attack = Math.Sqrt(pOpponent.Bio.Tier);// *pOpponent.Bio.Tier;
                double tDefence = Math.Sqrt(Tech.Tier);// *Tech.Tier;
                double bDefence = Math.Sqrt(Bio.Tier);// *Bio.Tier;
                //наш боец защищается бионикой
                if (Bio.Mastery >= Rnd.Get(10) && 
                    Rnd.ChooseOne(bDefence, attack))//боец отразил удар
                {
                    m_pBio.Parry++;//удар успешно отражён
                }
                else
                {
                    //боец защищается техникой
                    if (Tech.Mastery >= Rnd.Get(20) && 
                        Rnd.ChooseOne(tDefence, attack))//боец отразил удар
                    {
                        m_pTech.Parry++;//удар успешно отражён
                    }
                    else
                    {
                        //броня не выдержала
                        m_eDeathReason = DeathReason.BioHit;
                    }
                }
            }
        }
    }
}
