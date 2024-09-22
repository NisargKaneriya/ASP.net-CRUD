using Admin_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Admin_Project.Controllers
{
    public class OrderdetailController : Controller
    {
        private IConfiguration configuration;

        public OrderdetailController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public List<UserDropDownModel> GetUserDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_User_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<UserDropDownModel> userDropDownList = new List<UserDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                UserDropDownModel userDropDown = new UserDropDownModel();
                userDropDown.UserID = Convert.ToInt32(data["UserID"]);
                userDropDown.UserName = data["UserName"].ToString();
                userDropDownList.Add(userDropDown);
            }
            return userDropDownList;
        }

        public List<ProductDropDownModel> GetProductDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Product_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<ProductDropDownModel> productDropDownList = new List<ProductDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                ProductDropDownModel productDropDown = new ProductDropDownModel();
                productDropDown.ProductID = Convert.ToInt32(data["ProductID"]);
                productDropDown.ProductName = data["ProductName"].ToString();
                productDropDownList.Add(productDropDown);
            }
            return productDropDownList;
        }

        public List<OrderDropDownModel> GetOrderDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Order_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<OrderDropDownModel> orderDropDownList = new List<OrderDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                OrderDropDownModel orderDropDown = new OrderDropDownModel();
                orderDropDown.OrderID = Convert.ToInt32(data["OrderID"]);
                orderDropDown.OrderNumber = data["OrderNumber"].ToString();

                orderDropDownList.Add(orderDropDown);
            }
            return orderDropDownList;
        }

        public IActionResult OrderdetailList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_OrderDetail_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult OrderdetailDelete(int OrderDetailID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType=CommandType.StoredProcedure;
                command.CommandText = "PR_OrderDetail_Delete";
                command.Parameters.AddWithValue("@OrderDetailID", OrderDetailID);
                command.ExecuteNonQuery();

            }
            catch(Exception ex) 
            {
                TempData["Error Message"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("OrderdetailList");
        }

        public IActionResult Create(int OrderDetailID)

        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            #region OrderdetailID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_OrderDetail_SelectAllByPK";
            command.Parameters.AddWithValue("@OrderDetailID", OrderDetailID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            OrderdetailModel orderdetailModel = new OrderdetailModel();

            foreach (DataRow dataRow in table.Rows)
            {
                orderdetailModel.OrderDetailID = Convert.ToInt32(@dataRow["OrderDetailID"]);
                orderdetailModel.OrderID = Convert.ToInt32(@dataRow["OrderID"]);
                orderdetailModel.ProductID = Convert.ToInt32(@dataRow["ProductID"]);
                orderdetailModel.Quantity = Convert.ToInt32(@dataRow["Quantity"]);
                orderdetailModel.Amount = Convert.ToDecimal(@dataRow["Amount"]);
                orderdetailModel.TotalAmount = Convert.ToDecimal(@dataRow["TotalAmount"]);
                orderdetailModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            #endregion

            ViewBag.UserDropDown = GetUserDropDown();
            ViewBag.ProductDropDown = GetProductDropDown();
            ViewBag.OrderDropDown = GetOrderDropDown();
            return View(orderdetailModel);
        }

        public IActionResult OrderdetailSave(OrderdetailModel orderdetailModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (orderdetailModel.OrderDetailID == null)
                {
                    command.CommandText = "PR_OrderDetail_Insert";
                }
                else
                {
                    command.CommandText = "PR_OrderDetail_Update";
                    command.Parameters.AddWithValue("@OrderDetailID", orderdetailModel.@OrderDetailID);
                }
                command.Parameters.AddWithValue("@OrderID", orderdetailModel.OrderID);
                command.Parameters.AddWithValue("@ProductID", orderdetailModel.ProductID);
                command.Parameters.AddWithValue("@Quantity", orderdetailModel.Quantity);
                command.Parameters.AddWithValue("@Amount", orderdetailModel.Amount);
                command.Parameters.AddWithValue("@TotalAmount", orderdetailModel.TotalAmount);
                command.Parameters.AddWithValue("@UserID", orderdetailModel.UserID);
                command.ExecuteNonQuery();
                return RedirectToAction("OrderdetailList");
            }

            ViewBag.UserDropDown = GetUserDropDown();
            ViewBag.ProductDropDown = GetProductDropDown();
            ViewBag.OrderDropDown = GetOrderDropDown();
            return View("Create", orderdetailModel);
        }
    }
}
