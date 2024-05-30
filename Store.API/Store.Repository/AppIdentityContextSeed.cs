using Microsoft.AspNetCore.Identity;
using Store.Data.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class AppIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Mostafa Mohamed",
                    Email = "Mustafa1582002@gmail.com",
                    UserName = "Mostafa",
                    Address = new Address
                    {
                        FirstName = "Mostafa",
                        LastName = "Mohamed",
                        City = "Helwan",
                        State = "Cairo",
                        Street = "blabla",
                        ZipCode = "12345"
                    }
                };
                await userManager.CreateAsync(user, "Password123!");
            }
        }
    }
}
