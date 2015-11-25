using System;
using System.Text;
using Java.Security;
using Java.Security.Spec;

namespace Hubs1.Droid.Utils
{
    public class SignUtils
    {
        private const string Algorithm = "RSA";

        private const string SignAlgorithms = "SHA1WithRSA";

        private const string DefaultCharset = "UTF-8";

        public static string Sign(string content, string privateKey)
        {
            try
            {
                PKCS8EncodedKeySpec priPkcs8 = new PKCS8EncodedKeySpec(Convert.FromBase64String(privateKey));
                //Base64.decode(privateKey));
                KeyFactory keyf = KeyFactory.GetInstance(Algorithm, "BC");
                IPrivateKey priKey = keyf.GeneratePrivate(priPkcs8);

                Signature signature = Signature.GetInstance(SignAlgorithms);

                signature.InitSign(priKey);
                signature.Update(Encoding.Default.GetBytes(content));

                byte[] signed = signature.Sign();

                return Convert.ToBase64String(signed);
            }
            catch (Exception e)
            {
                string mesg = e.StackTrace;
            }

            return null;
        }
    }
}