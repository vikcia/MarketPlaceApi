namespace MarketPlaceApi.Entities;

public class UserOrderEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; } = decimal.Zero;
    public string DeliveryStatus { get; set; } = string.Empty;
}
