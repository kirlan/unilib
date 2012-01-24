using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Intuition,
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

        public enum FamilySize
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

        public enum Childhood
        {
            Family_upbringing,
            Common_upbringing,
            Communal_upbringing
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

        public GenderPriority m_eGenderPriority = GenderPriority.Genders_equality;

        public MindSet m_eMindSet = MindSet.Balanced_mind;

        public Sexuality m_eSexuality = Sexuality.Moderate_sexuality;

        public SexualOrientation m_eSexRelations = SexualOrientation.Heterosexual;

        public FamilySize m_eFamilySize = FamilySize.Monogamy;

        public BodyModifications m_eBodyModifications = BodyModifications.Body_Modifications_Allowed;

        public Childhood m_eChildhood = Childhood.Common_upbringing;

        public Progressiveness m_eProgress = Progressiveness.Moderate_Science;

        public Magic m_eMagic = Magic.Magic_Allowed;

        public Customs()
        {
            m_eGenderPriority = Rnd.OneChanceFrom(3) ? GenderPriority.Genders_equality : Rnd.OneChanceFrom(3) ? GenderPriority.Matriarchy : GenderPriority.Patriarchy;

            m_eMindSet = Rnd.OneChanceFrom(3) ? MindSet.Balanced_mind : Rnd.OneChanceFrom(3) ? MindSet.Logic : MindSet.Intuition;

            m_eSexuality = !Rnd.OneChanceFrom(3) ? Sexuality.Moderate_sexuality : Rnd.OneChanceFrom(3) ? Sexuality.Puritan : Sexuality.Lecherous;

            m_eSexRelations = Rnd.OneChanceFrom(9) ? SexualOrientation.Bisexual : Rnd.OneChanceFrom(5) ? SexualOrientation.Homosexual : SexualOrientation.Heterosexual;

            if(m_eSexRelations != SexualOrientation.Heterosexual && m_eGenderPriority != GenderPriority.Matriarchy)
                m_eSexRelations = Rnd.OneChanceFrom(9) ? SexualOrientation.Bisexual : Rnd.OneChanceFrom(5) ? SexualOrientation.Homosexual : SexualOrientation.Heterosexual;

            m_eFamilySize = Rnd.OneChanceFrom(3) ? FamilySize.Polygamy : Rnd.OneChanceFrom(9) ? FamilySize.Polyamory : FamilySize.Monogamy;

            m_eBodyModifications = Rnd.OneChanceFrom(3) ? BodyModifications.Body_Modifications_Allowed : Rnd.OneChanceFrom(3) ? BodyModifications.Body_Modifications_Mandatory : BodyModifications.Body_Modifications_Blamed;

            m_eChildhood = Rnd.OneChanceFrom(2) ? Childhood.Common_upbringing : Rnd.OneChanceFrom(3) ? Childhood.Family_upbringing : Childhood.Communal_upbringing;

            m_eProgress = Rnd.OneChanceFrom(2) ? Progressiveness.Moderate_Science : Rnd.OneChanceFrom(3) ? Progressiveness.Technofetishism : Progressiveness.Traditionalism;

            m_eMagic = Rnd.OneChanceFrom(2) ? Magic.Magic_Allowed : Rnd.OneChanceFrom(3) ? Magic.Magic_Praised : Magic.Magic_Feared;
        }

        public Customs(Customs m_pAncestorCustoms)
        {
            m_eGenderPriority = m_pAncestorCustoms.m_eGenderPriority;
            m_eMindSet = m_pAncestorCustoms.m_eMindSet;
            m_eSexuality = m_pAncestorCustoms.m_eSexuality;
            m_eSexRelations = m_pAncestorCustoms.m_eSexRelations;
            m_eFamilySize = m_pAncestorCustoms.m_eFamilySize;
            m_eBodyModifications = m_pAncestorCustoms.m_eBodyModifications;
            m_eChildhood = m_pAncestorCustoms.m_eChildhood;
            m_eProgress = m_pAncestorCustoms.m_eProgress;
            m_eMagic = m_pAncestorCustoms.m_eMagic;
            
            int iChoice = Rnd.Get(16);
            switch (iChoice)
            {
                case 0:
                    if (m_eGenderPriority == GenderPriority.Genders_equality)
                        m_eGenderPriority = Rnd.OneChanceFrom(3) ? GenderPriority.Matriarchy : GenderPriority.Patriarchy;
                    else
                        m_eGenderPriority = GenderPriority.Genders_equality;
                    break;
                case 1:
                    if (m_eMindSet == MindSet.Balanced_mind)
                        m_eMindSet = Rnd.OneChanceFrom(3) ? MindSet.Logic : MindSet.Intuition;
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
                    if (m_eSexRelations == SexualOrientation.Bisexual || !Rnd.OneChanceFrom(5))
                        m_eSexRelations = Rnd.OneChanceFrom(5) ? SexualOrientation.Homosexual : SexualOrientation.Heterosexual;
                    else
                        m_eSexRelations = SexualOrientation.Bisexual;

                    if (m_eSexRelations != SexualOrientation.Heterosexual && m_eGenderPriority != GenderPriority.Matriarchy)
                        m_eSexRelations = Rnd.OneChanceFrom(9) ? SexualOrientation.Bisexual : Rnd.OneChanceFrom(5) ? SexualOrientation.Homosexual : SexualOrientation.Heterosexual;

                    break;
                case 4:
                    if (m_eFamilySize == FamilySize.Polygamy)
                        m_eFamilySize = Rnd.OneChanceFrom(5) ? FamilySize.Polyamory : FamilySize.Monogamy;
                    else
                        m_eFamilySize = FamilySize.Polygamy;
                    break;
                case 5:
                    if (m_eBodyModifications == BodyModifications.Body_Modifications_Allowed)
                        m_eBodyModifications = Rnd.OneChanceFrom(5) ? BodyModifications.Body_Modifications_Mandatory : BodyModifications.Body_Modifications_Blamed;
                    else
                        m_eBodyModifications = BodyModifications.Body_Modifications_Allowed;
                    break;
                case 6:
                    if (m_eChildhood == Childhood.Common_upbringing)
                        m_eChildhood = Rnd.OneChanceFrom(3) ? Childhood.Family_upbringing : Childhood.Communal_upbringing;
                    else
                        m_eChildhood = Childhood.Common_upbringing;
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

            if (m_eMindSet == MindSet.Intuition)
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

            if (m_eFamilySize == FamilySize.Monogamy)
            {
                sResult += "\n   ";
                sResult += "monogamy";
            }
            if (m_eFamilySize == FamilySize.Polyamory)
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
                sResult += "modified body";
            }

            if (m_eChildhood == Childhood.Family_upbringing)
            {
                sResult += "\n   ";
                sResult += "family upbringing";
            }
            if (m_eChildhood == Childhood.Communal_upbringing)
            {
                sResult += "\n   ";
                sResult += "communal upbringing";
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
                sResult += "men";
            }
            if (m_eGenderPriority == GenderPriority.Matriarchy)
            {
                sResult += "women";
            }
            if (m_eGenderPriority == GenderPriority.Genders_equality)
            {
                sResult += "men and women";
            }

            bool bFirst = true;

            if (m_eMindSet == MindSet.Intuition)
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
                sResult += "have and use own magic abilities";
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
                sResult += "has some specific tatoo or pierceing";
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

            if (m_eChildhood == Childhood.Family_upbringing)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "rising own children personally";
            }
            if (m_eChildhood == Childhood.Communal_upbringing)
            {
                if (bFirst)
                {
                    sResult += ", who ";
                    bFirst = false;
                }
                else
                    sResult += ", ";
                sResult += "not rising own children personally";
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

            if (m_eFamilySize == FamilySize.Monogamy)
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
                        sResult += " spouse (of any gender)";
                }
            }
            if (m_eFamilySize == FamilySize.Polyamory)
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
                    sResult += ", but have lovers";

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
            if (m_eFamilySize == FamilySize.Polygamy)
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

            if (m_eFamilySize == pOpponent.m_eFamilySize)
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_eFamilySize.ToString().Replace('_', ' ') + "\n";
            }
            else
                if (m_eFamilySize != FamilySize.Polygamy &&
                    pOpponent.m_eFamilySize != FamilySize.Polygamy)
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.m_eFamilySize.ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.m_eFamilySize.ToString().Replace('_', ' ') + "\n";
                }

            if (m_eBodyModifications == pOpponent.m_eBodyModifications)
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_eBodyModifications.ToString().Replace('_', ' ') + "\n";
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

            if (m_eChildhood == pOpponent.m_eChildhood)
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_eChildhood.ToString().Replace('_', ' ') + "\n";
            }
            else
                if (m_eChildhood != Childhood.Common_upbringing &&
                    pOpponent.m_eChildhood != Childhood.Common_upbringing)
                {
                    iHostility += 2;
                    sNegativeReasons += " (-2) " + pOpponent.m_eChildhood.ToString().Replace('_', ' ') + "\n";
                }
                else
                {
                    iHostility++;
                    sNegativeReasons += " (-1) " + pOpponent.m_eChildhood.ToString().Replace('_', ' ') + "\n";
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
    }
}
