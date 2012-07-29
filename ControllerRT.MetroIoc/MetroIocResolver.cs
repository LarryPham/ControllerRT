using System;
using MetroIoc;

namespace ControllerRT.MetroIoc
{
    public class MetroIocResolver : IResolver
    {
        private readonly IContainer _container;

        public MetroIocResolver(IContainer container)
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