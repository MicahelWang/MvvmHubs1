using Java.Lang;
using Java.Security;

namespace Hubs1.Droid.Utils.weixin
{

    public class Md5Util
    {

        private static string ByteArrayToHexString(byte[] b)
        {
            StringBuffer resultSb = new StringBuffer();
            for (int i = 0; i < b.Length; i++)
                resultSb.Append(ByteToHexString(b[i]));

            return resultSb.ToString();
        }

        private static string ByteToHexString(byte b)
        {
            int n = b;
            if (n < 0)
                n += 256;
            int d1 = n / 16;
            int d2 = n % 16;
            return HexDigits[d1] + HexDigits[d2];
        }

        public static string Md5Encode(string origin, string charsetname)
        {
            string resultString = null;
            try
            {
                resultString = origin;
                MessageDigest md = MessageDigest.GetInstance("MD5");
                if (charsetname == null || "".Equals(charsetname))
                    resultString = ByteArrayToHexString(md.Digest(resultString
                            .ToBytes()));
                else
                    resultString = ByteArrayToHexString(md.Digest(resultString
                            .ToBytes(charsetname)));
            }
            catch (System.Exception exception)
            {
            }
            return resultString;
        }

        private static readonly string[] HexDigits = { "0", "1", "2", "3", "4", "5",
			"6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

}

}