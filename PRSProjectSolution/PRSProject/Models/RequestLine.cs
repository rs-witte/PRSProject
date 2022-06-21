using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PRSProject.Models
{
    public class RequestLine  //PRSDb "RequestLine" table setup
    {
        [Key]
        public int ID { get; set; } //PRIMARY KEY, auto-generated, increments by 1

        public int RequestID { get; set; }  //FOREIGN KEY (see Request table ID column)

        public int ProductID { get; set; }  //FOREIGN KEY (see Product table ID column)

        [Range(0, int.MaxValue)] //Defines min and max range for Quantity
        public int Quantity { get; set; } //DEFAULT: 1  (See PRSDbContext)

        [JsonIgnore]
        public virtual Product? Product { get; set; }

        [JsonIgnore]
        public virtual Request? Request { get; set; }
        
    }
}

