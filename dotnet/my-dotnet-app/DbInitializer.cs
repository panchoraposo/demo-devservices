using MyApp.Models;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        if (context.Movies.Any()) return;

        var movies = new List<Movie>
        {
            new() { Title = "Inception", Year = 2010, Genre = "Sci-Fi" },
            new() { Title = "The Matrix", Year = 1999, Genre = "Action" },
            new() { Title = "Interstellar", Year = 2014, Genre = "Sci-Fi" }
        };

        context.Movies.AddRange(movies);
        context.SaveChanges();
    }
}