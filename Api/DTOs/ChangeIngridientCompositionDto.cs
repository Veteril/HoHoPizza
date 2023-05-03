namespace API.DTOs
{
    public class ChangeIngridientCompositionDto
    {
        public int Id { get; set; }

        public float Weight { get; set; }

        public int FoodSlotId { get; set; }

        public int IngridientId { get; set; }
    }
}
