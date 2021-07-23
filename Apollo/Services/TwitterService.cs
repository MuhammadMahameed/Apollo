using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace Apollo.Services
{
    public class TwitterService
    {
        private readonly IConfiguration _configuration;
        public TwitterService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /*
        public async Task<IEnumerable> GetTweets()
        {
            var access_token = await GetAccessToken();
            var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format("https://api.twitter.com/2/users/17230018/tweets?count=5"));
            requestUserTimeline.Headers.Add("Authorization", "Bearer " + access_token);
            var httpClient = new HttpClient();
            HttpResponseMessage responseUserTimeLine = await httpClient.SendAsync(requestUserTimeline);
            var serializer = new JavaScriptSerializer();
            dynamic json = serializer.Deserialize<object>(await responseUserTimeLine.Content.ReadAsStringAsync());
            var enumerableTwitts = (json.data as IEnumerable<dynamic>);
            return enumerableTwitts;
        }
        */

        public async Task<string> GetTimelineEmbed(string url)
        {
            var requestOEmbed = new HttpRequestMessage(HttpMethod.Get, string.Format("https://publish.twitter.com/oembed?url={0}&theme=dark&dnt=true&limit=5&maxwidth=300&maxheight=400", url));
            var httpClient = new HttpClient();
            HttpResponseMessage responseOEmbed = await httpClient.SendAsync(requestOEmbed);
            var serializer = new JavaScriptSerializer();
            dynamic json = serializer.Deserialize<object>(await responseOEmbed.Content.ReadAsStringAsync());
            return json["html"];
        }

        public async Task<string> GetAccessToken()
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token");
            var customerInfo = Convert.ToBase64String(new UTF8Encoding().GetBytes(_configuration["Twitter:api_key"] + ":" + _configuration["Twitter:api_key_secret"]));
            request.Headers.Add("Authorization", "Basic " + customerInfo);
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            string json = await response.Content.ReadAsStringAsync();
            var serializer = new JavaScriptSerializer();
            dynamic item = serializer.Deserialize<object>(json);
            return item["access_token"];
        }
    }
}
