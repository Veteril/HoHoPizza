namespace API.Entities
{
    public class FoodSlot
    {
        public int Id {  get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public string ImageUrl { get; set; }

        public float Weight { get; set; }

        public int IngridientId { get; set; }
        
    }
}
