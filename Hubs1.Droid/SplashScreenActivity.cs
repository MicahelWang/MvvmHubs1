using Android.App;
using Android.Nfc;
using Android.OS;
using Android.Util;
using Cirrious.MvvmCross.Droid.Views;
using Com.Baidu.Location;
using Com.Baidu.Mapapi;

namespace Hubs1.Droid
{

    [Activity(Label = "汇通预定", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon")]
    public class SplashScreenActivity
       : MvxSplashScreenActivity, IBDLocationListener
    {
        private LocationClient _locClient;
        string Tag = "SplashScreenActivity";
        public SplashScreenActivity()
            : base(Resource.Layout.SplashScreen)
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            SDKInitializer.Initialize(ApplicationContext);
            base.OnCreate(bundle);
            GetLocation();
        }

        public void GetLocation()
        {
            //实例化位置客户端
            _locClient = new LocationClient(ApplicationContext);
            //设置option的属性
            LocationClientOption option = new LocationClientOption();
            option.SetLocationMode(LocationClientOption.LocationMode.HightAccuracy); //可选，默认高精度，设置定位模式，高精度，低功耗，仅设备
            option.CoorType = "gcj02"; //可选，默认gcj02，设置返回的定位结果坐标系，
            const int interval = 0;
            option.ScanSpan = interval; //可选，默认0，即仅定位一次，设置发起定位请求的间隔需要大于等于1000ms才是有效的
            option.SetIsNeedAddress(false); //可选，设置是否需要地址信息，默认不需要
            option.OpenGps = true; //可选，默认false,设置是否使用gps
            option.LocationNotify = true; //可选，默认false，设置是否当gps有效时按照1S1次频率输出GPS结果
            option.SetIgnoreKillProcess(true); //可选，默认true，定位SDK内部是一个SERVICE，并放到了独立进程，设置是否在stop的时候杀死这个进程，默认不杀死

            _locClient.LocOption = option;

            //给位置客户端注册位置监听器
            _locClient.RegisterLocationListener(this);


            //启动位置客户端
            _locClient.Start();

        }

        public void OnReceiveLocation(BDLocation location)
        {
            if (location == null)
            {
                return;
            }
            Log.Info(Tag, "BDLocationListener OnReceiveLocation");

            int locType = location.LocType;
            //longitude = location.Longitude;
            //latitude = location.Latitude;
            CurrentData.Latitude = location.Latitude;
            CurrentData.Longitude = location.Longitude;
        }
    }
}

