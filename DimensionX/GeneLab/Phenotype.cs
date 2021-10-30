using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneLab.Genetix;
using LandscapeGeneration;
using nsUniLibControls;
using Random;

namespace GeneLab
{
    public abstract class Phenotype : GenetixBase
    {
        public class PhensStorage
        {
            public readonly Dictionary<Type, dynamic> m_cInternal = new Dictionary<Type, dynamic>();

            public PhensStorage Set<T>(T value) where T : GenetixBase
            {
                m_cInternal[typeof(T)] = value;
                
                return this;
            }

            public T Get<T>() where T : GenetixBase
            {
                return m_cInternal[typeof(T)];
            }

            public bool HasIdentical<T>(PhensStorage pOther) where T : GenetixBase
            {
                return HasIdentical(typeof(T), pOther);
            }

            public bool HasIdentical(Type phenType, PhensStorage pOther)
            {
                if (pOther == null)
                    return false;

                dynamic storedValue;
                if (!m_cInternal.TryGetValue(phenType, out storedValue))
                    return false;

                dynamic storedValueOther;
                if (!pOther.m_cInternal.TryGetValue(phenType, out storedValueOther))
                    return false;

                return storedValue.IsIdentical(storedValueOther);
            }

            public bool IsIdentical(PhensStorage pOther)
            {
                if (pOther == null)
                    return false;

                if (pOther.m_cInternal.Count != m_cInternal.Count)
                    return false;

                foreach (var phen in m_cInternal)
                {
                    if (!HasIdentical(phen.Key, pOther))
                        return false;
                }

                return true;
            }
            public static PhensStorage operator +(PhensStorage pBase, PhensStorage pDifferences)
            {
                PhensStorage pNew = new PhensStorage();

                pNew.Add(pBase);
                pNew.Add(pDifferences);

                return pNew;
            }
            public void Add(PhensStorage pDifferences)
            {
                if (pDifferences != null)
                {
                    foreach (var phen in pDifferences.m_cInternal)
                    {
                        Set(phen.Value);
                    }
                }
            }
            public static PhensStorage operator -(PhensStorage pOther, PhensStorage pBase)
            {
                PhensStorage pDifference = new PhensStorage();

                foreach (var phen in pOther.m_cInternal)
                {
                    if (!pOther.HasIdentical(phen.Key, pBase))
                        pDifference.Set(phen.Value);
                }

                return pDifference;
            }

        }

        public readonly PhensStorage m_pValues = new PhensStorage();

        protected Phenotype()
        {
            var pBaseHuman = GetBaseHuman();
            foreach (var phen in pBaseHuman.m_cInternal)
                m_pValues.Set(phen.Value);
        }

        public Phenotype(PhensStorage cValues)
            : this()
        {
            foreach (var value in cValues.m_cInternal)
                m_pValues.Set(value.Value);
            
            m_pValues.Get<HairsGenetix>().CheckHairColors();
        }

        private readonly static PhensStorage s_WhiteManEtalon = new PhensStorage().
                                                Set(BodyGenetix.Human).
                                                Set(BreastsGenetix.TwoMale).
                                                Set(NutritionGenetix.Human).
                                                Set(HeadGenetix.Human).
                                                Set(LegsGenetix.Human).
                                                Set(ArmsGenetix.Human).
                                                Set(WingsGenetix.None).
                                                Set(TailGenetix.None).
                                                Set(HideGenetix.HumanWhite).
                                                Set(BrainGenetix.HumanReal).
                                                Set(LifeCycleGenetix.Human).
                                                Set(FaceGenetix.Human).
                                                Set(EarsGenetix.Human).
                                                Set(EyesGenetix.Human).
                                                Set(HairsGenetix.HumanWhiteM);
        private readonly static PhensStorage s_WhiteWomanDiff = new PhensStorage().
                                                Set(BreastsGenetix.TwoAverage).
                                                Set(HairsGenetix.HumanWhiteF);

        public static PhensStorage GetBaseHuman(Gender gender = Gender.Male)
        {
            return gender == Gender.Male ? s_WhiteManEtalon : s_WhiteManEtalon + s_WhiteWomanDiff;
        }


