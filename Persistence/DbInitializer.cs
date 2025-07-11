using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Seeders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class  DbInitializer
    {
        public static async Task InitializeAsync(ISibersDbContext context,UserManager<User> userManager)
        {
            // Применяем миграции
            var dbContext = (SibersDbContext)context;
            await  dbContext.Database.MigrateAsync();

            // Запускаем сидер
           await RoleSeeder.SeedAsync(dbContext);
           await UserSeeder.SeedAsync(dbContext, userManager);


        }
    }
}
