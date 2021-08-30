using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace MovieWebsite.Movies.Models
{
    public class Character
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
        
        [JsonIgnore] [Required] 
        public Franchise Franchise { get; set; }
        [NotMapped]
        public int FranchiseId => Franchise.Id;
        
        [JsonIgnore]
        public ICollection<Person> Actors { get; set; } = new List<Person>();
        [NotMapped]
        public IEnumerable<int> ActorIds => Actors.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = new List<Book>();
        [NotMapped]
        public IEnumerable<int> BookIds => Books.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Game> Games { get; set; } = new List<Game>();
        [NotMapped]
        public IEnumerable<int> GamesIds => Games.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
        [NotMapped]
        public IEnumerable<int> MoviesIds => Movies.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Serial> Serials { get; set; } = new List<Serial>();
        [NotMapped]
        public IEnumerable<int> SerialsIds => Serials.Select(x => x.Id);
    }
    
    
}