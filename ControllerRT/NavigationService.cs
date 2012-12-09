using System;
using Windows.UI.Xaml.Controls;

namespace ControllerRT
{
    public interface INavigationService
    {
        TViewController NavigateTo<TViewController>(Func<TViewController, Action> target)
            where TViewController : IViewController;

        TViewController NavigateTo<TViewController, T1>(Func<TViewController, Action<T1>> target, T1 p1)
            where TViewController : IViewController;

        TViewController NavigateTo<TViewController, T1, T2>(Func<TViewController, Action<T1, T2>> target, T1 p1, T2 p2)
            where TViewController : IViewController;

        TViewController NavigateTo<TViewController, T1, T2, T3>(Func<TViewController, Action<T1, T2, T3>> target, T1 p1, T2 p2, T3 p3)
            where TViewController : IViewController;

        void GoBack();

        bool CanGoBack();

        void GoForward();

        bool CanGoForward();

        void GoHome();

        bool Navigate<T>(object parameter);

        bool Navigate(Type source, object parameter);
    }

    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;
        private readonly IResolver _resolver;
        private readonly IViewResolver _viewResolver;

        public NavigationService(
            Frame frame,
            IResolver resolver,
            IViewResolver viewResolver)
        {
            _frame = frame;
            _resolver = resolver;
            _viewResolver = viewResolver;
        }

        public void GoBack()
        {
            _frame.GoBack();
        }

        public bool CanGoBack()
        {
            return _frame.CanGoBack;
        }

        public void GoForward()
        {
            _frame.GoForward();
        }

        public bool CanGoForward()
        {
            return _frame.CanGoForward;
        }

        public void GoHome()
        {
            while (CanGoBack())
            {
                GoBack();
            }
        }

        public TViewController NavigateTo<TViewController>(Func<TViewController, Action> target)
            where TViewController : IViewController
        {
            var controller = NavigateToBase<TViewController>();
            target(controller)();
            return controller;
        }

        public TViewController NavigateTo<TViewController, T1>(Func<TViewController, Action<T1>> target, T1 p1)
            where TViewController : IViewController
        {
            var controller = NavigateToBase<TViewController>();
            target(controller)(p1);
            return controller;
        }

        public TViewController NavigateTo<TViewController, T1, T2>(Func<TViewController, Action<T1, T2>> target, T1 p1, T2 p2)
            where TViewController : IViewController
        {
            var controller = NavigateToBase<TViewController>();
            target(controller)(p1, p2);
            return controller;
        }

        public TViewController NavigateTo<TViewController, T1, T2, T3>(Func<TViewController, Action<T1, T2, T3>> target, T1 p1, T2 p2, T3 p3)
            where TViewController : IViewController
        {
            var controller = NavigateToBase<TViewController>();
            target(controller)(p1, p2, p3);
            return controller;
        }

        private TViewController NavigateToBase<TViewController>()
            where TViewController : IViewController
        {
            var viewController = _resolver.Resolve<TViewController>();

            Type viewType = _viewResolver.Resolve(viewController.ViewModelType);

            Navigate(viewType, viewController.ViewModelObject);

            return viewController;
        }

        public bool Navigate<T>(object parameter = null)
        {
            var type = typeof(T);

            return Navigate(type, parameter);
        }

        public bool Navigate(Type source, object parameter = null)
        {
            return _frame.Navigate(source, parameter);
        }
    }

    public interface IResolver
    {
        T Resolve<T>();

        object Resolve(Type type);
    }

    public interface IViewResolver
    {
        Type Resolve<TViewModel>();

        Type Resolve(Type viewModelType);
    }
}