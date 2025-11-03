using System.ComponentModel.DataAnnotations;

namespace AirBB.Models;

public class Client
{
    [Key]
    public int UserId { get; set; }

    [Required, StringLength(60)]
    public string Name { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DOB { get; set; }
}
