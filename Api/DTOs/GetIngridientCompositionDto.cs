namespace API.DTOs
{
    public class GetIngridientCompositionDto
    {
        public int Id { get; set; }
        public float Weight { get; set; }
        public int IngridientId { get; set; }
        public string IngridientName { get; set; }
    }
}
