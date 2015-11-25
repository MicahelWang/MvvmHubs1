using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace Hubs1.Core.ViewModels
{
    public class HotelDataModel : BaseViewModel
    {
        private double R = 6371229;
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public double Distance { get; set; }

        public string DistanceDescription => $"距离{Distance.ToString("0.00")}km";

        public override string ToString()
        {
            return string.Format("{0} {3}km({1}, {2})", Name, Longitude.ToString("0.0000"), Latitude.ToString("0.0000"), Distance.ToString("0.00"));
        }

        public void GetDistance(double longt1, double lat1)
        {
            double x, y;
            x = (Longitude - longt1) * Math.PI * R
              * Math.Cos(((lat1 + Latitude) / 2) * Math.PI / 180) / 180;
            y = (Latitude - lat1) * Math.PI * R / 180;

            Distance = (Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) / 1000);
        }

        public ICommand ShowCategoryCommand
        {
            get { return new MvxCommand(() => ShowViewModel<HotelViewModel>(this)); }
        }
    }
}
