using System;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using UASD.WebHandling;

namespace UASD
{
    using System.Threading.Tasks;
    using Utilities;
    public partial class Client
    {
        public static Uri BaseUri = new Uri(Properties.Strings.BaseUrl);
        private CookieContainer[] SessionID = new CookieContainer[1];

        public Client() { }

        public async Task<bool> LoginAsync(string ID, string NIP)
        {
            WebClient Client = new WebClient();
            Client.Cookies[0].Add(BaseUri, new Cookie("TESTID", "set"));
            string Response = await Client.UploadStringAsync(Properties.Strings.LoginUrl, $"sid={ID}&PIN={NIP}");
            if (Response.Contains("NIP ha expirado")) {
                throw new ExpiredLoginException();
            }
            else if (Response.Contains("<meta http-equiv=\"refresh\""))
            {
                this.Username = Convert.Username(Response);
                this.SessionID = Client.Cookies;
                IsLoggedIn = true;
                return true;
            }
            IsLoggedIn = false;
            return false;
        }
        private WebClient GetClient()
        {
            if (this.SessionID == null)
            {
                IsLoggedIn = false;
                throw new NotLoggedInException();
            }
            return new WebClient(this.SessionID);
        }
        private bool CheckStatus(HtmlDocument doc)
        {
            try
            {
                IsLoggedIn = !doc.GetElementsByTagName("title").First().InnerText.Contains("Acceso");
            }
            catch
            {
                IsLoggedIn = false;
                throw new NoDataReceivedException();
            }
            return !IsLoggedIn;
        }
        private async Task<HtmlDocument> ReceiveAsync(Uri uri)
        {
            HtmlDocument Response = new HtmlDocument();
            Response.LoadHtml(await GetClient().DownloadStringAsync(uri));
            if (CheckStatus(Response)) throw new NotLoggedInException();
            return Response;
        }
        private async Task<HtmlDocument> ReceiveAsync(string url) => await ReceiveAsync(new Uri(url));
        private async Task<HtmlDocument> SubmitAsync(string url, string data)
        {
            HtmlDocument Response = new HtmlDocument();
            Response.LoadHtml(await GetClient().UploadStringAsync(url, data));
            if (CheckStatus(Response)) throw new NotLoggedInException();
            return Response;
        }
        public void Logout() { IsLoggedIn = false; }
    }
}
