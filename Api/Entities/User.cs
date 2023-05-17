namespace API.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }

        public string UserPhone { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public float Money { get; set; }

        public bool IsAdmin { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<Order> Orders { get; }
    }
}
