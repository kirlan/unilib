using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneLab.Genetix
{
    public enum HideType
    { 
        /// <summary>
        /// просто кожа
        /// </summary>
        Skin,
        /// <summary>
        /// короткий мех
        /// </summary>
        FurShort,
        /// <summary>
        /// длинный мех
        /// </summary>
        FurLong,
        /// <summary>
        /// перья
        /// </summary>
        Feathers,
        /// <summary>
        /// хитиновый панцирь
        /// </summary>
        Chitin,
        /// <summary>
        /// чешуя
        /// </summary>
        Scales,
        /// <summary>
        /// костяной панцирь
        /// </summary>
        Shell
    }

    public enum HideColor
    { 
        White,
        ,
        Red,
        Darkblond,

    }

    public class HideGenetix: GenetixBase
    {
        public GenetixBase MutateRace()
        {
            throw new NotImplementedException();
        }

        public GenetixBase MutateNation()
        {
            throw new NotImplementedException();
        }

        public GenetixBase MutateFamily()
        {
            throw new NotImplementedException();
        }

        public GenetixBase MutateIndividual()
        {
            throw new NotImplementedException();
        }
    }
}
