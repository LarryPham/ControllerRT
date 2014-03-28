using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ControllerRT
{
    public class NavigationPage<T> : Page
    {
        protected T ViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = (T)e.Parameter;
        }
    }
}