using System;

namespace ControllerRT
{
    public interface IViewResolver
    {
        string Resolve<TViewModel>();

        string Resolve(Type viewModelType);
    }
}