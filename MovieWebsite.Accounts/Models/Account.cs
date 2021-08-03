using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebsite.Accounts.Models
{
    public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Login { get; set; }
        public long AvatarId { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime LastOnlineTime { get; set; }
    }
}