        public abstract Phenotype Clone();

        #region String representation

        /// <summary>
        /// are very clever cratures ... could have only a few children during whole lifetime, which are mostly females.
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string sResult = GetComparsion(GetBaseHuman());
            if (sResult.Length == 0)
                sResult = "are just humans.";

            if (!sResult.StartsWith("are"))
            {
                if (m_pValues.Get<LegsGenetix>().LegsCount != GetBaseHuman().Get<LegsGenetix>().LegsCount)
                {
                    sResult = "are sentient creatures. They " + sResult;
                }
                else if (m_pValues.Get<ArmsGenetix>().ArmsCount != GetBaseHuman().Get<ArmsGenetix>().ArmsCount ||
                    m_pValues.Get<ArmsGenetix>().ArmsType != GetBaseHuman().Get<ArmsGenetix>().ArmsType)
                {
                    sResult = "are human-like creatures. They " + sResult;
                }
                else
                {
                    sResult = "are quite common humans. They " + sResult;
                }
            }

            return sResult;
        }

        /// <summary>
        /// Возвращает строку, описывающую черты, отличиющие фенотип от заданного, примерный формат:
        /// "are very clever cratures ... could have only a few children during whole lifetime, which are mostly females."
        /// </summary>
        /// <returns></returns>
        public string GetComparsion(PhensStorage pOriginalValues)
        {
            if (pOriginalValues.IsIdentical(m_pValues))
                return "";

            string sResult = "";

            if (!pOriginalValues.HasIdentical<BrainGenetix>(m_pValues))
                sResult += m_pValues.Get<BrainGenetix>().GetDescription() + ".";

            if (!pOriginalValues.HasIdentical<HeadGenetix>(m_pValues))
            {
                if (sResult != "")
                    sResult += " They ";

                sResult += "have ";

                if (!pOriginalValues.HasIdentical<HeadGenetix>(m_pValues))
                {
                    sResult += m_pValues.Get<HeadGenetix>().GetDescription();
                }

                sResult += ".";
            }

            if (!pOriginalValues.HasIdentical<BodyGenetix>(m_pValues) ||
                !pOriginalValues.HasIdentical<HideGenetix>(m_pValues) ||
                !pOriginalValues.HasIdentical<ArmsGenetix>(m_pValues) ||
                !pOriginalValues.HasIdentical<LegsGenetix>(m_pValues) ||
                !pOriginalValues.HasIdentical<WingsGenetix>(m_pValues) ||
                !pOriginalValues.HasIdentical<TailGenetix>(m_pValues))
            {
                if (sResult != "")
                    sResult += " They ";

                sResult += "have ";

                List<string> cBodyDescription = new List<string>();

                bool bBody = false;
                if (!pOriginalValues.HasIdentical<BodyGenetix>(m_pValues))
                {
                    cBodyDescription.Add(m_pValues.Get<BodyGenetix>().GetDescription());
                    bBody = true;
                }

                if (!pOriginalValues.HasIdentical<HideGenetix>(m_pValues))
                {
                    cBodyDescription.Add(m_pValues.Get<HideGenetix>().GetDescription());
                }

                if (!pOriginalValues.HasIdentical<ArmsGenetix>(m_pValues))
                {
                    if (m_pValues.Get<ArmsGenetix>().ArmsCount != ArmsCount.None)
                    {
                        cBodyDescription.Add(m_pValues.Get<ArmsGenetix>().GetDescription());
                    }
                    else
                    {
                        cBodyDescription.Add("no arms");
                    }
                }

                if (!pOriginalValues.HasIdentical<LegsGenetix>(m_pValues))
                {
                    cBodyDescription.Add(m_pValues.Get<LegsGenetix>().GetDescription());
                }

                if (!pOriginalValues.HasIdentical<WingsGenetix>(m_pValues))
                {
                    if (m_pValues.Get<WingsGenetix>().WingsCount != WingsCount.None)
                    {
                        cBodyDescription.Add(m_pValues.Get<WingsGenetix>().GetDescription());
                    }
                    else
                    {
                        cBodyDescription.Add("no wings");
                    }
                }

                if (!pOriginalValues.HasIdentical<TailGenetix>(m_pValues))
                {
                    if (m_pValues.Get<TailGenetix>().TailLength != TailLength.None)
                    {
                        cBodyDescription.Add(m_pValues.Get<TailGenetix>().GetDescription());
                    }
                    else
                    {
                        cBodyDescription.Add("no tail");
                    }
                }

                foreach (string part in cBodyDescription)
                {
                    if (part != cBodyDescription.First())
                    {
                        if (bBody)
                        {
                            sResult += " with ";
                            bBody = false;
                        }
                        else if (part == cBodyDescription.Last())
                        {
                            sResult += " and ";
                        }
                        else
                        {
                            sResult += ", ";
                        }
                    }

                    sResult += part;
                }

                sResult += ".";
            }

            if (!pOriginalValues.HasIdentical<NutritionGenetix>(m_pValues) ||
                !pOriginalValues.HasIdentical<BreastsGenetix>(m_pValues))
            {
                string sBody2 = "";
                if (!pOriginalValues.HasIdentical<NutritionGenetix>(m_pValues))
                {
                    sBody2 = m_pValues.Get<NutritionGenetix>().GetDescription();
                }

                if (!pOriginalValues.HasIdentical<BreastsGenetix>(m_pValues))
                {
                    string sBreastsDescription = m_pValues.Get<BreastsGenetix>().GetDescription();
                    if (sBody2 != "" && sBreastsDescription != "")
                        sBody2 += " and ";
                    sBody2 += sBreastsDescription;
                }

                if (sBody2 != "")
                {
                    if (sResult != "")
                        sResult += " They ";

                    sResult += sBody2;

                    sResult += ".";
                }
            }

            if (!pOriginalValues.HasIdentical<EyesGenetix>(m_pValues) ||
                !pOriginalValues.HasIdentical<EarsGenetix>(m_pValues) ||
                !pOriginalValues.HasIdentical<FaceGenetix>(m_pValues))
            {
                if (sResult != "")
                    sResult += " They ";

                sResult += "have ";

                bool bSemicolon = false;
                if (!pOriginalValues.HasIdentical<EyesGenetix>(m_pValues) ||
                    !pOriginalValues.HasIdentical<EarsGenetix>(m_pValues))
                {
                    if (!pOriginalValues.HasIdentical<EyesGenetix>(m_pValues))
                    {
                        sResult += m_pValues.Get<EyesGenetix>().GetDescription();
                        bSemicolon = true;
                    }

                    if (!pOriginalValues.HasIdentical<EarsGenetix>(m_pValues))
                    {
                        if (bSemicolon)
                            sResult += " and ";

                        sResult += m_pValues.Get<EarsGenetix>().GetDescription() + " of a ";
                    }
                    else
                    {
                        if (bSemicolon && !pOriginalValues.HasIdentical<FaceGenetix>(m_pValues))
                            sResult += " at the ";
                    }
                }

                if (!pOriginalValues.HasIdentical<FaceGenetix>(m_pValues))
                    sResult += m_pValues.Get<FaceGenetix>().GetDescription();
                else
                    if (!pOriginalValues.HasIdentical<EarsGenetix>(m_pValues))
                    sResult += "head";// m_pFace.m_eNoseType == NoseType.Normal ? "face" : "muzzle";

                sResult += ".";
            }

            if (!pOriginalValues.HasIdentical<HairsGenetix>(m_pValues))
            {
                if (sResult != "")
                    sResult += " They ";

                if (m_pValues.Get<HairsGenetix>().GetDescription() != "")
                    sResult += m_pValues.Get<HairsGenetix>().GetDescription();
                else
                    sResult = "are bald, and have no beard or moustache.";
            }

            if (!pOriginalValues.HasIdentical<LifeCycleGenetix>(m_pValues))
            {
                if (sResult != "")
                    sResult += " They ";

                sResult += "usually " + m_pValues.Get<LifeCycleGenetix>().GetDescription() + ".";
            }

            if (sResult == "")
                return GetComparsion(pOriginalValues);

            return sResult;
        }

