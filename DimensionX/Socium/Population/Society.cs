using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Socium.Psychology;
using LandscapeGeneration;
using Random;
using Socium.Settlements;
using Socium.Nations;
using GeneLab.Genetix;

namespace Socium.Population
{
    /// <summary>
    /// Сообщество - группа людей, обладающих сходным менталитетом и обычаями, имеющих доступ к определённому уровню технических благ,
    /// объединённых единой властной иерархией и подчинённых единому закону. 
    /// Может владеть отдельными зданиями в поселениях, но не требует территориальной целостности.
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
        public int TechLevel { get; set; } = 0;
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
        public int MagicLimit { get; set; } = 0;

        /// <summary>
        /// В обществе могут быть определены различные культурные нормы и обычаи для мужчини женщин.
        /// Важно: значения обычаев, ругулирующих отношения между полами (m_eGenderPriority, m_eSexRelations и m_eMarriage)
        /// должно обязательно совпадать во всех вариантах культуры независимо от пола.
        /// </summary>
        public Dictionary<Gender, Culture> Culture { get; } = new Dictionary<Gender, Culture>();

        /// <summary>
        /// Культура "сильного" пола
        /// </summary>
        public Culture DominantCulture
        {
            get
            {
                if (Culture[Gender.Male].Customs.Has(Customs.GenderPriority.Matriarchy))
                    return Culture[Gender.Female];

                return Culture[Gender.Male];
            }
        }

        /// <summary>
        /// Культура "слабого" пола
        /// </summary>
        public Culture InferiorCulture
        {
            get
            {
                if (Culture[Gender.Male].Customs.Has(Customs.GenderPriority.Matriarchy))
                    return Culture[Gender.Male];

                return Culture[Gender.Female];
            }
        }

        protected void FixSexCustoms()
        {
            InferiorCulture.Customs.Accept(DominantCulture.Customs.ValueOf<Customs.MarriageType>());
            InferiorCulture.Customs.Accept(DominantCulture.Customs.ValueOf<Customs.GenderPriority>());
            InferiorCulture.Customs.Accept(DominantCulture.Customs.ValueOf<Customs.SexualOrientation>());
        }

        public string Name { get; set; } = "Nameless";

        public List<LocationX> Settlements { get; } = new List<LocationX>();

        /// <summary>
        /// Returns most common weapon, used on that TL
        /// </summary>
        /// <param name="iLevel"></param>
        /// <param name="eProgress"></param>
        /// <returns></returns>
        public static string GetTechString(int iLevel, Customs.Science eProgress)
        {
            switch (iLevel)
            {
                case 0:
                    return eProgress == Customs.Science.Ingenuity ? "obsidian weapons" : "stone weapons";
                case 1:
                    return eProgress == Customs.Science.Ingenuity ? "bronze weapons" : eProgress == Customs.Science.Technophobia ? "stone weapons, rare iron weapons" : "iron weapons";
                case 2:
                    return eProgress == Customs.Science.Ingenuity ? "repeating crossbows" : eProgress == Customs.Science.Technophobia ? "iron weapons, rare steel weapons" : "steel weapons";
                case 3:
                    return eProgress == Customs.Science.Ingenuity ? "multibarrel guns" : eProgress == Customs.Science.Technophobia ? "steel weapons, rare muskets" : "muskets";
                case 4:
                    return eProgress == Customs.Science.Ingenuity ? "lightning guns" : eProgress == Customs.Science.Technophobia ? "muskets, rare rifles" : "rifles";//railroads
                case 5:
                    return eProgress == Customs.Science.Ingenuity ? "smartguns" : eProgress == Customs.Science.Technophobia ? "rifles, rare submachine guns" : "submachine guns";//aviation
                case 6:
                    return eProgress == Customs.Science.Ingenuity ? "mecha suits" : eProgress == Customs.Science.Technophobia ? "submachine guns, rare beam guns" : "beam guns";
                case 7:
                    return eProgress == Customs.Science.Ingenuity ? "nanites" : eProgress == Customs.Science.Technophobia ? "beam guns, rare desintegrators" : "desintegrators";//limited teleportation
                case 8:
                    return eProgress == Customs.Science.Ingenuity ? "desintegrators, rare reality destructors" : "reality destructors";//unlimited teleportation
            }

            return "stone weapons";
        }

        /// <summary>
        /// Returns short description of available supernatural abilities
        /// </summary>
        /// <param name="iLevel"></param>
        /// <returns></returns>
        public static string GetMagicString(int iLevel)
        {
            switch (iLevel)
            {
                case 1:
                    return "mediums";
                case 2:
                    return "psionics";
                case 3:
                    return "supers";//animal empower?
                case 4:
                    return "wizards";//portals
                case 5:
                    return "demons";
                case 6:
                    return "gods";//limited teleportation
                case 7:
                    return "ethereals";//unlimited teleportation
                case 8:
                    return "the One";
            }

            return "none";
        }

