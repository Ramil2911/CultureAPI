using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace MovieWebsite.Movies.Models
{
    public enum Genre : ushort
    {
        Action,
        Adventure,
        Comedy,
        Crime,
        Fantasy,
        Historical,
        HistoricalFiction,
        Horror,
        Romance,
        Satire,
        ScienceFiction, 
        Cyberpunk,
        Speculative,
        Thriller,
        Western,
        Political,
        Drama,
        Sports,
        Military,
        Anime,
        SliceOfLife,
        Magic,
        Supernatural,
        Psychological,
        Japanese
    }
}