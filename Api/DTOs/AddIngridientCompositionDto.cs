namespace API.DTOs
{
    public class AddIngridientCompositionDto
    {
        public float Weight { get; set; }

        public int FoodSlotId { get; set; }

        public int IngridientId { get; set; }
    }
}
