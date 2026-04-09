using AuctionLotManager.Models;
using AuctionLotManager.Services;
using Xunit;

namespace AuctionLotManager.Tests;

public class AuctionLotTests
{
    [Fact]
    public void IsActive_ReturnsTrue_WhenWithinTime()
    {
        var lot = new AuctionLot
        {
            Title = "Test",
            Description = "Test",
            StartingPrice = 10,
            ReservePrice = 20,
            Category = "Toys",
            StartTime = DateTime.UtcNow.AddHours(-1),
            EndTime = DateTime.UtcNow.AddHours(1)
        };

        Assert.True(lot.IsActive);
    }

    [Fact]
    public void ImageUrl_ReturnsNull_WhenImageFileNameIsEmpty()
    {
        var lot = new AuctionLot
        {
            Title = "Test",
            Description = "Desc",
            StartingPrice = 10,
            ReservePrice = 20,
            Category = "Art"
        };

        Assert.Null(lot.ImageUrl);
    }

    [Fact]
    public void ImageUrl_ReturnsUploadsPath_WhenImageFileNameExists()
    {
        var lot = new AuctionLot
        {
            Title = "Test",
            Description = "Desc",
            StartingPrice = 10,
            ReservePrice = 20,
            Category = "Art",
            ImageFileName = "photo.png"
        };

        Assert.Equal("/uploads/photo.png", lot.ImageUrl);
    }

    [Fact]
    public async Task DeleteAsync_RemovesLot()
    {
        var service = new InMemoryAuctionLotService();

        var lot = new AuctionLot
        {
            Id = Guid.NewGuid(),
            Title = "Delete Me",
            Description = "Desc",
            StartingPrice = 10,
            ReservePrice = 20,
            Category = "Art",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddDays(3)
        };

        await service.CreateAsync(lot);
        await service.DeleteAsync(lot.Id);

        var result = await service.GetByIdAsync(lot.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesTitle()
    {
        var service = new InMemoryAuctionLotService();

        var lot = new AuctionLot
        {
            Id = Guid.NewGuid(),
            Title = "Old Title",
            Description = "Desc",
            StartingPrice = 10,
            ReservePrice = 20,
            Category = "Art",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddDays(2)
        };

        await service.CreateAsync(lot);

        lot.Title = "New Title";
        await service.UpdateAsync(lot);

        var updated = await service.GetByIdAsync(lot.Id);

        Assert.NotNull(updated);
        Assert.Equal("New Title", updated!.Title);
    }

    [Fact]
    public void ToAuctionLot_MapsFieldsCorrectly()
    {
        var model = new LotUpsertModel
        {
            Id = Guid.NewGuid(),
            Title = "  Test Title  ",
            Description = "  Test Description  ",
            StartingPrice = 50,
            ReservePrice = 100,
            Category = "  Toys  ",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddDays(7),
            ImageFileName = "item.png"
        };

        var lot = model.ToAuctionLot();

        Assert.Equal("Test Title", lot.Title);
        Assert.Equal("Test Description", lot.Description);
        Assert.Equal("Toys", lot.Category);
        Assert.Equal("item.png", lot.ImageFileName);
    }

    [Fact]
    public void FromAuctionLot_MapsFieldsCorrectly()
    {
        var lot = new AuctionLot
        {
            Id = Guid.NewGuid(),
            Title = "Camera",
            Description = "Vintage camera",
            StartingPrice = 75,
            ReservePrice = 120,
            Category = "Electronics",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddDays(5),
            ImageFileName = "camera.jpg"
        };

        var model = LotUpsertModel.FromAuctionLot(lot);

        Assert.Equal(lot.Id, model.Id);
        Assert.Equal("Camera", model.Title);
        Assert.Equal("Vintage camera", model.Description);
        Assert.Equal("Electronics", model.Category);
        Assert.Equal("camera.jpg", model.ImageFileName);
    }


}