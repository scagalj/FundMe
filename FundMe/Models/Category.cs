using System.Collections.Generic;

namespace FundMe.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Naziv { get; set; }

        public virtual ICollection<Campaign> Campaign { get; set; }
    }
}