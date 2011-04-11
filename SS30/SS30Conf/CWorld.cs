using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS30Conf.Actions;
using SS30Conf.Items;
using SS30Conf.Persons;
using SS30Conf.Time;
using Random;

namespace SS30Conf
{
    public class CWorld: CConfigObject
    {
        private List<CSkill> m_cSkills = new List<CSkill>();

        public List<CSkill> Skills
        {
            get { return m_cSkills; }
        }
        private List<CLeisure> m_cLeisures = new List<CLeisure>();

        public List<CLeisure> Leisures
        {
            get { return m_cLeisures; }
        }
        private List<CRoom> m_cRooms = new List<CRoom>();

        public List<CRoom> Rooms
        {
            get { return m_cRooms; }
        }
        private List<CPerson> m_cPersons = new List<CPerson>();

        public List<CPerson> Persons
        {
            get { return m_cPersons; }
        }

        private CPerson m_pHero;

        public CPerson Hero
        {
            get { return m_pHero; }
        }

        private List<CAction> m_cActions = new List<CAction>();

        public List<CAction> Actions
        {
            get { return m_cActions; }
        }
        private List<CItem> m_cItems = new List<CItem>();

        public List<CItem> Items
        {
            get { return m_cItems; }
        }
        private Dictionary<PersonStats, CPersonStat> m_cStats = new Dictionary<PersonStats,CPersonStat>();

        public Dictionary<PersonStats, CPersonStat> Stats
        {
            get { return m_cStats; }
        }
        private List<CFetish> m_cFetishes = new List<CFetish>();

        public List<CFetish> Fetishes
        {
            get { return m_cFetishes; }
        }

        public CWorld(): base(StringCategory.CONFIGURATION, "SS30")
        {
            Reset();
        }

        private bool ExecuteSheduledAction(CPerson pActor)
        {
            List<CAction> cBackgroundActions = GetAvailableActions(pActor, false, true);
            foreach (CAction pBackgroundAction in cBackgroundActions)
                pBackgroundAction.Execute(pActor);

            CAction pSheduled = pActor.SheduledActions[CTimeKeeper.Instance.DayTime];
            if (pSheduled.IsAvailable(pActor))
            {
                pSheduled.Execute(pActor);
                return true;
            }
            else
            {
                List<CAction> cAlternateActions = GetAvailableActions(pActor, false, false);
                if (cAlternateActions.Count > 0)
                {
                    cAlternateActions[Rnd.Get(cAlternateActions.Count)].Execute(pActor);
                    return true;
                }
            }
            return false;
        }

