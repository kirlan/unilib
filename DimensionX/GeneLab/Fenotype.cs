using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneLab.Genetix;
using LandscapeGeneration;
using nsUniLibControls;

namespace GeneLab
{
    public abstract class Fenotype : GenetixBase
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

        protected Fenotype()
        { }

        public Fenotype(BodyGenetix pBody,
                        HeadGenetix pHead,
                        LegsGenetix pLegs,
                        ArmsGenetix pArms,
                        WingsGenetix pWings,
                        TailGenetix pTail,
                        HideGenetix pHide,
                        BrainGenetix pBrain,
                        LifeCycleGenetix pLifeCycle,
                        FaceGenetix pFace,
                        EarsGenetix pEars,
                        EyesGenetix pEyes,
                        HairsGenetix pHairs)
        {
            m_pBody = pBody;
            m_pHead = pHead;
            m_pLegs = pLegs;
            m_pArms = pArms;
            m_pWings = pWings;
            m_pTail = pTail;
            m_pHide = pHide;
            m_pBrain = pBrain;
            m_pLifeCycle = pLifeCycle;
            m_pFace = pFace;
            m_pEars = pEars;
            m_pEyes = pEyes;
            m_pHairs = pHairs;
            m_pHairs.CheckHairColors();
        }

        public abstract Fenotype GetHumanEtalon();
        public abstract string GetDescription();
        public abstract bool IsIdentical(GenetixBase pOther);
        public abstract GenetixBase MutateFamily();
        public abstract GenetixBase MutateIndividual();
        public abstract GenetixBase MutateNation();
        public abstract GenetixBase MutateRace();

        /// <summary>
        /// Max 11
        /// </summary>
        /// <param name="pOpponents"></param>
        /// <returns></returns>
        public int GetFenotypeDifference(Fenotype pOpponents)
        {
            string sPositiveReasons = "";
            string sNegativeReasons = "";

            return GetFenotypeDifference(pOpponents, ref sPositiveReasons, ref sNegativeReasons);
        }

