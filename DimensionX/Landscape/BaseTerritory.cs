using LandscapeGeneration.PathFind;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandscapeGeneration
{
    public abstract class BaseTerritory : TransportationNode, ITerritory
    {
        public readonly Dictionary<Type, dynamic> m_cInfoLayers = new Dictionary<Type, dynamic>();

        #region ITerritory Members

        /// <summary>
        /// Границы с другими ТАКИМИ ЖЕ объектами
        /// </summary>
        public Dictionary<ITerritory, List<VoronoiEdge>> BorderWith { get; } = new Dictionary<ITerritory, List<VoronoiEdge>>();

        public virtual bool Forbidden { get; } = false;

        public ITerritory AddLayer<T>(T value) where T : class, IInfoLayer
        {
            m_cInfoLayers[typeof(T)] = value;

            return this;
        }

        public T GetLayer<T>() where T : class, IInfoLayer
        {
            if (m_cInfoLayers.TryGetValue(typeof(T), out dynamic pResult))
                return pResult;
            else
                return null;
        }
 
        public bool HasLayer<T>() where T : class, IInfoLayer
        {
            return m_cInfoLayers.ContainsKey(typeof(T));
        }

        public float PerimeterLength { get; protected set; } = 0;

        #endregion

        public BaseTerritory(bool bForbidden)
        {
            Forbidden = bForbidden;
        }

        public BaseTerritory()
        { }
    }
}
