using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using SystemsUI.Library.Models;

namespace SystemsUI.Library.API
{
    public class DebugEndpoint : IDebugEndpoint
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public DebugEndpoint(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }

        public async Task<List<DebugProcessModel>> GetAll()
        {
            using (HttpResponseMessage response = await _client.GetAsync($"PosDebug/GetWithParams"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<DebugProcessModel>>();
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
