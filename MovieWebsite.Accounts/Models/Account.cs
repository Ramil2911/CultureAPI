using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieWebsite.Accounts.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public Guid AvatarGuid { get; set; }
        public string Mail { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public int Karma { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime LastOnlineTime { get; set; }
        [JsonIgnore]
        public AccountShort Short => new AccountShort(this);
    }

    /// <summary>
    /// Short implementation for Account to return from endpoints
    /// </summary>
    public class AccountShort
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="account">Parent account to make short for</param>
        public AccountShort(Account account)
        {
            Id = account.Id;
            Username = account.Username;
            AvatarId = account.AvatarGuid;
            Role = account.Role;
            LastOnlineTime = account.LastOnlineTime;
            Karma = account.Karma;
        }
        
        /// <summary>
        /// UserId
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }
        public int Karma { get; set; }
        /// <summary>
        /// Avatar as image guid from Image service
        /// </summary>
        public Guid AvatarId { get; set; }
        /// <summary>
        /// User's role
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// Last time when user was online
        /// </summary>
        public DateTime LastOnlineTime { get; set; }
    }
}