using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return; //si ya tenemos un user en la db no se hace el Seed

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json"); //sino tenemos un user

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true }; //por si alguna propierty del json seed esta en minus

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options); //deserializo los user para tener un objecto c#

            foreach (var user in users) //recorro todos los users para asignarles un password
            {
                using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd)"));
                user.PasswordSalt = hmac.Key;

                context.Users.Add(user); // añado el user al for each

            }

            await context.SaveChangesAsync(); //inserto el user en db
        }
    }
}