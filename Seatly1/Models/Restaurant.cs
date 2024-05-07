using System;
using System.Collections.Generic;

namespace Seatly1.Models;

public partial class Restaurant
{
    public int RestaurantId { get; set; }

    public string? RestaurantAccount { get; set; }

    public string? LoginPassword { get; set; }

    public string? RestaurantName { get; set; }

    public string? RestaurantCategory { get; set; }

    public string? RestaurantPhoto { get; set; }

    public string? MenuPhoto { get; set; }

    public string? RestaurantInfo { get; set; }

    public string? Address { get; set; }

    public bool? ReservationAvailable { get; set; }

    public bool? WaitAvailable { get; set; }

    public string? ReservationUrl { get; set; }

    public string? DepartmentStoreName { get; set; }

    public string? Hashtag { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool? Validation { get; set; }
}
