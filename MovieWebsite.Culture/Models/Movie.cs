using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieWebsite.Shared;

namespace MovieWebsite.Movies.Models
{
    public class Movie
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

        [Required, MinLength(1)] public HashSet<Genre> Genres { get; set; } = new HashSet<Genre>();

        [Required] public Franchise Franchise { get; set; }
        public ICollection<Person> Directors { get; set; } = new List<Person>();
        public ICollection<Person> Actors { get; set; } = new List<Person>();
        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}