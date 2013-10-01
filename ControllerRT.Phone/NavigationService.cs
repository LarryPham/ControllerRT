using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace ControllerRT
{
    public class NavigationService : INavigationService
    {
        private readonly PhoneApplicationFrame _frame;
        private readonly IResolver _resolver;
        private readonly IViewResolver _viewResolver;

        public NavigationService(
            PhoneApplicationFrame frame,
            IResolver resolver,
            IViewResolver viewResolver)
        {
            _frame = frame;
            _resolver = resolver;
            _viewResolver = viewResolver;

            _frame.Navigated += OnNavigated;
            _frame.Navigating += OnNavigating;
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

            string viewPath = _viewResolver.Resolve(viewController.ViewModelType);

            Navigate(viewPath, viewController.ViewModelObject);

            return viewController;
        }

        public bool Navigate(string viewPath, object parameter = null)
        {
            var key = Guid.NewGuid().ToString();
            _viewModels[key] = parameter;

            string url = string.Format("{0}?vm={1}", viewPath, key);
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                return false;
            }

            _frame.Dispatcher.BeginInvoke(() => _frame.Navigate(uri));
            return true;
        }

        private static Dictionary<string, object> _viewModels = new Dictionary<string, object>();

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            var page = e.Content as PhoneApplicationPage;
            if (page == null)
                return;

            string vmUid;
            if (page.NavigationContext.QueryString.TryGetValue("vm", out vmUid))
            {
                object vm;
                if (_viewModels.TryGetValue(vmUid, out vm))
                {
                    page.DataContext = vm;
                }
            }
        }

        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            // Cancels the default page navigation on app start.
            if (e.Uri.OriginalString.EndsWith("MainPage.xaml") &&
                e.IsCancelable)
            {
                e.Cancel = true;
            }
        }
    }
}