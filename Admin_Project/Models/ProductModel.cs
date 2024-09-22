using System.ComponentModel.DataAnnotations;



 
    public class ProductModel
    {
        public int? ProductID { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]

        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product Price is required.")]

        public decimal ProductPrice { get; set; }

        [Required(ErrorMessage = "Product Code is required.")]

        public string ProductCode { get; set; }


        public string Description { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }

    }

    public class ProductDropDownModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }





