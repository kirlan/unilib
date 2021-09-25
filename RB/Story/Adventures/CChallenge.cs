using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Socium;

namespace RB.Story.Adventures
{
    public class CChallenge : CAdventure
    {
        /// <summary>
        /// Как часто это испытание предлагается герою
        /// </summary>
        public enum Occurance
        {
            /// <summary>
            /// Один раз, при первом посещении локации
            /// </summary>
            Unique,
            /// <summary>
            /// До тех пор, пока он его не попробует пройти
            /// </summary>
            Once,
            /// <summary>
            /// Всегда
            /// </summary>
            Repetable
        }

        /// <summary>
        /// Необходимость прохождения испытания
        /// </summary>
        public enum Importance
        {
            /// <summary>
            /// Испытание предлагается сразу, как только герой входит в локацию, отказать не возможно
            /// </summary>
            Essential,
            /// <summary>
            /// Герой может выбирать, браться ли ему за это испытание
            /// </summary>
            Optional,
        }

        public enum ChallengeType
        {
            Body,
            Mind,
            Charisma
        }

        public enum RewardType
        {
            Status,
            Equipment,
            Follower,
            Nothing
        }


        private RewardType m_eReward = RewardType.Nothing;
        public RewardType Reward
        {
            get { return m_eReward; }
        }

        private RewardType m_ePenalty = RewardType.Nothing;
        public RewardType Penalty
        {
            get { return m_ePenalty; }
        }

        public void SetConditions(RewardType eReward, RewardType ePenalty)
        {
            m_eReward = eReward;
            m_ePenalty = ePenalty;
        }

        private CPerson m_pOpponent;

        public CPerson Opponent
        {
            get { return m_pOpponent; }
        }

        private ChallengeType m_eType;
        /// <summary>
        /// Тип испытания
        /// </summary>
        public ChallengeType Type
        {
            get { return m_eType; }
            set { m_eType = value; }
        }

        public Occurance m_eOccurance = Occurance.Once;

        public Importance m_eImportance = Importance.Optional;

        public bool m_bAvailable = true;

        public CChallenge(CPerson pOpponent, Importance eImportance, Occurance eOccurance)
        {
            m_sName = "Встреча с " + pOpponent.ToString();

            m_pOpponent = pOpponent;
            m_eType = ChallengeType.Body;

            m_eImportance = eImportance;
            m_eOccurance = eOccurance;
        }
    }
}
