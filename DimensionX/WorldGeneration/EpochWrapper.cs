using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Socium;
using Socium.Nations;

namespace WorldGeneration
{
    public class EpochWrapper
    {
        private Epoch m_pEpoch = new Epoch();

        public Epoch GetEpochInfo()
        {
            return m_pEpoch;
        }

        public EpochWrapper()
        { }

        public EpochWrapper(Epoch pEpoch)
        {
            m_pEpoch = pEpoch;
        }

        public string Name
        {
            get { return m_pEpoch.Name; }
            set { m_pEpoch.Name = value; }
        }

        public int Length
        {
            get { return m_pEpoch.Length; }
            set { m_pEpoch.Length = value; }
        }

        private ProgressPreset m_pNativesPreset = null;

        internal ProgressPreset NativesPreset
        {
            get { return m_pNativesPreset; }
            set
            {
                m_pNativesPreset = value;

                if (m_pNativesPreset != null)
                {
                    m_pEpoch.NativesMinTechLevel = m_pNativesPreset.m_iMinTechLevel;
                    m_pEpoch.NativesMaxTechLevel = m_pNativesPreset.m_iMaxTechLevel;
                    m_pEpoch.NativesMinMagicLevel = m_pNativesPreset.m_iMinMagicLevel;
                    m_pEpoch.NativesMaxMagicLevel = m_pNativesPreset.m_iMaxMagicLevel;
                }
            }
        }

        public int NativesMinTechLevel
        {
            get { return m_pEpoch.NativesMinTechLevel; }
            set
            {
                if (m_pNativesPreset == null)
                    m_pEpoch.NativesMinTechLevel = value;
                else
                    if (value != m_pNativesPreset.m_iMinTechLevel)
                    {
                        m_pEpoch.NativesMinTechLevel = value;
                        m_pNativesPreset = null;
                    }
            }
        }

        public int NativesMaxTechLevel
        {
            get { return m_pEpoch.NativesMaxTechLevel; }
            set
            {
                if (m_pNativesPreset == null)
                    m_pEpoch.NativesMaxTechLevel = value;
                else
                    if (value != m_pNativesPreset.m_iMaxTechLevel)
                    {
                        m_pEpoch.NativesMaxTechLevel = value;
                        m_pNativesPreset = null;
                    }
            }
        }

        public int NativesMinMagicLevel
        {
            get { return m_pEpoch.NativesMinMagicLevel; }
            set
            {
                if (m_pNativesPreset == null)
                    m_pEpoch.NativesMinMagicLevel = value;
                else
                    if (value != m_pNativesPreset.m_iMinMagicLevel)
                    {
                        m_pEpoch.NativesMinMagicLevel = value;
                        m_pNativesPreset = null;
                    }
            }
        }

        public int NativesMaxMagicLevel
        {
            get { return m_pEpoch.NativesMaxMagicLevel; }
            set
            {
                if (m_pNativesPreset == null)
                    m_pEpoch.NativesMaxMagicLevel = value;
                else
                    if (value != m_pNativesPreset.m_iMaxMagicLevel)
                    {
                        m_pEpoch.NativesMaxMagicLevel = value;
                        m_pNativesPreset = null;
                    }
            }
        }

        public string NativesPresetString
        {
            get
            {
                if (m_pNativesPreset != null)
                    return m_pNativesPreset.m_sName;
                else
                    return string.Format("Custom (T[{0}-{1}]M[{2}-{3}])", m_pEpoch.NativesMinTechLevel, m_pEpoch.NativesMaxTechLevel, m_pEpoch.NativesMinMagicLevel, m_pEpoch.NativesMaxMagicLevel);
            }
        }

        private ProgressPreset m_pInvadersPreset = null;

        internal ProgressPreset InvadersPreset
        {
            get { return m_pInvadersPreset; }
            set
            {
                m_pInvadersPreset = value;

                if (m_pInvadersPreset != null)
                {
                    m_pEpoch.InvadersMinTechLevel = m_pInvadersPreset.m_iMinTechLevel;
                    m_pEpoch.InvadersMaxTechLevel = m_pInvadersPreset.m_iMaxTechLevel;
                    m_pEpoch.InvadersMinMagicLevel = m_pInvadersPreset.m_iMinMagicLevel;
                    m_pEpoch.InvadersMaxMagicLevel = m_pInvadersPreset.m_iMaxMagicLevel;
                }
            }
        }

        public int InvadersMinTechLevel
        {
            get { return m_pEpoch.InvadersMinTechLevel; }
            set
            {
                if (m_pInvadersPreset == null)
                    m_pEpoch.InvadersMinTechLevel = value;
                else
                    if (value != m_pInvadersPreset.m_iMinTechLevel)
                    {
                        m_pEpoch.InvadersMinTechLevel = value;
                        m_pInvadersPreset = null;
                    }
            }
        }

