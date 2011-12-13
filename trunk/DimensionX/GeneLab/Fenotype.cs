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
        public HairsGenetix m_pHairs;
        public EarsGenetix m_pEars;
        public EyesGenetix m_pEyes;
        public FaceGenetix m_pFace;

        public GenetixBase MutateRace()
        {
            Fenotype pMutant = new Fenotype();

            pMutant.m_pLegs = (LegsGenetix)m_pLegs.MutateRace();
            pMutant.m_pArms = (ArmsGenetix)m_pArms.MutateRace();
            pMutant.m_pWings = (WingsGenetix)m_pWings.MutateRace();
            pMutant.m_pTail = (TailGenetix)m_pTail.MutateRace();
            pMutant.m_pHide = (HideGenetix)m_pHide.MutateRace();
            pMutant.m_pBody = (BodyGenetix)m_pBody.MutateRace();
            pMutant.m_pBrain = (BrainGenetix)m_pBrain.MutateRace();
            pMutant.m_pLifeCycle = (LifeCycleGenetix)m_pLifeCycle.MutateRace();
            pMutant.m_pHead = (HeadGenetix)m_pHead.MutateRace();
            pMutant.m_pHairs = (HairsGenetix)m_pHairs.MutateRace();
            pMutant.m_pEars = (EarsGenetix)m_pEars.MutateRace();
            pMutant.m_pEyes = (EyesGenetix)m_pEyes.MutateRace();
            pMutant.m_pFace = (FaceGenetix)m_pFace.MutateRace();

            return pMutant;
        }

        public GenetixBase MutateNation()
        {
            Fenotype pMutant = new Fenotype();

            pMutant.m_pLegs = (LegsGenetix)m_pLegs.MutateNation();
            pMutant.m_pArms = (ArmsGenetix)m_pArms.MutateNation();
            pMutant.m_pWings = (WingsGenetix)m_pWings.MutateNation();
            pMutant.m_pTail = (TailGenetix)m_pTail.MutateNation();
            pMutant.m_pHide = (HideGenetix)m_pHide.MutateNation();
            pMutant.m_pBody = (BodyGenetix)m_pBody.MutateNation();
            pMutant.m_pBrain = (BrainGenetix)m_pBrain.MutateNation();
            pMutant.m_pLifeCycle = (LifeCycleGenetix)m_pLifeCycle.MutateNation();
            pMutant.m_pHead = (HeadGenetix)m_pHead.MutateNation();
            pMutant.m_pHairs = (HairsGenetix)m_pHairs.MutateNation();
            pMutant.m_pEars = (EarsGenetix)m_pEars.MutateNation();
            pMutant.m_pEyes = (EyesGenetix)m_pEyes.MutateNation();
            pMutant.m_pFace = (FaceGenetix)m_pFace.MutateNation();

            return pMutant;
        }

        public GenetixBase MutateFamily()
        {
            Fenotype pMutant = new Fenotype();

            pMutant.m_pLegs = (LegsGenetix)m_pLegs.MutateFamily();
            pMutant.m_pArms = (ArmsGenetix)m_pArms.MutateFamily();
            pMutant.m_pWings = (WingsGenetix)m_pWings.MutateFamily();
            pMutant.m_pTail = (TailGenetix)m_pTail.MutateFamily();
            pMutant.m_pHide = (HideGenetix)m_pHide.MutateFamily();
            pMutant.m_pBody = (BodyGenetix)m_pBody.MutateFamily();
            pMutant.m_pBrain = (BrainGenetix)m_pBrain.MutateFamily();
            pMutant.m_pLifeCycle = (LifeCycleGenetix)m_pLifeCycle.MutateFamily();
            pMutant.m_pHead = (HeadGenetix)m_pHead.MutateFamily();
            pMutant.m_pHairs = (HairsGenetix)m_pHairs.MutateFamily();
            pMutant.m_pEars = (EarsGenetix)m_pEars.MutateFamily();
            pMutant.m_pEyes = (EyesGenetix)m_pEyes.MutateFamily();
            pMutant.m_pFace = (FaceGenetix)m_pFace.MutateFamily();

            return pMutant;
        }

        public GenetixBase MutateIndividual()
        {
            Fenotype pMutant = new Fenotype();

            pMutant.m_pLegs = (LegsGenetix)m_pLegs.MutateIndividual();
            pMutant.m_pArms = (ArmsGenetix)m_pArms.MutateIndividual();
            pMutant.m_pWings = (WingsGenetix)m_pWings.MutateIndividual();
            pMutant.m_pTail = (TailGenetix)m_pTail.MutateIndividual();
            pMutant.m_pHide = (HideGenetix)m_pHide.MutateIndividual();
            pMutant.m_pBody = (BodyGenetix)m_pBody.MutateIndividual();
            pMutant.m_pBrain = (BrainGenetix)m_pBrain.MutateIndividual();
            pMutant.m_pLifeCycle = (LifeCycleGenetix)m_pLifeCycle.MutateIndividual();
            pMutant.m_pHead = (HeadGenetix)m_pHead.MutateIndividual();
            pMutant.m_pHairs = (HairsGenetix)m_pHairs.MutateIndividual();
            pMutant.m_pEars = (EarsGenetix)m_pEars.MutateIndividual();
            pMutant.m_pEyes = (EyesGenetix)m_pEyes.MutateIndividual();
            pMutant.m_pFace = (FaceGenetix)m_pFace.MutateIndividual();

            return pMutant;
        }
    }
}
