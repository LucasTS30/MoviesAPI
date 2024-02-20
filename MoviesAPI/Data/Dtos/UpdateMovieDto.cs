using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Data.DTOs;

public class UpdateMovieDto
{
    [Required(ErrorMessage = "The movie title is required.")]
    [StringLength(50, ErrorMessage = "The movie title cannot be longer than 50 characters")]
    public string Title { get; set; }

    [Required(ErrorMessage = "The duration field is required")]
    [Range(1, 360, ErrorMessage = "The duration should have at least 1 minute and at most 360 minutes")]
    public int Duration { get; set; }
    public string Director { get; set; }
    public string Genre { get; set; }
}
