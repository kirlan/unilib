using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneLab.Genetix;
using LandscapeGeneration;
using nsUniLibControls;

namespace GeneLab
{
    public abstract class Phenotype : GenetixBase
    {
        protected Dictionary<Type, dynamic> m_cPhens = new Dictionary<Type, dynamic>();

        public void Select<T>(T value) where T: GenetixBase
        {
            m_cPhens[typeof(T)] = value;
        }

        public T ValueOf<T>() where T : GenetixBase
        {
            return m_cPhens[typeof(T)];
        }

        public bool HasIdentical<T>(Phenotype pOther) where T : GenetixBase
        {
            return HasIdentical(typeof(T), pOther);
        }

        public bool HasIdentical(Type phenType, Phenotype pOther)
        {
            if (pOther == null)
                return false;

            dynamic storedValue;
            if (!m_cPhens.TryGetValue(phenType, out storedValue))
                return false;

            dynamic storedValueOther;
            if (!pOther.m_cPhens.TryGetValue(phenType, out storedValueOther))
                return false;

            return storedValue.IsIdentical(storedValueOther);
        }

        protected Phenotype()
        { }

        public Phenotype(BodyGenetix pBody,
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
            Select(pBody);
            Select(pHead);
            Select(pLegs);
            Select(pArms);
            Select(pWings);
            Select(pTail);
            Select(pHide);
            Select(pBrain);
            Select(pLifeCycle);
            Select(pFace);
            Select(pEars);
            Select(pEyes);
            Select(pHairs);
            
            pHairs.CheckHairColors();
        }

        public abstract Phenotype GetHumanEtalon(Gender gender = Gender.Male);

        public abstract Phenotype Clone();

        #region String representation

        /// <summary>
        /// are very clever cratures ... could have only a few children during whole lifetime, which are mostly females.
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string sResult = GetComparsion(GetHumanEtalon());
            if (sResult.Length == 0)
                sResult = "are just humans.";

            if (!sResult.StartsWith("are"))
                sResult = "are quite common humans." + sResult;

            return sResult;
        }

