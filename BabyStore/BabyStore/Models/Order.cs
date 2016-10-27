using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BabyStore.Models
{
    public class Order
    {
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Display(Name = "User")]
        public string UserId { get; set; }

        [Display(Name = "Deliver To")]
        public string DeliveryName { get; set; }

        [Display(Name = "Delivery Address")]
        public Address DeliveryAddress { get; set; }

        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Time of Order")]
        public DateTime DateCreated { get; set; }

        public List<OrderLine> OrderLines { get; set; }
    }
}