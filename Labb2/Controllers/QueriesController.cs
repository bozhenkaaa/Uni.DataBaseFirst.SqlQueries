using Labb2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Labb2.Controllers
{
    public class QueriesController : Controller
    {

        private const string CONN_STR = "Server= DESKTOP-BUN868F\\SQLEXPRESS; Database=Lab2; Trusted_Connection=True; Trust Server Certificate=True;";

        private const string Q1_PATH = @"C:\Users\admin\Desktop\Labb2\Labb2\Queries\Q1.sql";
        private const string Q2_PATH = @"C:\Users\admin\Desktop\Labb2\Labb2\Queries\Q2.sql";
        private const string Q3_PATH = @"C:\Users\admin\Desktop\Labb2\Labb2\Queries\Q3.sql";
        private const string Q4_PATH = @"C:\Users\admin\Desktop\Labb2\Labb2\Queries\Q4.sql";
        private const string Q5_PATH = @"C:\Users\admin\Desktop\Labb2\Labb2\Queries\Q5.sql";
        private const string Q6_PATH = @"C:\Users\admin\Desktop\Labb2\Labb2\Queries\Q6.sql";

        private const string Qd1_PATH = @"C:\Users\admin\Desktop\Labb2\Labb2\Queries\Qd1.sql";
        private const string Qd2_PATH = @"C:\Users\admin\Desktop\Labb2\Labb2\Queries\Qd2.sql";
        private const string Qd3 = @"C:\Users\admin\Desktop\Labb2\Labb2\Queries\Qd3.sql";


        private const string ERR_AVG = "Неможливо обрахувати загальну ціну, оскільки продукти відсутні.";
        private const string ERR_CUST = "Покупці, що задовольняють дану умову, відсутні.";
        private const string ERR_PROD = "Програмні продукти, що задовольняють дану умову, відсутні.";
        private const string ERR_DEV = "Розробники, що задовольняють дану умову, відсутні.";
        private const string ERR_COUNTRY = "Країни, що задовольняють дану умову, відсутні.";

        private readonly Labb2Context _context;

        public QueriesController(Labb2Context context)
        {
            _context = context;
        }

        public IActionResult Index(int errorCode)
        {
            var customers = _context.Customers.Select(c => c.Name).Distinct().ToList();
            if (errorCode == 1)
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.PriceError = "Введіть коректну вартість";
            }
            if (errorCode == 2)
            {
                ViewBag.ErrorFlag = 2;
                ViewBag.ProdNameError = "Поле необхідно заповнити";
            }

            var empty = new SelectList(new List<string> { "--Пусто--" });
            var anyCusts = _context.Customers.Any();
            var anyProducers = _context.Producers.Any();
            var anyCountries= _context.Countries.Any();
            

            ViewBag.ProducerIds = anyProducers ? new SelectList(_context.Producers, "Id", "Id") : empty;
            ViewBag.ProducerNames = anyProducers ? new SelectList(_context.Producers, "Name", "Name") : empty;
            ViewBag.CustomerNames = anyCusts ? new SelectList(customers) : empty;
            ViewBag.CustomerEmails = anyCusts ? new SelectList(_context.Customers, "Email", "Email") : empty;
            ViewBag.CustomerSurnames = anyCusts ? new SelectList(_context.Customers, "Surname", "Surname") : empty;
            ViewBag.Countries = _context.Countries.Any() ? new SelectList(_context.Countries, "Name", "Name") : empty;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Query1(Query1 queryModel)
        {
            //Знайти середню вартість машин виробника P.
            string query = System.IO.File.ReadAllText(Q1_PATH);
            query = query.Replace("X", "N\'" + queryModel.ProducerName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "Q1";

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        queryModel.AveragePrice = Convert.ToDecimal(result);
                    }
                    else
                    {
                        queryModel.ErrorFlag = 1;
                        queryModel.Error = ERR_AVG;
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Query2(Query1 queryModel)
        {
            string query = System.IO.File.ReadAllText(Q2_PATH);
            query = query.Replace("X", "N\'" + queryModel.CountryName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "Q2";
            queryModel.CarNames = new List<string>();
            queryModel.CarPrices = new List<decimal>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.CarNames.Add(reader.GetString(0));
                            queryModel.CarPrices.Add(reader.GetDecimal(1));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_PROD;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Query3(Query1 queryModel)
        {
            string query = System.IO.File.ReadAllText(Q3_PATH);
            query = query.Replace("X", "N\'" + queryModel.ProducerName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "Q3";
            queryModel.CustomerNames = new List<string>();
            queryModel.CustomerSurnames = new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.CustomerNames.Add(reader.GetString(0));
                            queryModel.CustomerSurnames.Add(reader.GetString(1));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_CUST;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Query4(Query1 queryModel)
        {
            string query = System.IO.File.ReadAllText(Q4_PATH);
            query = query.Replace("X", "N\'" + queryModel.CustomerName + "\'");
            query = query.Replace("Y", "N\'" + queryModel.CustomerSurname + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "Q4";
            queryModel.ProducerNames = new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.ProducerNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_DEV;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Query5(Query1 queryModel)
        {
            
            string query = System.IO.File.ReadAllText(Q5_PATH);
            query = query.Replace("X", "N\'" + queryModel.CustomerName + "\'");
            query = query.Replace("Y", "N\'" + queryModel.CustomerSurname + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "Q5";

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        
                        queryModel.SumPrice = Convert.ToDecimal(result);
                    }
                    else
                    {
                        queryModel.ErrorFlag = 1;
                        queryModel.Error = ERR_AVG;
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Query6(Query1 q) 
        {
            string query = System.IO.File.ReadAllText(Q6_PATH);
            query = query.Replace("X", "N\'" + q.ProducerName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            q.QueryId = "Q6";
            q.CarNames = new List<string>();
           

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            //q.CoffeeTypeNames.Add(reader.GetString(0));
                            q.CarNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            q.ErrorFlag = 1;
                            q.Error = ERR_PROD;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", q);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult Query7(Query1 queryModel)
        {
            string query = System.IO.File.ReadAllText(Qd1_PATH);
            query = query.Replace("Z", "N\'" + queryModel.CustomerEmail.ToString() + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.QueryId = "Qd1";
            queryModel.CustomerNames = new List<string>();
            queryModel.CustomerSurnames = new List<string>();
            queryModel.CustomerEmails = new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.CustomerSurnames.Add(reader.GetString(0));
                            queryModel.CustomerNames.Add(reader.GetString(1));
                            queryModel.CustomerEmails.Add(reader.GetString(2));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_CUST;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }
        public IActionResult Query8(Query1 q)
        {
            string query = System.IO.File.ReadAllText(Qd2_PATH);
            query = query.Replace("Z", q.ProducerId.ToString());
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            q.QueryId = "Qd2";
            q.CountryNames = new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            q.CountryNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            q.ErrorFlag = 1;
                            q.Error = ERR_COUNTRY;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", q);
        }


        public IActionResult Query9(Query1 queryModel)
        {
            string query = System.IO.File.ReadAllText(Qd3);
            query = query.Replace("Z", "N\'" + queryModel.CustomerName.ToString() + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.QueryId = "Qd3";
            queryModel.CustomerSurnames = new List<string>();
            queryModel.CustomerEmails = new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.CustomerSurnames.Add(reader.GetString(0));
                            queryModel.CustomerEmails.Add(reader.GetString(1));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_CUST;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }



        public IActionResult Result(Query1 queryResult)
        {
            return View(queryResult);
        }

    }
}