        /// <summary>
        /// Возвращает строку, описывающую черты, отличиющие фенотип от заданного, примерный формат:
        /// "are very clever cratures ... could have only a few children during whole lifetime, which are mostly females."
        /// </summary>
        /// <returns></returns>
        public string GetComparsion(Phenotype pOriginal)
        {
            if (pOriginal.IsIdentical(this))
                return "";

            string sResult = "";

            if (!pOriginal.HasIdentical<BrainGenetix>(this))
                sResult += "are " + ValueOf<BrainGenetix>().GetDescription() + ".";

            if (!pOriginal.HasIdentical<HeadGenetix>(this) ||
                !pOriginal.HasIdentical<ArmsGenetix>(this) ||
                !pOriginal.HasIdentical<LegsGenetix>(this) ||
                !pOriginal.HasIdentical<WingsGenetix>(this) ||
                !pOriginal.HasIdentical<TailGenetix>(this))
            {
                if (sResult != "")
                    sResult += " They ";

                sResult += "have ";

                bool bSemicolon = false;
                if (!pOriginal.HasIdentical<HeadGenetix>(this))
                {
                    sResult += ValueOf<HeadGenetix>().GetDescription();
                    bSemicolon = true;
                }

                if (!pOriginal.HasIdentical<ArmsGenetix>(this))
                {
                    if (ValueOf<ArmsGenetix>().m_eArmsCount != ArmsCount.None)
                    {
                        if (bSemicolon)
                            sResult += ", ";
                        sResult += ValueOf<ArmsGenetix>().GetDescription();

                        bSemicolon = true;
                    }
                    else
                    {
                        if (bSemicolon)
                            sResult += ", ";
                        sResult += "no arms";
                    }
                }

                if (!pOriginal.HasIdentical<LegsGenetix>(this))
                {
                    if (bSemicolon)
                        sResult += ", ";
                    sResult += ValueOf<LegsGenetix>().GetDescription();

                    bSemicolon = true;
                }

                if (!pOriginal.HasIdentical<WingsGenetix>(this))
                {
                    if (ValueOf<WingsGenetix>().m_eWingsCount != WingsCount.None)
                    {
                        if (bSemicolon)
                            sResult += ", ";
                        sResult += ValueOf<WingsGenetix>().GetDescription();

                        bSemicolon = true;
                    }
                    else
                    {
                        if (bSemicolon)
                            sResult += ", ";
                        sResult += "no wings";
                    }
                }

                if (!pOriginal.HasIdentical<TailGenetix>(this))
                {
                    if (ValueOf<TailGenetix>().m_eTailLength != TailLength.None)
                    {
                        if (bSemicolon)
                            sResult += " and ";
                        sResult += ValueOf<TailGenetix>().GetDescription();
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

            if (!pOriginal.HasIdentical<BodyGenetix>(this) ||
                !pOriginal.HasIdentical<HideGenetix>(this))
            {
                if (sResult != "")
                    sResult += " They ";

                bool bSemicolon = false;
                if (!pOriginal.HasIdentical<BodyGenetix>(this))
                {
                    sResult += ValueOf<BodyGenetix>().GetDescription();
                    bSemicolon = true;
                }
                else
                    sResult += "have ";

                if (!pOriginal.HasIdentical<HideGenetix>(this))
                {
                    if (bSemicolon)
                        sResult += " with ";
                    sResult += ValueOf<HideGenetix>().GetDescription();
                }

                sResult += ".";
            }

            if (!pOriginal.HasIdentical<EyesGenetix>(this) ||
                !pOriginal.HasIdentical<EarsGenetix>(this) ||
                !pOriginal.HasIdentical<FaceGenetix>(this))
            {
                if (sResult != "")
                    sResult += " They ";

                sResult += "have ";

                bool bSemicolon = false;
                if (!pOriginal.HasIdentical<EyesGenetix>(this) ||
                    !pOriginal.HasIdentical<EarsGenetix>(this))
                {
                    if (!pOriginal.HasIdentical<EyesGenetix>(this))
                    {
                        sResult += ValueOf<EyesGenetix>().GetDescription();
                        bSemicolon = true;
                    }

                    if (!pOriginal.HasIdentical<EarsGenetix>(this))
                    {
                        if (bSemicolon)
                            sResult += " and ";

                        sResult += ValueOf<EarsGenetix>().GetDescription() + " of a ";
                    }
                    else
                    {
                        if (bSemicolon && !pOriginal.HasIdentical<FaceGenetix>(this))
                            sResult += " at the ";
                    }
                }

                if (!pOriginal.HasIdentical<FaceGenetix>(this))
                    sResult += ValueOf<FaceGenetix>().GetDescription();
                else
                    if (!pOriginal.HasIdentical<EarsGenetix>(this))
                    sResult += "head";// m_pFace.m_eNoseType == NoseType.Normal ? "face" : "muzzle";

                sResult += ".";
            }

            if (!pOriginal.HasIdentical<HairsGenetix>(this))
            {
                if (sResult != "")
                    sResult += " They ";

                if (ValueOf<HairsGenetix>().GetDescription() != "")
                    sResult += ValueOf<HairsGenetix>().GetDescription();
                else
                    sResult = "are bald, and have no beard or moustache.";
            }

            if (!pOriginal.HasIdentical<LifeCycleGenetix>(this))
            {
                if (sResult != "")
                    sResult += " They ";

                sResult += "usually " + ValueOf<LifeCycleGenetix>().GetDescription() + ".";
            }

            if (sResult == "")
                return GetComparsion(pOriginal);

            return sResult;
        }

        #endregion

        public bool IsIdentical(GenetixBase pOther)
        {
            Phenotype pAnother = pOther as Phenotype;

            if (pAnother == null)
                return false;

            if (pAnother.m_cPhens.Count != m_cPhens.Count)
                return false;

            foreach (var phen in m_cPhens)
            {
                if (!pAnother.HasIdentical(phen.Key, this))
                    return false;
            }

            return true;
        }

        #region Mutations

        public GenetixBase MutateRace()
        {
            Phenotype pMutant = Clone();

            foreach (var phen in m_cPhens)
            {
                pMutant.Select(Convert.ChangeType(phen.Value.MutateRace(), phen.Key));
            }

            pMutant.ValueOf<HairsGenetix>().CheckHairColors();

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        public GenetixBase MutateNation()
        {
            Phenotype pMutant = Clone();

            foreach (var phen in m_cPhens)
            {
                pMutant.Select(Convert.ChangeType(phen.Value.MutateNation(), phen.Key));
            }

            pMutant.ValueOf<HairsGenetix>().CheckHairColors();

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        public GenetixBase MutateFamily()
        {
            Phenotype pMutant = Clone();

            foreach (var phen in m_cPhens)
            {
                pMutant.Select(Convert.ChangeType(phen.Value.MutateFamily(), phen.Key));
            }

            pMutant.ValueOf<HairsGenetix>().CheckHairColors();

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        public GenetixBase MutateIndividual()
        {
            Phenotype pMutant = Clone();

            foreach (var phen in m_cPhens)
            {
                pMutant.Select(Convert.ChangeType(phen.Value.MutateIndividual(), phen.Key));
            }

            pMutant.ValueOf<HairsGenetix>().CheckHairColors();

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        #endregion

        /// <summary>
        /// Max 11
        /// </summary>
        /// <param name="pOpponents"></param>
        /// <returns></returns>
        public int GetFenotypeDifference(Phenotype pOpponents)
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
        public int GetFenotypeDifference(Phenotype pOpponents, ref string sPositiveReasons, ref string sNegativeReasons)
        {
            int iHostility = 0;

            if (!HasIdentical<HeadGenetix>(pOpponents))
            {
                iHostility++;
                sNegativeReasons += " (-1) [APP] ugly head\n";
            }

            int iBodyDiff = 0;
            if (!HasIdentical<ArmsGenetix>(pOpponents))
                iBodyDiff++;
            //if (!pNation.m_pLegs.IsIdentical(pOpponents.m_pLegs))
            //    iBodyDiff++;
            //if (!pNation.m_pTail.IsIdentical(pOpponents.m_pTail))
            //    iBodyDiff++;
            if (ValueOf<HideGenetix>().m_eHideType != pOpponents.ValueOf<HideGenetix>().m_eHideType)
                iBodyDiff++;

            if (iBodyDiff > 0)
            {
                iHostility += iBodyDiff;
                sNegativeReasons += string.Format(" (-{0}) [APP] ugly body\n", iBodyDiff);
            }

            if (Math.Abs(ValueOf<BodyGenetix>().m_eBodyBuild - pOpponents.ValueOf<BodyGenetix>().m_eBodyBuild) > 1)
            {
                iHostility++;
                sNegativeReasons += string.Format(" (-1) [APP] too {0}\n", pOpponents.ValueOf<BodyGenetix>().m_eBodyBuild.ToString().ToLower());
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
            if (!HasIdentical<EyesGenetix>(pOpponents))
                iFaceDiff++;
            //if (!pNation.m_pEars.IsIdentical(pOpponents.m_pEars))
            //    iFaceDiff++;
            if (!HasIdentical<FaceGenetix>(pOpponents))
                iFaceDiff++;
            if (iFaceDiff > 0)
            {
                iHostility += iFaceDiff;
                sNegativeReasons += string.Format(" (-{0}) [APP] ugly face\n", iBodyDiff);
            }

            //а вот тут - берём личные показатели
            if (ValueOf<BodyGenetix>().m_eNutritionType != pOpponents.ValueOf<BodyGenetix>().m_eNutritionType)
            {
                if (ValueOf<BodyGenetix>().IsParasite())
                {
                    iHostility++;
                    sNegativeReasons += " (-1) [PSI] prey\n";
                }
                if (pOpponents.ValueOf<BodyGenetix>().IsParasite())
                {
                    iHostility += 4;
                    sNegativeReasons += " (-4) [PSI] predator\n";
                }
            }

            return iHostility;
        }

        // Находит все отличия между pBaseSample и pDifferencesSample и затем накладывает их на pBase
        public static Phenotype ApplyDifferences(Phenotype pBase, Phenotype pBaseSample, Phenotype pDifferencesSample)
        {
            Phenotype pNew = pBase.Clone();

            foreach (var phen in pBaseSample.m_cPhens)
            {
                if (!phen.Value.IsIdentical(pDifferencesSample.m_cPhens[phen.Key]))
                    pNew.Select(pDifferencesSample.m_cPhens[phen.Key]);
            }

            return pNew;
        }
    }

    public class Phenotype<LTI> : Phenotype
        where LTI: LandTypeInfo, new()
    {
        private readonly static Phenotype<LTI> s_HumanEtalonM = new Phenotype<LTI>(BodyGenetix.Human,
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
                                                HairsGenetix.HumanWhiteM);
        private readonly static Phenotype<LTI> s_HumanEtalonF = new Phenotype<LTI>(BodyGenetix.Human,
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
                                                HairsGenetix.HumanWhiteF);

        public override Phenotype GetHumanEtalon(Gender gender = Gender.Male)
        {
            return gender == Gender.Male ? s_HumanEtalonM : s_HumanEtalonF;
        }

        public override Phenotype Clone()
        {
            Phenotype<LTI> pClone = new Phenotype<LTI>();
            
            foreach (var phen in m_cPhens)
            {
                pClone.Select(m_cPhens[phen.Key]);
            }

            return pClone;
        }

        private Phenotype()
        { }

        public Phenotype(BodyGenetix pBody,
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
            : base(pBody, pHead, pLegs, pArms, pWings, pTail, pHide, pBrain, pLifeCycle, pFace, pEars, pEyes, pHairs)
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
            switch (ValueOf<LegsGenetix>().m_eLegsCount)
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


            if (ValueOf<LegsGenetix>().m_eLegsCount != LegsCount.NoneBlob &&
               ValueOf<LegsGenetix>().m_eLegsCount != LegsCount.NoneHover &&
               ValueOf<LegsGenetix>().m_eLegsCount != LegsCount.NoneTail)
            {
                switch (ValueOf<LegsGenetix>().m_eLegsType)
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

            if (ValueOf<TailGenetix>().m_eTailLength == TailLength.Long)
            {
                switch (ValueOf<TailGenetix>().m_eTailControl)
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

            if (ValueOf<WingsGenetix>().m_eWingsForce != WingsForce.None)
            {
                switch (ValueOf<WingsGenetix>().m_eWingsForce)
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
                if (ValueOf<WingsGenetix>().m_eWingsType == WingsType.Feathered)
                    foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Soft | LandscapeGeneration.Environment.Wet, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] = 0;
            }

            switch (ValueOf<HideGenetix>().m_eHideType)
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
            pColor.RGB = ValueOf<HideGenetix>().m_eHideColor;

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
            foreach (HairsColor eColor in ValueOf<HairsGenetix>().m_cHairColors)
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
            if (ValueOf<BodyGenetix>().m_eBodyBuild == BodyBuild.Skinny)
            {
                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Open))
                    cLandTypes[pLand.m_eType] *= 2;

                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Open, LandscapeGeneration.Environment.Flat))
                    cLandTypes[pLand.m_eType] = 0;
            }

            //склонность к тучности мешает выживанию на пересечённой местности
            if (ValueOf<BodyGenetix>().m_eBodyBuild == BodyBuild.Fat)
            {
                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Flat))
                    cLandTypes[pLand.m_eType] = 0;
            }

            //повышенная мускулистость отлично сочетается с горами
            if (ValueOf<BodyGenetix>().m_eBodyBuild == BodyBuild.Muscular)
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
    }
}
