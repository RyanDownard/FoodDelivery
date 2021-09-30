using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Count = 1;
        }

        public int ID { get; set; }
        public int MenuItemID { get; set; }
        [Range(1, 100, ErrorMessage = "Pleas select a count between 1-100")]
        public int Count { get; set; }
        public string ApplicationuserID { get; set; }

        [NotMapped]
        [ForeignKey("MenuItemID")]
        public virtual MenuItem MenuItem { get; set; }
        [NotMapped]
        [ForeignKey("ApplicationUserID")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
