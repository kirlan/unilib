using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandscapeGeneration
{
    public interface IInfoLayer
    {
        void SetOwner<T>(T value) where T : class, IInfoLayer;
        T GetOwner<T>() where T : class, IInfoLayer;
        bool HasOwner<T>() where T : class, IInfoLayer;
        void ClearOwner();

        void AddLayer<T>(T value) where T : class, IInfoLayer;
        T As<T>() where T : class, IInfoLayer;
        bool Is<T>() where T : class, IInfoLayer;
        void ClearLayers();
    }
}
