using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Apollo.Data;
using Apollo.Models;
using Microsoft.Extensions.Configuration;

namespace Apollo.Services
{
    public class BranchService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public BranchService(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<byte[]> GetBingMap(string coordinates)
        {
            if (coordinates != null)
            {
                var url = "https://dev.virtualearth.net/REST/v1/Imagery/Map/CanvasDark/?" + coordinates + "format=png&mapSize=730,730&key=" + _configuration["Bing:api_key"];
                var requestMap = new HttpRequestMessage(HttpMethod.Get, url);
                var httpClient = new HttpClient();
                HttpResponseMessage responseMap = await httpClient.SendAsync(requestMap);
                var stream = await responseMap.Content.ReadAsByteArrayAsync();
                return stream;
            }

            return Array.Empty<byte>();
        }

        public ArrayList FilterBranches(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return new ArrayList();
            }

            var strToLower = str.ToLower();

            var matchingBranches = _context.Branch
                .Where(s => s.AddressName.ToLower().Contains(strToLower) ||
                            s.Coordinate.ToLower().Contains(strToLower))
                .ToList();

            ArrayList matchingBranchesList = new ArrayList();

            foreach (Branch branch in matchingBranches)
            {
                matchingBranchesList.Add(new
                {
                    id = branch.Id,
                    addressName = branch.AddressName,
                    coordinate = branch.Coordinate
                });
            }

            return matchingBranchesList;
        }
    }
}
