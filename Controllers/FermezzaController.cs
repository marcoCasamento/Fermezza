using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Fermezza.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Fermezza.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class FermezzaController : ControllerBase
    {
        readonly HttpClient client;
        private readonly ILogger<FermezzaController> logger;

        public FermezzaController(IOptions<HaOptions> options, ILogger<FermezzaController> logger)
        {

            client = new HttpClient()
            {
                BaseAddress = new Uri(options.Value.BaseUrl)
            };
            client.DefaultRequestHeaders
                .Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", 
                options.Value.HaToken
                );

            Options = options;
            this.logger = logger;
        }

        public IOptions<HaOptions> Options { get; }

        [HttpGet("apriCancello")]
        public async Task<bool> apriCancello()
        {
            var parameter = new haIdEntity()
            {
                entity_id = "switch.cancello_fermezza"
            };
            var body = JsonConvert.SerializeObject(parameter);

            var response = await client.PostAsync($"services/switch/turn_on", new StringContent(body, Encoding.UTF8, "application/json"));
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return false;

            var responseContent = await response.Content.ReadAsStringAsync();

            return true;
        }
        [HttpGet("getWeather")]
        public async Task<WeatherData[]> getWeather()
        {
            List<WeatherData> weatherData = new List<WeatherData>();

            var response = await client.GetAsync("states/sensor.temperatura_esterna");
            var responseContent = await response.Content.ReadAsStringAsync();
            var ent = JsonConvert.DeserializeObject<haEntity>(responseContent);
            weatherData.Add(new WeatherData { Title = "Temperatura", LastUpdate = ent.last_updated, Value = ent.state + ent.attributes.unit_of_measurement });

            response = await client.GetAsync("states/sensor.umidita_relativa_esterna");
            responseContent = await response.Content.ReadAsStringAsync();
            ent = JsonConvert.DeserializeObject<haEntity>(responseContent);
            weatherData.Add(new WeatherData { Title = "Umidità relativa", LastUpdate = ent.last_updated, Value = ent.state + ent.attributes.unit_of_measurement });

            response = await client.GetAsync("states/sensor.pressione_atmosferica_0_slm");
            responseContent = await response.Content.ReadAsStringAsync();
            ent = JsonConvert.DeserializeObject<haEntity>(responseContent);
            weatherData.Add(new WeatherData { Title = "Pressione 0 mslm", LastUpdate = ent.last_updated, Value = ent.state + " " + ent.attributes.unit_of_measurement });

            response = await client.GetAsync("states/sensor.current_uv_index");
            responseContent = await response.Content.ReadAsStringAsync();
            ent = JsonConvert.DeserializeObject<haEntity>(responseContent);
            weatherData.Add(new WeatherData { Title = "Indice UV", LastUpdate = ent.last_updated, Value = ent.state});

            response = await client.GetAsync("states/sensor.current_ozone_level");
            responseContent = await response.Content.ReadAsStringAsync();
            ent = JsonConvert.DeserializeObject<haEntity>(responseContent);
            weatherData.Add(new WeatherData { Title = "Conc. Ozono", LastUpdate = ent.last_updated, Value = ent.state + " " + ent.attributes.unit_of_measurement });

            response = await client.GetAsync("states/sensor.sun_elevation");
            responseContent = await response.Content.ReadAsStringAsync();
            ent = JsonConvert.DeserializeObject<haEntity>(responseContent);
            weatherData.Add(new WeatherData { Title = "Elevazione Sole", LastUpdate = ent.last_updated, Value = ent.state + ent.attributes.unit_of_measurement });

            return weatherData.ToArray();

        }
    }


    //public class haEntity
    //{
    //    public string entity_id { get; set; }
    //}

    public class WeatherData
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public class haIdEntity
    {
        public string entity_id { get; set; }
    }
    public class haEntity
    {
        public Attributes attributes { get; set; }
        public Context context { get; set; }
        public string entity_id { get; set; }
        public DateTime last_changed { get; set; }
        public DateTime last_updated { get; set; }
        public string state { get; set; }
    }

    public class Attributes
    {
        public string friendly_name { get; set; }
        public string icon { get; set; }
        public string unit_of_measurement { get; set; }
    }

    public class Context
    {
        public string id { get; set; }
        public object parent_id { get; set; }
        public object user_id { get; set; }
    }

}
