using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace GeneLab.Genetix
{
    public enum HairsAmount
    {
        /// <summary>
        /// нет другой растительности на голове, кроме естественного покрова тела
        /// </summary>
        None,
        /// <summary>
        /// редкие/короткие волосы
        /// </summary>
        Sparse,
        /// <summary>
        /// густые и длинные волосы
        /// </summary>
        Thick
    }

    public enum HairsType
    {
        /// <summary>
        /// обычные волосы
        /// </summary>
        Hair,
        /// <summary>
        /// вибриссы, как у животных
        /// </summary>
        Whiskers,
        /// <summary>
        /// щупальца
        /// </summary>
        Tentackles
    }

    public enum HairsColor
    {
        /// <summary>
        /// светлые волосы
        /// </summary>
        Blonde,
        /// <summary>
        /// тёмные волосы
        /// </summary>
        Brunette,
        /// <summary>
        /// рыжие волосы
        /// </summary>
        Red,
        /// <summary>
        /// русые волосы
        /// </summary>
        DarkBlond,
        /// <summary>
        /// альбинос - чисто белые волосы
        /// </summary>
        Albino,
        /// <summary>
        /// иссиня-чёрные волосы
        /// </summary>
        Black,
        /// <summary>
        /// зелёные волосы
        /// </summary>
        Green,
        /// <summary>
        /// синие волосы
        /// </summary>
        Blue,
        /// <summary>
        /// цвет волос совпадает с цветом естественного покрова тела
        /// </summary>
        Hide
    }
    
    public class HairsGenetix : GenetixBase
    {
        /// <summary>
        /// Males usually are bald, but have dense beard and moustache, while females have long hairs and no beard or moustache. Most common colors are blue and green.
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            if (m_eHairsM == HairsAmount.None &&
                 m_eHairsF == HairsAmount.None &&
                 m_eBeardM == HairsAmount.None &&
                 m_eBeardF == HairsAmount.None)
                return "";

            string sColors = "";
            if (m_eHairsType == HairsType.Tentackles)
            {
                if (m_cHairColors.Count == 1)
                    sColors = "Most common 'hairs' color is ";
                else
                    sColors = "Most common 'hairs' colors are ";
            }
            else
            {
                if (m_cHairColors.Count == 1)
                    sColors = "Most common hairs color is ";
                else
                    sColors = "Most common hairs colors are ";
            }

            bool bColorsFull = false;
            for (int i = 0; i < m_cHairColors.Count; i++)
            {
                if (bColorsFull)
                {
                    if (m_cHairColors[i] != HairsColor.Hide)
                    {
                        if (i == m_cHairColors.Count - 1)
                            sColors += " and ";
                        else
                            sColors += ", ";
                    }
                }
                switch (m_cHairColors[i])
                {
                    case HairsColor.Albino:
                        sColors += "snow-white";
                        bColorsFull = true;
                        break;
                    case HairsColor.Blonde:
                        sColors += "white";
                        bColorsFull = true;
                        break;
                    case HairsColor.Brunette:
                        sColors += "dark";
                        bColorsFull = true;
                        break;
                    case HairsColor.Black:
                        sColors += "black";
                        bColorsFull = true;
                        break;
                    case HairsColor.DarkBlond:
                        sColors += "gold";
                        bColorsFull = true;
                        break;
                    case HairsColor.Red:
                        sColors += "red";
                        bColorsFull = true;
                        break;
                    case HairsColor.Blue:
                        sColors += "blue";
                        bColorsFull = true;
                        break;
                    case HairsColor.Green:
                        sColors += "green";
                        bColorsFull = true;
                        break;
                }
            }
            if (!bColorsFull)
                sColors = "";
            else
                sColors = " " + sColors + ".";

            string sMales = "?";
            string sFemales = "?";

            string sHairM = "?";
            string sBeardM = "?";
            string sHairF = "?";
            string sBeardF = "?";

            switch (m_eHairsType)
            {
                case HairsType.Whiskers:
                    {
                        switch (m_eHairsM)
                        {
                            case HairsAmount.None:
                                sHairM = "";
                                break;
                            case HairsAmount.Sparse:
                                sHairM = "sparse mane";
                                break;
                            case HairsAmount.Thick:
                                sHairM = "dense mane";
                                break;
                        }

                        switch (m_eBeardM)
                        {
                            case HairsAmount.None:
                                sBeardM = "";
                                break;
                            case HairsAmount.Sparse:
                                sBeardM = "sparse whiskers";
                                break;
                            case HairsAmount.Thick:
                                sBeardM = "dense whiskers";
                                break;
                        }

                        switch (m_eHairsF)
                        {
                            case HairsAmount.None:
                                sHairF = "";
                                break;
                            case HairsAmount.Sparse:
                                sHairF = "sparse mane";
                                break;
                            case HairsAmount.Thick:
                                sHairF = "dense mane";
                                break;
                        }

                        switch (m_eBeardF)
                        {
                            case HairsAmount.None:
                                sBeardF = "";
                                break;
                            case HairsAmount.Sparse:
                                sBeardF = "sparse whiskers";
                                break;
                            case HairsAmount.Thick:
                                sBeardF = "dense whiskers";
                                break;
                        }

                        sMales = "have ";
                        sFemales = "have ";

                        if (sHairM == "")
                            sMales += sBeardM;
                        else
                            if (sBeardM == "")
                                sMales += sHairM;
                            else
                                sMales += sHairM + " and " + sBeardM;

                        if (sMales == "have ")
                            sMales = "";

                        if (sHairF == "")
                            sFemales += sBeardF;
                        else
                            if (sBeardF == "")
                                sFemales += sHairF;
                            else
                                sFemales += sHairF + " and " + sBeardF;

                        if (sFemales == "have ")
                            sFemales = "";
                    }
                    break;
                case HairsType.Hair:
                    {
                        switch (m_eHairsM)
                        {
                            case HairsAmount.None:
                                sHairM = "";
                                break;
                            case HairsAmount.Sparse:
                                sHairM = "short hairs";
                                break;
                            case HairsAmount.Thick:
                                sHairM = "long hairs";
                                break;
                        }

                        switch (m_eBeardM)
                        {
                            case HairsAmount.None:
                                sBeardM = "no beard or moustache";
                                break;
                            case HairsAmount.Sparse:
                                sBeardM = "sparse beard and moustache";
                                break;
                            case HairsAmount.Thick:
                                sBeardM = "dense beard and moustache";
                                break;
                        }

                        switch (m_eHairsF)
                        {
                            case HairsAmount.None:
                                sHairF = "";
                                break;
                            case HairsAmount.Sparse:
                                sHairF = "short hairs";
                                break;
                            case HairsAmount.Thick:
                                sHairF = "long hairs";
                                break;
                        }

                        switch (m_eBeardF)
                        {
                            case HairsAmount.None:
                                sBeardF = "no beard or moustache";
                                break;
                            case HairsAmount.Sparse:
                                sBeardF = "sparse beard and moustache";
                                break;
                            case HairsAmount.Thick:
                                sBeardF = "dense beard and moustache";
                                break;
                        }

                        sMales = "have ";
                        sFemales = "have ";

                        if (sHairM == "")
                        {
                            if (m_eBeardM == HairsAmount.None)
                                sMales = "are bald, and have " + sBeardM;
                            else
                                sMales = "are bald, but have " + sBeardM;
                        }
                        else
                        {
                            sMales += sHairM + " and " + sBeardM;
                        }

                        if (sHairF == "")
                        {
                            if (m_eBeardF == HairsAmount.None)
                                sFemales = "are bald, and have " + sBeardF;
                            else
                                sFemales = "are bald, but have " + sBeardF;
                        }
                        else
                        {
                            sFemales += sHairF + " and " + sBeardF;
                        }

                        if (sFemales == "have ")
                            sFemales = "";
                    }
                    break;
                case HairsType.Tentackles:
                    {
                        switch (m_eHairsM)
                        {
                            case HairsAmount.None:
                                sHairM = "";
                                break;
                            case HairsAmount.Sparse:
                                sHairM = "several tentacles on head instead of hairs";
                                break;
                            case HairsAmount.Thick:
                                sHairM = "a lot of tentacles on head instead of hairs";
                                break;
                        }

                        switch (m_eBeardM)
                        {
                            case HairsAmount.None:
                                sBeardM = "";
                                break;
                            case HairsAmount.Sparse:
                                sBeardM = "several tentacles around mouth";
                                break;
                            case HairsAmount.Thick:
                                sBeardM = "a lot of tentacles around mouth";
                                break;
                        }

                        switch (m_eHairsF)
                        {
                            case HairsAmount.None:
                                sHairF = "";
                                break;
                            case HairsAmount.Sparse:
                                sHairF = "several tentacles on head instead of hairs";
                                break;
                            case HairsAmount.Thick:
                                sHairF = "a lot of tentacles on head instead of hairs";
                                break;
                        }

                        switch (m_eBeardF)
                        {
                            case HairsAmount.None:
                                sBeardF = "";
                                break;
                            case HairsAmount.Sparse:
                                sBeardF = "several tentacles around mouth";
                                break;
                            case HairsAmount.Thick:
                                sBeardF = "a lot of tentacles around mouth";
                                break;
                        }

                        sMales = "have ";
                        sFemales = "have ";

                        if (sHairM == "")
                            sMales += sBeardM;
                        else
                            if (sBeardM == "")
                                sMales += sHairM;
                            else
                                sMales += sHairM + " and " + sBeardM;

                        if (sMales == "have ")
                            sMales = "";

                        if (sHairF == "")
                            sFemales += sBeardF;
                        else
                            if (sBeardF == "")
                                sFemales += sHairF;
                            else
                                sFemales += sHairF + " and " + sBeardF;

                        if (sFemales == "have ")
                            sFemales = "";
                    }
                    break;
            }

            if (sMales == sFemales)
                return "Both males and females " + sMales + "." + sColors;
            else
            {
                if (sMales == "")
                    return "Females usually " + sFemales + ", while males not." + sColors;

                if (sFemales == "")
                    return "Males usually " + sMales + ", while females not." + sColors;

                return "Males usually " + sMales + ", while females " + sFemales + "." + sColors;
            }
        }

        /// <summary>
        /// He is bald, but has dense blue beard and moustache.
        /// </summary>
        /// <returns></returns>
        public string GetDescription(CPerson._Gender eGender)
        {
            if (m_eHairsM == HairsAmount.None &&
                 m_eHairsF == HairsAmount.None &&
                 m_eBeardM == HairsAmount.None &&
                 m_eBeardF == HairsAmount.None)
                return eGender == CPerson._Gender.Male ? "He is completely bald." : "She is completely bald.";

            string sColor = "";
            switch (m_cHairColors[0])
            {
                case HairsColor.Albino:
                    sColor += "snow-white";
                    break;
                case HairsColor.Blonde:
                    sColor += "white";
                    break;
                case HairsColor.Brunette:
                    sColor += "dark";
                    break;
                case HairsColor.Black:
                    sColor += "black";
                    break;
                case HairsColor.DarkBlond:
                    sColor += "gold";
                    break;
                case HairsColor.Red:
                    sColor += "red";
                    break;
                case HairsColor.Blue:
                    sColor += "blue";
                    break;
                case HairsColor.Green:
                    sColor += "green";
                    break;
            }

            string sResult = eGender == CPerson._Gender.Male ? "He has " : "She has ";

            string sHair = "?";
            string sBeard = "?";

            HairsAmount eHeadAmount = eGender == CPerson._Gender.Male ? m_eHairsM : m_eHairsF;
            HairsAmount eBeardAmount = eGender == CPerson._Gender.Male ? m_eBeardM : m_eBeardF;
            switch (m_eHairsType)
            {
                case HairsType.Whiskers:
                    {
                        switch (eHeadAmount)
                        {
                            case HairsAmount.None:
                                sHair = "";
                                break;
                            case HairsAmount.Sparse:
                                sHair = "sparse " + sColor + " mane";
                                break;
                            case HairsAmount.Thick:
                                sHair = "dense " + sColor + " mane";
                                break;
                        }
                        switch (eBeardAmount)
                        {
                            case HairsAmount.None:
                                sBeard = "";
                                break;
                            case HairsAmount.Sparse:
                                sBeard = "sparse " + (sHair == "" ? sColor + " ":"") + "whiskers";
                                break;
                            case HairsAmount.Thick:
                                sBeard = "dense " + (sHair == "" ? sColor + " " : "") + "whiskers";
                                break;
                        }


                        if (sHair == "")
                            sResult += sBeard;
                        else
                            if (sBeard == "")
                                sResult += sHair;
                            else
                                sResult += sHair + " and " + sBeard;

                        if (sHair == "" && sBeard == "")
                            sResult = "";
                    }
                    break;
                case HairsType.Hair:
                    {
                        switch (eHeadAmount)
                        {
                            case HairsAmount.None:
                                sHair = "";
                                break;
                            case HairsAmount.Sparse:
                                sHair = "short " + sColor + " hairs";
                                break;
                            case HairsAmount.Thick:
                                sHair = "long " + sColor + " hairs";
                                break;
                        }
                        switch (eBeardAmount)
                        {
                            case HairsAmount.None:
                                sBeard = "no beard or moustache";
                                break;
                            case HairsAmount.Sparse:
                                sBeard = "sparse " + (sHair == "" ? sColor + " " : "") + "beard and moustache";
                                break;
                            case HairsAmount.Thick:
                                sBeard = "dense " + (sHair == "" ? sColor + " " : "") + "beard and moustache";
                                break;
                        }

                        if (sHair == "")
                        {
                            if (eBeardAmount == HairsAmount.None)
                                sResult = (eGender == CPerson._Gender.Male ? "He" : "She") + " is bald, and has " + sBeard;
                            else
                                sResult = (eGender == CPerson._Gender.Male ? "He" : "She") + " is bald, but has " + sColor + " " + sBeard;
                        }
                        else
                        {
                            sResult += sHair + " and " + sBeard;
                        }
                    }
                    break;
                case HairsType.Tentackles:
                    {
                        switch (eHeadAmount)
                        {
                            case HairsAmount.None:
                                sHair = "";
                                break;
                            case HairsAmount.Sparse:
                                sHair = "several " + sColor + " tentacles on head instead of hairs";
                                break;
                            case HairsAmount.Thick:
                                sHair = "a lot of " + sColor + " tentacles on head instead of hairs";
                                break;
                        }

                        switch (eBeardAmount)
                        {
                            case HairsAmount.None:
                                sBeard = "";
                                break;
                            case HairsAmount.Sparse:
                                sBeard = "several " + (sHair == "" ? sColor + " " : "") + "tentacles around mouth";
                                break;
                            case HairsAmount.Thick:
                                sBeard = "a lot of " + (sHair == "" ? sColor + " " : "") + "tentacles around mouth";
                                break;
                        }

                        if (sHair == "")
                            sResult += sBeard;
                        else
                            if (sBeard == "")
                                sResult += sHair;
                            else
                                sResult += sHair + " and " + sBeard;

                        if (sHair == "" && sBeard == "")
                            sResult = "";
                    }
                    break;
            }

            if (sResult != "")
                sResult += ".";

            return sResult;
        }

        public static HairsGenetix HumanWhite
        {
            get { return new HairsGenetix(HairsAmount.Sparse, HairsAmount.Thick, HairsAmount.Sparse, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.Brunette, HairsColor.DarkBlond, HairsColor.Red, HairsColor.Black }); }
        }

        public static HairsGenetix HumanBlack
        {
            get { return new HairsGenetix(HairsAmount.Sparse, HairsAmount.Sparse, HairsAmount.None, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette }); }
        }

        public static HairsGenetix Elf
        {
            get { return new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsAmount.None, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.Brunette, HairsColor.DarkBlond, HairsColor.Black }); }
        }

        public static HairsGenetix Drow
        {
            get { return new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsAmount.None, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.DarkBlond, HairsColor.Brunette }); }
        }

        public static HairsGenetix Dwarf
        {
            get { return new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.Brunette, HairsColor.DarkBlond, HairsColor.Red, HairsColor.Black }); }
        }

        public static HairsGenetix Ktulhu
        {
            get { return new HairsGenetix(HairsAmount.Sparse, HairsAmount.Thick, HairsAmount.Sparse, HairsAmount.None, HairsType.Tentackles, new HairsColor[] { HairsColor.Hide }); }
        }

        public static HairsGenetix None
        {
            get { return new HairsGenetix(HairsAmount.None, HairsAmount.None, HairsAmount.None, HairsAmount.None, HairsType.Hair, new HairsColor[] { }); }
        }

        public static HairsGenetix AnimalWhiskers
        {
            get { return new HairsGenetix(HairsAmount.None, HairsAmount.None, HairsAmount.Sparse, HairsAmount.Sparse, HairsType.Whiskers, new HairsColor[] { HairsColor.Hide }); }
        }

        public HairsAmount m_eHairsM = HairsAmount.Sparse;

        public HairsAmount m_eHairsF = HairsAmount.Thick;

        public HairsAmount m_eBeardM = HairsAmount.Thick;

        public HairsAmount m_eBeardF = HairsAmount.None;

        public HairsType m_eHairsType = HairsType.Hair;

        public List<HairsColor> m_cHairColors = new List<HairsColor>(new HairsColor[] { HairsColor.Brunette, HairsColor.Blonde, HairsColor.DarkBlond, HairsColor.Red });

        public bool IsIdentical(GenetixBase pOther)
        {
            HairsGenetix pAnother = pOther as HairsGenetix;

            if (pAnother == null)
                return false;

            if (pAnother.m_cHairColors.Count != m_cHairColors.Count)
                return false;
            else
            {
                foreach (HairsColor eColor in m_cHairColors)
                    if (!pAnother.m_cHairColors.Contains(eColor))
                        return false;
            }

            return m_eHairsM == pAnother.m_eHairsM &&
                m_eHairsF == pAnother.m_eHairsF &&
                m_eBeardM == pAnother.m_eBeardM &&
                m_eBeardF == pAnother.m_eBeardF &&
                m_eHairsType == pAnother.m_eHairsType;
        }

        public HairsGenetix()
        { }

        public bool CheckHairColors()
        {
            if (m_cHairColors.Count == 0 &&
                (m_eHairsM != HairsAmount.None ||
                 m_eHairsF != HairsAmount.None ||
                 m_eBeardM != HairsAmount.None ||
                 m_eBeardF != HairsAmount.None))
                return true;

            List<HairsColor> cTest = new List<HairsColor>();
            foreach (HairsColor eColor in m_cHairColors)
            {
                if (cTest.Contains(eColor))
                    return true;

                cTest.Add(eColor);
            }

            return false;
        }

        public HairsGenetix(HairsGenetix pPredcessor)
        {
            m_eHairsM = pPredcessor.m_eHairsM;
            m_eHairsF = pPredcessor.m_eHairsF;
            m_eBeardM = pPredcessor.m_eBeardM;
            m_eBeardF = pPredcessor.m_eBeardF;
            m_eHairsType = pPredcessor.m_eHairsType;

            m_cHairColors.Clear();
            m_cHairColors.AddRange(pPredcessor.m_cHairColors);

            CheckHairColors();
        }

        public HairsGenetix(HairsAmount eHairsM, HairsAmount eHairsF, HairsAmount eBeardM, HairsAmount eBeardF, HairsType eHairsType, HairsColor[] aHairColors)
        {
            m_eHairsM = eHairsM;
            m_eHairsF = eHairsF;
            m_eBeardM = eBeardM;
            m_eBeardF = eBeardF;
            m_eHairsType = eHairsType;

            m_cHairColors = new List<HairsColor>(aHairColors);

            CheckHairColors();
        }
        
        public GenetixBase MutateRace()
        {
            CheckHairColors();

            if (Rnd.OneChanceFrom(2))
            {
                HairsGenetix pMutant = new HairsGenetix(this);

                //if (Rnd.OneChanceFrom(10))
                //    pMutant.m_eHairsType = (HairsType)Rnd.Get(typeof(HairsType));

                if (Rnd.OneChanceFrom(3))
                {
                    pMutant.m_eHairsM = MutateHairsAmount(pMutant.m_eHairsM);
                    if (Rnd.OneChanceFrom(2) || m_eHairsType == HairsType.Whiskers)
                        pMutant.m_eHairsF = MutateHairsAmount(pMutant.m_eHairsF);
                    else
                        pMutant.m_eHairsF = HairsAmount.Thick;

                    if (Rnd.OneChanceFrom(3) || m_eHairsType == HairsType.Whiskers)
                        pMutant.m_eBeardM = MutateHairsAmount(pMutant.m_eBeardM);
                    else
                        pMutant.m_eBeardM = pMutant.m_eHairsM;

//                    if (Rnd.OneChanceFrom(3) || m_eHairsType == HairsType.Whiskers)
                    if (m_eHairsType == HairsType.Whiskers)
                        pMutant.m_eBeardF = MutateHairsAmount(pMutant.m_eBeardF);
                    else
                        pMutant.m_eBeardF = HairsAmount.None;
                }

                if (pMutant.m_eBeardM == HairsAmount.None &&
                    pMutant.m_eBeardF == HairsAmount.None &&
                    pMutant.m_eHairsType == HairsType.Whiskers)
                    pMutant.m_eHairsType = HairsType.Hair;

                if (!pMutant.IsIdentical(this))
                {
                    int iCount = pMutant.m_cHairColors.Count;
                    for (int i = 0; i < iCount; i++)
                    {
                        int iChoice = Rnd.Get(3);
                        switch (iChoice)
                        {
                            case 0:
                                if (pMutant.m_cHairColors.Count > 1)
                                    pMutant.m_cHairColors.Remove(pMutant.m_cHairColors[Rnd.Get(pMutant.m_cHairColors.Count)]);
                                break;
                            case 1:
                                if (pMutant.m_cHairColors.Count < Enum.GetValues(typeof(HairsColor)).Length)
                                {
                                    HairsColor pColor;
                                    do
                                    {
                                        pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                                    }
                                    while (pMutant.m_cHairColors.Contains(pColor));
                                    pMutant.m_cHairColors[Rnd.Get(pMutant.m_cHairColors.Count)] = pColor;
                                }
                                break;
                            case 2:
                                if (pMutant.m_cHairColors.Count < Enum.GetValues(typeof(HairsColor)).Length)
                                {
                                    HairsColor pColor;
                                    do
                                    {
                                        pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                                    }
                                    while (pMutant.m_cHairColors.Contains(pColor));
                                    pMutant.m_cHairColors.Add(pColor);
                                }
                                break;
                        }
                    }

                    if (iCount == 0 &&
                        (pMutant.m_eHairsM != HairsAmount.None ||
                         pMutant.m_eHairsF != HairsAmount.None ||
                         pMutant.m_eBeardM != HairsAmount.None ||
                         pMutant.m_eBeardF != HairsAmount.None))
                    {
                        iCount = 1 + Rnd.Get(Enum.GetValues(typeof(HairsColor)).Length);
                        for (int i = 0; i < iCount; i++)
                        {
                            HairsColor pColor;
                            do
                            {
                                pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                            }
                            while (pMutant.m_cHairColors.Contains(pColor));
                            pMutant.m_cHairColors.Add(pColor);
                        }
                    }
                }

                if (pMutant.CheckHairColors())
                    return this;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            if (CheckHairColors())
                return this;

            return this;
        }

        private HairsAmount MutateHairsAmount(HairsAmount pOriginal)
        {
            switch (pOriginal)
            {
                case HairsAmount.None:
                    return Rnd.OneChanceFrom(2) ? HairsAmount.None : HairsAmount.Sparse;
                case HairsAmount.Sparse:
                    return Rnd.OneChanceFrom(2) ? HairsAmount.Sparse : (Rnd.OneChanceFrom(2) ? HairsAmount.None : HairsAmount.Thick);
                case HairsAmount.Thick:
                    return Rnd.OneChanceFrom(2) ? HairsAmount.Thick : HairsAmount.Sparse;
            }

            return (HairsAmount)Rnd.Get(typeof(HairsAmount));
        }

        public GenetixBase MutateNation()
        {
            CheckHairColors();

            if (Rnd.OneChanceFrom(10))
            {
                HairsGenetix pMutant = new HairsGenetix(this);

                pMutant.m_eHairsM = MutateHairsAmount(pMutant.m_eHairsM);
                pMutant.m_eHairsF = MutateHairsAmount(pMutant.m_eHairsF);

                if (Rnd.OneChanceFrom(3))
                    pMutant.m_eBeardM = MutateHairsAmount(pMutant.m_eBeardM);
                else
                    pMutant.m_eBeardM = pMutant.m_eHairsM;

//              if (Rnd.OneChanceFrom(3) || m_eHairsType == HairsType.Whiskers)
                if (m_eHairsType == HairsType.Whiskers)
                    pMutant.m_eBeardF = MutateHairsAmount(pMutant.m_eBeardF);
                else
                    pMutant.m_eBeardF = HairsAmount.None;

                if (pMutant.m_eBeardM == HairsAmount.None &&
                    pMutant.m_eBeardF == HairsAmount.None &&
                    pMutant.m_eHairsType == HairsType.Whiskers)
                    pMutant.m_eHairsType = HairsType.Hair;

                int iCount = pMutant.m_cHairColors.Count;
                for (int i = 0; i < iCount; i++)
                {
                    int iChoice = Rnd.Get(3);
                    switch (iChoice)
                    {
                        case 0:
                            if (pMutant.m_cHairColors.Count > 1)
                                pMutant.m_cHairColors.Remove(pMutant.m_cHairColors[Rnd.Get(pMutant.m_cHairColors.Count)]);
                            break;
                        case 1:
                            if (pMutant.m_cHairColors.Count < Enum.GetValues(typeof(HairsColor)).Length)
                            {
                                HairsColor pColor;
                                do
                                {
                                    pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                                }
                                while (pMutant.m_cHairColors.Contains(pColor));
                                pMutant.m_cHairColors[Rnd.Get(pMutant.m_cHairColors.Count)] = pColor;
                            }
                            break;
                        case 2:
                            if (pMutant.m_cHairColors.Count < Enum.GetValues(typeof(HairsColor)).Length)
                            {
                                HairsColor pColor;
                                do
                                {
                                    pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                                }
                                while (pMutant.m_cHairColors.Contains(pColor));
                                pMutant.m_cHairColors.Add(pColor);
                            }
                            break;
                    }
                }

                if (iCount == 0 &&
                    (pMutant.m_eHairsM != HairsAmount.None ||
                     pMutant.m_eHairsF != HairsAmount.None ||
                     pMutant.m_eBeardM != HairsAmount.None ||
                     pMutant.m_eBeardF != HairsAmount.None))
                {
                    iCount = 1 + Rnd.Get(Enum.GetValues(typeof(HairsColor)).Length);
                    for (int i = 0; i < iCount; i++)
                    {
                        HairsColor pColor;
                        do
                        {
                            pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                        }
                        while (pMutant.m_cHairColors.Contains(pColor));
                        pMutant.m_cHairColors.Add(pColor);
                    }
                }

                if (pMutant.CheckHairColors())
                    return this;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            if (CheckHairColors())
                return this;

            return this;
        }

        public GenetixBase MutateFamily()
        {
            CheckHairColors();

            if (Rnd.OneChanceFrom(3))
            {
                HairsGenetix pMutant = new HairsGenetix(this);

                pMutant.m_cHairColors.Clear();

                int iCount = m_cHairColors.Count;
                for (int i = 0; i < iCount; i++)
                {
                    HairsColor pColor;
                    do
                    {
                        pColor = m_cHairColors[Rnd.Get(m_cHairColors.Count)];
                    }
                    while (pMutant.m_cHairColors.Contains(pColor)); 
                    
                    if (Rnd.OneChanceFrom(pMutant.m_cHairColors.Count + 1))
                        pMutant.m_cHairColors.Add(pColor);
                }

                if (pMutant.m_cHairColors.Count == 0)
                    pMutant.m_cHairColors.Add(m_cHairColors[Rnd.Get(m_cHairColors.Count)]);

                pMutant.CheckHairColors();

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateIndividual()
        {
            CheckHairColors();

            HairsGenetix pMutant = new HairsGenetix(this);

            pMutant.m_cHairColors.Clear();
            if (m_cHairColors.Count == 0)
                CheckHairColors();
            pMutant.m_cHairColors.Add(m_cHairColors[Rnd.Get(m_cHairColors.Count)]);

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }
    }
}
