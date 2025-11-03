using System.ComponentModel.DataAnnotations;

namespace AirBB.Models;

public class Location
{
    public int LocationId { get; set; }

    [Required, StringLength(60)]
    public string Name { get; set; } = string.Empty;

    // nav
    public ICollection<Residence> Residences { get; set; } = new List<Residence>();
}
