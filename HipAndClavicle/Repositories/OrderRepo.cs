﻿using Microsoft.EntityFrameworkCore;

namespace HipAndClavicle.Repositories;

public class OrderRepo : IOrderRepo
{
    ApplicationDbContext _context;
    public OrderRepo(ApplicationDbContext context)
    {
        _context = context;
    }
    #region Orders
    public async Task UpdateOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOrderAsync(Order order)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    public async Task<Order> GetOrderForPaymentAsync(int orderId)
    {
        return await _context.Orders
            .Include(order => order.Items)
            .ThenInclude(items => items.Item)
            .ThenInclude(product => product.ProductImage)
            .Include(order => order.Items)
            .ThenInclude(items => items.SetSize)
            .FirstAsync(o => o.OrderId == orderId);
    }
    #endregion

    #region OrderItems
    public async Task CreateOrderItemAsync(OrderItem orderItem)
    {
        await _context.OrderItems.AddAsync(orderItem);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// gets all the <see cref="OrderItem"/>'s in all of the orders stored in the database.
    /// </summary>
    /// <returns></returns>
    public async Task<List<OrderItem>> GetOrderItemsAsync() =>
        await _context.OrderItems
            .Include(oi => oi.Item)
            .ThenInclude(i => i.AvailableColors)
            .ToListAsync();

    public async Task UpdateOrderItemAsync(OrderItem orderItem)
    {
        _context.OrderItems.Update(orderItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOrderItemAsync(OrderItem orderItem)
    {
        _context.OrderItems.Remove(orderItem);
        await _context.SaveChangesAsync();
    }
    #endregion

}
