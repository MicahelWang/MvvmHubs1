using Hubs1.Core.ViewModels;

namespace Hubs1.Core.Models
{
    public class OrderModel
    {
        public string Checkin { get; set; }
        public string Checkout { get; set; }
        public string Nights { get; set; }
        public HotelDataModel Base { get; set; }
        public string CoverPic { get; set; }
        public string Pics { get; set; }
        public string Plans { get; set; }
        public string[] Tags { get; set; }
    }
}