using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;
using Persona.Conditions;
using Persona.Consequences;
using nsUniLibXML;
using System.Xml;

namespace Persona
{
    /// <summary>
    /// Игровой модуль. Содержит набор ситуаций, список категорий по которым эти ситуации распределены и 
    /// список параметров, описывающих игровую ситуацию в конкретный момент времени.
    /// </summary>
    public class Module
    {
        /// <summary>
        /// Короткое имя модуля.
        /// </summary>
        public string m_sName = "Новый модуль";

        /// <summary>
        /// Длинное описание модуля, модет содержать в себе описание игрового мира, в котором происходит действие,
        /// предысторию событий, указания на то, чего следует ожидать игрокам...
        /// </summary>
        public string m_sDescription = "Описание отсутствует";

        /// <summary>
        /// Текст, выводимый при выборе действий, например - описывающий текущую локацию или ситуацию.
        /// В XML не пишется, изменяется при помощи специальной команды.
        /// </summary>
        public string m_sHeader = "";

        /// <summary>
        /// Список действий, порождающих события, например - "Секс", "Работа", "Учёба"...
        /// </summary>
        public List<Action> m_cActions = new List<Action>();

        /// <summary>
        /// Список возможных событий.
        /// </summary>
        public List<Event> m_cEvents = new List<Event>();

        /// <summary>
        /// Список триггеров.
        /// </summary>
        public List<Trigger> m_cTriggers = new List<Trigger>();

        /// <summary>
        /// Список функций.
        /// </summary>
        public List<Function> m_cFunctions = new List<Function>();

        /// <summary>
        /// Список числовых параметров, описывающих игровую ситуацию в конкретный момент времени.
        /// Числовые параметры могут участвовать в условиях сравнения и попадания в диапазон.
        /// </summary>
        public List<NumericParameter> m_cNumericParameters = new List<NumericParameter>();

        /// <summary>
        /// Список логических параметров, описывающих игровую ситуацию в конкретный момент времени.
        /// Логические параметры могут участвовать в условиях проверки истинности.
        /// </summary>
        public List<BoolParameter> m_cBoolParameters = new List<BoolParameter>();

        /// <summary>
        /// Список строковых параметров, описывающих игровую ситуацию в конкретный момент времени.
        /// Строковые параметры не могут участвовать ни в каких условиях.
        /// </summary>
        public List<StringParameter> m_cStringParameters = new List<StringParameter>();

        /// <summary>
        /// Список коллекций объектов. Коллекции могут содержать в себе, например, информацию обо всех доступных в игре NPC или всех доступных локациях...
        /// </summary>
        public List<ObjectsCollection> m_cCollections = new List<ObjectsCollection>();

