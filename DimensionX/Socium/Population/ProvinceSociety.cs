using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Socium.Nations;
using Socium.Settlements;

namespace Socium.Population
{
    public class ProvinceSociety : Society
    {
        public Nation m_pTitularNation = null;

        public ProvinceSociety(LandX pSeed)
        {
            m_pTitularNation = pSeed.m_pNation;
            m_sName = m_pTitularNation.m_pRace.m_pLanguage.RandomCountryName();
        }

        public override void AddBuildings(Settlement pSettlement)
        {
            throw new NotImplementedException();
        }

        public override string GetEstateName(Estate.Position ePosition)
        {
            throw new NotImplementedException();
        }

        public override int GetImportedTech()
        {
            throw new NotImplementedException();
        }

        public override string GetImportedTechString()
        {
            throw new NotImplementedException();
        }

        protected override BuildingInfo ChooseNewBuilding(Settlement pSettlement)
        {
            throw new NotImplementedException();
        }
    }
}
