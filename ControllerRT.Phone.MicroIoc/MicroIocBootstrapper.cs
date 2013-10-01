using System.Windows.Controls;
using MicroIoc;
using Microsoft.Phone.Controls;
using System;

namespace ControllerRT.MicroIoc
{
    public abstract class MicroIocBootstrapper
    {
        protected readonly MicroIocContainer Container;

        protected MicroIocBootstrapper()
        {
            Container = new MicroIocContainer();
        }

        public TViewController Run<TViewController>(Func<TViewController, Action> target)
            where TViewController : IViewController
        {
            CreateRootFrame();
            var controller = Container.Resolve<INavigationService>().NavigateTo(target);
            return controller;
        }

        public TViewController Run<TViewController, T1>(Func<TViewController, Action<T1>> target, T1 p1)
            where TViewController : IViewController
        {
            CreateRootFrame();
            var controller = Container.Resolve<INavigationService>().NavigateTo(target, p1);
            return controller;
        }

        public TViewController Run<TViewController, T1, T2>(Func<TViewController, Action<T1, T2>> target, T1 p1, T2 p2)
            where TViewController : IViewController
        {
            CreateRootFrame();
            var controller = Container.Resolve<INavigationService>().NavigateTo(target, p1, p2);
            return controller;
        }

        public TViewController Run<TViewController, T1, T2, T3>(Func<TViewController, Action<T1, T2, T3>> target, T1 p1, T2 p2, T3 p3)
            where TViewController : IViewController
        {
            CreateRootFrame();
            var controller = Container.Resolve<INavigationService>().NavigateTo(target, p1, p2, p3);
            return controller;
        }

        protected abstract void ConfigureContainer();

        private void CreateRootFrame()
        {
            var rootFrame = new PhoneApplicationFrame();

            Container
                .RegisterInstance<Frame>(rootFrame)
                .RegisterInstance<PhoneApplicationFrame>(rootFrame)
                .RegisterInstance<IMicroIocContainer>(Container)
                .RegisterSingleton<IResolver, MetroIocResolver>()
                .RegisterSingleton<INavigationService, NavigationService>();

            ConfigureContainer();
        }
    }

    public static class ContainerExtensions
    {
        public static IMicroIocContainer RegisterSingleton<TFrom, TTo>(this IMicroIocContainer container, string key = null) where TTo : TFrom
        {
            return container.Register<TFrom, TTo>(key, isSingleton: true);
        }
    }
}