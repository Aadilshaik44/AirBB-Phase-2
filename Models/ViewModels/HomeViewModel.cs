using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirBB.Models;

public class HomeViewModel
{
    // filter inputs (bound from form / retained in session)
    public string ActiveLocationId { get; set; } = "All";   // "All" or int as string
    public string ActiveGuests { get; set; } = "1";
    public string ActiveStart { get; set; } = "";           // MM/DD/YYYY or ""
    public string ActiveEnd { get; set; } = "";

    // lists for UI
    public List<SelectListItem> Locations { get; set; } = new();
    public List<Residence> Residences { get; set; } = new();

    // reservations for badge / page
    public List<Reservation> Reservations { get; set; } = new();
    public int ReservationsCount => Reservations?.Count ?? 0;
}
