using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using BenTools.Mathematics;

namespace Socium
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
            /// Сладострастие - жажда плотских удовольсвий, интерес к сексу, превалирующий над другими факторами при формировании отношения к другому индивиду
            /// </summary>
            Voluptuousness, //Voluptuousness,
            /// <summary>
            /// Вероломство - склонность к обману и мошенничеству
            /// </summary>
            Guile, //Guile,
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

        private Dictionary<Mentality, float> m_cMentalityValues = new Dictionary<Mentality, float>();
        /// <summary>
        /// Моральные качества - в диапазоне 0..2
        /// </summary>
        public Dictionary<Mentality, float> MentalityValues
        {
            get { return m_cMentalityValues; }
        }

        public Culture()
        {
            foreach (Mentality prop in Mentalities)
            {
                m_cMentalityValues[prop] = 2.0f - (float)Math.Pow(Rnd.Get(1.0f), 2);
            }
        }

        public Culture(float fValue)
        {
            foreach (Mentality prop in Mentalities)
            {
                m_cMentalityValues[prop] = Math.Min(2.0f, Math.Max(0, fValue));
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
                int iDistance = 1 + Rnd.Get(20);
                m_cMentalityValues[eMentality] = (Rnd.Get(2.0f) + iDistance * pAncestorCulture.m_cMentalityValues[eMentality]) / (iDistance + 1);
            }
        }

        public void Evolve()
        {
            foreach (Mentality eMentality in Mentalities)
            {
                //ментальные качества у нас отрицательные, поэтому для того, чтобы эволюционировать, их значения нужно снизить.
                float fFutureValue = (float)Math.Pow(Rnd.Get(1.0f), 2);// (float)Math.Pow(Rnd.Get(2.0f), 2) / 2.0f;
                if (fFutureValue < m_cMentalityValues[eMentality])
                    m_cMentalityValues[eMentality] = (fFutureValue + 9 * m_cMentalityValues[eMentality]) / 10;
            }
        }

        public void Degrade()
        {
            foreach (Mentality eMentality in Mentalities)
            {
                //ментальные качества у нас отрицательные, поэтому для того, чтобы деградировать, их значения нужно поднять.
                float fFutureValue = 2.0f - (float)Math.Pow(Rnd.Get(1.0f), 2);
                if (fFutureValue > m_cMentalityValues[eMentality])
                    m_cMentalityValues[eMentality] = (fFutureValue + 9 * m_cMentalityValues[eMentality]) / 10;
            }
        }

        /// <summary>
        /// Различие между культурами от +1 (полная противоположность) до -1 (полное совпадение)
        /// </summary>
        /// <param name="different">другая культура</param>
        /// <returns>скалярное произведение нормализованных векторов культуры / (количество моральных качеств)</returns>
        public float GetDifference(Culture different)
        {
            BTVector culture1 = new BTVector(Mentalities.Length);
            BTVector culture2 = new BTVector(Mentalities.Length);
            foreach (Mentality prop in Mentalities)
            {
                culture1.Set((int)prop, m_cMentalityValues[prop] - 1.0f);
                culture2.Set((int)prop, different.m_cMentalityValues[prop] - 1.0f);
            }
            return -(float)(culture1 * culture2) / Mentalities.Length;
        }

        private class MentalityString
        {
            public float m_fMinValue;
            public float m_fMaxValue;
            public string m_sString;

            public MentalityString(float fMin, float fMax, string sName)
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
        private static Dictionary<Mentality, MentalityString[]> s_cMentalityStrings = null;

        private static Dictionary<Mentality, MentalityString[]> MentalityStrings
        {
            get
            {
                if (s_cMentalityStrings == null)
                {
                    s_cMentalityStrings = new Dictionary<Mentality, MentalityString[]>();
                    s_cMentalityStrings[Mentality.Agression] = new MentalityString[] 
                        {   new MentalityString(0.0f, 0.2f, "absolutely pacifistic"),
                            new MentalityString(0.2f, 0.4f, "pacifistic"),
                            new MentalityString(0.4f, 0.6f, "very peaceful"),
                            new MentalityString(0.6f, 0.8f, "quite peaceful"),
                            new MentalityString(0.8f, 1.0f, "peaceful"),
                            new MentalityString(1.0f, 1.2f, "not so peaceful"),
                            new MentalityString(1.2f, 1.4f, "agressive"),
                            new MentalityString(1.4f, 1.6f, "quite agressive"),
                            new MentalityString(1.6f, 1.8f, "very agressive"),
                            new MentalityString(1.8f, 2.0f, "extremely agressive"),
                        };
                    s_cMentalityStrings[Mentality.Selfishness] = new MentalityString[] 
                        {   new MentalityString(0.0f, 0.2f, "absolutely altruistic"),
                            new MentalityString(0.2f, 0.4f, "very altruistic"),
                            new MentalityString(0.4f, 0.6f, "quite altruistic"),
                            new MentalityString(0.6f, 0.8f, "altruistic"),
                            new MentalityString(0.8f, 1.0f, "not so selfish"),
                            new MentalityString(1.0f, 1.2f, "selfish"),
                            new MentalityString(1.2f, 1.4f, "quite selfish"),
                            new MentalityString(1.4f, 1.6f, "very selfish"),
                            new MentalityString(1.6f, 1.8f, "extremely selfish"),
                            new MentalityString(1.8f, 2.0f, "absolutely egomaniacal"),
                        };
                    s_cMentalityStrings[Mentality.Exploitation] = new MentalityString[] 
                        {   new MentalityString(0.0f, 0.2f, "full communism"),
                            new MentalityString(0.2f, 0.4f, "socialism"),
                            new MentalityString(0.4f, 0.6f, "all workers are shareholders"),
                            new MentalityString(0.6f, 0.8f, "uses freelancers"),
                            new MentalityString(0.8f, 1.0f, "uses contract workers"),
                            new MentalityString(1.0f, 1.2f, "uses paid workers"),
                            new MentalityString(1.2f, 1.4f, "uses serfs"),
                            new MentalityString(1.4f, 1.6f, "treats slaves as second-class citizens"),
                            new MentalityString(1.6f, 1.8f, "treats slaves as valuable property"),
                            new MentalityString(1.8f, 2.0f, "treats slaves as talking livestock"),
                        };
                    s_cMentalityStrings[Mentality.Fanaticism] = new MentalityString[] 
                        {   new MentalityString(0.0f, 0.2f, "absolutely open-minded"),
                            new MentalityString(0.2f, 0.4f, "very open-minded"),
                            new MentalityString(0.4f, 0.6f, "quite open-minded"),
                            new MentalityString(0.6f, 0.8f, "moderately open-minded"),
                            new MentalityString(0.8f, 1.0f, "not so open-minded"),
                            new MentalityString(1.0f, 1.2f, "narrow-minded"),
                            new MentalityString(1.2f, 1.4f, "quite narrow-minded"),
                            new MentalityString(1.4f, 1.6f, "very narrow-minded"),
                            new MentalityString(1.6f, 1.8f, "extremely narrow-minded"),
                            new MentalityString(1.8f, 2.0f, "absolute fanatics"),
                        };
                    s_cMentalityStrings[Mentality.Voluptuousness] = new MentalityString[] 
                        {   new MentalityString(0.0f, 0.2f, "enlightened"),
                            new MentalityString(0.2f, 0.4f, "ascets"),
                            new MentalityString(0.4f, 0.6f, "adepts of inner grow"),
                            new MentalityString(0.6f, 0.8f, "philosophers"),
                            new MentalityString(0.8f, 1.0f, "thinkers"),
                            new MentalityString(1.0f, 1.2f, "bourgeois"),
                            new MentalityString(1.2f, 1.4f, "hedonists"),
                            new MentalityString(1.4f, 1.6f, "delight seekers"),
                            new MentalityString(1.6f, 1.8f, "obsessed by carnal desires"),
                            new MentalityString(1.8f, 2.0f, "slaves of own desires"),
                        };
                    s_cMentalityStrings[Mentality.Guile] = new MentalityString[] 
                        {   new MentalityString(0.0f, 0.2f, "absolutely honest"),
                            new MentalityString(0.2f, 0.4f, "very honest"),
                            new MentalityString(0.4f, 0.6f, "quite honest"),
                            new MentalityString(0.6f, 0.8f, "honest"),
                            new MentalityString(0.8f, 1.0f, "not so honest"),
                            new MentalityString(1.0f, 1.2f, "treacherous"),
                            new MentalityString(1.2f, 1.4f, "quite treacherous"),
                            new MentalityString(1.4f, 1.6f, "very treacherous"),
                            new MentalityString(1.6f, 1.8f, "extremely treacherous"),
                            new MentalityString(1.8f, 2.0f, "absolute scoundrels"),
                        };
                }
                return s_cMentalityStrings;
            }
        }

        public string GetMentalityString(Mentality eMentality)
        {
            foreach (MentalityString pString in MentalityStrings[eMentality])
            {
                if (m_cMentalityValues[eMentality] >= pString.m_fMinValue && 
                    m_cMentalityValues[eMentality] <= pString.m_fMaxValue)
                    return pString.m_sString;
            }
            return "error";
        }
    }
}
