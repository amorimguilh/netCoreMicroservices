using System.Collections.Generic;

namespace  OrderService.Models
{
    public class Order 
    {
        public string ItemDescription { get; set; }
        public float Value { get; set; }
        public HashSet<string> AvailableCoupons { get; set; }
    }    
}
