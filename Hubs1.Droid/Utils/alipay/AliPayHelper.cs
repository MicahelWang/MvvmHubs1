using System;
using Java.IO;

namespace Hubs1.Droid.Utils.alipay
{
    public static class AlipayHelper
    {


        // 商户PID
        private const string Partner = "2088601988973166";
        // 商户收款账号
        private const string Seller = "daniel.sheng@hubs1.net";
        // 商户私钥，pkcs8格式
        private const string RsaPrivate =
            "MIICXQIBAAKBgQDqZ7GvLXOs3jGrL5CadN8sCJDN5z4j/440ecByCOFER2JaUxYqgP1xkxoO08jsMrb8xIVH8ZVk8Bh+FRlGPv33HmSNeFeWLD99Hmcez7q3ufQjgM9+nSysB3ZqH5PbC61u0viBRzumKNNZPcD5cGVXf1/6gbXwUSl7VdUZtb+BQwIDAQABAoGANEXxmwZA0KEwZGOUFM3RYwl5+WEDO5XnCY7JOuT+D3L33PrAa7QeUPCkFABowzvAEPhmXJfkYAEnBR8BZufvTHpeh+iB+0ytsqjghXF7vjIS4NOINX2i7dxkdw6Mgc6z2WTrnZrSHYQQrxo3sfuv62hbR/hkd5wlXNbEpXo8sdkCQQD7+GOCXuBbMF7QqM17/bOlIz4eyiIVrOgJHJ38APwZPZacQTajPSFhjQW55fjTuEnOF0gfaXQviDE97Q4f94gtAkEA7idj52ZiAeB78/gvjkHCfHVXIFEea1xSTrWpi0uJ2XJ841alLKkw7MqAPFuRcnsl5SewwrxO/68cKdoiD3slLwJBAKwI+ZgnwzM/oV0CA9pma+OSXnWTORZnib2+5jcLTRTGsjBZbj6iHEqIiKJWXD/iEbI+vyWDUKqraEOJuR0mWrkCQQDAtz1f3njGvgpwFVEeU9Z28llLvHjaNf4g1V5aawkk3kFzkgTdjBgcNY3pyct8EW1L739ZRhrDSseGko7naiZfAkBfNzf1KGmC2unVG8XfK4Mb/RbOaEa8wRhxgLwe6qIG2QdolQ/xQp2eWp0tB+tlixU+Z3i990opUD0oYFO+bRuQ";

        // 支付宝公钥
        private const string RsaPublic =
            "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCfG5mphhQOkt4hmz/J63wBHGAuJnctT4DRt1318futmT4i23cz0SCc4xhGFTwPLQPSwkggPWSrSKBO5V7dHobVrO4gRTncUeolRw7MmLouyRXopF1mgoBJanAkhTuoAl4Dtslfidkaqtc3APSmgVcRDarK2+kKU9NgQ3/Ebcb6UQIDAQAB";

        public static bool CheckConfig()
        {
            if (string.IsNullOrWhiteSpace(Partner) || string.IsNullOrWhiteSpace(RsaPrivate)
                    || string.IsNullOrWhiteSpace(Seller))
            {

                return false;
            }
            return true;

        }

        /// <summary>
        /// get the out_trade_no for an order. 生成商户订单号，该值在商户端应保持唯一（可自定义格式规范）
        /// </summary>
        /// <returns></returns>
        private static string GetOutTradeNo()
        {
            string key = DateTime.Now.ToString("MMddHHmmss");

            Random r = new Random();
            key = key + r.Next();
            key = key.Substring(0, 15);
            return key;
        }

        /// <summary>
        /// create the order info. 创建订单信息
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        private static string GetOrderInfo(string subject, string body, string price)
        {

            // 签约合作者身份ID
            string orderInfo = "partner=" + "\"" + Partner + "\"";

            // 签约卖家支付宝账号
            orderInfo += "&seller_id=" + "\"" + Seller + "\"";

            // 商户网站唯一订单号
            orderInfo += "&out_trade_no=" + "\"" + GetOutTradeNo() + "\"";

            // 商品名称
            orderInfo += "&subject=" + "\"" + subject + "\"";

            // 商品详情
            orderInfo += "&body=" + "\"" + body + "\"";

            // 商品金额
            orderInfo += "&total_fee=" + "\"" + price + "\"";

            // 服务器异步通知页面路径
            orderInfo += "&notify_url=" + "\"" + "http://notify.msp.hk/notify.htm"
                         + "\"";

            // 服务接口名称， 固定值
            orderInfo += "&service=\"mobile.securitypay.pay\"";

            // 支付类型， 固定值
            orderInfo += "&payment_type=\"1\"";

            // 参数编码， 固定值
            orderInfo += "&_input_charset=\"utf-8\"";

            // 设置未付款交易的超时时间
            // 默认30分钟，一旦超时，该笔交易就会自动被关闭。
            // 取值范围：1m～15d。
            // m-分钟，h-小时，d-天，1c-当天（无论交易何时创建，都在0点关闭）。
            // 该参数数值不接受小数点，如1.5h，可转换为90m。
            orderInfo += "&it_b_pay=\"30m\"";

            // extern_token为经过快登授权获取到的alipay_open_id,带上此参数用户将使用授权的账户进行支付
            // orderInfo += "&extern_token=" + "\"" + extern_token + "\"";

            // 支付宝处理完请求后，当前页面跳转到商户指定页面的路径，可空
            orderInfo += "&return_url=\"m.alipay.com\"";

            // 调用银行卡支付，需配置此参数，参与签名， 固定值 （需要签约《无线银行卡快捷支付》才能使用）
            // orderInfo += "&paymethod=\"expressGateway\"";

            return orderInfo;
        }

        /// <summary>
        /// sign the order info. 对订单信息进行签名
        /// </summary>
        /// <param name="content">待签名订单信息</param>
        /// <returns></returns>

        private static string Sign(string content)
        {
            return SignUtils.Sign(content, RsaPrivate);
        }

        /// <summary>
        /// get the sign type we use. 获取签名方式
        /// </summary>
        /// <returns></returns>
        private static string GetSignType()
        {
            return "sign_type=\"RSA\"";
        }


        public static string GetPayInfo()
        {

            // 订单
            string orderInfo = GetOrderInfo("测试的商品", "该测试商品的详细描述", "0.01");

            // 对订单做RSA 签名
            string sign = Sign(orderInfo);
            try
            {
                // 仅需对sign 做URL编码
                sign = Java.Net.URLEncoder.Encode(sign, "UTF-8");
            }
            catch (UnsupportedEncodingException ex)
            {
                ex.PrintStackTrace();
            }

            string payInfo = orderInfo + "&sign=\"" + sign + "\"&"
                                            + GetSignType();
            return payInfo;



        }



    }
}