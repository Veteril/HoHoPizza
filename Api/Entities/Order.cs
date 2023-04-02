namespace API.Entities
{
    public class Order
    {
        public int id { get; set; }

        public int userId { get; set; }

        public int paymentTypeId { get; set; }

        public int orderStatusId { get; set; }

    }
}
