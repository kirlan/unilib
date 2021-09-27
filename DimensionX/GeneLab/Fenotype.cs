using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneLab.Genetix;
using LandscapeGeneration;
using nsUniLibControls;

namespace GeneLab
{
    public class Fenotype<LTI> : GenetixBase
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

        #region String representation

        /// <summary>
        /// are very clever cratures ... could have only a few children during whole lifetime, which are mostly females.
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
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
            foreach (LandType eLand in Enum.GetValues(typeof(LandType)))
            {
                if ((eAllowedProp == LandscapeGeneration.Environment.None || aLandTypes[i].LandProperties.HasFlag(eAllowedProp)) && 
                    (eForbiddenProp == LandscapeGeneration.Environment.None || !aLandTypes[i].LandProperties.HasFlag(eForbiddenProp)))
                    cResult.Add(eLand);

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
                        foreach(ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Open, LandscapeGeneration.Environment.Soft))
                            cLandTypes[pLand] += iMultiplier;
                        break;
                    //case LegsType.Foots:
                    //    cLandTypes[LandType.Plains] += iMultiplier;
                    //    cLandTypes[LandType.Savanna] += iMultiplier;
                    //    cLandTypes[LandType.Tundra] += iMultiplier;
                    //    break;
                    //звериные лапы с когтями - на равнинах и в лесах
                    case LegsType.Paws:
                        foreach(ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Soft))
                            cLandTypes[pLand] += iMultiplier;
                        break;
                    //птичьи лапы с когтями - в лесах и в горах
                    case LegsType.Claws:
                        foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Flat))
                            cLandTypes[pLand] += iMultiplier;
                        break;
                    //паучьи лапы - в песках и горах
                    case LegsType.Spidery:
                        foreach(ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Wet))
                            cLandTypes[pLand] += iMultiplier;
                        break;
                    //щупальца - в болотах
                    case LegsType.Tentacles:
                        foreach(ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Soft | LandscapeGeneration.Environment.Wet, LandscapeGeneration.Environment.Cold))
                            cLandTypes[pLand] += iMultiplier;
                        break;
                }
            }
            else
            {
                //безногие расы более комфортно себя чувствуют в пустынях и болотах
                foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Soft, LandscapeGeneration.Environment.None))
                    cLandTypes[pLand] += iMultiplier;
            }

            if (m_pTail.m_eTailLength == TailLength.Long)
            {
                switch (m_pTail.m_eTailControl)
                {
                    //длинный плохоуправляемый хвост помогает удерживать равновесие, что важно в лесах и горах
                    case TailControl.Crude:
                        foreach(ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Flat))
                            cLandTypes[pLand] *= 2;
                        break;
                    //длинный и ловкий хвост помогает скакать по веткам деревьев
                    case TailControl.Skillful:
                        foreach(ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Flat | LandscapeGeneration.Environment.Open))
                            cLandTypes[pLand] *= 2;
                        break;
                }
            }

            if (m_pWings.m_eWingsForce != WingsForce.None)
            {
                switch (m_pWings.m_eWingsForce)
                {
                    //слабые крылья хороши там, где есть высокие места, откуда можно планировать - в лесах и горах
                    case WingsForce.Gliding:
                        foreach(ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Flat))
                            cLandTypes[pLand] *= 2;
                        break;
                    //сильные крылья хороши так же и на равнинах, где можно высоко взлететь и получить дополнительный обзор
                    case WingsForce.Flying:
                        foreach(ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Soft))
                            cLandTypes[pLand] *= 2;
                        break;
                }

                //в болотах живут крылатые только с кожистыми или насекомыми крыльями
                if (m_pWings.m_eWingsType == WingsType.Feathered)
                    foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Soft | LandscapeGeneration.Environment.Wet, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand] = 0;
            }

            switch (m_pHide.m_eHideType)
            {
                //существа с голой кожей не любят болота
                case HideType.BareSkin:
                    //cLandTypes[LandType.Plains] *= 2;
                    //cLandTypes[LandType.Savanna] *= 2;

                   foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Soft | LandscapeGeneration.Environment.Wet, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand] = 0;
                    break;
                //длинный мех подходит для холодных регионов и не подходит для жарких и влажных
                case HideType.FurLong:
                    foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Cold, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand] *= 2;

                    foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand] = 0;
                    foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Wet, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand] = 0;
                    break;
                //хитин, наоборот, подходит для жарких и влажных мест, но не подходит для холодных
                case HideType.Chitin:
                    foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand] *= 2;
                    foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Wet, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand] *= 2;

                    foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Cold, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand] = 0;
                    break;
                //аналогично чешуя
                case HideType.Scales:
                    foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand] *= 2;
                    foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Wet, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand] *= 2;

                    foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Cold, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand] = 0;
                    break;
                //костяные панцири хороши для болота и не подходят для холодных регионов
                case HideType.Shell:
                    foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Wet | LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand] *= 2;

                    foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Cold, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand] = 0;
                    break;
            }

            KColor pColor = new KColor();
            pColor.RGB = m_pHide.m_eHideColor;

            //светлая кожа не подходит для жарких регионов
            if (pColor.Lightness > 0.75)
            {
                foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                    cLandTypes[pLand] = 0;
            }

            //тёмная кожа не подходит для холодных регионов и даёт бонусы в особо жарких местах
            if (pColor.Lightness < 0.25)
            {
                foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                    cLandTypes[pLand] *= 2;

                foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Cold, LandscapeGeneration.Environment.None))
                    cLandTypes[pLand] = 0;
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
                foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                    cLandTypes[pLand] = 0;
            }

            //с другой стороны, тёмные волосы более свойственны жителям жарких стран
            if (iPigmented > iPigmentless * 2)
            {
                foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Hot, LandscapeGeneration.Environment.None))
                    cLandTypes[pLand] *= 2;
            }

            //ловкость и стройность отлично подходит для лесов, но плохо сочетается с горами
            if (m_pBody.m_eBodyBuild == BodyBuild.Skinny)
            {
                foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Open))
                    cLandTypes[pLand] *= 2;

                foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Open, LandscapeGeneration.Environment.Flat))
                    cLandTypes[pLand] = 0;
            }

            //склонность к тучности мешает выживанию на пересечённой местности
            if (m_pBody.m_eBodyBuild == BodyBuild.Fat)
            {
                foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Flat))
                    cLandTypes[pLand] = 0;
            }

            //повышенная мускулистость отлично сочетается с горами
            if (m_pBody.m_eBodyBuild == BodyBuild.Muscular)
            {
                foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.Open, LandscapeGeneration.Environment.Flat))
                    cLandTypes[pLand] *= 2;
            }

            foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Habitable))
                cLandTypes[pLand] = 0;

            //ищем наиболее предпочтительные регионы
            int iMax = 0;
            foreach (var pLand in cLandTypes)
                if (iMax < pLand.Value)
                    iMax = pLand.Value;

            //если предпочтительных вообще нет, то все регионы одинаково предпочтительны
            if (iMax == 0)
            {
                foreach (ITerritory pLand in aAllLands)
                    cLandTypes[pLand] = 1;

                foreach (ITerritory pLand in GetLands(aAllLands, LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Habitable))
                    cLandTypes[pLand] = 0;

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

        #region Mutations

        public GenetixBase MutateRace()
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

        public GenetixBase MutateNation()
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

        public GenetixBase MutateFamily()
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

        public GenetixBase MutateIndividual()
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


        public bool IsIdentical(GenetixBase pOther)
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
