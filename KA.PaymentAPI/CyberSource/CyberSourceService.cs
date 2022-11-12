using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.PaymentAPI.CyberSource
{
    public class CyberSourceService
    {
        string SecretKey = "Ms06+ltqDxD4iD8wdnibejJ4ziuUzdPhqh0cb/lmW1A=";
        string KeyId = "a450966c-db47-4ed4-80ea-57b60c789915";
        string MerchantId = "kla001";
        private readonly IHttpClientFactory _httpClientFactory;

        public CyberSourceService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async void SendRequest(string jsonObj)
        {
            var digest = CyberSourceHelper.GenerateDigest(jsonObj);
            //var signatureParams = @$"host: apitest.cybersource.com\ndate: Thu, 18 Jul 2019 00:18:03 GMT\n(request-target): post /pts/v2/payments/\ndigest: {digest}\nv-c-merchant-id: {MerchantId}";
            var signatureParams = "host: apitest.cybersource.com\n(request-target): post /pts/v2/payments/\ndigest: " + digest + "\nv-c-merchant-id: " + MerchantId;
            var signatureHash = CyberSourceHelper.GenerateSignatureFromParams(signatureParams, SecretKey);

            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri("https://apitest.cybersource.com");
            httpClient.DefaultRequestHeaders.Add("v-c-merchant-id", MerchantId);
            httpClient.DefaultRequestHeaders.Add("v-c-date", DateTime.Now.ToString("ddd, dd MMM yyy HH':'mm':'ss 'GMT'"));
            httpClient.DefaultRequestHeaders.Add("Digest", digest);
            httpClient.DefaultRequestHeaders.Add("Signature", "keyid=\"" + KeyId + "\", algorithm=\"HmacSHA256\", headers=\"host (request-target) digest v-c-merchant-id\", signature=\"" + signatureHash + "\"");
            httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

            var requestData = new StringContent(jsonObj, Encoding.UTF8, "application/json");

            try
            {
                var httpResponse = await httpClient.PostAsync("/pts/v2/payments/", requestData);
                var result = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}

//https://github.com/sarn1/example-cybersource-csharp-api-rest/blob/e53425a1f8c68115d178d6cba43ec4e134c7964c/rest-sample/rest-sample/Program.cs#L78
//https://developer.cybersource.com/docs/cybs/en-us/platform/get-started/all/rest/get-started-rest/authentication/GenerateHeader/httpSignatureAuthentication.html
//https://developer.cybersource.com/api-reference-assets/index.html#payments_payments_process-a-payment_samplerequests-dropdown_simple-authorization-internet_liveconsole-tab-request-headers
//https://sarn.phamornsuwana.com/2019/03/31/c-code-for-cybersource-rest-apis/
