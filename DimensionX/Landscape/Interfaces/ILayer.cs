using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandscapeGeneration
{
    public interface ILayer
    {
        BaseTerritory Territory { get; set; }
    }
}
