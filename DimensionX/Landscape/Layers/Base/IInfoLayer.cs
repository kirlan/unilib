using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandscapeGeneration
{
    /// <summary>
    /// Информационный слой - содержит методы для взаимодействия с другими слоями той же территории
    /// </summary>
    public interface IInfoLayer
    {
        void AddLayer<T>(T value) where T : class, IInfoLayer;
        T As<T>() where T : class, IInfoLayer;
        bool Is<T>() where T : class, IInfoLayer;
        void ClearLayers();
    }

    /// <summary>
    /// Информационный слой, содержащий информацию о слое более высокого уровня - <typeparamref name="OWNER"/>
    /// </summary>
    /// <typeparam name="OWNER"></typeparam>
    public interface IInfoLayer<OWNER> : IInfoLayer
        where OWNER : class, IInfoLayer
    {
        void SetOwner(OWNER value);
        OWNER GetOwner();
        bool HasOwner();
        void ClearOwner();
    }
}
