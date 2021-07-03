using System;
using Domain.Base;
using Domain.Identity;
using Domain.OrderModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DAL.APP.EF
{
    public partial class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Bill> Bills { get; set; } = null!;
        public virtual DbSet<BillLine> BillLines { get; set; } = null!;
        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<ContactType> ContactTypes { get; set; } = null!;
        public virtual DbSet<Cost> Costs { get; set; } = null!;
        public virtual DbSet<CreditCard> CreditCards { get; set; } = null!;
        public virtual DbSet<CreditCardInfo> CreditCardInfos { get; set; } = null!;
        public virtual DbSet<Food> Foods { get; set; } = null!;
        public virtual DbSet<FoodGroup> FoodGroups { get; set; } = null!;
        public virtual DbSet<FoodInOrder> FoodInOrders { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<PaymentType> PaymentTypes { get; set; } = null!;
        public virtual DbSet<Restaurant> Restaurants { get; set; } = null!;
        public virtual DbSet<RestaurantSubscription> RestaurantSubscriptions { get; set; } = null!;
        public virtual DbSet<Subscription> Subscriptions { get; set; } = null!;
        
        
        public virtual DbSet<LangString> LangStrings { get; set; } = null!;
        public virtual DbSet<Translation> Translations { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //TODO: should be in base dbContext
            modelBuilder.Entity<Translation>().HasKey(key => new {key.Culture, key.LangStringId});

            modelBuilder.Entity<BillLine>(entity =>
            {
                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.BillLines)
                    .HasForeignKey(d => d.BillId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Bill>(entity =>
                entity.HasOne<Order>(b => b.Order!)
                    .WithMany(bills => bills.Bills)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
            );

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasOne(d => d.ContactType)
                    .WithMany(p => p!.Contacts)
                    .HasForeignKey(d => d.ContactTypeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p!.Contacts)
                    .HasForeignKey(d => d.RestaurantId);

                entity.HasOne(d => d.AppUser)
                    .WithMany(p => p!.Contacts)
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CreditCard>(entity =>
            {
                entity.HasOne(d => d.CreditCardInfo)
                    .WithMany(p => p!.CreditCards)
                    .HasForeignKey(d => d.CreditCardInfoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.AppUser)
                    .WithMany(p => p!.CreditCards)
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull); // cascade? 
            });
            modelBuilder.Entity<Food>(entity =>
            {
                entity.HasOne<Cost>(x => x.Cost!)
                    .WithMany(f => f.Foods)
                    .HasForeignKey(x => x.CostId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade); // setnull?


                entity.HasOne<Restaurant>(g => g.Restaurant!)
                    .WithMany(food => food.RestaurantFood)
                    .HasForeignKey(fg => fg.RestaurantId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<FoodInOrder>(entity =>
            {
                entity.HasOne<Food>(x => x.Food!)
                    .WithMany(x => x.FoodInOrders)
                    .HasForeignKey(x => x.FoodId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(d => d.AppUser)
                    .WithMany(p => p!.Orders)
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);


                entity.HasMany<FoodInOrder>(d => d.FoodInOrders)
                    .WithOne(p => p!.Order!)
                    .HasForeignKey(d => d.OrderId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p!.Orders)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.HasOne(d => d.AppUser)
                    .WithMany(p => p!.Restaurants)
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                //MiGHT DELETE
                entity.HasMany(d => d.RestaurantOrder)
                    .WithOne(p => p.Restaurant!)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.RestaurantFood)
                    .WithOne(p => p!.Restaurant!)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(d => d.Contacts)
                    .WithOne(p => p!.Restaurant!)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RestaurantSubscription>(entity =>
            {
                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p!.RestaurantSubscriptions)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Subscription)
                    .WithMany(p => p!.RestaurantSubscriptions)
                    .HasForeignKey(d => d.SubscriptionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}