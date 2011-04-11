using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace EconomyModel
{
    public class CFactory: CSettlement
    {
        public Dictionary<Goods, int> m_pProduction = new Dictionary<Goods, int>();

        public CFactory()
            : base()
        {
            foreach (Goods goods in Enum.GetValues(typeof(Goods)))
                m_pProduction[goods] = 0;
        }

        public override void BuildColony()
        {
            base.BuildColony();

            int freeProduction = m_iSize;
            int prod = Rnd.Get(freeProduction + 1);
            freeProduction -= prod;
            SetProduction(Goods.Food, prod);

            prod = Rnd.Get(freeProduction + 1);
            freeProduction -= prod;
            SetProduction(Goods.Ore, prod);

            SetProduction(Goods.Oil, freeProduction);
        }

        private Goods GetRandomFutureProduction()
        {
            List<Goods> cGoods = new List<Goods>();

            foreach (Goods goods in Enum.GetValues(typeof(Goods)))
            {
                if (goods != Goods.Food &&
                    goods != Goods.Ore &&
                    goods != Goods.Oil &&
                    goods != Goods.None &&
                    m_pProduction[goods] == 0)
                {
                    cGoods.Add(goods);
                }
            }

            if (cGoods.Count > 0)
            {
                return cGoods[Rnd.Get(cGoods.Count)];
            }

            return Goods.None;
        }

        private Goods GetRandomCurrentProduction()
        {
            List<Goods> cGoods = new List<Goods>();

            foreach (Goods goods in Enum.GetValues(typeof(Goods)))
            {
                if (goods != Goods.Food &&
                    goods != Goods.Ore &&
                    goods != Goods.Oil &&
                    goods != Goods.None &&
                    m_pProduction[goods] != 0)
                {
                    cGoods.Add(goods);
                }
            }

            if (cGoods.Count > 0)
            {
                return cGoods[Rnd.Get(cGoods.Count)];
            }

            return Goods.None;
        }

        public void SetProduction(Goods goods, int amount)
        {
            if (amount > 0)
            {
                m_pStock[goods].m_bOffer = true;
                m_pProduction[goods] = amount;
                m_pStock[goods].m_iStore = amount*amount;
            }
        }

        //в будущем обязательно заменить скачкообразный апгрейд на плавное линейное развитие
        public override void Upgrade()
        {
            int iPeasants = m_iPopulation;

            base.Upgrade();

            int freeProduction = m_iPopulation - iPeasants;

            SetConsumption(Goods.Alloys, freeProduction);
            SetConsumption(Goods.Chemicals, freeProduction);
            SetConsumption(Goods.Programms, freeProduction);

            do
            {
                Goods choosen = GetRandomFutureProduction();
                if (choosen == Goods.None)
                    break;
                
                int prod = Rnd.Get(freeProduction + 1);
                freeProduction -= prod;
                SetProduction(choosen, prod);
            }
            while(freeProduction > 0);

            if (freeProduction > 0)
            { 
                Goods choosen = GetRandomCurrentProduction();
                if (choosen != Goods.None)
                {
                    m_pProduction[choosen] += freeProduction;
                }
            }
        }

        public override void Advance()
        {
            foreach (Goods goods in Enum.GetValues(typeof(Goods)))
            {
                if (m_pProduction[goods] > 0)
                {
                    m_pStock[goods].m_iStore += (int)(m_pProduction[goods] * m_pProduction[goods] * m_fHappiness);
                }
            }

            base.Advance();
        }
    }
}
