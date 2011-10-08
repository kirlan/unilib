using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AITAS_Engine.Attributes;
using AITAS_Engine.Skills;

namespace AITAS_Engine
{
    public class Character
    {
        public string m_sName;

        public string m_sAppearance;

        public string m_sBackground;

        public string m_iHomeTL;

        public string m_sPersonalGoal;

        public int m_iStoryPointsBase;

        public int m_iStoryPointsCurrent;

        public Dictionary<Attr, CharAttribute> m_cAttributes = new Dictionary<Attr, CharAttribute>();

        public Dictionary<string, Trait> m_cTraits = new Dictionary<string, Trait>();

        public Dictionary<Skl, Skill> m_cSkills = new Dictionary<Skl, Skill>();

        public Character()
        {
            m_cAttributes[Attr.Awareness] = new AttributeAwareness();
            m_cAttributes[Attr.Coordination] = new AttributeCoordination();
            m_cAttributes[Attr.Ingenuity] = new AttributeIngenuity();
            m_cAttributes[Attr.Presence] = new AttributePresence();
            m_cAttributes[Attr.Resolve] = new AttributeResolve();
            m_cAttributes[Attr.Strength] = new AttributeStrength();

            m_cSkills[Skl.Athletics] = new SkillAthletics();
            m_cSkills[Skl.Convince] = new SkillConvince();
            m_cSkills[Skl.Craft] = new SkillCraft();
            m_cSkills[Skl.Creation] = new SkillCreation();
            m_cSkills[Skl.Entertainment] = new SkillEntertainment();
            m_cSkills[Skl.Fighting] = new SkillFighting();
            m_cSkills[Skl.Knowledge] = new SkillKnowledge();
            m_cSkills[Skl.Marksman] = new SkillMarksman();
            m_cSkills[Skl.Medicine] = new SkillMedicine();
            m_cSkills[Skl.Science] = new SkillScience();
            m_cSkills[Skl.Subterfuge] = new SkillSubterfuge();
            m_cSkills[Skl.Survival] = new SkillSurvival();
            m_cSkills[Skl.Technology] = new SkillTechnology();
            m_cSkills[Skl.Transport] = new SkillTransport();
        }

        public static string GetTLDescription(int iTL)
        {
            switch (iTL)
            {
                case 1: return "Каменный Век";
                case 2: return "Эпоха Железа - от бронзового века до средних веков, мечи и луки";
                case 3: return "Ренесанс - 15-17 века истории Земли, порох, каравеллы, расцвет исскуств";
                case 4: return "Индустриальная Эра - 18-20 века истории Земли, индустриальная революция, паровые машины, мануфактуры";
                case 5: return "Освоение Ближнего Космоса - конец 20 века и начало 21. Межпланетные перелёты в пределах одной звёздной системы";
                case 6: return "Освоение Дальнего Космоса - с конца 21 века до 30. Сверхсветовые звездолёты";
                case 7: return "Золотой век космической цивилизации - Земля далёкого будущего, временные путешествия ещё не открыты, телепортация уже используется";
                case 8: return "Путешествия во времени - Земля 51 века";
                case 9: return "Продвинутые путешествия во времени - Далеки";
                case 10: return "Повелители Времени";
                case 11: return "Древние Повелители Времени - времена Рассилона, Омеги и Зоны Смерти";
                case 12: return "Запредельное - технологии, доступные только Вечным";
                default: return "Неизвестно";
            }
        }
    }
}
