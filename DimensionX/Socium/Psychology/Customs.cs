using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneLab;
using GeneLab.Genetix;
using Random;

namespace Socium.Psychology
{
    public class Customs
    {
        public enum GenderPriority
        {
            Matriarchy,
            Genders_equality,
            Patriarchy,
        }

        public enum MindSet
        {
            Emotions,
            Balanced_mind,
            Logic
        }

        public enum Sexuality
        {
            Lecherous,
            Moderate_sexuality,
            Puritan
        }

        public enum SexualOrientation
        {
            Heterosexual,
            Bisexual,
            Homosexual
        }

        public enum MarriageType
        {
            Monogamy,
            Polygamy,
            Polyamory
        }

        public enum BodyModifications
        {
            Body_Modifications_Mandatory,
            Body_Modifications_Allowed,
            Body_Modifications_Blamed
        }

        public enum BodyModificationsTypes
        {
            Specific_Face_Paint,
            Specific_Body_Paint,
            Small_Ear_Rings,
            Huge_Ear_Rings,
            Nose_Ring,
            Specific_Face_Tatoo,
            Specific_Body_Tatoo,
            Brand,
            Circumcision
        }

        public enum FamilyValues
        {
            Praised_Family_Values,
            Moderate_Family_Values,
            No_Family_Values
        }

        public enum Science
        {
            Technophobia,
            Moderate_Science,
            Ingenuity
        }

        public enum Magic
        {
            Magic_Feared,
            Magic_Allowed,
            Magic_Praised
        }

        public enum Clothes
        {
            Covering_Clothes,
            Revealing_Clothes,
            Minimal_Clothes
        }

        public enum Hairstyle
        {
            Gorgeous_Hairstyles,
            Practical_Hairsyles,
            Bald_Heads
        }

        private bool m_bNoHair = false;

        public enum Beard
        {
            Groomed_Beards,
            Common_Beards,
            Shaved_Faces
        }

        private bool m_bNoBeard = false;

        public enum Adornments
        {
            No_Adornments,
            Some_Adornments,
            Lavish_Adornments
        }

        private readonly Dictionary<Type, dynamic> m_cCustoms = new Dictionary<Type, dynamic>();

        public void Accept<T>(T value) where T : Enum
        {
            m_cCustoms[typeof(T)] = value;
        }

        public T ValueOf<T>() where T : Enum
        {
            return m_cCustoms[typeof(T)];
        }

        public bool Has<T>(T value) where T : Enum
        {
            if (!m_cCustoms.TryGetValue(typeof(T), out dynamic storedValue))
                return false;

            return storedValue == value;
        }

        public bool HasIdentical<T>(Customs pOther) where T : Enum
        {
            return HasIdentical(typeof(T), pOther);
        }

        public bool HasIdentical(Type customType, Customs pOther)
        {
            if (pOther == null)
                return false;

            if (!m_cCustoms.TryGetValue(customType, out dynamic storedValue))
                return false;

            if (!pOther.m_cCustoms.TryGetValue(customType, out dynamic storedValueOther))
                return false;

            return storedValue == storedValueOther;
        }

        public List<BodyModificationsTypes> MandatoryBodyModifications { get; } = new List<BodyModificationsTypes>();

        // Находит все отличия между pBaseSample и pDifferencesSample и затем накладывает их на pBase
        public static Customs ApplyDifferences(Customs pBase, Customs pBaseSample, Customs pDifferencesSample)
        {
            Customs pNew = new Customs(pBase, Mutation.None);

            foreach (var custom in pBase.m_cCustoms)
            {
                if (custom.Value != pBaseSample.m_cCustoms[custom.Key])
                    pNew.Accept(pDifferencesSample.m_cCustoms[custom.Key]);
            }

            foreach (BodyModificationsTypes eMod in pDifferencesSample.MandatoryBodyModifications)
            {
                if (!pBaseSample.MandatoryBodyModifications.Contains(eMod) && !pNew.MandatoryBodyModifications.Contains(eMod))
                    pNew.MandatoryBodyModifications.Add(eMod);
            }

            foreach (BodyModificationsTypes eMod in pBaseSample.MandatoryBodyModifications)
            {
                if (!pDifferencesSample.MandatoryBodyModifications.Contains(eMod) && pNew.MandatoryBodyModifications.Contains(eMod))
                    pNew.MandatoryBodyModifications.Remove(eMod);
            }

            return pNew;
        }

        public enum Mutation
        {
            None,
            Possible,
            Mandatory
        }

        private bool TrySimpleMutation<T>(Type selectedCustom, T middleValue, T majorValue, T minorValue, int majorWeight = 1, int minorWeight = 1) where T : Enum
        {
            if (selectedCustom != typeof(T))
                return false;

            if (Has(middleValue))
                Accept(Rnd.ChooseOne(majorWeight, minorWeight) ? majorValue : minorValue);
            else
                Accept(middleValue);

            return true;
        }

