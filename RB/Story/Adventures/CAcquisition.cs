using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Socium;

namespace RB.Story.Adventures
{
    public class CAcquisition : CAdventure
    {
        private int m_iKeysCount;
        public int KeysCount
        {
            get { return m_iKeysCount; }
        }

        private List<CSeekingSimple> m_cKeysSeeking = new List<CSeekingSimple>();
        public List<CSeekingSimple> KeysSeeking
        {
            get { return m_cKeysSeeking; }
        }

        private CChallenge m_pChallenge;
        internal CChallenge FinalChallenge
        {
            get { return m_pChallenge; }
        }

        public CAcquisition(CPerson pKeeper, int iKeysCount)
        {
            m_sName = "Обретение";

            m_iKeysCount = iKeysCount;

            if (pKeeper != null)
                m_pChallenge = new CChallenge(pKeeper, CChallenge.Importance.Optional, CChallenge.Occurance.Once);
        }
    }
}
