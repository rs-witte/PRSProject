using System.ComponentModel.DataAnnotations;

namespace PRSProject.Models
{
    public class RequestLine  //PRSDb "RequestLine" table setup
    {
        [Key]
        public int ID { get; set; } //PRIMARY KEY, auto-generated, increments by 1

        public int RequestID { get; set; }  //FOREIGN KEY (see Request table ID column)

        public int ProductID { get; set; }  //FOREIGN KEY (see Product table ID column)

        public int Quantity { get; set; }  //DEFAULT: 1 - Note: Cannot be set to negative (0+)

    }
}