        public Module()
        {
            m_sName = "КиберШлюха 2115";
            m_sDescription = "История о нелёгкой жизни уличной шлюхи в трущобах киберпанковского мира.";

            m_cActions.Add(new Action("Тина"));
            m_cActions.Add(new Action("Мэри"));
            m_cActions.Add(new Action("Джейн"));
            m_cActions.Add(new Action("Ал"));

            m_cNumericParameters.Add(new NumericParameter("Сообразительность", "Основные"));
            m_cNumericParameters.Add(new NumericParameter("Репутация", "Основные"));
            m_cNumericParameters.Add(new NumericParameter("Размер груди", "Основные"));
            m_cNumericParameters.Add(new NumericParameter("Красота", "Основные"));

            m_cNumericParameters.Add(new NumericParameter("Настроение", "Вторичные"));
            m_cNumericParameters.Add(new NumericParameter("Здоровье", "Вторичные"));
            m_cNumericParameters.Add(new NumericParameter("Энергия", "Вторичные"));
            m_cNumericParameters.Add(new NumericParameter("Интоксикация", "Вторичные"));

            m_cNumericParameters.Add(new NumericParameter("Орал", "Опыт"));
            m_cNumericParameters.Add(new NumericParameter("Анал", "Опыт"));
            m_cNumericParameters.Add(new NumericParameter("Традиционный секс", "Опыт"));
            m_cNumericParameters.Add(new NumericParameter("Лесби", "Опыт"));
            m_cNumericParameters.Add(new NumericParameter("Садо-мазо", "Опыт"));
            m_cNumericParameters.Add(new NumericParameter("Эксгибиционизм", "Опыт"));

            m_cNumericParameters.Add(new NumericParameter("Наличка", "Деньги"));
            m_cNumericParameters.Add(new NumericParameter("Банковский счёт", "Деньги"));
            m_cNumericParameters.Add(new NumericParameter("Долг", "Деньги"));

            m_cBoolParameters.Add(new BoolParameter("Персонаж выбран", "Системный"));

            Event pTina = new Event(m_cActions[0]);
            pTina.m_sID = "newGame_Tina";
            pTina.m_pDescription.m_sText = "Тина - профессиональная модель. Она жила в пентхаусе, работала со знаменитыми кутурье и никогда не знала нужды... До тех пор, пока однажды не была похищена, изнасилована и выброшена в трущобах без денег и документов. У неё фантастическое тело, но нет никакого опыта.";
            pTina.m_pDescription.m_cConditions.Add(new ConditionStatus(m_cBoolParameters[0]));
            Reaction pTinaOk = new Reaction();
            pTinaOk.m_sName = "Выбрать Тину";
            pTinaOk.m_cConsequences.Add(new ParameterSet(m_cBoolParameters[0], "1"));
            pTina.m_cReactions.Add(pTinaOk);
            Reaction pTinaCancel = new Reaction();
            pTinaCancel.m_sName = "Выбрать другую шлюху";
            pTinaCancel.m_cConsequences.Add(new SystemCommand(SystemCommand.ActionType.Return));
            pTina.m_cReactions.Add(pTinaCancel);
            m_cEvents.Add(pTina);

            Event pMary = new Event(m_cActions[1]);
            pMary.m_sID = "newGame_Mary";
            pMary.m_pDescription.m_sText = "Мэри выросла в трущобах и никогда не знала другой жизни. Её мать была шлюхой, она никогда не знала кто её отец, у неё дюжина братьев и сестёр, а торговать своим телом она начала в 12. Она не королева красоты, но опыта у неё хоть отбавляй.";
            pMary.m_pDescription.m_cConditions.Add(new ConditionStatus(m_cBoolParameters[0]));
            Reaction pMaryOk = new Reaction();
            pMaryOk.m_sName = "Выбрать Мэри";
            pMaryOk.m_cConsequences.Add(new ParameterSet(m_cBoolParameters[0], "1"));
            pMary.m_cReactions.Add(pMaryOk);
            Reaction pMaryCancel = new Reaction();
            pMaryCancel.m_sName = "Выбрать другую шлюху";
            pMaryCancel.m_cConsequences.Add(new SystemCommand(SystemCommand.ActionType.Return));
            pMary.m_cReactions.Add(pMaryCancel);
            m_cEvents.Add(pMary);

            Event pJane = new Event(m_cActions[2]);
            pJane.m_sID = "newGame_Jane";
            pJane.m_pDescription.m_sText = "Джейн была обычной девушкой из хорошей семьи, но в колледже связалась с дурной компанией. Вскоре её отчислили из колледжа, а затем она убежала из дома. Её парень предложил ей заняться проституцией, чтобы заработать на наркоту, и так она и поступила. Она привлекательна и имеет неплохой опыт.";
            pJane.m_pDescription.m_cConditions.Add(new ConditionStatus(m_cBoolParameters[0]));
            Reaction pJaneOk = new Reaction();
            pJaneOk.m_sName = "Выбрать Джейн";
            pJaneOk.m_cConsequences.Add(new ParameterSet(m_cBoolParameters[0], "1"));
            pJane.m_cReactions.Add(pJaneOk);
            Reaction pJaneCancel = new Reaction();
            pJaneCancel.m_sName = "Выбрать другую шлюху";
            pJaneCancel.m_cConsequences.Add(new SystemCommand(SystemCommand.ActionType.Return));
            pJane.m_cReactions.Add(pJaneCancel);
            m_cEvents.Add(pJane);

            Event pAl = new Event(m_cActions[3]);
            pAl.m_sID = "newGame_Al";
            pAl.m_pDescription.m_sText = "Ал - транс. Он всегда мечтал быть женщиной. Его родители выгнали его из дома, когда он заявил, что он гей. После этого уже 2 года он живёт в трущобах. Со своим частично модифицированным телом он выглядит как некрасивая девушка с огромной грудью.";
            pAl.m_pDescription.m_cConditions.Add(new ConditionStatus(m_cBoolParameters[0]));
            Reaction pAlOk = new Reaction();
            pAlOk.m_sName = "Выбрать Ала";
            pAlOk.m_cConsequences.Add(new ParameterSet(m_cBoolParameters[0], "1"));
            pAl.m_cReactions.Add(pAlOk);
            Reaction pAlCancel = new Reaction();
            pAlCancel.m_sName = "Выбрать другую шлюху";
            pAlCancel.m_cConsequences.Add(new SystemCommand(SystemCommand.ActionType.Return));
            pAl.m_cReactions.Add(pAlCancel);
            m_cEvents.Add(pAl);
        }

