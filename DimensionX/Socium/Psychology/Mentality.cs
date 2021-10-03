using BenTools.Mathematics;
using Random;
using System;
using System.Collections.Generic;

namespace Socium.Psychology
{
    /// <summary>
    /// Черты характера
    /// </summary>
    public enum Trait
    {
        /// <summary>
        /// Агрессивность - нападать на врага или убегать от него
        /// </summary>
        Agression,
        /// <summary>
        /// Нетерпимость - какой уровень неприязни считать достаточным чтобы признать врагом
        /// </summary>
        Fanaticism, //Fanaticism,
        /// <summary>
        /// Набожность - насколько человек признаёт над собой власть высших сил
        /// </summary>
        Piety, //Voluptuousness,
        /// <summary>
        /// Вероломство - склонность к обману и мошенничеству
        /// </summary>
        Treachery, //Guile,
        /// <summary>
        /// Эгоизм, забота только о собственном благе, в противоположность заботе о благе своего общества
        /// </summary>
        Selfishness, //Selfishness,
        // <summary>
        // Высокомерие - важность социального статуса при общении и назначении должностей (статус определяется соответствием (или несоответствием) индивидуума определённым для данного общества критериям - половая принадлежность, возраст, богатство, внешность, интеллект, магические способности и пр.)
        // (Отказался, т.к. это дублирует фанатизм)
        // </summary>
        //Segregation, //Arrogance,
        /// <summary>
        /// Простота - склонность к простым удовольствиям, непонимание высокого искусства.
        /// </summary>
        Simplicity//Exploitation
    };
    
    /// <summary>
    /// Менталитет - набор черт характера, сопутствующих друг-другу
    /// </summary>
    public class Mentality
    {
        public static Array AllTraits = Enum.GetValues(typeof(Trait));

        private Dictionary<Trait, float[]> m_cTraits = new Dictionary<Trait, float[]>();
        /// <summary>
        /// Моральные качества - в диапазоне 0..2
        /// </summary>
        public Dictionary<Trait, float[]> Traits
        {
            get { return m_cTraits; }
        }

        public Mentality(MentalityTemplate cTemplate)
        {
            foreach (Trait prop in AllTraits)
            {
                m_cTraits[prop] = new float[9];
                float[] aDerivative = new float[9];//произвдных на 1 больше, чем уровней, чтобы последний уровень был меньше 2.0
                float fSum = 0;


                AdvancementRate eRate = cTemplate[prop];
                if (eRate == AdvancementRate.Random)
                {
                    Array aRates = Enum.GetValues(typeof(AdvancementRate));
                    eRate = (AdvancementRate)aRates.GetValue(Rnd.Get(aRates.Length - 1));
                }
                for (int i = 0; i < 9; i++)
                {
                    switch (eRate)
                    {
                        case AdvancementRate.UniformlyLoose:
                            aDerivative[i] = 0.05f + Rnd.Get(0.35f);
                            break;
                        case AdvancementRate.UniformlyModerate:
                            aDerivative[i] = 0.1f + Rnd.Get(0.25f);
                            break;
                        case AdvancementRate.UniformlyPrecise:
                            aDerivative[i] = 0.15f + Rnd.Get(0.15f);
                            break;
                        case AdvancementRate.Delayed:
                            aDerivative[i] = 0.45f * i / 8 + Rnd.Get(0.03f);
                            break;
                        case AdvancementRate.Rapid:
                            aDerivative[i] = 0.45f - 0.45f * i / 8 + Rnd.Get(0.03f);
                            break;
                        case AdvancementRate.Leap:
                            if (i < 4)
                                aDerivative[i] = 0.5f * i / 4 + Rnd.Get(0.03f);
                            else
                                aDerivative[i] = 0.5f - 0.5f * (i - 4) / 4 + Rnd.Get(0.03f);
                            break;
                        case AdvancementRate.Plateau:
                            if (i < 4)
                                aDerivative[i] = 0.5f - 0.5f * i / 4 + Rnd.Get(0.03f);
                            else
                                aDerivative[i] = 0.5f * (i - 4) / 4 + Rnd.Get(0.03f);
                            break;
                        //case 5:
                        //    aDerivative[i] = 0.7f * (float)Math.Pow((float)i / 8, 4) + Rnd.Get(0.01f);
                        //    break;
                    }
                    fSum += aDerivative[i];
                }

                float fCurrent = 2;
                for (int i = 0; i < 9; i++)
                {
                    fCurrent -= aDerivative[i]*2 / fSum; 
                    m_cTraits[prop][i] = Math.Max(0, fCurrent);
                }
            }
        }

