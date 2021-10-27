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
            if (m_eHairs == HairsAmount.None &&
                 m_eBeard == HairsAmount.None)
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

            string sHairs = "?";

            string sHair = "?";
            string sBeard = "?";

            switch (m_eHairsType)
            {
                case HairsType.Whiskers:
                    {
                        switch (m_eHairs)
                        {
                            case HairsAmount.None:
                                sHair = "";
                                break;
                            case HairsAmount.Sparse:
                                sHair = "sparse mane";
                                break;
                            case HairsAmount.Thick:
                                sHair = "dense mane";
                                break;
                        }

                        switch (m_eBeard)
                        {
                            case HairsAmount.None:
                                sBeard = "";
                                break;
                            case HairsAmount.Sparse:
                                sBeard = "sparse whiskers";
                                break;
                            case HairsAmount.Thick:
                                sBeard = "dense whiskers";
                                break;
                        }

                        sHairs = "have ";

                        if (sHair == "")
                            sHairs += sBeard;
                        else
                            if (sBeard == "")
                                sHairs += sHair;
                            else
                                sHairs += sHair + " and " + sBeard;

                        if (sHairs == "have ")
                            sHairs = "";
                    }
                    break;
                case HairsType.Hair:
                    {
                        switch (m_eHairs)
                        {
                            case HairsAmount.None:
                                sHair = "";
                                break;
                            case HairsAmount.Sparse:
                                sHair = "short hairs";
                                break;
                            case HairsAmount.Thick:
                                sHair = "long hairs";
                                break;
                        }

                        switch (m_eBeard)
                        {
                            case HairsAmount.None:
                                sBeard = "no beard or moustache";
                                break;
                            case HairsAmount.Sparse:
                                sBeard = "sparse beard and moustache";
                                break;
                            case HairsAmount.Thick:
                                sBeard = "dense beard and moustache";
                                break;
                        }

                        sHairs = "have ";

                        if (sHair == "")
                        {
                            if (m_eBeard == HairsAmount.None)
                                sHairs = "are bald, and have " + sBeard;
                            else
                                sHairs = "are bald, but have " + sBeard;
                        }
                        else
                        {
                            sHairs += sHair + " and " + sBeard;
                        }
                    }
                    break;
                case HairsType.Tentackles:
                    {
                        switch (m_eHairs)
                        {
                            case HairsAmount.None:
                                sHair = "";
                                break;
                            case HairsAmount.Sparse:
                                sHair = "several tentacles on head instead of hairs";
                                break;
                            case HairsAmount.Thick:
                                sHair = "a lot of tentacles on head instead of hairs";
                                break;
                        }

                        switch (m_eBeard)
                        {
                            case HairsAmount.None:
                                sBeard = "";
                                break;
                            case HairsAmount.Sparse:
                                sBeard = "several tentacles around mouth";
                                break;
                            case HairsAmount.Thick:
                                sBeard = "a lot of tentacles around mouth";
                                break;
                        }

                        sHairs = "have ";

                        if (sHair == "")
                            sHairs += sBeard;
                        else
                            if (sBeard == "")
                                sHairs += sHair;
                            else
                                sHairs += sHair + " and " + sBeard;

                        if (sHairs == "have ")
                            sHairs = "";
                    }
                    break;
            }

            return "They " + sHairs + "." + sColors;
        }

        /// <summary>
        /// He is bald, but has dense blue beard and moustache.
        /// </summary>
        /// <returns></returns>
        public string GetDescription(Gender eGender)
        {
            if (m_eHairs == HairsAmount.None &&
                 m_eBeard == HairsAmount.None)
                return eGender == Gender.Male ? "He is completely bald." : "She is completely bald.";

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

            string sResult = eGender == Gender.Male ? "He has " : "She has ";

            string sHair = "?";
            string sBeard = "?";

            switch (m_eHairsType)
            {
                case HairsType.Whiskers:
                    {
                        switch (m_eHairs)
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
                        switch (m_eBeard)
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
                        switch (m_eHairs)
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
                        switch (m_eBeard)
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
                            if (m_eBeard == HairsAmount.None)
                                sResult = (eGender == Gender.Male ? "He" : "She") + " is bald, and has " + sBeard;
                            else
                                sResult = (eGender == Gender.Male ? "He" : "She") + " is bald, but has " + sColor + " " + sBeard;
                        }
                        else
                        {
                            sResult += sHair + " and " + sBeard;
                        }
                    }
                    break;
                case HairsType.Tentackles:
                    {
                        switch (m_eHairs)
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

                        switch (m_eBeard)
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

        /// <summary>
        /// thick hair, thick beard, natural colors
        /// </summary>
        public static HairsGenetix HumanWhiteM
        {
            get { return new HairsGenetix(HairsAmount.Thick, HairsAmount.Thick, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.Brunette, HairsColor.DarkBlond, HairsColor.Red, HairsColor.Black }); }
        }

        /// <summary>
        /// thick hair, no beard, natural colors
        /// </summary>
        public static HairsGenetix HumanWhiteF
        {
            get { return new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.Brunette, HairsColor.DarkBlond, HairsColor.Red, HairsColor.Black }); }
        }

        /// <summary>
        /// thick hair, sparse beard, dark colors
        /// </summary>
        public static HairsGenetix HumanBlackM
        {
            get { return new HairsGenetix(HairsAmount.Thick, HairsAmount.Sparse, HairsType.Hair, new HairsColor[] { HairsColor.Brunette }); }
        }

        /// <summary>
        /// thick hair, no beard, dark colors
        /// </summary>
        public static HairsGenetix HumanBlackF
        {
            get { return new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Brunette }); }
        }

        /// <summary>
        /// thick hair, no beard, natural colors except red
        /// </summary>
        public static HairsGenetix Elf
        {
            get { return new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.Brunette, HairsColor.DarkBlond, HairsColor.Black }); }
        }

        /// <summary>
        /// thick hair, no beard, light colors
        /// </summary>
        public static HairsGenetix Drow
        {
            get { return new HairsGenetix(HairsAmount.Thick, HairsAmount.None, HairsType.Hair, new HairsColor[] { HairsColor.Blonde, HairsColor.DarkBlond }); }
        }

        /// <summary>
        /// tentacles everywhere
        /// </summary>
        public static HairsGenetix Ktulhu
        {
            get { return new HairsGenetix(HairsAmount.Sparse, HairsAmount.Sparse, HairsType.Tentackles, new HairsColor[] { HairsColor.Hide }); }
        }

        /// <summary>
        /// no hair, no beard
        /// </summary>
        public static HairsGenetix None
        {
            get { return new HairsGenetix(HairsAmount.None, HairsAmount.None, HairsType.Hair, new HairsColor[] { }); }
        }

        /// <summary>
        /// no hair, sparse whiskers
        /// </summary>
        public static HairsGenetix AnimalWhiskers
        {
            get { return new HairsGenetix(HairsAmount.None, HairsAmount.Sparse, HairsType.Whiskers, new HairsColor[] { HairsColor.Hide }); }
        }

        public HairsAmount m_eHairs = HairsAmount.Sparse;

        public HairsAmount m_eBeard = HairsAmount.Thick;

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

            return m_eHairs == pAnother.m_eHairs &&
                m_eBeard == pAnother.m_eBeard &&
                m_eHairsType == pAnother.m_eHairsType;
        }

        public HairsGenetix()
        { }

        public bool CheckHairColors()
        {
            if (m_cHairColors.Count == 0 &&
                (m_eHairs != HairsAmount.None ||
                 m_eBeard != HairsAmount.None))
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
            m_eHairs = pPredcessor.m_eHairs;
            m_eBeard = pPredcessor.m_eBeard;
            m_eHairsType = pPredcessor.m_eHairsType;

            m_cHairColors.Clear();
            m_cHairColors.AddRange(pPredcessor.m_cHairColors);

            CheckHairColors();
        }

        public HairsGenetix(HairsAmount eHairs, HairsAmount eBeard, HairsType eHairsType, HairsColor[] aHairColors)
        {
            m_eHairs = eHairs;
            m_eBeard = eBeard;
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
                    pMutant.m_eHairs = MutateHairsAmount(pMutant.m_eHairs);
                    if (Rnd.OneChanceFrom(2) || m_eHairsType == HairsType.Whiskers)
                        pMutant.m_eHairs = MutateHairsAmount(pMutant.m_eHairs);
                    else
                        pMutant.m_eHairs = HairsAmount.Thick;

                    if (Rnd.OneChanceFrom(3) || m_eHairsType == HairsType.Whiskers)
                        pMutant.m_eBeard = MutateHairsAmount(pMutant.m_eBeard);
                    else
                        pMutant.m_eBeard = pMutant.m_eHairs;
                }

                if (pMutant.m_eBeard == HairsAmount.None &&
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
                        (pMutant.m_eHairs != HairsAmount.None ||
                         pMutant.m_eBeard != HairsAmount.None))
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

        public GenetixBase MutateGender()
        {
            CheckHairColors();

            if (Rnd.OneChanceFrom(10))
            {
                HairsGenetix pMutant = new HairsGenetix(this);

                pMutant.m_eHairs = MutateHairsAmount(pMutant.m_eHairs);

                var eOriginalBeard = pMutant.m_eBeard;

                if (Rnd.OneChanceFrom(3))
                    pMutant.m_eBeard = MutateHairsAmount(pMutant.m_eBeard);
                else
                    pMutant.m_eBeard = pMutant.m_eHairs;

                if (eOriginalBeard < pMutant.m_eBeard && !Rnd.OneChanceFrom(10))
                    pMutant.m_eBeard = eOriginalBeard;

                if (pMutant.m_eBeard == HairsAmount.None &&
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
                    (pMutant.m_eHairs != HairsAmount.None ||
                     pMutant.m_eBeard != HairsAmount.None))
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

        public GenetixBase MutateNation()
        {
            CheckHairColors();

            if (Rnd.OneChanceFrom(10))
            {
                HairsGenetix pMutant = new HairsGenetix(this);

                pMutant.m_eHairs = MutateHairsAmount(pMutant.m_eHairs);

                if (Rnd.OneChanceFrom(3))
                    pMutant.m_eBeard = MutateHairsAmount(pMutant.m_eBeard);
                else
                    pMutant.m_eBeard = pMutant.m_eHairs;


                if (pMutant.m_eBeard == HairsAmount.None &&
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
                    (pMutant.m_eHairs != HairsAmount.None ||
                     pMutant.m_eBeard != HairsAmount.None))
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
