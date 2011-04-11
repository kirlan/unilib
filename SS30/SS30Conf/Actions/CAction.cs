using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using nsUniLibXML;
using System.ComponentModel;
using SS30Conf.Persons;
using SS30Conf.Time;

namespace SS30Conf.Actions
{
    public enum PersonFilter
    {
        ANY_PERSON,
        ANY_MAID,
        ANY_GUEST,
        HERO,
        SOLO,//только для цели действия - если оно не требует цели
        SPECIFIED_PERSON
    }

    public enum RoomFilter
    {
        ANY_ROOM,
        ANY_SERVICE_ROOM,
        ANY_GUEST_ROOM,
        ANY_LEISURE_ROOM,
        SPECIFIED_ROOM
    }

    public enum PriorityFilter
    { 
        TARGET,
        ROOM,
        BACKGROUND
    }

    public enum DayTimeFilter
    { 
        ANY_TIME,
        MORNING_DAY,
        MORNING,
        DAY,
        EVENING
    }

    public enum ActionType
    { 
        WORK,
        PUNISHMENT,
        SEX,
        REST
    }

    /// <summary>
    /// Действие - любое программируемое взаимодействие объектов игрового мира.
    /// </summary>
    public class CAction: CConfigObject
    {
        private CConfigProperty<ActionType> m_pActionType;

        public ActionType ActionType
        {
            get { return m_pActionType.Value; }
            set { m_pActionType.Value = value; }
        }

        private CConfigProperty<int> m_pDuration;
        /// <summary>
        /// Длительность выполнения действия В МИНУТАХ.
        /// Если действие запланировано и длительнось меньше периода планировки (по умолчанию - 6 часов)
        /// больше чем в 2 раза (т.е. за отведённое время действие может выполниться как минимум дважды),
        /// оно будет выполняться циклически до тех пор, пока оставшееся до начала выполнения следующего 
        /// запланированного действия время не станет меньше длительности действия.
        /// Если выполняются одновременно больше одного действия (одно запланированное и несколько 
        /// "спонтанных", т.е. с Pririty == NONE), то каждое действие повторяется по своему собственному 
        /// таймеру.
        /// </summary>
        public int Duration
        {
            get { return m_pDuration.Value; }
            set { m_pDuration.Value = value; }
        }

        private CConfigProperty<PersonFilter> m_pActorFilter;

        /// <summary>
        /// Кто может выступать инициатором действия.
        /// Это основной критерий, т.к. любые действия выполняются кем-то.
        /// Т.е. мы всегда сначала выбираем исполнителя, а потом уже смотрим, какие действия ему
        /// доступны.
        /// </summary>
        public PersonFilter ActorFilter
        {
            get { return m_pActorFilter.Value; }
            set { m_pActorFilter.Value = value; }
        }

        private CConfigProperty<PersonFilter> m_pTargetFilter;
        /// <summary>
        /// Кто может быть целью действия.
        /// Если Priority == TARGET, то этот фильтр является приоритетным условием и
        /// цель обязательно должна быть явно задана. 
        /// Если цель не задана однозначно в фильтре (т.е. выбрано любое из значений ANY_...), то:
        /// Если исполнитель - герой или девушка, то цель выбирает игрок из списка соответствующего 
        /// указанному фильтру.
        /// Если исполнитель - гость, то цель выбирается AI гостя.
        /// Если Priority == ROOM, то наличие в указанной комнате цели, соответствующей фильтру, 
        /// лишь определяет доступность действия.
        /// </summary>
        public PersonFilter TargetFilter
        {
            get { return m_pTargetFilter.Value; }
            set { m_pTargetFilter.Value = value; }
        }

        private CConfigProperty<RoomFilter> m_pRoomFilter;
        /// <summary>
        /// Где может выполняться действие
        /// При выполнении действия исполнитель переместится в указанную комнату.
        /// Если Priority == ROOM, то этот фильтр является приоритетным условием и комната 
        /// обязательно должна быть явно задана. 
        /// Если комната не задана однозначно в фильтре (т.е. выбрано любое из значений ANY_...), то:
        /// Если исполнитель - герой или девушка, то комнату выбирает игрок из списка соответствующего 
        /// указанному фильтру.
        /// Если исполнитель - гость, то комната выбирается AI гостя.
        /// Если Priority == TARGET, то выбор комнаты осуществляется выбранной целью, а доступность 
        /// действия определяется соответствием этому фильтру.
        /// </summary>
        public RoomFilter RoomFilter
        {
            get { return m_pRoomFilter.Value; }
            set { m_pRoomFilter.Value = value; }
        }

