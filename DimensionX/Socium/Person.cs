using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneLab;
using GeneLab.Genetix;
using LandscapeGeneration;
using Random;
using Socium.Nations;
using Socium.Population;
using Socium.Psychology;
using Socium.Settlements;

namespace Socium
{
    public class Person
    {
        public enum _Age
        {
            /// <summary>
            /// юнец
            /// </summary>
            Young = 0,
            /// <summary>
            /// в самом расцвете сил
            /// </summary>
            Adult,
            /// <summary>
            /// пожилой, но ещё крепкий
            /// </summary>
            Aged,
            /// <summary>
            /// древний старик
            /// </summary>
            Old
        }

        public enum Appearance
        {
            Unattractive = -1,
            Average = 0,
            Handsome
        }

        public enum Injury
        {
            Scars,
            One_eyed,
            Lame,
            One_handed,
            One_legged
        }

        public static bool IsMinorInjury(Injury eInjury)
        {
            return eInjury == Injury.Scars || eInjury == Injury.One_eyed || eInjury == Injury.Lame;
        }

        public enum MentalDisorder
        {
            Bad_tempered,
            Alchholic,
            Gambler,
            Drug_addict,
            //Absent_minded,
            Paranoiac,
            Psychopath,
            Insane
        }

        public static bool IsMinorDisorder(MentalDisorder eDisorder)
        {
            return eDisorder == MentalDisorder.Bad_tempered ||
                eDisorder == MentalDisorder.Alchholic ||
                eDisorder == MentalDisorder.Gambler ||
                eDisorder == MentalDisorder.Drug_addict;
        }

        /// <summary>
        /// В порядке возрастания значимости!
        /// </summary>
        public enum Relation
        {
            Associate,
            Archenemy,
            Friend,
            /// <summary>
            /// Relative is my agent
            /// </summary>
            Patron,
            /// <summary>
            /// Relative is my patron
            /// </summary>
            Agent,
            Relative,
            Cousin,
            /// <summary>
            /// Relative is my unkle or aunt
            /// </summary>
            NephewNiece,
            /// <summary>
            /// Relative is my nephew or niece
            /// </summary>
            UnkleAunt,
            /// <summary>
            /// Relative is my grand-child
            /// </summary>
            GrandParentOf,
            /// <summary>
            /// Relative is my child
            /// </summary>
            ParentOf,
            Sibling,
            Lover,
            Spouse,
            /// <summary>
            /// Relative is my true parent
            /// </summary>
            BastardOf,
            /// <summary>
            /// Relative is my grand-parent
            /// </summary>
            GrandChildOf,
            /// <summary>
            /// Relative is my parent
            /// </summary>
            ChildOf,
            /// <summary>
            /// Now I'm known as Relative
            /// </summary>
            FormerLifeOf,
            /// <summary>
            /// I was known as Relative in the past
            /// </summary>
            PresentLifeOf,
            AlterEgo
        }

        /// <summary>
        /// Дети, родители, братья, сёстры, деды, бабки, дяди, тёти, племянники и кузины - все, кто имеют общий геном
        /// и секс между ними считается кровосмешением
        /// </summary>
        /// <param name="pRelation"></param>
        /// <returns></returns>
        static public bool IsBloodKinship(Relation pRelation)
        {
            return pRelation == Relation.ChildOf ||
                pRelation == Relation.ParentOf ||
                pRelation == Relation.GrandChildOf ||
                pRelation == Relation.GrandParentOf ||
                pRelation == Relation.UnkleAunt ||
                pRelation == Relation.NephewNiece ||
                pRelation == Relation.Cousin ||
                pRelation == Relation.Sibling;
        }

        /// <summary>
        /// Дети, родители, братья, сёстры и супруги - все, кто имеет общую фамилию
        /// </summary>
        /// <param name="pRelation"></param>
        /// <returns></returns>
        static public bool IsFamily(Relation pRelation)
        {
            return pRelation == Relation.Spouse ||
                pRelation == Relation.ChildOf ||
                pRelation == Relation.ParentOf ||
                pRelation == Relation.Sibling;
        }

        /// <summary>
        /// Дети, родители, братья, сёстры, супруги и дальние родственники - все кто официально считаются роднёй
        /// </summary>
        /// <param name="pRelation"></param>
        /// <returns></returns>
        static public bool IsMutualKinship(Relation pRelation)
        {
            return pRelation == Relation.Relative ||
                IsBloodKinship(pRelation) ||
                IsFamily(pRelation);
        }

        /// <summary>
        /// Дети, родители, братья, сёстры, супруги, дальние родственники и бастарды - все кто хоть как-то связан семейными узами
        /// </summary>
        /// <param name="pRelation"></param>
        /// <returns></returns>
        static public bool IsKinship(Relation pRelation)
        {
            return pRelation == Relation.BastardOf ||
                IsMutualKinship(pRelation);
        }

        /// <summary>
        /// if lover of spouse
        /// </summary>
        /// <param name="pRelation"></param>
        /// <returns></returns>
        static public bool IsSexualRelation(Relation pRelation)
        {
            return pRelation == Relation.Lover || pRelation == Relation.Spouse;
        }

        public string WhoAmITo(Person pOther)
        {
            if(!Relations.ContainsKey(pOther))
                return "";

            switch(Relations[pOther])
            {
                case Relation.Spouse:
                    return Gender == Gender.Male ? "husband" : "wife";
                case Relation.Sibling:
                    return Gender == Gender.Male ? "brother" : "sister";
                case Relation.Cousin:
                    return "cousin";
                case Relation.ChildOf:
                    return Gender == Gender.Male ? "son" : "daughter";
                case Relation.GrandChildOf:
                    return Gender == Gender.Male ? "grand-son" : "grand-daughter";
                case Relation.BastardOf:
                    return Gender == Gender.Male ? "bastard" : "bastard daughter";
                case Relation.ParentOf:
                    if(pOther.Relations.ContainsKey(this) && pOther.Relations[this] == Relation.BastardOf)
                        return Gender == Gender.Male ? "true father" : "true mother";
                    else
                        return Gender == Gender.Male ? "father" : "mother";
                case Relation.GrandParentOf:
                    return Gender == Gender.Male ? "grand-father" : "grand-mother";
                case Relation.Lover:
                    return "lover";
                case Relation.UnkleAunt:
                    return Gender == Gender.Male ? "unkle" : "aunt";
                case Relation.NephewNiece:
                    return Gender == Gender.Male ? "nephew" : "niece";
                case Relation.Relative:
                    return Gender == Gender.Male ? "kinsman" : "kinswoman";
                case Relation.Archenemy:
                    return "sworn enemy";
                case Relation.Associate:
                    return "acquaintance";
                case Relation.Friend:
                    return "friend";
                case Relation.AlterEgo:
                    return "alter ego";
                case Relation.Patron:
                    return "patron";
                case Relation.Agent:
                    return "debtor";
                case Relation.FormerLifeOf:
                    return "former personality";
                case Relation.PresentLifeOf:
                    return "new personality";
                default:
                    throw new ArgumentException("Unknown relation type!");
            }
        }

        public enum Skill
        {
            Body,
            Mind,
            Charm
        }

        /// <summary>
        /// Вычисляет наиболее и наименее престижные навыки на основании культурных ценностей и обычаев
        /// </summary>
        public static void GetSkillPreferences(Culture pCulture, ref Skill eMostRespectedSkill, ref Skill eLeastRespectedSkill)
        {
            Dictionary<Skill, int> cSkillPreferences = new Dictionary<Skill, int>
            {
                [Skill.Body] = 0,
                [Skill.Mind] = 0,
                [Skill.Charm] = 0
            };

            if (pCulture.GetTrait(Trait.Agression) > 1)
                cSkillPreferences[Skill.Body]++;
            if (pCulture.GetTrait(Trait.Agression) > 1.5)
                cSkillPreferences[Skill.Body]++;

            //if (m_pCulture.MentalityValues[Mentality.Rudeness][m_iCultureLevel] < 1)
            //    cSkillPreferences[CPerson.Skill.Mind]++;
            //if (m_pCulture.MentalityValues[Mentality.Rudeness][m_iCultureLevel] < 0.5)
            //    cSkillPreferences[CPerson.Skill.Mind]++;

            //if (m_pCulture.MentalityValues[Mentality.Treachery][m_iCultureLevel] > 1)
            //    cSkillPreferences[CPerson.Skill.Mind]++;
            //if (m_pCulture.MentalityValues[Mentality.Treachery][m_iCultureLevel] > 1.5)
            //    cSkillPreferences[CPerson.Skill.Mind]++;

            if (pCulture.Customs.Has(Customs.MindSet.Emotions))
                cSkillPreferences[Skill.Mind]--;
            if (pCulture.Customs.Has(Customs.MindSet.Logic))
                cSkillPreferences[Skill.Mind]++;

            if (pCulture.Customs.Has(Customs.Magic.Magic_Praised))
                cSkillPreferences[Skill.Mind]++;

            if (pCulture.Customs.Has(Customs.Science.Technophobia))
                cSkillPreferences[Skill.Mind]--;
            if (pCulture.Customs.Has(Customs.Science.Ingenuity))
                cSkillPreferences[Skill.Mind]++;

            if (pCulture.Customs.Has(Customs.Sexuality.Puritan))
                cSkillPreferences[Skill.Charm]--;
            if (pCulture.Customs.Has(Customs.Sexuality.Lecherous))
                cSkillPreferences[Skill.Charm]++;

            if (pCulture.Customs.Has(Customs.MarriageType.Polyamory))
                cSkillPreferences[Skill.Charm]++;

            eMostRespectedSkill = Skill.Body;
            eLeastRespectedSkill = Skill.Charm;

            int iMax = int.MinValue;
            int iMin = int.MaxValue;

            foreach (Person.Skill eSkill in Enum.GetValues(typeof(Person.Skill)))
            {
                if (cSkillPreferences[eSkill] > iMax)
                {
                    eMostRespectedSkill = eSkill;
                    iMax = cSkillPreferences[eSkill];
                }
            }
            foreach (Person.Skill eSkill in Enum.GetValues(typeof(Person.Skill)))
            {
                if (eSkill == eMostRespectedSkill)
                    continue;

                if (cSkillPreferences[eSkill] < iMin)
                {
                    eLeastRespectedSkill = eSkill;
                    iMin = cSkillPreferences[eSkill];
                }
            }
        }

        public _Age Age { get; private set; }

        internal Gender Gender { get; }

        public Gender GetCompatibleSexPartnerGender(bool bOfficial)
        {
            if (HomeLocation.OwnerProvince.LocalSociety.DominantCulture.Customs.Has(Customs.SexualOrientation.Heterosexual))
            {
                //В гетеросексуальном обществе - только персонаж другого пола
                return Gender == Gender.Male ? Gender.Female : Gender.Male;
            }
            else if (HomeLocation.OwnerProvince.LocalSociety.DominantCulture.Customs.Has(Customs.SexualOrientation.Homosexual))
            {
                //В гомосексуальном обществе - только персонаж того же пола
                return Gender;
            }
            else if (HomeLocation.OwnerProvince.LocalSociety.DominantCulture.Customs.Has(Customs.SexualOrientation.Bisexual))
            {
                //Официальные браки разрешаем заключать только между персонажами разного пола - просто дань канону (Блейд, Конан, etc.)
                if (bOfficial)
                    return Gender == Gender.Male ? Gender.Female : Gender.Male;
                
                //В патриархальном бисексуальном обществе мужчины могут иметь партнеров любого пола, а женщины - только мужчин
                if (HomeLocation.OwnerProvince.LocalSociety.DominantCulture.Customs.Has(Customs.GenderPriority.Patriarchy) &&
                    Gender == Gender.Female)
                {
                    return Gender.Male;
                }
                //В матриархальном бисексуальном обществе - наоборот, женщины свободны в выборе, а мужчины не могут иметь отношения друг с другом
                if (HomeLocation.OwnerProvince.LocalSociety.DominantCulture.Customs.Has(Customs.GenderPriority.Matriarchy) &&
                    Gender == Gender.Male)
                {
                    return Gender.Female;
                }
            }

            return Rnd.OneChanceFrom(2) ? Gender.Male : Gender.Female;
        }

        public Dictionary<Person, Relation> Relations { get; } = new Dictionary<Person, Relation>();
        public Person Patron { get; } = null;
        public List<Person> Minions { get; } = new List<Person>();

        public int GetFactionSize()
        {
            int iCount = 1;

            foreach (Person pMinion in Minions)
                iCount += pMinion.GetFactionSize();

            return iCount;
        }

        public int GetFactionInfluence(bool bCombined)
        {
            int iInfluence = GetInfluence(bCombined);

            foreach (Person pMinion in Minions)
                iInfluence += pMinion.GetFactionInfluence(bCombined);

            return iInfluence;
        }

        public Person GetFaction()
        {
            int iDepth;
            return GetFaction(out iDepth);
        }

        public Person GetFaction(out int iDepth)
        {
            iDepth = 0;

            if (Patron == null)
                return null;

            if (Patron == this)
                return this;

            iDepth = 1;

            Person pFaction = Patron;
            while (pFaction.Patron != null && pFaction.Patron != pFaction)
            {
                pFaction = pFaction.Patron;
                iDepth++;
            }

            return pFaction;
        }

        public Nation Nation { get; }
        public LocationX HomeLocation { get; }
        public Building Building { get; } = null;

        public string Name { get; private set; }
        public string Family { get; }

        private readonly bool m_bFinished = false;

        public StateSociety State { get; }
        public Estate Estate { get; }

        public Culture Culture { get; private set; }

        public Dictionary<Skill, ProfessionInfo.SkillLevel> Skills { get; } = new Dictionary<Skill, ProfessionInfo.SkillLevel>();
        public ProfessionInfo Profession { get; }

        private Phenotype m_pFamilyPhenotypeM;
        private Phenotype m_pFamilyPhenotypeF;
        public Phenotype m_pPhenotype { get; private set; }

        public Appearance Look { get; private set; } = Appearance.Average;
        public List<Injury> Injuries { get; } = new List<Injury>();
        public List<MentalDisorder> MentalDisorders { get; } = new List<MentalDisorder>();

        private void InitSocial(Person pRelative)
        {
            var pOriginCulture = pRelative != null && pRelative.Gender == Gender ? pRelative.Culture : Estate.Culture[Gender];

            var pCulture = new Mentality(pOriginCulture.Mentality, true);
            var iCultureLevel = pOriginCulture.ProgressLevel;
            
            if (Rnd.OneChanceFrom(10))
            {
                if (Rnd.OneChanceFrom(2))
                    iCultureLevel++;
                else
                    iCultureLevel--;
            }

            if (iCultureLevel < State.Culture[Gender].ProgressLevel - 1)
                iCultureLevel = State.Culture[Gender].ProgressLevel - 1;

            if (iCultureLevel > State.Culture[Gender].ProgressLevel + 1)
                iCultureLevel = State.Culture[Gender].ProgressLevel + 1;

            if (iCultureLevel < 0)
                iCultureLevel = 0;

            if (iCultureLevel > 8)
                iCultureLevel = 8;

            var pCustoms = new Customs(pOriginCulture.Customs, Customs.Mutation.Possible);

            Culture = new Culture(pCulture, iCultureLevel, pCustoms);

            m_pFamilyPhenotypeM = pRelative == null ? (Phenotype)Nation.PhenotypeMale.MutateFamily() : pRelative.m_pFamilyPhenotypeM;
            m_pFamilyPhenotypeF = pRelative == null ? (Phenotype)Nation.PhenotypeFemale.MutateFamily() : pRelative.m_pFamilyPhenotypeF;
            if (Gender == Gender.Male)
                m_pPhenotype = (Phenotype)m_pFamilyPhenotypeM.MutateIndividual();
            else
                m_pPhenotype = (Phenotype)m_pFamilyPhenotypeF.MutateIndividual();

            Culture.Customs.ApplyFenotype(m_pPhenotype);

            foreach (var pSkill in Profession.Skills)
            {
                ProfessionInfo.SkillLevel eLevel = pSkill.Value;
                if (pSkill.Key == HomeLocation.OwnerState.m_pSociety.MostRespectedSkill && Rnd.OneChanceFrom(2))
                    eLevel++;
                if (pSkill.Key == HomeLocation.OwnerState.m_pSociety.LeastRespectedSkill && Rnd.OneChanceFrom(2))
                    eLevel--;

                if (pSkill.Key == Estate.MostRespectedSkill && Rnd.OneChanceFrom(3))
                    eLevel++;
                if (pSkill.Key == Estate.LeastRespectedSkill && Rnd.OneChanceFrom(3))
                    eLevel--;

                if (eLevel < ProfessionInfo.SkillLevel.None)
                    eLevel = ProfessionInfo.SkillLevel.None;

                if (eLevel > ProfessionInfo.SkillLevel.Excellent)
                    eLevel = ProfessionInfo.SkillLevel.Excellent;

                Skills[pSkill.Key] = eLevel;
            }

            if (Estate.Rank == Estate.SocialRank.Outlaws)
            {
                if (Rnd.OneChanceFrom(2))
                    Look = Appearance.Unattractive;
                else if (Rnd.OneChanceFrom(5))
                    Look = Appearance.Handsome;
            }
            else if (Estate.Rank == Estate.SocialRank.Lowlifes)
            {
                if (Rnd.OneChanceFrom(3))
                    Look = Appearance.Unattractive;
                else if (Rnd.OneChanceFrom(10))
                    Look = Appearance.Handsome;
            }
            else if (Estate.Rank == Estate.SocialRank.Elite)
            {
                if (Rnd.OneChanceFrom(3))
                    Look = Appearance.Handsome;
                else if (Rnd.OneChanceFrom(5))
                    Look = Appearance.Unattractive;
            }
            else
            {
                if (Rnd.OneChanceFrom(5))
                    Look = Appearance.Handsome;
                else if (Rnd.OneChanceFrom(4))
                    Look = Appearance.Unattractive;
            }
        }

        public void ApplyInjuries()
        {
            for (_Age eAge = _Age.Adult; eAge <= Age; eAge++)
            {
                if (Rnd.Chances(1 + (int)Profession.Skills[Skill.Body], 8 + State.InfrastructureLevel * 2))
                {
                    Injury eInjury = (Injury)Rnd.Get(typeof(Injury));
                    if (!Injuries.Contains(eInjury))
                        Injuries.Add(eInjury);
                }

                if (Rnd.Chances(1 + (int)Profession.Skills[Skill.Mind] + (int)Profession.Skills[Skill.Charm], 8 + State.InfrastructureLevel * 2))
                {
                    MentalDisorder eMentalDisorder = (MentalDisorder)Rnd.Get(typeof(MentalDisorder));
                    if (!MentalDisorders.Contains(eMentalDisorder))
                        MentalDisorders.Add(eMentalDisorder);
                }
            }

            if (Estate.Rank == Estate.SocialRank.Outlaws)
            {
                if (Rnd.OneChanceFrom(3))
                {
                    Injury eInjury = (Injury)Rnd.Get(typeof(Injury));
                    if (!Injuries.Contains(eInjury))
                        Injuries.Add(eInjury);
                }

                if (Rnd.OneChanceFrom(3))
                {
                    MentalDisorder eMentalDisorder = (MentalDisorder)Rnd.Get(typeof(MentalDisorder));
                    if (!MentalDisorders.Contains(eMentalDisorder))
                        MentalDisorders.Add(eMentalDisorder);
                }
            }
        }

        /// <summary>
        /// В домах с полным или частичным семейным владением, новых жильцов могут приглашат только члены семьи, кровно связанные с главой семьи.
        /// Нельзя войти в семью, заявившись братом жены младшего сына.
        /// </summary>
        /// <returns></returns>
        public bool CouldInviteNewDwellers()
        {
            if (Building.Persons.Count == 1)
                return true;

            if (Building.Persons[0] == this)
                return true;

            if (Building.Info.Ownership == FamilyOwnership.None)
                return true;

            if (Building.Info.Ownership == FamilyOwnership.Owners && Profession != Building.Info.OwnerProfession)
                return true;

            List<Person> cBloodRelatives = new List<Person>() { this };
            bool bUpdate;
            do
            {
                bUpdate = false;
                foreach (var pRelation in Relations)
                {
                    if (IsBloodKinship(pRelation.Value) &&
                        Building == pRelation.Key.Building &&
                        !cBloodRelatives.Contains(pRelation.Key))
                    {
                        if (pRelation.Key == Building.Persons[0])
                            return true;

                        cBloodRelatives.Add(pRelation.Key);
                        bUpdate = true;
                    }
                }
            }
            while (bUpdate);

            return false;
        }