        public Mentality(float fValue)
        {
            foreach (Trait prop in AllTraits)
            {
                m_cTraits[prop] = new float[9];
                for (int i = 0; i < 9; i++) 
                    m_cTraits[prop][i] = Math.Min(2.0f, Math.Max(0, fValue));
            }
        }

        private static Mentality s_pIdeal = new Mentality(0);

        public static Mentality IdealSociety
        {
            get { return s_pIdeal; }
        }

        public Mentality(Mentality pAncestorMentality, bool bMutate)
        {
            foreach (Trait eTrait in AllTraits)
            {
                if (bMutate)
                {
                    m_cTraits[eTrait] = new float[9];
                    float[] aDerivative = new float[9];//произвдных на 1 больше, чем уровней, чтобы последний уровень был меньше 2.0
                    float fSum = 2;
                    for (int i = 0; i < 9; i++)
                    {
                        aDerivative[i] = fSum - pAncestorMentality.m_cTraits[eTrait][i];
                        fSum -= aDerivative[i];
                    }
                    //aDerivative[9] = fSum;
                    fSum = 0;
                    for (int i = 0; i < 9; i++)
                    {
                        aDerivative[i] = (9 * aDerivative[i] + 0.05f + Rnd.Get(0.4f)) / 10;
                        fSum += aDerivative[i];
                    }

                    float fCurrent = 2;
                    for (int i = 0; i < 9; i++)
                    {
                        fCurrent -= aDerivative[i] * 2 / fSum;
                        m_cTraits[eTrait][i] = Math.Max(0, fCurrent);
                    }
                }
                else
                {
                    m_cTraits[eTrait] = new float[9];
                    for (int i = 0; i < 9; i++)
                        m_cTraits[eTrait][i] = pAncestorMentality.m_cTraits[eTrait][i];
                }
            }
        }

        //public void Evolve()
        //{
        //    foreach (Mentality eMentality in Mentalities)
        //    {
        //        //ментальные качества у нас отрицательные, поэтому для того, чтобы эволюционировать, их значения нужно снизить.
        //        float fFutureValue = (float)Math.Pow(Rnd.Get(1.18f), 4);// (float)Math.Pow(Rnd.Get(2.0f), 2) / 2.0f;
        //        if (fFutureValue < m_cMentalityValues[eMentality])
        //            m_cMentalityValues[eMentality] = (fFutureValue + 12 * m_cMentalityValues[eMentality]) / 13;
        //    }
        //}

        //public void Degrade()
        //{
        //    foreach (Mentality eMentality in Mentalities)
        //    {
        //        //ментальные качества у нас отрицательные, поэтому для того, чтобы деградировать, их значения нужно поднять.
        //        float fFutureValue = 2.0f - (float)Math.Pow(Rnd.Get(1.18f), 4);
        //        if (fFutureValue > m_cMentalityValues[eMentality])
        //            m_cMentalityValues[eMentality] = (fFutureValue + 24 * m_cMentalityValues[eMentality]) / 25;
        //    }
        //}

        /// <summary>
        /// Различие между культурами от +1 (полная противоположность) до -1 (полное совпадение)
        /// </summary>
        /// <param name="different">другая культура</param>
        /// <returns>скалярное произведение нормализованных векторов культуры / (количество моральных качеств)</returns>
        public float GetDifference(Mentality different, int iOwnProgress, int iOpponentProgress)
        {
            BTVector culture1 = new BTVector(AllTraits.Length);
            BTVector culture2 = new BTVector(AllTraits.Length);
            foreach (Trait prop in AllTraits)
            {
                culture1.Set((int)prop, m_cTraits[prop][iOwnProgress] - 1.0f);
                culture2.Set((int)prop, different.m_cTraits[prop][iOpponentProgress] - 1.0f);
            }
            return (float)Math.Sqrt(BTVector.Dist(culture1, culture2)) - 1f;
            //return -(float)(culture1 * culture2) / Mentalities.Length;
        }

