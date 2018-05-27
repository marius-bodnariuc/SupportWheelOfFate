using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SupportWheelOfFate.API.Test
{
    public class SchedulesWebRequestShould
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public SchedulesWebRequestShould()
        {
            _server = new TestServer(
                new WebHostBuilder().UseStartup<StartupForIntegrationTests>());

            _client = _server.CreateClient();
        }

        [Fact]
        public async Task ReturnWorkSchedulesCollectionAsJson()
        {
            var response = await _client.GetAsync("/api/schedules");
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            var schedules = JsonConvert.DeserializeObject<IEnumerable<ScheduleDTO>>(stringContent);

            Assert.Equal(4, schedules.Count());
        }
    }
}
