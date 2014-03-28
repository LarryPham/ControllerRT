using Autofac;
using System;
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
            Container = containerBuilder
                .RegisterSingleton<IResolver, AutofacResolver>()
                .RegisterSingleton<INavigationService, NavigationService>()
                .RegisterSingleton<IFlyoutService, FlyoutService>()
                .Build();
        }

        public TViewController Run<TViewController>(Func<TViewController, Action> target)
            where TViewController : IViewController
        {
            Frame rootFrame = CreateRootFrame();
            var controller = Container.Resolve<INavigationService>().NavigateTo(target);
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
            return controller;
        }

        public TViewController Run<TViewController, T1>(Func<TViewController, Action<T1>> target, T1 p1)
            where TViewController : IViewController
        {
            Frame rootFrame = CreateRootFrame();
            var controller = Container.Resolve<INavigationService>().NavigateTo(target, p1);
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
            return controller;
        }

        public TViewController Run<TViewController, T1, T2>(Func<TViewController, Action<T1, T2>> target, T1 p1, T2 p2)
            where TViewController : IViewController
        {
            Frame rootFrame = CreateRootFrame();
            var controller = Container.Resolve<INavigationService>().NavigateTo(target, p1, p2);
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
            return controller;
        }

        public TViewController Run<TViewController, T1, T2, T3>(Func<TViewController, Action<T1, T2, T3>> target, T1 p1, T2 p2, T3 p3)
            where TViewController : IViewController
        {
            Frame rootFrame = CreateRootFrame();
            var controller = Container.Resolve<INavigationService>().NavigateTo(target, p1, p2, p3);
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
            return controller;
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

    public static class ContainerExtensions
    {
        public static ContainerBuilder RegisterSingleton<TFrom, TTo>(this ContainerBuilder containerBuilder, string key = null) where TTo : TFrom
        {
            containerBuilder.RegisterType<TTo>().As<TFrom>().SingleInstance();
            return containerBuilder;
        }
    }
}