# Simple Auction Lot Manager
Built with ASP.NET Core Blazor Server, C# and in-memory storage

# Design?
Used in-memory storage so the project is quick to run, easy to review, and does not require a datbase for quick accessment of 
coding style ( normally I would use Entity Framework and/or Web API) 

# Features implemented
- List all auction lots
- Add a new lot
- Edit a lot
- Delete a lot
- Validation
- Search bar
- Active-only filter
- Pagination
- Live countdown timer for lot close time
- Sorting on table columns

# Tech stack
- .NET 8
- ASP.NET Core Blazor Server
- C#
- In-memory singleton service
- CSS styling with Bootstrap compatible layout

# Project structure
- Models – domain and form models
- Services – in-memory CRUD logic
- Pages – Blazor pages for list/create/edit
- Tests – sample unit tests for the service layer etc.

# Setup instructions
Uses .NET 8 SDK. ( should be ready to go ) or Restore NUGET packages
-- bash dotnet restore dotnet run
Hit the Play button

# Architecture decisions
- Blazor Server was chosen to keep the stack fully in C# and for speed of producing it
- Service abstraction (IAuctionLotService) keeps business logic separate from UI.
- Used in-memory service 


# Misc
- Countdown timer: implemented with a reusable Blazor component that updates every second.
- Search/filter: supports free-text search and active-only filtering.
- Pagination: implemented on the main lot listing.


# Improvments
- Use Entity Framework
- Add richer auction workflows such as bid history 
- Add authentication and role-based permissions
- Multi language for on screen text
- Connect to Database with Stored Procedures 
- WCAG 2.2 Level AAA compliance




