using System;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace Hubs1.Core.ViewModels
{
    public class HotelDataModel : BaseViewModel
    {
        public int? GroupId { get; set; }
        public int BrandId { get; set; }
        public int HotelCode { get; set; }
        public string HotelInvitationCode { get; set; }
        public string ElongCode { get; set; }
        public string HotelName { get; set; }
        public int StarRate { get; set; }
        public int ServiceRank { get; set; }
        public string Address { get; set; }
        public string Districtid { get; set; }
        public double GoogleLon { get; set; }
        public double GoogleLat { get; set; }
        public string TotaRooms { get; set; }
        public double BaiduLon { get; set; }
        public double BaiduLat { get; set; }
        public double Price { get; set; }
        public string Facilities { get; set; }
        public double Distance { get; set; }
        public string MemberType { get; set; }
        public string Coupon { get; set; }
        public string Derate { get; set; }
        public string Phone { get; set; }
        public string Establishmentdate { get; set; }
        public string Summary { get; set; }
        public string Attr { get; set; }
        public string CityCode { get; set; }
        public string CoverPic { get; set; }
        public bool IsMerchantHotel { get; set; }

        public string DistanceDescription=> $"距离{Distance} km";
        public override string ToString()
        {
            return $"距离{Distance} km";
        }
    }
}
