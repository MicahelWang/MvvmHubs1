using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

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

        public void Init(OrderDataModel orderData)
        {
            OrderData = orderData;
        }
    }

}