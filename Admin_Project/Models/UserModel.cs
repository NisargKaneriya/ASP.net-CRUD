using System.ComponentModel.DataAnnotations;


    public class UserModel
    {
        public int? UserID { get; set; }

        [Required(ErrorMessage = "User Name is required.")]
       
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
       
        public string Password { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
       
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Address is required.")]
      
        public string Address { get; set; }

        [Required(ErrorMessage = "Is Active status is required.")]
        public bool IsActive { get; set; }
    }

    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }

   
