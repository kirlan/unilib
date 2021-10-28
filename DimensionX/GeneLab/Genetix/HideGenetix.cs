﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using System.Drawing;
using nsUniLibControls;

namespace GeneLab.Genetix
{
    public enum HideType
    { 
        /// <summary>
        /// просто кожа
        /// </summary>
        BareSkin,
        /// <summary>
        /// короткий мех
        /// </summary>
        FurShort,
        /// <summary>
        /// длинный мех
        /// </summary>
        FurLong,
        /// <summary>
        /// перья
        /// </summary>
        Feathers,
        /// <summary>
        /// хитиновый панцирь
        /// </summary>
        Chitin,
        /// <summary>
        /// чешуя
        /// </summary>
        Scales,
        /// <summary>
        /// костяной панцирь
        /// </summary>
        Shell
    }

    public class HideGenetix: GenetixBase
    {
        /// <summary>
        /// jet-black fur with azure spots
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string sHide = "?";
            switch (HideType)
            {
                case HideType.BareSkin:
                    sHide = "skin";
                    break;
                case HideType.FurShort:
                    sHide = "fur";
                    break;
                case HideType.FurLong:
                    sHide = "long hairs";
                    break;
                case HideType.Feathers:
                    sHide = "feathers";
                    break;
                case HideType.Scales:
                    sHide = "scales";
                    break;
                case HideType.Chitin:
                    sHide = "chitin shell";
                    break;
                case HideType.Shell:
                    sHide = "bone shell";
                    break;
            }

            return HideColorStr + " " + sHide + (Spots ? (" with " + SpotsColorStr + " spots") : "");
        }
        
        /// <summary>
        /// pink skin
        /// </summary>
        public static HideGenetix HumanWhite
        {
            get { return new HideGenetix(HideType.BareSkin, Color.Moccasin); }
        }

        /// <summary>
        /// yellow skin
        /// </summary>
        public static HideGenetix HumanYellow
        {
            get { return new HideGenetix(HideType.BareSkin, Color.Yellow); }
        }

        /// <summary>
        /// red skin
        /// </summary>
        public static HideGenetix HumanRed
        {
            get { return new HideGenetix(HideType.BareSkin, Color.Red); }
        }

        /// <summary>
        /// black skin
        /// </summary>
        public static HideGenetix HumanBlack
        {
            get { return new HideGenetix(HideType.BareSkin, Color.FromArgb(0x40, 0x20 ,0)); }
        }

        /// <summary>
        /// brown skin
        /// </summary>
        public static HideGenetix HumanTan
        {
            get { return new HideGenetix(HideType.BareSkin, Color.FromArgb(0x80, 0x60, 0x40)); }
        }

        /// <summary>
        /// show-white skin
        /// </summary>
        public static HideGenetix Vampire
        {
            get { return new HideGenetix(HideType.BareSkin, Color.White); }
        }

        /// <summary>
        /// green skin
        /// </summary>
        public static HideGenetix Orc
        {
            get { return new HideGenetix(HideType.BareSkin, Color.Green); }
        }

        /// <summary>
        /// blue skin
        /// </summary>
        public static HideGenetix Navi
        {
            get { return new HideGenetix(HideType.BareSkin, Color.Blue); }
        }

        /// <summary>
        /// absolutely black skin
        /// </summary>
        public static HideGenetix Drow
        {
            get { return new HideGenetix(HideType.BareSkin, Color.Black); }
        }

        /// <summary>
        /// black chitin with blue spots
        /// </summary>
        public static HideGenetix Insect
        {
            get { return new HideGenetix(HideType.Chitin, Color.Black, Color.Blue); }
        }

        /// <summary>
        /// yellow scales with green spots
        /// </summary>
        public static HideGenetix Reptile
        {
            get { return new HideGenetix(HideType.Scales, Color.Yellow, Color.Green); }
        }

        /// <summary>
        /// brown feathers with gray spots
        /// </summary>
        public static HideGenetix Bird
        {
            get { return new HideGenetix(HideType.Feathers, Color.Brown, Color.Gray); }
        }

        /// <summary>
        /// brown fur with black spots
        /// </summary>
        public static HideGenetix Beast
        {
            get { return new HideGenetix(HideType.FurShort, Color.Brown, Color.Black); }
        }



        public HideType HideType { get; private set; } = HideType.BareSkin;

