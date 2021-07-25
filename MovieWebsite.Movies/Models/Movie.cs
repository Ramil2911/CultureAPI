
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebsite.Movies.Models.Compositions
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
        
        public override Short CreateShort()
        {
            return new ShortMovie() {Id = Id, Name = Name};
        }
    }

    public class ShortMovie : Short
    {
        public override long Id { get; set; }
        public string Name { get; init; }
    }
}