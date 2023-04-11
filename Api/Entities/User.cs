﻿namespace API.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }

        public string Password { get; set; }

        public float Money { get; set; }

        public bool IsAdmin { get; set; }
    }
}
