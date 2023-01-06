using LandscapeGeneration.PathFind;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandscapeGeneration
{
    public abstract class Territory : TransportationNode
    {
        public readonly Dictionary<Type, dynamic> m_cInfoLayers = new Dictionary<Type, dynamic>();

        #region Territory Members

        /// <summary>
        /// Границы с другими ТАКИМИ ЖЕ объектами
        /// </summary>
        public Dictionary<Territory, List<VoronoiEdge>> BorderWith { get; } = new Dictionary<Territory, List<VoronoiEdge>>();

        public virtual bool Forbidden { get; } = false;

        public Territory AddLayer<T>(T value) where T : class, IInfoLayer
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

        /// <summary>
        /// Суммарная длина всех линий в BorderWith
        /// </summary>
        public float PerimeterLength { get; protected set; } = 0;

        #endregion

        public Territory(bool bForbidden)
        {
            Forbidden = bForbidden;
        }

        public Territory()
        { }
    }
}
