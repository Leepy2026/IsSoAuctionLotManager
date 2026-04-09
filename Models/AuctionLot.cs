using System.ComponentModel.DataAnnotations;

namespace AuctionLotManager.Models;

public class AuctionLot
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 1_000_000, ErrorMessage = "Starting price must be greater than 0.")]
    public decimal StartingPrice { get; set; }

    [Range(0.01, 1_000_000, ErrorMessage = "Reserve price must be greater than 0.")]
    public decimal ReservePrice { get; set; }

    [Required]
    public required string Category { get; set; }

    [Required]
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime EndTime { get; set; } = DateTime.UtcNow.AddDays(7);

    public bool IsActive => DateTime.UtcNow >= StartTime && DateTime.UtcNow < EndTime;

    public string? ImageFileName { get; set; }
    public string? ImageUrl => string.IsNullOrWhiteSpace(ImageFileName)
        ? null
        : $"/uploads/{ImageFileName}";
}
