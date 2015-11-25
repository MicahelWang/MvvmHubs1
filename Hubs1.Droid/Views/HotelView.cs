using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Com.Alipay.Sdk.App;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;
using Hubs1.Core.ViewModels;
using Hubs1.Droid.Utils;
using Hubs1.Droid.Utils.alipay;
using Hubs1.Droid.Utils.weixin;
using Java.Lang;

namespace Hubs1.Droid.Views
{
    [Activity(Label = "酒店信息")]
    public class HotelView : MvxActivity
    {
        private readonly BitmapDescriptor _hotelBitmap = BitmapDescriptorFactory.FromResource(Resource.Drawable.dot);
        private readonly BitmapDescriptor _localtionBitmap = BitmapDescriptorFactory.FromResource(Resource.Drawable.map_location);
        private Handler _handler;
        private WeixinpayHelper weixinpayHelper;

        private const string Tag = "HotelView";
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

            #region Handler  回调处理

            _handler = new Handler((msg) =>
            {
                var msgWhat = (MsgWhat)msg.What;

                switch (msgWhat)
                {
                    #region 支付宝支付结果回调

                    case MsgWhat.AlipayPayFlag:
                        {
                            PayResult payResult = new PayResult((string)msg.Obj);

                            // 支付宝返回此次支付结果及加签，建议对支付宝签名信息拿签约时支付宝提供的公钥做验签
                            string resultInfo = payResult.Result;

                            string resultStatus = payResult.ResultStatus;

                            // 判断resultStatus 为“9000”则代表支付成功，具体状态码代表含义可参考接口文档
                            switch (resultStatus)
                            {
                                case "9000":
                                    {
                                        Toast.MakeText(this, "支付成功",
                                   ToastLength.Short).Show();
                                        break;
                                    }
                                // 判断resultStatus 为非“9000”则代表可能支付失败
                                // “8000”代表支付结果因为支付渠道原因或者系统原因还在等待支付结果确认，最终交易是否成功以服务端异步通知为准（小概率状态）
                                case "8000":
                                    {
                                        Toast.MakeText(this, "支付结果确认中",
                                        ToastLength.Short).Show(); break;
                                    }
                                case "4000":
                                    {
                                        Toast.MakeText(this, "未安装支付宝",
                                   ToastLength.Short).Show();
                                        break;
                                    }
                                default:
                                    {
                                        // 其他值就可以判断为支付失败，包括用户主动取消支付，或者系统返回的错误
                                        Toast.MakeText(this, "支付失败",
                                            ToastLength.Short).Show();
                                        break;
                                    }
                            }
                            break;
                        }

                        #endregion 支付宝支付结果回调
                        //#region 微信获取认证结果回调


                }
            });

            #endregion Handler  回调处理
            InitOverlay();
        }

        public void ShowActionSheet()
        {
            #region ActinSheet Items
            List<ActionSheetArgs> items = new List<ActionSheetArgs>();

            var alipayItem = new ActionSheetArgs("支付宝");
            alipayItem.OnClick += () =>
            {



                if (!AlipayHelper.CheckConfig())
                {
                    Toast.MakeText(ApplicationContext,
                        "系统异常.",
                        ToastLength.Long);
                    Log.Error(Tag, "Aplipay Config Exception ");
                    return;
                }
                string payInfo = AlipayHelper.GetPayInfo();
                // 完整的符合支付宝参数规范的订单信息
                Runnable payRunnable = new Runnable(() =>
                {
                    PayTask alipay = new PayTask(this);
                    // 调用支付接口，获取支付结果
                    string result = alipay.Pay(payInfo);

                    Message msg = new Message
                    {
                        What = (int)MsgWhat.AlipayPayFlag,
                        Obj = result
                    };
                    _handler.SendMessage(msg);
                });

                // 必须异步调用
                Thread payThread = new Thread(payRunnable);
                payThread.Start();
            };
            items.Add(alipayItem);
            var weixinItem = new ActionSheetArgs("微信");
            weixinItem.OnClick += () =>
            {
                Toast.MakeText(this, " 微信 click", 0).Show();
                Runnable wxpayRunnable = new Runnable(() =>
                {
                    weixinpayHelper = new WeixinpayHelper(this);
                    weixinpayHelper.Execute();
                });

                // 必须异步调用
                Thread payThread = new Thread(wxpayRunnable);
                payThread.Start();


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