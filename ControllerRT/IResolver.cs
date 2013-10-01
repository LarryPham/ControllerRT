using System;

namespace ControllerRT
{
    public interface IResolver
    {
        T Resolve<T>();

        object Resolve(Type type);
    }
}