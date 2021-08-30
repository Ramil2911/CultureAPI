using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace MovieWebsite.Movies.Models
{
    public class Franchise
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        
        [JsonIgnore]
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
        [NotMapped] 
        public IEnumerable<int> MoviesIds => Movies.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Serial> Serials { get; set; } = new List<Serial>();
        [NotMapped]
        public IEnumerable<int> SerialsIds => Serials.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = new List<Book>();
        [NotMapped]
        public IEnumerable<int> BooksIds => Books.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Game> Games { get; set; } = new List<Game>();
        [NotMapped]
        public IEnumerable<int> GamesIds => Games.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Character> Characters { get; set; } = new List<Character>();
        [NotMapped]
        public IEnumerable<int> CharactersIds => Characters.Select(x => x.Id);
        
        [JsonIgnore]
        public ICollection<Company> Companies { get; set; } = new List<Company>();
        [NotMapped]
        public IEnumerable<int> CompaniesIds => Companies.Select(x => x.Id);
    }
    
    /*i'm still unsure, but there are 2 ways to implement "short" types. using short types like defined below or using
    System.Text.Json and EF Core attributes above. First one is more flexible but needs much more code and pretty boring,
    but second one is less flexible (idk how to implement conditional serialization,
    it's supported by Newtonsoft.Json but not System.Text.Json) and less code.*/

    /*public class ShortFranchise
    {
        public ShortFranchise(Franchise franchise, bool includeMovies = false, bool includeSerials = false, bool includeBooks = false,
            bool includeGames = false, bool includeCharacters = false, bool includeCompanies = false)
        {
            Id = franchise.Id;
            Name = franchise.Name;
            
            MovieIds = franchise.Movies.Select(x => x.Id).ToArray();
            if (includeMovies) Movies = franchise.Movies;
            
            SerialIds = franchise.Serials.Select(x => x.Id).ToArray();
            if (includeSerials) Serials = franchise.Serials;
            
            BookIds = franchise.Books.Select(x => x.Id).ToArray();
            if (includeBooks) Books = franchise.Books;
            
            GamesIds = franchise.Games.Select(x => x.Id).ToArray();
            if (includeGames) Serials = franchise.Serials;
            
            CharacterIds = franchise.Characters.Select(x => x.Id).ToArray();
            if (includeCharacters) Characters = franchise.Characters;
            
            CompaniesIds = franchise.Companies.Select(x => x.Id).ToArray();
            if (includeCompanies) Companies = franchise.Companies;
        }
        
        public int Id { get; set; }
        public string Name { get; set; } = "";
        
        public ICollection<int> MovieIds { get; set; }
        public ICollection<Movie>? Movies { get; set; } 
        
        public ICollection<int> SerialIds { get; set; }
        public ICollection<Serial>? Serials { get; set; }
        
        public ICollection<int> BookIds { get; set; }
        public ICollection<Book>? Books { get; set; }
        
        public ICollection<int> GamesIds { get; set; }
        public ICollection<Game>? Games { get; set; }
        
        public ICollection<int> CharacterIds { get; set; }
        public ICollection<Character>? Characters { get; set; }
        
        public ICollection<int> CompaniesIds { get; set; }
        public ICollection<Company>? Companies { get; set; }
    }*/
}