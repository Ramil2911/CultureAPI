using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public ICollection<Character> Characters { get; set; } = new List<Character>();
        [NotMapped]
        public IEnumerable<int> CharactersIds => Characters.Select(x => x.Id);
        [JsonIgnore]
        public ICollection<Movie> MoviesAsDirector { get; set; } = new List<Movie>();
        [NotMapped]
        public IEnumerable<int> MoviesAsDirectorIds => MoviesAsDirector.Select(x => x.Id);
        [JsonIgnore]
        public ICollection<Movie> MoviesAsActor { get; set; } = new List<Movie>();
        [NotMapped]
        public IEnumerable<int> MoviesAsActorIds => MoviesAsActor.Select(x => x.Id);
        [JsonIgnore]
        public ICollection<Serial> SerialsAsDirector { get; set; } = new List<Serial>();
        [NotMapped]
        public IEnumerable<int> SerialsAsDirectorIds => SerialsAsDirector.Select(x => x.Id);
        [JsonIgnore]
        public ICollection<Serial> SerialsAsActor { get; set; } = new List<Serial>();
        [NotMapped]
        public IEnumerable<int> SerialsAsActorIds => SerialsAsActor.Select(x => x.Id);
    }
}