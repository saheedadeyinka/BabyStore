using System.ComponentModel.DataAnnotations;

namespace BabyStore.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        [Display(Name = "File")]
        public string FileName { get; set; }
    }
}