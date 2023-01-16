using Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandscapeGeneration
{
    /// <summary>
    /// Бассеин - группа сопредельных локаций (<see cref="Location"/>/<see cref="LocationBasin"/>) все воды с которых стекают в один водоём (море или озеро).
    /// Для простоты так же включает в себя территорию самого водоёма.
    /// Используется для формирования названий водоёмов и сздании рек.
    /// </summary>
    public class Basin : TerritoryCluster<Basin, Landscape, LocationBasin>
    {
        private bool m_bFinished = false;

        public int MaxDistance { get; private set; } = 0;
        public int MinDistance { get; private set; } = 0;

        public override void Start(LocationBasin pSeed)
        {
            if (pSeed.HasOwner())
                throw new InvalidOperationException("Location is a part of other basin already!");

            base.Start(pSeed);
            pSeed.Distance = -200;

            pSeed.SetOwner(this);
        }

        private int m_iOceanSize = 0;

        public bool GrowOcean()
        {
            return Grow(true, true, 0) != null;
        }

        public bool GrowLand()
        {
            if (m_iOceanSize == 0)
            {
                Reverse();
                m_iOceanSize = Contents.Count;
            }

            return Grow(false, true, m_iOceanSize * m_iOceanSize) != null;
        }

        public bool GrowForced()
        {
            return Grow(false, false, 0) != null;
        }

        /// <summary>
        /// Присоединяет к континенту сопредельную неприкаянную тектоническую плиту.
        /// </summary>
        /// <returns></returns>
        private LocationBasin Grow(bool bOnlyOcean, bool bCheckRidges, int iMaxSize)
        {
            if (m_bFinished)
                return null;

            LocationBasin pAddon = null;

            //Сохраняем текущий край в отдельный массив, чтобы потом бежать по нему и модифицировать актуальный край
            LocationBasin[] aBorderLocations = Border.Keys.ToArray();
            foreach (LocationBasin pBorderLoc in aBorderLocations)
            {
                Location pOuterLoc = pBorderLoc.Origin;

                if (pOuterLoc.Forbidden || pBorderLoc.HasOwner())
                    continue;

                if (bOnlyOcean && !pBorderLoc.Origin.GetOwner().IsWater)
                    continue;

                LocationBasin pBestInnerLoc = null;
                foreach (LocationBasin pLinkedLoc in pOuterLoc.BorderWithKeys.Select(x => x.As<LocationBasin>()))
                {
                    if (pLinkedLoc.GetOwner() == this)
                    {
                        if (pBestInnerLoc == null || pLinkedLoc.H < pBestInnerLoc.H)
                        {
                            pBestInnerLoc = pLinkedLoc;
                        }
                    }
                }

                if (pBestInnerLoc == null || (bCheckRidges && pBestInnerLoc.H > pBorderLoc.H && pBestInnerLoc.Origin.GetOwner().IsWater == pBorderLoc.Origin.GetOwner().IsWater))
                    continue;

                if (iMaxSize > 0 && !Rnd.Chances(iMaxSize, Contents.Count))
                    continue;

                pAddon = pBorderLoc;

                Contents.Add(pAddon);
                pAddon.SetOwner(this);
                pAddon.Distance = Math.Max(0, pBestInnerLoc.Distance ?? 0) + 1;

                if (MaxDistance < (pAddon.Distance ?? 0))
                    MaxDistance = pAddon.Distance ?? 0;

                Border[pAddon].Clear();
                Border.Remove(pAddon);

                foreach (var pLinked in pAddon.BorderWith)
                {
                    if (!pLinked.Key.Origin.Forbidden && Contents.Contains(pLinked.Key))
                        continue;

                    if (!Border.ContainsKey(pLinked.Key))
                        Border[pLinked.Key] = new List<VoronoiEdge>();
                    foreach (var pLine in pLinked.Value)
                        Border[pLinked.Key].Add(new VoronoiEdge(pLine));
                }
            }

            m_bFinished = pAddon == null;

            //Возвращаем ссылку на последню добавленную локацию как признак успешного роста.
            return pAddon;
        }

        [Obsolete("Use basin's own Grow methods instead!", true)]
        public override LocationBasin Grow(int iMaxSize)
        {
            throw new InvalidOperationException("Use basin's own Grow methods instead!");
        }

        private void Reverse()
        {
            if (MinDistance != 0)
                throw new InvalidOperationException("Can't call Reverse twice on same basin!");

            foreach (LocationBasin pLoc in Contents)
            {
                pLoc.Distance -= MaxDistance;
            }
            MinDistance = -MaxDistance;
            MaxDistance = 0;

            m_bFinished = false;
        }

        public void ResetGrow()
        {
            m_bFinished = false;
        }
    }
}
