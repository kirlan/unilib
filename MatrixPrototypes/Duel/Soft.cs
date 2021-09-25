using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace Duel
{
    public class Soft
    {
        public enum Category
        {
            Tracer,
            Interceptor,
            Scaner,
            Chameleon,
            Buzzer,
            BFB_Filter
        }

        public enum Model
        {
            Base,
            Custom,
            Military,
            Black,
            Experimental
        }

        public enum Result
        {
            CritSuccess,
            Success,
            Failure,
            CritFailure
        }

        private Category m_eCATG;

        public Category CATG
        {
            get { return m_eCATG; }
        }

        private int m_iGENR;

        public int GENR
        {
            get { return m_iGENR; }
        }

        private Model m_eMODL;

        public Model MODL
        {
            get { return m_eMODL; }
        }

        public bool Quick
        {
            get { return IsQuick(m_eCATG); }
        }

        public static bool IsQuick(Category eCATG)
        {
            switch (eCATG)
            {
                case Category.Interceptor:
                    return false;
                case Category.Chameleon:
                    return false;
                case Category.BFB_Filter:
                    return false;
                case Category.Scaner:
                    return false;
                case Category.Tracer:
                    return true;
                case Category.Buzzer:
                    return true;
            }

            return false;
        }

        public Soft(Category eCATG, int iGENR, Model eMODL)
        {
            m_eCATG = eCATG;
            m_iGENR = iGENR;
            m_eMODL = eMODL;
        }

        public override string ToString()
        {
            return String.Format("{0} v.{1}.0 ({2} Mod)", CATG.ToString().Replace('_', ' '), GENR, MODL.ToString().Replace('_', ' '));
        }

        private int GetEfficiency(Cowboy pOwner)
        {
            int iSkill = 0;
            switch (m_eCATG)
            {
                case Category.Interceptor:
                    iSkill = pOwner.TUNE.Value;
                    break;
                case Category.Chameleon:
                    iSkill = pOwner.TUNE.Value;
                    break;
                case Category.BFB_Filter:
                    iSkill = pOwner.TUNE.Value;
                    break;
                case Category.Scaner:
                    iSkill = pOwner.TUNE.Value;
                    break;
                case Category.Tracer:
                    iSkill = pOwner.HACK.Value;
                    break;
                case Category.Buzzer:
                    iSkill = pOwner.HACK.Value;
                    break;
            }

            int iRes = iSkill + m_iGENR + Rnd.Toss1d6() + Rnd.Toss1d6();

            if (m_eMODL == Model.Military)
            {
                int iRes2 = iSkill + m_iGENR + Rnd.Toss1d6() + Rnd.Toss1d6();
                if (iRes2 > iRes)
                    iRes = iRes2;
            }

            if (m_eMODL == Model.Custom)
            {
                int iRes2 = iSkill + m_iGENR + Rnd.Toss1d6() + Rnd.Toss1d6();
                if (iRes2 < iRes)
                    iRes = iRes2;
            }

            return iRes;
        }

        public static Result Attack(Cowboy pAvatar1, Soft pSoft1, Cowboy pAvatar2, Soft pSoft2)
        {
            int iEff1 = pSoft1.GetEfficiency(pAvatar1);
            int iEff2 = pSoft2.GetEfficiency(pAvatar2);

            int iDelta = 9;

            if(pSoft1.m_eMODL == Model.Experimental)
                iDelta /= 2;

            if(pSoft2.m_eMODL == Model.Experimental)
                iDelta /= 2;

            if (iEff1 > iEff2 + iDelta && pSoft2.MODL != Model.Black)
                return Result.CritSuccess;

            if (iEff1 + iDelta < iEff2 && pSoft1.MODL != Model.Black)
                return Result.CritFailure;

            if (iEff1 >= iEff2)
                return Result.Success;
            else
                return Result.Failure;
        }

        public void Run(Cowboy pOwner, Cowboy pOpponent)
        {
            switch (m_eCATG)
            {
                case Category.Interceptor:
                    break;
                case Category.Chameleon:
                    pOwner.Invisible = true;
                    break;
                case Category.BFB_Filter:
                    break;
                case Category.Scaner:
                    {
                        Soft pCham = pOpponent.GetSoft(Category.Chameleon);
                        if (pCham.m_iGENR > 0)
                        {
                            Result eRes = Attack(pOwner, this, pOpponent, pCham);
                            switch (eRes)
                            {
                                case Result.CritSuccess:
                                    pOpponent.Invisible = false;
                                    pOwner.Event("Enemy detected!");
                                    pOpponent.Event(pCham.ToString() + " abnormally terminated!");
                                    pOpponent.KillSoft(Category.Chameleon);
                                    break;
                                case Result.Success:
                                    pOpponent.Invisible = false;
                                    pOwner.Event("Enemy detected!");
                                    break;
                                case Result.Failure:
                                    pOpponent.Event("Stay invisible...");
                                    break;
                                case Result.CritFailure:
                                    pOwner.Event(this.ToString() + " abnormally terminated!");
                                    pOwner.KillSoft(this.m_eCATG);
                                    break;
                            }
                        }
                    }
                    break;
                case Category.Tracer:
                    {
                        if (pOpponent.Invisible)
                        {
                            pOwner.Event("Enemy not detected!");
                            //pOwner.Event(this.ToString() + " terminated!");
                            pOwner.KillSoft(this.m_eCATG);
                        }
                        else
                        {
                            Soft pInterceptor = pOpponent.GetSoft(Category.Interceptor);
                            Result eRes = Attack(pOwner, this, pOpponent, pInterceptor);
                            switch (eRes)
                            {
                                case Result.CritSuccess:
                                    pOwner.Event("Enemy traced 2 points!");
                                    pOpponent.Damage(2);
                                    break;
                                case Result.Success:
                                    pOwner.Event("Enemy traced 1 point!");
                                    pOpponent.Damage(1);
                                    break;
                                case Result.Failure:
                                    pOwner.Event("Trace attempt failed!");
                                    if (pInterceptor.m_iGENR > 0)
                                        pOpponent.Event("Interceptor: trace attempt prevented!");
                                    break;
                                case Result.CritFailure:
                                    pOwner.Event("Trace attempt failed!");
                                    if (pInterceptor.m_iGENR > 0)
                                    {
                                        pOpponent.Event("Interceptor: traced 1 point!");
                                        pOwner.Damage(1);
                                    }
                                    break;
                            }

                            //pOwner.Event(this.ToString() + " terminated!");
                            pOwner.KillSoft(this.m_eCATG);
                        }
                    }
                    break;
                case Category.Buzzer:
                    {
                        if (pOpponent.Invisible)
                        {
                            pOwner.Event("Enemy not detected!");
                            //pOwner.Event(this.ToString() + " terminated!");
                            pOwner.KillSoft(this.m_eCATG);
                        }
                        else
                        {
                            Soft pFilter = pOpponent.GetSoft(Category.BFB_Filter);
                            Result eRes = Attack(pOwner, this, pOpponent, pFilter);
                            switch (eRes)
                            {
                                case Result.CritSuccess:
                                    pOwner.Event("Enemy fully neutralized!");
                                    pOpponent.Paralize(true);
                                    break;
                                case Result.Success:
                                    pOwner.Event("Enemy partially neutralized!");
                                    pOpponent.Paralize(false);
                                    break;
                                case Result.Failure:
                                    pOwner.Event("Neutralization attempt failed!");
                                    if (pFilter.m_iGENR > 0)
                                        pOpponent.Event("BFB Filter: neutralization attempt prevented!");
                                    break;
                                case Result.CritFailure:
                                    pOwner.Event("Neutralization attempt failed!");
                                    if (pFilter.m_iGENR > 0)
                                    {
                                        pOpponent.Event("BFB Filter: noize signal reflected!");
                                        pOwner.Paralize(true);
                                    }
                                    break;
                            }

                            //pOwner.Event(this.ToString() + " terminated!");
                            pOwner.KillSoft(this.m_eCATG);
                        }
                    }
                    break;
            }
        }
    }
}
