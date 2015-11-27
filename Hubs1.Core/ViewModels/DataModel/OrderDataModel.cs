using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace Hubs1.Core.ViewModels.DataModel{
    

    public class OrderDataModel : BaseViewModel
    {
        public string Checkin { get; set; }
        public string Checkout { get; set; }
        public string Nights { get; set; }
        public HotelDataModel Base { get; set; }
        public string CoverPic { get; set; }
        public string Pics { get; set; }
        public string Plans { get; set; }
        public string[] Tags { get; set; }

        public ICommand ViewHotelCommand
        {
            get { return new MvxCommand(() => ShowViewModel<HotelViewModel>(this.Base)); }
        }
    }
}