        public Customs()
        {
            Accept(Rnd.OneChanceFrom(5) ? GenderPriority.Genders_equality : Rnd.OneChanceFrom(2) ? GenderPriority.Matriarchy : GenderPriority.Patriarchy);

            Accept(!Rnd.OneChanceFrom(3) ? MindSet.Balanced_mind : Rnd.OneChanceFrom(2) ? MindSet.Logic : MindSet.Emotions);

            Accept(!Rnd.OneChanceFrom(3) ? Sexuality.Moderate_sexuality : Rnd.OneChanceFrom(5) ? Sexuality.Puritan : Sexuality.Lecherous);

            if (Has(GenderPriority.Matriarchy))
                Accept(Rnd.OneChanceFrom(3) ? SexualOrientation.Bisexual : Rnd.OneChanceFrom(2) ? SexualOrientation.Homosexual : SexualOrientation.Heterosexual);
            else
                Accept(Rnd.OneChanceFrom(5) ? SexualOrientation.Bisexual : Rnd.OneChanceFrom(9) ? SexualOrientation.Homosexual : SexualOrientation.Heterosexual);

            Accept(Rnd.OneChanceFrom(3) ? MarriageType.Polygamy : Rnd.OneChanceFrom(2) ? MarriageType.Polyamory : MarriageType.Monogamy);

            Accept(Rnd.OneChanceFrom(5) ? BodyModifications.Body_Modifications_Allowed : Rnd.OneChanceFrom(3) ? BodyModifications.Body_Modifications_Mandatory : BodyModifications.Body_Modifications_Blamed);

            if (Has(BodyModifications.Body_Modifications_Mandatory))
                MandatoryBodyModifications.Add((BodyModificationsTypes)Rnd.Get(typeof(BodyModificationsTypes)));

            Accept(Rnd.OneChanceFrom(3) ? Clothes.Revealing_Clothes : Rnd.OneChanceFrom(3) ? Clothes.Minimal_Clothes : Clothes.Covering_Clothes);

            Accept(Rnd.OneChanceFrom(3) ? Hairstyle.Gorgeous_Hairstyles : Rnd.OneChanceFrom(3) ? Hairstyle.Practical_Hairsyles : Hairstyle.Bald_Heads);

            Accept(Rnd.OneChanceFrom(3) ? Beard.Groomed_Beards : Rnd.OneChanceFrom(3) ? Beard.Common_Beards : Beard.Shaved_Faces);

            Accept(Rnd.OneChanceFrom(3) ? Adornments.Some_Adornments : Rnd.OneChanceFrom(3) ? Adornments.No_Adornments : Adornments.Lavish_Adornments);

            Accept(Rnd.OneChanceFrom(2) ? FamilyValues.Moderate_Family_Values : Rnd.OneChanceFrom(2) ? FamilyValues.Praised_Family_Values : FamilyValues.No_Family_Values);

            Accept(Rnd.OneChanceFrom(2) ? Science.Moderate_Science : Rnd.OneChanceFrom(2) ? Science.Ingenuity : Science.Technophobia);

            Accept(Rnd.OneChanceFrom(2) ? Magic.Magic_Allowed : Rnd.OneChanceFrom(2) ? Magic.Magic_Praised : Magic.Magic_Feared);
        }

