using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebsite.Movies.Models
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int PosterId { get; set; }
        
        public string Description { get; set; }
        
        [MinLength(2)]
        public string Name { get; set; }
        
        [MinLength(2)]
        public string OriginalName { get; set; }
        
        public Franchise Franchise { get; set; }
        public ICollection<Movie> MoviesAsPublisher { get; set; } = new List<Movie>();
        public ICollection<Movie> MoviesAsDeveloper { get; set; } = new List<Movie>();
        public ICollection<Serial> SerialsAsPublisher { get; set; } = new List<Serial>();
        public ICollection<Serial> SerialsAsDeveloper { get; set; } = new List<Serial>();
        public ICollection<Game> GamesAsPublisher { get; set; } = new List<Game>();
        public ICollection<Game> GamesAsDeveloper { get; set; } = new List<Game>();
    }
}