using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SystemsUI.Library.Models;

namespace SystemsUI.Library.API
{
    public class StationEndpoint : IStationEndpoint
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public StationEndpoint(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }

        public async Task<List<StationModel>> GetAll()
        {
            using (HttpResponseMessage response = await _client.GetAsync("station"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<StationModel>>();

                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
