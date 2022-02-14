using System;

namespace RESTfulAPIAzFunction
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(format:"n");
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string Username { get; set; } 
        public string Password { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
    }

    public class UserCreateModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
    }

    public class UserUpdateModel
    {
        public string Name { get; set; }
    }

}
