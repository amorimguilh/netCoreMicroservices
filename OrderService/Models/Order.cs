using System.Collections.Generic;

namespace  OrderService.Models
{
    public class Order 
    {
        public string OrderDescription { get; set; }
        public float Value { get; set; }
        public HashSet<Item> AvailableItems { get; set; }
    }    
}
