using System.Net;

namespace CreatTransferTransactiom.DLL
{
    public class HTTPl
    {
        public static HttpWebRequest CreateWebRequestTransfer()
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var CreateTransfereUrl = MyConfig.GetValue<string>("AppSettings:CreateTransfer");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(CreateTransfereUrl);
            webRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }
    }
}
