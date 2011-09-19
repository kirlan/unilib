using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration;
using Random;
using NameGen;
using LandscapeGeneration.PathFind;
using Socium.Settlements;
using Socium.Nations;
using Socium.Psichology;

namespace Socium
{
    public class Province : BorderBuilder<LandX>, ITerritory
    {
        /// <summary>
        /// ТОЛКО ДЛЯ ОТЛАДКИ!!!!
        /// </summary>
        public Dictionary<Province, string> m_cConnectionString = new Dictionary<Province,string>();

        public string m_sName;

        private bool m_bForbidden = false;

        private static Province m_pForbidden = new Province(true);

        public bool Forbidden
        {
            get { return m_bForbidden; /* this == Province.m_pForbidden; */ }
        }

        public List<LandX> m_cContents = new List<LandX>();

        private Dictionary<object, List<Line>> m_cBorderWith = new Dictionary<object, List<Line>>();

        private object m_pOwner = null;

        public object Owner
        {
            get { return m_pOwner; }
            set { m_pOwner = value; }
        }

        public Dictionary<object, List<Line>> BorderWith
        {
            get { return m_cBorderWith; }
        }


        public object[] m_aBorderWith = null;

        private float m_fPerimeter = 0;

        public float PerimeterLength
        {
            get { return m_fPerimeter; }
        }

        internal void FillBorderWithKeys()
        {
            m_aBorderWith = new List<object>(m_cBorderWith.Keys).ToArray();

            m_fPerimeter = 0;
            foreach (var pBorder in m_cBorderWith)
                foreach (Line pLine in pBorder.Value)
                    m_fPerimeter += pLine.m_fLength;
        }

        public Nation m_pNation = null;

        public LandX m_pCenter;

        public LocationX m_pAdministrativeCenter = null;

        /// <summary>
        /// 1 - Бронзовый век. В ходу бронзовые кирасы и кожаные доспехи, бронзовое холодное оружие, из стрелкового оружия – луки и дротики.
        /// 2 - Железный век. Стальное холодное оружие, кольчуги, рыцарские доспехи. Из стрелкового оружия - луки и арбалеты.
        /// 3 - Эпоха пороха. Примитивное однозарядное огнестрельное оружие, лёгкие сабли и шпаги, облегчённая броня (кирасы, кожаные куртки).
        /// 4 - Индустриальная эра. Нарезное огнестрельное оружие, примитивная бронетехника и авиация, химическое оружие массового поражения.
        /// 5 - Атомная эра. Автоматическое огнестрельное оружие, бронежилеты, развитая бронетехника и авиация, ядерные ракеты и бомбы.
        /// 6 - Энергетическая эра. Силовые поля, лучевое оружие.
        /// 7 - Квантовая эра. Ограниченная телепортация, материализация, дезинтеграция.
        /// 8 - Переход. Полная и неограниченная власть человека над пространственно-временным континуумом.
        /// </summary>
        public int m_iTechLevel = 0;
        /// <summary>
        /// 1 - Мистика. Ритуальная магия, требующая длительной предварительной подготовки. Используя психотропные вещества, гипноз и йогические практики, мистики могут общаться с миром духов, получая из него информацию или заключая сделки с его обитателями.
        /// 2 - Псионика. Познание окружающего мира силой собственной мысли. Эмпатия, телепатия, ясновиденье, дальновиденье.
        /// 3 - Сверхспособности. Управление материальными объектами без физического контакта: телекинез, пирокинез, левитация и т.д.... Как правило, один отдельно взятый персонаж может развить у себя только одну-две сверхспособности.
        /// 4 - Традиционная фэнтезийная магия. То же самое, что и на предыдущем уровне, но гораздо доступнее. Один маг может владеть десятками заклинаний на разные случаи жизни.
        /// 5 - Джинны. Способность произвольно менять облик, массу и физические свойства собственного тела. Использование магии без заклинаний - просто усилием мысли.
        /// 6 - Титаны. Уровень личного могущества, обычно приписываемый языческим богам. Телепортация, материализация, управление течением времени.
        /// 7 - Трансцендентность. Отсутствие привязки к физическом телу. Разум способен существовать  в нематериальной форме, при этом сохраняя все свои возможности воспринимать окружающую среду и воздействать на неё.
        /// 8 - Единое. Границы между индивидуальностями стираются, фактически всё сообщество является единым разумным существом, неизмеримо более могущественным, чем составляющие его отдельные личности сами по себе.
        /// </summary>
        public int m_iMagicLimit = 0;
        /// <summary>
        /// Доступный жителям уровень жизни.
        /// Зависит от технического и магического развития, определяет доступные формы государственного правления
        /// </summary>
        public int m_iInfrastructureLevel = 0;

        public Culture m_pCulture = null;
        public Customs m_pCustoms = null;

        /// <summary>
        /// Как часто встрачаются носители магических способностей
        /// </summary>
        public MagicAbilityPrevalence m_eMagicAbilityPrevalence = MagicAbilityPrevalence.rare;
        /// <summary>
        /// Процент реально крутых магов среди всех носителей магических способностей
        /// </summary>
        public MagicAbilityDistribution m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

        public float m_fFish = 0;
        public float m_fGrain = 0;
        public float m_fGame = 0;
        public float m_fOre = 0;
        public float m_fWood = 0;
        public int m_iPopulation = 0;

        public List<LocationX> m_cSettlements = new List<LocationX>();

        public Province(bool bForbidden)
        {
            m_bForbidden = bForbidden;
        }

        public Province()
        {}