        public Color HideColor { get; private set; } = Color.AntiqueWhite;
        public string HideColorStr { get; private set; } = "white";

        public bool Spots { get; private set; } = false;

        public Color SpotsColor { get; private set; } = Color.AntiqueWhite;
        public string SpotsColorStr { get; private set; } = "white";

        public bool IsIdentical(GenetixBase pOther)
        {
            HideGenetix pAnother = pOther as HideGenetix;

            if (pAnother == null)
                return false;

            if (!Spots && !pAnother.Spots)
                return HideType == pAnother.HideType &&
                HideColorStr == pAnother.HideColorStr;

            return HideType == pAnother.HideType &&
                HideColorStr == pAnother.HideColorStr &&
                Spots == pAnother.Spots &&
                SpotsColorStr == pAnother.SpotsColorStr;
        }
        
        public HideGenetix()
        { }

        public HideGenetix(HideGenetix pPredcessor)
        {
            HideType = pPredcessor.HideType;
            HideColor = pPredcessor.HideColor;
            Spots = pPredcessor.Spots;
            SpotsColor = pPredcessor.SpotsColor;

            HideColorStr = GetPredefinedColor(HideColor);
            SpotsColorStr = GetPredefinedColor(SpotsColor);
        }

        public HideGenetix(HideType eHideType, Color eHideColor)
        {
            HideType = eHideType;
            HideColor = eHideColor;
            Spots = false;

            HideColorStr = GetPredefinedColor(eHideColor);
        }

        public HideGenetix(HideType eHideType, Color eHideColor, Color eSpotsColor)
        {
            HideType = eHideType;
            HideColor = eHideColor;
            Spots = true;
            SpotsColor = eSpotsColor;

            HideColorStr = GetPredefinedColor(eHideColor);
            SpotsColorStr = GetPredefinedColor(eSpotsColor);
        }

        private void MutateHideType()
        {
            int iChance = 0;
            switch (HideType)
            {
                case HideType.BareSkin:
                    {
                        int[] aChances = new int[] { 8, 2, 0, 0, 0, 0, 0 };
                        iChance = Rnd.ChooseOne(aChances, 1);
                    }
                    break;
                case HideType.FurShort:
                    {
                        int[] aChances = new int[] { 2, 8, 8, 0, 0, 1, 0 };
                        iChance = Rnd.ChooseOne(aChances, 1);
                    }
                    break;
                case HideType.FurLong:
                    {
                        int[] aChances = new int[] { 2, 8, 8, 0, 0, 1, 0 };
                        iChance = Rnd.ChooseOne(aChances, 1);
                    }
                    break;
                case HideType.Feathers:
                    {
                        int[] aChances = new int[] { 0, 2, 0, 8, 0, 0, 0 };
                        iChance = Rnd.ChooseOne(aChances, 1);
                    }
                    break;
                case HideType.Chitin:
                    {
                        int[] aChances = new int[] { 0, 0, 0, 0, 8, 0, 2 };
                        iChance = Rnd.ChooseOne(aChances, 1);
                    }
                    break;
                case HideType.Scales:
                    {
                        int[] aChances = new int[] { 2, 0, 0, 0, 0, 8, 4 };
                        iChance = Rnd.ChooseOne(aChances, 1);
                    }
                    break;
                case HideType.Shell:
                    {
                        int[] aChances = new int[] { 1, 0, 0, 0, 2, 4, 8 };
                        iChance = Rnd.ChooseOne(aChances, 1);
                    }
                    break;
            }
            switch (iChance)
            {
                case 0:
                    HideType = HideType.BareSkin;
                    break;
                case 1:
                    HideType = HideType.FurShort;
                    break;
                case 2:
                    HideType = HideType.FurLong;
                    break;
                case 3:
                    HideType = HideType.Feathers;
                    break;
                case 4:
                    HideType = HideType.Chitin;
                    break;
                case 5:
                    HideType = HideType.Scales;
                    break;
                case 6:
                    HideType = HideType.Shell;
                    break;
            }
        }

