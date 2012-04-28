using System;
using Autofac;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ControllerRT.Autofac
{
    public abstract class AutofacBootstrapper
    {
        protected readonly IContainer Container;

        protected AutofacBootstrapper()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<AutofacResolver>().As<IResolver>();
            containerBuilder.RegisterType<NavigationService>().As<INavigationService>();
            Container = containerBuilder.Build();
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

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(rootFrame);
            containerBuilder.RegisterInstance(Container);
            containerBuilder.Update(Container);

            ConfigureContainer();

            return rootFrame;
        }
    }
}