        public int ExecuteHeroAction(CAction pAction)
        {
            if(pAction != null)
                pAction.Execute(m_pHero);

            int iTimePassed = 0;
            bool bDecisionPointReached = false;

            do
            {
                CTimeKeeper.NewEventInfo info = CTimeKeeper.Instance.PassTime();
                if (info != null)
                {
                    iTimePassed += info.TimePassed;
                    switch (info.Record.Type)
                    {
                        case RecordType.ACTION_END:
                            {
                                CEventActionFinished pEvent = info.Record as CEventActionFinished;
                                if (pEvent.Actor != m_pHero)
                                    ExecuteSheduledAction(pEvent.Actor);
                                else
                                    bDecisionPointReached = true;
                            }
                            break;
                        case RecordType.BUILDING_END:
                            break;
                        case RecordType.DAYTIME_END:
                            {
                                CEventSheduleFinished pEvent = info.Record as CEventSheduleFinished;
                                switch (pEvent.DayTime)
                                { 
                                    case DayTime.MORNING:
                                        m_pHero.Room.RecordEvent("Обед.");
                                        StartMaidsShedule();
                                        break;
                                    case DayTime.AFTERNOON:
                                        m_pHero.Room.RecordEvent("Ужин.");
                                        bDecisionPointReached = true;
                                        break;
                                    case DayTime.EVENING:
                                        m_pHero.Room.RecordEvent("Полночь.");
                                        CTimeKeeper.Instance.StartShedule();
                                        break;
                                    case DayTime.NIGHT:
                                        m_pHero.Room.RecordEvent("Вы проснулись.");
                                        foreach (CPerson person in Persons)
                                        {
                                            person.Rest(true);
                                        }
                                        bDecisionPointReached = true;
                                        break;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    bDecisionPointReached = true;
                }
            }
            while (!bDecisionPointReached);
            
            foreach(CRoom room in Rooms)
                room.RecordEvent("");
            
            return iTimePassed;
        }

        public void Reset()
        {
            CConfigRepository.Instance.Reset();
            Skills.Clear();
            Leisures.Clear();
            Rooms.Clear();
            Persons.Clear();
            Actions.Clear();
            Items.Clear();
            Stats.Clear();
            Fetishes.Clear();

            Name = "SS30";
            Description = "Configuration file for SS30";

            m_cStats[PersonStats.health] = new CPersonStat(PersonStats.health, "Health");
            m_cStats[PersonStats.health].Description = "Health is a resource, used to lower fatique.";
            m_cStats[PersonStats.health].SetAvailable(StringSubCategory.PERSON_MAID);
            m_cStats[PersonStats.fatique] = new CPersonStat(PersonStats.fatique, "Fatique");
            m_cStats[PersonStats.fatique].Description = "Fatique shows a loss of stamina. Maid with too high fatique can't work and have sex.";
            m_cStats[PersonStats.fatique].SetAvailable(StringSubCategory.PERSON_MAID);
            m_cStats[PersonStats.endurance] = new CPersonStat(PersonStats.endurance, "Endurance");
            m_cStats[PersonStats.endurance].Description = "Endurance determines the speed of rising fatique during everyday work.";
            m_cStats[PersonStats.endurance].SetAvailable(StringSubCategory.PERSON_MAID);

            m_cStats[PersonStats.happiness] = new CPersonStat(PersonStats.happiness, "Happiness");
            m_cStats[PersonStats.happiness].Description = "Happiness is a resource, used to lower stress.";
            m_cStats[PersonStats.happiness].SetAvailable(StringSubCategory.PERSON_MAID);
            m_cStats[PersonStats.stress] = new CPersonStat(PersonStats.stress, "Stress");
            m_cStats[PersonStats.stress].Description = "Stress is a measure of maid's bad emotional feelings. Maid with too high stress can't work and have sex.";
            m_cStats[PersonStats.stress].SetAvailable(StringSubCategory.PERSON_MAID);
            m_cStats[PersonStats.obedience] = new CPersonStat(PersonStats.obedience, "Obedience");
            m_cStats[PersonStats.obedience].Description = "Obedience determines the speed of rising stress during punishments.";
            m_cStats[PersonStats.obedience].SetAvailable(StringSubCategory.PERSON_MAID);

            m_cStats[PersonStats.sexuality] = new CPersonStat(PersonStats.sexuality, "Sexuality");
            m_cStats[PersonStats.sexuality].Description = "Sexuality is a resourse, used to rise an arousal.";
            m_cStats[PersonStats.sexuality].SetAvailable(StringSubCategory.PERSON_MAID);
            m_cStats[PersonStats.arousal] = new CPersonStat(PersonStats.arousal, "Arousal");
            m_cStats[PersonStats.arousal].Description = "Arousal is a measure of maid's lust. Maid with too high arousal works worse, but acts better during sex.";
            m_cStats[PersonStats.arousal].SetAvailable(StringSubCategory.PERSON_MAID);
            m_cStats[PersonStats.sensuality] = new CPersonStat(PersonStats.sensuality, "Sensuality");
            m_cStats[PersonStats.sensuality].Description = "Sensuality determines the speed of lowering arousal during sex.";
            m_cStats[PersonStats.sensuality].SetAvailable(StringSubCategory.PERSON_MAID);

            m_cStats[PersonStats.leisure_need] = new CPersonStat(PersonStats.leisure_need, "Leisure Need");
            m_cStats[PersonStats.leisure_need].Description = "An importance of a good leisure time for a guest.";
            m_cStats[PersonStats.leisure_need].SetAvailable(StringSubCategory.PERSON_GUEST);
            m_cStats[PersonStats.leisure_setisfaction] = new CPersonStat(PersonStats.leisure_setisfaction, "Leisure Satisfaction");
            m_cStats[PersonStats.leisure_setisfaction].Description = "How much satisfied a guest with his leisure time in hotel.";
            m_cStats[PersonStats.leisure_setisfaction].SetAvailable(StringSubCategory.PERSON_GUEST);

            m_cStats[PersonStats.service_need] = new CPersonStat(PersonStats.service_need, "Service Need");
            m_cStats[PersonStats.service_need].Description = "An importance of a good service for a guest.";
            m_cStats[PersonStats.service_need].SetAvailable(StringSubCategory.PERSON_GUEST);
            m_cStats[PersonStats.service_satisfaction] = new CPersonStat(PersonStats.service_satisfaction, "Service Satisfaction");
            m_cStats[PersonStats.service_satisfaction].Description = "How much satisfied a guest with a service quality in a hotel.";
            m_cStats[PersonStats.service_satisfaction].SetAvailable(StringSubCategory.PERSON_GUEST);

            m_cStats[PersonStats.sex_need] = new CPersonStat(PersonStats.sex_need, "Sex Need");
            m_cStats[PersonStats.sex_need].Description = "An importance of a good sex for a guest.";
            m_cStats[PersonStats.sex_need].SetAvailable(StringSubCategory.PERSON_GUEST);
            m_cStats[PersonStats.sex_satisfaction] = new CPersonStat(PersonStats.sex_satisfaction, "Sex Satisfaction");
            m_cStats[PersonStats.sex_satisfaction].Description = "How much satisfied a guest with a sex quality in a hotel.";
            m_cStats[PersonStats.sex_satisfaction].SetAvailable(StringSubCategory.PERSON_GUEST);

            CRoom pNewRoom = new CRoom("kitchen", "Kitchen", RoomType.SERVICE);
            pNewRoom.Description = "Kitchen is used to cooking.";
            pNewRoom.CostToBuild = 0;
            pNewRoom.DaysToBuild = 0;
            Rooms.Add(pNewRoom);

            pNewRoom = new CRoom("maids_1", "Maids' room", RoomType.SERVICE);
            pNewRoom.Description = "There is where 3 maids could live.";
            pNewRoom.CostToBuild = 0;
            pNewRoom.DaysToBuild = 0;
            Rooms.Add(pNewRoom);

            pNewRoom = new CRoom("hero", "Hero's room", RoomType.SERVICE);
            pNewRoom.Description = "There is where a hero live. Also used for night training.";
            pNewRoom.CostToBuild = 0;
            pNewRoom.DaysToBuild = 0;
            Rooms.Add(pNewRoom);

            pNewRoom = new CRoom("secretary", "Mia's room", RoomType.SERVICE);
            pNewRoom.Description = "There is where hero's secretary live. Also used for maids' punishment.";
            pNewRoom.CostToBuild = 0;
            pNewRoom.DaysToBuild = 0;
            Rooms.Add(pNewRoom);

            pNewRoom = new CRoom("guest_1", "Guest's room 1", RoomType.GUEST);
            pNewRoom.Description = "There is one of rooms where guests could live.";
            pNewRoom.CostToBuild = 0;
            pNewRoom.DaysToBuild = 0;
            Rooms.Add(pNewRoom);

            pNewRoom = new CRoom("guest_2", "Guest's room 2", RoomType.GUEST);
            pNewRoom.Description = "There is one of rooms where guests could live.";
            pNewRoom.CostToBuild = 0;
            pNewRoom.DaysToBuild = 0;
            Rooms.Add(pNewRoom);

            pNewRoom = new CRoom("guest_3", "Guest's room 3", RoomType.GUEST);
            pNewRoom.Description = "There is one of rooms where guests could live.";
            pNewRoom.CostToBuild = 0;
            pNewRoom.DaysToBuild = 0;
            Rooms.Add(pNewRoom);
            
            m_pHero = new CSpecial("hero", "You", Gender.MALE);
            Persons.Add(m_pHero);
        }

        public bool Load(string sFilename)
        {
            return CConfigRepository.Instance.Load(sFilename, this);
        }

        public void Save(string sFilename)
        {
            CConfigRepository.Instance.SaveXML(sFilename);
        }

        /// <summary>
        /// Получаем список доступных прямо сейчас для указанного исполнителя действий
        /// В список не будут включены "фоновые" и недоступные для планирования действия.
        /// </summary>
        /// <param name="actor">исполнитель</param>
        public List<CAction> GetAvailableActions(CPerson actor)
        {
            return GetAvailableActions(actor, CTimeKeeper.Instance.DayTime, true, false);
        }

        /// <summary>
        /// Получаем список доступных прямо сейчас для указанного исполнителя действий
        /// </summary>
        /// <param name="actor">исполнитель</param>
        /// <param name="bShedulable">если true, то будут показаны только те действия, которые могут быть запланированы</param>
        /// <param name="bBackground">если true, то будут показаны только "фоновые" действия</param>
        /// <returns></returns>
        public List<CAction> GetAvailableActions(CPerson actor, bool bShedulable, bool bBackground)
        {
            return GetAvailableActions(actor, CTimeKeeper.Instance.DayTime, bShedulable, bBackground);
        }

        /// <summary>
        /// Получаем список доступных для планирования на указанное время суток указанным 
        /// исполнителем действий.
        /// В список не будут включены "фоновые" и недоступные для планирования действия.
        /// </summary>
        /// <param name="actor">исполнитель</param>
        /// <param name="eDayTime">время суток</param>
        /// <returns></returns>
        public List<CAction> GetAvailableActions(CPerson actor, DayTime eDayTime)
        { 
            return GetAvailableActions(actor, eDayTime, true, false);
        }

        /// <summary>
        /// Получаем список доступных в указанное время суток для указанного исполнителя действий
        /// </summary>
        /// <param name="actor">исполнитель</param>
        /// <param name="eDayTime">время суток</param>
        /// <param name="bShedulable">если true, то будут показаны только те действия, которые могут быть запланированы</param>
        /// <param name="bBackground">если true, то будут показаны только "фоновые" действия. Иначе "фоновые" показаны не будут.</param>
        /// <returns></returns>
        public List<CAction> GetAvailableActions(CPerson actor, DayTime eDayTime, bool bShedulable, bool bBackground)
        { 
            List<CAction> cResult = new List<CAction>();
            foreach (CAction pAction in Actions)
            {
                if (bShedulable && pAction.Hidden)
                    continue;

                if (bBackground && pAction.Priority != PriorityFilter.BACKGROUND)
                    continue;

                if (!bBackground && pAction.Priority == PriorityFilter.BACKGROUND)
                    continue;

                if (!pAction.CheckActor(actor))
                    continue;

                if (!pAction.CheckTime(eDayTime))
                    continue;

                if (eDayTime != CTimeKeeper.Instance.DayTime || pAction.IsAvailable(actor))
                {
                    cResult.Add(pAction);
                }
            }
            return cResult;
        }

        public void StartMaidsShedule()
        {
            CTimeKeeper.Instance.StartShedule();
            foreach (CPerson person in Persons)
            {
                if (person.SubCategory == StringSubCategory.PERSON_MAID)
                {
                    if (person.SheduledActions.ContainsKey(CTimeKeeper.Instance.DayTime) &&
                        person.SheduledActions[CTimeKeeper.Instance.DayTime] != null)
                    {
                        ExecuteSheduledAction(person);
                    }
                }
            }
        }

        public List<CAction> GetActionsByName(CPerson actor, string sActionName)
        {
            List<CAction> cResult = new List<CAction>();
            foreach (CAction action in Actions)
            {
                if (action.Name == sActionName && action.IsAvailable(actor))
                    cResult.Add(action);
            }
            return cResult;
        }
    }
}
