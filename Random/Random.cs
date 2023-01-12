using System;
using System.Collections.Generic;
using System.Text;

namespace Random
{
    public class Rnd
    {
        private static System.Random rnd = new System.Random();

        private static int seed = 0;

        public static int GetNewSeed()
        {
            seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
            SetSeed(seed);
            return seed;
        }

        public static void SetSeed(int iSeed)
        {
            seed = iSeed;

            rnd = new System.Random(seed);

            //int iTest = rnd.Next();
        }

        private static readonly List<double> s_cDebug = new List<double>();
        private static readonly List<double> s_cDebug2 = new List<double>();

        static private bool s_bDebug = false;

        public static void SetDebug(bool bDebug)
        {
            s_bDebug = bDebug;
            if (s_bDebug)
            {
                s_cDebug2.Clear();
                s_cDebug2.AddRange(s_cDebug);

                s_cDebug.Clear();
            }
        }

        /// <summary>
        /// Toss a dice!
        /// </summary>
        /// <returns>random number in 1..6</returns>
        public static int Toss1d6()
        {
            return GetInRange(1, 6);
        }

        /// <summary>
        /// A double-precision floating point number greater than or equal to 0.0, and less than 1.0.
        /// </summary>
        /// <returns>random double in 0..1</returns>
        public static double Sample()
        {
            double fNext = rnd.NextDouble();
            if (s_bDebug)
            {
                s_cDebug.Add(fNext);

                if (s_cDebug.Count <= s_cDebug2.Count)
                {
                    if (fNext != s_cDebug2[s_cDebug.Count - 1])
                        return fNext;
                }
            }

            return fNext;
        }

        /// <summary>
        /// Returns random integer in given range (max value inclusive!)
        /// </summary>
        /// <param name="iMin">min. possible value</param>
        /// <param name="iMax">max. possible value</param>
        /// <returns>random number in iMin..iMax</returns>
        public static int GetInRange(int iMin, int iMax)
        {
            return iMin + Get(iMax - iMin + 1);
        }

        /// <summary>
        /// Returns nonnegative integer, lesser then val
        /// </summary>
        /// <param name="val">max (can't be returned)</param>
        /// <returns>random integer</returns>
        public static int Get(int val)
        {
            if (val <= 0)
                return 0;

            int iNext = rnd.Next(val);
            if (s_bDebug)
            {
                s_cDebug.Add(iNext);

                if (s_cDebug.Count <= s_cDebug2.Count)
                {
                    if (iNext != s_cDebug2[s_cDebug.Count - 1])
                        return iNext;
                }
            }

            return iNext;
        }

        /// <summary>
        /// Returns float, lesser then val by absolute value
        /// </summary>
        /// <param name="val">max (can't be returned)</param>
        /// <returns>random float</returns>
        public static float Get(float val)
        {
            if (val <= 0.0f)
                return -Get(-val);

            return val * (float)Sample();
        }

        /// <summary>
        /// Returns double, lesser then val by absolute value
        /// </summary>
        /// <param name="val">max (can't be returned)</param>
        /// <returns>random double</returns>
        public static double Get(double val)
        {
            if (val <= 0.0)
                return -Get(-val);

            return val * Sample();
        }

        /// <summary>
        /// Returns random element of given enum
        /// </summary>
        /// <param name="enumType">Enum type (by typeof(EnumName))</param>
        /// <returns></returns>
        public static object Get(Type enumType)
        {
            return Enum.GetValues(enumType).GetValue(Rnd.Get(Enum.GetValues(enumType).Length));
        }

        /// <summary>
        /// Returns exponentially random element of given enum.
        /// First elements of enum have a much higher chances to be choosen.
        /// </summary>
        /// <param name="enumType">Enum type (by typeof(EnumName))</param>
        /// <returns></returns>
        public static object GetExp(Type enumType, int iPow)
        {
            Array pArray = Enum.GetValues(enumType);

            int i = Get(100);
            double m = Math.Pow(i, iPow);
            double n = Math.Pow(100, iPow);
            int iChance = (int)(m * pArray.Length / n);

            return pArray.GetValue(iChance);
        }

        /// <summary>
        /// Returns exponentially random element in range (0..val-1).
        /// First elements of enum have a much higher chances to be choosen.
        /// </summary>
        /// <param name="enumType">Enum type (by typeof(EnumName))</param>
        /// <returns></returns>
        public static int GetExp(int val, int iPow)
        {
            int i = Get(100);
            double m = Math.Pow(i, iPow);
            double n = Math.Pow(100, iPow);
            return (int)(m * val / n);
        }

        /// <summary>
        /// Returns random element from given char collection
        /// </summary>
        /// <param name="cColl"></param>
        /// <returns></returns>
        public static T Get<T>(ICollection<T> cColl)
        {
            int iChoosen = Get(cColl.Count);

            foreach (T iChance in cColl)
            {
                if (iChoosen <= 0)
                    return iChance;
                iChoosen--;
            }

            throw new Exception("No chances!");
        }

        /// <summary>
        /// Возвращает true в одном случае из указанного числа.
        /// Если параметр меньше или равен 0, возвращает false.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool OneChanceFrom(int val)
        {
            if (val <= 0)
                return false;

            return Get(val) == 0;
        }

