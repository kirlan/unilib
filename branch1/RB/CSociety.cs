using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;

namespace RB
{
    /// <summary>
    /// Сообщество — это совокупность людей, связанных единой властной иерархией. 
    /// Это может быть государство, банда, тайный орден, варварское племя и т.д.
    /// 
    /// Каждое сообщество имеет свой собственный уровень культурного развития, а так же 
    /// собственную индустриальную базу, определяющую доступный членам сообщества спектр 
    /// технического и биологического инструментария. В общем случае уровень культуры и 
    /// индустриальной базы достаточно близко соответствуют базовым уровням развития мира, 
    /// но могут случаться и достаточно значимые отклонения, так в одном мире могут сочетаться 
    /// феодальное государство и высокотехнологичные пришельцы со звёзд, технологическая и 
    /// биологическая цивилизации и т.д.
    /// </summary>
    class CSociety
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

        private CSocietyType m_pType;

        /// <summary>
        /// Властная иерархия сообщества представляется системой рангов, определяющих 
        /// положение различных сословий в обществе и меру доступа их членов к принадлежащим 
        /// сообществу материальным благам. Лидером сообщества может быть только представитель 
        /// наиболее высокого сословия.
        /// 
        /// Система рангов определяется типом сообщества и не зависит от уровней развития 
        /// сообщества. Ранги считаются снизу (т.е. от наиболее бесправных и презираемых 
        /// членов общества) и разные типы сообществ могут иметь различную «высоту» 
        /// максимального доступного ранга.
        /// </summary>
        private Dictionary<CEstate, int> m_cRanks = new Dictionary<CEstate, int>();

        internal Dictionary<CEstate, int> Ranks
        {
            get { return m_cRanks; }
        }

        private List<CLocation> m_cLands = new List<CLocation>();

        internal List<CLocation> Lands
        {
            get { return m_cLands; }
        }

        private List<CLocation> m_cSettlements = new List<CLocation>();

        internal List<CLocation> Settlements
        {
            get { return m_cSettlements; }
        }

        private string m_sName;

        public string Name
        {
            get { return m_pType.Name + " " + Name; }
        }

        public CSociety(CWorld pHomeWorld)
        {
            int iTechLevel = (int)(Math.Pow(Rnd.Get(20), 2) / 100);
            int iBioLevel = (int)(Math.Pow(Rnd.Get(20), 2) / 100);
            int iCultureLevel = (int)(Math.Pow(Rnd.Get(20), 2) / 100);

            m_iBaseBody = Rnd.Get(5) + (int)(Math.Pow(Rnd.Get(20), 2) / 100);
            m_iBaseMind = Rnd.Get(5) + (int)(Math.Pow(Rnd.Get(20), 2) / 100);

            if (Rnd.OneChanceFrom(5))
            {
                int iGender = Rnd.Get(Enum.GetValues(typeof(GenderPriority)).Length());
                m_eGenderPriority = Enum.GetValues(typeof(GenderPriority)).GetValue(iGender);
            }
            else
                m_eGenderPriority = pHomeWorld.GenderPriority;

            if (Rnd.OneChanceFrom(2))
                m_iTechLevel = pHomeWorld.TechLevel + iTechLevel;
            else
                m_iTechLevel = pHomeWorld.TechLevel - iTechLevel;

            if (m_iTechLevel < 0)
                m_iTechLevel = 0;
            if (m_iTechLevel > 8)
                m_iTechLevel = 8;

            if (Rnd.OneChanceFrom(2))
                m_iBioLevel = pHomeWorld.BioLevel + iBioLevel;
            else
                m_iBioLevel = pHomeWorld.BioLevel - iBioLevel;

            if (m_iBioLevel < 0)
                m_iBioLevel = 0;
            if (m_iBioLevel > 8)
                m_iBioLevel = 8;

            if (Rnd.OneChanceFrom(2))
                m_iCultureLevel = pHomeWorld.CultureLevel + iCultureLevel;
            else
                m_iCultureLevel = pHomeWorld.CultureLevel - iCultureLevel;

            if (m_iCultureLevel < 0)
                m_iCultureLevel = 0;
            if (m_iCultureLevel > 8)
                m_iCultureLevel = 8;

            m_sName = NameGenerator.GetAbstractName(Rnd.Get(int.MaxValue));

            m_pType = new CSocietyType();

            for (int i = 0; i < m_pType.Ranks.Count; i++)
                AddEstate(i);
        }

        public CEstate AddEstate(int iRank)
        {
            CEstate pNewEstate = new CEstate(this);
            m_cRanks[pNewEstate] = iRank;
            return pNewEstate;
        }
    }
}
