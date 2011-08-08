using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace EconomyModel
{
    public class CSettlement: CMarket
    {
        public Dictionary<Goods, int> m_pConsumption = new Dictionary<Goods, int>();

        public int m_iSize = 0;
        public int m_iPopulation = 0;

        public double m_fHappiness = 1;

        private bool m_bUnlimitedCredits = false;

        public bool UnlimitedCredits
        {
            get { return m_bUnlimitedCredits; }
            set 
            { 
                m_bUnlimitedCredits = value;
                Wealth = Wealth;
            }
        }

        public void SetConsumption(Goods goods, int amount)
        {
            m_pStock[goods].m_bDemand = true;
            m_pConsumption[goods] = amount;
        }

        public CSettlement()
            : base()
        {
            foreach (Goods goods in Enum.GetValues(typeof(Goods)))
                m_pConsumption[goods] = 0;

            m_iSize = 1 + Rnd.Get(10);
        }

        public virtual void BuildColony()
        {
            m_iPopulation = m_iSize;
            //m_fWealth = m_iPopulation;
            UpdatePrimalNeeds();
        }

        private void UpdatePrimalNeeds()
        {
            SetConsumption(Goods.Food, m_iPopulation);
            SetConsumption(Goods.Medicine, m_iPopulation);
            SetConsumption(Goods.Clothes, m_iPopulation);
            SetConsumption(Goods.Electronics, m_iPopulation);
        }

        //в будущем обязательно заменить скачкообразный апгрейд на плавный рост населения и благосостояния
        public virtual void Upgrade()
        {
            m_iPopulation *= 3;
            UpdatePrimalNeeds();
            UpdateLuxuryNeeds();
        }

        private void UpdateLuxuryNeeds()
        {
            SetConsumption(Goods.Jevelry, m_iPopulation/3);
            SetConsumption(Goods.FacionClothes, m_iPopulation/3);
            SetConsumption(Goods.Alcogol, m_iPopulation/3);
            SetConsumption(Goods.Parfume, m_iPopulation/3);
            SetConsumption(Goods.Drugs, m_iPopulation/3);
            SetConsumption(Goods.MusicRecords, m_iPopulation/3);
            SetConsumption(Goods.SimStimRecords, m_iPopulation/3);
            SetConsumption(Goods.VRModules, m_iPopulation/3);
        }

        public override double Wealth
        {
            get { return m_fWealth; }
            set 
            { 
                m_fWealth = value;
                if (m_fWealth <= m_iPopulation*10 && m_bUnlimitedCredits)
                    m_fWealth = m_iPopulation*10;
            }
        }

        public override void Advance()
        {
            double fSucceed = 1;
            foreach (Goods goods in Enum.GetValues(typeof(Goods)))
            {
                if (m_pConsumption[goods] > 0)
                {
                    if (m_pStock[goods].m_iStore < m_pConsumption[goods])
                    {
                        double fPartialSucceed = (double)m_pStock[goods].m_iStore / m_pConsumption[goods];
                        fSucceed *= 0.9 + fPartialSucceed / 10;
                    }
                    m_pStock[goods].m_iStore -= m_pConsumption[goods];
                    if (m_pStock[goods].m_iStore < 0)
                        m_pStock[goods].m_iStore = 0;
                }
            }
            m_fHappiness = (m_fHappiness + fSucceed) / 2;

            base.Advance();
        }
    }
}