        public override void Start(LandX pSeed)
        {
            if (pSeed.m_pProvince != null)
                throw new Exception("That land already belongs to province!!!");

            m_cBorderWith.Clear();
            m_cContents.Clear();

            base.Start(pSeed);

            m_pCenter = pSeed;
            m_pNation = pSeed.m_pNation;

            m_sName = m_pNation.m_pRace.m_pLanguage.RandomCountryName();

            m_cContents.Add(pSeed);
            pSeed.m_pProvince = this;
            pSeed.m_iProvincePresence = 1;

            m_cGrowCosts.Clear();

            m_bFullyGrown = false;
        }

        private int GetGrowCost(LandX pLand)
        {
            if (pLand.IsWater)
                return -1;

            float fCost = pLand.Type.m_iMovementCost;

            //if (pLand.m_pProvince != this)
            //{
            //    //граница провинции с этой землёй
            //    float fProvinceBorderLength = 0;
            //    foreach (Line pLine in m_cBorder[pLand])
            //        fProvinceBorderLength += pLine.m_fLength;

            //    //граница этой земли с окружающими землями
            //    float fLinkedLandsBorderLength = 0;
            //    foreach (var pLinkTerr in pLand.BorderWith)
            //    {
            //        if ((pLinkTerr.Key as ITerritory).Forbidden)
            //            continue;

            //        Line[] cLines = pLinkTerr.Value.ToArray();
            //        foreach (Line pLine in cLines)
            //            fLinkedLandsBorderLength += pLine.m_fLength;
            //    }

            //    fCost *= fLinkedLandsBorderLength / fProvinceBorderLength;

            //    //если общая граница провинции с землёй меньше общей длины границы земли в 4 раза или больше, то это - очень плохой вариант.
            //    //стоимость расширения в эту землю - выше.
            //    if (fCost > 4)
            //        fCost *= 10;
            //    //если общая граница провинции с землёй меньше общей длины границы земли в 2 раза или меньше, то это - очень хороший вариант.
            //    //стоимость расширения в эту землю - ниже.
            //    if (fCost < 2)
            //        fCost /= 10;
            //}

            foreach (LandTypeInfoX pType in m_pNation.m_pRace.m_aPrefferedLands)
                if (pType == pLand.Type)
                    fCost /= 10;// (float)pLand.Type.m_iMovementCost;//2;

            foreach (LandTypeInfoX pType in m_pNation.m_pRace.m_aHatedLands)
                if (pType == pLand.Type)
                    fCost *= 10;// (float)pLand.Type.m_iMovementCost;//2;

            if (pLand.m_pNation != m_pNation)
            {
                if (m_pNation.m_bDying)
                    fCost *= 5000;
                else
                    fCost *= 2;
            }

            if (m_pNation.m_bHegemon)
                fCost /= 2;

            if (fCost < 1)
                fCost = 1;

            return (int)fCost;
        }

        private Dictionary<LandX, int> m_cGrowCosts = new Dictionary<LandX, int>();

        private LandX GrowPresence(LandX pLand, int iValue)
        {
            pLand.m_iProvincePresence += iValue;

            int iOwnCost = 0;
            if (!m_cGrowCosts.TryGetValue(pLand, out iOwnCost))
            {
                iOwnCost = GetGrowCost(pLand);
                m_cGrowCosts[pLand] = iOwnCost;
            }

            if (pLand.m_iProvincePresence > iOwnCost)
            {
                float fBestValue = float.MaxValue;
                int iBestCost = 0;
                LandX pBestLand = null;

                foreach (var pLinkTerr in pLand.BorderWith)
                {
                    if ((pLinkTerr.Key as ITerritory).Forbidden)
                        continue;

                    LandX pLinkedLand = pLinkTerr.Key as LandX;

                    if (pLinkedLand.IsWater || (pLinkedLand.m_pProvince != this && pLinkedLand.m_pProvince != null))
                        continue;

                    int iCost = 0;
                    if (!m_cGrowCosts.TryGetValue(pLinkedLand, out iCost))
                    {
                        iCost = GetGrowCost(pLinkedLand);
                        m_cGrowCosts[pLinkedLand] = iCost;
                    }

                    if (iCost == -1)
                        continue;

                    float fCostModified = iCost;

                    if (pLinkedLand.m_pProvince == null)
                    {
                        //общая граница провинции и новой земли
                        float fSharedPerimeter = 1;
                        Line[] aBorderLine = m_cBorder[pLinkedLand].ToArray();
                        foreach (Line pLine in aBorderLine)
                            fSharedPerimeter += pLine.m_fLength;

                        //fCommonLength /= fTotalLength;

                        //if (fCommonLength < 0.25f)
                        //    fCommonLength /= 10;
                        //if (fCommonLength > 0.5f)
                        //    fCommonLength *= 10;

                        fCostModified = iCost * pLinkedLand.PerimeterLength / fSharedPerimeter;

                        if (fSharedPerimeter < pLinkedLand.PerimeterLength / 4)
                            fCostModified *= 10;
                        if (fSharedPerimeter > pLinkedLand.PerimeterLength / 2)
                            fCostModified /= 10;

                        fCostModified = Math.Max(1, fCostModified);
                    }

                    if (pLinkedLand.m_iProvincePresence + fCostModified < fBestValue ||
                        (pLinkedLand.m_iProvincePresence + fCostModified == fBestValue &&
                         Rnd.OneChanceFrom(pLand.BorderWith.Count)))
                    {
                        fBestValue = pLinkedLand.m_iProvincePresence + fCostModified;
                        iBestCost = iCost;
                        pBestLand = pLinkedLand;
                    }
                }

                if (pBestLand != null && pBestLand.m_iProvincePresence + iBestCost < pLand.m_iProvincePresence - iBestCost)
                {
                    pLand.m_iProvincePresence -= iBestCost;
                    if (pBestLand.m_pProvince == null)
                    {
                        pBestLand.m_iProvincePresence += iBestCost;
                        return pBestLand;
                    }
                    else
                    {
                        return GrowPresence(pBestLand, iBestCost);
                    }
                }
                else
                    return null;
            }
            else
                return null;
        }

