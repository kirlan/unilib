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

        public string Name
        {
            get { return m_pEpoch.m_sName; }
            set { m_pEpoch.m_sName = value; }
        }

        public int Length
        {
            get { return m_pEpoch.m_iLength; }
            set { m_pEpoch.m_iLength = value; }
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
                    m_pEpoch.m_iNativesMinTechLevel = m_pNativesPreset.m_iMinTechLevel;
                    m_pEpoch.m_iNativesMaxTechLevel = m_pNativesPreset.m_iMaxTechLevel;
                    m_pEpoch.m_iNativesMinMagicLevel = m_pNativesPreset.m_iMinMagicLevel;
                    m_pEpoch.m_iNativesMaxMagicLevel = m_pNativesPreset.m_iMaxMagicLevel;
                }
            }
        }

        public int NativesMinTechLevel
        {
            get { return m_pEpoch.m_iNativesMinTechLevel; }
            set
            {
                if (m_pNativesPreset == null)
                    m_pEpoch.m_iNativesMinTechLevel = value;
                else
                    if (value != m_pNativesPreset.m_iMinTechLevel)
                    {
                        m_pEpoch.m_iNativesMinTechLevel = value;
                        m_pNativesPreset = null;
                    }
            }
        }

        public int NativesMaxTechLevel
        {
            get { return m_pEpoch.m_iNativesMaxTechLevel; }
            set
            {
                if (m_pNativesPreset == null)
                    m_pEpoch.m_iNativesMaxTechLevel = value;
                else
                    if (value != m_pNativesPreset.m_iMaxTechLevel)
                    {
                        m_pEpoch.m_iNativesMaxTechLevel = value;
                        m_pNativesPreset = null;
                    }
            }
        }

        public int NativesMinMagicLevel
        {
            get { return m_pEpoch.m_iNativesMinMagicLevel; }
            set
            {
                if (m_pNativesPreset == null)
                    m_pEpoch.m_iNativesMinMagicLevel = value;
                else
                    if (value != m_pNativesPreset.m_iMinMagicLevel)
                    {
                        m_pEpoch.m_iNativesMinMagicLevel = value;
                        m_pNativesPreset = null;
                    }
            }
        }

        public int NativesMaxMagicLevel
        {
            get { return m_pEpoch.m_iNativesMaxMagicLevel; }
            set
            {
                if (m_pNativesPreset == null)
                    m_pEpoch.m_iNativesMaxMagicLevel = value;
                else
                    if (value != m_pNativesPreset.m_iMaxMagicLevel)
                    {
                        m_pEpoch.m_iNativesMaxMagicLevel = value;
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
                    return string.Format("Custom (T[{0}-{1}]M[{2}-{3}])", m_pEpoch.m_iNativesMinTechLevel, m_pEpoch.m_iNativesMaxTechLevel, m_pEpoch.m_iNativesMinMagicLevel, m_pEpoch.m_iNativesMaxMagicLevel);
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
                    m_pEpoch.m_iInvadersMinTechLevel = m_pInvadersPreset.m_iMinTechLevel;
                    m_pEpoch.m_iInvadersMaxTechLevel = m_pInvadersPreset.m_iMaxTechLevel;
                    m_pEpoch.m_iInvadersMinMagicLevel = m_pInvadersPreset.m_iMinMagicLevel;
                    m_pEpoch.m_iInvadersMaxMagicLevel = m_pInvadersPreset.m_iMaxMagicLevel;
                }
            }
        }

        public int InvadersMinTechLevel
        {
            get { return m_pEpoch.m_iInvadersMinTechLevel; }
            set
            {
                if (m_pInvadersPreset == null)
                    m_pEpoch.m_iInvadersMinTechLevel = value;
                else
                    if (value != m_pInvadersPreset.m_iMinTechLevel)
                    {
                        m_pEpoch.m_iInvadersMinTechLevel = value;
                        m_pInvadersPreset = null;
                    }
            }
        }

        public int InvadersMaxTechLevel
        {
            get { return m_pEpoch.m_iInvadersMaxTechLevel; }
            set
            {
                if (m_pInvadersPreset == null)
                    m_pEpoch.m_iInvadersMaxTechLevel = value;
                else
                    if (value != m_pInvadersPreset.m_iMaxTechLevel)
                    {
                        m_pEpoch.m_iInvadersMaxTechLevel = value;
                        m_pInvadersPreset = null;
                    }
            }
        }

        public int InvadersMinMagicLevel
        {
            get { return m_pEpoch.m_iInvadersMinMagicLevel; }
            set
            {
                if (m_pInvadersPreset == null)
                    m_pEpoch.m_iInvadersMinMagicLevel = value;
                else
                    if (value != m_pInvadersPreset.m_iMinMagicLevel)
                    {
                        m_pEpoch.m_iInvadersMinMagicLevel = value;
                        m_pInvadersPreset = null;
                    }
            }
        }

        public int InvadersMaxMagicLevel
        {
            get { return m_pEpoch.m_iInvadersMaxMagicLevel; }
            set
            {
                if (m_pInvadersPreset == null)
                    m_pEpoch.m_iInvadersMaxMagicLevel = value;
                else
                    if (value != m_pInvadersPreset.m_iMaxMagicLevel)
                    {
                        m_pEpoch.m_iInvadersMaxMagicLevel = value;
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
                            if (pRace.m_sName == sName)
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

                m_pEpoch.m_cNatives.Clear();
                m_pEpoch.m_cNatives.AddRange(cRaces);
            }
        }

        public List<Race> NativesRaces
        {
            get { return new List<Race>(m_pEpoch.m_cNatives); }
            set
            {
                m_aNativesRacesSets = new RacesSet[] { };
                m_pEpoch.m_cNatives.Clear();
                m_pEpoch.m_cNatives.AddRange(value);
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
                    foreach (Race pRace in m_pEpoch.m_cNatives)
                    {
                        if (sResult.Length > 0)
                            sResult += ", ";

                        sResult += pRace.m_sName;
                    }
                }

                return sResult;
            }
        }

        public int NativesCount
        {
            get { return m_pEpoch.m_iNativesCount; }
            set { m_pEpoch.m_iNativesCount = value; }
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
                            if (pRace.m_sName == sName)
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

                m_pEpoch.m_cInvaders.Clear();
                m_pEpoch.m_cInvaders.AddRange(cRaces);
            }
        }

        public List<Race> InvadersRaces
        {
            get { return new List<Race>(m_pEpoch.m_cInvaders); }
            set
            {
                m_aInvadersRacesSets = new RacesSet[] { };
                m_pEpoch.m_cInvaders.Clear();
                m_pEpoch.m_cInvaders.AddRange(value);
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
                    foreach (Race pRace in m_pEpoch.m_cInvaders)
                    {
                        if (sResult.Length > 0)
                            sResult += ", ";

                        sResult += pRace.m_sName;
                    }
                }

                return sResult;
            }
        }
        
        public int InvadersCount
        {
            get { return m_pEpoch.m_iInvadersCount; }
            set { m_pEpoch.m_iInvadersCount = value; }
        }
    }
}
