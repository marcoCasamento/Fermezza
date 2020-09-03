using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fermezza.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FermezzaController : ControllerBase
    {
        readonly HttpClient client;
        public FermezzaController()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri("http://192.168.1.205:8123/api/")
            };
            client.DefaultRequestHeaders
                .Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhOWE3MjU3YTE4NDA0ZGUwYjgyYWE5ZWQwYTZhZGZkYSIsImlhdCI6MTU4Mzk0NTY3NiwiZXhwIjoxODk5MzA1Njc2fQ.MGcD6DfIDuqp8gkx-69xCU7q9jwwb5l7__5G6aOTqD0");
        }
        [HttpGet("apriCancello")]
        public async Task<bool> apriCancello()
        {
            var parameter = new haEntity()
            {
                entity_id = "switch.cancello_fermezza"
            };
            var body = JsonConvert.SerializeObject(parameter);
            
            var response = await client.PostAsync($"services/switch/turn_on", new StringContent(body, Encoding.UTF8, "application/json"));
            var responseContent = await response.Content.ReadAsStringAsync();

            return true;
        }
    }


    public class haEntity
    {
        public string entity_id { get; set; }
    }

}
