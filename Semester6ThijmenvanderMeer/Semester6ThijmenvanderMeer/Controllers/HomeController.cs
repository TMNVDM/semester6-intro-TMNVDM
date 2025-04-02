using Microsoft.AspNetCore.Mvc;
using Semester6ThijmenvanderMeer.Models;
using System.Data.SqlClient;

namespace Semester6ThijmenvanderMeer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Person person)
        {
            string encryptedCC = AesEncryption.Encrypt(person.CreditCardNumber);
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Persons (FirstName, LastName, Street, HouseNumber, PostalCode, City, CreditCardNumber)
                                 VALUES (@FirstName, @LastName, @Street, @HouseNumber, @PostalCode, @City, @CreditCardNumber)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", person.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", person.LastName);
                    cmd.Parameters.AddWithValue("@Street", person.Street);
                    cmd.Parameters.AddWithValue("@HouseNumber", person.HouseNumber);
                    cmd.Parameters.AddWithValue("@PostalCode", person.PostalCode);
                    cmd.Parameters.AddWithValue("@City", person.City);
                    cmd.Parameters.AddWithValue("@CreditCardNumber", encryptedCC);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View("Succes");
        }
    }
}
