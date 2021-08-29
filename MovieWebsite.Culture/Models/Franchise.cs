using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebsite.Movies.Models
{
    public class Franchise
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
        public ICollection<Serial> Serials { get; set; } = new List<Serial>();
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<Game> Games { get; set; } = new List<Game>();
        public ICollection<Character> Characters { get; set; } = new List<Character>();
        public ICollection<Company> Companies { get; set; } = new List<Company>();
    }
}