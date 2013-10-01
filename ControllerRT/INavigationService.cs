using System;

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
    }
}