        public Customs(Customs pAncestorCustoms, Mutation eDifference)
        {
            foreach (var custom in pAncestorCustoms.m_cCustoms)
            {
                Accept(custom.Value);
            }

            MandatoryBodyModifications.AddRange(pAncestorCustoms.MandatoryBodyModifications);

            if (eDifference == Mutation.None)
                return;

            if (eDifference == Mutation.Mandatory || Rnd.OneChanceFrom(2))
            {
                var mutatedCustomType = Rnd.Get(m_cCustoms.Keys);

                if (TrySimpleMutation(mutatedCustomType, MindSet.Balanced_mind, MindSet.Logic, MindSet.Emotions) ||
                    TrySimpleMutation(mutatedCustomType, Sexuality.Moderate_sexuality, Sexuality.Lecherous, Sexuality.Puritan, 4) ||
                    TrySimpleMutation(mutatedCustomType, MarriageType.Polygamy, MarriageType.Monogamy, MarriageType.Polyamory, 4) ||
                    TrySimpleMutation(mutatedCustomType, FamilyValues.Moderate_Family_Values, FamilyValues.Praised_Family_Values, FamilyValues.No_Family_Values) ||
                    TrySimpleMutation(mutatedCustomType, Science.Moderate_Science, Science.Technophobia, Science.Ingenuity, 2) ||
                    TrySimpleMutation(mutatedCustomType, Magic.Magic_Allowed, Magic.Magic_Feared, Magic.Magic_Praised, 2) ||
                    TrySimpleMutation(mutatedCustomType, Clothes.Revealing_Clothes, Clothes.Covering_Clothes, Clothes.Minimal_Clothes, 2) ||
                    TrySimpleMutation(mutatedCustomType, Hairstyle.Practical_Hairsyles, Hairstyle.Gorgeous_Hairstyles, Hairstyle.Bald_Heads, 2) ||
                    TrySimpleMutation(mutatedCustomType, Beard.Common_Beards, Beard.Shaved_Faces, Beard.Groomed_Beards) ||
                    TrySimpleMutation(mutatedCustomType, Adornments.Some_Adornments, Adornments.Lavish_Adornments, Adornments.No_Adornments, 2))
                {
                    // just to keep pretty if-else-if-else-if... structure
                }
                else if (mutatedCustomType == typeof(GenderPriority))
                {
                    if (Has(GenderPriority.Genders_equality))
                        Accept(Rnd.OneChanceFrom(2) ? GenderPriority.Matriarchy : GenderPriority.Patriarchy);
                    else
                        Accept(Rnd.OneChanceFrom(2) ? (Has(GenderPriority.Matriarchy) ? GenderPriority.Patriarchy : GenderPriority.Matriarchy) : GenderPriority.Genders_equality);
                }
                else if (mutatedCustomType == typeof(SexualOrientation))
                {
                    if (Has(SexualOrientation.Bisexual))
                    {
                        if (Has(GenderPriority.Matriarchy))
                            Accept(Rnd.OneChanceFrom(2) ? SexualOrientation.Homosexual : SexualOrientation.Heterosexual);
                        else
                            Accept(Rnd.OneChanceFrom(9) ? SexualOrientation.Homosexual : SexualOrientation.Heterosexual);
                    }
                    else
                    {
                        if (Has(GenderPriority.Matriarchy))
                            Accept(Rnd.OneChanceFrom(2) ? (Has(SexualOrientation.Homosexual) ? SexualOrientation.Heterosexual : SexualOrientation.Homosexual) : SexualOrientation.Bisexual);
                        else
                            Accept(Rnd.OneChanceFrom(9) ? (Has(SexualOrientation.Homosexual) ? SexualOrientation.Heterosexual : SexualOrientation.Homosexual) : SexualOrientation.Bisexual);
                    }
                }
                else if (mutatedCustomType == typeof(BodyModifications))
                {
                    if (Has(BodyModifications.Body_Modifications_Allowed))
                        Accept(Rnd.OneChanceFrom(5) ? BodyModifications.Body_Modifications_Mandatory : BodyModifications.Body_Modifications_Blamed);
                    else if (Has(BodyModifications.Body_Modifications_Blamed) || Rnd.OneChanceFrom(2))
                        Accept(BodyModifications.Body_Modifications_Allowed);

                    if (Has(BodyModifications.Body_Modifications_Mandatory))
                    {
                        if (MandatoryBodyModifications.Count > 1 && Rnd.OneChanceFrom(3))
                        {
                            MandatoryBodyModifications.RemoveAt(Rnd.Get(MandatoryBodyModifications.Count));
                        }
                        else if (MandatoryBodyModifications.Count > 0 && Rnd.OneChanceFrom(2))
                        {
                            BodyModificationsTypes eNewValue;
                            do
                            {
                                eNewValue = (BodyModificationsTypes)Rnd.Get(typeof(BodyModificationsTypes));
                            }
                            while (MandatoryBodyModifications.Contains(eNewValue));
                            MandatoryBodyModifications[Rnd.Get(MandatoryBodyModifications.Count)] = eNewValue;
                        }
                        else
                        {
                            BodyModificationsTypes eNewValue;
                            do
                            {
                                eNewValue = (BodyModificationsTypes)Rnd.Get(typeof(BodyModificationsTypes));
                            }
                            while (MandatoryBodyModifications.Contains(eNewValue));
                            MandatoryBodyModifications.Add(eNewValue);
                        }
                    }
                }
            }

            if (eDifference == Mutation.Mandatory && this.Equals(pAncestorCustoms))
                    throw new InvalidOperationException("Customs not changed!");
        }

        public void ApplyFenotype(Phenotype pFenotype)
        {
            if (pFenotype.m_pValues.Get<EarsGenetix>().EarsType == EarsType.None)
            {
                if (MandatoryBodyModifications.Contains(BodyModificationsTypes.Small_Ear_Rings))
                    MandatoryBodyModifications.Remove(BodyModificationsTypes.Small_Ear_Rings);
                if (MandatoryBodyModifications.Contains(BodyModificationsTypes.Huge_Ear_Rings))
                    MandatoryBodyModifications.Remove(BodyModificationsTypes.Huge_Ear_Rings);
            }
            if (pFenotype.m_pValues.Get<FaceGenetix>().NoseType == NoseType.None)
            {
                if (MandatoryBodyModifications.Contains(BodyModificationsTypes.Nose_Ring))
                    MandatoryBodyModifications.Remove(BodyModificationsTypes.Nose_Ring);
            }

            if (pFenotype.m_pValues.Get<HairsGenetix>().Hairs == HairsAmount.None)
            {
                Accept(Hairstyle.Bald_Heads);
                m_bNoHair = true;
            }
            if (pFenotype.m_pValues.Get<HairsGenetix>().Beard == HairsAmount.None)
            {
                Accept(Beard.Shaved_Faces);
                m_bNoBeard = true;
            }
        }

