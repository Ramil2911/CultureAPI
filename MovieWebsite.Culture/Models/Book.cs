using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace MovieWebsite.Movies.Models
{
    public class Book
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
        
        [Required] [JsonIgnore]
        public Franchise Franchise { get; set; }
        [NotMapped] 
        public int FranchiseId => Franchise.Id;
        
        [JsonIgnore]
        public ICollection<Person> Authors { get; set; } = new List<Person>();
        [NotMapped]
        public IEnumerable<int> AuthorIds => Authors.Select(x => x.Id);

        [JsonIgnore]
        public ICollection<Character> Characters { get; set; } = new List<Character>();
        [NotMapped]
        public IEnumerable<int> CharactersIds => Characters.Select(x => x.Id);
    }

    /*public class ShortBook
    {
        public ShortBook(Book book, bool includeAuthors = false, bool includeCharacters = false)
        {
            Id = book.Id;
            PosterId = book.PosterId;
            Description = book.Description;
            Name = book.Description;
            OriginalName = book.OriginalName;
            FranchiseId = book.Franchise.Id;

            AuthorIds = book.Authors.Select(x => x.Id).ToArray();
            if (includeAuthors) Authors = book.Authors;
            
            CharacterIds = book.Characters.Select(x => x.Id).ToArray();
            if (includeCharacters) Characters = book.Characters;
        }
        public int Id { get; set; }
        public Guid PosterId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public int FranchiseId { get; set; }
        
        public ICollection<Person> Authors { get; set; }
        public ICollection<int>? AuthorIds { get; set; }

        public ICollection<Character> Characters { get; set; }
        public ICollection<int>? CharacterIds { get; set; }
        
    }*/
}