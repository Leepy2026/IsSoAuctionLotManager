using AuctionLotManager.Models;
using AuctionLotManager.Services;

namespace AuctionLotManager.Tests;

public class InMemoryAuctionLotServiceTests
{
    [Fact]
    public async Task SeededData_ShouldBeAvailable()
    {
        var service = new InMemoryAuctionLotService();

        var lots = await service.GetAllAsync();

        Assert.NotEmpty(lots);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddLot()
    {
        var service = new InMemoryAuctionLotService();
        var lot = new AuctionLot
        {
            Title = "Test Lot",
            Description = "Test Description",
            Category = "General",
            StartingPrice = 100,
            ReservePrice = 150,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddDays(1)
        };

        await service.CreateAsync(lot);

        var lots = await service.GetAllAsync("Test Lot");
        Assert.Contains(lots, x => x.Title == "Test Lot");
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyLot()
    {
        var service = new InMemoryAuctionLotService();
        var lot = (await service.GetAllAsync()).First();

        lot.Title = "Updated Title";
        await service.UpdateAsync(lot);

        var updated = await service.GetByIdAsync(lot.Id);
        Assert.Equal("Updated Title", updated?.Title);
    }
}
