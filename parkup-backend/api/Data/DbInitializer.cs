using System.Text.Json;
using api.Context;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public static class DbInitializer
    {
        private static bool changesMade = false;

        private static void SetChangesMadeTrue()
        {
            if (!changesMade)
                changesMade = true;
        }

        public static async Task Seed(ParkUpDbContext context, ILogger logger)
        {
            try
            {
                await context.Database.MigrateAsync();

                // Saving buildings
                if (!context.Buildings.Any())
                {
                    var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "buildings.json");
                    var json = await File.ReadAllTextAsync(jsonPath);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var buildings = JsonSerializer.Deserialize<List<Building>>(json, options);
                    
                    if (buildings != null && buildings.Count > 0)
                    {
                        await context.Buildings.AddRangeAsync(buildings);
                        logger.LogInformation("Buildings data saved, total number of buildings in database: " + buildings.Count);
                        SetChangesMadeTrue();
                    }
                } else
                {
                    logger.LogWarning("Could not save buildings in the database");
                }

                // Saving requesters
                if (!context.Requesters.Any())
                {
                    var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "requesters.json");
                    var json = await File.ReadAllTextAsync(jsonPath);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var requesters = JsonSerializer.Deserialize<List<Requester>>(json, options);

                    if (requesters != null && requesters.Count > 0)
                    {
                        await context.Requesters.AddRangeAsync(requesters);
                        logger.LogInformation("Requesters data saved, total number of requesters in database: " + requesters.Count);
                        SetChangesMadeTrue();
                    }
                } else
                {
                    logger.LogWarning("Could not save requesters in the database");
                }

                // Saving parking spots
                if (!context.ParkingSpots.Any())
                {
                    var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "parkingspots.json");
                    var json = await File.ReadAllTextAsync(jsonPath);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var parkingSpots = JsonSerializer.Deserialize<List<ParkingSpot>>(json, options);

                    if (parkingSpots != null && parkingSpots.Count > 0)
                    {
                        await context.ParkingSpots.AddRangeAsync(parkingSpots);
                        logger.LogInformation("Parking spots data saved, total number of parking spots: " + parkingSpots.Count);
                        SetChangesMadeTrue();
                    } else
                    {
                        logger.LogWarning("Could not save parking spots in the database");
                    }
                }

                // Saving requester eligibilities
                if (!context.RequesterEligibilities.Any())
                {
                    var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "requestereligibilities.json");
                    var json = await File.ReadAllTextAsync(jsonPath);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var requesterEligibilities = JsonSerializer.Deserialize<List<RequesterEligibility>>(json, options);

                    if (requesterEligibilities != null && requesterEligibilities.Count > 0)
                    {
                        await context.RequesterEligibilities.AddRangeAsync(requesterEligibilities);
                        logger.LogInformation("Requester eligibilities data saved, total number of req. elig.: " + requesterEligibilities.Count);
                    } else
                    {
                        logger.LogWarning("Could not save requester eligibilites in the database");
                    }
                    changesMade = true;
                }

                // Saving parking reservations
                if (!context.ParkingReservations.Any())
                {
                    var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "parkingreservations.json");
                    var json = await File.ReadAllTextAsync(jsonPath);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var parkingReservations = JsonSerializer.Deserialize<List<ParkingReservation>>(json);

                    if (parkingReservations != null && parkingReservations.Count > 0)
                    {
                        await context.ParkingReservations.AddRangeAsync(parkingReservations);
                        logger.LogInformation("Parking reservations data saved, total number of parking reservations: " + parkingReservations.Count);
                        SetChangesMadeTrue();
                    } else
                    {
                        logger.LogWarning("Could not save parking reservations in the database");
                    }
                }
            } catch (Exception ex)
            {
                logger.LogError("Error while seeding database " + ex);
            }
        }
    }
}
