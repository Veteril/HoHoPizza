namespace API.Entities
{
    public class FoodSlot
    {
        public int Id {  get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public string ImageUrl { get; set; }

        public float Weight { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
