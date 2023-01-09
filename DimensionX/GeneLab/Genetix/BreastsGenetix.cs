using Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneLab.Genetix
{
    public enum BreastsCount
    {
        None,
        Two,
        Four,
        Six,
        Eight,
        Dozen
    }

    public enum BreastSize
    {
        None,
        Small,
        Normal,
        Big
    }

    public class BreastsGenetix : GenetixBase
    {
        /// <summary>
        /// 'have 2 small breasts'
        /// or just '' if no breasts
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return GetDescription(null, true);
        }

        /// <summary>
        /// 'have 2 small breasts'
        /// or - if gender specified -
        /// 'He/She have 2 small breasts'
        /// or just '' if no breasts
        /// </summary>
        /// <returns></returns>
        public string GetDescription(Gender? eGender, bool bFull)
        {
            string sBreasts = "";
            switch (BreastsCount)
            {
                case BreastsCount.Two:
                    if (bFull)
                        sBreasts = "2";
                    break;
                case BreastsCount.Four:
                    sBreasts = "4";
                    break;
                case BreastsCount.Six:
                    sBreasts = "6";
                    break;
                case BreastsCount.Eight:
                    sBreasts = "8";
                    break;
                case BreastsCount.Dozen:
                    sBreasts = "many";
                    break;
            }

            if (sBreasts != "")
            {
                sBreasts += " ";
                switch (BreastSize)
                {
                    case BreastSize.None:
                        if (eGender.HasValue && eGender != Gender.Male)
                            sBreasts += "tiny breasts";
                        else
                            sBreasts = "";
                        break;
                    case BreastSize.Small:
                        sBreasts += "small breasts";
                        break;
                    case BreastSize.Normal:
                        if (bFull)
                            sBreasts += "average breasts";
                        else
                            sBreasts = "";
                        break;
                    case BreastSize.Big:
                        sBreasts += "big breasts";
                        break;
                }
            }

            if (sBreasts != "")
            {
                if (eGender.HasValue)
                    return (eGender == Gender.Male ? " He" : " She") + " has " + sBreasts + ".";
                else
                    return "have " + sBreasts;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// no breasts, no nipples
        /// </summary>
        public static BreastsGenetix None
        {
            get { return new BreastsGenetix(BreastsCount.None, BreastSize.None); }
        }

        /// <summary>
        /// no breasts, 2 nipples
        /// </summary>
        public static BreastsGenetix TwoMale
        {
            get { return new BreastsGenetix(BreastsCount.Two, BreastSize.None); }
        }

        /// <summary>
        /// 2 small breasts
        /// </summary>
        public static BreastsGenetix TwoSmall
        {
            get { return new BreastsGenetix(BreastsCount.Two, BreastSize.Small); }
        }

        /// <summary>
        /// 2 average breasts
        /// </summary>
        public static BreastsGenetix TwoAverage
        {
            get { return new BreastsGenetix(BreastsCount.Two, BreastSize.Normal); }
        }

        /// <summary>
        /// 2 big breasts
        /// </summary>
        public static BreastsGenetix TwoBig
        {
            get { return new BreastsGenetix(BreastsCount.Two, BreastSize.Big); }
        }

        /// <summary>
        /// no breasts, 4 nipples
        /// </summary>
        public static BreastsGenetix FourMale
        {
            get { return new BreastsGenetix(BreastsCount.Four, BreastSize.None); }
        }

        /// <summary>
        /// 4 small breasts
        /// </summary>
        public static BreastsGenetix FourSmall
        {
            get { return new BreastsGenetix(BreastsCount.Four, BreastSize.Small); }
        }

        /// <summary>
        /// 4 average breasts
        /// </summary>
        public static BreastsGenetix FourAverage
        {
            get { return new BreastsGenetix(BreastsCount.Four, BreastSize.Normal); }
        }

        /// <summary>
        /// 4 big breasts
        /// </summary>
        public static BreastsGenetix FourBig
        {
            get { return new BreastsGenetix(BreastsCount.Four, BreastSize.Big); }
        }

        /// <summary>
        /// no breasts, 6 nipples
        /// </summary>
        public static BreastsGenetix SixMale
        {
            get { return new BreastsGenetix(BreastsCount.Six, BreastSize.None); }
        }

        /// <summary>
        /// 6 big breasts
        /// </summary>
        public static BreastsGenetix SixBig
        {
            get { return new BreastsGenetix(BreastsCount.Six, BreastSize.Big); }
        }

        /// <summary>
        /// no breasts, 8 nipples
        /// </summary>
        public static BreastsGenetix EightMale
        {
            get { return new BreastsGenetix(BreastsCount.Eight, BreastSize.None); }
        }

        /// <summary>
        /// 8 small breasts
        /// </summary>
        public static BreastsGenetix EightSmall
        {
            get { return new BreastsGenetix(BreastsCount.Eight, BreastSize.Small); }
        }

        public BreastsCount BreastsCount { get; private set; } = BreastsCount.Two;

        public BreastSize BreastSize { get; private set; } = BreastSize.None;

        public bool IsIdentical(GenetixBase pOther)
        {
            if (!(pOther is BreastsGenetix pAnother))
                return false;

            return BreastsCount == pAnother.BreastsCount &&
                BreastSize == pAnother.BreastSize;
        }

        public BreastsGenetix()
        { }

        public BreastsGenetix(BreastsGenetix pPredcessor)
        {
            BreastsCount = pPredcessor.BreastsCount;
            BreastSize = pPredcessor.BreastSize;
        }

        public BreastsGenetix(BreastsCount eBreastsCount, BreastSize eBreastSize)
        {
            BreastsCount = eBreastsCount;
            BreastSize = eBreastSize;
        }

        #region GenetixBase Members

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                BreastsGenetix pMutant = new BreastsGenetix(this);

                if (Rnd.OneChanceFrom(10))
                {
                    switch (BreastsCount)
                    {
                        case BreastsCount.Two:
                            pMutant.BreastsCount = BreastsCount.Four;
                            break;
                        case BreastsCount.Four:
                            pMutant.BreastsCount = Rnd.OneChanceFrom(2) ? BreastsCount.Two : BreastsCount.Six;
                            break;
                        case BreastsCount.Six:
                            pMutant.BreastsCount = Rnd.OneChanceFrom(2) ? BreastsCount.Four : BreastsCount.Eight;
                            break;
                        case BreastsCount.Eight:
                            pMutant.BreastsCount = Rnd.OneChanceFrom(2) ? BreastsCount.Six : BreastsCount.Dozen;
                            break;
                        case BreastsCount.Dozen:
                            pMutant.BreastsCount = BreastsCount.Eight;
                            break;
                    }
                }

                if (pMutant.BreastsCount != BreastsCount.None && Rnd.OneChanceFrom(2))
                {
                    switch (BreastSize)
                    {
                        //case BreastSize.None:
                        //    pMutant.m_eBreastSize = BreastSize.Small;
                        //    break;
                        case BreastSize.Small:
                            pMutant.BreastSize = /*Rnd.OneChanceFrom(2) ? BreastSize.None :*/ BreastSize.Big;
                            break;
                        case BreastSize.Big:
                            pMutant.BreastSize = BreastSize.Small;
                            break;
                    }
                }

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateGender()
        {
            if (Rnd.OneChanceFrom(2))
            {
                BreastsGenetix pMutant = new BreastsGenetix(this);

                if (pMutant.BreastsCount != BreastsCount.None && Rnd.OneChanceFrom(2))
                {
                    switch (BreastSize)
                    {
                        case BreastSize.None:
                            pMutant.BreastSize = BreastSize.Small;
                            break;
                        case BreastSize.Small:
                            pMutant.BreastSize = /*Rnd.OneChanceFrom(2) ? BreastSize.None :*/ BreastSize.Big;
                            break;
                        case BreastSize.Big:
                            pMutant.BreastSize = BreastSize.Small;
                            break;
                    }
                }

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateNation()
        {
            if (Rnd.OneChanceFrom(10))
            {
                BreastsGenetix pMutant = new BreastsGenetix(this);

                if (pMutant.BreastsCount != BreastsCount.None && Rnd.OneChanceFrom(2))
                {
                    switch (BreastSize)
                    {
                        case BreastSize.Small:
                            pMutant.BreastSize = BreastSize.Big;
                            break;
                        case BreastSize.Big:
                            pMutant.BreastSize = BreastSize.Small;
                            break;
                    }
                }

                return pMutant;
            }

            return this;
        }

        public GenetixBase MutateFamily()
        {
            if (Rnd.OneChanceFrom(10))
            {
                BreastsGenetix pMutant = new BreastsGenetix(this);

                if (pMutant.BreastsCount != BreastsCount.None && Rnd.OneChanceFrom(2))
                {
                    switch (BreastSize)
                    {
                        case BreastSize.Small:
                            pMutant.BreastSize = BreastSize.Big;
                            break;
                        case BreastSize.Big:
                            pMutant.BreastSize = BreastSize.Small;
                            break;
                    }
                }

                return pMutant;
            }

            return this;
        }

        public GenetixBase MutateIndividual()
        {
            BreastsGenetix pMutant = new BreastsGenetix(this);

            if (pMutant.BreastsCount != BreastsCount.None && Rnd.OneChanceFrom(3))
            {
                switch (BreastSize)
                {
                    case BreastSize.Small:
                        pMutant.BreastSize = Rnd.OneChanceFrom(2) ? BreastSize.None : BreastSize.Big;
                        break;
                    case BreastSize.Big:
                        pMutant.BreastSize = BreastSize.Small;
                        break;
                }
            }

            if (!pMutant.IsIdentical(this))
                return pMutant;

            return this;
        }

        #endregion
    }
}