        /// <summary>
        /// Anarchic - Liberal - Lawful - Autocratic - Despotic
        /// </summary>
        /// <param name="iControl"></param>
        /// <returns></returns>
        public static string GetControlString(int iControl)
        {
            switch (iControl)
            {
                case 1:
                    return "Liberal";
                case 2:
                    return "Lawful";
                case 3:
                    return "Autocratic";
                case 4:
                    return "Despotic";
            }
            return "Anarchic";
        }

        /// <summary>
        /// When it comes to numbers, technofetishist weapon as effective as TL+1 and tradionalistic weapon counts as TL-1
        /// </summary>
        /// <returns></returns>
        public int GetEffectiveTech()
        {
            int iMaxTech = TechLevel;

            if (DominantCulture.Customs.Has(Customs.Science.Ingenuity))
                iMaxTech++;
            else if (DominantCulture.Customs.Has(Customs.Science.Technophobia))
                iMaxTech--;

            if (iMaxTech > 8)
                iMaxTech = 8;
            if (iMaxTech < 0)
                iMaxTech = 0;

            return iMaxTech;
        }

        public override string ToString()
        {
            return string.Format("(C{1}/{2}T{3}M{4}) - {0}", Name, Culture[Gender.Male].ProgressLevel, Culture[Gender.Female].ProgressLevel, TechLevel, MagicLimit);
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

        public Dictionary<ProfessionInfo, Customs.GenderPriority> GenderProfessionPreferences { get; } = new Dictionary<ProfessionInfo, Customs.GenderPriority>();

        public void CalculateGenderProfessionPreferences()
        {
            foreach (var pProfession in ProfessionInfo.s_cAllProfessions)
            {
                GenderProfessionPreferences[pProfession] = GetProfessionGenderPriority(pProfession);
            }
        }

        /// <summary>
        /// Возвращает уровень престижности указанной профессии в соответствии с отношением в обществе к требуемым в ней навыкам
        /// </summary>
        /// <param name="pProfession"></param>
        /// <returns></returns>
        public int GetProfessionSkillPreference(ProfessionInfo pProfession)
        {
            int iPreference = 0;

            switch (pProfession.Skills[m_eMostRespectedSkill])
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
            switch (pProfession.Skills[m_eLeastRespectedSkill])
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

        public Customs.GenderPriority GetProfessionGenderPriority(ProfessionInfo pProfession)
        {
            // По умолчанию - гендерные предпочтения профессии совпадают представлениями сообщества о "сильном" поле.
            var eGenderPriority = DominantCulture.Customs.ValueOf<Customs.GenderPriority>();

            // но, если это подчинённая должность...
            if (!pProfession.IsMaster)
            {
                // ...связанная с тем, чтобы нравиться клиенту...
                if (pProfession.Skills[Person.Skill.Charm] != ProfessionInfo.SkillLevel.None)
                {
                    // ...то в гетеросексуальном обществе она считается более подходящей противоположному полу
                    if (Culture[Gender.Male].Customs.Has(Customs.SexualOrientation.Heterosexual))
                    {
                        if (eGenderPriority == Customs.GenderPriority.Patriarchy)
                            eGenderPriority = Customs.GenderPriority.Matriarchy;
                        else if (eGenderPriority == Customs.GenderPriority.Matriarchy)
                            eGenderPriority = Customs.GenderPriority.Patriarchy;
                    }
                    // ...а в бисексуальном обществе - такая профессия так же бисексуальна
                    else if (Culture[Gender.Male].Customs.Has(Customs.SexualOrientation.Bisexual))
                    {
                        eGenderPriority = Customs.GenderPriority.Genders_equality;
                    }
                }
                else
                {
                    // ...если же профессия НЕ связанна с тем, чтобы нравиться клиенту -
                    // смотрим, насколько она считается престижной в данном сословии.
                    // более престижные профессии - считаются подходящими "сильному" полу
                    int iPreference = GetProfessionSkillPreference(pProfession);

                    if (iPreference == 0)
                        eGenderPriority = Customs.GenderPriority.Genders_equality;
                    // менее престижные профессии - считаются подходящими "слабому" полу
                    if (iPreference < 0)
                        eGenderPriority = GetMinorGender();
                }
            }

            return eGenderPriority;
        }

        /// <summary>
        /// В зависимости от того, какой пол считается "сильным" - возвращаем противоположный
        /// </summary>
        /// <param name="eGenderPriority">общественная норма</param>
        /// <returns></returns>
        internal virtual Customs.GenderPriority GetMinorGender()
        {
            var eGenderPriority = Culture[Gender.Male].Customs.ValueOf<Customs.GenderPriority>();

            if (eGenderPriority == Customs.GenderPriority.Patriarchy)
                return Customs.GenderPriority.Matriarchy;
            else if (eGenderPriority == Customs.GenderPriority.Matriarchy)
                return Customs.GenderPriority.Patriarchy;

            return Customs.GenderPriority.Genders_equality;
        }

        protected abstract BuildingInfo ChooseNewBuilding(Settlement pSettlement);

        public abstract void AddBuildings(Settlement pSettlement);
    }
}
