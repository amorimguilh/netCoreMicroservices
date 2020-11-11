using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CouponService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CouponService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ILogger<CouponController> _logger;
        private readonly CouponContext _context;

        public CouponController(ILogger<CouponController> logger, CouponContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            IEnumerable<string> result = null;
                result = _context.Coupons.Select(cp => cp.Code);
               
            return result;
        }
    }
}
