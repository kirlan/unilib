using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace Socium
{
    public enum GenderPriority
    {
        Matriarchy,
        Genders_equality,
        Patriarchy,
    }

    public enum MindBodyPriority
    { 
        Brutes,
        Balanced_body_and_mind,
        Thinkers
    }

    public enum Sexuality
    {
        Lecherous,
        Moderate_sexuality,
        Puritan
    }

    public enum SexRelations
    { 
        Heterosexual,
        Bisexual,
        Homosexual
    }

    public enum MagicPosition
    { 
        Magic_is_praised,
        Magic_is_allowed,
        Magic_is_feared
    }

    public enum FamilySize
    { 
        Monogamy,
        Polygamy,
        Polyamory
    }

    public class Customs
    {
        public GenderPriority m_eGenderPriority = GenderPriority.Genders_equality;

        public MindBodyPriority m_eMindBodyPriority = MindBodyPriority.Balanced_body_and_mind;

        public Sexuality m_eSexuality = Sexuality.Moderate_sexuality;

        public SexRelations m_eSexRelations = SexRelations.Heterosexual;

        public MagicPosition m_eMagicPosition = MagicPosition.Magic_is_allowed;

        public FamilySize m_eFamilySize = FamilySize.Monogamy;

        public Customs()
        {
            m_eGenderPriority = Rnd.OneChanceFrom(3) ? GenderPriority.Matriarchy : GenderPriority.Patriarchy;

            m_eMindBodyPriority = Rnd.OneChanceFrom(3) ? MindBodyPriority.Thinkers : MindBodyPriority.Brutes;

            m_eSexuality = Rnd.OneChanceFrom(3) ? Sexuality.Puritan : Sexuality.Lecherous;

            m_eSexRelations = Rnd.OneChanceFrom(3) ? SexRelations.Homosexual : SexRelations.Heterosexual;

            m_eMagicPosition = Rnd.OneChanceFrom(3) ? MagicPosition.Magic_is_praised : MagicPosition.Magic_is_feared;

            m_eFamilySize = Rnd.OneChanceFrom(3) ? FamilySize.Polyamory : FamilySize.Monogamy;
        }

        public Customs(Customs m_pAncestorCustoms)
        {
            m_eGenderPriority = m_pAncestorCustoms.m_eGenderPriority;

            if (Rnd.OneChanceFrom(3))
            {
                if (m_eGenderPriority == GenderPriority.Genders_equality)
                    m_eGenderPriority = Rnd.OneChanceFrom(3) ? GenderPriority.Matriarchy : GenderPriority.Patriarchy;
                else
                    m_eGenderPriority = GenderPriority.Genders_equality;
            }

            m_eMindBodyPriority = m_pAncestorCustoms.m_eMindBodyPriority;

            if (Rnd.OneChanceFrom(3))
            {
                if (m_eMindBodyPriority == MindBodyPriority.Balanced_body_and_mind)
                    m_eMindBodyPriority = Rnd.OneChanceFrom(3) ? MindBodyPriority.Thinkers : MindBodyPriority.Brutes;
                else
                    m_eMindBodyPriority = MindBodyPriority.Balanced_body_and_mind;
            }

            m_eSexuality = m_pAncestorCustoms.m_eSexuality;

            if (Rnd.OneChanceFrom(3))
            {
                if (m_eSexuality == Sexuality.Moderate_sexuality)
                    m_eSexuality = Rnd.OneChanceFrom(3) ? Sexuality.Puritan : Sexuality.Lecherous;
                else
                    m_eSexuality = Sexuality.Moderate_sexuality;
            }

            m_eSexRelations = m_pAncestorCustoms.m_eSexRelations;

            if (Rnd.OneChanceFrom(3))
            {
                if (m_eSexRelations == SexRelations.Bisexual)
                    m_eSexRelations = Rnd.OneChanceFrom(3) ? SexRelations.Homosexual : SexRelations.Heterosexual;
                else
                    m_eSexRelations = SexRelations.Bisexual;
            }

            m_eMagicPosition = m_pAncestorCustoms.m_eMagicPosition;

            if (Rnd.OneChanceFrom(3))
            {
                if (m_eMagicPosition == MagicPosition.Magic_is_allowed)
                    m_eMagicPosition = Rnd.OneChanceFrom(3) ? MagicPosition.Magic_is_praised : MagicPosition.Magic_is_feared;
                else
                    m_eMagicPosition = MagicPosition.Magic_is_allowed;
            }

            m_eFamilySize = m_pAncestorCustoms.m_eFamilySize;

            if (Rnd.OneChanceFrom(3))
            {
                if (m_eFamilySize == FamilySize.Polygamy)
                    m_eFamilySize = Rnd.OneChanceFrom(3) ? FamilySize.Polyamory : FamilySize.Monogamy;
                else
                    m_eFamilySize = FamilySize.Polygamy;
            }
        }

        public void Evolve()
        {
            int iChoice = Rnd.Get(6);

            switch (iChoice)
            {
                case 0:
                    m_eGenderPriority = GenderPriority.Genders_equality;
                    break;
                case 1:
                    m_eMindBodyPriority = MindBodyPriority.Balanced_body_and_mind;
                    break;
                case 2:
                    m_eSexuality = Sexuality.Moderate_sexuality;
                    break;
                case 3:
                    m_eSexRelations = SexRelations.Bisexual;
                    break;
                case 4:
                    m_eMagicPosition = MagicPosition.Magic_is_allowed;
                    break;
                case 5:
                    m_eFamilySize = FamilySize.Polygamy;
                    break;
            }
        }

        public void Degrade()
        {
            int iChoice = Rnd.Get(6);

            switch (iChoice)
            {
                case 0:
                    m_eGenderPriority = Rnd.OneChanceFrom(3) ? GenderPriority.Matriarchy : GenderPriority.Patriarchy;
                    break;
                case 1:
                    m_eMindBodyPriority = Rnd.OneChanceFrom(3) ? MindBodyPriority.Thinkers : MindBodyPriority.Brutes;
                    break;
                case 2:
                    m_eSexuality = Rnd.OneChanceFrom(3) ? Sexuality.Puritan : Sexuality.Lecherous;
                    break;
                case 3:
                    m_eSexRelations = Rnd.OneChanceFrom(3) ? SexRelations.Homosexual : SexRelations.Heterosexual;
                    break;
                case 4:
                    m_eMagicPosition = Rnd.OneChanceFrom(3) ? MagicPosition.Magic_is_praised : MagicPosition.Magic_is_feared;
                    break;
                case 5:
                    m_eFamilySize = Rnd.OneChanceFrom(3) ? FamilySize.Polyamory : FamilySize.Monogamy;
                    break;
            }
        }

        //public string GetCustomsString()
        //{
        //    string sResult = "";

        //    if (m_eGenderPriority == GenderPriority.Patriarchy)
        //    {
        //        sResult += "\n   ";
        //        sResult += "females";
        //    }
        //    if (m_eGenderPriority == GenderPriority.Matriarchy)
        //    {
        //        sResult += "\n   ";
        //        sResult += "males";
        //    }

        //    if (m_eMindBodyPriority == MindBodyPriority.Hunks)
        //    {
        //        sResult += "\n   ";
        //        sResult += "weakness";
        //    }
        //    if (m_eMindBodyPriority == MindBodyPriority.Thinkers)
        //    {
        //        sResult += "\n   ";
        //        sResult += "stupidity";
        //    }

        //    if (m_eSexuality == Sexuality.Chaste)
        //    {
        //        sResult += "\n   ";
        //        sResult += "lechery";
        //    }
        //    if (m_eSexuality == Sexuality.Lecherous)
        //    {
        //        sResult += "\n   ";
        //        sResult += "chastity";
        //    }

        //    if (m_eSexRelations == SexRelations.Heterosexual)
        //    {
        //        sResult += "\n   ";
        //        sResult += "homosexuality";
        //    }
        //    if (m_eSexRelations == SexRelations.Homosexual)
        //    {
        //        sResult += "\n   ";
        //        sResult += "heterosexuality";
        //    }

        //    if (m_eMagicPosition == MagicPosition.Magic_is_feared)
        //    {
        //        sResult += "\n   ";
        //        sResult += "psi abilities";
        //    }
        //    if (m_eMagicPosition == MagicPosition.Magic_is_praised)
        //    {
        //        sResult += "\n   ";
        //        sResult += "psi inability";
        //    }

        //    if (m_eFamilySize == FamilySize.Monogamy)
        //    {
        //        sResult += "\n   ";
        //        sResult += "adulter";
        //    }
        //    if (m_eFamilySize == FamilySize.Free_love)
        //    {
        //        sResult += "\n   ";
        //        sResult += "marriage";
        //    }

        //    return "Social stigmas: " + sResult + "\n";
        //}

        public string GetCustomsString()
        {
            string sResult = "";

            if (m_eGenderPriority == GenderPriority.Patriarchy)
            {
                sResult += "\n   ";
                sResult += "masculinity";
            }
            if (m_eGenderPriority == GenderPriority.Matriarchy)
            {
                sResult += "\n   ";
                sResult += "femininity";
            }

            if (m_eMindBodyPriority == MindBodyPriority.Brutes)
            {
                sResult += "\n   ";
                sResult += "brute force";
            }
            if (m_eMindBodyPriority == MindBodyPriority.Thinkers)
            {
                sResult += "\n   ";
                sResult += "keen intellect";
            }

            if (m_eSexuality == Sexuality.Puritan)
            {
                sResult += "\n   ";
                sResult += "chastity";
            }
            if (m_eSexuality == Sexuality.Lecherous)
            {
                sResult += "\n   ";
                sResult += "lechery";
            }

            if (m_eSexRelations == SexRelations.Heterosexual)
            {
                sResult += "\n   ";
                sResult += "heterosexuality";
            }
            if (m_eSexRelations == SexRelations.Homosexual)
            {
                sResult += "\n   ";
                sResult += "homosexuality";
            }

            if (m_eMagicPosition == MagicPosition.Magic_is_feared)
            {
                sResult += "\n   ";
                sResult += "magic rejection";
            }
            if (m_eMagicPosition == MagicPosition.Magic_is_praised)
            {
                sResult += "\n   ";
                sResult += "magic";
            }

            if (m_eFamilySize == FamilySize.Monogamy)
            {
                sResult += "\n   ";
                sResult += "monogamy";
            }
            if (m_eFamilySize == FamilySize.Polyamory)
            {
                sResult += "\n   ";
                sResult += "polyamory";
            }

            return "Praises: " + sResult + "\n";
        }

        public int GetDifference(Customs pOpponent, ref string sReasons)
        {
            int iHostility = 0;

            if (m_eGenderPriority == pOpponent.m_eGenderPriority)
            {
                iHostility--;
                sReasons += pOpponent.m_eGenderPriority.ToString().Replace('_', ' ') + " \t(+1)\n";
            }
            else
                if (m_eGenderPriority != GenderPriority.Genders_equality &&
                    pOpponent.m_eGenderPriority != GenderPriority.Genders_equality)
                {
                    iHostility++;
                    sReasons += pOpponent.m_eGenderPriority.ToString().Replace('_', ' ') + " \t(-1)\n";
                }

            if (m_eMindBodyPriority == pOpponent.m_eMindBodyPriority)
            {
                iHostility--;
                sReasons += pOpponent.m_eMindBodyPriority.ToString().Replace('_', ' ') + " \t(+1)\n";
            }
            else
                if (m_eMindBodyPriority != MindBodyPriority.Balanced_body_and_mind &&
                    pOpponent.m_eMindBodyPriority != MindBodyPriority.Balanced_body_and_mind)
                {
                    iHostility++;
                    sReasons += pOpponent.m_eMindBodyPriority.ToString().Replace('_', ' ') + " \t(-1)\n";
                }

            if (m_eSexuality == pOpponent.m_eSexuality)
            {
                iHostility--;
                sReasons += pOpponent.m_eSexuality.ToString().Replace('_', ' ') + " \t(+1)\n";
            }
            else
                if (m_eSexuality != Sexuality.Moderate_sexuality &&
                    pOpponent.m_eSexuality != Sexuality.Moderate_sexuality)
                {
                    iHostility++;
                    sReasons += pOpponent.m_eSexuality.ToString().Replace('_', ' ') + " \t(-1)\n";
                }

            if (m_eSexRelations == pOpponent.m_eSexRelations)
            {
                iHostility--;
                sReasons += pOpponent.m_eSexRelations.ToString().Replace('_', ' ') + " \t(+1)\n";
            }
            else
                if (m_eSexRelations != SexRelations.Bisexual &&
                    pOpponent.m_eSexRelations != SexRelations.Bisexual)
                {
                    iHostility++;
                    sReasons += pOpponent.m_eSexRelations.ToString().Replace('_', ' ') + " \t(-1)\n";
                }


            if (m_eMagicPosition == pOpponent.m_eMagicPosition)
            {
                iHostility--;
                sReasons += pOpponent.m_eMagicPosition.ToString().Replace('_', ' ') + " \t(+1)\n";
            }
            else
                if (m_eMagicPosition != MagicPosition.Magic_is_allowed &&
                    pOpponent.m_eMagicPosition != MagicPosition.Magic_is_allowed)
                {
                    iHostility++;
                    sReasons += pOpponent.m_eMagicPosition.ToString().Replace('_', ' ') + " \t(-1)\n";
                }

            if (m_eFamilySize == pOpponent.m_eFamilySize)
            {
                iHostility--;
                sReasons += pOpponent.m_eFamilySize.ToString().Replace('_', ' ') + " \t(+1)\n";
            }
            else
                if (m_eFamilySize != FamilySize.Polygamy &&
                    pOpponent.m_eFamilySize != FamilySize.Polygamy)
                {
                    iHostility++;
                    sReasons += pOpponent.m_eFamilySize.ToString().Replace('_', ' ') + " \t(-1)\n";
                }

            return iHostility;
        }
    }
}
