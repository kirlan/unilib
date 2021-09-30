using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneLab;
using GeneLab.Genetix;
using Random;

namespace Socium.Psichology
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

        public enum Progressiveness
        {
            Traditionalism,
            Moderate_Science,
            Technofetishism
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

        public enum Adornments
        {
            No_Adornments,
            Some_Adornments,
            Lavish_Adornments
        }

        public GenderPriority m_eGenderPriority = GenderPriority.Genders_equality;

        public MindSet m_eMindSet = MindSet.Balanced_mind;

        public Sexuality m_eSexuality = Sexuality.Moderate_sexuality;

        public SexualOrientation m_eSexRelations = SexualOrientation.Heterosexual;

        public MarriageType m_eMarriage = MarriageType.Monogamy;

        public BodyModifications m_eBodyModifications = BodyModifications.Body_Modifications_Allowed;

        public List<BodyModificationsTypes> m_cMandatoryModifications = new List<BodyModificationsTypes>();

        public FamilyValues m_eFamilyValues = FamilyValues.Moderate_Family_Values;

        public Progressiveness m_eProgress = Progressiveness.Moderate_Science;

        public Magic m_eMagic = Magic.Magic_Allowed;

        public Clothes m_eClothes = Clothes.Revealing_Clothes;

        public Adornments m_eAdornments = Adornments.Some_Adornments;

        public Customs()
        {
            m_eGenderPriority = Rnd.OneChanceFrom(5) ? GenderPriority.Genders_equality : Rnd.OneChanceFrom(2) ? GenderPriority.Matriarchy : GenderPriority.Patriarchy;

            m_eMindSet = !Rnd.OneChanceFrom(3) ? MindSet.Balanced_mind : Rnd.OneChanceFrom(2) ? MindSet.Logic : MindSet.Emotions;

            m_eSexuality = !Rnd.OneChanceFrom(3) ? Sexuality.Moderate_sexuality : Rnd.OneChanceFrom(5) ? Sexuality.Puritan : Sexuality.Lecherous;

            if (m_eGenderPriority == GenderPriority.Matriarchy)
                m_eSexRelations = Rnd.OneChanceFrom(3) ? SexualOrientation.Bisexual : Rnd.OneChanceFrom(2) ? SexualOrientation.Homosexual : SexualOrientation.Heterosexual;
            else
                m_eSexRelations = Rnd.OneChanceFrom(5) ? SexualOrientation.Bisexual : Rnd.OneChanceFrom(9) ? SexualOrientation.Homosexual : SexualOrientation.Heterosexual;

            m_eMarriage = Rnd.OneChanceFrom(3) ? MarriageType.Polygamy : Rnd.OneChanceFrom(2) ? MarriageType.Polyamory : MarriageType.Monogamy;

            m_eBodyModifications = Rnd.OneChanceFrom(5) ? BodyModifications.Body_Modifications_Allowed : Rnd.OneChanceFrom(3) ? BodyModifications.Body_Modifications_Mandatory : BodyModifications.Body_Modifications_Blamed;

            if (m_eBodyModifications == BodyModifications.Body_Modifications_Mandatory)
                m_cMandatoryModifications.Add((BodyModificationsTypes)Rnd.Get(typeof(BodyModificationsTypes)));

            m_eClothes = Rnd.OneChanceFrom(3) ? Clothes.Revealing_Clothes : Rnd.OneChanceFrom(3) ? Clothes.Minimal_Clothes : Clothes.Covering_Clothes;

            m_eAdornments = Rnd.OneChanceFrom(3) ? Adornments.Some_Adornments : Rnd.OneChanceFrom(3) ? Adornments.No_Adornments : Adornments.Lavish_Adornments;

            m_eFamilyValues = Rnd.OneChanceFrom(2) ? FamilyValues.Moderate_Family_Values : Rnd.OneChanceFrom(2) ? FamilyValues.Praised_Family_Values : FamilyValues.No_Family_Values;

            m_eProgress = Rnd.OneChanceFrom(2) ? Progressiveness.Moderate_Science : Rnd.OneChanceFrom(2) ? Progressiveness.Technofetishism : Progressiveness.Traditionalism;

            m_eMagic = Rnd.OneChanceFrom(2) ? Magic.Magic_Allowed : Rnd.OneChanceFrom(2) ? Magic.Magic_Praised : Magic.Magic_Feared;
        }

        // Находит все отличия между pBaseSample и pDifferencesSample и затем накладывает их на pBase
        public static Customs ApplyDifferences(Customs pBase, Customs pBaseSample, Customs pDifferencesSample)
        {
            Customs pNew = new Customs(pBase, Mutation.None);

            if (pDifferencesSample.m_eGenderPriority != pBaseSample.m_eGenderPriority)
                pNew.m_eGenderPriority = pDifferencesSample.m_eGenderPriority;

            if (pDifferencesSample.m_eMindSet != pBaseSample.m_eMindSet)
                pNew.m_eMindSet = pDifferencesSample.m_eMindSet;

            if (pDifferencesSample.m_eSexuality != pBaseSample.m_eSexuality)
                pNew.m_eSexuality = pDifferencesSample.m_eSexuality;

            if (pDifferencesSample.m_eSexRelations != pBaseSample.m_eSexRelations)
                pNew.m_eSexRelations = pDifferencesSample.m_eSexRelations;

            if (pDifferencesSample.m_eMarriage != pBaseSample.m_eMarriage)
                pNew.m_eMarriage = pDifferencesSample.m_eMarriage;

            if (pDifferencesSample.m_eBodyModifications != pBaseSample.m_eBodyModifications)
                pNew.m_eBodyModifications = pDifferencesSample.m_eBodyModifications;

            if (pDifferencesSample.m_eClothes != pBaseSample.m_eClothes)
                pNew.m_eClothes = pDifferencesSample.m_eClothes;

            if (pDifferencesSample.m_eAdornments != pBaseSample.m_eAdornments)
                pNew.m_eAdornments = pDifferencesSample.m_eAdornments;

            if (pDifferencesSample.m_eFamilyValues != pBaseSample.m_eFamilyValues)
                pNew.m_eFamilyValues = pDifferencesSample.m_eFamilyValues;

            if (pDifferencesSample.m_eProgress != pBaseSample.m_eProgress)
                pNew.m_eProgress = pDifferencesSample.m_eProgress;

            if (pDifferencesSample.m_eMagic != pBaseSample.m_eMagic)
                pNew.m_eMagic = pDifferencesSample.m_eMagic;

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

        public Customs(Customs pAncestorCustoms, Mutation eDifference)
        {
            m_eGenderPriority = pAncestorCustoms.m_eGenderPriority;
            m_eMindSet = pAncestorCustoms.m_eMindSet;
            m_eSexuality = pAncestorCustoms.m_eSexuality;
            m_eSexRelations = pAncestorCustoms.m_eSexRelations;
            m_eMarriage = pAncestorCustoms.m_eMarriage;
            m_eBodyModifications = pAncestorCustoms.m_eBodyModifications;
            m_eClothes = pAncestorCustoms.m_eClothes;
            m_eAdornments = pAncestorCustoms.m_eAdornments;
            m_eFamilyValues = pAncestorCustoms.m_eFamilyValues;
            m_eProgress = pAncestorCustoms.m_eProgress;
            m_eMagic = pAncestorCustoms.m_eMagic;

            m_cMandatoryModifications.AddRange(pAncestorCustoms.m_cMandatoryModifications);

            if (eDifference == Mutation.None)
                return;

            int iChoice = Rnd.Get(eDifference == Mutation.Mandatory ? 11:22);
            switch (iChoice)
            {
                case 0:
                    if (m_eGenderPriority == GenderPriority.Genders_equality)
                        m_eGenderPriority = Rnd.OneChanceFrom(2) ? GenderPriority.Matriarchy : GenderPriority.Patriarchy;
                    else
                    {
                        m_eGenderPriority = Rnd.OneChanceFrom(2) ? (m_eGenderPriority == GenderPriority.Matriarchy ? GenderPriority.Patriarchy : GenderPriority.Matriarchy) : GenderPriority.Genders_equality;
                    }
                    break;
                case 1:
                    if (m_eMindSet == MindSet.Balanced_mind)
                        m_eMindSet = Rnd.OneChanceFrom(2) ? MindSet.Logic : MindSet.Emotions;
                    else
                        m_eMindSet = MindSet.Balanced_mind;
                    break;
                case 2:
                    if (m_eSexuality == Sexuality.Moderate_sexuality)
                        m_eSexuality = Rnd.OneChanceFrom(5) ? Sexuality.Puritan : Sexuality.Lecherous;
                    else
                        m_eSexuality = Sexuality.Moderate_sexuality;
                    break;
                case 3:
                    if (m_eSexRelations == SexualOrientation.Bisexual)
                    {
                        if (m_eGenderPriority == GenderPriority.Matriarchy)
                            m_eSexRelations = Rnd.OneChanceFrom(2) ? SexualOrientation.Homosexual : SexualOrientation.Heterosexual;
                        else
                            m_eSexRelations = Rnd.OneChanceFrom(9) ? SexualOrientation.Homosexual : SexualOrientation.Heterosexual;
                    }
                    else
                    {
                        if (m_eGenderPriority == GenderPriority.Matriarchy)
                            m_eSexRelations = Rnd.OneChanceFrom(2) ? (m_eSexRelations == SexualOrientation.Homosexual ? SexualOrientation.Heterosexual : SexualOrientation.Homosexual) : SexualOrientation.Bisexual;
                        else
                            m_eSexRelations = Rnd.OneChanceFrom(9) ? (m_eSexRelations == SexualOrientation.Homosexual ? SexualOrientation.Heterosexual : SexualOrientation.Homosexual) : SexualOrientation.Bisexual;
                    }
                    break;
                case 4:
                    if (m_eMarriage == MarriageType.Polygamy)
                        m_eMarriage = Rnd.OneChanceFrom(5) ? MarriageType.Polyamory : MarriageType.Monogamy;
                    else
                        m_eMarriage = MarriageType.Polygamy;
                    break;
                case 5:
                    if (m_eBodyModifications == BodyModifications.Body_Modifications_Allowed)
                        m_eBodyModifications = Rnd.OneChanceFrom(5) ? BodyModifications.Body_Modifications_Mandatory : BodyModifications.Body_Modifications_Blamed;
                    else if (m_eBodyModifications == BodyModifications.Body_Modifications_Blamed || Rnd.OneChanceFrom(2))
                        m_eBodyModifications = BodyModifications.Body_Modifications_Allowed;

                    if (m_eBodyModifications == BodyModifications.Body_Modifications_Mandatory)
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
                    break;
                case 6:
                    if (m_eFamilyValues == FamilyValues.Moderate_Family_Values)
                        m_eFamilyValues = Rnd.OneChanceFrom(2) ? FamilyValues.Praised_Family_Values : FamilyValues.No_Family_Values;
                    else
                        m_eFamilyValues = FamilyValues.Moderate_Family_Values;
                    break;
                case 7:
                    if (m_eProgress == Progressiveness.Moderate_Science)
                        m_eProgress = Rnd.OneChanceFrom(3) ? Progressiveness.Technofetishism : Progressiveness.Traditionalism;
                    else
                        m_eProgress = Progressiveness.Moderate_Science;
                    break;
                case 8:
                    if (m_eMagic == Magic.Magic_Allowed)
                        m_eMagic = Rnd.OneChanceFrom(3) ? Magic.Magic_Praised : Magic.Magic_Feared;
                    else
                        m_eMagic = Magic.Magic_Allowed;
                    break;
                case 9:
                    if (m_eClothes == Clothes.Revealing_Clothes)
                        m_eClothes = Rnd.OneChanceFrom(3) ? Clothes.Minimal_Clothes : Clothes.Covering_Clothes;
                    else
                        m_eClothes = Clothes.Revealing_Clothes;
                    break;
                case 10:
                    if (m_eAdornments == Adornments.Some_Adornments)
                        m_eAdornments = Rnd.OneChanceFrom(3) ? Adornments.No_Adornments : Adornments.Lavish_Adornments;
                    else
                        m_eAdornments = Adornments.Some_Adornments;
                    break;
            }

            if (eDifference == Mutation.Mandatory && this.Equals(pAncestorCustoms))
                    throw new Exception("Customs not changed!");
        }

        public void FixBodyModifications(Fenotype pFenotype)
        {
            if (pFenotype.m_pEars.m_eEarsType == EarsType.None)
            {
                if (m_cMandatoryModifications.Contains(BodyModificationsTypes.Small_Ear_Rings))
                    m_cMandatoryModifications.Remove(BodyModificationsTypes.Small_Ear_Rings);
                if (m_cMandatoryModifications.Contains(BodyModificationsTypes.Huge_Ear_Rings))
                    m_cMandatoryModifications.Remove(BodyModificationsTypes.Huge_Ear_Rings);
            }
            if (pFenotype.m_pFace.m_eNoseType == NoseType.None)
            {
                if (m_cMandatoryModifications.Contains(BodyModificationsTypes.Nose_Ring))
                    m_cMandatoryModifications.Remove(BodyModificationsTypes.Nose_Ring);
            }
        }

        //public void Evolve()
        //{
        //    int iChoice = Rnd.Get(5);

        //    switch (iChoice)
        //    {
        //        case 0:
        //            m_eGenderPriority = GenderPriority.Genders_equality;
        //            break;
        //        case 1:
        //            m_eMindBodyPriority = MindBodyPriority.Balanced_body_and_mind;
        //            break;
        //        case 2:
        //            m_eSexuality = Sexuality.Moderate_sexuality;
        //            break;
        //        case 3:
        //            m_eSexRelations = SexualOrientation.Bisexual;
        //            break;
        //        case 4:
        //        //    m_eReligion = Religion.Agnosticism;
        //        //    break;
        //        //case 5:
        //            m_eFamilySize = FamilySize.Polygamy;
        //            break;
        //    }
        //}

        //public void Degrade()
        //{
        //    int iChoice = Rnd.Get(5);

        //    switch (iChoice)
        //    {
        //        case 0:
        //            m_eGenderPriority = Rnd.OneChanceFrom(3) ? GenderPriority.Matriarchy : GenderPriority.Patriarchy;
        //            break;
        //        case 1:
        //            m_eMindBodyPriority = Rnd.OneChanceFrom(3) ? MindBodyPriority.Thinkers : MindBodyPriority.Brutes;
        //            break;
        //        case 2:
        //            m_eSexuality = Rnd.OneChanceFrom(3) ? Sexuality.Puritan : Sexuality.Lecherous;
        //            break;
        //        case 3:
        //            m_eSexRelations = Rnd.OneChanceFrom(3) ? SexualOrientation.Homosexual : SexualOrientation.Heterosexual;
        //            break;
        //        case 4:
        //        //    m_eReligion = Rnd.OneChanceFrom(3) ? Religion.Atheism : Religion.Piety;
        //        //    break;
        //        //case 5:
        //            m_eFamilySize = Rnd.OneChanceFrom(3) ? FamilySize.Polyamory : FamilySize.Monogamy;
        //            break;
        //    }
        //}

        public string GetCustomsString()
        {
            string sResult = "";

            if (m_eGenderPriority == GenderPriority.Patriarchy)
            {
                sResult += "\n   ";
                sResult += "manhood";
            }
            if (m_eGenderPriority == GenderPriority.Matriarchy)
            {
                sResult += "\n   ";
                sResult += "womanhood";
            }

            if (m_eMindSet == MindSet.Emotions)
            {
                sResult += "\n   ";
                sResult += "emotions";
            }
            if (m_eMindSet == MindSet.Logic)
            {
                sResult += "\n   ";
                sResult += "pure logic";
            }

            if (m_eSexuality == Sexuality.Puritan)
            {
                sResult += "\n   ";
                sResult += "chastity";
            }
            if (m_eSexuality == Sexuality.Lecherous)
            {
                sResult += "\n   ";
                sResult += "unlimited sexuality";
            }

            if (m_eSexRelations == SexualOrientation.Heterosexual)
            {
                sResult += "\n   ";
                sResult += "cross sex relations";
            }
            if (m_eSexRelations == SexualOrientation.Homosexual)
            {
                sResult += "\n   ";
                sResult += "same sex relations";
            }

            if (m_eMarriage == MarriageType.Monogamy)
            {
                sResult += "\n   ";
                sResult += "monogamy";
            }
            if (m_eMarriage == MarriageType.Polyamory)
            {
                sResult += "\n   ";
                sResult += "wedlock denial";
            }

            if (m_eBodyModifications == BodyModifications.Body_Modifications_Blamed)
            {
                sResult += "\n   ";
                sResult += "unmodified body";
            }
            if (m_eBodyModifications == BodyModifications.Body_Modifications_Mandatory)
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

            if (m_eClothes == Clothes.Minimal_Clothes)
            {
                sResult += "\n   ";
                sResult += "common nudity";
            }
            if (m_eClothes == Clothes.Covering_Clothes)
            {
                sResult += "\n   ";
                sResult += "restricted nudity";
            }

            if (m_eAdornments == Adornments.No_Adornments)
            {
                sResult += "\n   ";
                sResult += "no jewelry";
            }
            if (m_eAdornments == Adornments.Lavish_Adornments)
            {
                sResult += "\n   ";
                sResult += "lavish jewelry";
            }

            if (m_eFamilyValues == FamilyValues.Praised_Family_Values)
            {
                sResult += "\n   ";
                sResult += "family relations";
            }
            if (m_eFamilyValues == FamilyValues.No_Family_Values)
            {
                sResult += "\n   ";
                sResult += "no family ties";
            }

            if (m_eProgress == Progressiveness.Traditionalism)
            {
                sResult += "\n   ";
                sResult += "traditions";
            }
            if (m_eProgress == Progressiveness.Technofetishism)
            {
                sResult += "\n   ";
                sResult += "technical progress";
            }

            if (m_eMagic == Magic.Magic_Feared)
            {
                sResult += "\n   ";
                sResult += "magic denial";
            }
            if (m_eMagic == Magic.Magic_Praised)
            {
                sResult += "\n   ";
                sResult += "magic";
            }

            return "Praises: " + sResult + "\n";
        }

        public string GetCustomsString2()
        {
            string sResult = "";

            if (m_eGenderPriority == GenderPriority.Patriarchy)
            {
                sResult += "males";
            }
            if (m_eGenderPriority == GenderPriority.Matriarchy)
            {
                sResult += "females";
            }
            if (m_eGenderPriority == GenderPriority.Genders_equality)
            {
                sResult += "males and females";
            }

            bool bFirst = true;

            if (m_eMindSet == MindSet.Emotions)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "are guided mostly by emotions";
            }
            if (m_eMindSet == MindSet.Logic)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "are guided mostly by pure logic";
            }

            if (m_eMagic == Magic.Magic_Feared)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "denies any form of magic";
            }
            if (m_eMagic == Magic.Magic_Praised)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "accepts any form of magic";
            }

            if (m_eBodyModifications == BodyModifications.Body_Modifications_Blamed)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "has no tatoo or pierceing";
            }
            if (m_eBodyModifications == BodyModifications.Body_Modifications_Mandatory)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "has ";
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

            if (m_eClothes == Clothes.Minimal_Clothes)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "prefers to wear minimal clothes";
            }
            if (m_eClothes == Clothes.Covering_Clothes)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "wears clothes hiding entire body";
            }

            if (m_eAdornments == Adornments.No_Adornments)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "uses no jewelry";
            }
            if (m_eAdornments == Adornments.Lavish_Adornments)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "uses a lot of jewelry";
            }

            if (m_eProgress == Progressiveness.Traditionalism)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "doesn't like novelties";
            }
            if (m_eProgress == Progressiveness.Technofetishism)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "likes any novelties";
            }

            if (m_eFamilyValues == FamilyValues.Praised_Family_Values)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "highly appreciates family values";
            }
            if (m_eFamilyValues == FamilyValues.No_Family_Values)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "not paying much attention to family ties";
            }

            if (m_eSexuality == Sexuality.Puritan)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "uses sex only for reproduction";
            }
            if (m_eSexuality == Sexuality.Lecherous)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "makes sex a lot";
            }

            if (m_eMarriage == MarriageType.Monogamy)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += " and ";
                sResult += "have one";

                if (m_eSexRelations == SexualOrientation.Heterosexual)
                {
                    if (m_eGenderPriority == GenderPriority.Patriarchy)
                    {
                        sResult += " wife";
                    }
                    if (m_eGenderPriority == GenderPriority.Matriarchy)
                    {
                        sResult += " husband";
                    }
                    if (m_eGenderPriority == GenderPriority.Genders_equality)
                    {
                        sResult += " spouse";
                    }
                }
                if (m_eSexRelations == SexualOrientation.Homosexual)
                {
                    if (m_eSexuality == Sexuality.Puritan)
                        sResult += " companion of the same gender";
                    else
                        sResult += " spouse of the same gender";
                }
                if (m_eSexRelations == SexualOrientation.Bisexual)
                {
                    if (m_eSexuality == Sexuality.Puritan)
                        sResult += " companion of any gender";
                    else
                        sResult += " spouse (of any gender)";
                }
            }
            if (m_eMarriage == MarriageType.Polyamory)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += " and ";
                sResult += "denies wedlock";

                if (m_eSexuality == Sexuality.Puritan)
                    sResult += ", but may be in platonic relations with persons";
                else
                    sResult += ", but have a number of lovers";

                if (m_eSexRelations == SexualOrientation.Heterosexual)
                {
                    sResult += " of opposite gender";
                }
                if (m_eSexRelations == SexualOrientation.Homosexual)
                {
                    sResult += " of the same gender";
                }
                if (m_eSexRelations == SexualOrientation.Bisexual)
                {
                    sResult += " of both genders";
                }
            }
            if (m_eMarriage == MarriageType.Polygamy)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += " and ";

                if (m_eSexRelations == SexualOrientation.Heterosexual)
                {
                    if (m_eSexRelations == SexualOrientation.Heterosexual)
                    {
                        if (m_eGenderPriority == GenderPriority.Patriarchy)
                        {
                            sResult += "have many wifes";
                        }
                        if (m_eGenderPriority == GenderPriority.Matriarchy)
                        {
                            sResult += "have many husbands";
                        }
                        if (m_eGenderPriority == GenderPriority.Genders_equality)
                        {
                            sResult += "lives in big heterosexual families";
                        }
                    }
                }
                if (m_eSexRelations == SexualOrientation.Homosexual)
                {
                    if (m_eSexuality == Sexuality.Puritan)
                    {
                        if (m_eGenderPriority == GenderPriority.Patriarchy)
                        {
                            sResult += "lives in large brotherhoods";
                        }
                        if (m_eGenderPriority == GenderPriority.Matriarchy)
                        {
                            sResult += "lives in large sisterhoods";
                        }
                        if (m_eGenderPriority == GenderPriority.Genders_equality)
                        {
                            sResult += "lives in large communes with persons of the same gender";
                        }
                    }
                    else
                        sResult += "lives in big homosexual families";
                }
                if (m_eSexRelations == SexualOrientation.Bisexual)
                {
                    if (m_eSexuality == Sexuality.Puritan)
                        sResult += "lives in large communes with persons of both genders";
                    else
                        sResult += "lives in big bisexual families";
                }
            }

            return "The most respected members of society are " + sResult + ".\n";
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

            if (m_eGenderPriority != pOther.m_eGenderPriority)
            {
                if (m_eGenderPriority == GenderPriority.Patriarchy)
                    sResult += "proclaims males supremacy";
                if (m_eGenderPriority == GenderPriority.Matriarchy)
                    sResult += "proclaims females supremacy";
                if (m_eGenderPriority == GenderPriority.Genders_equality)
                    sResult += "has no gender prevalence";
            }

            if (m_eMindSet != pOther.m_eMindSet)
            {
                if (sResult != "")
                    sResult += ", ";

                if (m_eMindSet == MindSet.Emotions)
                    sResult += "shows a lot of emotions";
                if (m_eMindSet == MindSet.Logic)
                    sResult += "supppresses their emotions";
                if (m_eMindSet == MindSet.Balanced_mind)
                    sResult += "combines emotions and logic";
            }

            if (m_eMagic != pOther.m_eMagic)
            {
                if (sResult != "")
                    sResult += ", ";
                
                if (m_eMagic == Magic.Magic_Feared)
                    sResult += "rejects any form of magic";
                if (m_eMagic == Magic.Magic_Praised)
                    sResult += "praises any form of magic";
                if (m_eMagic == Magic.Magic_Allowed)
                    sResult += pOther.m_eMagic == Magic.Magic_Feared ? "has no fear for magic" : "doesn't like magic too much";
            }

            if (m_eBodyModifications != pOther.m_eBodyModifications || !ListsEqual(m_cMandatoryModifications, pOther.m_cMandatoryModifications))
            {
                if (sResult != "")
                    sResult += ", ";

                if (m_eBodyModifications == BodyModifications.Body_Modifications_Blamed)
                    sResult += "condemns any form of body modification";
                if (m_eBodyModifications == BodyModifications.Body_Modifications_Mandatory)
                {
                    List<BodyModificationsTypes> cOthers = new List<BodyModificationsTypes>();
                    if(pOther.m_eBodyModifications == BodyModifications.Body_Modifications_Mandatory)
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
                                sResult += ", but should not have ";
                            else
                                sResult += "should not have ";
                        }
                        else
                        { 
                        }

                        sResult += eMod.ToString().Replace('_', ' ').ToLower();
                        bFirst2 = false;
                    }
                }
                if (m_eBodyModifications == BodyModifications.Body_Modifications_Allowed)
                    sResult += pOther.m_eBodyModifications == BodyModifications.Body_Modifications_Blamed ? "could have some tatoo or pierceing" : "could have tatoo or pierceing on their choice";
            }

            if (m_eClothes != pOther.m_eClothes)
            {
                if (sResult != "")
                    sResult += ", ";

                if (m_eClothes == Clothes.Covering_Clothes)
                    sResult += "hides their entire bodies by clothes";
                if (m_eClothes == Clothes.Minimal_Clothes)
                    sResult += "wears only minimum necessary clothes";
                if (m_eClothes == Clothes.Revealing_Clothes)
                    sResult += "wears anything they want";
            }

            if (m_eAdornments != pOther.m_eAdornments)
            {
                if (sResult != "")
                    sResult += ", ";

                if (m_eAdornments == Adornments.No_Adornments)
                    sResult += "wears no adornments";
                if (m_eAdornments == Adornments.Lavish_Adornments)
                    sResult += "wears a lot of adornments";
                if (m_eAdornments == Adornments.Some_Adornments)
                    sResult += pOther.m_eAdornments == Adornments.No_Adornments ? "could wear some adornments" : "wears not so much adorments";
            }

            if (m_eProgress != pOther.m_eProgress)
            {
                if (sResult != "")
                    sResult += ", ";

                if (m_eProgress == Progressiveness.Traditionalism)
                    sResult += "rejects any novelties";
                if (m_eProgress == Progressiveness.Technofetishism)
                    sResult += "likes any novelties";
                if (m_eProgress == Progressiveness.Moderate_Science)
                    sResult += pOther.m_eProgress == Progressiveness.Traditionalism ? "accepts some regulated progress" : "doesn't like novelties so much";
            }

            if (m_eFamilyValues != pOther.m_eFamilyValues)
            {
                if (sResult != "")
                    sResult += ", ";

                if (m_eFamilyValues == FamilyValues.Praised_Family_Values)
                    sResult += "have very tight family bonds";
                if (m_eFamilyValues == FamilyValues.No_Family_Values)
                    sResult += "have no family ties";
                if (m_eFamilyValues == FamilyValues.Moderate_Family_Values)
                    sResult += "have not so tight family bonds";
            }

            if (m_eSexuality != pOther.m_eSexuality)
            {
                if (sResult != "")
                    sResult += ", ";

                if (m_eSexuality == Sexuality.Puritan)
                    sResult += "not interested in sex, except for reproduction";
                if (m_eSexuality == Sexuality.Lecherous)
                    sResult += "has insatiable libido";
                if (m_eSexuality == Sexuality.Moderate_sexuality)
                    sResult += pOther.m_eSexuality == Sexuality.Puritan ? "may have sex for fun and pleasure" : "doesn't make sex so much";
            }

            if (m_eSexRelations != pOther.m_eSexRelations)
            {
                if (sResult != "")
                    sResult += ", ";

                if (m_eSexRelations == SexualOrientation.Heterosexual)
                    sResult += "prefers heterosexual relations";
                if (m_eSexRelations == SexualOrientation.Homosexual)
                    sResult += "prefers homosexual relations";
                if (m_eSexRelations == SexualOrientation.Bisexual)
                    sResult += pOther.m_eSexRelations == SexualOrientation.Heterosexual ? "allows homosexual relations" : "allows heterosexual relations";
            }

            if (m_eMarriage != pOther.m_eMarriage)
            {
                if (sResult != "")
                    sResult += " and ";

                if (m_eMarriage == MarriageType.Monogamy)
                {
                    sResult += pOther.m_eMarriage == MarriageType.Polygamy ? "could have only one" : "could have one";

                    if (m_eSexRelations == SexualOrientation.Heterosexual)
                    {
                        if (m_eGenderPriority == GenderPriority.Patriarchy)
                        {
                            sResult += " wife";
                        }
                        if (m_eGenderPriority == GenderPriority.Matriarchy)
                        {
                            sResult += " husband";
                        }
                        if (m_eGenderPriority == GenderPriority.Genders_equality)
                        {
                            sResult += " spouse of opposite gender";
                        }
                    }
                    if (m_eSexRelations == SexualOrientation.Homosexual)
                    {
                        if (m_eSexuality == Sexuality.Puritan)
                            sResult += " companion of the same gender";
                        else
                            sResult += " spouse of the same gender";
                    }
                    if (m_eSexRelations == SexualOrientation.Bisexual)
                    {
                        if (m_eSexuality == Sexuality.Puritan)
                            sResult += " companion of any gender";
                        else
                            sResult += " spouse of any gender";
                    }
                }
                if (m_eMarriage == MarriageType.Polyamory)
                {
                    sResult += "rejects marriage";

                    if (m_eSexuality == Sexuality.Puritan)
                        sResult += ", but may be in platonic relations with persons";
                    else
                        sResult += ", but could have a number of lovers";

                    if (m_eSexRelations == SexualOrientation.Heterosexual)
                    {
                        sResult += " of opposite gender";
                    }
                    if (m_eSexRelations == SexualOrientation.Homosexual)
                    {
                        sResult += " of the same gender";
                    }
                    if (m_eSexRelations == SexualOrientation.Bisexual)
                    {
                        sResult += " of both genders";
                    }
                }
                if (m_eMarriage == MarriageType.Polygamy)
                {
                    if (m_eSexRelations == SexualOrientation.Heterosexual)
                    {
                        if (m_eSexRelations == SexualOrientation.Heterosexual)
                        {
                            if (m_eGenderPriority == GenderPriority.Patriarchy)
                            {
                                sResult += "could have many wifes";
                            }
                            if (m_eGenderPriority == GenderPriority.Matriarchy)
                            {
                                sResult += "could have many husbands";
                            }
                            if (m_eGenderPriority == GenderPriority.Genders_equality)
                            {
                                sResult += "lives in big heterosexual families";
                            }
                        }
                    }
                    if (m_eSexRelations == SexualOrientation.Homosexual)
                    {
                        if (m_eSexuality == Sexuality.Puritan)
                        {
                            if (m_eGenderPriority == GenderPriority.Patriarchy)
                            {
                                sResult += "lives in large brotherhoods";
                            }
                            if (m_eGenderPriority == GenderPriority.Matriarchy)
                            {
                                sResult += "lives in large sisterhoods";
                            }
                            if (m_eGenderPriority == GenderPriority.Genders_equality)
                            {
                                sResult += "lives in large communes with persons of the same gender";
                            }
                        }
                        else
                            sResult += "lives in big homosexual families";
                    }
                    if (m_eSexRelations == SexualOrientation.Bisexual)
                    {
                        if (m_eSexuality == Sexuality.Puritan)
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
        /// Возвращает значение от 6 (полные противоположности) до -6 (полное совпадение).
        /// </summary>
        public int GetDifference(Customs pOpponent)
        {
            string s1 = "", s2 = "";
            return GetDifference(pOpponent, ref s1, ref s2);
        }

        /// <summary>
        /// Вычисляет враждебность друг сообществ на основании их обычаев.
        /// Возвращает значение от 6 (полные противоположности) до -6 (полное совпадение).
        /// </summary>
        /// <param name="pOpponent">обычаи другого сообщества</param>
        /// <param name="sReasons">мотивация враждебности</param>
        /// <returns></returns>
        public int GetDifference(Customs pOpponent, ref string sPositiveReasons, ref string sNegativeReasons)
        {
            int iHostility = 0;

            if (m_eGenderPriority == pOpponent.m_eGenderPriority)
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_eGenderPriority.ToString().Replace('_', ' ') + "\n";
            }
            else
                if (m_eGenderPriority != GenderPriority.Genders_equality &&
                    pOpponent.m_eGenderPriority != GenderPriority.Genders_equality)
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.m_eGenderPriority.ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.m_eGenderPriority.ToString().Replace('_', ' ') + "\n";
                }

            if (m_eMindSet == pOpponent.m_eMindSet)
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_eMindSet.ToString().Replace('_', ' ') + "\n";
            }
            else
                if (m_eMindSet != MindSet.Balanced_mind &&
                    pOpponent.m_eMindSet != MindSet.Balanced_mind)
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.m_eMindSet.ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.m_eMindSet.ToString().Replace('_', ' ') + "\n";
                }


            if (m_eSexuality == pOpponent.m_eSexuality)
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_eSexuality.ToString().Replace('_', ' ') + "\n";
            }
            else
                if (m_eSexuality != Sexuality.Moderate_sexuality &&
                    pOpponent.m_eSexuality != Sexuality.Moderate_sexuality)
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.m_eSexuality.ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.m_eSexuality.ToString().Replace('_', ' ') + "\n";
                }

            if (m_eSexRelations == pOpponent.m_eSexRelations)
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_eSexRelations.ToString().Replace('_', ' ') + "\n";
            }
            else
                if (m_eSexRelations != SexualOrientation.Bisexual &&
                    pOpponent.m_eSexRelations != SexualOrientation.Bisexual)
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.m_eSexRelations.ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.m_eSexRelations.ToString().Replace('_', ' ') + "\n";
                }

            if (m_eMarriage == pOpponent.m_eMarriage)
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_eMarriage.ToString().Replace('_', ' ') + "\n";
            }
            else
                if (m_eMarriage != MarriageType.Polygamy &&
                    pOpponent.m_eMarriage != MarriageType.Polygamy)
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.m_eMarriage.ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.m_eMarriage.ToString().Replace('_', ' ') + "\n";
                }

            if (m_eBodyModifications == pOpponent.m_eBodyModifications)
            {
                if (m_eBodyModifications != BodyModifications.Body_Modifications_Mandatory || ListsEqual(m_cMandatoryModifications, pOpponent.m_cMandatoryModifications))
                {
                    iHostility--;
                    sPositiveReasons += " (+1) " + pOpponent.m_eBodyModifications.ToString().Replace('_', ' ') + "\n";
                }
            }
            else
                if (m_eBodyModifications != BodyModifications.Body_Modifications_Allowed &&
                    pOpponent.m_eBodyModifications != BodyModifications.Body_Modifications_Allowed)
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.m_eBodyModifications.ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.m_eBodyModifications.ToString().Replace('_', ' ') + "\n";
                }

            if (m_eClothes == pOpponent.m_eClothes)
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_eClothes.ToString().Replace('_', ' ') + "\n";
            }
            else
                if (m_eClothes != Clothes.Revealing_Clothes &&
                    pOpponent.m_eClothes != Clothes.Revealing_Clothes)
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.m_eClothes.ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.m_eClothes.ToString().Replace('_', ' ') + "\n";
                }

            if (m_eAdornments == pOpponent.m_eAdornments)
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_eAdornments.ToString().Replace('_', ' ') + "\n";
            }
            else
                if (m_eAdornments != Adornments.Some_Adornments &&
                    pOpponent.m_eAdornments != Adornments.Some_Adornments)
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.m_eAdornments.ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.m_eAdornments.ToString().Replace('_', ' ') + "\n";
                }

            if (m_eFamilyValues == pOpponent.m_eFamilyValues)
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_eFamilyValues.ToString().Replace('_', ' ') + "\n";
            }
            else
                if (m_eFamilyValues != FamilyValues.Moderate_Family_Values &&
                    pOpponent.m_eFamilyValues != FamilyValues.Moderate_Family_Values)
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.m_eFamilyValues.ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.m_eFamilyValues.ToString().Replace('_', ' ') + "\n";
                }

            if (m_eProgress == pOpponent.m_eProgress)
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_eProgress.ToString().Replace('_', ' ') + "\n";
            }
            else
                if (m_eProgress != Progressiveness.Moderate_Science &&
                    pOpponent.m_eProgress != Progressiveness.Moderate_Science)
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.m_eProgress.ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.m_eProgress.ToString().Replace('_', ' ') + "\n";
                }

            if (m_eMagic == pOpponent.m_eMagic)
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_eMagic.ToString().Replace('_', ' ') + "\n";
            }
            else
                if (m_eMagic != Magic.Magic_Allowed &&
                    pOpponent.m_eMagic != Magic.Magic_Allowed)
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.m_eMagic.ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.m_eMagic.ToString().Replace('_', ' ') + "\n";
                }

            return iHostility;
        }

        public override bool Equals(object obj)
        {
            Customs pOther = obj as Customs;
            if (pOther == null)
                return false;

            return m_eGenderPriority == pOther.m_eGenderPriority &&
                    m_eMindSet == pOther.m_eMindSet &&
                    m_eSexuality == pOther.m_eSexuality &&
                    m_eSexRelations == pOther.m_eSexRelations &&
                    m_eMarriage == pOther.m_eMarriage &&
                    m_eBodyModifications == pOther.m_eBodyModifications &&
                    (m_eBodyModifications != BodyModifications.Body_Modifications_Mandatory || ListsEqual(m_cMandatoryModifications, pOther.m_cMandatoryModifications)) &&
                    m_eClothes == pOther.m_eClothes &&
                    m_eAdornments == pOther.m_eAdornments &&
                    m_eFamilyValues == pOther.m_eFamilyValues &&
                    m_eProgress == pOther.m_eProgress &&
                    m_eMagic == pOther.m_eMagic;
        }
    }
}
