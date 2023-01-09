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
        /// <summary>
        /// 2 pairs of strong leather wings
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            if (WingsCount == WingsCount.None)
                return "";

            string sWings = "";
            switch (WingsForce)
            {
                case WingsForce.None:
                    sWings = "small";
                    break;
                case WingsForce.Gliding:
                    sWings = "weak";
                    break;
                case WingsForce.Flying:
                    sWings = "strong";
                    break;
            }

            switch (WingsType)
            {
                case WingsType.Feathered:
                    sWings += " feathered wings";
                    break;
                case WingsType.Leather:
                    sWings += " leather wings";
                    break;
                case WingsType.Insectoid:
                    sWings += " insect wings";
                    break;
                case WingsType.Tentacles:
                    sWings = "long tentacles on their back";
                    break;
            }

            return (WingsCount == WingsCount.Dipterous ? "pair" : "2 pairs") + " of " + sWings;
        }

        /// <summary>
        /// no wings
        /// </summary>
        public static WingsGenetix None
        {
            get { return new WingsGenetix(WingsCount.None, WingsType.Leather, WingsForce.None); }
        }

        /// <summary>
        /// 2 strong feathered wings
        /// </summary>
        public static WingsGenetix Bird
        {
            get { return new WingsGenetix(WingsCount.Dipterous, WingsType.Feathered, WingsForce.Flying); }
        }

        /// <summary>
        /// 4 strong feathered wings
        /// </summary>
        public static WingsGenetix Seraph
        {
            get { return new WingsGenetix(WingsCount.Quadrupterous, WingsType.Feathered, WingsForce.Flying); }
        }

        /// <summary>
        /// 2 strong leathery wings
        /// </summary>
        public static WingsGenetix Bat
        {
            get { return new WingsGenetix(WingsCount.Dipterous, WingsType.Leather, WingsForce.Flying); }
        }

        /// <summary>
        /// 4 strong insectoid wings
        /// </summary>
        public static WingsGenetix Insect
        {
            get { return new WingsGenetix(WingsCount.Quadrupterous, WingsType.Insectoid, WingsForce.Flying); }
        }

        /// <summary>
        /// 4 flexible tentacles
        /// </summary>
        public static WingsGenetix Tentacles
        {
            get { return new WingsGenetix(WingsCount.Quadrupterous, WingsType.Tentacles, WingsForce.None); }
        }

        public WingsCount WingsCount { get; private set; } = WingsCount.Dipterous;

        public WingsType WingsType { get; private set; } = WingsType.Feathered;

        public WingsForce WingsForce { get; private set; } = WingsForce.Flying;

        public bool IsIdentical(GenetixBase pOther)
        {
            if (!(pOther is WingsGenetix pAnother))
                return false;

            if (WingsCount == WingsCount.None &&
                pAnother.WingsCount == WingsCount.None)
            {
                return true;
            }

            return WingsCount == pAnother.WingsCount &&
                WingsType == pAnother.WingsType &&
                WingsForce == pAnother.WingsForce;
        }

        public WingsGenetix()
        { }

        public WingsGenetix(WingsGenetix pPredcessor)
        {
            WingsCount = pPredcessor.WingsCount;
            WingsType = pPredcessor.WingsType;
            WingsForce = pPredcessor.WingsForce;
        }

        public WingsGenetix(WingsCount eArmsCount, WingsType eArmsType, WingsForce eWingsForce)
        {
            WingsCount = eArmsCount;
            WingsType = eArmsType;
            WingsForce = eWingsForce;

            if (WingsType == WingsType.Tentacles || WingsCount == WingsCount.None)
                WingsForce = WingsForce.None;
        }

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                WingsGenetix pMutant = new WingsGenetix(this);

                if (Rnd.OneChanceFrom(2))
                    pMutant.WingsCount = (WingsCount)Rnd.Get(typeof(WingsCount));

                int iChance = Rnd.Get(4);
                if (WingsForce != WingsForce.None)
                    iChance = Rnd.Get(3);

                switch (iChance)
                {
                    case 0:
                        pMutant.WingsType = WingsType.Feathered;
                        break;
                    case 1:
                        pMutant.WingsType = WingsType.Leather;
                        break;
                    case 2:
                        pMutant.WingsType = WingsType.Insectoid;
                        break;
                    case 3:
                        pMutant.WingsType = WingsType.Tentacles;
                        break;
                }

                pMutant.WingsForce = (WingsForce)Rnd.Get(typeof(WingsForce));

                if (pMutant.WingsType == WingsType.Tentacles || pMutant.WingsCount == WingsCount.None)
                    pMutant.WingsForce = WingsForce.None;

                if(!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateGender()
        {
            if (Rnd.OneChanceFrom(20))
            {
                WingsGenetix pMutant = new WingsGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.WingsCount == WingsCount.Dipterous)
                        pMutant.WingsCount = WingsCount.Quadrupterous;
                    if (pMutant.WingsCount == WingsCount.Quadrupterous)
                        pMutant.WingsCount = WingsCount.Dipterous;
                }

                pMutant.WingsForce = (WingsForce)Rnd.Get(typeof(WingsForce));

                if (pMutant.WingsType == WingsType.Tentacles || pMutant.WingsCount == WingsCount.None)
                    pMutant.WingsForce = WingsForce.None;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateNation()
        {
            if (Rnd.OneChanceFrom(20))
            {
                WingsGenetix pMutant = new WingsGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.WingsCount == WingsCount.Dipterous)
                        pMutant.WingsCount = WingsCount.Quadrupterous;
                    if (pMutant.WingsCount == WingsCount.Quadrupterous)
                        pMutant.WingsCount = WingsCount.Dipterous;
                }

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.WingsType == WingsType.Feathered)
                        pMutant.WingsType = WingsType.Leather;
                    if (pMutant.WingsType == WingsType.Leather)
                        pMutant.WingsType = WingsType.Feathered;
                }

                pMutant.WingsForce = (WingsForce)Rnd.Get(typeof(WingsForce));

                if (pMutant.WingsType == WingsType.Tentacles || pMutant.WingsCount == WingsCount.None)
                    pMutant.WingsForce = WingsForce.None;

                if (!pMutant.IsIdentical(this))
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
                WingsGenetix pMutant = new WingsGenetix(this) { WingsForce = (WingsForce)Rnd.Get(typeof(WingsForce)) };

                if (pMutant.WingsType == WingsType.Tentacles || pMutant.WingsCount == WingsCount.None)
                    pMutant.WingsForce = WingsForce.None;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }
    }
}
