using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using MovieWebsite.Shared;

namespace MovieWebsite.Movies.Models
{
    public class Serial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public Guid PosterId { get; set; }
        
        public string Description { get; set; }
        
        [MinLength(2)]
        public string Name { get; set; }
        
        [MinLength(2)]
        public string OriginalName { get; set; }
        
        [Required, MinLength(1)] public HashSet<Genre> Genres { get; set; } = new HashSet<Genre>();
        
        [Required] [JsonIgnore] 
        public Franchise Franchise { get; set; }
        [NotMapped] 
        public int FranchiseId => Franchise.Id;
        
        [JsonIgnore]
        public ICollection<Person> Directors { get; set; } = new List<Person>();
        [NotMapped]
        public IEnumerable<int> DirectorsIds => Directors.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Person> Actors { get; set; } = new List<Person>();
        [NotMapped]
        public IEnumerable<int> ActorsIds => Actors.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Character> Characters { get; set; } = new List<Character>();
        [NotMapped]
        public IEnumerable<int> CharactersIds => Characters.Select(x => x.Id);
    }
}