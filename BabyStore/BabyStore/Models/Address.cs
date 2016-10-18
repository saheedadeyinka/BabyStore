using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyStore.Models
{
    [ComplexType]
    public class Address
    {
        //public int Id { get; set; }
        [Required]
        [Column("AddressLine1")]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        [Column("AddressLine2")]
        public string AddressLine2 { get; set; }

        [Required]
        [Column("Town")]
        public string Town { get; set; }

        [Required]
        [Column("Country")]
        public string Country { get; set; }

        [Required]
        [Column("PostCode")]
        public string PostCode { get; set; }
    }
}