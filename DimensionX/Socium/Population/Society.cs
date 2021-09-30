using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Socium.Psichology;
using LandscapeGeneration;
using Random;
using Socium.Settlements;
using Socium.Nations;

namespace Socium.Population
{
    /// <summary>
    /// Сообщество - группа людей, разделяющих близкие культурные ценности и обычаи, имеющих доступ к определённому уровню технических благ,
    /// объединённых единой властной иерархией и подчинённых единому закону. Может владеть отдельными зданиями в поселениях, но не требует территориальной целостности.
    /// </summary>
    public abstract class Society
    {
        /// <summary>
        /// 0 - Каменный век. Шкуры, дубинки, каменные топоры, копья с каменными или костяными наконечниками. GURPS TL0
        /// 1 - Бронзовый век. В ходу бронзовые кирасы и кожаные доспехи, бронзовое холодное оружие, из стрелкового оружия – луки и дротики. GURPS TL1
        /// 2 - Железный век. Стальное холодное оружие, кольчуги, рыцарские доспехи. Из стрелкового оружия - луки и арбалеты. GURPS TL2-3
        /// 3 - Эпоха пороха. Примитивное однозарядное огнестрельное оружие, лёгкие сабли и шпаги, облегчённая броня (кирасы, кожаные куртки). GURPS TL4-5
        /// 4 - Индустриальная эра. Нарезное огнестрельное оружие, примитивная бронетехника и авиация, химическое оружие массового поражения. GURPS TL6
        /// 5 - Атомная эра. Автоматическое огнестрельное оружие, бронежилеты, развитая бронетехника и авиация, ядерные ракеты и бомбы. GURPS TL7-8
        /// 6 - Энергетическая эра. Силовые поля, лучевое оружие. GURPS TL9-10
        /// 7 - Квантовая эра. Ограниченная телепортация, материализация, дезинтеграция. GURPS TL11-12
        /// 8 - Переход. Полная и неограниченная власть человека над пространственно-временным континуумом. GURPS TL12+
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
        /// 0 - Анархия. Есть номинальная власть, но она не занимается охраной правопорядка.
        /// 1 - Власти занимаются только самыми вопиющими преступлениями.
        /// 2 - Есть законы, их надо соблюдать, кто не соблюдает - тот преступник, а вор должен сидеть в тюрьме.
        /// 3 - Законы крайне строги, широко используется смертная казнь.
        /// 4 - Тоталитарная диктатура. Все граждане, кроме правящей верхушки, попадают под презумпцию виновности.
        /// </summary>
        public int m_iControl = 0;
        /// <summary>
        /// Уровень социального (не)равенства.
        /// 0 - рабство
        /// 1 - крепостное право
        /// 2 - капитализм
        /// 3 - социализм
        /// 4 - коммунизм
        /// </summary>
        public int m_iSocialEquality = 0;

        public Creed m_pCreed = null;

        /// <summary>
        /// Процент реально крутых магов среди всех носителей магических способностей
        /// </summary>
        public MagicAbilityDistribution m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

        public string m_sName = "Nameless";

        protected List<LocationX> m_cLands = new List<LocationX>();

        internal List<LocationX> Lands
        {
            get { return m_cLands; }
        }

        protected List<LocationX> m_cSettlements = new List<LocationX>();

        internal List<LocationX> Settlements
        {
            get { return m_cSettlements; }
        }