        public void SaveXML(string sFilename)
        {
            UniLibXML pXml = new UniLibXML("Persona");

            XmlNode pModuleNode = pXml.CreateNode(pXml.Root, "Module");
            pXml.AddAttribute(pModuleNode, "name", m_sName);
            pXml.AddAttribute(pModuleNode, "desc", m_sDescription);

            XmlNode pParameters = pXml.CreateNode(pModuleNode, "Parameters");
            foreach (NumericParameter pParam in m_cNumericParameters)
            {
                XmlNode pParamNode = pXml.CreateNode(pParameters, "Numeric");
                pParam.WriteXML(pXml, pParamNode);
            }
            foreach (BoolParameter pParam in m_cBoolParameters)
            {
                XmlNode pParamNode = pXml.CreateNode(pParameters, "Bool");
                pParam.WriteXML(pXml, pParamNode);
            }
            foreach (StringParameter pParam in m_cStringParameters)
            {
                XmlNode pParamNode = pXml.CreateNode(pParameters, "String");
                pParam.WriteXML(pXml, pParamNode);
            }

            XmlNode pActionsNode = pXml.CreateNode(pModuleNode, "Actions");
            foreach (Action pAction in m_cActions)
            {
                XmlNode pActionNode = pXml.CreateNode(pActionsNode, "Action");
                pAction.WriteXML(pXml, pActionNode);
            }

            XmlNode pCollectionsNode = pXml.CreateNode(pModuleNode, "Collections");
            foreach (ObjectsCollection pCollection in m_cCollections)
            {
                XmlNode pCollectionNode = pXml.CreateNode(pCollectionsNode, "Collection");
                pCollection.WriteXML(pXml, pCollectionNode);
            }

            XmlNode pEventsNode = pXml.CreateNode(pModuleNode, "Events");
            foreach (Event pEvent in m_cEvents)
            {
                XmlNode pEventNode = pXml.CreateNode(pEventsNode, "Event");
                pEvent.WriteXML(pXml, pEventNode);
            }

            XmlNode pTriggersNode = pXml.CreateNode(pModuleNode, "Triggers");
            foreach (Trigger pTrigger in m_cTriggers)
            {
                XmlNode pTriggerNode = pXml.CreateNode(pTriggersNode, "Trigger");
                pTrigger.WriteXML(pXml, pTriggerNode);
            }

            XmlNode pFunctionsNode = pXml.CreateNode(pModuleNode, "Functions");
            foreach (Function pFunction in m_cFunctions)
            {
                XmlNode pFunctionNode = pXml.CreateNode(pFunctionsNode, "Function");
                pFunction.WriteXML(pXml, pFunctionNode);
            }

            pXml.Write(sFilename);
        }

