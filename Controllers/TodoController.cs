using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UAParser;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class TodoController : Controller
    {
        private readonly IConfiguration config;

        public TodoController(IConfiguration configuration)
        {   
            config = configuration;
        }
        
        // GET
        [HttpGet("{ip}")]
        public async Task<string> GetLocation(string ip)
        {            
            string apiResponse;
            string url = config["AzureMapsHost"];
            string subscriptionKey = config["AzureMapsSubscriptionKey"];
            parseUA(config["UserAgent"]);

            using (var httpClient = new HttpClient())
            {
                string finalUrl = url + ip + subscriptionKey;
                using (var response = await httpClient.GetAsync(finalUrl))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    return apiResponse;
                }                
            }
        }
        
        private void parseUA(string userAgent)
        {
            var parser = Parser.GetDefault();
            var parsed = parser.Parse(userAgent);
            Console.WriteLine("Device - " + parsed.Device);
            Console.WriteLine("OS - " + parsed.OS);
            Console.WriteLine("UA - " + parsed.UA);
        }
    }
}
