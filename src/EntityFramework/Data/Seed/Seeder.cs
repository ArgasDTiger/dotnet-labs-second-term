using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace EntityFramework.Data.Seed;

public sealed class Seeder
{
    private readonly MoviesRentContext _context;
    private const string ClientsJsonLocation = "../EntityFramework/Data/Seed/clients.json";
    private const string MoviesJsonLocation = "../EntityFramework/Data/Seed/movies.json";

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public Seeder(MoviesRentContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        if (!ShouldSeed())
        {
            return;
        }

        await SeedMoviesAndClients();
        await _context.SaveChangesAsync();

        await SeedClientMovies();
        await _context.SaveChangesAsync();
    }

    private async Task SeedMoviesAndClients()
    {
        var movies = await GetAndUnwrapFromJson<Movie>(MoviesJsonLocation);
        var clients = await GetAndUnwrapFromJson<Client>(ClientsJsonLocation);

        if (movies is null && clients is null)
        {
            throw new Exception("Failed to seed data (no json files found).");
        }

        if (movies is not null)
        {
            _context.Set<Movie>().AddRange(movies);
        }

        if (clients is not null)
        {
            _context.Set<Client>().AddRange(clients);
        }
    }

    private static async Task<List<TEntity>?> GetAndUnwrapFromJson<TEntity>(string location)
    {
        var text = await File.ReadAllTextAsync(location);
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, List<TEntity>>>(text, JsonSerializerOptions);
        return dictionary?.Values.FirstOrDefault();
    }

    private async Task SeedClientMovies()
    {
        Random random = new();
        var clientsIds = await _context.Set<Client>().Select(c => c.Id).ToArrayAsync();
        var movies = await _context.Set<Movie>().ToArrayAsync();

        foreach (var movie in movies)
        {
            var clientMoviesForThisMovie = new List<ClientMovie>();

            double probability = random.NextDouble();

            int numberOfClients = probability switch
            {
                < 0.1 => 4,
                < 0.4 => 3,
                < 0.9 => 2,
                _ => 1
            };

            for (int i = 0; i < numberOfClients; i++)
            {
                var startDate = DateTime.UtcNow
                    .AddDays(-random.Next(1, 600))
                    .AddHours(random.Next(0, 24))
                    .AddMinutes(random.Next(0, 60))
                    .AddSeconds(random.Next(0, 60));

                var newClientMovie = new ClientMovie
                {
                    ClientId = clientsIds[random.Next(clientsIds.Length)],
                    StartDate = startDate,
                    ExpectedReturnDate = startDate.AddDays(random.Next(1, 30))
                };

                clientMoviesForThisMovie.Add(newClientMovie);
            }

            movie.ClientMovies = clientMoviesForThisMovie;
        }
    }

    private bool ShouldSeed()
    {
        return !(_context.Set<Movie>().Any() && !_context.Set<Client>().Any());
    }
}