﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FundMe.Models
{
    public class Image
    {
        public int ID { get; set; }
        [Display(Name = "File")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string FileName { get; set; }

        public override string ToString()
        {
            return FileName;
        }
    }
}