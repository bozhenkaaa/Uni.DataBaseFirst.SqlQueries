
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Labb2.Models
{
    public class Query1
    {
        public string QueryId { get; set; }

        public string Error { get; set; }

        public int ErrorFlag { get; set; }

        public string ProducerName { get; set; }

        public string CountryName { get; set; }

        public decimal AveragePrice { get; set; }
        public decimal SumPrice { get; set; }

        public List<string> CustomerNames { get; set; }

        public List<string> CustomerSurnames { get; set; }

        public List<string> CustomerEmails { get; set; }

        public List<string> CarNames { get; set; }

        public List<decimal> CarPrices { get; set; }

        public string CustomerName { get; set; }

        public string CustomerSurname { get; set; }

        public string CustomerEmail { get; set; }

        public List<string> ProducerNames { get; set; }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "Введіть коректну вартість")]
        [Display(Name = "Вартість В")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        public string CarName { get; set; }

        public int ProducerId { get; set; }

        public List<string> CountryNames { get; set; }
    }
}