        public string GetCustomsList()
        {
            StringBuilder sResult = new StringBuilder();

            void addLine(string str)
            {
                sResult.AppendLine().Append("   ").Append(str);
            }

            if (Has(GenderPriority.Patriarchy))
            {
                addLine("manhood");
            }
            else if (Has(GenderPriority.Matriarchy))
            {
                addLine("womanhood");
            }

            if (Has(MindSet.Emotions))
            {
                addLine("emotions");
            }
            else if (Has(MindSet.Logic))
            {
                addLine("pure logic");
            }

            if (Has(Sexuality.Puritan))
            {
                addLine("chastity");
            }
            else if (Has(Sexuality.Lecherous))
            {
                addLine("unlimited sexuality");
            }

            if (Has(SexualOrientation.Heterosexual))
            {
                addLine("cross sex relations");
            }
            else if (Has(SexualOrientation.Homosexual))
            {
                addLine("same sex relations");
            }

            if (Has(MarriageType.Monogamy))
            {
                addLine("monogamy");
            }
            else if (Has(MarriageType.Polyamory))
            {
                addLine("wedlock denial");
            }

            if (Has(BodyModifications.Body_Modifications_Blamed))
            {
                addLine("unmodified body");
            }
            else if (Has(BodyModifications.Body_Modifications_Mandatory))
            {
                addLine("modified body: ");
                bool bFirst = true;
                foreach (BodyModificationsTypes eMod in MandatoryBodyModifications)
                {
                    if (!bFirst)
                    {
                        sResult.Append(", ");
                    }
                    sResult.Append(eMod.ToString().Replace('_', ' ').ToLower());
                    bFirst = false;
                }
            }

            if (Has(Clothes.Minimal_Clothes))
            {
                addLine("common nudity");
            }
            else if (Has(Clothes.Covering_Clothes))
            {
                addLine("restricted nudity");
            }

            if (Has(Hairstyle.Bald_Heads))
            {
                addLine("bald heads");
            }
            else if (Has(Hairstyle.Gorgeous_Hairstyles))
            {
                addLine("long hairs");
            }

            if (Has(Beard.Shaved_Faces))
            {
                addLine("no beards");
            }
            else if (Has(Beard.Groomed_Beards))
            {
                addLine("long beards");
            }

            if (Has(Adornments.No_Adornments))
            {
                addLine("no jewelry");
            }
            else if (Has(Adornments.Lavish_Adornments))
            {
                addLine("lavish jewelry");
            }

            if (Has(FamilyValues.Praised_Family_Values))
            {
                addLine("family relations");
            }
            else if (Has(FamilyValues.No_Family_Values))
            {
                addLine("no family ties");
            }

            if (Has(Science.Technophobia))
            {
                addLine("traditions");
            }
            else if (Has(Science.Ingenuity))
            {
                addLine("technical progress");
            }

            if (Has(Magic.Magic_Feared))
            {
                addLine("magic denial");
            }
            else if (Has(Magic.Magic_Praised))
            {
                addLine("magic");
            }

            return "Praises: " + sResult + "\n";
        }

