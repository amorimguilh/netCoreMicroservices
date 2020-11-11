using Microsoft.EntityFrameworkCore;

namespace CouponService.Models
{
    public class CouponContext : DbContext
    {
        public CouponContext(DbContextOptions<CouponContext> options) : base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }
    }
}