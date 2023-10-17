using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mastercom_api.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

namespace mastercom_api.Helper
{
    public class MastercomAPI
    {

        private readonly Config _appsettings;
        public MastercomAPI(IOptions<Config> options)
        {
            _appsettings = options.Value;
            Console.WriteLine(_appsettings.endpoint);
        }

        public async Task<HtmlDocument> doRequest(GenericEndpointRequest loginData, string page)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_appsettings.endpoint}?form_stato=studente&stato_principale={page}&stato_secondario=&permission&operazione&current_user={loginData.UserId}&current_key={loginData.UserKey}&db_key=mastercom_2023_2024&tipo_utente=studente&header=SI&from_app");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return htmlDoc;
        }

        public async Task<HtmlDocument> doLogin(LoginRequest loginData)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_appsettings.endpoint}?password_user={loginData.PasswordUser}&user={loginData.User}&form_login=true");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return htmlDoc;
        }
    }
}