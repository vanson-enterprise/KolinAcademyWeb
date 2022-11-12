using System.Security.Cryptography;
using System.Text;

namespace KA.PaymentAPI.CyberSource
{
    public static class CyberSourceHelper
    {
        public static string GenerateDigest(string bodyText)
        {
            var digest = "";
            using (var sha256hash = SHA256.Create())
            {
                byte[] payloadBytes = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(bodyText));
                digest = Convert.ToBase64String(payloadBytes);
                digest = "SHA-256=" + digest;
            }
            return digest;
        }

        public static string GenerateSignatureFromParams(string signatureParams, string secretKey)
        {
            var sigBytes = Encoding.UTF8.GetBytes(signatureParams);
            var decodeSecret = Convert.FromBase64String(secretKey);
            var hmacSha256 = new HMACSHA256(decodeSecret);
            var messageHash = hmacSha256.ComputeHash(sigBytes);
            return Convert.ToBase64String(messageHash);
        }

        public static String sign(String data, String secretKey)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secretKey);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);
            byte[] messageBytes = encoding.GetBytes(data);
            return Convert.ToBase64String(hmacsha256.ComputeHash(messageBytes));
        }

        public static string BuildDataToSign(IDictionary<string, string> paramDict)
        {
            string[] signedFieldNames = paramDict["signed_field_names"].Split(",");
            IList<string> dataToSign = new List<string>();
            foreach (string signedFieldName in signedFieldNames)
            {
                dataToSign.Add(signedFieldName + "=" + paramDict[signedFieldName]);
            }
            return String.Join(",", dataToSign);
        }
    }
}
