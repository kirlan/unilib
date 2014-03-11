using System;
using System.Collections.Generic;
using System.Text;

namespace Random
{
    public class Rnd
    {
        static System.Random rnd = new System.Random();

        /// <summary>
        /// Toss a dice!
        /// </summary>
        /// <returns>random number in 1..6</returns>
        public static int Toss1d6()
        {
            return 1 + rnd.Next(6);
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

            return rnd.Next(val);
        }

        /// <summary>
        /// Returns nonnegative float, lesser then val
        /// </summary>
        /// <param name="val">max (can't be returned)</param>
        /// <returns>random float</returns>
        public static float Get(float val)
        {
            if (val <= 0.0f)
                return 0.0f;

            return val * (float)rnd.NextDouble();
        }

        /// <summary>
        /// Returns nonnegative double, lesser then val
        /// </summary>
        /// <param name="val">max (can't be returned)</param>
        /// <returns>random double</returns>
        public static double Get(double val)
        {
            if (val <= 0.0)
                return 0.0;

            return val * rnd.NextDouble();
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
        public static char Get(ICollection<char> cColl)
        {
            int iChoosen = Get(cColl.Count);

            foreach (char iChance in cColl)
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

            return rnd.Next(val) == 0;
        }

        public static bool Chances(int iChances, int iTotal)
        {
            return rnd.Next(iTotal) < iChances;
        }

        private static double a = Math.Sqrt(Math.PI);

        public static bool Gauss(int iValue, int k)
        {
            //double Gau1 = A * Math.Pow(Math.E, -a * 0 * 0);
            //double Gau2 = A * Math.Pow(Math.E, -a * 0.2 * 0.2);
            //double Gau3 = A * Math.Pow(Math.E, -a * 0.4 * 0.4);
            //double Gau4 = A * Math.Pow(Math.E, -a * 0.6 * 0.6);
            //double Gau5 = A * Math.Pow(Math.E, -a * 0.8 * 0.8);
            //double Gau6 = A * Math.Pow(Math.E, -a * 1.0 * 1.0);

            double fValue = (double)iValue / k;

            double Gau = Math.Pow(Math.E, -a * fValue * fValue);

            return rnd.NextDouble() < Gau;
        }

        public static bool ChooseOne(double iChances1, double iChances2)
        {
            //return rnd.Next((int)(iChances1 * 10000 + iChances2 * 10000)) < (int)(iChances1 * 10000);
            return rnd.NextDouble()*(iChances1 * 10000 + iChances2 * 10000) < iChances1 * 10000;
        }

        /// <summary>
        /// Возвращает true в iChances1 случаях и false в iChances2 случаях
        /// </summary>
        /// <param name="iChances1"></param>
        /// <param name="iChances2"></param>
        /// <returns></returns>
        public static bool ChooseOne(int iChances1, int iChances2)
        {
            return rnd.Next(iChances1 + iChances2) < iChances1;
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

        public static int ChooseOne(ICollection<float> cChances, int iPow)
        {
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

            return -1;
        }

        public static int ChooseOne(ICollection<float> cChances)
        {
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
            
            return -1;
        }
    }
}
