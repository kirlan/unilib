using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace GeneLab.Genetix
{
    public enum EyesPlacement
    {
        /// <summary>
        /// все глаза расположены на передней стороне головы и смотрят в одном направлении (вперёд).
        /// </summary>
        Tunnel,
        /// <summary>
        /// глаза разнесены на разные стороны головы и смотрят в разные стороны, обеспечивая максимальный угол обзора
        /// </summary>
        Panoramic,
        /// <summary>
        /// глаза расположены на подвижных стебельках и могут смотреть в любую сторону
        /// </summary>
        Stalks
    }

    public enum EyesType
    {
        /// <summary>
        /// обычные глаза, как у человека (круглый зрачок, закрывающееся веко)
        /// </summary>
        Normal,
        /// <summary>
        /// глаза, как у кошки (вертикальный зрачок, закрывающееся веко)
        /// </summary>
        CatEye,
        /// <summary>
        /// рыбий глаз - с круглым зрачком, прозрачной роговой пластинкой, без закрывающегося века
        /// </summary>
        FishEye,
        /// <summary>
        /// фасеточный глаз, как у стрекозы
        /// </summary>
        Facetted
    }

    public class EyesGenetix : GenetixBase
    {
        /// <summary>
        /// 3 facetted eyes, placed on flexible stalks
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string sEyes = "?";
            switch (EyesType)
            {
                case EyesType.Normal:
                    sEyes = "eye";
                    break;
                case EyesType.CatEye:
                    sEyes = "feline eye";
                    break;
                case EyesType.FishEye:
                    sEyes = "fishy eye";
                    break;
                case EyesType.Facetted:
                    sEyes = "facetted eye";
                    break;
            }
            if (EyesCount > 1)
                sEyes += "s";

            string sPlace = "?";
            switch (EyesPlacement)
            {
                case EyesPlacement.Tunnel:
                    sPlace = "looking forward";
                    break;
                case EyesPlacement.Panoramic:
                    sPlace = "looking asides";
                    break;
                case EyesPlacement.Stalks:
                    sPlace = EyesCount == 1 ? "placed on flexible stalk" : "placed on flexible stalks";
                    break;
            }

            return EyesCount.ToString() + " " + sEyes + ", " + sPlace;
        }

        // 2 normal, at front
        public static EyesGenetix Human
        {
            get { return new EyesGenetix(2, EyesType.Normal, EyesPlacement.Tunnel); }
        }

        /// <summary>
        /// 3 normal, at front
        /// </summary>
        public static EyesGenetix Ktulhu
        {
            get { return new EyesGenetix(3, EyesType.Normal, EyesPlacement.Tunnel); }
        }

        /// <summary>
        /// 2 normal, at sides
        /// </summary>
        public static EyesGenetix Herbivore
        {
            get { return new EyesGenetix(2, EyesType.Normal, EyesPlacement.Panoramic); }
        }

        /// <summary>
        /// 2 cat eyes, at front
        /// </summary>
        public static EyesGenetix Cat
        {
            get { return new EyesGenetix(2, EyesType.CatEye, EyesPlacement.Tunnel); }
        }

        /// <summary>
        /// 2 fish eyes, at sides
        /// </summary>
        public static EyesGenetix Fish
        {
            get { return new EyesGenetix(2, EyesType.FishEye, EyesPlacement.Panoramic); }
        }

        /// <summary>
        /// 2 facetted eyes, ar sides
        /// </summary>
        public static EyesGenetix Insect
        {
            get { return new EyesGenetix(2, EyesType.Facetted, EyesPlacement.Panoramic); }
        }

        /// <summary>
        /// 8 acetted eyes, at front
        /// </summary>
        public static EyesGenetix Spider
        {
            get { return new EyesGenetix(8, EyesType.Facetted, EyesPlacement.Tunnel); }
        }

        public EyesPlacement EyesPlacement { get; private set; } = EyesPlacement.Tunnel;

        public int EyesCount { get; private set; } = 2;

        public EyesType EyesType { get; private set; } = EyesType.Normal;

        public bool IsIdentical(GenetixBase pOther)
        {
            if (!(pOther is EyesGenetix pAnother))
                return false;

            return EyesPlacement == pAnother.EyesPlacement &&
                EyesCount == pAnother.EyesCount &&
                EyesType == pAnother.EyesType;
        }

        public EyesGenetix()
        { }

        public EyesGenetix(EyesGenetix pPredcessor)
        {
            EyesPlacement = pPredcessor.EyesPlacement;
            EyesType = pPredcessor.EyesType;
            EyesCount = pPredcessor.EyesCount;
        }

        public EyesGenetix(int iEyesCount, EyesType eEyesType, EyesPlacement eEyesPlacement)
        {
            EyesPlacement = eEyesPlacement;
            EyesType = eEyesType;
            EyesCount = iEyesCount;
        }

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                EyesGenetix pMutant = new EyesGenetix(this);

                if (Rnd.OneChanceFrom(5))
                    pMutant.EyesPlacement = (EyesPlacement)Rnd.Get(typeof(EyesPlacement));

                if (Rnd.OneChanceFrom(2))
                    pMutant.EyesType = (EyesType)Rnd.Get(typeof(EyesType));

                if (Rnd.OneChanceFrom(2))
                {
                    int iChance = (int)Math.Pow(Rnd.Get(23), 2) / 100;
                    switch (iChance)
                    {
                        case 0:
                            pMutant.EyesCount = 2;
                            break;
                        case 1:
                            pMutant.EyesCount = 1;
                            break;
                        case 2:
                            pMutant.EyesCount = 3;
                            break;
                        case 3:
                            pMutant.EyesCount = 4;
                            break;
                        case 4:
                            pMutant.EyesCount = 8;
                            break;
                    }
                }

                if (pMutant.EyesCount == 1 && pMutant.EyesPlacement == EyesPlacement.Panoramic)
                    pMutant.EyesPlacement = EyesPlacement.Tunnel;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateGender()
        {
            if (Rnd.OneChanceFrom(50))
            {
                EyesGenetix pMutant = new EyesGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    int iChance = (int)Math.Pow(Rnd.Get(23), 2) / 100;
                    switch (iChance)
                    {
                        case 0:
                            pMutant.EyesCount = 2;
                            break;
                        case 1:
                            pMutant.EyesCount = 1;
                            break;
                        case 2:
                            pMutant.EyesCount = 3;
                            break;
                        case 3:
                            pMutant.EyesCount = 4;
                            break;
                        case 4:
                            pMutant.EyesCount = 8;
                            break;
                    }
                }

                if (pMutant.EyesCount == 1 && pMutant.EyesPlacement == EyesPlacement.Panoramic)
                    pMutant.EyesPlacement = EyesPlacement.Tunnel;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateNation()
        {
            return this;
        }

        public GenetixBase MutateFamily()
        {
            return this;
        }

        public GenetixBase MutateIndividual()
        {
            return this;
        }
    }
}