        public string GetCustomsDescription()
        {
            string sGenderPriority = "";
            StringBuilder sResult = new StringBuilder();

            if (Has(GenderPriority.Patriarchy))
            {
                sGenderPriority = "patriarchal";
            }
            else if (Has(GenderPriority.Matriarchy))
            {
                sGenderPriority = "matriarchal";
            }
            else if (Has(GenderPriority.Genders_equality))
            {
                sGenderPriority = "gender equality";
            }

            void addLine(string str)
            {
                if (sResult.Length > 0)
                    sResult.Append(", ");
                sResult.Append(str);
            }

            if (Has(MindSet.Emotions))
            {
                addLine("are guided mostly by emotions");
            }
            else if (Has(MindSet.Logic))
            {
                addLine("are guided mostly by pure logic");
            }

            if (Has(Magic.Magic_Feared))
            {
                addLine("denies any form of magic");
            }
            else if (Has(Magic.Magic_Praised))
            {
                addLine("accepts any form of magic");
            }

            if (Has(BodyModifications.Body_Modifications_Blamed))
            {
                addLine("condemns any tatoo or pierceing");
            }
            else if (Has(BodyModifications.Body_Modifications_Mandatory))
            {
                addLine("should have ");
                bool bFirst2 = true;
                foreach (BodyModificationsTypes eMod in MandatoryBodyModifications)
                {
                    if (!bFirst2)
                    {
                        sResult.Append(" and ");
                    }
                    sResult.Append(eMod.ToString().Replace('_', ' ').ToLower());
                    bFirst2 = false;
                }
            }

            if (Has(Clothes.Minimal_Clothes))
            {
                addLine("wears minimal clothes");
            }
            else if (Has(Clothes.Covering_Clothes))
            {
                addLine("wears clothes hiding entire body");
            }

            if (!m_bNoHair)
            {
                if (Has(Hairstyle.Bald_Heads))
                {
                    addLine("shave their heads");
                }
                else if (Has(Hairstyle.Gorgeous_Hairstyles))
                {
                    addLine("spends a lot of effort on hair care");
                }
            }

            if (!m_bNoBeard)
            {
                if (Has(Beard.Shaved_Faces))
                {
                    addLine("have no beard or moustaches");
                }
                else if (Has(Beard.Groomed_Beards))
                {
                    addLine("carefully grooms their beards");
                }
            }

            if (Has(Adornments.No_Adornments))
            {
                addLine("uses no jewelry");
            }
            else if (Has(Adornments.Lavish_Adornments))
            {
                addLine("uses a lot of jewelry");
            }

            if (Has(Science.Technophobia))
            {
                addLine("are afraid of new technologies");
            }
            else if (Has(Science.Ingenuity))
            {
                addLine("are pretty creative");
            }

            if (Has(FamilyValues.Praised_Family_Values))
            {
                addLine("highly appreciates family values");
            }
            else if (Has(FamilyValues.No_Family_Values))
            {
                addLine("not paying much attention to family ties");
            }

            if (Has(Sexuality.Puritan))
            {
                addLine("uses sex only for reproduction");
            }
            else if (Has(Sexuality.Lecherous))
            {
                addLine("makes sex a lot");
            }

            if (Has(MarriageType.Monogamy))
            {
                if (sResult.Length > 0)
                    sResult.Append(" and ");
                sResult.Append("have one");

                if (Has(SexualOrientation.Heterosexual))
                {
                    if (Has(GenderPriority.Patriarchy))
                    {
                        sResult.Append(" wife");
                    }
                    else if (Has(GenderPriority.Matriarchy))
                    {
                        sResult.Append(" husband");
                    }
                    else if (Has(GenderPriority.Genders_equality))
                    {
                        sResult.Append(" spouse");
                    }
                }
                else if (Has(SexualOrientation.Homosexual))
                {
                    if (Has(Sexuality.Puritan))
                        sResult.Append(" partner of the same gender");
                    else
                        sResult.Append(" spouse of the same gender");
                }
                else if (Has(SexualOrientation.Bisexual))
                {
                    if (Has(Sexuality.Puritan))
                        sResult.Append(" partner of any gender");
                    else
                        sResult.Append(" spouse (of any gender)");
                }
            }
            else if (Has(MarriageType.Polyamory))
            {
                if (sResult.Length > 0)
                    sResult.Append(" and ");
                sResult.Append("denies wedlock");

                if (Has(Sexuality.Puritan))
                    sResult.Append(", but may be in platonic relations with persons");
                else
                    sResult.Append(", but have a number of lovers");

                if (Has(SexualOrientation.Heterosexual))
                {
                    sResult.Append(" of opposite gender");
                }
                else if (Has(SexualOrientation.Homosexual))
                {
                    sResult.Append(" of the same gender");
                }
                else if (Has(SexualOrientation.Bisexual))
                {
                    sResult.Append(" of both genders");
                }
            }
            else if (Has(MarriageType.Polygamy))
            {
                if (sResult.Length > 0)
                    sResult.Append(" and ");

                if (Has(SexualOrientation.Heterosexual))
                {
                    if (Has(GenderPriority.Patriarchy))
                    {
                        sResult.Append("have many wifes");
                    }
                    else if (Has(GenderPriority.Matriarchy))
                    {
                        sResult.Append("have many husbands");
                    }
                    else if (Has(GenderPriority.Genders_equality))
                    {
                        sResult.Append("lives in big heterosexual families");
                    }
                }
                else if (Has(SexualOrientation.Homosexual))
                {
                    if (Has(Sexuality.Puritan))
                    {
                        if (Has(GenderPriority.Patriarchy))
                        {
                            sResult.Append("lives in large brotherhoods");
                        }
                        else if (Has(GenderPriority.Matriarchy))
                        {
                            sResult.Append("lives in large sisterhoods");
                        }
                        else if (Has(GenderPriority.Genders_equality))
                        {
                            sResult.Append("lives in large communes with persons of the same gender");
                        }
                    }
                    else
                    {
                        sResult.Append("lives in big homosexual families");
                    }
                }
                else if (Has(SexualOrientation.Bisexual))
                {
                    if (Has(Sexuality.Puritan))
                        sResult.Append("lives in large communes with persons of both genders");
                    else
                        sResult.Append("lives in big bisexual families");
                }
            }

            return "This is a " + sGenderPriority + " society, which members " + sResult.ToString() + ".";
        }

        public static bool ListsEqual(IList pList1, IList pList2)
        {
            if (pList1 == pList2)
                return true;

            if (pList1 == null || pList2 == null)
                return false;

            if (pList1.Count != pList2.Count)
                return false;

            foreach (var pItem1 in pList1)
            {
                if (!pList2.Contains(pItem1))
                    return false;
            }

            foreach (var pItem2 in pList2)
            {
                if (!pList1.Contains(pItem2))
                    return false;
            }

            return true;
        }

