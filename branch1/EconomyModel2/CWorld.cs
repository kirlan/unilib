using System;
using System.Collections.Generic;
using System.Text;
using Random;
using EconomyModel2.Buildings;
using NameGen;
using EconomyModel2.Trading;

namespace EconomyModel2
{
    public enum WorldType
    { 
        Colony,
        AdvancedWorld,
        Metropoly
    }

    public class CWorld
    {
        private int m_iX;
        public int X
        {
            get { return m_iX; }
        }

        private int m_iY;
        public int Y
        {
            get { return m_iY; }
        }

        private int m_iSize;
        public int Size
        {
            get { return m_iSize; }
        }

        private string m_sName = "";

        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        public CWorld(int iX, int iY, WorldType _type)
        {
            m_pSpaceport = new CSpaceport(this);

            m_iX = iX;
            m_iY = iY;

            m_iSize = 1 + Rnd.Get(10);

            switch (_type)
            {
                case WorldType.Colony:
                    BuildColony();
                    break;
                case WorldType.AdvancedWorld:
                    BuildAdvancedWorld();
                    break;
                case WorldType.Metropoly:
                    BuildMetropoly();
                    break;
            }
            BuildSpaceport();

            m_sName = NameGenerator.GetAbstractName(Rnd.Get(9999));
        }

        private CSpaceport m_pSpaceport = null;

        public CSpaceport Spaceport
        {
            get { return m_pSpaceport; }
        }

        private void Clear()
        {
            m_iPopulation = 0;
            m_cBuildings.Clear();
            m_pStock.Clear();
        }

        private void BuildMetropoly()
        {
            Clear();

            BuildAdvancedWorld();
        }

        private void BuildAdvancedWorld()
        {
            Clear();

            BuildColony();

            int iPeasants = m_iPopulation;
            m_iPopulation *= 3;
            BuildLivingQuarteries(m_iSize*2);

            int freeProduction = m_iPopulation - iPeasants;
            BuildFactories(GOODS.ManufacturedGoods, freeProduction);

            //Goods choosen = GetRandomFutureProduction();
            //BuildFactory(choosen, freeProduction);
        }

        private void BuildColony()
        {
            Clear();

            m_iPopulation = m_iSize;
            BuildLivingQuarteries(m_iSize);

            BuildFactories(GOODS.PrimeResources, m_iSize);
        }

        private void BuildLivingQuarteries(int iSize)
        {
            CLivingQuarters living = null;

            foreach (CBuilding building in m_cBuildings)
            {
                if (building is CLivingQuarters)
                {
                    living = (CLivingQuarters)building;
                    break;
                }
            }
            if (living == null)
            {
                living = new CLivingQuarters(m_pStock);
                m_cBuildings.Add(living);
            }

            living.UpgradeToLevel(living.Level + iSize);
        }

        private void BuildSpaceport()
        {
            if (m_cBuildings.Contains(m_pSpaceport))
                m_cBuildings.Remove(m_pSpaceport);

            m_cBuildings.Add(m_pSpaceport);
        }

        private bool AlreadyProducing(Goods goods)
        {
            foreach (CBuilding factory in m_cBuildings)
            {
                if (factory.Production.ContainsKey(goods) && factory.Production[goods].Amount > 0)
                    return true;
            }
            return false;
        }

        private Goods GetRandomFutureProduction(List<Goods> possibleList)
        {
            List<Goods> cGoods = new List<Goods>();

            foreach (Goods goods in possibleList)
            {
                if (!AlreadyProducing(goods))
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

        private void BuildFactories(List<Goods> productionList, int count)
        {
            do
            {
                Goods choosen = GetRandomFutureProduction(productionList);
                if (choosen == Goods.None)
                    break;

                int prod = Rnd.Get(count + 1);
                count -= prod;
                BuildFactory(choosen, prod);
            }
            while (count > 0);

            if (count > 0)
            {
                CBuilding factory = m_cBuildings[Rnd.Get(m_cBuildings.Count)];
                factory.UpgradeToLevel(factory.Level + count);
            }
        }

        private void BuildFactory(Goods goods, int iLevel)
        {
            if (iLevel > 0)
            {
                if (GOODS.ManufacturedGoods.Contains(goods))
                {
                    CFactory factory = new CFactory(m_pStock, goods);
                    factory.UpgradeToLevel(iLevel);
                    m_cBuildings.Add(factory);
                }
                else
                {
                    CRefinery factory = new CRefinery(m_pStock, goods);
                    factory.UpgradeToLevel(iLevel);
                    m_cBuildings.Add(factory);
                }
            }
        }

        private CStock m_pStock = new CStock();
        public CStock Stock
        {
            get
            {
                return m_pStock;
            }
        }

        private List<CBuilding> m_cBuildings = new List<CBuilding>();
        public List<CBuilding> Buildings
        {
            get
            {
                return m_cBuildings;
            }
        }

        private int m_iPopulation = 0;
        public int Population
        {
            get
            {
                return m_iPopulation;
            }
        }

        public void Consume()
        {
            foreach (CBuilding building in m_cBuildings)
                building.Consume();

            m_pStock.ResetShortage();
        }

        public void Produce()
        {
            foreach (CBuilding building in m_cBuildings)
            {
                building.Produce();
            }
        }
    }
}
