using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hubs1.Core.Models;
using Hubs1.Core.Utils;

namespace Hubs1.Core.ViewModels
{
    public class HotelListViewModel : BaseViewModel
    {

        private const string Url = "http://weixin.hubs1.net/api/v1/app/open/hotel/searchHotel.json?token=0";

        private List<HotelDataModel> _list;
        public List<HotelDataModel> List
        {
            get { return _list; }
            set { _list = value; RaisePropertyChanged(() => List); }
        }

        public void SetCurrentPosition(double lng, double lat)
        {
            var fliter = new RequestFliter
            {
                MyLat = lat,
                MyLng = lng
            };
            var body = fliter.ToJson();

            GeneralAsyncLoad(Url, ProcessResponse, "POST", body);
        }
        public void Init()
        {
            List = new List<HotelDataModel>();
        }

        private void ProcessResponse(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();
            var pageModel = text.DeserializeJsonToObject<PageModel>();
            if (pageModel != null)
            {
                if (pageModel.List != null)
                    foreach (var item in pageModel.List)
                    {
                        List.Add(item.Base);
                    }
            }

            ReportError("response context " + text);

        }

        class RequestFliter
        {
            //"brandId": "",
            public string BrandId { get; set; } = "";
            //"canBooking": false,
            public bool CanBooking { get; set; } = false;
            //"canUseCoupon": false,
            public bool CanUseCoupon { get; set; } = false;
            //"checkin": "2015-11-09",
            public string Checkin { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
            //"citycode": "Shanghai",
            public string Citycode { get; set; } = "Shanghai";
            //"distance": 10000,
            public int Distance { get; set; } = 10000;
            //"district": "",
            public string District { get; set; } = "";
            //"isBaiduLngLat": true,
            public bool IsBaiduLngLat { get; set; } = true;
            //"keyword": "",
            public string Keyword { get; set; } = "";
            //"maxPrice": 60000,
            public double MaxPrice { get; set; } = 60000;
            //"minPrice": 0,
            public double MinPrice { get; set; } = 0;
            //"mylat": 31.23039300,
            public double MyLat { get; set; }
            //"mylng": 121.47370400,
            public double MyLng { get; set; }
            //"nights": 1,
            public int Nights { get; set; } = 1;
            //"order": 1,
            public int Order { get; set; } = 1;
            //"page": 1,
            public int Page { get; set; } = 1;
            //"pageSize": 20,
            public int PageSize { get; set; } = 20;
            //"searchSource": 2,
            public int SearchSource { get; set; } = 2;
            //"serviceFacility": "",
            public string ServiceFacility { get; set; } = "";
            //"stars": "0,1,3,4,5"
            public string Stars { get; set; } = "";
        }

    }
}