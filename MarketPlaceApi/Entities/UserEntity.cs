﻿namespace MarketPlaceApi.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string  Password { get; set; }
    public string Role { get; set; }
}
