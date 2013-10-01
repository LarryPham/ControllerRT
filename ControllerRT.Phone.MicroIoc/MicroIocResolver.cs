using System;
using System.ComponentModel;
using MicroIoc;

namespace ControllerRT.MicroIoc
{
    public class MetroIocResolver : IResolver
    {
        private readonly IMicroIocContainer _container;

        public MetroIocResolver(IMicroIocContainer container)
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