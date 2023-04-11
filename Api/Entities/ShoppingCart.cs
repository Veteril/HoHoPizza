namespace API.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int FoodId { get; set; }

        public float Count { get; set; }
    }
}
