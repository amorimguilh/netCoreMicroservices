using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderService.Models;


namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            // Consume another microservice to get the working coupons
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://couponapi/");
                //HTTP GET
                var result = await client.GetAsync("coupon");
                
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var coupons = JsonConvert.DeserializeObject<string[]>(json);
                    var order = new Order{
                        AvailableCoupons = new HashSet<string>(coupons),
                        ItemDescription = "This is a description",
                        Value = 50
                    };

                    return new List<Order> {
                        order
                    };
                }
            }

            return null;
        }
    }
}
