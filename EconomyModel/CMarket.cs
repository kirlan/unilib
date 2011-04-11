using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomyModel
{
    public enum Goods
    { 
        None,
        Ore,
        Oil,
        Food,
        Alloys,
        Chemicals,
        Programms,
        Weapon,
        Armor,
        Medicine,
        Clothes,
        Electronics,
        Jevelry,
        FacionClothes,
        Alcogol,
        Parfume,
        Drugs,
        MusicRecords,
        SimStimRecords,
        VRModules
    }

    public class CMarketRecord
    {
        public int m_iStore = 0;
        public double m_fPrice = 1;
        public bool m_bDemand = false;
        public bool m_bOffer = false;
    }

    public class CMarket
    {
        public static Goods[] PrimaryNeeds = 
        { 
            Goods.Clothes, 
            Goods.Electronics, 
            Goods.Food, 
            Goods.Medicine 
        };
        public static Goods[] Luctury = 
        { 
            Goods.Alcogol, 
            Goods.Drugs, 
            Goods.FacionClothes, 
            Goods.Jevelry,
            Goods.MusicRecords,
            Goods.Parfume,
            Goods.SimStimRecords,
            Goods.VRModules
        };

        public Dictionary<Goods, CMarketRecord> m_pStock = new Dictionary<Goods, CMarketRecord>();

        public List<CTradingRoute> m_cRoutes = new List<CTradingRoute>();

        protected double m_fWealth = 0;

        public virtual double Wealth
        {
            get { return m_fWealth; }
            set { m_fWealth = value; }
        }

        public CMarket()
        {
            foreach (Goods goods in Enum.GetValues(typeof(Goods)))
                m_pStock[goods] = new CMarketRecord();
        }

        public bool IsStable()
        {
            foreach (Goods goods in Enum.GetValues(typeof(Goods)))
            {
                if(PrimaryNeeds.Contains(goods) && m_pStock[goods].m_iStore <= 0)
                    return false;
            }
            return true;
        }

        public virtual void Advance()
        {
            foreach (Goods goods in Enum.GetValues(typeof(Goods)))
            {
                double ink = 1.1;

                if (Luctury.Contains(goods) && !IsStable())
                    ink = 0.999;

                if (m_pStock[goods].m_iStore == 0 && m_pStock[goods].m_bDemand)
                    m_pStock[goods].m_fPrice = m_pStock[goods].m_fPrice * ink;

                if (m_pStock[goods].m_iStore > 0)
                    m_pStock[goods].m_fPrice = m_pStock[goods].m_fPrice * 0.999;
//                m_pStock[goods].m_fPrice = m_pStock[goods].m_fPrice * Math.Pow(0.999, m_pStock[goods].m_iStore);

                if (m_pStock[goods].m_bDemand && m_pStock[goods].m_iStore == 0 && m_pStock[goods].m_fPrice > m_fWealth)
                    m_pStock[goods].m_fPrice = m_fWealth;

                if (m_pStock[goods].m_fPrice < 0.01)
                    m_pStock[goods].m_fPrice = 0.01;
            }
        }

        public static double GetProfit(CMarket pSeller, CMarket pByer, Goods eGoods)
        {
            return pByer.m_pStock[eGoods].m_fPrice - pSeller.m_pStock[eGoods].m_fPrice;
        }

        public static Goods FindBestByingPrice(CMarket pSeller, CMarket pByer)
        {
            Goods eBestDeal = Goods.None;
            double fBestProfit = 0;
            foreach (Goods goods in Enum.GetValues(typeof(Goods)))
            {
                if (pSeller.m_pStock[goods].m_iStore > 0 && pByer.m_pStock[goods].m_bDemand)
                {
                    double fProfit = GetProfit(pSeller, pByer, goods);
                    if (fProfit > fBestProfit)
                    {
                        fBestProfit = fProfit;
                        eBestDeal = goods;
                    }
                }
            }
            return eBestDeal;
        }
    }
}
