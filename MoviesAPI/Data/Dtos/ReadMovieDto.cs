using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Data.Dtos;

public class ReadMovieDto
{

    public string Title { get; set; }
    public int Duration { get; set; }
    public string Director { get; set; }
    public string Genre { get; set; }
    public DateTime QueryTime { get; set; } = DateTime.Now;
}
