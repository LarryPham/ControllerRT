using Autofac;

namespace ControllerRT.Autofac
{
    public class AutofacResolver : IResolver
    {
        private readonly IContainer _container;

        public AutofacResolver(IContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}