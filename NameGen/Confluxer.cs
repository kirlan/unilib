using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using System.Globalization;

namespace NameGen
{
    /// <summary>
    /// The language confluxer takes a list of words and associates every sequential pair of letters 
    /// with the list of letters that might possibly follow the pair. The program chooses a pair that 
    /// can occur at the beginning of a word, then selects one of the letters from the associated list,
    /// and forms a new pair, repeating until a whitespace character is found or until a max-length 
    /// is reached. Simple.
    /// </summary>
    public class Confluxer
    {
        class Voxel
        {
            public string m_sPair;
            public char m_sNext;
            public bool m_bLast = false;

            public Voxel(string sPair, char sNext)
            {
                m_sPair = sPair;
                m_sNext = sNext;
            }

            public Voxel(string sPair)
            {
                m_sPair = sPair;
                m_sNext = ' ';
                m_bLast = true;
            }
        }

        private Dictionary<char, List<Voxel>> m_cDictionary = new Dictionary<char, List<Voxel>>();
        private Dictionary<char, List<Voxel>> m_cDictionaryStart = new Dictionary<char, List<Voxel>>();
        private Dictionary<char, List<Voxel>> m_cDictionaryFinish = new Dictionary<char, List<Voxel>>();

        private Dictionary<int, int> m_cLengthProbability = new Dictionary<int,int>();

        private int m_iSize = 2;

        public Confluxer(string sDictionary, int iSize)
        {
            m_iSize = iSize;

            sDictionary = sDictionary.Trim();

            string[] aDictionary = sDictionary.Split(' ', '\n', '\t');

            foreach (string sWord in aDictionary)
            {
                if (sWord.Length == 0)
                    continue;

                if(!m_cLengthProbability.ContainsKey(sWord.Length))
                    m_cLengthProbability[sWord.Length] = 0;
                
                m_cLengthProbability[sWord.Length]++;

                if (sWord.Length <= m_iSize)
                {
                    Voxel pOnlyVoxel = new Voxel(sWord);
                    if (!m_cDictionaryStart.ContainsKey(sWord[0]))
                        m_cDictionaryStart[sWord[0]] = new List<Voxel>();

                    m_cDictionaryStart[sWord[0]].Add(pOnlyVoxel);
                    continue;
                }

                Voxel pVoxel = new Voxel(sWord.Substring(0, m_iSize), sWord[m_iSize]);
                if (!m_cDictionaryStart.ContainsKey(sWord[0]))
                    m_cDictionaryStart[sWord[0]] = new List<Voxel>();

                m_cDictionaryStart[sWord[0]].Add(pVoxel);

                for (int i = 1; i < sWord.Length - m_iSize; i++)
                {
                    pVoxel = new Voxel(sWord.Substring(i, m_iSize), sWord[i + m_iSize]);
                    if (!m_cDictionary.ContainsKey(sWord[i]))
                        m_cDictionary[sWord[i]] = new List<Voxel>();

                    m_cDictionary[sWord[i]].Add(pVoxel);
                }

                for (int i = m_iSize; i > 0; i--)
                {
                    Voxel pLastVoxel = new Voxel(sWord.Substring(sWord.Length - i, i));
                    if (!m_cDictionaryFinish.ContainsKey(sWord[sWord.Length - i]))
                        m_cDictionaryFinish[sWord[sWord.Length - i]] = new List<Voxel>();

                    m_cDictionaryFinish[sWord[sWord.Length - i]].Add(pLastVoxel);
                }
            }
        }

        private static string Capitalize(string value)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
        }

        public string Generate()
        {
            int iLengthPropb = Rnd.ChooseOne(m_cLengthProbability.Values, 1);
            int iLength = 0;
            foreach(int iLen in m_cLengthProbability.Keys)
            {
                if(iLengthPropb <= 0)
                {
                    iLength = iLen;
                    break;
                }
                iLengthPropb--;
            }

            char iKey = Rnd.Get(m_cDictionaryStart.Keys);
            Voxel pVoxel = m_cDictionaryStart[iKey][Rnd.Get(m_cDictionaryStart[iKey].Count)];
            string sWord = pVoxel.m_sPair;
            while (sWord.Length < iLength - m_iSize || !m_cDictionaryFinish.ContainsKey(pVoxel.m_sNext))
            {
                if (!m_cDictionary.ContainsKey(pVoxel.m_sNext))
                    break;

                Voxel pNextVoxel = m_cDictionary[pVoxel.m_sNext][Rnd.Get(m_cDictionary[pVoxel.m_sNext].Count)];
                sWord += pNextVoxel.m_sPair;
                
                pVoxel = pNextVoxel;
            }

            if (m_cDictionaryFinish.ContainsKey(pVoxel.m_sNext))
            {
                Voxel pNextVoxel = m_cDictionaryFinish[pVoxel.m_sNext][Rnd.Get(m_cDictionaryFinish[pVoxel.m_sNext].Count)];
                sWord += pNextVoxel.m_sPair;
            }

            return Capitalize(sWord);
        }
    }
}