        /// <summary>
        /// Создаёт случайного персонажа в заданной локации, имеющего заданную профессию (обычно - правитель государства или его наследник).
        /// Если есть такая возможность, делает его членом семьи (родитель, ребёнок, брат/сестра или муж/жена) одного из уже живущих в указанном строении.
        /// </summary>
        /// <param name="pHomeLocation"></param>
        /// <param name="pBuilding"></param>
        /// <param name="bOwner"></param>
        public Person(World pWorld, LocationX pHomeLocation, Building pBuilding, bool bOwner)
        {
            HomeLocation = pHomeLocation;
            State = HomeLocation.OwnerState.m_pSociety;
            Nation = State.TitularNation;
            Building = pBuilding;

            Gender = Rnd.OneChanceFrom(2) ? Gender.Male : Gender.Female;

            Person pRelative = null;

            if (Building.Persons.Count > 0)
            {
                List<Person> cRightfullOwners = new List<Person>();
                //Если дом принадлежит одной семье, но в нём есть прислуга, то мы можем войти в семью только если мы владелец и породниться только с другим владельцем.
                foreach (Person pDweller in Building.Persons)
                {
                    if ((bOwner && pDweller.Profession == Building.Info.OwnerProfession) ||
                        (!bOwner && Building.Info.Ownership != FamilyOwnership.Owners))
                    {
                        if (pDweller.CouldInviteNewDwellers())
                            cRightfullOwners.Add(pDweller);
                    }
                }

                if (cRightfullOwners.Count > 0)
                    pRelative = cRightfullOwners[Rnd.Get(cRightfullOwners.Count)];
            }

            if (pRelative != null)
            {
                Relation eChoice = Relation.Sibling;
                if (pRelative.Profession == Building.Info.OwnerProfession)
                {
                    if (bOwner)
                        eChoice = Relation.Spouse;//Rnd.OneChanceFrom(5) ? Relation.Sibling : Relation.Spouse;
                    else
                        eChoice = Rnd.OneChanceFrom(3) ? Relation.Sibling : Relation.ChildOf;
                }
                else
                {
                    //if (bOwner)
                    //    eChoice = Relation.ParentOf;
                    //else
                    eChoice = Relation.Spouse;// Rnd.OneChanceFrom(2) ? Relation.Sibling : Relation.Spouse;
                }

                //if (eChoice == Relation.ParentOf)
                //{
                //    CPerson pParent = pRelative.GetParent();
                //    if (pParent != null)// && m_pHome.Owner.m_pCustoms.m_eFamilySize == Customs.FamilySize.Monogamy)
                //        eChoice = Relation.Sibling;
                //}

                if (eChoice == Relation.Spouse)
                {
                    Person pSpouse = pRelative.GetSpouse();
                    if (pSpouse == null)
                    {
                        eChoice = Relation.Sibling;
                        if (Building.Info.Ownership == FamilyOwnership.Full)
                            bOwner = false;
                    }
                }

                Relations[pRelative] = eChoice;

                #region Fix gender for selected relation type
                if (IsSexualRelation(eChoice))
                    Gender = pRelative.GetCompatibleSexPartnerGender(true);

                //Если мы собираемся стать чьим-то родителем, а у ребёнка уже есть родитель
                //if (eChoice == Relation.ParentOf && pRelative.m_cRelations.ContainsValue(Relation.ChildOf))
                //{
                //    CPerson pOtherParent = pRelative.GetHighestRelative(Relation.ChildOf);
                //    Gender = pOtherParent.GetCompatibleSexPartnerGender();
                //}
                #endregion
            }
            else
            {
                //Если не нашли родственника в той же локации
                if (State.Polity.HasDinasty)// && Rnd.OneChanceFrom(3))
                    pRelative = pWorld.GetPossibleRelative(Nation, State.InfrastructureLevel);

                if (pRelative != null)
                {
                    if(pRelative.Profession != ProfessionInfo.Nobody)
                        Relations[pRelative] = Rnd.OneChanceFrom(3) ? Relation.Sibling : Relation.Cousin;
                    else
                        Relations[pRelative] = Relation.Sibling;
                }
            }

            if (pRelative != null && IsBloodKinship(Relations[pRelative]))
                Nation = pRelative.Nation;

            if(bOwner)
                Profession = Building.Info.OwnerProfession;
            else
                Profession = Building.Info.WorkersProfession;

            foreach (var pEstate in State.Estates)
            {
                if (pEstate.Value.GenderProfessionPreferences.ContainsKey(Profession))
                {
                    Estate = pEstate.Value;
                    break;
                }
            }

            if (pRelative == null)
            {
                if (Estate.GenderProfessionPreferences[Profession] == Customs.GenderPriority.Matriarchy && Gender == Gender.Male)
                    Gender = Gender.Female;

                if (Estate.GenderProfessionPreferences[Profession] == Customs.GenderPriority.Patriarchy && Gender == Gender.Female)
                    Gender = Gender.Male;
            }

            Name = Gender == Gender.Male ? Nation.Race.Language.RandomMaleName() : Nation.Race.Language.RandomFemaleName();
            Family = Nation.Race.Language.RandomSurname();
            if (pRelative != null)
            {
                if (IsFamily(Relations[pRelative]))
                    Family = pRelative.Family;
            }

            SetShortName();

            InitSocial(pRelative);

            m_bFinished = true;
        }

