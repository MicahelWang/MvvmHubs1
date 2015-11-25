using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hubs1.Core.Utils;

namespace Hubs1.Core.ViewModels
{
    public class HotelListViewModel : BaseViewModel
    {
        private const string url = "http://weixin.hubs1.net/api/v1/app/open/hotel/searchHotel.json?token=0";

        string body = "{ \"citycode\":\"Shanghai\", \"checkin\":\"2015-11-09\", \"nights\":1, \"mylng\":121.47370400, \"mylat\":31.23039300, \"isBaiduLngLat\":true, \"order\":1, \"stars\": \"0,1,3,4,5\", \"minPrice\":0, \"maxPrice\":60000, \"canUseCoupon\":false, \"canBooking\":false, \"brandId\":\"\", \"district\":\"\", \"serviceFacility\":\"\", \"keyword\":\"\", \"distance\":10000, \"searchSource\":2, \"pageSize\":20, \"page\":1 }";
        private List<HotelDataModel> _list;
        public List<HotelDataModel> List
        {
            get { return _list; }
            set { _list = value; RaisePropertyChanged(() => List); }
        }

        public void SetCurrentPosition(double longt, double lat)
        {
            InitializationData();
            foreach (var item in List)
            {
                //var result = GpsUtils.Wgs2Bd(item.Longitude, item.Latitude);
                //item.Longitude = result[0];
                //item.Latitude = result[1];
                item.GetDistance(longt, lat);
            }
            List = List.OrderBy(m => m.Distance).ToList();
         
        }
        public void Init()
        {
            GeneralAsyncLoad(url, ProcessResponse, body);
            _list = new List<HotelDataModel>();
        }

        private void ProcessResponse(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();

        }

