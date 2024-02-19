namespace MarketPlaceApi.Dtos;

public class OrderDto
{
    public int ItemId { get; set; }
    public int UserId { get; set; }
    public string DeliveryStatus { get; set; } = string.Empty;
}
