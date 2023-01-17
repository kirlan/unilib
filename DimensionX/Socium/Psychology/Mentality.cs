using BenTools.Mathematics;
using Random;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

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
        public static ImmutableArray<Trait> AllTraits { get; } = Enum.GetValues(typeof(Trait)).Cast<Trait>().ToImmutableArray();

        public static Mentality IdealSociety { get; } = new Mentality(0);

        /// <summary>
        /// Моральные качества - в диапазоне 0..2
        /// </summary>
        public Dictionary<Trait, float[]> Traits { get; } = new Dictionary<Trait, float[]>();

        public Mentality(MentalityTemplate cTemplate)
        {
            foreach (Trait prop in AllTraits)
            {
                Traits[prop] = new float[9];
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
                    }
                    fSum += aDerivative[i];
                }

                float fCurrent = 2;
                for (int i = 0; i < 9; i++)
                {
                    fCurrent -= aDerivative[i]*2 / fSum;
                    Traits[prop][i] = Math.Max(0, fCurrent);
                }
            }
        }

        public Mentality(float fValue)
        {
            foreach (Trait prop in AllTraits)
            {
                Traits[prop] = new float[9];
                for (int i = 0; i < 9; i++)
                    Traits[prop][i] = Math.Min(2.0f, Math.Max(0, fValue));
            }
        }

        public Mentality(Mentality pAncestorMentality, bool bMutate)
        {
            foreach (Trait eTrait in AllTraits)
            {
                if (bMutate)
                {
                    Traits[eTrait] = new float[9];
                    float[] aDerivative = new float[9];//произвдных на 1 больше, чем уровней, чтобы последний уровень был меньше 2.0
                    float fSum = 2;
                    for (int i = 0; i < 9; i++)
                    {
                        aDerivative[i] = fSum - pAncestorMentality.Traits[eTrait][i];
                        fSum -= aDerivative[i];
                    }

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
                        Traits[eTrait][i] = Math.Max(0, fCurrent);
                    }
                }
                else
                {
                    Traits[eTrait] = new float[9];
                    for (int i = 0; i < 9; i++)
                        Traits[eTrait][i] = pAncestorMentality.Traits[eTrait][i];
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
                culture1.Set((int)prop, Traits[prop][iOwnProgress] - 1.0f);
                culture2.Set((int)prop, different.Traits[prop][iOpponentProgress] - 1.0f);
            }
            return (float)Math.Sqrt(BTVector.Dist(culture1, culture2)) - 1f;
        }

        /// <summary>
        /// Именованная фаза развития определённой черты характера
        /// </summary>
        private sealed class TraitPhase
        {
            public float m_fMinValue;
            public float m_fMaxValue;
            public string m_sName;
            private readonly Trait m_eTrait;

            public TraitPhase(Trait eTrait, float fMin, float fMax, string sName)
            {
                m_eTrait = eTrait;
                m_fMinValue = fMin;
                m_fMaxValue = fMax;
                m_sName = sName;
            }

            public bool Check(Mentality pMentality, int iLevel)
            {
                return pMentality.Traits[m_eTrait][iLevel] >= m_fMinValue &&
                    pMentality.Traits[m_eTrait][iLevel] <= m_fMaxValue;
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
                return s_cTraitPhases ?? (s_cTraitPhases = new Dictionary<Trait, TraitPhase[]>
                    {
                        [Trait.Agression] = new[]
                        {   new TraitPhase(Trait.Agression, 0.0f, 0.33f, "(+3) completely pacifistic"),
                            new TraitPhase(Trait.Agression, 0.33f, 0.66f, "(+2) quite amiable"),
                            new TraitPhase(Trait.Agression, 0.66f, 1.0f, "(+1) peaceful"),
                            new TraitPhase(Trait.Agression, 1.0f, 1.33f, "(-1) hot-tempered"),
                            new TraitPhase(Trait.Agression, 1.33f, 1.66f, "(-2) agressive"),
                            new TraitPhase(Trait.Agression, 1.66f, 2.0f, "(-3) extremely agressive"),
                        },
                        [Trait.Selfishness] = new[]
                        {   new TraitPhase(Trait.Selfishness, 0.0f, 0.33f, "(+3) completely selfless"),
                            new TraitPhase(Trait.Selfishness, 0.33f, 0.66f, "(+2) quite selfless"),
                            new TraitPhase(Trait.Selfishness, 0.66f, 1.0f, "(+1) not so egoistic"),
                            new TraitPhase(Trait.Selfishness, 1.0f, 1.33f, "(-1) quite egoistic"),
                            new TraitPhase(Trait.Selfishness, 1.33f, 1.66f, "(-2) very selfish"),
                            new TraitPhase(Trait.Selfishness, 1.66f, 2.0f, "(-3) completely selfish"),
                        },
                        [Trait.Simplicity] = new[]
                        {   new TraitPhase(Trait.Simplicity, 0.0f, 0.33f, "(+3) creativity as a supreme value"),
                            new TraitPhase(Trait.Simplicity, 0.33f, 0.66f, "(+2) true art"),
                            new TraitPhase(Trait.Simplicity, 0.66f, 1.0f, "(+1) pop art"),
                            new TraitPhase(Trait.Simplicity, 1.0f, 1.33f, "(-1) applied art"),
                            new TraitPhase(Trait.Simplicity, 1.33f, 1.66f, "(-2) primitive pleasures"),
                            new TraitPhase(Trait.Simplicity, 1.66f, 2.0f, "(-3) only natural needs satisfaction"),
                        },
                        [Trait.Fanaticism] = new[]
                        {   new TraitPhase(Trait.Fanaticism, 0.0f, 0.33f, "(+3) fully tolerant"),
                            new TraitPhase(Trait.Fanaticism, 0.33f, 0.66f, "(+2) liberal"),
                            new TraitPhase(Trait.Fanaticism, 0.66f, 1.0f, "(+1) open-minded"),
                            new TraitPhase(Trait.Fanaticism, 1.0f, 1.33f, "(-1) narrow-minded"),
                            new TraitPhase(Trait.Fanaticism, 1.33f, 1.66f, "(-2) intolerant"),
                            new TraitPhase(Trait.Fanaticism, 1.66f, 2.0f, "(-3) fanatical"),
                        },
                        [Trait.Piety] = new[]
                        {   new TraitPhase(Trait.Piety, 0.0f, 0.33f, "(+3) atheism"),
                            new TraitPhase(Trait.Piety, 0.33f, 0.66f, "(+2) syntheism"),
                            new TraitPhase(Trait.Piety, 0.66f, 1.0f, "(+1) pantheism"),
                            new TraitPhase(Trait.Piety, 1.0f, 1.33f, "(-1) monotheism"),
                            new TraitPhase(Trait.Piety, 1.33f, 1.66f, "(-2) polytheism"),
                            new TraitPhase(Trait.Piety, 1.66f, 2.0f, "(-3) animism"),
                        },
                        [Trait.Treachery] = new[]
                        {   new TraitPhase(Trait.Treachery, 0.0f, 0.33f, "(+3) righteous"),
                            new TraitPhase(Trait.Treachery, 0.33f, 0.66f, "(+2) honest"),
                            new TraitPhase(Trait.Treachery, 0.66f, 1.0f, "(+1) lawful"),
                            new TraitPhase(Trait.Treachery, 1.0f, 1.33f, "(-1) dishonest"),
                            new TraitPhase(Trait.Treachery, 1.33f, 1.66f, "(-2) criminal"),
                            new TraitPhase(Trait.Treachery, 1.66f, 2.0f, "(-3) anarchist"),
                        }
                    });
            }
        }

        public string GetTraitString(Trait eTrait, int iLevel)
        {
            foreach (TraitPhase pPhase in TraitPhases[eTrait])
            {
                if (pPhase.Check(this, iLevel))
                    return pPhase.m_sName;
            }
            return "error (" + Traits[eTrait][iLevel].ToString() + ")";
        }

        public string GetDiffString(int iLevel, Mentality pOther, int iOtherlevel)
        {
            StringBuilder sResult = new StringBuilder();
            bool bFirst = true;
            Trait eLastDefferentTrait = Trait.Agression;
            foreach (Trait eTrait in AllTraits)
            {
                string sTrait1 = GetTraitString(eTrait, iLevel);
                string sTrait2 = pOther.GetTraitString(eTrait, iOtherlevel);

                if (sTrait1 != sTrait2)
                {
                    eLastDefferentTrait = eTrait;
                }
            }
            foreach (Trait eTrait in AllTraits)
            {
                string sTrait1 = GetTraitString(eTrait, iLevel);
                string sTrait2 = pOther.GetTraitString(eTrait, iOtherlevel);

                if (sTrait1 != sTrait2)
                {
                    if (!bFirst)
                        sResult.Append(eTrait == eLastDefferentTrait ? " and " : ", ");

                    bool bReverse = false;
                    if (Traits[eTrait][iLevel] > pOther.Traits[eTrait][iOtherlevel] + 0.33)
                    {
                        sResult.Append("much more ");
                    }
                    else if (Traits[eTrait][iLevel] > pOther.Traits[eTrait][iOtherlevel])
                    {
                        sResult.Append("more ");
                    }
                    else if (Traits[eTrait][iLevel] < pOther.Traits[eTrait][iOtherlevel] - 0.33)
                    {
                        sResult.Append("much more ");
                        bReverse = true;
                    }
                    else
                    {
                        sResult.Append("more ");
                        bReverse = true;
                    }

                    switch (eTrait)
                    {
                        case Trait.Agression:
                            sResult.Append(bReverse ? "peaceful" : "agressive");
                            break;
                        case Trait.Fanaticism:
                            sResult.Append(bReverse ? "tolerant" : "fanatical");
                            break;
                        case Trait.Piety:
                            sResult.Append(bReverse ? "rational" : "religious");
                            break;
                        case Trait.Simplicity:
                            sResult.Append(bReverse ? "refined" : "simple");
                            break;
                        case Trait.Selfishness:
                            sResult.Append(bReverse ? "generous" : "selfish");
                            break;
                        case Trait.Treachery:
                            sResult.Append(bReverse ? "honorable" : "treacherous");
                            break;
                    }

                    bFirst = false;
                }
            }

            return sResult.ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Mentality pOther))
                return false;

            foreach (Trait eTrait in AllTraits)
            {
                if (Traits[eTrait] != pOther.Traits[eTrait])
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();

            foreach (Trait eTrait in AllTraits)
                hash.Add(Traits[eTrait]);

            return hash.ToHashCode();
        }
    }
}
