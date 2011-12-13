using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace GeneLab.Genetix
{
    public enum Intelligence
    {
        /// <summary>
        /// высшая мыслительная деятельность отсутствует полностью, есть только рефлексы
        /// </summary>
        None,
        /// <summary>
        /// высшая мыслительная деятельность есть, но крайне примитивная. Дрессировка и обучение невозможны.
        /// </summary>
        Basic,
        /// <summary>
        /// существо не разумное, но способное к обучению и дрессировке
        /// </summary>
        Capable,
        /// <summary>
        /// разумный, но недалёкий
        /// </summary>
        Primitive,
        /// <summary>
        /// нормальный разумный
        /// </summary>
        Sapient,
        /// <summary>
        /// высокоразумный
        /// </summary>
        Ingenious
    }

    public enum MagicAbilityPrevalence
    {
        /// <summary>
        /// маги - большая редкость
        /// </summary>
        Rare,
        /// <summary>
        /// маги - обычное дело
        /// </summary>
        Common,
        /// <summary>
        /// не маги - большая редкость
        /// </summary>
        AlmostEveryone
    }


    public class BrainGenetix: GenetixBase
    {
        public Intelligence m_eIntelligence = Intelligence.Sapient;

        public MagicAbilityPrevalence m_eMagicAbilityPrevalence = MagicAbilityPrevalence.Rare;

        public int m_iMagicAbilityPotential = 0;

        public BrainGenetix()
        { }

        public BrainGenetix(BrainGenetix pPredcessor)
        {
            m_eIntelligence = pPredcessor.m_eIntelligence;
            m_eMagicAbilityPrevalence = pPredcessor.m_eMagicAbilityPrevalence;
            m_iMagicAbilityPotential = pPredcessor.m_iMagicAbilityPotential;
        }

        public BrainGenetix(Intelligence eIntelligence, MagicAbilityPrevalence eMagicAbilityPrevalence, int iMagicAbilityPotential)
        {
            m_eIntelligence = eIntelligence;
            m_eMagicAbilityPrevalence = eMagicAbilityPrevalence;
            m_iMagicAbilityPotential = iMagicAbilityPotential;

            while (m_iMagicAbilityPotential > 8)
                m_iMagicAbilityPotential -= 8;

            while (m_iMagicAbilityPotential < 0)
                m_iMagicAbilityPotential += 8;

            if (m_eIntelligence == Intelligence.None)
                m_iMagicAbilityPotential = 0;

            if (m_eIntelligence == Intelligence.Basic ||
                m_eIntelligence == Intelligence.Capable)
                m_iMagicAbilityPotential = Math.Min(3, m_iMagicAbilityPotential); //для животных базовый уровень - хоть до джедаев/супергероев включительно... например, все единороги неразумны, но владеют магией на уровне джедаев

            if (m_eIntelligence == Intelligence.Primitive)
                m_iMagicAbilityPotential = 0; //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

            if (m_eIntelligence == Intelligence.Sapient)
                m_iMagicAbilityPotential = Math.Min(4, m_iMagicAbilityPotential); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

            if (m_eIntelligence == Intelligence.Ingenious)
                m_iMagicAbilityPotential = Math.Max(4, m_iMagicAbilityPotential); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
        }

        #region GenetixBase Members

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                BrainGenetix pMutant = new BrainGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    switch (pMutant.m_eIntelligence)
                    {
                        case Intelligence.None:
                            pMutant.m_eIntelligence = Intelligence.Basic;
                            break;
                        case Intelligence.Basic:
                            pMutant.m_eIntelligence = Intelligence.Capable;
                            break;
                        case Intelligence.Capable:
                            pMutant.m_eIntelligence = Intelligence.Basic;
                            break;
                        case Intelligence.Primitive:
                            pMutant.m_eIntelligence = Intelligence.Sapient;
                            break;
                        case Intelligence.Sapient:
                            pMutant.m_eIntelligence = Rnd.OneChanceFrom(2) ? Intelligence.Primitive : Intelligence.Ingenious;
                            break;
                        case Intelligence.Ingenious:
                            pMutant.m_eIntelligence = Intelligence.Sapient;
                            break;
                    }
                }

                if (Rnd.OneChanceFrom(2))
                {
                    pMutant.m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.Get(typeof(MagicAbilityPrevalence));

                    //дополнительно снижаем частоту распространения магических способностей среди животных
                    if ((pMutant.m_eIntelligence == Intelligence.None ||
                         pMutant.m_eIntelligence == Intelligence.Basic ||
                         pMutant.m_eIntelligence == Intelligence.Capable) &&
                        Rnd.OneChanceFrom(2))
                        pMutant.m_eMagicAbilityPrevalence = MagicAbilityPrevalence.Rare;
                }

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.m_eIntelligence == Intelligence.None)
                        pMutant.m_iMagicAbilityPotential = 0;

                    if (pMutant.m_eIntelligence == Intelligence.Basic ||
                        pMutant.m_eIntelligence == Intelligence.Capable)
                        pMutant.m_iMagicAbilityPotential = Rnd.Get(4); //для животных базовый уровень - хоть до джедаев/супергероев включительно... например, все единороги неразумны, но владеют магией на уровне джедаев

                    if (pMutant.m_eIntelligence == Intelligence.Primitive)
                        pMutant.m_iMagicAbilityPotential = 0; //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.m_eIntelligence == Intelligence.Sapient)
                        pMutant.m_iMagicAbilityPotential = Rnd.Get(5); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.m_eIntelligence == Intelligence.Ingenious)
                        pMutant.m_iMagicAbilityPotential = 4 + Rnd.Get(5); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }
                else
                {
                    if (pMutant.m_eIntelligence == Intelligence.Primitive)
                        pMutant.m_iMagicAbilityPotential = 0; //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.m_eIntelligence == Intelligence.Sapient)
                        pMutant.m_iMagicAbilityPotential = Math.Min(4, pMutant.m_iMagicAbilityPotential); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.m_eIntelligence == Intelligence.Ingenious)
                        pMutant.m_iMagicAbilityPotential = Math.Max(4, pMutant.m_iMagicAbilityPotential); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }

                return pMutant;
            }

            return this;
        }

        public GenetixBase MutateNation()
        {
            if (Rnd.OneChanceFrom(20))
            {
                BrainGenetix pMutant = new BrainGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    pMutant.m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.Get(typeof(MagicAbilityPrevalence));

                    //дополнительно снижаем частоту распространения магических способностей среди животных
                    if ((pMutant.m_eIntelligence == Intelligence.None ||
                         pMutant.m_eIntelligence == Intelligence.Basic ||
                         pMutant.m_eIntelligence == Intelligence.Capable) &&
                        Rnd.OneChanceFrom(2))
                        pMutant.m_eMagicAbilityPrevalence = MagicAbilityPrevalence.Rare;
                }

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.m_eIntelligence == Intelligence.None)
                        pMutant.m_iMagicAbilityPotential = 0;

                    if (pMutant.m_eIntelligence == Intelligence.Basic ||
                        pMutant.m_eIntelligence == Intelligence.Capable)
                        pMutant.m_iMagicAbilityPotential = Rnd.Get(4); //для животных базовый уровень - хоть до джедаев/супергероев включительно... например, все единороги неразумны, но владеют магией на уровне джедаев

                    if (pMutant.m_eIntelligence == Intelligence.Primitive)
                        pMutant.m_iMagicAbilityPotential = 0; //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.m_eIntelligence == Intelligence.Sapient)
                        pMutant.m_iMagicAbilityPotential = Rnd.Get(5); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.m_eIntelligence == Intelligence.Ingenious)
                        pMutant.m_iMagicAbilityPotential = 4 + Rnd.Get(5); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }
                else
                {
                    if (pMutant.m_eIntelligence == Intelligence.Primitive)
                        pMutant.m_iMagicAbilityPotential = 0; //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.m_eIntelligence == Intelligence.Sapient)
                        pMutant.m_iMagicAbilityPotential = Math.Min(4, pMutant.m_iMagicAbilityPotential); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.m_eIntelligence == Intelligence.Ingenious)
                        pMutant.m_iMagicAbilityPotential = Math.Max(4, pMutant.m_iMagicAbilityPotential); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }

                return pMutant;
            }

            return this;
        }

        public GenetixBase MutateFamily()
        {
            if (Rnd.OneChanceFrom(5))
            {
                BrainGenetix pMutant = new BrainGenetix(this);

                if (Rnd.OneChanceFrom(10))
                {
                    switch (pMutant.m_eIntelligence)
                    {
                        case Intelligence.Basic:
                            pMutant.m_eIntelligence = Intelligence.Capable;
                            break;
                        case Intelligence.Capable:
                            pMutant.m_eIntelligence = Intelligence.Basic;
                            break;
                        case Intelligence.Primitive:
                            pMutant.m_eIntelligence = Intelligence.Sapient;
                            break;
                        case Intelligence.Sapient:
                            pMutant.m_eIntelligence = Rnd.OneChanceFrom(2) ? Intelligence.Primitive : Intelligence.Ingenious;
                            break;
                        case Intelligence.Ingenious:
                            pMutant.m_eIntelligence = Intelligence.Sapient;
                            break;
                    }
                } 
                
                if (Rnd.OneChanceFrom(2))
                {
                    pMutant.m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.Get(typeof(MagicAbilityPrevalence));

                    //дополнительно снижаем частоту распространения магических способностей среди животных
                    if ((pMutant.m_eIntelligence == Intelligence.None ||
                         pMutant.m_eIntelligence == Intelligence.Basic ||
                         pMutant.m_eIntelligence == Intelligence.Capable) &&
                        Rnd.OneChanceFrom(2))
                        pMutant.m_eMagicAbilityPrevalence = MagicAbilityPrevalence.Rare;
                }

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.m_eIntelligence == Intelligence.None)
                        pMutant.m_iMagicAbilityPotential = 0;

                    if (pMutant.m_eIntelligence == Intelligence.Basic ||
                        pMutant.m_eIntelligence == Intelligence.Capable)
                        pMutant.m_iMagicAbilityPotential = Rnd.Get(4); //для животных базовый уровень - хоть до джедаев/супергероев включительно... например, все единороги неразумны, но владеют магией на уровне джедаев

                    if (pMutant.m_eIntelligence == Intelligence.Primitive)
                        pMutant.m_iMagicAbilityPotential = 0; //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.m_eIntelligence == Intelligence.Sapient)
                        pMutant.m_iMagicAbilityPotential = Rnd.Get(5); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.m_eIntelligence == Intelligence.Ingenious)
                        pMutant.m_iMagicAbilityPotential = 4 + Rnd.Get(5); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }
                else
                {
                    if (pMutant.m_eIntelligence == Intelligence.Primitive)
                        pMutant.m_iMagicAbilityPotential = 0; //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.m_eIntelligence == Intelligence.Sapient)
                        pMutant.m_iMagicAbilityPotential = Math.Min(4, pMutant.m_iMagicAbilityPotential); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.m_eIntelligence == Intelligence.Ingenious)
                        pMutant.m_iMagicAbilityPotential = Math.Max(4, pMutant.m_iMagicAbilityPotential); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }

                return pMutant;
            }

            return this;
        }

        public GenetixBase MutateIndividual()
        {
            if (Rnd.OneChanceFrom(50))
            {
                BrainGenetix pMutant = new BrainGenetix(this);

                switch (pMutant.m_eIntelligence)
                {
                    case Intelligence.Basic:
                        pMutant.m_eIntelligence = Intelligence.Capable;
                        break;
                    case Intelligence.Capable:
                        pMutant.m_eIntelligence = Intelligence.Basic;
                        break;
                    case Intelligence.Primitive:
                        pMutant.m_eIntelligence = Intelligence.Sapient;
                        break;
                    case Intelligence.Sapient:
                        pMutant.m_eIntelligence = Rnd.OneChanceFrom(2) ? Intelligence.Primitive : Intelligence.Ingenious;
                        break;
                    case Intelligence.Ingenious:
                        pMutant.m_eIntelligence = Intelligence.Sapient;
                        break;
                }

                return pMutant;
            }

            return this;
        }

        #endregion
    }
}
