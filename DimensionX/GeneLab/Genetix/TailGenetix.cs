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

    public class TailGenetix: GenetixBase
    {
        /// <summary>
        /// a long, but almost useless tail
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string sTail = "";
            switch (m_eTailLength)
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
                switch (m_eTailControl)
                {
                    case TailControl.None:
                        sTail += m_eTailLength == TailLength.Long ? ", but absolutely useless tail" : " and absolutely useless tail";
                        break;
                    case TailControl.Crude:
                        sTail += m_eTailLength == TailLength.Long ? ", but almost useless tail" : " and almost useless tail";
                        break;
                    case TailControl.Skillful:
                        sTail += m_eTailLength == TailLength.Long ? " and very strong and flexible tail" : ", but very strong and flexible tail";
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

        public TailLength m_eTailLength = TailLength.None;

        public TailControl m_eTailControl = TailControl.None;

        public bool IsIdentical(GenetixBase pOther)
        {
            TailGenetix pAnother = pOther as TailGenetix;

            if (pAnother == null)
                return false;

            return m_eTailLength == pAnother.m_eTailLength &&
                m_eTailControl == pAnother.m_eTailControl;
        }
        
        public TailGenetix()
        { }

        public TailGenetix(TailGenetix pPredcessor)
        {
            m_eTailLength = pPredcessor.m_eTailLength;
            m_eTailControl = pPredcessor.m_eTailControl;
        }

        public TailGenetix(TailLength eTailLength, TailControl eTailControl)
        {
            m_eTailLength = eTailLength;
            m_eTailControl = eTailControl;

            if (m_eTailLength == TailLength.None)
                m_eTailControl = TailControl.None;

            if (m_eTailLength == TailLength.Short && m_eTailControl == TailControl.Skillful)
                m_eTailControl = TailControl.Crude;

            if (m_eTailLength == TailLength.Long && m_eTailControl == TailControl.None)
                m_eTailControl = TailControl.Crude;
        }
        
        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                TailGenetix pMutant = new TailGenetix(this);

                if (pMutant.m_eTailLength != TailLength.None || Rnd.OneChanceFrom(2))
                    pMutant.m_eTailLength = (TailLength)Rnd.Get(typeof(TailLength));

                pMutant.m_eTailControl = (TailControl)Rnd.Get(typeof(TailControl));

                if (pMutant.m_eTailLength == TailLength.None)
                    pMutant.m_eTailControl = TailControl.None;

                if (pMutant.m_eTailLength == TailLength.Short && pMutant.m_eTailControl == TailControl.Skillful)
                    pMutant.m_eTailControl = TailControl.Crude;

                if (pMutant.m_eTailLength == TailLength.Long && pMutant.m_eTailControl == TailControl.None)
                    pMutant.m_eTailControl = TailControl.Crude;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateGender()
        {
            if (Rnd.OneChanceFrom(10))
            {
                TailGenetix pMutant = new TailGenetix(this);

                if (pMutant.m_eTailLength != TailLength.None)
                    pMutant.m_eTailLength = (TailLength)Rnd.Get(typeof(TailLength));

                pMutant.m_eTailControl = (TailControl)Rnd.Get(typeof(TailControl));

                if (pMutant.m_eTailLength == TailLength.None)
                    pMutant.m_eTailControl = TailControl.None;

                if (pMutant.m_eTailLength == TailLength.Short && pMutant.m_eTailControl == TailControl.Skillful)
                    pMutant.m_eTailControl = TailControl.Crude;

                if (pMutant.m_eTailLength == TailLength.Long && pMutant.m_eTailControl == TailControl.None)
                    pMutant.m_eTailControl = TailControl.Crude;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateNation()
        {
            if (Rnd.OneChanceFrom(20))
            {
                TailGenetix pMutant = new TailGenetix(this);

                if (pMutant.m_eTailLength != TailLength.None)
                    pMutant.m_eTailLength = (TailLength)Rnd.Get(typeof(TailLength));

                pMutant.m_eTailControl = (TailControl)Rnd.Get(typeof(TailControl));

                if (pMutant.m_eTailLength == TailLength.None)
                    pMutant.m_eTailControl = TailControl.None;

                if (pMutant.m_eTailLength == TailLength.Short && pMutant.m_eTailControl == TailControl.Skillful)
                    pMutant.m_eTailControl = TailControl.Crude;

                if (pMutant.m_eTailLength == TailLength.Long && pMutant.m_eTailControl == TailControl.None)
                    pMutant.m_eTailControl = TailControl.Crude;

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
            return this;
        }
    }
}
