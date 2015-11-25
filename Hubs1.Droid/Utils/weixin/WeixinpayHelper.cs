using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Android.Content;
using Android.Util;
using Android.Widget;
using Com.Tencent.MM.Sdk.Modelpay;
using Com.Tencent.MM.Sdk.Openapi;
using Java.Lang;

namespace Hubs1.Droid.Utils.weixin
{
    public class WeixinpayHelper
    {

        private readonly PayReq _payRequest;
        private readonly IWXAPI _msgApi = null;
        private Dictionary<string, string> _resultunifiedorder;
        private System.Text.StringBuilder _sb;
        private const string Tag = "WeixinpayHelper";
        private readonly Context _context;

        public WeixinpayHelper(Context context)
        {
            _context = context;
            _payRequest = new PayReq();
            _msgApi = WXAPIFactory.CreateWXAPI(_context, null);
            _sb = new System.Text.StringBuilder();
            _msgApi.RegisterApp(Constants.ApiKey);
        }


        public void Execute()
        {

            var checkRunnable = new Runnable(() =>
            {


                const string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
                string entity = GenProductArgs();
                var buf = Util.httpPost(url, entity);

                string content = Encoding.Default.GetString(buf);
                _resultunifiedorder = DecodeXml(content);
                VoidPayWindow(content);
            });

            var checkThread = new Thread(checkRunnable);
            checkThread.Start();
        }


        void VoidPayWindow(string data)
        {
            Dictionary<string, string> xml = null;
            try
            {
                xml = DecodeXml(data);
            }
            catch (System.Exception)
            {

                Toast.MakeText(_context, "服务器请求异常",
                    ToastLength.Long).Show();
                return;

            }

            if (xml.ContainsKey("return_code"))
            {
                var returnCode = xml["return_code"];

                if (returnCode == "FAIL")
                {
                    var errMsg = xml["return_msg"];
                    Toast.MakeText(_context, "请求微信异常：" + errMsg,
                        ToastLength.Long).Show();
                }
                else
                {
                    _sb.Append("prepay_id\n" + xml["prepay_id"] + "\n\n");
                    GenPayReq();

                    SendPayReq();
                }
            }
        }



        private Dictionary<string, string> DecodeXml(string content)
        {

            try
            {

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(content); //加载xml
                Dictionary<string, string> xml = new Dictionary<string, string>();
                var nodes = xmlDoc.DocumentElement.SelectNodes("/xml/*");
                foreach (XmlNode node in nodes)
                {
                    if (xml.ContainsKey(node.Name))
                    {
                        xml[node.Name] = node.InnerText;
                    }
                    else
                    {
                        xml.Add(node.Name, node.InnerText);
                    }

                }



                return xml;
            }
            catch (System.Exception e)
            {

            }
            return null;

        }


        public string GenProductArgs()
        {
            try
            {
                string nonceStr = genNonceStr();

                List<KeyValuePair<string, string>> packageParams = new List<KeyValuePair<string, string>>();
                packageParams.Add(new KeyValuePair<string, string>("appid", Constants.AppId));
                packageParams.Add(new KeyValuePair<string, string>("body", "ceshishangping"));
                packageParams.Add(new KeyValuePair<string, string>("mch_id", Constants.MchId));
                packageParams.Add(new KeyValuePair<string, string>("nonce_str", nonceStr));
                packageParams.Add(new KeyValuePair<string, string>("notify_url", "http://121.40.35.3/test"));
                packageParams.Add(new KeyValuePair<string, string>("out_trade_no", genOutTradNo()));
                packageParams.Add(new KeyValuePair<string, string>("spbill_create_ip", "127.0.0.1"));
                packageParams.Add(new KeyValuePair<string, string>("total_fee", "1"));
                packageParams.Add(new KeyValuePair<string, string>("trade_type", "APP"));
                string sign = GenPackageSign(packageParams);
                packageParams.Add(new KeyValuePair<string, string>("sign", sign));
                string xmlstring = toXml(packageParams);
                return xmlstring;

            }
            catch (System.Exception e)
            {

                return null;
            }


        }

        private string genNonceStr()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            //Random random = new Random();
            return guid;
        }

        private long genTimeStamp()
        {
            return (long)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalMilliseconds / 1000;
        }



        private string genOutTradNo()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            //Random random = new Random();
            return guid;
        }

        private void GenPayReq()
        {

            _payRequest.AppId = Constants.AppId;
            _payRequest.PartnerId = Constants.MchId;
            _payRequest.PrepayId = _resultunifiedorder["prepay_id"];
            _payRequest.PackageValue = "Sign=WXPay";
            _payRequest.NonceStr = genNonceStr();
            _payRequest.TimeStamp = genTimeStamp().ToString();


            List<KeyValuePair<string, string>> signParams = new List<KeyValuePair<string, string>>();
            signParams.Add(new KeyValuePair<string, string>("appid", _payRequest.AppId));
            signParams.Add(new KeyValuePair<string, string>("noncestr", _payRequest.NonceStr));
            signParams.Add(new KeyValuePair<string, string>("package", _payRequest.PackageValue));
            signParams.Add(new KeyValuePair<string, string>("partnerid", _payRequest.PartnerId));
            signParams.Add(new KeyValuePair<string, string>("prepayid", _payRequest.PrepayId));
            signParams.Add(new KeyValuePair<string, string>("timestamp", _payRequest.TimeStamp));

            _payRequest.Sign = genAppSign(signParams);

            _sb.Append("sign\n" + _payRequest.Sign + "\n\n");

            //show.setText(sb.ToString());



        }

        private void SendPayReq()
        {
            Log.Info(Tag, "Constants.AppId" + Constants.AppId);

            var result = _msgApi.SendReq(_payRequest);
        }

        private string GenPackageSign(List<KeyValuePair<string, string>> param)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < param.Count; i++)
            {
                sb.Append(param[i].Key);
                sb.Append('=');
                sb.Append(param[i].Value);
                sb.Append('&');
            }
            sb.Append("key=");
            sb.Append(Constants.ApiKey);


            string packageSign = Md5.GetMessageDigest(sb.ToString().ToBytes()).ToUpper();
            return packageSign;
        }

        private string genAppSign(List<KeyValuePair<string, string>> param)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < param.Count; i++)
            {
                sb.Append(param[i].Key);
                sb.Append('=');
                sb.Append(param[i].Value);
                sb.Append('&');
            }
            sb.Append("key=");
            sb.Append(Constants.ApiKey);
            string appSign = Md5.GetMessageDigest(sb.ToString().ToBytes()).ToUpper();
            return appSign;
        }

        private string toXml(List<KeyValuePair<string, string>> param)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<xml>");
            for (int i = 0; i < param.Count; i++)
            {
                sb.Append("<" + param[i].Key + ">");

                sb.Append(param[i].Value);
                sb.Append("</" + param[i].Key + ">");
            }
            sb.Append("</xml>");


            return sb.ToString();
        }

    }


}