        /// <summary>
        /// Именованная фаза развития определённой черты характера
        /// </summary>
        private class TraitPhase
        {
            public float m_fMinValue;
            public float m_fMaxValue;
            public string m_sName;
            private Trait m_eTrait;

            public TraitPhase(Trait eTrait, float fMin, float fMax, string sName)
            {
                m_eTrait = eTrait;
                m_fMinValue = fMin;
                m_fMaxValue = fMax;
                m_sName = sName;
            }

            public bool Check(Mentality pMentality, int iLevel)
            {
                return pMentality.m_cTraits[m_eTrait][iLevel] >= m_fMinValue &&
                    pMentality.m_cTraits[m_eTrait][iLevel] <= m_fMaxValue;
            }
        }

        /// <summary>
        /// НЕ ИСПОЛЬЗОВАТЬ!
        /// Обращаться к свойству MentalityClusters.
        /// </summary>
        private static Dictionary<Trait, TraitPhase[]> s_cTraitPhases = null;

        private static Dictionary<Trait, TraitPhase[]> TraitPhases
        {
            get
            {
                if (s_cTraitPhases == null)
                {
                    s_cTraitPhases = new Dictionary<Trait, TraitPhase[]>();
                    s_cTraitPhases[Trait.Agression] = new TraitPhase[] 
                        {   new TraitPhase(Trait.Agression, 0.0f, 0.33f, "(+3) completely pacifistic"),
                            new TraitPhase(Trait.Agression, 0.33f, 0.66f, "(+2) quite amiable"),
                            new TraitPhase(Trait.Agression, 0.66f, 1.0f, "(+1) peaceful"),
                            new TraitPhase(Trait.Agression, 1.0f, 1.33f, "(-1) hot-tempered"),
                            new TraitPhase(Trait.Agression, 1.33f, 1.66f, "(-2) agressive"),
                            new TraitPhase(Trait.Agression, 1.66f, 2.0f, "(-3) extremely agressive"),

                        };
                    s_cTraitPhases[Trait.Selfishness] = new TraitPhase[] 
                        {   new TraitPhase(Trait.Selfishness, 0.0f, 0.33f, "(+3) completely selfless"),
                            new TraitPhase(Trait.Selfishness, 0.33f, 0.66f, "(+2) quite selfless"),
                            new TraitPhase(Trait.Selfishness, 0.66f, 1.0f, "(+1) not so egoistic"),
                            new TraitPhase(Trait.Selfishness, 1.0f, 1.33f, "(-1) quite egoistic"),
                            new TraitPhase(Trait.Selfishness, 1.33f, 1.66f, "(-2) very selfish"),
                            new TraitPhase(Trait.Selfishness, 1.66f, 2.0f, "(-3) completely selfish"),
                        };
                    s_cTraitPhases[Trait.Simplicity] = new TraitPhase[] 
                        {   new TraitPhase(Trait.Simplicity, 0.0f, 0.33f, "(+3) creativity as a supreme value"),
                            new TraitPhase(Trait.Simplicity, 0.33f, 0.66f, "(+2) true art"),
                            new TraitPhase(Trait.Simplicity, 0.66f, 1.0f, "(+1) pop art"),
                            new TraitPhase(Trait.Simplicity, 1.0f, 1.33f, "(-1) applied art"),
                            new TraitPhase(Trait.Simplicity, 1.33f, 1.66f, "(-2) primitive pleasures"),
                            new TraitPhase(Trait.Simplicity, 1.66f, 2.0f, "(-3) only natural needs satisfaction"),
                        };
                    s_cTraitPhases[Trait.Fanaticism] = new TraitPhase[] 
                        {   new TraitPhase(Trait.Fanaticism, 0.0f, 0.33f, "(+3) fully tolerant"),
                            new TraitPhase(Trait.Fanaticism, 0.33f, 0.66f, "(+2) liberal"),
                            new TraitPhase(Trait.Fanaticism, 0.66f, 1.0f, "(+1) open-minded"),
                            new TraitPhase(Trait.Fanaticism, 1.0f, 1.33f, "(-1) narrow-minded"),
                            new TraitPhase(Trait.Fanaticism, 1.33f, 1.66f, "(-2) intolerant"),
                            new TraitPhase(Trait.Fanaticism, 1.66f, 2.0f, "(-3) fanatical"),
                        };
                    s_cTraitPhases[Trait.Piety] = new TraitPhase[] 
                        {   new TraitPhase(Trait.Piety, 0.0f, 0.33f, "(+3) atheism"),
                            new TraitPhase(Trait.Piety, 0.33f, 0.66f, "(+2) syntheism"),
                            new TraitPhase(Trait.Piety, 0.66f, 1.0f, "(+1) pantheism"),
                            new TraitPhase(Trait.Piety, 1.0f, 1.33f, "(-1) monotheism"),
                            new TraitPhase(Trait.Piety, 1.33f, 1.66f, "(-2) polytheism"),
                            new TraitPhase(Trait.Piety, 1.66f, 2.0f, "(-3) animism"),
                        };
                    s_cTraitPhases[Trait.Treachery] = new TraitPhase[] 
                        {   new TraitPhase(Trait.Treachery, 0.0f, 0.33f, "(+3) righteous"),
                            new TraitPhase(Trait.Treachery, 0.33f, 0.66f, "(+2) honest"),
                            new TraitPhase(Trait.Treachery, 0.66f, 1.0f, "(+1) lawful"),
                            new TraitPhase(Trait.Treachery, 1.0f, 1.33f, "(-1) dishonest"),
                            new TraitPhase(Trait.Treachery, 1.33f, 1.66f, "(-2) criminal"),
                            new TraitPhase(Trait.Treachery, 1.66f, 2.0f, "(-3) anarchist"),
                        };
                }
                return s_cTraitPhases;
            }
        }

