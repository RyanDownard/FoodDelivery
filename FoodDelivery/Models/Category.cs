using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Category")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Display Order")]
        [Required ]
        public int DisplayOrder { get; set; }
    }
}
