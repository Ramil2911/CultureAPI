using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace MovieWebsite.Movies.Models
{
    public class Company
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

        [JsonIgnore] public ICollection<Franchise> Franchises { get; set; } = new List<Franchise>();

        [NotMapped] public IEnumerable<int> FranchisesIds => Franchises.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Movie> MoviesAsPublisher { get; set; } = new List<Movie>();
        [NotMapped]
        public IEnumerable<int> MoviesAsPublisherIds => MoviesAsPublisher.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Movie> MoviesAsDeveloper { get; set; } = new List<Movie>();
        [NotMapped]
        public IEnumerable<int> MoviesAsDeveloperIds => MoviesAsDeveloper.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Serial> SerialsAsPublisher { get; set; } = new List<Serial>();
        [NotMapped]
        public IEnumerable<int> SerialsAsPublisherIds => SerialsAsPublisher.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Serial> SerialsAsDeveloper { get; set; } = new List<Serial>();
        [NotMapped]
        public IEnumerable<int> SerialsAsDeveloperIds => SerialsAsDeveloper.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Game> GamesAsPublisher { get; set; } = new List<Game>();
        [NotMapped]
        public IEnumerable<int> GamesAsPublisherIds => GamesAsPublisher.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Game> GamesAsDeveloper { get; set; } = new List<Game>();
        [NotMapped]
        public IEnumerable<int> GamesAsDeveloperIds => GamesAsDeveloper.Select(x => x.Id);
    }
}