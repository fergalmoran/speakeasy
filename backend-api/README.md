# SpeakEasy Backend API

ASP.NET Core backend providing secure messaging API with end-to-end encryption support.

## Features

- User authentication with JWT
- Message encryption support
- Key exchange management
- PostgreSQL or SQLite database
- RESTful API design

## Setup

1. Install dependencies:
```bash
dotnet restore
```

2. Configure database in `appsettings.json`

3. Run migrations:
```bash
dotnet ef database update
```

4. Run the API:
```bash
dotnet run
```

## API Documentation

Once running, visit `http://localhost:5000/swagger` for API documentation.

## Database Providers

Switch between SQLite and PostgreSQL by changing `DatabaseProvider` in `appsettings.json`:

```json
"DatabaseProvider": "Sqlite"  // or "PostgreSQL"
```

## Environment Variables

- `ASPNETCORE_ENVIRONMENT` - Set to `Development` or `Production`
- Connection strings and JWT key should be configured in `appsettings.json`

## Project Structure

```
backend-api/
├── Controllers/       # API controllers
├── Data/             # Database context
├── DTOs/             # Data transfer objects
├── Models/           # Domain models
├── Services/         # Business logic
└── Program.cs        # Application entry point
```
