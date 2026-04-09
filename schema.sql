CREATE TABLE AuctionLots
(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(1000) NOT NULL,
    StartingPrice DECIMAL(18,2) NOT NULL,
    ReservePrice DECIMAL(18,2) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    StartTimeUtc DATETIME2 NOT NULL,
    EndTimeUtc DATETIME2 NOT NULL
);

CREATE INDEX IX_AuctionLots_Category ON AuctionLots(Category);
CREATE INDEX IX_AuctionLots_EndTimeUtc ON AuctionLots(EndTimeUtc);
