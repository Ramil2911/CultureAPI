using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MovieWebsite.Blogs.Models
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public Guid Guid { get; set; }
        public int AuthorId { get; set; }
        public string Text { get; set; } = "";
        public HashSet<string> AttachmentUrls { get; set; } = new HashSet<string>();
        public bool IsDeleted { get; set; } = false;
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public HashSet<int> RankUppers { get; set; } = new HashSet<int>();
        public HashSet<int> RankDowners { get; set; } = new HashSet<int>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }

    public class ShortComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public Guid Guid { get; set; }
        public int AuthorId { get; set; }
        public string Text { get; set; }
        public HashSet<string> AttachmentUrls { get; set; }
        public DateTime TimeStamp { get; set; }
        public HashSet<int> RankUppers { get; set; }
        public HashSet<int> RankDowners { get; set; }
        public List<ShortComment> Comments { get; set; }
        public bool HasChildren { get; set; }
        
        public ShortComment(Comment comment)
        {
            Guid = comment.Guid;
            AuthorId = comment.AuthorId;
            Text = comment.Text;
            AttachmentUrls = comment.AttachmentUrls;
            TimeStamp = comment.TimeStamp;
            RankUppers = comment.RankUppers;
            RankDowners = comment.RankDowners;
            Comments = comment.Comments.Select(x=>new ShortComment(x)).ToList();
            CheckChildren();
        }

        private void CheckChildren() //3-level loading is default
        {
            HasChildren = Comments.Count != 0;
            if (!HasChildren) return;
            foreach (var vComment1 in Comments)
            {
                vComment1.HasChildren = vComment1.Comments.Count != 0;
                if (!vComment1.HasChildren) continue;
                foreach (var vComment2 in vComment1.Comments)
                {
                    vComment2.HasChildren = vComment2.Comments.Count != 0;
                    if (!vComment2.HasChildren) continue;
                    foreach (var vComment3 in vComment2.Comments)
                    {
                        vComment3.HasChildren = vComment3.Comments.Count != 0;
                        vComment3.Comments.Clear();
                    }
                }
            }
        }
    }
}