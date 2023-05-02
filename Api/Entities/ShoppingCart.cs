namespace API.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public float Count { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int FoodSlotId { get; set; }

        public FoodSlot FoodSlot { get; set;}

    }
}
