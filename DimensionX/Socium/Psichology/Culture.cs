using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using BenTools.Mathematics;

namespace Socium.Psichology
{
    public enum Mentality
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
    
    public class Culture
    {
        public static Array Mentalities = Enum.GetValues(typeof(Mentality));

        private Dictionary<Mentality, float[]> m_cMentalityValues = new Dictionary<Mentality, float[]>();
        /// <summary>
        /// Моральные качества - в диапазоне 0..2
        /// </summary>
        public Dictionary<Mentality, float[]> MentalityValues
        {
            get { return m_cMentalityValues; }
        }

        public Culture(CultureTemplate cTemplate)
        {
            foreach (Mentality prop in Mentalities)
            {
                m_cMentalityValues[prop] = new float[9];
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
                    m_cMentalityValues[prop][i] = Math.Max(0, fCurrent);
                }
            }
        }

        public Culture(float fValue)
        {
            foreach (Mentality prop in Mentalities)
            {
                m_cMentalityValues[prop] = new float[9];
                for (int i = 0; i < 9; i++) 
                    m_cMentalityValues[prop][i] = Math.Min(2.0f, Math.Max(0, fValue));
            }
        }

        private static Culture s_pIdeal = new Culture(0);

        public static Culture IdealSociety
        {
            get { return s_pIdeal; }
        }

