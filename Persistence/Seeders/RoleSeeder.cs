using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(ISibersDbContext dbContext) 
        {
            if (!await dbContext.Roles.AnyAsync()) 
            {
                var roles = GetRoles();
                await dbContext.Roles.AddRangeAsync(roles);
                await dbContext.SaveChangesAsync();
            }

        }

        private static Role[] GetRoles() 
        {
            return new Role[]
            {
                new Role { Name = "Director", NormalizedName = "DIRECTOR" },
                new Role { Name = "Employer", NormalizedName = "EMPLOYER" },
                new Role { Name = "ProjectManager", NormalizedName = "PROJECTMANAGER" }
            };
        }
    }
}