        public bool ForcedGrow()
        {
            object[] aBorder = new List<object>(m_cBorder.Keys).ToArray();

            m_bFullyGrown = true;

            if (m_pNation.m_bDying)
                return !m_bFullyGrown;

            foreach (ITerritory pTerr in aBorder)
            {
                if (pTerr.Forbidden)
                    continue;

                LandX pLand = pTerr as LandX;

                if (pLand.m_pProvince == null && !pLand.IsWater)
                {
                    m_cContents.Add(pLand);
                    pLand.m_pProvince = this;

                    m_cBorder[pLand].Clear();
                    m_cBorder.Remove(pLand);

                    foreach (var pAddonLinkedLand in pLand.BorderWith)
                    {
                        if (!(pAddonLinkedLand.Key as ITerritory).Forbidden && m_cContents.Contains(pAddonLinkedLand.Key as LandX))
                            continue;

                        if (!m_cBorder.ContainsKey(pAddonLinkedLand.Key))
                            m_cBorder[pAddonLinkedLand.Key] = new List<Line>();
                        Line[] cLines = pAddonLinkedLand.Value.ToArray();
                        foreach (Line pLine in cLines)
                            m_cBorder[pAddonLinkedLand.Key].Add(new Line(pLine));
                    }

                    m_bFullyGrown = false;
                }
            }

            return !m_bFullyGrown;
        }

        public bool m_bFullyGrown = false;

        /// <summary>
        /// Присоединяет к провинции сопредельную нечейную землю.
        /// Чем длиннее общая граница с землёй - тем выше вероятность того, что выбрана будет именно она.
        /// </summary>
        /// <returns></returns>
        public bool Grow(int iMaxProvinceSize)
        {
            m_bFullyGrown = false;
            //if (m_pCenter.m_iProvinceForce > 20*Math.Sqrt(iMaxProvinceSize/Math.PI))
            if (m_cContents.Count >= iMaxProvinceSize || m_pCenter.m_iProvincePresence > 200 * Math.Sqrt(iMaxProvinceSize / Math.PI))
            {
                //GrowForce(m_pCenter, 1);
                m_bFullyGrown = true;
                return false;
            }

            LandX pAddon = GrowPresence(m_pCenter, 1);
            if (pAddon != null)
            {
                m_cContents.Add(pAddon);
                pAddon.m_pProvince = this;

                m_cBorder[pAddon].Clear();
                m_cBorder.Remove(pAddon);

                foreach (var pAddonLinkedLand in pAddon.BorderWith)
                {
                    if (!(pAddonLinkedLand.Key as ITerritory).Forbidden && m_cContents.Contains(pAddonLinkedLand.Key as LandX))
                        continue;

                    if (!m_cBorder.ContainsKey(pAddonLinkedLand.Key))
                        m_cBorder[pAddonLinkedLand.Key] = new List<Line>();
                    Line[] cLines = pAddonLinkedLand.Value.ToArray();
                    foreach (Line pLine in cLines)
                        m_cBorder[pAddonLinkedLand.Key].Add(new Line(pLine));
                }
            }

            return true;
        }

        public Dictionary<Nation, int> m_cNationsCount = new Dictionary<Nation, int>();

        //public bool CheckNations()
        //{ 
        //}

        /// <summary>
        /// Заполняет словарь границ с другими провинциями.
        /// </summary>
        public void Finish(float fCycleShift)
        {
            ChainBorder(fCycleShift);

            foreach (ITerritory pLand in m_cBorder.Keys)
            {
                Province pProvince;
                if (pLand.Forbidden || (pLand as LandX).m_pProvince == null)
                    pProvince = Province.m_pForbidden;
                else
                    pProvince = (pLand as LandX).m_pProvince;

                if (!m_cBorderWith.ContainsKey(pProvince))
                    m_cBorderWith[pProvince] = new List<Line>();
                m_cBorderWith[pProvince].AddRange(m_cBorder[pLand]);
            }
            FillBorderWithKeys();

            int iMaxPop = 0;
            Nation pMaxNation = null;
            m_cNationsCount.Clear();

            foreach (LandX pLand in m_cContents)
            {
                bool bRestricted = true;
                foreach (LocationX pLoc in pLand.m_cContents)
                    if (!pLoc.Forbidden && !pLoc.m_bBorder)
                        bRestricted = false;

                if (bRestricted)
                    continue;

                int iCount = 0;
                m_cNationsCount.TryGetValue((pLand.Area as AreaX).m_pNation, out iCount);
                m_cNationsCount[(pLand.Area as AreaX).m_pNation] = iCount + pLand.m_cContents.Count;
                if (m_cNationsCount[(pLand.Area as AreaX).m_pNation] > iMaxPop)
                {
                    iMaxPop = m_cNationsCount[(pLand.Area as AreaX).m_pNation];
                    pMaxNation = (pLand.Area as AreaX).m_pNation;
                }
            }

            if (pMaxNation != null && !m_pNation.m_bDying)
                m_pNation = pMaxNation;

            foreach (LandX pLand in m_cContents)
                pLand.m_pNation = m_pNation;
        }

        public void BuildLairs(int iScale)
        {
            foreach (LandX pLand in m_cContents)
                pLand.BuildLair();
        }