        public string GetCustomsDiffString2(Customs pOther)
        {
            StringBuilder sResult = new StringBuilder();

            if (!HasIdentical<GenderPriority>(pOther))
            {
                if (Has(GenderPriority.Patriarchy))
                    sResult.Append("proclaims males supremacy");
                else if (Has(GenderPriority.Matriarchy))
                    sResult.Append("proclaims females supremacy");
                else if (Has(GenderPriority.Genders_equality))
                    sResult.Append("has no gender prevalence");
            }

            if (!HasIdentical<MindSet>(pOther))
            {
                if (sResult.Length > 0)
                    sResult.Append(", ");

                if (Has(MindSet.Emotions))
                    sResult.Append("shows a lot of emotions");
                else if (Has(MindSet.Logic))
                    sResult.Append("supppresses their emotions");
                else if (Has(MindSet.Balanced_mind))
                    sResult.Append("combines emotions and logic");
            }

            if (!HasIdentical<Magic>(pOther))
            {
                if (sResult.Length > 0)
                    sResult.Append(", ");

                if (Has(Magic.Magic_Feared))
                    sResult.Append("fears all magic");
                else if (Has(Magic.Magic_Praised))
                    sResult.Append("praises any form of magic");
                else if (Has(Magic.Magic_Allowed))
                    sResult.Append(pOther.Has(Magic.Magic_Feared) ? "has no fear for magic" : "doesn't like magic too much");
            }

            if (!HasIdentical<BodyModifications>(pOther) || !ListsEqual(MandatoryBodyModifications, pOther.MandatoryBodyModifications))
            {
                if (sResult.Length > 0)
                    sResult.Append(", ");

                if (Has(BodyModifications.Body_Modifications_Blamed))
                {
                    sResult.Append("condemns any tatoo or pierceing");
                }
                else if (Has(BodyModifications.Body_Modifications_Mandatory))
                {
                    List<BodyModificationsTypes> cOthers = new List<BodyModificationsTypes>();
                    if (pOther.Has(BodyModifications.Body_Modifications_Mandatory))
                        cOthers.AddRange(pOther.MandatoryBodyModifications);

                    bool bFirst2 = true;
                    foreach (BodyModificationsTypes eMod in MandatoryBodyModifications)
                    {
                        if (cOthers.Contains(eMod))
                            continue;
                        if (bFirst2)
                            sResult.Append("should have ");
                        else
                            sResult.Append(" and ");

                        sResult.Append(eMod.ToString().Replace('_', ' ').ToLower());
                        bFirst2 = false;
                    }

                    bool bFirst3 = true;
                    foreach (BodyModificationsTypes eMod in cOthers)
                    {
                        if (MandatoryBodyModifications.Contains(eMod))
                            continue;
                        if (bFirst3)
                        {
                            if (bFirst2)
                                sResult.Append("should not have ");
                            else
                                sResult.Append(", but should not have ");
                        }

                        sResult.Append(eMod.ToString().Replace('_', ' ').ToLower());
                        bFirst3 = false;
                    }
                }
                else if (Has(BodyModifications.Body_Modifications_Allowed))
                {
                    sResult.Append(pOther.Has(BodyModifications.Body_Modifications_Blamed) ? "allowed to have some tatoo or pierceing" : "could have tatoo or pierceing on their choice");
                }
            }

            if (!HasIdentical<Clothes>(pOther))
            {
                if (sResult.Length > 0)
                    sResult.Append(", ");

                if (Has(Clothes.Covering_Clothes))
                    sResult.Append("wears extremely covering clothes");
                else if (Has(Clothes.Minimal_Clothes))
                    sResult.Append("wears extremely revealing clothes");
                else if (Has(Clothes.Revealing_Clothes))
                    sResult.Append(pOther.Has(Clothes.Covering_Clothes) ? "wears revealing clothes" : "wears covering clothes");
            }

            if (!HasIdentical<Hairstyle>(pOther) && !m_bNoHair)
            {
                if (sResult.Length > 0)
                    sResult.Append(", ");

                if (Has(Hairstyle.Bald_Heads))
                    sResult.Append("shaves their heads completely bald");
                else if (Has(Hairstyle.Gorgeous_Hairstyles))
                    sResult.Append("makes complex hairsyles");
                else if (Has(Hairstyle.Practical_Hairsyles))
                    sResult.Append(pOther.Has(Hairstyle.Bald_Heads) ? "not shaves their heads" : "not spends much efforts on their hairs");
            }

            if (!HasIdentical<Beard>(pOther) && !m_bNoBeard)
            {
                if (sResult.Length > 0)
                    sResult.Append(", ");

                if (Has(Beard.Shaved_Faces))
                    sResult.Append("has nor beard, neither moustaches");
                else if (Has(Beard.Groomed_Beards))
                    sResult.Append("has perfectly groomed beards");
                else if (Has(Beard.Common_Beards))
                    sResult.Append(pOther.Has(Beard.Shaved_Faces) ? "has hairs on their faces" : "not grooming their beards much");
            }

            if (!HasIdentical<Adornments>(pOther))
            {
                if (sResult.Length > 0)
                    sResult.Append(", ");

                if (Has(Adornments.No_Adornments))
                    sResult.Append("uses no jevelry");
                else if (Has(Adornments.Lavish_Adornments))
                    sResult.Append("uses a lot of jevelry");
                else if (Has(Adornments.Some_Adornments))
                    sResult.Append(pOther.Has(Adornments.No_Adornments) ? "could use some jevelry" : "uses not so much jevelry");
            }

            if (!HasIdentical<Science>(pOther))
            {
                if (sResult.Length > 0)
                    sResult.Append(", ");

                if (Has(Science.Technophobia))
                    sResult.Append("rejects any novelties");
                else if (Has(Science.Ingenuity))
                    sResult.Append("likes any novelties");
                else if (Has(Science.Moderate_Science))
                    sResult.Append(pOther.Has(Science.Technophobia) ? "have no fear of novelties" : "doesn't like novelties so much");
            }

            if (!HasIdentical<FamilyValues>(pOther))
            {
                if (sResult.Length > 0)
                    sResult.Append(", ");

                if (Has(FamilyValues.Praised_Family_Values))
                    sResult.Append("are very bound to family");
                else if (Has(FamilyValues.No_Family_Values))
                    sResult.Append("not bound to family at all");
                else if (Has(FamilyValues.Moderate_Family_Values))
                    sResult.Append("not so bound to family");
            }

            if (!HasIdentical<Sexuality>(pOther))
            {
                if (sResult.Length > 0)
                    sResult.Append(", ");

                if (Has(Sexuality.Puritan))
                    sResult.Append("not interested in sex, except for reproduction");
                else if (Has(Sexuality.Lecherous))
                    sResult.Append("has insatiable libido");
                else if (Has(Sexuality.Moderate_sexuality))
                    sResult.Append(pOther.Has(Sexuality.Puritan) ? "likes to have sex for fun and pleasure" : "less interested in sex");
            }

            if (!HasIdentical<SexualOrientation>(pOther))
            {
                if (sResult.Length > 0)
                    sResult.Append(", ");

                if (Has(SexualOrientation.Heterosexual))
                    sResult.Append("prefers heterosexual relations");
                else if (Has(SexualOrientation.Homosexual))
                    sResult.Append("prefers homosexual relations");
                else if (Has(SexualOrientation.Bisexual))
                    sResult.Append(pOther.Has(SexualOrientation.Heterosexual) ? "allows homosexual relations" : "allows heterosexual relations");
            }

            if (!HasIdentical<MarriageType>(pOther))
            {
                if (sResult.Length > 0)
                    sResult.Append(" and ");

                if (Has(MarriageType.Monogamy))
                {
                    sResult.Append(pOther.Has(MarriageType.Polygamy) ? "could have only one" : "could have one");

                    if (Has(SexualOrientation.Heterosexual))
                    {
                        if (Has(GenderPriority.Patriarchy))
                        {
                            sResult.Append(" wife");
                        }
                        else if (Has(GenderPriority.Matriarchy))
                        {
                            sResult.Append(" husband");
                        }
                        else if (Has(GenderPriority.Genders_equality))
                        {
                            sResult.Append(" spouse of opposite gender");
                        }
                    }
                    else if (Has(SexualOrientation.Homosexual))
                    {
                        if (Has(Sexuality.Puritan))
                            sResult.Append(" partner of the same gender");
                        else
                            sResult.Append(" spouse of the same gender");
                    }
                    else if (Has(SexualOrientation.Bisexual))
                    {
                        if (Has(Sexuality.Puritan))
                            sResult.Append(" partner of any gender");
                        else
                            sResult.Append(" spouse of any gender");
                    }
                }
                else if (Has(MarriageType.Polyamory))
                {
                    sResult.Append("rejects marriage");

                    if (Has(Sexuality.Puritan))
                        sResult.Append(", but may be in platonic relations with persons");
                    else
                        sResult.Append(", but could have a number of lovers");

                    if (Has(SexualOrientation.Heterosexual))
                    {
                        sResult.Append(" of opposite gender");
                    }
                    else if (Has(SexualOrientation.Homosexual))
                    {
                        sResult.Append(" of the same gender");
                    }
                    else if (Has(SexualOrientation.Bisexual))
                    {
                        sResult.Append(" of both genders");
                    }
                }
                else if (Has(MarriageType.Polygamy))
                {
                    if (Has(SexualOrientation.Heterosexual))
                    {
                        if (Has(GenderPriority.Patriarchy))
                        {
                            sResult.Append("could have many wifes");
                        }
                        else if (Has(GenderPriority.Matriarchy))
                        {
                            sResult.Append("could have many husbands");
                        }
                        else if (Has(GenderPriority.Genders_equality))
                        {
                            sResult.Append("lives in big heterosexual families");
                        }
                    }
                    else if (Has(SexualOrientation.Homosexual))
                    {
                        if (Has(Sexuality.Puritan))
                        {
                            if (Has(GenderPriority.Patriarchy))
                            {
                                sResult.Append("lives in large brotherhoods");
                            }
                            else if (Has(GenderPriority.Matriarchy))
                            {
                                sResult.Append("lives in large sisterhoods");
                            }
                            else if (Has(GenderPriority.Genders_equality))
                            {
                                sResult.Append("lives in large communes with persons of the same gender");
                            }
                        }
                        else
                        {
                            sResult.Append("lives in big homosexual families");
                        }
                    }
                    else if (Has(SexualOrientation.Bisexual))
                    {
                        if (Has(Sexuality.Puritan))
                            sResult.Append("lives in large communes with persons of both genders");
                        else
                            sResult.Append("lives in big bisexual families");
                    }
                }
            }

            return sResult.ToString();
        }

