using System.ComponentModel.DataAnnotations;

namespace FundMe.Models
{
    public class Adress
    {
        [Required]
        [Display(Name="Adress Line")]
        public string AdressLine { get; set; }
        [Required]
        public string Town { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string PostCode { get; set; }
    }
}