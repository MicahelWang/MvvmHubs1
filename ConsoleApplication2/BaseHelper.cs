using System.Collections.Generic;

namespace ConsoleApplication2
{
    public class BaseHelper
    {
        /*
        Level卆肝葎��20、19、18、17、16、15、14、13、12、11、10、9、8、7、6、5、4、3
       

        {"10m", "20m", "50m", "100m", "200m", "500m", "1km", "2km", "5km", "10km", "20km", "25km", "50km", "100km", "200km", "500km", "1000km", "2000km"} 
     */

        public static int GetZoomLevel(double distance)
        {
            Dictionary<int, double> dict = new Dictionary<int, double>
            {
                {20, 0.01},
                {19, 0.02},
                {18, 0.05},
                {17, 0.1},
                {16, 0.2},
                {15, 0.5},
                {14, 1},
                {13, 2},
                {12, 5},
                {11, 10},
                {10, 20},
                {9, 25},
                {8, 50},
                {7, 100},
                {6, 200},
                {5, 500},
                {4, 1000},
                {3, 2000},

            };
            int index = DichotomySearch(dict, distance, 20, 3);
            return index;
        }


        private static int DichotomySearch(Dictionary<int, double> map, double key, int high, int low)
        {
            if (map[low] < key)
            {
                return low;
            }
            if (map[high] > key)
            {
                return high;
            }
            for (int i = low; i < high; i++)
            {
                var val = map[i];
                var nextVal = map[i+1];
                if (key<=val && key <nextVal)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}