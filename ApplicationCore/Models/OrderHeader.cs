using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class OrderHeader
    {
        [Key]
        public int ID { get; set; }
        public string UserID { get; set; }
        [Required]
        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime OrderDate { get; set; }
        [Required]
        [Display(Name = "Order Total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double OrderTotal { get; set; }
        [Required]
        [Display(Name = "Delivery Time")]
        public DateTime DeliveryTime { get; set; }
        [Required]
        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        [Display(Name = "Delivery Name")]
        public string DeliveryName { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public string TransactionID { get; set; }
        public string Comments { get; set; }
        public string PaymentStatus { get; set; }


        [ForeignKey("UserID")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