        private void CheckDifference<T>(Customs pOpponent, T middleValue, ref int iHostility, ref StringBuilder sPositiveReasons, ref StringBuilder sNegativeReasons) where T : Enum, IComparable
        {
            if (HasIdentical<T>(pOpponent))
            {
                iHostility--;
                sPositiveReasons.Append(" (+1) ").AppendLine(pOpponent.ValueOf<T>().ToString().Replace('_', ' '));
            }
            else
            {
                if (!Has(middleValue) && !pOpponent.Has(middleValue))
                {
                    iHostility += 2;
                    sNegativeReasons.Append(" (-2) ").AppendLine(pOpponent.ValueOf<T>().ToString().Replace('_', ' '));
                }
                else
                {
                    iHostility++;
                    sNegativeReasons.Append(" (-1) ").AppendLine(pOpponent.ValueOf<T>().ToString().Replace('_', ' '));
                }
            }
        }

        /// <summary>
        /// Вычисляет враждебность друг сообществ на основании их обычаев.
        /// Возвращает значение от 22 (полные противоположности) до -11 (полное совпадение).
        /// </summary>
        public int GetDifference(Customs pOpponent)
        {
            StringBuilder s1 = new StringBuilder(), s2 = new StringBuilder();
            return GetDifference(pOpponent, ref s1, ref s2);
        }

