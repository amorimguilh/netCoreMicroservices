namespace OrderService.Models
{
    public class OrderResponse
    {
        public string OrderDescription { get; set; }
        public float Value { get; set; }
        public string[] Items { get; set; }
    }    
}
