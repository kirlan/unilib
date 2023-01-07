using LandscapeGeneration.PathFind;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandscapeGeneration
{
    public abstract class Territory : TransportationNode, IInfoLayer
    {
        private KeyValuePair<Type, dynamic>? m_cOwnerInfoLayer = null;

        public void SetOwner<T>(T value) where T : class, IInfoLayer
        {
            if (m_cOwnerInfoLayer.HasValue)
                throw new Exception("Layer Ref already occupied! Clear it first!");

            m_cOwnerInfoLayer = new KeyValuePair<Type, dynamic>(typeof(T), value);
        }

        public T GetOwner<T>() where T : class, IInfoLayer
        {
            if (m_cOwnerInfoLayer.HasValue && m_cOwnerInfoLayer.Value.Key == typeof(T))
                return m_cOwnerInfoLayer.Value.Value;
            else
                return null;
        }

        public bool HasOwner<T>() where T : class, IInfoLayer
        {
            return m_cOwnerInfoLayer.HasValue && m_cOwnerInfoLayer.Value.Key == typeof(T);
        }

        public void ClearOwner()
        {
            m_cOwnerInfoLayer = null;
        }

        private readonly Dictionary<Type, dynamic> m_cInfoLayers = new Dictionary<Type, dynamic>();

        public void AddLayer<T>(T value) where T : class, IInfoLayer
        {
            m_cInfoLayers[typeof(T)] = value;
        }

        public T As<T>() where T : class, IInfoLayer
        {
            if (m_cInfoLayers.TryGetValue(typeof(T), out dynamic pResult))
                return pResult;
            else
                return null;
        }

        public bool Is<T>() where T : class, IInfoLayer
        {
            return m_cInfoLayers.ContainsKey(typeof(T));
        }

        public void ClearLayers()
        {
            m_cInfoLayers.Clear();
        }

        #region Territory Members

        /// <summary>
        /// Границы с другими ТАКИМИ ЖЕ объектами
        /// </summary>
        public virtual Dictionary<Territory, List<VoronoiEdge>> BorderWith { get; } = new Dictionary<Territory, List<VoronoiEdge>>();

        public virtual bool Forbidden { get; } = false;

        /// <summary>
        /// Суммарная длина всех линий в BorderWith
        /// </summary>
        public virtual float PerimeterLength { get; protected set; } = 0;

        #endregion

        public Territory(bool bForbidden)
        {
            Forbidden = bForbidden;
        }

        public Territory()
        { }
    }

    public abstract class TerritoryExtended<BASE> : Territory
        where BASE: Territory
    {
        public override Dictionary<Territory, List<VoronoiEdge>> BorderWith => Origin.BorderWith;

        public override bool Forbidden => Origin.Forbidden;

        public override float PerimeterLength => Origin.PerimeterLength;

        public override float GetMovementCost()
        {
            return Origin.GetMovementCost();
        }

        public BASE Origin => this.As<BASE>();

        public TerritoryExtended(BASE pBase)
        {
            AddLayer(pBase);
        }
    }
}
