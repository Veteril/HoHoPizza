namespace API.Entities
{
    public class FoodSlot
    {
        public int id {  get; set; }

        public string name { get; set; }

        public float price { get; set; }

        public string imageUrl { get; set; }

        public float weight { get; set; }

        public int ingridientId { get; set; }
    }
}
