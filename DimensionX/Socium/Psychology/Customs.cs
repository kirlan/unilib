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

        public bool m_bNoHair = false;

        public enum Beard
        {
            Groomed_Beards,
            Common_Beards,
            Shaved_Faces
        }

        public bool m_bNoBeard = false;

        public enum Adornments
        {
            No_Adornments,
            Some_Adornments,
            Lavish_Adornments
        }

        private Dictionary<Type, dynamic> m_cCustoms = new Dictionary<Type, dynamic>();

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
            dynamic storedValue;
            if (!m_cCustoms.TryGetValue(typeof(T), out storedValue))
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

            dynamic storedValue;
            if (!m_cCustoms.TryGetValue(customType, out storedValue))
                return false;

            dynamic storedValueOther;
            if (!pOther.m_cCustoms.TryGetValue(customType, out storedValueOther))
                return false;

            return storedValue == storedValueOther;
        }

        public List<BodyModificationsTypes> m_cMandatoryModifications = new List<BodyModificationsTypes>();

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
                m_cMandatoryModifications.Add((BodyModificationsTypes)Rnd.Get(typeof(BodyModificationsTypes)));

            Accept(Rnd.OneChanceFrom(3) ? Clothes.Revealing_Clothes : Rnd.OneChanceFrom(3) ? Clothes.Minimal_Clothes : Clothes.Covering_Clothes);

            Accept(Rnd.OneChanceFrom(3) ? Hairstyle.Gorgeous_Hairstyles : Rnd.OneChanceFrom(3) ? Hairstyle.Practical_Hairsyles : Hairstyle.Bald_Heads);

            Accept(Rnd.OneChanceFrom(3) ? Beard.Groomed_Beards : Rnd.OneChanceFrom(3) ? Beard.Common_Beards : Beard.Shaved_Faces);

            Accept(Rnd.OneChanceFrom(3) ? Adornments.Some_Adornments : Rnd.OneChanceFrom(3) ? Adornments.No_Adornments : Adornments.Lavish_Adornments);

            Accept(Rnd.OneChanceFrom(2) ? FamilyValues.Moderate_Family_Values : Rnd.OneChanceFrom(2) ? FamilyValues.Praised_Family_Values : FamilyValues.No_Family_Values);

            Accept(Rnd.OneChanceFrom(2) ? Science.Moderate_Science : Rnd.OneChanceFrom(2) ? Science.Ingenuity : Science.Technophobia);

            Accept(Rnd.OneChanceFrom(2) ? Magic.Magic_Allowed : Rnd.OneChanceFrom(2) ? Magic.Magic_Praised : Magic.Magic_Feared);
        }

        // Находит все отличия между pBaseSample и pDifferencesSample и затем накладывает их на pBase
        public static Customs ApplyDifferences(Customs pBase, Customs pBaseSample, Customs pDifferencesSample)
        {
            Customs pNew = new Customs(pBase, Mutation.None);

            foreach (var custom in pBase.m_cCustoms)
            {
                if (custom.Value != pBaseSample.m_cCustoms[custom.Key])
                    pNew.Accept(pDifferencesSample.m_cCustoms[custom.Key]);
            }

            foreach (BodyModificationsTypes eMod in pDifferencesSample.m_cMandatoryModifications)
                if (!pBaseSample.m_cMandatoryModifications.Contains(eMod) && !pNew.m_cMandatoryModifications.Contains(eMod))
                    pNew.m_cMandatoryModifications.Add(eMod);

            foreach (BodyModificationsTypes eMod in pBaseSample.m_cMandatoryModifications)
                if (!pDifferencesSample.m_cMandatoryModifications.Contains(eMod) && pNew.m_cMandatoryModifications.Contains(eMod))
                    pNew.m_cMandatoryModifications.Remove(eMod);

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

        public Customs(Customs pAncestorCustoms, Mutation eDifference)
        {
            foreach (var custom in pAncestorCustoms.m_cCustoms)
            {
                Accept(custom.Value);
            }

            m_cMandatoryModifications.AddRange(pAncestorCustoms.m_cMandatoryModifications);

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
                        if (m_cMandatoryModifications.Count > 1 && Rnd.OneChanceFrom(3))
                            m_cMandatoryModifications.RemoveAt(Rnd.Get(m_cMandatoryModifications.Count));
                        else if (m_cMandatoryModifications.Count > 0 && Rnd.OneChanceFrom(2))
                        {
                            BodyModificationsTypes eNewValue;
                            do
                            {
                                eNewValue = (BodyModificationsTypes)Rnd.Get(typeof(BodyModificationsTypes));
                            }
                            while (m_cMandatoryModifications.Contains(eNewValue));
                            m_cMandatoryModifications[Rnd.Get(m_cMandatoryModifications.Count)] = eNewValue;
                        }
                        else
                        {
                            BodyModificationsTypes eNewValue;
                            do
                            {
                                eNewValue = (BodyModificationsTypes)Rnd.Get(typeof(BodyModificationsTypes));
                            }
                            while (m_cMandatoryModifications.Contains(eNewValue));
                            m_cMandatoryModifications.Add(eNewValue);
                        }
                    }
                }
            }

            if (eDifference == Mutation.Mandatory && this.Equals(pAncestorCustoms))
                    throw new Exception("Customs not changed!");
        }

        public void ApplyFenotype(Phenotype pFenotype)
        {
            if (pFenotype.m_pValues.Get<EarsGenetix>().EarsType == EarsType.None)
            {
                if (m_cMandatoryModifications.Contains(BodyModificationsTypes.Small_Ear_Rings))
                    m_cMandatoryModifications.Remove(BodyModificationsTypes.Small_Ear_Rings);
                if (m_cMandatoryModifications.Contains(BodyModificationsTypes.Huge_Ear_Rings))
                    m_cMandatoryModifications.Remove(BodyModificationsTypes.Huge_Ear_Rings);
            }
            if (pFenotype.m_pValues.Get<FaceGenetix>().NoseType == NoseType.None)
            {
                if (m_cMandatoryModifications.Contains(BodyModificationsTypes.Nose_Ring))
                    m_cMandatoryModifications.Remove(BodyModificationsTypes.Nose_Ring);
            }

            if (pFenotype.m_pValues.Get<HairsGenetix>().Hairs == HairsAmount.None)
            {
                Accept(Hairstyle.Bald_Heads);
            }
            if (pFenotype.m_pValues.Get<HairsGenetix>().Beard == HairsAmount.None)
            {
                Accept(Beard.Shaved_Faces);
            }
        }

        public string GetCustomsList()
        {
            string sResult = "";

            if (Has(GenderPriority.Patriarchy))
            {
                sResult += "\n   ";
                sResult += "manhood";
            }
            else if (Has(GenderPriority.Matriarchy))
            {
                sResult += "\n   ";
                sResult += "womanhood";
            }

            if (Has(MindSet.Emotions))
            {
                sResult += "\n   ";
                sResult += "emotions";
            }
            else if (Has(MindSet.Logic))
            {
                sResult += "\n   ";
                sResult += "pure logic";
            }

            if (Has(Sexuality.Puritan))
            {
                sResult += "\n   ";
                sResult += "chastity";
            }
            else if (Has(Sexuality.Lecherous))
            {
                sResult += "\n   ";
                sResult += "unlimited sexuality";
            }

            if (Has(SexualOrientation.Heterosexual))
            {
                sResult += "\n   ";
                sResult += "cross sex relations";
            }
            else if (Has(SexualOrientation.Homosexual))
            {
                sResult += "\n   ";
                sResult += "same sex relations";
            }

            if (Has(MarriageType.Monogamy))
            {
                sResult += "\n   ";
                sResult += "monogamy";
            }
            else if (Has(MarriageType.Polyamory))
            {
                sResult += "\n   ";
                sResult += "wedlock denial";
            }

            if (Has(BodyModifications.Body_Modifications_Blamed))
            {
                sResult += "\n   ";
                sResult += "unmodified body";
            }
            else if (Has(BodyModifications.Body_Modifications_Mandatory))
            {
                sResult += "\n   ";
                sResult += "modified body: ";
                bool bFirst = true;
                foreach (BodyModificationsTypes eMod in m_cMandatoryModifications)
                {
                    if (!bFirst)
                    {
                        sResult += ", ";
                    }
                    sResult += eMod.ToString().Replace('_', ' ').ToLower();
                    bFirst = false;
                }
            }

            if (Has(Clothes.Minimal_Clothes))
            {
                sResult += "\n   ";
                sResult += "common nudity";
            }
            else if (Has(Clothes.Covering_Clothes))
            {
                sResult += "\n   ";
                sResult += "restricted nudity";
            }

            if (Has(Hairstyle.Bald_Heads))
            {
                sResult += "\n   ";
                sResult += "bald heads";
            }
            else if (Has(Hairstyle.Gorgeous_Hairstyles))
            {
                sResult += "\n   ";
                sResult += "long hairs";
            }

            if (Has(Beard.Shaved_Faces))
            {
                sResult += "\n   ";
                sResult += "no beards";
            }
            else if (Has(Beard.Groomed_Beards))
            {
                sResult += "\n   ";
                sResult += "long beards";
            }

            if (Has(Adornments.No_Adornments))
            {
                sResult += "\n   ";
                sResult += "no jewelry";
            }
            else if (Has(Adornments.Lavish_Adornments))
            {
                sResult += "\n   ";
                sResult += "lavish jewelry";
            }

            if (Has(FamilyValues.Praised_Family_Values))
            {
                sResult += "\n   ";
                sResult += "family relations";
            }
            else if (Has(FamilyValues.No_Family_Values))
            {
                sResult += "\n   ";
                sResult += "no family ties";
            }

            if (Has(Science.Technophobia))
            {
                sResult += "\n   ";
                sResult += "traditions";
            }
            else if (Has(Science.Ingenuity))
            {
                sResult += "\n   ";
                sResult += "technical progress";
            }

            if (Has(Magic.Magic_Feared))
            {
                sResult += "\n   ";
                sResult += "magic denial";
            }
            else if (Has(Magic.Magic_Praised))
            {
                sResult += "\n   ";
                sResult += "magic";
            }

            return "Praises: " + sResult + "\n";
        }

        public string GetCustomsDescription()
        {
            string sGenderPriority = "";
            string sResult = "";

            if (Has(GenderPriority.Patriarchy))
            {
                sGenderPriority = "patriarchal";
                //sResult += "males";
            }
            else if (Has(GenderPriority.Matriarchy))
            {
                sGenderPriority = "matriarchal";
                //sResult += "females";
            }
            else if (Has(GenderPriority.Genders_equality))
            {
                sGenderPriority = "gender equality";
                //sResult += "males and females";
            }

            if (Has(MindSet.Emotions))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "are guided mostly by emotions";
            }
            else if (Has(MindSet.Logic))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "are guided mostly by pure logic";
            }

            if (Has(Magic.Magic_Feared))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "denies any form of magic";
            }
            else if (Has(Magic.Magic_Praised))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "accepts any form of magic";
            }

            if (Has(BodyModifications.Body_Modifications_Blamed))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "condemns any tatoo or pierceing";
            }
            else if (Has(BodyModifications.Body_Modifications_Mandatory))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "should have ";
                bool bFirst2 = true;
                foreach (BodyModificationsTypes eMod in m_cMandatoryModifications)
                {
                    if (!bFirst2)
                    {
                        sResult += " and ";
                    }
                    sResult += eMod.ToString().Replace('_', ' ').ToLower();
                    bFirst2 = false;
                }
            }

            if (Has(Clothes.Minimal_Clothes))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "wears minimal clothes";
            }
            else if (Has(Clothes.Covering_Clothes))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "wears clothes hiding entire body";
            }

            if (Has(Hairstyle.Bald_Heads))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "shave their heads";
            }
            else if (Has(Hairstyle.Gorgeous_Hairstyles))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "spends a lot of effort on hair care";
            }

            if (Has(Beard.Shaved_Faces))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "have no beard or moustaches";
            }
            else if (Has(Beard.Groomed_Beards))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "carefully grooms their beards";
            }

            if (Has(Adornments.No_Adornments))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "uses no jewelry";
            }
            else if (Has(Adornments.Lavish_Adornments))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "uses a lot of jewelry";
            }

            if (Has(Science.Technophobia))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "are afraid of new technologies";
            }
            else if (Has(Science.Ingenuity))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "are pretty creative";
            }

            if (Has(FamilyValues.Praised_Family_Values))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "highly appreciates family values";
            }
            else if (Has(FamilyValues.No_Family_Values))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "not paying much attention to family ties";
            }

            if (Has(Sexuality.Puritan))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "uses sex only for reproduction";
            }
            else if (Has(Sexuality.Lecherous))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += ", ";
                sResult += "makes sex a lot";
            }

            if (Has(MarriageType.Monogamy))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += " and ";
                sResult += "have one";

                if (Has(SexualOrientation.Heterosexual))
                {
                    if (Has(GenderPriority.Patriarchy))
                    {
                        sResult += " wife";
                    }
                    else if (Has(GenderPriority.Matriarchy))
                    {
                        sResult += " husband";
                    }
                    else if (Has(GenderPriority.Genders_equality))
                    {
                        sResult += " spouse";
                    }
                }
                else if (Has(SexualOrientation.Homosexual))
                {
                    if (Has(Sexuality.Puritan))
                        sResult += " companion of the same gender";
                    else
                        sResult += " spouse of the same gender";
                }
                else if (Has(SexualOrientation.Bisexual))
                {
                    if (Has(Sexuality.Puritan))
                        sResult += " companion of any gender";
                    else
                        sResult += " spouse (of any gender)";
                }
            }
            else if (Has(MarriageType.Polyamory))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += " and ";
                sResult += "denies wedlock";

                if (Has(Sexuality.Puritan))
                    sResult += ", but may be in platonic relations with persons";
                else
                    sResult += ", but have a number of lovers";

                if (Has(SexualOrientation.Heterosexual))
                {
                    sResult += " of opposite gender";
                }
                else if (Has(SexualOrientation.Homosexual))
                {
                    sResult += " of the same gender";
                }
                else if (Has(SexualOrientation.Bisexual))
                {
                    sResult += " of both genders";
                }
            }
            else if (Has(MarriageType.Polygamy))
            {
                if (!string.IsNullOrEmpty(sResult))
                    sResult += " and ";

                if (Has(SexualOrientation.Heterosexual))
                {
                    if (Has(GenderPriority.Patriarchy))
                    {
                        sResult += "have many wifes";
                    }
                    else if (Has(GenderPriority.Matriarchy))
                    {
                        sResult += "have many husbands";
                    }
                    else if (Has(GenderPriority.Genders_equality))
                    {
                        sResult += "lives in big heterosexual families";
                    }
                }
                else if (Has(SexualOrientation.Homosexual))
                {
                    if (Has(Sexuality.Puritan))
                    {
                        if (Has(GenderPriority.Patriarchy))
                        {
                            sResult += "lives in large brotherhoods";
                        }
                        else if (Has(GenderPriority.Matriarchy))
                        {
                            sResult += "lives in large sisterhoods";
                        }
                        else if (Has(GenderPriority.Genders_equality))
                        {
                            sResult += "lives in large communes with persons of the same gender";
                        }
                    }
                    else
                        sResult += "lives in big homosexual families";
                }
                else if (Has(SexualOrientation.Bisexual))
                {
                    if (Has(Sexuality.Puritan))
                        sResult += "lives in large communes with persons of both genders";
                    else
                        sResult += "lives in big bisexual families";
                }
            }

            return "This is a " + sGenderPriority + " society, which members " + sResult + ".";
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
                if (!pList2.Contains(pItem1))
                    return false;

            foreach (var pItem2 in pList2)
                if (!pList1.Contains(pItem2))
                    return false;

            return true;
        }

        public string GetCustomsDiffString2(Customs pOther)
        {
            string sResult = "";

            if (!HasIdentical<GenderPriority>(pOther))
            {
                if (Has(GenderPriority.Patriarchy))
                    sResult += "proclaims males supremacy";
                else if (Has(GenderPriority.Matriarchy))
                    sResult += "proclaims females supremacy";
                else if (Has(GenderPriority.Genders_equality))
                    sResult += "has no gender prevalence";
            }

            if (!HasIdentical<MindSet>(pOther))
            {
                if (sResult != "")
                    sResult += ", ";

                if (Has(MindSet.Emotions))
                    sResult += "shows a lot of emotions";
                else if (Has(MindSet.Logic))
                    sResult += "supppresses their emotions";
                else if (Has(MindSet.Balanced_mind))
                    sResult += "combines emotions and logic";
            }

            if (!HasIdentical<Magic>(pOther))
            {
                if (sResult != "")
                    sResult += ", ";
                
                if (Has(Magic.Magic_Feared))
                    sResult += "fears all magic";
                else if (Has(Magic.Magic_Praised))
                    sResult += "praises any form of magic";
                else if (Has(Magic.Magic_Allowed))
                    sResult += pOther.Has(Magic.Magic_Feared) ? "has no fear for magic" : "doesn't like magic too much";
            }

            if (!HasIdentical<BodyModifications>(pOther) || !ListsEqual(m_cMandatoryModifications, pOther.m_cMandatoryModifications))
            {
                if (sResult != "")
                    sResult += ", ";

                if (Has(BodyModifications.Body_Modifications_Blamed))
                    sResult += "condemns any tatoo or pierceing";
                else if (Has(BodyModifications.Body_Modifications_Mandatory))
                {
                    List<BodyModificationsTypes> cOthers = new List<BodyModificationsTypes>();
                    if(pOther.Has(BodyModifications.Body_Modifications_Mandatory))
                        cOthers.AddRange(pOther.m_cMandatoryModifications);

                    bool bFirst2 = true;
                    foreach (BodyModificationsTypes eMod in m_cMandatoryModifications)
                    {
                        if(cOthers.Contains(eMod))
                            continue;
                        if (bFirst2)
                            sResult += "should have ";
                        else
                            sResult += " and ";

                        sResult += eMod.ToString().Replace('_', ' ').ToLower();
                        bFirst2 = false;
                    }

                    bool bFirst3 = true;
                    foreach (BodyModificationsTypes eMod in cOthers)
                    {
                        if(m_cMandatoryModifications.Contains(eMod))
                            continue;
                        if (bFirst3)
                        {
                            if (bFirst2)
                                sResult += "should not have ";
                            else
                                sResult += ", but should not have ";
                        }
                        else
                        { 
                        }

                        sResult += eMod.ToString().Replace('_', ' ').ToLower();
                        bFirst3 = false;
                    }
                }
                else if (Has(BodyModifications.Body_Modifications_Allowed))
                    sResult += pOther.Has(BodyModifications.Body_Modifications_Blamed) ? "allowed to have some tatoo or pierceing" : "could have tatoo or pierceing on their choice";
            }

            if (!HasIdentical<Clothes>(pOther))
            {
                if (sResult != "")
                    sResult += ", ";

                if (Has(Clothes.Covering_Clothes))
                    sResult += "hides their entire bodies by clothes";
                else if (Has(Clothes.Minimal_Clothes))
                    sResult += "wears only minimal clothes";
                else if (Has(Clothes.Revealing_Clothes))
                    sResult += pOther.Has(Clothes.Covering_Clothes) ? "wears revealing clothes" : "wears covering clothes";
            }

            if (!HasIdentical<Hairstyle>(pOther))
            {
                if (sResult != "")
                    sResult += ", ";

                if (Has(Hairstyle.Bald_Heads))
                    sResult += "shaves their heads completely bald";
                else if (Has(Hairstyle.Gorgeous_Hairstyles))
                    sResult += "makes complex hairsyles";
                else if (Has(Hairstyle.Practical_Hairsyles))
                    sResult += pOther.Has(Hairstyle.Bald_Heads) ? "not shaves their heads" : "not spends enough efforts on their hairs";
            }

            if (!HasIdentical<Beard>(pOther))
            {
                if (sResult != "")
                    sResult += ", ";

                if (Has(Beard.Shaved_Faces))
                    sResult += "has nor beard, neither moustaches";
                else if (Has(Beard.Groomed_Beards))
                    sResult += "has perfectly groomed beards";
                else if (Has(Beard.Common_Beards))
                    sResult += pOther.Has(Beard.Shaved_Faces) ? "has hairs on their faces" : "not grooming their beards properly";
            }

            if (!HasIdentical<Adornments>(pOther))
            {
                if (sResult != "")
                    sResult += ", ";

                if (Has(Adornments.No_Adornments))
                    sResult += "uses no jevelry";
                else if (Has(Adornments.Lavish_Adornments))
                    sResult += "uses a lot of jevelry";
                else if (Has(Adornments.Some_Adornments))
                    sResult += pOther.Has(Adornments.No_Adornments) ? "could use some jevelry" : "uses not so much jevelry";
            }

            if (!HasIdentical<Science>(pOther))
            {
                if (sResult != "")
                    sResult += ", ";

                if (Has(Science.Technophobia))
                    sResult += "rejects any novelties";
                else if (Has(Science.Ingenuity))
                    sResult += "likes any novelties";
                else if (Has(Science.Moderate_Science))
                    sResult += pOther.Has(Science.Technophobia) ? "have no fear of novelties" : "doesn't like novelties so much";
            }

            if (!HasIdentical<FamilyValues>(pOther))
            {
                if (sResult != "")
                    sResult += ", ";

                if (Has(FamilyValues.Praised_Family_Values))
                    sResult += "are very bound to family";
                else if (Has(FamilyValues.No_Family_Values))
                    sResult += "not bound to family at all";
                else if (Has(FamilyValues.Moderate_Family_Values))
                    sResult += "not so bound to family";
            }

            if (!HasIdentical<Sexuality>(pOther))
            {
                if (sResult != "")
                    sResult += ", ";

                if (Has(Sexuality.Puritan))
                    sResult += "not interested in sex, except for reproduction";
                else if (Has(Sexuality.Lecherous))
                    sResult += "has insatiable libido";
                else if (Has(Sexuality.Moderate_sexuality))
                    sResult += pOther.Has(Sexuality.Puritan) ? "likes to have sex for fun and pleasure" : "less interested in sex";
            }

            if (!HasIdentical<SexualOrientation>(pOther))
            {
                if (sResult != "")
                    sResult += ", ";

                if (Has(SexualOrientation.Heterosexual))
                    sResult += "prefers heterosexual relations";
                else if (Has(SexualOrientation.Homosexual))
                    sResult += "prefers homosexual relations";
                else if (Has(SexualOrientation.Bisexual))
                    sResult += pOther.Has(SexualOrientation.Heterosexual) ? "allows homosexual relations" : "allows heterosexual relations";
            }

            if (!HasIdentical<MarriageType>(pOther))
            {
                if (sResult != "")
                    sResult += " and ";

                if (Has(MarriageType.Monogamy))
                {
                    sResult += pOther.Has(MarriageType.Polygamy) ? "could have only one" : "could have one";

                    if (Has(SexualOrientation.Heterosexual))
                    {
                        if (Has(GenderPriority.Patriarchy))
                        {
                            sResult += " wife";
                        }
                        else if (Has(GenderPriority.Matriarchy))
                        {
                            sResult += " husband";
                        }
                        else if (Has(GenderPriority.Genders_equality))
                        {
                            sResult += " spouse of opposite gender";
                        }
                    }
                    else if (Has(SexualOrientation.Homosexual))
                    {
                        if (Has(Sexuality.Puritan))
                            sResult += " companion of the same gender";
                        else
                            sResult += " spouse of the same gender";
                    }
                    else if (Has(SexualOrientation.Bisexual))
                    {
                        if (Has(Sexuality.Puritan))
                            sResult += " companion of any gender";
                        else
                            sResult += " spouse of any gender";
                    }
                }
                else if (Has(MarriageType.Polyamory))
                {
                    sResult += "rejects marriage";

                    if (Has(Sexuality.Puritan))
                        sResult += ", but may be in platonic relations with persons";
                    else
                        sResult += ", but could have a number of lovers";

                    if (Has(SexualOrientation.Heterosexual))
                    {
                        sResult += " of opposite gender";
                    }
                    else if (Has(SexualOrientation.Homosexual))
                    {
                        sResult += " of the same gender";
                    }
                    else if (Has(SexualOrientation.Bisexual))
                    {
                        sResult += " of both genders";
                    }
                }
                else if (Has(MarriageType.Polygamy))
                {
                    if (Has(SexualOrientation.Heterosexual))
                    {
                        if (Has(GenderPriority.Patriarchy))
                        {
                            sResult += "could have many wifes";
                        }
                        else if (Has(GenderPriority.Matriarchy))
                        {
                            sResult += "could have many husbands";
                        }
                        else if (Has(GenderPriority.Genders_equality))
                        {
                            sResult += "lives in big heterosexual families";
                        }
                    }
                    else if (Has(SexualOrientation.Homosexual))
                    {
                        if (Has(Sexuality.Puritan))
                        {
                            if (Has(GenderPriority.Patriarchy))
                            {
                                sResult += "lives in large brotherhoods";
                            }
                            else if (Has(GenderPriority.Matriarchy))
                            {
                                sResult += "lives in large sisterhoods";
                            }
                            else if (Has(GenderPriority.Genders_equality))
                            {
                                sResult += "lives in large communes with persons of the same gender";
                            }
                        }
                        else
                            sResult += "lives in big homosexual families";
                    }
                    else if (Has(SexualOrientation.Bisexual))
                    {
                        if (Has(Sexuality.Puritan))
                            sResult += "lives in large communes with persons of both genders";
                        else
                            sResult += "lives in big bisexual families";
                    }
                }
            }

            //if (bFirst && m_eGenderPriority == pOther.m_eGenderPriority)
            //    sResult = "";

            return sResult;
        }

        /// <summary>
        /// Вычисляет враждебность друг сообществ на основании их обычаев.
        /// Возвращает значение от 22 (полные противоположности) до -11 (полное совпадение).
        /// </summary>
        public int GetDifference(Customs pOpponent)
        {
            string s1 = "", s2 = "";
            return GetDifference(pOpponent, ref s1, ref s2);
        }

        private void CheckDifference<T>(Customs pOpponent, T middleValue, ref int iHostility, ref string sPositiveReasons, ref string sNegativeReasons) where T : Enum, IComparable
        {
            if (HasIdentical<T>(pOpponent))
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.ValueOf<T>().ToString().Replace('_', ' ') + "\n";
            }
            else
            {
                if (!Has(middleValue) && !pOpponent.Has(middleValue))
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.ValueOf<T>().ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.ValueOf<T>().ToString().Replace('_', ' ') + "\n";
                }
            }
        }

        /// <summary>
        /// Вычисляет враждебность друг сообществ на основании их обычаев.
        /// Возвращает значение от 22 (полные противоположности) до -11 (полное совпадение).
        /// </summary>
        /// <param name="pOpponent">обычаи другого сообщества</param>
        /// <param name="sReasons">мотивация враждебности</param>
        /// <returns></returns>
        public int GetDifference(Customs pOpponent, ref string sPositiveReasons, ref string sNegativeReasons)
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
                if (!Has(BodyModifications.Body_Modifications_Mandatory) || ListsEqual(m_cMandatoryModifications, pOpponent.m_cMandatoryModifications))
                {
                    iHostility--;
                    sPositiveReasons += " (+1) " + pOpponent.ValueOf<BodyModifications>().ToString().Replace('_', ' ') + "\n";
                }
            }
            else
            {
                if (!Has(BodyModifications.Body_Modifications_Allowed) &&
                    !pOpponent.Has(BodyModifications.Body_Modifications_Allowed))
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.ValueOf<BodyModifications>().ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.ValueOf<BodyModifications>().ToString().Replace('_', ' ') + "\n";
                }
            }

            return iHostility;
        }

        public override bool Equals(object obj)
        {
            Customs pOther = obj as Customs;
            if (pOther == null)
                return false;

            if (m_cCustoms.Count != pOther.m_cCustoms.Count)
                return false;

            foreach (var custom in m_cCustoms)
            {
                if (!pOther.Has(custom.Value))
                    return false;
            }

            if (Has(BodyModifications.Body_Modifications_Mandatory) && 
                !ListsEqual(m_cMandatoryModifications, pOther.m_cMandatoryModifications))
                return false;

            return true;
        }
    }
}
