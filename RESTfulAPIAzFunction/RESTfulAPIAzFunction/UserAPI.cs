using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace RESTfulAPIAzFunction
{
    public static class UserAPI
    {
        public static readonly List<User> Items = new List<User>();

        [FunctionName("CreateUser")]
        public static async Task<IActionResult> CreateUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Creating a new user.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UserCreateModel>(requestBody);

            var user = new User()
            {
                Username = data.Username,
                Password = data.Password,
                Mail = data.Mail,
                Name = data.Name
            };

            Items.Add(user);

            return new OkObjectResult(user);
        }

        [FunctionName("GetAllUsers")]
        public static IActionResult GetAllUsers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting all users.");

            return new OkObjectResult(Items);
        }

        [FunctionName("GetUserById")]
        public static IActionResult GetUserById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Getting user using the id value.");

            var user = Items.FirstOrDefault(u => u.Id == id);

            if (user == null)
            { return new NotFoundResult(); }

            return new OkObjectResult(user);
        }

        [FunctionName("GetUserByUsername")]
        public static IActionResult GetUserByUsername(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{username}")] HttpRequest req,
            ILogger log, string username)
        {
            log.LogInformation("Getting user using the username value.");

            var user = Items.FirstOrDefault(u => u.Username == username);

            if (user == null)
            { return new NotFoundResult(); }

            return new OkObjectResult(user);
        }


        [FunctionName("UpdateUser")]
        public static async Task<IActionResult> UpdateUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Update the name of the user.");

            var user = Items.FirstOrDefault(u => u.Id == id);

            if (user == null)
            { return new NotFoundResult(); }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<UserUpdateModel>(requestBody);

            user.Name = updated.Name;

            if (!string.IsNullOrEmpty(updated.Name))
            { user.Name = updated.Name; }

            return new OkObjectResult(user);
        }

        [FunctionName("DeleteUser")]
        public static IActionResult DeleteUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "user/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Deleting the user.");

            var user = Items.FirstOrDefault(u => u.Id == id);
            if (user == null)
            { return new NotFoundResult(); }

            Items.Remove(user);
            return new OkResult();
        }
    }
}
