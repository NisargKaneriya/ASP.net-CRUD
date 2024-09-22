using System.ComponentModel.DataAnnotations;

namespace Admin_Project.Models
{
    public class OrderModel
    {
        public int? OrderID { get; set; }

        [Required]
        public string OrderNumber { get; set; }

        [Required(ErrorMessage = "Order Date is required.")]
        
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        public int CustomerID { get; set; }

        //[StringLength(500, ErrorMessage = "Payment Mode cannot be longer than 50 characters.")]
        public string? PaymentMode { get; set; }

       
        public decimal? TotalAmount { get; set; }

        [Required(ErrorMessage = "Shipping Address is required.")]
        //[StringLength(500, ErrorMessage = "Shipping Address cannot be longer than 250 characters.")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }
    }
    public class OrderDropDownModel
    {
        public int OrderID { get; set; }

        [Required]
        public string OrderNumber { get; set; }
    }



}
