using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PRSProject.Models
{
    [Index(nameof(PartNbr), IsUnique = true)]
    public class Product //PRSDb "Product" table setup
    {
        [Key]
        public int Id { get; set; } //PRIMARY KEY, auto-generated, increments by 1

        [StringLength(30)]
        public string PartNbr { get; set; } //Must be unique for all rows (Not PK), Vendor identifier for product

        [StringLength(30)]
        public string Name { get; set; } //Displayed to user, product name as given by company

        [Column(TypeName = "decimal(11,2)")]  //TypeName refers to Price, Max length 11 (2 reserved for places after decimal)
        public decimal Price { get; set; }    //No default value for Price

        [StringLength(30)]
        public string Unit { get; set; }

        [StringLength(255)]
        public string? PhotoPath { get; set; } //Nullable
        //TODO: OPTIONAL - set default image file and file path for products without an image ("No Image Available")
        
        public int VendorId { get; set; } //FOREIGN KEY (see Vendor table ID column)

        [JsonIgnore]
        public List<RequestLine>? RequestLines { get; set; } //Product ID is a FOREIGN KEY on Requests table

    }
}
