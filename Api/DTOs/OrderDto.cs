using API.Entities;

namespace API.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }

        public UserDto User { get; set; }

        public float TotalPrice { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public PaymentType PaymentType { get; set; }

        public IEnumerable<ShoppingCartDto> ShoppingCarts { get; set; }
    }
}
