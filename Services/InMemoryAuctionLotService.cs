using AuctionLotManager.Models;


namespace AuctionLotManager.Services;

public class InMemoryAuctionLotService : IAuctionLotService
{
    private readonly List<AuctionLot> _lots = new();
    private readonly object _sync = new();
    
    public InMemoryAuctionLotService()
    {
        Seed();
    }

    /// <summary>
    /// GetAllAsync - Get all the data
    /// </summary>
    /// <param name="search"></param>
    /// <param name="activeOnly"></param>
    /// <returns></returns>
    public Task<IReadOnlyList<AuctionLot>> GetAllAsync(string? search = null, bool activeOnly = false)
    {
        IEnumerable<AuctionLot> query;

        lock (_sync)
        {
            query = _lots
                .Select(Clone)
                .OrderByDescending(x => x.StartTime)
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(x =>
                x.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                x.Description.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                x.Category.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        if (activeOnly)
        {
            query = query.Where(x => x.IsActive);
        }

        return Task.FromResult<IReadOnlyList<AuctionLot>>(query.ToList());
    }

    /// <summary>
    /// GetByIdAsync - needs Id to get data item by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<AuctionLot?> GetByIdAsync(Guid id)
    {
        lock (_sync)
        {
            var lot = _lots.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(lot is null ? null : Clone(lot));
        }
    }

    /// <summary>
    /// CreateAsync - store out new data item
    /// </summary>
    /// <param name="lot"></param>
    /// <returns></returns>
    public Task CreateAsync(AuctionLot lot)
    {
        Validate(lot);

        lock (_sync)
        {
            _lots.Add(Clone(lot));
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// UpdateAsync - Update exisiting data item
    /// </summary>
    /// <param name="lot"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task UpdateAsync(AuctionLot lot)
    {
        Validate(lot);

        lock (_sync)
        {
            var existing = _lots.FirstOrDefault(x => x.Id == lot.Id)
                ?? throw new InvalidOperationException("Lot not found.");

            existing.Title = lot.Title;
            existing.Description = lot.Description;
            existing.StartingPrice = lot.StartingPrice;
            existing.ReservePrice = lot.ReservePrice;
            existing.Category = lot.Category;
            existing.StartTime = lot.StartTime;
            existing.EndTime = lot.EndTime;
            existing.ImageFileName = lot.ImageFileName;
        }

        return Task.CompletedTask;
    }

   

    /// <summary>
    /// Use async to demostrate a fake db call ( delay of 1 sec ) 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(Guid id)
    {
        
        lock (_sync)
        {
            var lot = _lots.FirstOrDefault(x => x.Id == id);
            if (lot is not null)
            {
                _lots.Remove(lot);
            }
        }
    }


    /// <summary>
    /// Validate - Handles simple price and title validation 
    /// </summary>
    /// <param name="lot"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private static void Validate(AuctionLot lot)
    {
        if (string.IsNullOrWhiteSpace(lot.Title))
            throw new InvalidOperationException("Title is required.");

        if (lot.ReservePrice < lot.StartingPrice)
            throw new InvalidOperationException("Reserve price must be greater than or equal to the starting price.");

        if (lot.EndTime <= lot.StartTime)
            throw new InvalidOperationException("End time must be after start time.");
    }

    /// <summary>
    /// Clone - create POC for shuttling data
    /// </summary>
    /// <param name="lot"></param>
    /// <returns></returns>
    private static AuctionLot Clone(AuctionLot lot)
    {
        return new AuctionLot
        {
            Id = lot.Id,
            Title = lot.Title,
            Description = lot.Description,
            StartingPrice = lot.StartingPrice,
            ReservePrice = lot.ReservePrice,
            Category = lot.Category,
            StartTime = lot.StartTime,
            EndTime = lot.EndTime,
            ImageFileName = lot.ImageFileName
        };
    }

    /// <summary>
    /// Fake Data 
    /// </summary>
    private void Seed()
    {
        _lots.AddRange(new[]
        {
                new AuctionLot {Title = "Darth Vader Figure", Description = "N/A", Category = Category.Toys.ToString(), StartingPrice = 25, EndTime = DateTime.Now.AddDays(1) },
                new AuctionLot {Title = "Luke Skywalker Figure", Description = "N/A", Category = "Toys", StartingPrice = 20, ReservePrice= 110, EndTime = DateTime.Now.AddDays(1) },
                new AuctionLot {Title = "Yoda Collectible",  Description = "N/A",Category = "Toys", StartingPrice = 30, ReservePrice= 120, EndTime = DateTime.Now.AddDays(1) },
                new AuctionLot {Title = "Stormtrooper Figure",  Description = "N/A", Category = "Toys", StartingPrice = 18, ReservePrice= 30,EndTime = DateTime.Now.AddDays(1) },
                new AuctionLot {Title = "Boba Fett Figure", Description = "N/A", Category = "Toys", StartingPrice = 35, ReservePrice= 70,EndTime = DateTime.Now.AddDays(1) },
                new AuctionLot {Title = "Princess Leia Figure", Description = "N/A", Category = "Toys", StartingPrice = 22, ReservePrice= 70,EndTime = DateTime.Now.AddDays(1) },
                new AuctionLot {Title = "Han Solo Figure", Description = "N/A", Category = "Toys", StartingPrice = 28, ReservePrice= 120,EndTime = DateTime.Now.AddDays(1) },
                new AuctionLot {Title = "Chewbacca Figure", Description = "N/A", Category = "Toys", StartingPrice = 26, ReservePrice= 80,EndTime = DateTime.Now.AddDays(1) },
                new AuctionLot {Title = "R2-D2 Model", Description = "N/A", Category = "Toys", StartingPrice = 32, ReservePrice= 50,EndTime = DateTime.Now.AddDays(1) },
                new AuctionLot {Title = "C-3PO Model", Description = "1978 1st Edition Star Wars Figure ", Category = "Toys", StartingPrice = 30, ReservePrice= 90,EndTime = DateTime.Now.AddDays(1), ImageFileName="09f02fc6-3a72-416e-985d-894e4433a196.png" },
                new AuctionLot {Title = "1965 Mini Cooper", Description = "1965 Austin Mini Cooper S front-wheel-drive sport edition ", Category = "Classic Cars", StartingPrice = 8000, ReservePrice= 20000,EndTime = DateTime.Now.AddDays(7), ImageFileName ="231d1e7b-f6ef-4c41-af55-7d8a9139385c.jpg" }, 
                new AuctionLot {Title = "Jaguar E-Type Series 1", Description = "N/A", Category = "Classic Cars", StartingPrice = 55000, ReservePrice= 70000,EndTime = DateTime.Now.AddDays(7) },
                new AuctionLot {Title = "Ford Escort Mk1", Description = "N/A", Category = "Classic Cars", StartingPrice = 12000, ReservePrice= 5000,EndTime = DateTime.Now.AddDays(7) },
                new AuctionLot {Title = "Triumph Spitfire 1972", Description = "N/A", Category = "Classic Cars", StartingPrice = 9000, ReservePrice= 15000,EndTime = DateTime.Now.AddDays(7) },
                new AuctionLot {Title = "Morris Minor 1960", Description = "N/A", Category = "Classic Cars", StartingPrice = 7000, ReservePrice= 7000,EndTime = DateTime.Now.AddDays(7) },
                new AuctionLot {Title = "Victorian Tea Set", Description = "N/A", Category = "Antiques", StartingPrice = 150, ReservePrice= 120,EndTime = DateTime.Now.AddDays(-1) },
                new AuctionLot {Title = "Edwardian Writing Desk", Description = "N/A", Category = "Antiques", StartingPrice = 400, ReservePrice= 400,EndTime = DateTime.Now.AddDays(-2) },
                new AuctionLot {Title = "Antique Pocket Watch", Description = "N/A", Category = "Antiques", StartingPrice = 250, ReservePrice= 300,EndTime = DateTime.Now.AddDays(-3) },
                new AuctionLot {Title = "Vintage Brass Telescope", Description = "N/A", Category = "Antiques", StartingPrice = 300, ReservePrice= 300,EndTime = DateTime.Now.AddDays(-1) },
                new AuctionLot {Title = "Georgian Silver Spoon Set", Description = "N/A", Category = "Antiques", StartingPrice = 180, ReservePrice= 200,EndTime = DateTime.Now.AddDays(-2) },
                new AuctionLot {Title = "Commercial Aircraft Wheel",Description = "Refurbished aviation wheel assembly with service history.",Category = "Aviation",StartingPrice = 1500,ReservePrice = 2500,StartTime = DateTime.UtcNow.AddDays(-1),EndTime = DateTime.UtcNow.AddDays(3)},
        });
    }
}