        private void MutateHideColor()
        {
            if (HideType == HideType.BareSkin)
            {
                Spots = false;

                KColor pColor = new KColor();
                pColor.RGB = HideColor;

                if (Rnd.OneChanceFrom(2))
                    pColor.Hue += Math.Pow(Rnd.Get(8f), 2);
                else
                    pColor.Hue -= Math.Pow(Rnd.Get(8f), 2);

                if (Rnd.OneChanceFrom(2))
                    pColor.Saturation += Math.Pow(Rnd.Get(1f), 2);
                else
                    pColor.Saturation -= Math.Pow(Rnd.Get(1f), 2);

                double fDelta = Math.Pow(Rnd.Get(0.5f), 3);
                if (Rnd.OneChanceFrom(2))
                    pColor.Lightness += fDelta;
                else
                    pColor.Lightness -= fDelta;

                HideColor = pColor.RGB;
                HideColorStr = GetPredefinedColor(pColor);
            }
            else
            {
                MutateHideColorAnimalsOnly();
            }
        }

        private void MutateHideColorAnimalsOnly()
        {
            if (HideType != HideType.BareSkin)
            {
                Spots = Rnd.OneChanceFrom(2);

                KColor pColor = new KColor();
                pColor.RGB = HideColor;

                if (HideType > HideType.FurLong)
                {
                    pColor.Hue = Rnd.Get(360);
                }
                else
                {
                    if (Rnd.OneChanceFrom(2))
                        pColor.Hue += Math.Pow(Rnd.Get(13f), 2);
                    else
                        pColor.Hue -= Math.Pow(Rnd.Get(13f), 2);
                }

                if (Rnd.OneChanceFrom(2))
                    pColor.Saturation += Math.Pow(Rnd.Get(1f), 2);
                else
                    pColor.Saturation -= Math.Pow(Rnd.Get(1f), 2);

                if (Rnd.OneChanceFrom(2))
                    pColor.Lightness += Math.Pow(Rnd.Get(1f), 2);
                else
                    pColor.Lightness -= Math.Pow(Rnd.Get(1f), 2);

                HideColor = pColor.RGB;
                HideColorStr = GetPredefinedColor(pColor);

                if (Spots)
                {
                    do
                    {
                        pColor.RGB = SpotsColor;

                        if (HideType > HideType.FurLong)
                        {
                            pColor.Hue = Rnd.Get(360);
                        }
                        else
                        {
                            if (Rnd.OneChanceFrom(2))
                                pColor.Hue += Math.Pow(Rnd.Get(13f), 2);
                            else
                                pColor.Hue -= Math.Pow(Rnd.Get(13f), 2);
                        }

                        pColor.Saturation = Rnd.Get(1f);
                        pColor.Lightness = Rnd.Get(1f);

                        SpotsColor = pColor.RGB;
                        SpotsColorStr = GetPredefinedColor(pColor);
                    }
                    while (SpotsColorStr == HideColorStr);
                }
            }
        }

        private string GetPredefinedColor(Color eColor)
        {
            KColor pColor = new KColor();
            pColor.RGB = eColor;

            return GetPredefinedColor(pColor);
        }

