namespace API.DTOs
{
    public class ShoppingCartDto
    {
        public float Count { get; set; }

        public FoodSlotInOrderDto FoodSlot { get; set; }
    }
}
