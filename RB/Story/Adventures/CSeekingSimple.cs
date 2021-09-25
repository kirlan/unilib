using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Socium;

namespace RB.Story.Adventures
{
    public class CSeekingSimple : CAdventure
    {
        private CChallenge m_pChallenge;
        internal CChallenge FinalChallenge
        {
            get { return m_pChallenge; }
        }

        private List<CAdventure> m_cAdventures = new List<CAdventure>();
        public List<CAdventure> SeekingSteps
        {
            get { return m_cAdventures; }
        }

        public CSeekingSimple(CPerson pKeeper)
        {
            m_sName = "Поиск";

            m_pChallenge = new CChallenge(pKeeper, CChallenge.Importance.Optional, CChallenge.Occurance.Once);
        }
    }
}