        /// <summary>
        /// Создаёт случайного персонажа в заданном поселении. Если локация не задана (null) - то в случайном поселении.
        /// С вероятностью 1/4 может оказаться членом семьи (родитель, ребёнок, брат/сестра или муж/жена) одного из уже имеющихся персонажей.
        /// Может выбрасывать эксепшен, если не срастается!
        /// </summary>
        /// <param name="pWorld"></param>
        /// <param name="pPreferredHome"></param>
        public Person(World pWorld, LocationX pPreferredHome)
        {
            Person pRelative = null;
            if (Rnd.OneChanceFrom(4))
                pRelative = pWorld.GetPossibleRelative(pPreferredHome);

            Gender = Rnd.OneChanceFrom(2) ? Gender.Male : Gender.Female;

            Estate.SocialRank eEstate;
            do
            {
                eEstate = (Estate.SocialRank)Rnd.Get(typeof(Estate.SocialRank));
            }
            while (pPreferredHome != null && !pPreferredHome.HaveEstate(eEstate));

            //Если персонаж связан с другим персонажем...
            if (pRelative != null)
            {
                #region Making relations
                Relation eChoice = Relation.Associate;
                //do
                //{
                switch (Rnd.Get(3))
                {
                    case 0:
                        eChoice = Relation.ChildOf;
                        break;
                    case 1:
                        eChoice = Relation.Sibling;
                        break;
                    case 2:
                        eChoice = Relation.Spouse;
                        break;
                }
                //eChoice = (Relation)Rnd.Get(typeof(Relation));

                //if (eChoice == Relation.ParentOf)
                //{
                //    // Could pRelative have a new parent? If no - will select other relation type...
                //    //CPerson pParent = pRelative.GetParent();
                //    //if (pParent != null)// && m_pHome.Owner.m_pCustoms.m_eFamilySize == Customs.FamilySize.Monogamy)
                //        eChoice = Relation.Associate; // Set not a family!
                //}

                if (eChoice == Relation.Spouse)
                {
                    // Could pRelative have a spouse? If no - will select other relation type...
                    Person pSpouse = pRelative.GetSpouse();
                    if (pSpouse == null)
                        eChoice = Relation.Sibling; // Set not a family!
                    else
                        pRelative = pSpouse;
                }
                //}
                //while(!IsFamily(eChoice));

                Relations[pRelative] = eChoice;
                #endregion

                #region Fix gender for selected relation type
                if (IsSexualRelation(eChoice))
                    Gender = pRelative.GetCompatibleSexPartnerGender(true);

                //Если мы собираемся стать чьим-то родителем, а у ребёнка уже есть родитель
                if (eChoice == Relation.ParentOf && pRelative.Relations.ContainsValue(Relation.ChildOf))
                {
                    Person pOtherParent = pRelative.GetHighestRelative(Relation.ChildOf);
                    Gender = pOtherParent.GetCompatibleSexPartnerGender(true);
                }

                CheckGender(pRelative);
                #endregion

                #region Choosing estate
                if (IsFamily(Relations[pRelative]))
                    eEstate = pRelative.Estate.Rank;

                if (Relations[pRelative] == Relation.Relative)
                {
                    if (!Rnd.Chances(pRelative.State.SocialEquality, 8))
                        eEstate = pRelative.Estate.Rank;
                    else if (Rnd.OneChanceFrom(2))
                        eEstate = pRelative.Estate.Rank - 1;
                    else
                        eEstate = pRelative.Estate.Rank + 1;
                }

                if (eEstate < Estate.SocialRank.Outlaws)
                    eEstate = Estate.SocialRank.Outlaws;

                if (eEstate > Estate.SocialRank.Elite)
                    eEstate = Estate.SocialRank.Elite;
                #endregion

                #region Choosing home location
                if (pPreferredHome == null)
                {
                    //2 шанса из 3, что он живёт там же.
                    //или 3 из 3 - если близкий родственник
                    if (IsFamily(Relations[pRelative]) ||
                        !Rnd.OneChanceFrom(3))
                    {
                        pPreferredHome = pRelative.HomeLocation;
                    }
                    else
                    {
                        List<LocationX> cPossibleHomes = new List<LocationX>();
                        switch (eEstate)
                        {
                            case Estate.SocialRank.Outlaws:
                                foreach (Province pProvince in pRelative.HomeLocation.OwnerState.Contents)
                                    cPossibleHomes.AddRange(pProvince.LocalSociety.Settlements);
                                break;
                            case Estate.SocialRank.Lowlifes:
                                cPossibleHomes.Add(pRelative.HomeLocation);
                                cPossibleHomes.Add(pRelative.HomeLocation);
                                cPossibleHomes.Add(pRelative.HomeLocation);
                                cPossibleHomes.Add(pRelative.HomeLocation.OwnerState.m_pMethropoly.AdministrativeCenter);
                                break;
                            case Estate.SocialRank.Commoners:
                                cPossibleHomes.Add(pRelative.HomeLocation);
                                foreach (State pState in pWorld.m_aStates)
                                {
                                    cPossibleHomes.Add(pRelative.HomeLocation);
                                    cPossibleHomes.Add(pState.m_pMethropoly.AdministrativeCenter);
                                }
                                break;
                            case Estate.SocialRank.Clergy:
                                foreach (Province pProvince in pRelative.HomeLocation.OwnerState.Contents)
                                    cPossibleHomes.AddRange(pProvince.LocalSociety.Settlements);
                                foreach (State pState in pWorld.m_aStates)
                                {
                                    cPossibleHomes.Add(pRelative.HomeLocation);
                                    cPossibleHomes.Add(pState.m_pMethropoly.AdministrativeCenter);
                                }
                                break;
                            case Estate.SocialRank.Elite:
                                foreach (Province pProvince in pRelative.HomeLocation.OwnerState.Contents)
                                    cPossibleHomes.AddRange(pProvince.LocalSociety.Settlements);
                                foreach (State pState in pWorld.m_aStates)
                                    foreach (Province pProvince in pState.Contents)
                                        cPossibleHomes.AddRange(pProvince.LocalSociety.Settlements);
                                break;
                        }

                        if (cPossibleHomes.Count > 0)
                            pPreferredHome = cPossibleHomes[Rnd.Get(cPossibleHomes.Count)];
                    }
                }
                #endregion
            }

            HomeLocation = pWorld.GetRandomSettlement(pPreferredHome, eEstate);
            if (HomeLocation == null)
                throw new InvalidOperationException("Can't find home settlement for choosen estate");
            State = HomeLocation.OwnerState.m_pSociety;

            Estate = State.Estates[eEstate];

            Nation = State.TitularNation;

            if (pRelative != null)
                Nation = pRelative.Nation;

            #region Fix gender for selected nation genetix
            if (pRelative == null ||
                (!IsSexualRelation(Relations[pRelative]) &&
                Relations[pRelative] != Relation.ParentOf))
            {
                Phenotype pPhenotypeM = Nation.PhenotypeMale;
                Phenotype pPhenotypeF = Nation.PhenotypeFemale;

                if (pRelative != null)
                {
                    pPhenotypeM = pRelative.m_pFamilyPhenotypeM;
                    pPhenotypeF = pRelative.m_pFamilyPhenotypeF;

                    if (pRelative.Gender == Gender.Male)
                        pPhenotypeM = pRelative.m_pPhenotype;
                }

                if (pPhenotypeM.m_pValues.Get<LifeCycleGenetix>().BirthRate == BirthRate.High &&
                    pPhenotypeF.m_pValues.Get<LifeCycleGenetix>().BirthRate == BirthRate.Low)
                {
                    Gender = Rnd.OneChanceFrom(10) ? Gender.Female : Gender.Male;
                }
                else if (pPhenotypeM.m_pValues.Get<LifeCycleGenetix>().BirthRate == BirthRate.Low &&
                    pPhenotypeF.m_pValues.Get<LifeCycleGenetix>().BirthRate == BirthRate.High)
                {
                    Gender = Rnd.OneChanceFrom(10) ? Gender.Male : Gender.Female;
                }
                else if (pPhenotypeM.m_pValues.Get<LifeCycleGenetix>().BirthRate >
                    pPhenotypeF.m_pValues.Get<LifeCycleGenetix>().BirthRate)
                {
                    Gender = Rnd.OneChanceFrom(4) ? Gender.Female : Gender.Male;
                }
                else if (pPhenotypeM.m_pValues.Get<LifeCycleGenetix>().BirthRate <
                    pPhenotypeF.m_pValues.Get<LifeCycleGenetix>().BirthRate)
                {
                    Gender = Rnd.OneChanceFrom(4) ? Gender.Male : Gender.Female;
                }

                CheckGender(pRelative);
            }
            #endregion

            #region Determine home building
            Building = FindSuitableBuilding(pRelative);

            if (pRelative != null && IsFamily(Relations[pRelative]) && pRelative.CouldInviteNewDwellers())
            {
                if (pRelative.HomeLocation == HomeLocation)
                    Building = pRelative.Building;
                else
                {
                    //если родственник живёт во дворце правителя, то мы тоже можем жить только во дворце правителя
                    if (pRelative.Building.Info == pRelative.State.Polity.StateCapital.MainBuilding)
                    {
                        foreach (Building pBuilding in HomeLocation.Settlement.Buildings)
                        {
                            if (pBuilding.Info == State.Polity.StateCapital.MainBuilding ||
                                pBuilding.Info == State.Polity.ProvinceCapital.MainBuilding)
                            {
                                Building = pBuilding;
                                break;
                            }
                        }
                    }
                }
            }

            if (Building == null)
            {
                if (pRelative != null)
                    Building = pRelative.Building;
                else
                {
                    Building = FindSuitableBuilding(pRelative);
                    throw new InvalidOperationException("Can't find home building for choosen estate");
                }
            }
            #endregion

            #region Determine profession
            ProfessionInfo pHomeOwner = Building.Info.OwnerProfession;
            List<ProfessionInfo> cPossibleProfessions = new List<ProfessionInfo>();
            if (Estate.GenderProfessionPreferences.ContainsKey(pHomeOwner))
                cPossibleProfessions.Add(pHomeOwner);

            ProfessionInfo pHomeWorkers = Building.Info.WorkersProfession;
            if (Estate.GenderProfessionPreferences.ContainsKey(pHomeWorkers))
                cPossibleProfessions.Add(pHomeWorkers);

            if (cPossibleProfessions.Count > 0)
            {
                Profession = cPossibleProfessions[Rnd.Get(cPossibleProfessions.Count)];
            }
            else
                Profession = ProfessionInfo.Nobody;

            if (pRelative != null)
            {
                #region Fix profession choice for relatives in shared home building
                if (pRelative.Building == Building)
                {
                    //Супруги и братья/сёстры всегда имеют одну и ту же профессию
                    if (Relations[pRelative] == Relation.Spouse)// ||
                        //m_cRelations[pRelative] == Relation.Sibling)
                        Profession = pRelative.Profession;

                    //живущие с родителями дети не могут иметь более высокий социальный статус, чем их родители
                    if (Relations[pRelative] == Relation.ChildOf &&
                        Profession == pHomeOwner &&
                        pRelative.Profession == pHomeWorkers)
                    {
                        if (cPossibleProfessions.Contains(pHomeWorkers))
                            Profession = pHomeWorkers;
                    }

                    //живущие с детьми родители не могут иметь более низкий социальный статус, чем их дети
                    if (Relations[pRelative] == Relation.ParentOf &&
                        Profession == pHomeWorkers &&
                        pRelative.Profession == pHomeOwner)
                    {
                        if (cPossibleProfessions.Contains(pHomeOwner))
                            Profession = pHomeOwner;
                    }

                    //в царских дворцах дети всегда занимают подчинённый ранг, а родители - старший
                    //if (m_pBuilding.m_pInfo.m_eOwnership == FamilyOwnership.Full)
                    //{
                    //    if (m_cRelations[pRelative] == Relation.ChildOf && pRelative.m_pProfession == pHomeOwner.m_pProfession)
                    //        m_pProfession = pHomeWorkers.m_pProfession;

                    //    if (m_cRelations[pRelative] == Relation.ParentOf && pRelative.m_pProfession == pHomeWorkers.m_pProfession)
                    //        m_pProfession = pHomeOwner.m_pProfession;
                    //}
                }
                #endregion

                #region Fix gender for choosen profession
                //Важно - условие соответствия гендерным предпочтениям страты не распространяется на семьи правителей!
                if (pRelative.Building != Building ||
                    Building.Info.Ownership != FamilyOwnership.Full)
                {
                    if (Profession != ProfessionInfo.Nobody)
                    {
                        if (IsSexualRelation(Relations[pRelative]) ||
                            (Relations[pRelative] == Relation.ParentOf &&
                             pRelative.Relations.ContainsValue(Relation.ChildOf)))
                        {
                            if (Estate.GenderProfessionPreferences[Profession] == Customs.GenderPriority.Matriarchy && Gender == Gender.Male)
                                Profession = ProfessionInfo.Nobody;

                            if (Estate.GenderProfessionPreferences[Profession] == Customs.GenderPriority.Patriarchy && Gender == Gender.Female)
                                Profession = ProfessionInfo.Nobody;
                        }
                        else
                        {
                            if (Estate.GenderProfessionPreferences[Profession] == Customs.GenderPriority.Matriarchy && Gender == Gender.Male)
                                Gender = Gender.Female;

                            if (Estate.GenderProfessionPreferences[Profession] == Customs.GenderPriority.Patriarchy && Gender == Gender.Female)
                                Gender = Gender.Male;

                            CheckGender(pRelative);
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region Fix gender for choosen profession
                if (Profession != ProfessionInfo.Nobody)
                {
                    if (Estate.GenderProfessionPreferences[Profession] == Customs.GenderPriority.Matriarchy && Gender == Gender.Male)
                        Gender = Gender.Female;

                    if (Estate.GenderProfessionPreferences[Profession] == Customs.GenderPriority.Patriarchy && Gender == Gender.Female)
                        Gender = Gender.Male;
                }
                #endregion
            }

            if (Profession == pHomeOwner)
            {
                List<Person> cOwners = new List<Person>();
                foreach (Person pNeighbour in Building.Persons)
                {
                    if (pNeighbour.Profession == pHomeOwner)
                        cOwners.Add(pNeighbour);
                }

                if (cOwners.Count >= Building.Info.OwnersCount)
                    Profession = ProfessionInfo.Nobody;
            }

            #endregion

            #region Name & Surname
            Name = Gender == Gender.Male ? Nation.Race.Language.RandomMaleName() : Nation.Race.Language.RandomFemaleName();
            Family = Nation.Race.Language.RandomSurname();
            if (pRelative != null)
            {
                if (IsFamily(Relations[pRelative]) && pRelative.CouldInviteNewDwellers())
                    Family = pRelative.Family;
            }
            #endregion

            SetShortName();

            CheckGender(pRelative);

            InitSocial(pRelative);

            m_bFinished = true;
        }

        /// <summary>
        /// Находит строение в выбранной локации, которое может принять нового жильца, принадлежащего к выбранному сословию.
        /// Перед вызовом функции должны быть заданы поля m_pHome и m_eEstate!
        /// </summary>
        /// <param name="pRelative">Персонаж, с которым мы связаны - вляяет на доступность вселения в строения с ограничениями на семейные связи жильцов.</param>
        /// <returns></returns>
        private Building FindSuitableBuilding(Person pRelative)
        {
            List<Building> cPossibleBuildings = new List<Building>();
            foreach (Building pBuilding in HomeLocation.Settlement.Buildings)
            {
                if (pRelative != null &&
                    pBuilding.Info.Ownership == FamilyOwnership.Full &&
                    pBuilding.Persons.Count > 0)
                {
                    if (!pBuilding.Persons.Contains(pRelative) ||
                        !IsFamily(Relations[pRelative]))
                        continue;
                }

                if (Estate.GenderProfessionPreferences.ContainsKey(pBuilding.Info.OwnerProfession))
                {
                    List<Person> cOwners = new List<Person>();
                    foreach (Person pDweller in pBuilding.Persons)
                        if (pDweller.Profession == pBuilding.Info.OwnerProfession)
                            cOwners.Add(pDweller);

                    if (cOwners.Count < pBuilding.Info.OwnersCount)
                    {
                        if (pBuilding.Info.Ownership != FamilyOwnership.Owners || 
                            cOwners.Count == 0 ||
                            (pRelative != null && cOwners.Contains(pRelative)))
                            cPossibleBuildings.Add(pBuilding);
                    }
                }

                if (Estate.GenderProfessionPreferences.ContainsKey(pBuilding.Info.WorkersProfession))
                {
                    List<Person> cWorkers = new List<Person>();
                    foreach (Person pDweller in pBuilding.Persons)
                        if (pDweller.Profession == pBuilding.Info.WorkersProfession)
                            cWorkers.Add(pDweller);

                    if (cWorkers.Count < pBuilding.Info.WorkersCount)
                        cPossibleBuildings.Add(pBuilding);
                }
            }

            if (cPossibleBuildings.Count > 0)
                return cPossibleBuildings[Rnd.Get(cPossibleBuildings.Count)];

            return null;
        }

         /// <summary>
        /// ДЛЯ ОТЛАДКИ!!!
        /// Проверяет, правильно ли выставлен пол персонажа для выбранного типа взаимоотношений.
        /// </summary>
        /// <param name="pRelative"></param>
        /// <returns></returns>
        private bool CheckGender(Person pRelative)
        {
            if (pRelative != null)
            {
                if (Relations[pRelative] == Relation.Lover)
                {
                    if (Gender == pRelative.Gender && pRelative.Culture.Customs.Has(Customs.SexualOrientation.Heterosexual))
                        return false;

                    if (Gender != pRelative.Gender && pRelative.Culture.Customs.Has(Customs.SexualOrientation.Homosexual))
                        return false;
                }
                
                if (Relations[pRelative] == Relation.Spouse)
                {
                    if (Gender == pRelative.Gender && pRelative.State.DominantCulture.Customs.Has(Customs.SexualOrientation.Heterosexual))
                        return false;

                    if (Gender != pRelative.Gender && pRelative.State.DominantCulture.Customs.Has(Customs.SexualOrientation.Homosexual))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Настраивает взаимность в отношениях, а так же добавляет персонажа в список жильцов дома, где он живёт.
        /// </summary>
        public void FixReferences()
        {
            foreach (var pRelation in Relations)
            {
                switch (pRelation.Value)
                {
                    case Relation.Associate:
                        pRelation.Key.Relations[this] = Relation.Associate;
                        break;
                    case Relation.Friend:
                        pRelation.Key.Relations[this] = Relation.Friend;
                        break;
                    case Relation.AlterEgo:
                        pRelation.Key.Relations[this] = Relation.AlterEgo;
                        break;
                    case Relation.Patron:
                        pRelation.Key.Relations[this] = Relation.Agent;
                        break;
                    case Relation.Agent:
                        pRelation.Key.Relations[this] = Relation.Patron;
                        break;
                    case Relation.FormerLifeOf:
                        pRelation.Key.Relations[this] = Relation.PresentLifeOf;
                        break;
                    case Relation.PresentLifeOf:
                        pRelation.Key.Relations[this] = Relation.FormerLifeOf;
                        break;
                    case Relation.Lover:
                        pRelation.Key.Relations[this] = Relation.Lover;
                        break;
                    case Relation.Spouse:
                        pRelation.Key.Relations[this] = Relation.Spouse;
                        break;
                    case Relation.ParentOf:
                        pRelation.Key.Relations[this] = Relation.ChildOf;
                        break;
                    case Relation.GrandParentOf:
                        pRelation.Key.Relations[this] = Relation.GrandChildOf;
                        break;
                    case Relation.ChildOf:
                        pRelation.Key.Relations[this] = Relation.ParentOf;
                        break;
                    case Relation.GrandChildOf:
                        pRelation.Key.Relations[this] = Relation.GrandParentOf;
                        break;
                    case Relation.Sibling:
                        pRelation.Key.Relations[this] = Relation.Sibling;
                        break;
                    case Relation.Cousin:
                        pRelation.Key.Relations[this] = Relation.Cousin;
                        break;
                    case Relation.UnkleAunt:
                        pRelation.Key.Relations[this] = Relation.NephewNiece;
                        break;
                    case Relation.NephewNiece:
                        pRelation.Key.Relations[this] = Relation.UnkleAunt;
                        break;
                    case Relation.BastardOf:
                        pRelation.Key.Relations[this] = Relation.ParentOf;
                        break;
                    case Relation.Relative:
                        pRelation.Key.Relations[this] = Relation.Relative;
                        break;
                    default:
                        throw new ArgumentException("Unknown relation type!");
                }
            }

            Building.Persons.Add(this);
        }

        /// <summary>
        /// Брат моего брата - мой брат, а ребенок моего супруга - мой ребенок и т.п.
        /// </summary>
        /// <returns></returns>
        public bool FixKinship()
        {
            Dictionary<Person, Relation> cNewKins = new Dictionary<Person, Relation>();

            foreach (var pRelative in Relations)
            {
                if (IsMutualKinship(pRelative.Value))
                {
                    foreach (var pFarRelative in pRelative.Key.Relations)
                    {
                        if (pFarRelative.Key == this)
                            continue;

                        if (Relations.ContainsKey(pFarRelative.Key))
                            continue;

                        if (IsMutualKinship(pFarRelative.Value))
                        {
                            switch (pRelative.Value)
                            {
                                case Relation.ChildOf:
                                    #region родственники моего родителя
                                    switch (pFarRelative.Value)
                                    {
                                        //родитель родителя - дед или бабка
                                        case Relation.ChildOf:
                                            cNewKins[pFarRelative.Key] = Relation.GrandChildOf;
                                            break;
                                        //ребёнок родителя - брат/сестра
                                        case Relation.ParentOf:
                                            cNewKins[pFarRelative.Key] = Relation.Sibling;
                                            break;
                                        //брат/сестра родителя - мои дядя или тётя
                                        case Relation.Sibling:
                                            cNewKins[pFarRelative.Key] = Relation.NephewNiece;
                                            break;
                                        //супруг родителя - мой родитель
                                        case Relation.Spouse:
                                            cNewKins[pFarRelative.Key] = Relation.ChildOf;
                                            break;
                                        //дед/бабка родителя - дальний родственник
                                        case Relation.GrandChildOf:
                                            cNewKins[pFarRelative.Key] = Relation.Relative;
                                            break;
                                        //внуки моих родителей, но не мои дети - мои племянники
                                        case Relation.GrandParentOf:
                                            cNewKins[pFarRelative.Key] = Relation.UnkleAunt;
                                            break;
                                        //дяди/тёти моих родителей - слишком дальние родственники
                                        //case Relation.NephewNiece:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //племянники моих родителей - мои кузины
                                        case Relation.UnkleAunt:
                                            cNewKins[pFarRelative.Key] = Relation.Cousin;
                                            break;
                                        //кузины моих родителей (т.е. мои двоюродные дяди/тёти) - слишком дальние родственники
                                        //case Relation.Cousin:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                    }
                                    #endregion
                                    break;
                                case Relation.ParentOf:
                                    #region родственники ребёнка
                                    switch (pFarRelative.Value)
                                    {
                                    //родитель моего ребёнка, но не мой супруг - мой любовник
                                        case Relation.ChildOf:
                                            {
                                                if (GetSpouse() != null)
                                                    cNewKins[pFarRelative.Key] = Relation.Spouse;
                                                else
                                                    cNewKins[pFarRelative.Key] = Relation.Lover;

                                                if (State.DominantCulture.Customs.Has(Customs.SexualOrientation.Heterosexual) &&
                                                    Gender == pFarRelative.Key.Gender)
                                                    cNewKins[pFarRelative.Key] = Relation.Friend;
                                                if (State.DominantCulture.Customs.Has(Customs.SexualOrientation.Homosexual) &&
                                                    Gender != pFarRelative.Key.Gender)
                                                    cNewKins[pFarRelative.Key] = Relation.Friend;
                                            }
                                            break;
                                        //ребёнок моего ребёнка - внук
                                        case Relation.ParentOf:
                                            cNewKins[pFarRelative.Key] = Relation.GrandParentOf;
                                            break;
                                        //брат/сестра моего ребёнка - мой ребёнок
                                        case Relation.Sibling:
                                            {
                                                if (pFarRelative.Key.Relations.ContainsValue(Relation.ChildOf))
                                                    cNewKins[pFarRelative.Key] = Relation.Relative;
                                                else
                                                    cNewKins[pFarRelative.Key] = Relation.ParentOf;
                                            }
                                            break;
                                        //супруг ребёнка - дальний родственник
                                        case Relation.Spouse:
                                            cNewKins[pFarRelative.Key] = Relation.Relative;
                                            break;
                                        //дед/бабка ребёнка, но не мой родитель - дальний родственник
                                        case Relation.GrandChildOf:
                                            cNewKins[pFarRelative.Key] = Relation.Relative;
                                            break;
                                        //внуки моих детей - дальние родственники
                                        case Relation.GrandParentOf:
                                            cNewKins[pFarRelative.Key] = Relation.Relative;
                                            break;
                                        //дяди/тёти моих детей... тут сложно, это могут быть мои братья/сёстры или братья/сёстры моего супруга... определим эти отношения через другие связи
                                        //case Relation.NephewNiece:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //племянники моих детей (внучатые племянники) - дальние родственники... гм... слишком дальние.
                                        //case Relation.UnkleAunt:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //кузины моих детей - мои племянники
                                        case Relation.Cousin:
                                            cNewKins[pFarRelative.Key] = Relation.UnkleAunt;
                                            break;
                                    }
                                    #endregion
                                    break;
                                case Relation.Sibling:
                                    #region родственники брата/сестры
                                    switch(pFarRelative.Value)
                                    {
                                        //родитель брата/сестры, но не мой мой родитель
                                        case Relation.ChildOf:
                                            {
                                                if (!Relations.ContainsValue(Relation.ChildOf))
                                                    cNewKins[pFarRelative.Key] = Relation.ChildOf;
                                            }
                                            break;
                                        //ребёнок брата/сестры - мой племянник
                                        case Relation.ParentOf:
                                            cNewKins[pFarRelative.Key] = Relation.UnkleAunt;
                                            break;
                                        //брат/сестра брата/сестры - мой брат/сестра
                                        case Relation.Sibling:
                                            cNewKins[pFarRelative.Key] = Relation.Sibling;
                                            break;
                                        //супруг брата/сестры - дальний родственник
                                        //case Relation.Spouse:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //дед/бабка брата/сестры - мои дед/бабка
                                        case Relation.GrandChildOf:
                                            cNewKins[pFarRelative.Key] = Relation.GrandChildOf;
                                            break;
                                        //внуки моих брата/сестры - слишком дальние родственники
                                        //case Relation.GrandParentOf:
                                        //    cNewKins[pFarRelative.Key] = Relation.UnkleAunt;
                                        //    break;
                                        //дяди/тёти моих брата/сестры - мои дяди/тёти
                                        case Relation.NephewNiece:
                                            cNewKins[pFarRelative.Key] = Relation.NephewNiece;
                                            break;
                                        //племянники моих брата/сестры - мои племянники
                                        case Relation.UnkleAunt:
                                            cNewKins[pFarRelative.Key] = Relation.UnkleAunt;
                                            break;
                                        //кузины моих брата/сестры - мои кузины
                                        case Relation.Cousin:
                                            cNewKins[pFarRelative.Key] = Relation.Cousin;
                                            break;
                                    }
                                    #endregion
                                    break;
                                case Relation.Spouse:
                                    #region родственники моего супруга
                                    switch (pFarRelative.Value)
                                    {
                                        //родитель супруга - дальний родственник
                                        case Relation.ChildOf:
                                            cNewKins[pFarRelative.Key] = Relation.Relative;
                                            break;
                                        //ребёнок супруга - мой ребёнок
                                        case Relation.ParentOf:
                                            cNewKins[pFarRelative.Key] = Relation.ParentOf;
                                            break;
                                        //брат/сестра супруга - дальний родственник
                                        //case Relation.Sibling:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //супруг супруга - мой супруг
                                        case Relation.Spouse:
                                            cNewKins[pFarRelative.Key] = Relation.Spouse;
                                            break;
                                        //дед/бабка супруга - дальний родственник
                                        //case Relation.GrandChildOf:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //внуки супруга - мои внуки
                                        case Relation.GrandParentOf:
                                            cNewKins[pFarRelative.Key] = Relation.GrandParentOf;
                                            break;
                                        //дяди/тёти супруга - слишком дальние родственники
                                        //case Relation.NephewNiece:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //племянники супруга - мои племянники
                                        //case Relation.UnkleAunt:
                                        //    cNewKins[pFarRelative.Key] = Relation.UnkleAunt;
                                        //    break;
                                        //кузины супруга - слишком дальние родственники
                                        //case Relation.Cousin:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;                                    }
                                    }
                                    #endregion
                                    break;
                                case Relation.GrandChildOf:
                                    #region родственники моих деда/бабки
                                    switch (pFarRelative.Value)
                                    {
                                        //родитель деда/бабки - дальний родственник
                                        case Relation.ChildOf:
                                            cNewKins[pFarRelative.Key] = Relation.Relative;
                                            break;
                                        //ребёнок деда/бабки, но не родитель - дядя/тётя
                                        case Relation.ParentOf:
                                            cNewKins[pFarRelative.Key] = Relation.NephewNiece;
                                            break;
                                        //брат/сестра деда/бабки - дальний родственний
                                        case Relation.Sibling:
                                            cNewKins[pFarRelative.Key] = Relation.Relative;
                                            break;
                                        //супруг деда/бабки - мой дед/бабка
                                        case Relation.Spouse:
                                            cNewKins[pFarRelative.Key] = Relation.GrandChildOf;
                                            break;
                                        //дед/бабка деда/бабки - дальний родственник
                                        case Relation.GrandChildOf:
                                            cNewKins[pFarRelative.Key] = Relation.Relative;
                                            break;
                                        //внуки моих деда/бабки - могут быть моими братьями/сёстрами, а могут и кузинами... решим через другие отношения
                                        //case Relation.GrandParentOf:
                                        //    cNewKins[pFarRelative.Key] = Relation.Cousin;
                                        //    break;
                                        //дяди/тёти моих деда/бабки - слишком дальние родственники
                                        //case Relation.NephewNiece:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //племянники моих деда/бабки - слишком дальние родственники
                                        //case Relation.UnkleAunt:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //кузины моих деда/бабки - слишком дальние родственники
                                        //case Relation.Cousin:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                    }
                                    #endregion
                                    break;
                                case Relation.GrandParentOf:
                                    #region родственники моего внука
                                    switch (pFarRelative.Value)
                                    {
                                        //родитель внука, но не мой ребёнок - дальний родственник (скорее всего, супруг моего ребёнка)
                                        case Relation.ChildOf:
                                            cNewKins[pFarRelative.Key] = Relation.Relative;
                                            break;
                                        //ребёнок внука - дальний родственник
                                        case Relation.ParentOf:
                                            cNewKins[pFarRelative.Key] = Relation.Relative;
                                            break;
                                        //брат/сестра внука - мой внук
                                        case Relation.Sibling:
                                            cNewKins[pFarRelative.Key] = Relation.GrandParentOf;
                                            break;
                                        //супруг внука - дальний родственник
                                        //case Relation.Spouse:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //дед/бабка внука - тут сложно...
                                        //case Relation.GrandChildOf:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //внуки моих внуков - дальние родственники
                                        case Relation.GrandParentOf:
                                            cNewKins[pFarRelative.Key] = Relation.Relative;
                                            break;
                                        //дяди/тёти моих внуков - сложно сказать
                                        //case Relation.NephewNiece:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //племянники моих внуков - слишком дальние родственники
                                        //case Relation.UnkleAunt:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //кузины моих внуков - слишком дальние родственники
                                        //case Relation.Cousin:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                    }
                                    #endregion
                                    break;
                                case Relation.NephewNiece:
                                    #region родственники моего дяди/тёти
                                    switch (pFarRelative.Value)
                                    {
                                        //родитель дяди/тёти - дед или бабка
                                        case Relation.ChildOf:
                                            cNewKins[pFarRelative.Key] = Relation.GrandChildOf;
                                            break;
                                        //ребёнок дяди/тёти - кузин
                                        case Relation.ParentOf:
                                            cNewKins[pFarRelative.Key] = Relation.Cousin;
                                            break;
                                        //брат/сестра дяди/тёти - или мои родители, или мои другие дядя/тётя... сложно сказать
                                        //case Relation.Sibling:
                                        //    cNewKins[pFarRelative.Key] = Relation.NephewNiece;
                                        //    break;
                                        //супруг дяди/тёти - мои дядя или тётя
                                        //case Relation.Spouse:
                                        //    cNewKins[pFarRelative.Key] = Relation.NephewNiece;
                                        //    break;
                                        //дед/бабка дяди/тёти, он же дед/бабка и моего родителя - дальний родственник
                                        case Relation.GrandChildOf:
                                            cNewKins[pFarRelative.Key] = Relation.Relative;
                                            break;
                                        //внуки моих дяди/тёти (двоюродные племянники) - слишком дальние родственники
                                        //case Relation.GrandParentOf:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //дяди/тёти моих дяди/тёти - слишком дальние родственники
                                        //case Relation.NephewNiece:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //племянники моих дяди/тёти - или мои братья/сёстры, или мои кузины... сложно сказать
                                        //case Relation.UnkleAunt:
                                        //    cNewKins[pFarRelative.Key] = Relation.Cousin;
                                        //    break;
                                        //кузины моих дяди/тёти они же кузины моих родителей - слишком дальние родственники
                                        //case Relation.Cousin:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                    }
                                    #endregion
                                    break;
                                case Relation.UnkleAunt:
                                    #region родственники моего племянника/племянницы
                                    switch (pFarRelative.Value)
                                    {
                                        //родитель племянника/племянницы - брат/сестра или его супруг. сложно сказать...
                                        //case Relation.ChildOf:
                                        //    cNewKins[pFarRelative.Key] = Relation.Sibling;
                                        //    break;
                                        //ребёнок племянника/племянницы - слишком дальний родственник
                                        //case Relation.ParentOf:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //брат/сестра племянника/племянницы - тоже племянник/племянница
                                        case Relation.Sibling:
                                            cNewKins[pFarRelative.Key] = Relation.UnkleAunt;
                                            break;
                                        //супруг племянника/племянницы - слишком дальний родственник
                                        //case Relation.Spouse:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //дед/бабка племянника/племянницы - или мой родитель, или родитель супруга кого-то из моих братьев/сесёр. сложно...
                                        //case Relation.GrandChildOf:
                                        //    if (!m_cRelations.ContainsValue(Relation.ChildOf))
                                        //        cNewKins[pFarRelative.Key] = Relation.ChildOf;
                                        //    break;
                                        //внуки моих племянников - слишком дальние родственники
                                        //case Relation.GrandParentOf:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //дяди/тёти моих племянников - либо мои братья/сёстры, либо их супруги. сложно...
                                        //case Relation.NephewNiece:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //племянники моих племянников - слишком дальние родственники
                                        //case Relation.UnkleAunt:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //кузины моих племянников - слишком дальние родственники
                                        //case Relation.Cousin:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                    }
                                    #endregion
                                    break;
                                case Relation.Cousin:
                                    #region родственники моего кузена/кузины
                                    switch (pFarRelative.Value)
                                    {
                                        //родитель кузена/кузины - мои дядя или тётя
                                        case Relation.ChildOf:
                                            cNewKins[pFarRelative.Key] = Relation.NephewNiece;
                                            break;
                                        //ребёнок кузена/кузины (двоюродный племянник) - слишком дальний родственник
                                        //case Relation.ParentOf:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //брат/сестра кузена/кузины - мои кузен/кузина
                                        case Relation.Sibling:
                                            cNewKins[pFarRelative.Key] = Relation.Cousin;
                                            break;
                                        //супруг кузена/кузины - слишком дальний родственник
                                        //case Relation.Spouse:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //дед/бабка кузена/кузины - сложно сказать, может быть и мой дед/бабка, а может родитель супруга дяди/тёти
                                        //case Relation.GrandChildOf:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //внуки моих кузена/кузины - слишком дальние родственники
                                        //case Relation.GrandParentOf:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //дяди/тёти моих кузена/кузины - или мои родители, или их братья/сёстры и их супруги. сложно сказать...
                                        //case Relation.NephewNiece:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //племянники моих кузена/кузины (двоюродные племянники) - слишком дальние родственники
                                        //case Relation.UnkleAunt:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                        //кузины моих кузена/кузины - или мои братья/сёстры, или мои кузены/кузины. сложно...
                                        //case Relation.Cousin:
                                        //    cNewKins[pFarRelative.Key] = Relation.Relative;
                                        //    break;
                                    }
                                    #endregion
                                    break;
                            }
                        }
                    }
                }
            }

            foreach (var pNewKin in cNewKins)
            {
                Relations[pNewKin.Key] = pNewKin.Value;
                pNewKin.Key.Relations[this] = pNewKin.Value;
                switch (pNewKin.Value)
                {
                    case Relation.ParentOf:
                        pNewKin.Key.Relations[this] = Relation.ChildOf;
                        break;
                    case Relation.GrandParentOf:
                        pNewKin.Key.Relations[this] = Relation.GrandChildOf;
                        break;
                    case Relation.ChildOf:
                        pNewKin.Key.Relations[this] = Relation.ParentOf;
                        break;
                    case Relation.GrandChildOf:
                        pNewKin.Key.Relations[this] = Relation.GrandParentOf;
                        break;
                    case Relation.BastardOf:
                        pNewKin.Key.Relations[this] = Relation.ParentOf;
                        break;
                    case Relation.UnkleAunt:
                        pNewKin.Key.Relations[this] = Relation.NephewNiece;
                        break;
                    case Relation.NephewNiece:
                        pNewKin.Key.Relations[this] = Relation.UnkleAunt;
                        break;
                }
            }

            return cNewKins.Count > 0;
        }

        /// <summary>
        /// Главы домов и начальники на производстве - как минимум среднего возраста.
        /// Так же родитель всегда старше ребенка, а наставник - подчиненного (только если они относятся к одномку сословию).
        /// Возвращает true, если возраст изменился.
        /// </summary>
        /// <returns></returns>
        public bool FixAge()
        {
            bool bFixed = false;

            if (Profession.IsMaster && Age == _Age.Young)
            {
                Age = _Age.Adult;
                bFixed = true;
            }

            if (Relations.ContainsValue(Relation.ParentOf))
            {
                foreach (var pRelative in Relations)
                {
                    if (pRelative.Value == Relation.ParentOf ||
                        (pRelative.Value == Relation.Patron &&
                         pRelative.Key.Estate.Rank == Estate.Rank))
                    {
                        if (Age != _Age.Old && Age < pRelative.Key.Age + 1)
                        {
                            Age = pRelative.Key.Age + 1;
                            if (Age > _Age.Old)
                                Age = _Age.Old;
                            bFixed = true;
                        }
                    }
                    if(pRelative.Value == Relation.AlterEgo ||
                        pRelative.Value == Relation.PresentLifeOf ||
                        pRelative.Value == Relation.FormerLifeOf)
                    {
                        if (Age < pRelative.Key.Age)
                        {
                            Age = pRelative.Key.Age;
                            bFixed = true;
                        }
                        if (Age > pRelative.Key.Age)
                        {
                            pRelative.Key.Age = Age;
                            bFixed = true;
                        }
                    }
                }
            }

            return bFixed;
        }

        /// <summary>
        /// Образует треугольники в отношениях, т.е. если кто-то связан с двумя другими персонажами, а сами они между собой не связаны - 
        /// то с высокой вероятностью формируется новая связь между ними.
        /// </summary>
        public void AddDrama()
        {
            //if (m_cRelations.Count > 6)
            //    return;

            List<Person> cNewRelatives = new List<Person>();

            foreach (var pRelative in Relations)
            {
                if (pRelative.Value == Relation.Associate)
                    continue;

                foreach (var pFarRelative in pRelative.Key.Relations)
                {
                    if (pFarRelative.Key == this)
                        continue;

                    if (Relations.ContainsKey(pFarRelative.Key))
                        continue;

                    if (pFarRelative.Value == Relation.Associate)
                        continue;

                    //if (pFarRelative.Key.m_cRelations.Count > 6)
                    //    continue;

                    cNewRelatives.Add(pFarRelative.Key);
                }
            }

            while (cNewRelatives.Count > 0)
            {
                Person pNewKin = cNewRelatives[Rnd.Get(cNewRelatives.Count)];

                int iChances = 20;
                if (pNewKin.Profession.IsMaster)
                    iChances /= 2;
                if (pNewKin.Estate.Rank != Estate.Rank &&
                    pNewKin.Estate.IsOutlaw() &&
                    Estate.IsOutlaw())
                    iChances *= 10;

                //int iFirstLook = (int)(-CalcNormalizedHostility(pNewKin, true)*10);
                //if (iFirstLook > 0)
                //    iChances /= iFirstLook;
                //if (iFirstLook < 0)
                //    iChances *= -iFirstLook;

                if (iChances < 1)
                    iChances = 1;

                if (Rnd.OneChanceFrom(iChances))//(m_cRelations.Count + pNewKin.m_cRelations.Count)/2))
                    AddRelation(pNewKin);

                cNewRelatives.Remove(pNewKin);
            }
        }

        /// <summary>
        /// Добавляет необходимые знакомства между слоями социальной иерархии - все работники знают своего непосредственного начальника, все дворяне знают короля и т.п.
        /// </summary>
        /// <param name="pWorld"></param>
        public void FixSubordination(World pWorld)
        {
            //мы должны знать всех, кто живёт в том же строении
            foreach (Person pDweller in Building.Persons)
                AddRelation(pDweller);

            //главы домов должны знать главу города
            if (Profession.IsMaster)
            {
                foreach (Building pBuilding in HomeLocation.Settlement.Buildings)
                    if (pBuilding.Info == State.Polity.ProvinceCapital.MainBuilding ||
                        pBuilding.Info == State.Polity.StateCapital.MainBuilding)
                        foreach (Person pDweller in pBuilding.Persons)
                            if (pDweller.Profession.IsMaster || pBuilding.Info.Ownership == FamilyOwnership.Full)
                            {
                                AddRelation(pDweller);
                                if (Rnd.OneChanceFrom(2))
                                    break;
                            }
            }
            
            //члены высшего общества в одном городе всегда знают друг-друга
            if (Estate.IsElite())
            {
                foreach (Building pBuilding in HomeLocation.Settlement.Buildings)
                    foreach (Person pDweller in pBuilding.Persons)
                    {
                        if (pDweller.Estate.IsElite())
                            AddRelation(pDweller);
                    }
            }

            //бандиты знают всех глав домов (?) своего города, а главари бандитов - знают главарей из других городов, в том числе заграничных
            if (Estate.IsOutlaw())
            {
                foreach (Building pBuilding in HomeLocation.Settlement.Buildings)
                {
                    foreach (Person pDweller in pBuilding.Persons)
                    {
                        if (pDweller.Profession.IsMaster && Rnd.OneChanceFrom(2))
                            AddRelation(pDweller);
                        if (pDweller.Estate.IsOutlaw())
                            AddRelation(pDweller);
                    }
                }

                if (Profession.IsMaster)
                {
                    foreach (State pState in pWorld.m_aStates)
                    {
                        foreach (LocationX pLand in pState.m_pSociety.Settlements)
                        {
                            foreach (Building pBuilding in pLand.Settlement.Buildings)
                            {
                                foreach (Person pDweller in pBuilding.Persons)
                                {
                                    if (pDweller.Estate.IsOutlaw() && pDweller.Profession.IsMaster && Rnd.OneChanceFrom(2))
                                    {
                                        AddRelation(pDweller);
                                        if (Rnd.OneChanceFrom(2))
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //мы должны знать правителей своего государства
            //foreach (Building pBuilding in m_pHome.Owner.Settlements[0].Settlement.m_cBuildings)
            //    if (pBuilding.m_pInfo == m_pHome.Owner.m_pInfo.m_pStateCapital.m_pMainBuilding)
            //        foreach (CPerson pDweller in pBuilding.m_cPersons)
            //            if (pDweller.m_pProfession.m_bMaster || pBuilding.m_pInfo.m_eOwnership == FamilyOwnership.Full)
            //                AddRelation(pDweller);

            //главы городов одного государства должны знать друг-друга
            if (Building.Info == State.Polity.ProvinceCapital.MainBuilding &&
                Profession.IsMaster)
            {
                foreach (LocationX pLand in State.Settlements)
                {
                    foreach (Building pBuilding in pLand.Settlement.Buildings)
                    {
                        if ((pBuilding.Info == State.Polity.ProvinceCapital.MainBuilding && Rnd.OneChanceFrom(2)) ||
                            pBuilding.Info == State.Polity.StateCapital.MainBuilding)
                        {
                            foreach (Person pDweller in pBuilding.Persons)
                            {
                                if (pDweller.Profession.IsMaster || pBuilding.Info.Ownership == FamilyOwnership.Full)
                                {
                                    AddRelation(pDweller);
                                    if (Rnd.OneChanceFrom(2))
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            //главы государств должны знать друг-друга
            if (Building.Info == State.Polity.StateCapital.MainBuilding &&
                Profession.IsMaster)
            {
                foreach (State pState in pWorld.m_aStates)
                {
                    foreach (LocationX pLand in pState.m_pSociety.Settlements)
                    {
                        foreach (Building pBuilding in pLand.Settlement.Buildings)
                        {
                            if (pBuilding.Info == pState.m_pSociety.Polity.StateCapital.MainBuilding)
                            {
                                foreach (Person pDweller in pBuilding.Persons)
                                {
                                    if (pDweller.Profession.IsMaster)// || pBuilding.m_pInfo.m_eOwnership == FamilyOwnership.Full)
                                    {
                                        AddRelation(pDweller);
                                        if (Rnd.OneChanceFrom(2))
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void UpdateRelations()
        {
            Dictionary<Person, float> cTensions = new Dictionary<Person, float>();
            float fMaxAttraction = float.MinValue;
            float fMinAttraction = float.MaxValue;
            int iEnemies = 0;
            int iFriends = 0;
            int iLovers = 0;
            foreach (var pRelative in Relations)
            {
                float fAttraction = GetAttraction(this, pRelative.Key);
                if (fAttraction > fMaxAttraction)
                    fMaxAttraction = fAttraction;
                if (fAttraction < fMinAttraction)
                    fMinAttraction = fAttraction;

                cTensions[pRelative.Key] = fAttraction;

                if (pRelative.Value == Relation.Archenemy)
                    iEnemies++;
                if (pRelative.Value == Relation.Friend)
                    iFriends++;
                if (pRelative.Value == Relation.Lover)
                    iLovers++;
            }

            if (fMaxAttraction == 0)
                fMaxAttraction = 1;
            if (fMinAttraction == 0)
                fMinAttraction = 1;

            Dictionary<Person, float> cEnemies = new Dictionary<Person, float>();
            Dictionary<Person, float> cFriends = new Dictionary<Person, float>();
            Dictionary<Person, float> cLovers = new Dictionary<Person, float>();

            Dictionary<Person, Relation> cChanges = new Dictionary<Person,Relation>();

            foreach (var pRelative in Relations)
            {
                float fTension = cTensions[pRelative.Key];
                if (fTension < 0)
                {
                    cEnemies[pRelative.Key] = fTension / fMinAttraction;
                    //int iRelEnemies = 1;
                    //foreach(var pRelRel in pRelative.Key.m_cRelations)
                    //{
                    //    if(pRelRel.Value == Relation.Archenemy)
                    //        iRelEnemies++;
                    //}
                    //cEnemies[pRelative.Key] /= iRelEnemies*iRelEnemies;
                }
                else if (pRelative.Value == Relation.Associate)
                {
                    //cChances[Relation.Patron] = 1;
                    //cChances[Relation.Agent] = 1;

                    //if (pRelative.Key.GetInfluence(true) < GetInfluence(true))
                    //    cChances[Relation.Patron] = 2;
                    //else
                    //    cChances[Relation.Agent] = 2;

                    cFriends[pRelative.Key] = fTension / fMaxAttraction;
                }

                //if (pRelative.Value != Relation.Associate)
                //    continue;

                if (Culture.Customs.Has(Customs.Sexuality.Puritan))
                    continue;

                //int iMaxTensionMod = fMaxTension;
                //if (m_pCustoms.m_eSexuality == Customs.Sexuality.Lecherous)
                //{ 
                //    iTension = iTension - (iMaxTension + iMinTension) / 2;
                //    iMaxTensionMod = (iMaxTension - iMinTension) / 2;
                //}

                if (fTension > 0)
                {
                    if (Age < _Age.Old &&
                        pRelative.Key.Age < _Age.Old &&
                        Math.Abs(Age - pRelative.Key.Age) <= 1 &&
                        pRelative.Key.Gender == GetCompatibleSexPartnerGender(false) &&
                        Gender == pRelative.Key.GetCompatibleSexPartnerGender(false))
                    {
                        if (//m_pProfession != pRelative.Key.m_pProfession &&
                            //!(IsRuler() && pRelative.Key.IsRuler()) &&
                            pRelative.Key.GetLover() != null)
                        {
                            if (pRelative.Key.Culture.Customs.Has(Customs.Sexuality.Moderate_sexuality))
                            {
                                cLovers[pRelative.Key] = fTension / fMaxAttraction;
                            }
                            else if (pRelative.Key.Culture.Customs.Has(Customs.Sexuality.Lecherous))
                            { 
                                cLovers[pRelative.Key] = 2f * fTension / fMaxAttraction;
                            }
                        }
                    }
                }
            }

            if (cLovers.Count > 0)
            {
                Person pLover = cLovers.Keys.ElementAt(Rnd.ChooseOne(cLovers.Values, 3));
                if (Relations[pLover] == Relation.Associate)
                {
                    cChanges[pLover] = Relation.Lover;

                    if (cEnemies.ContainsKey(pLover))
                        cEnemies.Remove(pLover);
                    if (cFriends.ContainsKey(pLover))
                        cFriends.Remove(pLover);
                }
            }

           if (cEnemies.Count > 0)
            {
                //if (iEnemies == 0)
                {
                    Person pEnemy = cEnemies.Keys.ElementAt(Rnd.ChooseOne(cEnemies.Values, 3));
                    if(Relations[pEnemy] == Relation.Associate)
                        cChanges[pEnemy] = Relation.Archenemy;
                }
            }

            if (cFriends.Count > 0)
            {
                if (iFriends == 0)
                {
                    Person pFriend = cFriends.Keys.ElementAt(Rnd.ChooseOne(cFriends.Values, 3));
                    cChanges[pFriend] = Relation.Friend;
                }
            }

            foreach (var pChange in cChanges)
            {
                Relations[pChange.Key] = pChange.Value;
                pChange.Key.Relations[this] = pChange.Value;
                switch (pChange.Value)
                {
                    case Relation.Patron:
                        pChange.Key.Relations[this] = Relation.Agent;
                        break;
                    case Relation.Agent:
                        pChange.Key.Relations[this] = Relation.Patron;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает вероятную возможную связь между 2 персонажами.
        /// Возможны только варианты FormerLife, PresentLife, BastardOf, AlterEgo и Associate.
        /// </summary>
        /// <param name="pPretender"></param>
        /// <returns></returns>
        private Relation PossibleRelation(Person pPretender)
        {
            Dictionary<Relation, float> cChances = new Dictionary<Relation, float>();
            //if (m_pHome.Owner.m_pCustoms.GenderPriority == Customs.GenderPriority.Genders_equality ||
            //    Gender == pPretender.Gender)
            //{
            //    //не может быть ни дружбы, ни вражды между элитой и люмпенами
            //    if ((m_eEstate != CSocialOrder.Estate.Low || pPretender.m_eEstate != CSocialOrder.Estate.Elite) &&
            //        (m_eEstate != CSocialOrder.Estate.Elite || pPretender.m_eEstate != CSocialOrder.Estate.Low))
            //    {
            //        cChances[Relation.Associate] = 2.0f - m_pCulture.MentalityValues[Mentality.Selfishness][m_iCultureLevel];
            //        cChances[Relation.Friend] = 2.0f - m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel];
            //    }
            //}
            //if (m_eEstate != pPretender.m_eEstate)
            //{
            //    if (pPretender.m_eEstate != CSocialOrder.Estate.Low)
            //        cChances[Relation.Agent] = m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel];
            //    if (pPretender.m_eEstate != CSocialOrder.Estate.Elite)
            //        cChances[Relation.Patron] = m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel];
            //}
            cChances[Relation.Associate] = 2;
            
            if (Estate.Rank != pPretender.Estate.Rank &&
                Gender == pPretender.Gender &&
                Age == pPretender.Age &&
                !Relations.ContainsValue(Relation.FormerLifeOf) &&
                !Relations.ContainsValue(Relation.PresentLifeOf) &&
                !pPretender.Relations.ContainsValue(Relation.FormerLifeOf) &&
                !pPretender.Relations.ContainsValue(Relation.PresentLifeOf))
            {
                if (!IsRuler() && !pPretender.Relations.ContainsValue(Relation.ChildOf) && !pPretender.Relations.ContainsValue(Relation.Sibling))
                    cChances[Relation.FormerLifeOf] = 1;
                if (!pPretender.IsRuler() && !Relations.ContainsValue(Relation.ChildOf) && !Relations.ContainsValue(Relation.Sibling))
                    cChances[Relation.PresentLifeOf] = 1;
            }

            if (Age < pPretender.Age)
            { 
                switch(m_pPhenotype.m_pValues.Get<LifeCycleGenetix>().BirthRate)
                {
                    case BirthRate.Moderate:
                        cChances[Relation.BastardOf] = 0.25f;
                        break;
                    case BirthRate.High:
                        cChances[Relation.BastardOf] = 0.5f;
                        break;
                }
            }
            //if (m_pProfession != pPretender.m_pProfession &&
            //    (!IsRuler() || !pPretender.IsRuler()))
            //{
            //    switch (m_pCustoms.m_eSexuality)
            //    {
            //        case Psichology.Customs.Sexuality.Moderate_sexuality:
            //            cChances[Relation.Lover] = 1;
            //            break;
            //        case Psichology.Customs.Sexuality.Lecherous:
            //            cChances[Relation.Lover] = 2;
            //            break;
            //    }
            //}

            if (Gender == pPretender.Gender &&
                HomeLocation == pPretender.HomeLocation)
            {
                float fKoeff = 0;
                foreach(var pSkill in Profession.Skills)
                {
                    fKoeff += Math.Abs(pSkill.Value - pPretender.Profession.Skills[pSkill.Key]);
                }
                fKoeff /= Profession.Skills.Count * 3;

                if (Estate.Rank != pPretender.Estate.Rank &&
                    (Estate.IsOutlaw() || pPretender.Estate.IsOutlaw()) &&
                    fKoeff < 0.25)
                    fKoeff = 0.25f;

                cChances[Relation.AlterEgo] = fKoeff;
            }

            if (cChances.Count == 0)
                return Relation.Associate;

            Relation eChoice = cChances.Keys.ElementAt(Rnd.ChooseOne(cChances.Values));
            //if (eChoice == Relation.Lover)
            //{
            //    CPerson pLover = pPretender.GetLover();
            //    if (pLover == null)
            //        eChoice = Relation.Friend;

            //    if (m_pCustoms.m_eSexRelations == Customs.SexualOrientation.Heterosexual &&
            //        Gender == pPretender.Gender)
            //        eChoice = Relation.Friend;
            //    if (m_pCustoms.m_eSexRelations == Customs.SexualOrientation.Homosexual &&
            //        Gender != pPretender.Gender)
            //        eChoice = Relation.Friend;
            //}

            //if (eChoice == Relation.Patron && pPretender.m_eEstate > m_eEstate)
            //    eChoice = Relation.Agent;
            //if (eChoice == Relation.Agent && pPretender.m_eEstate < m_eEstate)
            //    eChoice = Relation.Patron;

            return eChoice;
        }

        /// <summary>
        /// Является ли персонаж правителем государства или города?
        /// </summary>
        /// <returns></returns>
        public bool IsRuler()
        {
            return IsRuler(true);
        }
        /// <summary>
        /// Является ли персонаж правителем государства или города?
        /// </summary>
        /// <returns></returns>
        public bool IsRuler(bool bMinor)
        {
            if (HomeLocation == null)
                return false;

            if (Profession == null)
                return false;

            if (State.Polity.StateCapital.MainBuilding.OwnerProfession == Profession)
                return true;

            if (bMinor && State.Polity.ProvinceCapital.MainBuilding.OwnerProfession == Profession)
                return true;

            return false;
        }

        public bool IsInGovernment()
        {
            if (HomeLocation == null)
                return false;

            if (Profession == null)
                return false;

            //if (m_pHome.Owner.m_pInfo.m_pStateCapital.m_pMainBuilding.m_pOwner == m_pProfession)
            //    return true;

            if (//m_pHome.Owner.m_pInfo.m_pStateCapital.m_pMainBuilding.m_eOwnership == FamilyOwnership.Full && 
                State.Polity.StateCapital.MainBuilding == Building.Info)
                return true;

            return false;
        }

        public int GetInfluence(bool bCombined)
        {
            float fBaseInfluence = 1;

            switch (State.InfrastructureLevel)
            {
                // 0 - Каменный век. Шкуры, дубинки, каменные топоры, копья с каменными или костяными наконечниками. GURPS TL0
                case 0:
                    fBaseInfluence = 1;//1
                    break;
                // 1 - Бронзовый век. В ходу бронзовые кирасы и кожаные доспехи, бронзовое холодное оружие, из стрелкового оружия – луки и дротики. GURPS TL1
                case 1:
                    fBaseInfluence = 2;//2
                    break;
                // 2 - Железный век. Стальное холодное оружие, кольчуги, рыцарские доспехи. Из стрелкового оружия - луки и арбалеты. GURPS TL2-3
                case 2:
                    fBaseInfluence = 5;//3-4
                    break;
                // 3 - Эпоха пороха. Примитивное однозарядное огнестрельное оружие, лёгкие сабли и шпаги, облегчённая броня (кирасы, кожаные куртки). GURPS TL4-5
                case 3:
                    fBaseInfluence = 15;//8-20
                    break;
                // 4 - Индустриальная эра. Нарезное огнестрельное оружие, примитивная бронетехника и авиация, химическое оружие массового поражения. GURPS TL6
                case 4:
                    fBaseInfluence = 35;//40
                    break;
                // 5 - Атомная эра. Автоматическое огнестрельное оружие, бронежилеты, развитая бронетехника и авиация, ядерные ракеты и бомбы. GURPS TL7-8
                case 5:
                    fBaseInfluence = 65;//60-80
                    break;
                // 6 - Энергетическая эра. Силовые поля, лучевое оружие. GURPS TL9-10
                case 6:
                    fBaseInfluence = 100;//120-200
                    break;
                // 7 - Квантовая эра. Ограниченная телепортация, материализация, дезинтеграция. GURPS TL11-12
                case 7:
                    fBaseInfluence = 150;//300-400
                    break;
                // 8 - Переход. Полная и неограниченная власть человека над пространственно-временным континуумом. GURPS TL12+
                case 8:
                    fBaseInfluence = 200;
                    break;
            }

            float fPersonalInfluence = fBaseInfluence;

            if (Profession.IsMaster)
                fPersonalInfluence *= 2;

            switch (Estate.Rank)
            {
                case Estate.SocialRank.Lowlifes:
                    //Формула рассчёта коэффициента значимости для низшего сословия: (0.75x3 - 0.8x2 + 3.4x + 1.2) / 5
                    //Она даёт значения 0.2 1 2 5 10
                    fPersonalInfluence *= (float)(0.75 * Math.Pow(State.SocialEquality, 3) - 0.8 * Math.Pow(State.SocialEquality, 2) + 3.4 * State.SocialEquality + 1.2) / 5;
                    //if (m_pHome.Owner.m_iSocialEquality == 0)
                    //    fPersonalInfluence /= 5;
                    //else if (m_pHome.Owner.m_iSocialEquality == 1)
                    //{
                    //    fPersonalInfluence /= 2;
                    //}
                    break;
                case Estate.SocialRank.Commoners:
                    //Для среднего сословия: (-0.58x3 + 3.4x2 + 0.4x + 1.16) / 2
                    //Она даёт значения 0.5 2 5 9 10
                    fPersonalInfluence *= (float)(-0.58 * Math.Pow(State.SocialEquality, 3) + 3.4 * Math.Pow(State.SocialEquality, 2) + 0.4 * State.SocialEquality + 1.6) / 2;
                    //fPersonalInfluence *= 2;
                    break;
                case Estate.SocialRank.Clergy:
                    fPersonalInfluence *= 5;//20;
                    break;
                case Estate.SocialRank.Elite:
                    //для элиты коэффициент всегда 10, независимо от величины социального неравенства.
                    //регинальные правители и главы государств имеют бонус
                    if(IsInGovernment())
                        fPersonalInfluence *= 50;//10000;
                    else if (IsRuler())
                        fPersonalInfluence *= 22;//1000;
                    else
                        fPersonalInfluence *= 10;//20;
                    break;
                case Estate.SocialRank.Outlaws:
                    fPersonalInfluence *= 50f / (State.Control + 1);
                    break;
            }

            if (State.DominantCulture.Customs.Has(Customs.GenderPriority.Matriarchy) && Gender == Gender.Male)
                fPersonalInfluence /= 2;

            if (State.DominantCulture.Customs.Has(Customs.GenderPriority.Patriarchy) && Gender == Gender.Female)
                fPersonalInfluence /= 2;

            if (Age == _Age.Young)
                fPersonalInfluence /= 2;

            if (Age == _Age.Aged)
            {
                if (State.MostRespectedSkill == Skill.Body)
                    fPersonalInfluence *= 0.75f;
                if (State.MostRespectedSkill == Skill.Mind)
                    fPersonalInfluence *= 1.25f;
            }

            if (Age == _Age.Old)
                fPersonalInfluence /= 5;

            if (bCombined)
            {
                float fSocialInfluence = fPersonalInfluence;

                foreach (var pLink in Relations)
                {
                    if (pLink.Value == Relation.ChildOf)
                        fSocialInfluence += GetProximity(this, pLink.Key) * pLink.Key.GetInfluence(false) / 2;// m_cRelations.Count;
                    else if (pLink.Value == Relation.ParentOf)
                        fSocialInfluence += GetProximity(this, pLink.Key) * pLink.Key.GetInfluence(false) / 5;// m_cRelations.Count;
                    //else if (pLink.Value == Relation.BastardOf)
                    //    fSocialInfluence += pLink.Key.GetInfluence(false) / 10;// m_cRelations.Count;
                    //else if (pLink.Value == Relation.AlterEgo)
                    //    fSocialInfluence += pLink.Key.GetInfluence(false);// m_cRelations.Count;
                    //else if (pLink.Value == Relation.Lover)
                    //    fSocialInfluence += pLink.Key.GetInfluence(false) / 2;// m_cRelations.Count;
                    //else if (pLink.Value == Relation.Agent)
                    //    fSocialInfluence += pLink.Key.GetInfluence(false) / 2;// m_cRelations.Count;
                    else if (pLink.Value == Relation.Associate)
                        fSocialInfluence += GetProximity(this, pLink.Key) * pLink.Key.GetInfluence(false) / 10;// m_cRelations.Count;
                    //else if (pLink.Value == Relation.FormerLife || pLink.Value == Relation.PresentLife)
                    //    fSocialInfluence += pLink.Key.GetInfluence(false) / 100;// m_cRelations.Count;
                    else
                        fSocialInfluence += GetProximity(this, pLink.Key) * pLink.Key.GetInfluence(false) / 3;// m_cRelations.Count;
                }

                fPersonalInfluence = (float)Math.Sqrt(fPersonalInfluence * fSocialInfluence);
            }

            if (fPersonalInfluence < 1)
                fPersonalInfluence = 1;

            return (int)fPersonalInfluence;
        }

        //private float GetNegativeRelationWeight(Relation eRelation)
        //{
        //    float fK = 1;
        //    //if (eRelation == Relation.Friend)
        //    //    fK = 0.1f;
        //    if (eRelation == Relation.AlterEgo || eRelation == Relation.FormerLife || eRelation == Relation.PresentLife)
        //        fK = 0;
        //    if (eRelation == Relation.Patron)
        //        fK = 0.15f;
        //    if (eRelation == Relation.Associate)
        //        fK = 0.75f;
        //    //if (eRelation == Relation.ChildOf || eRelation == Relation.ParentOf)
        //    if (eRelation == Relation.ParentOf)
        //        fK = 0.5f;
        //    if (eRelation == Relation.Relative)
        //        fK = 0.2f;
        //    if (eRelation == Relation.BastardOf)
        //        fK = 1.25f;

        //    return fK;
        //}

        //private float GetPositiveRelationWeight(Relation eRelation)
        //{
        //    float fK = 1;
        //    //if (eRelation == Relation.Friend || eRelation == Relation.Lover || eRelation == Relation.Spouse)
        //    //    fK = 1.25f;
        //    if (eRelation == Relation.AlterEgo || eRelation == Relation.FormerLife || eRelation == Relation.PresentLife)
        //        fK = 0;
        //    if (eRelation == Relation.Patron)
        //        fK = 0.15f;
        //    if (eRelation == Relation.Associate)
        //        fK = 0.75f;
        //    //if (eRelation == Relation.ChildOf || eRelation == Relation.ParentOf)
        //    if (eRelation == Relation.ParentOf)
        //        fK = 1.5f;
        //    if (eRelation == Relation.Relative)
        //        fK = 0.2f;
        //    //if (eRelation == Relation.BastardOf)
        //    //    fK = 1.25f;

        //    return fK;
        //}

        //private int GetInfluenceDiff(CPerson pPerson)
        //{
        //    if (!m_cRelations.ContainsKey(pPerson))
        //        return 0;

        //    int iInf = GetInfluence(true);
        //    int iLinkInf = pPerson.GetInfluence(true);

        //    return iLinkInf - iInf;
        //}

        //public int GetAdmiration(CPerson pPerson)
        //{
        //    int iDiff = GetInfluenceDiff(pPerson);

        //    int iAdmiration = 0;
        //    if (iDiff > 0)
        //        iAdmiration += (int)(GetPositiveRelationWeight(m_cRelations[pPerson]) * iDiff);

        //    //iAdmiration = (int)((float)iAdmiration * (2.0 - m_pHome.Owner.m_pCulture.MentalityValues[Mentality.Selfishness][m_pHome.Owner.m_iCultureLevel]));

        //    return iAdmiration / 10;
        //}

        //public int GetCompassion(CPerson pPerson)
        //{
        //    int iDiff = GetInfluenceDiff(pPerson);

        //    int iCompassion = 0;
        //    if (iDiff < 0)
        //        iCompassion += (int)(GetPositiveRelationWeight(m_cRelations[pPerson]) * (-iDiff));

        //    //iCompassion = (int)((float)iCompassion * (2.0 - m_pHome.Owner.m_pCulture.MentalityValues[Mentality.Fanaticism][m_pHome.Owner.m_iCultureLevel]));

        //    return iCompassion / 10;
        //}

        //public int GetEnvy(CPerson pPerson)
        //{
        //    int iDiff = GetInfluenceDiff(pPerson);

        //    int iEnvy = 0;
        //    if (iDiff > 0)
        //        iEnvy += (int)(GetNegativeRelationWeight(m_cRelations[pPerson]) * iDiff);

        //    //iEnvy = (int)((float)iEnvy * m_pHome.Owner.m_pCulture.MentalityValues[Mentality.Selfishness][m_pHome.Owner.m_iCultureLevel]);

        //    return iEnvy / 10;
        //}

        //public int GetArrogance(CPerson pPerson)
        //{
        //    int iDiff = GetInfluenceDiff(pPerson);

        //    int iArrogance = 0;
        //    if (iDiff < 0)
        //        iArrogance += (int)(GetNegativeRelationWeight(m_cRelations[pPerson]) * (-iDiff));

        //    //iArrogance = (int)((float)iArrogance * m_pHome.Owner.m_pCulture.MentalityValues[Mentality.Fanaticism][m_pHome.Owner.m_iCultureLevel]);

        //    return iArrogance / 10;
        //}

        //public int GetEnvy()
        //{ 
        //    int iEnvy = 0;
        //    int iInf = GetInfluence(true);
        //    foreach (var pLink in m_cRelations)
        //    {
        //        int iLinkInf = pLink.Key.GetInfluence(true);
        //        if (iLinkInf > iInf)
        //            iEnvy += (int)(GetNegativeRelationWeight(pLink.Value)* (iLinkInf - iInf));
        //    }

        //    iEnvy = (int)((float)iEnvy * m_pCulture.MentalityValues[Mentality.Selfishness][m_iCultureLevel]);

        //    return iEnvy/10;
        //}

        //public int GetArrogance()
        //{
        //    int iArrogance = 0;
        //    int iInf = GetInfluence(true);
        //    foreach (var pLink in m_cRelations)
        //    {
        //        int iLinkInf = pLink.Key.GetInfluence(true);
        //        if (iLinkInf < iInf)
        //            iArrogance += (int)(GetNegativeRelationWeight(pLink.Value) * (iInf - iLinkInf));
        //    }

        //    iArrogance = (int)((float)iArrogance * m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel]);

        //    return iArrogance / 10;
        //}

        //public int GetPowerHunger()
        //{
        //    int iEnvy = GetEnvy();
        //    int iArrogance = GetArrogance();
        //    int iDiscontent = iEnvy > iArrogance ? iEnvy - iArrogance : 0;

        //    return iDiscontent * GetInfluence(true);
        //}

        public int GetFenotypeDifference(Person pOpponent, ref string sPositiveReasons, ref string sNegativeReasons)
        {
            //Будем сравнивать эталонную внешность для нашей нации с реальной внешностью оппонента
            Phenotype pNation = pOpponent.Gender == Gender.Male ? Nation.PhenotypeMale : Nation.PhenotypeFemale;
            Phenotype pOpponents = pOpponent.m_pPhenotype;

            return pNation.GetFenotypeDifference(pOpponents, ref sPositiveReasons, ref sNegativeReasons);
        }

        /// <summary>
        /// Вычисляет враждебность друг персонажей на основании обычаев их народов.
        /// Возвращает значение от 6 (полные противоположности) до -6 (полное совпадение).
        /// </summary>
        /// <param name="pOpponent">обычаи другого сообщества</param>
        /// <param name="sReasons">мотивация враждебности</param>
        /// <returns></returns>
        public int GetCustomsDifference(Person pOpponent, ref string sPositiveReasons, ref string sNegativeReasons)
        {
            int iHostility = 0;

            Customs pMine = Culture.Customs;
            //pMine - это поведение, которое мы считаем "правильным". Если в нашем обществе есть различия между нормами поведения для мужчин и женщин,
            //а оппонент другого пола - эти различия тоже нужно учитывать.
            if (Estate.IsSegregated() && Gender != pOpponent.Gender)
            {
                pMine = Customs.ApplyDifferences(Culture.Customs, Estate.DominantCulture.Customs, Estate.Culture[pOpponent.Gender].Customs);
                pMine.ApplyFenotype(pOpponent.m_pPhenotype);
            }
            Customs pOpponents = pOpponent.Culture.Customs;

            if (!pMine.HasIdentical<Customs.GenderPriority>(pOpponents) &&
                !pMine.Has(Customs.GenderPriority.Genders_equality) &&
                !pOpponents.Has(Customs.GenderPriority.Genders_equality))
            {
                if (pMine.Has(Customs.GenderPriority.Patriarchy))
                {
                    if (Gender == Gender.Male && pOpponent.Gender == Gender.Female)
                    {
                        if (!pOpponent.IsSlave())
                        {
                            iHostility++;
                            sNegativeReasons += " (-1) [SOC] impudent female\n";
                        }
                    }
                    //if (Gender == Gender.Male && pOpponent.Gender == Gender.Male)
                    //{
                    //    iHostility++;
                    //    sNegativeReasons += " (-1) [SOC] female's servant\n";
                    //}
                }
                else
                {
                    if (Gender == Gender.Female && pOpponent.Gender == Gender.Male)
                    {
                        if (!pOpponent.IsSlave())
                        {
                            iHostility++;
                            sNegativeReasons += " (-1) [SOC] impudent male\n";
                        }
                    } 
                    //if (Gender == Gender.Female && pOpponent.Gender == Gender.Female)
                    //{
                    //    iHostility++;
                    //    sNegativeReasons += " (-1) [SOC] male's servant\n";
                    //}
                }
            }

            if (pMine.Has(Customs.MindSet.Logic))
            {
                if (pOpponent.Skills[Skill.Mind] < ProfessionInfo.SkillLevel.Bad)//m_cSkills[Skill.Mind] - 1)
                {
                    iHostility++;
                    sNegativeReasons += " (-1) [SOC] too stupid\n";
                }
                else if (pOpponent.Skills[Skill.Mind] > ProfessionInfo.SkillLevel.Good)// m_cSkills[Skill.Mind] + 1)
                {
                    iHostility -= 2;
                    sPositiveReasons += " (+2) [SOC] genius\n";
                }
                //else if (pOpponent.m_cSkills[Skill.Mind] > m_cSkills[Skill.Mind])
                //{
                //    iHostility--;
                //    sPositiveReasons += " (+1) smart one\n";
                //}
            }
            else if (pMine.Has(Customs.MindSet.Emotions))
            { 
                //if (pOpponent.m_cSkills[Skill.Mind] <= m_cSkills[Skill.Mind])
                //{
                //    iHostility--;
                //    sPositiveReasons += " (+1) right-minded\n";
                //}
                //else 
                if (pOpponent.Skills[Skill.Mind] > ProfessionInfo.SkillLevel.Good)//m_cSkills[Skill.Mind] + 1)
                {
                    iHostility += 1;
                    sNegativeReasons += " (-1) [SOC] egghead\n";
                }
                //else if (pOpponent.m_cSkills[Skill.Mind] > ProfessionInfo.SkillLevel.Bad)//m_cSkills[Skill.Mind])
                //{
                //    iHostility++;
                //    sNegativeReasons += " (-1) [SOC] too smart\n";
                //}
            }
            //else if (pMine.Has(Customs.MindSet.Balanced_mind)
            //{
            //    if (pOpponent.m_cSkills[Skill.Mind] < ProfessionInfo.SkillLevel.Bad)//m_cSkills[Skill.Mind] - 1)
            //    {
            //        iHostility++;
            //        sNegativeReasons += " (-1) [SOC] too stupid\n";
            //    }
            //    else if (pOpponent.m_cSkills[Skill.Mind] > ProfessionInfo.SkillLevel.Good)//m_cSkills[Skill.Mind] + 1)
            //    {
            //        iHostility++;
            //        sNegativeReasons += " (-1) [SOC] too smart\n";
            //    }
            //    //else if (pOpponent.m_cSkills[Skill.Mind] == m_cSkills[Skill.Mind])
            //    //{
            //    //    iHostility--;
            //    //    sPositiveReasons += " (+1) right-minded\n";
            //    //}
            //}

            if (!pMine.HasIdentical<Customs.Sexuality>(pOpponents) &&
                !pMine.Has(Customs.Sexuality.Moderate_sexuality) &&
                !pOpponents.Has(Customs.Sexuality.Moderate_sexuality))
            {
                iHostility++;
                sNegativeReasons += " (-1) [SOC] " + pOpponents.ValueOf<Customs.Sexuality>().ToString().Replace('_', ' ').ToLower() + "\n";
            }

            if (!pMine.HasIdentical<Customs.SexualOrientation>(pOpponents) && 
                !pMine.Has(Customs.SexualOrientation.Bisexual) &&
                !pOpponents.Has(Customs.Sexuality.Puritan))
            {
                if (pOpponents.Has(Customs.SexualOrientation.Bisexual))
                {
                    iHostility += 1;
                    sNegativeReasons += " (-1) [SOC] " + pOpponents.ValueOf<Customs.SexualOrientation>().ToString().Replace('_', ' ').ToLower() + "\n";
                }
                //else
                //{
                //    //foreach (var pRelative in pOpponent.m_cRelations)
                //    //{
                //    //    if (IsSexualRelation(pRelative.Value))
                //    //    {
                //    //        if (pMine.m_eSexRelations == Customs.SexualOrientation.Heterosexual && pOpponent.Gender == pRelative.Key.Gender)
                //    //        {
                //    //            iHostility++;
                //    //            sNegativeReasons += " (-1) have homosexual partner\n";
                //    //            break;
                //    //        }
                //    //        if (pMine.m_eSexRelations == Customs.SexualOrientation.Homosexual && pOpponent.Gender != pRelative.Key.Gender)
                //    //        {
                //    //            iHostility++;
                //    //            sNegativeReasons += " (-1) have heterosexual partner\n";
                //    //            break;
                //    //        }
                //    //    }
                //    //}
                //    iHostility++;
                //    sNegativeReasons += " (-1) [SOC] loose morale\n";
                //}

            }

            if (pMine.ValueOf<Customs.MarriageType>() < pOpponents.ValueOf<Customs.MarriageType>())
            {
                iHostility++;
                sNegativeReasons += " (-1) [SOC] " + pOpponents.ValueOf<Customs.MarriageType>().ToString().Replace('_', ' ').ToLower() + "\n";
            }

            if (pMine.Has(Customs.BodyModifications.Body_Modifications_Mandatory))
            {
                if (pOpponents.Has(Customs.BodyModifications.Body_Modifications_Blamed))
                {
                    iHostility++;
                    sNegativeReasons += " (-1) [SOC] no proper body modifications\n";
                }
                else if (pOpponents.Has(Customs.BodyModifications.Body_Modifications_Mandatory))
                {
                    iHostility--;
                    sPositiveReasons += " (+1) [SOC] have proper body modifications\n";
                }
            }
            else if (pMine.Has(Customs.BodyModifications.Body_Modifications_Blamed))
            { 
                //if (pOpponents.m_eBodyModifications == Customs.BodyModifications.Body_Modifications_Allowed)
                //{
                //    iHostility++;
                //    sNegativeReasons += " (-1) [SOC] have body modifications\n";
                //}
                //else 
                if (pOpponents.Has(Customs.BodyModifications.Body_Modifications_Mandatory))
                {
                    iHostility += 1;
                    sNegativeReasons += " (-1) [SOC] have disgusting body modifications\n";
                }
            }

            if (pMine.Has(Customs.Clothes.Minimal_Clothes))
            {
                if (pOpponents.Has(Customs.Clothes.Covering_Clothes))
                {
                    iHostility++;
                    sNegativeReasons += " (-1) [SOC] prude\n";
                }
            }
            else if (pMine.Has(Customs.Clothes.Covering_Clothes))
            { 
                //if (pOpponents.m_eClothes == Customs.Clothes.Revealing_Clothes)
                //{
                //    iHostility++;
                //    sNegativeReasons += " (-1) [SOC] debaucher\n";
                //}
                //else 
                if (pOpponents.Has(Customs.Clothes.Minimal_Clothes))
                {
                    iHostility += 1;
                    sNegativeReasons += " (-1) [SOC] shameless\n";
                }
            }

            //switch (pMine.m_eAdornments)
            //{
            //    case Customs.Adornments.Lavish_Adornments:
            //        if (pOpponents.m_eAdornments == Customs.Adornments.No_Adornments)
            //        {
            //            iHostility++;
            //            sNegativeReasons += " (-1) [SOC] cheapskate\n";
            //        }
            //        break;
            //    case Customs.Adornments.No_Adornments:
            //        if (pOpponents.m_eAdornments == Customs.Adornments.Some_Adornments)
            //        {
            //            iHostility++;
            //            sNegativeReasons += " (-1) [SOC] extravagant\n";
            //        }
            //        else if (pOpponents.m_eAdornments == Customs.Adornments.Lavish_Adornments)
            //        {
            //            iHostility += 2;
            //            sNegativeReasons += " (-2) [SOC] extravagant\n";
            //        }
            //        break;
            //}

            //if(this == pOpponent)
            //{
            //    iHostility -= 2;
            //    sPositiveReasons += " (+2) self-esteem\n";
            //}

            if (Relations.ContainsKey(pOpponent))
            {
                if (IsFamily(Relations[pOpponent]))
                {
                    if (pMine.Has(Customs.FamilyValues.Praised_Family_Values))
                    {
                        iHostility -= 2;
                        sPositiveReasons += " (+2) [SOC] family ties\n";
                    }
                    else if (pMine.Has(Customs.FamilyValues.Moderate_Family_Values))
                    {
                        iHostility -= 1;
                        sPositiveReasons += " (+1) [SOC] family ties\n";
                    }
                }
                else if (IsKinship(Relations[pOpponent]))
                {
                    if (pMine.Has(Customs.FamilyValues.Praised_Family_Values))
                    {
                        iHostility -= 1;
                        sPositiveReasons += " (+1) [SOC] blood ties\n";
                    }
                }
            }

            //switch (pMine.m_eProgress)
            //{
            //    case Customs.Progressiveness.Traditionalism:
            //        if (pOpponent.m_pProfession.m_cSkills[Skill.Mind] > ProfessionInfo.SkillLevel.Bad)
            //        {
            //            iHostility++;
            //            sNegativeReasons += " (-1) [SOC] schemer\n";
            //        }
            //        break;
            //    case Customs.Progressiveness.Technofetishism:
            //        if (pOpponent.m_pProfession.m_cSkills[Skill.Mind] < ProfessionInfo.SkillLevel.Bad)
            //        {
            //            iHostility++;
            //            sNegativeReasons += " (-1) [SOC] ignoramus\n";
            //        }
            //        break;
            //}

            if (pOpponent.State.MagicLimit > 0)
            {
                if (pMine.Has(Customs.Magic.Magic_Praised))
                {
                    iHostility--;
                    sPositiveReasons += " (+1) [SOC] mage\n";
                }
                else if (pMine.Has(Customs.Magic.Magic_Feared))
                {
                    iHostility++;
                    sNegativeReasons += " (-1) [SOC] mage\n";
                }            
            }

            return iHostility;
        }

        /// <summary>
        /// is a very clever crature ... could have only a few children during whole lifetime, which are mostly females.
        /// </summary>
        /// <returns></returns>
        public string GetFenotypeDescription()
        {
            string sResult = GetFenotypeComparsion(Phenotype.GetBaseHuman(Gender));
            if (sResult.Length == 0)
                sResult = "is just a common " + Age.ToString().ToLower() + " " + (Gender == Gender.Male ? "man" : "woman") + ".";

            if (!sResult.StartsWith("is"))
                sResult = "is a quite common " + Age.ToString().ToLower() + " " + (Gender == Gender.Male ? "man" : "woman") + ". " + sResult;

            return sResult;
        }

        /// <summary>
        /// Возвращает строку, описывающую черты, отличиющие фенотип от заданного, примерный формат:
        /// "is very clever crature ... could have only a few children during whole lifetime, which are mostly females."
        /// </summary>
        /// <returns></returns>
        public string GetFenotypeComparsion(Phenotype.PhensStorage pOriginal)
        {
            if (pOriginal.IsIdentical(m_pPhenotype.m_pValues))
                return "";

            string sResult = "";

            //if (!pOriginal.m_pBrain.IsIdentical(m_pFenotype.m_pBrain))
            //    sResult += "is a " + m_pFenotype.m_pBrain.GetDescription(false) + ".";

            if (!pOriginal.HasIdentical<HeadGenetix>(m_pPhenotype.m_pValues) ||
                !pOriginal.HasIdentical<ArmsGenetix>(m_pPhenotype.m_pValues) ||
                !pOriginal.HasIdentical<LegsGenetix>(m_pPhenotype.m_pValues) ||
                !pOriginal.HasIdentical<WingsGenetix>(m_pPhenotype.m_pValues) ||
                !pOriginal.HasIdentical<TailGenetix>(m_pPhenotype.m_pValues))
            {
                if (sResult != "")
                    sResult += " ";

                sResult += Gender == Gender.Male ? "He has " : "She has ";

                bool bSemicolon = false;
                if (!pOriginal.HasIdentical<HeadGenetix>(m_pPhenotype.m_pValues))
                {
                    sResult += m_pPhenotype.m_pValues.Get<HeadGenetix>().GetDescription();
                    bSemicolon = true;
                }

                if (!pOriginal.HasIdentical<ArmsGenetix>(m_pPhenotype.m_pValues))
                {
                    if (m_pPhenotype.m_pValues.Get<ArmsGenetix>().ArmsCount != ArmsCount.None)
                    {
                        if (bSemicolon)
                            sResult += ", ";
                        sResult += m_pPhenotype.m_pValues.Get<ArmsGenetix>().GetDescription();

                        bSemicolon = true;
                    }
                    else
                    {
                        if (bSemicolon)
                            sResult += ", ";
                        sResult += "no arms";
                    }
                }

                if (!pOriginal.HasIdentical<LegsGenetix>(m_pPhenotype.m_pValues))
                {
                    if (bSemicolon)
                        sResult += ", ";
                    sResult += m_pPhenotype.m_pValues.Get<LegsGenetix>().GetDescription();

                    bSemicolon = true;
                }

                if (!pOriginal.HasIdentical<WingsGenetix>(m_pPhenotype.m_pValues))
                {
                    if (m_pPhenotype.m_pValues.Get<WingsGenetix>().WingsCount != WingsCount.None)
                    {
                        if (bSemicolon)
                            sResult += ", ";
                        sResult += m_pPhenotype.m_pValues.Get<WingsGenetix>().GetDescription();

                        bSemicolon = true;
                    }
                    else
                    {
                        if (bSemicolon)
                            sResult += ", ";
                        sResult += "no wings";
                    }
                }

                if (!pOriginal.HasIdentical<TailGenetix>(m_pPhenotype.m_pValues))
                {
                    if (m_pPhenotype.m_pValues.Get<TailGenetix>().TailLength != TailLength.None)
                    {
                        if (bSemicolon)
                            sResult += " and ";
                        sResult += m_pPhenotype.m_pValues.Get<TailGenetix>().GetDescription();
                    }
                    else
                    {
                        if (bSemicolon)
                            sResult += " and ";
                        sResult += "no tail";
                    }
                }

                sResult += ".";
            }

            if (!pOriginal.HasIdentical<BodyGenetix>(m_pPhenotype.m_pValues) ||
                !pOriginal.HasIdentical<HideGenetix>(m_pPhenotype.m_pValues))
            {
                if (sResult != "")
                    sResult += " ";

                bool bSemicolon = false;
                if (!pOriginal.HasIdentical<BodyGenetix>(m_pPhenotype.m_pValues))
                {
                    sResult += (Gender == Gender.Male ? "He is a " : "She is a ") + m_pPhenotype.m_pValues.Get<BodyGenetix>().GetDescription(Gender, false);
                    bSemicolon = true;
                }
                else
                    sResult += (Gender == Gender.Male ? "He " : "She ") + "has ";

                if (!pOriginal.HasIdentical<HideGenetix>(m_pPhenotype.m_pValues))
                {
                    if (bSemicolon)
                        sResult += " with ";
                    sResult += m_pPhenotype.m_pValues.Get<HideGenetix>().GetDescription();
                }

                sResult += ".";

                sResult += m_pPhenotype.m_pValues.Get<NutritionGenetix>().GetDescription(Gender, true);
            }

            if (!pOriginal.HasIdentical<EyesGenetix>(m_pPhenotype.m_pValues) ||
                !pOriginal.HasIdentical<EarsGenetix>(m_pPhenotype.m_pValues) ||
                !pOriginal.HasIdentical<FaceGenetix>(m_pPhenotype.m_pValues))
            {
                if (sResult != "")
                    sResult += " ";

                bool bSemicolon = false;
                if (!pOriginal.HasIdentical<EyesGenetix>(m_pPhenotype.m_pValues) ||
                    !pOriginal.HasIdentical<EarsGenetix>(m_pPhenotype.m_pValues))
                {
                    sResult += Gender == Gender.Male ? "He has " : "She has ";

                    if (!pOriginal.HasIdentical<EyesGenetix>(m_pPhenotype.m_pValues))
                    {
                        sResult += m_pPhenotype.m_pValues.Get<EyesGenetix>().GetDescription();
                        bSemicolon = true;
                    }

                    if (!pOriginal.HasIdentical<EarsGenetix>(m_pPhenotype.m_pValues))
                    {
                        if (bSemicolon)
                            sResult += " and ";

                        sResult += m_pPhenotype.m_pValues.Get<EarsGenetix>().GetDescription() + " of ";
                    }
                    else
                    {
                        if (bSemicolon && !pOriginal.HasIdentical<FaceGenetix>(m_pPhenotype.m_pValues))
                            sResult += " at ";
                    }
                }
                else
                    sResult += Gender == Gender.Male ? "He has " : "She has ";

                if (!pOriginal.HasIdentical<FaceGenetix>(m_pPhenotype.m_pValues))
                    sResult += m_pPhenotype.m_pValues.Get<FaceGenetix>().GetDescription();
                else
                    if (!pOriginal.HasIdentical<EarsGenetix>(m_pPhenotype.m_pValues))
                        sResult += "the head";// m_pFace.m_eNoseType == NoseType.Normal ? "face" : "muzzle";

                sResult += ".";
            }

            //if (!pOriginal.m_pHairs.m_cHairColors.Contains(m_pFenotype.m_pHairs.m_cHairColors[0]) ||
            //    pOriginal.m_pHairs.m_eHairsType != m_pFenotype.m_pHairs.m_eHairsType ||
            //    (Gender == CPerson.Gender.Male && (pOriginal.m_pHairs.m_eHairsM != m_pFenotype.m_pHairs.m_eHairsM || pOriginal.m_pHairs.m_eBeardM != m_pFenotype.m_pHairs.m_eBeardM)) ||
            //    (Gender == CPerson.Gender.Female && (pOriginal.m_pHairs.m_eHairsF != m_pFenotype.m_pHairs.m_eHairsF || pOriginal.m_pHairs.m_eBeardF != m_pFenotype.m_pHairs.m_eBeardF)))
            {
                if (sResult != "")
                    sResult += " ";

                if (m_pPhenotype.m_pValues.Get<HairsGenetix>().GetDescription() != "")
                    sResult += m_pPhenotype.m_pValues.Get<HairsGenetix>().GetDescription(Gender);
                else
                    sResult = "Both males and females are bald, and have no beard or moustache.";
            }

            if ((pOriginal.Get<LifeCycleGenetix>().DyingRate == DyingRate.High && Age > _Age.Adult) ||
                (pOriginal.Get<LifeCycleGenetix>().DyingRate == DyingRate.Moderate && Age > _Age.Aged))
            {
                if (sResult != "")
                    sResult += " ";

                sResult += (Gender == Gender.Male ? "He" : "She") + " looks very old for " + (Gender == Gender.Male ? "his" : "her") + " age.";
            }

            if ((pOriginal.Get<LifeCycleGenetix>().DyingRate == DyingRate.Low && Age > _Age.Adult))
            {
                if (sResult != "")
                    sResult += " ";

                sResult += (Gender == Gender.Male ? "He" : "She") + " looks surprisingly young for " + (Gender == Gender.Male ? "his" : "her") + " age.";
            }

            if (sResult == "")
                return GetFenotypeComparsion(pOriginal);

            return sResult;
        }

        public string GetCustomsDescription()
        {
            string sResult = "";

            if (Gender == Gender.Male)
            {
                if (Culture.Customs.Has(Customs.GenderPriority.Patriarchy))
                    sResult += "proclaims males supremacy";
                else if (Culture.Customs.Has(Customs.GenderPriority.Matriarchy))
                    sResult += "admits females supremacy";
                //if (m_pCustoms.GenderPriority == Customs.GenderPriority.Genders_equality)
                //    sResult += "admits genders equality";
            }
            else
            {
                if (Culture.Customs.Has(Customs.GenderPriority.Patriarchy))
                    sResult += "admits males supremacy";
                else if (Culture.Customs.Has(Customs.GenderPriority.Matriarchy))
                    sResult += "proclaims females supremacy";
                //if (m_pCustoms.GenderPriority == Customs.GenderPriority.Genders_equality)
                //    sResult += "admits genders equality";
            }

            if (sResult != "" && !Culture.Customs.Has(Customs.MindSet.Balanced_mind))
                sResult += ", ";

            if (Culture.Customs.Has(Customs.MindSet.Emotions))
                sResult += "usually behaves " + (Gender == Gender.Male ? "himself" : "herself") + " very emotinally";
            else if (Culture.Customs.Has(Customs.MindSet.Logic))
                sResult += "usually behaves " + (Gender == Gender.Male ? "himself" : "herself") + " quite unemotional";
            //if (m_pCustoms.m_eMindSet == Customs.MindSet.Balanced_mind)
            //    sResult += "combines emotions and logic";

            if (sResult != "" && !Culture.Customs.Has(Customs.Magic.Magic_Allowed))
                sResult += ", ";

            if (Culture.Customs.Has(Customs.Magic.Magic_Feared))
                sResult += "shows a lot of fear about magic";
            else if (Culture.Customs.Has(Customs.Magic.Magic_Praised))
                sResult += "shows a lot of interest in magic";
            //if (m_pCustoms.m_eMagic == Customs.Magic.Magic_Allowed)
            //    sResult += pOther.m_eMagic == Magic.Magic_Feared ? "has no fear for magic" : "doesn't like magic too much";

            if (sResult != "" && !Culture.Customs.Has(Customs.Science.Moderate_Science))
                sResult += ", ";

            if (Culture.Customs.Has(Customs.Science.Technophobia))
                sResult += "rejects any technological novelties";
            else if (Culture.Customs.Has(Customs.Science.Ingenuity))
                sResult += "likes any technological novelties";
            //if (m_pCustoms.m_eProgress == Customs.Progressiveness.Moderate_Science)
            //    sResult += pOther.m_eProgress == Progressiveness.Traditionalism ? "accepts some regulated progress" : "doesn't like novelties so much";

            if (sResult != "" && (Culture.Customs.Has(Customs.BodyModifications.Body_Modifications_Blamed) || Culture.Customs.MandatoryBodyModifications.Count > 0))
                sResult += ", ";

            if (Culture.Customs.Has(Customs.BodyModifications.Body_Modifications_Blamed))
                sResult += "rejects any form of body modification";
            else
            {
                bool bFirst2 = true;
                foreach (Customs.BodyModificationsTypes eMod in Culture.Customs.MandatoryBodyModifications)
                {
                    if (bFirst2)
                        sResult += "has ";
                    else
                        sResult += " and ";

                    sResult += eMod.ToString().Replace('_', ' ').ToLower();
                    bFirst2 = false;
                }
            }
            //if (m_eBodyModifications == BodyModifications.Body_Modifications_Allowed)
            //    sResult += pOther.m_eBodyModifications == BodyModifications.Body_Modifications_Blamed ? "could have some tatoo or pierceing" : "could have tatoo or pierceing on their choice";

            if (sResult != "")
                sResult += ", ";

            if (Culture.Customs.Has(Customs.Clothes.Covering_Clothes))
                sResult += "wears hiding clothes";
            else if (Culture.Customs.Has(Customs.Clothes.Minimal_Clothes))
                sResult += "wears quite revealing clothes";
            else if (Culture.Customs.Has(Customs.Clothes.Revealing_Clothes))
                sResult += "wears common clothes";

            if (sResult != "" && !Culture.Customs.Has(Customs.Adornments.No_Adornments))
                sResult += " with ";

            //if (m_pCustoms.m_eAdornments == Customs.Adornments.No_Adornments)
            //    sResult += "wears no adornments";
            if (Culture.Customs.Has(Customs.Adornments.Lavish_Adornments))
                sResult += "a lot of adornments";
            else if (Culture.Customs.Has(Customs.Adornments.Some_Adornments))
                sResult += "some adornments";

            if (sResult != "" && !Culture.Customs.Has(Customs.FamilyValues.Moderate_Family_Values))
                sResult += ", ";

            if (Culture.Customs.Has(Customs.FamilyValues.Praised_Family_Values))
                sResult += "have very tight family bonds";
            else if (Culture.Customs.Has(Customs.FamilyValues.No_Family_Values))
                sResult += "have absolutely no family ties";
            //if (m_pCustoms.m_eFamilyValues == Customs.FamilyValues.Moderate_Family_Values)
            //    sResult += "have not so tight family bonds";

            if (sResult != "")
                sResult += ". ";

            sResult += Gender == Gender.Male ? "He is " : "She is ";

            if (Culture.Customs.Has(Customs.Sexuality.Puritan))
                sResult += "not interested in sex";
            else
            {
                if (Culture.Customs.Has(Customs.Sexuality.Lecherous))
                    sResult += "a quite horny ";
                else if (Culture.Customs.Has(Customs.Sexuality.Moderate_sexuality))
                    sResult += "a moderate ";

                if (!Culture.Customs.Has(Customs.Sexuality.Puritan))
                {
                    if (Culture.Customs.Has(Customs.SexualOrientation.Heterosexual))
                        sResult += Gender == Gender.Male ? "stright male" : "strignt female";
                    else if (Culture.Customs.Has(Customs.SexualOrientation.Homosexual))
                        sResult += Gender == Gender.Male ? "gay" : "lesbian";
                    else if (Culture.Customs.Has(Customs.SexualOrientation.Bisexual))
                        sResult += Gender == Gender.Male ? "besexual male" : "besexual female";
                }
            }

            sResult = (Gender == Gender.Male ? "He " : "She ") + sResult + ".";
            //if (m_eMarriage != pOther.m_eMarriage)
            //{
            //    if (sResult != "")
            //        sResult += " and ";

            //    if (m_eMarriage == MarriageType.Monogamy)
            //    {
            //        sResult += pOther.m_eMarriage == MarriageType.Polygamy ? "could have only one" : "could have one";

            //        if (m_eSexRelations == SexualOrientation.Heterosexual)
            //        {
            //            if (GenderPriority == GenderPriority.Patriarchy)
            //            {
            //                sResult += " wife";
            //            }
            //            if (GenderPriority == GenderPriority.Matriarchy)
            //            {
            //                sResult += " husband";
            //            }
            //            if (GenderPriority == GenderPriority.Genders_equality)
            //            {
            //                sResult += " spouse of opposite gender";
            //            }
            //        }
            //        if (m_eSexRelations == SexualOrientation.Homosexual)
            //        {
            //            if (m_eSexuality == Sexuality.Puritan)
            //                sResult += " companion of the same gender";
            //            else
            //                sResult += " spouse of the same gender";
            //        }
            //        if (m_eSexRelations == SexualOrientation.Bisexual)
            //        {
            //            if (m_eSexuality == Sexuality.Puritan)
            //                sResult += " companion of any gender";
            //            else
            //                sResult += " spouse of any gender";
            //        }
            //    }
            //    if (m_eMarriage == MarriageType.Polyamory)
            //    {
            //        sResult += "rejects marriage";

            //        if (m_eSexuality == Sexuality.Puritan)
            //            sResult += ", but may be in platonic relations with persons";
            //        else
            //            sResult += ", but could have a number of lovers";

            //        if (m_eSexRelations == SexualOrientation.Heterosexual)
            //        {
            //            sResult += " of opposite gender";
            //        }
            //        if (m_eSexRelations == SexualOrientation.Homosexual)
            //        {
            //            sResult += " of the same gender";
            //        }
            //        if (m_eSexRelations == SexualOrientation.Bisexual)
            //        {
            //            sResult += " of both genders";
            //        }
            //    }
            //    if (m_eMarriage == MarriageType.Polygamy)
            //    {
            //        if (m_eSexRelations == SexualOrientation.Heterosexual)
            //        {
            //            if (m_eSexRelations == SexualOrientation.Heterosexual)
            //            {
            //                if (GenderPriority == GenderPriority.Patriarchy)
            //                {
            //                    sResult += "could have many wifes";
            //                }
            //                if (GenderPriority == GenderPriority.Matriarchy)
            //                {
            //                    sResult += "could have many husbands";
            //                }
            //                if (GenderPriority == GenderPriority.Genders_equality)
            //                {
            //                    sResult += "lives in big heterosexual families";
            //                }
            //            }
            //        }
            //        if (m_eSexRelations == SexualOrientation.Homosexual)
            //        {
            //            if (m_eSexuality == Sexuality.Puritan)
            //            {
            //                if (GenderPriority == GenderPriority.Patriarchy)
            //                {
            //                    sResult += "lives in large brotherhoods";
            //                }
            //                if (GenderPriority == GenderPriority.Matriarchy)
            //                {
            //                    sResult += "lives in large sisterhoods";
            //                }
            //                if (GenderPriority == GenderPriority.Genders_equality)
            //                {
            //                    sResult += "lives in large communes with persons of the same gender";
            //                }
            //            }
            //            else
            //                sResult += "lives in big homosexual families";
            //        }
            //        if (m_eSexRelations == SexualOrientation.Bisexual)
            //        {
            //            if (m_eSexuality == Sexuality.Puritan)
            //                sResult += "lives in large communes with persons of both genders";
            //            else
            //                sResult += "lives in big bisexual families";
            //        }
            //    }
            //}

            //if (bFirst && GenderPriority == pOther.GenderPriority)
            //    sResult = "";

            return sResult;
        }
        
        public string GetDescription()
        {
            StringBuilder pResult = new StringBuilder();

            string sPersonalFenotype = GetFenotypeDescription();
            pResult.AppendLine(Name + " " + Family + " " + sPersonalFenotype);

            StringBuilder pInjuries = new StringBuilder();
            for (int i=0; i< Injuries.Count; i++)
            {
                Injury eInjury = Injuries[i];
                if (i > 0)
                {
                    if (i < Injuries.Count - 1)
                        pInjuries.Append(", ");
                    else
                        pInjuries.Append(" and ");
                }
                switch (eInjury)
                {
                    case Injury.Scars:
                        pInjuries.Append("remarkable scars");
                        break;
                    case Injury.One_eyed:
                        pInjuries.Append("only one eye");
                        break;
                    case Injury.Lame:
                        pInjuries.Append("lameness");
                        break;
                    case Injury.One_handed:
                        pInjuries.Append("only one hand");
                        break;
                    case Injury.One_legged:
                        pInjuries.Append("only one leg");
                        break;
                    default:
                        pInjuries.Append("unknown injury");
                        break;
                }
            }
            if (pInjuries.Length > 0)
                pResult.AppendLine((Gender == Gender.Male ? "He has " : "She has ") + pInjuries.ToString());

            pResult.AppendLine(GetCustomsDescription());

            StringBuilder pDisorders = new StringBuilder();
            for (int i = 0; i < MentalDisorders.Count; i++)
            {
                MentalDisorder eDisorder = MentalDisorders[i];
                if (i > 0)
                {
                    if (i < MentalDisorders.Count - 1)
                        pDisorders.Append(", ");
                    else
                        pDisorders.Append(" and ");
                }
                switch (eDisorder)
                {
                    case MentalDisorder.Bad_tempered:
                        pDisorders.Append("bad temper");
                        break;
                    case MentalDisorder.Alchholic:
                        pDisorders.Append("alcoholism");
                        break;
                    case MentalDisorder.Gambler:
                        pDisorders.Append("gambling passion");
                        break;
                    case MentalDisorder.Drug_addict:
                        pDisorders.Append("drug abuse");
                        break;
                    case MentalDisorder.Paranoiac:
                        pDisorders.Append("paranoia");
                        break;
                    case MentalDisorder.Psychopath:
                        pDisorders.Append("psychopathy");
                        break;
                    case MentalDisorder.Insane:
                        pDisorders.Append("insanity");
                        break;
                    default:
                        pDisorders.Append("unknown disorder");
                        break;
                }
            }
            if (pDisorders.Length > 0)
                pResult.AppendLine((Gender == Gender.Male ? "His" : "Her") + " most remarkable " +
                    (MentalDisorders.Count > 1 ? "flaws are " : "flaw is ") + pDisorders.ToString() + ".");


            if (Profession != null)
            {
                pResult.AppendFormat("{0} is a {1}, one of {2}s estate.\n{0} is ", Gender == Gender.Male ? "He" : "She", Gender == Gender.Male ? Profession.NameMasculine : Profession.NameFeminine, Estate.Name.ToLower());
                StringBuilder pSkills = new StringBuilder();
                switch (Skills[Skill.Body])
                {
                    case ProfessionInfo.SkillLevel.None:
                        pSkills.Append("quite weak");
                        break;
                    case ProfessionInfo.SkillLevel.Good:
                        pSkills.Append("quite strong");
                        break;
                    case ProfessionInfo.SkillLevel.Excellent:
                        pSkills.Append("very strong");
                        break;
                }
                if (pSkills.Length > 0 && Skills[Skill.Mind] != ProfessionInfo.SkillLevel.Bad)
                {
                    if (Skills[Skill.Charm] != ProfessionInfo.SkillLevel.Bad)
                        pSkills.Append(", ");
                    else
                        pSkills.Append(" and ");
                }
                switch (Skills[Skill.Mind])
                {
                    case ProfessionInfo.SkillLevel.None:
                        pSkills.Append("quite dumb");
                        break;
                    case ProfessionInfo.SkillLevel.Good:
                        pSkills.Append("quite clever");
                        break;
                    case ProfessionInfo.SkillLevel.Excellent:
                        pSkills.Append("very clever");
                        break;
                }
                if (pSkills.Length > 0 && Skills[Skill.Charm] != ProfessionInfo.SkillLevel.Bad)
                    pSkills.Append(" and ");
                switch (Skills[Skill.Charm])
                {
                    case ProfessionInfo.SkillLevel.None:
                        pSkills.Append("quite rude");
                        break;
                    case ProfessionInfo.SkillLevel.Good:
                        pSkills.Append("quite charismatic");
                        break;
                    case ProfessionInfo.SkillLevel.Excellent:
                        pSkills.Append("very charismatic");
                        break;
                }
                if (pSkills.Length == 0)
                    pSkills.Append("just ordinal");

                pResult.AppendFormat("{0} {1}.\n", pSkills.ToString(), Gender == Gender.Male ? "man" : "woman");
            }

            return pResult.ToString();
        }

        /// <summary>
        /// Отношение к другому персонажу с учётом взаимного социального положения и возможности контактов.
        /// Диапазон - примерно от -20к до +20к
        /// </summary>
        /// <param name="pOpponent"></param>
        /// <returns></returns>
        public float GetAttitudeTo(Person pOpponent)
        {
            float fProximity = GetProximity(this, pOpponent);
            float fHostility = -CalcNormalizedHostility(pOpponent, false);

            return fHostility * fProximity * GetInfluence(true);
        }

        public float GetFactionAttitudeTo(Person pOpponent)
        {
            float fProximity = GetProximity(this, pOpponent);
            float fHostility = -CalcNormalizedHostility(pOpponent, false);

            return fHostility * fProximity * GetFactionInfluence(true);
        }

        public static float GetAttraction(Person pPerson1, Person pPerson2)
        {
            //если взаимные отношения откладывать по осям x и y, то напряжение - это проекция полученной точки на ось x=y.
            //sin(45) = 0.70710...
            return (float)(0.7 * pPerson1.GetAttitudeTo(pPerson2) + 0.7 * pPerson2.GetAttitudeTo(pPerson1));
        }

        public static float GetFactionAttraction(Person pPerson1, Person pPerson2)
        {
            //если взаимные отношения откладывать по осям x и y, то напряжение - это проекция полученной точки на ось x=y.
            //sin(45) = 0.70710...
            return (float)(0.7 * pPerson1.GetFactionAttitudeTo(pPerson2) + 0.7 * pPerson2.GetFactionAttitudeTo(pPerson1));
        }

        public Person GetNemezis()
        {
            float fMinAttraction = float.MaxValue;
            Person pNemezis = null;
            foreach (var pRelation in Relations)
            {
                if (pRelation.Value == Relation.FormerLifeOf ||
                    pRelation.Value == Relation.PresentLifeOf)
                    continue;

                float fAttraction = GetAttraction(this, pRelation.Key);
                if (fAttraction < fMinAttraction)
                {
                    fMinAttraction = fAttraction;
                    pNemezis = pRelation.Key;
                }
            }

            return pNemezis;
        }

        public float CalcNormalizedHostility(Person pOpponent, bool bFirstLook)
        {
            int iMinHostility = int.MaxValue;
            int iMaxHostility = int.MinValue;
            int iActualHostility = 0;

            Dictionary<Person, int> cHostility = new Dictionary<Person, int>();

            foreach (var pRelation in Relations)
            {
                int iHostility = CalcHostility(pRelation.Key, bFirstLook);

                if (iHostility < iMinHostility)
                    iMinHostility = iHostility;
                if (iHostility > iMaxHostility)
                    iMaxHostility = iHostility;

                if (pRelation.Key == pOpponent)
                    iActualHostility = iHostility;

                cHostility[pRelation.Key] = iHostility;
            }

            if (!Relations.ContainsKey(pOpponent))
            {
                int iHostility = CalcHostility(pOpponent, bFirstLook);

                if (iHostility < iMinHostility)
                    iMinHostility = iHostility;
                if (iHostility > iMaxHostility)
                    iMaxHostility = iHostility;

                cHostility[pOpponent] = iHostility;
            }

            int iMedian = Median.GetMedian(cHostility.Values.ToList());

            float fHostility = 0;

            if (iActualHostility < iMedian)
            {
                int iHostilityScale = iMedian - iMinHostility;
                if (iHostilityScale > 0)
                    iActualHostility -= iMedian;
                else
                    iHostilityScale = Math.Abs(iActualHostility);

                if (iHostilityScale == 0)
                    return 0;

                fHostility =  (float)iActualHostility / iHostilityScale;
            }
            else 
            {
                int iHostilityScale = iMaxHostility - iMedian;
                if (iHostilityScale > 0)
                    iActualHostility -= iMedian;
                else
                    iHostilityScale = Math.Abs(iActualHostility);

                if (iHostilityScale == 0)
                    return 0;

                fHostility = (float)iActualHostility / iHostilityScale;
            }

            float fProximity = GetProximity(this, pOpponent);

            int iMyInfluence = GetInfluence(false);
            int iOpponentInfluence = pOpponent.GetInfluence(false);

            float fInfluenceDiff = (float)iMyInfluence / iOpponentInfluence;
            if (iMyInfluence > iOpponentInfluence)
                fInfluenceDiff = (float)iOpponentInfluence / iMyInfluence;

            if (fHostility > 0)
                fHostility = (float)(Culture.GetTrait(Trait.Fanaticism) * fHostility * fInfluenceDiff * fProximity);
            else if (fHostility < 0)
                fHostility = (float)((2.0f - Culture.GetTrait(Trait.Fanaticism)) * fHostility * fInfluenceDiff * fProximity);

            return fHostility;
        }

        /// <summary>
        /// Негативное отношение к другому персонажу.
        /// </summary>
        /// <param name="pOpponent"></param>
        /// <returns></returns>
        public int CalcHostility(Person pOpponent, bool bFirstLook)
        {
            string sReasons;
            return CalcHostility(pOpponent, out sReasons, bFirstLook);
        }

        /// <summary>
        /// Негативное отношение к другому персонажу.
        /// </summary>
        /// <param name="pOpponent"></param>
        /// <returns></returns>
        public int CalcHostility(Person pOpponent, out string sReasons, bool bFirstLook)
        {
            if (pOpponent == null)
            {
                sReasons = "null pOpponent";
                return 0;
            }

            int iHostility = 0;

            string sPositiveReasons = "";
            string sNegativeReasons = "";

            iHostility += GetFenotypeDifference(pOpponent, ref sPositiveReasons, ref sNegativeReasons);

            if (iHostility == 0 && Age != _Age.Old)
            {
                if(Culture.Customs.Has(Customs.Sexuality.Lecherous) ||
                    (Culture.Customs.Has(Customs.Sexuality.Moderate_sexuality) && 
                     (!Relations.ContainsKey(pOpponent) || !IsBloodKinship(Relations[pOpponent]))))
                {
                    if (pOpponent == this ||
                        Culture.Customs.Has(Customs.SexualOrientation.Bisexual) ||
                        (Culture.Customs.Has(Customs.SexualOrientation.Heterosexual) && pOpponent.Gender != Gender) ||
                        (Culture.Customs.Has(Customs.SexualOrientation.Homosexual) && pOpponent.Gender == Gender))
                    {
                        int iBeauty = 0;
                        if (pOpponent.Age == _Age.Young)
                            iBeauty = 2;
                        else if (pOpponent.Age == _Age.Adult && Age != _Age.Young)
                            iBeauty = 1;

                        if(pOpponent.Look == Appearance.Unattractive)
                            iBeauty /= 2;

                        if(pOpponent.Look == Appearance.Handsome)
                            iBeauty *= 2;

                        if (Culture.Customs.Has(Customs.Sexuality.Lecherous))
                            iBeauty += 2;

                        if(iBeauty > 0)
                        {
                            iHostility -= iBeauty;
                            sPositiveReasons += string.Format(" (+{0}) [APP] sexappeal\n", iBeauty);
                        }
                    }
                }
            }

            foreach (Injury eInjury in pOpponent.Injuries)
            {
                if (IsMinorInjury(eInjury))
                {
                    //iHostility++;
                    //sNegativeReasons += " (-1) [APP] " + eInjury.ToString().Replace('_', '-').ToLower() + "\n";
                }
                else
                {
                    iHostility += 1;
                    sNegativeReasons += " (-1) [APP] " + eInjury.ToString().Replace('_', '-').ToLower() + "\n";
                }
            }

            foreach (MentalDisorder eDisorder in pOpponent.MentalDisorders)
            {
                if (IsMinorDisorder(eDisorder))
                {
                    //iHostility++;
                    //sNegativeReasons += " (-1) [PSI] " + eDisorder.ToString().Replace('_', '-').ToLower() + "\n";
                }
                else
                {
                    iHostility += 1;
                    sNegativeReasons += " (-1) [PSI] " + eDisorder.ToString().Replace('_', '-').ToLower() + "\n";
                }
            }

            if (pOpponent.Skills[Skill.Charm] > ProfessionInfo.SkillLevel.Good)// m_cSkills[Skill.Charm] + 1)
            {
                iHostility -= 2;
                sPositiveReasons += " (+2) [PSI] very charismatic\n";
            }
            else if (pOpponent.Skills[Skill.Charm] > ProfessionInfo.SkillLevel.Bad)//m_cSkills[Skill.Charm])
            {
                iHostility--;
                sPositiveReasons += " (+1) [PSI] charismatic\n";
            }


            if (Nation != pOpponent.Nation)
            {
                //iHostility++;
                //sNegativeReasons += " (-1) " + pOpponent.m_pNation.ToString() + "\n";

                if (Nation.Race.Language != pOpponent.Nation.Race.Language)
                {
                    iHostility++;
                    sNegativeReasons += " (-1) [SOC] different language\n";
                }
            }
            //else
            //{
            //    iHostility--;
            //    sPositiveReasons += " (+1) same nation\n";
            //}

            iHostility += GetCustomsDifference(pOpponent, ref sPositiveReasons, ref sNegativeReasons);

            if (!bFirstLook)
            {
                //if (GetInfluence(true) * 2 < pOpponent.GetInfluence(true))
                //{
                //    iHostility += 4;
                //    sNegativeReasons += " (-4) envy for power\n";
                //}
                //else if (GetInfluence(true) < pOpponent.GetInfluence(true))
                //{
                //    iHostility += 2;
                //    sNegativeReasons += " (-2) envy for power\n";
                //}

                if (pOpponent.State.InfrastructureLevel > State.InfrastructureLevel + 1)
                {
                    iHostility += 2;//= pOpponent.m_iLifeLevel - m_iLifeLevel;
                    sNegativeReasons += string.Format(" (-{0}) [SOC] envy for civilization\n", 2);//pOpponent.m_iLifeLevel - m_iLifeLevel);
                }
                else
                {
                    if (pOpponent.State.InfrastructureLevel < State.InfrastructureLevel - 1)
                    {
                        iHostility++;//= m_iLifeLevel - pOpponent.m_iLifeLevel;
                        sNegativeReasons += string.Format(" (-{0}) [SOC] savage\n", 1);//m_iLifeLevel - pOpponent.m_iLifeLevel);
                    }
                }

                if (IsSlave())
                {
                    if (pOpponent.State.SocialEquality == 0 && pOpponent.Estate.IsMiddleUp())
                    {
                        iHostility += 2;
                        sNegativeReasons += " (-2) [SOC] slaver\n";
                    }
                }
                else if (Estate.Rank != pOpponent.Estate.Rank)
                {
                    if (pOpponent.Estate.IsOutlaw())
                    {
                        iHostility += 2;
                        sNegativeReasons += " (-2) [SOC] outlaw\n";
                    }
                    else
                    {
                        iHostility++;
                        sNegativeReasons += " (-1) [SOC] different social status\n";
                    }
                }

                if (State.SocialEquality == 0 && Estate.IsMiddleUp())
                {
                    if (pOpponent.IsSlave())
                    {
                        iHostility += 2;
                        sNegativeReasons += " (-2) [SOC] slave\n";
                    }
                }
                if (State.SocialEquality > 1)
                {
                    if (pOpponent.State.SocialEquality == 0 && pOpponent.Estate.IsMiddleUp())
                    {
                        iHostility++;
                        sNegativeReasons += " (-1) [SOC] slaver\n";
                    }
                }

                float iCultureDifference = Culture.Mentality.GetDifference(pOpponent.Culture.Mentality, Culture.ProgressLevel, pOpponent.Culture.ProgressLevel);
                if (iCultureDifference == -1 || pOpponent == this)
                {
                    //iHostility -= 3;
                    //sPositiveReasons += " (+3) same cultural values\n";
                }
                else if (iCultureDifference < -0.75)
                {
                    iHostility -= 1;
                    sPositiveReasons += " (+1) [SOC] close cultural values\n";
                }
                else if (iCultureDifference < -0.5)
                {
                    iHostility++;
                    sNegativeReasons += " (-1) [SOC] mismatching cultural values\n";
                }
                else if (iCultureDifference < 0)
                {
                    iHostility += 3;
                    sNegativeReasons += " (-3) [SOC] different cultural values\n";
                }
                else
                {
                    iHostility += 6;
                    sNegativeReasons += " (-6) [SOC] incompatible cultural values\n";
                }

                if (pOpponent != this && !Relations.ContainsKey(pOpponent))
                {
                    iHostility++;
                    sNegativeReasons += " (-1) [SOC] stranger\n";
                }

                //if (m_pHomeLocation.Owner != pOpponent.m_pHomeLocation.Owner && (!m_cRelations.ContainsKey(pOpponent) || !IsKinship(m_cRelations[pOpponent])))
                //{
                //    iHostility++;
                //    sNegativeReasons += " (-1) [SOC] foreigner\n";
                //}

                //if (pOpponent != this)
                //{
                //    int iSelfHate = CalcHostility(this, true);
                //    if (iSelfHate > 0)
                //    {
                //        iSelfHate = 1;
                //        iHostility += iSelfHate;
                //        sNegativeReasons += string.Format(" ({0:+#;-#;0}) [PSI] self-esteem\n", -iSelfHate);
                //    }
                //    if (iSelfHate < 0)
                //    {
                //        iSelfHate = -1;
                //        iHostility += iSelfHate;
                //        sPositiveReasons += string.Format(" ({0:+#;-#;0}) [PSI] self-esteem\n", -iSelfHate);
                //    }
                //}
            }

            if (sPositiveReasons == "")
                sPositiveReasons = " - none\n";

            if (sNegativeReasons == "")
                sNegativeReasons = " - none\n";

            sReasons = "\nGood:\n" + sPositiveReasons + "Bad:\n" + sNegativeReasons + "----\n";

            string sProximity;
            float fProximity = GetProximity(this, pOpponent, out sProximity);

            int iMyInfluence = GetInfluence(false);
            int iOpponentInfluence = pOpponent.GetInfluence(false);

            float fInfluenceDiff = (float)iMyInfluence / iOpponentInfluence;
            if (iMyInfluence > iOpponentInfluence)
                fInfluenceDiff = (float)iOpponentInfluence / iMyInfluence;

            if (iHostility > 0)
            {
                //iHostility = (int)(100.0 * m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] * iHostility * fInfluenceDiff * fProximity + 0.25);
                //iHostility = (int)(m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] * iHostility * Math.Min(pOpponent.GetInfluence(false), GetInfluence(false) * 2) / GetInfluence(false) + 0.25);
                sReasons += string.Format("Fanaticism \t(x{0}%)\n", (int)(Culture.GetTrait(Trait.Fanaticism) * 100));

                sReasons += string.Format("Social distance \t(x{0}%)\n", (int)(fInfluenceDiff * 100));
                //iHostility = (int)(m_pHome.Owner.m_pCulture.MentalityValues[Mentality.Agression][m_pHome.Owner.m_iCultureLevel] * iHostility + 0.25);
                //sReasons += string.Format("Agression \t(x{0}%)\n", (int)(m_pHome.Owner.m_pCulture.MentalityValues[Mentality.Agression][m_pHome.Owner.m_iCultureLevel] * 100));

                //sReasons += string.Format("Social weight \t(x{0}%)\n", (int)(Math.Min(pOpponent.GetInfluence(false), GetInfluence(false) * 2) * 100 / GetInfluence(false)));
                if (pOpponent != this)
                {
                    sReasons += string.Format("Proximity \t(x{0}%)\n", (int)(fProximity * 100));
                    sReasons += sProximity + "\n";
                }

                //if (iHostility == 0)
                //    iHostility = 1;
            }
            else if (iHostility < 0)
            {
                //iHostility = (int)(100.0 * (2.0f - m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel]) * iHostility * fInfluenceDiff * fProximity - 0.25);
                //iHostility = (int)((2.0f - m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel]) * iHostility * Math.Min(pOpponent.GetInfluence(false), GetInfluence(false) * 2) / GetInfluence(false) - 0.25);
                sReasons += string.Format("Tolerance \t(x{0}%)\n", (int)((2.0f - Culture.GetTrait(Trait.Fanaticism)) * 100));

                sReasons += string.Format("Social distance \t(x{0}%)\n", (int)(fInfluenceDiff * 100));
                //iHostility = (int)((2.0f - m_pHome.Owner.m_pCulture.MentalityValues[Mentality.Agression][m_pHome.Owner.m_iCultureLevel]) * iHostility - 0.25);
                //sReasons += string.Format("Amiability \t(x{0}%)\n", (int)((2.0f - m_pHome.Owner.m_pCulture.MentalityValues[Mentality.Agression][m_pHome.Owner.m_iCultureLevel]) * 100));

                //sReasons += string.Format("Social weight \t(x{0}%)\n", (int)(Math.Min(pOpponent.GetInfluence(false), GetInfluence(false) * 2) * 100 / GetInfluence(false)));
                if (pOpponent != this)
                {
                    sReasons += string.Format("Proximity \t(x{0}%)\n", (int)(fProximity * 100));
                    sReasons += sProximity + "\n";
                }

                //if (iHostility == 0)
                //    iHostility = -1;
            }

            //if (fContact < fBorder / 2)
            //    iHostility = iHostility / 2;

            sReasons += string.Format("----\nTotal \t({0:+#;-#;0})\n", -iHostility);
            return iHostility*100;
        }

        /// <summary>
        /// Альтер эго обязательно умеет всё то же, что и основная личность, а при смене судьбы ничего не забывается.
        /// </summary>
        /// <param name="pRelative"></param>
        private void FixSkills(Person pRelative)
        {
            if (!Relations.ContainsKey(pRelative))
                return;

            if (Relations[pRelative] == Relation.AlterEgo ||
                Relations[pRelative] == Relation.FormerLifeOf ||
                Relations[pRelative] == Relation.PresentLifeOf)
            {
                foreach (Skill eSkill in Enum.GetValues(typeof(Skill)))
                {
                    if (Relations[pRelative] != Relation.FormerLifeOf &&
                        pRelative.Skills[eSkill] > Skills[eSkill])
                        Skills[eSkill] = pRelative.Skills[eSkill];

                    if (Relations[pRelative] != Relation.PresentLifeOf &&
                        Skills[eSkill] > pRelative.Skills[eSkill])
                        pRelative.Skills[eSkill] = Skills[eSkill];
                }
            }
        }

        /// <summary>
        /// Альтер эго имеет то же тело, что и основная личность.
        /// </summary>
        /// <param name="pRelative"></param>
        public void FixHealth()
        {
            foreach (var pRelation in Relations)
            {
                if (pRelation.Value == Relation.AlterEgo ||
                    pRelation.Value == Relation.PresentLifeOf)
                {
                    foreach (Customs.BodyModificationsTypes eMod in pRelation.Key.Culture.Customs.MandatoryBodyModifications)
                        if (!Culture.Customs.MandatoryBodyModifications.Contains(eMod))
                            Culture.Customs.MandatoryBodyModifications.Add(eMod);

                    foreach (Injury eInjury in pRelation.Key.Injuries)
                    {
                        if (!Injuries.Contains(eInjury))
                        {
                            if (IsMinorInjury(eInjury))
                            {
                                if (pRelation.Value == Relation.PresentLifeOf || Rnd.OneChanceFrom(2))
                                    Injuries.Add(eInjury);
                            }
                            else
                            {
                                if (pRelation.Value == Relation.PresentLifeOf || Rnd.Chances(4, 5))
                                    Injuries.Add(eInjury);
                            }
                        }
                    }

                    foreach (MentalDisorder eDisorder in pRelation.Key.MentalDisorders)
                    {
                        if (!MentalDisorders.Contains(eDisorder))
                        {
                            if (IsMinorDisorder(eDisorder))
                            {
                                if (pRelation.Value == Relation.PresentLifeOf || Rnd.OneChanceFrom(2))
                                    MentalDisorders.Add(eDisorder);
                            }
                            else
                            {
                                if (pRelation.Value == Relation.PresentLifeOf || Rnd.Chances(4, 5))
                                    MentalDisorders.Add(eDisorder);
                            }
                        }
                    }
                }
             }
        }

        /// <summary>
        /// Создает связь между двумя персонажами (кроме близкородственных (дети-родители, братья/сёстры))
        /// </summary>
        /// <param name="pPretender"></param>
        public void AddRelation(Person pPretender)
        {
            if (Relations.ContainsKey(pPretender) || pPretender == this)
                return;

            Relation eRelation = PossibleRelation(pPretender);
            Relations[pPretender] = eRelation;
            pPretender.Relations[this] = eRelation;
            switch (eRelation)
            {
                case Relation.Patron:
                    pPretender.Relations[this] = Relation.Agent;
                    break;
                case Relation.Agent:
                    pPretender.Relations[this] = Relation.Patron;
                    break;
                case Relation.FormerLifeOf:
                    pPretender.Relations[this] = Relation.PresentLifeOf;
                    break;
                case Relation.PresentLifeOf:
                    pPretender.Relations[this] = Relation.FormerLifeOf;
                    break;
                case Relation.BastardOf:
                    pPretender.Relations[this] = Relation.ParentOf;
                    break;
            }
            if (eRelation == Relation.PresentLifeOf ||
                eRelation == Relation.FormerLifeOf)
            {
                if (!Rnd.OneChanceFrom(5))
                {
                    Name = pPretender.Name;
                    SetShortName();
                }
            }
            FixSkills(pPretender);
        }

        public bool IsSlave()
        {
            return Estate.Rank == Estate.SocialRank.Lowlifes && State.SocialEquality == 0;
        }

        public bool IsSerf()
        {
            return Estate.Rank == Estate.SocialRank.Lowlifes && State.SocialEquality == 1;
        }

        /// <summary>
        /// Вычисляет степень общего между двумя персонажами.
        /// Чем больше ощего - тем больше возвращаемое значение.
        /// Диапазон возвращаемого значения 0..11 (возможно, устарело!)
        /// </summary>
        /// <param name="pPerson1"></param>
        /// <param name="pPerson2"></param>
        /// <returns></returns>
        public static float GetProximity(Person pPerson1, Person pPerson2)
        {
            string sResults;
            return GetProximity(pPerson1, pPerson2, out sResults);
        }

        /// <summary>
        /// Вычисляет степень общего между двумя персонажами.
        /// Чем больше ощего - тем больше возвращаемое значение.
        /// Диапазон возвращаемого значения 0..1
        /// </summary>
        /// <param name="pPerson1"></param>
        /// <param name="pPerson2"></param>
        /// <returns></returns>
        public static float GetProximity(Person pPerson1, Person pPerson2, out string sResults)
        {
            int iConnections = 0;// 10 - pPerson1.m_cRelations.Count / 2 - pPerson2.m_cRelations.Count / 2;
            int iConnectionsMax = 0;

            StringBuilder pResult = new StringBuilder();

            Relation eRel;
            if (pPerson1.Relations.TryGetValue(pPerson2, out eRel))
            {
                if (IsFamily(eRel))
                {
                    iConnections += 2;
                    pResult.AppendLine(" (+2) Family");
                }
            }
            iConnectionsMax += 2;

            //Людей объединяет общий дом...
            if (pPerson1.Building == pPerson2.Building)
            {
                iConnections++;
                pResult.AppendLine(" (+1) Same home");
            }
            iConnectionsMax++;

            //...общее место жительства
            if (pPerson1.HomeLocation == pPerson2.HomeLocation)
            {
                iConnections += 3;
                pResult.AppendLine(" (+3) Same city");
            }
            iConnectionsMax += 3;

            //...принадлежность к одной нации
            if (pPerson1.Nation == pPerson2.Nation)
            {
                iConnections++;
                pResult.AppendLine(" (+1) Same nation");
            }
            iConnectionsMax++;

            //...принадлежность к одной расе
            if (pPerson1.Nation.Race == pPerson2.Nation.Race)
            {
                iConnections++;
                pResult.AppendLine(" (+1) Same race");
            }
            iConnectionsMax++;

            //...общий язык
            if (pPerson1.Nation.Race.Language == pPerson2.Nation.Race.Language)
            {
                iConnections++;
                pResult.AppendLine(" (+1) Same language");
            }
            iConnectionsMax++;

            if (pPerson1.Profession == pPerson2.Profession)
            {
                iConnections++;
                pResult.AppendLine(" (+1) Same profession");
            }
            iConnectionsMax++;

            //if (pPerson1.m_pProfession.m_bMaster && pPerson2.m_pProfession.m_bMaster)
            //    iConnections++;

            if (pPerson1.Estate == pPerson2.Estate)
            {
                iConnections++;
                pResult.AppendLine(" (+1) Same estate");
            }
            iConnectionsMax++;


            //if (pPerson1.m_eEstate == CSocialOrder.Estate.Outlaw &&
            //    pPerson2.m_eEstate == CSocialOrder.Estate.Outlaw)
            //    iConnections++;

            if (pPerson1 != pPerson2)
            {
                if (pPerson1.IsSlave() || pPerson2.IsSlave())
                {
                    int iDiff = 2 * (1 + Math.Abs(pPerson1.Estate.Rank - pPerson2.Estate.Rank));
                    iConnections -= iDiff;
                    if(iDiff != 0)
                        pResult.AppendLine(" (-" + iDiff.ToString() + ") Social status: Slave");
                }

                if (pPerson1.IsSerf() || pPerson2.IsSerf())
                {
                    int iDiff = 1 + Math.Abs(pPerson1.Estate.Rank - pPerson2.Estate.Rank);
                    iConnections -= iDiff;
                    if (iDiff != 0)
                        pResult.AppendLine(" (-" + iDiff.ToString() + ") Social status: Serf");
                }

                int iDiff2 = (int)Math.Sqrt((int)pPerson1.Skills[Skill.Mind] * (int)pPerson2.Skills[Skill.Mind]);
                iConnections += iDiff2;
                if (iDiff2 != 0)
                    pResult.AppendLine(" (+" + iDiff2.ToString() + ") Intelligence");
            }
            iConnectionsMax += 2;

            if (pPerson1.HomeLocation.OwnerState == pPerson2.HomeLocation.OwnerState)
            {
                //если они живут в одном государстве, то для низших слоёв общества это имеет меньшее значение, чем для всех остальных
                if (pPerson1.Estate.Rank == Estate.SocialRank.Lowlifes ||
                    pPerson2.Estate.Rank == Estate.SocialRank.Lowlifes)
                {
                    if (!pPerson1.IsSlave() && !pPerson2.IsSlave())
                    {
                        iConnections++;
                        pResult.AppendLine(" (+1) Same state (low estate)");
                    }
                }
                else
                {
                    iConnections += 2;
                    pResult.AppendLine(" (+2) Same state");
                }
            }
            //если они живут в разных государствах, то для низших слоёв общества всё остальное уже не имеет никакого значения.
            else if (pPerson1.Estate.Rank != Estate.SocialRank.Lowlifes &&
                    pPerson2.Estate.Rank != Estate.SocialRank.Lowlifes)
            {
                int iAdd = 0;

                //Зарубежные связи могут иметь разбойники...
                if (pPerson1.Estate.IsOutlaw() ||
                    pPerson2.Estate.IsOutlaw())
                {
                    //разбойники не имеют национальности
                    if (pPerson1.Estate.IsOutlaw() &&
                        pPerson2.Estate.IsOutlaw())
                    {
                        iAdd = Math.Max(1, iAdd);
                        pResult.AppendLine(" (+" + iAdd.ToString() + ") Interstate outlaws");
                    }
                    //лидеры разбойников скорее имеют зарубежные связи, чем рядовые слены банд
                    if (pPerson1.Estate.IsOutlaw() &&
                        pPerson1.Profession.IsMaster &&
                        pPerson2.Estate.IsOutlaw() &&
                        pPerson2.Profession.IsMaster)
                    {
                        iAdd = Math.Max(2, iAdd);
                        pResult.AppendLine(" (+" + iAdd.ToString() + ") Interstate outlaw leaders");
                    }
                }

                //...так же и средний класс
                if (pPerson1.Estate.Rank == Estate.SocialRank.Commoners ||
                    pPerson2.Estate.Rank == Estate.SocialRank.Commoners)
                {
                    //но - только если один из них живёт в столице, да и то связи эти слабые - как другие города для люмпенов
                    if (pPerson1.HomeLocation == pPerson1.HomeLocation.OwnerState.m_pMethropoly.AdministrativeCenter ||
                        pPerson2.HomeLocation == pPerson2.HomeLocation.OwnerState.m_pMethropoly.AdministrativeCenter)
                    {
                        iAdd = Math.Max(1, iAdd);
                        pResult.AppendLine(" (+" + iAdd.ToString() + ") Interstate capital residents");
                    }
                }

                //...и конечно же элита
                if (pPerson1.Estate.IsElite() ||
                    pPerson2.Estate.IsElite())
                {
                    //жители столиц имеют более сильные связи, как если бы они жили в одном государстве
                    if (pPerson1.HomeLocation == pPerson1.HomeLocation.OwnerState.m_pMethropoly.AdministrativeCenter ||
                        pPerson2.HomeLocation == pPerson2.HomeLocation.OwnerState.m_pMethropoly.AdministrativeCenter)
                    {
                        iAdd = Math.Max(2, iAdd);
                        pResult.AppendLine(" (+" + iAdd.ToString() + ") Interstate capital residents (elite)");
                    }
                    else
                    {
                        iAdd = Math.Max(1, iAdd);
                        pResult.AppendLine(" (+" + iAdd.ToString() + ") Interstate relations (elite)");
                    }
                }

                iConnections += iAdd;
            }
            iConnectionsMax += 2;

            //iConnections = iConnections * 20 / (1 + pPerson1.m_cRelations.Count + pPerson2.m_cRelations.Count);

            if (iConnections < 1)
            {
                pResult.AppendLine(" (+" + (1 - iConnections).ToString() + ") Balancing coefficient");
                iConnections = 1;
            }

            sResults = pResult.ToString();
            return (float)iConnections / iConnectionsMax;
        }

        /// <summary>
        /// Волновым фронтом ищет всех хоть как-то связанных родственными узами (IsKinship())
        /// </summary>
        /// <returns></returns>
        public int GetFamilySize()
        {
            List<Person> cProcessed = new List<Person>();
            List<Person> cFamily = new List<Person>();
            cFamily.Add(this);
            cProcessed.Add(this);

            List<Person> cCurrentWave = new List<Person>();
            List<Person> cNewWave = new List<Person>();
            cCurrentWave.Add(this);
            do
            {
                cNewWave.Clear();
                foreach (Person pActive in cCurrentWave)
                {
                    foreach (var pRelative in pActive.Relations)
                    {
                        if (IsKinship(pRelative.Value) || cProcessed.Contains(pRelative.Key))
                            continue;

                        cNewWave.Add(pRelative.Key);
                        cProcessed.Add(pRelative.Key);
                    }
                }
                cFamily.AddRange(cNewWave);
                cCurrentWave.Clear();
                cCurrentWave.AddRange(cNewWave);
            }
            while (cCurrentWave.Count > 0);

            return cFamily.Count;
        }

        /// <summary>
        /// Ищет всех братьев и сестёр, даже если у нас нет прямой ссылки друг на друга, но есть общая ссылка на кого-то третьего...
        /// </summary>
        /// <returns></returns>
        public Person[] GetSiblings()
        {
            List<Person> cProcessed = new List<Person>();
            List<Person> cSiblings = new List<Person>();
            cSiblings.Add(this);
            cProcessed.Add(this);

            List<Person> cCurrentWave = new List<Person>();
            List<Person> cNewWave = new List<Person>();
            cCurrentWave.Add(this);
            do
            {
                cNewWave.Clear();
                foreach (Person pActive in cCurrentWave)
                {
                    foreach (var pRelative in pActive.Relations)
                    {
                        if (pRelative.Value != Relation.Sibling || cProcessed.Contains(pRelative.Key))
                            continue;

                        cNewWave.Add(pRelative.Key);
                        cProcessed.Add(pRelative.Key);
                    }
                }
                cSiblings.AddRange(cNewWave);
                cCurrentWave.Clear();
                cCurrentWave.AddRange(cNewWave);
            }
            while (cCurrentWave.Count > 0);

            return cSiblings.ToArray();
        }

        /// <summary>
        /// Возвращает ссылку на родителя указанного персонажа, если таковой имеется
        /// </summary>
        /// <returns></returns>
        public Person GetParent()
        {
            if (Relations.ContainsValue(Relation.ChildOf))
            {
                foreach (var pRelative in Relations)
                    if (pRelative.Value == Relation.ChildOf)
                        return pRelative.Key;
            }

            Person[] cSiblings = GetSiblings();
            foreach (Person pSibling in cSiblings)
            {
                if (pSibling.Relations.ContainsValue(Relation.ChildOf))
                {
                    foreach (var pRelative in pSibling.Relations)
                        if (pRelative.Value == Relation.ChildOf)
                            return pRelative.Key;
                }
            }

            return null;
        }

        /// <summary>
        /// Может ли этот персонаж стать чьим-то супругом?
        /// Возвращает ссылку на самого себя, если может,
        /// null если не может или ссылку на другого персонажа, если данный персонаж является младшим членом в полигамной семье
        /// </summary>
        /// <returns></returns>
        public Person GetSpouse()
        {
            if (Culture.Customs.Has(Customs.MarriageType.Monogamy))
            {
                if (Relations.ContainsValue(Relation.Spouse))
                    return null; //моногамная семья и супруг уже есть
            }
            else if (Culture.Customs.Has(Customs.MarriageType.Polyamory))
            {
                return null; //институт семьи вообще отсутствует
            }
            else if (Culture.Customs.Has(Customs.MarriageType.Polygamy))
            { 
                if (Relations.ContainsValue(Relation.Spouse))
                {
                    //если в обществе патриархат, а потенциальный супруг - замужняя женщина, то нужно найти женатого на ней мужчину
                    if (Culture.Customs.Has(Customs.GenderPriority.Patriarchy) && Gender == Gender.Female)
                    {
                        foreach (var pRel in Relations)
                        {
                            if (pRel.Value == Relation.Spouse && pRel.Key.Gender == Gender.Male)
                            {
                                return pRel.Key;
                            }
                        }
                        return null;
                    }
                    //если в обществе матриархат, а потенциальный супруг - женатый мужчина, то нужно найти его жену
                    if (Culture.Customs.Has(Customs.GenderPriority.Matriarchy) && Gender == Gender.Male)
                    {
                        foreach (var pRel in Relations)
                        {
                            if (pRel.Value == Relation.Spouse && pRel.Key.Gender == Gender.Female)
                            {
                                return pRel.Key;
                            }
                        }
                        return null;
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// Может ли этот персонаж стать чьим-то любовником?
        /// Возвращает ссылку на самого себя, если может,
        /// null если не может
        /// </summary>
        /// <returns></returns>
        public Person GetLover()
        {
            if (Culture.Customs.Has(Customs.Sexuality.Puritan))
            {
                //if (!Rnd.OneChanceFrom(5))
                    return null;
            }
            else if (Culture.Customs.Has(Customs.Sexuality.Moderate_sexuality))
            { 
                if (Relations.ContainsValue(Relation.Lover))// && !Rnd.OneChanceFrom(5))
                    return null;
            }

            return this;
        }

        private string m_sShortName;
        private string m_sFullName = null;

        /// <summary>
        /// Имя, фамилия, профессия
        /// </summary>
        public string ShortName
        {
            get { return m_sShortName; }
        }

        /// <summary>
        /// Формирмирует короткое описание персонажа - имя, фамилия, профессия
        /// </summary>
        private void SetShortName()
        {
            StringBuilder sShortName = new StringBuilder();

            sShortName.AppendFormat("{0} {1}", Name, Family);

            if (Profession != ProfessionInfo.Nobody)
            {
                sShortName.AppendFormat(", {0}", Gender == Gender.Male ? Profession.NameMasculine : Profession.NameFeminine);
                if (Building.Info == State.Polity.StateCapital.MainBuilding)
                    sShortName.AppendFormat(" of {0} {1}", State.Name, State.Polity.Name);
                else if (Building.Info == State.Polity.ProvinceCapital.MainBuilding)
                    sShortName.AppendFormat(" of {0} {1} ({2} {3})", HomeLocation.Settlement.Profile.Name.ToLower(), HomeLocation.Settlement.Name, State.Name, State.Polity.Name);
                else
                    sShortName.AppendFormat(" from {0} {1} ({2} {3})", HomeLocation.Settlement.Profile.Name.ToLower(), HomeLocation.Settlement.Name, State.Name, State.Polity.Name);
            }

            m_sShortName = sShortName.ToString();
        }

        private bool GetSignificantOther(Relation eRelation, ref Person pSignificantOther)
        {
            if (Relations.ContainsValue(eRelation))
            {
                Person pPretender = GetHighestRelative(eRelation);
                if (pSignificantOther == null ||
                    (pSignificantOther.Profession == ProfessionInfo.Nobody &&
                     pPretender.Profession != ProfessionInfo.Nobody))
                {
                    pSignificantOther = pPretender;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Формирует полное описание персонажа - возраст, сословие, имя, фамилия, профессия и наиболее значительная из имеющихся социальных связей
        /// </summary>
        public void SetFullName()
        {
            StringBuilder sFullName = new StringBuilder();

            sFullName.AppendFormat("{0} {1} ", Age.ToString().ToLower(), Estate.Name.ToLower());
            sFullName.Append(m_sShortName);

            Person pSignificantOther = null;

            Array aRelations = Enum.GetValues(typeof(Relation));
            Array.Reverse(aRelations);

            foreach (Relation eRelation in aRelations)
                GetSignificantOther(eRelation, ref pSignificantOther);

            if (pSignificantOther == null && Relations.Count > 0)
                throw new Exception("Unknown relation type in relatives list!");

            if (pSignificantOther != null)
                sFullName.AppendFormat(", {0} of {1}", WhoAmITo(pSignificantOther), pSignificantOther.ShortName);

            if (Relations.Count > 1)
                sFullName.AppendFormat(" ({0} more relations)", Relations.Count-1);

            m_sFullName = sFullName.ToString();
        }

        public override string ToString()
        {
            return (Gender == Gender.Male ? "[M] " : "[F] ") + (m_sFullName == null ? m_sShortName : m_sFullName);
        }

        /// <summary>
        /// Определяет наиблее значимого из знакомых, связанных с персонажем определённым видом отношений.
        /// </summary>
        /// <param name="eRelation"></param>
        /// <returns></returns>
        private Person GetHighestRelative(Relation eRelation)
        {
            if (!Relations.ContainsValue(eRelation))
                return null;

            List<Person> cRelatives = new List<Person>();

            foreach (var pRelative in Relations)
            {
                if (pRelative.Value == eRelation)
                    cRelatives.Add(pRelative.Key);
            }

            Person pMostSignificant = cRelatives[0];
            int iMax = 0;

            foreach (Person pPerson in cRelatives)
            {
                int iImporance = pPerson.GetInfluence(false);
                if (iImporance > iMax)
                {
                    pMostSignificant = pPerson;
                    iMax = iImporance;
                }
            }

            return pMostSignificant;
        }


        ///// <summary>
        ///// Принять удар в поединке.
        ///// Шанс того, что удар противника вообще достигнет цели, определяется мастерством противника.
        ///// Если удар прошёл, то у бойца есть шанс выставить блок, определяемый его собственным мастерством.
        ///// Если боец пропустил удар - он проиграл.
        ///// Если удар удалось заблокировать, то сравниваем экипировку бойца и противника и считаем шансы того,
        ///// что экипировка бойца могла выдержать удар противника.
        ///// В поединке оба бойца используют и технику и бионику.
        ///// </summary>
        ///// <param name="pOpponent">противник</param>
        ///// <returns>true если боец проиграл поединок</returns>
        //public bool TakeHit(CPerson pOpponent)
        //{
        //    //оппонент бьёт техникой
        //    if (pOpponent.TechLevel > 0 && pOpponent.Body * pOpponent.Body >= Rnd.Get(200))
        //    {
        //        double attack = Math.Sqrt(pOpponent.TechLevel);// *pOpponent.TechLevel;
        //        double tDefence = Math.Sqrt(TechLevel);// *TechLevel;
        //        double bDefence = Math.Sqrt(BioLevel);// *BioLevel;
        //        //боец защищается техникой
        //        if (Body < Rnd.Get(10) ||
        //            Rnd.ChooseOne(attack, tDefence))//боец пропустил удар или броня его не спасла
        //        {
        //            //боец защищается бионикой
        //            if (Mind < Rnd.Get(20) ||
        //                Rnd.ChooseOne(attack, bDefence))//боец пропустил удар или суперспособности его не спасли
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    //оппонент бьёт бионикой
        //    if (pOpponent.BioLevel > 0 && pOpponent.Mind * pOpponent.Mind >= Rnd.Get(200))
        //    {
        //        double attack = Math.Sqrt(pOpponent.BioLevel);// *pOpponent.BioLevel;
        //        double tDefence = Math.Sqrt(TechLevel);// *TechLevel;
        //        double bDefence = Math.Sqrt(BioLevel);// *BioLevel;
        //        //боец защищается бионикой
        //        if (Mind < Rnd.Get(10) ||
        //            Rnd.ChooseOne(attack, bDefence))//боец пропустил удар или суперспособности его не спасли
        //        {
        //            //боец защищается техникой
        //            if (Body < Rnd.Get(20) ||
        //                Rnd.ChooseOne(attack, tDefence))//боец пропустил удар или броня его не спасла
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// Атаковать противника техникой и бионикой.
        ///// </summary>
        ///// <param name="pOpponent">противник</param>
        ///// <returns>true если противник повержен</returns>
        //public bool Attack(CPerson pOpponent)
        //{
        //    bool bWin1 = false, bWin2 = false;

        //    do
        //    {
        //        bWin1 = pOpponent.TakeHit(this);
        //        bWin2 = TakeHit(pOpponent);
        //    }
        //    while (!bWin1 && !bWin2);

        //    return bWin1;
        //}

        ///// <summary>
        ///// Взаимодействие двух персонажей.
        ///// В зависимости от их культурного уровня, может вылиться в драку или мирную дискуссию.
        ///// </summary>
        ///// <param name="pOpponent">оппонент</param>
        ///// <returns>true если противник повежен - физически или интеллектуально</returns>
        //public bool Interact(CPerson pOpponent)
        //{
        //    Dictionary<CSociety, int> cIntersections = new Dictionary<CSociety, int>();

        //    foreach (CEstate pEstate in m_cEstates)
        //    {
        //        foreach (CEstate pOppEstate in pOpponent.m_cEstates)
        //        {
        //            if (pEstate.Society == pOppEstate.Society)
        //                cIntersections[pEstate.Society] = pOppEstate.Rank - pEstate.Rank;
        //        }
        //    }

        //    bool bAttackPossible = (cIntersections.Count == 0);
        //    foreach (CSociety pSociety in cIntersections.Keys)
        //    {
        //        if (cIntersections[pSociety] == 1)
        //            bAttackPossible = true;
        //    }

        //    bool bSexPossible = (Gender != pOpponent.Gender);

        //    List<int> cChances = new List<int>();

        //    //оценка
        //    cChances[0] = 10;
        //    //общение
        //    cChances[1] = 0;
        //    if (m_cRelations.ContainsKey(pOpponent))
        //    {
        //        switch (m_cRelations[pOpponent])
        //        {
        //            case Relation.Enemy:
        //                cChances[1] = 0;
        //                break;
        //            case Relation.Opponent:
        //                cChances[1] = 2;
        //                break;
        //            case Relation.Neutral:
        //                cChances[1] = 5;
        //                break;
        //            case Relation.Friend:
        //                cChances[1] = 7;
        //                break;
        //            case Relation.Ally:
        //                cChances[1] = 10;
        //                break;
        //        }
        //    }
        //    //нападение
        //    cChances[2] = 0;
        //    if (bAttackPossible && m_cRelations.ContainsKey(pOpponent))
        //    {
        //        switch (m_cRelations[pOpponent])
        //        {
        //            case Relation.Enemy:
        //                cChances[1] = 10;
        //                break;
        //            case Relation.Opponent:
        //                cChances[1] = 7;
        //                break;
        //            case Relation.Neutral:
        //                cChances[1] = 5;
        //                break;
        //            case Relation.Friend:
        //                cChances[1] = 2;
        //                break;
        //            case Relation.Ally:
        //                cChances[1] = 0;
        //                break;
        //        }
        //    }
        //    //секс
        //    cChances[3] = 0;
        //    if (bSexPossible && m_cRelations.ContainsKey(pOpponent) && !m_cKindred.ContainsKey(pOpponent))
        //        cChances[2] = 10;

        //    //int iChance = Rnd.ChooseOne(cChances, 1);

        //    //switch (iChance)
        //    //{
        //    //    //оценка
        //    //    case 0:
        //    //        if (!m_cRelations.ContainsKey(pOpponent))
        //    //        { 
        //    //            if(Rnd.OneChanceFrom())
        //    //        }
        //    //        break;
        //    //    //общение   
        //    //    case 1:
        //    //        break;
        //    //    //нападение
        //    //    case 2:
        //    //        break;
        //    //    //секс
        //    //    case 3:
        //    //        break;
        //    //}

        //    if (CultureLevel < Rnd.Get(10))
        //        return Attack(pOpponent);
        //    else
        //    {
        //        if (pOpponent.CultureLevel < Rnd.Get(10))
        //            return pOpponent.Attack(this);
        //        else
        //            return true;//Rnd.ChooseOne(m_iCharisma, pOpponent.m_iCharisma);
        //    }
        //}
     }
}
