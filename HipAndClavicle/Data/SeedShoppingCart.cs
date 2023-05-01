﻿
using HipAndClavicle.Models;

namespace HipAndClavicle.Data
{
    public class SeedShoppingCart
    {
        public static async Task Seed(ApplicationDbContext context, IServiceProvider services)
        {
            if (await context.ShoppingCartItems.AnyAsync())
            {
                return;
            }

            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var devin = await userManager.FindByNameAsync("dfreem987");
            var michael = await userManager.FindByNameAsync("michael123");
            var steven = await userManager.FindByNameAsync("steven123");
            var nehemiah = await userManager.FindByNameAsync("nehemiah123");

            ShoppingCart[] carts = {
            new ShoppingCart { Owner = devin },
            new ShoppingCart { Owner = michael },
            new ShoppingCart { Owner = steven },
            new ShoppingCart { Owner = nehemiah }
            };

            var listing1 = await context.Listings.FirstOrDefaultAsync(p => p.ListingId == 1);
            var listing2 = await context.Listings.FirstOrDefaultAsync(p => p.ListingId == 3);

            for (int i = 0; i < carts.Length; i++)
            {
                var cart = carts[i];

                var shoppingCartItem1 = new ShoppingCartItem
                {
                    ListingItem = listing1,
                    Quantity = 2
                };

                var shoppingCartItem2 = new ShoppingCartItem
                {
                    ListingItem = listing2,
                    Quantity = 1
                };

                await context.ShoppingCarts.AddAsync(cart);
                await context.ShoppingCartItems.AddRangeAsync(shoppingCartItem1, shoppingCartItem2);
            }

            await context.SaveChangesAsync();
        }
    } 
}