        public void InitializationData()
        {
         
            _list.Add(new HotelDataModel() { Latitude = 31.21938946823591, Longitude = 121.44572496414184, Name = "金门大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.230000, Longitude = 121.476000, Name = "PMS 中软测试酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.220894186037426, Longitude = 121.46296620368957, Name = "新锦江大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.181047789207966, Longitude = 121.43518924713135, Name = "华亭宾馆" });
            _list.Add(new HotelDataModel() { Latitude = 31.203854609746724, Longitude = 121.41012668609619, Name = "锦江虹桥宾馆" });
            _list.Add(new HotelDataModel() { Latitude = 31.197500, Longitude = 121.387500, Name = "新苑宾馆-测试酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.234086966300285, Longitude = 121.4723539352417, Name = "锦江金门大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.227000, Longitude = 121.480000, Name = "锦江都城青年会经典酒店1" });
            _list.Add(new HotelDataModel() { Latitude = 31.233389758364698, Longitude = 121.47172093391418, Name = "锦江国际饭店" });
            _list.Add(new HotelDataModel() { Latitude = 31.204267559849956, Longitude = 121.41145706176758, Name = "银河宾馆" });
            _list.Add(new HotelDataModel() { Latitude = 31.198412676650566, Longitude = 121.44904017448425, Name = "锦江青松城大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.230967838191443, Longitude = 121.3983678817749, Name = "金沙江大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.23520615775418, Longitude = 121.48757815361023, Name = "新城饭店" });
            _list.Add(new HotelDataModel() { Latitude = 31.245058139900607, Longitude = 121.48505687713623, Name = "新亚大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.218150810609426, Longitude = 121.44397616386413, Name = "上海宾馆" });
            _list.Add(new HotelDataModel() { Latitude = 31.218352667033244, Longitude = 121.44526362419128, Name = "静安宾馆" });
            _list.Add(new HotelDataModel() { Latitude = 31.213599751388894, Longitude = 121.43236756324768, Name = "锦江都城上海达华经典酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.23753623513008, Longitude = 121.48224592208862, Name = "南京饭店" });
            _list.Add(new HotelDataModel() { Latitude = 31.205405457472807, Longitude = 121.47171020507812, Name = "天诚大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.220536358974804, Longitude = 121.46071314811706, Name = "锦江饭店" });
            _list.Add(new HotelDataModel() { Latitude = 31.235527234822246, Longitude = 121.47753596305847, Name = "东亚饭店-test酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.1897489872964, Longitude = 121.43797874450683, Name = "上海建国宾馆" });
            _list.Add(new HotelDataModel() { Latitude = 31.18101107359755, Longitude = 121.43502831459045, Name = "南华亭酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.189124877352842, Longitude = 121.3604736328125, Name = "龙柏饭店" });
            _list.Add(new HotelDataModel() { Latitude = 31.200000, Longitude = 121.441667, Name = "上海建工锦江大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.25388185476643, Longitude = 121.49248123168945, Name = "上海绿地九龙宾馆-test" });
            _list.Add(new HotelDataModel() { Latitude = 31.004000, Longitude = 121.419000, Name = "锦江都城上海闵行饭店" });
            _list.Add(new HotelDataModel() { Latitude = 31.202845, Longitude = 121.447302, Name = "安亭别墅花园酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.045500, Longitude = 121.745842, Name = "[重复]汇亨新亚大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.275605, Longitude = 121.545694, Name = "上海甸园宾馆" });
            _list.Add(new HotelDataModel() { Latitude = 31.227793560939002, Longitude = 121.52342319488525, Name = "上海紫金山大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.204221676594173, Longitude = 121.47072315216064, Name = "上海斯格威铂尔曼大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.20766285901391, Longitude = 121.48805022239685, Name = "东鼎宾馆" });
            _list.Add(new HotelDataModel() { Latitude = 31.224821019673175, Longitude = 121.52956008911133, Name = "上海柏雅居服务公寓" });
            _list.Add(new HotelDataModel() { Latitude = 31.236297815333568, Longitude = 121.42712116241455, Name = "旅居快捷酒店" });
            _list.Add(new HotelDataModel() { Latitude = 30.858772568027262, Longitude = 121.56147837638855, Name = "上海金海岸度假村" });
            _list.Add(new HotelDataModel() { Latitude = 31.228729341617146, Longitude = 121.45437240600586, Name = "上海爱莎金煦全套房酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.158779, Longitude = 121.421543, Name = "上海河滨国际酒店公寓" });
            _list.Add(new HotelDataModel() { Latitude = 31.22832567265717, Longitude = 121.48159146308899, Name = "上海新黄浦酒店公寓" });
            _list.Add(new HotelDataModel() { Latitude = 31.176500, Longitude = 121.385500, Name = "上海亚世都酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.235032, Longitude = 121.439835, Name = "春籘宫精品酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.205556, Longitude = 121.595833, Name = "东郊宾馆" });
            _list.Add(new HotelDataModel() { Latitude = 30.916500, Longitude = 121.462000, Name = "上海新发展古华山庄" });
            _list.Add(new HotelDataModel() { Latitude = 31.186111, Longitude = 121.345833, Name = "上海华港雅阁酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.23234393681464, Longitude = 121.48475646972656, Name = "上海中福世福汇大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.226000, Longitude = 121.480000, Name = "上海华美国际酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.214581550841018, Longitude = 121.41388177871704, Name = "上海美仑大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.254000, Longitude = 121.554000, Name = "上海景缘国际酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.252643649300527, Longitude = 121.60013437271118, Name = "最佳西方上海浦东圣莎大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.235365, Longitude = 121.446618, Name = "同乡宾馆" });
            _list.Add(new HotelDataModel() { Latitude = 31.191000, Longitude = 121.444500, Name = "汉庭商务酒店南丹路店" });
            _list.Add(new HotelDataModel() { Latitude = 31.039500, Longitude = 121.744500, Name = "上海衡山度假村" });
            _list.Add(new HotelDataModel() { Latitude = 31.229000, Longitude = 121.454500, Name = "上海骏翱国际酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.261000, Longitude = 121.489000, Name = "上海虹口世纪大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.245754, Longitude = 121.453547, Name = "上海金水湾大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.224500, Longitude = 121.435000, Name = "上海新东纺大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.216667, Longitude = 121.529167, Name = "上海中油阳光大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.27580915812983, Longitude = 121.59767746925354, Name = "瑞阁美华国际酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.23018804007575, Longitude = 121.45438313484192, Name = "上海银发大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.248965624714163, Longitude = 121.48927330970764, Name = "金富运大酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.156914, Longitude = 121.443489, Name = "上海航空酒店（南站店）" });
            _list.Add(new HotelDataModel() { Latitude = 31.18231446904834, Longitude = 121.38231754302978, Name = "上海迦南酒店" });
            _list.Add(new HotelDataModel() { Latitude = 31.276635, Longitude = 121.521250, Name = "上海新凤城迎宾馆" });
            _list.Add(new HotelDataModel() { Latitude = 31.209103530205826, Longitude = 121.47337317466736, Name = "城中酒店公寓" });
            _list.Add(new HotelDataModel() { Latitude = 31.261000, Longitude = 121.564000, Name = "上海双拥大厦" });
            _list.Add(new HotelDataModel() { Latitude = 31.189000, Longitude = 121.448500, Name = "上海雅舍宾馆" });
            _list.Add(new HotelDataModel() { Latitude = 31.205556, Longitude = 121.445833, Name = "上海衡山宾馆" });
            _list.Add(new HotelDataModel() { Latitude = 31.223500, Longitude = 121.480500, Name = "上海希尔福服务公寓" });

        }




    }
}