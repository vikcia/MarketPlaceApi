namespace MarketPlaceApi.Entities;

public class OrderEntity
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int UserId { get; set; }
    public string DeliveryStatus { get; set; } = string.Empty;
}
