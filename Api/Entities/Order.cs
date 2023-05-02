namespace API.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public float TotalPrice { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public int PaymentTypeId { get; set; }

        public int OrderStatusId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public PaymentType PaymentType { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
