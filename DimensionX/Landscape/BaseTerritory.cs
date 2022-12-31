using LandscapeGeneration.PathFind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandscapeGeneration
{
    public class BaseTerritory: TransportationNode
    {
        private Dictionary<Type, dynamic> m_cLayers = new Dictionary<Type, dynamic>();

        public void AddLayer<T>(T value) where T : ILayer
        {
            m_cLayers[typeof(T)] = value;
            value.Territory = this;
        }

        public T GetLayer<T>() where T : ILayer
        {
            return m_cLayers[typeof(T)];
        }

        public bool HasLayer<T>() where T : ILayer
        {
            return m_cLayers.ContainsKey(typeof(T));
        }

        private static BaseTerritory m_pForbidden = new BaseTerritory(true);

        public Dictionary<BaseTerritory, List<Location.Edge>> BorderWith { get; } = new Dictionary<BaseTerritory, List<Location.Edge>>();

        public bool Forbidden { get; private set; } = false;

        public float PerimeterLength { get; private set; } = 0;

        public BaseTerritory(bool bForbidden)
        {
            Forbidden = bForbidden;
        }

        public BaseTerritory()
        { }

        public object[] m_aBorderWith = null;

        internal void FillBorderWithKeys()
        {
            m_aBorderWith = new List<object>(BorderWith.Keys).ToArray();

            PerimeterLength = 0;
            foreach (var pBorder in BorderWith)
                foreach (var pLine in pBorder.Value)
                    PerimeterLength += pLine.m_fLength;
        }

        public override float GetMovementCost()
        {
            return 1000;
        }
    }
}
