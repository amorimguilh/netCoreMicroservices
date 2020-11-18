using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ItemService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ItemService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<ItemController> _logger;
        private readonly ItemContext _context;

        public ItemController(ILogger<ItemController> logger, ItemContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<string> GetItems()
        {
            var result = _context.Items.Select(item => item.Description);
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] Item item)
        {
            if(item == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Naughty");
            }

            await _context.Items.AddAsync(item);
            var result = await _context.SaveChangesAsync();

            Response.StatusCode = (int)HttpStatusCode.Accepted;
            return Content("Nice");
        }
    }
}
