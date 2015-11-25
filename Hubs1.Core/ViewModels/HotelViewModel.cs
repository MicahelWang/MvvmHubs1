using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace Hubs1.Core.ViewModels
{
    public class HotelViewModel : BaseViewModel
    {
        private HotelDataModel _hotelData;

        public HotelDataModel HotelData
        {
            get
            {
                return _hotelData;
            }

            set
            {
                _hotelData = value;
                RaisePropertyChanged(() => HotelData);
            }
        }

        public string PayInfo => "支付";

        public void Init(HotelDataModel hotelData)
        {
            HotelData = hotelData;
        }
        public ICommand PayCommand
        {
            get { return new MvxCommand(() => { }); }
        }
    }

    public class ThridViewModel : BaseViewModel
    {
       
        public string TxtBtnIos6 => "IOS6 Style";
        public string TxtBtnIos7 => "IOS7 Style";
        
    }
}