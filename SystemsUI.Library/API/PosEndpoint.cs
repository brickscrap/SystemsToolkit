using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SystemsUI.Library.Models;

namespace SystemsUI.Library.API
{
    public class PosEndpoint : IPosEndpoint
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public PosEndpoint(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }

        public async Task<List<PosModel>> GetAllByStationId(string stationId)
        {
            using (HttpResponseMessage response = await _client.GetAsync($"pos/{stationId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<PosModel>>();
                    return result;
                }
                else
                {
                    // TODO: Handle this exception properly
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
