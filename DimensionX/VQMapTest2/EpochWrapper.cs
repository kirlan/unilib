using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Socium;

namespace VQMapTest2
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

        private SocietyPreset m_pNativesPreset = null;

        internal SocietyPreset NativesPreset
        {
            get { return m_pNativesPreset; }
            set
            {
                m_pNativesPreset = value;

                m_pEpoch.m_iMinTechLevel = m_pNativesPreset.m_iMinTechLevel;
                m_pEpoch.m_iMaxTechLevel = m_pNativesPreset.m_iMaxTechLevel;
                m_pEpoch.m_iMinMagicLevel = m_pNativesPreset.m_iMinMagicLevel;
                m_pEpoch.m_iMaxMagicLevel = m_pNativesPreset.m_iMaxMagicLevel;
            }
        }

        public int MinTechLevel
        {
            set
            {
                if (m_pNativesPreset == null)
                    m_pEpoch.m_iMinTechLevel = value;
                else
                    if (value != m_pNativesPreset.m_iMinTechLevel)
                    {
                        m_pEpoch.m_iMinTechLevel = value;
                        m_pNativesPreset = null;
                    }
            }
        }

        public int MaxTechLevel
        {
            set
            {
                if (m_pNativesPreset == null)
                    m_pEpoch.m_iMaxTechLevel = value;
                else
                    if (value != m_pNativesPreset.m_iMaxTechLevel)
                    {
                        m_pEpoch.m_iMaxTechLevel = value;
                        m_pNativesPreset = null;
                    }
            }
        }

        public int MinMagicLevel
        {
            set
            {
                if (m_pNativesPreset == null)
                    m_pEpoch.m_iMinMagicLevel = value;
                else
                    if (value != m_pNativesPreset.m_iMinMagicLevel)
                    {
                        m_pEpoch.m_iMinMagicLevel = value;
                        m_pNativesPreset = null;
                    }
            }
        }

        public int MaxMagicLevel
        {
            set
            {
                if (m_pNativesPreset == null)
                    m_pEpoch.m_iMaxMagicLevel = value;
                else
                    if (value != m_pNativesPreset.m_iMaxMagicLevel)
                    {
                        m_pEpoch.m_iMaxMagicLevel = value;
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
                    return string.Format("T[{0}-{1}]M[{2}-{3}]", m_pEpoch.m_iMinTechLevel, m_pEpoch.m_iMaxTechLevel, m_pEpoch.m_iMinMagicLevel, m_pEpoch.m_iMaxMagicLevel);
            }
        }

        private SocietyPreset m_pInvadersPreset = null;

        internal SocietyPreset InvadersPreset
        {
            get { return m_pInvadersPreset; }
            set
            {
                m_pInvadersPreset = value;

                m_pEpoch.m_iInvadersMinTechLevel = m_pInvadersPreset.m_iMinTechLevel;
                m_pEpoch.m_iInvadersMaxTechLevel = m_pInvadersPreset.m_iMaxTechLevel;
                m_pEpoch.m_iInvadersMinMagicLevel = m_pInvadersPreset.m_iMinMagicLevel;
                m_pEpoch.m_iInvadersMaxMagicLevel = m_pInvadersPreset.m_iMaxMagicLevel;
            }
        }

        public int InvadersMinTechLevel
        {
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

        private RacesSet[] m_aRacesSets = { };

        public RacesSet[] NativesRacesSets
        {
            get { return m_aRacesSets; }
            set
            {
                m_aRacesSets = value;
                List<RaceTemplate> cRaces = new List<RaceTemplate>();
                foreach (RaceTemplate pTemplate in Race.m_cTemplates)
                {
                    bool bPresent = false;
                    foreach (RacesSet pSet in m_aRacesSets)
                    {
                        foreach (string sName in pSet.m_aRaces)
                            if (pTemplate.m_sName == sName)
                            {
                                bPresent = true;
                                break;
                            }

                        if (bPresent)
                            break;
                    }

                    if (bPresent)
                        cRaces.Add(pTemplate);
                }

                m_pEpoch.m_cRaceTemplates.Clear();
                m_pEpoch.m_cRaceTemplates.AddRange(cRaces);
            }
        }

        public List<RaceTemplate> NativesRaces
        {
            get { return new List<RaceTemplate>(m_pEpoch.m_cRaceTemplates); }
            set
            {
                m_aRacesSets = new RacesSet[] { };
                m_pEpoch.m_cRaceTemplates.Clear();
                m_pEpoch.m_cRaceTemplates.AddRange(value);
            }
        }

        private RacesSet[] m_aInvadersRacesSets = { };

        public RacesSet[] InvadersRacesSets
        {
            get { return m_aInvadersRacesSets; }
            set
            {
                m_aInvadersRacesSets = value;
                List<RaceTemplate> cRaces = new List<RaceTemplate>();
                foreach (RaceTemplate pTemplate in Race.m_cTemplates)
                {
                    bool bPresent = false;
                    foreach (RacesSet pSet in m_aInvadersRacesSets)
                    {
                        foreach (string sName in pSet.m_aRaces)
                            if (pTemplate.m_sName == sName)
                            {
                                bPresent = true;
                                break;
                            }

                        if (bPresent)
                            break;
                    }

                    if (bPresent)
                        cRaces.Add(pTemplate);
                }

                m_pEpoch.m_cInvadersRaceTemplates.Clear();
                m_pEpoch.m_cInvadersRaceTemplates.AddRange(cRaces);
            }
        }

        public List<RaceTemplate> InvadersRaces
        {
            get { return new List<RaceTemplate>(m_pEpoch.m_cInvadersRaceTemplates); }
            set
            {
                m_aInvadersRacesSets = new RacesSet[] { };
                m_pEpoch.m_cInvadersRaceTemplates.Clear();
                m_pEpoch.m_cInvadersRaceTemplates.AddRange(value);
            }
        }
    }
}
