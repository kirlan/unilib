using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneLab.Genetix;

namespace GeneLab
{
    public class Fenotype: GenetixBase
    {
        public LegsGenetix m_pLegs;
        public ArmsGenetix m_pArms;
        public WingsGenetix m_pWings;
        public TailGenetix m_pTail;

        public GenetixBase MutateRace()
        {
            m_pLegs = (LegsGenetix)m_pLegs.MutateRace();
            m_pArms = (ArmsGenetix)m_pArms.MutateRace();
            m_pWings = (WingsGenetix)m_pWings.MutateRace();
            m_pTail = (TailGenetix)m_pTail.MutateRace();

            return this;
        }

        public GenetixBase MutateNation()
        {
            m_pLegs = (LegsGenetix)m_pLegs.MutateNation();
            m_pArms = (ArmsGenetix)m_pArms.MutateNation();
            m_pWings = (WingsGenetix)m_pWings.MutateNation();
            m_pTail = (TailGenetix)m_pTail.MutateNation();

            return this;
        }

        public GenetixBase MutateFamily()
        {
            m_pLegs = (LegsGenetix)m_pLegs.MutateFamily();
            m_pArms = (ArmsGenetix)m_pArms.MutateFamily();
            m_pWings = (WingsGenetix)m_pWings.MutateFamily();
            m_pTail = (TailGenetix)m_pTail.MutateFamily();

            return this;
        }

        public GenetixBase MutateIndividual()
        {
            m_pLegs = (LegsGenetix)m_pLegs.MutateIndividual();
            m_pArms = (ArmsGenetix)m_pArms.MutateIndividual();
            m_pWings = (WingsGenetix)m_pWings.MutateIndividual();
            m_pTail = (TailGenetix)m_pTail.MutateIndividual();

            return this;
        }
    }
}