        private CBindingConfigProperty<CRoom> m_pRoom;

        /// <summary>
        /// Если выбран RoomFilter == RoomFilter.SPECIFIED_ROOM,
        /// то действие будет доступно только в том случае, если в отеле есть такая комната.
        /// </summary>
        public CRoom Room
        {
            get { return m_pRoom.Object; }
            set { m_pRoom.Object = value; }
        }

        private CConfigProperty<PriorityFilter> m_pPriority;
        /// <summary>
        /// Какой из фильтров - цель или комната - является приоритетным.
        /// Приоритетный фильтр осуществляет выбор цели/места, а второй фильтр при этом определяет 
        /// доступность действия при выбранной цели/месте. Т.е. если исполниель - девушка и выбран 
        /// приоритетный фильтр по цели - любой гость, а фильтр по месту - любая комната для 
        /// досуга, то игроку при планировании действия будет предложено выбрать гостя из числа тех, 
        /// которые в заданное время суток будут находиться в любой из комнат для досуга. При 
        /// выполнении действия в этом случае девушка переместится в комнату для досуга, в которой 
        /// будет находиться выбранный гость. Если в указанное время согласно планировщику ни один 
        /// из гостей не будет находиться в комнатах для досуга, действие будет недоступно в 
        /// планировщике.
        /// Если в фильтре по месту при этом будет указана конкретная комната для досуга и нельзя
        /// будет с уверенностью утверждать, будет ли гость в указанное время именно в этой комнате,
        /// то действие будет доступно в планировщике, но при попытке выполнить его, если выбранный 
        /// гость будет в какой-то другой комнате, то действие выполнить не удастся.
        /// Если приоритетный фильтр по комнате - любая комната для досуга, а фильтр по цели - 
        /// любой гость, то игроку будет предложено выбрать комнату для досуга из числа тех,
        /// в которых в заданное время возможно будет находиться хоть один гость. Если в комнате 
        /// будет больше одного гостя, действие будет выполнено несколько раз, по разу для каждого
        /// из гостей в комнате. 
        /// Если в комнате не окажется ни одного гостя, действие выполнено не будет.
        /// 
        /// Если задан приоритет NONE, то действие не может быть запланировано и будет выполняться
        /// автоматически в случае если исполнитель окажется в соответствующей фильтру комнате с 
        /// соответствующей фильтру целью в соответствующее фильтру время суток.
        /// 
        /// Если исполнителю доступно одновременно несколько действий с одинаковым именем и приоритетным
        /// условием, то считаем, что это всё варианты одного и того же действия, т.е. в планировщике
        /// предлагаем только один выбор и затем последовательно пытаемся выполнить все варианты.
        /// Например, есть действие "Обслуживание", исполнитель - девушка, приоритетная цель - ANY_GUEST,
        /// комната - SPECIFIED_ROOM(Бассеин) и есть другое действие "Обслуживание", точно такое же, 
        /// но комната - SPECIFIED_ROOM(Библиотека). В этом случае мы в планировщике предлагаем только 
        /// одно действие "Обслуживание", при выборе которого игроку предлагается выбрать конкретного гостя
        /// из числа тех, кто, возможно, будет в указанное время в бессеине или в библиотеке. При попытке 
        /// же выполнить действие, если указанный гость будет в бассеине, то выполнится первое действие,
        /// если гость будет в библиотеке - то второе, если же гость будет где-то ещё, то ни одно из двух 
        /// действий выполнить не удастся. Если исполнитель не смог выполнить ни одно из действий, 
        /// запланированных на указанное время суток, то игрок получает сообщение о том, что в указанное 
        /// время суток исполнитель бесцельно шатался по отелю или что-нибудь в этом роде...
        /// 
        /// Если совпадают названия действий и значение Priority, а значение самого приоритетного
        /// фильтра не совпадают, то действия всё-равно считаются вариантами одного и того же, а
        /// при планировании игроку предлагается выбрать цель/место из объединённого списка, 
        /// соответствующего всем инвариантным фильтрам.
        /// Если же названия действий совпадают, а значение Priority - нет, то эти действия выводятся в
        /// планировщике как 2 разных выбора и пусть тот кто составлял такую кривую конфигурацию сам
        /// разбирвается, какое действие - какое.
        /// </summary>
        public PriorityFilter Priority
        {
            get { return m_pPriority.Value; }
            set { m_pPriority.Value = value; }
        }

