using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BindDropdown_ASPNet_Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace BindDropdown_ASPNet_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger,IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            ViewBag.Departments = GetDepartmentDetails();
            return View();
        }
        [HttpPost]
        public IActionResult Index(string DeptId, string DeptName)
        {
            ViewBag.Message = "Department Id: " + DeptId;
            ViewBag.Message += "\\ | Department Name: " + DeptName;
            ViewBag.Departments = GetDepartmentDetails();
            return View();
        }
        private static List<Department> GetDepartmentDetails()
        {
            string constr = @"Data Source=MSCNUR1888004\COREPROGRAMM;Initial Catalog=Employees;uid=sa;password=123456";
            List<Department> departments = new List<Department>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT DeptName, DeptId FROM Department";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            departments.Add(new Department
                            {
                                DeptId = Convert.ToInt32(sdr["DeptId"]),
                                DeptName = sdr["DeptName"].ToString(),
                            });
                        }
                    }
                    con.Close();
                }
            }
            return departments;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
