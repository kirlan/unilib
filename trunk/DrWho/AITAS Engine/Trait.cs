using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine
{
    public enum TraitType
    { 
        RollModifer,
        Passive,
        SpecialCase,
        SpecialAbility,
        Acquaintance,
        Gadget
    }

    public class Trait
    {
        public static Trait[] s_aGoodTraits =
        {
            new Trait("Attractive", "Особая привлекательность", 1, TraitType.RollModifer, "Unattractive"),
            new Trait("Brave", "Особая храбрость и бесстрашие", 1, TraitType.RollModifer, new string[] {"Cowardly", "Screamer!"}),
            new Trait("Charming", "Особый шарм", 1, TraitType.RollModifer),
            new Trait("Indomitable", "Особо мощная сила воли", 2, TraitType.RollModifer),
            new Trait("Lucky", "Везунчик", 1, TraitType.SpecialCase),
            new Trait("Photographic Memory", "Фотографическая память", 2, TraitType.SpecialCase, "Forgetful"),
            new Trait("Sense of Direction", "Вы всегда знаете, где север, где низ, и откуда вы пришли", 1, TraitType.RollModifer),

            new Trait("Animal Friendship", "Особое взаимопонимание с животными", 1, TraitType.RollModifer),
            new Trait("Boffin", "Гений-электронщик, может делать гаджеты", 2, TraitType.SpecialAbility, "Technically Inept"),
            new Trait("Empathic", "Может ощущать эмоции других", 1, TraitType.RollModifer),
            new Trait("Hypnosis (Minor)", "Особый дар убеждения", 1, TraitType.RollModifer, new string[] {"Hypnosis (Major)", "Hypnosis (Extra)"}),
            new Trait("Hypnosis (Major)", "При успехе загипнотизированной жертве можно приказать сделать почти что угодно", 2, TraitType.RollModifer, new string[] {"Hypnosis (Minor)", "Hypnosis (Extra)"}),
            new Trait("Psychic Training", "Специальная техника, позволяющая сопротивляться психическим манипуляциям и уловкам", 1, TraitType.RollModifer),
            new Trait("Resourceful Pockets", "Шанс вдруг найти в кармане что-то, что поможет выпутаться из текущей ситуации", 1, TraitType.SpecialAbility),
            new Trait("Run for your Life!", "Сматываться - это исскуство, и вы великолепно им овладели", 1, TraitType.SpecialCase),
            new Trait("Screamer!", "Оглушающий крик, слышимый издалека", 1, TraitType.SpecialAbility, "Brave"),
            new Trait("Technically Adept", "Общий язык с техникой", 1, TraitType.RollModifer, "Technically Inept"),
            new Trait("Voice of Authority", "Когда ваш персонаж говорите - его слушают", 1, TraitType.RollModifer),

            new Trait("Face in the Crowd", "Незапоминающаяся внешность, неприметность", 1, TraitType.RollModifer, "Distinctive"),
            new Trait("Fast Healing", "Ускоренное заживление ран (+1/час)", 2, TraitType.Passive, "Fast Healing (Extra)"),
            new Trait("Keen Sight", "Особо острое зрение", 1, TraitType.RollModifer, new string[] {"Keen Senses", "Impaired Sight (Minor)", "Impaired Sight (Major)", "Colorblind", "Infravision"}),
            new Trait("Keen Hearing", "Особо тонкий слух", 1, TraitType.RollModifer, new string[] {"Keen Senses", "Impaired Hearing (Minor)", "Impaired Hearing (Major)", "Sonar Vision"}),
            new Trait("Keen Touch", "Особо острое осязание", 1, TraitType.RollModifer, new string[] {"Keen Senses"}),
            new Trait("Keen Smell", "Особо тонкое обоняние", 1, TraitType.RollModifer, new string[] {"Keen Senses", "Impaired Smell", "Alien Smell"}),
            new Trait("Keen Taste", "Особо тонкий вкус", 1, TraitType.RollModifer, new string[] {"Keen Senses"}),
            new Trait("Keen Senses", "Особо острые органы чувств (все 5)", 2, TraitType.RollModifer, new string[] {"Keen Sight", "Keen Hearing", "Keen Touch", "Keen Smell", "Keen Taste", "Impaired Sight (Minor)", "Impaired Sight (Major)", "Impaired Hearing (Minor)", "Impaired Hearing (Major)", "Impaired Smell", "Colorblind", "Sonar Vision", "Infravision", "Alien Smell"}),
            new Trait("Quick Reflexes", "Очень быстрая, практически инстинктивная реакция на события", 1, TraitType.SpecialCase, new string[] {"Slow Reflexes (Minor)", "Slow Reflexes (Major)"}),
            new Trait("Tough", "Особая живучесть", 1, TraitType.SpecialCase),

            new Trait("Friends (Minor)", "Связи, дающие доступ к сравнительно надёжной, общедоступной информации", 1, TraitType.SpecialAbility, "Friends (Major)"),
            new Trait("Friends (Major)", "Связи, дающие доступ к крайне надёжной, эксклюзивной информации (UNIT, Торчвуд)", 2, TraitType.SpecialAbility, "Friends (Minor)"),
            new Trait("Owed Fawour (Minor)", "Кое-кто задолжал персонажу мелкую услугу или небольшую сумму денег (до 1000 фунтов стерлингов/~600 долларов)", 1, TraitType.Acquaintance),
            new Trait("Owed Fawour (Major)", "Кое-кто обязан персонажу жизнью или должен крупную сумму денег (больше 10000 фунтов стерлингов/~6000 долларов)", 2, TraitType.Acquaintance),
        };
            
        public static Trait[] s_aBadTraits =
        {
            new Trait("Argumentative", "Персонаж будет отстаивать свою точку зрения даже под угрозой смерти", -1, TraitType.SpecialCase),
            new Trait("By the Book", "Персонаж чтит 'букву закона' превыше здравого смысла", -1, TraitType.RollModifer),
            new Trait("Code of Conduct (Minor)", "У персонажа есть определённые моральные нормы, которым он стремится следовать невзирая на последствия", -1, TraitType.SpecialCase, "Code of Conduct (Major)"),
            new Trait("Code of Conduct (Major)", "У персонажа есть определённый свод жёстких правил, которым он стремится следовать невзирая на последствия", -2, TraitType.SpecialCase, "Code of Conduct (Minor)"),
            new Trait("Clumsy", "Нервный. Находясь под стрессом, наверняка наломает дров", -1, TraitType.Passive),
            new Trait("Cowardly", "Трус", -1, TraitType.SpecialCase, "Brave"),
            new Trait("Eccentric (Minor)", "Персонаж в некоторых ситуациях (когда напуган, счастлив, ревнует, невничает...) ведёт себя странно с точки зрения окружающих", -1, TraitType.SpecialCase, "Eccentric (Major)"),
            new Trait("Eccentric (Major)", "Персонаж постоянно ведёт себя странно с точки зрения окружающих", -2, TraitType.SpecialCase, "Eccentric (Minor)"),
            new Trait("Impulsive", "Персонаж склонен сначала делать, а потом думать", -1, TraitType.SpecialCase),
            new Trait("Insatiable Curiosity", "Любопытство, не знающее границ", -1, TraitType.SpecialCase, new string[] {"Unadventurous (Minor)", "Unadventurous (Major)"}),
            new Trait("Selfish", "Эгоистичный - едва ли будет делать что-то, в чём не увидит собственной выгоды", -1, TraitType.SpecialCase),
            new Trait("Technically Inept", "Есть люди, просто несовместимые с любой сколько-то сложной техникой...", -1, TraitType.RollModifer),
            new Trait("Distinctive", "Непохожий на других, 'белая ворона'", -1, TraitType.RollModifer, "Face in the Crowd"),
            new Trait("Unattractive", "Персонаж не обязательно уродлив, но так или иначе окружающие не в восторге от его внешности", -1, TraitType.RollModifer, "Attractive"),
            new Trait("Unlucky", "Невезучий", -1, TraitType.SpecialCase),
            
            new Trait("Amnesia (Minor)", "У персонажа есть провалы в памяти", -1, TraitType.SpecialCase, "Amnesia (Major)"),
            new Trait("Amnesia (Major)", "Персонаж не помнит абсолютно ничего о своём прошлом", -2, TraitType.SpecialCase, "Amnesia (Minor)"),
            new Trait("Dependency (Minor)", "Лёгкая зависимость от чего-то/кого-то. Персонаж может обходиться без объекта зависимости некоторое время, после которого начинает испытывать лёгкое недомогание", -1, TraitType.RollModifer, "Dependency (Major)"),
            new Trait("Dependency (Major)", "Серьёзная зависимость от чего-то/кого-то. Персонаж не может обходиться без объекта зависимости, будучи лишён его - начинает испытывать серьёзное недомогание", -2, TraitType.RollModifer, "Dependency (Minor)"),
            new Trait("Forgetful", "Склероз, батенька...", -1, TraitType.RollModifer, "Photographic Memory"),
            new Trait("Colorblind", "Персонаж не различает цвета", -1, TraitType.RollModifer, new string[] {"Keen Senses", "Keen Sight", "Impaired Sight (Major)"}),
            new Trait("Impaired Sight (Minor)", "Персонаж нуждается в очках", -1, TraitType.RollModifer, new string[] {"Keen Senses", "Keen Sight", "Impaired Sight (Major)"}),
            new Trait("Impaired Sight (Major)", "Персонаж слепой", -2, TraitType.SpecialCase, new string[] {"Keen Senses", "Keen Sight", "Impaired Sight (Minor)", "Colorblind"}),
            new Trait("Impaired Hearing (Minor)", "Персонаж нуждается в слуховом аппарате", -1, TraitType.RollModifer, new string[] {"Keen Senses", "Keen Hearing", "Impaired Hearing (Major)", "Sonar Vision"}),
            new Trait("Impaired Hearing (Major)", "Персонаж глухой", -2, TraitType.SpecialCase, new string[] {"Keen Senses", "Keen Hearing", "Impaired Hearing (Minor)", "Sonar Vision"}),
            new Trait("Impaired Smell", "Персонаж не различает запахи", -1, TraitType.RollModifer, new string[] {"Keen Senses", "Keen Smell", "Alien Smell"}),
            new Trait("Acrophobia", "Панический страх высоты", -1, TraitType.RollModifer),
            new Trait("Claustrophobia", "Панический страх замкнутых помещений", -1, TraitType.RollModifer),
            new Trait("Aquaphobia", "Панический страх воды", -1, TraitType.RollModifer),
            new Trait("Arachnophobia", "Панический страх пауков", -1, TraitType.RollModifer),
            new Trait("Hemophobia", "Панический страх крови", -1, TraitType.RollModifer),
            new Trait("Xenophobia", "Панический страх пришельцев", -1, TraitType.RollModifer),
            new Trait("Ophidiophobia", "Панический страх змей", -1, TraitType.RollModifer),
            new Trait("Obsession (Minor)", "У персонажа есть навязчивая идея. Она не слишком мешает ему жить обычной жизнью, но иногда он совершает что-то 'странное'", -1, TraitType.SpecialCase, "Obsession (Major)"),
            new Trait("Obsession (Major)", "У персонажа есть навязчивая идея, которой полностью подчинена вся его жизнь.", -2, TraitType.SpecialCase, "Obsession (Minor)"),
            new Trait("Slow Reflexes (Minor)", "Замедленная реакция", -1, TraitType.SpecialCase, new string[] {"Slow Reflexes (Major)", "Quick Reflexes"}),
            new Trait("Slow Reflexes (Major)", "Замедленная реакция", -1, TraitType.SpecialCase, new string[] {"Slow Reflexes (Minor)", "Quick Reflexes"}),
            new Trait("Weakness (Minor)", "Особая уязвимость к чему-либо", -1, TraitType.RollModifer),
            new Trait("Weakness (Major)", "Фатальная уязвимость к чему-либо, 'ахилессова пята'", -2, TraitType.SpecialCase),

            new Trait("Adversary (Minor)", "У персонажа есть не слишком сильный враг, желающий вам навредить", -1, TraitType.Acquaintance),
            new Trait("Adversary (Major)", "У персонажа есть могущественный враг, мечтающий о вашей смерти", -2, TraitType.Acquaintance),
            new Trait("Dark Secret (Minor)", "У персонажа есть некая тайна, которая, будучи раскрыта, может повредить его репутации и отношениям с окружающими", -1, TraitType.SpecialCase, "Dark Secret (Major)"),
            new Trait("Dark Secret (Major)", "У персонажа есть некая тайна, которая, будучи раскрыта, может вызвать существенную агрессию со стороны окружающих", -2, TraitType.SpecialCase, "Dark Secret (Minor)"),
            new Trait("Obligation (Minor)", "Персонаж работает на некоторую незначительную организацию. Он может получать приказы от руководства организации, но мало чем рискует в случае отказа их выполнять.", -1, TraitType.SpecialCase, "Obligation (Major)"),
            new Trait("Obligation (Major)", "Персонаж работает на некоторую могущественную организацию. Получаемые им приказы обязательны к исполнению, даже если это ставит под угрозу жизнь его собственную или его близких.", -2, TraitType.SpecialCase, "Obligation (Minor)"),
            new Trait("Outcast", "Персонаж когда-то сделал что-то, навсегда сделавшее его 'человеком второго сорта' в глазах членов определённой социальной группы", -1, TraitType.RollModifer),
            new Trait("Owes Fawour (Minor)", "Персонаж задолжал кое-кому мелкую услугу или небольшую сумму денег (до 1000 фунтов стерлингов/~600 долларов)", -1, TraitType.Acquaintance),
            new Trait("Owes Fawour (Major)", "Персонаж обязан кое-кому жизнью или должен крупную сумму денег (больше 10000 фунтов стерлингов/~6000 долларов)", -2, TraitType.Acquaintance),
        };

        public static Trait[] s_aSpecialTraits =
        {
            new Trait("Fast Healing (Extra)", "Ускоренное заживление ран (+1/мин)", 3, TraitType.Passive, "Fast Healing"),
            new Trait("Fear Factor x1", "Персонаж знает, как внушить страх", 1, TraitType.RollModifer, new string[] {"Fear Factor x2", "Fear Factor x3"}),
            new Trait("Fear Factor x2", "Персонаж знает, как внушить страх", 2, TraitType.RollModifer, new string[] {"Fear Factor x1", "Fear Factor x3"}),
            new Trait("Fear Factor x3", "Персонаж знает, как внушить страх", 3, TraitType.RollModifer, new string[] {"Fear Factor x1", "Fear Factor x2"}),

            new Trait("Psychic", "Способность читать чужие мысли", 1, "Psychic Training", TraitType.SpecialAbility),
            new Trait("Clairvoyance", "Способность видеть удалённые места, как если бы персонаж сам был там. Требует усиленной концентрации.", 2, "Psychic", TraitType.SpecialAbility),
            new Trait("Precognition", "Способность предвидеть варианты будущего", 1, "Psychic", TraitType.SpecialAbility),
            new Trait("Telepathy", "Способность не только читать, но и передавать мысли", 1, "Psychic", TraitType.SpecialAbility),
            new Trait("Possess", "Персонаж способен полностью подчинить волю жертвы", 1, "Psychic", TraitType.SpecialAbility, new string[] {"Hypnosis (Minor)", "Hypnosis (Major)"}),
            
            new Trait("Cyborg (Minor)", "Персонаж имеет кибернетические импланты, которые пусть с трудом, но можно скрыть под одеждой", 1, TraitType.SpecialCase, new string[] {"Cyborg (Major)", "Robot (Minor)", "Robot (Major)"}),
            new Trait("Cyborg (Major)", "Персонаж имеет кибернетические импланты, однако внешне практически неотличим от обычного человека", 3, TraitType.SpecialCase, new string[] {"Cyborg (Minor)", "Robot (Minor)", "Robot (Major)"}),
            new Trait("Robot (Minor)", "Персонаж является роботом и это видно с первого взгляда", 2, TraitType.SpecialCase, new string[] {"Robot (Major)", "Cyborg (Minor)", "Cyborg (Major)", "Alien"}),
            new Trait("Robot (Major)", "Персонаж является роботом, однако внешне практически неотличим от обычного человека", 4, TraitType.SpecialCase, new string[] {"Robot (Minor)", "Cyborg (Minor)", "Cyborg (Major)", "Alien"}),
            new Trait("Built-in Gadget (Minor)", "В тело персонажа встроен простой гаджет", 1, new string[] {"Cyborg (Minor)", "Cyborg (Major)", "Robot (Minor)", "Robot (Major)"}, TraitType.Gadget),
            new Trait("Built-in Gadget (Major)", "В тело персонажа встроен сложный гаджет", 2, new string[] {"Cyborg (Minor)", "Cyborg (Major)", "Robot (Minor)", "Robot (Major)"}, TraitType.Gadget),
            new Trait("Built-in Gadget (Extra)", "В тело персонажа встроен сверхсложный гаджет", 4, new string[] {"Cyborg (Minor)", "Cyborg (Major)", "Robot (Minor)", "Robot (Major)"}, TraitType.Gadget),

            new Trait("Experienced (Minor)", "Персонаж многое повидал, многому научился", -2, -2, 3, TraitType.SpecialCase, new string[] {"Experienced (Major)", "Inexperienced (Minor)", "Inexperienced (Major)"}),
            new Trait("Experienced (Major)", "Персонаж имеет существенный жизненный опыт", -4, -4, 6, TraitType.SpecialCase, new string[] {"Experienced (Minor)", "Inexperienced (Minor)", "Inexperienced (Major)"}),
            new Trait("Inexperienced (Minor)", "Недостаток жизненного опыта", 2, 2, -3, TraitType.SpecialCase, new string[] {"Experienced (Minor)", "Experienced (Major)", "Inexperienced (Major)"}),
            new Trait("Inexperienced (Major)", "Персонаж практически ничего ещё не видел в жизни", 4, 4, -6, TraitType.SpecialCase, new string[] {"Experienced (Minor)", "Experienced (Major)", "Inexperienced (Minor)"}),
            
            new Trait("Feel the Turn of the Universe", "Персонаж способен ощущать гармонию (и дисгармонию) Вселенной", 1, TraitType.RollModifer),
            //new Trait("Time Agent", "Оперативник Агентства Времени, секретной шпионской организации, использующей путешествия во времени - из 51 века Земли.", 2, 0, 2, TraitType.SpecialCase),
            new Trait("Vortex (Minor)", "Умение управлять ТАРДИС или каким другим устройством для перемещения во времени", 1, TraitType.RollModifer),
            new Trait("Time Traveller 1TL (Minor)", "Опыт жизни в первобытном обществе", 1, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 2TL (Minor)", "Опыт жизни в ранне-средневековом обществе", 1, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 3TL (Minor)", "Опыт жизни в поздне-средневековом обществе", 1, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 4TL (Minor)", "Опыт жизни в ранне-индустриальном обществе", 1, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 5TL (Minor)", "Опыт жизни в поздне-индустриальном обществе", 1, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 6TL (Minor)", "Опыт жизни в развивающемся межзвёздном обществе", 1, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 7TL (Minor)", "Опыт жизни в развитом межзвёздном обществе", 1, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 8TL (Minor)", "Опыт жизни в обществе, практикующем ограниченные путешествия во времни", 1, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 9TL (Minor)", "Опыт жизни в обществе, практикующем неограниченные путешествия во времени", 1, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 2TL (Major)", "Опыт жизни в ранне-средневековом обществе", 2, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 3TL (Major)", "Опыт жизни в поздне-средневековом обществе", 2, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 4TL (Major)", "Опыт жизни в ранне-индустриальном обществе", 2, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 5TL (Major)", "Опыт жизни в поздне-индустриальном обществе", 2, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 6TL (Major)", "Опыт жизни в развивающемся межзвёздном обществе", 2, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 7TL (Major)", "Опыт жизни в развитом межзвёздном обществе", 2, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 8TL (Major)", "Опыт жизни в обществе, практикующем ограниченные путешествия во времни", 2, TraitType.SpecialCase, "Time Lord (Extra)"),
            new Trait("Time Traveller 9TL (Major)", "Опыт жизни в обществе, практикующем неограниченные путешествия во времени", 2, TraitType.SpecialCase, "Time Lord (Extra)"),

            new Trait("Unadventurous (Minor)", "Персонаж не слишком интересуется приключениями и чудесами вселенной - дома куда лучше", -1, TraitType.RollModifer, new string[] {"Unadventurous (Major)", "Insatiable Curiosity"}),
            new Trait("Unadventurous (Major)", "Персонаж не желает иметь ничего общего с приключениями и чудесами вселенной, он хочет вернуться домой и никогда не вспоминать о Докторе", -2, TraitType.RollModifer, new string[] {"Unadventurous (Minor)", "Insatiable Curiosity"}),
        };

        public static Trait[] s_aAlienTraits =
        {
            new Trait("Alien", "Пришелец, но внешне неотличимый от человека", 2, TraitType.SpecialCase, new string[] {"Robot (Minor)", "Robot (Major)"}),
            new Trait("Alien Appearance (Minor)", "Пришелец-гуманоид, внешне явно отличающийся от людей", -2, "Alien", TraitType.SpecialCase, new string[] {"Alien Appearance (Major)", "Time Lord"}),
            new Trait("Alien Appearance (Major)", "Пришелец-негуманоид", -4, "Alien", TraitType.SpecialCase, new string[] {"Alien Appearance (Minor)", "Time Lord"}),
            new Trait("Additional Limbs (Minor)", "Дополнительная пара конечностей (рук или ног) - всего 6", 1, "Alien Appearance (Minor)", TraitType.SpecialCase),
            new Trait("Additional Limbs (Major)", "Две дополнительные пары конечностей (рук или ног) - всего 8", 1, "Alien Appearance (Major)", TraitType.SpecialCase),
            new Trait("Infravision", "Способность видеть в инфракрасном спектре", 1, "Alien", TraitType.RollModifer, new string[] {"Keen Senses", "Keen Sight"}),
            new Trait("Sonar Vision", "Способность 'видеть' c помощью отражённых звуковых волн", 1, "Alien", TraitType.RollModifer, new string[] {"Keen Senses", "Keen Hearing", "Impaired Hearing (Minor)", "Impaired Hearing (Major)"}),
            new Trait("Extraordinary Smell", "Фантастически тонкое обоняние, лучше чем у собаки", 1, "Alien", TraitType.RollModifer, new string[] {"Keen Senses", "Keen Smell", "Impaired Smell"}),
            new Trait("Natural Armour (Minor)", "Природная броня (толстая кожа)", 1, "Alien", TraitType.SpecialCase, new string[] {"Natural Armour (Major)", "Natural Armour (Extra)"}),
            new Trait("Natural Armour (Major)", "Природная броня (чешуя)", 2, "Alien", TraitType.SpecialCase, new string[] {"Natural Armour (Minor)", "Natural Armour (Extra)"}),
            new Trait("Natural Armour (Extra)", "Природная броня (костяные пластины)", 3, "Alien", TraitType.SpecialCase, new string[] {"Natural Armour (Major)", "Natural Armour (Minor)"}),
            new Trait("Natural Weapons (Minor)", "Природное оружие ближнего боя (клыки, когти, шипы, электрошокер...)", 1, "Alien", TraitType.SpecialCase),
            new Trait("Natural Weapons (Major)", "Природное оружие, действующее на небольшом расстоянии, но не требущее физического контакта (ядовитое дыхание, встроенный огнемёт или дробовик...)", 2, "Alien", TraitType.SpecialCase),
            new Trait("Natural Weapons (Extra)", "Природное оружие, действующее на большом расстоянии (встроенная пушка или лучемёт...)", 4, "Alien", TraitType.SpecialCase),
            new Trait("Shapeshift (Minor)", "Персонаж способен принимать облик представителя другой расы, но не слишком убедительно", 1, "Alien", TraitType.SpecialAbility, new string[] {"Shapeshift (Major)", "Shapeshift (Extra)"}),
            new Trait("Shapeshift (Major)", "Персонаж способен физически трансформироваться в представителя другой расы", 2, "Alien", TraitType.SpecialAbility, new string[] {"Shapeshift (Minor)", "Shapeshift (Extra)"}),
            new Trait("Shapeshift (Extra)", "Персонаж способен физически трансформироваться в представителей нескольких других рас и копировать внешность других персонажей", 4, "Alien", TraitType.SpecialAbility, new string[] {"Shapeshift (Minor)", "Shapeshift (Major)"}),
            new Trait("Dweller: Underwater", "Персонаж способен жить под водой", 1, "Alien", TraitType.Passive, "Environmental"),
            new Trait("Dweller: Volcano", "Персонаж способен жить при экстремально высоких температурах", 1, "Alien", TraitType.Passive, "Environmental"),
            new Trait("Dweller: Icescape", "Персонаж способен жить при экстремально низких температурах", 1, "Alien", TraitType.Passive, "Environmental"),
            new Trait("Dweller: Void", "Персонаж способен жить в вакууме", 1, "Alien", TraitType.Passive, "Environmental"),
            new Trait("Environmental", "Персонаж способен жить в любом окружении", 2, "Alien", TraitType.Passive, new string[] {"Dweller: Underwater", "Dweller: Volcano", "Dweller: Icescape", "Dweller: Void"}),
            new Trait("Climbing (Minor)", "Персонаж может легко и непринуждённо лазить по отвесным стенам (если только они не сделаны из абсолютно гладкого материала, как стекло)", 1, "Alien", TraitType.SpecialAbility, "Climbing (Major)"),
            new Trait("Climbing (Major)", "Персонаж может легко и непринуждённо лазить по любым поверхностям и потолку", 2, "Alien", TraitType.SpecialAbility, "Climbing (Minor)"),
            new Trait("Flight (Minor)", "Персонаж способен парить не слишком высоко над землёй и медленно перемещаться по воздуху", 1, "Alien", TraitType.SpecialAbility, "Flight (Major)"),
            new Trait("Flight (Major)", "Персонаж способен летать как птица - быстро и на любой высоте", 2, "Alien", TraitType.SpecialAbility, "Flight (Minor)"),
            new Trait("Teleport", "Персонаж способен телепортироваться без использования каких-либо вспомогательных устройств", 2, "Alien", TraitType.SpecialAbility),
            new Trait("Vortex (Major)", "Способность перемещаться во времени без использования ТАРДИС или любого другого вспомогательного устройства", 8, "Alien", TraitType.SpecialAbility),

            new Trait("Networked (Minor)", "Персонаж способен чувствовать других представителей своей расы в округе, знает, когда у кого-то из них проблемы и способен действовать синхронно с ними", 1, "Alien", TraitType.SpecialCase, "Networked (Major)"),
            new Trait("Networked (Major)", "Персонаж находится в постоянном мысленном контакте с другими представителями своей расы в округе", 2, "Alien", TraitType.SpecialCase, "Networked (Minor)"),
            new Trait("Immunity", "Персонаж абсолютно неуязвим к какому-либо виду урона (пули, кислота, яд...)", 2, "Alien", TraitType.SpecialCase),
            new Trait("Immortal (Minor)", "Персонаж не стареет и не может умереть от старости, но может погибнуть от любой другой причины", 2, "Alien", TraitType.SpecialCase, "Immortal (Major)"),
            new Trait("Immortal (Major)", "Персонаж абсолютно бессмертен", 5, 0, 4, "Alien", TraitType.SpecialCase, "Immortal (Minor)"),
            
            new Trait("Enslaved", "Персонаж является членом порабощённой или искуственно созданной для служения расы", -2, "Alien", TraitType.RollModifer),
            new Trait("Last of My Kind", "Последний представитель своей расы (во всяком случае, так думает сам персонаж)", -1, "Alien", TraitType.RollModifer),

            new Trait("Time Lord (Minor)", "Неиницированный Повелитель Времени. Персонаж обладает врождёнными расовыми качествами Повелителей Времени: способность к регенерации, повышенная живучесть и высокий интеллект", 0, 0, 4, "Alien", TraitType.RollModifer, new string[] {"Time Lord (Major)", "Time Lord (Extra)"}),
            new Trait("Time Lord (Major)", "Инициированный Повелитель Времени. Персонаж обладает всеми врождёнными качествами Повелителей Времени (способность к регенерации, повышенная живучесть, высокий интеллект), а так же способен чувствовать гармонию Вселенной и взаимодействовать с Временным Вихрем", 1, 0, 4, "Alien", TraitType.RollModifer, new string[] {"Time Lord (Minor)", "Time Lord (Extra)"}),
            new Trait("Time Lord (Extra)", "Опытный Повелитель Времени. Персонаж обладает всеми врождёнными качествами Повелителей Времени, способен чувствовать гармонию Вселенной и взаимодействовать с Временным Вихрем, а так же имеет значительный опыт путешествий во времени", 2, 0, 4, "Alien", TraitType.RollModifer, new string[] {"Time Lord (Minor)", "Time Lord (Major)"}),
        };

        public static Trait[] s_aGadgetTraits =
        {
            new Trait("Delete", "Гаджет может изъять предмет из реальности, сохранив его в своей памяти", 1, TraitType.SpecialAbility),
            new Trait("Forcefild (Minor)", "Гаджет может создать слабое защитное силовое поле", 1, TraitType.SpecialCase),
            new Trait("Forcefild (Major)", "Гаджет может создать сильное защитное силовое поле", 2, TraitType.SpecialCase),
            new Trait("One Shot", "Гаджет может быть использован только 1 раз", -1, TraitType.SpecialCase),
            new Trait("Open/Close", "Гаджет может открывать и закрывать замки без ключа", 1, TraitType.SpecialAbility),
            new Trait("Restriction", "Гаджет имеет существенное ограничение (не работает на определённых целях, требует специального топлива, имеет сложное управление...)", -1, TraitType.SpecialCase),
            new Trait("Scan", "Гаджет может считывать некоторую информацию со сканируемого предмета на небольшом расстоянии", 1, TraitType.SpecialAbility),
            new Trait("Transmit", "Гаджет может передавать, принимать и перехватывать сигналы (радио, телефонные линии, луч телепортера...)", 1, TraitType.SpecialAbility),
            new Trait("Teleport", "Гаджет может осуществлять телепортацию на дистанцию до 400км.", 2, TraitType.SpecialAbility),
            new Trait("Weld", "Гаджет может генерировать высокую температуру, используемую для поджигания, прожигания или сваривания", 1, TraitType.SpecialAbility),
        };

        public TraitType m_eType;

        public int m_iCostCP;
        public int m_iCostSkill = 0;
        public int m_iCostStory = 0;

        public string m_sName;

        public string m_sDescription;

        public List<string> m_cPrerequisites = new List<string>();

        public List<string> m_cAntipods = new List<string>();

        private TraitType traitType;

        public Trait(string sName, string sDescription, int iCost, TraitType eType)
        {
            m_sName = sName;
            m_sDescription = sDescription;
            m_iCostCP = iCost;
            m_eType = eType;
        }

        public Trait(string sName, string sDescription, int iCost, string sPrerequisite, TraitType eType)
            : this(sName, sDescription, iCost, eType)
        {
            m_cPrerequisites.Add(sPrerequisite);
        }

        public Trait(string sName, string sDescription, int iCost, string[] aPrerequisites, TraitType eType)
            : this(sName, sDescription, iCost, eType)
        {
            m_cPrerequisites.AddRange(aPrerequisites);
        }

        public Trait(string sName, string sDescription, int iCost, string sPrerequisite, TraitType eType, string sAntipod)
            : this(sName, sDescription, iCost, eType, sAntipod)
        {
            m_cPrerequisites.Add(sPrerequisite);
        }

        public Trait(string sName, string sDescription, int iCost, string sPrerequisite, TraitType eType, string[] aAntipods)
            : this(sName, sDescription, iCost, eType, aAntipods)
        {
            m_cPrerequisites.Add(sPrerequisite);
        }

        public Trait(string sName, string sDescription, int iCost, TraitType eType, string sAntipod)
            : this(sName, sDescription, iCost, eType)
        {
            m_cAntipods.Add(sAntipod);
        }

        public Trait(string sName, string sDescription, int iCostCP, int iCostSkill, int iCostStory, TraitType eType)
            : this(sName, sDescription, iCostCP, eType)
        {
            m_iCostSkill = iCostSkill;
            m_iCostStory = iCostStory;
        }

        public Trait(string sName, string sDescription, int iCostCP, int iCostSkill, int iCostStory, string sPrerequisite, TraitType eType)
            : this(sName, sDescription, iCostCP, iCostSkill, iCostStory, eType)
        {
            m_cPrerequisites.Add(sPrerequisite);
        }

        public Trait(string sName, string sDescription, int iCostCP, int iCostSkill, int iCostStory, TraitType eType, string sAntipod)
            : this(sName, sDescription, iCostCP, eType, sAntipod)
        {
            m_iCostSkill = iCostSkill;
            m_iCostStory = iCostStory;
        }

        public Trait(string sName, string sDescription, int iCostCP, int iCostSkill, int iCostStory, string sPrerequisite, TraitType eType, string sAntipod)
            : this(sName, sDescription, iCostCP, iCostSkill, iCostStory, eType, sAntipod)
        {
            m_cPrerequisites.Add(sPrerequisite);
        }

        public Trait(string sName, string sDescription, int iCostCP, int iCostSkill, int iCostStory, string sPrerequisite, TraitType eType, string[] aAntipods)
            : this(sName, sDescription, iCostCP, iCostSkill, iCostStory, eType, aAntipods)
        {
            m_cPrerequisites.Add(sPrerequisite);
        }

        public Trait(string sName, string sDescription, int iCost, TraitType eType, string[] aAntipods)
            : this(sName, sDescription, iCost, eType)
        {
            m_cAntipods.AddRange(aAntipods);
        }

        public Trait(string sName, string sDescription, int iCostCP, int iCostSkill, int iCostStory, TraitType eType, string[] aAntipods)
            : this(sName, sDescription, iCostCP, eType, aAntipods)
        {
            m_iCostSkill = iCostSkill;
            m_iCostStory = iCostStory;
        }

        public override string ToString()
        {
            return string.Format("[{0}/{1}/{2}] {3}", m_iCostCP, m_iCostSkill, m_iCostStory, m_sName);
        }
    }
}
