using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PRSProject.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User //PRSDb "User" table setup
    {
        [Key]
        public int ID { get; set; } //PRIMARY KEY, auto-generated, increments by 1
        
        [StringLength(30)]
        public string Username { get; set; } //Must be unique for all rows (Not PK)
        
        [StringLength(30)] 
        public string Password { get; set; }

        [StringLength(30)] 
        public string Firstname { get; set; }
        
        [StringLength(30)] 
        public string Lastname { get; set; }
        
        [StringLength(12)]
        public string? Phone { get; set; } //Nullable

        [StringLength(255)]
        public string? Email { get; set; } //Nullable

        public bool IsReviewer { get; set; }

        public bool IsAdmin { get; set; }

        //public bool IsActive { get; set; } 
        //TODO: Need to ask if users should be inactivated/disabled instead of deleted if related entries on other table(s) exist (ID/UserID)

        [JsonIgnore]
        public List<Request>? Requests { get; set; } //User ID is a FOREIGN KEY on Requests table

        

    }
}
