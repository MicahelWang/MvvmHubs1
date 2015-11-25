using System;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.ViewModels;
using Hubs1.Core.ViewModels;

namespace Hubs1.Core
{
    public interface IErrorReporter
    {
        void ReportError(string error);
    }

    public class ErrorEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public ErrorEventArgs(string message)
        {
            Message = message;
        }
    }

    public interface IErrorSource
    {
        event EventHandler<ErrorEventArgs> ErrorReported;
    }

    public class ErrorApplicationObject
        : MvxMainThreadDispatchingObject
        , IErrorReporter
        , IErrorSource
    {
        public void ReportError(string error)
        {
            if (ErrorReported == null)
                return;

            InvokeOnMainThread(() =>
            {
                var handler = ErrorReported;
                handler?.Invoke(this, new ErrorEventArgs(error));
            });
        }

        public event EventHandler<ErrorEventArgs> ErrorReported;
    }

    public class App
        : MvxApplication
    {
        public App()
        {

            RegisterAppStart<HotelListViewModel>();

            var errorApplicationObject = new ErrorApplicationObject();
            Mvx.RegisterSingleton<IErrorReporter>(errorApplicationObject);
            Mvx.RegisterSingleton<IErrorSource>(errorApplicationObject);
        }
    }
}
