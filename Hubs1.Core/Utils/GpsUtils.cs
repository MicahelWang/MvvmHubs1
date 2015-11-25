using System;

namespace Hubs1.Core.Utils
{
    public static class GpsUtils
    {
        /// <summary>
        /// 转换纬度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static double TransformLatitude(double x, double y)
        {
            var ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x * Math.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * Math.PI) + 40.0 * Math.Sin(y / 3.0 * Math.PI)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * Math.PI) + 320 * Math.Sin(y * Math.PI / 30.0)) * 2.0 / 3.0;
            return ret;
        }
        /// <summary>
        ///  /// <summary>
        /// 转换纬度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static double TransformLongitude(double x, double y)
        {
            var ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x * Math.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * Math.PI) + 40.0 * Math.Sin(x / 3.0 * Math.PI)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * Math.PI) + 300.0 * Math.Sin(x / 30.0 * Math.PI)) * 2.0 / 3.0;
            return ret;
        }
        static double[] Delta(double lat, double lon)
        {
            // Krasovsky 1940
            //
            // a = 6378245.0, 1/f = 298.3
            // b = a * (1 - f)
            // ee = (a^2 - b^2) / a^2;
            const double a = 6378245.0;
            const double ee = 0.00669342162296594323;
            var radLat = lat / 180.0 * Math.PI;
            var magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            var sqrtMagic = Math.Sqrt(magic);
            var dLat = TransformLatitude(lon - 105.0, lat - 35.0);
            var dLon = TransformLongitude(lon - 105.0, lat - 35.0);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * Math.PI);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * Math.PI);
            return new double[] { dLat, dLon };
        }
        /// <summary>
        /// 判断指定坐标是否在中国。
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static bool OutOfChina(double lat, double lon)
        {
            if (lon < 72.004 || lon > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }
        /// <summary>
        /// 计算两个坐标之间的距离。
        /// </summary>
        /// <param name="latA">纬度A。</param>
        /// <param name="lonA">经度A。</param>
        /// <param name="latB">纬度B。</param>
        /// <param name="lonB">精度B。</param>
        /// <returns></returns>
        public static double DistanceFrom(double latA, double lonA, double latB, double lonB)
        {
            const double earthR = 6371000;
            var x = Math.Cos(latA * Math.PI / 180) * Math.Cos(latB * Math.PI / 180) * Math.Cos((lonA - lonB) * Math.PI / 180);
            var y = Math.Sin(latA * Math.PI / 180) * Math.Sin(latB * Math.PI / 180);
            var s = x + y;
            if (s > 1)
                s = 1;
            if (s < -1)
                s = -1;
            var alpha = Math.Acos(s);
            var distance = alpha * earthR;
            return distance;
        }
        /// <summary>
        /// 将 WGS-84 坐标转换为 GCJ-02 坐标。
        /// </summary>
        /// <param name="lat">纬度。</param>
        /// <param name="lon">经度。</param>
        public static double[] Wgs2Gcj(double lat, double lon)
        {
            var d = Delta(lat, lon);
            d[0] = lat + d[0];
            d[1] = lon + d[1];
            return d;
        }
        /// <summary>
        /// 将 GCJ-02 坐标转换为 WGS-84 坐标。
        /// </summary>
        /// <param name="lat">纬度。</param>
        /// <param name="lon">经度。</param>
        public static double[] Gcj2Wgs(double lat, double lon)
        {
            var d = Delta(lat, lon);
            d[0] = lat - d[0];
            d[1] = lon - d[1];
            return d;
        }
        /// <summary>
        /// 将 GCJ-02 坐标转换为 BD-09 坐标。
        /// </summary>
        /// <param name="lat">纬度。</param>
        /// <param name="lon">经度。</param>
        public static double[] Gcj2Bd(double lat, double lon)
        {
            const double X_PI = Math.PI * 3000.0 / 180.0;
            var x = lon;
            var y = lat;
            var z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * X_PI);
            var theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * X_PI);
            var bdLon = z * Math.Cos(theta) + 0.0065;
            var bdLat = z * Math.Sin(theta) + 0.006;
            return new double[] { bdLat, bdLon };
        }
        /// <summary>
        /// 将 BD-09 坐标转换为 GCJ-02 坐标。
        /// </summary>
        /// <param name="lat">纬度。</param>
        /// <param name="lon">经度。</param>
        public static double[] Bd2Gcj(double lat, double lon)
        {
            const double xPi = Math.PI * 3000.0 / 180.0;
            var x = lon - 0.0065;
            var y = lat - 0.006;
            var z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * xPi);
            var theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * xPi);
            var gcjLon = z * Math.Cos(theta);
            var gcjLat = z * Math.Sin(theta);
            return new double[] { gcjLat, gcjLon };
        }

        /// <summary>
        /// 将 WGS-84 坐标转换为 BD-09 坐标。
        /// </summary>
        /// <param name="lat">纬度。</param>
        /// <param name="lon">经度。</param>
        public static double[] Wgs2Bd(double lat, double lon)
        {
            var gcj = Wgs2Gcj(lat, lon);
            var bd = Gcj2Bd(gcj[0], gcj[1]);
            return bd;
        }

        /// <summary>
        /// 将 BD-09 坐标转换为 WGS-84 坐标。
        /// </summary>
        /// <param name="lat">纬度。</param>
        /// <param name="lon">经度。</param>
        public static double[] Bd2Wgs(double lat, double lon)
        {
            var gcj = Bd2Gcj(lat, lon);
            var wgs = Gcj2Wgs(gcj[0], gcj[1]);
            return wgs;
        }
    }
}