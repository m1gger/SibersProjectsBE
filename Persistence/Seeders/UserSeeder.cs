using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(ISibersDbContext dbContext,UserManager<User> userManager) 
        {
            if (!dbContext.Users.Any()) 
            {
                var user = new User
                {
                    UserName = "director",
                    Email = "someOdmenEmail@gmail.com"
                };
                await userManager.CreateAsync(user, "strongPassword");
                await userManager.AddToRoleAsync(user, "DIRECTOR");
            }
        }
    }
}