        public void LoadXML(string sFilename)
        {
            UniLibXML pXml = new UniLibXML("Persona");

            if (!pXml.Load(sFilename))
                return;

            m_cNumericParameters.Clear();
            m_cBoolParameters.Clear();
            m_cStringParameters.Clear();
            m_cActions.Clear();
            m_cEvents.Clear();
            m_cTriggers.Clear();
            m_cFunctions.Clear();
            m_cCollections.Clear();

            if (pXml.Root.ChildNodes.Count == 1 && pXml.Root.ChildNodes[0].Name == "Module")
            {
                XmlNode pModuleNode = pXml.Root.ChildNodes[0];

                pXml.GetStringAttribute(pModuleNode, "name", ref m_sName);
                pXml.GetStringAttribute(pModuleNode, "desc", ref m_sDescription);

                foreach (XmlNode pSection in pModuleNode.ChildNodes)
                {
                    if (pSection.Name == "Parameters")
                    {
                        foreach (XmlNode pParamNode in pSection.ChildNodes)
                        {
                            if (pParamNode.Name == "Numeric")
                            {
                                NumericParameter pParam = new NumericParameter(pXml, pParamNode);
                                m_cNumericParameters.Add(pParam);
                            }
                            if (pParamNode.Name == "Bool")
                            {
                                BoolParameter pParam = new BoolParameter(pXml, pParamNode);
                                m_cBoolParameters.Add(pParam);
                            }
                            if (pParamNode.Name == "String")
                            {
                                StringParameter pParam = new StringParameter(pXml, pParamNode);
                                m_cStringParameters.Add(pParam);
                            }
                        }
                    }

                    if (pSection.Name == "Collections")
                    {
                        foreach (XmlNode pCollectionNode in pSection.ChildNodes)
                        {
                            if (pCollectionNode.Name == "Collection")
                            {
                                ObjectsCollection pCollection = new ObjectsCollection(pXml, pCollectionNode);
                                m_cCollections.Add(pCollection);
                            }
                        }
                    }

                    if (pSection.Name == "Domains")
                    {
                        foreach (XmlNode pDomainNode in pSection.ChildNodes)
                        {
                            if (pDomainNode.Name == "Domain")
                            {
                                Action pDomain = new Action(pXml, pDomainNode);
                                m_cActions.Add(pDomain);
                            }
                        }
                    }
                    if (pSection.Name == "Actions")
                    {
                        foreach (XmlNode pActionNode in pSection.ChildNodes)
                        {
                            if (pActionNode.Name == "Action")
                            {
                                Action pAction = new Action(pXml, pActionNode);
                                m_cActions.Add(pAction);
                            }
                        }
                    }
                }

                List<Parameter> cParams = new List<Parameter>();
                cParams.AddRange(m_cNumericParameters);
                cParams.AddRange(m_cBoolParameters);
                cParams.AddRange(m_cStringParameters);

                //События считываем в отдельном цикле, т.к. для них нам обязательно нужно, чтобы домены и параметры были уже считаны.
                foreach (XmlNode pSection in pModuleNode.ChildNodes)
                {
                    if (pSection.Name == "Events")
                    {
                        foreach (XmlNode pEventNode in pSection.ChildNodes)
                        {
                            if (pEventNode.Name == "Event")
                            {
                                Event pEvent = new Event(pXml, pEventNode, m_cActions, cParams);
                                m_cEvents.Add(pEvent);
                            }
                        }
                    }
                    if (pSection.Name == "Triggers")
                    {
                        foreach (XmlNode pTriggerNode in pSection.ChildNodes)
                        {
                            if (pTriggerNode.Name == "Trigger")
                            {
                                Trigger pTrigger = new Trigger(pXml, pTriggerNode, cParams);
                                m_cTriggers.Add(pTrigger);
                            }
                        }
                    }
                    if (pSection.Name == "Functions")
                    {
                        foreach (XmlNode pFunctionNode in pSection.ChildNodes)
                        {
                            if (pFunctionNode.Name == "Function")
                            {
                                Function pFunction = new Function(pXml, pFunctionNode, cParams);
                                m_cFunctions.Add(pFunction);
                            }
                        }
                    }
                }
            }
        }

