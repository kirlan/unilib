using System;
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
        /// covered with jet-black and azure fur
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string sHide = "?";
            switch (m_eHideType)
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

            return "covered with " + m_sHideColor + (m_bSpots ? (" and " + m_sSpotsColor) : "") + " " + sHide;
        }
        
        public static HideGenetix HumanWhite
        {
            get { return new HideGenetix(HideType.BareSkin, Color.Moccasin); }
        }

        public static HideGenetix HumanYellow
        {
            get { return new HideGenetix(HideType.BareSkin, Color.Yellow); }
        }

        public static HideGenetix HumanRed
        {
            get { return new HideGenetix(HideType.BareSkin, Color.Red); }
        }

        public static HideGenetix HumanBlack
        {
            get { return new HideGenetix(HideType.BareSkin, Color.FromArgb(0x40, 0x20 ,0)); }
        }

        public static HideGenetix HumanTan
        {
            get { return new HideGenetix(HideType.BareSkin, Color.FromArgb(0x80, 0x60, 0x40)); }
        }

        public static HideGenetix Vampire
        {
            get { return new HideGenetix(HideType.BareSkin, Color.White); }
        }

        public static HideGenetix Orc
        {
            get { return new HideGenetix(HideType.BareSkin, Color.Green); }
        }

        public static HideGenetix Navi
        {
            get { return new HideGenetix(HideType.BareSkin, Color.Blue); }
        }

        public static HideGenetix Drow
        {
            get { return new HideGenetix(HideType.BareSkin, Color.Black); }
        }

        public static HideGenetix Insect
        {
            get { return new HideGenetix(HideType.Chitin, Color.Black, Color.Blue); }
        }

        public static HideGenetix Reptile
        {
            get { return new HideGenetix(HideType.Scales, Color.Yellow, Color.Green); }
        }

        public static HideGenetix Bird
        {
            get { return new HideGenetix(HideType.Feathers, Color.Brown, Color.Gray); }
        }

        public static HideGenetix Beast
        {
            get { return new HideGenetix(HideType.FurShort, Color.Brown, Color.Black); }
        }



        public HideType m_eHideType = HideType.BareSkin;

        public Color m_eHideColor = Color.AntiqueWhite;
        public string m_sHideColor = "white";

        public bool m_bSpots = false;

        public Color m_eSpotsColor = Color.AntiqueWhite;
        public string m_sSpotsColor = "white";

        public bool IsIdentical(GenetixBase pOther)
        {
            HideGenetix pAnother = pOther as HideGenetix;

            if (pAnother == null)
                return false;

            if (!m_bSpots && !pAnother.m_bSpots)
                return m_eHideType == pAnother.m_eHideType &&
                m_sHideColor == pAnother.m_sHideColor;

            return m_eHideType == pAnother.m_eHideType &&
                m_sHideColor == pAnother.m_sHideColor &&
                m_bSpots == pAnother.m_bSpots &&
                m_sSpotsColor == pAnother.m_sSpotsColor;
        }
        
        public HideGenetix()
        { }

        public HideGenetix(HideGenetix pPredcessor)
        {
            m_eHideType = pPredcessor.m_eHideType;
            m_eHideColor = pPredcessor.m_eHideColor;
            m_bSpots = pPredcessor.m_bSpots;
            m_eSpotsColor = pPredcessor.m_eSpotsColor;

            m_sHideColor = GetPredefinedColor(m_eHideColor);
            m_sSpotsColor = GetPredefinedColor(m_eSpotsColor);
        }

        public HideGenetix(HideType eHideType, Color eHideColor)
        {
            m_eHideType = eHideType;
            m_eHideColor = eHideColor;
            m_bSpots = false;

            m_sHideColor = GetPredefinedColor(eHideColor);
        }

        public HideGenetix(HideType eHideType, Color eHideColor, Color eSpotsColor)
        {
            m_eHideType = eHideType;
            m_eHideColor = eHideColor;
            m_bSpots = true;
            m_eSpotsColor = eSpotsColor;

            m_sHideColor = GetPredefinedColor(eHideColor);
            m_sSpotsColor = GetPredefinedColor(eSpotsColor);
        }

        private void MutateHideType()
        {
            int iChance = 0;
            switch (m_eHideType)
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
                    m_eHideType = HideType.BareSkin;
                    break;
                case 1:
                    m_eHideType = HideType.FurShort;
                    break;
                case 2:
                    m_eHideType = HideType.FurLong;
                    break;
                case 3:
                    m_eHideType = HideType.Feathers;
                    break;
                case 4:
                    m_eHideType = HideType.Chitin;
                    break;
                case 5:
                    m_eHideType = HideType.Scales;
                    break;
                case 6:
                    m_eHideType = HideType.Shell;
                    break;
            }
        }

        private void MutateHideColor()
        {
            if (m_eHideType == HideType.BareSkin)
            {
                m_bSpots = false;

                KColor pColor = new KColor();
                pColor.RGB = m_eHideColor;

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

                m_eHideColor = pColor.RGB;
                m_sHideColor = GetPredefinedColor(pColor);
            }
            else
            {
                MutateHideColorAnimalsOnly();
            }
        }

        private void MutateHideColorAnimalsOnly()
        {
            if (m_eHideType != HideType.BareSkin)
            {
                m_bSpots = Rnd.OneChanceFrom(2);

                KColor pColor = new KColor();
                pColor.RGB = m_eHideColor;

                if (m_eHideType > HideType.FurLong)
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

                m_eHideColor = pColor.RGB;
                m_sHideColor = GetPredefinedColor(pColor);

                if (m_bSpots)
                {
                    do
                    {
                        pColor.RGB = m_eSpotsColor;

                        if (m_eHideType > HideType.FurLong)
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

                        m_eSpotsColor = pColor.RGB;
                        m_sSpotsColor = GetPredefinedColor(pColor);
                    }
                    while (m_sSpotsColor == m_sHideColor);
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
                
            if (m_eHideType != HideType.BareSkin || Rnd.OneChanceFrom(10))
                pMutant.MutateHideColor();

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        public GenetixBase MutateNation()
        {
            if (Rnd.OneChanceFrom(5))
            {
                HideGenetix pMutant = new HideGenetix(this);

                switch (pMutant.m_eHideType)
                {
                    case HideType.FurShort:
                        {
                            if (Rnd.OneChanceFrom(10))
                                pMutant.m_eHideType = Rnd.OneChanceFrom(4) ? HideType.BareSkin : HideType.FurLong;
                        }
                        break;
                    case HideType.FurLong:
                        {
                            if (Rnd.OneChanceFrom(2))
                                pMutant.m_eHideType = HideType.FurShort;
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
            if (m_eHideType != HideType.BareSkin && Rnd.OneChanceFrom(5))
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
            if (m_eHideType != HideType.BareSkin && Rnd.OneChanceFrom(5))
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