        private string GetPredefinedColor(KColor pColor)
        {
            if (pColor.Lightness > 0.875)
                return "pale";

            if (pColor.Lightness < 0.125)
                return "jet-black";

            if (pColor.Saturation < 0.25)
                return "gray";

            if (pColor.Hue < 15)
            {
                if (pColor.Lightness < 0.25)
                    return "dark red";
                if (pColor.Lightness > 0.75)
                    return "pink";
                if (pColor.Saturation > 0.75)
                    return "bright red";
                return "red";
            }
            else if (pColor.Hue < 45)
            {
                if (pColor.Lightness < 0.25)
                    return "black";
                if (pColor.Lightness > 0.75)
                    return "white";
                if (pColor.Saturation > 0.75)
                    return "bright orange"; 
                return "tanned";
            }
            else if (pColor.Hue < 75)
            {
                if (pColor.Lightness < 0.25)
                    return "golden";
                if (pColor.Lightness > 0.75)
                    return "light yellow";
                if (pColor.Saturation > 0.75)
                    return "bright yellow";
                return "yellow";
            }
            else if (pColor.Hue < 105)
            {
                if (pColor.Lightness < 0.25)
                    return "dark green";
                if (pColor.Lightness > 0.75)
                    return "light green";
                if (pColor.Saturation > 0.75)
                    return "bright lime";
                return "lime";
            }
            else if (pColor.Hue < 135)
            {
                if (pColor.Lightness < 0.25)
                    return "dark green";
                if (pColor.Lightness > 0.75)
                    return "light green";
                if (pColor.Saturation > 0.75)
                    return "bright green";
                return "green";
            }
            else if (pColor.Hue < 165)
            {
                if (pColor.Lightness < 0.25)
                    return "dark green";
                if (pColor.Lightness > 0.75)
                    return "azure";
                if (pColor.Saturation > 0.75)
                    return "aquamarine";
                return "turquoise";
            }
            else if (pColor.Hue < 195)
            {
                if (pColor.Lightness < 0.25)
                    return "dark cyan";
                if (pColor.Lightness > 0.75)
                    return "azure";
                if (pColor.Saturation > 0.75)
                    return "bright cyan";
                return "cyan";
            }
            else if (pColor.Hue < 225)
            {
                if (pColor.Lightness < 0.25)
                    return "dark blue";
                if (pColor.Lightness > 0.75)
                    return "light blue";
                if (pColor.Saturation > 0.75)
                    return "bright blue";
                return "blue";
            }
            else if (pColor.Hue < 255)
            {
                if (pColor.Lightness < 0.25)
                    return "dark blue";
                if (pColor.Lightness > 0.75)
                    return "light blue";
                if (pColor.Saturation > 0.75)
                    return "bright blue";
                return "blue";
            }
            else if (pColor.Hue < 285)
            {
                if (pColor.Lightness < 0.25)
                    return "dark violet";
                if (pColor.Lightness > 0.75)
                    return "light violet";
                if (pColor.Saturation > 0.75)
                    return "bright violet";
                return "violet"; 
            }
            else if (pColor.Hue < 315)
            {
                if (pColor.Lightness < 0.25)
                    return "purple";
                if (pColor.Lightness > 0.75)
                    return "pink";
                if (pColor.Saturation > 0.75)
                    return "bright magenta";
                return "magenta";
            }
            else if (pColor.Hue < 345)
            {
                if (pColor.Lightness < 0.25)
                    return "plum";
                if (pColor.Lightness > 0.75)
                    return "pink";
                if (pColor.Saturation > 0.75)
                    return "bright pink";
                return "crimson";
            }
            else
            {
                if (pColor.Lightness < 0.25)
                    return "dark red";
                if (pColor.Lightness > 0.75)
                    return "pink";
                if (pColor.Saturation > 0.75)
                    return "bright red";
                return "red";
            }
        }

        public GenetixBase MutateRace()
        {
            HideGenetix pMutant = new HideGenetix(this);

            if (Rnd.OneChanceFrom(20))
                pMutant.MutateHideType();
                
            if (HideType != HideType.BareSkin || Rnd.OneChanceFrom(10))
                pMutant.MutateHideColor();

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        public GenetixBase MutateGender()
        {
            if (Rnd.OneChanceFrom(5))
            {
                HideGenetix pMutant = new HideGenetix(this);

                switch (pMutant.HideType)
                {
                    case HideType.FurShort:
                        {
                            if (Rnd.OneChanceFrom(10))
                                pMutant.HideType = Rnd.OneChanceFrom(4) ? HideType.BareSkin : HideType.FurLong;
                        }
                        break;
                    case HideType.FurLong:
                        {
                            if (Rnd.OneChanceFrom(2))
                                pMutant.HideType = HideType.FurShort;
                        }
                        break;
                }

                if (Rnd.OneChanceFrom(10))
                    pMutant.MutateHideColor();

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateNation()
        {
            if (Rnd.OneChanceFrom(5))
            {
                HideGenetix pMutant = new HideGenetix(this);

                switch (pMutant.HideType)
                {
                    case HideType.FurShort:
                        {
                            if (Rnd.OneChanceFrom(10))
                                pMutant.HideType = Rnd.OneChanceFrom(4) ? HideType.BareSkin : HideType.FurLong;
                        }
                        break;
                    case HideType.FurLong:
                        {
                            if (Rnd.OneChanceFrom(2))
                                pMutant.HideType = HideType.FurShort;
                        }
                        break;
                }

                pMutant.MutateHideColor();

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateFamily()
        {
            if (HideType != HideType.BareSkin && Rnd.OneChanceFrom(5))
            {
                HideGenetix pMutant = new HideGenetix(this);

                pMutant.MutateHideColorAnimalsOnly();

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateIndividual()
        {
            if (HideType != HideType.BareSkin && Rnd.OneChanceFrom(5))
            {
                HideGenetix pMutant = new HideGenetix(this);

                pMutant.MutateHideColorAnimalsOnly();

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }
    }
}
