using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class OrderDetails
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public int MenuItemID { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }


        [ForeignKey("OrderID")]
        public virtual OrderHeader OrderHeader { get; set; }
        [ForeignKey("MenuItemID")]
        public virtual MenuItem MenuItem { get; set; }
    }
}