        public string GetTraitString(Trait eTrait, int iLevel)
        {
            foreach (TraitPhase pPhase in TraitPhases[eTrait])
            {
                if (pPhase.Check(this, iLevel))
                    return pPhase.m_sName;
            }
            return "error (" + m_cTraits[eTrait][iLevel].ToString() + ")";
        }

        public string GetDiffString(int iLevel, Mentality pOther, int iOtherlevel)
        {
            string sResult = "";
            bool bFirst = true;
            foreach (Trait eTrait in AllTraits)
            {
                string sTrait1 = GetTraitString(eTrait, iLevel);
                string sTrait2 = pOther.GetTraitString(eTrait, iOtherlevel);

                if (sTrait1 != sTrait2)
                {
                    if (!bFirst)
                        sResult += ", ";

                    if (m_cTraits[eTrait][iLevel] > pOther.m_cTraits[eTrait][iOtherlevel] + 0.33)
                        sResult += "much more ";
                    else if (m_cTraits[eTrait][iLevel] > pOther.m_cTraits[eTrait][iOtherlevel])
                        sResult += "more ";
                    else if (m_cTraits[eTrait][iLevel] < pOther.m_cTraits[eTrait][iOtherlevel] - 0.33)
                        sResult += "much less ";
                    else
                        sResult += "less ";

                    switch (eTrait)
                    {
                        case Trait.Agression:
                            sResult += "agressive";
                            break;
                        case Trait.Fanaticism:
                            sResult += "fanatical";
                            break;
                        case Trait.Piety:
                            sResult += "religious";
                            break;
                        case Trait.Simplicity:
                            sResult += "rude";
                            break;
                        case Trait.Selfishness:
                            sResult += "selfish";
                            break;
                        case Trait.Treachery:
                            sResult += "treacherous";
                            break;
                    }

                    bFirst = false;
                }
            }

            return sResult;
        }

        public override bool Equals(object obj)
        {
            var pOther = obj as Mentality;
            if (pOther == null)
                return false;

            foreach (Trait eTrait in AllTraits)
            {
                if (m_cTraits[eTrait] != pOther.m_cTraits[eTrait])
                    return false;
            }

            return true;
        }
    }
}
