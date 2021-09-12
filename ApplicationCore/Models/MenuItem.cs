using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApplicationCore.Models
{
    public class MenuItem
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Menu Item")]
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Price should be greater than $1.")]
        public float Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [Display(Name = "Category")]
        public int CategoryID { get; set; }
        [Display(Name = "Food Type")]
        public int FoodTypeID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
        [ForeignKey("FoodTypeID")]
        public virtual FoodType FoodType { get; set; }
    }
}
