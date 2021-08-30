using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebsite.Movies.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public Guid PosterId { get; set; }
        
        public string Description { get; set; }
        
        [MinLength(2)]
        public string FullName { get; set; }
        
        [MinLength(2)]
        public string OriginalFullName { get; set; }
        public ICollection<Character> Characters { get; set; } = new List<Character>();
        public ICollection<Movie> MoviesAsDirector { get; set; } = new List<Movie>();
        public ICollection<Movie> MoviesAsActor { get; set; } = new List<Movie>();
        public ICollection<Serial> SerialsAsDirector { get; set; } = new List<Serial>();
        public ICollection<Serial> SerialsAsActor { get; set; } = new List<Serial>();
    }
}