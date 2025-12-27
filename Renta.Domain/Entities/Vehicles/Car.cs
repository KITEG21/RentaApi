using System;
using System.Collections.Generic;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Entities.Multimedia;
using Renta.Domain.Enums;

namespace Renta.Domain.Entities.Vehicles;

public class Car : Entity
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }
    public int Miles { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MechanicalDetails { get; set; } = string.Empty;
    public string PaymentConditions { get; set; } = string.Empty;
    public string History { get; set; } = string.Empty;
    public SellStatus Status { get; set; } = SellStatus.Available;
    
    // Foreign Keys
    public Guid DealerId { get; set; }
    
    // Navigation properties
    public User Dealer { get; set; } = null!;
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    public ICollection<Video> Videos { get; set; } = new List<Video>();

}