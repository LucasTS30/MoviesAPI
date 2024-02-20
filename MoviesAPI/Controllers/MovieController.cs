using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Data;
using MoviesAPI.Data.Dtos;
using MoviesAPI.Data.DTOs;
using MoviesAPI.Models;

namespace MoviesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private MovieContext _context;
    private IMapper _mapper;

    public MovieController(MovieContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Add a movie in the database
    /// </summary>
    /// <param name="movieDto">Object with the necessary fieds to create a movie</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AddMovie([FromBody] CreateMovieDto movieDto)
    {
        Movie movie = _mapper.Map<Movie>(movieDto);
        _context.Movies.Add(movie);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetMovieById), 
            new { id = movie.Id }, 
            movie);
    }

    /// <summary>
    /// List with movies
    /// </summary>
    /// <param name="skip">Number of movies that will be skipped</param>
    /// <param name="take">Number of movies that will be taked</param>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<ReadMovieDto> GetMovies([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadMovieDto>>(_context.Movies.Skip(skip).Take(take));
    }

    /// <summary>
    /// Get a movie
    /// </summary>
    /// <param name="id">Movie identifier to select the movie that will be selected</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public IActionResult GetMovieById(int id)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        if(movie == null) return NotFound();
        var movieDto = _mapper.Map<ReadMovieDto>(movie);
        return Ok(movieDto);
    }

    /// <summary>
    /// Update a movie
    /// </summary>
    /// <param name="id">Movie identifier to select the movie that will be updated</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public IActionResult UpdateMovie(int id, [FromBody] UpdateMovieDto movieDto)
    {
        var movie = _context.Movies.FirstOrDefault(
                        movie => movie.Id == id);
        if(movie == null) return NotFound();
        _mapper.Map(movieDto, movie);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Update a movie partially
    /// </summary>
    /// <param name="id">Movie identifier to select the movie that will be partially updated</param>
    /// <param name="patch">Json with the field and values that will be updated</param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    public IActionResult PartialUpdateMovie(int id, JsonPatchDocument<UpdateMovieDto> patch)
    {
        var movie = _context.Movies.FirstOrDefault(
                        movie => movie.Id == id);
        if (movie == null) return NotFound();

        var movieToUpdate = _mapper.Map<UpdateMovieDto>(movie);

        patch.ApplyTo(movieToUpdate, ModelState);

        if(!TryValidateModel(movieToUpdate))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(movieToUpdate, movie);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Delete a movie
    /// </summary>
    /// <param name="id">Movie identifier to select the movie that will be deleted</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        var movie = _context.Movies.FirstOrDefault(
                        movie => movie.Id == id);
        if (movie == null) return NotFound();
        _context.Remove(movie);
        _context.SaveChanges();
        return NoContent();
    }
}
