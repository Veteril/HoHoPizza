namespace API.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PaymentTypeId { get; set; }

        public int OrderStatusId { get; set; }

        public OrderStatus OrderStatus { get; set; }

    }
}
