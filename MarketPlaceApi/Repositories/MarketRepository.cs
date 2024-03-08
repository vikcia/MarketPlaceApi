using Dapper;
using MarketPlaceApi.Dtos;
using MarketPlaceApi.Entities;
using MarketPlaceApi.Interfaces;
using MarketPlaceApi.Services;
using System.Data;

namespace MarketPlaceApi.Repositories;

public class MarketRepository : IMarketRepository
{
    private readonly IDbConnection _connection;

    public MarketRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<ItemEntity?> GetItemById(int id)
    {
        string sql = @"SELECT id FROM items 
                        WHERE id=@Id";

        return await _connection.QuerySingleOrDefaultAsync<ItemEntity>(sql, new { id });
    }

    public async Task<ItemEntity?> AddItem(ItemDto item)
    {
        string sql = @"
                INSERT INTO items(
                    name,
                    price)
                VALUES( 
                    @Name,
                    @Price)
                RETURNING *";

        return await _connection.QuerySingleOrDefaultAsync<ItemEntity>(sql, item);
    }

    public async Task<OrderEntity?> AddOrder(OrderDto order)
    {
        string sql = @"
                INSERT INTO orders(
                    item_id,
                    user_id,
                    delivery_status)
                VALUES( 
                    @ItemId,
                    @UserId,
                    @DeliveryStatus)
                RETURNING *";

        var parameters = new { ItemId = order.ItemId, UserId = order.UserId, DeliveryStatus = Status.InProgress.ToString().ToLower() };

        return await _connection.QuerySingleOrDefaultAsync<OrderEntity>(sql, parameters);
    }

    public async Task<List<UserOrderEntity>> GetUserOrders(int id)
    {
        string sql = @"SELECT 
                        item_id, 
                        user_id, 
                        delivery_status, 
                        name,
                        price
                    FROM orders
                    INNER JOIN items ON orders.item_id = items.id
                    WHERE orders.user_id=@UserId;";

        var parameters = new { UserId = id };

        var result = await _connection.QueryAsync<UserOrderEntity>(sql, parameters);

        return result.ToList();
    }

    public async Task<OrderEntity?> UpdateOrderAsCompleted(int id)
    {
        string sql = @"UPDATE orders 
                        SET delivery_status=@DeliveryStatus
                        WHERE id=@Id 
                        RETURNING *";

        var parameters = new { Id = id, DeliveryStatus = Status.Completed.ToString().ToLower() };

        return await _connection.QuerySingleOrDefaultAsync<OrderEntity>(sql, parameters);
    }

    public async Task<DateTime> DeleteUnpaidOrdersAfterTimeLimit(DateTime time)
    {
        string sql = @"DELETE FROM orders 
                        WHERE created_at<@CreatedAt
                        AND delivery_status = inprogress";

        return await _connection.QuerySingleOrDefaultAsync<DateTime>(sql, new { CreatedAt = time });
    }
}