        private CConfigProperty<bool> m_pHidden;
        /// <summary>
        /// Если true, то действие не может быть запланировано (если исполнитель - девушка) 
        /// или явно выбрано (если исполнитель - герой), независимо от значения приоритета.
        /// </summary>
        public bool Hidden
        {
            get { return m_pHidden.Value; }
            set { m_pHidden.Value = value; }
        }

        private CConfigProperty<DayTimeFilter> m_pDayTimeFilter;
        /// <summary>
        /// Когда может выполняться действие.
        /// В игре всегда может быть одновременно только какое-то конкретное время суток.
        /// Соответственно, действие либо доступно, либо нет - без вариантов.
        /// </summary>
        public DayTimeFilter DayTimeFilter
        {
            get { return m_pDayTimeFilter.Value; }
            set { m_pDayTimeFilter.Value = value; }
        }

        private CConfigProperty<string> m_pTargetStatusList;
        private Dictionary<string, bool> m_cTargetStatusList = new Dictionary<string, bool>();
        /// <summary>
        /// Набор статусов, которым должна соответствовать или не соответствовать цель действия
        /// для того, чтобы действие было доступно.
        /// Ключ словаря - статус. Значение, соответвующее ключу, определяет, должна ли цель 
        /// обладать указанным статусом для доступности действия (true), или наоборот, действие доступно
        /// только для целей, у которых данный статус отсутствует (false).
        /// </summary>
        [ImmutableObject(true)]
        public Dictionary<string, bool> TargetStatusList
        {
            get { return m_cTargetStatusList; }
        }

        public void UpdateTargetStatusList(Dictionary<string, bool> newList)
        {
            bool bModified = false;

            List<string> removedKeys = new List<string>();
            foreach (string key in m_cTargetStatusList.Keys)
            {
                if (!newList.ContainsKey(key))
                    removedKeys.Add(key);
            }
            foreach (string key in removedKeys)
                m_cTargetStatusList.Remove(key);

            bModified = removedKeys.Count > 0;

            foreach (string key in newList.Keys)
            {
                if (!m_cTargetStatusList.ContainsKey(key) || (m_cTargetStatusList.ContainsKey(key) && m_cTargetStatusList[key] != newList[key]))
                {
                    bModified = true;
                    m_cTargetStatusList[key] = newList[key];
                }
            }

            if (bModified)
            {
                string sNewList = "";
                foreach (string key in m_cTargetStatusList.Keys)
                {
                    sNewList += string.Format("{0}{1} ", m_cTargetStatusList[key] ? "+" : "-", key);
                }
                m_pTargetStatusList.Value = sNewList;
            }
        }

        private void ParseTargetStatusList()
        {
            string[] statuses = m_pTargetStatusList.Value.Split(new char[] { ' ' });
            foreach (string status in statuses)
            {
                if (status.StartsWith("+"))
                    m_cTargetStatusList[status.Substring(1)] = true;
                if (status.StartsWith("-"))
                    m_cTargetStatusList[status.Substring(1)] = false;
            }
        }

        private CConfigProperty<string> m_pActorStatusList;
        private Dictionary<string, bool> m_cActorStatusList = new Dictionary<string, bool>();
        /// <summary>
        /// Набор статусов, которым должен соответствовать или не соответствовать исполнитель
        /// для того, чтобы действие было доступно.
        /// Ключ словаря - статус. Значение, соответвующее ключу, определяет, должен ли исполнитель 
        /// обладать указанным статусом для доступности действия (true), или наоборот, действие доступно
        /// только исполнителям, у которых данный статус отсутствует (false).
        /// </summary>
        [ImmutableObject(true)]
        public Dictionary<string, bool> ActorStatusList
        {
            get { return m_cActorStatusList; }
        }

        public void UpdateActorStatusList(Dictionary<string, bool> newList)
        {
            bool bModified = false;

            List<string> removedKeys = new List<string>();
            foreach (string key in m_cActorStatusList.Keys)
            {
                if (!newList.ContainsKey(key))
                    removedKeys.Add(key);
            }
            foreach (string key in removedKeys)
                m_cActorStatusList.Remove(key);

            bModified = removedKeys.Count > 0;

            foreach (string key in newList.Keys)
            {
                if (!m_cActorStatusList.ContainsKey(key) || (m_cActorStatusList.ContainsKey(key) && m_cActorStatusList[key] != newList[key]))
                {
                    bModified = true;
                    m_cActorStatusList[key] = newList[key];
                }
            }

            if (bModified)
            {
                string sNewList = "";
                foreach (string key in m_cActorStatusList.Keys)
                {
                    sNewList += string.Format("{0}{1} ", m_cActorStatusList[key] ? "+" : "-", key);
                }
                m_pActorStatusList.Value = sNewList;
            }
        }

