using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace MovieWebsite.Blogs.Models
{
    /// <summary>
    /// Database entity for post
    /// </summary>
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public Guid Guid { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public string BodyRaw { get; set; }
        public string BodyHtml { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Note { get; set; }
        public DateTime TimeStamp { get; set; }
        public HashSet<int> RankUppers { get; set; } = new HashSet<int>();
        public HashSet<int> RankDowners { get; set; } = new HashSet<int>();
        public List<Comment> Comments { get; set; } = new List<Comment>();

        /// <summary>
        /// A method to create short version of object
        /// </summary>
        /// <returns>Short version of object</returns>
        [JsonIgnore]
        public ShortPost Short
        => new ShortPost(this);
    }

    /// <summary>
    /// Short version of post to return from endpoint
    /// </summary>
    public class ShortPost
    {
        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="post">Post to use as data source</param>
        public ShortPost(Post post)
        {
            Guid = post.Guid;
            AuthorId = post.AuthorId;
            Title = post.Title;
            BodyRaw = post.BodyRaw;
            Note = post.Note;
            TimeStamp = post.TimeStamp;
            Rank = post.RankUppers.Count - post.RankDowners.Count;
            RankUppers = post.RankUppers;
            RankDowners = post.RankDowners;
        }
        
        /// <summary>
        /// Guid of post
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// Id of author of post
        /// </summary>
        public int AuthorId { get; set; }
        /// <summary>
        /// Title of post
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Raw body of post. Json data which can be passed as data for Editor.js
        /// </summary>
        public string BodyRaw { get; set; }
        /// <summary>
        /// Note of administration
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// Time when post was created
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// Rank of post
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// Ids of those who ranked post up
        /// </summary>
        public HashSet<int> RankUppers { get; set; }
        /// <summary>
        /// Ids of those who ranked post down
        /// </summary>
        public HashSet<int> RankDowners { get; set; }
    }
}