        public Culture(Culture pAncestorCulture)
        {
            foreach (Mentality eMentality in Mentalities)
            {
                m_cMentalityValues[eMentality] = new float[9];
                float[] aDerivative = new float[9];//произвдных на 1 больше, чем уровней, чтобы последний уровень был меньше 2.0
                float fSum = 2;
                for (int i = 0; i < 9; i++)
                {
                    aDerivative[i] = fSum - pAncestorCulture.m_cMentalityValues[eMentality][i];
                    fSum -= aDerivative[i];
                }
                //aDerivative[9] = fSum;
                fSum = 0;
                for (int i = 0; i < 9; i++)
                {
                    aDerivative[i] = (9*aDerivative[i] + 0.05f + Rnd.Get(0.4f))/10;
                    fSum += aDerivative[i];
                }

                float fCurrent = 2;
                for (int i = 0; i < 9; i++)
                {
                    fCurrent -= aDerivative[i]*2 / fSum;
                    m_cMentalityValues[eMentality][i] = Math.Max(0, fCurrent);
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
        public float GetDifference(Culture different, int iOwnLevel, int iOpponentLevel)
        {
            BTVector culture1 = new BTVector(Mentalities.Length);
            BTVector culture2 = new BTVector(Mentalities.Length);
            foreach (Mentality prop in Mentalities)
            {
                culture1.Set((int)prop, m_cMentalityValues[prop][iOwnLevel] - 1.0f);
                culture2.Set((int)prop, different.m_cMentalityValues[prop][iOpponentLevel] - 1.0f);
            }
            return (float)Math.Sqrt(BTVector.Dist(culture1, culture2)) - 1f;
            //return -(float)(culture1 * culture2) / Mentalities.Length;
        }

        private class MentalityCluster
        {
            public float m_fMinValue;
            public float m_fMaxValue;
            public string m_sString;
            private Mentality m_eMentality;

            public MentalityCluster(Mentality eMentality, float fMin, float fMax, string sName)
            {
                m_eMentality = eMentality;
                m_fMinValue = fMin;
                m_fMaxValue = fMax;
                m_sString = sName;
            }

            public bool Check(Culture pCulture, int iLevel)
            {
                return pCulture.m_cMentalityValues[m_eMentality][iLevel] >= m_fMinValue &&
                    pCulture.m_cMentalityValues[m_eMentality][iLevel] <= m_fMaxValue;
            }
        }

        /// <summary>
        /// НЕ ИСПОЛЬЗОВАТЬ!
        /// Обращаться к свойству MentalityClusters.
        /// </summary>
        private static Dictionary<Mentality, MentalityCluster[]> s_cMentalityClusters = null;

        private static Dictionary<Mentality, MentalityCluster[]> MentalityClusters
        {
            get
            {
                if (s_cMentalityClusters == null)
                {
                    s_cMentalityClusters = new Dictionary<Mentality, MentalityCluster[]>();
                    s_cMentalityClusters[Mentality.Agression] = new MentalityCluster[] 
                        {   new MentalityCluster(Mentality.Agression, 0.0f, 0.33f, "(+3) completely pacifistic"),
                            new MentalityCluster(Mentality.Agression, 0.33f, 0.66f, "(+2) quite amiable"),
                            new MentalityCluster(Mentality.Agression, 0.66f, 1.0f, "(+1) peaceful"),
                            new MentalityCluster(Mentality.Agression, 1.0f, 1.33f, "(-1) hot-tempered"),
                            new MentalityCluster(Mentality.Agression, 1.33f, 1.66f, "(-2) agressive"),
                            new MentalityCluster(Mentality.Agression, 1.66f, 2.0f, "(-3) extremely agressive"),

                        };
                    s_cMentalityClusters[Mentality.Selfishness] = new MentalityCluster[] 
                        {   new MentalityCluster(Mentality.Selfishness, 0.0f, 0.33f, "(+3) completely selfless"),
                            new MentalityCluster(Mentality.Selfishness, 0.33f, 0.66f, "(+2) quite selfless"),
                            new MentalityCluster(Mentality.Selfishness, 0.66f, 1.0f, "(+1) not so egoistic"),
                            new MentalityCluster(Mentality.Selfishness, 1.0f, 1.33f, "(-1) quite egoistic"),
                            new MentalityCluster(Mentality.Selfishness, 1.33f, 1.66f, "(-2) very selfish"),
                            new MentalityCluster(Mentality.Selfishness, 1.66f, 2.0f, "(-3) completely selfish"),
                        };
                    s_cMentalityClusters[Mentality.Simplicity] = new MentalityCluster[] 
                        {   new MentalityCluster(Mentality.Simplicity, 0.0f, 0.33f, "(+3) creativity as a supreme value"),
                            new MentalityCluster(Mentality.Simplicity, 0.33f, 0.66f, "(+2) true art"),
                            new MentalityCluster(Mentality.Simplicity, 0.66f, 1.0f, "(+1) pop art"),
                            new MentalityCluster(Mentality.Simplicity, 1.0f, 1.33f, "(-1) applied art"),
                            new MentalityCluster(Mentality.Simplicity, 1.33f, 1.66f, "(-2) primitive pleasures"),
                            new MentalityCluster(Mentality.Simplicity, 1.66f, 2.0f, "(-3) only natural needs satisfaction"),
                        };
                    s_cMentalityClusters[Mentality.Fanaticism] = new MentalityCluster[] 
                        {   new MentalityCluster(Mentality.Fanaticism, 0.0f, 0.33f, "(+3) fully tolerant"),
                            new MentalityCluster(Mentality.Fanaticism, 0.33f, 0.66f, "(+2) liberal"),
                            new MentalityCluster(Mentality.Fanaticism, 0.66f, 1.0f, "(+1) open-minded"),
                            new MentalityCluster(Mentality.Fanaticism, 1.0f, 1.33f, "(-1) narrow-minded"),
                            new MentalityCluster(Mentality.Fanaticism, 1.33f, 1.66f, "(-2) intolerant"),
                            new MentalityCluster(Mentality.Fanaticism, 1.66f, 2.0f, "(-3) fanatical"),
                        };
                    s_cMentalityClusters[Mentality.Piety] = new MentalityCluster[] 
                        {   new MentalityCluster(Mentality.Piety, 0.0f, 0.33f, "(+3) atheism"),
                            new MentalityCluster(Mentality.Piety, 0.33f, 0.66f, "(+2) syntheism"),
                            new MentalityCluster(Mentality.Piety, 0.66f, 1.0f, "(+1) pantheism"),
                            new MentalityCluster(Mentality.Piety, 1.0f, 1.33f, "(-1) monotheism"),
                            new MentalityCluster(Mentality.Piety, 1.33f, 1.66f, "(-2) polytheism"),
                            new MentalityCluster(Mentality.Piety, 1.66f, 2.0f, "(-3) animism"),
                        };
                    s_cMentalityClusters[Mentality.Treachery] = new MentalityCluster[] 
                        {   new MentalityCluster(Mentality.Treachery, 0.0f, 0.33f, "(+3) righteous"),
                            new MentalityCluster(Mentality.Treachery, 0.33f, 0.66f, "(+2) honest"),
                            new MentalityCluster(Mentality.Treachery, 0.66f, 1.0f, "(+1) lawful"),
                            new MentalityCluster(Mentality.Treachery, 1.0f, 1.33f, "(-1) dishonest"),
                            new MentalityCluster(Mentality.Treachery, 1.33f, 1.66f, "(-2) criminal"),
                            new MentalityCluster(Mentality.Treachery, 1.66f, 2.0f, "(-3) anarchist"),
                        };
                }
                return s_cMentalityClusters;
            }
        }

        public string GetMentalityString(Mentality eMentality, int iLevel)
        {
            foreach (MentalityCluster pString in MentalityClusters[eMentality])
            {
                if (pString.Check(this, iLevel))
                    return pString.m_sString;
            }
            return "error (" + m_cMentalityValues[eMentality][iLevel].ToString() + ")";
        }

        public string GetMentalityDiffString(int iLevel, Culture pOther, int iOtherlevel)
        {
            string sResult = "";
            bool bFirst = true;
            foreach (Mentality eMentality in Mentalities)
            {
                string sMentality1 = GetMentalityString(eMentality, iLevel);
                string sMentality2 = pOther.GetMentalityString(eMentality, iOtherlevel);

                if (sMentality1 != sMentality2)
                {
                    if (!bFirst)
                        sResult += ", ";

                    if (m_cMentalityValues[eMentality][iLevel] > pOther.m_cMentalityValues[eMentality][iOtherlevel] + 0.33)
                        sResult += "much more ";
                    else if (m_cMentalityValues[eMentality][iLevel] > pOther.m_cMentalityValues[eMentality][iOtherlevel])
                        sResult += "more ";
                    else if (m_cMentalityValues[eMentality][iLevel] < pOther.m_cMentalityValues[eMentality][iOtherlevel] - 0.33)
                        sResult += "much less ";
                    else
                        sResult += "less ";

                    switch (eMentality)
                    {
                        case Mentality.Agression:
                            sResult += "agressive";
                            break;
                        case Mentality.Fanaticism:
                            sResult += "fanatical";
                            break;
                        case Mentality.Piety:
                            sResult += "religious";
                            break;
                        case Mentality.Simplicity:
                            sResult += "rude";
                            break;
                        case Mentality.Selfishness:
                            sResult += "selfish";
                            break;
                        case Mentality.Treachery:
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
            var pOther = obj as Culture;
            if (pOther == null)
                return false;

            foreach (Mentality eMentality in Mentalities)
            {
                if (m_cMentalityValues[eMentality] != pOther.m_cMentalityValues[eMentality])
                    return false;
            }

            return true;
        }
    }
}
