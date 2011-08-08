using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaEngine
{
    /// <summary>
    /// Слот экипировки персонажа. Любой экипируемый предмет при экипировке помещается в один из 
    /// имеющихся у персонажа незанятых слотов.
    /// </summary>
    public class EquipmentSlot
    {
        private string m_sName;

        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }
    }
}
