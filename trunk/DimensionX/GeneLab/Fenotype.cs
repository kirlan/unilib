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
        public HideGenetix m_pHide;
        public BodyGenetix m_pBody;
        public BrainGenetix m_pBrain;
        public LifeCycleGenetix m_pLifeCycle;
        public HeadGenetix m_pHead;

        public GenetixBase MutateRace()
        {
            m_pLegs = (LegsGenetix)m_pLegs.MutateRace();
            m_pArms = (ArmsGenetix)m_pArms.MutateRace();
            m_pWings = (WingsGenetix)m_pWings.MutateRace();
            m_pTail = (TailGenetix)m_pTail.MutateRace();
            m_pHide = (HideGenetix)m_pHide.MutateRace();
            m_pBody = (BodyGenetix)m_pBody.MutateRace();
            m_pBrain = (BrainGenetix)m_pBrain.MutateRace();
            m_pLifeCycle = (LifeCycleGenetix)m_pLifeCycle.MutateRace();
            m_pHead = (HeadGenetix)m_pHead.MutateRace();

            return this;
        }

        public GenetixBase MutateNation()
        {
            m_pLegs = (LegsGenetix)m_pLegs.MutateNation();
            m_pArms = (ArmsGenetix)m_pArms.MutateNation();
            m_pWings = (WingsGenetix)m_pWings.MutateNation();
            m_pTail = (TailGenetix)m_pTail.MutateNation();
            m_pHide = (HideGenetix)m_pHide.MutateNation();
            m_pBody = (BodyGenetix)m_pBody.MutateNation();
            m_pBrain = (BrainGenetix)m_pBrain.MutateNation();
            m_pLifeCycle = (LifeCycleGenetix)m_pLifeCycle.MutateNation();
            m_pHead = (HeadGenetix)m_pHead.MutateNation();

            return this;
        }

        public GenetixBase MutateFamily()
        {
            m_pLegs = (LegsGenetix)m_pLegs.MutateFamily();
            m_pArms = (ArmsGenetix)m_pArms.MutateFamily();
            m_pWings = (WingsGenetix)m_pWings.MutateFamily();
            m_pTail = (TailGenetix)m_pTail.MutateFamily();
            m_pHide = (HideGenetix)m_pHide.MutateFamily();
            m_pBody = (BodyGenetix)m_pBody.MutateFamily();
            m_pBrain = (BrainGenetix)m_pBrain.MutateFamily();
            m_pLifeCycle = (LifeCycleGenetix)m_pLifeCycle.MutateFamily();
            m_pHead = (HeadGenetix)m_pHead.MutateFamily();

            return this;
        }

        public GenetixBase MutateIndividual()
        {
            m_pLegs = (LegsGenetix)m_pLegs.MutateIndividual();
            m_pArms = (ArmsGenetix)m_pArms.MutateIndividual();
            m_pWings = (WingsGenetix)m_pWings.MutateIndividual();
            m_pTail = (TailGenetix)m_pTail.MutateIndividual();
            m_pHide = (HideGenetix)m_pHide.MutateIndividual();
            m_pBody = (BodyGenetix)m_pBody.MutateIndividual();
            m_pBrain = (BrainGenetix)m_pBrain.MutateIndividual();
            m_pLifeCycle = (LifeCycleGenetix)m_pLifeCycle.MutateIndividual();
            m_pHead = (HeadGenetix)m_pHead.MutateIndividual();

            return this;
        }
    }
}
