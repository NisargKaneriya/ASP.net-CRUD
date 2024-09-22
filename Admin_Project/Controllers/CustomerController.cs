using Admin_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Admin_Project.Controllers
{
    public class CustomerController : Controller
    {
        private IConfiguration configuration;

        public CustomerController(IConfiguration _configuration)
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

        public IActionResult CustomerList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Customer_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult CustomerDelete(int CustomerID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Customer_Delete";
                command.Parameters.AddWithValue("@CustomerID", CustomerID);
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                TempData["Error Message"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("CustomerList");
        }

        public IActionResult Create(int CustomerID)

        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            #region ProductByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Customer_SelectByPK";
            command.Parameters.AddWithValue("@CustomerID", CustomerID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            CustomerModel customerModel = new CustomerModel();

            foreach (DataRow dataRow in table.Rows)
            {
                customerModel.CustomerID = Convert.ToInt32(@dataRow["CustomerID"]);
                customerModel.CustomerName = @dataRow["CustomerName"].ToString();
                customerModel.HomeAddress = @dataRow["HomeAddress"].ToString();
                customerModel.Email = @dataRow["Email"].ToString();
                customerModel.MobileNo = @dataRow["MobileNo"].ToString();
                customerModel.GST_NO = @dataRow["GST_NO"].ToString();
                customerModel.CityName = @dataRow["CityName"].ToString();
                customerModel.PinCode = Convert.ToInt32(@dataRow["PinCode"]);
                customerModel.NetAmount = Convert.ToDouble(@dataRow["NetAmount"]);
                customerModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            #endregion

            ViewBag.UserDropDown = GetUserDropDown();
            return View(customerModel);
        }

        public IActionResult CustomerSave(CustomerModel customerModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (customerModel.CustomerID == null)
                {
                    command.CommandText = "PR_Customer_Insert";
                }
                else
                {
                    command.CommandText = "PR_Customer_Update";
                    command.Parameters.AddWithValue("@CustomerID", customerModel.CustomerID);
                }
                command.Parameters.AddWithValue("@CustomerName", customerModel.CustomerName);
                command.Parameters.AddWithValue("@HomeAddress", customerModel.HomeAddress);
                command.Parameters.AddWithValue("@Email", customerModel.Email);
                command.Parameters.AddWithValue("@MobileNo", customerModel.MobileNo);
                command.Parameters.AddWithValue("@GSTNO", customerModel.GST_NO);
                command.Parameters.AddWithValue("@CityName", customerModel.CityName);
                command.Parameters.AddWithValue("@PinCode", customerModel.PinCode);
                command.Parameters.AddWithValue("@NetAmount", customerModel.NetAmount);
                command.Parameters.AddWithValue("@UserID", customerModel.UserID);
                command.ExecuteNonQuery();
                return RedirectToAction("CustomerList");
            }

            ViewBag.UserDropDown = GetUserDropDown();
            return View("Create", customerModel);
        }

    }
}