        /// <summary>
        /// Returns most common weapon, used on that TL
        /// </summary>
        /// <param name="iLevel"></param>
        /// <param name="eProgress"></param>
        /// <returns></returns>
        public static string GetTechString(int iLevel, Customs.Progressiveness eProgress)
        {
            string sTech = "stone weapons";
            switch (iLevel)
            {
                case 0:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "obsidian weapons" : "stone weapons";
                    break;
                case 1:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "bronze weapons" : eProgress == Customs.Progressiveness.Traditionalism ? "stone weapons, rare iron weapons" : "iron weapons";
                    break;
                case 2:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "repeating crossbows" : eProgress == Customs.Progressiveness.Traditionalism ? "iron weapons, rare steel weapons" : "steel weapons";
                    break;
                case 3:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "multibarrel guns" : eProgress == Customs.Progressiveness.Traditionalism ? "steel weapons, rare muskets" : "muskets";
                    break;
                case 4:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "lightning guns" : eProgress == Customs.Progressiveness.Traditionalism ? "muskets, rare rifles" : "rifles";//railroads
                    break;
                case 5:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "smartguns" : eProgress == Customs.Progressiveness.Traditionalism ? "rifles, rare submachine guns" : "submachine guns";//aviation
                    break;
                case 6:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "mecha suits" : eProgress == Customs.Progressiveness.Traditionalism ? "submachine guns, rare beam guns" : "beam guns";
                    break;
                case 7:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "nanites" : eProgress == Customs.Progressiveness.Traditionalism ? "beam guns, rare desintegrators" : "desintegrators";//limited teleportation
                    break;
                case 8:
                    sTech = eProgress == Customs.Progressiveness.Traditionalism ? "desintegrators, rare reality destructors" : "reality destructors";//unlimited teleportation
                    break;
            }

            return sTech;
        }

        /// <summary>
        /// Returns short description of available supernatural abilities
        /// </summary>
        /// <param name="iLevel"></param>
        /// <returns></returns>
        public static string GetMagicString(int iLevel)
        {
            string sMagic = "none";
            switch (iLevel)
            {
                case 1:
                    sMagic = "mystics";
                    break;
                case 2:
                    sMagic = "seers";
                    break;
                case 3:
                    sMagic = "jedi";//animal empower?
                    break;
                case 4:
                    sMagic = "wizards";//portals
                    break;
                case 5:
                    sMagic = "jinnes";
                    break;
                case 6:
                    sMagic = "titans";//limited teleportation
                    break;
                case 7:
                    sMagic = "gods";//unlimited teleportation
                    break;
                case 8:
                    sMagic = "the One";
                    break;
            }

            return sMagic;
        }

        /// <summary>
        /// Anarchic - Liberal - Lawful - Autocratic - Despotic
        /// </summary>
        /// <param name="iControl"></param>
        /// <returns></returns>
        public static string GetControlString(int iControl)
        {
            string sControl = "Anarchic";
            switch (iControl)
            {
                case 1:
                    sControl = "Liberal";
                    break;
                case 2:
                    sControl = "Lawful";
                    break;
                case 3:
                    sControl = "Autocratic";
                    break;
                case 4:
                    sControl = "Despotic";
                    break;
            }
            return sControl;
        }

        /// <summary>
        /// Slavery - Serfdom - Hired labour - Social equality - Utopia
        /// </summary>
        /// <param name="iEquality"></param>
        /// <returns></returns>
        public static string GetEqualityString(int iEquality)
        {
            string sEquality = "Slavery";
            switch (iEquality)
            {
                case 1:
                    sEquality = "Serfdom";
                    break;
                case 2:
                    sEquality = "Hired labour";
                    break;
                case 3:
                    sEquality = "Social equality";
                    break;
                case 4:
                    sEquality = "Utopia";
                    break;
            }
            return sEquality;
        }

        /// <summary>
        /// When it comes to numbers, technofetishist weapon as effective as TL+1 and tradionalistic weapon counts as TL-1
        /// </summary>
        /// <returns></returns>
        public int GetEffectiveTech()
        {
            int iMaxTech = m_iTechLevel;
            if (m_pCreed.m_pCustoms.m_eProgress == Customs.Progressiveness.Technofetishism)
                iMaxTech++;

            if (m_pCreed.m_pCustoms.m_eProgress == Customs.Progressiveness.Traditionalism)
                iMaxTech--;

            if (iMaxTech > 8)
                iMaxTech = 8;
            if (iMaxTech < 0)
                iMaxTech = 0;

            return iMaxTech;
        }

