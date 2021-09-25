using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace RB.Genetix.GenetixParts
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
            switch (m_eEyesType)
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
            if (m_iEyesCount > 1)
                sEyes += "s";

            string sPlace = "?";
            switch (m_eEyesPlacement)
            {
                case EyesPlacement.Tunnel:
                    sPlace = "looking forward";
                    break;
                case EyesPlacement.Panoramic:
                    sPlace = "looking asides";
                    break;
                case EyesPlacement.Stalks:
                    sPlace = m_iEyesCount == 1 ? "placed on flexible stalk" : "placed on flexible stalks";
                    break;
            }

            return m_iEyesCount.ToString() + " " + sEyes + ", " + sPlace;
        }

        public static EyesGenetix Human
        {
            get { return new EyesGenetix(2, EyesType.Normal, EyesPlacement.Tunnel); }
        }

        public static EyesGenetix Ktulhu
        {
            get { return new EyesGenetix(3, EyesType.Normal, EyesPlacement.Tunnel); }
        }

        public static EyesGenetix Herbivore
        {
            get { return new EyesGenetix(2, EyesType.Normal, EyesPlacement.Panoramic); }
        }

        public static EyesGenetix Cat
        {
            get { return new EyesGenetix(2, EyesType.CatEye, EyesPlacement.Tunnel); }
        }

        public static EyesGenetix Fish
        {
            get { return new EyesGenetix(2, EyesType.FishEye, EyesPlacement.Panoramic); }
        }

        public static EyesGenetix Insect
        {
            get { return new EyesGenetix(2, EyesType.Facetted, EyesPlacement.Panoramic); }
        }

        public static EyesGenetix Spider
        {
            get { return new EyesGenetix(8, EyesType.Facetted, EyesPlacement.Tunnel); }
        }

        public EyesPlacement m_eEyesPlacement = EyesPlacement.Tunnel;

        public int m_iEyesCount = 2;

        public EyesType m_eEyesType = EyesType.Normal;

        public bool IsIdentical(GenetixBase pOther)
        {
            EyesGenetix pAnother = pOther as EyesGenetix;

            if (pAnother == null)
                return false;

            return m_eEyesPlacement == pAnother.m_eEyesPlacement &&
                m_iEyesCount == pAnother.m_iEyesCount &&
                m_eEyesType == pAnother.m_eEyesType;
        }

        public EyesGenetix()
        { }

        public EyesGenetix(EyesGenetix pPredcessor)
        {
            m_eEyesPlacement = pPredcessor.m_eEyesPlacement;
            m_eEyesType = pPredcessor.m_eEyesType;
            m_iEyesCount = pPredcessor.m_iEyesCount;
        }

        public EyesGenetix(int iEyesCount, EyesType eEyesType, EyesPlacement eEyesPlacement)
        {
            m_eEyesPlacement = eEyesPlacement;
            m_eEyesType = eEyesType;
            m_iEyesCount = iEyesCount;
        }
        
        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                EyesGenetix pMutant = new EyesGenetix(this);

                if (Rnd.OneChanceFrom(5))
                    pMutant.m_eEyesPlacement = (EyesPlacement)Rnd.Get(typeof(EyesPlacement));

                if (Rnd.OneChanceFrom(2))
                    pMutant.m_eEyesType = (EyesType)Rnd.Get(typeof(EyesType));

                if (Rnd.OneChanceFrom(2))
                {
                    int iChance = (int)Math.Pow(Rnd.Get(23), 2) / 100;
                    switch (iChance)
                    {
                        case 0:
                            pMutant.m_iEyesCount = 2;
                            break;
                        case 1:
                            pMutant.m_iEyesCount = 1;
                            break;
                        case 2:
                            pMutant.m_iEyesCount = 3;
                            break;
                        case 3:
                            pMutant.m_iEyesCount = 4;
                            break;
                        case 4:
                            pMutant.m_iEyesCount = 8;
                            break;
                    }
                }

                if (pMutant.m_iEyesCount == 1 && pMutant.m_eEyesPlacement == EyesPlacement.Panoramic)
                    pMutant.m_eEyesPlacement = EyesPlacement.Tunnel;

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
