using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace MovieWebsite.Movies.Models
{
    public class Game
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
        
        [JsonIgnore] 
        public Franchise? Franchise { get; set; }
        [NotMapped] 
        public int FranchiseId => Franchise?.Id ?? -1;
        
        [JsonIgnore]
        public ICollection<Company> Developers { get; set; } = new List<Company>();
        [NotMapped] 
        public IEnumerable<int> DevelopersIds => Developers.Select(x => x.Id);
        public ICollection<Company> Publishers { get; set; } = new List<Company>();
        [NotMapped] 
        public IEnumerable<int> PublishersIds => Publishers.Select(x => x.Id);
        public ICollection<Character> Characters { get; set; } = new List<Character>();
        [NotMapped] 
        public IEnumerable<int> CharactersIds => Characters.Select(x => x.Id);
    }
}