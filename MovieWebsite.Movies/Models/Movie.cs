using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieWebsite.Shared;

namespace MovieWebsite.Movies.Models
{
    public class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        public long PosterId { get; set; }
        
        public string Description { get; set; }
        
        [MinLength(2)]
        public string Name { get; set; }
        
        [MinLength(2)]
        public string OriginalName { get; set; }

        [Required, MinLength(1)] public HashSet<Genre> Genres { get; set; } = new HashSet<Genre>();
        
        [Required] public HashSet<long> DirectorIds { get; set; } = new HashSet<long>();
        [Required] public HashSet<long> ActorIds { get; set; } = new HashSet<long>();
        [Required] public HashSet<long> CharacterIds { get; set; } = new HashSet<long>();
    }
}