        public static bool Chances(int iChances, int iTotal)
        {
            return Get(iTotal) < iChances;
        }

        public static bool Chances(float fChances, float fTotal)
        {
            return Get(fTotal) < fChances;
        }

        private static readonly double SqrtPI = Math.Sqrt(Math.PI);

        public static bool Gauss(int iValue, int k)
        {
            //double Gau1 = A * Math.Pow(Math.E, -a * 0 * 0);
            //double Gau2 = A * Math.Pow(Math.E, -a * 0.2 * 0.2);
            //double Gau3 = A * Math.Pow(Math.E, -a * 0.4 * 0.4);
            //double Gau4 = A * Math.Pow(Math.E, -a * 0.6 * 0.6);
            //double Gau5 = A * Math.Pow(Math.E, -a * 0.8 * 0.8);
            //double Gau6 = A * Math.Pow(Math.E, -a * 1.0 * 1.0);

            double fValue = (double)iValue / k;

            double Gau = Math.Pow(Math.E, -SqrtPI * fValue * fValue);

            return Sample() < Gau;
        }

        public static bool ChooseOne(double iChances1, double iChances2)
        {
            //return rnd.Next((int)(iChances1 * 10000 + iChances2 * 10000)) < (int)(iChances1 * 10000);
            return Sample() * (iChances1 * 10000 + iChances2 * 10000) < iChances1 * 10000;
        }

        /// <summary>
        /// Возвращает true в iChances1 случаях и false в iChances2 случаях
        /// Эквивалентно вызову Chances(iChances1, iChances1 + iChances2)
        /// </summary>
        /// <param name="iChances1"></param>
        /// <param name="iChances2"></param>
        /// <returns></returns>
        public static bool ChooseOne(int iChances1, int iChances2)
        {
            return Chances(iChances1, iChances1 + iChances2);
        }

        public static int ChooseOne(ICollection<int> cChances, int iPow)
        {
            int iTotal = 0;
            foreach (int iChance in cChances)
                iTotal += (int)Math.Pow(iChance, iPow);

            int iChoosen = Get(iTotal);

            int index = 0;
            foreach (int iChance in cChances)
            {
                if (iChance > 0)
                {
                    iChoosen -= (int)Math.Pow(iChance, iPow);
                    if (iChoosen <= 0)
                        return index;
                }
                index++;
            }

            return -1;
        }

        public static int ChooseBest(ICollection<float> cChances)
        {
            if (cChances.Count == 0)
                return -1;

            float fBest = 0;
            List<int> cBests = new List<int>();
            int iIndex = 0;
            foreach (float fChance in cChances)
            {
                if (fBest == fChance)
                {
                    cBests.Add(iIndex);
                }
                else if (fBest < fChance)
                {
                    fBest = fChance;
                    cBests.Clear();
                    cBests.Add(iIndex);
                }
                iIndex++;
            }

            return cBests[Get(cBests.Count)];
        }

        public static int ChooseBest(ICollection<int> cChances)
        {
            if (cChances.Count == 0)
                return -1;

            int iBest = 0;
            List<int> cBests = new List<int>();
            int iIndex = 0;
            foreach (int iChance in cChances)
            {
                if (iBest == iChance)
                {
                    cBests.Add(iIndex);
                }
                else if (iBest < iChance)
                {
                    iBest = iChance;
                    cBests.Clear();
                    cBests.Add(iIndex);
                }
                iIndex++;
            }

            return cBests[Get(cBests.Count)];
        }

        /// <summary>
        /// Возвращает индекс случайного элемента в коллекции в соответствии с их весами.
        /// ВНИМАНИЕ: Если коллекция пустая - возвращает -1
        /// </summary>
        /// <param name="cChances">коллекция весов</param>
        /// <param name="iPow">степень в которую возводится вес при сравнении</param>
        /// <returns></returns>
        public static int ChooseOne(ICollection<float> cChances, int iPow)
        {
            if (cChances.Count == 0)
                return -1;

            float fTotal = 0;
            foreach (float fChance in cChances)
                fTotal += (float)Math.Pow(fChance, iPow);

            float fChoosen = Get(fTotal);

            int index = 0;
            foreach (float fChance in cChances)
            {
                if (fChance > 0.0f)
                {
                    fChoosen -= (float)Math.Pow(fChance, iPow);
                    if (fChoosen <= 0.0f)
                        return index;
                }
                index++;
            }

            index = 0;
            foreach (float fChance in cChances)
            {
                if (fChance > 0.0f)
                    return index;

                index++;
            }

            return Get(cChances.Count);
        }

        public static int ChooseOne(ICollection<float> cChances)
        {
            if (cChances.Count == 0)
                return -1;

            float fTotal = 0;
            foreach (float fChance in cChances)
                fTotal += fChance;

            float fChoosen = Get(fTotal);

            int index = 0;
            foreach (float fChance in cChances)
            {
                if (fChance > 0.0f)
                {
                    fChoosen -= fChance;
                    if (fChoosen <= 0.0f)
                        return index;
                }
                index++;
            }

            index = 0;
            foreach (float fChance in cChances)
            {
                if (fChance > 0.0f)
                    return index;

                index++;
            }

            return Get(cChances.Count);
        }
    }
}
