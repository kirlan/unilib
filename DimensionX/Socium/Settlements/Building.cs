using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace Socium.Settlements
{
    public enum BuildingSize
    {
        Unique,
        Small,
        Medium,
        Large,
        Huge
    }
    
    public class BuildingInfo
    {
        public string m_sName;
        public string m_sOwnerM;
        public string m_sOwnerF;

        public int m_iMinPop;
        public int m_iMaxPop;

        public BuildingSize m_eSize;
        
        public BuildingInfo(string sName, string sOwnerM, string sOwnerF, BuildingSize eSize)
        {
            m_sName = sName;
            m_sOwnerM = sOwnerM;
            m_sOwnerF = sOwnerF;

            m_eSize = eSize;

            switch (m_eSize)
            {
                case BuildingSize.Unique:
                    m_iMinPop = 1;
                    m_iMaxPop = 1;
                    break;
                case BuildingSize.Small:
                    m_iMinPop = 5;
                    m_iMaxPop = 15;
                    break;
                case BuildingSize.Medium:
                    m_iMinPop = 10;
                    m_iMaxPop = 30;
                    break;
                case BuildingSize.Large:
                    m_iMinPop = 15;
                    m_iMaxPop = 45;
                    break;
                case BuildingSize.Huge:
                    m_iMinPop = 20;
                    m_iMaxPop = 70;
                    break;
            }
        }
    }

    public class Building
    {
        public Settlement m_pSettlement;

        public BuildingInfo m_pInfo;

        /// <summary>
        /// Центральное здание
        /// </summary>
        /// <param name="pSettlement">поселение</param>
        /// <param name="pInfo">шаблон здания</param>
        /// <param name="bCapital">в этом здании живёт местный правитель (false) или глава всего государства (true)</param>
        public Building(Settlement pSettlement, BuildingInfo pInfo, bool bCapital)
        {
            m_pSettlement = pSettlement;
            m_pInfo = pInfo;

            /*
            AddPopulation(1 + Rnd.Get(5), Settlement.Info[pSettlement.m_eSize].m_iMaxProfessionRank, pSettlement.m_pLand.m_pState.m_iTier * pSettlement.m_pState.m_iTier);

            GenerateName();

            if (bCapital)
            {
                foreach (Opponent pRuler in m_pState.m_cRulers)
                {
                    m_cDwellers.Add(pRuler);
                    pRuler.m_pHome = this;
                }
                foreach (Opponent pHeir in m_pState.m_cHeirs)
                {
                    m_cDwellers.Add(pHeir);
                    pHeir.m_pHome = this;
                }
            }
            else
            {
                ValuedString[] profM = { new ValuedString(pInfo.m_sOwnerM, pInfo.m_iRank) };
                ValuedString[] profF = { new ValuedString(pInfo.m_sOwnerF, pInfo.m_iRank) };
                Opponent pFounder = new Opponent(this, m_pState.GetRandomRace(State.DwellersCathegory.MajorRaces),
                    profM, profF, true, null);
                m_cDwellers.Add(pFounder);

                int iPop = Rnd.Get(5);
                for (int i = 0; i < iPop; i++)
                {
                    Opponent pPretendent = new Opponent(this, m_pState.GetRandomRace(State.DwellersCathegory.MajorRaces),
                        profM, profF, true, pFounder);
                    m_cDwellers.Add(pPretendent);
                }
            }
             */
        }

        /// <summary>
        /// Произвольное здание в поселении
        /// </summary>
        /// <param name="pSettlement"></param>
        public Building(Settlement pSettlement, Province pProvince)
        {
            m_pSettlement = pSettlement;

            int iInfrastructureLevel = ((State)pProvince.Owner).m_iInfrastructureLevel;
            int iControl = ((State)pProvince.Owner).m_iControl;

            switch (m_pSettlement.m_pInfo.m_eSize)
            {
                case SettlementSize.Hamlet:
                    if (pSettlement.m_cBuildings.Count > 0)// && Rnd.OneChanceFrom(2))
                    {
                        if (((State)pProvince.Owner).m_iSocialEquality == 0 && Rnd.OneChanceFrom(4))
                        {
                            m_pInfo = new BuildingInfo("Slave Pens", "slave", "slave", BuildingSize.Small);
                        }
                        else
                        {
                            if (Rnd.Get(4) < iControl ||
                               (iInfrastructureLevel < 2 && Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Piety][pProvince.m_iInfrastructureLevel]))
                            {
                                if (iInfrastructureLevel < 2)
                                    m_pInfo = new BuildingInfo("Hut", "warrior", "warrior", BuildingSize.Small);
                                else
                                    if (iInfrastructureLevel < 4)
                                        m_pInfo = new BuildingInfo("Guard Tower", "guard", "guard", BuildingSize.Small);
                                    else
                                        if (iInfrastructureLevel == 8)
                                            m_pInfo = new BuildingInfo("Emergency Post", "watcher", "watcher", BuildingSize.Small);
                                        else
                                            m_pInfo = new BuildingInfo("Police Station", "policeman", "policeman", BuildingSize.Small);
                            }
                            else
                                if (iInfrastructureLevel < 2 && Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Piety][pProvince.m_iInfrastructureLevel])
                                    m_pInfo = new BuildingInfo("Hut", "shaman", "shaman", BuildingSize.Small);

                        }
                    }

                    if (m_pInfo == null)
                    {
                        switch (m_pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.Fishers:
                                m_pInfo = new BuildingInfo("Fishing boat", "fisher", "fisher", BuildingSize.Small);
                                break;
                            case SettlementSpeciality.Farmers:
                                m_pInfo = new BuildingInfo("Farm", "farmer", "farmer", BuildingSize.Small);
                                break;
                            case SettlementSpeciality.Peasants:
                                m_pInfo = new BuildingInfo("Hut", "peasant", "peasant", BuildingSize.Small);
                                break;
                            case SettlementSpeciality.Hunters:
                                m_pInfo = new BuildingInfo("Hut", "hunter", "hunter", BuildingSize.Small);
                                break;
                            case SettlementSpeciality.Miners:
                                m_pInfo = new BuildingInfo("Mine", "miner", "miner", BuildingSize.Small);
                                break;
                            case SettlementSpeciality.Lumberjacks:
                                m_pInfo = new BuildingInfo("Hut", "lumberjack", "lumberjack", BuildingSize.Small);
                                break;
                        }
                    }
                    break;
                case SettlementSize.Village:
                    if (pSettlement.m_cBuildings.Count > 0)// && Rnd.OneChanceFrom(2))
                    {
                        if (((State)pProvince.Owner).m_iSocialEquality == 0 && Rnd.OneChanceFrom(3))
                        {
                            m_pInfo = new BuildingInfo("Slave Pens", "slave", "slave", BuildingSize.Small);
                        }
                        else
                        {
                            if (Rnd.Get(4) < iControl)
                            {
                                if (iInfrastructureLevel < 2)
                                    m_pInfo = new BuildingInfo("Hut", "warrior", "warrior", BuildingSize.Small);
                                else
                                    if (iInfrastructureLevel < 4)
                                        m_pInfo = new BuildingInfo("Guard Tower", "guard", "guard", BuildingSize.Small);
                                    else
                                        if (iInfrastructureLevel == 8)
                                            m_pInfo = new BuildingInfo("Emergency Post", "watcher", "watcher", BuildingSize.Small);
                                        else
                                            m_pInfo = new BuildingInfo("Police Station", "policeman", "policeman", BuildingSize.Small);
                            }
                            else
                                if (Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Piety][pProvince.m_iInfrastructureLevel])
                                {
                                    if (iInfrastructureLevel < 2)
                                        m_pInfo = new BuildingInfo("Hut", "shaman", "shaman", BuildingSize.Small);
                                    else
                                        m_pInfo = new BuildingInfo("Church", "priest", "priest", BuildingSize.Small);
                                }
                                else
                                    if (Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][pProvince.m_iInfrastructureLevel])
                                        m_pInfo = new BuildingInfo("Inn", "barman", "barmaid", BuildingSize.Small);
                                    else
                                        if (Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Treachery][pProvince.m_iInfrastructureLevel])
                                            m_pInfo = new BuildingInfo("Hideout", "rogue", "rogue", BuildingSize.Small);
                        }
                    }

                    if(m_pInfo == null)
                    {
                        switch (m_pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.Fishers:
                                m_pInfo = new BuildingInfo("Fishing boat", "fisher", "fisher", BuildingSize.Small);
                                break;
                            case SettlementSpeciality.Farmers:
                                m_pInfo = new BuildingInfo("Farm", "farmer", "farmer", BuildingSize.Small);
                                break;
                            case SettlementSpeciality.Peasants:
                                m_pInfo = new BuildingInfo("Hut", "peasant", "peasant", BuildingSize.Small);
                                break;
                            case SettlementSpeciality.Hunters:
                                m_pInfo = new BuildingInfo("Hut", "hunter", "hunter", BuildingSize.Small);
                                break;
                            case SettlementSpeciality.Miners:
                                m_pInfo = new BuildingInfo("Mine", "miner", "miner", BuildingSize.Small);
                                break;
                            case SettlementSpeciality.Lumberjacks:
                                m_pInfo = new BuildingInfo("Hut", "lumberjack", "lumberjack", BuildingSize.Small);
                                break;
                        }
                    }
                    break;
                case SettlementSize.Town:
                    if (pSettlement.m_cBuildings.Count > 0)// && Rnd.OneChanceFrom(2))
                    {
                        if (((State)pProvince.Owner).m_iSocialEquality == 0 && Rnd.OneChanceFrom(3))
                        {
                            if(Rnd.OneChanceFrom(4))
                                m_pInfo = new BuildingInfo("Slave Market", "slaver", "slaver", BuildingSize.Medium);
                            else
                                m_pInfo = new BuildingInfo("Slave Pens", "slave", "slave", BuildingSize.Medium);
                        }
                        else
                        {
                            if (Rnd.Get(4) < iControl)
                            {
                                if (Rnd.OneChanceFrom(2))
                                {
                                    if (iInfrastructureLevel < 4)
                                            m_pInfo = new BuildingInfo("Guard Tower", "guard", "guard", BuildingSize.Medium);
                                    else
                                        if(iInfrastructureLevel == 8)
                                            m_pInfo = new BuildingInfo("Emergency Post", "watcher", "watcher", BuildingSize.Medium);
                                        else
                                            m_pInfo = new BuildingInfo("Police Station", "policeman", "policeman", BuildingSize.Medium);
                                }
                                else
                                {
                                    if (iInfrastructureLevel < 4)
                                        m_pInfo = new BuildingInfo("Prison", "guard", "guard", BuildingSize.Medium);
                                    else
                                        if (iInfrastructureLevel == 8)
                                            m_pInfo = new BuildingInfo("Holding", "watcher", "watcher", BuildingSize.Medium);
                                        else
                                            m_pInfo = new BuildingInfo("Prison", "policeman", "policeman", BuildingSize.Medium);
                                }
                            }
                            else
                                if (Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Piety][pProvince.m_iInfrastructureLevel])
                                {
                                    if(Rnd.OneChanceFrom(5))
                                        m_pInfo = new BuildingInfo("Temple", "priest", "priest", BuildingSize.Medium);
                                    else
                                        m_pInfo = new BuildingInfo("Church", "priest", "priest", BuildingSize.Small);
                                }
                                else
                                    if (Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][pProvince.m_iInfrastructureLevel])
                                    {
                                        if ((pProvince.m_pCustoms.m_eSexuality == Psichology.Customs.Sexuality.Lecherous && Rnd.OneChanceFrom(2)) ||
                                            (pProvince.m_pCustoms.m_eSexuality == Psichology.Customs.Sexuality.Moderate_sexuality && Rnd.OneChanceFrom(4)))
                                            m_pInfo = new BuildingInfo("Brothel", "whore", "whore", BuildingSize.Medium);
                                        else
                                            m_pInfo = new BuildingInfo("Bar", "barman", "barmaid", BuildingSize.Medium);
                                    }
                                    else
                                        if (Rnd.Get(2f) > pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][pProvince.m_iInfrastructureLevel])
                                            m_pInfo = new BuildingInfo("Theatre", "actor", "actor", BuildingSize.Medium);
                                        else
                                            if (Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Treachery][pProvince.m_iInfrastructureLevel])
                                                m_pInfo = new BuildingInfo("Hideout", "rogue", "rogue", BuildingSize.Medium);
                        }
                    }

                    if (m_pInfo == null)
                    {
                        switch (m_pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.Fishers:
                                m_pInfo = new BuildingInfo("Fishing boat", "fisher", "fisher", BuildingSize.Medium);
                                break;
                            case SettlementSpeciality.Naval:
                                m_pInfo = new BuildingInfo("Patrol boat", "sailor", "sailor", BuildingSize.Medium);
                                break;
                            case SettlementSpeciality.Tailors:
                                m_pInfo = new BuildingInfo("Workshop", "tailor", "tailor", BuildingSize.Small);
                                break;
                            case SettlementSpeciality.Jevellers:
                                m_pInfo = new BuildingInfo("Workshop", "jeveller", "jeveller", BuildingSize.Small);
                                break;
                            case SettlementSpeciality.Factory:
                                m_pInfo = new BuildingInfo("Workshop", "worker", "worker", BuildingSize.Small);
                                break;
                            case SettlementSpeciality.Artisans:
                                m_pInfo = new BuildingInfo("Workshop", "artisan", "artisan", BuildingSize.Small);
                                break;
                        }
                    }
                    break;
                case SettlementSize.City:
                    if (pSettlement.m_cBuildings.Count > 0)// && Rnd.OneChanceFrom(2))
                    {
                        if (((State)pProvince.Owner).m_iSocialEquality == 0 && Rnd.OneChanceFrom(3))
                        {
                            if (Rnd.OneChanceFrom(4))
                                m_pInfo = new BuildingInfo("Slave Market", "slaver", "slaver", BuildingSize.Large);
                            else
                                m_pInfo = new BuildingInfo("Slave Pens", "slave", "slave", BuildingSize.Medium);
                        }
                        else
                        {
                            if (Rnd.Get(4) < iControl)
                            {
                                if (Rnd.OneChanceFrom(2))
                                {
                                    if (iInfrastructureLevel < 4)
                                        m_pInfo = new BuildingInfo("Guard Tower", "guard", "guard", BuildingSize.Medium);
                                    else
                                        if (iInfrastructureLevel == 8)
                                            m_pInfo = new BuildingInfo("Emergency Post", "watcher", "watcher", BuildingSize.Medium);
                                        else
                                            if(Rnd.OneChanceFrom(4))
                                                m_pInfo = new BuildingInfo("Police Department", "policeman", "policeman", BuildingSize.Large);
                                            else
                                                m_pInfo = new BuildingInfo("Police Station", "policeman", "policeman", BuildingSize.Medium);
                                }
                                else
                                {
                                    if (iInfrastructureLevel < 4)
                                        m_pInfo = new BuildingInfo("Prison", "guard", "guard", BuildingSize.Medium);
                                    else
                                        if (iInfrastructureLevel == 8)
                                            m_pInfo = new BuildingInfo("Holding", "watcher", "watcher", BuildingSize.Medium);
                                        else
                                            m_pInfo = new BuildingInfo("Prison", "policeman", "policeman", BuildingSize.Medium);
                                }
                            }
                            else
                                if (Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Piety][pProvince.m_iInfrastructureLevel])
                                {
                                    if (Rnd.OneChanceFrom(5))
                                        m_pInfo = new BuildingInfo("Temple", "priest", "priest", BuildingSize.Medium);
                                    else
                                        m_pInfo = new BuildingInfo("Church", "priest", "priest", BuildingSize.Small);
                                }
                                else
                                    if (Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][pProvince.m_iInfrastructureLevel])
                                    {
                                        if ((pProvince.m_pCustoms.m_eSexuality == Psichology.Customs.Sexuality.Lecherous && Rnd.OneChanceFrom(2)) ||
                                            (pProvince.m_pCustoms.m_eSexuality == Psichology.Customs.Sexuality.Moderate_sexuality && Rnd.OneChanceFrom(4)))
                                            m_pInfo = new BuildingInfo("Brothel", "whore", "whore", BuildingSize.Medium);
                                        else
                                            m_pInfo = new BuildingInfo("Bar", "barman", "barmaid", BuildingSize.Medium);
                                    }
                                    else
                                        if (Rnd.Get(2f) > pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][pProvince.m_iInfrastructureLevel])
                                            m_pInfo = new BuildingInfo("Theatre", "actor", "actor", BuildingSize.Medium);
                                        else
                                            if (Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Treachery][pProvince.m_iInfrastructureLevel])
                                                m_pInfo = new BuildingInfo("Hideout", "rogue", "rogue", BuildingSize.Medium);
                            }
                        }
                        if (m_pInfo == null)
                        {
                            switch (m_pSettlement.m_eSpeciality)
                            {
                                case SettlementSpeciality.NavalAcademy:
                                    m_pInfo = new BuildingInfo("Naval Academy", "sailor", "sailor", BuildingSize.Large);
                                    break;
                                case SettlementSpeciality.Naval:
                                    m_pInfo = new BuildingInfo("War ship", "sailor", "sailor", BuildingSize.Large);
                                    break;
                                case SettlementSpeciality.Resort:
                                    m_pInfo = new BuildingInfo("Hotel", "barman", "barmaid", BuildingSize.Medium);
                                    break;
                                case SettlementSpeciality.Cultural:
                                    m_pInfo = new BuildingInfo("Theatre", "actor", "actor", BuildingSize.Medium);
                                    break;
                                case SettlementSpeciality.ArtsAcademy:
                                    m_pInfo = new BuildingInfo("Arts Academy", "painter", "painter", BuildingSize.Medium);
                                    break;
                                case SettlementSpeciality.Religious:
                                    m_pInfo = new BuildingInfo("Cathedral", "priest", "priest", BuildingSize.Large);
                                    break;
                                case SettlementSpeciality.MilitaryAcademy:
                                    m_pInfo = new BuildingInfo("Barracks", "soldier", "soldier", BuildingSize.Large);
                                    break;
                                case SettlementSpeciality.Gambling:
                                    m_pInfo = new BuildingInfo("Casino", "gambler", "gambler", BuildingSize.Medium);
                                    break;
                                case SettlementSpeciality.SciencesAcademy:
                                    m_pInfo = new BuildingInfo("University", "scientiest", "scientiest", BuildingSize.Medium);
                                    break;
                                case SettlementSpeciality.Tailors:
                                    m_pInfo = new BuildingInfo("Factory", "tailor", "tailor", BuildingSize.Medium);
                                    break;
                                case SettlementSpeciality.Jevellers:
                                    m_pInfo = new BuildingInfo("Factory", "jeveller", "jeveller", BuildingSize.Medium);
                                    break;
                                case SettlementSpeciality.Factory:
                                    m_pInfo = new BuildingInfo("Factory", "worker", "worker", BuildingSize.Medium);
                                    break;
                                case SettlementSpeciality.Artisans:
                                    m_pInfo = new BuildingInfo("Factory", "artisan", "artisan", BuildingSize.Medium);
                                    break;
                            }
                        }
                    break;
                case SettlementSize.Capital:
                    if (pSettlement.m_cBuildings.Count > 0)// && Rnd.OneChanceFrom(2))
                    {
                        if (((State)pProvince.Owner).m_iSocialEquality == 0 && Rnd.OneChanceFrom(3))
                        {
                            if (Rnd.OneChanceFrom(4))
                                m_pInfo = new BuildingInfo("Slave Market", "slaver", "slaver", BuildingSize.Large);
                            else
                                m_pInfo = new BuildingInfo("Slave Pens", "slave", "slave", BuildingSize.Medium);
                        }
                        else
                        {
                            if (Rnd.Get(4) < iControl)
                            {
                                if (Rnd.OneChanceFrom(2))
                                {
                                    if (iInfrastructureLevel < 4)
                                        m_pInfo = new BuildingInfo("Guard Tower", "guard", "guard", BuildingSize.Medium);
                                    else
                                        if (iInfrastructureLevel == 8)
                                            m_pInfo = new BuildingInfo("Emergency Post", "watcher", "watcher", BuildingSize.Medium);
                                        else
                                            if (Rnd.OneChanceFrom(4))
                                                m_pInfo = new BuildingInfo("Police Department", "policeman", "policeman", BuildingSize.Large);
                                            else
                                                m_pInfo = new BuildingInfo("Police Station", "policeman", "policeman", BuildingSize.Medium);
                                }
                                else
                                {
                                    if (iInfrastructureLevel < 4)
                                        m_pInfo = new BuildingInfo("Prison", "guard", "guard", BuildingSize.Medium);
                                    else
                                        if (iInfrastructureLevel == 8)
                                            m_pInfo = new BuildingInfo("Holding", "watcher", "watcher", BuildingSize.Medium);
                                        else
                                            m_pInfo = new BuildingInfo("Prison", "policeman", "policeman", BuildingSize.Medium);
                                }
                            }
                            else
                                if (Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Piety][pProvince.m_iInfrastructureLevel])
                                {
                                    if(Rnd.OneChanceFrom(5))
                                        m_pInfo = new BuildingInfo("Temple", "priest", "priest", BuildingSize.Medium);
                                    else
                                        m_pInfo = new BuildingInfo("Church", "priest", "priest", BuildingSize.Small);
                                }
                                else
                                    if (Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][pProvince.m_iInfrastructureLevel])
                                    {
                                        if ((pProvince.m_pCustoms.m_eSexuality == Psichology.Customs.Sexuality.Lecherous && Rnd.OneChanceFrom(2)) ||
                                            (pProvince.m_pCustoms.m_eSexuality == Psichology.Customs.Sexuality.Moderate_sexuality && Rnd.OneChanceFrom(4)))
                                            m_pInfo = new BuildingInfo("Brothel", "whore", "whore", BuildingSize.Medium);
                                        else
                                            m_pInfo = new BuildingInfo("Bar", "barman", "barmaid", BuildingSize.Medium);
                                    }
                                    else
                                        if (Rnd.Get(2f) > pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][pProvince.m_iInfrastructureLevel])
                                            m_pInfo = new BuildingInfo("Theatre", "actor", "actor", BuildingSize.Medium);
                                        else
                                            if (Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Treachery][pProvince.m_iInfrastructureLevel])
                                                m_pInfo = new BuildingInfo("Hideout", "rogue", "rogue", BuildingSize.Medium);
                        }
                    }

                    if (m_pInfo == null)
                    {
                        switch (m_pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.NavalAcademy:
                                m_pInfo = new BuildingInfo("Naval Academy", "sailor", "sailor", BuildingSize.Large);
                                break;
                            case SettlementSpeciality.Resort:
                                m_pInfo = new BuildingInfo("Hotel", "barman", "barmaid", BuildingSize.Medium);
                                break;
                            case SettlementSpeciality.Cultural:
                                m_pInfo = new BuildingInfo("Theatre", "actor", "actor", BuildingSize.Medium);
                                break;
                            case SettlementSpeciality.ArtsAcademy:
                                m_pInfo = new BuildingInfo("Arts Academy", "painter", "painter", BuildingSize.Medium);
                                break;
                            case SettlementSpeciality.Religious:
                                m_pInfo = new BuildingInfo("Cathedral", "priest", "priest", BuildingSize.Large);
                                break;
                            case SettlementSpeciality.MilitaryAcademy:
                                m_pInfo = new BuildingInfo("Barracks", "soldier", "soldier", BuildingSize.Large);
                                break;
                            case SettlementSpeciality.Gambling:
                                m_pInfo = new BuildingInfo("Casino", "gambler", "gambler", BuildingSize.Medium);
                                break;
                            case SettlementSpeciality.SciencesAcademy:
                                m_pInfo = new BuildingInfo("University", "scientiest", "scientiest", BuildingSize.Medium);
                                break;
                            case SettlementSpeciality.Tailors:
                                m_pInfo = new BuildingInfo("Factory", "tailor", "tailor", BuildingSize.Medium);
                                break;
                            case SettlementSpeciality.Jevellers:
                                m_pInfo = new BuildingInfo("Factory", "jeveller", "jeveller", BuildingSize.Medium);
                                break;
                            case SettlementSpeciality.Factory:
                                m_pInfo = new BuildingInfo("Factory", "worker", "worker", BuildingSize.Medium);
                                break;
                            case SettlementSpeciality.Artisans:
                                m_pInfo = new BuildingInfo("Factory", "artisan", "artisan", BuildingSize.Medium);
                                break;
                        }
                    }
                    break;
                case SettlementSize.Fort:
                    if (Rnd.OneChanceFrom(4))
                    {
                        if (((State)pProvince.Owner).m_iSocialEquality == 0 && Rnd.OneChanceFrom(3))
                            m_pInfo = new BuildingInfo("Slave Pens", "slave", "slave", BuildingSize.Medium);
                        else
                            if (Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Piety][pProvince.m_iInfrastructureLevel])
                            {
                                if(Rnd.OneChanceFrom(5))
                                    m_pInfo = new BuildingInfo("Temple", "priest", "priest", BuildingSize.Medium);
                                else
                                    m_pInfo = new BuildingInfo("Church", "priest", "priest", BuildingSize.Small);
                            }
                            else
                                if (Rnd.Get(2f) < pProvince.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][pProvince.m_iInfrastructureLevel])
                                {
                                    if ((pProvince.m_pCustoms.m_eSexuality == Psichology.Customs.Sexuality.Lecherous && Rnd.OneChanceFrom(2)) ||
                                        (pProvince.m_pCustoms.m_eSexuality == Psichology.Customs.Sexuality.Moderate_sexuality && Rnd.OneChanceFrom(4)))
                                        m_pInfo = new BuildingInfo("Brothel", "whore", "whore", BuildingSize.Medium);
                                    else
                                        m_pInfo = new BuildingInfo("Bar", "barman", "barmaid", BuildingSize.Medium);
                                }
                    }

                    if (m_pInfo == null)
                    {
                        switch (m_pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.Pirates:
                                m_pInfo = new BuildingInfo("Pirate ship", "pirate", "pirate", BuildingSize.Medium);
                                break;
                            case SettlementSpeciality.Naval:
                                m_pInfo = new BuildingInfo("War ship", "sailor", "sailor", BuildingSize.Medium);
                                break;
                            case SettlementSpeciality.Raiders:
                                m_pInfo = new BuildingInfo("Barracks", "bandit", "bandit", BuildingSize.Small);
                                break;
                            case SettlementSpeciality.Military:
                                m_pInfo = new BuildingInfo("Barracks", "soldier", "soldier", BuildingSize.Small);
                                break;
                        }
                    }
                    break;
            }
        }

        public override string ToString()
        {
            return m_pInfo.m_sName;
        }
    }
}
