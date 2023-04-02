namespace API.Entities
{
    public class ShoppingCart
    {
        public int id { get; set; }

        public int orderId { get; set; }

        public int FoodId { get; set; }

        public float Count { get; set; }
    }
}
