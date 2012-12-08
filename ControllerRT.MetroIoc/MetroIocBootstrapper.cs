using MetroIoc;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ControllerRT.MetroIoc
{
    public abstract class MetroIocBootstrapper
    {
        protected readonly MetroContainer Container;

        protected MetroIocBootstrapper()
        {
            Container = new MetroContainer();
        }

        public void Run<TViewController>(Func<TViewController, Action> target)
            where TViewController : IViewController
        {
            Frame rootFrame = CreateRootFrame();
            Container.Resolve<INavigationService>().NavigateTo(target);
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        public void Run<TViewController, T1>(Func<TViewController, Action<T1>> target, T1 p1)
            where TViewController : IViewController
        {
            Frame rootFrame = CreateRootFrame();
            Container.Resolve<INavigationService>().NavigateTo(target, p1);
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        public void Run<TViewController, T1, T2>(Func<TViewController, Action<T1, T2>> target, T1 p1, T2 p2)
            where TViewController : IViewController
        {
            Frame rootFrame = CreateRootFrame();
            Container.Resolve<INavigationService>().NavigateTo(target, p1, p2);
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        public void Run<TViewController, T1, T2, T3>(Func<TViewController, Action<T1, T2, T3>> target, T1 p1, T2 p2, T3 p3)
            where TViewController : IViewController
        {
            Frame rootFrame = CreateRootFrame();
            Container.Resolve<INavigationService>().NavigateTo(target, p1, p2, p3);
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        protected abstract void ConfigureContainer();

        private Frame CreateRootFrame()
        {
            var rootFrame = new Frame();

            Container
                .RegisterInstance<Frame>(rootFrame)
                .RegisterInstance<IContainer>(Container)
                .RegisterSingleton<IResolver, MetroIocResolver>()
                .RegisterSingleton<INavigationService, NavigationService>();

            ConfigureContainer();

            return rootFrame;
        }
    }

    public static class ContainerExtensions
    {
        public static IContainer RegisterSingleton<TFrom, TTo>(this IContainer container, string key = null) where TTo : TFrom
        {
            return container.Register<TFrom, TTo>(key, new Singleton());
        }
    }
}