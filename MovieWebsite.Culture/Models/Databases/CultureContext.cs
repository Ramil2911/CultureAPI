using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MovieWebsite.Shared;

namespace MovieWebsite.Movies.Models.Databases
{
    public class CultureContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Serial> Serials { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Franchise> Franchises { get; set; }
        public DbSet<Game> Games { get; set; }

        /*public CultureContext()
        {
            Database.EnsureCreated();
        }
        
        public CultureContext(DbContextOptions<CultureContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }*/
            
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=compositiondb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Map movies+persons
            modelBuilder.Entity<Movie>()
                .HasMany(x => x.Actors)
                .WithMany(x => x.MoviesAsActor);
            modelBuilder.Entity<Movie>()
                .HasMany(x => x.Directors)
                .WithMany(x => x.MoviesAsDirector);
            //Map serials+persons
            modelBuilder.Entity<Serial>()
                .HasMany(x => x.Actors)
                .WithMany(x => x.SerialsAsActor);
            modelBuilder.Entity<Serial>()
                .HasMany(x => x.Directors)
                .WithMany(x => x.SerialsAsDirector);
            //Map games+companies
            modelBuilder.Entity<Game>()
                .HasMany(x => x.Publishers)
                .WithMany(x => x.GamesAsPublisher);
            modelBuilder.Entity<Game>()
                .HasMany(x => x.Developers)
                .WithMany(x => x.GamesAsDeveloper);
            /*//Map movies
            modelBuilder.Entity<Movie>()
                .HasMany(x => x.Directors)
                .WithMany(x => x.Movies)
                .UsingEntity<MoviePerson>(
                    x => x
                        .HasOne(y => y.Person)
                        .WithMany(y => y.PersonMovies)
                        .HasForeignKey(y => y.PersonId)
                    ,
                    x=> x
                        .HasOne(y => y.Movie)
                        .WithMany(y=>y.MovieDirectors)
                        .HasForeignKey(y=>y.MovieId),
                    y=>y.HasKey(k=> new { k.MovieId, k.PersonId }));
            modelBuilder.Entity<Movie>()
                .HasMany(x => x.Actors)
                .WithMany(x => x.Movies)
                .UsingEntity<MoviePerson>(
                    x => x
                        .HasOne(y => y.Person)
                        .WithMany(y => y.PersonMovies)
                        .HasForeignKey(y => y.PersonId)
                    ,
                    x=> x
                        .HasOne(y => y.Movie)
                        .WithMany(y=>y.MovieDirectors)
                        .HasForeignKey(y=>y.MovieId),
                    y=>y.HasKey(k=> new { k.MovieId, k.PersonId }));
            modelBuilder.Entity<Movie>()
                .HasMany(x => x.Characters)
                .WithMany(x => x.Movies)
                .UsingEntity<MovieCharacter>(x=>x
                        .HasOne(y=>y.Character)
                        .WithMany(y=>y.MovieCharacters)
                        .HasForeignKey(y=>y.CharacterId)
                    , 
                    x=>x
                        .HasOne(y=>y.Movie)
                        .WithMany(y=>y.MovieCharacters)
                        .HasForeignKey(y=>y.MovieId)
                    ,
                    y=>y.HasKey(k=> new { k.CharacterId, k.MovieId }));
            
            //Map serials
            modelBuilder.Entity<Serial>()
                .HasMany(x => x.Actors)
                .WithMany(x => x.Serials)
                .UsingEntity<PersonSerial>(x=>x
                    .HasOne(y=>y.Person)
                    .WithMany(y=>y.PersonSerials)
                    .HasForeignKey(y=>y.PersonId)
                ,
                y=>y
                    .HasOne(y=>y.Serial)
                    .WithMany(y=>y.ActorSerials)
                    .HasForeignKey(y=>y.SerialId)
                ,
                y=>y.HasKey(k=> new { k.PersonId, k.SerialId }));
            modelBuilder.Entity<Serial>()
                .HasMany(x => x.Characters)
                .WithMany(x => x.Serials)
                .UsingEntity<SerialCharacter>(x => x
                        .HasOne(y => y.Character)
                        .WithMany(y => y.SerialCharacters)
                        .HasForeignKey(y => y.CharacterId)
                    ,
                    x => x
                        .HasOne(y => y.Serial)
                        .WithMany(y => y.SerialCharacters)
                        .HasForeignKey(y => y.SerialId)
                    ,
                    y=>y.HasKey(k=> new {k.CharacterId, k.SerialId }));
            modelBuilder.Entity<Serial>()
                .HasMany(x => x.Directors)
                .WithMany(x => x.Serials)
                .UsingEntity<PersonSerial>(x=>x
                        .HasOne(y=>y.Person)
                        .WithMany(y=>y.PersonSerials)
                        .HasForeignKey(y=>y.PersonId)
                    ,
                    y=>y
                        .HasOne(y=>y.Serial)
                        .WithMany(y=>y.ActorSerials)
                        .HasForeignKey(y=>y.SerialId)
                    ,
                    y=>y.HasKey(k=> new { k.PersonId, k.SerialId }));
            
            //Map books
            modelBuilder.Entity<Book>()
                .HasMany(x => x.Authors)
                .WithMany(x => x.Books)
                .UsingEntity<PersonBook>(x=>x
                    .HasOne(y=>y.Person)
                    .WithMany(y=>y.PersonBooks)
                    .HasForeignKey(y=>y.PersonId)
                ,
                x=>x
                    .HasOne(y=>y.Book)
                    .WithMany(y=>y.BookAuthors)
                    .HasForeignKey(y=>y.BookId)
                ,
                x=>x.HasKey(k=> new {k.BookId, k.PersonId}));
            modelBuilder.Entity<Book>()
                .HasMany(x => x.Characters)
                .WithMany(x => x.Books)
                .UsingEntity<BookCharacter>(x=>x
                        .HasOne(y=>y.Character)
                        .WithMany(y=>y.BookCharacters)
                        .HasForeignKey(y=>y.CharacterId)
                    ,
                    x=>x
                        .HasOne(y=>y.Book)
                        .WithMany(y=>y.BookCharacters)
                        .HasForeignKey(y=>y.BookId)
                    ,
                    x=>x.HasKey(k=> new {k.BookId, k.CharacterId}));
            
            //Map person+character
            modelBuilder.Entity<Person>()
                .HasMany(x => x.Characters)
                .WithMany(x => x.Actors)
                .UsingEntity<PersonCharacter>(x=>x
                    .HasOne(y=>y.Character)
                    .WithMany(y=>y.PersonCharacters)
                    .HasForeignKey(y=>y.CharacterId)
                ,
                x=>x
                    .HasOne(y=>y.Person)
                    .WithMany(y=>y.PersonCharacters)
                    .HasForeignKey(y=>y.PersonId));*/
            var enumValueComparer = new ValueComparer<HashSet<Genre>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToHashSet());
            var intValueComparer = new ValueComparer<HashSet<int>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToHashSet());
            
            modelBuilder.Entity<Movie>()
                .Property(e => e.Genres)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Enum.Parse<Genre>).ToHashSet())
                .Metadata.SetValueComparer(enumValueComparer);
            modelBuilder.Entity<Serial>()
                .Property(e => e.Genres)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Enum.Parse<Genre>).ToHashSet())
                .Metadata.SetValueComparer(enumValueComparer);
        }
    }
}