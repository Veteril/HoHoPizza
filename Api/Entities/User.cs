namespace API.Entities
{
    public class User
    {
        public int id { get; set; }
        
        public string userName { get; set; }

        public string password { get; set; }

        public float money { get; set; }

        public bool isAdmin { get; set; }
    }
}
