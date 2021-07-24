using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Apollo.Services
{
    public class BranchService
    {
        private readonly IConfiguration _configuration;
        public BranchService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<byte[]> GetBingMap(string coordinates)
        {
            var url = "https://dev.virtualearth.net/REST/v1/Imagery/Map/Road/?" + coordinates + "format=png&key=" + _configuration["Bing:api_key"];
            var requestMap = new HttpRequestMessage(HttpMethod.Get, url);
            var httpClient = new HttpClient();
            HttpResponseMessage responseMap = await httpClient.SendAsync(requestMap);
            var stream = await responseMap.Content.ReadAsByteArrayAsync();
            return stream;
        }
    }
}
