using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;
using Hubs1.Core.ViewModels;
using Hubs1.Droid.Utils;

namespace Hubs1.Droid.Views
{
    [Activity(Label = "酒店信息")]
    public class HotelView : MvxActivity
    {
        private readonly BitmapDescriptor _hotelBitmap = BitmapDescriptorFactory.FromResource(Resource.Drawable.dot);
        private readonly BitmapDescriptor _localtionBitmap = BitmapDescriptorFactory.FromResource(Resource.Drawable.map_location);

        public new HotelViewModel ViewModel
        {
            get { return (HotelViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.HotelView);

            var btnPay = FindViewById<Button>(Resource.Id.btnPay);
            btnPay.Click += delegate
            {
                SetTheme(Resource.Style.ActionSheetStyleIOS6);
                ShowActionSheet();
            };
            InitOverlay();
        }

        public void ShowActionSheet()
        {
            #region ActinSheet Items
            List<ActionSheetArgs> items = new List<ActionSheetArgs>();

            var alipayItem = new ActionSheetArgs("支付宝");
            alipayItem.OnClick += () =>
            {
                Toast.MakeText(this, " 支付宝 click", 0).Show();
            };
            items.Add(alipayItem);
            var weixinItem = new ActionSheetArgs("微信");
            weixinItem.OnClick += () =>
            {
                Toast.MakeText(this, " 微信 click", 0).Show();
            };
            items.Add(weixinItem);

            #endregion
            var menuView = new ActionSheet(this);
            menuView.SetCancelButtonTitle("取消");// before add items
            menuView.Items = items;
            menuView.CancelableOnTouchOutside = true;
            menuView.ShowMenu();
        }

        private void InitOverlay()
        {
            var map = FindViewById<MapView>(Resource.Id.bmapView);
            var mBaiduMap = map.Map;
            //位置
            LatLng hotelLatLng = new LatLng(ViewModel.HotelData.Latitude, ViewModel.HotelData.Longitude);
            OverlayOptions hotelOverlayOptions = new MarkerOptions()
                .InvokeIcon(_hotelBitmap)
                .InvokePosition(hotelLatLng)
                .InvokeZIndex(9);
            Marker hotelMarker = mBaiduMap.AddOverlay(hotelOverlayOptions).JavaCast<Marker>();

            #region 当前位置




            LatLng locationLatLng = new LatLng(CurrentData.Latitude, CurrentData.Longitude);
            OverlayOptions locationOverlayOptions = new MarkerOptions()
                .InvokeIcon(_localtionBitmap)
                .InvokePosition(locationLatLng)
                .InvokeZIndex(9);
            Marker locationMarker = mBaiduMap.AddOverlay(locationOverlayOptions).JavaCast<Marker>();

            var zoomLevel = BaseHelper.GetZoomLevel(ViewModel.HotelData.Distance);
            //设置居中
            var mMapStatusUpdate = MapStatusUpdateFactory.NewLatLngZoom(hotelLatLng, zoomLevel);
            //改变地图状态
            mBaiduMap.SetMapStatus(mMapStatusUpdate);

            #endregion 当前位置


        }


    }
}