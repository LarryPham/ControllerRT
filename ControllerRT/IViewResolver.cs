using System;

namespace ControllerRT
{
    public interface IViewResolver
    {
        Type Resolve<TViewModel>();

        Type Resolve(Type viewModelType);
    }
}