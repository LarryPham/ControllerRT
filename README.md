# ControllerRT

### What is it?

This project is an MVVM + Controller framework for WinRT.  The goal is to make the page navigation completely unit testable.  What it also improves over traditional MVVM is the separation of properties involved in binding (remain in the view model) from the command logic (moved to the view controller).

This project is released under the Microsoft Public Licence: http://www.opensource.org/licenses/ms-pl.html

### Step 1 - Create a view model class.

For example:

```csharp
public interface ISomeViewModel
{
    string Title { get; set; }
}

public class SomeViewModel : ISomeViewModel
{
    public string Title { get; set; }
}
```

### Step 2 - Create a view controller class by inheriting from ViewController.

For example:

```csharp
public interface ISomeController : IViewController<ISomeViewModel>
{
    void Home();
}

public class SomeController : ViewController<ISomeViewModel>, ISomeController
{
    private readonly INavigationService _navigationService;

    public SomeController(
        IBoardsViewModel viewModel,
        INavigationService navigationService)
        : base(viewModel)
    {
        _navigationService = navigationService;
    }

    public void Home()
    {
        ViewModel.Title = "Hello, World!";
    }
}
```

### Step 3 - Create a Page control and set the DataContext in the code behind.

For example:

```csharp
public sealed partial class SomePage : Page
{
    public SomePage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        DataContext = (ISomeViewModel)e.Parameter;
    }
}
```

### Step 4 - Implement IViewResolver.

This class will take in the type "ISomeViewModel" specified on the view controller and should then determine the type of the view which should be navigated to (i.e. "SomePage").

For example:

```csharp
public class ViewResolver : IViewResolver
{
    public Type Resolve<TViewModel>()
    {
        return Resolve(typeof(TViewModel));
    }

    public Type Resolve(Type viewModelType)
    {
        string viewModelName = viewModelType.Name;
        string viewModelNamespace = viewModelType.Namespace;
        bool isInterface = viewModelType.GetTypeInfo().IsInterface;

        viewModelName = viewModelName.TrimStart('I');

        string viewName = viewModelName.Replace("ViewModel", "Page");
        viewName = viewModelNamespace + "." + viewName;

        return Type.GetType(viewName);
    }
}
```

Now we can navigate from a controller to another controller using the following call:

```csharp
_navigationService.NavigateTo((ISomeOtherController soc) => soc.Home, ...params...);
```

At this point you have everything you need to navigate from a controller to another controller, except for an implementation of IResolver<>.  To help do the final wiring, I've included two other projects: one that uses MetroIoc (a port of MicroIoc) and one that uses Autofac (beta).  Choose whichever takes your fancy.

### Step 5 - Implement a bootstrapper.

For example:

```csharp
public class MyMetroIocBootstrapper : MetroIocBootstrapper
{
    protected override void ConfigureContainer()
    {
        Container
            .Register<IViewResolver>(typeof(ViewResolver))
            .Register<ISomeViewModel>(typeof(SomeViewModel))
            .Register<ISomeController>(typeof(SomeController));
    }
}

public class MyAutofacBootstrapper : AutofacBootstrapper
{
    protected override void ConfigureContainer()
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterType<ViewResolver>().As<IViewResolver>();
        containerBuilder.RegisterType<SomeViewModel>().As<ISomeViewModel>();
        containerBuilder.RegisterType<SomeController>().As<ISomeController>();
        containerBuilder.RegisterType<SomePage>();
        containerBuilder.Update(Container);
    }
}
```

And now, we just need to add a few things to the App.xaml.cs to get it to navigate to our first controller:

```csharp

sealed partial class App : Application
{
    private readonly MyAutofacBootstrapper _bootstrapper = new MyAutofacBootstrapper();

    ...

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
        {
            //TODO: Load state from previously suspended application
        }

        _bootstrapper.Run<ISomeController>(sc => sc.Home);
    }
    
    ...
}
```

And we're done!