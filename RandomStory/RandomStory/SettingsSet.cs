using System;
using System.Collections.Generic;
using System.Text;
using Random;

namespace RandomStory
{
    public class SettingsSet
    {
        private List<Setting> m_cPrime;
        private List<Setting> m_cAllowed;
        private List<Setting> m_cCross;

        public SettingsSet(List<Setting> cPrimedWorlds, List<Setting> cAllowedWorlds)
        {
            m_cPrime = cPrimedWorlds;
            m_cAllowed = cAllowedWorlds;

            if (m_cPrime != null && m_cAllowed != null)
            {
                m_cCross = new List<Setting>(m_cPrime);
                m_cCross.AddRange(m_cAllowed);
            }
        }

        public Setting GetPrime()
        {
            if (m_cPrime == null || m_cPrime.Count == 0)
                return null;

            Setting pWorld = m_cPrime[0];
            for (int i = 1; i < m_cPrime.Count; i++)
            {
                if (!pWorld.Equals(m_cPrime[i]))
                    pWorld = new Setting(pWorld, m_cPrime[i]);
            }

            foreach (Setting pOriginal in m_cPrime)
                if (pOriginal.Equals(pWorld))
                    return pOriginal;

            m_cPrime.Add(pWorld);
            return pWorld;
        }

        public Setting GetRandom(int iCrossProbability)
        {
            List<Setting> cSettings = m_cAllowed;

            if (cSettings == null || cSettings.Count == 0)
                cSettings = m_cPrime;

            if (cSettings == null || cSettings.Count == 0)
                return null;

            if (Rnd.OneChanceFrom(iCrossProbability) && m_cPrime != null && m_cPrime.Count > 0)
            {
                List<Setting> cMerged = new List<Setting>(m_cPrime);
                cMerged.AddRange(m_cAllowed);

                if (iCrossProbability < 2)
                    iCrossProbability = 2;

                Setting pSetting1 = Rnd.OneChanceFrom(iCrossProbability) ? GetRandom(iCrossProbability * iCrossProbability) : cSettings[Rnd.Get(cSettings.Count)];
                Setting pSetting2;
                int iCounter = 5;
                do
                {
                    pSetting2 = Rnd.OneChanceFrom(iCrossProbability) ? GetRandom(iCrossProbability * iCrossProbability) : cMerged[Rnd.Get(cMerged.Count)];
                    iCounter--;
                }
                while (pSetting1.Equals(pSetting2) && iCounter > 0);

                string sNewName = Setting.GetName(pSetting1, pSetting2);

                foreach (Setting pOriginal in m_cCross)
                    if (pOriginal.Equals(sNewName))
                        return pOriginal;

                Setting pNewSetting = new Setting(pSetting1, pSetting2);
                m_cCross.Add(pNewSetting);
                return pNewSetting;
            }
            else
                return cSettings[Rnd.Get(cSettings.Count)];
        }
    }
}
