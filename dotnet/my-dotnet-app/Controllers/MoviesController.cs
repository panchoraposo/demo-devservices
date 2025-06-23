using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;

namespace MyApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly AppDbContext _db;

    public MoviesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> Get() =>
        await _db.Movies.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> Get(int id)
    {
        var movie = await _db.Movies.FindAsync(id);
        return movie is null ? NotFound() : Ok(movie);
    }

    [HttpPost]
    public async Task<ActionResult<Movie>> Post(Movie movie)
    {
        _db.Movies.Add(movie);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Movie updated)
    {
        var movie = await _db.Movies.FindAsync(id);
        if (movie is null) return NotFound();

        movie.Title = updated.Title;
        movie.Genre = updated.Genre;
        movie.Year = updated.Year;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var movie = await _db.Movies.FindAsync(id);
        if (movie is null) return NotFound();

        _db.Movies.Remove(movie);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}