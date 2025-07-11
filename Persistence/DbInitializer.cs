using Application.Interfaces;
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
        public static async Task InitializeAsync(ISibersDbContext context)
        {
            // Применяем миграции
            var dbContext = (SibersDbContext)context;
            await  dbContext.Database.MigrateAsync();

            // Запускаем сидер
           await RoleSeeder.SeedAsync(dbContext);
          

        }
    }
}
