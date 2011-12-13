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
    
    class EyesGenetix : GenetixBase
    {
        public EyesPlacement m_eEyesPlacement = EyesPlacement.Tunnel;

        public int m_iEyesCount = 2;

        public EyesType m_eEyesType = EyesType.Normal;

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
                bool bMutation = false;

                EyesGenetix pMutant = new EyesGenetix(this);

                if (Rnd.OneChanceFrom(5))
                {
                    pMutant.m_eEyesPlacement = (EyesPlacement)Rnd.Get(typeof(EyesPlacement));
                    if (pMutant.m_eEyesPlacement != m_eEyesPlacement)
                        bMutation = true;
                }

                if (Rnd.OneChanceFrom(2))
                {
                    pMutant.m_eEyesType = (EyesType)Rnd.Get(typeof(EyesType));
                    if (pMutant.m_eEyesType != m_eEyesType)
                        bMutation = true;
                }

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
                    if (pMutant.m_iEyesCount != m_iEyesCount)
                        bMutation = true;
                }

                if(bMutation)
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
