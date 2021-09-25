using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Solutions;
using Random;
using RB.Story.Solutions.Threat;
using RB.Socium;

namespace RB.Story.Troubles.Harm
{
    public abstract class CHarm : CTrouble
    {
        public enum HarmLiquidationType
        {
            Direct,
            Alternative,
            Impossible
        }

        private CPerson m_pVillain;
        protected bool m_bCanBeRestored;
        /// <summary>
        /// Может ли нанесённый урон быть ликвидирован путём восстановления повреждённого объекта
        /// </summary>
        public bool CanBeRestored
        {
            get { return m_bCanBeRestored; }
        }

        protected bool m_bCanBeReplaced;
        /// <summary>
        /// Может ли нанесённый урон быть ликвидирован путём замены повреждённого объекта
        /// </summary>
        public bool CanBeReplaced
        {
            get { return m_bCanBeReplaced; }
        }
        /// <summary>
        /// Главный злодей-вредитель
        /// </summary>
        public CPerson Villain
        {
            get { return m_pVillain; }
        }

        private bool m_bThreat;
        /// <summary>
        /// Является вред уже нанесённым, или ещё только угрозой?
        /// </summary>
        public bool Threat
        {
            get { return m_bThreat; }
            set { m_bThreat = value; }
        }

        public CHarm(CPerson pVictim, CPerson pVillain)
            : base(pVictim)
        {
            m_pVillain = pVillain;

            m_bCanBeRestored = true;
            m_bCanBeReplaced = true;
        }

        public override CPlot GetSolution()
        {
            if (Threat)
            {
                if(Rnd.Chances(1, 2))
                    return new CDefenceSeeking(this, GetLiquidation());
                else
                    return new CVillainPunishment(this, GetLiquidation());
            }

            return GetLiquidation();
        }

        protected HarmLiquidationType ChooseSolution()
        {
            List<HarmLiquidationType> cLiquidation = new List<HarmLiquidationType>();

            if (CanBeRestored)
                cLiquidation.Add(HarmLiquidationType.Direct);
            if (CanBeReplaced)
                cLiquidation.Add(HarmLiquidationType.Alternative);

            if (cLiquidation.Count == 0)
                cLiquidation.Add(HarmLiquidationType.Impossible);

            int iChoice = Rnd.Get(cLiquidation.Count);

            return cLiquidation[iChoice];
        }

        protected abstract CHarmLiquidation GetLiquidation();

    }
}
