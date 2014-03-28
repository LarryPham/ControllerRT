using System;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ControllerRT
{
    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;
        private readonly IResolver _resolver;
        private readonly IViewResolver _viewResolver;
        private SettingsFlyout _flyout;

        public NavigationService(
            Frame frame,
            IResolver resolver,
            IViewResolver viewResolver)
        {
            _frame = frame;
            _resolver = resolver;
            _viewResolver = viewResolver;

            _frame.Navigated += OnNavigated;
        }

        public void GoBack()
        {
            if (_flyout != null)
            {
                ClearFlyout();
            }
            else
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

            var viewTypeInfo = viewType.GetTypeInfo();
            if (viewTypeInfo.IsSubclassOf(typeof(Page)))
            {
                Navigate(viewType, viewController.ViewModelObject);
            }
            else if (viewTypeInfo.IsSubclassOf(typeof(UserControl)))
            {
                Flyout(viewType, viewController.ViewModelObject);
            }
            else
            {
                throw new NotSupportedException("Can only navigate to a Page or UserControl");
            }

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

        public void Flyout(Type source, object parameter = null)
        {
            var view = (Control)Activator.CreateInstance(source);
            view.DataContext = parameter;
            _flyout = new SettingsFlyout
            {
                Title = view.Name,
                Content = view,
                Width = view.Width,
                Foreground = view.Foreground,
                Background = view.Background,
                HeaderBackground = view.BorderBrush,
                BorderBrush = view.BorderBrush,
                RequestedTheme = _frame.RequestedTheme,
                VerticalAlignment = VerticalAlignment.Stretch,
                Padding = new Thickness(0),
            };

            ScrollViewer.SetVerticalScrollBarVisibility(_flyout, ScrollBarVisibility.Disabled);
            ScrollViewer.SetVerticalScrollMode(_flyout, ScrollMode.Disabled);

            _flyout.BackClick += OnFlyoutBack;
            _flyout.ShowIndependent();
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            var element = e.Content as FrameworkElement;
            if (element == null)
                return;

            element.DataContext = e.Parameter;
        }

        private void OnFlyoutBack(object sender, BackClickEventArgs e)
        {
            ClearFlyout();
        }

        private void ClearFlyout()
        {
            var flyout = _flyout;
            if (flyout != null)
            {
                flyout.BackClick -= OnFlyoutBack;
                flyout.Hide();
                _flyout = null;
            }
        }
    }
}