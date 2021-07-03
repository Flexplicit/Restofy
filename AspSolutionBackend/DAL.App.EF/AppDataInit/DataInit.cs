using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Identity;
using Domain.OrderModels;
using Domain.OrderModels.DbEnums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.APP.EF.AppDataInit
{
    public static class DataInit
    {
        public static void DropDatabase(AppDbContext context)
        {
            context.Database.EnsureDeleted();
        }

        public static void MigrateDatabase(AppDbContext context)
        {
            context.Database.Migrate();
        }

        private static IEnumerable<TEnum> GetUniqueEnumList<TEnum>(IEnumerable<TEnum> currentEnums)
        {
            return Enum
                .GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Where(@enum => !currentEnums.Contains<TEnum>(@enum));
        }

        private static async Task SeedImmutableData(AppDbContext context)
        {
            // Immutable Data: Food Groups
            var foodGroups = context.FoodGroups.ToList();
            var groupsForSeeding = foodGroups.Select(x => x.FoodGroupType);
            var groupTypes = GetUniqueEnumList(groupsForSeeding).Select(x => new FoodGroup {FoodGroupType = x})
                .ToList();
            await context.AddRangeAsync(groupTypes);

            // Immutable Data: Supported payment types
            var paymentGroups = context.PaymentTypes.ToList();
            var paymentGroupsForSeeding = paymentGroups.Select(x => x.Type);
            var paymentGroupTypes = GetUniqueEnumList(paymentGroupsForSeeding)
                .Select(x => new PaymentType {Type = x})
                .ToList();
            await context.AddRangeAsync(paymentGroupTypes);

            // Immutable Data: Supported contact types
            var contactTypes = context.ContactTypes.ToList();
            var contactGroupsForSeeding = contactTypes.Select(x => x.TypeName);
            var contactGroupTypes = GetUniqueEnumList(contactGroupsForSeeding)
                .Select(x => new ContactType {TypeName = x}).ToList();
            await context.AddRangeAsync(contactGroupTypes);

            var freeSubscription = new Subscription()
            {
                Cost = 0,
                Description = "Poor man's subscription to try out the application before getting buying it",
                SubscriptionType = ESubscriptionType.Free,
                ValidDayCount = 7
            };
            var premiumSubscription = new Subscription()
            {
                Cost = 10,
                Description = "30 day subscription to continue using the application",
                SubscriptionType = ESubscriptionType.Premium,
                ValidDayCount = 30
            };
            var proSubscription = new Subscription()
            {
                Cost = 15,
                Description = "45 day subscription and get a premium tag on your restaurant",
                SubscriptionType = ESubscriptionType.Pro,
                ValidDayCount = 45
            };
            await context.AddRangeAsync(new List<Subscription>()
                {freeSubscription, premiumSubscription, proSubscription});
            await context.SaveChangesAsync();
        }


        public static async Task SeedAppTestData(AppDbContext context)
        {
            await SeedImmutableData(context);

            var freeSubscription = context.Subscriptions.First(x => x.SubscriptionType == ESubscriptionType.Free);
            var burgerGroupType = context.FoodGroups.First(x => x.FoodGroupType == FoodGroupType.Burger);

            var restaurantSubscription = new RestaurantSubscription()
            {
                ActiveSince = DateTime.Now,
                ActiveUntill = DateTime.Now.AddDays(freeSubscription.ValidDayCount),
                Subscription = freeSubscription,
                PaypalId = "-1"
            };
            var user = context.Users.FirstOrDefaultAsync(entity => entity.UserName.Equals("renesissask@gmail.com"))
                .Result;
            user.Restaurants = new List<Restaurant>();
            var restaurant = new Restaurant
            {
                AppUser = user, NameLang = "McDonalds", DescriptionLang = "Fast food", RestaurantAddress = "Viimsi tee 5",
                Picture = "https://i.gyazo.com/75d282232432e445aa939cc28766a6ad.jpg",
                RestaurantSubscriptions = new List<RestaurantSubscription>
                {
                    restaurantSubscription
                }
            };

            var cost1 = new Cost() {CostWithoutVat = (decimal) 10.2, CostWithVat = 12, Vat = (decimal) 0.2};
            var cost2 = new Cost() {CostWithoutVat = 1, CostWithVat = (decimal) 1.5, Vat = (decimal) 0.2};
            var food1 = new Food()
            {
                Description = "Juicy burger",
                FoodNameLang = "Bic Mac",
                Picture = "https://i.gyazo.com/b71db332866f7c1bcee01d356bf339a8.png",
                Restaurant = restaurant,
                FoodGroup = burgerGroupType,
                Cost = cost1
            };
            var food2 = new Food()
            {
                Description = "Cheesy burger",
                FoodNameLang = "Cheese burger",
                Picture = "https://i.gyazo.com/8d6af3463f29dfde358f36b2a2116475.jpg",
                Restaurant = restaurant,
                FoodGroup = burgerGroupType,
                Cost = cost2
            };
            restaurant.RestaurantFood = new List<Food> {food1, food2};
            await context.AddAsync(restaurant);
            await context.AddAsync(food1);
            await context.AddAsync(food2);
            await context.AddAsync(cost1);
            await context.AddAsync(cost2);
            context.Update(user);
            await context.SaveChangesAsync();
        }

        public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
            IConfiguration cfg)
        {
            var role = new AppRole {Name = cfg["Users:AdminUser:Role"]};
            var res = roleManager.CreateAsync(role).Result;
            IdentityErrorLogger(res);
            var user = new AppUser
            {
                Email = cfg["Users:AdminUser:Email"],
                FirstName = cfg["Users:AdminUser:FirstName"],
                LastName = cfg["Users:AdminUser:LastName"],
                UserName = cfg["Users:AdminUser:Email"]
            };

            res = userManager.CreateAsync(user, cfg["Users:AdminUser:Pw"]).Result;
            IdentityErrorLogger(res);

            res = userManager.AddToRoleAsync(user, role.Name).Result;
            IdentityErrorLogger(res);

            user = new AppUser
            {
                Email = cfg["Users:NormalUser:Email"],
                FirstName = cfg["Users:NormalUser:FirstName"],
                LastName = cfg["Users:NormalUser:LastName"],
                UserName = cfg["Users:NormalUser:Email"]
            };

            res = userManager.CreateAsync(user, cfg["Users:NormalUser:Pw"]).Result;
            IdentityErrorLogger(res);
        }

        public static bool IdentityErrorLogger(IdentityResult identityResult)
        {
            if (identityResult.Succeeded) return false;
            foreach (var error in identityResult.Errors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nIdentity Error: {error.Code} - {error.Description}");
            }

            return true;
        }
    }
}