using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using Com.Baidu.Mapapi;
using Com.Baidu.Mapapi.Map;
using Hubs1.Core.ViewModels;

namespace Hubs1.Droid.Views
{
    [Activity(Label = "附近的酒店")]
    public class HotelListView : MvxActivity
    {
        public new HotelListViewModel ViewModel
        {
            get { return (HotelListViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            SDKInitializer.Initialize(ApplicationContext);

            SetContentView(Resource.Layout.HotelListView);
            ViewModel.SetCurrentPosition(CurrentData.Longitude, CurrentData.Latitude);
        }
    }
}