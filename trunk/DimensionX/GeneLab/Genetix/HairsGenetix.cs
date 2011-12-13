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
        /// редкие волосы
        /// </summary>
        Sparse,
        /// <summary>
        /// густые волосы
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
    
    class HairsGenetix : GenetixBase
    {
        public HairsAmount m_eHairsM = HairsAmount.Sparse;

        public HairsAmount m_eHairsF = HairsAmount.Thick;

        public HairsAmount m_eBeardM = HairsAmount.Thick;

        public HairsAmount m_eBeardF = HairsAmount.None;

        public HairsType m_eHairsType = HairsType.Hair;

        public List<HairsColor> m_cHairColors = new List<HairsColor>(new HairsColor[] { HairsColor.Brunette, HairsColor.Blonde, HairsColor.DarkBlond, HairsColor.Red });

        public HairsGenetix()
        { }

        public HairsGenetix(HairsGenetix pPredcessor)
        {
            m_eHairsM = pPredcessor.m_eHairsM;
            m_eHairsF = pPredcessor.m_eHairsF;
            m_eBeardM = pPredcessor.m_eBeardM;
            m_eBeardF = pPredcessor.m_eBeardF;
            m_eHairsType = pPredcessor.m_eHairsType;

            m_cHairColors = pPredcessor.m_cHairColors;
        }

        public HairsGenetix(HairsAmount eHairsM, HairsAmount eHairsF, HairsAmount eBeardM, HairsAmount eBeardF, HairsType eHairsType, List<HairsColor> cHairColors)
        {
            m_eHairsM = eHairsM;
            m_eHairsF = eHairsF;
            m_eBeardM = eBeardM;
            m_eBeardF = eBeardF;
            m_eHairsType = eHairsType;

            m_cHairColors = cHairColors;
        }
        
        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                bool bMutation = false;

                HairsGenetix pMutant = new HairsGenetix(this);

                if (Rnd.OneChanceFrom(5))
                {
                    pMutant.m_eHairsType = (HairsType)Rnd.Get(typeof(HairsType));
                    if (pMutant.m_eHairsType != m_eHairsType)
                        bMutation = true;
                }

                pMutant.m_eHairsM = (HairsAmount)Rnd.Get(typeof(HairsAmount));
                pMutant.m_eHairsF = (HairsAmount)Rnd.Get(typeof(HairsAmount));

                if (Rnd.OneChanceFrom(3))
                    pMutant.m_eBeardM = (HairsAmount)Rnd.Get(typeof(HairsAmount));
                else
                    pMutant.m_eBeardM = pMutant.m_eHairsM;

                if (Rnd.OneChanceFrom(3))
                    pMutant.m_eBeardF = (HairsAmount)Rnd.Get(typeof(HairsAmount));
                else
                    pMutant.m_eBeardF = HairsAmount.None;
                
                if (pMutant.m_eHairsM != m_eHairsM ||
                    pMutant.m_eHairsF != m_eHairsF ||
                    pMutant.m_eBeardM != m_eBeardM ||
                    pMutant.m_eBeardF != m_eBeardF)
                    bMutation = true;

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

                if (pMutant.m_cHairColors.Count != m_cHairColors.Count)
                    bMutation = true;
                else
                {
                    foreach (HairsColor eColor in m_cHairColors)
                        if (!pMutant.m_cHairColors.Contains(eColor))
                            bMutation = true;
                }

                if (bMutation)
                    return pMutant;
            }

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
            if (Rnd.OneChanceFrom(10))
            {
                bool bMutation = false;

                HairsGenetix pMutant = new HairsGenetix(this);

                pMutant.m_eHairsM = MutateHairsAmount(pMutant.m_eHairsM);
                pMutant.m_eHairsF = MutateHairsAmount(pMutant.m_eHairsF);

                if (Rnd.OneChanceFrom(3))
                    pMutant.m_eBeardM = MutateHairsAmount(pMutant.m_eBeardM);
                else
                    pMutant.m_eBeardM = pMutant.m_eHairsM;

                if (Rnd.OneChanceFrom(3))
                    pMutant.m_eBeardF = MutateHairsAmount(pMutant.m_eBeardF);
                else
                    pMutant.m_eBeardF = HairsAmount.None;

                if (pMutant.m_eHairsM != m_eHairsM ||
                    pMutant.m_eHairsF != m_eHairsF ||
                    pMutant.m_eBeardM != m_eBeardM ||
                    pMutant.m_eBeardF != m_eBeardF)
                    bMutation = true;

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

                if (pMutant.m_cHairColors.Count != m_cHairColors.Count)
                    bMutation = true;
                else
                {
                    foreach (HairsColor eColor in m_cHairColors)
                        if (!pMutant.m_cHairColors.Contains(eColor))
                            bMutation = true;
                }

                if(bMutation)
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateFamily()
        {
            if (Rnd.OneChanceFrom(3))
            {
                bool bMutation = false;

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


                if (pMutant.m_cHairColors.Count != m_cHairColors.Count)
                    bMutation = true;
                else
                {
                    foreach (HairsColor eColor in m_cHairColors)
                        if (!pMutant.m_cHairColors.Contains(eColor))
                            bMutation = true;
                } 
                
                if(bMutation)
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateIndividual()
        {
            bool bMutation = false;

            HairsGenetix pMutant = new HairsGenetix(this);

            pMutant.m_cHairColors.Clear();
            pMutant.m_cHairColors.Add(m_cHairColors[Rnd.Get(m_cHairColors.Count)]);

            if (pMutant.m_cHairColors.Count != m_cHairColors.Count)
                bMutation = true;
            else
            {
                foreach (HairsColor eColor in m_cHairColors)
                    if (!pMutant.m_cHairColors.Contains(eColor))
                        bMutation = true;
            }

            if (bMutation) 
                return pMutant;

            return this;
        }
    }
}
