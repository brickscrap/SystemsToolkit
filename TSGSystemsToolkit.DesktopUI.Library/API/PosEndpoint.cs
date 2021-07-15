using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TSGSystemsToolkit.DesktopUI.Library.Models;

namespace TSGSystemsToolkit.DesktopUI.Library.API
{
    public class PosEndpoint : IPosEndpoint
    {
        private readonly IAPIHelper _apiHelper;

        public PosEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<POSModel>> GetAllByStationId(string stationId)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/pos/{stationId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<POSModel>>();

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
