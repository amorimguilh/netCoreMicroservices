using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
        private static readonly string itemUri = "http://item-service:6000/";

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
                client.BaseAddress = new Uri(itemUri);
                //HTTP GET
                var result = await client.GetAsync("item");
                
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var coupons = JsonConvert.DeserializeObject<Item[]>(json);
                    var order = new Order{
                        AvailableItems = new HashSet<Item>(coupons),
                        OrderDescription = "This is a description",
                        Value = 50
                    };

                    return new List<Order> {
                        order
                    };
                }
            }
            return null;
        }
    
    
        [HttpGet("check")]
        public async Task<string> Check()
        {
            return await Task.Run(()=> "Working fine");
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] Order order)
        {   
            if(order == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Naughty");
            }

            using (var client = new HttpClient())
            {
                var uri = itemUri + "item";
                var jsonObject = JsonConvert.SerializeObject(order.AvailableItems.First());
                var httpContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(uri, httpContent);
                
                Response.StatusCode = (int)result.StatusCode;
                return Content(result.StatusCode.ToString());
            }
        }
    }
}
