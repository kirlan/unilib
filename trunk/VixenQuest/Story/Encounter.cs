﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using VixenQuest.World;

namespace VixenQuest
{
    /// <summary>
    /// Встреча с оппонентом. Может быть вырожденной - "шёл-шёл, никого не встретил".
    /// Характеризуется локацией, оппонентом и списком случившихся действий.
    /// </summary>
    public class Encounter
    {
        public List<Action> m_cActions = new List<Action>();

        public Opponent m_pTarget;

        public Location m_pLocation;

        private int m_iCurrentActionIndex = 0;

        public Action CurrentAction
        {
            get { return m_cActions[m_iCurrentActionIndex]; }
        }

        public bool LastAction
        {
            get { return m_iCurrentActionIndex >= m_cActions.Count - 1; }
        }

        public bool FirstAction
        {
            get { return m_iCurrentActionIndex == 0; }
        }

        /// <summary>
        /// Возвращение на рынок
        /// Нужно избавиться от этого, торговать нужно с реальными торговцами на реальных рынках
        /// </summary>
        /// <param name="pVixen"></param>
        /// <param name="pWorld"></param>
        public Encounter(Vixen pVixen, Universe pWorld)
        {
            m_pLocation = pVixen.m_pHome;
            Action pGoingToMarket = new Action(pVixen, pWorld.m_pMarketplace, ActionType.Move);
            m_cActions.Add(pGoingToMarket);

            foreach (Item pLoot in pVixen.Loot.Keys)
            {
                if (pLoot.m_iPrice > 0 && pVixen.Loot[pLoot] > 0)
                {
                    Action pSellAction = new Action(pLoot, pVixen.Loot[pLoot], pWorld.m_pMarketplace);
                    m_cActions.Add(pSellAction);
                }
            }
            Action pBargainAction = new Action(pVixen, pWorld.m_pMarketplace, ActionType.Bargain);
            m_cActions.Add(pBargainAction);

            Action pReturnBackAction = new Action(pVixen, m_pLocation, ActionType.Move);
            m_cActions.Add(pReturnBackAction);

            m_pTarget = null;
            m_iCurrentActionIndex = 0;
            m_pReward = null;
        }

        public Reward m_pReward;

        /// <summary>
        /// Вырожденный энкаунтер - "шёл-шёл, никого не встретил".
        /// Содержит единственное действие - путешествие по локации.
        /// </summary>
        /// <param name="pVixen"></param>
        /// <param name="pTargetLoc"></param>
        public Encounter(Vixen pVixen, Location pTargetLoc, Location pDestination)
        {
            Action pGoingTo = new Action(pVixen, pTargetLoc, pDestination);
            m_cActions.Add(pGoingTo);
        }

