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
        private static readonly string itemUri = "http://item-service/";

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<OrderResponse>> Get()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(itemUri);
                //HTTP GET
                var result = await client.GetAsync("item");
                
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var items = JsonConvert.DeserializeObject<string[]>(json);
                    var order = new OrderResponse{
                        Items = items,
                        OrderDescription = "This is a order description",
                        Value = 50
                    };

                    return new List<OrderResponse> {
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
                client.BaseAddress = new Uri(itemUri);
                var jsonObject = JsonConvert.SerializeObject(new Item {
                    Description = order.ItemDescription
                });
                var result = await client.PostAsync("item", new StringContent(jsonObject, Encoding.UTF8, "application/json"));
                
                Response.StatusCode = (int)result.StatusCode;
                return Content(result.StatusCode.ToString());
            }
        }
    }
}