        #endregion

        public bool IsIdentical(GenetixBase pOther)
        {
            Phenotype pAnother = pOther as Phenotype;

            if (pAnother == null)
                return false;

            return m_pValues.IsIdentical(pAnother.m_pValues);
        }

        #region Mutations

        public GenetixBase MutateRace()
        {
            Phenotype pMutant = Clone();

            foreach (var phen in m_pValues.m_cInternal)
            {
                pMutant.m_pValues.Set(Convert.ChangeType(phen.Value.MutateRace(), phen.Key));
            }

            if (Rnd.OneChanceFrom(20))
            {
                NutritionGenetix pNutritionMutation = new NutritionGenetix(m_pValues.Get<NutritionGenetix>());

                pNutritionMutation.MutateNutritionType(pMutant.m_pValues.Get<BodyGenetix>());

                pMutant.m_pValues.Set(pNutritionMutation);
            }


            pMutant.m_pValues.Get<HairsGenetix>().CheckHairColors();

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        public GenetixBase MutateGender()
        {
            Phenotype pMutant = Clone();

            foreach (var phen in m_pValues.m_cInternal)
            {
                pMutant.m_pValues.Set(Convert.ChangeType(phen.Value.MutateGender(), phen.Key));
            }

            if (Rnd.OneChanceFrom(100))
            {
                NutritionGenetix pNutritionMutation = new NutritionGenetix(m_pValues.Get<NutritionGenetix>());

                pNutritionMutation.MutateNutritionType(pMutant.m_pValues.Get<BodyGenetix>());

                pMutant.m_pValues.Set(pNutritionMutation);
            }


            pMutant.m_pValues.Get<HairsGenetix>().CheckHairColors();

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        public GenetixBase MutateNation()
        {
            Phenotype pMutant = Clone();

            foreach (var phen in m_pValues.m_cInternal)
            {
                pMutant.m_pValues.Set(Convert.ChangeType(phen.Value.MutateNation(), phen.Key));
            }

            pMutant.m_pValues.Get<HairsGenetix>().CheckHairColors();

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        public GenetixBase MutateFamily()
        {
            Phenotype pMutant = Clone();

            foreach (var phen in m_pValues.m_cInternal)
            {
                pMutant.m_pValues.Set(Convert.ChangeType(phen.Value.MutateFamily(), phen.Key));
            }

            pMutant.m_pValues.Get<HairsGenetix>().CheckHairColors();

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        public GenetixBase MutateIndividual()
        {
            Phenotype pMutant = Clone();

            foreach (var phen in m_pValues.m_cInternal)
            {
                pMutant.m_pValues.Set(Convert.ChangeType(phen.Value.MutateIndividual(), phen.Key));
            }

            pMutant.m_pValues.Get<HairsGenetix>().CheckHairColors();

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

            if (!m_pValues.HasIdentical<HeadGenetix>(pOpponents.m_pValues))
            {
                iHostility++;
                sNegativeReasons += " (-1) [APP] ugly head\n";
            }

            int iBodyDiff = 0;
            if (!m_pValues.HasIdentical<ArmsGenetix>(pOpponents.m_pValues))
                iBodyDiff++;
            if (!m_pValues.HasIdentical<BreastsGenetix>(pOpponents.m_pValues))
                iBodyDiff++;
            //if (!pNation.m_pLegs.IsIdentical(pOpponents.m_pLegs))
            //    iBodyDiff++;
            //if (!pNation.m_pTail.IsIdentical(pOpponents.m_pTail))
            //    iBodyDiff++;
            if (m_pValues.Get<HideGenetix>().HideType != pOpponents.m_pValues.Get<HideGenetix>().HideType)
                iBodyDiff++;

            if (iBodyDiff > 0)
            {
                iHostility += iBodyDiff;
                sNegativeReasons += string.Format(" (-{0}) [APP] ugly body\n", iBodyDiff);
            }

            if (Math.Abs(m_pValues.Get<BodyGenetix>().BodyBuild - pOpponents.m_pValues.Get<BodyGenetix>().BodyBuild) > 1)
            {
                iHostility++;
                sNegativeReasons += string.Format(" (-1) [APP] too {0}\n", pOpponents.m_pValues.Get<BodyGenetix>().BodyBuild.ToString().ToLower());
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
            if (!m_pValues.HasIdentical<EyesGenetix>(pOpponents.m_pValues))
                iFaceDiff++;
            //if (!pNation.m_pEars.IsIdentical(pOpponents.m_pEars))
            //    iFaceDiff++;
            if (!m_pValues.HasIdentical<FaceGenetix>(pOpponents.m_pValues))
                iFaceDiff++;
            if (iFaceDiff > 0)
            {
                iHostility += iFaceDiff;
                sNegativeReasons += string.Format(" (-{0}) [APP] ugly face\n", iBodyDiff);
            }

            //а вот тут - берём личные показатели
            if (m_pValues.Get<NutritionGenetix>().NutritionType != pOpponents.m_pValues.Get<NutritionGenetix>().NutritionType)
            {
                iHostility++;
                sNegativeReasons += " (-1) [PSI] weird food preferences\n";

                if (m_pValues.Get<NutritionGenetix>().IsParasite())
                {
                    iHostility++;
                    sNegativeReasons += " (-1) [PSI] prey\n";
                }
                if (pOpponents.m_pValues.Get<NutritionGenetix>().IsParasite())
                {
                    iHostility += 4;
                    sNegativeReasons += " (-4) [PSI] predator\n";
                }
            }

            return iHostility;
        }

        public static T Combine<T>(T pBase, PhensStorage pDifferences) where T : Phenotype
        {
            T pNew = (T)pBase.Clone();

            pNew.m_pValues.Add(pDifferences);

            return pNew;
        }

        public override string ToString()
        {
            return GetDescription();
        }
    }

    public class Phenotype<LTI> : Phenotype
        where LTI: LandTypeInfo, new()
    {
        public override Phenotype Clone()
        {
            return new Phenotype<LTI>(m_pValues);
        }

        private Phenotype()
        { }

        public Phenotype(PhensStorage cValues)
            : base(cValues)
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
            switch (m_pValues.Get<LegsGenetix>().LegsCount)
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


            if (m_pValues.Get<LegsGenetix>().LegsCount != LegsCount.NoneBlob &&
               m_pValues.Get<LegsGenetix>().LegsCount != LegsCount.NoneHover &&
               m_pValues.Get<LegsGenetix>().LegsCount != LegsCount.NoneTail)
            {
                switch (m_pValues.Get<LegsGenetix>().LegsType)
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

            if (m_pValues.Get<TailGenetix>().TailLength == TailLength.Long)
            {
                switch (m_pValues.Get<TailGenetix>().TailControl)
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

            if (m_pValues.Get<WingsGenetix>().WingsForce != WingsForce.None)
            {
                switch (m_pValues.Get<WingsGenetix>().WingsForce)
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
                if (m_pValues.Get<WingsGenetix>().WingsType == WingsType.Feathered)
                    foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Soft | LandscapeGeneration.Environment.Wet, LandscapeGeneration.Environment.None))
                        cLandTypes[pLand.m_eType] = 0;
            }

            switch (m_pValues.Get<HideGenetix>().HideType)
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
            pColor.RGB = m_pValues.Get<HideGenetix>().HideColor;

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
            foreach (HairsColor eColor in m_pValues.Get<HairsGenetix>().m_cHairColors)
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
            if (m_pValues.Get<BodyGenetix>().BodyBuild == BodyBuild.Skinny)
            {
                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Open))
                    cLandTypes[pLand.m_eType] *= 2;

                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.Open, LandscapeGeneration.Environment.Flat))
                    cLandTypes[pLand.m_eType] = 0;
            }

            //склонность к тучности мешает выживанию на пересечённой местности
            if (m_pValues.Get<BodyGenetix>().BodyBuild == BodyBuild.Fat)
            {
                foreach (LTI pLand in GetLands(LandscapeGeneration.Environment.None, LandscapeGeneration.Environment.Flat))
                    cLandTypes[pLand.m_eType] = 0;
            }

            //повышенная мускулистость отлично сочетается с горами
            if (m_pValues.Get<BodyGenetix>().BodyBuild == BodyBuild.Muscular)
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
