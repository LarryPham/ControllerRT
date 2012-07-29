using System;
using Windows.UI.Xaml.Controls;

namespace ControllerRT
{
    public interface INavigationService
    {
        void NavigateTo<TViewController>(Func<TViewController, Action> target)
            where TViewController : IViewController;

        void NavigateTo<TViewController, T1>(Func<TViewController, Action<T1>> target, T1 p1)
            where TViewController : IViewController;

        void NavigateTo<TViewController, T1, T2>(Func<TViewController, Action<T1, T2>> target, T1 p1, T2 p2)
            where TViewController : IViewController;

        void NavigateTo<TViewController, T1, T2, T3>(Func<TViewController, Action<T1, T2, T3>> target, T1 p1, T2 p2, T3 p3)
            where TViewController : IViewController;

        void GoBack();

        void GoForward();

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

        public void GoForward()
        {
            _frame.GoForward();
        }

        public void NavigateTo<TViewController>(Func<TViewController, Action> target)
            where TViewController : IViewController
        {
            target(NavigateToBase<TViewController>())();
        }

        public void NavigateTo<TViewController, T1>(Func<TViewController, Action<T1>> target, T1 p1)
            where TViewController : IViewController
        {
            target(NavigateToBase<TViewController>())(p1);
        }

        public void NavigateTo<TViewController, T1, T2>(Func<TViewController, Action<T1, T2>> target, T1 p1, T2 p2)
            where TViewController : IViewController
        {
            target(NavigateToBase<TViewController>())(p1, p2);
        }

        public void NavigateTo<TViewController, T1, T2, T3>(Func<TViewController, Action<T1, T2, T3>> target, T1 p1, T2 p2, T3 p3)
            where TViewController : IViewController
        {
            target(NavigateToBase<TViewController>())(p1, p2, p3);
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