        /// <summary>
        /// Max 11
        /// </summary>
        /// <param name="pOpponents"></param>
        /// <param name="sPositiveReasons"></param>
        /// <param name="sNegativeReasons"></param>
        /// <returns></returns>
        public int GetFenotypeDifference(Fenotype pOpponents, ref string sPositiveReasons, ref string sNegativeReasons)
        {
            int iHostility = 0;

            if (!m_pHead.IsIdentical(pOpponents.m_pHead))
            {
                iHostility++;
                sNegativeReasons += " (-1) [APP] ugly head\n";
            }

            int iBodyDiff = 0;
            if (!m_pArms.IsIdentical(pOpponents.m_pArms))
                iBodyDiff++;
            //if (!pNation.m_pLegs.IsIdentical(pOpponents.m_pLegs))
            //    iBodyDiff++;
            //if (!pNation.m_pTail.IsIdentical(pOpponents.m_pTail))
            //    iBodyDiff++;
            if (m_pHide.m_eHideType != pOpponents.m_pHide.m_eHideType)
                iBodyDiff++;

            if (iBodyDiff > 0)
            {
                iHostility += iBodyDiff;
                sNegativeReasons += string.Format(" (-{0}) [APP] ugly body\n", iBodyDiff);
            }

            if (Math.Abs(m_pBody.m_eBodyBuild - pOpponents.m_pBody.m_eBodyBuild) > 1)
            {
                iHostility++;
                sNegativeReasons += string.Format(" (-1) [APP] too {0}\n", pOpponents.m_pBody.m_eBodyBuild.ToString().ToLower());
            }

            //if (pNation.m_pHide.m_eHideType == pOpponents.m_pHide.m_eHideType)
            //{
            //    if (!pNation.m_pHide.IsIdentical(pOpponents.m_pHide))
            //    {
            //        iHostility++;
            //        sNegativeReasons += string.Format(" (-1) strange {0} body color\n", pOpponents.m_pHide.m_sHideColor + (pOpponents.m_pHide.m_bSpots ? (" with " + pOpponents.m_pHide.m_sSpotsColor) : ""));
            //    }
            //}

            int iFaceDiff = 0;
            if (!m_pEyes.IsIdentical(pOpponents.m_pEyes))
                iFaceDiff++;
            //if (!pNation.m_pEars.IsIdentical(pOpponents.m_pEars))
            //    iFaceDiff++;
            if (!m_pFace.IsIdentical(pOpponents.m_pFace))
                iFaceDiff++;
            if (iFaceDiff > 0)
            {
                iHostility += iFaceDiff;
                sNegativeReasons += string.Format(" (-{0}) [APP] ugly face\n", iBodyDiff);
            }

            //а вот тут - берём личные показатели
            if (m_pBody.m_eNutritionType != pOpponents.m_pBody.m_eNutritionType)
            {
                if (m_pBody.IsParasite())
                {
                    iHostility++;
                    sNegativeReasons += " (-1) [PSI] prey\n";
                }
                if (pOpponents.m_pBody.IsParasite())
                {
                    iHostility += 4;
                    sNegativeReasons += " (-4) [PSI] predator\n";
                }
            }

            return iHostility;
        }
    }

    public class Fenotype<LTI> : Fenotype
        where LTI: LandTypeInfo, new()
    {
        private readonly static Fenotype<LTI> s_HumanEtalon = new Fenotype<LTI>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.HumanReal,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                HairsGenetix.HumanWhite);

        public override Fenotype GetHumanEtalon()
        {
            return s_HumanEtalon;
        }

        #region String representation

        /// <summary>
        /// are very clever cratures ... could have only a few children during whole lifetime, which are mostly females.
        /// </summary>
        /// <returns></returns>
        public override string GetDescription() 
        {
            string sResult = GetComparsion(s_HumanEtalon);
            if (sResult.Length == 0)
                sResult = "are just humans.";

            if (!sResult.StartsWith("are"))
                sResult = "are quite common humans." + sResult;

            return sResult;
            //return "are " + m_pBrain.GetDescription() +
            //       ". They have " + m_pHead.GetDescription() +
            //       (m_pArms.m_eArmsCount != ArmsCount.None ? ", " + m_pArms.GetDescription() : "") +
            //       ", " + m_pLegs.GetDescription() +
            //       (m_pWings.m_eWingsCount != WingsCount.None ? ", " + m_pWings.GetDescription() : "") +
            //       (m_pTail.m_eTailLength != TailLength.None ? " and " + m_pTail.GetDescription() + ". " : ". ") +
            //       "They " + m_pBody.GetDescription() + ", " + m_pHide.GetDescription() + ". " +
            //       "On the head they have " + m_pEyes.GetDescription() + " and " + m_pEars.GetDescription() + " of a " + m_pFace.GetDescription() + ". " +
            //       m_pHairs.GetDescription() + " " +
            //       "Usually they " + m_pLifeCycle.GetDescription() + ".";
        }

        /// <summary>
        /// Возвращает строку, описывающую черты, отличиющие фенотип от заданного, примерный формат:
        /// "are very clever cratures ... could have only a few children during whole lifetime, which are mostly females."
        /// </summary>
        /// <returns></returns>
        public string GetComparsion(Fenotype<LTI> pOriginal)
        {
            if (pOriginal.IsIdentical(this))
                return "";

            string sResult = "";

            if (!pOriginal.m_pBrain.IsIdentical(m_pBrain))
                sResult += "are " + m_pBrain.GetDescription() + ".";

            if (!pOriginal.m_pHead.IsIdentical(m_pHead) ||
                !pOriginal.m_pArms.IsIdentical(m_pArms) ||
                !pOriginal.m_pLegs.IsIdentical(m_pLegs) ||
                !pOriginal.m_pWings.IsIdentical(m_pWings) ||
                !pOriginal.m_pTail.IsIdentical(m_pTail))
            {
                if (sResult != "")
                    sResult += " ";

                sResult += "They have ";

                bool bSemicolon = false;
                if (!pOriginal.m_pHead.IsIdentical(m_pHead))
                {
                    sResult += m_pHead.GetDescription();
                    bSemicolon = true;
                }

                if (!pOriginal.m_pArms.IsIdentical(m_pArms))
                {
                    if (m_pArms.m_eArmsCount != ArmsCount.None)
                    {
                        if (bSemicolon)
                            sResult += ", ";
                        sResult += m_pArms.GetDescription();

                        bSemicolon = true;
                    }
                    else
                    {
                        if (bSemicolon)
                            sResult += ", ";
                        sResult += "no arms";
                    }
                }

                if (!pOriginal.m_pLegs.IsIdentical(m_pLegs))
                {
                    if (bSemicolon)
                        sResult += ", ";
                    sResult += m_pLegs.GetDescription();

                    bSemicolon = true;
                }

                if (!pOriginal.m_pWings.IsIdentical(m_pWings))
                {
                    if (m_pWings.m_eWingsCount != WingsCount.None)
                    {
                        if (bSemicolon)
                            sResult += ", ";
                        sResult += m_pWings.GetDescription();

                        bSemicolon = true;
                    }
                    else
                    {
                        if (bSemicolon)
                            sResult += ", ";
                        sResult += "no wings";
                    }
                }

                if (!pOriginal.m_pTail.IsIdentical(m_pTail))
                {
                    if (m_pTail.m_eTailLength != TailLength.None)
                    {
                        if (bSemicolon)
                            sResult += " and ";
                        sResult += m_pTail.GetDescription();
                    }
                    else
                    {
                        if (bSemicolon)
                            sResult += " and ";
                        sResult += "no tail";
                    }
                }

                sResult += ".";
            }

            if (!pOriginal.m_pBody.IsIdentical(m_pBody) ||
                !pOriginal.m_pHide.IsIdentical(m_pHide))
            {
                if (sResult != "")
                    sResult += " ";

                bool bSemicolon = false;
                if (!pOriginal.m_pBody.IsIdentical(m_pBody))
                {
                    sResult += "They " + m_pBody.GetDescription();
                    bSemicolon = true;
                }
                else
                    sResult += "They have ";

                if(!pOriginal.m_pHide.IsIdentical(m_pHide))
                {
                    if (bSemicolon)
                        sResult += " with ";
                    sResult += m_pHide.GetDescription();
                }

                sResult += ".";
            }

            if (!pOriginal.m_pEyes.IsIdentical(m_pEyes) ||
                !pOriginal.m_pEars.IsIdentical(m_pEars) ||
                !pOriginal.m_pFace.IsIdentical(m_pFace))
            {
                if (sResult != "")
                    sResult += " ";

                bool bSemicolon = false;
                if (!pOriginal.m_pEyes.IsIdentical(m_pEyes) ||
                    !pOriginal.m_pEars.IsIdentical(m_pEars))
                {
                    sResult += "They have ";

                    if (!pOriginal.m_pEyes.IsIdentical(m_pEyes))
                    {
                        sResult += m_pEyes.GetDescription();
                        bSemicolon = true;
                    }

                    if (!pOriginal.m_pEars.IsIdentical(m_pEars))
                    {
                        if (bSemicolon)
                            sResult += " and ";

                        sResult += m_pEars.GetDescription() + " of a ";
                    }
                    else
                    {
                        if (bSemicolon && !pOriginal.m_pFace.IsIdentical(m_pFace))
                            sResult += " at the ";
                    }
                }
                else
                    sResult += "They have ";

                if (!pOriginal.m_pFace.IsIdentical(m_pFace))
                    sResult += m_pFace.GetDescription();
                else
                    if (!pOriginal.m_pEars.IsIdentical(m_pEars))
                        sResult += "head";// m_pFace.m_eNoseType == NoseType.Normal ? "face" : "muzzle";

                sResult += ".";
            }

            if (!pOriginal.m_pHairs.IsIdentical(m_pHairs))
            {
                if (sResult != "")
                    sResult += " ";

                if (m_pHairs.GetDescription() != "")
                    sResult += m_pHairs.GetDescription();
                else
                    sResult = "Both males and females are bald, and have no beard or moustache.";
            }

            if (!pOriginal.m_pLifeCycle.IsIdentical(m_pLifeCycle))
            {
                if (sResult != "")
                    sResult += " ";

                sResult += "Usually they " + m_pLifeCycle.GetDescription() + ".";
            }

            if (sResult == "")
                return GetComparsion(pOriginal);

            return sResult;
        }

        #endregion
        
        private Fenotype()
        { }

        public Fenotype(BodyGenetix pBody,
                        HeadGenetix pHead,
                        LegsGenetix pLegs,
                        ArmsGenetix pArms,
                        WingsGenetix pWings,
                        TailGenetix pTail,
                        HideGenetix pHide,
                        BrainGenetix pBrain,
                        LifeCycleGenetix pLifeCycle,
                        FaceGenetix pFace,
                        EarsGenetix pEars,
                        EyesGenetix pEyes,
                        HairsGenetix pHairs): base(pBody, pHead, pLegs, pArms, pWings, pTail, pHide, pBrain, pLifeCycle, pFace, pEars, pEyes, pHairs)
        {
        }

        #region Territory preferences

        /// <summary>
        /// Возвращает список территорий, обладающих перечисленными допустимыми признаками и не обладающих запрещёнными.
        /// </summary>
        /// <param name="aLandTypes"></param>
        /// <param name="eAllowedProp"></param>
        /// <param name="eForbiddenProp"></param>
        /// <returns></returns>
        private LTI[] GetLands(LandscapeGeneration.Environment eAllowedProp, LandscapeGeneration.Environment eForbiddenProp)
        {
            List<LTI> cResult = new List<LTI>();
            foreach (var pLand in LandTypes<LTI>.m_pInstance.m_pLandTypes)
            {
                if ((eAllowedProp == LandscapeGeneration.Environment.None || pLand.Value.m_eEnvironment.HasFlag(eAllowedProp)) && 
                    (eForbiddenProp == LandscapeGeneration.Environment.None || !pLand.Value.m_eEnvironment.HasFlag(eForbiddenProp)))
                    cResult.Add(pLand.Value);

            }

            return cResult.ToArray();
        }

        /// <summary>
        /// Возвращает списки предпочитаемых и нежелательных территорий
        /// </summary>
        /// <param name="aAllLands"></param>
        /// <param name="aPreferred"></param>
        /// <param name="aHated"></param>
        public void GetTerritoryPreferences(out LTI[] aPreferred, out LTI[] aHated)
        {
            Dictionary<LandType, int> cLandTypes = new Dictionary<LandType, int>();
            foreach (LandType eLand in Enum.GetValues(typeof(LandType)))
                cLandTypes[eLand] = 1;

            //ноги дают преимущество на различных типа территории.
            //чем больше ног - тем больше преимущество
            int iMultiplier = 1;
            switch (m_pLegs.m_eLegsCount)
            {
                case LegsCount.Quadrupedal:
                    iMultiplier = 2;
                    break;
                case LegsCount.Hexapod:
                    iMultiplier = 3;
                    break;
                case LegsCount.Octapod:
                    iMultiplier = 4;
                    break;
            }


            if (m_pLegs.m_eLegsCount != LegsCount.NoneBlob &&
               m_pLegs.m_eLegsCount != LegsCount.NoneHover &&
               m_pLegs.m_eLegsCount != LegsCount.NoneTail)
            {
                switch (m_pLegs.m_eLegsType)
                {
                    //копыта дают премущества на равнинах и в горах
                    case LegsType.Hoofs:
                        foreach(LTI pLand in GetLands(LandscapeGeneration.Environment.Open, LandscapeGeneration.Environment.Soft))
                            cLandTypes[pLand.m_eType] += iMultiplier;
                        break;
                    //case LegsType.Foots:
                    //    cLandTypes[LandType.Plains] += iMultiplier;
                    //    cLandTypes[LandType.Savanna] += iMultiplier;
                    //    cLandTypes[LandType.Tundra] += iMultiplier;
                    //    break;
                    //звериные лапы с когтями - на равнинах и в лесах
                    case LegsType.Paws:
                        foreach(LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Soft))
                            cLandTypes[pLand.m_eType] += iMultiplier;
                        break;
                    //птичьи лапы с когтями - в лесах и в горах
                    case LegsType.Claws:
                        foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Flat))
                            cLandTypes[pLand.m_eType] += iMultiplier;
                        break;
                    //паучьи лапы - в песках и горах
                    case LegsType.Spidery:
                        foreach(LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Wet))
                            cLandTypes[pLand.m_eType] += iMultiplier;
                        break;
                    //щупальца - в болотах
                    case LegsType.Tentacles:
                        foreach(LTI pLand in GetLands(LandscapeGeneration.Environment.Soft | LandscapeGeneration.Environment.Wet, LandscapeGeneration.Environment.Cold))
                            cLandTypes[pLand.m_eType] += iMultiplier;
                        break;
                }
            }
            else
            {
                //безногие расы более комфортно себя чувствуют в пустынях и болотах
                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Soft, LandscapeGeneration.Environment.None))
                    cLandTypes[pLand.m_eType] += iMultiplier;
            }

            if (m_pTail.m_eTailLength == TailLength.Long)
            {
                switch (m_pTail.m_eTailControl)
                {
                    //длинный плохоуправляемый хвост помогает удерживать равновесие, что важно в лесах и горах
                    case TailControl.Crude:
                        foreach(LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Flat))
                            cLandTypes[pLand.m_eType] *= 2;
                        break;
                    //длинный и ловкий хвост помогает скакать по веткам деревьев
                    case TailControl.Skillful:
                        foreach(LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Flat | LandscapeGeneration.Environment.Open))
                            cLandTypes[pLand.m_eType] *= 2;
                        break;
                }
            }

            if (m_pWings.m_eWingsForce != WingsForce.None)
            {
                switch (m_pWings.m_eWingsForce)
                {
                    //слабые крылья хороши там, где есть высокие места, откуда можно планировать - в лесах и горах
                    case WingsForce.Gliding:
                        foreach(LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Flat))
                            cLandTypes[pLand.m_eType] *= 2;
                        break;
                    //сильные крылья хороши так же и на равнинах, где можно высоко взлететь и получить дополнительный обзор
                    case WingsForce.Flying:
                        foreach(LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Soft))
                            cLandTypes[pLand.m_eType] *= 2;
                        break;
                }

                //в болотах живут крылатые только с кожистыми или насекомыми крыльями
                if (m_pWings.m_eWingsType == WingsType.Feathered)
                    foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Soft | LandscapeGeneration.Environment.Wet, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] = 0;
            }

            switch (m_pHide.m_eHideType)
            {
                //существа с голой кожей не любят болота
                case HideType.BareSkin:
                    //cLandTypes[LandType.Plains] *= 2;
                    //cLandTypes[LandType.Savanna] *= 2;

                   foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Soft | LandscapeGeneration.Environment.Wet, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] = 0;
                    break;
                //длинный мех подходит для холодных регионов и не подходит для жарких и влажных
                case HideType.FurLong:
                    foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Cold, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] *= 2;

                    foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] = 0;
                    foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Wet, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] = 0;
                    break;
                //хитин, наоборот, подходит для жарких и влажных мест, но не подходит для холодных
                case HideType.Chitin:
                    foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] *= 2;
                    foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Wet, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] *= 2;

                    foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Cold, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] = 0;
                    break;
                //аналогично чешуя
                case HideType.Scales:
                    foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] *= 2;
                    foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Wet, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] *= 2;

                    foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Cold, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] = 0;
                    break;
                //костяные панцири хороши для болота и не подходят для холодных регионов
                case HideType.Shell:
                    foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Wet | LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] *= 2;

                    foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Cold, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] = 0;
                    break;
            }

            KColor pColor = new KColor();
            pColor.RGB = m_pHide.m_eHideColor;

            //светлая кожа не подходит для жарких регионов
            if (pColor.Lightness > 0.75)
            {
                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                    cLandTypes[pLand.m_eType] = 0;
            }

            //тёмная кожа не подходит для холодных регионов и даёт бонусы в особо жарких местах
            if (pColor.Lightness < 0.25)
            {
                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                    cLandTypes[pLand.m_eType] *= 2;

                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Cold, LandscapeGeneration.Environment.None))
                    cLandTypes[pLand.m_eType] = 0;
            }

            int iPigmented = 0;
            int iPigmentless = 0;
            foreach (HairsColor eColor in m_pHairs.m_cHairColors)
            {
                if (eColor == HairsColor.Albino ||
                    eColor == HairsColor.Blonde ||
                    eColor == HairsColor.Red)
                    iPigmentless++;

                if (eColor == HairsColor.Black ||
                    eColor == HairsColor.Brunette ||
                    eColor == HairsColor.Blue)
                    iPigmented++;
            }

            //светловолосые расы не живут в жарких странах
            if (iPigmentless > iPigmented * 2)
            {
                //cLandTypes[LandType.Tundra] *= 2;
                //cLandTypes[LandType.Taiga] *= 2;
                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                    cLandTypes[pLand.m_eType] = 0;
            }

            //с другой стороны, тёмные волосы более свойственны жителям жарких стран
            if (iPigmented > iPigmentless * 2)
            {
                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                    cLandTypes[pLand.m_eType] *= 2;
            }

            //ловкость и стройность отлично подходит для лесов, но плохо сочетается с горами
            if (m_pBody.m_eBodyBuild == BodyBuild.Skinny)
            {
                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Open))
                    cLandTypes[pLand.m_eType] *= 2;

                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Open, LandscapeGeneration.Environment.Flat))
                    cLandTypes[pLand.m_eType] = 0;
            }

            //склонность к тучности мешает выживанию на пересечённой местности
            if (m_pBody.m_eBodyBuild == BodyBuild.Fat)
            {
                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Flat))
                    cLandTypes[pLand.m_eType] = 0;
            }

            //повышенная мускулистость отлично сочетается с горами
            if (m_pBody.m_eBodyBuild == BodyBuild.Muscular)
            {
                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Open, LandscapeGeneration.Environment.Flat))
                    cLandTypes[pLand.m_eType] *= 2;
            }

            foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Habitable))
                cLandTypes[pLand.m_eType] = 0;

            //ищем наиболее предпочтительные регионы
            int iMax = 0;
            foreach (var pLand in cLandTypes)
                if (iMax < pLand.Value)
                    iMax = pLand.Value;

            //если предпочтительных вообще нет, то все пригодные для жизни регионы одинаково предпочтительны
            if (iMax == 0)
            {
                foreach (LTI pLand in Enum.GetValues(typeof(LandType)))
                    cLandTypes[pLand.m_eType] = 1;

                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Habitable))
                    cLandTypes[pLand.m_eType] = 0;

                iMax = 1;
            }

            //заполняем выходные структуры
            List<LTI> cPreferred = new List<LTI>();
            List<LTI> cHated = new List<LTI>();

            foreach (LandType eLand in Enum.GetValues(typeof(LandType)))
            {
                if (cLandTypes[eLand] == iMax)
                    cPreferred.Add(LandTypes<LTI>.m_pInstance.m_pLandTypes[eLand]);
                if (cLandTypes[eLand] == 0 && eLand != LandType.Ocean && eLand != LandType.Coastral)
                    cHated.Add(LandTypes<LTI>.m_pInstance.m_pLandTypes[eLand]);
            }

            aPreferred = cPreferred.ToArray();
            aHated = cHated.ToArray();
        }

        #endregion
        #region Mutations

        public override GenetixBase MutateRace()
        {
            Fenotype<LTI> pMutant = new Fenotype<LTI>();

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
            pMutant.m_pHairs.CheckHairColors();
            pMutant.m_pEars = (EarsGenetix)m_pEars.MutateRace();
            pMutant.m_pEyes = (EyesGenetix)m_pEyes.MutateRace();
            pMutant.m_pFace = (FaceGenetix)m_pFace.MutateRace();

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        public override GenetixBase MutateNation()
        {
            Fenotype<LTI> pMutant = new Fenotype<LTI>();

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
            pMutant.m_pHairs.CheckHairColors();
            pMutant.m_pEars = (EarsGenetix)m_pEars.MutateNation();
            pMutant.m_pEyes = (EyesGenetix)m_pEyes.MutateNation();
            pMutant.m_pFace = (FaceGenetix)m_pFace.MutateNation();

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        public override GenetixBase MutateFamily()
        {
            Fenotype<LTI> pMutant = new Fenotype<LTI>();

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
            pMutant.m_pHairs.CheckHairColors();
            pMutant.m_pEars = (EarsGenetix)m_pEars.MutateFamily();
            pMutant.m_pEyes = (EyesGenetix)m_pEyes.MutateFamily();
            pMutant.m_pFace = (FaceGenetix)m_pFace.MutateFamily();

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        public override GenetixBase MutateIndividual()
        {
            Fenotype<LTI> pMutant = new Fenotype<LTI>();

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
            pMutant.m_pHairs.CheckHairColors();
            pMutant.m_pEars = (EarsGenetix)m_pEars.MutateIndividual();
            pMutant.m_pEyes = (EyesGenetix)m_pEyes.MutateIndividual();
            pMutant.m_pFace = (FaceGenetix)m_pFace.MutateIndividual();

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        #endregion

        #region GenetixBase Members


        public override bool IsIdentical(GenetixBase pOther)
        {
            Fenotype<LTI> pAnother = pOther as Fenotype<LTI>;

            if (pAnother == null)
                return false;

            return pAnother.m_pLegs.IsIdentical(m_pLegs) &&
                pAnother.m_pArms.IsIdentical(m_pArms) &&
                pAnother.m_pWings.IsIdentical(m_pWings) &&
                pAnother.m_pTail.IsIdentical(m_pTail) &&
                pAnother.m_pHide.IsIdentical(m_pHide) &&
                pAnother.m_pBody.IsIdentical(m_pBody) &&
                pAnother.m_pBrain.IsIdentical(m_pBrain) &&
                pAnother.m_pLifeCycle.IsIdentical(m_pLifeCycle) &&
                pAnother.m_pHead.IsIdentical(m_pHead) &&
                pAnother.m_pHairs.IsIdentical(m_pHairs) &&
                pAnother.m_pEars.IsIdentical(m_pEars) &&
                pAnother.m_pEyes.IsIdentical(m_pEyes) &&
                pAnother.m_pFace.IsIdentical(m_pFace);
        }

        #endregion
    }
}