        /// <summary>
        /// Вычисляет враждебность друг сообществ на основании их обычаев.
        /// Возвращает значение от 22 (полные противоположности) до -11 (полное совпадение).
        /// </summary>
        /// <param name="pOpponent">обычаи другого сообщества</param>
        /// <param name="sReasons">мотивация враждебности</param>
        /// <returns></returns>
        public int GetDifference(Customs pOpponent, ref StringBuilder sPositiveReasons, ref StringBuilder sNegativeReasons)
        {
            int iHostility = 0;

            CheckDifference(pOpponent, GenderPriority.Genders_equality, ref iHostility, ref sPositiveReasons, ref sNegativeReasons);

            CheckDifference(pOpponent, MindSet.Balanced_mind, ref iHostility, ref sPositiveReasons, ref sNegativeReasons);

            CheckDifference(pOpponent, Sexuality.Moderate_sexuality, ref iHostility, ref sPositiveReasons, ref sNegativeReasons);

            CheckDifference(pOpponent, SexualOrientation.Bisexual, ref iHostility, ref sPositiveReasons, ref sNegativeReasons);

            CheckDifference(pOpponent, MarriageType.Polygamy, ref iHostility, ref sPositiveReasons, ref sNegativeReasons);

            CheckDifference(pOpponent, Clothes.Revealing_Clothes, ref iHostility, ref sPositiveReasons, ref sNegativeReasons);

            CheckDifference(pOpponent, Hairstyle.Practical_Hairsyles, ref iHostility, ref sPositiveReasons, ref sNegativeReasons);

            CheckDifference(pOpponent, Beard.Common_Beards, ref iHostility, ref sPositiveReasons, ref sNegativeReasons);

            CheckDifference(pOpponent, Adornments.Some_Adornments, ref iHostility, ref sPositiveReasons, ref sNegativeReasons);

            CheckDifference(pOpponent, FamilyValues.Moderate_Family_Values, ref iHostility, ref sPositiveReasons, ref sNegativeReasons);

            CheckDifference(pOpponent, Science.Moderate_Science, ref iHostility, ref sPositiveReasons, ref sNegativeReasons);

            CheckDifference(pOpponent, Magic.Magic_Allowed, ref iHostility, ref sPositiveReasons, ref sNegativeReasons);

            if (HasIdentical<BodyModifications>(pOpponent))
            {
                if (!Has(BodyModifications.Body_Modifications_Mandatory) || ListsEqual(MandatoryBodyModifications, pOpponent.MandatoryBodyModifications))
                {
                    iHostility--;
                    sPositiveReasons.Append(" (+1) ").AppendLine(pOpponent.ValueOf<BodyModifications>().ToString().Replace('_', ' '));
                }
            }
            else
            {
                if (!Has(BodyModifications.Body_Modifications_Allowed) &&
                    !pOpponent.Has(BodyModifications.Body_Modifications_Allowed))
                {
                    iHostility += 2;
                    sNegativeReasons.Append(" (-2) ").AppendLine(pOpponent.ValueOf<BodyModifications>().ToString().Replace('_', ' '));
                }
                else
                {
                    iHostility++;
                    sNegativeReasons.Append(" (-1) ").AppendLine(pOpponent.ValueOf<BodyModifications>().ToString().Replace('_', ' '));
                }
            }

            return iHostility;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Customs pOther))
                return false;

            if (m_cCustoms.Count != pOther.m_cCustoms.Count)
                return false;

            foreach (var custom in m_cCustoms)
            {
                if (!pOther.Has(custom.Value))
                    return false;
            }

            if (Has(BodyModifications.Body_Modifications_Mandatory) &&
                !ListsEqual(MandatoryBodyModifications, pOther.MandatoryBodyModifications))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            foreach (var custom in m_cCustoms)
                hash.Add(custom);

            if (Has(BodyModifications.Body_Modifications_Mandatory))
            {
                foreach (var bodyModification in MandatoryBodyModifications)
                    hash.Add(bodyModification);
            }

            return hash.ToHashCode();
        }
    }
}
