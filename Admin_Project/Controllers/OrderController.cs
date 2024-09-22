using Admin_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Admin_Project.Controllers
{
    public class OrderController : Controller
    {
        private IConfiguration configuration;

        public OrderController(IConfiguration _configuration)
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

        public List<CustomerDropDownModel> GetCustomerDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Customer_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<CustomerDropDownModel> customerDropDownList = new List<CustomerDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                CustomerDropDownModel customerDropDown = new CustomerDropDownModel();
                customerDropDown.CustomerID = Convert.ToInt32(data["CustomerID"]);
                customerDropDown.CustomerName = data["CustomerName"].ToString();
                customerDropDownList.Add(customerDropDown);
            }
            return customerDropDownList;
        }


        public IActionResult OrderList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Order_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult OrderDelete(int OrderId)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("connectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Order_Delete";
                command.Parameters.AddWithValue("@OrderId", OrderId);
                command.ExecuteNonQuery();
             }
            catch (Exception ex)
            {
                TempData["Error Message"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("OrderList");
        }

        public IActionResult Create(int OrderID)

        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            #region OrderID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Order_SelectByPK";
            command.Parameters.AddWithValue("@OrderID", OrderID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            OrderModel orderModel = new OrderModel();

            foreach (DataRow dataRow in table.Rows)
            {
                orderModel.OrderID = Convert.ToInt32(@dataRow["OrderID"]);
                orderModel.OrderDate = Convert.ToDateTime(@dataRow["OrderDate"]);
                orderModel.CustomerID = Convert.ToInt32(@dataRow["CustomerID"]);
                orderModel.PaymentMode = @dataRow["PaymentMode"].ToString();
                orderModel.TotalAmount = Convert.ToDecimal(@dataRow["TotalAmount"]);
                orderModel.ShippingAddress = @dataRow["ShippingAddress"].ToString();
                orderModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
                orderModel.OrderNumber = @dataRow["OrderNumber"].ToString();
            }

            #endregion

            ViewBag.UserDropDown = GetUserDropDown();
            ViewBag.CustomerDropDown = GetCustomerDropDown();
            return View(orderModel);
        }

        public IActionResult OrderSave(OrderModel orderModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (orderModel.OrderID == null)
                {
                    command.CommandText = "PR_Order_Insert";
                }
                else
                {
                    command.CommandText = "PR_Order_Update";
                    command.Parameters.AddWithValue("@OrderID", orderModel.OrderID);
                }
                command.Parameters.AddWithValue("@OrderDate", orderModel.OrderDate);
                command.Parameters.AddWithValue("@CustomerID", orderModel.CustomerID);
                command.Parameters.AddWithValue("@PaymentMode", orderModel.PaymentMode);
                command.Parameters.AddWithValue("@TotalAmount", orderModel.TotalAmount);
                command.Parameters.AddWithValue("@ShippingAddress", orderModel.ShippingAddress);
                command.Parameters.AddWithValue("@UserID", orderModel.UserID);
                command.Parameters.AddWithValue("@OrderNumber", orderModel.OrderNumber);
                command.ExecuteNonQuery();
                return RedirectToAction("OrderList");
            }

            
            ViewBag.UserDropDown = GetUserDropDown();
            ViewBag.CustomerDropDown = GetCustomerDropDown();
            return View("Create", orderModel);
        }


    }
}
