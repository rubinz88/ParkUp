# ParkUp

ParkUp is a parking-space reservation backend. It provides a REST API for managing parking spots, requesters, eligibility rules, and parking reservations.

## Features

- PostgreSQL-backed persistence
- Entity Framework Core migrations
- Seeded buildings, parking spots, requesters, eligibility types, statuses, and reservations
- Reservation overlap validation
- Requester eligibility validation
- Reservation cancellation
- Swagger/OpenAPI documentation
- Layered architecture with controllers, services, repositories, and DTOs

## Technology Stack

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL 17
- Npgsql
- Docker Compose
- xUnit tests

## Project Structure

```text
ParkUp/
├── parkup-backend/
│   ├── api/
│   │   ├── Controllers/
│   │   ├── Data/
│   │   ├── DTOs/
│   │   ├── Models/
│   │   ├── Repositories/
│   │   ├── Services/
│   │   └── Migrations/
│   └── api.Tests/
├── parkup-docker/
│   ├── docker-compose.yml
│   ├── .env.example
│   └── postgresql-db/
├── documentation/
└── README.md
```

## Running with Docker Compose

### Prerequisites

- Docker Desktop
- Docker Compose

### Configuration

From the `parkup-docker` directory, create a local `.env` file from the example:

```powershell
Copy-Item .env.example .env
```

Update the passwords in `.env` before starting the services.

### Start the application

```powershell
cd parkup-docker
docker compose up --build
```

The services will be available at:

- API: <http://localhost:8002>
- Swagger UI: <http://localhost:8002/swagger>
- Adminer: <http://localhost:8000>
- PostgreSQL: `localhost:5432`

The backend waits for PostgreSQL to become healthy, applies pending migrations, and initializes the database with the seed data.

To stop the services:

```powershell
docker compose down
```

To also remove the database volume and start with an empty database:

```powershell
docker compose down -v
```

## API Endpoints

### Requesters

```text
GET    /api/requesters
POST   /api/requesters
PATCH  /api/requesters/{id}
DELETE /api/requesters/{id}
PUT    /api/requesters/{id}/eligibility
```

Example requester:

```json
{
  "name": "John Doe",
  "email": "john.doe@example.com"
}
```

Set the requester's eligibility:

```json
{
  "eligibilityTypeId": 1
}
```

The seeded eligibility types are:

- `1` - Disabled
- `2` - LargeFamily
- `3` - OversizedVehicle

### Parking spots

```text
GET    /api/ParkingSpot
GET    /api/ParkingSpot/{id}
POST   /api/ParkingSpot
PUT    /api/ParkingSpot/{id}
PATCH  /api/ParkingSpot/{id}
DELETE /api/ParkingSpot/{id}
```

### Reservations

```text
POST   /api/reservations
GET    /api/reservations/{id}
DELETE /api/reservations/{id}
GET    /api/parkingspots/{parkingSpotId}/reservations
```

When creating a reservation, the API validates:

- requester and parking spot existence;
- requester eligibility for the selected parking spot;
- valid start and end dates;
- overlapping active reservations.

Cancelled and rejected reservations do not block a new reservation. Adjacent reservations are allowed, for example `10:00-12:00` followed by `12:00-14:00`.

Detailed request and response examples are available in [`documentation/2-API.md`](documentation/2-API.md).

## Running Tests

From the repository root:

```powershell
dotnet test parkup-backend\api.Tests\api.Tests.csproj
```

To build the API separately:

```powershell
dotnet build parkup-backend\api\api.csproj
```

## Database and Seed Data

The database is managed with Entity Framework Core migrations. On startup, the backend:

1. applies pending migrations;
2. synchronizes PostgreSQL identity sequences;
3. loads seed data when the corresponding tables are empty.

Seed files are located in `parkup-backend/api/Data`.

## Documentation

- [API documentation](documentation/2-API.md)
- [Database design](documentation/1-Database-Design.md)