        private void ParseActorStatusList()
        {
            string[] statuses = m_pActorStatusList.Value.Split(new char[] { ' ' });
            foreach (string status in statuses)
            {
                if (status.StartsWith("+"))
                    m_cActorStatusList[status.Substring(1)] = true;
                if (status.StartsWith("-"))
                    m_cActorStatusList[status.Substring(1)] = false;
            }
        }

        /// <summary>
        /// Возможные последствия в зависимости от произвольных условий
        /// </summary>
        private SortedList<int, CReaction> m_cReactions = new SortedList<int, CReaction>();

        public IList<CReaction> Reactions
        {
            get { return m_cReactions.Values; }
        }
        // в файл реакции записываются отдельными строчками, при этом именно в реакциях пишется, к какому действию
        // они относятся. В строчке, описывающей действие, никаких упоминаний о реакциях нет.
        // Так сделано для того, чтобы реакции можно было легко добавлять и изменять в модах.

        public bool CanUsePriority(int iPriority)
        {
            return !m_cReactions.ContainsKey(iPriority);
        }

        public int GetMaxPriority()
        {
            if (m_cReactions.Keys.Count > 0)
                return m_cReactions.Keys[m_cReactions.Keys.Count - 1];
            else
                return 0;
        }

        public bool AddReaction(CReaction pReaction)
        {
            if (m_cReactions.ContainsKey(pReaction.Priority))
                return false;
            m_cReactions.Add(pReaction.Priority, pReaction);
            return true;
        }

        internal bool OnPriorityChanged(CReaction pReaction)
        { 
            if(!RemoveReaction(pReaction))
                return false;

            return AddReaction(pReaction);
        }

        public bool RemoveReaction(CReaction pReaction)
        {
            if(!m_cReactions.ContainsValue(pReaction))
                return false;

            int index = m_cReactions.IndexOfValue(pReaction);
            m_cReactions.RemoveAt(index);

            return true;
        }

        public bool CheckActor(CPerson actor)
        {
            bool fail = false;
            foreach (string status in ActorStatusList.Keys)
            {
                if (ActorStatusList[status] && !actor.Statuses.Contains(status))
                    fail = true;

                if (!ActorStatusList[status] && actor.Statuses.Contains(status))
                    fail = true;
            }

            if (fail)
                return false;

            switch (ActorFilter)
            {
                case PersonFilter.ANY_GUEST:
                    return actor.SubCategory == StringSubCategory.PERSON_GUEST;
                case PersonFilter.ANY_MAID:
                    return actor.SubCategory == StringSubCategory.PERSON_MAID;
                case PersonFilter.HERO:
                    return actor.SubCategory == StringSubCategory.PERSON_SPECIAL;
                case PersonFilter.SPECIFIED_PERSON:
                    //return actor == Actor
                    break;
                case PersonFilter.ANY_PERSON:
                    return true;
            }
            return false;
        }

        private bool CheckRoom(CRoom room)
        {
            if (Priority == PriorityFilter.ROOM)
                return true;

            switch (RoomFilter)
            {
                case RoomFilter.ANY_SERVICE_ROOM:
                    return room.RoomType == RoomType.SERVICE;
                case RoomFilter.ANY_GUEST_ROOM:
                    return room.RoomType == RoomType.GUEST;
                case RoomFilter.ANY_LEISURE_ROOM:
                    return room.RoomType == RoomType.LEISURE;
                case RoomFilter.SPECIFIED_ROOM:
                    return room == Room;
                case RoomFilter.ANY_ROOM:
                    return true;
            }
            return false;
        }

        public bool CheckTime()
        {
            return CheckTime(CTimeKeeper.Instance.DayTime);
        }

        public bool CheckTime(DayTime eDayTime)
        {
            switch (DayTimeFilter)
            {
                case DayTimeFilter.DAY:
                    return eDayTime == DayTime.AFTERNOON;
                case DayTimeFilter.EVENING:
                    return eDayTime == DayTime.EVENING;
                case DayTimeFilter.MORNING:
                    return eDayTime == DayTime.MORNING;
                case DayTimeFilter.MORNING_DAY:
                    return eDayTime == DayTime.AFTERNOON || eDayTime == DayTime.MORNING;
                case DayTimeFilter.ANY_TIME:
                    return true;
            }
            return false;
        }