        /// <summary>
        /// Low TL states could have access to higher TL weapons from other countries.
        /// Returns higher TL if possible or -1 is there is no available higher TL.
        /// Currently not using, so always -1.
        /// </summary>
        /// <returns></returns>
        public abstract int GetImportedTech();

        /// <summary>
        /// Return most common imported weapon (see GetImportedTech() for details)
        /// Currently not using, so always empty string.
        /// </summary>
        /// <returns></returns>
        public abstract string GetImportedTechString();

        public override string ToString()
        {
            return string.Format("(C{1}T{2}M{3}) - {0}", m_sName, m_pCreed.m_iCultureLevel, m_iTechLevel, m_iMagicLimit);
        }

        protected Person.Skill m_eMostRespectedSkill;

        /// <summary>
        /// Наиболее престижный навык. Задаётся в BuildCapital!
        /// </summary>
        public Person.Skill MostRespectedSkill
        {
            get { return m_eMostRespectedSkill; }
        }

        protected Person.Skill m_eLeastRespectedSkill;

        /// <summary>
        /// Наименее престижный навык. Задаётся в BuildCapital!
        /// </summary>
        public Person.Skill LeastRespectedSkill
        {
            get { return m_eLeastRespectedSkill; }
        }

        /// <summary>
        /// Возвращает уровень престижности указанной профессии в соответствии с отношением в обществе к требуемым в ней навыкам
        /// </summary>
        /// <param name="pProfession"></param>
        /// <returns></returns>
        public int GetProfessionSkillPreference(ProfessionInfo pProfession)
        {
            int iPreference = 0;

            switch (pProfession.m_cSkills[m_eMostRespectedSkill])
            {
                case ProfessionInfo.SkillLevel.Bad:
                    iPreference++;
                    break;
                case ProfessionInfo.SkillLevel.Good:
                    iPreference += 2;
                    break;
                case ProfessionInfo.SkillLevel.Excellent:
                    iPreference += 3;
                    break;
            }
            switch (pProfession.m_cSkills[m_eLeastRespectedSkill])
            {
                case ProfessionInfo.SkillLevel.Bad:
                    iPreference--;
                    break;
                case ProfessionInfo.SkillLevel.Good:
                    iPreference -= 2;
                    break;
                case ProfessionInfo.SkillLevel.Excellent:
                    iPreference -= 3;
                    break;
            }

            return iPreference;
        }

        public Dictionary<Estate.Position, Estate> m_cEstates = new Dictionary<Estate.Position, Estate>();

        public Dictionary<ProfessionInfo, int> m_cPeople = new Dictionary<ProfessionInfo, int>();

        internal virtual Customs.GenderPriority FixGenderPriority(Customs.GenderPriority ePriority)
        {
            return ePriority;
        }

