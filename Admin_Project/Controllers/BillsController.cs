using Admin_Project.Bal;
using Admin_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Admin_Project.Controllers
{
    [CheckAccess]
    public class BillsController : Controller
    {
        private IConfiguration configuration;

        public BillsController(IConfiguration _configuration)
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

                orderDropDownList.Add(orderDropDown);
            }
            return orderDropDownList;
        }
        
        public IActionResult BillsList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Bills_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult BillsDelete(int BillID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Bills_Delete";
                command.Parameters.AddWithValue("@BillID",BillID);
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                TempData["Error Message"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("BillsList");
        }

        public IActionResult Create(int BillID)

        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            #region ProductByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Bills_SelectByPK";
            command.Parameters.AddWithValue("@BillID", BillID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            BillsModel billsModel = new BillsModel();

            foreach (DataRow dataRow in table.Rows)
            {
                billsModel.BillID = Convert.ToInt32(@dataRow["BillID"]);
                billsModel.BillNumber = (@dataRow["BillNumber"]).ToString();
                billsModel.BillDate = Convert.ToDateTime(@dataRow["BillDate"]);
                billsModel.OrderID = Convert.ToInt32(@dataRow["OrderID"]);
                billsModel.TotalAmount = Convert.ToDecimal(@dataRow["TotalAmount"]);
                billsModel.Discount = Convert.ToDecimal(@dataRow["Discount"]);
                billsModel.NetAmount = Convert.ToDecimal(@dataRow["NetAmount"]);
                billsModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            #endregion

            ViewBag.UserDropDown = GetUserDropDown();
            ViewBag.OrderDropDown = GetOrderDropDown();
            return View(billsModel);
        }

        public IActionResult BillsSave(BillsModel billsModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (billsModel.BillID == null )
                {
                    command.CommandText = "PR_Bills_Insert"; ;
                }
                else
                {
                    command.CommandText = "PR_Bills_Update";
                    command.Parameters.AddWithValue("@BillID", billsModel.BillID);
                }
                command.Parameters.AddWithValue("@BillNumber", billsModel.BillNumber);
                command.Parameters.AddWithValue("@BillDate", billsModel.BillDate);
                command.Parameters.AddWithValue("@OrderID", billsModel.OrderID);
                command.Parameters.AddWithValue("@TotalAmount", billsModel.TotalAmount);
                command.Parameters.AddWithValue("@Discount", billsModel.Discount);
                command.Parameters.AddWithValue("@NetAmount", billsModel.NetAmount);
                command.Parameters.AddWithValue("@UserID", billsModel.UserID);
                command.ExecuteNonQuery();
                return RedirectToAction("BillsList");
            }

            ViewBag.UserDropDown = GetUserDropDown();
            ViewBag.OrderDropDown = GetOrderDropDown();
            return View("Create", billsModel);
        }


    }

}
