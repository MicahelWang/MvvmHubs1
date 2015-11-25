using System;

namespace Hubs1.Droid.Utils
{
    public class PayResult
    {
        public PayResult(string rawResult)
        {

            if (string.IsNullOrWhiteSpace(rawResult))
                return;

            string[] resultParams = rawResult.Split(';');
            foreach (string resultParam in resultParams)
            {
                if (resultParam.StartsWith("resultStatus"))
                {
                    ResultStatus = GetValue(resultParam, "resultStatus");
                }
                if (resultParam.StartsWith("result"))
                {
                    Result = GetValue(resultParam, "result");
                }
                if (resultParam.StartsWith("memo"))
                {
                    Memo = GetValue(resultParam, "memo");
                }
            }
        }

        public string ResultStatus { get; }

        public string Result { get; }

        public string Memo { get; }

        public override string ToString()
        {
            return "resultStatus={" + ResultStatus + "};memo={" + Memo
                    + "};result={" + Result + "}";
        }

        private string GetValue(string content, string key)
        {
            string prefix = key + "={";
            return content.Substring(content.IndexOf(prefix, StringComparison.Ordinal) + prefix.Length,
                    content.LastIndexOf("}", StringComparison.Ordinal) - prefix.Length);
        }

    }
}