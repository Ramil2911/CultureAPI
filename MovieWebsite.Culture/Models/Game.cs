using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebsite.Movies.Models
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public Guid?PosterId { get; set; }
        
        public string Description { get; set; }
        
        [MinLength(2)]
        public string Name { get; set; }
        
        [MinLength(2)]
        public string OriginalName { get; set; }
        
        [Required] public Franchise Franchise { get; set; }
        public ICollection<Company> Developers { get; set; } = new List<Company>();
        public ICollection<Company> Publishers { get; set; } = new List<Company>();
        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}