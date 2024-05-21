    namespace TodoApi.Models
    {
        public class Product
        {
            public int Id {get; set;}
            public string Name{get; set;}
            public string? Description {get; set;}
            public int Price {get; set;}
            public string Category{get; set;}
            public string? Subcategory {get; set;}
        }

        public class LoginRequest
        {
        public string Username { get; set; }
        public string Password { get; set; }
        }

    }