        public void SpecializeSettlements()
        { 
            foreach(LocationX pLoc in m_cSettlements)
            {
                if (pLoc.m_pSettlement.m_eSpeciality != SettlementSpeciality.None)
                    continue;

                bool bCoast = false;
                foreach (Location pLink in pLoc.m_aBorderWith)
                {
                    if (pLink.Owner != null && (pLink.Owner as LandX).IsWater)
                        bCoast = true;
                }
                
                LandX pLand = pLoc.Owner as LandX;

                switch (pLoc.m_pSettlement.m_pInfo.m_eSize)
                {
                    case SettlementSize.Hamlet:
                        if (bCoast)
                        {
                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Fishers;
                        }
                        else
                        {
                            List<float> cResources = new List<float>();
                            cResources.Add(pLand.Type.m_fGrain * pLand.m_cContents.Count);
                            cResources.Add(pLand.Type.m_fGame * pLand.m_cContents.Count);
                            cResources.Add(pLand.Type.m_fOre * pLand.m_cContents.Count);
                            cResources.Add(pLand.Type.m_fWood * pLand.m_cContents.Count);

                            int iChoosen = Rnd.ChooseOne(cResources, 2);

                            switch (iChoosen)
                            {
                                case 0:
                                    pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(2) ? SettlementSpeciality.Farmers : SettlementSpeciality.Peasants;
                                    break;
                                case 1:
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Hunters;
                                    break;
                                case 2:
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Miners;
                                    break;
                                case 3:
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Lumberjacks;
                                    break;
                            }
                        }
                        break;
                    case SettlementSize.Village:
                        if (bCoast)
                        {
                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Fishers;
                        }
                        else
                        {
                            List<float> cResources = new List<float>();
                            cResources.Add(pLand.Type.m_fGrain * pLand.m_cContents.Count);
                            cResources.Add(pLand.Type.m_fGame * pLand.m_cContents.Count);
                            cResources.Add(pLand.Type.m_fOre * pLand.m_cContents.Count);
                            cResources.Add(pLand.Type.m_fWood * pLand.m_cContents.Count);

                            int iChoosen = Rnd.ChooseOne(cResources, 2);

                            switch (iChoosen)
                            {
                                case 0:
                                    pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(2) ? SettlementSpeciality.Farmers : SettlementSpeciality.Peasants;
                                    break;
                                case 1:
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Hunters;
                                    break;
                                case 2:
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Miners;
                                    break;
                                case 3:
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Lumberjacks;
                                    break;
                            }
                        }
                        break;
                    case SettlementSize.Town:
                        if (bCoast && !Rnd.OneChanceFrom(3))
                        {
                            if (m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel] > 1 + Rnd.Get(1f))
                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Naval;
                            else
                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Fishers;
                        }
                        else
                        {
                            if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iInfrastructureLevel] < 1 + Rnd.Get(1f))
                            {
                                List<float> cResources = new List<float>();
                                cResources.Add(m_fGrain + m_fGame);
                                cResources.Add(m_fOre);
                                cResources.Add(m_fWood);

                                int iChoosen = Rnd.ChooseOne(cResources, 2);

                                switch (iChoosen)
                                {
                                    case 0:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                        break;
                                    case 1:
                                        pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                        break;
                                    case 2:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                        break;
                                }
                            }
                            else
                            {
                                List<float> cResources = new List<float>();
                                cResources.Add((pLand.Type.m_fGrain + pLand.Type.m_fGame) * pLand.m_cContents.Count);
                                cResources.Add(pLand.Type.m_fOre * pLand.m_cContents.Count);
                                cResources.Add(pLand.Type.m_fWood * pLand.m_cContents.Count);

                                int iChoosen = Rnd.ChooseOne(cResources, 2);

                                switch (iChoosen)
                                {
                                    case 0:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                        break;
                                    case 1:
                                        pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                        break;
                                    case 2:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                        break;
                                }
                            }
                        }
                        break;
                    case SettlementSize.City:
                        if (bCoast && m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel] > 1 + Rnd.Get(1f))
                            pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.NavalAcademy : SettlementSpeciality.Naval;
                        else
                        {
                            if (bCoast && m_pCulture.MentalityValues[Mentality.Rudeness][m_iInfrastructureLevel] > 1 + Rnd.Get(1f))
                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Resort;
                            else
                            {
                                if (Rnd.OneChanceFrom(2))
                                {
                                    List<float> cResources = new List<float>();
                                    cResources.Add(2 - m_pCulture.MentalityValues[Mentality.Rudeness][m_iInfrastructureLevel]);
                                    cResources.Add(m_pCulture.MentalityValues[Mentality.Piety][m_iInfrastructureLevel]);
                                    cResources.Add(m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel]);
                                    cResources.Add(m_pCulture.MentalityValues[Mentality.Treachery][m_iInfrastructureLevel]);
                                    cResources.Add(2 - m_pCulture.MentalityValues[Mentality.Selfishness][m_iInfrastructureLevel]);

                                    int iChoosen = Rnd.ChooseOne(cResources, 2);

                                    switch (iChoosen)
                                    {
                                        case 0:
                                            pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Cultural : SettlementSpeciality.ArtsAcademy;
                                            break;
                                        case 1:
                                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Religious;
                                            break;
                                        case 2:
                                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.MilitaryAcademy;
                                            break;
                                        case 3:
                                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Gambling;
                                            break;
                                        case 4:
                                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.SciencesAcademy;
                                            break;
                                    }
                                }
                                else
                                {
                                    if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iInfrastructureLevel] < 1 + Rnd.Get(1f))
                                    {
                                        List<float> cResources = new List<float>();
                                        cResources.Add(m_fGrain + m_fGame);
                                        cResources.Add(m_fOre);
                                        cResources.Add(m_fWood);

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                                break;
                                            case 1:
                                                pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                break;
                                            case 2:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        List<float> cResources = new List<float>();
                                        cResources.Add((pLand.Type.m_fGrain + pLand.Type.m_fGame) * pLand.m_cContents.Count);
                                        cResources.Add(pLand.Type.m_fOre * pLand.m_cContents.Count);
                                        cResources.Add(pLand.Type.m_fWood * pLand.m_cContents.Count);

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                                break;
                                            case 1:
                                                pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                break;
                                            case 2:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case SettlementSize.Capital:
                        if (bCoast && m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel] > 1 + Rnd.Get(1f))
                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.NavalAcademy;
                        else
                        {
                            if (bCoast && m_pCulture.MentalityValues[Mentality.Rudeness][m_iInfrastructureLevel] > 1 + Rnd.Get(1f))
                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Resort;
                            else
                            {
                                if (Rnd.OneChanceFrom(2))
                                {
                                    List<float> cResources = new List<float>();
                                    cResources.Add(2 - m_pCulture.MentalityValues[Mentality.Rudeness][m_iInfrastructureLevel]);
                                    cResources.Add(m_pCulture.MentalityValues[Mentality.Piety][m_iInfrastructureLevel]);
                                    cResources.Add(m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel]);
                                    cResources.Add(m_pCulture.MentalityValues[Mentality.Treachery][m_iInfrastructureLevel]);
                                    cResources.Add(2 - m_pCulture.MentalityValues[Mentality.Selfishness][m_iInfrastructureLevel]);

                                    int iChoosen = Rnd.ChooseOne(cResources, 2);

                                    switch (iChoosen)
                                    {
                                        case 0:
                                            pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Cultural : SettlementSpeciality.ArtsAcademy;
                                            break;
                                        case 1:
                                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Religious;
                                            break;
                                        case 2:
                                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.MilitaryAcademy;
                                            break;
                                        case 3:
                                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Gambling;
                                            break;
                                        case 4:
                                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.SciencesAcademy;
                                            break;
                                    }
                                }
                                else
                                {
                                    if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iInfrastructureLevel] < 1 + Rnd.Get(1f))
                                    {
                                        List<float> cResources = new List<float>();
                                        cResources.Add(m_fGrain + m_fGame);
                                        cResources.Add(m_fOre);
                                        cResources.Add(m_fWood);

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                                break;
                                            case 1:
                                                pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                break;
                                            case 2:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        List<float> cResources = new List<float>();
                                        cResources.Add((pLand.Type.m_fGrain + pLand.Type.m_fGame) * pLand.m_cContents.Count);
                                        cResources.Add(pLand.Type.m_fOre * pLand.m_cContents.Count);
                                        cResources.Add(pLand.Type.m_fWood * pLand.m_cContents.Count);

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                                break;
                                            case 1:
                                                pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                break;
                                            case 2:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case SettlementSize.Fort:
                        if (bCoast)
                            if (m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel] > 1.75 &&
                                m_pCulture.MentalityValues[Mentality.Treachery][m_iInfrastructureLevel] > 1.75)
                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Pirates;
                            else
                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Naval;
                        else
                            if (m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel] > 1.75 &&
                                m_pCulture.MentalityValues[Mentality.Treachery][m_iInfrastructureLevel] > 1.75)
                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Raiders;
                            else
                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Military;
                        break;
                }
            }
        }

        public void BuildSettlements(SettlementSize eSize, bool bFast)
        {
            if (!State.InfrastructureLevels[m_iInfrastructureLevel].m_cAvailableSettlements.Contains(eSize))
                return;

            int iMinCount = m_cContents.Count / 3;
            switch (eSize)
            {
                case SettlementSize.City:
                    iMinCount = m_cContents.Count / 9;
                    break;
                case SettlementSize.Town:
                    iMinCount = m_cContents.Count / 6;
                    break;
            }

            Dictionary<LandX, float> cLandsChances = new Dictionary<LandX, float>();
            foreach (LandX pLand in m_cContents)
                cLandsChances[pLand] = (float)pLand.m_cContents.Count * pLand.Type.m_cSettlementsDensity[eSize];

            for (int i = 0; i < iMinCount; i++)
            {
                int iChance = Rnd.ChooseOne(cLandsChances.Values, 1);

                foreach (LandX pLand in cLandsChances.Keys)
                {
                    iChance--;
                    if (iChance < 0)
                    {
                        LocationX pSettlement = pLand.BuildSettlement(Settlement.Info[eSize], false, bFast);
                        if (pSettlement != null)
                        {
                            m_cSettlements.Add(pSettlement);
                            //bHaveOne = true;
                        }
                        cLandsChances[pLand] = 0;
                        break;
                    }
                }
            }

            foreach (LandX pLand in m_cContents)
            {
                bool bHaveOne = false;
                foreach (LocationX pLoc in pLand.m_cContents)
                    if (pLoc.m_pSettlement != null)
                    {
                        bHaveOne = true;
                        break;
                    }
                if (bHaveOne)
                    continue;

                int iSettlementsCount = (int)(pLand.m_cContents.Count * pLand.Type.m_cSettlementsDensity[eSize]);
                if (iSettlementsCount == 0)
                {
                    int iSettlementChance = (int)(1 / (pLand.m_cContents.Count * pLand.Type.m_cSettlementsDensity[eSize]));
                    if (Rnd.OneChanceFrom(iSettlementChance))
                        iSettlementsCount = 1;
                }
                else
                    iSettlementsCount = 1;

                for (int i = 0; i < iSettlementsCount; i++)
                {
                    //if (bHaveOne && !Rnd.OneChanceFrom(3))
                    //    continue;

                    LocationX pSettlement = pLand.BuildSettlement(Settlement.Info[eSize], false, bFast);
                    if (pSettlement != null)
                    {
                        m_cSettlements.Add(pSettlement);
                        //bHaveOne = true;
                    }
                }
            }
        }

        /// <summary>
        /// Присоединяет в общую транспортную сеть ещё не присоединённые города государства.
        /// </summary>
        /// <param name="eRoadLevel">Уровень новых дорог: 1 - просёлок, 2 - обычная дорога, 3 - имперская дорога</param>
        public void BuildRoads(RoadQuality eRoadLevel, float fCycleShift)
        {
            if (eRoadLevel > State.InfrastructureLevels[m_iInfrastructureLevel].m_eMaxGroundRoad)
                eRoadLevel = State.InfrastructureLevels[m_iInfrastructureLevel].m_eMaxGroundRoad;

            if (eRoadLevel == RoadQuality.None)
                return;

            foreach (LandX pLand in m_cContents)
                foreach (LocationX pLoc in pLand.m_cContents)
                    foreach (TransportationNode pLinked in pLoc.m_cLinks.Keys)
                    {
                        if (pLinked is LocationX)
                        {
                            LandX pLinkedOwner = (pLinked as LocationX).Owner as LandX;
                            if (pLinkedOwner.m_pProvince != this)
                                pLoc.m_cLinks[pLinked].m_bClosed = true;
                        }
                        else
                            pLoc.m_cLinks[pLinked].m_bClosed = true;
                    }

            List<LocationX> cConnected = new List<LocationX>();
            cConnected.Add(m_pAdministrativeCenter);

            LocationX[] aSettlements = m_cSettlements.ToArray();

            foreach (LocationX pTown in aSettlements)
            {
                if (!cConnected.Contains(pTown) && (pTown.m_cRoads[RoadQuality.Normal].Count > 0 || pTown.m_cRoads[RoadQuality.Good].Count > 1))
                    cConnected.Add(pTown);
            }

            while (cConnected.Count < aSettlements.Length)
            {
                //Road pBestRoad = null;
                LocationX pBestTown1 = null;
                LocationX pBestTown2 = null;
                float fMinLength = float.MaxValue;

                foreach (LocationX pTown in aSettlements)
                {
                    if (cConnected.Contains(pTown))
                        continue;

                    foreach (LocationX pOtherTown in cConnected)
                    {
                        float fDist = pTown.DistanceTo(pOtherTown, fCycleShift);// (float)Math.Sqrt((pTown.X - pOtherTown.X) * (pTown.X - pOtherTown.X) + (pTown.Y - pOtherTown.Y) * (pTown.Y - pOtherTown.Y));

                        if (fDist < fMinLength &&
                            (fMinLength == float.MaxValue ||
                             Rnd.OneChanceFrom(2)))
                        {
                            fMinLength = fDist;
                            //pBestRoad = pRoad;

                            pBestTown1 = pTown;
                            pBestTown2 = pOtherTown;
                        }
                    }
                }
                if (pBestTown2 != null)
                {
                    World.BuildRoad(pBestTown1, pBestTown2, eRoadLevel, fCycleShift);

                    fMinLength = float.MaxValue;
                    LocationX pBestTown3 = null;
                    foreach (LocationX pOtherTown in cConnected)
                    {
                        float fDist = pBestTown1.DistanceTo(pOtherTown, fCycleShift);// (float)Math.Sqrt((pBestTown1.X - pOtherTown.X) * (pBestTown1.X - pOtherTown.X) + (pBestTown1.Y - pOtherTown.Y) * (pBestTown1.Y - pOtherTown.Y));

                        if (pOtherTown != pBestTown2 &&
                            fDist < fMinLength &&
                            (fMinLength == float.MaxValue ||
                             Rnd.OneChanceFrom(2)))
                        {
                            fMinLength = fDist;
                            //pBestRoad = pRoad;

                            pBestTown3 = pOtherTown;
                        }
                    }

                    if (pBestTown3 != null)
                        World.BuildRoad(pBestTown1, pBestTown3, eRoadLevel, fCycleShift);

                    cConnected.Add(pBestTown1);
                }
            }

            foreach (LandX pLand in m_cContents)
                foreach (LocationX pLoc in pLand.m_cContents)
                    foreach (TransportationNode pLink in pLoc.m_cLinks.Keys)
                        pLoc.m_cLinks[pLink].m_bClosed = false;
        }

        public LocationX BuildCapital(bool bFast)
        {
            m_iTechLevel = m_pNation.m_iTechLevel;
            //m_iMagicLimit = m_pRace.m_iMagicLimit;

            //m_eMagicAbilityPrevalence = m_pRace.m_eMagicAbilityPrevalence;
            //m_eMagicAbilityDistribution = m_pRace.m_eMagicAbilityDistribution;
            m_pCulture = new Culture(m_pNation.m_pCulture);
            m_pCustoms = new Customs(m_pNation.m_pCustoms);

            //if (Rnd.OneChanceFrom(3))
            //    m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.Get(typeof(MagicAbilityPrevalence));
            //if (Rnd.OneChanceFrom(3))
            //    m_eMagicAbilityDistribution = (MagicAbilityDistribution)Rnd.Get(typeof(MagicAbilityDistribution));

            int iAverageMagicLimit = 0;

            m_fFish = 0;
            m_fGrain = 0;
            m_fGame = 0;
            m_fWood = 0;
            m_fOre = 0;
            m_iPopulation = 0;

            foreach (LandX pLand in m_cContents)
            {
                int iCoast = 0;
                int iBorder = 0;
                foreach (LocationX pLoc in pLand.m_cContents)
                {
                    foreach (LocationX pLink in pLoc.m_aBorderWith)
                    {
                        if (pLink.Owner != pLoc.Owner)
                            iBorder++;
                        if (pLink.Owner != null && (pLink.Owner as LandX).IsWater)
                            iCoast++;
                    }
                }

                m_fFish += iCoast*3/pLand.MovementCost;
                m_fGrain += pLand.m_cContents.Count * pLand.Type.m_fGrain;
                m_fGame += pLand.m_cContents.Count * pLand.Type.m_fGame;
                m_fWood += pLand.m_cContents.Count * pLand.Type.m_fWood;
                m_fOre += pLand.m_cContents.Count * pLand.Type.m_fOre;

                m_iPopulation += pLand.m_cContents.Count;
                iAverageMagicLimit += m_pNation.m_iMagicLimit * pLand.m_cContents.Count;
            }

            iAverageMagicLimit = iAverageMagicLimit / m_iPopulation;

            if (m_fWood*2 < m_iPopulation && m_fOre*2 < m_iPopulation)
                m_iTechLevel -= 2;
            else if (m_fWood + m_fOre < m_iPopulation)// || m_iOre < m_iPopulation)
                m_iTechLevel--;
            else if ((m_fWood > m_iPopulation*2 && m_fOre > m_iPopulation*2) || Rnd.OneChanceFrom(8))
                m_iTechLevel++;

            if (m_pNation.m_bInvader)
            {
                if (m_iTechLevel < m_pNation.m_pEpoch.m_iInvadersMinTechLevel)
                    m_iTechLevel = m_pNation.m_pEpoch.m_iInvadersMinTechLevel;
                if (m_iTechLevel > m_pNation.m_pEpoch.m_iInvadersMaxTechLevel)
                    m_iTechLevel = m_pNation.m_pEpoch.m_iInvadersMaxTechLevel;
            }
            else
            {
                if (m_iTechLevel < m_pNation.m_pEpoch.m_iNativesMinTechLevel)
                    m_iTechLevel = m_pNation.m_pEpoch.m_iNativesMinTechLevel;
                if (m_iTechLevel > m_pNation.m_pEpoch.m_iNativesMaxTechLevel)
                    m_iTechLevel = m_pNation.m_pEpoch.m_iNativesMaxTechLevel;
            }

            //m_iInfrastructureLevel = 4 - (int)(m_pCulture.GetDifference(Culture.IdealSociety, m_iTechLevel, m_iTechLevel) * 4);
            m_iInfrastructureLevel = m_iTechLevel;// -(int)(m_iTechLevel * Math.Pow(Rnd.Get(1f), 3));
            
            if (m_cContents.Count == 1)
                m_iInfrastructureLevel /= 2;

            if (m_iTechLevel == 0 && iAverageMagicLimit == 0)
                m_iInfrastructureLevel = 0;

            if ((m_fGrain + m_fFish + m_fGame)*2 < m_iPopulation)
                m_iInfrastructureLevel--;// = Rnd.Get(m_iCultureLevel);            
            if (m_fGrain + m_fFish + m_fGame < m_iPopulation || Rnd.OneChanceFrom(10))
                m_iInfrastructureLevel--;// = Rnd.Get(m_iCultureLevel);
            if (m_fGrain + m_fFish + m_fGame > m_iPopulation * 2 && Rnd.OneChanceFrom(10))
                m_iInfrastructureLevel++;

            if (m_iInfrastructureLevel < 0)//Math.Max(m_iTechLevel + 1, iAverageMagicLimit) / 2)
                m_iInfrastructureLevel = 0;//Math.Max(m_iTechLevel + 1, iAverageMagicLimit) / 2;
            if (m_iInfrastructureLevel > m_iTechLevel + 1)//Math.Max(m_iTechLevel + 1, iAverageMagicLimit - 1))
                m_iInfrastructureLevel = m_iTechLevel + 1;// Math.Max(m_iTechLevel + 1, iAverageMagicLimit - 1);
            if (m_iInfrastructureLevel > 8)
                m_iInfrastructureLevel = 8;

            //int iOldLevel = m_pNation.m_iTechLevel;// Math.Max(m_pRace.m_iTechLevel, m_pRace.m_iMagicLimit / 2);
            //int iNewLevel = m_iInfrastructureLevel;// Math.Max(m_iTechLevel, iAverageMagicLimit / 2);
            //if (iNewLevel > iOldLevel)
            //    for (int i = 0; i < iNewLevel*iNewLevel - iOldLevel*iOldLevel; i++)
            //    {
            //        m_pCulture.Evolve();
            //        m_pCustoms.Evolve();
            //    }
            //else
            //    for (int i = 0; i < iOldLevel - iNewLevel; i++)
            //    {
            //        m_pCulture.Degrade();
            //        m_pCustoms.Degrade();
            //    }

            if (m_iTechLevel > m_iInfrastructureLevel * 2)
                m_iTechLevel = m_iInfrastructureLevel * 2;
            if (m_pNation.m_bInvader)
            {
                if (m_iTechLevel > m_pNation.m_pEpoch.m_iInvadersMaxTechLevel)
                    m_iTechLevel = m_pNation.m_pEpoch.m_iInvadersMaxTechLevel;
            }
            else
            {
                if (m_iTechLevel > m_pNation.m_pEpoch.m_iNativesMaxTechLevel)
                    m_iTechLevel = m_pNation.m_pEpoch.m_iNativesMaxTechLevel;
            }

            SettlementInfo pSettlementInfo = Settlement.Info[SettlementSize.Hamlet];

            if (State.InfrastructureLevels[m_iInfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.Capital))
                pSettlementInfo = Settlement.Info[SettlementSize.Capital];
            else
                if (State.InfrastructureLevels[m_iInfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.City))
                    pSettlementInfo = Settlement.Info[SettlementSize.City];
                else
                    if (State.InfrastructureLevels[m_iInfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.Town))
                        pSettlementInfo = Settlement.Info[SettlementSize.Town];
                    else
                        if (State.InfrastructureLevels[m_iInfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.Village))
                            pSettlementInfo = Settlement.Info[SettlementSize.Village];

            BuildAdministrativeCenter(pSettlementInfo, bFast);
            if (m_pAdministrativeCenter != null)
                m_cSettlements.Add(m_pAdministrativeCenter);
            else
                throw new Exception("Can't build capital!");

            if (m_pCenter.Area != null)
                m_sName = m_pNation.m_pRace.m_pLanguage.RandomCountryName();

            return m_pAdministrativeCenter;
        }

        private LocationX BuildAdministrativeCenter(SettlementInfo pCenter, bool bFast)
        {
            Dictionary<LandX, float> cLandsChances = new Dictionary<LandX, float>();

            foreach (LandX pLand in m_cContents)
            {
                bool bRestricted = true;
                foreach (LocationX pLoc in pLand.m_cContents)
                    if (!pLoc.Forbidden && !pLoc.m_bBorder)
                        bRestricted = false;

                if (bRestricted)
                    continue;

                cLandsChances[pLand] = (float)pLand.m_cContents.Count * pLand.Type.m_cSettlementsDensity[pCenter.m_eSize];

                bool bProvinceBorder = false;
                bool bStateBorder = false;
                foreach (ITerritory pTerr in pLand.m_aBorderWith)
                { 
                    if(pTerr.Forbidden)
                        continue;

                    LandX pLink = pTerr as LandX;

                    if (pLink.IsWater)
                        continue;

                    if (pLink.m_pProvince != this)
                        bProvinceBorder = true;

                    if (pLink.m_pProvince.Owner != this.Owner)
                        bStateBorder = true;
                }

                if (bProvinceBorder)
                    cLandsChances[pLand] /= 100.0f;

                if (bStateBorder)
                    cLandsChances[pLand] /= 100.0f;
            }

            if (cLandsChances.Count == 0)
                return null;

            int iChance = Rnd.ChooseOne(cLandsChances.Values, 1);

            foreach (LandX pLand in cLandsChances.Keys)
            {
                iChance--;
                if (iChance < 0)
                {
                    m_pAdministrativeCenter = pLand.BuildSettlement(pCenter, true, bFast);
                    break;
                }
            }

            //Грязный хак: низкокультурные сообщества не могут быть многонациональными
            //State pState = Owner as State;
            //if (pState.m_iInfrastructureLevel < 2)
            //    m_pRace = pState.m_pRace;

            return m_pAdministrativeCenter;
        }

        public override string ToString()
        {
            //if (m_pAdministrativeCenter != null)
                return string.Format("province {0} ({2}, {1})", m_sName, m_pAdministrativeCenter == null ? "-" : m_pAdministrativeCenter.ToString(), m_pNation);
            //else
            //    return "province " + m_sName + " [" + m_pRace.ToString() + "]";
        }

        public override float GetMovementCost()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Негативное отношение к другому государству.
        /// </summary>
        /// <param name="pOpponent"></param>
        /// <returns></returns>
        public int CalcHostility(Province pOpponent)
        {
            int iHostility = 0;

            if (m_pNation != pOpponent.m_pNation)
            {
                iHostility++;

                if (m_pNation.m_pRace.m_pLanguage != pOpponent.m_pNation.m_pRace.m_pLanguage)
                    iHostility++;
            }
            else
                iHostility--;

            iHostility += m_pCustoms.GetDifference(pOpponent.m_pCustoms);

            //if (m_iFood < m_iPopulation && pOpponent.m_iFood > pOpponent.m_iPopulation * 2)
            //    iHostility++;
            //if (m_iWood < m_iPopulation && pOpponent.m_iWood > pOpponent.m_iPopulation * 2)
            //    iHostility++;
            //if (m_iOre < m_iPopulation && pOpponent.m_iOre > pOpponent.m_iPopulation * 2)
            //    iHostility++;

            if (pOpponent.m_iInfrastructureLevel > m_iInfrastructureLevel)
                iHostility++;//= pOpponent.m_iLifeLevel - m_iLifeLevel;
            else
                if (pOpponent.m_iInfrastructureLevel < m_iInfrastructureLevel)
                    iHostility++;//= m_iLifeLevel - pOpponent.m_iLifeLevel;

            float iCultureDifference = m_pCulture.GetDifference(pOpponent.m_pCulture, m_iInfrastructureLevel, pOpponent.m_iInfrastructureLevel);
            if (iCultureDifference < -0.75)
                iHostility -= 2;
            else
                if (iCultureDifference < -0.5)
                    iHostility--;
                else
                    if (iCultureDifference > 0.5)
                        iHostility += 2;
                    else
                        if (iCultureDifference > 0)
                            iHostility++;

            if (iHostility > 0)
            {
                iHostility = (int)(m_pCulture.MentalityValues[Mentality.Fanaticism][m_iInfrastructureLevel] * iHostility + 0.25);
                iHostility = (int)(m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel] * iHostility + 0.25);
                if (iHostility == 0)
                    iHostility = 1;
            }
            else
            {
                if (iHostility < 0)
                {
                    iHostility = (int)((2.0f - m_pCulture.MentalityValues[Mentality.Fanaticism][m_iInfrastructureLevel]) * iHostility - 0.25);
                    iHostility = (int)((2.0f - m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel]) * iHostility - 0.25);
                    if (iHostility == 0)
                        iHostility = -1;
                }
            }

            return iHostility;
        }
    }
}
