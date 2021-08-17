using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebsite.Characters.Models
{
    public class Character
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int PosterId { get; set; }
        
        public string Description { get; set; }
        
        [MinLength(2)]
        public string FullName { get; set; }
        
        [MinLength(2)]
        public string OriginalFullName { get; set; }

        [Required, MinLength(1)] public HashSet<int> Persons { get; set; } = new HashSet<int>();
        [Required, MinLength(1)] public HashSet<int> Movies { get; set; } = new HashSet<int>();
    }
}