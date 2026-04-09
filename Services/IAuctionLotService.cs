using AuctionLotManager.Models;

namespace AuctionLotManager.Services;

public interface IAuctionLotService
{
    Task<IReadOnlyList<AuctionLot>> GetAllAsync(string? search = null, bool activeOnly = false);
    Task<AuctionLot?> GetByIdAsync(Guid id);
    Task CreateAsync(AuctionLot lot);
    Task UpdateAsync(AuctionLot lot);
    Task DeleteAsync(Guid id);
}
