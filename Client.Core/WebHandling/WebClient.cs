using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UASD.Properties;

namespace UASD.WebHandling
{
    public class WebClient
    {
        public CookieContainer[] Cookies = new CookieContainer[] { new CookieContainer() };
        private HttpClientHandler ClientHandler;
        private HttpClient Client;

        public WebClient()
        {
            ClientHandler = new HttpClientHandler() { CookieContainer = Cookies[0] };
            Client = new HttpClient(ClientHandler);
        }
        public WebClient(CookieContainer[] cc)
        {
            Cookies = cc;
            ClientHandler = new HttpClientHandler() { CookieContainer = Cookies[0] };
            Client = new HttpClient(ClientHandler);
        }

        public async Task<string> DownloadStringAsync(Uri uri)
        {
            HttpResponseMessage response = await Client.GetAsync(uri);
            PerformCheck(uri, response);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UploadStringAsync(Uri uri, string message)
        {
            HttpResponseMessage response = await Client.PostAsync(uri, new StringContent(message));
            PerformCheck(uri, response);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> UploadStringAsync(string url, string message) => await UploadStringAsync(new Uri(url), message);

        public void ClearCookies()
        {
            Cookies[0] = new CookieContainer();
            ClientHandler = new HttpClientHandler() { CookieContainer = Cookies[0] };
            Client = new HttpClient(ClientHandler);
        }

        private void PerformCheck(Uri ru, HttpResponseMessage r)
        {
            if ("http://" + ru.Host + '/' == Strings.BaseUrl)
            {
                ClearCookies();
                foreach (string cookieHeader in r.Headers.GetValues("Set-Cookie"))
                    Cookies[0].SetCookies(new Uri(Strings.BaseUrl), cookieHeader);
            }
        }
    }
}
