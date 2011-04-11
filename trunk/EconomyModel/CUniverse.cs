using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using SimpleVectors;

namespace EconomyModel
{
    public class CUniverse
    {
        public List<CWorld> m_cWorlds = new List<CWorld>();

        public List<CTradingRoute> m_cRoutes = new List<CTradingRoute>();

        public int m_iWidth;
        public int m_iHeight;

        public CUniverse(int iWidth, int iHeight, int iHabitablePercent)
        {
            List<CWorld> advanced = new List<CWorld>();
            for (int x = 0; x < iWidth; x++)
            {
                for (int y = 0; y < iHeight; y++)
                {
                    if (Rnd.Get(100) < iHabitablePercent)
                    {
                        CWorld pNewWorld = new CWorld(x, y);
                        pNewWorld.BuildColony();
                        if (Rnd.Get(100) < iHabitablePercent)
                        {
                            pNewWorld.Upgrade();
                            advanced.Add(pNewWorld);
                        }
                        m_cWorlds.Add(pNewWorld);
                    }
                }
            }
            if (advanced.Count > 0)
            {
                int choosen = Rnd.Get(advanced.Count);
                advanced[choosen].UnlimitedCredits = true;
            }

            m_iWidth = iWidth;
            m_iHeight = iHeight;
        }

        public void Advance()
        { 
            List<CTradingRoute> dead = new List<CTradingRoute>();
            foreach (CTradingRoute pRoute in m_cRoutes)
            {
                if (pRoute.Profit > 0)
                {
                    int iAmount = pRoute.Volume;

                    pRoute.m_pSeller.Wealth += iAmount * pRoute.m_pSeller.m_pStock[pRoute.m_eGoods].m_fPrice;
                    pRoute.m_pByer.Wealth -= iAmount * pRoute.m_pByer.m_pStock[pRoute.m_eGoods].m_fPrice;

                    pRoute.m_pSeller.m_pStock[pRoute.m_eGoods].m_iStore -= iAmount;
                    pRoute.m_pByer.m_pStock[pRoute.m_eGoods].m_iStore += iAmount;
                }
                else
                {
                    dead.Add(pRoute);
                }
            }
            foreach (CTradingRoute pRoute in dead)
            {
                pRoute.m_pSeller.m_cRoutes.Remove(pRoute);
                pRoute.m_pByer.m_cRoutes.Remove(pRoute);
                m_cRoutes.Remove(pRoute);
            }

            foreach (CWorld pWorld in m_cWorlds)
            {
                pWorld.Advance();
            }
        }

        public void AddRoute()
        {
            CTradingRoute pBestRoute = null;

            foreach (CWorld pSeller in m_cWorlds)
            {
                foreach (CWorld pByer in m_cWorlds)
                {
                    if (pSeller != pByer)
                    {
                        Goods eBestDeal = CMarket.FindBestByingPrice(pSeller, pByer);
                        if (eBestDeal != Goods.None)
                        {
                            CTradingRoute pRoute = new CTradingRoute(pSeller, pByer, eBestDeal);

                            if (pBestRoute == null || pRoute.Profit > pBestRoute.Profit)
                            {
                                bool bDouble = false;
                                int iTotalVolume = 0;
                                foreach (CTradingRoute pDoubleRoute in pByer.m_cRoutes)
                                {
                                    if (pDoubleRoute.m_pSeller == pSeller && pDoubleRoute.m_eGoods == eBestDeal)
                                        bDouble = true;
                                    if (pDoubleRoute.m_eGoods == eBestDeal)
                                        iTotalVolume += pDoubleRoute.Volume;
                                }
                                if (!bDouble && iTotalVolume < pByer.m_pConsumption[pRoute.m_eGoods])
                                {
                                    pBestRoute = pRoute;
                                }
                            }
                        }
                    }
                }
            }

            if (pBestRoute != null && pBestRoute.Profit > 0)
            {
                pBestRoute.m_pSeller.m_cRoutes.Add(pBestRoute);
                pBestRoute.m_pByer.m_cRoutes.Add(pBestRoute);
                m_cRoutes.Add(pBestRoute);
            }
        }
    }
}
