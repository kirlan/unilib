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

    public class HairsGenetix : IGenetix
    {
        /// <summary>
        /// Males usually are bald, but have dense beard and moustache, while females have long hairs and no beard or moustache. Most common colors are blue and green.
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            if (Hairs == HairsAmount.None &&
                 Beard == HairsAmount.None)
            {
                return "";
            }

            StringBuilder sColors = new StringBuilder();

            bool bColorsFull = false;
            for (int i = 0; i < HairColors.Count; i++)
            {
                if (bColorsFull)
                {
                    if (HairColors[i] != HairsColor.Hide)
                    {
                        if (i == HairColors.Count - 1)
                            sColors.Append(" or ");
                        else
                            sColors.Append(", ");
                    }
                }
                switch (HairColors[i])
                {
                    case HairsColor.Albino:
                        sColors.Append("snow-white");
                        bColorsFull = true;
                        break;
                    case HairsColor.Blonde:
                        sColors.Append("white");
                        bColorsFull = true;
                        break;
                    case HairsColor.Brunette:
                        sColors.Append("dark");
                        bColorsFull = true;
                        break;
                    case HairsColor.Black:
                        sColors.Append("black");
                        bColorsFull = true;
                        break;
                    case HairsColor.DarkBlond:
                        sColors.Append("gold");
                        bColorsFull = true;
                        break;
                    case HairsColor.Red:
                        sColors.Append("red");
                        bColorsFull = true;
                        break;
                    case HairsColor.Blue:
                        sColors.Append("blue");
                        bColorsFull = true;
                        break;
                    case HairsColor.Green:
                        sColors.Append("green");
                        bColorsFull = true;
                        break;
                }
            }
            if (!bColorsFull)
            {
                sColors.Clear();
            }
            else
            {
                if (HairColors.Count == 1)
                    sColors.Append(" of ").Append(sColors).Append(" color");
                else
                    sColors.Append(" of ").Append(sColors).Append(" colors");
            }

            string sHairs = "?";

            string sHair = "?";
            string sBeard = "?";

            switch (HairsType)
            {
                case HairsType.Whiskers:
                    {
                        switch (Hairs)
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

                        switch (Beard)
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

                        if (sHair?.Length == 0 && sBeard?.Length == 0)
                        {
                            sHairs = "";
                        }
                        else
                        {
                            sHairs = "have ";

                            if (sHair?.Length == 0)
                                sHairs += sBeard + sColors;
                            else if (sBeard?.Length == 0)
                                sHairs += sHair + sColors;
                            else
                                sHairs += sHair + sColors + " and " + sBeard;
                        }
                    }
                    break;
                case HairsType.Hair:
                    {
                        switch (Hairs)
                        {
                            case HairsAmount.None:
                                sHair = "";
                                break;
                            case HairsAmount.Sparse:
                                sHair = "thin hairs";
                                break;
                            case HairsAmount.Thick:
                                sHair = "thick hairs";
                                break;
                        }

                        switch (Beard)
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

                        if (sHair?.Length == 0)
                        {
                            if (Beard == HairsAmount.None)
                                sHairs = "are bald, and have " + sBeard;
                            else
                                sHairs = "are bald, but have " + sBeard + sColors;
                        }
                        else
                        {
                            if (Beard == HairsAmount.None)
                                sHairs += sHair + sColors + " and " + sBeard;
                            else
                                sHairs += sHair + " and " + sBeard + sColors;
                        }
                    }
                    break;
                case HairsType.Tentackles:
                    {
                        switch (Hairs)
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

                        switch (Beard)
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

                        if (sHair?.Length == 0 && sBeard?.Length == 0)
                        {
                            sHairs = "";
                        }
                        else
                        {
                            sHairs = "have ";

                            if (sHair?.Length == 0)
                                sHairs += sBeard + sColors;
                            else if (sBeard?.Length == 0)
                                sHairs += sHair + sColors;
                            else
                                sHairs += sHair + " and " + sBeard + sColors;
                        }
                    }
                    break;
            }

            return sHairs + ".";
        }

        /// <summary>
        /// He is bald, but has dense blue beard and moustache.
        /// </summary>
        /// <returns></returns>
        public string GetDescription(Gender eGender)
        {
            if (Hairs == HairsAmount.None &&
                 Beard == HairsAmount.None)
            {
                return eGender == Gender.Male ? "He is completely bald." : "She is completely bald.";
            }

            string sColor = "";
            switch (HairColors[0])
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

            switch (HairsType)
            {
                case HairsType.Whiskers:
                    {
                        switch (Hairs)
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
                        switch (Beard)
                        {
                            case HairsAmount.None:
                                sBeard = "";
                                break;
                            case HairsAmount.Sparse:
                                sBeard = "sparse " + (sHair?.Length == 0 ? sColor + " ":"") + "whiskers";
                                break;
                            case HairsAmount.Thick:
                                sBeard = "dense " + (sHair?.Length == 0 ? sColor + " " : "") + "whiskers";
                                break;
                        }

                        if (sHair?.Length == 0)
                            sResult += sBeard;
                        else if (sBeard?.Length == 0)
                            sResult += sHair;
                        else
                            sResult += sHair + " and " + sBeard;

                        if (sHair?.Length == 0 && sBeard?.Length == 0)
                            sResult = "";
                    }
                    break;
                case HairsType.Hair:
                    {
                        switch (Hairs)
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
                        switch (Beard)
                        {
                            case HairsAmount.None:
                                sBeard = "no beard or moustache";
                                break;
                            case HairsAmount.Sparse:
                                sBeard = "sparse " + (sHair?.Length == 0 ? sColor + " " : "") + "beard and moustache";
                                break;
                            case HairsAmount.Thick:
                                sBeard = "dense " + (sHair?.Length == 0 ? sColor + " " : "") + "beard and moustache";
                                break;
                        }

                        if (sHair?.Length == 0)
                        {
                            if (Beard == HairsAmount.None)
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
                        switch (Hairs)
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

                        switch (Beard)
                        {
                            case HairsAmount.None:
                                sBeard = "";
                                break;
                            case HairsAmount.Sparse:
                                sBeard = "several " + (sHair?.Length == 0 ? sColor + " " : "") + "tentacles around mouth";
                                break;
                            case HairsAmount.Thick:
                                sBeard = "a lot of " + (sHair?.Length == 0 ? sColor + " " : "") + "tentacles around mouth";
                                break;
                        }

                        if (sHair?.Length == 0)
                            sResult += sBeard;
                        else if (sBeard?.Length == 0)
                            sResult += sHair;
                        else
                            sResult += sHair + " and " + sBeard;

                        if (sHair?.Length == 0 && sBeard?.Length == 0)
                            sResult = "";
                    }
                    break;
            }

            if (!string.IsNullOrEmpty(sResult))
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

        public HairsAmount Hairs { get; private set; } = HairsAmount.Sparse;

        public HairsAmount Beard { get; private set; } = HairsAmount.Thick;

        public HairsType HairsType { get; private set; } = HairsType.Hair;

        public List<HairsColor> HairColors { get; } = new List<HairsColor>(new HairsColor[] { HairsColor.Brunette, HairsColor.Blonde, HairsColor.DarkBlond, HairsColor.Red });

        public bool IsIdentical(IGenetix pOther)
        {
            if (!(pOther is HairsGenetix pAnother))
                return false;

            if (pAnother.HairColors.Count != HairColors.Count)
            {
                return false;
            }
            else
            {
                foreach (HairsColor eColor in HairColors)
                {
                    if (!pAnother.HairColors.Contains(eColor))
                        return false;
                }
            }

            return Hairs == pAnother.Hairs &&
                Beard == pAnother.Beard &&
                HairsType == pAnother.HairsType;
        }

        public bool CheckHairColors()
        {
            if (HairColors.Count == 0 &&
                (Hairs != HairsAmount.None ||
                 Beard != HairsAmount.None))
            {
                return true;
            }

            List<HairsColor> cTest = new List<HairsColor>();
            foreach (HairsColor eColor in HairColors)
            {
                if (cTest.Contains(eColor))
                    return true;

                cTest.Add(eColor);
            }

            return false;
        }

        public HairsGenetix()
        { }

        public HairsGenetix(HairsGenetix pPredcessor)
        {
            Hairs = pPredcessor.Hairs;
            Beard = pPredcessor.Beard;
            HairsType = pPredcessor.HairsType;

            HairColors.Clear();
            HairColors.AddRange(pPredcessor.HairColors);

            CheckHairColors();
        }

        public HairsGenetix(HairsAmount eHairs, HairsAmount eBeard, HairsType eHairsType, HairsColor[] aHairColors)
        {
            Hairs = eHairs;
            Beard = eBeard;
            HairsType = eHairsType;

            HairColors = new List<HairsColor>(aHairColors);

            CheckHairColors();
        }

        public IGenetix MutateRace()
        {
            CheckHairColors();

            if (Rnd.OneChanceFrom(2))
            {
                HairsGenetix pMutant = new HairsGenetix(this);

                //if (Rnd.OneChanceFrom(10))
                //    pMutant.m_eHairsType = (HairsType)Rnd.Get(typeof(HairsType));

                if (Rnd.OneChanceFrom(3))
                {
                    pMutant.Hairs = MutateHairsAmount(pMutant.Hairs);
                    if (Rnd.OneChanceFrom(2) || HairsType == HairsType.Whiskers)
                        pMutant.Hairs = MutateHairsAmount(pMutant.Hairs);
                    else
                        pMutant.Hairs = HairsAmount.Thick;

                    if (Rnd.OneChanceFrom(3) || HairsType == HairsType.Whiskers)
                        pMutant.Beard = MutateHairsAmount(pMutant.Beard);
                    else
                        pMutant.Beard = pMutant.Hairs;
                }

                if (pMutant.Beard == HairsAmount.None &&
                    pMutant.HairsType == HairsType.Whiskers)
                {
                    pMutant.HairsType = HairsType.Hair;
                }

                if (!pMutant.IsIdentical(this))
                {
                    int iCount = pMutant.HairColors.Count;
                    for (int i = 0; i < iCount; i++)
                    {
                        int iChoice = Rnd.Get(3);
                        switch (iChoice)
                        {
                            case 0:
                                if (pMutant.HairColors.Count > 1)
                                    pMutant.HairColors.Remove(pMutant.HairColors[Rnd.Get(pMutant.HairColors.Count)]);
                                break;
                            case 1:
                                if (pMutant.HairColors.Count < Enum.GetValues(typeof(HairsColor)).Length)
                                {
                                    HairsColor pColor;
                                    do
                                    {
                                        pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                                    }
                                    while (pMutant.HairColors.Contains(pColor));
                                    pMutant.HairColors[Rnd.Get(pMutant.HairColors.Count)] = pColor;
                                }
                                break;
                            case 2:
                                if (pMutant.HairColors.Count < Enum.GetValues(typeof(HairsColor)).Length)
                                {
                                    HairsColor pColor;
                                    do
                                    {
                                        pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                                    }
                                    while (pMutant.HairColors.Contains(pColor));
                                    pMutant.HairColors.Add(pColor);
                                }
                                break;
                        }
                    }

                    if (iCount == 0 &&
                        (pMutant.Hairs != HairsAmount.None ||
                         pMutant.Beard != HairsAmount.None))
                    {
                        iCount = 1 + Rnd.Get(Enum.GetValues(typeof(HairsColor)).Length);
                        for (int i = 0; i < iCount; i++)
                        {
                            HairsColor pColor;
                            do
                            {
                                pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                            }
                            while (pMutant.HairColors.Contains(pColor));
                            pMutant.HairColors.Add(pColor);
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

        public IGenetix MutateGender()
        {
            CheckHairColors();

            if (Rnd.OneChanceFrom(10))
            {
                HairsGenetix pMutant = new HairsGenetix(this);

                pMutant.Hairs = MutateHairsAmount(pMutant.Hairs);

                var eOriginalBeard = pMutant.Beard;

                if (Rnd.OneChanceFrom(3))
                    pMutant.Beard = MutateHairsAmount(pMutant.Beard);
                else
                    pMutant.Beard = pMutant.Hairs;

                if (eOriginalBeard < pMutant.Beard && !Rnd.OneChanceFrom(10))
                    pMutant.Beard = eOriginalBeard;

                if (pMutant.Beard == HairsAmount.None &&
                    pMutant.HairsType == HairsType.Whiskers)
                {
                    pMutant.HairsType = HairsType.Hair;
                }

                int iCount = pMutant.HairColors.Count;
                for (int i = 0; i < iCount; i++)
                {
                    int iChoice = Rnd.Get(3);
                    switch (iChoice)
                    {
                        case 0:
                            if (pMutant.HairColors.Count > 1)
                                pMutant.HairColors.Remove(pMutant.HairColors[Rnd.Get(pMutant.HairColors.Count)]);
                            break;
                        case 1:
                            if (pMutant.HairColors.Count < Enum.GetValues(typeof(HairsColor)).Length)
                            {
                                HairsColor pColor;
                                do
                                {
                                    pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                                }
                                while (pMutant.HairColors.Contains(pColor));
                                pMutant.HairColors[Rnd.Get(pMutant.HairColors.Count)] = pColor;
                            }
                            break;
                        case 2:
                            if (pMutant.HairColors.Count < Enum.GetValues(typeof(HairsColor)).Length)
                            {
                                HairsColor pColor;
                                do
                                {
                                    pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                                }
                                while (pMutant.HairColors.Contains(pColor));
                                pMutant.HairColors.Add(pColor);
                            }
                            break;
                    }
                }

                if (iCount == 0 &&
                    (pMutant.Hairs != HairsAmount.None ||
                     pMutant.Beard != HairsAmount.None))
                {
                    iCount = 1 + Rnd.Get(Enum.GetValues(typeof(HairsColor)).Length);
                    for (int i = 0; i < iCount; i++)
                    {
                        HairsColor pColor;
                        do
                        {
                            pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                        }
                        while (pMutant.HairColors.Contains(pColor));
                        pMutant.HairColors.Add(pColor);
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

        public IGenetix MutateNation()
        {
            CheckHairColors();

            if (Rnd.OneChanceFrom(10))
            {
                HairsGenetix pMutant = new HairsGenetix(this);

                pMutant.Hairs = MutateHairsAmount(pMutant.Hairs);

                if (Rnd.OneChanceFrom(3))
                    pMutant.Beard = MutateHairsAmount(pMutant.Beard);
                else
                    pMutant.Beard = pMutant.Hairs;

                if (pMutant.Beard == HairsAmount.None &&
                    pMutant.HairsType == HairsType.Whiskers)
                {
                    pMutant.HairsType = HairsType.Hair;
                }

                int iCount = pMutant.HairColors.Count;
                for (int i = 0; i < iCount; i++)
                {
                    int iChoice = Rnd.Get(3);
                    switch (iChoice)
                    {
                        case 0:
                            if (pMutant.HairColors.Count > 1)
                                pMutant.HairColors.Remove(pMutant.HairColors[Rnd.Get(pMutant.HairColors.Count)]);
                            break;
                        case 1:
                            if (pMutant.HairColors.Count < Enum.GetValues(typeof(HairsColor)).Length)
                            {
                                HairsColor pColor;
                                do
                                {
                                    pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                                }
                                while (pMutant.HairColors.Contains(pColor));
                                pMutant.HairColors[Rnd.Get(pMutant.HairColors.Count)] = pColor;
                            }
                            break;
                        case 2:
                            if (pMutant.HairColors.Count < Enum.GetValues(typeof(HairsColor)).Length)
                            {
                                HairsColor pColor;
                                do
                                {
                                    pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                                }
                                while (pMutant.HairColors.Contains(pColor));
                                pMutant.HairColors.Add(pColor);
                            }
                            break;
                    }
                }

                if (iCount == 0 &&
                    (pMutant.Hairs != HairsAmount.None ||
                     pMutant.Beard != HairsAmount.None))
                {
                    iCount = 1 + Rnd.Get(Enum.GetValues(typeof(HairsColor)).Length);
                    for (int i = 0; i < iCount; i++)
                    {
                        HairsColor pColor;
                        do
                        {
                            pColor = (HairsColor)Rnd.Get(typeof(HairsColor));
                        }
                        while (pMutant.HairColors.Contains(pColor));
                        pMutant.HairColors.Add(pColor);
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

        public IGenetix MutateFamily()
        {
            CheckHairColors();

            if (Rnd.OneChanceFrom(3))
            {
                HairsGenetix pMutant = new HairsGenetix(this);

                pMutant.HairColors.Clear();

                int iCount = HairColors.Count;
                for (int i = 0; i < iCount; i++)
                {
                    HairsColor pColor;
                    do
                    {
                        pColor = HairColors[Rnd.Get(HairColors.Count)];
                    }
                    while (pMutant.HairColors.Contains(pColor));

                    if (Rnd.OneChanceFrom(pMutant.HairColors.Count + 1))
                        pMutant.HairColors.Add(pColor);
                }

                if (pMutant.HairColors.Count == 0)
                    pMutant.HairColors.Add(HairColors[Rnd.Get(HairColors.Count)]);

                pMutant.CheckHairColors();

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public IGenetix MutateIndividual()
        {
            CheckHairColors();

            HairsGenetix pMutant = new HairsGenetix(this);

            pMutant.HairColors.Clear();
            if (HairColors.Count == 0)
                CheckHairColors();
            pMutant.HairColors.Add(HairColors[Rnd.Get(HairColors.Count)]);

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }
    }
}
