using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FundMe.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "The category name can't be empty")]
        [StringLength(20,MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 20 characters")]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        public virtual ICollection<Campaign> Campaign { get; set; }
    }
}