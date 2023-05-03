namespace API.DTOs
{
    public class GetFoodSlotDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<GetIngridientCompositionDto> IngridientCompositions { get; set; }

    }
}