        public bool m_bGameOver = false;

        public void Start()
        {
            foreach (var pParam in m_cNumericParameters)
                pParam.Init();
            foreach (var pParam in m_cBoolParameters)
                pParam.Init();
            foreach (var pParam in m_cStringParameters)
                pParam.Init();

            foreach (var pCollection in m_cCollections)
                pCollection.Init();

            m_bGameOver = false;
        }

        public bool m_bRollback = false;

        public bool m_bRandomRound = false;

        public List<Action> GetPossibleActions()
        {
            Dictionary<Action, int> cPossible = new Dictionary<Action, int>();

            int iMaxPriority = int.MinValue;

            if (!m_bGameOver)
            {
                foreach (Event pEvent in m_cEvents)
                {
                    if (cPossible.ContainsKey(pEvent.m_pAction))
                    {
                        if (cPossible[pEvent.m_pAction] < pEvent.m_iPriority)
                        {
                            cPossible[pEvent.m_pAction] = pEvent.m_iPriority;
                            if (iMaxPriority < pEvent.m_iPriority)
                                iMaxPriority = pEvent.m_iPriority;
                        }
                        continue;
                    }

                    bool bPossible = true;

                    foreach (Condition pCondition in pEvent.m_pDescription.m_cConditions)
                    {
                        if (!pCondition.Check())
                        {
                            bPossible = false;
                            break;
                        }
                    }

                    if (bPossible)
                    {
                        cPossible[pEvent.m_pAction] = pEvent.m_iPriority;
                        if (iMaxPriority < pEvent.m_iPriority)
                            iMaxPriority = pEvent.m_iPriority;
                    }
                }
            }

            List<Action> cAvailable = new List<Action>();

            foreach (var pEvent in cPossible)
                if (pEvent.Value == iMaxPriority)
                    cAvailable.Add(pEvent.Key);

            if (!m_bRollback)
                m_cPinnedEvents.Clear();

            m_bRollback = false;

            return cAvailable;
        }

        private Dictionary<Action, Event> m_cPinnedEvents = new Dictionary<Action, Event>();

        internal Event DoAction(Action pAction)
        {
            if (m_bGameOver)
                return null;

            if (m_cPinnedEvents.ContainsKey(pAction))
                return m_cPinnedEvents[pAction];

            List<Event> cPossible = new List<Event>();

            int iMaxPriority = int.MinValue;

            foreach (Event pEvent in m_cEvents)
            {
                if (pEvent.m_pAction.m_sName != pAction.m_sName)
                    continue;

                bool bPossible = true;

                foreach (Condition pCondition in pEvent.m_pDescription.m_cConditions)
                {
                    if (!pCondition.Check())
                    {
                        bPossible = false;
                        break;
                    }
                }

                if (bPossible)
                {
                    for (int i = 0; i < pEvent.m_iProbability; i++ )
                        cPossible.Add(pEvent);
                    if (iMaxPriority < pEvent.m_iPriority)
                        iMaxPriority = pEvent.m_iPriority;
                }
            }

            List<Event> cAvailable = new List<Event>();

            foreach (var pEvent in cPossible)
                if (pEvent.m_iPriority == iMaxPriority)
                    cAvailable.Add(pEvent);

            if (cAvailable.Count == 0)
                return null;

            Event pSelected = cAvailable[Random.Rnd.Get(cAvailable.Count)];

            m_cPinnedEvents[pAction] = pSelected;

            return pSelected;
        }
    }
}
