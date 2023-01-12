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
        /// разумный, но тупой
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

    //public enum MagicAbilityPrevalence
    //{
    //    /// <summary>
    //    /// маги - большая редкость
    //    /// </summary>
    //    Rare,
    //    /// <summary>
    //    /// маги - обычное дело
    //    /// </summary>
    //    Common,
    //    /// <summary>
    //    /// не маги - большая редкость
    //    /// </summary>
    //    AlmostEveryone
    //}

    public class BrainGenetix: IGenetix
    {
        /// <summary>
        /// are very clever creatures and can use some psionic abilities
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return GetDescription(true);
        }
        /// <summary>
        /// are very clever creature(s) and can use some psionic abilities
        /// </summary>
        /// <returns></returns>
        public string GetDescription(bool bPlural)
        {
            string sIntellect = "?";
            switch (Intelligence)
            {
                case Intelligence.None:
                    sIntellect = "are brainless creature";
                    break;
                case Intelligence.Basic:
                    sIntellect = "are not so clever creature";
                    break;
                case Intelligence.Capable:
                    sIntellect = "are very clever creature";
                    break;
                case Intelligence.Primitive:
                    sIntellect = "have quite low IQ";
                    break;
                case Intelligence.Sapient:
                    sIntellect = "have average IQ";
                    break;
                case Intelligence.Ingenious:
                    sIntellect = "have extremely high IQ";
                    break;
            }

            if (bPlural && Intelligence < Intelligence.Primitive)
                sIntellect += "s";

            string sMagicForce = "?";
            switch (MagicAbilityPotential)
            {
                case 0:
                    sMagicForce = "can't use any magic";
                    break;
                case 1:
                    sMagicForce = "can see the unseen";
                    break;
                case 2:
                    sMagicForce = "can use some psionic abilities";
                    break;
                case 3:
                    sMagicForce = "can use super-powers";
                    break;
                case 4:
                    sMagicForce = "can use average magic";
                    break;
                case 5:
                    sMagicForce = "can use powerful magic";
                    break;
                case 6:
                    sMagicForce = "can use extremely powerful magic";
                    break;
                case 7:
                    sMagicForce = "have a god-like supernatural potencial";
                    break;
                case 8:
                    sMagicForce = "have an omnipotence supernatural potencial";
                    break;
            }

            return sIntellect + " and " + /*sMagic + */sMagicForce;
        }

        /// <summary>
        /// sapient, average magic
        /// </summary>
        public static BrainGenetix HumanFantasy
        {
            get { return new BrainGenetix(Intelligence.Sapient, /*MagicAbilityPrevalence.Common,*/ 4); }
        }

        /// <summary>
        /// sapient, no magic
        /// </summary>
        public static BrainGenetix HumanReal
        {
            get { return new BrainGenetix(Intelligence.Sapient, /*MagicAbilityPrevalence.Rare,*/ 0); }
        }

        /// <summary>
        /// sapient, limited magic (supers)
        /// </summary>
        public static BrainGenetix HumanSF
        {
            get { return new BrainGenetix(Intelligence.Sapient, /*MagicAbilityPrevalence.Rare,*/ 3); }
        }

        /// <summary>
        /// ingenious, average magic
        /// </summary>
        public static BrainGenetix Elf
        {
            get { return new BrainGenetix(Intelligence.Ingenious, /*MagicAbilityPrevalence.Common,*/ 4); }
        }

        /// <summary>
        /// primitive, low magic (shamanism)
        /// </summary>
        public static BrainGenetix Barbarian
        {
            get { return new BrainGenetix(Intelligence.Primitive, /*MagicAbilityPrevalence.Rare,*/ 2); }
        }

        public Intelligence Intelligence { get; private set; } = Intelligence.Sapient;

        //public MagicAbilityPrevalence m_eMagicAbilityPrevalence = MagicAbilityPrevalence.Rare;

        public int MagicAbilityPotential { get; private set; } = 0;

        public bool IsIdentical(IGenetix pOther)
        {
            if (!(pOther is BrainGenetix pAnother))
                return false;

            return Intelligence == pAnother.Intelligence &&
                //m_eMagicAbilityPrevalence == pAnother.m_eMagicAbilityPrevalence &&
                MagicAbilityPotential == pAnother.MagicAbilityPotential;
        }

        public BrainGenetix()
        { }

        public BrainGenetix(BrainGenetix pPredcessor)
        {
            Intelligence = pPredcessor.Intelligence;
            //m_eMagicAbilityPrevalence = pPredcessor.m_eMagicAbilityPrevalence;
            MagicAbilityPotential = pPredcessor.MagicAbilityPotential;
        }

        public BrainGenetix(Intelligence eIntelligence, /*MagicAbilityPrevalence eMagicAbilityPrevalence, */int iMagicAbilityPotential)
        {
            Intelligence = eIntelligence;
            //m_eMagicAbilityPrevalence = eMagicAbilityPrevalence;
            MagicAbilityPotential = iMagicAbilityPotential;

            while (MagicAbilityPotential > 8)
                MagicAbilityPotential -= 8;

            while (MagicAbilityPotential < 0)
                MagicAbilityPotential += 8;

            if (Intelligence == Intelligence.None)
                MagicAbilityPotential = 0;

            if (Intelligence == Intelligence.Basic ||
                Intelligence == Intelligence.Capable)
            {
                MagicAbilityPotential = Math.Min(3, MagicAbilityPotential); //для животных базовый уровень - хоть до джедаев/супергероев включительно... например, все единороги неразумны, но владеют магией на уровне джедаев
            }

            if (Intelligence == Intelligence.Primitive)
                MagicAbilityPotential = 0; //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

            if (Intelligence == Intelligence.Sapient)
                MagicAbilityPotential = Math.Min(4, MagicAbilityPotential); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

            if (Intelligence == Intelligence.Ingenious)
                MagicAbilityPotential = Math.Max(4, MagicAbilityPotential); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
        }

        #region GenetixBase Members

        public IGenetix MutateRace()
        {
            if (Rnd.OneChanceFrom(5))
            {
                BrainGenetix pMutant = new BrainGenetix(this);

                if (Rnd.OneChanceFrom(4))
                {
                    switch (pMutant.Intelligence)
                    {
                        case Intelligence.None:
                            pMutant.Intelligence = Intelligence.Basic;
                            break;
                        case Intelligence.Basic:
                            pMutant.Intelligence = Intelligence.Capable;
                            break;
                        case Intelligence.Capable:
                            pMutant.Intelligence = Intelligence.Basic;
                            break;
                        case Intelligence.Primitive:
                            pMutant.Intelligence = Intelligence.Sapient;
                            break;
                        case Intelligence.Sapient:
                            pMutant.Intelligence = Rnd.OneChanceFrom(2) ? Intelligence.Primitive : Intelligence.Ingenious;
                            break;
                        case Intelligence.Ingenious:
                            pMutant.Intelligence = Intelligence.Sapient;
                            break;
                    }
                }

                //if (Rnd.OneChanceFrom(2))
                //{
                //    pMutant.m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.Get(typeof(MagicAbilityPrevalence));

                //    //дополнительно снижаем частоту распространения магических способностей среди животных
                //    if ((pMutant.m_eIntelligence == Intelligence.None ||
                //         pMutant.m_eIntelligence == Intelligence.Basic ||
                //         pMutant.m_eIntelligence == Intelligence.Capable) &&
                //        Rnd.OneChanceFrom(2))
                //        pMutant.m_eMagicAbilityPrevalence = MagicAbilityPrevalence.Rare;
                //}

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.Intelligence == Intelligence.None)
                        pMutant.MagicAbilityPotential = 0;

                    if (pMutant.Intelligence == Intelligence.Basic ||
                        pMutant.Intelligence == Intelligence.Capable)
                    {
                        pMutant.MagicAbilityPotential = Rnd.Get(4); //для животных базовый уровень - хоть до джедаев/супергероев включительно... например, все единороги неразумны, но владеют магией на уровне джедаев
                    }

                    if (pMutant.Intelligence == Intelligence.Primitive)
                        pMutant.MagicAbilityPotential = Rnd.Get(2); //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.Intelligence == Intelligence.Sapient)
                        pMutant.MagicAbilityPotential = Rnd.Get(5); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.Intelligence == Intelligence.Ingenious)
                        pMutant.MagicAbilityPotential = 4 + Rnd.Get(5); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }
                else
                {
                    if (pMutant.Intelligence == Intelligence.Primitive)
                        pMutant.MagicAbilityPotential = 0; //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.Intelligence == Intelligence.Sapient)
                        pMutant.MagicAbilityPotential = Math.Min(4, pMutant.MagicAbilityPotential); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.Intelligence == Intelligence.Ingenious)
                        pMutant.MagicAbilityPotential = Math.Max(4, pMutant.MagicAbilityPotential); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public IGenetix MutateGender()
        {
            if (Rnd.OneChanceFrom(10))
            {
                BrainGenetix pMutant = new BrainGenetix(this);

                if (Rnd.OneChanceFrom(10))
                {
                    switch (pMutant.Intelligence)
                    {
                        case Intelligence.None:
                            pMutant.Intelligence = Intelligence.Basic;
                            break;
                        case Intelligence.Basic:
                            pMutant.Intelligence = Intelligence.Capable;
                            break;
                        case Intelligence.Capable:
                            pMutant.Intelligence = Intelligence.Basic;
                            break;
                        case Intelligence.Primitive:
                            pMutant.Intelligence = Rnd.OneChanceFrom(2) ? Intelligence.Sapient : Intelligence.Capable;
                            break;
                        case Intelligence.Sapient:
                            pMutant.Intelligence = Rnd.OneChanceFrom(2) ? Intelligence.Primitive : Intelligence.Ingenious;
                            break;
                        case Intelligence.Ingenious:
                            pMutant.Intelligence = Intelligence.Sapient;
                            break;
                    }
                }

                //if (Rnd.OneChanceFrom(2))
                //{
                //    pMutant.m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.Get(typeof(MagicAbilityPrevalence));

                //    //дополнительно снижаем частоту распространения магических способностей среди животных
                //    if ((pMutant.m_eIntelligence == Intelligence.None ||
                //         pMutant.m_eIntelligence == Intelligence.Basic ||
                //         pMutant.m_eIntelligence == Intelligence.Capable) &&
                //        Rnd.OneChanceFrom(2))
                //        pMutant.m_eMagicAbilityPrevalence = MagicAbilityPrevalence.Rare;
                //}

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.Intelligence == Intelligence.None)
                        pMutant.MagicAbilityPotential = 0;

                    if (pMutant.Intelligence == Intelligence.Basic ||
                        pMutant.Intelligence == Intelligence.Capable)
                    {
                        pMutant.MagicAbilityPotential = Rnd.Get(4); //для животных базовый уровень - хоть до джедаев/супергероев включительно... например, все единороги неразумны, но владеют магией на уровне джедаев
                    }

                    if (pMutant.Intelligence == Intelligence.Primitive)
                        pMutant.MagicAbilityPotential = Rnd.Get(2); //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.Intelligence == Intelligence.Sapient)
                        pMutant.MagicAbilityPotential = Rnd.Get(5); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.Intelligence == Intelligence.Ingenious)
                        pMutant.MagicAbilityPotential = 4 + Rnd.Get(5); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }
                else
                {
                    if (pMutant.Intelligence == Intelligence.Primitive)
                        pMutant.MagicAbilityPotential = 0; //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.Intelligence == Intelligence.Sapient)
                        pMutant.MagicAbilityPotential = Math.Min(4, pMutant.MagicAbilityPotential); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.Intelligence == Intelligence.Ingenious)
                        pMutant.MagicAbilityPotential = Math.Max(4, pMutant.MagicAbilityPotential); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public IGenetix MutateNation()
        {
            if (Rnd.OneChanceFrom(20))
            {
                BrainGenetix pMutant = new BrainGenetix(this);

                //if (Rnd.OneChanceFrom(2))
                //{
                //    pMutant.m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.Get(typeof(MagicAbilityPrevalence));

                //    //дополнительно снижаем частоту распространения магических способностей среди животных
                //    if ((pMutant.m_eIntelligence == Intelligence.None ||
                //         pMutant.m_eIntelligence == Intelligence.Basic ||
                //         pMutant.m_eIntelligence == Intelligence.Capable) &&
                //        Rnd.OneChanceFrom(2))
                //        pMutant.m_eMagicAbilityPrevalence = MagicAbilityPrevalence.Rare;
                //}

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.Intelligence == Intelligence.None)
                        pMutant.MagicAbilityPotential = 0;

                    if (pMutant.Intelligence == Intelligence.Basic ||
                        pMutant.Intelligence == Intelligence.Capable)
                    {
                        pMutant.MagicAbilityPotential = Rnd.Get(4); //для животных базовый уровень - хоть до джедаев/супергероев включительно... например, все единороги неразумны, но владеют магией на уровне джедаев
                    }

                    if (pMutant.Intelligence == Intelligence.Primitive)
                        pMutant.MagicAbilityPotential = Rnd.Get(2); //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.Intelligence == Intelligence.Sapient)
                        pMutant.MagicAbilityPotential = Rnd.Get(5); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.Intelligence == Intelligence.Ingenious)
                        pMutant.MagicAbilityPotential = 4 + Rnd.Get(5); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }
                else
                {
                    if (pMutant.Intelligence == Intelligence.Primitive)
                        pMutant.MagicAbilityPotential = 0; //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.Intelligence == Intelligence.Sapient)
                        pMutant.MagicAbilityPotential = Math.Min(4, pMutant.MagicAbilityPotential); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.Intelligence == Intelligence.Ingenious)
                        pMutant.MagicAbilityPotential = Math.Max(4, pMutant.MagicAbilityPotential); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public IGenetix MutateFamily()
        {
            if (Rnd.OneChanceFrom(5))
            {
                BrainGenetix pMutant = new BrainGenetix(this);

                if (Rnd.OneChanceFrom(10))
                {
                    switch (pMutant.Intelligence)
                    {
                        case Intelligence.Basic:
                            pMutant.Intelligence = Intelligence.Capable;
                            break;
                        case Intelligence.Capable:
                            pMutant.Intelligence = Intelligence.Basic;
                            break;
                        case Intelligence.Primitive:
                            pMutant.Intelligence = Intelligence.Sapient;
                            break;
                        case Intelligence.Sapient:
                            pMutant.Intelligence = Rnd.OneChanceFrom(2) ? Intelligence.Primitive : Intelligence.Ingenious;
                            break;
                        case Intelligence.Ingenious:
                            pMutant.Intelligence = Intelligence.Sapient;
                            break;
                    }
                }

                //if (Rnd.OneChanceFrom(2))
                //{
                //    pMutant.m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.Get(typeof(MagicAbilityPrevalence));

                //    //дополнительно снижаем частоту распространения магических способностей среди животных
                //    if ((pMutant.m_eIntelligence == Intelligence.None ||
                //         pMutant.m_eIntelligence == Intelligence.Basic ||
                //         pMutant.m_eIntelligence == Intelligence.Capable) &&
                //        Rnd.OneChanceFrom(2))
                //        pMutant.m_eMagicAbilityPrevalence = MagicAbilityPrevalence.Rare;
                //}

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.Intelligence == Intelligence.None)
                        pMutant.MagicAbilityPotential = 0;

                    if (pMutant.Intelligence == Intelligence.Basic ||
                        pMutant.Intelligence == Intelligence.Capable)
                    {
                        pMutant.MagicAbilityPotential = Rnd.Get(4); //для животных базовый уровень - хоть до джедаев/супергероев включительно... например, все единороги неразумны, но владеют магией на уровне джедаев
                    }

                    if (pMutant.Intelligence == Intelligence.Primitive)
                        pMutant.MagicAbilityPotential = Rnd.Get(2); //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.Intelligence == Intelligence.Sapient)
                        pMutant.MagicAbilityPotential = Rnd.Get(5); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.Intelligence == Intelligence.Ingenious)
                        pMutant.MagicAbilityPotential = 4 + Rnd.Get(5); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }
                else
                {
                    if (pMutant.Intelligence == Intelligence.Primitive)
                        pMutant.MagicAbilityPotential = 0; //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.Intelligence == Intelligence.Sapient)
                        pMutant.MagicAbilityPotential = Math.Min(4, pMutant.MagicAbilityPotential); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.Intelligence == Intelligence.Ingenious)
                        pMutant.MagicAbilityPotential = Math.Max(4, pMutant.MagicAbilityPotential); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public IGenetix MutateIndividual()
        {
            if (Rnd.OneChanceFrom(5))
            {
                BrainGenetix pMutant = new BrainGenetix(this);

                if (Rnd.OneChanceFrom(10))
                {
                    switch (pMutant.Intelligence)
                    {
                        case Intelligence.Basic:
                            pMutant.Intelligence = Intelligence.Capable;
                            break;
                        case Intelligence.Capable:
                            pMutant.Intelligence = Intelligence.Basic;
                            break;
                        case Intelligence.Primitive:
                            pMutant.Intelligence = Intelligence.Sapient;
                            break;
                        case Intelligence.Sapient:
                            pMutant.Intelligence = Rnd.OneChanceFrom(2) ? Intelligence.Primitive : Intelligence.Ingenious;
                            break;
                        case Intelligence.Ingenious:
                            pMutant.Intelligence = Intelligence.Sapient;
                            break;
                    }
                }

                if (Rnd.OneChanceFrom(10))
                {
                    if (pMutant.Intelligence == Intelligence.None)
                        pMutant.MagicAbilityPotential = 0;

                    if (pMutant.Intelligence == Intelligence.Basic ||
                        pMutant.Intelligence == Intelligence.Capable)
                    {
                        pMutant.MagicAbilityPotential = Rnd.Get(4); //для животных базовый уровень - хоть до джедаев/супергероев включительно... например, все единороги неразумны, но владеют магией на уровне джедаев
                    }

                    if (pMutant.Intelligence == Intelligence.Primitive)
                        pMutant.MagicAbilityPotential = Rnd.Get(2); //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.Intelligence == Intelligence.Sapient)
                        pMutant.MagicAbilityPotential = Rnd.Get(5); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.Intelligence == Intelligence.Ingenious)
                        pMutant.MagicAbilityPotential = 4 + Rnd.Get(5); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }
                else
                {
                    if (pMutant.Intelligence == Intelligence.Primitive)
                        pMutant.MagicAbilityPotential = 0; //примитивные народы в массе своей магией не владеют вообще, могущественными магами могут быть только единицы, даже если весь народ обладает магическими способностями

                    if (pMutant.Intelligence == Intelligence.Sapient)
                        pMutant.MagicAbilityPotential = Math.Min(4, pMutant.MagicAbilityPotential); //обычные люди в массе могут владеть магией не выше уровня стандартных фэнтезийным магов

                    if (pMutant.Intelligence == Intelligence.Ingenious)
                        pMutant.MagicAbilityPotential = Math.Max(4, pMutant.MagicAbilityPotential); //высокоразумные расы с лёгкостью осваивают магию до уровня стандартных фэнтезийным магов, дальше как получится
                }

                return pMutant;
            }

            return this;
        }

        #endregion
    }
}
