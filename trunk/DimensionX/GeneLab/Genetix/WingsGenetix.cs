using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace GeneLab.Genetix
{
    public enum WingsCount
    { 
        /// <summary>
        /// безкрылый
        /// </summary>
        None,
        /// <summary>
        /// двукрылый - как птицы
        /// </summary>
        Dipterous,
        /// <summary>
        /// четырёхкрылый - как стрекозы
        /// </summary>
        Quadrupterous
    }

    public enum WingsType
    { 
        /// <summary>
        /// оперённые крылья как у птиц
        /// </summary>
        Feathered,
        /// <summary>
        /// кожистые крылья, как у летучих мышей
        /// </summary>
        Leather,
        /// <summary>
        /// тонкие, жёсткие, полупрозрачные крылья, как у насекомых
        /// </summary>
        Insectoid,
        /// <summary>
        /// и не крылья вовсе, а щупальца, растущие из спины - вместо крыльев
        /// </summary>
        Tentacles
    }

    public enum WingsForce
    {
        /// <summary>
        /// никакой пользы с этих крыльев
        /// </summary>
        None,
        /// <summary>
        /// можно только планировать с высоких мест
        /// </summary>
        Gliding,
        /// <summary>
        /// способность к полноценному полёту
        /// </summary>
        Flying
    }

    public class WingsGenetix: GenetixBase
    {
        public WingsCount m_eWingsCount = WingsCount.Dipterous;

        public WingsType m_eWingsType = WingsType.Feathered;

        public WingsForce m_eWingsForce = WingsForce.Flying;

        public WingsGenetix()
        { }

        public WingsGenetix(WingsGenetix pPredcessor)
        {
            m_eWingsCount = pPredcessor.m_eWingsCount;
            m_eWingsType = pPredcessor.m_eWingsType;
            m_eWingsForce = pPredcessor.m_eWingsForce;
        }

        public WingsGenetix(WingsCount eArmsCount, WingsType eArmsType, WingsForce eWingsForce)
        {
            m_eWingsCount = eArmsCount;
            m_eWingsType = eArmsType;
            m_eWingsForce = eWingsForce;

            if (m_eWingsType == WingsType.Tentacles || m_eWingsCount == WingsCount.None)
                m_eWingsForce = WingsForce.None;
        }
        
        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                bool bMutation = false;

                WingsGenetix pMutant = new WingsGenetix(this);

                if (Rnd.OneChanceFrom(2))
                    pMutant.m_eWingsCount = (WingsCount)Rnd.Get(typeof(WingsCount));

                if (pMutant.m_eWingsCount != m_eWingsCount)
                    bMutation = true;

                int iChance = Rnd.Get(4);
                if (m_eWingsForce != WingsForce.None)
                    iChance = Rnd.Get(3);
                    
                switch (iChance)
                {
                    case 0:
                        pMutant.m_eWingsType = WingsType.Feathered;
                        break;
                    case 1:
                        pMutant.m_eWingsType = WingsType.Leather;
                        break;
                    case 2:
                        pMutant.m_eWingsType = WingsType.Insectoid;
                        break;
                    case 3:
                        pMutant.m_eWingsType = WingsType.Tentacles;
                        break;
                }

                if (pMutant.m_eWingsType != m_eWingsType)
                    bMutation = true;

                pMutant.m_eWingsForce = (WingsForce)Rnd.Get(typeof(WingsForce));

                if (pMutant.m_eWingsType == WingsType.Tentacles || pMutant.m_eWingsCount == WingsCount.None)
                    pMutant.m_eWingsForce = WingsForce.None;

                if (pMutant.m_eWingsForce != m_eWingsForce)
                    bMutation = true;

                if(bMutation)
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateNation()
        {
            if (Rnd.OneChanceFrom(20))
            {
                bool bMutation = false;

                WingsGenetix pMutant = new WingsGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.m_eWingsCount == WingsCount.Dipterous)
                    {
                        pMutant.m_eWingsCount = WingsCount.Quadrupterous;
                        bMutation = true;
                    }
                    if (pMutant.m_eWingsCount == WingsCount.Quadrupterous)
                    {
                        pMutant.m_eWingsCount = WingsCount.Dipterous;
                        bMutation = true;
                    }
                }

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.m_eWingsType == WingsType.Feathered)
                    {
                        pMutant.m_eWingsType = WingsType.Leather;
                        bMutation = true;
                    }
                    if (pMutant.m_eWingsType == WingsType.Leather)
                    {
                        pMutant.m_eWingsType = WingsType.Feathered;
                        bMutation = true;
                    }
                }

                pMutant.m_eWingsForce = (WingsForce)Rnd.Get(typeof(WingsForce));

                if (pMutant.m_eWingsType == WingsType.Tentacles || pMutant.m_eWingsCount == WingsCount.None)
                    pMutant.m_eWingsForce = WingsForce.None;

                if (pMutant.m_eWingsForce != m_eWingsForce)
                    bMutation = true;

                if (bMutation)
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateFamily()
        {
            return this;
        }

        public GenetixBase MutateIndividual()
        {
            if (Rnd.OneChanceFrom(50))
            {
                bool bMutation = false;

                WingsGenetix pMutant = new WingsGenetix(this);

                pMutant.m_eWingsForce = (WingsForce)Rnd.Get(typeof(WingsForce));

                if (pMutant.m_eWingsType == WingsType.Tentacles || pMutant.m_eWingsCount == WingsCount.None)
                    pMutant.m_eWingsForce = WingsForce.None;

                if (pMutant.m_eWingsForce != m_eWingsForce)
                    bMutation = true;

                if (bMutation)
                    return pMutant;
            }

            return this;
        }
    }
}
