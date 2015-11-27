using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Hubs1.Core.Utils;
using Hubs1.Core.ViewModels.DataModel;

namespace Hubs1.Core.ViewModels
{
    public class HotelViewModel : BaseViewModel
    {
        private OrderDataModel _orderData;

        public OrderDataModel OrderData
        {
            get
            {
                return _orderData;
            }
            set
            {
                _orderData = value;
                RaisePropertyChanged(() => OrderData);
            }
        }

        public string PayInfo => "支付";

        public void Init(HotelDataModel hotelData)
        {
            OrderData = new OrderDataModel
            {
                CoverPic = "",
                Base = hotelData
            };
        }
    }

}