using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace RB
{
    enum Gender
    { 
        Male,
        Female
    }

    enum Relation
    { 
        Enemy,
        Opponent,
        Neutral,
        Friend,
        Ally
    }

    enum Kinship
    { 
        Parent,
        Child,
        Relative
    }

    class CPerson
    {
        private Gender m_eGender;

        internal Gender Gender
        {
            get { return m_eGender; }
        }

        int m_iBody;

        public int Body
        {
            get { return m_iBody; }
        }

        int m_iMind;

        public int Mind
        {
            get { return m_iMind; }
        }

        int m_iTechLevel;

        public int TechLevel
        {
            get { return m_iTechLevel; }
        }
        int m_iBioLevel;

        public int BioLevel
        {
            get { return m_iBioLevel; }
        }
        int m_iCultureLevel;

        public int CultureLevel
        {
            get { return m_iCultureLevel; }
        }

        private List<CEstate> m_cEstates = new List<CEstate>();

        private CParty m_pParty;

        private Dictionary<CPerson, Relation> m_cRelations = new Dictionary<CPerson, Relation>();
        private Dictionary<CPerson, Kinship> m_cKindred = new Dictionary<CPerson,Kinship>();
        private List<CPerson> m_cLovers = new List<CPerson>();
        private CPerson m_pSpose = null;

        public CPerson(CEstate pEstate)
        {
            m_cEstates.Add(pEstate);

            m_iTechLevel = pEstate.TechLevel;
            m_iBioLevel = pEstate.BioLevel;
            m_iCultureLevel = pEstate.CultureLevel;

            int iBody = (int)(Math.Pow(Rnd.Get(20), 2) / 100);
            int iMind = (int)(Math.Pow(Rnd.Get(20), 2) / 100);

            if (Rnd.OneChanceFrom(2))
                m_iBody = pEstate.BaseBody + iBody;
            else
                m_iBody = pEstate.BaseBody - iBody;

            if (m_iBody < 0)
                m_iBody = 0;
            if (m_iBody > 8)
                m_iBody = 8;

            if (Rnd.OneChanceFrom(2))
                m_iMind = pEstate.BaseMind + iMind;
            else
                m_iMind = pEstate.BaseMind - iMind;

            if (m_iMind < 0)
                m_iMind = 0;
            if (m_iMind > 8)
                m_iMind = 8;

            int iMaleChances, iFemaleChances;

            switch (pEstate.GenderPriority)
            {
                case GenderPriority.OnlyFemale:
                    iMaleChances = 0;
                    iFemaleChances = 10;
                    break;
                case GenderPriority.MostlyFemale:
                    iMaleChances = 3;
                    iFemaleChances = 6;
                    break;
                case GenderPriority.MostlyMale:
                    iMaleChances = 6;
                    iFemaleChances = 3;
                    break;
                case GenderPriority.OnlyMale:
                    iMaleChances = 10;
                    iFemaleChances = 0;
                    break;
                default:
                    iMaleChances = 5;
                    iFemaleChances = 5;
                    break;
            }

            if (Rnd.ChooseOne(iMaleChances, iFemaleChances))
                m_eGender = Gender.Male;
            else
                m_eGender = Gender.Female;
        }

        /// <summary>
        /// Принять удар в поединке.
        /// Шанс того, что удар противника вообще достигнет цели, определяется мастерством противника.
        /// Если удар прошёл, то у бойца есть шанс выставить блок, определяемый его собственным мастерством.
        /// Если боец пропустил удар - он проиграл.
        /// Если удар удалось заблокировать, то сравниваем экипировку бойца и противника и считаем шансы того,
        /// что экипировка бойца могла выдержать удар противника.
        /// В поединке оба бойца используют и технику и бионику.
        /// </summary>
        /// <param name="pOpponent">противник</param>
        /// <returns>true если боец проиграл поединок</returns>
        public bool TakeHit(CPerson pOpponent)
        {
            //оппонент бьёт техникой
            if (pOpponent.TechLevel > 0 && pOpponent.Body * pOpponent.Body >= Rnd.Get(200))
            {
                double attack = Math.Sqrt(pOpponent.TechLevel);// *pOpponent.TechLevel;
                double tDefence = Math.Sqrt(TechLevel);// *TechLevel;
                double bDefence = Math.Sqrt(BioLevel);// *BioLevel;
                //боец защищается техникой
                if (Body < Rnd.Get(10) ||
                    Rnd.ChooseOne(attack, tDefence))//боец пропустил удар или броня его не спасла
                {
                    //боец защищается бионикой
                    if (Mind < Rnd.Get(20) ||
                        Rnd.ChooseOne(attack, bDefence))//боец пропустил удар или суперспособности его не спасли
                    {
                        return true;
                    }
                }
            }
            //оппонент бьёт бионикой
            if (pOpponent.BioLevel > 0 && pOpponent.Mind * pOpponent.Mind >= Rnd.Get(200))
            {
                double attack = Math.Sqrt(pOpponent.BioLevel);// *pOpponent.BioLevel;
                double tDefence = Math.Sqrt(TechLevel);// *TechLevel;
                double bDefence = Math.Sqrt(BioLevel);// *BioLevel;
                //боец защищается бионикой
                if (Mind < Rnd.Get(10) ||
                    Rnd.ChooseOne(attack, bDefence))//боец пропустил удар или суперспособности его не спасли
                {
                    //боец защищается техникой
                    if (Body < Rnd.Get(20) ||
                        Rnd.ChooseOne(attack, tDefence))//боец пропустил удар или броня его не спасла
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Атаковать противника техникой и бионикой.
        /// </summary>
        /// <param name="pOpponent">противник</param>
        /// <returns>true если противник повержен</returns>
        public bool Attack(CPerson pOpponent)
        {
            bool bWin1 = false, bWin2 = false;

            do
            {
                bWin1 = pOpponent.TakeHit(this);
                bWin2 = TakeHit(pOpponent);
            }
            while (!bWin1 && !bWin2);

            return bWin1;
        }

        /// <summary>
        /// Взаимодействие двух персонажей.
        /// В зависимости от их культурного уровня, может вылиться в драку или мирную дискуссию.
        /// </summary>
        /// <param name="pOpponent">оппонент</param>
        /// <returns>true если противник повежен - физически или интеллектуально</returns>
        public bool Interact(CPerson pOpponent)
        {
            Dictionary<CSociety, int> cIntersections = new Dictionary<CSociety, int>();

            foreach (CEstate pEstate in m_cEstates)
            {
                foreach (CEstate pOppEstate in pOpponent.m_cEstates)
                {
                    if (pEstate.Society == pOppEstate.Society)
                        cIntersections[pEstate.Society] = pOppEstate.Rank - pEstate.Rank;
                }
            }

            bool bAttackPossible = (cIntersections.Count == 0);
            foreach (CSociety pSociety in cIntersections.Keys)
            {
                if (cIntersections[pSociety] == 1)
                    bAttackPossible = true;
            }

            bool bSexPossible = (m_eGender != pOpponent.m_eGender);

            List<int> cChances = new List<int>();

            //оценка
            cChances[0] = 10;
            //общение
            cChances[1] = 0;
            if (m_cRelations.ContainsKey(pOpponent))
            {
                switch (m_cRelations[pOpponent])
                {
                    case Relation.Enemy:
                        cChances[1] = 0;
                        break;
                    case Relation.Opponent:
                        cChances[1] = 2;
                        break;
                    case Relation.Neutral:
                        cChances[1] = 5;
                        break;
                    case Relation.Friend:
                        cChances[1] = 7;
                        break;
                    case Relation.Ally:
                        cChances[1] = 10;
                        break;
                }
            }
            //нападение
            cChances[2] = 0;
            if (bAttackPossible && m_cRelations.ContainsKey(pOpponent))
            {
                switch (m_cRelations[pOpponent])
                {
                    case Relation.Enemy:
                        cChances[1] = 10;
                        break;
                    case Relation.Opponent:
                        cChances[1] = 7;
                        break;
                    case Relation.Neutral:
                        cChances[1] = 5;
                        break;
                    case Relation.Friend:
                        cChances[1] = 2;
                        break;
                    case Relation.Ally:
                        cChances[1] = 0;
                        break;
                }
            }
            //секс
            cChances[3] = 0;
            if (bSexPossible && m_cRelations.ContainsKey(pOpponent) && !m_cKindred.ContainsKey(pOpponent))
                cChances[2] = 10;

            int iChance = Rnd.ChooseOne(cChances, 1);

            switch (iChance)
            {
                //оценка
                case 0:
                    if (!m_cRelations.ContainsKey(pOpponent))
                    { 
                        if(Rnd.OneChanceFrom())
                    }
                    break;
                //общение   
                case 1:
                    break;
                //нападение
                case 2:
                    break;
                //секс
                case 3:
                    break;
            }

            if (CultureLevel < Rnd.Get(10))
                return Attack(pOpponent);
            else
            {
                if (pOpponent.CultureLevel < Rnd.Get(10))
                    return pOpponent.Attack(this);
                else
                    return Rnd.ChooseOne(m_iCharisma, pOpponent.m_iCharisma);
            }
        }
    }
}
