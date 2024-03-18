using System.ComponentModel.DataAnnotations;

namespace MarketPlaceApi.Entities;

public class User
{

    [Required]
    public string Name { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Role { get; set; }
}
