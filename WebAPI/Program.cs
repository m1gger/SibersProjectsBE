using WebAPI;
using Persistence;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;

public class Program
{
    public async static Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();


        using (var scope = host.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            try
            {
                var context = serviceProvider.GetRequiredService<SibersDbContext>();
                var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                await DbInitializer.InitializeAsync(context,userManager);
             
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Произошла ошибка при инициализации базы данных: {exception.Message}");
            }
        }

        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>()
                .UseWebRoot("wwwroot");
            });
}