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
        
        [Required] public HashSet<int> DirectorIds { get; set; } = new HashSet<int>();
        [Required] public HashSet<int> ActorIds { get; set; } = new HashSet<int>();
        [Required] public HashSet<int> CharacterIds { get; set; } = new HashSet<int>();
    }
}