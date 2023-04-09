﻿namespace HipAndClavicle.ViewModels;

public class MerchantVM
{
    public AppUser Admin { get; set; } = default!;
    public List<Order> CurrentOrders { get; set; } = new();
    public List<Order> ShippedOrders { get; set; } = new();

    public List<Product> Products { get; set; } = new();

    public MerchantVM()
    {
        
    }

    public MerchantVM(AppUser merchant)
    {
        Admin = merchant;
    }
}