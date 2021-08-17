using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebsite.Blogs.Models
{
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
    }
}