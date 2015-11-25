using Android.Content;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Hubs1.Core;

namespace Hubs1.Droid
{
    public class ErrorDisplayer
    {
        private readonly Context _applicationContext;

        public ErrorDisplayer(Context applicationContext)
        {
            _applicationContext = applicationContext;

            var source = Mvx.Resolve<IErrorSource>();
            source.ErrorReported += (sender, args) => ShowError(args.Message);
        }

        private void ShowError(string message)
        {
            //var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity as IMvxBindingContextOwner;
            //// note that we're not using Binding in this Inflation - but the overhead is minimal - so use it anyway!
            //View layoutView = activity.BindingInflate(Android.Resource.Layout.ToastLayout_Error, null);
            //var text1 = layoutView.FindViewById<TextView>(Android.Resource.Id.ErrorText1);
            //text1.Text = "Sorry!";
            //var text2 = layoutView.FindViewById<TextView>(Android.Resource.Id.ErrorText2);
            //text2.Text = message;

            //var toast = new Toast(_applicationContext);
            //toast.SetGravity(GravityFlags.CenterVertical, 0, 0);
            //toast.Duration = ToastLength.Long;
            //toast.View = layoutView;
            //toast.Show();
        }
    }
}