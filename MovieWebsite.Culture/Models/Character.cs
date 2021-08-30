using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebsite.Movies.Models
{
    public class Character
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public Guid?PosterId { get; set; }
        
        public string Description { get; set; }
        
        [MinLength(2)]
        public string FullName { get; set; }
        
        [MinLength(2)]
        public string OriginalFullName { get; set; }
        
        [Required] public Franchise Franchise { get; set; }
        public ICollection<Person> Actors { get; set; } = new List<Person>();
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<Game> Games { get; set; } = new List<Game>();
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
        public ICollection<Serial> Serials { get; set; } = new List<Serial>();
    }
}