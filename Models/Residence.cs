using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirBB.Models;

public class Residence
{
    public int ResidenceId { get; set; }

    [Required, StringLength(90)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string ResidencePicture { get; set; } = string.Empty;

    // FK
    public int LocationId { get; set; }
    public Location? Location { get; set; } 

    [Range(1, 16)]
    public int GuestNumber { get; set; }

    [Range(0, 20)]
    public int BedroomNumber { get; set; }

    [Range(0, 20)]
    public int BathroomNumber { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Range(1, 50000)]
    public decimal PricePerNight { get; set; }
}
