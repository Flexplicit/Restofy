using System.Linq;
using DAL.APP.EF;
using Domain.OrderModels;
using Domain.OrderModels.DbEnums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject.Helpers
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // find the dbcontext
                var descriptor = services
                    .SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<AppDbContext>)
                    );
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<AppDbContext>(options =>
                {
                    // do we need unique db?
                    options.UseInMemoryDatabase("testDb");
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();

                db.Database.EnsureCreated();

                // data is already seeded
                if (db.ContactTypes.Any()) return;

                // seed data
                db.FoodGroups.Add(new FoodGroup
                {
                    FoodGroupType = FoodGroupType.Burger
                });
                db.PaymentTypes.Add(new PaymentType()
                {
                    Type = EPaymentType.Card
                });
                db.PaymentTypes.Add(new PaymentType()
                {
                    Type = EPaymentType.Cash
                });
                db.SaveChanges();
            });
        }
    }
}