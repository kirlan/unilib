using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace GeneLab.Genetix
{
    public enum TailLength
    {
        /// <summary>
        /// нет хвоста - как у человека
        /// </summary>
        None,
        /// <summary>
        /// хвост есть, но чисто символический - как у птиц или у зайца
        /// </summary>
        Short,
        /// <summary>
        /// хвост есть и длинный - как у большинства животных
        /// </summary>
        Long
    }

    public enum TailControl
    {
        /// <summary>
        /// хвостом шевелить нельзя
        /// </summary>
        None,
        /// <summary>
        /// доступны грубые движения - как большинству животных
        /// </summary>
        Crude,
        /// <summary>
        /// доступны точные движения - как обезьянам
        /// </summary>
        Skillful
    }

    public class TailGenetix: IGenetix
    {
        /// <summary>
        /// a long, but almost useless tail
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string sTail = "";
            switch (TailLength)
            {
                case TailLength.Short:
                    sTail = "a short";
                    break;
                case TailLength.Long:
                    sTail = "a long";
                    break;
            }

            if (sTail != "")
            {
                switch (TailControl)
                {
                    case TailControl.None:
                        sTail += TailLength == TailLength.Long ? ", but absolutely useless tail" : " and absolutely useless tail";
                        break;
                    case TailControl.Crude:
                        sTail += TailLength == TailLength.Long ? ", but almost useless tail" : " and almost useless tail";
                        break;
                    case TailControl.Skillful:
                        sTail += TailLength == TailLength.Long ? " and very strong and flexible tail" : ", but very strong and flexible tail";
                        break;
                }
            }

            return sTail;
        }

        /// <summary>
        /// no tail
        /// </summary>
        public static TailGenetix None
        {
            get { return new TailGenetix(TailLength.None, TailControl.None); }
        }

        /// <summary>
        /// long flexible tail
        /// </summary>
        public static TailGenetix Monkey
        {
            get { return new TailGenetix(TailLength.Long, TailControl.Skillful); }
        }

        /// <summary>
        /// long, not so flexible tail
        /// </summary>
        public static TailGenetix Long
        {
            get { return new TailGenetix(TailLength.Long, TailControl.Crude); }
        }

        /// <summary>
        /// short, not so flexible tail
        /// </summary>
        public static TailGenetix Short
        {
            get { return new TailGenetix(TailLength.Short, TailControl.Crude); }
        }

        public TailLength TailLength { get; private set; } = TailLength.None;

        public TailControl TailControl { get; private set; } = TailControl.None;

        public bool IsIdentical(IGenetix pOther)
        {
            if (!(pOther is TailGenetix pAnother))
                return false;

            return TailLength == pAnother.TailLength &&
                TailControl == pAnother.TailControl;
        }

        public TailGenetix()
        { }

        public TailGenetix(TailGenetix pPredcessor)
        {
            TailLength = pPredcessor.TailLength;
            TailControl = pPredcessor.TailControl;
        }

        public TailGenetix(TailLength eTailLength, TailControl eTailControl)
        {
            TailLength = eTailLength;
            TailControl = eTailControl;

            if (TailLength == TailLength.None)
                TailControl = TailControl.None;

            if (TailLength == TailLength.Short && TailControl == TailControl.Skillful)
                TailControl = TailControl.Crude;

            if (TailLength == TailLength.Long && TailControl == TailControl.None)
                TailControl = TailControl.Crude;
        }

        public IGenetix MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                TailGenetix pMutant = new TailGenetix(this);

                if (pMutant.TailLength != TailLength.None || Rnd.OneChanceFrom(2))
                    pMutant.TailLength = (TailLength)Rnd.Get(typeof(TailLength));

                pMutant.TailControl = (TailControl)Rnd.Get(typeof(TailControl));

                if (pMutant.TailLength == TailLength.None)
                    pMutant.TailControl = TailControl.None;

                if (pMutant.TailLength == TailLength.Short && pMutant.TailControl == TailControl.Skillful)
                    pMutant.TailControl = TailControl.Crude;

                if (pMutant.TailLength == TailLength.Long && pMutant.TailControl == TailControl.None)
                    pMutant.TailControl = TailControl.Crude;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public IGenetix MutateGender()
        {
            if (Rnd.OneChanceFrom(10))
            {
                TailGenetix pMutant = new TailGenetix(this);

                if (pMutant.TailLength != TailLength.None)
                    pMutant.TailLength = (TailLength)Rnd.Get(typeof(TailLength));

                pMutant.TailControl = (TailControl)Rnd.Get(typeof(TailControl));

                if (pMutant.TailLength == TailLength.None)
                    pMutant.TailControl = TailControl.None;

                if (pMutant.TailLength == TailLength.Short && pMutant.TailControl == TailControl.Skillful)
                    pMutant.TailControl = TailControl.Crude;

                if (pMutant.TailLength == TailLength.Long && pMutant.TailControl == TailControl.None)
                    pMutant.TailControl = TailControl.Crude;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public IGenetix MutateNation()
        {
            if (Rnd.OneChanceFrom(20))
            {
                TailGenetix pMutant = new TailGenetix(this);

                if (pMutant.TailLength != TailLength.None)
                    pMutant.TailLength = (TailLength)Rnd.Get(typeof(TailLength));

                pMutant.TailControl = (TailControl)Rnd.Get(typeof(TailControl));

                if (pMutant.TailLength == TailLength.None)
                    pMutant.TailControl = TailControl.None;

                if (pMutant.TailLength == TailLength.Short && pMutant.TailControl == TailControl.Skillful)
                    pMutant.TailControl = TailControl.Crude;

                if (pMutant.TailLength == TailLength.Long && pMutant.TailControl == TailControl.None)
                    pMutant.TailControl = TailControl.Crude;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public IGenetix MutateFamily()
        {
            return this;
        }

        public IGenetix MutateIndividual()
        {
            return this;
        }
    }
}
