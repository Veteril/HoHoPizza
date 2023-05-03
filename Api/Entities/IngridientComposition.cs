namespace API.Entities
{
    public class IngridientComposition
    {
        public int Id { get; set; }
        
        public float Weight { get; set; }

        public int FoodSlotId { get; set; }

        public int IngridientId { get; set; }

        public FoodSlot FoodSlot { get; set; }

        public Ingridient Ingridient { get; set; }
    }
}
