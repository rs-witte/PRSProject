using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PRSProject.Models
{
    [Index(nameof(Code), IsUnique = true)]
    public class Vendor //PRSDb "Vendor" table setup
    {
        [Key]
        public int Id { get; set; } //PRIMARY KEY, auto-generated, increments by 1

        [StringLength(30)]
        public string Code { get; set; } //Must be unique for all rows (Not PK)
        
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(30)]
        public string Address { get; set; }

        [StringLength(30)]
        public string City { get; set; }

        [StringLength(2)]
        public string State { get; set; }

        [StringLength(5)]
        public string Zip { get; set; }

        [StringLength(12)]
        public string? Phone { get; set; } //Nullable

        [StringLength(255)]
        public string? Email { get; set; } //Nullable

        //public bool IsActive { get; set; } 
        //TODO: Need to ask if vendors should be inactivated/disabled instead of deleted if related entries on other table(s) exist (ID/VendorID)

        [JsonIgnore]
        public List<Product>? Products { get; set; } //Vendor ID is a FOREIGN KEY on Requests table
    }
}
