using System.ComponentModel.DataAnnotations;



    public class CustomerModel
    {
        public int? CustomerID { get; set; }

        [Required(ErrorMessage = "Customer Name is required.")]

        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Home Address is required.")]
     
        public string HomeAddress { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
    
        public string MobileNo { get; set; }


        public string GST_NO { get; set; }

        [Required(ErrorMessage = "City Name is required.")]
  
        public string CityName { get; set; }

        [Required(ErrorMessage = "Pin Code is required.")]
       
        public int PinCode { get; set; }

        [Required(ErrorMessage = "Net Amount is required.")]
       
        public double NetAmount { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }
    }



public class CustomerDropDownModel
{
    public int CustomerID { get; set; }
    public string CustomerName { get; set; }
}

