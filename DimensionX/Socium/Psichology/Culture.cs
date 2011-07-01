﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using BenTools.Mathematics;

namespace Socium.Psichology
{
    public class Culture
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
            /// Эксплуатация - рапределение результатов труда между социальными слоями, крайняя форма - рабство, когда низший слой (рабы) - не получают почти ничего.
            /// </summary>
            Exploitation//Exploitation
        };

        public static Array Mentalities = Enum.GetValues(typeof(Mentality));

        private Dictionary<Mentality, float[]> m_cMentalityValues = new Dictionary<Mentality, float[]>();
        /// <summary>
        /// Моральные качества - в диапазоне 0..2
        /// </summary>
        public Dictionary<Mentality, float[]> MentalityValues
        {
            get { return m_cMentalityValues; }
        }

        public Culture()
        {
            foreach (Mentality prop in Mentalities)
            {
                m_cMentalityValues[prop] = new float[9];
                float[] aDerivative = new float[9];//произвдных на 1 больше, чем уровней, чтобы последний уровень был меньше 2.0
                float fSum = 0;
                for (int i = 0; i < 9; i++)
                {
                    aDerivative[i] = 0.1f + Rnd.Get(0.88f);
                    fSum += aDerivative[i];
                }

                float fCurrent = 2;
                for (int i = 0; i < 9; i++)
                {
                    fCurrent -= aDerivative[i] * 2 / fSum; 
                    m_cMentalityValues[prop][i] = fCurrent;
                }
                    //2.0f - (float)Math.Pow(Rnd.Get(0.5f), 4);
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
                    aDerivative[i] = (4*aDerivative[i] + 0.22f + Rnd.Get(0.44f))/5;
                    fSum += aDerivative[i];
                }

                float fCurrent = 2;
                for (int i = 0; i < 9; i++)
                {
                    fCurrent -= aDerivative[i] * 2 / fSum; 
                    m_cMentalityValues[eMentality][i] = fCurrent;
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
            return -(float)(culture1 * culture2) / Mentalities.Length;
        }

        private class MentalityCluster
        {
            public float m_fMinValue;
            public float m_fMaxValue;
            public string m_sString;

            public MentalityCluster(float fMin, float fMax, string sName)
            {
                m_fMinValue = fMin;
                m_fMaxValue = fMax;
                m_sString = sName;
            }
        }

        /// <summary>
        /// НЕ ИСПОЛЬЗОВАТЬ!
        /// Обращаться к свойству MoraleStrings.
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
                        {   new MentalityCluster(0.0f, 0.33f, "(+3) completely pacifistic"),
                            new MentalityCluster(0.33f, 0.66f, "(+2) quite amiable"),
                            new MentalityCluster(0.66f, 1.0f, "(+1) peaceful"),
                            new MentalityCluster(1.0f, 1.33f, "(-1) unfriendly"),
                            new MentalityCluster(1.33f, 1.66f, "(-2) agressive"),
                            new MentalityCluster(1.66f, 2.0f, "(-3) extremely agressive"),

                        };
                    s_cMentalityClusters[Mentality.Selfishness] = new MentalityCluster[] 
                        {   new MentalityCluster(0.0f, 0.33f, "(+3) completely selfless"),
                            new MentalityCluster(0.33f, 0.66f, "(+2) quite selfless"),
                            new MentalityCluster(0.66f, 1.0f, "(+1) not so egoistic"),
                            new MentalityCluster(1.0f, 1.33f, "(-1) egoistic"),
                            new MentalityCluster(1.33f, 1.66f, "(-2) very selfish"),
                            new MentalityCluster(1.66f, 2.0f, "(-3) completely selfish"),
                        };
                    s_cMentalityClusters[Mentality.Exploitation] = new MentalityCluster[] 
                        {   new MentalityCluster(0.0f, 0.33f, "(+3) communism"),
                            new MentalityCluster(0.33f, 0.66f, "(+2) corporations"),
                            new MentalityCluster(0.66f, 1.0f, "(+1) paid workers"),
                            new MentalityCluster(1.0f, 1.33f, "(-1) serfdom"),
                            new MentalityCluster(1.33f, 1.66f, "(-2) slavery"),
                            new MentalityCluster(1.66f, 2.0f, "(-3) jungle law"),
                        };
                    s_cMentalityClusters[Mentality.Fanaticism] = new MentalityCluster[] 
                        {   new MentalityCluster(0.0f, 0.33f, "(+3) fully tolerant"),
                            new MentalityCluster(0.33f, 0.66f, "(+2) liberal"),
                            new MentalityCluster(0.66f, 1.0f, "(+1) open-minded"),
                            new MentalityCluster(1.0f, 1.33f, "(-1) narrow-minded"),
                            new MentalityCluster(1.33f, 1.66f, "(-2) fanatical"),
                            new MentalityCluster(1.66f, 2.0f, "(-3) closed community"),
                        };
                    s_cMentalityClusters[Mentality.Piety] = new MentalityCluster[] 
                        {   new MentalityCluster(0.0f, 0.33f, "(+3) ateists"),
                            new MentalityCluster(0.33f, 0.66f, "(+2) agnostics"),
                            new MentalityCluster(0.66f, 1.0f, "(+1) formal believers"),
                            new MentalityCluster(1.0f, 1.33f, "(-1) pragmatic believers"),
                            new MentalityCluster(1.33f, 1.66f, "(-2) divine servants"),
                            new MentalityCluster(1.66f, 2.0f, "(-3) divine toys"),
                        };
                    s_cMentalityClusters[Mentality.Treachery] = new MentalityCluster[] 
                        {   new MentalityCluster(0.0f, 0.33f, "(+3) absolutely honest"),
                            new MentalityCluster(0.33f, 0.66f, "(+2) honest"),
                            new MentalityCluster(0.66f, 1.0f, "(+1) lawful"),
                            new MentalityCluster(1.0f, 1.33f, "(-1) sly"),
                            new MentalityCluster(1.33f, 1.66f, "(-2) treacherous"),
                            new MentalityCluster(1.66f, 2.0f, "(-3) absolute scoundrels"),
                        };
                }
                return s_cMentalityClusters;
            }
        }

        public string GetMentalityString(Mentality eMentality, int iLevel)
        {
            foreach (MentalityCluster pString in MentalityClusters[eMentality])
            {
                if (m_cMentalityValues[eMentality][iLevel] >= pString.m_fMinValue && 
                    m_cMentalityValues[eMentality][iLevel] <= pString.m_fMaxValue)
                    return pString.m_sString;
            }
            return "error";
        }
    }
}
