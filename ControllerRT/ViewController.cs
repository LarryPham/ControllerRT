using System;

namespace ControllerRT
{
    public interface IViewController
    {
        Type ViewModelType { get; }

        object ViewModelObject { get; }
    }

    public interface IViewController<out TViewModel> : IViewController
    {
        TViewModel ViewModel { get; }
    }

    public abstract class ViewController : IViewController
    {
        public abstract Type ViewModelType { get; }

        public abstract object ViewModelObject { get; }
    }

    public class ViewController<TViewModel> : ViewController, IViewController<TViewModel>
    {
        public TViewModel ViewModel { get; private set; }

        public override Type ViewModelType
        {
            get { return typeof(TViewModel); }
        }

        public override object ViewModelObject
        {
            get { return ViewModel; }
        }

        public ViewController(TViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }
}