        /// <summary>
        /// Обычный энкаунтер - встреча с оппонентом и цепочка вызванных ею действий.
        /// Начинается с отдыха и путешествия по локации, заканчивается сексом.
        /// </summary>
        /// <param name="pVixen"></param>
        /// <param name="pCurrentLocation"></param>
        public Encounter(Vixen pVixen, Opponent pTarget, Location pDestination)
        {
            //Location[] aPath = Universe.GetPathTo(pVixen, pTarget); 
            
            //if(aPath.Length > 1)
            //    throw new ArgumentException("Target too far from hero!");

            Action pLastAction = null;

            int iCounter0 = 0;
            bool bHaveSex = false;
            //пытаемся создать энкаунтер с сексом (в случае неудачи, делаем не больше 100 попыток)
            do
            {
                //начинаем с чистого листа
                m_cActions.Clear();
                bHaveSex = false;
                
                //опционально - небольшая прогулка после предыдущего энкаунтера
                //if (Rnd.OneChanceFrom(2))
                //{
                //    Action pBeginTrawel = new Action(pVixen, pTarget.m_pHome, pDestination);
                //    m_cActions.Add(pBeginTrawel);
                //}
                //первое действие в энкаунтере - отдых
                Action pRest = new Action(pVixen, pTarget.m_pHome, ActionType.Rest);
                m_cActions.Add(pRest);

                //второе действие - продолжение путешествия
                Action pContinueTrawel = new Action(pVixen, pTarget.m_pHome, pDestination);
                m_cActions.Add(pContinueTrawel);
                
                m_pTarget = pTarget;

                //определим тип третьего действия - результат встречи с оппонентом
                ActionType eFirstAction = ActionType.Rest;
                if (pVixen.WannaFuck(m_pTarget) && m_pTarget.WannaFuck(pVixen) && Rnd.OneChanceFrom(2))
                    eFirstAction = ActionType.Seducing; //оба готовы друг-друга трахнуть - будет секс
                else
                {
                    if (pVixen.WannaFuck(m_pTarget))
                        eFirstAction = ActionType.Pursue; //ГГ готов трахнуть оппонента, но оппонент этого не хочет - ловим оппонента
                    else
                        if (m_pTarget.WannaFuck(pVixen))
                            eFirstAction = ActionType.Evade;//Оппонент ловит ГГ
                        else
                            return; //никто никого не хочет - разошлись как в море корабли
                }

                //третье действие - в соответствии с выбором сделанным выше
                pLastAction = new Action(pVixen, pTarget.m_pHome, m_pTarget, eFirstAction);
                m_cActions.Add(pLastAction);

                //определим затраты сил и энергии на третье действие
                int iVixenTiredness = pLastAction.CostToVixen(pVixen);
                int iTargetTiredness = pLastAction.CostToTarget(pVixen);

                pLastAction.SetProgress(pVixen, iVixenTiredness, iTargetTiredness);

                //четвёртое действие - по результатам третьего (собственно секс)
                pLastAction = AddAction(pVixen, pLastAction, iVixenTiredness, iTargetTiredness);
                if (pLastAction != null)
                {
                    //пересчитываем остаток сил и здоровья у обоих участников
                    iVixenTiredness += pLastAction.CostToVixen(pVixen);
                    iTargetTiredness += pLastAction.CostToTarget(pVixen);

                    //если четвёртое действие не по силам одному из участников - отменяем его, иначе записываем в список
                    if (iVixenTiredness >= pVixen.EffectiveStats[Stat.Potency] &&
                        iTargetTiredness >= m_pTarget.Stats[Stat.Potency])
                    {
                        pLastAction = null;
                    }
                    else
                    {
                        pLastAction.SetProgress(pVixen, iVixenTiredness, iTargetTiredness);

                        m_cActions.Add(pLastAction);
                        bHaveSex = true;
                    }
                }

                iCounter0++;
            }
            while (!bHaveSex && iCounter0 < 100);

            m_iCurrentActionIndex = 0;

            //обнуляем потенцию проигравшего после последнего действия
            if (m_cActions[m_cActions.Count - 1].m_iTargetPotency > 0 && m_cActions[m_cActions.Count - 1].m_iVixenPotency > 0)
            {
                if (m_cActions[m_cActions.Count - 1].m_iTargetPotency <= m_cActions[m_cActions.Count - 1].m_iVixenPotency)
                    m_cActions[m_cActions.Count - 1].m_iTargetPotency = 0;
                else
                    m_cActions[m_cActions.Count - 1].m_iVixenPotency = 0;
            }

            m_pReward = Reward.MakeReward(m_pTarget);
        }

