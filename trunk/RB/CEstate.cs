using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace RB
{
    /// <summary>
    /// Сословие – это совокупность людей, объединённых общим техногенным, биогенным и 
    /// культурным уровнями развития. Каждое сообщество состоит из нескольких сословий. 
    /// Уровни развития сословий соотносятся с уровнями развития сообщества так же, как 
    /// уровни развития сообществ с базовыми уровнями развития мира – т.е. в общем совпадают, 
    /// но могут быть и отклонения. Таким образом, у нас может образоваться классическое 
    /// пост-апокалиптическое общество, когда – например – правящая верхушка сохраняет 
    /// доступ к высоким технологиям, а низшие слои прозябают в нищете и дикости.
    /// 
    /// Один и тот же индивидуум может принадлежать к нескольким сообществам, входя в каждом 
    /// сообществе в одно из сословий. Принадлежать к нескольким сословиям в пределах одного 
    /// сообщества невозможно.
    /// </summary>
    class CEstate
    {
        GenderPriority m_eGenderPriority = GenderPriority.Equal;

        internal GenderPriority GenderPriority
        {
            get { return m_eGenderPriority; }
        }

        int m_iBaseBody;

        public int BaseBody
        {
            get { return m_iBaseBody; }
        }

        int m_iBaseMind;

        public int BaseMind
        {
            get { return m_iBaseMind; }
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

        private CSociety m_pSociety;

        internal CSociety Society
        {
            get { return m_pSociety; }
        }

        public int Rank
        {
            get
            {
                if (m_pSociety.Ranks.ContainsKey(this))
                    return m_pSociety.Ranks[this];
                else
                    return -1;
            }
        }

        public CEstate(CSociety pSociety)
        {
            m_pSociety = pSociety;

            int iTechLevel = (int)(Math.Pow(Rnd.Get(20), 2) / 100);
            int iBioLevel = (int)(Math.Pow(Rnd.Get(20), 2) / 100);
            int iCultureLevel = (int)(Math.Pow(Rnd.Get(20), 2) / 100);

            if (Rnd.OneChanceFrom(2))
                m_iTechLevel = pSociety.TechLevel + iTechLevel;
            else
                m_iTechLevel = pSociety.TechLevel - iTechLevel;

            if (m_iTechLevel < 0)
                m_iTechLevel = 0;
            if (m_iTechLevel > 8)
                m_iTechLevel = 8;

            if (Rnd.OneChanceFrom(2))
                m_iBioLevel = pSociety.BioLevel + iBioLevel;
            else
                m_iBioLevel = pSociety.BioLevel - iBioLevel;

            if (m_iBioLevel < 0)
                m_iBioLevel = 0;
            if (m_iBioLevel > 8)
                m_iBioLevel = 8;

            if (Rnd.OneChanceFrom(2))
                m_iCultureLevel = pSociety.CultureLevel + iCultureLevel;
            else
                m_iCultureLevel = pSociety.CultureLevel - iCultureLevel;

            if (m_iCultureLevel < 0)
                m_iCultureLevel = 0;
            if (m_iCultureLevel > 8)
                m_iCultureLevel = 8;

            int iBody = (int)(Math.Pow(Rnd.Get(20), 2) / 100);
            int iMind = (int)(Math.Pow(Rnd.Get(20), 2) / 100);

            if (Rnd.OneChanceFrom(2))
                m_iBaseBody = pSociety.BaseBody + iBody;
            else
                m_iBaseBody = pSociety.BaseBody - iBody;

            if (m_iBaseBody < 0)
                m_iBaseBody = 0;
            if (m_iBaseBody > 8)
                m_iBaseBody = 8;

            if (Rnd.OneChanceFrom(2))
                m_iBaseMind = pSociety.BaseMind + iMind;
            else
                m_iBaseMind = pSociety.BaseMind - iMind;

            if (m_iBaseMind < 0)
                m_iBaseMind = 0;
            if (m_iBaseMind > 8)
                m_iBaseMind = 8;

            if (Rnd.OneChanceFrom(5))
            {
                int iGender = Rnd.Get(Enum.GetValues(typeof(GenderPriority)).Length());
                m_eGenderPriority = Enum.GetValues(typeof(GenderPriority)).GetValue(iGender);
            }
            else
                m_eGenderPriority = pSociety.GenderPriority;
        }
    }
}