        /// <summary>
        /// Распределяет население государства по сословиям. 
        /// Вызывается в CWorld.PopulateMap после того, как созданы и застроены все поселения в государстве - т.е. после того, как сформировано собственно население.
        /// </summary>
        public void SetEstates()
        {
            // Базовые настройки культуры и обычаев для сообщества
            Estate pBase = new Estate(this, Estate.Position.Middle);
            // Элита может немного отличаться от среднего класса
            m_cEstates[Estate.Position.Elite] = new Estate(pBase, Estate.Position.Elite);
            // средний класс - это и есть основа всего сообщества
            m_cEstates[Estate.Position.Middle] = pBase;
            // низший класс - его обычаи должны отличаться не только от среднего класса, но и от элиты
            do
            {
                m_cEstates[Estate.Position.Low] = new Estate(pBase, Estate.Position.Low);
            }
            while (m_cEstates[Estate.Position.Elite].m_pMajorsCreed.Equals(m_cEstates[Estate.Position.Low].m_pMajorsCreed));
            // аутсайдеры - строим либо на базе среднего класса, либо на базе низшего - и следим, чтобы тоже отличалось от всех 3 других сословий
            do
            {
                if (!Rnd.OneChanceFrom(3) && m_iSocialEquality != 0)
                {
                    m_cEstates[Estate.Position.Outlaw] = new Estate(m_cEstates[Estate.Position.Low], Estate.Position.Outlaw);
                }
                else
                    m_cEstates[Estate.Position.Outlaw] = new Estate(pBase, Estate.Position.Outlaw);
            }
            while (m_cEstates[Estate.Position.Elite].m_pMajorsCreed.Equals(m_cEstates[Estate.Position.Outlaw].m_pMajorsCreed) ||
                m_cEstates[Estate.Position.Low].m_pMajorsCreed.Equals(m_cEstates[Estate.Position.Outlaw].m_pMajorsCreed) ||
                pBase.m_pMajorsCreed.Equals(m_cEstates[Estate.Position.Outlaw].m_pMajorsCreed));

            // перебираем все поселения, где присутсвует сообщество
            foreach (LocationX pLocation in m_cLands)
            {
                if (pLocation.m_pSettlement == null)
                    continue;

                if (pLocation.m_pSettlement.m_cBuildings.Count > 0)
                {
                    // перебираем все строения в поселениях
                    foreach (Building pBuilding in pLocation.m_pSettlement.m_cBuildings)
                    {
                        int iCount = 0;
                        int iOwnersCount = pBuilding.m_pInfo.OwnersCount;
                        int iWorkersCount = pBuilding.m_pInfo.WorkersCount;

                        var pOwner = pBuilding.m_pInfo.m_pOwnerProfession;
                        m_cPeople.TryGetValue(pOwner, out iCount);
                        m_cPeople[pOwner] = iCount + iOwnersCount;

                        var pWorkers = pBuilding.m_pInfo.m_pWorkersProfession;
                        m_cPeople.TryGetValue(pWorkers, out iCount);
                        m_cPeople[pWorkers] = iCount + iWorkersCount;
                    }
                }
            }

            // таблица предпочтений в профессиях - в соответствии с необходимыми для профессии скиллами и полом
            SortedDictionary<int, List<ProfessionInfo>> cProfessionPreference = new SortedDictionary<int, List<ProfessionInfo>>();

            int iTotalPopulation = 0;

            // заполняем таблицу предпочтительных профессий - с точки зрения общих культурных норм сообщества
            foreach (var pProfession in m_cPeople)
            {
                iTotalPopulation += pProfession.Value;

                int iPreference = GetProfessionSkillPreference(pProfession.Key);

                if (pProfession.Key.m_bMaster)
                    iPreference += 4;

                int iDiscrimination = (int)(m_pCreed.GetMentalityValue(Mentality.Fanaticism) + 0.5);

                var eProfessionGenderPriority = GetProfessionGenderPriority(pProfession.Key, m_pCreed.m_pCustoms);

                if (m_pCreed.m_pCustoms.m_eGenderPriority == Customs.GenderPriority.Patriarchy &&
                    eProfessionGenderPriority == Customs.GenderPriority.Matriarchy)
                    iPreference -= iDiscrimination;
                if (m_pCreed.m_pCustoms.m_eGenderPriority == Customs.GenderPriority.Matriarchy &&
                    eProfessionGenderPriority == Customs.GenderPriority.Patriarchy)
                    iPreference -= iDiscrimination;

                if (!cProfessionPreference.ContainsKey(iPreference))
                    cProfessionPreference[iPreference] = new List<ProfessionInfo>();

                cProfessionPreference[iPreference].Add(pProfession.Key);
            }

            //Численность сословий - в зависимости от m_iSocialEquality.
            //При рабовладельческом строе (0) элита должна составлять порядка 1% населения, низшее сословие - 90-95%
            //При коммунизме - численность сословий равна. Или может наоборот, должна быть элита 90+%

            int iLowEstateCount = iTotalPopulation * (92 - m_iSocialEquality * 23) / 100;
            int iEliteEstateCount = iTotalPopulation * (10 + m_iSocialEquality * 5) / 100;

            foreach (var pProfession in m_cPeople)
            {
                if (pProfession.Key.m_eCasteRestriction.HasValue)
                {
                    var pEstate = m_cEstates[Estate.Position.Elite];
                    switch (pProfession.Key.m_eCasteRestriction)
                    {
                        case ProfessionInfo.Caste.Elite:
                            pEstate = m_cEstates[Estate.Position.Elite];
                            iEliteEstateCount -= pProfession.Value;
                            break;
                        case ProfessionInfo.Caste.Low:
                            pEstate = m_cEstates[Estate.Position.Low];
                            iLowEstateCount -= pProfession.Value;
                            break;
                        case ProfessionInfo.Caste.Outlaw:
                            pEstate = m_cEstates[Estate.Position.Outlaw];
                            break;
                    }
                    pEstate.m_cGenderProfessionPreferences[pProfession.Key] = GetProfessionGenderPriority(pProfession.Key, pEstate.m_pMajorsCreed.m_pCustoms);
                    RemoveStrataPreference(ref cProfessionPreference, pProfession.Key);
                }
            }

            if (m_iSocialEquality != 0 || m_cEstates[Estate.Position.Low].m_cGenderProfessionPreferences.Count == 0)
            {
                while (iLowEstateCount > 0)
                {
                    if (cProfessionPreference.Count == 0)
                        break;

                    ProfessionInfo pBestFit = null;
                    int iLowestPreference = cProfessionPreference.Keys.First();

                    int iBestFit = int.MinValue;
                    foreach (ProfessionInfo pProfession in cProfessionPreference[iLowestPreference])
                    {
                        if (pProfession.m_eCasteRestriction != ProfessionInfo.Caste.MiddleOrUp &&
                            m_cPeople[pProfession] < iLowEstateCount && m_cPeople[pProfession] > iBestFit)
                        {
                            pBestFit = pProfession;
                            iBestFit = m_cPeople[pProfession];
                        }
                    }
                    if (pBestFit == null)
                    {
                        iBestFit = int.MaxValue;
                        foreach (ProfessionInfo pProfession in cProfessionPreference[iLowestPreference])
                        {
                            if (pProfession.m_eCasteRestriction != ProfessionInfo.Caste.MiddleOrUp &&
                                m_cPeople[pProfession] < iBestFit)
                            {
                                pBestFit = pProfession;
                                iBestFit = m_cPeople[pProfession];
                            }
                        }
                    }
                    if (pBestFit != null)
                    {
                        m_cEstates[Estate.Position.Low].m_cGenderProfessionPreferences[pBestFit] = GetProfessionGenderPriority(pBestFit, m_cEstates[Estate.Position.Low].m_pMajorsCreed.m_pCustoms);
                        iLowEstateCount -= iBestFit;
                        RemoveStrataPreference(ref cProfessionPreference, pBestFit);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            while (iEliteEstateCount > 0)
            {
                if (cProfessionPreference.Count == 0)
                    break;

                ProfessionInfo pBestFit = null;
                int iHighestPreference = cProfessionPreference.Keys.Last();

                int iBestFit = iEliteEstateCount;
                foreach (ProfessionInfo pStrata in cProfessionPreference[iHighestPreference])
                {
                    if (m_cPeople[pStrata] < iBestFit)
                    {
                        pBestFit = pStrata;
                        iBestFit = m_cPeople[pStrata];
                    }
                }
                if (pBestFit != null)
                {
                    m_cEstates[Estate.Position.Elite].m_cGenderProfessionPreferences[pBestFit] = GetProfessionGenderPriority(pBestFit, m_cEstates[Estate.Position.Elite].m_pMajorsCreed.m_pCustoms);
                    iEliteEstateCount -= iBestFit;
                    RemoveStrataPreference(ref cProfessionPreference, pBestFit);
                }
                else
                {
                    break;
                }
            }

            foreach (var pPreference in cProfessionPreference)
            {
                foreach (ProfessionInfo pProfession in pPreference.Value)
                {
                    m_cEstates[Estate.Position.Middle].m_cGenderProfessionPreferences[pProfession] = GetProfessionGenderPriority(pProfession, m_cEstates[Estate.Position.Elite].m_pMajorsCreed.m_pCustoms);
                }
            }

            foreach (var pEstate in m_cEstates)
                pEstate.Value.FixGenderProfessionPreferences();
        }

        /// <summary>
        /// Привести гендерные преференции страты в соответствии с обычаями сословия
        /// </summary>
        /// <param name="pEstate"></param>
        public Customs.GenderPriority GetProfessionGenderPriority(ProfessionInfo pProfession, Customs pCustoms)
        {
            // По умолчанию - гендерные предпочтения страты совпадают представлениями сословия о "сильном" поле.
            var eGenderPriority = pCustoms.m_eGenderPriority;

            // но, если это подчинённая должность...
            if (!pProfession.m_bMaster)
            {
                // ...связанная с тем, чтобы нравиться клиенту...
                if (pProfession.m_cSkills[Person.Skill.Charm] != ProfessionInfo.SkillLevel.None)
                {
                    // ...то в гетеросексуальном обществе она считается более подходящей "слабому" полу
                    if (pCustoms.m_eSexRelations == Customs.SexualOrientation.Heterosexual)
                    {
                        if (eGenderPriority == Customs.GenderPriority.Patriarchy)
                            eGenderPriority = Customs.GenderPriority.Matriarchy;
                        else if (eGenderPriority == Customs.GenderPriority.Matriarchy)
                            eGenderPriority = Customs.GenderPriority.Patriarchy;
                    }
                    // ...а в бисексуальном обществе - такая профессия так же бисексуальна
                    else if (pCustoms.m_eSexRelations == Customs.SexualOrientation.Bisexual)
                        eGenderPriority = Customs.GenderPriority.Genders_equality;
                }
                else
                {
                    // ...если профессия НЕ связанна с тем, чтобы нравиться клиенту -
                    // смотрим, насколько она считается престижной в данном сословии.
                    // более престижные профессии - считаются подходящими "сильному" полу
                    int iPreference = GetProfessionSkillPreference(pProfession);

                    if (iPreference == 0)
                        eGenderPriority = Customs.GenderPriority.Genders_equality;
                    // менее престижные профессии - считаются подходящими "слабому" полу
                    if (iPreference < 0)
                        eGenderPriority = FixGenderPriority(eGenderPriority);
                }
            }

            return eGenderPriority;
        }

        /// <summary>
        /// Удаляет профессию из списка преференций
        /// </summary>
        /// <param name="cProfessionPreference"></param>
        /// <param name="pProfession"></param>
        private void RemoveStrataPreference(ref SortedDictionary<int, List<ProfessionInfo>> cProfessionPreference, ProfessionInfo pProfession)
        {
            List<int> cCemetary = new List<int>();
            foreach (var pPreference in cProfessionPreference)
            {
                if (pPreference.Value.Contains(pProfession))
                {
                    pPreference.Value.Remove(pProfession);
                    if (pPreference.Value.Count == 0)
                        cCemetary.Add(pPreference.Key);
                }
            }

            foreach (int iPreference in cCemetary)
                cProfessionPreference.Remove(iPreference);
        }

        public abstract string GetEstateName(Estate.Position ePosition);

        protected abstract BuildingInfo ChooseNewBuilding(Settlement pSettlement);

        public abstract void AddBuildings(Settlement pSettlement);
    }
}
