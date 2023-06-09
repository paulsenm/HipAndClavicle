﻿using Microsoft.EntityFrameworkCore;
using HipAndClavicle.Models;
using HipAndClavicle.Models.JunctionTables;

namespace HipAndClavicle.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public ApplicationDbContext()
    {

    }
    public DbSet<Image> Images { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    public DbSet<Color> NamedColors { get; set; }
    public DbSet<UserSettings> Settings { get; set; }
    public DbSet<Listing> Listings { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<UserMessage> Messages { get; set; }
    public DbSet<SetSize> SetSizes { get; set; }
    public DbSet<ColorFamily> ColorFamilies { get; set; }

    public DbSet<ShippingAddress> Addresses { get; set; }
    public DbSet<UserMessage> UserMessages { get; set; }

    public DbSet<Ship> Shipping { get; set; } = default!;

    public DbSet<ListingColorJT> ListingColorsJT { get; set;}
}
