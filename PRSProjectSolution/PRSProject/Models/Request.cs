using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PRSProject.Models
{
    public class Request //PRSDb "Request" table setup
    {
        [Key]
        public int Id { get; set; } //PRIMARY KEY, auto-generated, increments by 1

        [StringLength(80)]
        public string Description { get; set; }

        [StringLength(80)]
        public string Justification { get; set; }

        [StringLength(80)]
        public string? RejectionReason { get; set; } //Nullable - Note: Must be provided by user upon rejection

        [StringLength(20)]
        public string DeliveryMode { get; set; } = "Pickup"; //DEFAULT: "Pickup"
        //TODO: Allowed DeliveryMode values list & validation needed ---> Pickup, Mail/Delivery...etc.

        [StringLength(10)]
        public string Status { get; set; } = "NEW"; //DEFAULT: "NEW" - Note: Cannot be set by user
        //TODO: Allowed/acceptable Status values list needed ---> NEW, REVIEW, APPROVED, REJECTED

        [Column(TypeName = "decimal(11,2)")] //TypeName refers to Total, Max length 11 (2 reserved for places after decimal)
        public decimal Total { get; set; }  // DEFAULT: 0 - Note: Cannot be set by user, auto-calc (set by app)

        public int UserId { get; set; } //FOREIGN KEY (see User table ID column)

        [JsonIgnore]
        public List<RequestLine>? RequestLines { get; set; } //Request ID is a FOREIGN KEY on Request Lines table
    }
}