        public int Experience
        {
            get
            {
                if (m_pTarget != null)
                    return m_pTarget.EncounterRank;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Определяет возможное развитие событий после указанного действия
        /// </summary>
        /// <param name="pVixen">ГГ</param>
        /// <param name="pLastAction">последнее случившееся действие</param>
        /// <param name="iVixenTiredness">усталость ГГ</param>
        /// <param name="iTargetTiredness">усталость оппонента</param>
        /// <returns></returns>
        private Action AddAction(Vixen pVixen, Action pLastAction, int iVixenTiredness, int iTargetTiredness)
        {
            //мы убегали...
            if (pLastAction.m_eType == ActionType.Evade)
            {
                if (pLastAction.Success(pVixen))
                    return null; //...и нас не поймали
                else
                    if (iTargetTiredness < m_pTarget.Stats[Stat.Potency])
                        return AddPassiveSexAction(pVixen, pLastAction.m_pLocation); //...и нас поймали
                    else
                        return null; //...и нас поймали бы, если бы у преследователя не кончились силы
            }
            //мы догоняли...
            if (pLastAction.m_eType == ActionType.Pursue)
            {
                if (pLastAction.Success(pVixen) && iVixenTiredness < pVixen.EffectiveStats[Stat.Potency])
                    return AddActiveSexAction(pVixen, pLastAction.m_pLocation); //...и догнали!
                else
                    return null; //...и не догнали :(
            }
            //прелюдия к сексу
            if (pLastAction.m_eType == ActionType.Seducing)
            {
                if (pLastAction.Success(pVixen) && iVixenTiredness < pVixen.EffectiveStats[Stat.Potency])
                    return AddActiveSexAction(pVixen, pLastAction.m_pLocation); //инициатива на нашей стороне
                else
                    if (iTargetTiredness < m_pTarget.Stats[Stat.Potency])
                        return AddPassiveSexAction(pVixen, pLastAction.m_pLocation); //инициатива не на нашей стороне
                    else
                        return null; // секс был бы, но все слишком устали
            }
            //продолжение секса
            if (pLastAction.m_eType == ActionType.Fucking ||
                pLastAction.m_eType == ActionType.AssFucking ||
                pLastAction.m_eType == ActionType.OralFucking ||
                pLastAction.m_eType == ActionType.Sado ||
                pLastAction.m_eType == ActionType.Fucked ||
                pLastAction.m_eType == ActionType.AssFucked ||
                pLastAction.m_eType == ActionType.OralFucked ||
                pLastAction.m_eType == ActionType.Maso)
            {
                if ((pLastAction.Passive || iTargetTiredness >= m_pTarget.Stats[Stat.Potency]) &&
                    iVixenTiredness < pVixen.EffectiveStats[Stat.Potency])
                {
                    //инициатива была не на нашей стороне, но противник устал, а мы - нет
                    //мы хотим его?
                    if (pVixen.WannaFuck(m_pTarget))
                        return AddActiveSexAction(pVixen, pLastAction.m_pLocation); //да - значит теперь инициатива наша
                    else
                        return new Action(pVixen, pLastAction.m_pLocation, m_pTarget, ActionType.Evade); //нет - просто сматываемся
                }
                else
                    //мы устали ИЛИ в прошлом действии инициатива была наша - меняемся ролями
                    //если у оппонента ещё есть силы
                    if (iTargetTiredness < m_pTarget.Stats[Stat.Potency])
                    {
                        //и он хочет нас
                        if(m_pTarget.WannaFuck(pVixen))
                            return AddPassiveSexAction(pVixen, pLastAction.m_pLocation); //то он нас трахает
                        else
                            return new Action(pVixen, pLastAction.m_pLocation, m_pTarget, ActionType.Pursue); //иначе - сматывается
                    }
                    //else
                    //    if (iVixenTiredness < pVixen.EffectiveStats[Stat.Potency] * 3)
                    //        return AddActiveSexAction(pVixen, pLastAction.m_pLocation);
                    //    else
                    //        return null;
            }

            return null;
        }

        private Action AddActiveSexAction(Vixen pVixen, Location pLocation)
        {
            List<int> cChances = new List<int>();

            if ((pVixen.Gender == Gender.Male && m_pTarget.Gender == Gender.Male) ||
                (m_pTarget.m_pRace.m_eSapience == Sapience.Animal && pVixen.Gender != Gender.Male))
                cChances.Add(0);
            else
                cChances.Add(pVixen.Skills[VixenSkill.Traditional]);

            if (m_pTarget.m_pRace.m_eSapience == Sapience.Animal && m_pTarget.Gender != Gender.Male)
                cChances.Add(0);
            else
                cChances.Add(pVixen.Skills[VixenSkill.Oral]);
            
            if (m_pTarget.m_pRace.m_eSapience == Sapience.Animal && pVixen.Gender != Gender.Male)
                cChances.Add(0);
            else
                cChances.Add(pVixen.Skills[VixenSkill.Anal]);
            
            if (m_pTarget.m_pRace.m_eSapience == Sapience.Animal)
                cChances.Add(0);
            else
                cChances.Add(pVixen.Skills[VixenSkill.SM]);

            int iChoosen = Rnd.ChooseOne(cChances, 3);

            ActionType pNewAction = ActionType.Rest;

            switch (iChoosen)
            {
                case 0:
                    pNewAction = ActionType.Fucking; break;
                case 1:
                    pNewAction = ActionType.OralFucking; break;
                case 2:
                    pNewAction = ActionType.AssFucking; break;
                case 3:
                    pNewAction = ActionType.Sado; break;
            }

            if (pNewAction != ActionType.Rest)
                return new Action(pVixen, pLocation, m_pTarget, pNewAction);
            else
                return null;
        }

        private Action AddPassiveSexAction(Vixen pVixen, Location pLocation)
        {
            List<int> cChances = new List<int>();

            if ((pVixen.Gender == Gender.Male && m_pTarget.Gender == Gender.Male) ||
                (m_pTarget.m_pRace.m_eSapience == Sapience.Animal && m_pTarget.Gender != Gender.Male))
                cChances.Add(0);
            else
                cChances.Add(m_pTarget.Skills[VixenSkill.Traditional]);
            
            if (m_pTarget.m_pRace.m_eSapience == Sapience.Animal && pVixen.Gender == Gender.Male)
                cChances.Add(0);
            else
                cChances.Add(m_pTarget.Skills[VixenSkill.Oral]);
            
            if (m_pTarget.m_pRace.m_eSapience == Sapience.Animal && m_pTarget.Gender != Gender.Male)
                cChances.Add(0);
            else
                cChances.Add(m_pTarget.Skills[VixenSkill.Anal]);
            
            if (m_pTarget.m_pRace.m_eSapience == Sapience.Animal)
                cChances.Add(0);
            else
                cChances.Add(m_pTarget.Skills[VixenSkill.SM]);

            int iChoosen = Rnd.ChooseOne(cChances, 3);

            ActionType pNewAction = ActionType.Rest;

            switch (iChoosen)
            {
                case 0:
                    pNewAction = ActionType.Fucked; break;
                case 1:
                    pNewAction = ActionType.OralFucked; break;
                case 2:
                    pNewAction = ActionType.AssFucked; break;
                case 3:
                    pNewAction = ActionType.Maso; break;
            }

            if (pNewAction != ActionType.Rest)
                return new Action(pVixen, pLocation, m_pTarget, pNewAction);
            else
                return null;
        }

        public bool m_bFinished = false;

        public void Advance()
        {
            if (m_bFinished)
                return;

            //if (CurrentAction.m_eType != ActionType.Move
            //   && CurrentAction.m_eType != ActionType.SellLoot)
            //    m_iProgress++;
            
            Action pLastAction = CurrentAction;

            m_iCurrentActionIndex++;
            
            if (m_iCurrentActionIndex >= m_cActions.Count)
            {
                m_iCurrentActionIndex = m_cActions.Count - 1;
                m_bFinished = true;
            }
        }

        public bool Succcess
        {
            get
            {
                if (m_pTarget == null)
                    return true;

                if (CurrentAction.m_iTargetPotency < CurrentAction.m_iVixenPotency)
                    return true;
                else
                    return false;
            }
        }
    }
}