        public int InvadersMaxTechLevel
        {
            get { return m_pEpoch.InvadersMaxTechLevel; }
            set
            {
                if (m_pInvadersPreset == null)
                    m_pEpoch.InvadersMaxTechLevel = value;
                else
                    if (value != m_pInvadersPreset.m_iMaxTechLevel)
                    {
                        m_pEpoch.InvadersMaxTechLevel = value;
                        m_pInvadersPreset = null;
                    }
            }
        }

        public int InvadersMinMagicLevel
        {
            get { return m_pEpoch.InvadersMinMagicLevel; }
            set
            {
                if (m_pInvadersPreset == null)
                    m_pEpoch.InvadersMinMagicLevel = value;
                else
                    if (value != m_pInvadersPreset.m_iMinMagicLevel)
                    {
                        m_pEpoch.InvadersMinMagicLevel = value;
                        m_pInvadersPreset = null;
                    }
            }
        }

        public int InvadersMaxMagicLevel
        {
            get { return m_pEpoch.InvadersMaxMagicLevel; }
            set
            {
                if (m_pInvadersPreset == null)
                    m_pEpoch.InvadersMaxMagicLevel = value;
                else
                    if (value != m_pInvadersPreset.m_iMaxMagicLevel)
                    {
                        m_pEpoch.InvadersMaxMagicLevel = value;
                        m_pInvadersPreset = null;
                    }
            }
        }

        private RacesSet[] m_aNativesRacesSets = { };

        public RacesSet[] NativesRacesSets
        {
            get { return m_aNativesRacesSets; }
            set
            {
                m_aNativesRacesSets = value;
                List<Race> cRaces = new List<Race>();
                foreach (Race pRace in Race.m_cAllRaces)
                {
                    bool bPresent = false;
                    foreach (RacesSet pSet in m_aNativesRacesSets)
                    {
                        foreach (string sName in pSet.m_aRaces)
                            if (pRace.Name == sName)
                            {
                                bPresent = true;
                                break;
                            }

                        if (bPresent)
                            break;
                    }

                    if (bPresent)
                        cRaces.Add(pRace);
                }

                m_pEpoch.Natives.Clear();
                m_pEpoch.Natives.AddRange(cRaces);
            }
        }

        public List<Race> NativesRaces
        {
            get { return new List<Race>(m_pEpoch.Natives); }
            set
            {
                m_aNativesRacesSets = new RacesSet[] { };
                m_pEpoch.Natives.Clear();
                m_pEpoch.Natives.AddRange(value);
            }
        }

        public string NativesRacesString
        {
            get 
            {
                string sResult = "";

                if (m_aNativesRacesSets.Length > 0)
                {
                    foreach (RacesSet pSet in m_aNativesRacesSets)
                    {
                        if (sResult.Length > 0)
                            sResult += ", ";

                        sResult += pSet.m_sName;
                    }
                }
                else
                {
                    foreach (Race pRace in m_pEpoch.Natives)
                    {
                        if (sResult.Length > 0)
                            sResult += ", ";

                        sResult += pRace.Name;
                    }
                }

                return sResult;
            }
        }

        public int NativesCount
        {
            get { return m_pEpoch.NativesCount; }
            set { m_pEpoch.NativesCount = value; }
        }

        private RacesSet[] m_aInvadersRacesSets = { };

        public RacesSet[] InvadersRacesSets
        {
            get { return m_aInvadersRacesSets; }
            set
            {
                m_aInvadersRacesSets = value;
                List<Race> cRaces = new List<Race>();
                foreach (Race pRace in Race.m_cAllRaces)
                {
                    bool bPresent = false;
                    foreach (RacesSet pSet in m_aInvadersRacesSets)
                    {
                        foreach (string sName in pSet.m_aRaces)
                            if (pRace.Name == sName)
                            {
                                bPresent = true;
                                break;
                            }

                        if (bPresent)
                            break;
                    }

                    if (bPresent)
                        cRaces.Add(pRace);
                }

                m_pEpoch.Invaders.Clear();
                m_pEpoch.Invaders.AddRange(cRaces);
            }
        }

        public List<Race> InvadersRaces
        {
            get { return new List<Race>(m_pEpoch.Invaders); }
            set
            {
                m_aInvadersRacesSets = new RacesSet[] { };
                m_pEpoch.Invaders.Clear();
                m_pEpoch.Invaders.AddRange(value);
            }
        }

        public string InvadersRacesString
        {
            get
            {
                string sResult = "";

                if (m_aInvadersRacesSets.Length > 0)
                {
                    foreach (RacesSet pSet in m_aInvadersRacesSets)
                    {
                        if (sResult.Length > 0)
                            sResult += ", ";

                        sResult += pSet.m_sName;
                    }
                }
                else
                {
                    foreach (Race pRace in m_pEpoch.Invaders)
                    {
                        if (sResult.Length > 0)
                            sResult += ", ";

                        sResult += pRace.Name;
                    }
                }

                return sResult;
            }
        }
        
        public int InvadersCount
        {
            get { return m_pEpoch.InvadersCount; }
            set { m_pEpoch.InvadersCount = value; }
        }
    }
}
