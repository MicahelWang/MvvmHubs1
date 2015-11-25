using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Hubs1.Core.ViewModels;
using Hubs1.Droid.Utils;

namespace Hubs1.Droid.Views
{
    [Activity(Label = "酒店信息" , ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AppTheme") ]
    public class ThridView : MvxActivity
    {

        public new ThridViewModel ViewModel
        {
            get { return (ThridViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.ThridView);
            var btn6 = FindViewById<Button>(Resource.Id.btnIOS6);
            var btn7 = FindViewById<Button>(Resource.Id.btnIOS7);

            btn6.Click += delegate
            {
                SetTheme(Resource.Style.ActionSheetStyleIOS6);
                ShowActionSheet();
            };
            btn7.Click += delegate
            {
                SetTheme(Resource.Style.ActionSheetStyleIOS7);
                ShowActionSheet();
            };
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
        
    }
}