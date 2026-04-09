using System.ComponentModel.DataAnnotations;

namespace AuctionLotManager.Models;

public class LotUpsertModel
{
    public Guid? Id { get; set; }

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
    public string Category { get; set; } = "General";

    [Required]
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime EndTime { get; set; } = DateTime.UtcNow.AddDays(7);

    public string? ImageFileName { get; set; }

    public AuctionLot ToAuctionLot()
    {
        return new AuctionLot
        {
            Id = Id ?? Guid.NewGuid(),
            Title = Title.Trim(),
            Description = Description.Trim(),
            StartingPrice = StartingPrice,
            ReservePrice = ReservePrice,
            Category = Category.Trim(),
            StartTime = DateTime.SpecifyKind(StartTime, DateTimeKind.Utc),
            EndTime = DateTime.SpecifyKind(EndTime, DateTimeKind.Utc),
            ImageFileName = ImageFileName
        };
    }

    public static LotUpsertModel FromAuctionLot(AuctionLot lot)
    {
        return new LotUpsertModel
        {
            Id = lot.Id,
            Title = lot.Title,
            Description = lot.Description,
            StartingPrice = lot.StartingPrice,
            ReservePrice = lot.ReservePrice,
            Category = lot.Category,
            StartTime = DateTime.SpecifyKind(lot.StartTime, DateTimeKind.Utc),
            EndTime = DateTime.SpecifyKind(lot.EndTime, DateTimeKind.Utc),
            ImageFileName = lot.ImageFileName
        };
    }
}