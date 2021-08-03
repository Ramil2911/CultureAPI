using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebsite.Persons.Models
{
    public class Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        public long PosterId { get; set; }
        
        public string Description { get; set; }
        
        [MinLength(2)]
        public string FullName { get; set; }
        
        [MinLength(2)]
        public string OriginalFullName { get; set; }

        [Required, MinLength(1)] public HashSet<long> Characters { get; set; } = new HashSet<long>();
        [Required, MinLength(1)] public HashSet<long> Movies { get; set; } = new HashSet<long>();
    }
}