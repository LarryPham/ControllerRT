using Microsoft.Practices.Unity;
using System;

namespace ControllerRT.Unity
{
    public class UnityIocResolver : IResolver
    {
        private readonly IUnityContainer _container;

        public UnityIocResolver(IUnityContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }
    }
}