        private bool CheckTarget(CPerson actor, CPerson target)
        {
            bool fail = false;
            foreach (string status in TargetStatusList.Keys)
            {
                if (TargetStatusList[status] && !target.Statuses.Contains(status))
                    fail = true;

                if (!TargetStatusList[status] && target.Statuses.Contains(status))
                    fail = true;
            }

            if (fail)
                return false;

            switch (TargetFilter)
            {
                case PersonFilter.ANY_GUEST:
                    return target != actor && target.SubCategory == StringSubCategory.PERSON_GUEST;
                case PersonFilter.ANY_MAID:
                    return target != actor && target.SubCategory == StringSubCategory.PERSON_MAID;
                case PersonFilter.HERO:
                    return target != actor && target.SubCategory == StringSubCategory.PERSON_SPECIAL;
                case PersonFilter.SOLO:
                    return target == actor;
                case PersonFilter.SPECIFIED_PERSON:
                    //return target == Target;
                    break;
                case PersonFilter.ANY_PERSON:
                    return target != actor;
            }
            return false;
        }

        public bool IsAvailable(CPerson actor)
        {
            if (!CheckActor(actor))
                return false;

            foreach (CPerson target in CConfigRepository.Instance.Strings[StringCategory.PERSON].Values)
            {
                if (CheckTarget(actor, target) && CheckRoom(target.Room))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Выполняет действие - выдаёт первую применимую реакцию в списке
        /// </summary>
        public void Execute(CPerson actor)
        {
            if (!CheckActor(actor) || !CheckTime())
                return;

            if (Priority == PriorityFilter.ROOM)
            {
                if(RoomFilter != RoomFilter.SPECIFIED_ROOM)
                    return;
                actor.GoTo(Room);
            }
            if (Priority == PriorityFilter.TARGET)
            {
                if (TargetFilter != PersonFilter.SPECIFIED_PERSON &&
                    TargetFilter != PersonFilter.HERO)
                    return;
                //actor.GoTo(Target);
            }

            if (!CheckRoom(actor.Room))
                return;

            foreach (CPerson target in actor.Room.Persons)
            {
                if (CheckTarget(actor, target))
                {
                    if (Description != "")
                        actor.Room.RecordEvent(string.Format("{0}: {1}", actor.Name, Description));
                    if (Priority != PriorityFilter.BACKGROUND)
                        CTimeKeeper.Instance.ExecuteAction(this, actor, target);
                    switch (ActionType)
                    {
                        case ActionType.WORK:
                            actor.Tire();
                            break;
                        case ActionType.PUNISHMENT:
                            actor.Punish();
                            break;
                        case ActionType.SEX:
                            actor.Cum();
                            if (actor != target)
                                target.Cum();
                            break;
                    }
                    foreach (CReaction react in m_cReactions.Values)
                    {
                        if (react.Check(actor, target))
                        {
                            react.Execute(actor, target);
                            if (!react.Continue)
                                break;
                        }
                    }
                }
            }
        }

        public CAction()
            : base(StringCategory.ACTION)
        {
            Name = "New Action";
        }

        public CAction(UniLibXML xml, XmlNode pNode, CWorld pWorld)
            : base(xml, pNode)
        {
            ParseActorStatusList();
            ParseTargetStatusList();
            pWorld.Actions.Add(this);
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pDuration = new CConfigProperty<int>(this, "Duration", 30);

            m_pActorFilter = new CConfigProperty<PersonFilter>(this, "ActorFilter", PersonFilter.ANY_MAID);

            m_pTargetFilter = new CConfigProperty<PersonFilter>(this, "TargetFilter", PersonFilter.SOLO);

            m_pRoomFilter = new CConfigProperty<RoomFilter>(this, "RoomFilter", RoomFilter.ANY_ROOM);

            m_pRoom = new CBindingConfigProperty<CRoom>(this, "Room", StringCategory.ROOM);

            m_pPriority = new CConfigProperty<PriorityFilter>(this, "Priority", PriorityFilter.ROOM);

            m_pDayTimeFilter = new CConfigProperty<DayTimeFilter>(this, "DayTime", DayTimeFilter.ANY_TIME);

            m_pActorStatusList = new CConfigProperty<string>(this, "Statuses0", " ");

            m_pTargetStatusList = new CConfigProperty<string>(this, "Statuses", " ");

            m_pHidden = new CConfigProperty<bool>(this, "Hidden", false);

            m_pActionType = new CConfigProperty<ActionType>(this, "Type", ActionType.WORK);
        }
    }
}
