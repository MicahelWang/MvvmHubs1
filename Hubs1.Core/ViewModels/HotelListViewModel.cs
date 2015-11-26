using System;
using System.IO;
using Hubs1.Core.Utils;

namespace Hubs1.Core.ViewModels
{
    public class HotelListViewModel : BaseViewModel
    {

        private const string Url = "http://weixin.hubs1.net/api/v1/app/open/hotel/searchHotel.json?token=0";

        private PageDataModel<OrderDataModel> _pageDataModel;

        public PageDataModel<OrderDataModel> PageDataModel
        {
            get
            {
                return _pageDataModel;
            }

            set
            {
                _pageDataModel = value;
                RaisePropertyChanged(() => PageDataModel);
            }
        }

        public void SetCurrentPosition(double lng, double lat)
        {
            var fliter = new RequestFliter
            {
                mylat = lat,
                mylng = lng
            };
            var body = fliter.ToJson();
            AsyncLoad(body);
        }
        public void Init()
        {
        }
        private void AsyncLoad(string body)
        {
            GeneralAsyncLoad(Url, ProcessResponse, "POST", body);
        }
        private void ProcessResponse(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var text = reader.ReadToEnd();
                PageDataModel = text.DeserializeJsonToObject<PageDataModel<OrderDataModel>>();
            }
        }

        class RequestFliter
        {
            //"brandId": "",
            public string brandId { get; set; } = "";
            //"canBooking": false,
            public bool canBooking { get; set; } = false;
            //"canUseCoupon": false,
            public bool canUseCoupon { get; set; } = false;
            //"checkin": "2015-11-09",
            public string checkin { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
            //"citycode": "Shanghai",
            public string citycode { get; set; } = "Shanghai";
            //"distance": 10000,
            public int distance { get; set; } = 10000;
            //"district": "",
            public string district { get; set; } = "";
            //"isBaiduLngLat": true,
            public bool isBaiduLngLat { get; set; } = true;
            //"keyword": "",
            public string keyword { get; set; } = "";
            //"maxPrice": 60000,
            public double maxPrice { get; set; } = 60000;
            //"minPrice": 0,
            public double minPrice { get; set; } = 0;
            //"mylat": 31.23039300,
            public double mylat { get; set; }
            //"mylng": 121.47370400,
            public double mylng { get; set; }
            //"nights": 1,
            public int nights { get; set; } = 1;
            //"order": 1,
            public int order { get; set; } = 1;
            //"page": 1,
            public int page { get; set; } = 1;
            //"pageSize": 20,
            public int pageSize { get; set; } = 20;
            //"searchSource": 2,
            public int searchSource { get; set; } = 2;
            //"serviceFacility": "",
            public string serviceFacility { get; set; } = "";
            //"stars": "0,1,3,4,5"
            public string stars { get; set; } = "";
        }

    }
}