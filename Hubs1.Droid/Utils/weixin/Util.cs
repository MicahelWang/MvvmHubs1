using System;
using System.Collections.Generic;
using System.Text;
using Android.Util;
using Java.Net;
using Java.Security;
using Javax.Net.Ssl;
using Org.Apache.Http;
using Org.Apache.Http.Client;
using Org.Apache.Http.Client.Methods;
using Org.Apache.Http.Conn.Schemes;
using Org.Apache.Http.Entity;
using Org.Apache.Http.Impl.Client;
using Org.Apache.Http.Impl.Conn.Tsccm;
using Org.Apache.Http.Params;
using Org.Apache.Http.Protocol;
using Org.Apache.Http.Util;

namespace Hubs1.Droid.Utils.weixin
{

    public static class Util
    {
        private const string Tag = "Util.Util";

        public static Byte[] ToBytes(this string source)
        {
            return System.Text.Encoding.Default.GetBytes(source);
        }


        public static Byte[] ToBytes(this string source, string charset)
        {
            System.Text.Encoding encoding = Encoding.GetEncoding(charset);
            return encoding.GetBytes(source);
        }


        //public static byte[] bmpToByteArray(Bitmap bmp, bool needRecycle)
        //{
        //    var output = new MemoryStream();
        //    bmp.Compress(Bitmap.CompressFormat.Png, 100, output);
        //    if (needRecycle)
        //    {
        //        bmp.Recycle();
        //    }

        //    byte[] result = output.ToArray();
        //    try
        //    {
        //        output.Close();
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //    return result;
        //}


        public static byte[] httpGet(string url)
        {
            if (url == null || url.Length == 0)
            {
                Log.Error(Tag, "httpGet, url is null");
                return null;
            }

            IHttpClient httpClient = getNewHttpClient();
            HttpGet httpGet = new HttpGet(url);

            try
            {
                IHttpResponse resp = httpClient.Execute(httpGet);
                if (resp.StatusLine.StatusCode != Org.Apache.Http.HttpStatus.ScOk)
                {
                    Log.Error(Tag, "httpGet fail, status code = " + resp.StatusLine.StatusCode);
                    return null;
                }

                return EntityUtils.ToByteArray(resp.Entity);
            }
            catch (Exception e)
            {
                Log.Error(Tag, "httpGet exception, e = " + e.Message);
                System.Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        public static byte[] httpPost(String url, String entity)
        {
            if (url == null || url.Length == 0)
            {
                Log.Error(Tag, "httpPost, url is null");
                return null;
            }

            IHttpClient httpClient = getNewHttpClient();

            HttpPost httpPost = new HttpPost(url);

            try
            {
                httpPost.Entity = new StringEntity(entity);
                httpPost.SetHeader("Accept", "application/json");
                httpPost.SetHeader("Content-type", "application/json");

                IHttpResponse resp = httpClient.Execute(httpPost);
                if (resp.StatusLine.StatusCode != Org.Apache.Http.HttpStatus.ScOk)
                {
                    Log.Error(Tag, "httpGet fail, status code = " + resp.StatusLine.StatusCode);
                    return null;
                }

                return EntityUtils.ToByteArray(resp.Entity);
            }
            catch (Exception e)
            {
                Log.Error(Tag, "httpPost exception, e = " + e.Message);
                System.Console.WriteLine(e.StackTrace);
                return null;
            }
        }





        private static IHttpClient getNewHttpClient()
        {
            try
            {
                KeyStore trustStore = KeyStore.GetInstance(KeyStore.DefaultType);
                trustStore.Load(null, null);
                Org.Apache.Http.Conn.Ssl.SSLSocketFactory sf = new SSLSocketFactoryEx(trustStore);

                sf.HostnameVerifier = Org.Apache.Http.Conn.Ssl.SSLSocketFactory.AllowAllHostnameVerifier;


                IHttpParams @params = new BasicHttpParams();

                HttpProtocolParams.SetVersion(@params, Org.Apache.Http.HttpVersion.Http11);
                HttpProtocolParams.SetContentCharset(@params, HTTP.Utf8);

                SchemeRegistry registry = new SchemeRegistry();
                registry.Register(new Scheme("http", PlainSocketFactory.SocketFactory, 80));
                registry.Register(new Scheme("https", sf, 443));

                Org.Apache.Http.Conn.IClientConnectionManager ccm = new ThreadSafeClientConnManager(@params, registry);

                return new DefaultHttpClient(ccm, @params);
            }
            catch (Exception e)
            {
                return new DefaultHttpClient();
            }
        }

        private class SSLSocketFactoryEx : Org.Apache.Http.Conn.Ssl.SSLSocketFactory
        {

            private SSLContext sslContext = SSLContext.GetInstance("TLS");


            public SSLSocketFactoryEx(KeyStore truststore) : base(truststore)
            {
                ITrustManager tm = new X509TrustManager();

                sslContext.Init(null, new ITrustManager[] { tm }, null);
            }

            public override Socket CreateSocket(Socket socket, string host, int port, bool autoClose)
            {
                return sslContext.SocketFactory.CreateSocket(socket, host, port, autoClose);
            }


            public override Socket CreateSocket()
            {
                return sslContext.SocketFactory.CreateSocket();
            }
        }


        private static int MAX_DECODE_PICTURE_SIZE = 1920 * 1440;


        public static List<String> stringsToList(String[] src)
        {
            if (src == null || src.Length == 0)
            {
                return null;
            }
            List<string> result = new List<String>();
            for (int i = 0; i < src.Length; i++)
            {
                result.Add(src[i]);
            }
            return result;
        }
    }

    public class X509TrustManager : Java.Lang.Object, IX509TrustManager
    {
        public void CheckClientTrusted(Java.Security.Cert.X509Certificate[] chain, string authType)
        {

        }

        public void CheckServerTrusted(Java.Security.Cert.X509Certificate[] chain, string authType)
        {

        }

        public Java.Security.Cert.X509Certificate[] GetAcceptedIssuers()
        {
            return null;
        }
    }

}