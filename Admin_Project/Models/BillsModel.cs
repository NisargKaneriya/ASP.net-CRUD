using System.ComponentModel.DataAnnotations;

namespace Admin_Project.Models
{
    public class BillsModel
    {
        public int? BillID { get; set; }

        [Required(ErrorMessage = "Enter valid data!!!")]
        public string BillNumber { get; set; }

        [Required(ErrorMessage = "Bill Date is required.")]
       
        public DateTime BillDate { get; set; }

        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Total Amount is required.")]
        public decimal TotalAmount { get; set; }

        public decimal? Discount { get; set; }

        [Required(ErrorMessage = "Net Amount is required.")]
        public decimal NetAmount { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }
    }
}
